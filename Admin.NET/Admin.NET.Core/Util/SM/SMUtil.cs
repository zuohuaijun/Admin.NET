using Org.BouncyCastle.Utilities.Encoders;

namespace Admin.NET.Core;

/// <summary>
/// SM工具类
/// </summary>
public class SMUtil
{
    public static string PublicKey = App.GetConfig<string>("Crypto:PublicKey"); // 公钥
    public static string PrivateKey = App.GetConfig<string>("Crypto:PrivateKey"); // 私钥

    /// <summary>
    /// SM2加密
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string SM2Encrypt(string plainText)
    {
        byte[] sourceData = Encoding.Default.GetBytes(plainText);
        return SM2Util.Encrypt(Hex.Decode(PublicKey), sourceData);
    }

    /// <summary>
    /// SM2解密
    /// </summary>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public static string SM2Decrypt(string cipherText)
    {
        return Encoding.Default.GetString(SM2Util.Decrypt(Hex.Decode(PrivateKey), Hex.Decode(cipherText)));
    }

    /// <summary>
    /// SM4加密（ECB）
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string SM4EncryptECB(string plainText)
    {
        var sm4 = new SM4Util();
        return sm4.Encrypt_ECB(plainText);
    }

    /// <summary>
    /// SM4解密（ECB）
    /// </summary>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public static string SM4DecryptECB(string cipherText)
    {
        var sm4 = new SM4Util();
        return sm4.Decrypt_ECB(cipherText);
    }

    /// <summary>
    /// SM4加密（CBC）
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string SM4EncryptCBC(string plainText)
    {
        var sm4 = new SM4Util();
        return sm4.Encrypt_CBC(plainText);
    }

    /// <summary>
    /// SM4解密（CBC）
    /// </summary>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public static string SM4DecryptCBC(string cipherText)
    {
        var sm4 = new SM4Util();
        return sm4.Decrypt_CBC(cipherText);
    }

    public static string StrToHex(string str)
    {
        return Encoding.ASCII.GetString(Hex.Encode(Encoding.UTF8.GetBytes(str))).ToUpper();
    }
}