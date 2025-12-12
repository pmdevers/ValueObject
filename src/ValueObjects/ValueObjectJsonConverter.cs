using System.Text.Json;
using System.Text.Json.Serialization;

namespace ValueObjects;

/// <inheritdoc />
public sealed class ValueObjectJsonConverter : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.GetInterfaces()
            .Where(x => x.IsGenericType && x.GetGenericTypeDefinition().Equals(typeof(IValueObject<,>)))
            .Any();
    }

    /// <inheritdoc />
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var type = typeToConvert.GetInterfaces()
            .Where(x => x.IsGenericType && x.GetGenericTypeDefinition().Equals(typeof(IValueObject<,>)))
            .FirstOrDefault();

        if (type is null)
        {
            return null;
        }

        var converterType = typeof(ValueObjectJsonConverter<,>).MakeGenericType(type.GenericTypeArguments);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}