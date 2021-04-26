using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.EventBus;

namespace Dilon.Core
{
    public class LogSubscribeHandler: ISubscribeHandler
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
                var services = scope.ServiceProvider;

                var _rep = App.GetService<IRepository<SysLogOp>>(services);   // services 传递进去
                _rep.InsertNow(log);

            });


        }

        [SubscribeMessage("create:exlog")]
        public void CreateExLog(string eventId, object payload)
        {
            // 这里解析服务
            SysLogEx log = (SysLogEx)payload;
            Scoped.Create((_, scope) =>
            {
                var services = scope.ServiceProvider;
                var _rep = App.GetService<IRepository<SysLogEx>>(services);   // services 传递进去
                _rep.InsertNow(log);
            });
           
        }

        [SubscribeMessage("create:vislog")]
        public void CreateVisLog(string eventId, object payload)
        {
            // 这里解析服务
            SysLogVis log = (SysLogVis)payload;
            Scoped.Create((_, scope) =>
            {
                var services = scope.ServiceProvider;
                var _rep = App.GetService<IRepository<SysLogVis>>(services);   // services 传递进去
                _rep.InsertNow(log);
            });
        }
    }
}
