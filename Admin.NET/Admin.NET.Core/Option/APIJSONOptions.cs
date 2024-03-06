// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// APIJSON配置选项
/// </summary>
public sealed class APIJSONOptions : IConfigurableOptions
{
    /// <summary>
    /// 角色集合
    /// </summary>
    public List<APIJSON_Role> Roles { get; set; }
}

/// <summary>
/// APIJSON角色权限
/// </summary>
public class APIJSON_Role
{
    /// <summary>
    /// 角色名称
    /// </summary>
    public string RoleName { get; set; }

    /// <summary>
    /// 查询
    /// </summary>
    public APIJSON_RoleItem Select { get; set; }

    /// <summary>
    /// 增加
    /// </summary>
    public APIJSON_RoleItem Insert { get; set; }

    /// <summary>
    /// 更新
    /// </summary>
    public APIJSON_RoleItem Update { get; set; }

    /// <summary>
    /// 删除
    /// </summary>
    public APIJSON_RoleItem Delete { get; set; }
}

/// <summary>
/// APIJSON角色权限内容
/// </summary>
public class APIJSON_RoleItem
{
    /// <summary>
    /// 表集合
    /// </summary>
    public string[] Table { get; set; }

    /// <summary>
    /// 列集合
    /// </summary>
    public string[] Column { get; set; }

    /// <summary>
    /// 过滤器
    /// </summary>
    public string[] Filter { get; set; }
}