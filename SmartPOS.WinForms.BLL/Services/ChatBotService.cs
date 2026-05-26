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
                bool deepAnalysis = IsDeepAnalysisRequest(normalized);
                ChatBotResponse localResponse = deepAnalysis
                    ? BuildResponse("DeepAnalysis", BuildDeepLocalAnalysisAnswer())
                    : BuildLocalResponse(normalized);

                ChatBotResponse aiResponse = TryBuildAiResponse(question, localResponse, deepAnalysis);
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

        private bool IsDeepAnalysisRequest(string normalized)
        {
            if (ContainsAny(
                normalized,
                "phan tich sau",
                "phan tich chuyen sau",
                "chuyen sau",
                "toan bo database",
                "toan bo data base",
                "toan bo du lieu",
                "du lieu day du",
                "dinh huong",
                "chien luoc",
                "xu huong khach hang",
                "hanh vi khach hang",
                "phan khuc khach hang"))
            {
                return true;
            }

            bool asksForTrend = ContainsAny(normalized, "xu huong", "du bao", "nguyen nhan", "chien luoc", "dinh huong");
            bool asksForBusinessArea = ContainsAny(normalized, "khach hang", "doanh thu", "san pham", "ban hang", "cua hang", "ton kho");

            return asksForTrend && asksForBusinessArea;
        }

        private ChatBotResponse TryBuildAiResponse(string question, ChatBotResponse localResponse, bool deepAnalysis)
        {
            if (_aiProvider == null || !_aiProvider.IsConfigured)
            {
                return null;
            }

            try
            {
                string context = deepAnalysis ? BuildDeepBusinessContext() : BuildBusinessContext();
                string answer = _aiProvider.Analyze(question, context, deepAnalysis);
                if (string.IsNullOrWhiteSpace(answer))
                {
                    return null;
                }

                string intent = deepAnalysis ? "GeminiDeepAnalysis" : "Gemini";
                if (localResponse != null && !string.IsNullOrWhiteSpace(localResponse.Intent))
                {
                    intent = localResponse.Intent + (deepAnalysis ? "+GeminiDeepAnalysis" : "+Gemini");
                }

                return BuildResponse(intent, answer);
            }
            catch (Exception ex)
            {
                if (localResponse == null)
                {
                    return BuildResponse("GeminiError", "AI nâng cao chưa khả dụng.\r\nChi tiết: " + ex.Message);
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

        private string BuildDeepBusinessContext()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Chế độ phân tích sâu SmartPOS.");
            builder.AppendLine("Thời điểm dữ liệu: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            builder.AppendLine("Phạm vi: snapshot tổng hợp từ toàn bộ database hiện tại; dữ liệu cá nhân được gom nhóm để tránh gửi thông tin không cần thiết.");
            builder.AppendLine("Lưu ý: các nhận định nguyên nhân/xu hướng phải bám vào số liệu bên dưới.");

            AppendContextSection(builder, "Tổng quan bán hàng toàn hệ thống", delegate
            {
                AppendSalesOverview(builder, "Toàn thời gian", 0);
                AppendSalesOverview(builder, "90 ngày gần nhất", 90);
                AppendSalesOverview(builder, "30 ngày gần nhất", 30);
                AppendSalesOverview(builder, "7 ngày gần nhất", 7);
            });

            AppendContextSection(builder, "Xu hướng doanh thu theo ngày 30 ngày", delegate
            {
                var items = _chatBotRepository.GetRevenueTrendByDay(30).ToList();
                AppendTimeSeriesLines(builder, items, 30);
            });

            AppendContextSection(builder, "Xu hướng doanh thu theo tháng 12 tháng", delegate
            {
                var items = _chatBotRepository.GetRevenueTrendByMonth(12).ToList();
                AppendTimeSeriesLines(builder, items, 12);
            });

            AppendContextSection(builder, "So sánh doanh thu danh mục 30 ngày", delegate
            {
                var categories = _chatBotRepository.GetRevenueComparisonByCategory(30)
                    .OrderBy(x => x.ChangeAmount)
                    .ToList();

                if (!categories.Any())
                {
                    builder.AppendLine("- Không có dữ liệu.");
                    return;
                }

                foreach (ChatBotCategoryComparisonResponse item in categories)
                {
                    builder.AppendLine("- " + item.TenLoai +
                        ": kỳ này " + Money(item.CurrentRevenue) +
                        ", kỳ trước " + Money(item.PreviousRevenue) +
                        ", chênh " + Money(item.ChangeAmount) +
                        " (" + item.ChangePercent.ToString("0.##") + "%)");
                }
            });

            AppendContextSection(builder, "Phân khúc khách hàng theo hạng", delegate
            {
                var segments = _chatBotRepository.GetCustomerSegments().ToList();
                if (!segments.Any())
                {
                    builder.AppendLine("- Không có dữ liệu khách hàng.");
                    return;
                }

                foreach (ChatBotCustomerSegmentResponse item in segments)
                {
                    builder.AppendLine("- " + item.Segment +
                        ": KH " + item.CustomerCount.ToString("N0") +
                        ", đang hoạt động " + item.ActiveCustomerCount.ToString("N0") +
                        ", hóa đơn " + item.InvoiceCount.ToString("N0") +
                        ", doanh thu " + Money(item.Revenue) +
                        ", AOV " + Money(item.AverageOrderValue) +
                        ", mua TB " + item.AveragePurchaseCount.ToString("0.##") +
                        ", điểm còn " + item.PointsAvailable.ToString("N0") +
                        ", điểm đã đổi " + item.PointsRedeemed.ToString("N0") +
                        ", mua gần nhất " + DateOrDash(item.LastPurchaseAt));
                }
            });

            AppendContextSection(builder, "Xu hướng khách hàng theo lần mua gần nhất", delegate
            {
                AppendCustomerBehaviorLines(builder, _chatBotRepository.GetCustomerRecencySegments().ToList());
            });

            AppendContextSection(builder, "Xu hướng khách hàng theo giá trị chi tiêu", delegate
            {
                AppendCustomerBehaviorLines(builder, _chatBotRepository.GetCustomerValueSegments().ToList());
            });

            AppendContextSection(builder, "Hiệu suất sản phẩm 90 ngày", delegate
            {
                var products = _chatBotRepository.GetDeepProductPerformance(90, 15).ToList();
                AppendDeepProductLines(builder, products);
            });

            AppendContextSection(builder, "Sản phẩm rủi ro biên lợi nhuận", delegate
            {
                var products = _chatBotRepository.GetProductMarginRisks(10).ToList();
                AppendDeepProductLines(builder, products);
            });

            AppendContextSection(builder, "Hàng tồn cao bán chậm", delegate
            {
                var products = _chatBotRepository.GetHighStockSlowMovingProducts(50, 5, 30, 10).ToList();
                AppendProductInsightLines(builder, products, true, true, false);
            });

            AppendContextSection(builder, "Gợi ý nhập hàng theo tồn kho và tốc độ bán", delegate
            {
                var products = _chatBotRepository.GetRestockSuggestions(10, 14, 10).ToList();
                AppendProductInsightLines(builder, products, true, false, true);
            });

            AppendContextSection(builder, "Hóa đơn mới nhất", delegate
            {
                var invoices = _chatBotRepository.GetLatestInvoices(8).ToList();
                if (!invoices.Any())
                {
                    builder.AppendLine("- Không có dữ liệu.");
                    return;
                }

                foreach (ChatBotInvoiceSummaryResponse item in invoices)
                {
                    builder.AppendLine("- #" + item.MaHD +
                        " | " + item.NgayLap.ToString("dd/MM/yyyy HH:mm") +
                        " | " + Money(item.TongTien) +
                        " | " + GetInvoiceStatusText(item.TrangThai));
                }
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

        private void AppendSalesOverview(StringBuilder builder, string label, int days)
        {
            ChatBotSalesOverviewResponse item = _chatBotRepository.GetSalesOverview(days);
            builder.AppendLine("- " + label +
                ": hóa đơn " + item.PaidInvoiceCount.ToString("N0") + "/" + item.InvoiceCount.ToString("N0") +
                ", hủy " + item.CancelledInvoiceCount.ToString("N0") +
                ", doanh thu " + Money(item.Revenue) +
                ", tạm tính " + Money(item.Subtotal) +
                ", giảm ưu đãi " + Money(item.OfferDiscount) +
                ", giảm điểm " + Money(item.PointDiscount) +
                ", AOV " + Money(item.AverageOrderValue) +
                ", KH định danh " + item.UniqueCustomerCount.ToString("N0") +
                ", đơn KH " + item.CustomerInvoiceCount.ToString("N0") +
                ", đơn khách lẻ " + item.WalkInInvoiceCount.ToString("N0") +
                ", từ " + DateOrDash(item.FirstInvoiceAt) +
                " đến " + DateOrDash(item.LastInvoiceAt));
        }

        private void AppendTimeSeriesLines(StringBuilder builder, IList<ChatBotTimeSeriesResponse> items, int maxRows)
        {
            if (items == null || items.Count == 0)
            {
                builder.AppendLine("- Không có dữ liệu.");
                return;
            }

            foreach (ChatBotTimeSeriesResponse item in items.Take(maxRows))
            {
                builder.AppendLine("- " + item.PeriodLabel +
                    ": doanh thu " + Money(item.Revenue) +
                    ", hóa đơn " + item.InvoiceCount.ToString("N0") +
                    ", KH định danh " + item.UniqueCustomerCount.ToString("N0") +
                    ", AOV " + Money(item.AverageOrderValue) +
                    ", giảm ưu đãi/điểm " + Money(item.OfferDiscount + item.PointDiscount));
            }
        }

        private void AppendCustomerBehaviorLines(StringBuilder builder, IList<ChatBotCustomerBehaviorResponse> items)
        {
            if (items == null || items.Count == 0)
            {
                builder.AppendLine("- Không có dữ liệu.");
                return;
            }

            foreach (ChatBotCustomerBehaviorResponse item in items)
            {
                builder.AppendLine("- " + item.Segment +
                    ": KH " + item.CustomerCount.ToString("N0") +
                    " (" + item.SharePercent.ToString("0.##") + "%)" +
                    ", hóa đơn " + item.InvoiceCount.ToString("N0") +
                    ", doanh thu " + Money(item.Revenue) +
                    ", AOV " + Money(item.AverageOrderValue));
            }
        }

        private void AppendDeepProductLines(StringBuilder builder, IList<ChatBotProductInsightResponse> products)
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

                line.Append(" | tồn: " + item.SoLuongTon.ToString("N0"));

                if (item.QuantitySold > 0)
                {
                    line.Append(" | bán: " + item.QuantitySold.ToString("N0"));
                    line.Append(" | doanh thu: " + Money(item.Revenue));
                    line.Append(" | lãi gộp ước tính: " + Money(item.GrossProfit));
                    line.Append(" | biên: " + item.ProfitMarginPercent.ToString("0.##") + "%");
                    line.Append(" | vốn tồn: " + Money(item.StockValue));
                }
                else
                {
                    line.Append(" | giá bán: " + Money(item.Revenue));
                    line.Append(" | giá nhập: " + Money(item.CostOfGoodsSold));
                    line.Append(" | lãi/unit: " + Money(item.GrossProfit));
                    line.Append(" | biên: " + item.ProfitMarginPercent.ToString("0.##") + "%");
                    line.Append(" | vốn tồn: " + Money(item.StockValue));
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

        private string BuildDeepLocalAnalysisAnswer()
        {
            ChatBotSalesOverviewResponse allTime = _chatBotRepository.GetSalesOverview(0);
            ChatBotSalesOverviewResponse last30 = _chatBotRepository.GetSalesOverview(30);
            ChatBotSalesOverviewResponse last7 = _chatBotRepository.GetSalesOverview(7);
            var customerRecency = _chatBotRepository.GetCustomerRecencySegments().ToList();
            var customerValues = _chatBotRepository.GetCustomerValueSegments().ToList();
            var topProducts = _chatBotRepository.GetDeepProductPerformance(90, 5).ToList();
            var slowMoving = _chatBotRepository.GetHighStockSlowMovingProducts(50, 5, 30, 5).ToList();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Phân tích sâu nội bộ từ database SmartPOS:");
            builder.AppendLine();
            builder.AppendLine("Kết luận điều hành:");
            builder.AppendLine("- Toàn thời gian có " + allTime.PaidInvoiceCount.ToString("N0") +
                " hóa đơn đã thanh toán, doanh thu " + Money(allTime.Revenue) +
                ", AOV " + Money(allTime.AverageOrderValue) + ".");
            builder.AppendLine("- 30 ngày gần nhất đạt " + Money(last30.Revenue) +
                " từ " + last30.PaidInvoiceCount.ToString("N0") +
                " hóa đơn; 7 ngày gần nhất đạt " + Money(last7.Revenue) + ".");

            builder.AppendLine();
            builder.AppendLine("Xu hướng khách hàng:");
            foreach (ChatBotCustomerBehaviorResponse item in customerRecency.Take(5))
            {
                builder.AppendLine("- " + item.Segment + ": " +
                    item.CustomerCount.ToString("N0") + " KH, " +
                    item.SharePercent.ToString("0.##") + "%, doanh thu " +
                    Money(item.Revenue) + ".");
            }

            foreach (ChatBotCustomerBehaviorResponse item in customerValues.Take(4))
            {
                builder.AppendLine("- Nhóm " + item.Segment + ": " +
                    item.CustomerCount.ToString("N0") + " KH, AOV " +
                    Money(item.AverageOrderValue) + ".");
            }

            builder.AppendLine();
            builder.AppendLine("Sản phẩm cần chú ý:");
            foreach (ChatBotProductInsightResponse item in topProducts)
            {
                builder.AppendLine("- " + item.TenSP + ": bán " + item.QuantitySold.ToString("N0") +
                    ", doanh thu " + Money(item.Revenue) +
                    ", lãi gộp ước tính " + Money(item.GrossProfit) + ".");
            }

            foreach (ChatBotProductInsightResponse item in slowMoving)
            {
                builder.AppendLine("- Tồn cao bán chậm: " + item.TenSP +
                    ", tồn " + item.SoLuongTon.ToString("N0") +
                    ", bán 30 ngày " + item.QuantitySold.ToString("N0") + ".");
            }

            builder.AppendLine();
            builder.AppendLine("Định hướng hành động:");
            builder.AppendLine("- Tập trung giữ nhóm khách mua trong 30 ngày bằng ưu đãi cá nhân và đổi điểm khi thanh toán.");
            builder.AppendLine("- Kéo lại nhóm 31-180 ngày bằng phiếu ưu đãi có hạn dùng ngắn.");
            builder.AppendLine("- Ưu tiên nhập hàng theo sản phẩm bán nhanh, tránh tăng vốn ở nhóm tồn cao bán chậm.");
            builder.AppendLine("- Kiểm tra biên lợi nhuận sản phẩm trước khi tạo khuyến mãi mạnh.");

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
            else if (!string.IsNullOrWhiteSpace(intent) &&
                     intent.IndexOf("DeepAnalysis", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                defaults.Insert(0, "Phân tích sâu toàn bộ cửa hàng");
                defaults.Insert(1, "Xu hướng khách hàng 90 ngày?");
                defaults.Insert(2, "Định hướng nhập hàng và ưu đãi?");
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

        private static string Money(decimal amount)
        {
            return amount.ToString("N0") + " đ";
        }

        private static string DateOrDash(DateTime? value)
        {
            return value.HasValue ? value.Value.ToString("dd/MM/yyyy") : "-";
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
