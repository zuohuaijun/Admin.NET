namespace Admin.NET.Core;

/// <summary>
/// 对象存储配置选项
/// </summary>
public sealed class OSSProviderOptions : IConfigurableOptions
{
    /// <summary>
    /// 是否启用OSS存储
    /// </summary>
    public bool IsEnable { get; set; }

    public int Provider { get; set; }

    public string Endpoint { get; set; }

    public string AccessKey { get; set; }

    public string SecretKey { get; set; }

    public string Region { get; set; }

    public string SessionToken { get; set; }

    public bool IsEnableHttps { get; set; }

    public bool IsEnableCache { get; set; }
}