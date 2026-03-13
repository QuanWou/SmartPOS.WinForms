using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.WinForms.DTO.Requests
{
    public class ProductSearchRequest
    {
        public string Keyword { get; set; }

        public int? MaLoai { get; set; }

        public bool? TrangThai { get; set; }
    }
}
