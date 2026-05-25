using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.BLL.Interfaces
{
    public interface ICustomerOfferService
    {
        CustomerOfferDTO GetById(int maUuDai);

        IEnumerable<CustomerOfferDTO> GetByCustomerId(int maKH);

        IEnumerable<CustomerOfferDTO> GetAvailableByCustomerId(int maKH);

        OperationResult Insert(CustomerOfferDTO offer);

        OperationResult UpdateStatus(int maUuDai, bool trangThai);
    }
}
