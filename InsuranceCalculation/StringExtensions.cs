using System;
using System.Globalization;

namespace InsuranceCalculation
{
    public static class StringExtensions
    {
        private static readonly CultureInfo Provider = CultureInfo.CurrentCulture;
        private const string Format = "dd/MM/yyyy";
        private const DateTimeStyles Style = DateTimeStyles.AssumeLocal;

        /// <summary>
        /// Try parsing a date string
        /// </summary>
        /// <param name="dateInput"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseDate(this string dateInput, out DateTime result)
        {
            return DateTime.TryParseExact(dateInput, Format, Provider, Style, out result);
        }
    }
}