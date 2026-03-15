using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.WinForms.DTO.Entities
{
    public class StockInDetailDTO
    {
        public Guid LineId { get; set; }

        public int MaCTPN { get; set; }

        public int MaPN { get; set; }

        public int MaSP { get; set; }

        public int SoLuong { get; set; }

        public decimal GiaNhapLucNhap { get; set; }

        public decimal ThanhTien { get; set; }

        public DateTime? HanSuDung { get; set; }
    }
}
