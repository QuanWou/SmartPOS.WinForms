using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.BLL.Interfaces
{
    public interface IReportService
    {
        List<InvoicePrintItemDTO> GetInvoicePrintData(int maHD);
    }
}