using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using SmartPOS.WinForms.DAL.Data;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;

namespace SmartPOS.WinForms.DAL.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly DbHelper _dbHelper;

        public InvoiceRepository()
        {
            _dbHelper = new DbHelper();
        }

        public IEnumerable<InvoiceDTO> GetAll()
        {
            string sql = @"
                SELECT
                    MaHD,
                    NgayLap,
                    MaNV,
                    TongTien,
                    GhiChu,
                    TrangThai
                FROM Invoices
                ORDER BY MaHD DESC";

            return _dbHelper.Query<InvoiceDTO>(sql);
        }

        public InvoiceDTO GetById(int maHD)
        {
            string sql = @"
                SELECT
                    MaHD,
                    NgayLap,
                    MaNV,
                    TongTien,
                    GhiChu,
                    TrangThai
                FROM Invoices
                WHERE MaHD = @MaHD";

            return _dbHelper.QueryFirstOrDefault<InvoiceDTO>(sql, new { MaHD = maHD });
        }

        public IEnumerable<InvoiceDetailDTO> GetDetailsByInvoiceId(int maHD)
        {
            string sql = @"
                SELECT
                    MaCTHD,
                    MaHD,
                    MaSP,
                    SoLuong,
                    DonGiaLucBan,
                    ThanhTien
                FROM InvoiceDetails
                WHERE MaHD = @MaHD
                ORDER BY MaCTHD ASC";

            return _dbHelper.Query<InvoiceDetailDTO>(sql, new { MaHD = maHD });
        }

        public int Insert(CheckoutRequest request)
        {
            using (var scope = new DbTransactionScope())
            {
                try
                {
                    decimal tongTien = 0;
                    foreach (var item in request.ChiTietHoaDon)
                    {
                        tongTien += item.ThanhTien;
                    }

                    string sqlInvoice = @"
                        INSERT INTO Invoices
                        (
                            NgayLap,
                            MaNV,
                            TongTien,
                            GhiChu,
                            TrangThai
                        )
                        VALUES
                        (
                            GETDATE(),
                            @MaNV,
                            @TongTien,
                            @GhiChu,
                            @TrangThai
                        );
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    int maHD = scope.Connection.ExecuteScalar<int>(
                        sqlInvoice,
                        new
                        {
                            request.MaNV,
                            TongTien = tongTien,
                            request.GhiChu,
                            TrangThai = "Paid"
                        },
                        scope.Transaction);

                    foreach (var item in request.ChiTietHoaDon)
                    {
                        string sqlDetail = @"
                            INSERT INTO InvoiceDetails
                            (
                                MaHD,
                                MaSP,
                                SoLuong,
                                DonGiaLucBan,
                                ThanhTien
                            )
                            VALUES
                            (
                                @MaHD,
                                @MaSP,
                                @SoLuong,
                                @DonGiaLucBan,
                                @ThanhTien
                            )";

                        scope.Connection.Execute(
                            sqlDetail,
                            new
                            {
                                MaHD = maHD,
                                item.MaSP,
                                item.SoLuong,
                                item.DonGiaLucBan,
                                item.ThanhTien
                            },
                            scope.Transaction);

                        string sqlUpdateStock = @"
                            UPDATE Products
                            SET
                                SoLuongTon = SoLuongTon - @SoLuong,
                                NgayCapNhat = GETDATE()
                            WHERE MaSP = @MaSP";

                        scope.Connection.Execute(
                            sqlUpdateStock,
                            new
                            {
                                item.MaSP,
                                item.SoLuong
                            },
                            scope.Transaction);
                    }

                    scope.Commit();
                    return maHD;
                }
                catch
                {
                    scope.Rollback();
                    throw;
                }
            }
        }

        public int UpdateStatus(int maHD, string trangThai)
        {
            string sql = @"
                UPDATE Invoices
                SET TrangThai = @TrangThai
                WHERE MaHD = @MaHD";

            return _dbHelper.Execute(sql, new
            {
                MaHD = maHD,
                TrangThai = trangThai
            });
        }
    }
}