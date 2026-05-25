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

        public int? MaKH { get; set; }

        public decimal TongTienTruocGiam { get; set; }

        public int? MaUuDai { get; set; }

        public decimal PhanTramUuDai { get; set; }

        public decimal GiamGiaUuDai { get; set; }

        public int DiemSuDung { get; set; }

        public decimal GiamGiaDiem { get; set; }

        public decimal TongTien { get; set; }

        public string GhiChu { get; set; }

        public string TrangThai { get; set; }
    }
}
