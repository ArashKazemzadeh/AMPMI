using AQS_Aplication.Interfaces.Context;
using AQS_Domin.Entities;
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
    }
}
