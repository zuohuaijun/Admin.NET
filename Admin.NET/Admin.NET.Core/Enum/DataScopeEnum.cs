// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 角色数据范围枚举
/// </summary>
[Description("角色数据范围枚举")]
public enum DataScopeEnum
{
    /// <summary>
    /// 全部数据
    /// </summary>
    [Description("全部数据")]
    All = 1,

    /// <summary>
    /// 本部门及以下数据
    /// </summary>
    [Description("本部门及以下数据")]
    DeptChild = 2,

    /// <summary>
    /// 本部门数据
    /// </summary>
    [Description("本部门数据")]
    Dept = 3,

    /// <summary>
    /// 仅本人数据
    /// </summary>
    [Description("仅本人数据")]
    Self = 4,

    /// <summary>
    /// 自定义数据
    /// </summary>
    [Description("自定义数据")]
    Define = 5
}