using Furion;
using Furion.DatabaseAccessor;
using Microsoft.Extensions.DependencyInjection;

namespace Dilon.EntityFramework.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseAccessor(options =>
            {
                options.AddDb<DefaultDbContext>();
                options.AddDb<MultiTenantDbContext, MultiTenantDbContextLocator>();
            }, "Dilon.Database.Migrations");
        }
    }
}