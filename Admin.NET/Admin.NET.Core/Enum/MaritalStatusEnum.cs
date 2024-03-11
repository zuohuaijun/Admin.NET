// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 婚姻状况枚举
/// </summary>
[Description("婚姻状况枚举")]
public enum MaritalStatusEnum
{
    /// <summary>
    /// 未婚
    /// </summary>
    [Description("未婚")]
    UnMarried = 1,

    /// <summary>
    /// 已婚
    /// </summary>
    [Description("已婚")]
    Married = 2,

    /// <summary>
    /// 离异
    /// </summary>
    [Description("离异")]
    Divorce = 3,

    /// <summary>
    /// 再婚
    /// </summary>
    [Description("再婚")]
    Remarry = 4,

    /// <summary>
    /// 丧偶
    /// </summary>
    [Description("丧偶")]
    Widowed = 5,

    /// <summary>
    /// 未知
    /// </summary>
    [Description("未知")]
    None = 6,
}