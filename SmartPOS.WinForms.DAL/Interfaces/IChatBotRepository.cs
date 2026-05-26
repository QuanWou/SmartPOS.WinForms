using System.Collections.Generic;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.DAL.Interfaces
{
    public interface IChatBotRepository
    {
        ChatBotMetricResponse GetTodayRevenue();

        ChatBotMetricResponse GetCustomerStats();

        ChatBotSalesOverviewResponse GetSalesOverview(int days);

        IEnumerable<ChatBotTimeSeriesResponse> GetRevenueTrendByDay(int days);

        IEnumerable<ChatBotTimeSeriesResponse> GetRevenueTrendByMonth(int months);

        IEnumerable<ChatBotProductInsightResponse> GetLowStockProducts(int threshold, int take);

        IEnumerable<ChatBotProductInsightResponse> GetTopSellingProducts(int days, int take);

        IEnumerable<ChatBotProductInsightResponse> GetDeepProductPerformance(int days, int take);

        IEnumerable<ChatBotProductInsightResponse> GetProductMarginRisks(int take);

        IEnumerable<ChatBotInvoiceSummaryResponse> GetLatestInvoices(int take);
        

        IEnumerable<ChatBotCategoryComparisonResponse> GetRevenueComparisonByCategory(int days);

        IEnumerable<ChatBotCustomerSegmentResponse> GetCustomerSegments();

        IEnumerable<ChatBotCustomerBehaviorResponse> GetCustomerRecencySegments();

        IEnumerable<ChatBotCustomerBehaviorResponse> GetCustomerValueSegments();

        IEnumerable<ChatBotProductInsightResponse> GetHighStockSlowMovingProducts(int stockThreshold, int soldThreshold, int days, int take);

        IEnumerable<ChatBotProductInsightResponse> GetRestockSuggestions(int stockThreshold, int days, int take);
    }
}
