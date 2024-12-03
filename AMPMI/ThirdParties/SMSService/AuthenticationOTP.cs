using AQS_Aplication.Interfaces.IInfrastructure.IAuthenticationOTPService;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ThirdParties.SMSService
{
    public partial class AuthenticationOTP : IAuthenticationOTP
    {
        const string Token = "r7gElQcTws4HGeF2ZD0GxKoigS72HqnMf6gtpUAQQdxvdlaQOHYPuB9RsuQHJSSS";
        const int TemplatedId = 454583;
        const string Replace = "CODE";
        const string ApiRoute = "https://api.sms.ir/v1/send/verify";
        const string TokenKeyName = "x-api-key";
        public async Task<HttpStatusCode> SendOTPAsync(string mobile,string otp)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add(TokenKeyName, Token);
                VerifySendModel model = new VerifySendModel()
                {
                    Mobile = mobile,
                    TemplateId = TemplatedId,
                    Parameters = new VerifySendParameterModel[]
                    {
                    new VerifySendParameterModel {
                        Name = Replace,
                        Value = otp.ToString()
                    }
                    }
                };

                string payload = JsonSerializer.Serialize(model);
                StringContent stringContent = new(payload, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(ApiRoute, stringContent);
                return response.StatusCode;
            }
            catch (WebException e)
            {
                // TODO : Log
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
