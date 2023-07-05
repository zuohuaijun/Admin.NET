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

        // 若实体贴有多库特性，则返回指定的连接
        if (typeof(T).IsDefined(typeof(TenantAttribute), false))
        {
            base.Context = iTenant.GetConnectionScopeWithAttr<T>();
            return;
        }

        // 若实体贴有系统表特性，则返回默认的连接
        if (typeof(T).IsDefined(typeof(SystemTableAttribute), false))
        {
            base.Context = iTenant.GetConnectionScope(SqlSugarConst.ConfigId);
            return;
        }

        // 若当前未登录或是默认租户Id，则返回默认的连接
        var tenantId = App.GetRequiredService<UserManager>().TenantId;
        if (tenantId < 1 || tenantId.ToString() == SqlSugarConst.ConfigId) return;

        // 根据租户Id切库
        if (!iTenant.IsAnyConnection(tenantId.ToString()))
        {
            var tenant = App.GetRequiredService<SysCacheService>().Get<List<SysTenant>>(CacheConst.KeyTenant)
                .FirstOrDefault(u => u.Id == tenantId);

            // 获取主库连接配置
            var dbOptions = App.GetOptions<DbConnectionOptions>();
            var mainConnConfig = dbOptions.ConnectionConfigs.First(u => u.ConfigId == SqlSugarConst.ConfigId);

            // 如果是Id隔离，使用默认的连接字符串
            var connectionString = tenant.TenantType == TenantTypeEnum.Id ? iTenant.GetConnectionScope(SqlSugarConst.ConfigId).CurrentConnectionConfig.ConnectionString : tenant.Connection;
            var connectionConfig = new DbConnectionConfig
            {
                ConfigId = tenant.Id,
                DbType = tenant.DbType,
                ConnectionString = connectionString,
                IsAutoCloseConnection = true,

                // 继承主库的“启用驼峰转下划线”设置
                EnableUnderLine = mainConnConfig.EnableUnderLine,
            };
            iTenant.AddConnection(connectionConfig);
            SqlSugarSetup.SetDbConfig(connectionConfig);
            SqlSugarSetup.SetDbAop(iTenant.GetConnectionScope(tenantId.ToString()));
        }
        base.Context = iTenant.GetConnectionScope(tenantId.ToString());
    }
}