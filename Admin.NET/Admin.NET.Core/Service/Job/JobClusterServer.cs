// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core.Service;

/// <summary>
/// 作业集群控制
/// </summary>
public class JobClusterServer : IJobClusterServer
{
    private readonly Random rd = new(DateTime.Now.Millisecond);

    public JobClusterServer()
    {
    }

    /// <summary>
    /// 当前作业调度器启动通知
    /// </summary>
    /// <param name="context">作业集群服务上下文</param>
    public async void Start(JobClusterContext context)
    {
        var _sysJobClusterRep = App.GetService<SqlSugarRepository<SysJobCluster>>();
        // 在作业集群表中，如果 clusterId 不存在，则新增一条（否则更新一条），并设置 status 为 ClusterStatus.Waiting
        if (await _sysJobClusterRep.IsAnyAsync(u => u.ClusterId == context.ClusterId))
        {
            await _sysJobClusterRep.AsUpdateable().SetColumns(u => u.Status == ClusterStatus.Waiting).Where(u => u.ClusterId == context.ClusterId).ExecuteCommandAsync();
        }
        else
        {
            await _sysJobClusterRep.AsInsertable(new SysJobCluster { ClusterId = context.ClusterId, Status = ClusterStatus.Waiting }).ExecuteCommandAsync();
        }
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
            // 控制集群心跳频率（放在头部为了防止 IsAnyAsync continue 没sleep占用大量IO和CPU）
            await Task.Delay(3000 + rd.Next(500, 1000)); // 错开集群同时启动

            try
            {
                ICache _cache = App.GetService<ICache>();
                //使用分布式锁
                using (_cache.AcquireLock("lock:JobClusterServer:WaitingForAsync", 1000))
                {
                    var _sysJobClusterRep = App.GetService<SqlSugarRepository<SysJobCluster>>();
                    // 在这里查询数据库，根据以下两种情况处理
                    // 1) 如果作业集群表已有 status 为 ClusterStatus.Working 则继续循环
                    // 2) 如果作业集群表中还没有其他服务或只有自己，则插入一条集群服务或调用 await WorkNowAsync(clusterId); 之后 return;
                    // 3) 如果作业集群表中没有 status 为 ClusterStatus.Working 的，调用 await WorkNowAsync(clusterId); 之后 return;
                    if (await _sysJobClusterRep.IsAnyAsync(u => u.Status == ClusterStatus.Working))
                        continue;

                    await WorkNowAsync(clusterId);
                    return;
                }
            }
            catch { }
        }
    }

    /// <summary>
    /// 当前作业调度器停止通知
    /// </summary>
    /// <param name="context">作业集群服务上下文</param>
    public async void Stop(JobClusterContext context)
    {
        var _sysJobClusterRep = App.GetService<SqlSugarRepository<SysJobCluster>>();
        // 在作业集群表中，更新 clusterId 的 status 为 ClusterStatus.Crashed
        await _sysJobClusterRep.UpdateAsync(u => new SysJobCluster { Status = ClusterStatus.Crashed }, u => u.ClusterId == context.ClusterId);
    }

    /// <summary>
    /// 当前作业调度器宕机
    /// </summary>
    /// <param name="context">作业集群服务上下文</param>
    public async void Crash(JobClusterContext context)
    {
        var _sysJobClusterRep = App.GetService<SqlSugarRepository<SysJobCluster>>();
        // 在作业集群表中，更新 clusterId 的 status 为 ClusterStatus.Crashed
        await _sysJobClusterRep.UpdateAsync(u => new SysJobCluster { Status = ClusterStatus.Crashed }, u => u.ClusterId == context.ClusterId);
    }

    /// <summary>
    /// 指示集群可以工作
    /// </summary>
    /// <param name="clusterId">集群 Id</param>
    /// <returns></returns>
    private async Task WorkNowAsync(string clusterId)
    {
        var _sysJobClusterRep = App.GetService<SqlSugarRepository<SysJobCluster>>();
        // 在作业集群表中，更新 clusterId 的 status 为 ClusterStatus.Working
        await _sysJobClusterRep.UpdateAsync(u => new SysJobCluster { Status = ClusterStatus.Working }, u => u.ClusterId == clusterId);
    }
}