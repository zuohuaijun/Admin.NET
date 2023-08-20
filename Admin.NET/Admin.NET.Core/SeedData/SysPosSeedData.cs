// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

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
            new SysPos{ Id=1300000000101, Name="党委书记", Code="dwsj", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="党委书记", TenantId=1300000000001 },
            new SysPos{ Id=1300000000102, Name="董事长", Code="dsz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="董事长", TenantId=1300000000001 },
            new SysPos{ Id=1300000000103, Name="副董事长", Code="fdsz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副董事长", TenantId=1300000000001 },
            new SysPos{ Id=1300000000104, Name="总经理", Code="zjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="总经理", TenantId=1300000000001 },
            new SysPos{ Id=1300000000105, Name="副总经理", Code="fzjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副总经理", TenantId=1300000000001 },
            new SysPos{ Id=1300000000106, Name="部门经理", Code="bmjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="部门经理", TenantId=1300000000001 },
            new SysPos{ Id=1300000000107, Name="部门副经理", Code="bmfjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="部门副经理", TenantId=1300000000001 },
            new SysPos{ Id=1300000000108, Name="主任", Code="zr", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="主任", TenantId=1300000000001 },
            new SysPos{ Id=1300000000109, Name="副主任", Code="fzr", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副主任", TenantId=1300000000001 },
            new SysPos{ Id=1300000000110, Name="局长", Code="jz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="局长", TenantId=1300000000001 },
            new SysPos{ Id=1300000000111, Name="副局长", Code="fjz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副局长", TenantId=1300000000001 },
            new SysPos{ Id=1300000000112, Name="科长", Code="kz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="科长", TenantId=1300000000001 },
            new SysPos{ Id=1300000000113, Name="副科长", Code="fkz", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副科长", TenantId=1300000000001 },
            new SysPos{ Id=1300000000114, Name="财务", Code="cw", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="财务", TenantId=1300000000001 },
            new SysPos{ Id=1300000000115, Name="职员", Code="zy", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="职员", TenantId=1300000000001 },
            new SysPos{ Id=1300000000116, Name="其他", Code="qt", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="其他", TenantId=1300000000001 },
        };
    }
}