using System;

namespace SmartPOS.WinForms.DTO.Responses
{
    public class ChatBotSalesOverviewResponse
    {
        public int InvoiceCount { get; set; }

        public int PaidInvoiceCount { get; set; }

        public int CancelledInvoiceCount { get; set; }

        public int CustomerInvoiceCount { get; set; }

        public int WalkInInvoiceCount { get; set; }

        public int UniqueCustomerCount { get; set; }

        public decimal Revenue { get; set; }

        public decimal Subtotal { get; set; }

        public decimal OfferDiscount { get; set; }

        public decimal PointDiscount { get; set; }

        public decimal AverageOrderValue { get; set; }

        public DateTime? FirstInvoiceAt { get; set; }

        public DateTime? LastInvoiceAt { get; set; }
    }
}
