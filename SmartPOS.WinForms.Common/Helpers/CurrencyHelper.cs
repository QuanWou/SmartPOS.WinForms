using System.Globalization;
using SmartPOS.WinForms.Common.Constants;

namespace SmartPOS.WinForms.Common.Helpers
{
    public static class CurrencyHelper
    {
        public static string FormatVnCurrency(decimal amount)
        {
            CultureInfo culture = new CultureInfo(AppConstants.CurrencyCulture);
            return string.Format(culture, "{0:c0}", amount);
        }
    }
}