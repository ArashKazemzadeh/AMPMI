using AQS_Aplication.Interfaces.IInfrastructure.IAuthenticationOTPService;
using AQS_Aplication.Interfaces.IServisces.IThirdParitesServices;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace AQS_Aplication.Services
{
    public class SMSOTPService : ISMSOTPService
    {
        private readonly IAuthenticationOTP _authenticationOTP;
        public SMSOTPService(IAuthenticationOTP authenticationOTP)
        {
            _authenticationOTP = authenticationOTP;
        }

        public async Task<int> GenerateUniqueOTPAsync()
        {
            return await Task.Run(() =>
            {
                string input = DateTime.UtcNow.Ticks.ToString() + Guid.NewGuid().ToString();
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                    string hashString = BitConverter.ToString(hashBytes).Replace("-", "");

                    string otpString = new string(hashString.Where(char.IsDigit).ToArray()).Substring(0, 6);
                    return int.Parse(otpString);
                }
            });
        }
        /// <summary>
        /// تولید یا بازگرداندن OTP
        /// </summary>
        /// <param name="generateNew">آیا OTP جدید تولید شود</param>
        public async Task<int> GenerateOrGetOTP(bool generateNew ,int otpCode)
        { if (!generateNew && otpCode > 0)
                return otpCode;

            otpCode = await this.GenerateUniqueOTPAsync();
            return otpCode;
        }
        public Task<HttpStatusCode> SendSMSForAuthentication(string mobile, string message)
        {
            return  _authenticationOTP.SendOTPAsync(mobile, message);
        }
    }
}
