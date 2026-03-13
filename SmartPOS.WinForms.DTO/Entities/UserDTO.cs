using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.WinForms.DTO.Entities
{
    public class UserDTO
    {
        public int MaNV { get; set; }

        public string TenNV { get; set; }

        public string TaiKhoan { get; set; }

        public string MatKhauHash { get; set; }

        public string Quyen { get; set; }

        public string SoDienThoai { get; set; }

        public string DiaChi { get; set; }

        public bool TrangThai { get; set; }

        public DateTime NgayTao { get; set; }
    }
}
