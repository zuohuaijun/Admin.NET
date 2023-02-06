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
    [IgnoreUpdate]
    public IEnumerable<SysUser> HasData()
    {
        var encryptPasswod = CryptogramUtil.Encrypt("123456");

        return new[]
        {
            new SysUser{ Id=1300000000101, Account="superadmin", Password=encryptPasswod, NickName="超级管理员", RealName="超级管理员", Phone="18020030720", Birthday=DateTime.Parse("1986-06-28"), Sex=GenderEnum.Male, AccountType=AccountTypeEnum.SuperAdmin, Remark="超级管理员", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), TenantId=1300000000001 },
            new SysUser{ Id=1300000000111, Account="admin", Password=encryptPasswod, NickName="系统管理员", RealName="系统管理员", Phone="18020030720", Birthday=DateTime.Parse("1986-06-28"), Sex=GenderEnum.Male, AccountType=AccountTypeEnum.Admin, Remark="系统管理员", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrgId=1300000000101, PosId=1300000000102, TenantId=1300000000001 },
            new SysUser{ Id=1300000000112, Account="user1", Password=encryptPasswod, NickName="部门主管", RealName="部门主管", Phone="18020030720", Birthday=DateTime.Parse("1986-06-28"), Sex=GenderEnum.Female, AccountType=AccountTypeEnum.User, Remark="部门主管", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrgId=1300000000102, PosId=1300000000108, TenantId=1300000000001 },
            new SysUser{ Id=1300000000113, Account="user2", Password=encryptPasswod, NickName="部门职员", RealName="部门职员", Phone="18020030720", Birthday=DateTime.Parse("1986-06-28"), Sex=GenderEnum.Female, AccountType=AccountTypeEnum.User, Remark="部门职员", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrgId=1300000000103, PosId=1300000000110, TenantId=1300000000001 },
            new SysUser{ Id=1300000000114, Account="user3", Password=encryptPasswod, NickName="普通用户", RealName="普通用户", Phone="18020030720", Birthday=DateTime.Parse("1986-06-28"), Sex=GenderEnum.Female, AccountType=AccountTypeEnum.User, Remark="普通用户", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrgId=1300000000104, PosId=1300000000115, TenantId=1300000000001 },
            new SysUser{ Id=1300000000115, Account="user4", Password=encryptPasswod, NickName="其他", RealName="其他", Phone="18020030720", Birthday=DateTime.Parse("1986-06-28"), Sex=GenderEnum.Female, AccountType=AccountTypeEnum.None, Remark="普通用户", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrgId=1300000000105, PosId=1300000000116, TenantId=1300000000001 },
        };
    }
}