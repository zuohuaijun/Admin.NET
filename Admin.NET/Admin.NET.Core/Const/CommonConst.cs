﻿// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

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