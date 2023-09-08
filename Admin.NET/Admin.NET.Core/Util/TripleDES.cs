using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Admin.NET.Core;

public static class TripleDES
{ 
    /// <summary>
    /// 加密文件
    /// </summary>
    /// <param name="inputFile">待加密文件路径</param>
    /// <param name="outputFile">加密后的文件路径</param>
    /// <param name="password">密码 （24位长度）</param>
    public static void EncryptFile(string inputFile, string outputFile, string password)
    { 
        using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
        {
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            tdes.Key = Encoding.UTF8.GetBytes(password);
            using (FileStream inputFileStream = new FileStream(inputFile, FileMode.Open))
            using (FileStream encryptedFileStream = new FileStream(outputFile, FileMode.Create))
            using (CryptoStream cryptoStream = new CryptoStream(encryptedFileStream, tdes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                inputFileStream.CopyTo(cryptoStream);
            }
        } 
    }

    /// <summary>
    /// 加密文件
    /// </summary>
    /// <param name="inputFile">加密的文件路径</param>
    /// <param name="outputFile">解密后的文件路径</param>
    /// <param name="password">密码 （24位长度）</param>
    public static void DecryptFile(string inputFile, string outputFile, string password)
    { 
        using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
        {
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            tdes.Key = Encoding.UTF8.GetBytes(password);
            using (FileStream encryptedFileStream = new FileStream(inputFile, FileMode.Open))
            using (FileStream decryptedFileStream = new FileStream(outputFile, FileMode.Create))
            using (CryptoStream cryptoStream = new CryptoStream(encryptedFileStream, tdes.CreateDecryptor(), CryptoStreamMode.Read))
            {
                cryptoStream.CopyTo(decryptedFileStream);
            }
        }
    }
}