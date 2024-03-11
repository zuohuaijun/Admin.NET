// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities.Encoders;

namespace Admin.NET.Core;

/// <summary>
/// GM工具类
/// </summary>
public class GMUtil
{
    /// <summary>
    /// SM2加密
    /// </summary>
    /// <param name="publicKeyHex"></param>
    /// <param name="data_string"></param>
    /// <returns></returns>
    public static string SM2Encrypt(string publicKeyHex, string data_string)
    {
        // 如果是130位公钥，.NET使用的话，把开头的04截取掉
        if (publicKeyHex.Length == 130)
        {
            publicKeyHex = publicKeyHex.Substring(2, 128);
        }
        // 公钥X，前64位
        string x = publicKeyHex.Substring(0, 64);
        // 公钥Y，后64位
        string y = publicKeyHex.Substring(64);
        // 获取公钥对象
        AsymmetricKeyParameter publicKey1 = GM.GetPublickeyFromXY(new BigInteger(x, 16), new BigInteger(y, 16));
        // Sm2Encrypt: C1C3C2
        // Sm2EncryptOld: C1C2C3
        byte[] digestByte = GM.Sm2Encrypt(Encoding.UTF8.GetBytes(data_string), publicKey1);
        string strSM2 = Hex.ToHexString(digestByte);
        return strSM2;
    }

    /// <summary>
    /// SM2解密
    /// </summary>
    /// <param name="privateKey_string"></param>
    /// <param name="encryptedData_string"></param>
    /// <returns></returns>
    public static string SM2Decrypt(string privateKey_string, string encryptedData_string)
    {
        if (!encryptedData_string.StartsWith("04"))
            encryptedData_string = "04" + encryptedData_string;
        BigInteger d = new(privateKey_string, 16);
        // 先拿到私钥对象，用ECPrivateKeyParameters 或 AsymmetricKeyParameter 都可以
        // ECPrivateKeyParameters bcecPrivateKey = GmUtil.GetPrivatekeyFromD(d);
        AsymmetricKeyParameter bcecPrivateKey = GM.GetPrivatekeyFromD(d);
        byte[] byToDecrypt = Hex.Decode(encryptedData_string);
        byte[] byDecrypted = GM.Sm2Decrypt(byToDecrypt, bcecPrivateKey);
        string strDecrypted = Encoding.UTF8.GetString(byDecrypted);
        return strDecrypted;
    }

    /// <summary>
    /// SM4加密（ECB）
    /// </summary>
    /// <param name="key_string"></param>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string SM4EncryptECB(string key_string, string plainText)
    {
        byte[] key = Hex.Decode(key_string);
        byte[] bs = GM.Sm4EncryptECB(key, Encoding.UTF8.GetBytes(plainText), GM.SM4_CBC_PKCS7PADDING);//NoPadding 的情况下需要校验数据长度是16的倍数. 使用 HandleSm4Padding 处理
        return Hex.ToHexString(bs);
    }

    /// <summary>
    /// SM4解密（ECB）
    /// </summary>
    /// <param name="key_string"></param>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public static string SM4DecryptECB(string key_string, string cipherText)
    {
        byte[] key = Hex.Decode(key_string);
        byte[] bs = GM.Sm4DecryptECB(key, Hex.Decode(cipherText), GM.SM4_CBC_PKCS7PADDING);
        return Encoding.UTF8.GetString(bs);
    }

    /// <summary>
    /// SM4加密（CBC）
    /// </summary>
    /// <param name="key_string"></param>
    /// <param name="iv_string"></param>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string SM4EncryptCBC(string key_string, string iv_string, string plainText)
    {
        byte[] key = Hex.Decode(key_string);
        byte[] iv = Hex.Decode(iv_string);
        byte[] bs = GM.Sm4EncryptCBC(key, Encoding.UTF8.GetBytes(plainText), iv, GM.SM4_CBC_PKCS7PADDING);
        return Hex.ToHexString(bs);
    }

    /// <summary>
    /// SM4解密（CBC）
    /// </summary>
    /// <param name="key_string"></param>
    /// <param name="iv_string"></param>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public static string SM4DecryptCBC(string key_string, string iv_string, string cipherText)
    {
        byte[] key = Hex.Decode(key_string);
        byte[] iv = Hex.Decode(iv_string);
        byte[] bs = GM.Sm4DecryptCBC(key, Hex.Decode(cipherText), iv, GM.SM4_CBC_PKCS7PADDING);
        return Encoding.UTF8.GetString(bs);
    }

    /// <summary>
    /// 补足 16 进制字符串的 0 字符，返回不带 0x 的16进制字符串
    /// </summary>
    /// <param name="input"></param>
    /// <param name="mode">1表示加密，0表示解密</param>
    /// <returns></returns>
    private static byte[] HandleSm4Padding(byte[] input, int mode)
    {
        if (input == null)
        {
            return null;
        }
        byte[] ret = (byte[])null;
        if (mode == 1)
        {
            int p = 16 - input.Length % 16;
            ret = new byte[input.Length + p];
            Array.Copy(input, 0, ret, 0, input.Length);
            for (int i = 0; i < p; i++)
            {
                ret[input.Length + i] = (byte)p;
            }
        }
        else
        {
            int p = input[input.Length - 1];
            ret = new byte[input.Length - p];
            Array.Copy(input, 0, ret, 0, input.Length - p);
        }
        return ret;
    }
}