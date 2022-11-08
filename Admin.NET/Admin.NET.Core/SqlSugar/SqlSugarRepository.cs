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
            base.Context = SqlSugarSetup.InitTenantDb(iTenant, tenantId);
        }
        else
        {
            base.Context = iTenant.GetConnectionScopeWithAttr<T>();
        }
    }
}