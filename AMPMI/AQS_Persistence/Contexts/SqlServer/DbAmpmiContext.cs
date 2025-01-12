using AQS_Application.Interfaces.IInfrastructure.IContext;
using AQS_Domin.Entities;
using AQS_Domin.Entities.Accounting;
using AQS_Persistence.Configs;
using Domin.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;



namespace AQS_Persistence.Contexts.SqlServer;
public class DbAmpmiContext(DbContextOptions<DbAmpmiContext> options) : IdentityDbContext<User, Role, long>(options), IDbAmpmiContext
{
    public DatabaseFacade Database => base.Database;

    #region DbSets
    public virtual DbSet<Blog> Blogs { get; set; }
    public virtual DbSet<BlogPicture> BlogPictures { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Company> Companies { get; set; }
    public virtual DbSet<CompanyPicture> CompanyPictures { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<SeenNotifByCompany> SeenNotifByCompanies { get; set; }
    public virtual DbSet<SiteAdmin> SiteAdmins { get; set; }
    public virtual DbSet<SubCategory> SubCategories { get; set; }
    public virtual DbSet<Banner> Banners { get; set; }
    public virtual DbSet<MiscellaneousData> MiscellaneousDatas { get; set; }
    public virtual DbSet<ProductPicture> ProductPictures { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 
        EntitiesConfig.FluentAPI_Config_Entities(modelBuilder);
    }
}
