using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.DAL.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<CustomerDTO> GetAll();

        CustomerDTO GetById(int maKH);

        CustomerDTO GetByPhone(string soDienThoai);

        IEnumerable<CustomerDTO> Search(CustomerSearchRequest request);

        int Insert(CustomerDTO customer);

        int Update(CustomerDTO customer);

        int UpdateStatus(int maKH, bool trangThai);

        CustomerStatsResponse GetStats();

        IEnumerable<CustomerPurchaseHistoryResponse> GetPurchaseHistory(int maKH);

        IEnumerable<CustomerPointTransactionDTO> GetPointHistory(int maKH);

        IEnumerable<CustomerCategoryTrendResponse> GetCategoryTrends(int maKH);

        IEnumerable<CustomerTopProductResponse> GetTopProducts(int maKH);
    }
}
