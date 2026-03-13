using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.WinForms.DTO.Entities
{
    public class StockInDTO
    {
        public int MaPN { get; set; }

        public DateTime NgayNhap { get; set; }

        public int MaNV { get; set; }

        public decimal TongTien { get; set; }

        public string GhiChu { get; set; }
    }
}
