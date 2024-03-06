// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

/// <summary>
/// 国密公钥私钥对输出
/// </summary>
public class SmKeyPairOutput
{
    /// <summary>
    /// 私匙
    /// </summary>
    public string PrivateKey { get; set; }

    /// <summary>
    /// 公匙
    /// </summary>
    public string PublicKey { get; set; }
}