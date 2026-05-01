using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.DAL.Interfaces
{
    public interface IChatBotRepository
    {
        ChatBotMetricResponse GetTodayRevenue();

        ChatBotMetricResponse GetCustomerStats();

        IEnumerable<ChatBotProductInsightResponse> GetLowStockProducts(int threshold, int take);

        IEnumerable<ChatBotProductInsightResponse> GetTopSellingProducts(int days, int take);

        IEnumerable<ChatBotInvoiceSummaryResponse> GetLatestInvoices(int take);

        IEnumerable<ChatBotCategoryComparisonResponse> GetRevenueComparisonByCategory(int days);

        IEnumerable<ChatBotProductInsightResponse> GetHighStockSlowMovingProducts(int stockThreshold, int soldThreshold, int days, int take);

        IEnumerable<ChatBotProductInsightResponse> GetRestockSuggestions(int stockThreshold, int days, int take);
    }
}
