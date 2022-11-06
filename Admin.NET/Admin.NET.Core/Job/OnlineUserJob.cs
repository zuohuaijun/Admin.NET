namespace Admin.NET.Core.Job;

/// <summary>
/// 在线用户任务调度
/// </summary>
public class OnlineUserJob : ISpareTimeWorker
{
    /// <summary>
    /// 服务重启清空在线用户（防止僵尸用户，掉线用户会自动重连）
    /// </summary>
    [SpareTime(1000, "服务重启清空在线用户", Description = "服务重启清空在线用户", DoOnce = true, StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
    public void ClearOnlineUser(SpareTimer timer, long count)
    {
        Scoped.CreateAsync(async (_, scope) =>
        {
            var services = scope.ServiceProvider;
            var rep = services.GetService<SqlSugarRepository<SysOnlineUser>>();
            await rep.AsDeleteable().ExecuteCommandAsync();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("【" + DateTime.Now + "】服务重启清空在线用户");

            // 缓存多租户
            await services.GetService<SysTenantService>().UpdateTenantCache();
        });
    }
}