using System.Collections.Generic;
using System.Linq;
using Dilon.Core.Service;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.TaskScheduler;
using Microsoft.Extensions.DependencyInjection;

namespace Dilon.Core.Job
{
    public class LogJobWorker : ISpareTimeWorker
    {
        // 日志暂存器，待写入
        private readonly List<SysLogEx> sysLogExs = new();
        private readonly List<SysLogOp> sysLogOps = new();
        private readonly List<SysLogVis> sysLogViss = new();
        
        private readonly IServiceScopeFactory _scopeFactory;

        public LogJobWorker()
        {
            _scopeFactory = App.GetService<IServiceScopeFactory>();
        }

        [SpareTime(10000, "LogExWritingService", Description = "后台批量写错误日志，配置项参数：{\"quantity\": 2}，不填默认为2", 
            DoOnce = false, StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
        public void DoLogEx(SpareTimer timer, long count)
        {
            DoWork(timer, count, "LogExWritingService_Parameters", sysLogExs);
        }

        [SpareTime(5000, "LogOpWritingService", Description = "后台批量写操作日志，配置项参数：{\"quantity\": 2}，不填默认为2", 
            DoOnce = false, StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
        public void DoLogOp(SpareTimer timer, long count)
        {
            DoWork(timer, count, "LogOpWritingService_Parameters", sysLogOps);
        }

        [SpareTime(8000, "LogVisWritingService", Description = "后台批量写访问日志，配置项参数：{\"quantity\": 2}，不填默认为2",
            DoOnce = false, StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
        public void DoLogVis(SpareTimer timer, long count)
        {
            DoWork(timer, count, "LogVisWritingService_Parameters", sysLogViss);
        }

        private void DoWork<T>(SpareTimer timer, long count, string cacheKey, List<T> logs) where T : EntityBase, new()
        {
            // 判断是否有异常
            if (timer.Exception.Any())
            {
                // todo: 增加任务运行日志
            }

            Scoped.Create((_, scope) =>
            {
                var services = scope.ServiceProvider;
                var sysLogExRepository = Db.GetRepository<T>(services);
                var sysCache = services.GetRequiredService<ISysCacheService>();

                // 默认值
                var quantity = 2;
                
                // 获取写数据库容量阀值，新增定时任务时会将配置项写入缓存
                if (sysCache != null)
                {
                    var parameters = sysCache.Get<Dictionary<string, string>>(cacheKey);

                    // 如果存在相关配置项
                    if (parameters != null && parameters.ContainsKey("quantity") &&
                        string.IsNullOrEmpty(parameters["quantity"]))
                        quantity = int.Parse(parameters["quantity"]);
                }

                // 后台队列中产生了日志，取出写入暂存器
                int queue = SimpleQueue<T>.Count();
                if (queue > 0)
                {
                    for (var i = 0; i < queue; i++)
                    {
                        if (SimpleQueue<T>.Try(out T obj))
                            logs.Add(obj);
                    }
                }

                // 达到系统配置的容量则写入数据库
                if (logs.Count > quantity)
                {
                    sysLogExRepository.InsertNowAsync(logs);
                    logs.Clear();
                }
            });
        }
    }
}