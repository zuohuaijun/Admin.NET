// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 租户类型枚举
/// </summary>
[Description("租户类型枚举")]
public enum TenantTypeEnum
{
    /// <summary>
    /// Id隔离
    /// </summary>
    [Description("Id隔离")]
    Id = 0,

    /// <summary>
    /// 库隔离
    /// </summary>
    [Description("库隔离")]
    Db = 1,
}