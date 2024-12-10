using AQS_Aplication.Interfaces.IInfrastructure.IAuthenticationOTPService;
using AQS_Aplication.Interfaces.IInfrastructure.IContext;
using AQS_Aplication.Interfaces.IServisces;
using AQS_Aplication.Interfaces.IServisces.IdentityServices;
using AQS_Aplication.Interfaces.IServisces.IThirdParitesServices;
using AQS_Aplication.Services;
using AQS_Persistence.Contexts.SqlServer;
using ThirdParties.SMSService;
using YourNamespace.Services;

namespace WebSite.EndPoint.ServicesConfigs
{
    public static class DependencyInjectionConfig
    {
        public static void AddScopeds(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();
            builder.Services.AddScoped<IAuthenticationOTP, AuthenticationOTP>();
            builder.Services.AddScoped<ICompanyService, CompanyService>();
            builder.Services.AddScoped<IDbAmpmiContext, DbAmpmiContext>();
            builder.Services.AddScoped<ISMSOTPService, SMSOTPService>();
        }
    }
}
