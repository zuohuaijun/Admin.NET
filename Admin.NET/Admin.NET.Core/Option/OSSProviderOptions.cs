using OnceMi.AspNetCore.OSS;

namespace Admin.NET.Core;

/// <summary>
/// 对象存储配置选项
/// </summary>
public sealed class OSSProviderOptions : OSSOptions, IConfigurableOptions
{
    /// <summary>
    /// 是否启用OSS存储
    /// </summary>
    public bool IsEnable { get; set; }
}