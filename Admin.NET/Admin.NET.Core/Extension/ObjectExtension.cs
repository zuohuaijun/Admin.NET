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
    /// List转DataTable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static DataTable ToDataTable<T>(this List<T> list)
    {
        DataTable result = new();
        if (list.Count > 0)
        {
            // result.TableName = list[0].GetType().Name; // 表名赋值          
            PropertyInfo[] propertys = list[0].GetType().GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                Type colType = pi.PropertyType;
                if (colType.IsGenericType && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    colType = colType.GetGenericArguments()[0];
                }
                if (IsIgnoreColumn(pi))
                    continue;
                result.Columns.Add(pi.Name, colType);
            }
            for (int i = 0; i < list.Count; i++)
            {
                ArrayList tempList = new();
                foreach (PropertyInfo pi in propertys)
                {
                    if (IsIgnoreColumn(pi))
                        continue;
                    object obj = pi.GetValue(list[i], null);
                    tempList.Add(obj);
                }
                object[] array = tempList.ToArray();
                result.LoadDataRow(array, true);
            }
        }
        return result;
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
    /// 排除SqlSugar忽略的列
    /// </summary>
    /// <param name="pi"></param>
    /// <returns></returns>
    private static bool IsIgnoreColumn(PropertyInfo pi)
    {
        var sc = pi.GetCustomAttributes<SugarColumn>(false).FirstOrDefault(u => u.IsIgnore == true);
        return sc != null;
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
            if (str.Contains("-") || str.Contains("/"))
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
            if (str.Contains("-") || str.Contains("/"))
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
}