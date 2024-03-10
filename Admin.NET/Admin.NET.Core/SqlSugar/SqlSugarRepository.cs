// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// SqlSugar 实体仓储
/// </summary>
/// <typeparam name="T"></typeparam>
public class SqlSugarRepository<T> : SimpleClient<T> where T : class, new()
{
    protected static ITenant ITenant { get; set; }
    protected static SqlSugarScopeProvider MasterDb { get; set; }

    public SqlSugarRepository()
    {
        ITenant ??= App.GetRequiredService<ISqlSugarClient>().AsTenant();
        MasterDb ??= ITenant.GetConnectionScope(SqlSugarConst.MainConfigId);
        base.Context = MasterDb;

        // 若实体贴有多库特性，则返回指定库连接
        if (typeof(T).IsDefined(typeof(TenantAttribute), false))
        {
            base.Context = ITenant.GetConnectionScopeWithAttr<T>();
            return;
        }

        // 若实体贴有日志表特性，则返回日志库连接
        if (typeof(T).IsDefined(typeof(LogTableAttribute), false))
        {
            base.Context = ITenant.IsAnyConnection(SqlSugarConst.LogConfigId)
                ? ITenant.GetConnectionScope(SqlSugarConst.LogConfigId)
                : ITenant.GetConnectionScope(SqlSugarConst.MainConfigId);
            return;
        }

        // 若实体贴有系统表特性，则返回默认库连接
        if (typeof(T).IsDefined(typeof(SysTableAttribute), false))
        {
            base.Context = ITenant.GetConnectionScope(SqlSugarConst.MainConfigId);
            return;
        }

        // 若未贴任何表特性或当前未登录或是默认租户Id，则返回默认库连接
        var tenantId = App.User?.FindFirst(ClaimConst.TenantId)?.Value;
        if (string.IsNullOrWhiteSpace(tenantId) || tenantId == SqlSugarConst.MainConfigId) return;

        // 根据租户Id切换库连接, 为空则返回默认库连接
        var sqlSugarScopeProvider = App.GetRequiredService<SysTenantService>().GetTenantDbConnectionScope(long.Parse(tenantId));
        if (sqlSugarScopeProvider == null) return;
        base.Context = sqlSugarScopeProvider;
    }
}