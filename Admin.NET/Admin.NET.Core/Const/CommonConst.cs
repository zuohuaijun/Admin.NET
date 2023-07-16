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
/// 通用常量
/// </summary>
[Const("平台配置")]
public class CommonConst
{
    /// <summary>
    /// 演示环境开关
    /// </summary>
    public const string SysDemoEnv = "sys_demo";

    /// <summary>
    /// 默认密码
    /// </summary>
    public const string SysPassword = "sys_password";

    /// <summary>
    /// 登录二次验证
    /// </summary>
    public const string SysSecondVer = "sys_second_ver";

    /// <summary>
    /// 开启图形验证码
    /// </summary>
    public const string SysCaptcha = "sys_captcha";

    /// <summary>
    /// 开启水印
    /// </summary>
    public const string SysWatermark = "sys_watermark";

    /// <summary>
    /// 开启操作日志
    /// </summary>
    public const string SysOpLog = "sys_oplog";

    /// <summary>
    /// Token过期时间
    /// </summary>
    public const string SysTokenExpire = "sys_token_expire";

    /// <summary>
    /// RefreshToken过期时间
    /// </summary>
    public const string SysRefreshTokenExpire = "sys_refresh_token_expire";

    /// <summary>
    /// 单用户登录
    /// </summary>
    public const string SysSingleLogin = "sys_single_login";

    /// <summary>
    /// 系统管理员角色编码
    /// </summary>
    public const string SysAdminRole = "sys_admin";

    /// <summary>
    /// 开启全局脱敏处理（默认不开启）
    /// </summary>
    public static bool SysSensitiveDetection = false;
}