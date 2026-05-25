namespace SmartPOS.WinForms.DTO.Responses
{
    public class CheckoutPreviewResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public decimal TongTienTruocGiam { get; set; }

        public int? MaUuDai { get; set; }

        public decimal PhanTramUuDai { get; set; }

        public decimal GiamGiaUuDai { get; set; }

        public int DiemSuDung { get; set; }

        public decimal GiamGiaDiem { get; set; }

        public int DiemToiDaCoTheDoi { get; set; }

        public decimal TongTien { get; set; }
    }
}
