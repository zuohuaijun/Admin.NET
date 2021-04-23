using Furion;
using Dilon.Core.Service;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 任务自启动
    /// </summary>
    public static class JobStarterServiceCollectionExtensions
    {
        public static IServiceCollection AddJobStarter(this IServiceCollection services)
        {
            var sysTimerService = App.GetService<ISysTimerService>();
            sysTimerService.StartTimerJobs();
            return services;
        }
    }
}