// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 性别枚举
/// </summary>
[Description("性别枚举")]
public enum GenderEnum
{
    /// <summary>
    /// 男
    /// </summary>
    [Description("男")]
    Male = 1,

    /// <summary>
    /// 女
    /// </summary>
    [Description("女")]
    Female = 2,

    /// <summary>
    /// 其他
    /// </summary>
    [Description("其他")]
    Other = 3
}