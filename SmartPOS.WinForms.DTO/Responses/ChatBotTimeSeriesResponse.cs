using System;

namespace SmartPOS.WinForms.DTO.Responses
{
    public class ChatBotTimeSeriesResponse
    {
        public string PeriodLabel { get; set; }

        public DateTime PeriodStart { get; set; }

        public int InvoiceCount { get; set; }

        public int UniqueCustomerCount { get; set; }

        public decimal Revenue { get; set; }

        public decimal Subtotal { get; set; }

        public decimal OfferDiscount { get; set; }

        public decimal PointDiscount { get; set; }

        public decimal AverageOrderValue { get; set; }
    }
}
