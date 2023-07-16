// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities.Encoders;

namespace Admin.NET.Core;

/// <summary>
/// SM2工具类
/// </summary>
public class SM2Util
{
    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="publicKey_string"></param>
    /// <param name="data_string"></param>
    /// <returns></returns>
    public static string Encrypt(string publicKey_string, string data_string)
    {
        var publicKey = Hex.Decode(publicKey_string);
        var data = Encoding.UTF8.GetBytes(data_string);
        return Encrypt(publicKey, data);
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="privateKey_string"></param>
    /// <param name="encryptedData_string"></param>
    /// <returns></returns>
    public static string Decrypt(string privateKey_string, string encryptedData_string)
    {
        var privateKey = Hex.Decode(privateKey_string);
        var encryptedData = Hex.Decode(encryptedData_string);
        var de_str = SM2Util.Decrypt(privateKey, encryptedData);
        string plainText = Encoding.UTF8.GetString(de_str);
        return plainText;
    }

    public static void GenerateKeyPair()
    {
        SM2 sm2 = SM2.Instance;
        AsymmetricCipherKeyPair key = sm2.ecc_key_pair_generator.GenerateKeyPair();
        ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)key.Private;
        ECPublicKeyParameters ecpub = (ECPublicKeyParameters)key.Public;
        BigInteger privateKey = ecpriv.D;
        ECPoint publicKey = ecpub.Q;

        Console.Out.WriteLine("公钥: " + Encoding.ASCII.GetString(Hex.Encode(publicKey.GetEncoded())).ToUpper());
        Console.Out.WriteLine("私钥: " + Encoding.ASCII.GetString(Hex.Encode(privateKey.ToByteArray())).ToUpper());
    }

    public static string Encrypt(byte[] publicKey, byte[] data)
    {
        if (null == publicKey || publicKey.Length == 0)
        {
            return null;
        }
        if (data == null || data.Length == 0)
        {
            return null;
        }

        byte[] source = new byte[data.Length];
        Array.Copy(data, 0, source, 0, data.Length);

        var cipher = new Cipher();
        SM2 sm2 = SM2.Instance;

        ECPoint userKey = sm2.ecc_curve.DecodePoint(publicKey);

        ECPoint c1 = cipher.Init_enc(sm2, userKey);
        cipher.Encrypt(source);

        byte[] c3 = new byte[32];
        cipher.Dofinal(c3);

        string sc1 = Encoding.ASCII.GetString(Hex.Encode(c1.GetEncoded()));
        string sc2 = Encoding.ASCII.GetString(Hex.Encode(source));
        string sc3 = Encoding.ASCII.GetString(Hex.Encode(c3));

        return (sc1 + sc2 + sc3).ToUpper();
    }

    public static byte[] Decrypt(byte[] privateKey, byte[] encryptedData)
    {
        if (null == privateKey || privateKey.Length == 0)
        {
            return null;
        }
        if (encryptedData == null || encryptedData.Length == 0)
        {
            return null;
        }

        string data = Encoding.ASCII.GetString(Hex.Encode(encryptedData));

        byte[] c1Bytes = Hex.Decode(Encoding.ASCII.GetBytes(data.Substring(0, 130)));
        int c2Len = encryptedData.Length - 97;
        byte[] c2 = Hex.Decode(Encoding.ASCII.GetBytes(data.Substring(130, 2 * c2Len)));
        byte[] c3 = Hex.Decode(Encoding.ASCII.GetBytes(data.Substring(130 + 2 * c2Len, 64)));

        SM2 sm2 = SM2.Instance;
        var userD = new BigInteger(1, privateKey);

        ECPoint c1 = sm2.ecc_curve.DecodePoint(c1Bytes);
        var cipher = new Cipher();
        cipher.Init_dec(userD, c1);
        cipher.Decrypt(c2);
        cipher.Dofinal(c3);

        return c2;
    }

    //[STAThread]
    //public static void Main()
    //{
    //    GenerateKeyPair();

    //    String plainText = "ererfeiisgod";
    //    byte[] sourceData = Encoding.Default.GetBytes(plainText);

    //    //下面的秘钥可以使用generateKeyPair()生成的秘钥内容
    //    // 国密规范正式私钥
    //    String prik = "3690655E33D5EA3D9A4AE1A1ADD766FDEA045CDEAA43A9206FB8C430CEFE0D94";
    //    // 国密规范正式公钥
    //    String pubk = "04F6E0C3345AE42B51E06BF50B98834988D54EBC7460FE135A48171BC0629EAE205EEDE253A530608178A98F1E19BB737302813BA39ED3FA3C51639D7A20C7391A";

    //    System.Console.Out.WriteLine("加密: ");
    //    String cipherText = SM2Utils.Encrypt(Hex.Decode(pubk), sourceData);
    //    System.Console.Out.WriteLine(cipherText);
    //    System.Console.Out.WriteLine("解密: ");
    //    plainText = Encoding.Default.GetString(SM2Utils.Decrypt(Hex.Decode(prik), Hex.Decode(cipherText)));
    //    System.Console.Out.WriteLine(plainText);

    //    Console.ReadLine();
    //}

    /// <summary>
    /// SM2加密
    /// </summary>
    /// <param name="plainText">明文</param>
    /// <returns>密文</returns>
    public static String 加密(String plainText)
    {
        // 国密规范正式公钥
        String pubk = "04F6E0C3345AE42B51E06BF50B98834988D54EBC7460FE135A48171BC0629EAE205EEDE253A530608178A98F1E19BB737302813BA39ED3FA3C51639D7A20C7391A";
        byte[] sourceData = Encoding.Default.GetBytes(plainText);
        String cipherText = SM2Util.Encrypt(Hex.Decode(pubk), sourceData);
        return cipherText;
    }

    /// <summary>
    /// SM2解密
    /// </summary>
    /// <param name="cipherText">密文</param>
    /// <returns>明文</returns>
    public static string 解密(String cipherText)
    {
        // 国密规范正式私钥
        String prik = "3690655E33D5EA3D9A4AE1A1ADD766FDEA045CDEAA43A9206FB8C430CEFE0D94";
        String plainText = Encoding.Default.GetString(SM2Util.Decrypt(Hex.Decode(prik), Hex.Decode(cipherText)));
        return plainText;
    }
}