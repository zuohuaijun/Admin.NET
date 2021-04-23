using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dilon.Core.Service;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.TaskScheduler;
using Microsoft.Extensions.DependencyInjection;

namespace Dilon.Core.Job
{
    /// <summary>
    /// 日志定时任务类
    /// </summary>
    public class LogJobWorker : ISpareTimeWorker
    {
        // 日志暂存器，待写入
        private readonly List<SysLogEx> _sysLogExs = new();
        private readonly List<SysLogOp> _sysLogOps = new();
        private readonly List<SysLogVis> _sysLogViss = new();
        
        /// <summary>
        /// 定期删除异常日志
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="count"></param>
        [SpareTime("@midnight", "LogExDeletionService", Description = "后台定期删除异常日志，配置项参数：{\"daysAgo\": 30}，不填默认为30", 
            DoOnce = false, StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
        public void DoDeleteLogEx(SpareTimer timer, long count)
        {
            // 判断是否有异常
            if (timer.Exception.Any())
            {
                // todo: 增加任务运行日志
            }
            
            // 默认值
            var daysAgo = 15;
            
            var sysCache = App.GetRequiredService<ISysCacheService>();
                
            // 获取写数据库容量阀值，新增定时任务时会将配置项写入缓存
            if (sysCache != null)
            {
                var parameters = sysCache.Get<Dictionary<string, string>>("LogDeletionService_Parameters");

                // 如果存在相关配置项
                if (parameters != null && parameters.ContainsKey("daysAgo") &&
                    string.IsNullOrEmpty(parameters["daysAgo"]))
                    daysAgo = int.Parse(parameters["daysAgo"]);
            }

            // 生成查询表达式
            Expression<Func<SysLogEx, bool>> expression = ex => ex.ExceptionTime < DateTimeOffset.Now.AddDays(-daysAgo);
            
            // 执行删除
            DoDeleteWork(expression);
        }
        
        /// <summary>
        /// 定期删除操作日志
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="count"></param>
        [SpareTime("@midnight", "LogOpDeletionService", Description = "后台定期删除操作日志，配置项参数：{\"daysAgo\": 7}，不填默认为7", 
            DoOnce = false, StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
        public void DoDeleteLogOp(SpareTimer timer, long count)
        {
            // 判断是否有异常
            if (timer.Exception.Any())
            {
                // todo: 增加任务运行日志
            }
            
            // 默认值
            var daysAgo = 7;
            
            var sysCache = App.GetRequiredService<ISysCacheService>();
                
            // 获取写数据库容量阀值，新增定时任务时会将配置项写入缓存
            if (sysCache != null)
            {
                var parameters = sysCache.Get<Dictionary<string, string>>("LogOpDeletionService_Parameters");

                // 如果存在相关配置项
                if (parameters != null && parameters.ContainsKey("daysAgo") &&
                    string.IsNullOrEmpty(parameters["daysAgo"]))
                    daysAgo = int.Parse(parameters["daysAgo"]);
            }

            // 生成查询表达式
            Expression<Func<SysLogOp, bool>> expression = ex => ex.OpTime < DateTimeOffset.Now.AddDays(-daysAgo);
            
            // 执行删除
            DoDeleteWork(expression);
        }
        
        /// <summary>
        /// 定期删除访问日志
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="count"></param>
        [SpareTime("@midnight", "LogVisDeletionService", Description = "后台定期删除访问日志，配置项参数：{\"daysAgo\": 15}，不填默认为15", 
            DoOnce = false, StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
        public void DoDeleteLogVis(SpareTimer timer, long count)
        {
            // 判断是否有异常
            if (timer.Exception.Any())
            {
                // todo: 增加任务运行日志
            }
            
            // 默认值
            var daysAgo = 15;
            
            var sysCache = App.GetRequiredService<ISysCacheService>();
                
            // 获取写数据库容量阀值，新增定时任务时会将配置项写入缓存
            if (sysCache != null)
            {
                var parameters = sysCache.Get<Dictionary<string, string>>("LogVisDeletionService_Parameters");

                // 如果存在相关配置项
                if (parameters != null && parameters.ContainsKey("daysAgo") &&
                    string.IsNullOrEmpty(parameters["daysAgo"]))
                    daysAgo = int.Parse(parameters["daysAgo"]);
            }

            // 生成查询表达式
            Expression<Func<SysLogVis, bool>> expression = ex => ex.VisTime < DateTimeOffset.Now.AddDays(-daysAgo);
            
            // 执行删除
            DoDeleteWork(expression);
        }

        /// <summary>
        /// 后台批量写错误日志
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="count"></param>
        [SpareTime(10000, "LogExWritingService", Description = "后台批量写错误日志，配置项参数：{\"quantity\": 2}，不填默认为2", 
            DoOnce = false, StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
        public void DoLogEx(SpareTimer timer, long count)
        {
            DoWork(timer, count, "LogExWritingService_Parameters", _sysLogExs);
        }

        /// <summary>
        /// 后台批量写操作日志
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="count"></param>
        [SpareTime(5000, "LogOpWritingService", Description = "后台批量写操作日志，配置项参数：{\"quantity\": 2}，不填默认为2", 
            DoOnce = false, StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
        public void DoLogOp(SpareTimer timer, long count)
        {
            DoWork(timer, count, "LogOpWritingService_Parameters", _sysLogOps);
        }

        /// <summary>
        /// 后台批量写访问日志
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="count"></param>
        [SpareTime(8000, "LogVisWritingService", Description = "后台批量写访问日志，配置项参数：{\"quantity\": 2}，不填默认为2",
            DoOnce = false, StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
        public void DoLogVis(SpareTimer timer, long count)
        {
            DoWork(timer, count, "LogVisWritingService_Parameters", _sysLogViss);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="count"></param>
        /// <param name="cacheKey"></param>
        /// <param name="logs"></param>
        /// <typeparam name="T"></typeparam>
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
                var logRep = Db.GetRepository<T>(services);
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
                    logRep.InsertNowAsync(logs);
                    logs.Clear();
                }
            });
        }
        
        /// <summary>
        /// 根据条件删除日志
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="T"></typeparam>
        private void DoDeleteWork<T>(Expression<Func<T,bool>> expression) where T : EntityBase, new()
        {
            Scoped.Create((_, scope) =>
            {
                var services = scope.ServiceProvider;
                var logRep = Db.GetRepository<T>(services);

                var data = logRep.Where(expression);
                logRep.DeleteNow(data);
            });
        }
    }
}