// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.GM;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.X509;

namespace Admin.NET.Core;

/**
 *
 * 用BC的注意点：
 * 这个版本的BC对SM3withSM2的结果为asn1格式的r和s，如果需要直接拼接的r||s需要自己转换。下面rsAsn1ToPlainByteArray、rsPlainByteArrayToAsn1就在干这事。
 * 这个版本的BC对SM2的结果为C1||C2||C3，据说为旧标准，新标准为C1||C3||C2，用新标准的需要自己转换。下面（被注释掉的）changeC1C2C3ToC1C3C2、changeC1C3C2ToC1C2C3就在干这事。java版的高版本有加上C1C3C2，csharp版没准以后也会加，但目前还没有，java版的目前可以初始化时“ SM2Engine sm2Engine = new SM2Engine(SM2Engine.Mode.C1C3C2);”。
 *
 */

public class GM
{
    private static X9ECParameters x9ECParameters = GMNamedCurves.GetByName("sm2p256v1");
    private static ECDomainParameters ecDomainParameters = new(x9ECParameters.Curve, x9ECParameters.G, x9ECParameters.N);

    /**
     *
     * @param msg
     * @param userId
     * @param privateKey
     * @return r||s，直接拼接byte数组的rs
     */

    public static byte[] SignSm3WithSm2(byte[] msg, byte[] userId, AsymmetricKeyParameter privateKey)
    {
        return RsAsn1ToPlainByteArray(SignSm3WithSm2Asn1Rs(msg, userId, privateKey));
    }

    /**
      * @param msg
      * @param userId
      * @param privateKey
      * @return rs in <b>asn1 format</b>
      */

    public static byte[] SignSm3WithSm2Asn1Rs(byte[] msg, byte[] userId, AsymmetricKeyParameter privateKey)
    {
        ISigner signer = SignerUtilities.GetSigner("SM3withSM2");
        signer.Init(true, new ParametersWithID(privateKey, userId));
        signer.BlockUpdate(msg, 0, msg.Length);
        byte[] sig = signer.GenerateSignature();
        return sig;
    }

    /**
    *
    * @param msg
    * @param userId
    * @param rs r||s，直接拼接byte数组的rs
    * @param publicKey
    * @return
    */

    public static bool VerifySm3WithSm2(byte[] msg, byte[] userId, byte[] rs, AsymmetricKeyParameter publicKey)
    {
        if (rs == null || msg == null || userId == null) return false;
        if (rs.Length != RS_LEN * 2) return false;
        return VerifySm3WithSm2Asn1Rs(msg, userId, RsPlainByteArrayToAsn1(rs), publicKey);
    }

    /**
     *
     * @param msg
     * @param userId
     * @param rs in <b>asn1 format</b>
     * @param publicKey
     * @return
     */

    public static bool VerifySm3WithSm2Asn1Rs(byte[] msg, byte[] userId, byte[] sign, AsymmetricKeyParameter publicKey)
    {
        ISigner signer = SignerUtilities.GetSigner("SM3withSM2");
        signer.Init(false, new ParametersWithID(publicKey, userId));
        signer.BlockUpdate(msg, 0, msg.Length);
        return signer.VerifySignature(sign);
    }

    /**
     * bc加解密使用旧标c1||c2||c3，此方法在加密后调用，将结果转化为c1||c3||c2
     * @param c1c2c3
     * @return
     */

    private static byte[] ChangeC1C2C3ToC1C3C2(byte[] c1c2c3)
    {
        int c1Len = (x9ECParameters.Curve.FieldSize + 7) / 8 * 2 + 1; //sm2p256v1的这个固定65。可看GMNamedCurves、ECCurve代码。
        const int c3Len = 32; //new SM3Digest().getDigestSize();
        byte[] result = new byte[c1c2c3.Length];
        Buffer.BlockCopy(c1c2c3, 0, result, 0, c1Len); //c1
        Buffer.BlockCopy(c1c2c3, c1c2c3.Length - c3Len, result, c1Len, c3Len); //c3
        Buffer.BlockCopy(c1c2c3, c1Len, result, c1Len + c3Len, c1c2c3.Length - c1Len - c3Len); //c2
        return result;
    }

    /**
     * bc加解密使用旧标c1||c3||c2，此方法在解密前调用，将密文转化为c1||c2||c3再去解密
     * @param c1c3c2
     * @return
     */

    private static byte[] ChangeC1C3C2ToC1C2C3(byte[] c1c3c2)
    {
        int c1Len = (x9ECParameters.Curve.FieldSize + 7) / 8 * 2 + 1; //sm2p256v1的这个固定65。可看GMNamedCurves、ECCurve代码。
        const int c3Len = 32; //new SM3Digest().GetDigestSize();
        byte[] result = new byte[c1c3c2.Length];
        Buffer.BlockCopy(c1c3c2, 0, result, 0, c1Len); //c1: 0->65
        Buffer.BlockCopy(c1c3c2, c1Len + c3Len, result, c1Len, c1c3c2.Length - c1Len - c3Len); //c2
        Buffer.BlockCopy(c1c3c2, c1Len, result, c1c3c2.Length - c3Len, c3Len); //c3
        return result;
    }

    /**
     * c1||c3||c2
     * @param data
     * @param key
     * @return
     */

    public static byte[] Sm2Decrypt(byte[] data, AsymmetricKeyParameter key)
    {
        return Sm2DecryptOld(ChangeC1C3C2ToC1C2C3(data), key);
    }

    /**
     * c1||c3||c2
     * @param data
     * @param key
     * @return
     */

    public static byte[] Sm2Encrypt(byte[] data, AsymmetricKeyParameter key)
    {
        return ChangeC1C2C3ToC1C3C2(Sm2EncryptOld(data, key));
    }

    /**
     * c1||c2||c3
     * @param data
     * @param key
     * @return
     */

    public static byte[] Sm2EncryptOld(byte[] data, AsymmetricKeyParameter pubkey)
    {
        SM2Engine sm2Engine = new SM2Engine();
        sm2Engine.Init(true, new ParametersWithRandom(pubkey, new SecureRandom()));
        return sm2Engine.ProcessBlock(data, 0, data.Length);
    }

    /**
     * c1||c2||c3
     * @param data
     * @param key
     * @return
     */

    public static byte[] Sm2DecryptOld(byte[] data, AsymmetricKeyParameter key)
    {
        SM2Engine sm2Engine = new SM2Engine();
        sm2Engine.Init(false, key);
        return sm2Engine.ProcessBlock(data, 0, data.Length);
    }

    /**
     * @param bytes
     * @return
     */

    public static byte[] Sm3(byte[] bytes)
    {
        SM3Digest digest = new();
        digest.BlockUpdate(bytes, 0, bytes.Length);
        byte[] result = DigestUtilities.DoFinal(digest);
        return result;
    }

    private const int RS_LEN = 32;

    private static byte[] BigIntToFixexLengthBytes(BigInteger rOrS)
    {
        // for sm2p256v1, n is 00fffffffeffffffffffffffffffffffff7203df6b21c6052b53bbf40939d54123,
        // r and s are the result of mod n, so they should be less than n and have length<=32
        byte[] rs = rOrS.ToByteArray();
        if (rs.Length == RS_LEN) return rs;
        else if (rs.Length == RS_LEN + 1 && rs[0] == 0) return Arrays.CopyOfRange(rs, 1, RS_LEN + 1);
        else if (rs.Length < RS_LEN)
        {
            byte[] result = new byte[RS_LEN];
            Arrays.Fill(result, (byte)0);
            Buffer.BlockCopy(rs, 0, result, RS_LEN - rs.Length, rs.Length);
            return result;
        }
        else
        {
            throw new ArgumentException("err rs: " + Hex.ToHexString(rs));
        }
    }

    /**
     * BC的SM3withSM2签名得到的结果的rs是asn1格式的，这个方法转化成直接拼接r||s
     * @param rsDer rs in asn1 format
     * @return sign result in plain byte array
     */

    private static byte[] RsAsn1ToPlainByteArray(byte[] rsDer)
    {
        Asn1Sequence seq = Asn1Sequence.GetInstance(rsDer);
        byte[] r = BigIntToFixexLengthBytes(DerInteger.GetInstance(seq[0]).Value);
        byte[] s = BigIntToFixexLengthBytes(DerInteger.GetInstance(seq[1]).Value);
        byte[] result = new byte[RS_LEN * 2];
        Buffer.BlockCopy(r, 0, result, 0, r.Length);
        Buffer.BlockCopy(s, 0, result, RS_LEN, s.Length);
        return result;
    }

    /**
     * BC的SM3withSM2验签需要的rs是asn1格式的，这个方法将直接拼接r||s的字节数组转化成asn1格式
     * @param sign in plain byte array
     * @return rs result in asn1 format
     */

    private static byte[] RsPlainByteArrayToAsn1(byte[] sign)
    {
        if (sign.Length != RS_LEN * 2) throw new ArgumentException("err rs. ");
        BigInteger r = new BigInteger(1, Arrays.CopyOfRange(sign, 0, RS_LEN));
        BigInteger s = new BigInteger(1, Arrays.CopyOfRange(sign, RS_LEN, RS_LEN * 2));
        Asn1EncodableVector v = new Asn1EncodableVector
        {
            new DerInteger(r),
            new DerInteger(s)
        };

        return new DerSequence(v).GetEncoded("DER");
    }

    public static AsymmetricCipherKeyPair GenerateKeyPair()
    {
        ECKeyPairGenerator kpGen = new();
        kpGen.Init(new ECKeyGenerationParameters(ecDomainParameters, new SecureRandom()));
        return kpGen.GenerateKeyPair();
    }

    public static ECPrivateKeyParameters GetPrivatekeyFromD(BigInteger d)
    {
        return new ECPrivateKeyParameters(d, ecDomainParameters);
    }

    public static ECPublicKeyParameters GetPublickeyFromXY(BigInteger x, BigInteger y)
    {
        return new ECPublicKeyParameters(x9ECParameters.Curve.CreatePoint(x, y), ecDomainParameters);
    }

    public static AsymmetricKeyParameter GetPublickeyFromX509File(FileInfo file)
    {
        FileStream fileStream = null;
        try
        {
            //file.DirectoryName + "\\" + file.Name
            fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
            X509Certificate certificate = new X509CertificateParser().ReadCertificate(fileStream);
            return certificate.GetPublicKey();
        }
        catch (Exception e)
        {
            //log.Error(file.Name + "读取失败，异常：" + e);
        }
        finally
        {
            if (fileStream != null)
                fileStream.Close();
        }
        return null;
    }

    public class Sm2Cert
    {
        public AsymmetricKeyParameter privateKey;
        public AsymmetricKeyParameter publicKey;
        public string certId;
    }

    private static byte[] ToByteArray(int i)
    {
        byte[] byteArray = new byte[4];
        byteArray[0] = (byte)(i >> 24);
        byteArray[1] = (byte)((i & 0xFFFFFF) >> 16);
        byteArray[2] = (byte)((i & 0xFFFF) >> 8);
        byteArray[3] = (byte)(i & 0xFF);
        return byteArray;
    }

    /**
     * 字节数组拼接
     *
     * @param params
     * @return
     */

    private static byte[] Join(params byte[][] byteArrays)
    {
        List<byte> byteSource = new();
        for (int i = 0; i < byteArrays.Length; i++)
        {
            byteSource.AddRange(byteArrays[i]);
        }
        byte[] data = byteSource.ToArray();
        return data;
    }

    /**
     * 密钥派生函数
     *
     * @param Z
     * @param klen
     *            生成klen字节数长度的密钥
     * @return
     */

    private static byte[] KDF(byte[] Z, int klen)
    {
        int ct = 1;
        int end = (int)Math.Ceiling(klen * 1.0 / 32);
        List<byte> byteSource = new();

        for (int i = 1; i < end; i++)
        {
            byteSource.AddRange(Sm3(Join(Z, ToByteArray(ct))));
            ct++;
        }
        byte[] last = Sm3(Join(Z, ToByteArray(ct)));
        if (klen % 32 == 0)
        {
            byteSource.AddRange(last);
        }
        else
            byteSource.AddRange(Arrays.CopyOfRange(last, 0, klen % 32));
        return byteSource.ToArray();
    }

    public static byte[] Sm4DecryptCBC(byte[] keyBytes, byte[] cipher, byte[] iv, string algo)
    {
        if (keyBytes.Length != 16) throw new ArgumentException("err key length");
        if (cipher.Length % 16 != 0 && algo.Contains("NoPadding")) throw new ArgumentException("err data length");

        KeyParameter key = ParameterUtilities.CreateKeyParameter("SM4", keyBytes);
        IBufferedCipher c = CipherUtilities.GetCipher(algo);
        if (iv == null) iv = ZeroIv(algo);
        c.Init(false, new ParametersWithIV(key, iv));
        return c.DoFinal(cipher);
    }

    public static byte[] Sm4EncryptCBC(byte[] keyBytes, byte[] plain, byte[] iv, string algo)
    {
        if (keyBytes.Length != 16) throw new ArgumentException("err key length");
        if (plain.Length % 16 != 0 && algo.Contains("NoPadding")) throw new ArgumentException("err data length");

        KeyParameter key = ParameterUtilities.CreateKeyParameter("SM4", keyBytes);
        IBufferedCipher c = CipherUtilities.GetCipher(algo);
        if (iv == null) iv = ZeroIv(algo);
        c.Init(true, new ParametersWithIV(key, iv));
        return c.DoFinal(plain);
    }

    public static byte[] Sm4EncryptECB(byte[] keyBytes, byte[] plain, string algo)
    {
        if (keyBytes.Length != 16) throw new ArgumentException("err key length");
        //NoPadding 的情况下需要校验数据长度是16的倍数.
        if (plain.Length % 16 != 0 && algo.Contains("NoPadding")) throw new ArgumentException("err data length");

        KeyParameter key = ParameterUtilities.CreateKeyParameter("SM4", keyBytes);
        IBufferedCipher c = CipherUtilities.GetCipher(algo);
        c.Init(true, key);
        return c.DoFinal(plain);
    }

    public static byte[] Sm4DecryptECB(byte[] keyBytes, byte[] cipher, string algo)
    {
        if (keyBytes.Length != 16) throw new ArgumentException("err key length");
        if (cipher.Length % 16 != 0 && algo.Contains("NoPadding")) throw new ArgumentException("err data length");

        KeyParameter key = ParameterUtilities.CreateKeyParameter("SM4", keyBytes);
        IBufferedCipher c = CipherUtilities.GetCipher(algo);
        c.Init(false, key);
        return c.DoFinal(cipher);
    }

    public const string SM4_ECB_NOPADDING = "SM4/ECB/NoPadding";
    public const string SM4_CBC_NOPADDING = "SM4/CBC/NoPadding";
    public const string SM4_CBC_PKCS7PADDING = "SM4/CBC/PKCS7Padding";

    /**
     * cfca官网CSP沙箱导出的sm2文件
     * @param pem 二进制原文
     * @param pwd 密码
     * @return
     */

    public static Sm2Cert ReadSm2File(byte[] pem, string pwd)
    {
        Sm2Cert sm2Cert = new();

        Asn1Sequence asn1Sequence = (Asn1Sequence)Asn1Object.FromByteArray(pem);
        //            ASN1Integer asn1Integer = (ASN1Integer) asn1Sequence.getObjectAt(0); //version=1
        Asn1Sequence priSeq = (Asn1Sequence)asn1Sequence[1];//private key
        Asn1Sequence pubSeq = (Asn1Sequence)asn1Sequence[2];//public key and x509 cert

        //            ASN1ObjectIdentifier sm2DataOid = (ASN1ObjectIdentifier) priSeq.getObjectAt(0);
        //            ASN1ObjectIdentifier sm4AlgOid = (ASN1ObjectIdentifier) priSeq.getObjectAt(1);
        Asn1OctetString priKeyAsn1 = (Asn1OctetString)priSeq[2];
        byte[] key = KDF(System.Text.Encoding.UTF8.GetBytes(pwd), 32);
        byte[] priKeyD = Sm4DecryptCBC(Arrays.CopyOfRange(key, 16, 32),
                priKeyAsn1.GetOctets(),
                Arrays.CopyOfRange(key, 0, 16), SM4_CBC_PKCS7PADDING);
        sm2Cert.privateKey = GetPrivatekeyFromD(new BigInteger(1, priKeyD));
        //            log.Info(Hex.toHexString(priKeyD));

        //            ASN1ObjectIdentifier sm2DataOidPub = (ASN1ObjectIdentifier) pubSeq.getObjectAt(0);
        Asn1OctetString pubKeyX509 = (Asn1OctetString)pubSeq[1];
        X509Certificate x509 = new X509CertificateParser().ReadCertificate(pubKeyX509.GetOctets());
        sm2Cert.publicKey = x509.GetPublicKey();
        sm2Cert.certId = x509.SerialNumber.ToString(10); //这里转10进账，有啥其他进制要求的自己改改
        return sm2Cert;
    }

    /**
     *
     * @param cert
     * @return
     */

    public static Sm2Cert ReadSm2X509Cert(byte[] cert)
    {
        Sm2Cert sm2Cert = new();

        X509Certificate x509 = new X509CertificateParser().ReadCertificate(cert);
        sm2Cert.publicKey = x509.GetPublicKey();
        sm2Cert.certId = x509.SerialNumber.ToString(10); //这里转10进账，有啥其他进制要求的自己改改
        return sm2Cert;
    }

    public static byte[] ZeroIv(string algo)
    {
        IBufferedCipher cipher = CipherUtilities.GetCipher(algo);
        int blockSize = cipher.GetBlockSize();
        byte[] iv = new byte[blockSize];
        Arrays.Fill(iv, (byte)0);
        return iv;
    }
}