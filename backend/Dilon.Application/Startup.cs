using Furion;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilon.Application
{
    public class Startup : AppStartup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSqlSugar(new ConnectionConfig
            {
                ConnectionString = App.Configuration.GetSection("ConnectionStrings:DefaultConnection").Value ,
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute//从特性读取主键自增信息
            },
            db =>
            {
                db.MappingTables.Add(typeof(Test).Name, typeof(Test).Name);
                db.CodeFirst.SetStringDefaultLength(200).InitTables(typeof(Test));
                //处理日志事务
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    Console.WriteLine(sql);
                    Console.WriteLine(string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value)));
                    Console.WriteLine();
                };
            }); ; ;
        }
    }

   

}
