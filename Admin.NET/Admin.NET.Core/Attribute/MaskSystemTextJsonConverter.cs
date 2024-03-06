// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Admin.NET.Core;

/// <summary>
/// 字符串掩码
/// </summary>
[SuppressSniffer]
public class MaskSystemTextJsonConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString().Mask());
    }
}

/// <summary>
/// 身份证掩码
/// </summary>
[SuppressSniffer]
public class MaskIdCardSystemTextJsonConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString().MaskIdCard());
    }
}

/// <summary>
/// 邮箱掩码
/// </summary>
[SuppressSniffer]
public class MaskEmailSystemTextJsonConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString().MaskEmail());
    }
}

/// <summary>
/// 银行卡号掩码
/// </summary>
[SuppressSniffer]
public class MaskBankCardSystemTextJsonConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString().MaskBankCard());
    }
}