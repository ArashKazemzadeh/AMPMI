namespace WebSite.EndPoint.ServicesConfigs
{
    public static class WebApplicationServices
    {
        public static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();
            builder.Services.AddMemoryCache();
        }
    }
}
