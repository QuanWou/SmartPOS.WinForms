using System;
using System.Collections.Generic;
using System.Text;
using SmartPOS.WinForms.DAL.Data;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.DAL.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DbHelper _dbHelper;

        public CustomerRepository()
        {
            _dbHelper = new DbHelper();
        }

        public IEnumerable<CustomerDTO> GetAll()
        {
            string sql = GetCustomerSelectSql() + @"
                ORDER BY c.MaKH DESC";

            return _dbHelper.Query<CustomerDTO>(sql);
        }

        public CustomerDTO GetById(int maKH)
        {
            string sql = GetCustomerSelectSql() + @"
                WHERE c.MaKH = @MaKH";

            return _dbHelper.QueryFirstOrDefault<CustomerDTO>(sql, new { MaKH = maKH });
        }

        public CustomerDTO GetByPhone(string soDienThoai)
        {
            string sql = GetCustomerSelectSql() + @"
                WHERE c.SoDienThoai = @SoDienThoai";

            return _dbHelper.QueryFirstOrDefault<CustomerDTO>(sql, new { SoDienThoai = soDienThoai });
        }

        public IEnumerable<CustomerDTO> Search(CustomerSearchRequest request)
        {
            StringBuilder sqlBuilder = new StringBuilder(GetCustomerSelectSql() + @"
                WHERE 1 = 1");

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                sqlBuilder.Append(@"
                    AND (
                        c.HoTen LIKE @Keyword
                        OR c.SoDienThoai LIKE @Keyword
                        OR c.DiaChi LIKE @Keyword
                    )");
            }

            if (!string.IsNullOrWhiteSpace(request.HangThanhVien))
            {
                sqlBuilder.Append(@"
                    AND c.HangThanhVien = @HangThanhVien");
            }

            if (request.TrangThai.HasValue)
            {
                sqlBuilder.Append(@"
                    AND c.TrangThai = @TrangThai");
            }

            sqlBuilder.Append(@"
                ORDER BY c.MaKH DESC");

            return _dbHelper.Query<CustomerDTO>(
                sqlBuilder.ToString(),
                new
                {
                    Keyword = "%" + request.Keyword + "%",
                    request.HangThanhVien,
                    request.TrangThai
                });
        }

        public int Insert(CustomerDTO customer)
        {
            string sql = @"
                INSERT INTO Customers
                (
                    HoTen,
                    SoDienThoai,
                    DiaChi,
                    NgayThamGia,
                    HangThanhVien,
                    TongChiTieu,
                    SoLanMua,
                    DiemHienCo,
                    TongDiemDaDoi,
                    TrangThai,
                    NgayCapNhat
                )
                VALUES
                (
                    @HoTen,
                    @SoDienThoai,
                    @DiaChi,
                    @NgayThamGia,
                    @HangThanhVien,
                    @TongChiTieu,
                    @SoLanMua,
                    @DiemHienCo,
                    @TongDiemDaDoi,
                    @TrangThai,
                    @NgayCapNhat
                );
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            object value = _dbHelper.ExecuteScalar(sql, customer);
            return Convert.ToInt32(value);
        }

        public int Update(CustomerDTO customer)
        {
            string sql = @"
                UPDATE Customers
                SET
                    HoTen = @HoTen,
                    SoDienThoai = @SoDienThoai,
                    DiaChi = @DiaChi,
                    HangThanhVien = @HangThanhVien,
                    TrangThai = @TrangThai,
                    NgayCapNhat = GETDATE()
                WHERE MaKH = @MaKH";

            return _dbHelper.Execute(sql, customer);
        }

        public int UpdateStatus(int maKH, bool trangThai)
        {
            string sql = @"
                UPDATE Customers
                SET
                    TrangThai = @TrangThai,
                    NgayCapNhat = GETDATE()
                WHERE MaKH = @MaKH";

            return _dbHelper.Execute(sql, new { MaKH = maKH, TrangThai = trangThai });
        }

        public CustomerStatsResponse GetStats()
        {
            string sql = @"
                SELECT
                    COUNT(1) AS TotalCustomers,
                    ISNULL(SUM(CASE WHEN TrangThai = 1 THEN 1 ELSE 0 END), 0) AS MemberCustomers,
                    ISNULL(SUM(CASE
                            WHEN NgayThamGia >= DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)
                                THEN 1
                            ELSE 0
                        END), 0) AS NewCustomersThisMonth,
                    ISNULL(SUM(TongDiemDaDoi), 0) AS RedeemedPoints,
                    ISNULL(SUM(TongChiTieu), 0) AS TotalSpend
                FROM Customers";

            return _dbHelper.QueryFirstOrDefault<CustomerStatsResponse>(sql) ?? new CustomerStatsResponse();
        }

        public IEnumerable<CustomerPurchaseHistoryResponse> GetPurchaseHistory(int maKH)
        {
            string sql = @"
                SELECT
                    MaHD,
                    NgayLap,
                    ISNULL(TongTienTruocGiam, TongTien) AS TongTienTruocGiam,
                    ISNULL(DiemSuDung, 0) AS DiemSuDung,
                    ISNULL(GiamGiaDiem, 0) AS GiamGiaDiem,
                    TongTien,
                    TrangThai
                FROM Invoices
                WHERE MaKH = @MaKH
                ORDER BY NgayLap DESC, MaHD DESC";

            return _dbHelper.Query<CustomerPurchaseHistoryResponse>(sql, new { MaKH = maKH });
        }

        public IEnumerable<CustomerPointTransactionDTO> GetPointHistory(int maKH)
        {
            string sql = @"
                SELECT
                    MaGD,
                    MaKH,
                    MaHD,
                    MaNV,
                    LoaiGiaoDich,
                    Diem,
                    GiaTriGiam,
                    GhiChu,
                    NgayTao
                FROM CustomerPointTransactions
                WHERE MaKH = @MaKH
                ORDER BY NgayTao DESC, MaGD DESC";

            return _dbHelper.Query<CustomerPointTransactionDTO>(sql, new { MaKH = maKH });
        }

        public IEnumerable<CustomerCategoryTrendResponse> GetCategoryTrends(int maKH)
        {
            string sql = @"
                WITH CategoryData AS
                (
                    SELECT
                        c.TenLoai,
                        SUM(id.SoLuong) AS SoLuong,
                        SUM(id.ThanhTien) AS TongTien
                    FROM Invoices i
                    INNER JOIN InvoiceDetails id ON id.MaHD = i.MaHD
                    INNER JOIN Products p ON p.MaSP = id.MaSP
                    INNER JOIN Categories c ON c.MaLoai = p.MaLoai
                    WHERE i.MaKH = @MaKH
                      AND i.TrangThai = 'Paid'
                    GROUP BY c.TenLoai
                )
                SELECT
                    TenLoai,
                    SoLuong,
                    TongTien,
                    CAST(CASE
                        WHEN SUM(SoLuong) OVER() = 0 THEN 0
                        ELSE SoLuong * 100.0 / SUM(SoLuong) OVER()
                    END AS DECIMAL(9,2)) AS TyLe
                FROM CategoryData
                ORDER BY SoLuong DESC, TongTien DESC";

            return _dbHelper.Query<CustomerCategoryTrendResponse>(sql, new { MaKH = maKH });
        }

        public IEnumerable<CustomerTopProductResponse> GetTopProducts(int maKH)
        {
            string sql = @"
                SELECT TOP 5
                    p.MaSP,
                    p.TenSP,
                    SUM(id.SoLuong) AS SoLuong,
                    COUNT(DISTINCT i.MaHD) AS SoLanMua,
                    SUM(id.ThanhTien) AS TongTien
                FROM Invoices i
                INNER JOIN InvoiceDetails id ON id.MaHD = i.MaHD
                INNER JOIN Products p ON p.MaSP = id.MaSP
                WHERE i.MaKH = @MaKH
                  AND i.TrangThai = 'Paid'
                GROUP BY p.MaSP, p.TenSP
                ORDER BY SUM(id.SoLuong) DESC, SUM(id.ThanhTien) DESC";

            return _dbHelper.Query<CustomerTopProductResponse>(sql, new { MaKH = maKH });
        }

        private string GetCustomerSelectSql()
        {
            return @"
                SELECT
                    c.MaKH,
                    c.HoTen,
                    c.SoDienThoai,
                    c.DiaChi,
                    c.NgayThamGia,
                    c.HangThanhVien,
                    c.TongChiTieu,
                    c.SoLanMua,
                    c.DiemHienCo,
                    c.TongDiemDaDoi,
                    c.TrangThai,
                    c.NgayCapNhat,
                    lastPurchase.LanMuaGanNhat,
                    topProduct.TopProduct
                FROM Customers c
                OUTER APPLY
                (
                    SELECT MAX(i.NgayLap) AS LanMuaGanNhat
                    FROM Invoices i
                    WHERE i.MaKH = c.MaKH
                      AND i.TrangThai = 'Paid'
                ) lastPurchase
                OUTER APPLY
                (
                    SELECT TOP 1 p.TenSP AS TopProduct
                    FROM Invoices i
                    INNER JOIN InvoiceDetails id ON id.MaHD = i.MaHD
                    INNER JOIN Products p ON p.MaSP = id.MaSP
                    WHERE i.MaKH = c.MaKH
                      AND i.TrangThai = 'Paid'
                    GROUP BY p.TenSP
                    ORDER BY SUM(id.SoLuong) DESC, SUM(id.ThanhTien) DESC
                ) topProduct";
        }
    }
}
