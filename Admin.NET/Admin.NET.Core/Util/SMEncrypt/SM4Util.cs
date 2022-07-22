using Org.BouncyCastle.Utilities.Encoders;

namespace Admin.NET.Core;

/// <summary>
/// SM4工具类
/// </summary>
public class SM4Util
{
    public string secretKey = "";
    public string iv = "";
    public bool hexString = false;

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
        sm4.Sm4_setkey_dec(ctx, keyBytes);
        byte[] decrypted = sm4.Sm4_crypt_cbc(ctx, ivBytes, Hex.Decode(cipherText));
        return Encoding.ASCII.GetString(decrypted);
    }
}