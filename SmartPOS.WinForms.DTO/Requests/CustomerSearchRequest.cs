namespace SmartPOS.WinForms.DTO.Requests
{
    public class CustomerSearchRequest
    {
        public string Keyword { get; set; }

        public string HangThanhVien { get; set; }

        public bool? TrangThai { get; set; }
    }
}
