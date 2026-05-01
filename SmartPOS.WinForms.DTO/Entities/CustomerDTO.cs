using System;

namespace SmartPOS.WinForms.DTO.Entities
{
    public class CustomerDTO
    {
        public int MaKH { get; set; }

        public string HoTen { get; set; }

        public string SoDienThoai { get; set; }

        public string DiaChi { get; set; }

        public DateTime NgayThamGia { get; set; }

        public string HangThanhVien { get; set; }

        public decimal TongChiTieu { get; set; }

        public int SoLanMua { get; set; }

        public int DiemHienCo { get; set; }

        public int TongDiemDaDoi { get; set; }

        public bool TrangThai { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        public DateTime? LanMuaGanNhat { get; set; }

        public string TopProduct { get; set; }
    }
}
