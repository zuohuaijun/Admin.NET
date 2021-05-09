using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.EventBus;

namespace Admin.NET.Core
{
    /// <summary>
    /// 日志订阅处理
    /// </summary>
    public class LogSubscribeHandler : ISubscribeHandler
    {
        public LogSubscribeHandler()
        {
        }

        [SubscribeMessage("create:oplog")]
        public void CreateOpLog(string eventId, object payload)
        {
            SysLogOp log = (SysLogOp)payload;
            Scoped.Create((_, scope) =>
            {
                var _rep = App.GetService<IRepository<SysLogOp>>(scope.ServiceProvider);
                _rep.InsertNow(log);
            });
        }

        [SubscribeMessage("create:exlog")]
        public void CreateExLog(string eventId, object payload)
        {
            SysLogEx log = (SysLogEx)payload;
            Scoped.Create((_, scope) =>
            {
                var _rep = App.GetService<IRepository<SysLogEx>>(scope.ServiceProvider);
                _rep.InsertNow(log);
            });
        }

        [SubscribeMessage("create:vislog")]
        public void CreateVisLog(string eventId, object payload)
        {
            SysLogVis log = (SysLogVis)payload;
            Scoped.Create((_, scope) =>
            {
                var _rep = App.GetService<IRepository<SysLogVis>>(scope.ServiceProvider);
                _rep.InsertNow(log);
            });
        }
    }
}