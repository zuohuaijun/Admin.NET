// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using System.Security.Cryptography;

namespace Admin.NET.Core;

/// <summary>
/// 3DES文件加解密
/// </summary>
public static class TripleDES
{
    /// <summary>
    /// 加密文件
    /// </summary>
    /// <param name="inputFile">待加密文件路径</param>
    /// <param name="outputFile">加密后的文件路径</param>
    /// <param name="password">密码 （24位长度）</param>
    [Obsolete]
    public static void EncryptFile(string inputFile, string outputFile, string password)
    {
        using var tdes = new TripleDESCryptoServiceProvider();
        tdes.Mode = CipherMode.ECB;
        tdes.Padding = PaddingMode.PKCS7;
        tdes.Key = Encoding.UTF8.GetBytes(password);
        using var inputFileStream = new FileStream(inputFile, FileMode.Open);
        using var encryptedFileStream = new FileStream(outputFile, FileMode.Create);
        using var cryptoStream = new CryptoStream(encryptedFileStream, tdes.CreateEncryptor(), CryptoStreamMode.Write);
        inputFileStream.CopyTo(cryptoStream);
    }

    /// <summary>
    /// 加密文件
    /// </summary>
    /// <param name="inputFile">加密的文件路径</param>
    /// <param name="outputFile">解密后的文件路径</param>
    /// <param name="password">密码 （24位长度）</param>
    [Obsolete]
    public static void DecryptFile(string inputFile, string outputFile, string password)
    {
        using var tdes = new TripleDESCryptoServiceProvider();
        tdes.Mode = CipherMode.ECB;
        tdes.Padding = PaddingMode.PKCS7;
        tdes.Key = Encoding.UTF8.GetBytes(password);
        using var encryptedFileStream = new FileStream(inputFile, FileMode.Open);
        using var decryptedFileStream = new FileStream(outputFile, FileMode.Create);
        using var cryptoStream = new CryptoStream(encryptedFileStream, tdes.CreateDecryptor(), CryptoStreamMode.Read);
        cryptoStream.CopyTo(decryptedFileStream);
    }
}