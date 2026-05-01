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

        public ChatBotService()
        {
            _chatBotRepository = new ChatBotRepository();
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
            catch (Exception ex)
            {
                return BuildResponse("Error", "Mình chưa thể truy vấn dữ liệu lúc này.\r\nChi tiết: " + ex.Message);
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
