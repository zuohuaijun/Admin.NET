using Furion;
using Furion.DatabaseAccessor;
using Microsoft.Extensions.DependencyInjection;

namespace Dilon.EntityFramework.Core
{
    [AppStartup(95)]
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseAccessor(options =>
            {
                options.CustomizeMultiTenants(); // 自定义租户

                options.AddDb<DefaultDbContext>();
                options.AddDb<MultiTenantDbContext, MultiTenantDbContextLocator>();
            }, "Dilon.Database.Migrations");
        }
    }
}