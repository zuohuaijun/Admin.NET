// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 雪花Id配置选项
/// </summary>
public sealed class SnowIdOptions : IdGeneratorOptions, IConfigurableOptions
{
    /// <summary>
    /// 缓存前缀
    /// </summary>
    public string WorkerPrefix { get; set; }
}