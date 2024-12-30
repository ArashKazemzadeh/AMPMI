using AQS_Application.Interfaces.IInfrastructure.IAuthenticationOTPService;
using AQS_Application.Interfaces.IServices.IThirdParitesServices;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace AQS_Application.Services
{
    public class SMSOTPService : ISMSOTPService
    {
        private readonly IAuthenticationOTP _authenticationOTP;
        public SMSOTPService(IAuthenticationOTP authenticationOTP)
        {
            _authenticationOTP = authenticationOTP;
        }

        public Task<int> GenerateUniqueOTPAsync()
        {
            int otp = 0;
            object locker = new();
            lock (locker)
            {
                Random random = new Random();
                 otp = random.Next(100000, 999999);
            }
            return Task.FromResult(otp);
        }
        public async Task<HttpStatusCode> SendSMSForAuthentication(string mobile, string message)
        {
            return await _authenticationOTP.SendOTPAsync(mobile, message);
        }
    }
}
