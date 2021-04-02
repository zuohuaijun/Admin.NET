using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;

namespace Dilon.EntityFramework.Core
{
    [AppDbContext("MultiTenantConnection", DbProvider.Sqlite)]
    public class MultiTenantDbContext : AppDbContext<MultiTenantDbContext, MultiTenantDbContextLocator>
    {
        public MultiTenantDbContext(DbContextOptions<MultiTenantDbContext> options) : base(options)
        {

        }
    }
}