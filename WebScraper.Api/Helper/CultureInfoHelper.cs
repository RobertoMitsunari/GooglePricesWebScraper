using System.Globalization;

namespace WebScraper.Api.Helper
{
    public static class CultureInfoHelper
    {
        public static NumberFormatInfo decimalNumber = new NumberFormatInfo();

        static CultureInfoHelper()
        {
            decimalNumber.NumberDecimalSeparator = ",";
            decimalNumber.NumberGroupSeparator = ".";
        }
    }
}
