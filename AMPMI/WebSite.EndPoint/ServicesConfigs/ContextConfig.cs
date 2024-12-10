using AQS_Domin.Entities.Acounting;
using AQS_Persistence.Configs;
using AQS_Persistence.Contexts.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebSite.EndPoint.ServicesConfigs
{
    public static class ContextConfig
    {
       public static void IdentityDatabaseContext(WebApplicationBuilder builder, string? connection)
        {
            builder.Services.AddDbContext<IdentityDatabaseContext>(options => options.UseSqlServer(connection));

            builder.Services.AddIdentityCore<User>()
                .AddRoles<Role>()
                .AddEntityFrameworkStores<IdentityDatabaseContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<CustomIdentityError>();

            builder.Services.Configure<IdentityOptions>(options =>
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

            builder.Services.ConfigureApplicationCookie(option =>
            {
                option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                option.LoginPath = "/account/login";
                option.AccessDeniedPath = "/Account/AccessDenied";
                option.SlidingExpiration = true;
            });
        }
        public static void DbAmpmiContext(WebApplicationBuilder builder , string? connection)
        {
            builder.Services.AddDbContext<DbAmpmiContext>(options => options.UseSqlServer(connection));

        }
    }
}
