using AQS_Aplication.Interfaces.IInfrastructure.IAuthenticationOTPService;
using AQS_Aplication.Interfaces.IServisces.IThirdParitesServices;
using AQS_Aplication.Services;
using AQS_Persistence.Contexts.SqlServer;
using Microsoft.EntityFrameworkCore;
using ThirdParties.SMSService;

namespace WebSite.EndPoint
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            #region  Connection String

            string connection = Configuration["ConnectionString:SqlServer"];
            services.AddDbContext<DbAmpmiContext>(option => option.UseSqlServer(connection));
            services.AddScoped<IAuthenticationOTP, AuthenticationOTP>();
            services.AddScoped<ISMSOTPService, SMSOTPService>();
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
               
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
