namespace SmartPOS.WinForms.Common.Helpers
{
    public static class BarcodeHelper
    {
        public static bool IsValidBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                return false;
            }

            barcode = barcode.Trim();

            if (barcode.Length < 4 || barcode.Length > 50)
            {
                return false;
            }

            return true;
        }

        public static string Normalize(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                return string.Empty;
            }

            return barcode.Trim();
        }
    }
}