namespace Admin.NET.Core;

/// <summary>
/// 缓存相关常量
/// </summary>
public class CacheConst
{
    /// <summary>
    /// 用户缓存
    /// </summary>
    public const string KeyUser = "user_";

    /// <summary>
    /// 菜单缓存
    /// </summary>
    public const string KeyMenu = "menu_";

    /// <summary>
    /// 权限缓存
    /// </summary>
    public const string KeyPermission = "permission_";

    /// <summary>
    /// 机构Id集合缓存
    /// </summary>
    public const string KeyOrgIdList = "org_";

    /// <summary>
    /// 验证码缓存
    /// </summary>
    public const string KeyVerCode = "verCode_";

    /// <summary>
    /// 所有缓存关键字集合
    /// </summary>
    public const string KeyAll = "keys";

    /// <summary>
    /// 定时任务缓存
    /// </summary>
    public const string KeyTimer = "timer";

    /// <summary>
    /// 在线用户缓存
    /// </summary>
    public const string KeyOnlineUser = "onlineuser";

    /// <summary>
    /// 常量下拉框
    /// </summary>
    public const string KeyConstSelector = "const_selector:";
}