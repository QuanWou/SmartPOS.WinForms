using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Entities;

namespace SmartPOS.WinForms.DAL.Interfaces
{
    public interface ICustomerOfferRepository
    {
        CustomerOfferDTO GetById(int maUuDai);

        IEnumerable<CustomerOfferDTO> GetByCustomerId(int maKH);

        IEnumerable<CustomerOfferDTO> GetAvailableByCustomerId(int maKH);

        int Insert(CustomerOfferDTO offer);

        int UpdateStatus(int maUuDai, bool trangThai);
    }
}
