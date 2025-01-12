using AQS_Domin.Entities;
using AQS_Domin.Entities.Accounting;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;


namespace AQS_Application.Interfaces.IInfrastructure.IContext
{
    public interface IDbAmpmiContext
    {
        public DatabaseFacade Database { get; }

        #region  DbSet<Entity>
        DbSet<Blog> Blogs { get; set; }
        DbSet<BlogPicture> BlogPictures { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Company> Companies { get; set; }
        DbSet<CompanyPicture> CompanyPictures { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<SeenNotifByCompany> SeenNotifByCompanies { get; set; }
        DbSet<SiteAdmin> SiteAdmins { get; set; }
        DbSet<SubCategory> SubCategories { get; set; }
        DbSet<Banner> Banners { get; set; }
        DbSet<MiscellaneousData> MiscellaneousDatas { get; set; }
        DbSet<ProductPicture> ProductPictures { get; set; }
        #endregion

        #region SaveChanges()
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        #endregion
    }
}
