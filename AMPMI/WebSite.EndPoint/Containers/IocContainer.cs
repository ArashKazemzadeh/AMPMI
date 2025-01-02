using AQS_Application.Interfaces.IInfrastructure.IAuthenticationOTPService;
using AQS_Application.Interfaces.IInfrastructure.IContext;
using AQS_Application.Interfaces.IServices.BaseServices;
using AQS_Application.Interfaces.IServices.IdentityServices;
using AQS_Application.Interfaces.IServices.IThirdParitesServices;
using AQS_Application.Services;
using AQS_Application.Services.IdentityServices;
using AQS_Persistence.Contexts.SqlServer;
using ThirdParties.SMSService;
using WebSite.EndPoint.Utility;
using YourNamespace.Services;

namespace WebSite.EndPoint.ServicesConfigs;




public static class IocContainer
{
    public static void AddScopes(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IRegistrationService, RegistrationService>();
        builder.Services.AddScoped<ILoginService, LoginService>();
        builder.Services.AddScoped<IAuthenticationOTP, AuthenticationOTP>();
        builder.Services.AddScoped<ICompanyService, CompanyService>();
        builder.Services.AddScoped<IDbAmpmiContext, DbAmpmiContext>();
        builder.Services.AddScoped<ISMSOTPService, SMSOTPService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<ISeenNotifByCompanyService, SeenNotifByCompanyService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
        builder.Services.AddScoped<IFileServices, FileService>();
        builder.Services.AddScoped<ICompanyPictureService, CompanyPictureService>();
    }
}

