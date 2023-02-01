namespace Admin.NET.Core;

/// <summary>
/// 系统租户表种子数据
/// </summary>
public class SysTenantSeedData : ISqlSugarEntitySeedData<SysTenant>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    [IgnoreUpdate]
    public IEnumerable<SysTenant> HasData()
    {
        return new[]
        {
            new SysTenant{ Id=1300000000001, OrgId=1300000000101, UserId=1300000000111, Host="www.dilon.vip", TenantType=TenantTypeEnum.Id, DbType=SqlSugar.DbType.Sqlite, Connection="DataSource=./Admin.NET.db", ConfigId=SqlSugarConst.ConfigId, Remark="系统默认", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
        };
    }
}