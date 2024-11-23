
using AQS_Domin.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace AQS_Persistence.ShadowPropertiesSetting
{
    public static class ShadowPropertiesHelper
    {
        public static void ConfigureShadowProperties(ModelBuilder modelBuilder)
        {
            //shadow properties : Product
            modelBuilder.Entity<Product>()
                .Property<DateTime>("CreatedAt");
            modelBuilder.Entity<Product>()
                .Property<DateTime>("UpdatedAt");
            modelBuilder.Entity<Product>()
                .Property<DateTime?>("DeletedAt");

            modelBuilder.Entity<Product>().HasQueryFilter(p => EF.Property<DateTime?>(p, "DeletedAt") == null);

            //shadow properties : Notification
            modelBuilder.Entity<Notification>()
                .Property<DateTime>("CreatedAt");
            modelBuilder.Entity<Notification>()
                .Property<DateTime>("UpdatedAt");
            modelBuilder.Entity<Notification>()
                .Property<DateTime?>("DeletedAt");

            modelBuilder.Entity<Notification>().HasQueryFilter(n => EF.Property<DateTime?>(n, "DeletedAt") == null);
        }

        public static void ConfigureShadowPropertiesForEntities(ChangeTracker changeTracker)
        {
            var now = DateTime.UtcNow;

            foreach (var entry in changeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = now;
                    entry.Property("UpdatedAt").CurrentValue = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("UpdatedAt").CurrentValue = now;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.Property("DeletedAt").CurrentValue = now;
                    entry.State = EntityState.Modified;
                }
            }
        }
    }

}
