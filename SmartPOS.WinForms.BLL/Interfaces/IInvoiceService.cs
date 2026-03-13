using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.BLL.Interfaces
{
    public interface IInvoiceService
    {
        IEnumerable<InvoiceDTO> GetAll();

        InvoiceDTO GetById(int maHD);

        IEnumerable<InvoiceDetailDTO> GetDetailsByInvoiceId(int maHD);

        OperationResult Checkout(CheckoutRequest request);

        OperationResult UpdateStatus(int maHD, string trangThai);
    }
}