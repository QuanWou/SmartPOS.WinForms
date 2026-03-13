using System.Collections.Generic;
using System.Text;
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
                    MaSP,
                    TenSP,
                    MaVach,
                    DonViTinh,
                    GiaNhap,
                    GiaBan,
                    SoLuongTon,
                    MaLoai,
                    HinhAnh,
                    MoTa,
                    TrangThai,
                    NgayTao,
                    NgayCapNhat
                FROM Products
                ORDER BY MaSP DESC";

            return _dbHelper.Query<ProductDTO>(sql);
        }

        public ProductDTO GetById(int maSP)
        {
            string sql = @"
                SELECT 
                    MaSP,
                    TenSP,
                    MaVach,
                    DonViTinh,
                    GiaNhap,
                    GiaBan,
                    SoLuongTon,
                    MaLoai,
                    HinhAnh,
                    MoTa,
                    TrangThai,
                    NgayTao,
                    NgayCapNhat
                FROM Products
                WHERE MaSP = @MaSP";

            return _dbHelper.QueryFirstOrDefault<ProductDTO>(sql, new { MaSP = maSP });
        }

        public ProductDTO GetByBarcode(string maVach)
        {
            string sql = @"
                SELECT 
                    MaSP,
                    TenSP,
                    MaVach,
                    DonViTinh,
                    GiaNhap,
                    GiaBan,
                    SoLuongTon,
                    MaLoai,
                    HinhAnh,
                    MoTa,
                    TrangThai,
                    NgayTao,
                    NgayCapNhat
                FROM Products
                WHERE MaVach = @MaVach
                  AND TrangThai = 1";

            return _dbHelper.QueryFirstOrDefault<ProductDTO>(sql, new { MaVach = maVach });
        }

        public IEnumerable<ProductDTO> Search(ProductSearchRequest request)
        {
            StringBuilder sqlBuilder = new StringBuilder(@"
                SELECT 
                    MaSP,
                    TenSP,
                    MaVach,
                    DonViTinh,
                    GiaNhap,
                    GiaBan,
                    SoLuongTon,
                    MaLoai,
                    HinhAnh,
                    MoTa,
                    TrangThai,
                    NgayTao,
                    NgayCapNhat
                FROM Products
                WHERE 1 = 1");

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                sqlBuilder.Append(@"
                    AND (
                        TenSP LIKE @Keyword
                        OR MaVach LIKE @Keyword
                    )");
            }

            if (request.MaLoai.HasValue)
            {
                sqlBuilder.Append(@"
                    AND MaLoai = @MaLoai");
            }

            if (request.TrangThai.HasValue)
            {
                sqlBuilder.Append(@"
                    AND TrangThai = @TrangThai");
            }

            sqlBuilder.Append(@"
                ORDER BY MaSP DESC");

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
                    TrangThai = @TrangThai,
                    NgayCapNhat = @NgayCapNhat
                WHERE MaSP = @MaSP";

            return _dbHelper.Execute(sql, product);
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