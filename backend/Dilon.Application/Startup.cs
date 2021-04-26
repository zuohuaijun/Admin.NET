using Furion;
using Microsoft.Extensions.DependencyInjection;

namespace Dilon.Application
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSqlSugar(new ConnectionConfig
            //{
            //    ConnectionString = App.Configuration["ConnectionStrings:DefaultConnection"],
            //    DbType = DbType.Sqlite,
            //    IsAutoCloseConnection = true,
            //    InitKeyType = InitKeyType.Attribute
            //},
            //db =>
            //{
            //    // db.DbMaintenance.CreateDatabase();
            //    // db.CodeFirst.SetStringDefaultLength(200).InitTables(typeof(Test));

            //    db.Aop.OnLogExecuting = (sql, pars) =>
            //    {
            //        App.PrintToMiniProfiler("SqlSugar", "Info", sql + "\r\n" + string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value)));
            //    };
            //});
        }
    }
}