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
        return new[]
        {
            new SysUser{ Id=252885263000000, UserName="superAdmin", Password="e10adc3949ba59abbe56e057f20f883e", NickName="超级管理员", RealName="超级管理员", Phone="18020030720", Birthday=DateTime.Parse("1986-06-28"), Sex=GenderEnum.Male, UserType=UserTypeEnum.SuperAdmin, Remark="超级管理员", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), TenantId=142307070918780 },
            new SysUser{ Id=252885263003721, UserName="admin", Password="e10adc3949ba59abbe56e057f20f883e", NickName="管理员", RealName="管理员", Phone="18020030720", Birthday=DateTime.Parse("1986-06-28"), Sex=GenderEnum.Male, UserType=UserTypeEnum.Admin, Remark="管理员", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrgId=252885263003721, PosId=252885263003720, TenantId=142307070918780 },
            new SysUser{ Id=252885263003722, UserName="guest", Password="e10adc3949ba59abbe56e057f20f883e", NickName="普通用户", RealName="普通用户", Phone="18020030720", Birthday=DateTime.Parse("1986-06-28"), Sex=GenderEnum.Female, UserType=UserTypeEnum.None, Remark="普通账号", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrgId=252885263003722, PosId=252885263003721, TenantId=142307070918780 },

            new SysUser{ Id=252885263003723, UserName="Admin", Password="e10adc3949ba59abbe56e057f20f883e", NickName="管理员", RealName="管理员", Phone="18020030720", Birthday=DateTime.Parse("1986-06-28"), Sex=GenderEnum.Male, UserType=UserTypeEnum.Admin, Remark="管理员", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrgId=252885263003744, PosId=252885263003724, TenantId=142307070918781 }
        };
    }
}