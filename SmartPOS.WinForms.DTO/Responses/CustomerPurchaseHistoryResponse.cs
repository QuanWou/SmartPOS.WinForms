using System;

namespace SmartPOS.WinForms.DTO.Responses
{
    public class CustomerPurchaseHistoryResponse
    {
        public int MaHD { get; set; }

        public DateTime NgayLap { get; set; }

        public decimal TongTienTruocGiam { get; set; }

        public int DiemSuDung { get; set; }

        public decimal GiamGiaDiem { get; set; }

        public decimal TongTien { get; set; }

        public string TrangThai { get; set; }
    }
}
