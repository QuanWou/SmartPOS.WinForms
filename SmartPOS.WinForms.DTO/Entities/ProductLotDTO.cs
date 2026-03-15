using System;

namespace SmartPOS.WinForms.DTO.Entities
{
    public class ProductLotDTO
    {
        public int MaLo { get; set; }

        public int? MaPN { get; set; }

        public int? MaCTPN { get; set; }

        public int MaSP { get; set; }

        public DateTime NgayNhap { get; set; }

        public DateTime? HanSuDung { get; set; }

        public int SoLuongNhap { get; set; }

        public int SoLuongTonLo { get; set; }

        public decimal GiaNhapLucNhap { get; set; }

        public string GhiChu { get; set; }

        public string TenSP { get; set; }

        public string MaVach { get; set; }

        public bool TrangThaiSanPham { get; set; }
    }
}
