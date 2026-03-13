namespace SmartPOS.WinForms.Common.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsNullOrWhiteSpace(string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        public static bool IsPositiveDecimal(decimal value)
        {
            return value > 0;
        }

        public static bool IsNonNegativeDecimal(decimal value)
        {
            return value >= 0;
        }

        public static bool IsPositiveInt(int value)
        {
            return value > 0;
        }

        public static bool IsNonNegativeInt(int value)
        {
            return value >= 0;
        }
    }
}