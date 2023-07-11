namespace Admin.NET.Plugin.Elsa;

/// <summary>
/// Elsa 配置选项
/// </summary>
public sealed class ElsaOptions : IConfigurableOptions
{
    /// <summary>
    /// 服务地址
    /// </summary>
    public Elsa_Server Server { get; set; }
}

public sealed class Elsa_Server
{
    /// <summary>
    /// 地址
    /// </summary>
    public string BaseUrl { get; set; }
}