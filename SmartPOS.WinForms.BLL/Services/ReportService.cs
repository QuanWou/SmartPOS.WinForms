using System.Collections.Generic;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.DAL.Interfaces;
using SmartPOS.WinForms.DAL.Repositories;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService()
        {
            _reportRepository = new ReportRepository();
        }

        public List<InvoicePrintItemDTO> GetInvoicePrintData(int maHD)
        {
            if (maHD <= 0)
            {
                return new List<InvoicePrintItemDTO>();
            }

            return _reportRepository.GetInvoicePrintData(maHD);
        }
    }
}