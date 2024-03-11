// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

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