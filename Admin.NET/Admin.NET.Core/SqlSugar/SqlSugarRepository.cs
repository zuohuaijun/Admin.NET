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

        // 根据租户业务实体是否切库
        if (!typeof(T).IsDefined(typeof(SystemTableAttribute), false))
        {
            var tenantId = App.GetRequiredService<UserManager>().TenantId; // 根据租户Id切库

            if (!iTenant.IsAnyConnection(tenantId.ToString()))
            {
                var tenant = App.GetRequiredService<SysCacheService>().Get<List<SysTenant>>(CacheConst.KeyTenant)
                    .FirstOrDefault(u => u.Id == tenantId);
                iTenant.AddConnection(new ConnectionConfig()
                {
                    ConfigId = tenant.Id,
                    DbType = tenant.DbType,
                    ConnectionString = tenant.Connection,
                    IsAutoCloseConnection = true
                });
                SqlSugarSetup.SetDbAop(iTenant.GetConnectionScope(tenantId.ToString()));
            }
            base.Context = iTenant.GetConnectionScope(tenantId.ToString());
        }
        else
        {
            base.Context = iTenant.GetConnectionScopeWithAttr<T>();
        }
    }
}