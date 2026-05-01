using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.BLL.Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<CustomerDTO> GetAll();

        CustomerDTO GetById(int maKH);

        CustomerDTO GetByPhone(string soDienThoai);

        IEnumerable<CustomerDTO> Search(CustomerSearchRequest request);

        OperationResult Insert(CustomerDTO customer);

        OperationResult Update(CustomerDTO customer);

        OperationResult UpdateStatus(int maKH, bool trangThai);

        CustomerStatsResponse GetStats();

        IEnumerable<CustomerPurchaseHistoryResponse> GetPurchaseHistory(int maKH);

        IEnumerable<CustomerPointTransactionDTO> GetPointHistory(int maKH);

        IEnumerable<CustomerCategoryTrendResponse> GetCategoryTrends(int maKH);

        IEnumerable<CustomerTopProductResponse> GetTopProducts(int maKH);

        int CalculateEarnedPoints(decimal amount);

        decimal CalculateRedeemValue(int points);

        string ResolveMemberRank(decimal totalSpend, int purchaseCount);

        OperationResult ValidatePointRedemption(int? maKH, int points, decimal orderTotal);
    }
}
