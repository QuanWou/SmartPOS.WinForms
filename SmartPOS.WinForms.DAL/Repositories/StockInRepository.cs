using System;
using System.Collections.Generic;
using Dapper;
using SmartPOS.WinForms.DAL.Data;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;

namespace SmartPOS.WinForms.DAL.Repositories
{
    public class StockInRepository : IStockInRepository
    {
        private readonly DbHelper _dbHelper;

        public StockInRepository()
        {
            _dbHelper = new DbHelper();
        }

        public IEnumerable<StockInDTO> GetAll()
        {
            string sql = @"
                SELECT
                    MaPN,
                    NgayNhap,
                    MaNV,
                    TongTien,
                    GhiChu
                FROM StockIns
                ORDER BY MaPN DESC";

            return _dbHelper.Query<StockInDTO>(sql);
        }

        public StockInDTO GetById(int maPN)
        {
            string sql = @"
                SELECT
                    MaPN,
                    NgayNhap,
                    MaNV,
                    TongTien,
                    GhiChu
                FROM StockIns
                WHERE MaPN = @MaPN";

            return _dbHelper.QueryFirstOrDefault<StockInDTO>(sql, new { MaPN = maPN });
        }

        public IEnumerable<StockInDetailDTO> GetDetailsByStockInId(int maPN)
        {
            string sql = @"
                SELECT
                    MaCTPN,
                    MaPN,
                    MaSP,
                    SoLuong,
                    GiaNhapLucNhap,
                    HanSuDung,
                    ThanhTien
                FROM StockInDetails
                WHERE MaPN = @MaPN
                ORDER BY MaCTPN ASC";

            return _dbHelper.Query<StockInDetailDTO>(sql, new { MaPN = maPN });
        }

        public int Insert(StockInRequest request)
        {
            using (var scope = new DbTransactionScope())
            {
                try
                {
                    decimal tongTien = 0;
                    foreach (var item in request.ChiTietNhapKho)
                    {
                        tongTien += item.ThanhTien;
                    }

                    string sqlStockIn = @"
                        INSERT INTO StockIns
                        (
                            NgayNhap,
                            MaNV,
                            TongTien,
                            GhiChu
                        )
                        VALUES
                        (
                            GETDATE(),
                            @MaNV,
                            @TongTien,
                            @GhiChu
                        );
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    int maPN = scope.Connection.ExecuteScalar<int>(
                        sqlStockIn,
                        new
                        {
                            request.MaNV,
                            TongTien = tongTien,
                            request.GhiChu
                        },
                        scope.Transaction);

                    foreach (var item in request.ChiTietNhapKho)
                    {
                        string sqlDetail = @"
                            INSERT INTO StockInDetails
                            (
                                MaPN,
                                MaSP,
                                SoLuong,
                                GiaNhapLucNhap,
                                HanSuDung,
                                ThanhTien
                            )
                            VALUES
                            (
                                @MaPN,
                                @MaSP,
                                @SoLuong,
                                @GiaNhapLucNhap,
                                @HanSuDung,
                                @ThanhTien
                            );
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        int maCTPN = scope.Connection.ExecuteScalar<int>(
                            sqlDetail,
                            new
                            {
                                MaPN = maPN,
                                item.MaSP,
                                item.SoLuong,
                                item.GiaNhapLucNhap,
                                item.HanSuDung,
                                item.ThanhTien
                            },
                            scope.Transaction);

                        string sqlLot = @"
                            INSERT INTO ProductLots
                            (
                                MaPN,
                                MaCTPN,
                                MaSP,
                                NgayNhap,
                                HanSuDung,
                                SoLuongNhap,
                                SoLuongTonLo,
                                GiaNhapLucNhap,
                                GhiChu
                            )
                            VALUES
                            (
                                @MaPN,
                                @MaCTPN,
                                @MaSP,
                                GETDATE(),
                                @HanSuDung,
                                @SoLuongNhap,
                                @SoLuongTonLo,
                                @GiaNhapLucNhap,
                                @GhiChu
                            )";

                        scope.Connection.Execute(
                            sqlLot,
                            new
                            {
                                MaPN = maPN,
                                MaCTPN = maCTPN,
                                item.MaSP,
                                item.HanSuDung,
                                SoLuongNhap = item.SoLuong,
                                SoLuongTonLo = item.SoLuong,
                                item.GiaNhapLucNhap,
                                request.GhiChu
                            },
                            scope.Transaction);

                        string sqlUpdateProduct = @"
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
                                GiaNhap = @GiaNhapLucNhap,
                                HanSuDung = (
                                    SELECT MIN(pl.HanSuDung)
                                    FROM ProductLots pl
                                    WHERE pl.MaSP = @MaSP
                                      AND pl.SoLuongTonLo > 0
                                      AND pl.HanSuDung IS NOT NULL
                                      AND pl.HanSuDung >= CAST(GETDATE() AS DATE)
                                ),
                                NgayCapNhat = GETDATE()
                            WHERE MaSP = @MaSP";

                        scope.Connection.Execute(
                            sqlUpdateProduct,
                            new
                            {
                                item.MaSP,
                                item.SoLuong,
                                item.GiaNhapLucNhap,
                                item.HanSuDung
                            },
                            scope.Transaction);
                    }

                    scope.Commit();
                    return maPN;
                }
                catch
                {
                    scope.Rollback();
                    throw;
                }
            }
        }
    }
}
