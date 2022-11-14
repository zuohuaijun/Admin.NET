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
    [IgnoreUpdate]
    public IEnumerable<SysPos> HasData()
    {
        return new[]
        {
            new SysPos{ Id=252885263003720, Name="党委书记", Code="dwsj", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="党委书记", TenantId=123456780000000 },
            new SysPos{ Id=252885263003721, Name="董事长", Code="dsz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="董事长", TenantId=123456780000000 },
            new SysPos{ Id=252885263003722, Name="副董事长", Code="fdsz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副董事长", TenantId=123456780000000 },
            new SysPos{ Id=252885263003723, Name="总经理", Code="zjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="总经理", TenantId=123456780000000 },
            new SysPos{ Id=252885263003724, Name="副总经理", Code="fzjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副总经理", TenantId=123456780000000 },
            new SysPos{ Id=252885263003725, Name="部门经理", Code="bmjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="部门经理", TenantId=123456780000000 },
            new SysPos{ Id=252885263003726, Name="部门副经理", Code="bmfjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="部门副经理", TenantId=123456780000000 },
            new SysPos{ Id=252885263003727, Name="主任", Code="zr", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="主任", TenantId=123456780000000 },
            new SysPos{ Id=252885263003728, Name="副主任", Code="fzr", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副主任", TenantId=123456780000000 },
            new SysPos{ Id=252885263003729, Name="局长", Code="jz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="局长", TenantId=123456780000000 },
            new SysPos{ Id=252885263003730, Name="副局长", Code="fjz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副局长", TenantId=123456780000000 },
            new SysPos{ Id=252885263003731, Name="科长", Code="kz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="科长", TenantId=123456780000000 },
            new SysPos{ Id=252885263003732, Name="副科长", Code="fkz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副科长", TenantId=123456780000000 },
            new SysPos{ Id=252885263003733, Name="职员", Code="zy", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="职员", TenantId=123456780000000 },
            new SysPos{ Id=252885263003734, Name="其他", Code="qt", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="其他", TenantId=123456780000000 },
        };
    }
}