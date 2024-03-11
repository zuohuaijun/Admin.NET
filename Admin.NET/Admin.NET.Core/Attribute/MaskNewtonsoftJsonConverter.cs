// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using Newtonsoft.Json;

namespace Admin.NET.Core;

/// <summary>
/// 字符串掩码
/// </summary>
[SuppressSniffer]
public class MaskNewtonsoftJsonConverter : JsonConverter<string>
{
    public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return reader.Value.ToString();
    }

    public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.ToString().Mask());
    }
}

/// <summary>
/// 身份证掩码
/// </summary>
[SuppressSniffer]
public class MaskIdCardNewtonsoftJsonConverter : JsonConverter<string>
{
    public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return reader.Value.ToString();
    }

    public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.ToString().MaskIdCard());
    }
}

/// <summary>
/// 邮箱掩码
/// </summary>
[SuppressSniffer]
public class MaskEmailNewtonsoftJsonConverter : JsonConverter<string>
{
    public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return reader.Value.ToString();
    }

    public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.ToString().MaskEmail());
    }
}

/// <summary>
/// 银行卡号掩码
/// </summary>
[SuppressSniffer]
public class MaskBankCardNewtonsoftJsonConverter : JsonConverter<string>
{
    public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return reader.Value.ToString();
    }

    public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.ToString().MaskBankCard());
    }
}