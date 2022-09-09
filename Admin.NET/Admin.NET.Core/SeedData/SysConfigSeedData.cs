namespace Admin.NET.Core;

/// <summary>
/// 系统配置表种子数据
/// </summary>
public class SysConfigSeedData : ISqlSugarEntitySeedData<SysConfig>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SysConfig> HasData()
    {
        return new[]
        {
            new SysConfig{ Id=252885263003800, Name="演示环境", Code="sys_demo_env", Value="False", SysFlag=YesNoEnum.Y, Remark="演示环境", Order=1, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=252885263003801, Name="默认密码", Code="sys_password", Value="123456", SysFlag=YesNoEnum.Y, Remark="默认密码", Order=2, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=252885263003802, Name="Token过期时间", Code="sys_token_expire", Value="10080", SysFlag=YesNoEnum.Y, Remark="Token过期时间", Order=3, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=252885263003803, Name="操作日志", Code="sys_op_log", Value="True", SysFlag=YesNoEnum.Y, Remark="开启操作日志", Order=4, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=252885263003804, Name="单用户登录", Code="sys_single_login", Value="True", SysFlag=YesNoEnum.Y, Remark="开启单用户登录", Order=5, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=252885263003805, Name="验证码", Code="sys_captcha", Value="True", SysFlag=YesNoEnum.Y, Remark="开启验证码", Order=6, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=252885263003806, Name="管理员角色编码", Code="sys_admin_role", Value="True", SysFlag=YesNoEnum.Y, Remark="管理员角色编码", Order=7, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
        };
    }
}