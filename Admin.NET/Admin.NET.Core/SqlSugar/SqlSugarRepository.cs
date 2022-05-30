using Furion;
using SqlSugar;
using System.Reflection;

namespace Admin.NET.Core
{
    /// <summary>
    /// SqlSugar仓储类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlSugarRepository<T> : SimpleClient<T> where T : class, new()
    {
        public SqlSugarRepository(ISqlSugarClient context = null) : base(context) // 默认值等于null不能少
        {
            base.Context = App.GetService<ISqlSugarClient>(); // 用手动获取方式支持切换仓储

            // 数据库上下文根据实体切换,业务分库(使用环境例如微服务)
            var entityType = typeof(T);

            //审计日志,获取系统配置需要切库
            if (entityType == typeof(SysLogAudit) || entityType == typeof(SysLogEx) || entityType == typeof(SysLogOp) || entityType == typeof(SysLogVis) || entityType == typeof(SysConfig))
            {
                Context = Context.AsTenant().GetConnectionScope(SqlSugarConst.ConfigId);
            }
            else
            {
                // 切换框架数据库
                if (entityType.IsDefined(typeof(SqlSugarEntityAttribute), false))
                {
                    var tenantAttribute = entityType.GetCustomAttribute<SqlSugarEntityAttribute>()!;
                    Context.AsTenant().ChangeDatabase(tenantAttribute.DbConfigId);
                }

                // 切换租户数据库
                if (entityType.IsDefined(typeof(TenantAttribute), false))
                {
                    var tenantAttribute = entityType.GetCustomAttribute<TenantAttribute>(false)!;
                    Context.AsTenant().ChangeDatabase(tenantAttribute.configId);
                }
            }
            
        }
    }
}