using Furion.EventBus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Admin.NET.Core
{
    /// <summary>
    /// 日志事件订阅
    /// </summary>
    public class LogEventSubscriber : IEventSubscriber
    {
        public IServiceProvider Services { get; }

        public LogEventSubscriber(IServiceProvider services)
        {
            Services = services;
        }

        /// <summary>
        /// 增加操作日志
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("Add:OpLog")]
        public async Task CreateOpLog(EventHandlerExecutingContext context)
        {
            using var scope = Services.CreateScope();
            var _rep = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysLogOp>>();
            await _rep.InsertAsync((SysLogOp)context.Source.Payload);
        }

        /// <summary>
        /// 增加异常日志
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("Add:ExLog")]
        public async Task CreateExLog(EventHandlerExecutingContext context)
        {
            using var scope = Services.CreateScope();
            var _rep = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysLogEx>>();
            await _rep.InsertAsync((SysLogEx)context.Source.Payload);
        }

        /// <summary>
        /// 增加访问日志
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("Add:VisLog")]
        public async Task CreateVisLog(EventHandlerExecutingContext context)
        {
            using var scope = Services.CreateScope();
            var _rep = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysLogVis>>();
            await _rep.InsertAsync((SysLogVis)context.Source.Payload);
        }
    }
}