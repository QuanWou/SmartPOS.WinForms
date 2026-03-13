using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.WinForms.DTO.Entities
{
    public class ProductDTO
    {
        public int MaSP { get; set; }

        public string TenSP { get; set; }

        public string MaVach { get; set; }

        public string DonViTinh { get; set; }

        public decimal GiaNhap { get; set; }

        public decimal GiaBan { get; set; }

        public int SoLuongTon { get; set; }

        public int MaLoai { get; set; }

        public string HinhAnh { get; set; }

        public string MoTa { get; set; }

        public bool TrangThai { get; set; }

        public DateTime NgayTao { get; set; }

        public DateTime? NgayCapNhat { get; set; }
    }
}
