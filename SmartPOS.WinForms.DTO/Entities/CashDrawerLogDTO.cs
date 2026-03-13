using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.WinForms.DTO.Entities
{
    public class CashDrawerLogDTO
    {
        public int MaLog { get; set; }

        public int? MaHD { get; set; }

        public int? MaNV { get; set; }

        public DateTime ThoiGianMo { get; set; }

        public string KetQua { get; set; }

        public string GhiChu { get; set; }
    }
}
