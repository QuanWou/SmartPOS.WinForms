using System;

namespace SmartPOS.WinForms.DTO.Responses
{
    public class InvoicePrintItemDTO
    {
        public int MaHD { get; set; }

        public DateTime NgayLap { get; set; }

        public string TenNhanVien { get; set; }

        public string GhiChu { get; set; }

        public string TrangThai { get; set; }

        public decimal TongTien { get; set; }

        public int MaSP { get; set; }

        public string TenSP { get; set; }

        public int SoLuong { get; set; }

        public decimal DonGiaLucBan { get; set; }

        public decimal ThanhTien { get; set; }
    }
}