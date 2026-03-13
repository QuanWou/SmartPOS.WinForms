using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;

namespace SmartPOS.WinForms.DAL.Interfaces
{
    public interface IInvoiceRepository
    {
        IEnumerable<InvoiceDTO> GetAll();

        InvoiceDTO GetById(int maHD);

        IEnumerable<InvoiceDetailDTO> GetDetailsByInvoiceId(int maHD);

        int Insert(CheckoutRequest request);

        int UpdateStatus(int maHD, string trangThai);
    }
}