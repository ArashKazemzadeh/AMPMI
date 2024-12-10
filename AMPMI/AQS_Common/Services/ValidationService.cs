using System.Text.RegularExpressions;

namespace AQS_Common.Services
{
    public static class ValidationExtensions
    {
        /// <summary>
        /// اعتبارسنجی شماره موبایل
        /// </summary>
        /// <param name="mobile">شماره موبایل</param>
        /// <returns>True اگر معتبر باشد</returns>
        public static bool IsValidMobileNumber(this string mobile)
        {
            return Regex.IsMatch(mobile, "^(\\+98|0)?9\\d{9}$");
        }
        public static int DigitCount(this int number)
        {
            int absoluteNumber = Math.Abs(number);
            int digitCount = absoluteNumber.ToString().Length;
            return digitCount;
        }
    }
}
