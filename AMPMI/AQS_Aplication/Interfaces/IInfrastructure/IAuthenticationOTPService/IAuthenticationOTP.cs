using System.Net;

namespace AQS_Application.Interfaces.IInfrastructure.IAuthenticationOTPService
{
    public interface IAuthenticationOTP
    {
        Task<HttpStatusCode> SendOTPAsync(string mobile, string otp);
    }
}
