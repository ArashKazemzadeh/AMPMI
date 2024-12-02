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
    }
}
