// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;

namespace Admin.NET.Core;

public class SM2
{
    public static SM2 Instance
    {
        get
        {
            return new SM2();
        }
    }

    public static SM2 InstanceTest
    {
        get
        {
            return new SM2();
        }
    }

    public static readonly string[] sm2_param = {
        "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFF",// p,0
        "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFC",// a,1
        "28E9FA9E9D9F5E344D5A9E4BCF6509A7F39789F515AB8F92DDBCBD414D940E93",// b,2
        "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFF7203DF6B21C6052B53BBF40939D54123",// n,3
        "32C4AE2C1F1981195F9904466A39C9948FE30BBFF2660BE1715A4589334C74C7",// gx,4
        "BC3736A2F4F6779C59BDCEE36B692153D0A9877CC62A474002DF32E52139F0A0" // gy,5
    };

    public string[] ecc_param = sm2_param;

    public readonly BigInteger ecc_p;
    public readonly BigInteger ecc_a;
    public readonly BigInteger ecc_b;
    public readonly BigInteger ecc_n;
    public readonly BigInteger ecc_gx;
    public readonly BigInteger ecc_gy;

    public readonly ECCurve ecc_curve;
    public readonly ECPoint ecc_point_g;

    public readonly ECDomainParameters ecc_bc_spec;

    public readonly ECKeyPairGenerator ecc_key_pair_generator;

    private SM2()
    {
        ecc_param = sm2_param;

        ecc_p = new BigInteger(ecc_param[0], 16);
        ecc_a = new BigInteger(ecc_param[1], 16);
        ecc_b = new BigInteger(ecc_param[2], 16);
        ecc_n = new BigInteger(ecc_param[3], 16);
        ecc_gx = new BigInteger(ecc_param[4], 16);
        ecc_gy = new BigInteger(ecc_param[5], 16);

        ecc_curve = new FpCurve(ecc_p, ecc_a, ecc_b, null, null);
        ecc_point_g = ecc_curve.CreatePoint(ecc_gx, ecc_gy);

        ecc_bc_spec = new ECDomainParameters(ecc_curve, ecc_point_g, ecc_n);

        ECKeyGenerationParameters ecc_ecgenparam;
        ecc_ecgenparam = new ECKeyGenerationParameters(ecc_bc_spec, new SecureRandom());

        ecc_key_pair_generator = new ECKeyPairGenerator();
        ecc_key_pair_generator.Init(ecc_ecgenparam);
    }

    public virtual byte[] Sm2GetZ(byte[] userId, ECPoint userKey)
    {
        var sm3 = new SM3Digest();
        byte[] p;
        // userId length
        int len = userId.Length * 8;
        sm3.Update((byte)(len >> 8 & 0x00ff));
        sm3.Update((byte)(len & 0x00ff));

        // userId
        sm3.BlockUpdate(userId, 0, userId.Length);

        // a,b
        p = ecc_a.ToByteArray();
        sm3.BlockUpdate(p, 0, p.Length);
        p = ecc_b.ToByteArray();
        sm3.BlockUpdate(p, 0, p.Length);
        // gx,gy
        p = ecc_gx.ToByteArray();
        sm3.BlockUpdate(p, 0, p.Length);
        p = ecc_gy.ToByteArray();
        sm3.BlockUpdate(p, 0, p.Length);

        // x,y
        p = userKey.AffineXCoord.ToBigInteger().ToByteArray();
        sm3.BlockUpdate(p, 0, p.Length);
        p = userKey.AffineYCoord.ToBigInteger().ToByteArray();
        sm3.BlockUpdate(p, 0, p.Length);

        // Z
        byte[] md = new byte[sm3.GetDigestSize()];
        sm3.DoFinal(md, 0);

        return md;
    }
}