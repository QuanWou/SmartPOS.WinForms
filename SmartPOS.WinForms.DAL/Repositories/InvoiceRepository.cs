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
                            );
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        int maCTHD = scope.Connection.ExecuteScalar<int>(
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

                        string sqlAvailableLots = @"
                            SELECT
                                MaLo,
                                MaPN,
                                MaCTPN,
                                MaSP,
                                NgayNhap,
                                HanSuDung,
                                SoLuongNhap,
                                SoLuongTonLo,
                                GiaNhapLucNhap,
                                GhiChu
                            FROM ProductLots
                            WHERE MaSP = @MaSP
                              AND SoLuongTonLo > 0
                              AND (HanSuDung IS NULL OR HanSuDung >= CAST(GETDATE() AS DATE))
                            ORDER BY
                                CASE WHEN HanSuDung IS NULL THEN 1 ELSE 0 END,
                                HanSuDung ASC,
                                NgayNhap ASC,
                                MaLo ASC";

                        List<ProductLotDTO> availableLots = new List<ProductLotDTO>(
                            scope.Connection.Query<ProductLotDTO>(
                                sqlAvailableLots,
                                new { item.MaSP },
                                scope.Transaction));

                        int totalAvailableQuantity = 0;
                        foreach (ProductLotDTO lot in availableLots)
                        {
                            totalAvailableQuantity += lot.SoLuongTonLo;
                        }

                        if (totalAvailableQuantity < item.SoLuong)
                        {
                            throw new InvalidOperationException(
                                "Không đủ tồn kho theo lô để thanh toán. Vui lòng tải lại giỏ hàng và thử lại.");
                        }

                        int remainingQuantity = item.SoLuong;
                        foreach (ProductLotDTO lot in availableLots)
                        {
                            if (remainingQuantity <= 0)
                            {
                                break;
                            }

                            int allocatedQuantity = remainingQuantity > lot.SoLuongTonLo
                                ? lot.SoLuongTonLo
                                : remainingQuantity;

                            string sqlUpdateLot = @"
                                UPDATE ProductLots
                                SET SoLuongTonLo = SoLuongTonLo - @AllocatedQuantity
                                WHERE MaLo = @MaLo
                                  AND SoLuongTonLo >= @AllocatedQuantity";

                            int lotRowsAffected = scope.Connection.Execute(
                                sqlUpdateLot,
                                new
                                {
                                    lot.MaLo,
                                    AllocatedQuantity = allocatedQuantity
                                },
                                scope.Transaction);

                            if (lotRowsAffected <= 0)
                            {
                                throw new InvalidOperationException(
                                    "Tồn kho lô hàng vừa thay đổi. Vui lòng tải lại giỏ hàng và thử lại.");
                            }

                            string sqlAllocation = @"
                                INSERT INTO InvoiceLotAllocations
                                (
                                    MaHD,
                                    MaCTHD,
                                    MaLo,
                                    SoLuong
                                )
                                VALUES
                                (
                                    @MaHD,
                                    @MaCTHD,
                                    @MaLo,
                                    @SoLuong
                                )";

                            scope.Connection.Execute(
                                sqlAllocation,
                                new
                                {
                                    MaHD = maHD,
                                    MaCTHD = maCTHD,
                                    MaLo = lot.MaLo,
                                    SoLuong = allocatedQuantity
                                },
                                scope.Transaction);

                            remainingQuantity -= allocatedQuantity;
                        }

                        string sqlUpdateStock = @"
                            UPDATE Products
                            SET
                                SoLuongTon = (
                                    SELECT ISNULL(SUM(CASE
                                        WHEN pl.HanSuDung IS NULL OR pl.HanSuDung >= CAST(GETDATE() AS DATE)
                                            THEN pl.SoLuongTonLo
                                        ELSE 0
                                    END), 0)
                                    FROM ProductLots pl
                                    WHERE pl.MaSP = @MaSP
                                ),
                                HanSuDung = (
                                    SELECT MIN(pl.HanSuDung)
                                    FROM ProductLots pl
                                    WHERE pl.MaSP = @MaSP
                                      AND pl.SoLuongTonLo > 0
                                      AND pl.HanSuDung IS NOT NULL
                                      AND pl.HanSuDung >= CAST(GETDATE() AS DATE)
                                ),
                                NgayCapNhat = GETDATE()
                            WHERE MaSP = @MaSP
                              AND TrangThai = 1";

                        int rowsAffected = scope.Connection.Execute(
                            sqlUpdateStock,
                            new
                            {
                                item.MaSP
                            },
                            scope.Transaction);

                        if (rowsAffected <= 0)
                        {
                            throw new InvalidOperationException(
                                "Không thể đồng bộ tồn kho sản phẩm sau khi trừ lô hàng.");
                        }
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
