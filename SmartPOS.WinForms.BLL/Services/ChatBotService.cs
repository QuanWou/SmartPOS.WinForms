using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DAL.Repositories;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.BLL.Services
{
    public class ChatBotService : IChatBotService
    {
        private readonly IChatBotRepository _chatBotRepository;
        private readonly IChatBotAiProvider _aiProvider;

        public ChatBotService()
        {
            _chatBotRepository = new ChatBotRepository();
            _aiProvider = new GeminiChatBotProvider();
        }

        public ChatBotResponse Ask(string question)
        {
            string normalized = Normalize(question);
            if (string.IsNullOrWhiteSpace(normalized))
            {
                return BuildResponse("Help", BuildHelpAnswer());
            }

            try
            {
                ChatBotResponse localResponse = BuildLocalResponse(normalized);
                ChatBotResponse aiResponse = TryBuildAiResponse(question, localResponse);
                return aiResponse ?? localResponse;
            }
            catch (Exception ex)
            {
                return BuildResponse("Error", "Mình chưa thể truy vấn dữ liệu lúc này.\r\nChi tiết: " + ex.Message);
            }
        }

        private ChatBotResponse BuildLocalResponse(string normalized)
        {
            if (ContainsAny(normalized, "sap het hang", "ton kho thap", "gan het hang"))
            {
                return BuildResponse("LowStock", BuildLowStockAnswer());
            }

            if (ContainsAny(normalized, "doanh thu hom nay", "hom nay doanh thu", "revenue today"))
            {
                return BuildResponse("TodayRevenue", BuildTodayRevenueAnswer());
            }

            if (ContainsAny(normalized, "top 5", "ban chay", "san pham nao ban", "top san pham"))
            {
                return BuildResponse("TopProducts", BuildTopProductsAnswer());
            }

            if (ContainsAny(normalized, "hoa don moi nhat", "don moi nhat", "latest invoice"))
            {
                return BuildResponse("LatestInvoices", BuildLatestInvoicesAnswer());
            }

            if (ContainsAny(normalized, "tong so khach hang", "bao nhieu khach", "so khach hang", "tong khach hang"))
            {
                return BuildResponse("CustomerCount", BuildCustomerCountAnswer());
            }

            if (ContainsAny(normalized, "doanh thu tuan", "giam vi", "phan tich doanh thu", "so voi tuan truoc"))
            {
                return BuildResponse("RevenueAnalysis", BuildRevenueAnalysisAnswer());
            }

            if (ContainsAny(normalized, "ton kho cao", "khuyen mai", "ban cham", "hang ton"))
            {
                return BuildResponse("PromotionSuggestion", BuildPromotionSuggestionAnswer());
            }

            if (ContainsAny(normalized, "nen nhap them", "can nhap them", "goi y nhap", "nhap hang"))
            {
                return BuildResponse("RestockSuggestion", BuildRestockSuggestionAnswer());
            }

            if (ContainsAny(normalized, "huong dan", "su dung", "pos", "ban hang"))
            {
                return BuildResponse("Guide", BuildGuideAnswer());
            }

            return BuildResponse("Fallback", BuildFallbackAnswer());
        }

        private ChatBotResponse TryBuildAiResponse(string question, ChatBotResponse localResponse)
        {
            if (_aiProvider == null || !_aiProvider.IsConfigured)
            {
                return null;
            }

            try
            {
                string context = BuildBusinessContext();
                string answer = _aiProvider.Analyze(question, context);
                if (string.IsNullOrWhiteSpace(answer))
                {
                    return null;
                }

                string intent = "OpenAI";
                if (localResponse != null && !string.IsNullOrWhiteSpace(localResponse.Intent))
                {
                    intent = localResponse.Intent + "+OpenAI";
                }

                return BuildResponse(intent, answer);
            }
            catch (Exception ex)
            {
                if (localResponse == null)
                {
                    return BuildResponse("OpenAIError", "AI nâng cao chưa khả dụng.\r\nChi tiết: " + ex.Message);
                }

                localResponse.Answer += "\r\n\r\nAI nâng cao chưa khả dụng, đang dùng phân tích nội bộ.\r\nChi tiết: " + ex.Message;
                return localResponse;
            }
        }

        private string BuildBusinessContext()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Thời điểm dữ liệu: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            builder.AppendLine("Lưu ý riêng tư: ngữ cảnh AI chỉ gửi số liệu vận hành và mã hóa đơn, không gửi tên khách hàng/nhân viên.");

            AppendContextSection(builder, "Doanh thu hôm nay", delegate
            {
                ChatBotMetricResponse metric = _chatBotRepository.GetTodayRevenue();
                builder.AppendLine("- Doanh thu: " + metric.Amount.ToString("N0") + " đ");
                builder.AppendLine("- Số hóa đơn đã thanh toán: " + metric.Count.ToString("N0"));
                builder.AppendLine("- Giá trị trung bình mỗi hóa đơn: " + metric.SecondaryAmount.ToString("N0") + " đ");
            });

            AppendContextSection(builder, "Khách hàng", delegate
            {
                ChatBotMetricResponse metric = _chatBotRepository.GetCustomerStats();
                builder.AppendLine("- Tổng khách hàng: " + metric.Count.ToString("N0"));
                builder.AppendLine("- Khách đang hoạt động: " + metric.SecondaryCount.ToString("N0"));
                builder.AppendLine("- Tổng chi tiêu ghi nhận: " + metric.Amount.ToString("N0") + " đ");
                builder.AppendLine("- Tổng điểm hiện có: " + metric.SecondaryAmount.ToString("N0"));
            });

            AppendContextSection(builder, "Sản phẩm sắp hết hàng", delegate
            {
                var products = _chatBotRepository.GetLowStockProducts(10, 8).ToList();
                AppendProductInsightLines(builder, products, true, false, false);
            });

            AppendContextSection(builder, "Top sản phẩm bán chạy 30 ngày", delegate
            {
                var products = _chatBotRepository.GetTopSellingProducts(30, 8).ToList();
                AppendProductInsightLines(builder, products, true, true, false);
            });

            AppendContextSection(builder, "Hóa đơn mới nhất", delegate
            {
                var invoices = _chatBotRepository.GetLatestInvoices(5).ToList();
                if (!invoices.Any())
                {
                    builder.AppendLine("- Không có dữ liệu.");
                    return;
                }

                foreach (ChatBotInvoiceSummaryResponse item in invoices)
                {
                    builder.AppendLine("- #" + item.MaHD +
                        " | " + item.NgayLap.ToString("dd/MM/yyyy HH:mm") +
                        " | " + item.TongTien.ToString("N0") + " đ" +
                        " | " + GetInvoiceStatusText(item.TrangThai));
                }
            });

            AppendContextSection(builder, "So sánh doanh thu theo danh mục 7 ngày", delegate
            {
                var categories = _chatBotRepository.GetRevenueComparisonByCategory(7)
                    .OrderBy(x => x.ChangeAmount)
                    .Take(8)
                    .ToList();

                if (!categories.Any())
                {
                    builder.AppendLine("- Không có dữ liệu.");
                    return;
                }

                foreach (ChatBotCategoryComparisonResponse item in categories)
                {
                    builder.AppendLine("- " + item.TenLoai +
                        ": kỳ này " + item.CurrentRevenue.ToString("N0") + " đ" +
                        ", kỳ trước " + item.PreviousRevenue.ToString("N0") + " đ" +
                        ", chênh " + item.ChangeAmount.ToString("N0") + " đ" +
                        " (" + item.ChangePercent.ToString("0.##") + "%)");
                }
            });

            AppendContextSection(builder, "Hàng tồn cao bán chậm", delegate
            {
                var products = _chatBotRepository.GetHighStockSlowMovingProducts(50, 5, 30, 8).ToList();
                AppendProductInsightLines(builder, products, true, true, false);
            });

            AppendContextSection(builder, "Gợi ý nhập hàng", delegate
            {
                var products = _chatBotRepository.GetRestockSuggestions(10, 14, 8).ToList();
                AppendProductInsightLines(builder, products, true, false, true);
            });

            return builder.ToString();
        }

        private void AppendContextSection(StringBuilder builder, string title, Action append)
        {
            builder.AppendLine();
            builder.AppendLine("## " + title);

            try
            {
                append();
            }
            catch
            {
                builder.AppendLine("- Không tải được dữ liệu phần này.");
            }
        }

        private void AppendProductInsightLines(
            StringBuilder builder,
            IList<ChatBotProductInsightResponse> products,
            bool includeStock,
            bool includeSales,
            bool includeAverageDailySold)
        {
            if (products == null || products.Count == 0)
            {
                builder.AppendLine("- Không có dữ liệu.");
                return;
            }

            foreach (ChatBotProductInsightResponse item in products)
            {
                StringBuilder line = new StringBuilder();
                line.Append("- " + item.TenSP);

                if (!string.IsNullOrWhiteSpace(item.MaVach))
                {
                    line.Append(" | mã: " + item.MaVach);
                }

                if (includeStock)
                {
                    line.Append(" | tồn: " + item.SoLuongTon.ToString("N0"));
                }

                if (includeSales)
                {
                    line.Append(" | bán: " + item.QuantitySold.ToString("N0"));
                    line.Append(" | doanh thu: " + item.Revenue.ToString("N0") + " đ");
                }

                if (includeAverageDailySold)
                {
                    line.Append(" | bán TB/ngày: " + item.AverageDailySold.ToString("0.##"));
                }

                builder.AppendLine(line.ToString());
            }
        }

        private string BuildLowStockAnswer()
        {
            var products = _chatBotRepository.GetLowStockProducts(10, 8).ToList();
            if (!products.Any())
            {
                return "Hiện chưa có sản phẩm đang bán nào ở mức tồn kho thấp (<= 10).";
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Các sản phẩm sắp hết hàng:");
            for (int i = 0; i < products.Count; i++)
            {
                ChatBotProductInsightResponse item = products[i];
                builder.AppendLine((i + 1) + ". " + item.TenSP + " - còn " + item.SoLuongTon + " đơn vị" +
                    (string.IsNullOrWhiteSpace(item.MaVach) ? string.Empty : " | mã: " + item.MaVach));
            }

            builder.AppendLine();
            builder.AppendLine("Gợi ý: ưu tiên nhập các mặt hàng còn <= 5 đơn vị trước, sau đó kiểm tra nhóm bán nhanh để tránh mất doanh thu.");
            return builder.ToString();
        }

        private string BuildTodayRevenueAnswer()
        {
            ChatBotMetricResponse metric = _chatBotRepository.GetTodayRevenue();
            return "Doanh thu hôm nay: " + metric.Amount.ToString("N0") + " đ\r\n" +
                   "Số hóa đơn đã thanh toán: " + metric.Count.ToString("N0") + "\r\n" +
                   "Giá trị trung bình mỗi hóa đơn: " + metric.SecondaryAmount.ToString("N0") + " đ";
        }

        private string BuildTopProductsAnswer()
        {
            var products = _chatBotRepository.GetTopSellingProducts(30, 5).ToList();
            if (!products.Any())
            {
                return "Chưa có dữ liệu bán hàng trong 30 ngày gần nhất.";
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Top 5 sản phẩm bán chạy trong 30 ngày gần nhất:");
            for (int i = 0; i < products.Count; i++)
            {
                ChatBotProductInsightResponse item = products[i];
                builder.AppendLine((i + 1) + ". " + item.TenSP +
                    " - bán " + item.QuantitySold.ToString("N0") + " đơn vị" +
                    ", doanh thu " + item.Revenue.ToString("N0") + " đ" +
                    ", tồn " + item.SoLuongTon.ToString("N0"));
            }

            return builder.ToString();
        }

        private string BuildLatestInvoicesAnswer()
        {
            var invoices = _chatBotRepository.GetLatestInvoices(5).ToList();
            if (!invoices.Any())
            {
                return "Chưa có hóa đơn nào trong hệ thống.";
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Các hóa đơn mới nhất:");
            foreach (ChatBotInvoiceSummaryResponse item in invoices)
            {
                builder.AppendLine("#" + item.MaHD +
                    " | " + item.NgayLap.ToString("dd/MM/yyyy HH:mm") +
                    " | " + item.TongTien.ToString("N0") + " đ" +
                    " | " + GetInvoiceStatusText(item.TrangThai) +
                    " | NV: " + (string.IsNullOrWhiteSpace(item.TenNhanVien) ? item.MaNV.ToString() : item.TenNhanVien) +
                    " | KH: " + item.TenKhachHang);
            }

            return builder.ToString();
        }

        private string BuildCustomerCountAnswer()
        {
            ChatBotMetricResponse metric = _chatBotRepository.GetCustomerStats();
            return "Tổng số khách hàng: " + metric.Count.ToString("N0") + "\r\n" +
                   "Khách đang hoạt động: " + metric.SecondaryCount.ToString("N0") + "\r\n" +
                   "Tổng chi tiêu đã ghi nhận: " + metric.Amount.ToString("N0") + " đ\r\n" +
                   "Tổng điểm hiện có của khách: " + metric.SecondaryAmount.ToString("N0");
        }

        private string BuildRevenueAnalysisAnswer()
        {
            var items = _chatBotRepository.GetRevenueComparisonByCategory(7).ToList();
            if (!items.Any())
            {
                return "Chưa đủ dữ liệu hóa đơn để so sánh doanh thu 7 ngày gần nhất với 7 ngày trước.";
            }

            decimal currentTotal = items.Sum(x => x.CurrentRevenue);
            decimal previousTotal = items.Sum(x => x.PreviousRevenue);
            decimal change = currentTotal - previousTotal;
            decimal changePercent = previousTotal == 0
                ? (currentTotal > 0 ? 100 : 0)
                : change * 100 / previousTotal;

            ChatBotCategoryComparisonResponse weakest = items
                .OrderBy(x => x.ChangeAmount)
                .FirstOrDefault();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Phân tích doanh thu 7 ngày gần nhất:");
            builder.AppendLine("Doanh thu kỳ này: " + currentTotal.ToString("N0") + " đ");
            builder.AppendLine("7 ngày trước: " + previousTotal.ToString("N0") + " đ");
            builder.AppendLine("Chênh lệch: " + change.ToString("N0") + " đ (" + changePercent.ToString("0.##") + "%)");

            if (weakest != null && weakest.ChangeAmount < 0)
            {
                builder.AppendLine();
                builder.AppendLine("Nguyên nhân nổi bật: nhóm " + weakest.TenLoai +
                    " giảm " + Math.Abs(weakest.ChangeAmount).ToString("N0") + " đ so với kỳ trước.");
            }

            var topDrops = items.Where(x => x.ChangeAmount < 0).Take(3).ToList();
            if (topDrops.Any())
            {
                builder.AppendLine("Nhóm giảm cần xem:");
                foreach (var item in topDrops)
                {
                    builder.AppendLine("- " + item.TenLoai + ": " + item.ChangeAmount.ToString("N0") + " đ (" + item.ChangePercent.ToString("0.##") + "%)");
                }
            }

            return builder.ToString();
        }

        private string BuildPromotionSuggestionAnswer()
        {
            var products = _chatBotRepository.GetHighStockSlowMovingProducts(50, 5, 30, 8).ToList();
            if (!products.Any())
            {
                return "Chưa phát hiện sản phẩm tồn kho cao nhưng bán chậm theo ngưỡng: tồn >= 50 và bán <= 5 đơn vị trong 30 ngày.";
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Có " + products.Count + " sản phẩm tồn kho cao, nên cân nhắc tạo khuyến mãi:");
            foreach (ChatBotProductInsightResponse item in products)
            {
                builder.AppendLine("- " + item.TenSP +
                    ": tồn " + item.SoLuongTon.ToString("N0") +
                    ", bán 30 ngày " + item.QuantitySold.ToString("N0") + " đơn vị.");
            }

            builder.AppendLine();
            builder.AppendLine("Gợi ý: ưu đãi combo, giảm nhẹ theo nhóm hàng hoặc đẩy bán kèm ở màn POS.");
            return builder.ToString();
        }

        private string BuildRestockSuggestionAnswer()
        {
            var products = _chatBotRepository.GetRestockSuggestions(10, 14, 8).ToList();
            if (!products.Any())
            {
                return "Chưa có sản phẩm cần nhập thêm theo ngưỡng tồn <= 10.";
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Gợi ý nhập thêm dựa trên tồn kho và tốc độ bán 14 ngày:");
            foreach (ChatBotProductInsightResponse item in products)
            {
                builder.AppendLine("- " + item.TenSP +
                    ": còn " + item.SoLuongTon.ToString("N0") +
                    ", bán TB " + item.AverageDailySold.ToString("0.##") + " đơn vị/ngày.");
            }

            return builder.ToString();
        }

        private string BuildGuideAnswer()
        {
            return "Hướng dẫn nhanh POS:\r\n" +
                   "1. Vào Bán hàng/POS.\r\n" +
                   "2. Tìm sản phẩm theo tên hoặc mã vạch, hoặc dùng quét camera/điện thoại.\r\n" +
                   "3. Chọn khách hàng nếu muốn tích điểm hoặc đổi điểm.\r\n" +
                   "4. Kiểm tra giỏ hàng, điểm đổi và bấm Thanh toán.\r\n" +
                   "5. Sau khi thanh toán có thể xem chi tiết hóa đơn.";
        }

        private string BuildHelpAnswer()
        {
            return "Mình có thể hỗ trợ tra cứu nhanh và phân tích dữ liệu bán hàng.\r\n" +
                   "Bạn có thể hỏi: doanh thu hôm nay, sản phẩm sắp hết hàng, top 5 bán chạy, hóa đơn mới nhất, tổng số khách hàng, hoặc gợi ý nhập hàng/khuyến mãi.";
        }

        private string BuildFallbackAnswer()
        {
            return "Mình chưa hiểu câu hỏi này. Hãy thử một trong các câu:\r\n" +
                   "- Sản phẩm nào sắp hết hàng?\r\n" +
                   "- Doanh thu hôm nay?\r\n" +
                   "- Top 5 sản phẩm bán chạy?\r\n" +
                   "- Hóa đơn mới nhất?\r\n" +
                   "- Tổng số khách hàng?\r\n" +
                   "- Có sản phẩm tồn kho cao cần khuyến mãi không?";
        }

        private ChatBotResponse BuildResponse(string intent, string answer)
        {
            return new ChatBotResponse
            {
                Intent = intent,
                Answer = answer,
                GeneratedAt = DateTime.Now,
                SuggestedQuestions = GetSuggestedQuestions(intent)
            };
        }

        private List<string> GetSuggestedQuestions(string intent)
        {
            List<string> defaults = new List<string>
            {
                "Doanh thu hôm nay?",
                "Sản phẩm nào sắp hết hàng?",
                "Top 5 sản phẩm bán chạy?",
                "Hóa đơn mới nhất?",
                "Tổng số khách hàng?"
            };

            if (intent == "RevenueAnalysis")
            {
                defaults.Insert(1, "Có sản phẩm tồn kho cao cần khuyến mãi không?");
            }
            else if (intent == "LowStock")
            {
                defaults.Insert(1, "Sản phẩm nào nên nhập thêm?");
            }
            else if (intent == "CustomerCount")
            {
                defaults.Insert(1, "Top 5 sản phẩm bán chạy?");
            }

            return defaults.Take(6).ToList();
        }

        private string GetInvoiceStatusText(string status)
        {
            if (string.Equals(status, "Paid", StringComparison.OrdinalIgnoreCase))
            {
                return "Đã thanh toán";
            }

            if (string.Equals(status, "Cancelled", StringComparison.OrdinalIgnoreCase))
            {
                return "Đã hủy";
            }

            return string.IsNullOrWhiteSpace(status) ? "-" : status;
        }

        private bool ContainsAny(string normalizedSource, params string[] keywords)
        {
            foreach (string keyword in keywords)
            {
                if (normalizedSource.Contains(Normalize(keyword)))
                {
                    return true;
                }
            }

            return false;
        }

        private string Normalize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            string normalized = value.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            StringBuilder builder = new StringBuilder(normalized.Length);
            foreach (char c in normalized)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder
                .ToString()
                .Replace('đ', 'd')
                .Normalize(NormalizationForm.FormC);
        }
    }
}
