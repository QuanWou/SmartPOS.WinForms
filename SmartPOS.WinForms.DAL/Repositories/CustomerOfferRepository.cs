using System;
using System.Collections.Generic;
using SmartPOS.WinForms.DAL.Data;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DTO.Entities;

namespace SmartPOS.WinForms.DAL.Repositories
{
    public class CustomerOfferRepository : ICustomerOfferRepository
    {
        private readonly DbHelper _dbHelper;

        public CustomerOfferRepository()
        {
            _dbHelper = new DbHelper();
        }

        public CustomerOfferDTO GetById(int maUuDai)
        {
            string sql = GetSelectSql() + @"
                WHERE o.MaUuDai = @MaUuDai";

            return _dbHelper.QueryFirstOrDefault<CustomerOfferDTO>(sql, new { MaUuDai = maUuDai });
        }

        public IEnumerable<CustomerOfferDTO> GetByCustomerId(int maKH)
        {
            string sql = GetSelectSql() + @"
                WHERE o.MaKH = @MaKH
                ORDER BY o.DaSuDung ASC, o.TrangThai DESC, o.NgayHetHan ASC, o.MaUuDai DESC";

            return _dbHelper.Query<CustomerOfferDTO>(sql, new { MaKH = maKH });
        }

        public IEnumerable<CustomerOfferDTO> GetAvailableByCustomerId(int maKH)
        {
            string sql = GetSelectSql() + @"
                WHERE o.MaKH = @MaKH
                  AND o.TrangThai = 1
                  AND o.DaSuDung = 0
                  AND (o.NgayHetHan IS NULL OR o.NgayHetHan >= CAST(GETDATE() AS DATE))
                ORDER BY
                    CASE WHEN o.NgayHetHan IS NULL THEN 1 ELSE 0 END,
                    o.NgayHetHan ASC,
                    o.MaUuDai DESC";

            return _dbHelper.Query<CustomerOfferDTO>(sql, new { MaKH = maKH });
        }

        public int Insert(CustomerOfferDTO offer)
        {
            string sql = @"
                INSERT INTO CustomerOffers
                (
                    MaKH,
                    TenUuDai,
                    PhanTramGiam,
                    NgayHetHan,
                    TrangThai,
                    DaSuDung,
                    GhiChu,
                    NgayTao,
                    NgayCapNhat
                )
                VALUES
                (
                    @MaKH,
                    @TenUuDai,
                    @PhanTramGiam,
                    @NgayHetHan,
                    @TrangThai,
                    0,
                    @GhiChu,
                    GETDATE(),
                    NULL
                );
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            object value = _dbHelper.ExecuteScalar(sql, offer);
            return Convert.ToInt32(value);
        }

        public int UpdateStatus(int maUuDai, bool trangThai)
        {
            string sql = @"
                UPDATE CustomerOffers
                SET
                    TrangThai = @TrangThai,
                    NgayCapNhat = GETDATE()
                WHERE MaUuDai = @MaUuDai";

            return _dbHelper.Execute(sql, new { MaUuDai = maUuDai, TrangThai = trangThai });
        }

        private string GetSelectSql()
        {
            return @"
                SELECT
                    o.MaUuDai,
                    o.MaKH,
                    o.TenUuDai,
                    o.PhanTramGiam,
                    o.NgayHetHan,
                    o.TrangThai,
                    o.DaSuDung,
                    o.MaHDDaDung,
                    o.NgaySuDung,
                    o.GhiChu,
                    o.NgayTao,
                    o.NgayCapNhat,
                    c.HoTen AS TenKhachHang
                FROM CustomerOffers o
                INNER JOIN Customers c ON c.MaKH = o.MaKH";
        }
    }
}
