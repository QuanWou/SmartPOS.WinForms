using System;

namespace SmartPOS.WinForms.DTO.Entities
{
    public class CustomerOfferDTO
    {
        public int MaUuDai { get; set; }

        public int MaKH { get; set; }

        public string TenUuDai { get; set; }

        public decimal PhanTramGiam { get; set; }

        public DateTime? NgayHetHan { get; set; }

        public bool TrangThai { get; set; }

        public bool DaSuDung { get; set; }

        public int? MaHDDaDung { get; set; }

        public DateTime? NgaySuDung { get; set; }

        public string GhiChu { get; set; }

        public DateTime NgayTao { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        public string TenKhachHang { get; set; }
    }
}
