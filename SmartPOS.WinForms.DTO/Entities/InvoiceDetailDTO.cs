using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.WinForms.DTO.Entities
{
    public class InvoiceDetailDTO
    {
        public int MaCTHD { get; set; }

        public int MaHD { get; set; }

        public int MaSP { get; set; }

        public int SoLuong { get; set; }

        public decimal DonGiaLucBan { get; set; }

        public decimal ThanhTien { get; set; }
    }
}

