using System.Net;

namespace AQS_Aplication.Interfaces.IServisces.IThirdParitesServices
{
    public interface ISMSOTPService
    {
        /// <summary>
        /// ارسال otp به کاربر برای احراز هویت
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<HttpStatusCode> SendSMSForAuthentication(string mobile, string message);
        /// <summary>
        /// تولید عدد یکتا برای تایید پیامک
        /// </summary>
        /// <returns></returns>
        Task<int> GenerateUniqueOTPAsync();
        /// <summary>
        /// تولید یا بازگرداندن OTP
        /// </summary>
        /// <param name="generateNew">آیا OTP جدید تولید شود</param>
        Task<int> GenerateOrGetOTP(bool generateNew, int otpCode);
    }
}
