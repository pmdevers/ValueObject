using System.Globalization;
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
        try
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return IValueObject<T, TValue>.Unknown;
            }

            object rawValue = typeof(TValue) switch
            {
                _ when typeof(TValue) == typeof(string) =>
                    reader.TokenType switch
                    {
                        JsonTokenType.String => reader.GetString()!,
                        JsonTokenType.Number or JsonTokenType.True or JsonTokenType.False => reader.GetString()!,
                        _ => reader.GetString()!
                    },

                _ when typeof(TValue) == typeof(int) =>
                    reader.TokenType == JsonTokenType.Number ? reader.GetInt32() : int.Parse(reader.GetString()!, CultureInfo.InvariantCulture),

                _ when typeof(TValue) == typeof(long) =>
                    reader.TokenType == JsonTokenType.Number ? reader.GetInt64() : long.Parse(reader.GetString()!, CultureInfo.InvariantCulture),

                _ when typeof(TValue) == typeof(Guid) =>
                    reader.TokenType == JsonTokenType.String ? reader.GetGuid() : Guid.Parse(reader.GetString()!),

                _ when typeof(TValue) == typeof(bool) =>
                    reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False
                        ? reader.GetBoolean()
                        : bool.Parse(reader.GetString()!),

                _ when typeof(TValue) == typeof(DateTime) =>
                    reader.TokenType == JsonTokenType.String ? reader.GetDateTime() : DateTime.Parse(reader.GetString()!, CultureInfo.InvariantCulture),

                _ when typeof(TValue) == typeof(decimal) =>
                    reader.TokenType == JsonTokenType.Number ? reader.GetDecimal() : decimal.Parse(reader.GetString()!, CultureInfo.InvariantCulture),

                _ => throw new NotSupportedException($"Unsupported type: {typeof(TValue)}")
            };

            return T.Create((TValue)rawValue);
        }
        catch
        {
            return IValueObject<T, TValue>.Unknown;
        }
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        // Use the object's "json" format (project convention) then write appropriate JSON token
        var formatted = value.ToString("json", null) ?? string.Empty;

        try
        {
            if (typeof(TValue) == typeof(string))
            {
                writer.WriteStringValue(formatted);
            }
            else if (typeof(TValue) == typeof(int))
            {
                writer.WriteNumberValue(int.Parse(formatted, CultureInfo.InvariantCulture));
            }
            else if (typeof(TValue) == typeof(long))
            {
                writer.WriteNumberValue(long.Parse(formatted, CultureInfo.InvariantCulture));
            }
            else if (typeof(TValue) == typeof(Guid))
            {
                writer.WriteStringValue(Guid.Parse(formatted).ToString());
            }
            else if (typeof(TValue) == typeof(bool))
            {
                writer.WriteBooleanValue(bool.Parse(formatted));
            }
            else if (typeof(TValue) == typeof(DateTime))
            {
                // DateTime is represented as JSON string
                writer.WriteStringValue(formatted);
            }
            else if (typeof(TValue) == typeof(decimal))
            {
                writer.WriteNumberValue(decimal.Parse(formatted, CultureInfo.InvariantCulture));
            }
            else
            {
                // Fallback: write raw JSON produced by ToString("json") — this expects the value's ToString("json") to be valid JSON.
                writer.WriteRawValue(formatted, skipInputValidation: false);
            }
        }
        catch
        {
            // On any failure fall back to writing null to keep behavior predictable
            writer.WriteNullValue();
        }
    }
}
