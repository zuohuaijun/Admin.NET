using Newtonsoft.Json.Linq;

namespace Admin.NET.Core;

/// <summary>
/// 序列化时long转string（防止js精度溢出）
/// </summary>
public class LongJsonConverter : JsonConverter
{
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JToken jtoken = JValue.ReadFrom(reader);
        return jtoken.Values();
    }

    public override bool CanConvert(Type objectType)
    {
        return typeof(System.Int64).Equals(objectType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value.ToString());
    }
}