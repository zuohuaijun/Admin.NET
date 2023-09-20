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
            new SysConfig{ Id=1300000000103, Name="Token过期时间", Code="sys_token_expire", Value="10080", SysFlag=YesNoEnum.Y, Remark="Token过期时间", OrderNo=3, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000104, Name="操作日志", Code="sys_oplog", Value="True", SysFlag=YesNoEnum.Y, Remark="开启操作日志", OrderNo=4, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000105, Name="单设备登录", Code="sys_single_login", Value="False", SysFlag=YesNoEnum.Y, Remark="开启单设备登录", OrderNo=5, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000106, Name="登录二次验证", Code="sys_second_ver", Value="False", SysFlag=YesNoEnum.Y, Remark="登录二次验证", OrderNo=6, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000107, Name="开启图形验证码", Code="sys_captcha", Value="True", SysFlag=YesNoEnum.Y, Remark="开启图形验证码", OrderNo=7, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000108, Name="开启水印", Code="sys_watermark", Value="False", SysFlag=YesNoEnum.Y, Remark="开启水印", OrderNo=8, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000109, Name="RefreshToken过期时间", Code="sys_refresh_token_expire", Value="20160", SysFlag=YesNoEnum.Y, Remark="RefreshToken过期时间，单位分钟（一般 refresh_token 的有效时间 > 2 * access_token 的有效时间）", OrderNo=9, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
        };
    }
}