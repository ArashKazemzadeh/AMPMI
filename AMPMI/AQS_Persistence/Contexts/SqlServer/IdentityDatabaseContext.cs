using AQS_Domin.Entities.Acounting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AQS_Persistence.Contexts.SqlServer
{
    public class IdentityDatabaseContext : IdentityDbContext<User,Role, long>
    {
        public IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options) 
            : base(options) {}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUser<long>>().ToTable("Users", "idn");
            builder.Entity<IdentityRole<long>>().ToTable("Roles", "idn");
            builder.Entity<IdentityRoleClaim<long>>().ToTable("RoleClaims", "idn");
            builder.Entity<IdentityUserClaim<long>>().ToTable("UserClaims", "idn");
            builder.Entity<IdentityUserLogin<long>>().ToTable("UserLogins", "idn");
            builder.Entity<IdentityUserRole<long>>().ToTable("UserRoles", "idn");
            builder.Entity<IdentityUserToken<long>>().ToTable("UserTokens", "idn");

            builder.Entity<IdentityUserLogin<long>>()
                .HasKey(p => new { p.LoginProvider, p.ProviderKey });
            builder.Entity<IdentityUserRole<long>>()
                .HasKey(p => new { p.UserId, p.RoleId });
            builder.Entity<IdentityUserToken<long>>()
                .HasKey(p => new { p.UserId, p.LoginProvider, p.Name });
        }
    }
}
