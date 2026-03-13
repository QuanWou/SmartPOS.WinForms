using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.WinForms.DTO.Entities
{
    public class CategoryDTO
    {
        public int MaLoai { get; set; }

        public string TenLoai { get; set; }

        public string MoTa { get; set; }

        public bool TrangThai { get; set; }
    }
}
