using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Admin.NET.Core;

/// <summary>
/// 通用工具类
/// </summary>
public static class CommonUtil
{
    /// <summary>
    /// 生成百分数
    /// </summary>
    /// <param name="PassCount"></param>
    /// <param name="allCount"></param>
    /// <returns></returns>
    public static string ExecPercent(decimal PassCount, decimal allCount)
    {
        string res = "";
        if (allCount > 0)
        {
            var value = (double)Math.Round(PassCount / allCount * 100, 1);
            if (value < 0)
                res = Math.Round(value + 5 / Math.Pow(10, 0 + 1), 0, MidpointRounding.AwayFromZero).ToString();
            else
                res = Math.Round(value, 0, MidpointRounding.AwayFromZero).ToString();
        }
        if (res == "") res = "0";
        return res + "%";
    }

    /// <summary>
    /// 获取服务地址
    /// </summary>
    /// <returns></returns>
    public static string GetLocalhost()
    {
        return $"{App.HttpContext.Request.Scheme}://{App.HttpContext.Request.Host.Value}";
    }

    /// <summary>
    /// XML序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string XmlSerialize<T>(T obj)
    {
        if (obj == null) return "";

        var xs = new XmlSerializer(obj.GetType());
        var stream = new MemoryStream();
        var setting = new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(false),
            Indent = true
        };
        using (var writer = XmlWriter.Create(stream, setting))
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            xs.Serialize(writer, obj, ns);
        }
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    /// <summary>
    /// 字符串转XML格式
    /// </summary>
    /// <param name="xmlStr"></param>
    /// <returns></returns>
    public static XElement XmlParse(string xmlStr)
    {
        try
        {
            return XElement.Parse(xmlStr);
        }
        catch
        {
            return null;
        }
    }
}