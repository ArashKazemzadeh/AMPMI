using AQS_Aplication.Interfaces.IInfrastructure.IAuthenticationOTPService;
using AQS_Aplication.Interfaces.IServisces.IThirdParitesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AQS_Aplication.Services
{
    public class SMSOTPService : ISMSOTPService
    {
        private readonly IAuthenticationOTP _authenticationOTP;
        public SMSOTPService(IAuthenticationOTP authenticationOTP)
        {
            _authenticationOTP = authenticationOTP;
        }
        public Task<HttpStatusCode> SendSMSForAuthentication(string mobile, string message)
        {
            return  _authenticationOTP.SendOTPAsync(mobile, message);
        }
    }
}
