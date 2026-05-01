namespace SmartPOS.WinForms.DTO.Responses
{
    public class CustomerStatsResponse
    {
        public int TotalCustomers { get; set; }

        public int MemberCustomers { get; set; }

        public int NewCustomersThisMonth { get; set; }

        public int RedeemedPoints { get; set; }

        public decimal TotalSpend { get; set; }
    }
}
