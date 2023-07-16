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

namespace Admin.NET.Core;

public class Cipher
{
    private int ct;
    private ECPoint p2;
    private SM3Digest sm3keybase;
    private SM3Digest sm3c3;
    private readonly byte[] key;
    private byte keyOff;

    public Cipher()
    {
        ct = 1;
        key = new byte[32];
        keyOff = 0;
    }

    public static byte[] ByteConvert32Bytes(BigInteger n)
    {
        if (n == null)
            return null;

        byte[] tmpd;
        if (n.ToByteArray().Length == 33)
        {
            tmpd = new byte[32];
            Array.Copy(n.ToByteArray(), 1, tmpd, 0, 32);
        }
        else if (n.ToByteArray().Length == 32)
        {
            tmpd = n.ToByteArray();
        }
        else
        {
            tmpd = new byte[32];
            for (int i = 0; i < 32 - n.ToByteArray().Length; i++)
            {
                tmpd[i] = 0;
            }
            Array.Copy(n.ToByteArray(), 0, tmpd, 32 - n.ToByteArray().Length, n.ToByteArray().Length);
        }
        return tmpd;
    }

    private void Reset()
    {
        sm3keybase = new SM3Digest();
        sm3c3 = new SM3Digest();

        byte[] p = ByteConvert32Bytes(p2.Normalize().XCoord.ToBigInteger());
        sm3keybase.BlockUpdate(p, 0, p.Length);
        sm3c3.BlockUpdate(p, 0, p.Length);

        p = ByteConvert32Bytes(p2.Normalize().YCoord.ToBigInteger());
        sm3keybase.BlockUpdate(p, 0, p.Length);
        ct = 1;
        NextKey();
    }

    private void NextKey()
    {
        var sm3keycur = new SM3Digest(this.sm3keybase);
        sm3keycur.Update((byte)(ct >> 24 & 0xff));
        sm3keycur.Update((byte)(ct >> 16 & 0xff));
        sm3keycur.Update((byte)(ct >> 8 & 0xff));
        sm3keycur.Update((byte)(ct & 0xff));
        sm3keycur.DoFinal(key, 0);
        keyOff = 0;
        ct++;
    }

    public ECPoint Init_enc(SM2 sm2, ECPoint userKey)
    {
        AsymmetricCipherKeyPair key = sm2.ecc_key_pair_generator.GenerateKeyPair();
        ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)key.Private;
        ECPublicKeyParameters ecpub = (ECPublicKeyParameters)key.Public;
        BigInteger k = ecpriv.D;
        ECPoint c1 = ecpub.Q;
        p2 = userKey.Multiply(k);
        Reset();
        return c1;
    }

    public void Encrypt(byte[] data)
    {
        sm3c3.BlockUpdate(data, 0, data.Length);
        for (int i = 0; i < data.Length; i++)
        {
            if (keyOff == key.Length)
            {
                NextKey();
            }
            data[i] ^= key[keyOff++];
        }
    }

    public void Init_dec(BigInteger userD, ECPoint c1)
    {
        p2 = c1.Multiply(userD);
        Reset();
    }

    public void Decrypt(byte[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (keyOff == key.Length)
            {
                NextKey();
            }
            data[i] ^= key[keyOff++];
        }

        sm3c3.BlockUpdate(data, 0, data.Length);
    }

    public void Dofinal(byte[] c3)
    {
        byte[] p = ByteConvert32Bytes(p2.Normalize().YCoord.ToBigInteger());
        sm3c3.BlockUpdate(p, 0, p.Length);
        sm3c3.DoFinal(c3, 0);
        Reset();
    }
}