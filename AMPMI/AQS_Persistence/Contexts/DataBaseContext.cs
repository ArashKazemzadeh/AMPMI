using AQS_Aplication.Interfaces.Context;
using AQS_Domin.Entities;
using AQS_Persistence.ShadowPropertiesSetting;
using Microsoft.EntityFrameworkCore;
namespace AQS_Persistence.Contexts
{
    public class DataBaseContext(DbContextOptions options) : DbContext(options) ,IDataBaseContext
    {
        #region Tables
        public DbSet<Product> Products { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User> Users { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ShadowPropertiesHelper.ConfigureShadowProperties(modelBuilder);
        }

        #region SaveChanges
        public override int SaveChanges()
        {
            ShadowPropertiesHelper.ConfigureShadowPropertiesForEntities(ChangeTracker);
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ShadowPropertiesHelper.ConfigureShadowPropertiesForEntities(ChangeTracker);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ShadowPropertiesHelper.ConfigureShadowPropertiesForEntities(ChangeTracker);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ShadowPropertiesHelper.ConfigureShadowPropertiesForEntities(ChangeTracker);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        #endregion
    }
}
