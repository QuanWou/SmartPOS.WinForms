using System;

namespace SmartPOS.WinForms.DTO.Responses
{
    public class ChatBotCustomerSegmentResponse
    {
        public string Segment { get; set; }

        public int CustomerCount { get; set; }

        public int ActiveCustomerCount { get; set; }

        public int InvoiceCount { get; set; }

        public decimal Revenue { get; set; }

        public decimal AverageOrderValue { get; set; }

        public decimal AveragePurchaseCount { get; set; }

        public decimal PointsAvailable { get; set; }

        public decimal PointsRedeemed { get; set; }

        public DateTime? LastPurchaseAt { get; set; }
    }
}
