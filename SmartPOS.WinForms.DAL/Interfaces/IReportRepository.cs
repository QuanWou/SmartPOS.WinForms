using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.DAL.Interfaces
{
    public interface IReportRepository
    {
        List<InvoicePrintItemDTO> GetInvoicePrintData(int maHD);
    }
}