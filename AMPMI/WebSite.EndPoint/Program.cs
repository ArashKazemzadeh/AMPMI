using WebSite.EndPoint.ServicesConfigs;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var SqlServer = configuration["ConnectionString:SqlServer"];

DependencyInjectionConfig.AddScopeds(builder);
ContextConfig.IdentityDatabaseContext(builder, SqlServer);
ContextConfig.DbAmpmiContext(builder, SqlServer);
BuilderServiceConfig.AddServices(builder);

var app = builder.Build();


if (app.Environment.IsDevelopment())
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.Run();
