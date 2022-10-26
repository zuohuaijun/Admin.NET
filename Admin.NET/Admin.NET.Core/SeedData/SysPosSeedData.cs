namespace Admin.NET.Core;

/// <summary>
/// 系统职位表种子数据
/// </summary>
public class SysPosSeedData : ISqlSugarEntitySeedData<SysPos>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SysPos> HasData()
    {
        return new[]
        {
            new SysPos{ Id=252885263003720, Name="党委书记", Code="dwsj", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="党委书记", TenantId=142307070918780 },
            new SysPos{ Id=252885263003721, Name="总经理", Code="zjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="总经理", TenantId=142307070918780 },
            new SysPos{ Id=252885263003722, Name="副总经理", Code="fzjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副总经理", Status=StatusEnum.Disable, TenantId=142307070918780 },
            new SysPos{ Id=252885263003723, Name="部门经理", Code="bmjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="部门经理", TenantId=142307070918780 },
            new SysPos{ Id=252885263003724, Name="部门副经理", Code="bmfjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="部门副经理", TenantId=142307070918780 },
            new SysPos{ Id=252885263003725, Name="局长", Code="jz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="局长", TenantId=142307070918780 },
            new SysPos{ Id=252885263003726, Name="副局长", Code="fjz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副局长", TenantId=142307070918780 },
            new SysPos{ Id=252885263003727, Name="科长", Code="kz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="科长", TenantId=142307070918780 },
            new SysPos{ Id=252885263003728, Name="副科长", Code="fkz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副科长", TenantId=142307070918780 },
            new SysPos{ Id=252885263003729, Name="职员", Code="zy", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="职员", TenantId=142307070918780 },
            new SysPos{ Id=252885263003730, Name="其他", Code="qt", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="其他", TenantId=142307070918780 },

            new SysPos{ Id=252885263004720, Name="党委书记", Code="dwsj", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="党委书记", TenantId=142307070918781 },
            new SysPos{ Id=252885263004721, Name="总经理", Code="zjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="总经理", TenantId=142307070918781 },
            new SysPos{ Id=252885263004722, Name="副总经理", Code="fzjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副总经理", Status=StatusEnum.Disable, TenantId=142307070918781 },
            new SysPos{ Id=252885263004723, Name="部门经理", Code="bmjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="部门经理", TenantId=142307070918781 },
            new SysPos{ Id=252885263004724, Name="部门副经理", Code="bmfjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="部门副经理", TenantId=142307070918781 },
            new SysPos{ Id=252885263004725, Name="局长", Code="jz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="局长", TenantId=142307070918781 },
            new SysPos{ Id=252885263004726, Name="副局长", Code="fjz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副局长", TenantId=142307070918781 },
            new SysPos{ Id=252885263004727, Name="科长", Code="kz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="科长", TenantId=142307070918781 },
            new SysPos{ Id=252885263004728, Name="副科长", Code="fkz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副科长", TenantId=142307070918781 },
            new SysPos{ Id=252885263004729, Name="职员", Code="zy", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="职员", TenantId=142307070918781 },
            new SysPos{ Id=252885263004730, Name="其他", Code="qt", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="其他", TenantId=142307070918781 },
        };
    }
}