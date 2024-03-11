// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

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
            new SysConfig{ Id=1300000000101, Name="演示环境", Code="sys_demo", Value="False", SysFlag=YesNoEnum.Y, Remark="演示环境", OrderNo=1, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000102, Name="默认密码", Code="sys_password", Value="123456", SysFlag=YesNoEnum.Y, Remark="默认密码", OrderNo=2, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000103, Name="Token过期时间", Code="sys_token_expire", Value="10080", SysFlag=YesNoEnum.Y, Remark="Token过期时间（分钟）", OrderNo=3, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000104, Name="操作日志", Code="sys_oplog", Value="True", SysFlag=YesNoEnum.Y, Remark="开启操作日志", OrderNo=4, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000105, Name="单设备登录", Code="sys_single_login", Value="False", SysFlag=YesNoEnum.Y, Remark="开启单设备登录", OrderNo=5, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000106, Name="登录二次验证", Code="sys_second_ver", Value="False", SysFlag=YesNoEnum.Y, Remark="登录二次验证", OrderNo=6, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000107, Name="开启图形验证码", Code="sys_captcha", Value="True", SysFlag=YesNoEnum.Y, Remark="开启图形验证码", OrderNo=7, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000108, Name="开启水印", Code="sys_watermark", Value="False", SysFlag=YesNoEnum.Y, Remark="开启水印", OrderNo=8, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000109, Name="RefreshToken过期时间", Code="sys_refresh_token_expire", Value="20160", SysFlag=YesNoEnum.Y, Remark="RefreshToken过期时间，单位分钟（一般 refresh_token 的有效时间 > 2 * access_token 的有效时间）", OrderNo=9, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
        };
    }
}