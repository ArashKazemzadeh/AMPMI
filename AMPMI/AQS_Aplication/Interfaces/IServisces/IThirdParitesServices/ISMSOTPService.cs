using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
    }
}
