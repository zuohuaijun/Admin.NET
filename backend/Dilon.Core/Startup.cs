using Furion;
using Microsoft.Extensions.DependencyInjection;

namespace Dilon.Core
{
    [AppStartup(90)]
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConcurrentQueue<SysLogEx>, SimpleQueue<SysLogEx>>();
            services.AddSingleton<IConcurrentQueue<SysLogOp>, SimpleQueue<SysLogOp>>();
            services.AddSingleton<IConcurrentQueue<SysLogVis>, SimpleQueue<SysLogVis>>();
            services.AddHostedService<SimpleLogHostedService>();
        }
    }
}