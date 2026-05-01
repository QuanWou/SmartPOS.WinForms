using System;

namespace SmartPOS.WinForms.DTO.Responses
{
    public class ChatBotInvoiceSummaryResponse
    {
        public int MaHD { get; set; }

        public DateTime NgayLap { get; set; }

        public int MaNV { get; set; }

        public string TenNhanVien { get; set; }

        public string TenKhachHang { get; set; }

        public decimal TongTien { get; set; }

        public string TrangThai { get; set; }
    }
}
