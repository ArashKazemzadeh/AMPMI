using AQS_Aplication.Interfaces.IInfrastructure.IContext;
using AQS_Domin.Entities;
using AQS_Persistence.Configs;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;
namespace AQS_Persistence.Contexts.SqlServer;
public partial class DbAmpmiContext : DbContext , IDbAmpmiContext
{
    public DbAmpmiContext(DbContextOptions<DbAmpmiContext> options)
        : base(options){}

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
    
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        EntitiesConfig.FluentAPI_Config_Entities(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


}
