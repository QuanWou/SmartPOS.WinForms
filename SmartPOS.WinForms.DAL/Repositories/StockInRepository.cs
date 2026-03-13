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
                                ThanhTien
                            )
                            VALUES
                            (
                                @MaPN,
                                @MaSP,
                                @SoLuong,
                                @GiaNhapLucNhap,
                                @ThanhTien
                            )";

                        scope.Connection.Execute(
                            sqlDetail,
                            new
                            {
                                MaPN = maPN,
                                item.MaSP,
                                item.SoLuong,
                                item.GiaNhapLucNhap,
                                item.ThanhTien
                            },
                            scope.Transaction);

                        string sqlUpdateProduct = @"
                            UPDATE Products
                            SET
                                SoLuongTon = SoLuongTon + @SoLuong,
                                GiaNhap = @GiaNhapLucNhap,
                                NgayCapNhat = GETDATE()
                            WHERE MaSP = @MaSP";

                        scope.Connection.Execute(
                            sqlUpdateProduct,
                            new
                            {
                                item.MaSP,
                                item.SoLuong,
                                item.GiaNhapLucNhap
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