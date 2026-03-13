using System.Collections.Generic;
using SmartPOS.WinForms.DAL.Data;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.DAL.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly DbHelper _dbHelper;

        public ReportRepository()
        {
            _dbHelper = new DbHelper();
        }

        public List<InvoicePrintItemDTO> GetInvoicePrintData(int maHD)
        {
            string sql = @"
                SELECT
                    i.MaHD,
                    i.NgayLap,
                    u.TenNV AS TenNhanVien,
                    i.GhiChu,
                    CASE
                        WHEN i.TrangThai = 'Paid' THEN N'Đã thanh toán'
                        WHEN i.TrangThai = 'Cancelled' THEN N'Đã hủy'
                        ELSE i.TrangThai
                    END AS TrangThai,
                    i.TongTien,
                    d.MaSP,
                    p.TenSP,
                    d.SoLuong,
                    d.DonGiaLucBan,
                    d.ThanhTien
                FROM Invoices i
                LEFT JOIN Users u ON i.MaNV = u.MaNV
                LEFT JOIN InvoiceDetails d ON i.MaHD = d.MaHD
                LEFT JOIN Products p ON d.MaSP = p.MaSP
                WHERE i.MaHD = @MaHD
                ORDER BY d.MaCTHD ASC";

            return new List<InvoicePrintItemDTO>(_dbHelper.Query<InvoicePrintItemDTO>(sql, new
            {
                MaHD = maHD
            }));
        }
    }
}