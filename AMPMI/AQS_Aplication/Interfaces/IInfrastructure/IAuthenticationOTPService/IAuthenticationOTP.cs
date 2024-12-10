using System.Net;

namespace AQS_Aplication.Interfaces.IInfrastructure.IAuthenticationOTPService
{
    public interface IAuthenticationOTP
    {
        Task<HttpStatusCode> SendOTPAsync(string mobile, string otp);
    }
}
