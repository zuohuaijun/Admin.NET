namespace Admin.NET.Core;

/// <summary>
/// 系统机构表种子数据
/// </summary>
public class SysOrgSeedData : ISqlSugarEntitySeedData<SysOrg>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SysOrg> HasData()
    {
        return new[]
        {
            new SysOrg{ Id=252885263003720, Pid=0, Name="大名科技有限公司", Code="1001", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="大名科技", TenantId=142307070918780 },
            new SysOrg{ Id=252885263003721, Pid=252885263003720, Name="市场部", Code="100101", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="市场部", TenantId=142307070918780 },
            new SysOrg{ Id=252885263003722, Pid=252885263003720, Name="研发部", Code="100102", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="研发部", TenantId=142307070918780 },
            new SysOrg{ Id=252885263003723, Pid=252885263003720, Name="财务部", Code="100103", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="财务部", TenantId=142307070918780 },
            new SysOrg{ Id=252885263003724, Pid=252885263003723, Name="财务部1", Code="10010301", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="财务部1", TenantId=142307070918780 },
            new SysOrg{ Id=252885263003725, Pid=252885263003723, Name="财务部2", Code="10010302", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="财务部2", TenantId=142307070918780 },

            new SysOrg{ Id=252885263003730, Pid=0, Name="分公司1", Code="1002", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="分公司1", TenantId=142307070918780 },
            new SysOrg{ Id=252885263003731, Pid=252885263003730, Name="市场部", Code="100201", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="市场部", TenantId=142307070918780 },
            new SysOrg{ Id=252885263003732, Pid=252885263003730, Name="研发部", Code="100202", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="研发部", TenantId=142307070918780 },
            new SysOrg{ Id=252885263003733, Pid=252885263003730, Name="财务部", Code="100203", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="财务部", TenantId=142307070918780 },

            new SysOrg{ Id=252885263003740, Pid=0, Name="分公司2", Code="1003", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="分公司2", TenantId=142307070918780  },
            new SysOrg{ Id=252885263003741, Pid=252885263003740, Name="市场部", Code="100301", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="市场部", TenantId=142307070918780 },
            new SysOrg{ Id=252885263003742, Pid=252885263003740, Name="研发部", Code="100302", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="市场部", TenantId=142307070918780 },
            new SysOrg{ Id=252885263003743, Pid=252885263003740, Name="财务部", Code="100303", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="市场部", TenantId=142307070918780 },

            new SysOrg{ Id=252885263004720, Pid=0, Name="租户1公司", Code="2001", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="租户1公司" , TenantId=142307070918781 },
        };
    }
}