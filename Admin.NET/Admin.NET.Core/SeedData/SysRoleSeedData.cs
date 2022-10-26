namespace Admin.NET.Core;

/// <summary>
/// 系统角色表种子数据
/// </summary>
public class SysRoleSeedData : ISqlSugarEntitySeedData<SysRole>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SysRole> HasData()
    {
        return new[]
        {
            new SysRole{ Id=252885263003721, Name="系统管理员", DataScope=DataScopeEnum.All, Code="sysAdmin", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="管理员", TenantId=142307070918780 },
            new SysRole{ Id=252885263003722, Name="部门主管", DataScope=DataScopeEnum.Dept_with_child, Code="deptAdmin", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="本部门及以下数据", TenantId=142307070918780 },
            new SysRole{ Id=252885263003723, Name="部门职员", DataScope=DataScopeEnum.Dept, Code="deptUser", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="本部门数据", TenantId=142307070918780 },
            new SysRole{ Id=252885263003724, Name="普通用户", DataScope=DataScopeEnum.Self, Code="user", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="仅本人数据", TenantId=142307070918780 },
            new SysRole{ Id=252885263003725, Name="其他", DataScope=DataScopeEnum.Define, Code="other", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="自定义数据", TenantId=142307070918780 },

            new SysRole{ Id=252885263004721, Name="系统管理员", DataScope=DataScopeEnum.All, Code="sysAdmin", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="管理员", TenantId=142307070918781 },
            new SysRole{ Id=252885263004722, Name="部门主管", DataScope=DataScopeEnum.Dept_with_child, Code="deptAdmin", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="本部门及以下数据", TenantId=142307070918781 },
            new SysRole{ Id=252885263004723, Name="部门职员", DataScope=DataScopeEnum.Dept, Code="deptUser", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="本部门数据", TenantId=142307070918781 },
            new SysRole{ Id=252885263004724, Name="普通用户", DataScope=DataScopeEnum.Self, Code="user", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="仅本人数据", TenantId=142307070918781 },
            new SysRole{ Id=252885263004725, Name="其他", DataScope=DataScopeEnum.Define, Code="other", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="自定义数据", TenantId=142307070918781 },
        };
    }
}