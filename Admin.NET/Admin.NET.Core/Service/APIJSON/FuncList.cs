// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using NewLife.Reflection;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Org.BouncyCastle.Asn1.X509.Qualified;

namespace Admin.NET.Core.Service;

/// <summary>
/// 自定义方法
/// </summary>
public class FuncList
{
    /// <summary>
    /// 字符串相加
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public string Merge(object a, object b)
    {
        return a.ToString() + b.ToString();
    }

    /// <summary>
    /// 对象合并
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public object MergeObj(object a, object b)
    {
        return new { a, b };
    }

    /// <summary>
    /// 是否包含
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool IsContain(object a, object b)
    {
        return a.ToString().Split(',').Contains(b);
    }
   

    /// <summary>
    /// 根据jtoken的实际类型来转换SugarParameter，避免全转成字符串
    /// </summary>
    /// <param name="jToken"></param>
    /// <returns></returns>
    public static dynamic TransJObjectToSugarPara(JToken jToken)
    {
        JTokenType jTokenType = jToken.Type;
        return jTokenType switch
        {
            JTokenType.Integer => jToken.ToObject(typeof(long)),
            JTokenType.Float => jToken.ToObject(typeof(decimal)),
            JTokenType.Boolean => jToken.ToObject(typeof(bool)),
            JTokenType.Date => jToken.ToObject(typeof(DateTime)),
            JTokenType.Bytes => jToken.ToObject(typeof(byte)),
            JTokenType.Guid => jToken.ToObject(typeof(Guid)),
            JTokenType.TimeSpan => jToken.ToObject(typeof(TimeSpan)),
            JTokenType.Array => TransJArrayToSugarPara(jToken),
            _ => jToken
        };
    }

    /// <summary>
    /// 根据jArray的实际类型来转换SugarParameter，避免全转成字符串
    /// </summary>
    /// <param name="jToken"></param>
    /// <returns></returns>
    public static dynamic TransJArrayToSugarPara(JToken jToken)
    {
        if (jToken is not JArray) return jToken;
        if (jToken.Any())
        {
            JTokenType jTokenType = jToken.First().Type;
            return jTokenType switch
            {
                JTokenType.Integer => jToken.ToObject<long[]>(),
                JTokenType.Float => jToken.ToObject<decimal[]>(),
                JTokenType.Boolean => jToken.ToObject<bool[]>(),
                JTokenType.Date => jToken.ToObject<DateTime[]>(),
                JTokenType.Bytes => jToken.ToObject<byte[]>(),
                JTokenType.Guid => jToken.ToObject<Guid[]>(),
                JTokenType.TimeSpan => jToken.ToObject<TimeSpan[]>(),
                _ => jToken.ToArray()
            } ;
        }
        else return (JArray)jToken;

    }

    /// <summary>
    /// 获取字符串里的值的真正类型
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string GetValueCSharpType(string input)
    {
        if (DateTime.TryParse(input, out _))
        {
            return "DateTime";
        }
        else if (int.TryParse(input, out _))
        {
            return "int";
        }
        else if (long.TryParse(input, out _))
        {
            return "long";
        }
        else if (decimal.TryParse(input, out _))
        {
            return "decimal";
        }
        else if (bool.TryParse(input, out _))
        {
            return "bool";
        }
        else
        {
            return "string";
        }
    }
}