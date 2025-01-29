using AQS_Domin.Entities.Accounting;
using AQS_Persistence.Configs;
using AQS_Persistence.Contexts.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebSite.EndPoint.ServicesConfigs
{
    public static class ContextContainer
    {
        public static void DatabaseContext(WebApplicationBuilder builder, string? connection)
        {
            builder.Services.AddDbContext<DbAmpmiContext>(options => options.UseSqlServer(connection));

            builder.Services.AddIdentity<User, Role>()
                .AddRoles<Role>()
                .AddEntityFrameworkStores<DbAmpmiContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<CustomIdentityError>();


            builder.Services.Configure<IdentityOptions>(options =>
            {
               
                options.User.AllowedUserNameCharacters =
                "" +
                " " +
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "0123456789" +
                "چجحخهعغفقثصضشسیبلاتنمکگظطزرذدئوژپ";
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            });

            builder.Services.ConfigureApplicationCookie(option =>
            {
                option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                option.LoginPath = "/Login/Login";
                option.AccessDeniedPath = "/Login/AccessDenied";
                option.SlidingExpiration = true;
            });

        }
    }
}
