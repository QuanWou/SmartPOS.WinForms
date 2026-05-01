using System;

namespace SmartPOS.WinForms.DTO.Entities
{
    public class CustomerPointTransactionDTO
    {
        public int MaGD { get; set; }

        public int MaKH { get; set; }

        public int? MaHD { get; set; }

        public int? MaNV { get; set; }

        public string LoaiGiaoDich { get; set; }

        public int Diem { get; set; }

        public decimal GiaTriGiam { get; set; }

        public string GhiChu { get; set; }

        public DateTime NgayTao { get; set; }
    }
}
