// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core;

/// <summary>
/// 对象拓展
/// </summary>
[SuppressSniffer]
public static partial class ObjectExtension
{
    /// <summary>
    /// 判断类型是否实现某个泛型
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="generic">泛型类型</param>
    /// <returns>bool</returns>
    public static bool HasImplementedRawGeneric(this Type type, Type generic)
    {
        // 检查接口类型
        var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
        if (isTheRawGenericType) return true;

        // 检查类型
        while (type != null && type != typeof(object))
        {
            isTheRawGenericType = IsTheRawGenericType(type);
            if (isTheRawGenericType) return true;
            type = type.BaseType;
        }

        return false;

        // 判断逻辑
        bool IsTheRawGenericType(Type type) => generic == (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
    }

    /// <summary>
    /// 将字典转化为QueryString格式
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="urlEncode"></param>
    /// <returns></returns>
    public static string ToQueryString(this Dictionary<string, string> dict, bool urlEncode = true)
    {
        return string.Join("&", dict.Select(p => $"{(urlEncode ? p.Key?.UrlEncode() : "")}={(urlEncode ? p.Value?.UrlEncode() : "")}"));
    }

    /// <summary>
    /// 将字符串URL编码
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string UrlEncode(this string str)
    {
        return string.IsNullOrEmpty(str) ? "" : System.Web.HttpUtility.UrlEncode(str, Encoding.UTF8);
    }

    /// <summary>
    /// 对象序列化成Json字符串
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToJson(this object obj)
    {
        return JSON.GetJsonSerializer().Serialize(obj);
    }

    /// <summary>
    /// Json字符串反序列化成对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T ToObject<T>(this string json)
    {
        return JSON.GetJsonSerializer().Deserialize<T>(json);
    }

    /// <summary>
    /// 将object转换为long，若失败则返回0
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static long ParseToLong(this object obj)
    {
        try
        {
            return long.Parse(obj.ToString());
        }
        catch
        {
            return 0L;
        }
    }

    /// <summary>
    /// 将object转换为long，若失败则返回指定值
    /// </summary>
    /// <param name="str"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static long ParseToLong(this string str, long defaultValue)
    {
        try
        {
            return long.Parse(str);
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// 将object转换为double，若失败则返回0
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static double ParseToDouble(this object obj)
    {
        try
        {
            return double.Parse(obj.ToString());
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// 将object转换为double，若失败则返回指定值
    /// </summary>
    /// <param name="str"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static double ParseToDouble(this object str, double defaultValue)
    {
        try
        {
            return double.Parse(str.ToString());
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// 将string转换为DateTime，若失败则返回日期最小值
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static DateTime ParseToDateTime(this string str)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return DateTime.MinValue;
            }
            if (str.Contains('-') || str.Contains('/'))
            {
                return DateTime.Parse(str);
            }
            else
            {
                int length = str.Length;
                switch (length)
                {
                    case 4:
                        return DateTime.ParseExact(str, "yyyy", System.Globalization.CultureInfo.CurrentCulture);

                    case 6:
                        return DateTime.ParseExact(str, "yyyyMM", System.Globalization.CultureInfo.CurrentCulture);

                    case 8:
                        return DateTime.ParseExact(str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

                    case 10:
                        return DateTime.ParseExact(str, "yyyyMMddHH", System.Globalization.CultureInfo.CurrentCulture);

                    case 12:
                        return DateTime.ParseExact(str, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);

                    case 14:
                        return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);

                    default:
                        return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                }
            }
        }
        catch
        {
            return DateTime.MinValue;
        }
    }

    /// <summary>
    /// 将string转换为DateTime，若失败则返回默认值
    /// </summary>
    /// <param name="str"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static DateTime ParseToDateTime(this string str, DateTime? defaultValue)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return defaultValue.GetValueOrDefault();
            }
            if (str.Contains('-') || str.Contains('/'))
            {
                return DateTime.Parse(str);
            }
            else
            {
                int length = str.Length;
                switch (length)
                {
                    case 4:
                        return DateTime.ParseExact(str, "yyyy", System.Globalization.CultureInfo.CurrentCulture);

                    case 6:
                        return DateTime.ParseExact(str, "yyyyMM", System.Globalization.CultureInfo.CurrentCulture);

                    case 8:
                        return DateTime.ParseExact(str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

                    case 10:
                        return DateTime.ParseExact(str, "yyyyMMddHH", System.Globalization.CultureInfo.CurrentCulture);

                    case 12:
                        return DateTime.ParseExact(str, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);

                    case 14:
                        return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);

                    default:
                        return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                }
            }
        }
        catch
        {
            return defaultValue.GetValueOrDefault();
        }
    }

    /// <summary>
    /// 判断是否有值
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this object obj)
    {
        return obj == null || string.IsNullOrEmpty(obj.ToString());
    }
}