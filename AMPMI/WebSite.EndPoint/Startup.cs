﻿using AQS_Aplication.Interfaces.IInfrastructure.IAuthenticationOTPService;
using AQS_Aplication.Interfaces.IServisces.IdentityServices;
using AQS_Aplication.Interfaces.IServisces.IThirdParitesServices;
using AQS_Aplication.Services;
using AQS_Domin.Entities.Acounting;
using AQS_Persistence.Configs;
using AQS_Persistence.Contexts.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ThirdParties.SMSService;
using YourNamespace.Services;

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
            services.AddScoped<LoginService>(); //تنظیمان دقیق نیست
            services.AddScoped<IRegistrationService,RegistrationService>();


            services.AddControllersWithViews();
            services.AddAuthentication();
            services.AddAuthorization();

            string? connection = Configuration["ConnectionString:SqlServer"];
            if (string.IsNullOrEmpty(connection))
            {
                throw new InvalidOperationException("Connection string 'SqlServer' is not configured.");
            }

            services.AddDbContext<DbAmpmiContext>(option => option.UseSqlServer(connection));
            services.AddDbContext<IdentityDatabaseContext>(option => option.UseSqlServer(connection));

            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<IdentityDatabaseContext>()
                    .AddDefaultTokenProviders()
                    .AddRoles<Role>()
                    .AddErrorDescriber<CustomIdentityError>();



            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 2;
                options.Password.RequiredUniqueChars = 1;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            });

            services.ConfigureApplicationCookie(option =>
            {
                option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                option.LoginPath = "/account/login";
                option.AccessDeniedPath = "/Account/AccessDenied";
                option.SlidingExpiration = true;
            });

            services.AddScoped<IAuthenticationOTP, AuthenticationOTP>();
            services.AddScoped<ISMSOTPService, SMSOTPService>();







            services.AddMemoryCache();

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
