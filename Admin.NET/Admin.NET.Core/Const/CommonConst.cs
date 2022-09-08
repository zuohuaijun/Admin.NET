namespace Admin.NET.Core;

/// <summary>
/// 通用常量
/// </summary>
public class CommonConst
{
    /// <summary>
    /// 默认密码
    /// </summary>
    public const string SysPassword = "123456";

    /// <summary>
    /// 系统管理员角色编码
    /// </summary>
    public const string SysAdminRoleCode = "sys_admin_role";

    /// <summary>
    /// 演示环境开关
    /// </summary>
    public const string SysDemoEnv = "sys_demo_env";

    /// <summary>
    /// 验证码开关
    /// </summary>
    public const string SysCaptcha = "sys_captcha";

    /// <summary>
    /// 开启操作日志
    /// </summary>
    public const string SysOpLog = "sys_op_log";

    /// <summary>
    /// 开启当用户登录
    /// </summary>
    public const string SysSingleLogin = "sys_single_login";

    /// <summary>
    /// 开启全局脱敏处理（默认不开启）
    /// </summary>
    public static bool SysSensitiveDetection = false;
}