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
    public IEnumerable<SysTenant> HasData()
    {
        return new[]
        {
            new SysTenant{ Id=142307070918780, Name="系统默认", AdminName="Administrator", Host="www.dilon.vip", Email="zuohuaijun@163.com", Phone="18020030720", TenantType=TenantTypeEnum.Db, DbType=SqlSugar.DbType.Sqlite, Connection="DataSource=./Admin.NET.db", Remark="系统默认", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysTenant{ Id=142307070918781, Name="测试租户", AdminName="TestAdmin", Host="www.dilon.top", Email="515096995@qq.com", Phone="18020030720", TenantType=TenantTypeEnum.Db, DbType=SqlSugar.DbType.Sqlite, Connection="DataSource=./Test.db", Remark="测试租户", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
        };
    }
}