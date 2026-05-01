namespace SmartPOS.WinForms.DTO.Responses
{
    public class ChatBotCategoryComparisonResponse
    {
        public string TenLoai { get; set; }

        public decimal CurrentRevenue { get; set; }

        public decimal PreviousRevenue { get; set; }

        public decimal ChangeAmount { get; set; }

        public decimal ChangePercent { get; set; }
    }
}
