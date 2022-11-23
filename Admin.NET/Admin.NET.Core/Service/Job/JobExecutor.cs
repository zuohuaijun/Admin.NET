namespace Admin.NET.Core.Service;

/// <summary>
/// 作业执行器
/// </summary>
public class JobExecutor : IJobExecutor
{
    public async Task ExecuteAsync(JobExecutingContext context, IJob jobHandler, CancellationToken stoppingToken)
    {
        // 实现失败重试策略，如失败重试3次
        await Retry.InvokeAsync(async () =>
        {
            await jobHandler.ExecuteAsync(context, stoppingToken);
        }, 3, 1000);
    }
}