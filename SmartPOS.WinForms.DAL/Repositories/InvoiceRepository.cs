using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using SmartPOS.WinForms.Common.Constants;
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
                    MaKH,
                    TongTienTruocGiam,
                    DiemSuDung,
                    GiamGiaDiem,
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
                    MaKH,
                    TongTienTruocGiam,
                    DiemSuDung,
                    GiamGiaDiem,
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
                    decimal tongTienTruocGiam = 0;
                    foreach (var item in request.ChiTietHoaDon)
                    {
                        tongTienTruocGiam += item.ThanhTien;
                    }

                    CustomerDTO customer = null;
                    decimal giamGiaDiem = 0;
                    int diemTichLuy = 0;

                    if (request.DiemSuDung < 0)
                    {
                        throw new InvalidOperationException("Số điểm sử dụng không hợp lệ.");
                    }

                    if (request.MaKH.HasValue && request.MaKH.Value > 0)
                    {
                        customer = scope.Connection.QueryFirstOrDefault<CustomerDTO>(
                            @"
                            SELECT
                                MaKH,
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
                            FROM Customers WITH (UPDLOCK, ROWLOCK)
                            WHERE MaKH = @MaKH
                              AND TrangThai = 1",
                            new { MaKH = request.MaKH.Value },
                            scope.Transaction);

                        if (customer == null)
                        {
                            throw new InvalidOperationException("Khách hàng không tồn tại hoặc đã ngừng hoạt động.");
                        }

                        if (request.DiemSuDung > customer.DiemHienCo)
                        {
                            throw new InvalidOperationException("Số điểm đổi vượt quá điểm hiện có của khách hàng.");
                        }

                        giamGiaDiem = request.DiemSuDung * LoyaltyConstants.RedeemValuePerPoint;
                        if (giamGiaDiem > tongTienTruocGiam)
                        {
                            throw new InvalidOperationException("Giá trị điểm đổi không được vượt quá tổng tiền đơn hàng.");
                        }
                    }
                    else if (request.DiemSuDung > 0)
                    {
                        throw new InvalidOperationException("Vui lòng chọn khách hàng trước khi đổi điểm.");
                    }

                    decimal tongTien = tongTienTruocGiam - giamGiaDiem;
                    diemTichLuy = CalculateEarnedPoints(tongTien);

                    string sqlInvoice = @"
                        INSERT INTO Invoices
                        (
                            NgayLap,
                            MaNV,
                            MaKH,
                            TongTienTruocGiam,
                            DiemSuDung,
                            GiamGiaDiem,
                            TongTien,
                            GhiChu,
                            TrangThai
                        )
                        VALUES
                        (
                            GETDATE(),
                            @MaNV,
                            @MaKH,
                            @TongTienTruocGiam,
                            @DiemSuDung,
                            @GiamGiaDiem,
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
                            request.MaKH,
                            TongTienTruocGiam = tongTienTruocGiam,
                            request.DiemSuDung,
                            GiamGiaDiem = giamGiaDiem,
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

                    if (customer != null)
                    {
                        if (request.DiemSuDung > 0)
                        {
                            int redeemRowsAffected = scope.Connection.Execute(
                                @"
                                UPDATE Customers
                                SET
                                    DiemHienCo = DiemHienCo - @DiemSuDung,
                                    TongDiemDaDoi = TongDiemDaDoi + @DiemSuDung,
                                    NgayCapNhat = GETDATE()
                                WHERE MaKH = @MaKH
                                  AND DiemHienCo >= @DiemSuDung",
                                new
                                {
                                    customer.MaKH,
                                    request.DiemSuDung
                                },
                                scope.Transaction);

                            if (redeemRowsAffected <= 0)
                            {
                                throw new InvalidOperationException("Điểm khách hàng vừa thay đổi. Vui lòng tải lại và thử lại.");
                            }

                            scope.Connection.Execute(
                                @"
                                INSERT INTO CustomerPointTransactions
                                (
                                    MaKH,
                                    MaHD,
                                    MaNV,
                                    LoaiGiaoDich,
                                    Diem,
                                    GiaTriGiam,
                                    GhiChu
                                )
                                VALUES
                                (
                                    @MaKH,
                                    @MaHD,
                                    @MaNV,
                                    @LoaiGiaoDich,
                                    @Diem,
                                    @GiaTriGiam,
                                    @GhiChu
                                )",
                                new
                                {
                                    customer.MaKH,
                                    MaHD = maHD,
                                    request.MaNV,
                                    LoaiGiaoDich = LoyaltyConstants.TransactionRedeem,
                                    Diem = -request.DiemSuDung,
                                    GiaTriGiam = giamGiaDiem,
                                    GhiChu = "Đổi điểm giảm giá hóa đơn"
                                },
                                scope.Transaction);
                        }

                        decimal newTotalSpend = customer.TongChiTieu + tongTien;
                        int newPurchaseCount = customer.SoLanMua + 1;
                        string newRank = ResolveMemberRank(newTotalSpend, newPurchaseCount);

                        scope.Connection.Execute(
                            @"
                            UPDATE Customers
                            SET
                                TongChiTieu = TongChiTieu + @TongTien,
                                SoLanMua = SoLanMua + 1,
                                DiemHienCo = DiemHienCo + @DiemTichLuy,
                                HangThanhVien = @HangThanhVien,
                                NgayCapNhat = GETDATE()
                            WHERE MaKH = @MaKH",
                            new
                            {
                                customer.MaKH,
                                TongTien = tongTien,
                                DiemTichLuy = diemTichLuy,
                                HangThanhVien = newRank
                            },
                            scope.Transaction);

                        if (diemTichLuy > 0)
                        {
                            scope.Connection.Execute(
                                @"
                                INSERT INTO CustomerPointTransactions
                                (
                                    MaKH,
                                    MaHD,
                                    MaNV,
                                    LoaiGiaoDich,
                                    Diem,
                                    GiaTriGiam,
                                    GhiChu
                                )
                                VALUES
                                (
                                    @MaKH,
                                    @MaHD,
                                    @MaNV,
                                    @LoaiGiaoDich,
                                    @Diem,
                                    @GiaTriGiam,
                                    @GhiChu
                                )",
                                new
                                {
                                    customer.MaKH,
                                    MaHD = maHD,
                                    request.MaNV,
                                    LoaiGiaoDich = LoyaltyConstants.TransactionEarn,
                                    Diem = diemTichLuy,
                                    GiaTriGiam = 0,
                                    GhiChu = "Tích điểm từ hóa đơn"
                                },
                                scope.Transaction);
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

        private int CalculateEarnedPoints(decimal amount)
        {
            if (amount <= 0)
            {
                return 0;
            }

            return (int)Math.Floor(amount / LoyaltyConstants.EarnAmountPerPoint);
        }

        private string ResolveMemberRank(decimal totalSpend, int purchaseCount)
        {
            if (totalSpend >= 15000000 || purchaseCount >= 15)
            {
                return LoyaltyConstants.RankPlatinum;
            }

            if (totalSpend >= 5000000 || purchaseCount >= 5)
            {
                return LoyaltyConstants.RankGold;
            }

            if (totalSpend >= 1000000 || purchaseCount >= 2)
            {
                return LoyaltyConstants.RankSilver;
            }

            return LoyaltyConstants.RankMember;
        }
    }
}
