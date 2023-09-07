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
/// 系统用户表种子数据
/// </summary>
public class SysUserSeedData : ISqlSugarEntitySeedData<SysUser>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SysUser> HasData()
    {
        var encryptPasswod = CryptogramUtil.Encrypt("123456");

        return new[]
        {
            new SysUser{ Id=1300000000101, Account="superadmin", Password=encryptPasswod, NickName="超级管理员", RealName="超级管理员", Phone="18020030720", Birthday=DateTime.Parse("1986-06-28"), Sex=GenderEnum.Male, AccountType=AccountTypeEnum.SuperAdmin, Remark="超级管理员", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), TenantId=1300000000001 },
            new SysUser{ Id=1300000000111, Account="admin", Password=encryptPasswod, NickName="系统管理员", RealName="系统管理员", Phone="18020030720", Birthday=DateTime.Parse("1986-06-28"), Sex=GenderEnum.Male, AccountType=AccountTypeEnum.SysAdmin, Remark="系统管理员", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrgId=1300000000101, PosId=1300000000102, TenantId=1300000000001 },
            new SysUser{ Id=1300000000112, Account="user1", Password=encryptPasswod, NickName="部门主管", RealName="部门主管", Phone="18020030720", Birthday=DateTime.Parse("1986-06-28"), Sex=GenderEnum.Female, AccountType=AccountTypeEnum.NormalUser, Remark="部门主管", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrgId=1300000000102, PosId=1300000000108, TenantId=1300000000001 },
            new SysUser{ Id=1300000000113, Account="user2", Password=encryptPasswod, NickName="部门职员", RealName="部门职员", Phone="18020030720", Birthday=DateTime.Parse("1986-06-28"), Sex=GenderEnum.Female, AccountType=AccountTypeEnum.NormalUser, Remark="部门职员", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrgId=1300000000103, PosId=1300000000110, TenantId=1300000000001 },
            new SysUser{ Id=1300000000114, Account="user3", Password=encryptPasswod, NickName="普通用户", RealName="普通用户", Phone="18020030720", Birthday=DateTime.Parse("1986-06-28"), Sex=GenderEnum.Female, AccountType=AccountTypeEnum.NormalUser, Remark="普通用户", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrgId=1300000000104, PosId=1300000000115, TenantId=1300000000001 },
            new SysUser{ Id=1300000000115, Account="user4", Password=encryptPasswod, NickName="其他", RealName="其他", Phone="18020030720", Birthday=DateTime.Parse("1986-06-28"), Sex=GenderEnum.Female, AccountType=AccountTypeEnum.Member, Remark="会员", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrgId=1300000000105, PosId=1300000000116, TenantId=1300000000001 },
        };
    }
}