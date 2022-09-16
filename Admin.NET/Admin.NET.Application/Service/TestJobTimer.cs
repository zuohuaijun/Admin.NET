using Furion.Logging;
using Furion.TaskScheduler;

namespace Admin.NET.Application.Service;

/// <summary>
/// 任务调度测试
/// </summary>
public class TestJobTimer : ISpareTimeWorker
{
    /// <summary>
    /// 定时器
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="count"></param>
    [SpareTime(5000, "定时器", StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
    public void CollectDeviceChannel(SpareTimer timer, long count)
    {
        ////// 写日志文件
        ////StringLoggingPart.Default.SetMessage("这是一个日志").LogInformation();

        //Scoped.Create((_, scope) =>
        //{
        //    //var services = scope.ServiceProvider;
        //    //var db = services.GetService<ISqlSugarClient>();

        //    // 写日志文件
        //    Log.Information("【定时器】" + DateTime.Now + "执行次数：" + count);
        //});
    }
}