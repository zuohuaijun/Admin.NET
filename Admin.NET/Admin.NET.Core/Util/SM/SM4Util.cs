// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Org.BouncyCastle.Utilities.Encoders;

namespace Admin.NET.Core;

/// <summary>
/// SM4工具类
/// </summary>
public class SM4Util
{
    public string secretKey = "1814546261730461"; // 长度必须为16字节
    public string iv = "0000000000000000";
    public bool hexString = false;
    public bool forJavascript = false;

    public string Encrypt_ECB(string plainText)
    {
        var ctx = new SM4_Context
        {
            isPadding = true,
            mode = SM4.SM4_ENCRYPT
        };

        byte[] keyBytes;
        if (hexString)
        {
            keyBytes = Hex.Decode(secretKey);
        }
        else
        {
            keyBytes = Encoding.ASCII.GetBytes(secretKey);
        }

        var sm4 = new SM4();
        sm4.FOR_JAVASCRIPT = forJavascript;

        sm4.Sm4_setkey_enc(ctx, keyBytes);
        byte[] encrypted = sm4.Sm4_crypt_ecb(ctx, Encoding.ASCII.GetBytes(plainText));

        string cipherText = Encoding.ASCII.GetString(Hex.Encode(encrypted));
        return cipherText;
    }

    public byte[] Encrypt_ECB(byte[] plainBytes, byte[] keyBytes)
    {
        var ctx = new SM4_Context
        {
            isPadding = false,
            mode = SM4.SM4_ENCRYPT
        };

        var sm4 = new SM4();

        sm4.FOR_JAVASCRIPT = forJavascript;

        sm4.Sm4_setkey_enc(ctx, keyBytes);
        byte[] encrypted = sm4.Sm4_crypt_ecb(ctx, plainBytes);
        return encrypted;

        //return Hex.Encode(encrypted);
    }

    public string Decrypt_ECB(string cipherText)
    {
        var ctx = new SM4_Context
        {
            isPadding = true,
            mode = SM4.SM4_DECRYPT
        };

        byte[] keyBytes;
        if (hexString)
        {
            keyBytes = Hex.Decode(secretKey);
        }
        else
        {
            keyBytes = Encoding.ASCII.GetBytes(secretKey);
        }

        var sm4 = new SM4();
        sm4.FOR_JAVASCRIPT = forJavascript;

        sm4.Sm4_setkey_dec(ctx, keyBytes);
        byte[] decrypted = sm4.Sm4_crypt_ecb(ctx, Hex.Decode(cipherText));
        return Encoding.ASCII.GetString(decrypted);
    }

    public string Encrypt_CBC(string plainText)
    {
        var ctx = new SM4_Context
        {
            isPadding = true,
            mode = SM4.SM4_ENCRYPT
        };

        byte[] keyBytes;
        byte[] ivBytes;
        if (hexString)
        {
            keyBytes = Hex.Decode(secretKey);
            ivBytes = Hex.Decode(iv);
        }
        else
        {
            keyBytes = Encoding.ASCII.GetBytes(secretKey);
            ivBytes = Encoding.ASCII.GetBytes(iv);
        }

        var sm4 = new SM4();
        sm4.FOR_JAVASCRIPT = forJavascript;
        sm4.Sm4_setkey_enc(ctx, keyBytes);
        byte[] encrypted = sm4.Sm4_crypt_cbc(ctx, ivBytes, Encoding.ASCII.GetBytes(plainText));

        string cipherText = Encoding.ASCII.GetString(Hex.Encode(encrypted));
        return cipherText;
    }

    public string Decrypt_CBC(string cipherText)
    {
        var ctx = new SM4_Context
        {
            isPadding = true,
            mode = SM4.SM4_DECRYPT
        };

        byte[] keyBytes;
        byte[] ivBytes;
        if (hexString)
        {
            keyBytes = Hex.Decode(secretKey);
            ivBytes = Hex.Decode(iv);
        }
        else
        {
            keyBytes = Encoding.ASCII.GetBytes(secretKey);
            ivBytes = Encoding.ASCII.GetBytes(iv);
        }

        var sm4 = new SM4();
        sm4.FOR_JAVASCRIPT = forJavascript;
        sm4.Sm4_setkey_dec(ctx, keyBytes);
        byte[] decrypted = sm4.Sm4_crypt_cbc(ctx, ivBytes, Hex.Decode(cipherText));
        return Encoding.ASCII.GetString(decrypted);
    }
}