namespace Admin.NET.Core;

/// <summary>
/// 密码配置选项
/// </summary>
public sealed class CryptoOptions : IConfigurableOptions
{
    /// <summary>
    /// 密码类型
    /// </summary>
    public string CryptoType { get; set; }

    /// <summary>
    /// 公钥
    /// </summary>
    public string PublicKey { get; set; }

    /// <summary>
    /// 私钥
    /// </summary>
    public string PrivateKey { get; set; }
}