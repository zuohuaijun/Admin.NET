// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 密码配置选项
/// </summary>
public sealed class CryptogramOptions : IConfigurableOptions
{
    /// <summary>
    /// 是否开启密码强度验证
    /// </summary>
    public bool StrongPassword { get; set; }

    /// <summary>
    /// 密码强度验证正则表达式
    /// </summary>
    public string PasswordStrengthValidation { get; set; }

    /// <summary>
    /// 密码强度验证提示
    /// </summary>
    public string PasswordStrengthValidationMsg { get; set; }

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