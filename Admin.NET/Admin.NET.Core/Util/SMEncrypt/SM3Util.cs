using Org.BouncyCastle.Utilities.Encoders;

namespace Admin.NET.Core;

/// <summary>
/// SM3工具类
/// </summary>
public class SM3Util
{
    public string secretKey = "";

    public string 加密(string data)
    {
        byte[] msg1 = Encoding.Default.GetBytes(data);
        //byte[] key1 = Encoding.Default.GetBytes(secretKey);

        //var keyParameter = new KeyParameter(key1);
        var sm3 = new SM3Digest();

        //HMac mac = new HMac(sm3); // 带密钥的杂凑算法
        //mac.Init(keyParameter);
        sm3.BlockUpdate(msg1, 0, msg1.Length);
        // byte[] result = new byte[sm3.GetMacSize()];
        byte[] result = new byte[sm3.GetDigestSize()];
        sm3.DoFinal(result, 0);
        return Encoding.ASCII.GetString(Hex.Encode(result));
    }
}