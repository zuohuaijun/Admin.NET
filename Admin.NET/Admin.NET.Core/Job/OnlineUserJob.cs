namespace Admin.NET.Core.Job;

/// <summary>
/// 任务调度
/// </summary>
public class OnlineUserJob : ISpareTimeWorker
{
    /// <summary>
    /// 清理在线用户定时器---服务启动时自动清空在线用户，防止存在僵尸用户(掉线用户会自动重连)
    /// </summary>
    [SpareTime(1000, "清空在线用户", Description = "服务启动时运行", DoOnce = true, StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
    public void ClearOnlineUser(SpareTimer timer, long count)
    {
        //Scoped.Create(async (_, scope) =>
        //{
        //    var services = scope.ServiceProvider;
        //    var rep = services.GetService<SqlSugarRepository<SysOnlineUser>>();
        //    await rep.AsDeleteable().ExecuteCommandAsync();

        //    Console.ForegroundColor = ConsoleColor.Blue;
        //    Console.WriteLine("【" + DateTime.Now + "——清空在线用户】\r\n服务重启触发清空在线用户");
        //});
    }
}