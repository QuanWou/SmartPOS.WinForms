using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartPOS.WinForms.DTO.Entities;

namespace SmartPOS.WinForms.DTO.Requests
{
    public class CheckoutRequest
    {
        public int MaNV { get; set; }

        public string GhiChu { get; set; }

        public List<InvoiceDetailDTO> ChiTietHoaDon { get; set; }
    }
}
