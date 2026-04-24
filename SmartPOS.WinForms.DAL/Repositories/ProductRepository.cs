using System.Collections.Generic;
using System.Text;
using Dapper;
using SmartPOS.WinForms.DAL.Data;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;

namespace SmartPOS.WinForms.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbHelper _dbHelper;

        public ProductRepository()
        {
            _dbHelper = new DbHelper();
        }

        public IEnumerable<ProductDTO> GetAll()
        {
            string sql = @"
                SELECT 
                    p.MaSP,
                    p.TenSP,
                    p.MaVach,
                    p.DonViTinh,
                    p.GiaNhap,
                    p.GiaBan,
                    COALESCE(lot.SellableQty, p.SoLuongTon) AS SoLuongTon,
                    p.MaLoai,
                    p.HinhAnh,
                    p.MoTa,
                    COALESCE(lot.NextExpiry, p.HanSuDung) AS HanSuDung,
                    p.TrangThai,
                    p.NgayTao,
                    p.NgayCapNhat
                FROM Products p
                OUTER APPLY
                (
                    SELECT
                        SUM(CASE
                            WHEN pl.HanSuDung IS NULL OR pl.HanSuDung >= CAST(GETDATE() AS DATE)
                                THEN pl.SoLuongTonLo
                            ELSE 0
                        END) AS SellableQty,
                        MIN(CASE
                            WHEN pl.SoLuongTonLo > 0
                                 AND pl.HanSuDung IS NOT NULL
                                 AND pl.HanSuDung >= CAST(GETDATE() AS DATE)
                                THEN pl.HanSuDung
                        END) AS NextExpiry
                    FROM ProductLots pl
                    WHERE pl.MaSP = p.MaSP
                ) lot
                ORDER BY p.MaSP DESC";

            return _dbHelper.Query<ProductDTO>(sql);
        }

        public ProductDTO GetById(int maSP)
        {
            string sql = @"
                SELECT 
                    p.MaSP,
                    p.TenSP,
                    p.MaVach,
                    p.DonViTinh,
                    p.GiaNhap,
                    p.GiaBan,
                    COALESCE(lot.SellableQty, p.SoLuongTon) AS SoLuongTon,
                    p.MaLoai,
                    p.HinhAnh,
                    p.MoTa,
                    COALESCE(lot.NextExpiry, p.HanSuDung) AS HanSuDung,
                    p.TrangThai,
                    p.NgayTao,
                    p.NgayCapNhat
                FROM Products p
                OUTER APPLY
                (
                    SELECT
                        SUM(CASE
                            WHEN pl.HanSuDung IS NULL OR pl.HanSuDung >= CAST(GETDATE() AS DATE)
                                THEN pl.SoLuongTonLo
                            ELSE 0
                        END) AS SellableQty,
                        MIN(CASE
                            WHEN pl.SoLuongTonLo > 0
                                 AND pl.HanSuDung IS NOT NULL
                                 AND pl.HanSuDung >= CAST(GETDATE() AS DATE)
                                THEN pl.HanSuDung
                        END) AS NextExpiry
                    FROM ProductLots pl
                    WHERE pl.MaSP = p.MaSP
                ) lot
                WHERE p.MaSP = @MaSP";

            return _dbHelper.QueryFirstOrDefault<ProductDTO>(sql, new { MaSP = maSP });
        }

        public ProductDTO GetByBarcode(string maVach)
        {
            string sql = @"
                SELECT 
                    p.MaSP,
                    p.TenSP,
                    p.MaVach,
                    p.DonViTinh,
                    p.GiaNhap,
                    p.GiaBan,
                    COALESCE(lot.SellableQty, p.SoLuongTon) AS SoLuongTon,
                    p.MaLoai,
                    p.HinhAnh,
                    p.MoTa,
                    COALESCE(lot.NextExpiry, p.HanSuDung) AS HanSuDung,
                    p.TrangThai,
                    p.NgayTao,
                    p.NgayCapNhat
                FROM Products p
                OUTER APPLY
                (
                    SELECT
                        SUM(CASE
                            WHEN pl.HanSuDung IS NULL OR pl.HanSuDung >= CAST(GETDATE() AS DATE)
                                THEN pl.SoLuongTonLo
                            ELSE 0
                        END) AS SellableQty,
                        MIN(CASE
                            WHEN pl.SoLuongTonLo > 0
                                 AND pl.HanSuDung IS NOT NULL
                                 AND pl.HanSuDung >= CAST(GETDATE() AS DATE)
                                THEN pl.HanSuDung
                        END) AS NextExpiry
                    FROM ProductLots pl
                    WHERE pl.MaSP = p.MaSP
                ) lot
                WHERE p.MaVach = @MaVach
                  AND p.TrangThai = 1";

            return _dbHelper.QueryFirstOrDefault<ProductDTO>(sql, new { MaVach = maVach });
        }

        public IEnumerable<ProductDTO> Search(ProductSearchRequest request)
        {
            StringBuilder sqlBuilder = new StringBuilder(@"
                SELECT 
                    p.MaSP,
                    p.TenSP,
                    p.MaVach,
                    p.DonViTinh,
                    p.GiaNhap,
                    p.GiaBan,
                    COALESCE(lot.SellableQty, p.SoLuongTon) AS SoLuongTon,
                    p.MaLoai,
                    p.HinhAnh,
                    p.MoTa,
                    COALESCE(lot.NextExpiry, p.HanSuDung) AS HanSuDung,
                    p.TrangThai,
                    p.NgayTao,
                    p.NgayCapNhat
                FROM Products p
                OUTER APPLY
                (
                    SELECT
                        SUM(CASE
                            WHEN pl.HanSuDung IS NULL OR pl.HanSuDung >= CAST(GETDATE() AS DATE)
                                THEN pl.SoLuongTonLo
                            ELSE 0
                        END) AS SellableQty,
                        MIN(CASE
                            WHEN pl.SoLuongTonLo > 0
                                 AND pl.HanSuDung IS NOT NULL
                                 AND pl.HanSuDung >= CAST(GETDATE() AS DATE)
                                THEN pl.HanSuDung
                        END) AS NextExpiry
                    FROM ProductLots pl
                    WHERE pl.MaSP = p.MaSP
                ) lot
                WHERE 1 = 1");

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                sqlBuilder.Append(@"
                    AND (
                        p.TenSP LIKE @Keyword
                        OR p.MaVach LIKE @Keyword
                    )");
            }

            if (request.MaLoai.HasValue)
            {
                sqlBuilder.Append(@"
                    AND p.MaLoai = @MaLoai");
            }

            if (request.TrangThai.HasValue)
            {
                sqlBuilder.Append(@"
                    AND p.TrangThai = @TrangThai");
            }

            sqlBuilder.Append(@"
                ORDER BY p.MaSP DESC");

            return _dbHelper.Query<ProductDTO>(
                sqlBuilder.ToString(),
                new
                {
                    Keyword = "%" + request.Keyword + "%",
                    request.MaLoai,
                    request.TrangThai
                });
        }

        public int Insert(ProductDTO product)
        {
            string sql = @"
                INSERT INTO Products
                (
                    TenSP,
                    MaVach,
                    DonViTinh,
                    GiaNhap,
                    GiaBan,
                    SoLuongTon,
                    MaLoai,
                    HinhAnh,
                    MoTa,
                    HanSuDung,
                    TrangThai,
                    NgayTao,
                    NgayCapNhat
                )
                VALUES
                (
                    @TenSP,
                    @MaVach,
                    @DonViTinh,
                    @GiaNhap,
                    @GiaBan,
                    @SoLuongTon,
                    @MaLoai,
                    @HinhAnh,
                    @MoTa,
                    @HanSuDung,
                    @TrangThai,
                    @NgayTao,
                    @NgayCapNhat
                )";

            return _dbHelper.Execute(sql, product);
        }

        public int Update(ProductDTO product)
        {
            string sql = @"
                UPDATE Products
                SET
                    TenSP = @TenSP,
                    MaVach = @MaVach,
                    DonViTinh = @DonViTinh,
                    GiaNhap = @GiaNhap,
                    GiaBan = @GiaBan,
                    SoLuongTon = @SoLuongTon,
                    MaLoai = @MaLoai,
                    HinhAnh = @HinhAnh,
                    MoTa = @MoTa,
                    HanSuDung = @HanSuDung,
                    TrangThai = @TrangThai,
                    NgayCapNhat = @NgayCapNhat
                WHERE MaSP = @MaSP";

            return _dbHelper.Execute(sql, product);
        }

        public bool HasTransactionHistory(int maSP)
        {
            string sql = @"
                SELECT CAST(
                    CASE
                        WHEN EXISTS (SELECT 1 FROM InvoiceDetails WHERE MaSP = @MaSP)
                          OR EXISTS (SELECT 1 FROM StockInDetails WHERE MaSP = @MaSP)
                          OR EXISTS (
                                SELECT 1
                                FROM ProductLots
                                WHERE MaSP = @MaSP
                                  AND (MaPN IS NOT NULL OR MaCTPN IS NOT NULL)
                            )
                          OR EXISTS (
                                SELECT 1
                                FROM InvoiceLotAllocations ila
                                INNER JOIN ProductLots pl ON pl.MaLo = ila.MaLo
                                WHERE pl.MaSP = @MaSP
                            )
                            THEN 1
                        ELSE 0
                    END AS BIT)";

            return _dbHelper.QueryFirstOrDefault<bool>(sql, new { MaSP = maSP });
        }

        public int Delete(int maSP)
        {
            using (var scope = new DbTransactionScope())
            {
                try
                {
                    // Delete InvoiceLotAllocations that reference this product's lots
                    scope.Connection.Execute(
                        @"DELETE FROM InvoiceLotAllocations 
                          WHERE MaLo IN (SELECT MaLo FROM ProductLots WHERE MaSP = @MaSP)",
                        new { MaSP = maSP },
                        scope.Transaction);

                    // Delete InvoiceDetails that reference this product
                    scope.Connection.Execute(
                        "DELETE FROM InvoiceDetails WHERE MaSP = @MaSP",
                        new { MaSP = maSP },
                        scope.Transaction);

                    // Delete ProductLots for this product (must be before StockInDetails)
                    scope.Connection.Execute(
                        "DELETE FROM ProductLots WHERE MaSP = @MaSP",
                        new { MaSP = maSP },
                        scope.Transaction);

                    // Delete StockInDetails for this product
                    scope.Connection.Execute(
                        "DELETE FROM StockInDetails WHERE MaSP = @MaSP",
                        new { MaSP = maSP },
                        scope.Transaction);

                    // Delete the product itself
                    int rowsAffected = scope.Connection.Execute(
                        "DELETE FROM Products WHERE MaSP = @MaSP",
                        new { MaSP = maSP },
                        scope.Transaction);

                    scope.Commit();
                    return rowsAffected;
                }
                catch
                {
                    scope.Rollback();
                    throw;
                }
            }
        }

        public int UpdateStatus(int maSP, bool trangThai)
        {
            string sql = @"
                UPDATE Products
                SET
                    TrangThai = @TrangThai,
                    NgayCapNhat = GETDATE()
                WHERE MaSP = @MaSP";

            return _dbHelper.Execute(sql, new
            {
                MaSP = maSP,
                TrangThai = trangThai
            });
        }

        public int UpdateStock(int maSP, int soLuongTonMoi)
        {
            string sql = @"
                UPDATE Products
                SET
                    SoLuongTon = @SoLuongTonMoi,
                    NgayCapNhat = GETDATE()
                WHERE MaSP = @MaSP";

            return _dbHelper.Execute(sql, new
            {
                MaSP = maSP,
                SoLuongTonMoi = soLuongTonMoi
            });
        }
    }
}
