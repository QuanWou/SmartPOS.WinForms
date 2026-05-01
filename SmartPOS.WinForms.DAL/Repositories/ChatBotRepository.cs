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
