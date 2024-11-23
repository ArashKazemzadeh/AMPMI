using AQS_Aplication.Interfaces.Context;
using AQS_Domin.Entities;
using AQS_Persistence.ShadowPropertiesSetting;
using Microsoft.EntityFrameworkCore;
namespace AQS_Persistence.Contexts
{
    public class DataBaseContext : DbContext, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ShadowPropertiesHelper.Test();

        }

    }
}
