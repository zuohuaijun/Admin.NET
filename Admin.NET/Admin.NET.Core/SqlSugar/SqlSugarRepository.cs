namespace Admin.NET.Core;

/// <summary>
/// SqlSugar仓储类
/// </summary>
/// <typeparam name="T"></typeparam>
public class SqlSugarRepository<T> : SimpleClient<T> where T : class, new()
{
    // protected ITenant itenant = null; // 多租户事务

    public SqlSugarRepository(ISqlSugarClient context = null) : base(context) // 默认值等于null不能少
    {
        base.Context = App.GetService<ISqlSugarClient>().AsTenant().GetConnectionWithAttr<T>();
        //itenant = App.GetService<ISqlSugarClient>().AsTenant() ;
    }
}