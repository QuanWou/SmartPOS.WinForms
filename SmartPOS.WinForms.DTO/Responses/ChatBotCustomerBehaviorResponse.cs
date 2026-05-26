namespace SmartPOS.WinForms.DTO.Responses
{
    public class ChatBotCustomerBehaviorResponse
    {
        public string Segment { get; set; }

        public int CustomerCount { get; set; }

        public int InvoiceCount { get; set; }

        public decimal Revenue { get; set; }

        public decimal AverageOrderValue { get; set; }

        public decimal SharePercent { get; set; }
    }
}
