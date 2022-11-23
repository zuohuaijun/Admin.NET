namespace Admin.NET.Core.Service;

/// <summary>
/// 作业集群控制
/// </summary>
public class JobClusterServer : IJobClusterServer
{
    /// <summary>
    /// 当前作业调度器启动通知
    /// </summary>
    /// <param name="context">作业集群服务上下文</param>
    public void Start(JobClusterContext context)
    {
        // 在作业集群表中，如果 clusterId 不存在，则新增一条（否则更新一条），并设置 status 为 ClusterStatus.Waiting
    }

    /// <summary>
    /// 等待被唤醒
    /// </summary>
    /// <param name="context">作业集群服务上下文</param>
    /// <returns><see cref="Task"/></returns>
    public async Task WaitingForAsync(JobClusterContext context)
    {
        var clusterId = context.ClusterId;

        while (true)
        {
            try
            {
                // 在这里查询数据库，根据以下两种情况处理
                // 1) 如果作业集群表已有 status 为 ClusterStatus.Working 则继续循环
                // 2) 如果作业集群表中还没有其他服务或只有自己，则插入一条集群服务或调用 await WorkNowAsync(clusterId); 之后 return;
                // 3) 如果作业集群表中没有 status 为 ClusterStatus.Working 的，调用 await WorkNowAsync(clusterId); 之后 return;

                await WorkNowAsync(clusterId);
                return;
            }
            catch { }

            // 控制集群心跳频率
            await Task.Delay(3000);
        }
    }

    /// <summary>
    /// 当前作业调度器停止通知
    /// </summary>
    /// <param name="context">作业集群服务上下文</param>
    public void Stop(JobClusterContext context)
    {
        // 在作业集群表中，更新 clusterId 的 status 为 ClusterStatus.Crashed
    }

    /// <summary>
    /// 当前作业调度器宕机
    /// </summary>
    /// <param name="context">作业集群服务上下文</param>
    public void Crash(JobClusterContext context)
    {
        // 在作业集群表中，更新 clusterId 的 status 为 ClusterStatus.Crashed
    }

    /// <summary>
    /// 指示集群可以工作
    /// </summary>
    /// <param name="clusterId">集群 Id</param>
    /// <returns></returns>
    private Task WorkNowAsync(string clusterId)
    {
        // 在作业集群表中，更新 clusterId 的 status 为 ClusterStatus.Working

        return Task.CompletedTask;
    }
}