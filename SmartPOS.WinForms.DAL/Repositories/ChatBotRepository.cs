using System.Collections.Generic;
using SmartPOS.WinForms.DAL.Data;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.DAL.Repositories
{
    public class ChatBotRepository : IChatBotRepository
    {
        private readonly DbHelper _dbHelper;

        public ChatBotRepository()
        {
            _dbHelper = new DbHelper();
        }

        public ChatBotMetricResponse GetTodayRevenue()
        {
            string sql = @"
                SELECT
                    COUNT(1) AS Count,
                    ISNULL(SUM(TongTien), 0) AS Amount,
                    ISNULL(AVG(TongTien), 0) AS SecondaryAmount
                FROM Invoices
                WHERE TrangThai = 'Paid'
                  AND CAST(NgayLap AS DATE) = CAST(GETDATE() AS DATE)";

            return _dbHelper.QueryFirstOrDefault<ChatBotMetricResponse>(sql) ?? new ChatBotMetricResponse();
        }

        public ChatBotMetricResponse GetCustomerStats()
        {
            string sql = @"
                SELECT
                    COUNT(1) AS Count,
                    ISNULL(SUM(CASE WHEN TrangThai = 1 THEN 1 ELSE 0 END), 0) AS SecondaryCount,
                    ISNULL(SUM(TongChiTieu), 0) AS Amount,
                    ISNULL(SUM(DiemHienCo), 0) AS SecondaryAmount
                FROM Customers";

            return _dbHelper.QueryFirstOrDefault<ChatBotMetricResponse>(sql) ?? new ChatBotMetricResponse();
        }

        public ChatBotSalesOverviewResponse GetSalesOverview(int days)
        {
            string sql = @"
                SELECT
                    COUNT(1) AS InvoiceCount,
                    ISNULL(SUM(CASE WHEN i.TrangThai = 'Paid' THEN 1 ELSE 0 END), 0) AS PaidInvoiceCount,
                    ISNULL(SUM(CASE WHEN i.TrangThai = 'Cancelled' THEN 1 ELSE 0 END), 0) AS CancelledInvoiceCount,
                    ISNULL(SUM(CASE WHEN i.TrangThai = 'Paid' AND i.MaKH IS NOT NULL THEN 1 ELSE 0 END), 0) AS CustomerInvoiceCount,
                    ISNULL(SUM(CASE WHEN i.TrangThai = 'Paid' AND i.MaKH IS NULL THEN 1 ELSE 0 END), 0) AS WalkInInvoiceCount,
                    COUNT(DISTINCT CASE WHEN i.TrangThai = 'Paid' AND i.MaKH IS NOT NULL THEN i.MaKH END) AS UniqueCustomerCount,
                    ISNULL(SUM(CASE WHEN i.TrangThai = 'Paid' THEN i.TongTien ELSE 0 END), 0) AS Revenue,
                    ISNULL(SUM(CASE WHEN i.TrangThai = 'Paid' THEN ISNULL(NULLIF(i.TongTienTruocGiam, 0), i.TongTien) ELSE 0 END), 0) AS Subtotal,
                    ISNULL(SUM(CASE WHEN i.TrangThai = 'Paid' THEN ISNULL(i.GiamGiaUuDai, 0) ELSE 0 END), 0) AS OfferDiscount,
                    ISNULL(SUM(CASE WHEN i.TrangThai = 'Paid' THEN ISNULL(i.GiamGiaDiem, 0) ELSE 0 END), 0) AS PointDiscount,
                    ISNULL(AVG(CASE WHEN i.TrangThai = 'Paid' THEN i.TongTien END), 0) AS AverageOrderValue,
                    MIN(CASE WHEN i.TrangThai = 'Paid' THEN i.NgayLap END) AS FirstInvoiceAt,
                    MAX(CASE WHEN i.TrangThai = 'Paid' THEN i.NgayLap END) AS LastInvoiceAt
                FROM Invoices i
                WHERE @Days <= 0
                   OR i.NgayLap >= DATEADD(DAY, 1 - @Days, CAST(GETDATE() AS DATE))";

            return _dbHelper.QueryFirstOrDefault<ChatBotSalesOverviewResponse>(sql, new { Days = days })
                   ?? new ChatBotSalesOverviewResponse();
        }

        public IEnumerable<ChatBotTimeSeriesResponse> GetRevenueTrendByDay(int days)
        {
            string sql = @"
                SELECT
                    CONVERT(VARCHAR(10), CAST(i.NgayLap AS DATE), 120) AS PeriodLabel,
                    CAST(i.NgayLap AS DATE) AS PeriodStart,
                    COUNT(1) AS InvoiceCount,
                    COUNT(DISTINCT i.MaKH) AS UniqueCustomerCount,
                    ISNULL(SUM(i.TongTien), 0) AS Revenue,
                    ISNULL(SUM(ISNULL(NULLIF(i.TongTienTruocGiam, 0), i.TongTien)), 0) AS Subtotal,
                    ISNULL(SUM(ISNULL(i.GiamGiaUuDai, 0)), 0) AS OfferDiscount,
                    ISNULL(SUM(ISNULL(i.GiamGiaDiem, 0)), 0) AS PointDiscount,
                    ISNULL(AVG(i.TongTien), 0) AS AverageOrderValue
                FROM Invoices i
                WHERE i.TrangThai = 'Paid'
                  AND i.NgayLap >= DATEADD(DAY, 1 - @Days, CAST(GETDATE() AS DATE))
                GROUP BY CAST(i.NgayLap AS DATE)
                ORDER BY PeriodStart ASC";

            return _dbHelper.Query<ChatBotTimeSeriesResponse>(sql, new { Days = days });
        }

        public IEnumerable<ChatBotTimeSeriesResponse> GetRevenueTrendByMonth(int months)
        {
            string sql = @"
                SELECT
                    CAST(YEAR(i.NgayLap) AS VARCHAR(4)) + '-' + RIGHT('0' + CAST(MONTH(i.NgayLap) AS VARCHAR(2)), 2) AS PeriodLabel,
                    DATEFROMPARTS(YEAR(i.NgayLap), MONTH(i.NgayLap), 1) AS PeriodStart,
                    COUNT(1) AS InvoiceCount,
                    COUNT(DISTINCT i.MaKH) AS UniqueCustomerCount,
                    ISNULL(SUM(i.TongTien), 0) AS Revenue,
                    ISNULL(SUM(ISNULL(NULLIF(i.TongTienTruocGiam, 0), i.TongTien)), 0) AS Subtotal,
                    ISNULL(SUM(ISNULL(i.GiamGiaUuDai, 0)), 0) AS OfferDiscount,
                    ISNULL(SUM(ISNULL(i.GiamGiaDiem, 0)), 0) AS PointDiscount,
                    ISNULL(AVG(i.TongTien), 0) AS AverageOrderValue
                FROM Invoices i
                WHERE i.TrangThai = 'Paid'
                  AND i.NgayLap >= DATEADD(MONTH, 1 - @Months, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1))
                GROUP BY YEAR(i.NgayLap), MONTH(i.NgayLap)
                ORDER BY PeriodStart ASC";

            return _dbHelper.Query<ChatBotTimeSeriesResponse>(sql, new { Months = months });
        }

        public IEnumerable<ChatBotProductInsightResponse> GetLowStockProducts(int threshold, int take)
        {
            string sql = @"
                SELECT TOP (@Take)
                    MaSP,
                    TenSP,
                    MaVach,
                    SoLuongTon,
                    0 AS QuantitySold,
                    0 AS Revenue,
                    0 AS AverageDailySold
                FROM Products
                WHERE TrangThai = 1
                  AND SoLuongTon <= @Threshold
                  AND (HanSuDung IS NULL OR HanSuDung >= CAST(GETDATE() AS DATE))
                ORDER BY SoLuongTon ASC, TenSP ASC";

            return _dbHelper.Query<ChatBotProductInsightResponse>(sql, new
            {
                Threshold = threshold,
                Take = take
            });
        }

        public IEnumerable<ChatBotProductInsightResponse> GetTopSellingProducts(int days, int take)
        {
            string sql = @"
                SELECT TOP (@Take)
                    p.MaSP,
                    p.TenSP,
                    p.MaVach,
                    p.SoLuongTon,
                    SUM(id.SoLuong) AS QuantitySold,
                    SUM(id.ThanhTien) AS Revenue,
                    CAST(SUM(id.SoLuong) * 1.0 / @Days AS DECIMAL(18,2)) AS AverageDailySold
                FROM Invoices i
                INNER JOIN InvoiceDetails id ON id.MaHD = i.MaHD
                INNER JOIN Products p ON p.MaSP = id.MaSP
                WHERE i.TrangThai = 'Paid'
                  AND i.NgayLap >= DATEADD(DAY, 1 - @Days, CAST(GETDATE() AS DATE))
                GROUP BY p.MaSP, p.TenSP, p.MaVach, p.SoLuongTon
                ORDER BY SUM(id.SoLuong) DESC, SUM(id.ThanhTien) DESC";

            return _dbHelper.Query<ChatBotProductInsightResponse>(sql, new
            {
                Days = days,
                Take = take
            });
        }

        public IEnumerable<ChatBotProductInsightResponse> GetDeepProductPerformance(int days, int take)
        {
            string sql = @"
                SELECT TOP (@Take)
                    p.MaSP,
                    p.TenSP,
                    p.MaVach,
                    p.SoLuongTon,
                    ISNULL(SUM(CASE WHEN i.MaHD IS NOT NULL THEN id.SoLuong ELSE 0 END), 0) AS QuantitySold,
                    ISNULL(SUM(CASE WHEN i.MaHD IS NOT NULL THEN id.ThanhTien ELSE 0 END), 0) AS Revenue,
                    CAST(ISNULL(SUM(CASE WHEN i.MaHD IS NOT NULL THEN id.SoLuong ELSE 0 END), 0) * 1.0 / @Days AS DECIMAL(18,2)) AS AverageDailySold,
                    ISNULL(SUM(CASE WHEN i.MaHD IS NOT NULL THEN id.SoLuong * p.GiaNhap ELSE 0 END), 0) AS CostOfGoodsSold,
                    ISNULL(SUM(CASE WHEN i.MaHD IS NOT NULL THEN id.ThanhTien - (id.SoLuong * p.GiaNhap) ELSE 0 END), 0) AS GrossProfit,
                    CAST(CASE
                        WHEN ISNULL(SUM(CASE WHEN i.MaHD IS NOT NULL THEN id.ThanhTien ELSE 0 END), 0) = 0 THEN 0
                        ELSE ISNULL(SUM(CASE WHEN i.MaHD IS NOT NULL THEN id.ThanhTien - (id.SoLuong * p.GiaNhap) ELSE 0 END), 0) * 100.0
                             / NULLIF(SUM(CASE WHEN i.MaHD IS NOT NULL THEN id.ThanhTien ELSE 0 END), 0)
                    END AS DECIMAL(18,2)) AS ProfitMarginPercent,
                    p.SoLuongTon * p.GiaNhap AS StockValue
                FROM Products p
                LEFT JOIN InvoiceDetails id ON id.MaSP = p.MaSP
                LEFT JOIN Invoices i ON i.MaHD = id.MaHD
                    AND i.TrangThai = 'Paid'
                    AND i.NgayLap >= DATEADD(DAY, 1 - @Days, CAST(GETDATE() AS DATE))
                WHERE p.TrangThai = 1
                GROUP BY p.MaSP, p.TenSP, p.MaVach, p.SoLuongTon, p.GiaNhap
                ORDER BY Revenue DESC, GrossProfit DESC, QuantitySold DESC";

            return _dbHelper.Query<ChatBotProductInsightResponse>(sql, new
            {
                Days = days,
                Take = take
            });
        }

        public IEnumerable<ChatBotProductInsightResponse> GetProductMarginRisks(int take)
        {
            string sql = @"
                SELECT TOP (@Take)
                    p.MaSP,
                    p.TenSP,
                    p.MaVach,
                    p.SoLuongTon,
                    0 AS QuantitySold,
                    p.GiaBan AS Revenue,
                    0 AS AverageDailySold,
                    p.GiaNhap AS CostOfGoodsSold,
                    p.GiaBan - p.GiaNhap AS GrossProfit,
                    CAST(CASE
                        WHEN p.GiaBan = 0 THEN -100
                        ELSE (p.GiaBan - p.GiaNhap) * 100.0 / p.GiaBan
                    END AS DECIMAL(18,2)) AS ProfitMarginPercent,
                    p.SoLuongTon * p.GiaNhap AS StockValue
                FROM Products p
                WHERE p.TrangThai = 1
                ORDER BY ProfitMarginPercent ASC, StockValue DESC, p.TenSP ASC";

            return _dbHelper.Query<ChatBotProductInsightResponse>(sql, new { Take = take });
        }

        public IEnumerable<ChatBotInvoiceSummaryResponse> GetLatestInvoices(int take)
        {
            string sql = @"
                SELECT TOP (@Take)
                    i.MaHD,
                    i.NgayLap,
                    i.MaNV,
                    u.TenNV AS TenNhanVien,
                    COALESCE(c.HoTen, N'Khách lẻ') AS TenKhachHang,
                    i.TongTien,
                    i.TrangThai
                FROM Invoices i
                LEFT JOIN Users u ON u.MaNV = i.MaNV
                LEFT JOIN Customers c ON c.MaKH = i.MaKH
                ORDER BY i.NgayLap DESC, i.MaHD DESC";

            return _dbHelper.Query<ChatBotInvoiceSummaryResponse>(sql, new { Take = take });
        }

        public IEnumerable<ChatBotCategoryComparisonResponse> GetRevenueComparisonByCategory(int days)
        {
            string sql = @"
                WITH CategoryRevenue AS
                (
                    SELECT
                        c.TenLoai,
                        SUM(CASE
                            WHEN i.NgayLap >= DATEADD(DAY, 1 - @Days, CAST(GETDATE() AS DATE))
                                THEN id.ThanhTien
                            ELSE 0
                        END) AS CurrentRevenue,
                        SUM(CASE
                            WHEN i.NgayLap >= DATEADD(DAY, 1 - (@Days * 2), CAST(GETDATE() AS DATE))
                             AND i.NgayLap < DATEADD(DAY, 1 - @Days, CAST(GETDATE() AS DATE))
                                THEN id.ThanhTien
                            ELSE 0
                        END) AS PreviousRevenue
                    FROM Invoices i
                    INNER JOIN InvoiceDetails id ON id.MaHD = i.MaHD
                    INNER JOIN Products p ON p.MaSP = id.MaSP
                    INNER JOIN Categories c ON c.MaLoai = p.MaLoai
                    WHERE i.TrangThai = 'Paid'
                      AND i.NgayLap >= DATEADD(DAY, 1 - (@Days * 2), CAST(GETDATE() AS DATE))
                    GROUP BY c.TenLoai
                )
                SELECT
                    TenLoai,
                    ISNULL(CurrentRevenue, 0) AS CurrentRevenue,
                    ISNULL(PreviousRevenue, 0) AS PreviousRevenue,
                    ISNULL(CurrentRevenue, 0) - ISNULL(PreviousRevenue, 0) AS ChangeAmount,
                    CAST(CASE
                        WHEN ISNULL(PreviousRevenue, 0) = 0 AND ISNULL(CurrentRevenue, 0) > 0 THEN 100
                        WHEN ISNULL(PreviousRevenue, 0) = 0 THEN 0
                        ELSE ((ISNULL(CurrentRevenue, 0) - ISNULL(PreviousRevenue, 0)) * 100.0 / PreviousRevenue)
                    END AS DECIMAL(18,2)) AS ChangePercent
                FROM CategoryRevenue
                ORDER BY ChangeAmount ASC, CurrentRevenue DESC";

            return _dbHelper.Query<ChatBotCategoryComparisonResponse>(sql, new { Days = days });
        }

        public IEnumerable<ChatBotCustomerSegmentResponse> GetCustomerSegments()
        {
            string sql = @"
                WITH PaidByCustomer AS
                (
                    SELECT
                        i.MaKH,
                        COUNT(1) AS InvoiceCount,
                        SUM(i.TongTien) AS Revenue,
                        MAX(i.NgayLap) AS LastPurchaseAt
                    FROM Invoices i
                    WHERE i.TrangThai = 'Paid'
                      AND i.MaKH IS NOT NULL
                    GROUP BY i.MaKH
                )
                SELECT
                    c.HangThanhVien AS Segment,
                    COUNT(1) AS CustomerCount,
                    ISNULL(SUM(CASE WHEN c.TrangThai = 1 THEN 1 ELSE 0 END), 0) AS ActiveCustomerCount,
                    ISNULL(SUM(ISNULL(p.InvoiceCount, 0)), 0) AS InvoiceCount,
                    ISNULL(SUM(ISNULL(p.Revenue, 0)), 0) AS Revenue,
                    CAST(CASE
                        WHEN ISNULL(SUM(ISNULL(p.InvoiceCount, 0)), 0) = 0 THEN 0
                        ELSE SUM(ISNULL(p.Revenue, 0)) * 1.0 / NULLIF(SUM(ISNULL(p.InvoiceCount, 0)), 0)
                    END AS DECIMAL(18,2)) AS AverageOrderValue,
                    CAST(ISNULL(AVG(CAST(c.SoLanMua AS DECIMAL(18,2))), 0) AS DECIMAL(18,2)) AS AveragePurchaseCount,
                    ISNULL(SUM(c.DiemHienCo), 0) AS PointsAvailable,
                    ISNULL(SUM(c.TongDiemDaDoi), 0) AS PointsRedeemed,
                    MAX(p.LastPurchaseAt) AS LastPurchaseAt
                FROM Customers c
                LEFT JOIN PaidByCustomer p ON p.MaKH = c.MaKH
                GROUP BY c.HangThanhVien
                ORDER BY Revenue DESC, CustomerCount DESC";

            return _dbHelper.Query<ChatBotCustomerSegmentResponse>(sql);
        }

        public IEnumerable<ChatBotCustomerBehaviorResponse> GetCustomerRecencySegments()
        {
            string sql = @"
                WITH CustomerPurchase AS
                (
                    SELECT
                        c.MaKH,
                        COUNT(i.MaHD) AS InvoiceCount,
                        ISNULL(SUM(i.TongTien), 0) AS Revenue,
                        MAX(i.NgayLap) AS LastPurchaseAt
                    FROM Customers c
                    LEFT JOIN Invoices i ON i.MaKH = c.MaKH
                        AND i.TrangThai = 'Paid'
                    GROUP BY c.MaKH
                ),
                Segmented AS
                (
                    SELECT
                        CASE
                            WHEN LastPurchaseAt IS NULL THEN N'Chưa từng mua'
                            WHEN LastPurchaseAt >= DATEADD(DAY, -30, GETDATE()) THEN N'Mua trong 30 ngày'
                            WHEN LastPurchaseAt >= DATEADD(DAY, -90, GETDATE()) THEN N'Mua 31-90 ngày'
                            WHEN LastPurchaseAt >= DATEADD(DAY, -180, GETDATE()) THEN N'Mua 91-180 ngày'
                            ELSE N'Ngủ đông >180 ngày'
                        END AS Segment,
                        InvoiceCount,
                        Revenue
                    FROM CustomerPurchase
                )
                SELECT
                    Segment,
                    COUNT(1) AS CustomerCount,
                    ISNULL(SUM(InvoiceCount), 0) AS InvoiceCount,
                    ISNULL(SUM(Revenue), 0) AS Revenue,
                    CAST(CASE
                        WHEN ISNULL(SUM(InvoiceCount), 0) = 0 THEN 0
                        ELSE SUM(Revenue) * 1.0 / NULLIF(SUM(InvoiceCount), 0)
                    END AS DECIMAL(18,2)) AS AverageOrderValue,
                    CAST(ISNULL(COUNT(1) * 100.0 / NULLIF((SELECT COUNT(1) FROM Customers), 0), 0) AS DECIMAL(18,2)) AS SharePercent
                FROM Segmented
                GROUP BY Segment
                ORDER BY
                    CASE Segment
                        WHEN N'Mua trong 30 ngày' THEN 1
                        WHEN N'Mua 31-90 ngày' THEN 2
                        WHEN N'Mua 91-180 ngày' THEN 3
                        WHEN N'Ngủ đông >180 ngày' THEN 4
                        ELSE 5
                    END";

            return _dbHelper.Query<ChatBotCustomerBehaviorResponse>(sql);
        }

        public IEnumerable<ChatBotCustomerBehaviorResponse> GetCustomerValueSegments()
        {
            string sql = @"
                WITH PaidByCustomer AS
                (
                    SELECT
                        i.MaKH,
                        COUNT(1) AS InvoiceCount,
                        SUM(i.TongTien) AS Revenue
                    FROM Invoices i
                    WHERE i.TrangThai = 'Paid'
                      AND i.MaKH IS NOT NULL
                    GROUP BY i.MaKH
                ),
                Segmented AS
                (
                    SELECT
                        CASE
                            WHEN c.TongChiTieu >= 5000000 THEN N'VIP >= 5 triệu'
                            WHEN c.TongChiTieu >= 1000000 THEN N'Giá trị cao 1-5 triệu'
                            WHEN c.TongChiTieu > 0 THEN N'Đã mua <1 triệu'
                            ELSE N'Chưa chi tiêu'
                        END AS Segment,
                        ISNULL(p.InvoiceCount, 0) AS InvoiceCount,
                        ISNULL(p.Revenue, 0) AS Revenue
                    FROM Customers c
                    LEFT JOIN PaidByCustomer p ON p.MaKH = c.MaKH
                )
                SELECT
                    Segment,
                    COUNT(1) AS CustomerCount,
                    ISNULL(SUM(InvoiceCount), 0) AS InvoiceCount,
                    ISNULL(SUM(Revenue), 0) AS Revenue,
                    CAST(CASE
                        WHEN ISNULL(SUM(InvoiceCount), 0) = 0 THEN 0
                        ELSE SUM(Revenue) * 1.0 / NULLIF(SUM(InvoiceCount), 0)
                    END AS DECIMAL(18,2)) AS AverageOrderValue,
                    CAST(ISNULL(COUNT(1) * 100.0 / NULLIF((SELECT COUNT(1) FROM Customers), 0), 0) AS DECIMAL(18,2)) AS SharePercent
                FROM Segmented
                GROUP BY Segment
                ORDER BY Revenue DESC, CustomerCount DESC";

            return _dbHelper.Query<ChatBotCustomerBehaviorResponse>(sql);
        }

        public IEnumerable<ChatBotProductInsightResponse> GetHighStockSlowMovingProducts(int stockThreshold, int soldThreshold, int days, int take)
        {
            string sql = @"
                SELECT TOP (@Take)
                    p.MaSP,
                    p.TenSP,
                    p.MaVach,
                    p.SoLuongTon,
                    ISNULL(SUM(CASE WHEN i.TrangThai = 'Paid' THEN id.SoLuong ELSE 0 END), 0) AS QuantitySold,
                    ISNULL(SUM(CASE WHEN i.TrangThai = 'Paid' THEN id.ThanhTien ELSE 0 END), 0) AS Revenue,
                    CAST(ISNULL(SUM(CASE WHEN i.TrangThai = 'Paid' THEN id.SoLuong ELSE 0 END), 0) * 1.0 / @Days AS DECIMAL(18,2)) AS AverageDailySold
                FROM Products p
                LEFT JOIN InvoiceDetails id ON id.MaSP = p.MaSP
                LEFT JOIN Invoices i ON i.MaHD = id.MaHD
                    AND i.NgayLap >= DATEADD(DAY, 1 - @Days, CAST(GETDATE() AS DATE))
                WHERE p.TrangThai = 1
                  AND p.SoLuongTon >= @StockThreshold
                GROUP BY p.MaSP, p.TenSP, p.MaVach, p.SoLuongTon
                HAVING ISNULL(SUM(CASE WHEN i.TrangThai = 'Paid' THEN id.SoLuong ELSE 0 END), 0) <= @SoldThreshold
                ORDER BY p.SoLuongTon DESC, QuantitySold ASC";

            return _dbHelper.Query<ChatBotProductInsightResponse>(sql, new
            {
                StockThreshold = stockThreshold,
                SoldThreshold = soldThreshold,
                Days = days,
                Take = take
            });
        }

        public IEnumerable<ChatBotProductInsightResponse> GetRestockSuggestions(int stockThreshold, int days, int take)
        {
            string sql = @"
                SELECT TOP (@Take)
                    p.MaSP,
                    p.TenSP,
                    p.MaVach,
                    p.SoLuongTon,
                    ISNULL(SUM(id.SoLuong), 0) AS QuantitySold,
                    ISNULL(SUM(id.ThanhTien), 0) AS Revenue,
                    CAST(ISNULL(SUM(id.SoLuong), 0) * 1.0 / @Days AS DECIMAL(18,2)) AS AverageDailySold
                FROM Products p
                LEFT JOIN InvoiceDetails id ON id.MaSP = p.MaSP
                LEFT JOIN Invoices i ON i.MaHD = id.MaHD
                    AND i.TrangThai = 'Paid'
                    AND i.NgayLap >= DATEADD(DAY, 1 - @Days, CAST(GETDATE() AS DATE))
                WHERE p.TrangThai = 1
                  AND p.SoLuongTon <= @StockThreshold
                  AND (p.HanSuDung IS NULL OR p.HanSuDung >= CAST(GETDATE() AS DATE))
                GROUP BY p.MaSP, p.TenSP, p.MaVach, p.SoLuongTon
                ORDER BY p.SoLuongTon ASC, AverageDailySold DESC, QuantitySold DESC";

            return _dbHelper.Query<ChatBotProductInsightResponse>(sql, new
            {
                StockThreshold = stockThreshold,
                Days = days,
                Take = take
            });
        }
    }
}
