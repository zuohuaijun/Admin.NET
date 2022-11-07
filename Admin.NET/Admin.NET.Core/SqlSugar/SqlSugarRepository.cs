namespace Admin.NET.Core;

/// <summary>
/// SqlSugar仓储类
/// </summary>
/// <typeparam name="T"></typeparam>
public class SqlSugarRepository<T> : SimpleClient<T> where T : class, new()
{
    protected ITenant iTenant = null; // 多租户事务

    public SqlSugarRepository(ISqlSugarClient context = null) : base(context)
    {
        iTenant = App.GetRequiredService<ISqlSugarClient>().AsTenant();

        // 根据当前租户Id切换数据库
        var tenantId = App.GetRequiredService<UserManager>().TenantId;
        if (tenantId > 1)
        {
            var tenant = App.GetRequiredService<SysCacheService>().Get<List<SysTenant>>(CacheConst.KeyTenant).FirstOrDefault(u => u.Id == tenantId);
            if (!iTenant.IsAnyConnection(tenant.ConfigId))
            {
                iTenant.AddConnection(new ConnectionConfig()
                {
                    ConfigId = tenant.ConfigId,
                    ConnectionString = tenant.Connection,
                    DbType = tenant.DbType,
                    IsAutoCloseConnection = true
                });
            }
            base.Context = iTenant.GetConnectionScope(tenant.ConfigId);
        }
        else
        {
            base.Context = iTenant.GetConnectionScopeWithAttr<T>();
        }
    }
}