using System.Collections.Generic;
using SmartPOS.WinForms.DAL.Data;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DTO.Entities;

namespace SmartPOS.WinForms.DAL.Repositories
{
    public class ProductLotRepository : IProductLotRepository
    {
        private readonly DbHelper _dbHelper;

        public ProductLotRepository()
        {
            _dbHelper = new DbHelper();
        }

        public IEnumerable<ProductLotDTO> GetAll()
        {
            string sql = @"
                SELECT
                    pl.MaLo,
                    pl.MaPN,
                    pl.MaCTPN,
                    pl.MaSP,
                    pl.NgayNhap,
                    pl.HanSuDung,
                    pl.SoLuongNhap,
                    pl.SoLuongTonLo,
                    pl.GiaNhapLucNhap,
                    pl.GhiChu,
                    p.TenSP,
                    p.MaVach,
                    p.TrangThai AS TrangThaiSanPham
                FROM ProductLots pl
                INNER JOIN Products p ON p.MaSP = pl.MaSP
                ORDER BY pl.HanSuDung ASC, pl.NgayNhap ASC, pl.MaLo ASC";

            return _dbHelper.Query<ProductLotDTO>(sql);
        }

        public IEnumerable<ProductLotDTO> GetByProductId(int maSP)
        {
            string sql = @"
                SELECT
                    pl.MaLo,
                    pl.MaPN,
                    pl.MaCTPN,
                    pl.MaSP,
                    pl.NgayNhap,
                    pl.HanSuDung,
                    pl.SoLuongNhap,
                    pl.SoLuongTonLo,
                    pl.GiaNhapLucNhap,
                    pl.GhiChu,
                    p.TenSP,
                    p.MaVach,
                    p.TrangThai AS TrangThaiSanPham
                FROM ProductLots pl
                INNER JOIN Products p ON p.MaSP = pl.MaSP
                WHERE pl.MaSP = @MaSP
                ORDER BY pl.HanSuDung ASC, pl.NgayNhap ASC, pl.MaLo ASC";

            return _dbHelper.Query<ProductLotDTO>(sql, new { MaSP = maSP });
        }

        public IEnumerable<ProductLotDTO> GetExpiringLots(int days)
        {
            string sql = @"
                SELECT
                    pl.MaLo,
                    pl.MaPN,
                    pl.MaCTPN,
                    pl.MaSP,
                    pl.NgayNhap,
                    pl.HanSuDung,
                    pl.SoLuongNhap,
                    pl.SoLuongTonLo,
                    pl.GiaNhapLucNhap,
                    pl.GhiChu,
                    p.TenSP,
                    p.MaVach,
                    p.TrangThai AS TrangThaiSanPham
                FROM ProductLots pl
                INNER JOIN Products p ON p.MaSP = pl.MaSP
                WHERE pl.SoLuongTonLo > 0
                  AND pl.HanSuDung IS NOT NULL
                  AND pl.HanSuDung >= CAST(GETDATE() AS DATE)
                  AND pl.HanSuDung <= DATEADD(DAY, @Days, CAST(GETDATE() AS DATE))
                ORDER BY pl.HanSuDung ASC, pl.NgayNhap ASC, pl.MaLo ASC";

            return _dbHelper.Query<ProductLotDTO>(sql, new { Days = days });
        }
    }
}
