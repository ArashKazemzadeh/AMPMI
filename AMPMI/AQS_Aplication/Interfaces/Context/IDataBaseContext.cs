using AQS_Domin.Entities;
using Microsoft.EntityFrameworkCore;


namespace AQS_Aplication.Interfaces.Context
{
    public interface IDataBaseContext
    {
        #region  DbSet<Entity>
        DbSet<User> Users { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Notification> Notifications { get; set; }
        #endregion

        #region SaveChanges()
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        #endregion
    }
}
