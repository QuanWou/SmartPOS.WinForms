using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.WinForms.DTO.Entities
{
    public class InvoiceDTO
    {
        public int MaHD { get; set; }

        public DateTime NgayLap { get; set; }

        public int MaNV { get; set; }

        public decimal TongTien { get; set; }

        public string GhiChu { get; set; }

        public string TrangThai { get; set; }
    }
}
