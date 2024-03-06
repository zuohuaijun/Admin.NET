// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 缓存类型枚举
/// </summary>
[Description("缓存类型枚举")]
public enum CacheTypeEnum
{
    /// <summary>
    /// 内存缓存
    /// </summary>
    [Description("内存缓存")]
    Memory,

    /// <summary>
    /// Redis缓存
    /// </summary>
    [Description("Redis缓存")]
    Redis
}