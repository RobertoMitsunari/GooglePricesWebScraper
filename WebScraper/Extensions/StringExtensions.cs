
using System.Globalization;


namespace WebScraper.Coletor.Extensions
{
    public static class StringExtensions
    {
        public static NumberFormatInfo decimalNumber = new NumberFormatInfo();

        static StringExtensions()
        {
            decimalNumber.NumberDecimalSeparator = ",";
            decimalNumber.NumberGroupSeparator = ".";
        }

        public static decimal ToDecimalBRFormat(this string source)
        {
            return decimal.Parse(source.Replace("R$", ""), decimalNumber);
        }
    }
}
