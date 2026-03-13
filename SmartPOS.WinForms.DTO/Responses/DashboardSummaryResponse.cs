using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.WinForms.DTO.Responses
{
    public class DashboardSummaryResponse
    {
        public int TongSanPham { get; set; }

        public int HoaDonHomNay { get; set; }

        public decimal DoanhThuHomNay { get; set; }

        public int SanPhamSapHetHang { get; set; }
    }
}
