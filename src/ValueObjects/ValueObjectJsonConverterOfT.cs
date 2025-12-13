using System.Text.Json;
using System.Text.Json.Serialization;

namespace ValueObjects;

/// <summary>
/// A json converter that converts json into a ValueObject
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class ValueObjectJsonConverter<T, TValue> : JsonConverter<T>
    where T : IValueObject<T, TValue>
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeof(TValue) == typeof(string))
        {
            var stringValue = reader.GetString();
            return T.Create(stringValue is null ? default! : (TValue)(object)stringValue);
        }

        return T.Create(JsonSerializer.Deserialize<TValue>(ref reader, options)!);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var type = T.ToValue(value);

        JsonSerializer.Serialize(writer, type, options);
    }
}
