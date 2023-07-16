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