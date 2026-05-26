using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using SmartPOS.WinForms.BLL.Interfaces;

namespace SmartPOS.WinForms.BLL.Services
{
    public class GeminiChatBotProvider : IChatBotAiProvider
    {
        private const string DefaultEndpointFormat = "https://generativelanguage.googleapis.com/v1beta/models/{0}:generateContent";
        private const string DefaultModel = "gemini-2.5-flash";
        private const int DefaultMaxOutputTokens = 1200;

        private static readonly HttpClient Client = CreateClient();
        private static int _nextApiKeyIndex = -1;

        private readonly string[] _apiKeys;
        private readonly string _endpointFormat;
        private readonly string _model;
        private readonly int _maxOutputTokens;
        private readonly int _thinkingBudget;
        private readonly decimal _temperature;
        private readonly bool _enabled;

        public GeminiChatBotProvider()
        {
            _enabled = !IsFalse(GetSetting("Gemini.Enabled", "true"));
            _endpointFormat = GetSetting("Gemini.EndpointFormat", DefaultEndpointFormat);
            _model = GetSetting("Gemini.Model", DefaultModel);
            _maxOutputTokens = GetIntSetting("Gemini.MaxOutputTokens", DefaultMaxOutputTokens);
            _thinkingBudget = GetIntSetting("Gemini.ThinkingBudget", 0, allowZero: true);
            _temperature = GetDecimalSetting("Gemini.Temperature", 0.2m);

            string keyEnvironmentVariable = GetSetting("Gemini.ApiKeyEnvironmentVariable", "GEMINI_API_KEY");
            string apiKeyValue = GetSetting("Gemini.ApiKey", null);

            if (string.IsNullOrWhiteSpace(apiKeyValue) && !string.IsNullOrWhiteSpace(keyEnvironmentVariable))
            {
                apiKeyValue = Environment.GetEnvironmentVariable(keyEnvironmentVariable);
            }

            _apiKeys = ParseApiKeys(apiKeyValue);
        }

        public bool IsConfigured
        {
            get { return _enabled && _apiKeys.Length > 0; }
        }

        public string Analyze(string question, string businessContext)
        {
            return Analyze(question, businessContext, false);
        }

        public string Analyze(string question, string businessContext, bool deepAnalysis)
        {
            if (!IsConfigured || string.IsNullOrWhiteSpace(question))
            {
                return null;
            }

            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            var serializer = new JavaScriptSerializer
            {
                MaxJsonLength = 1024 * 1024 * 4
            };

            var requestBody = new Dictionary<string, object>
            {
                { "systemInstruction", BuildContent(BuildInstructions(deepAnalysis)) },
                { "contents", new object[] { BuildContent(BuildInput(question, businessContext, deepAnalysis), "user") } },
                { "generationConfig", new Dictionary<string, object>
                    {
                        { "maxOutputTokens", deepAnalysis ? Math.Max(_maxOutputTokens, 2400) : _maxOutputTokens },
                        { "temperature", deepAnalysis ? Math.Min(_temperature + 0.1m, 0.5m) : _temperature },
                        { "thinkingConfig", new Dictionary<string, object>
                            {
                                { "thinkingBudget", deepAnalysis ? Math.Max(_thinkingBudget, 256) : _thinkingBudget }
                            }
                        }
                    }
                }
            };

            string endpoint = string.Format(_endpointFormat, Uri.EscapeDataString(_model));
            string json = serializer.Serialize(requestBody);
            Exception lastError = null;
            for (int attempt = 0; attempt < _apiKeys.Length; attempt++)
            {
                try
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
                    {
                        request.Headers.TryAddWithoutValidation("x-goog-api-key", GetNextApiKey());
                        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                        using (HttpResponseMessage response = Client.SendAsync(request).GetAwaiter().GetResult())
                        {
                            string responseJson = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                            if (!response.IsSuccessStatusCode)
                            {
                                lastError = new InvalidOperationException(BuildGeminiError(response.StatusCode, responseJson, serializer));
                                continue;
                            }

                            object payload = serializer.DeserializeObject(responseJson);
                            string text = CleanAnswer(ExtractCandidateText(payload));
                            if (!string.IsNullOrWhiteSpace(text))
                            {
                                if (IsLikelyIncomplete(text))
                                {
                                    continue;
                                }

                                return text;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lastError = ex;
                }
            }

            if (lastError != null)
            {
                throw lastError;
            }

            return null;
        }

        private string GetNextApiKey()
        {
            if (_apiKeys.Length == 1)
            {
                return _apiKeys[0];
            }

            int index = Interlocked.Increment(ref _nextApiKeyIndex);
            if (index == int.MaxValue)
            {
                Interlocked.Exchange(ref _nextApiKeyIndex, 0);
            }

            if (index < 0)
            {
                index = 0;
            }

            return _apiKeys[index % _apiKeys.Length];
        }

        private static string[] ParseApiKeys(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new string[0];
            }

            return value
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.Ordinal)
                .ToArray();
        }

        private static HttpClient CreateClient()
        {
            return new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(45)
            };
        }

        private static Dictionary<string, object> BuildContent(string text, string role = null)
        {
            var content = new Dictionary<string, object>
            {
                { "parts", new object[] { new Dictionary<string, object> { { "text", text ?? string.Empty } } } }
            };

            if (!string.IsNullOrWhiteSpace(role))
            {
                content["role"] = role;
            }

            return content;
        }

        private static string BuildInstructions(bool deepAnalysis)
        {
            if (deepAnalysis)
            {
                return "Bạn là trợ lý AI phân tích chuyên sâu được nhúng vào SmartPOS WinForms cho cửa hàng bán lẻ. " +
                       "Chỉ dùng dữ liệu trong ngữ cảnh SmartPOS được cung cấp; nếu dữ liệu thiếu hoặc mẫu dữ liệu nhỏ thì nói rõ giới hạn. " +
                       "Trả lời bằng tiếng Việt, thực tế, có định hướng hành động. Ưu tiên số liệu, xu hướng, nguyên nhân có thể kiểm chứng và rủi ro vận hành. " +
                       "Không bịa dữ liệu, không tự nhận đã thay đổi hệ thống, không yêu cầu thông tin nhạy cảm. " +
                       "Không dùng Markdown phức tạp; dùng plain text phù hợp WinForms Label. Có thể dùng tiêu đề ngắn và bullet dấu '-'. " +
                       "Khi phân tích khách hàng, tập trung vào phân khúc, tần suất mua, giá trị đơn hàng, khả năng quay lại và cách dùng ưu đãi/điểm. " +
                       "Khi đề xuất nhập hàng hoặc khuyến mãi, phải nêu lý do dựa trên tồn kho, tốc độ bán, biên lợi nhuận hoặc xu hướng doanh thu.";
            }

            return "Bạn là trợ lý AI được nhúng vào SmartPOS WinForms để phân tích dữ liệu vận hành cửa hàng bán lẻ. " +
                   "Chỉ dùng dữ liệu trong phần ngữ cảnh được cung cấp; nếu dữ liệu thiếu thì nói rõ thiếu dữ liệu. " +
                   "Trả lời bằng tiếng Việt, rõ ràng, ngắn gọn, ưu tiên số liệu và hành động đề xuất. " +
                   "Bắt buộc dùng plain text phù hợp với WinForms Label: không dùng Markdown, không dùng **, __, #, bảng, blockquote hoặc bullet lồng nhau. " +
                   "Mỗi ý chỉ một dòng ngắn; dùng dấu gạch ngang '-' cho bullet. " +
                   "Tổng cộng tối đa 8 dòng. Không viết lan man. Không dừng giữa câu. " +
                   "Không bịa dữ liệu, không tự nhận đã thay đổi hệ thống, và không yêu cầu người dùng cung cấp thông tin nhạy cảm.";
        }

        private static string BuildInput(string question, string businessContext, bool deepAnalysis)
        {
            StringBuilder builder = new StringBuilder();

            if (deepAnalysis)
            {
                builder.AppendLine("Câu hỏi người dùng:");
                builder.AppendLine(question.Trim());
                builder.AppendLine();
                builder.AppendLine("Snapshot dữ liệu SmartPOS để phân tích chuyên sâu:");
                builder.AppendLine(string.IsNullOrWhiteSpace(businessContext) ? "Không có dữ liệu ngữ cảnh." : businessContext);
                builder.AppendLine();
                builder.AppendLine("Yêu cầu trả lời:");
                builder.AppendLine("Định dạng plain text, không dùng bảng markdown.");
                builder.AppendLine("1. Kết luận điều hành: 2-3 câu ngắn, nêu vấn đề chính và cơ hội chính.");
                builder.AppendLine("2. Dấu hiệu từ dữ liệu: 4-7 bullet, mỗi bullet có số liệu cụ thể.");
                builder.AppendLine("3. Nguyên nhân có khả năng: 3-5 bullet, nói rõ đây là suy luận nếu dữ liệu chưa đủ.");
                builder.AppendLine("4. Xu hướng khách hàng: 3-5 bullet về phân khúc, tần suất quay lại, giá trị đơn và điểm/ưu đãi.");
                builder.AppendLine("5. Định hướng hành động: 5-8 bullet ưu tiên theo tác động, nêu việc làm ngay trong POS/kho/khách hàng.");
                builder.AppendLine("6. Rủi ro cần kiểm tra: tối đa 4 bullet.");
                builder.AppendLine("Không viết chung chung. Không bịa dữ liệu ngoài snapshot. Nếu dữ liệu ít, đề xuất cách thu thập thêm.");
                return builder.ToString();
            }

            builder.AppendLine("Câu hỏi người dùng:");
            builder.AppendLine(question.Trim());
            builder.AppendLine();
            builder.AppendLine("Ngữ cảnh dữ liệu SmartPOS hiện có:");
            builder.AppendLine(string.IsNullOrWhiteSpace(businessContext) ? "Không có dữ liệu ngữ cảnh." : businessContext);
            builder.AppendLine();
            builder.AppendLine("Yêu cầu trả lời:");
            builder.AppendLine("Định dạng bắt buộc, chỉ plain text:");
            builder.AppendLine("Kết luận: 1 câu ngắn, tối đa 25 từ.");
            builder.AppendLine();
            builder.AppendLine("Chi tiết:");
            builder.AppendLine("- Tối đa 3 bullet, mỗi bullet dưới 22 từ.");
            builder.AppendLine();
            builder.AppendLine("Đề xuất:");
            builder.AppendLine("- Tối đa 3 bullet, ưu tiên việc cần làm ngay.");
            builder.AppendLine();
            builder.AppendLine("Quy tắc trình bày:");
            builder.AppendLine("- Không dùng Markdown: không có **, __, #, bảng hoặc bullet dấu *.");
            builder.AppendLine("- Không viết đoạn văn dài; mỗi bullet nên dưới 22 từ.");
            builder.AppendLine("- Phải kết thúc trọn ý; không bỏ dở ở các từ như là, gồm, vì, do, và.");
            builder.AppendLine("- Nếu đề xuất nhập hàng/khuyến mãi, nêu lý do dựa trên tồn kho, tốc độ bán hoặc doanh thu.");
            return builder.ToString();
        }

        private static string CleanAnswer(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            string[] lines = text
                .Replace("\r\n", "\n")
                .Replace('\r', '\n')
                .Split('\n');

            StringBuilder builder = new StringBuilder();
            bool previousBlank = false;

            foreach (string rawLine in lines)
            {
                string line = rawLine.TrimEnd()
                    .Replace("**", string.Empty)
                    .Replace("__", string.Empty);

                string trimmed = line.Trim();

                while (trimmed.StartsWith("#", StringComparison.Ordinal))
                {
                    trimmed = trimmed.Substring(1).TrimStart();
                }

                if (trimmed.StartsWith("*", StringComparison.Ordinal))
                {
                    trimmed = "- " + trimmed.TrimStart('*', ' ');
                }
                else if (trimmed.StartsWith("-", StringComparison.Ordinal))
                {
                    trimmed = "- " + trimmed.TrimStart('-', ' ');
                }

                if (string.IsNullOrWhiteSpace(trimmed))
                {
                    if (!previousBlank && builder.Length > 0)
                    {
                        builder.AppendLine();
                    }

                    previousBlank = true;
                    continue;
                }

                builder.AppendLine(trimmed);
                previousBlank = false;
            }

            return builder.ToString().Trim();
        }

        private static bool IsLikelyIncomplete(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }

            string[] lines = text
                .Replace("\r\n", "\n")
                .Replace('\r', '\n')
                .Split('\n')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            if (lines.Length == 0)
            {
                return true;
            }

            string last = lines[lines.Length - 1].TrimEnd('.', '!', '?', ';', ',').Trim();
            string normalized = RemoveVietnameseDiacritics(last).ToLowerInvariant();
            string[] unfinishedWords =
            {
                "la",
                "gom",
                "vi",
                "do",
                "va",
                "nhung",
                "neu",
                "khi",
                "trong",
                "voi",
                "de"
            };

            string lastWord = normalized
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .LastOrDefault();

            foreach (string word in unfinishedWords)
            {
                if (string.Equals(lastWord, word, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return last.EndsWith(":", StringComparison.Ordinal) ||
                   last.EndsWith("-", StringComparison.Ordinal);
        }

        private static string RemoveVietnameseDiacritics(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            string normalized = value.Normalize(NormalizationForm.FormD);
            StringBuilder builder = new StringBuilder(normalized.Length);
            foreach (char c in normalized)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder.ToString().Replace('đ', 'd').Replace('Đ', 'D').Normalize(NormalizationForm.FormC);
        }

        private static string BuildGeminiError(HttpStatusCode statusCode, string responseJson, JavaScriptSerializer serializer)
        {
            string message = ExtractErrorMessage(responseJson, serializer);
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Gemini API trả về lỗi " + (int)statusCode + " (" + statusCode + ").";
            }

            return message;
        }

        private static string ExtractErrorMessage(string responseJson, JavaScriptSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(responseJson))
            {
                return null;
            }

            try
            {
                var payload = serializer.DeserializeObject(responseJson) as Dictionary<string, object>;
                if (payload == null || !payload.ContainsKey("error"))
                {
                    return null;
                }

                var error = payload["error"] as Dictionary<string, object>;
                if (error != null && error.ContainsKey("message"))
                {
                    return Convert.ToString(error["message"]);
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        private static string ExtractCandidateText(object payload)
        {
            var root = payload as Dictionary<string, object>;
            if (root == null || !root.ContainsKey("candidates"))
            {
                return null;
            }

            StringBuilder builder = new StringBuilder();
            foreach (object candidateItem in Enumerate(root["candidates"]))
            {
                var candidate = candidateItem as Dictionary<string, object>;
                if (candidate == null || !candidate.ContainsKey("content"))
                {
                    continue;
                }

                var content = candidate["content"] as Dictionary<string, object>;
                if (content == null || !content.ContainsKey("parts"))
                {
                    continue;
                }
                foreach (object partItem in Enumerate(content["parts"]))
                {
                    var part = partItem as Dictionary<string, object>;
                    if (part == null || !part.ContainsKey("text"))
                    {
                        continue;
                    }

                    string text = Convert.ToString(part["text"]);
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        builder.AppendLine(text);
                    }
                }
            }

            return builder.ToString().Trim();
        }

        private static IEnumerable<object> Enumerate(object value)
        {
            if (value == null || value is string)
            {
                return Enumerable.Empty<object>();
            }

            var enumerable = value as IEnumerable;
            if (enumerable == null)
            {
                return Enumerable.Empty<object>();
            }

            return enumerable.Cast<object>();
        }

        private static string GetSetting(string key, string fallback)
        {
            try
            {
                string value = ConfigurationManager.AppSettings[key];
                return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
            }
            catch
            {
                return fallback;
            }
        }

        private static int GetIntSetting(string key, int fallback, bool allowZero = false)
        {
            string value = GetSetting(key, null);
            int parsed;
            return int.TryParse(value, out parsed) && (parsed > 0 || (allowZero && parsed == 0)) ? parsed : fallback;
        }

        private static decimal GetDecimalSetting(string key, decimal fallback)
        {
            string value = GetSetting(key, null);
            decimal parsed;
            NumberStyles style = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;
            return (decimal.TryParse(value, style, CultureInfo.InvariantCulture, out parsed) ||
                    decimal.TryParse(value, style, CultureInfo.CurrentCulture, out parsed)) &&
                   parsed >= 0
                ? parsed
                : fallback;
        }

        private static bool IsFalse(string value)
        {
            return string.Equals(value, "false", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(value, "0", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(value, "no", StringComparison.OrdinalIgnoreCase);
        }
    }
}
