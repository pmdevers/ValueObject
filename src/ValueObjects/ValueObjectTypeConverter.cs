using System.ComponentModel;
using System.Globalization;

namespace ValueObjects;

internal sealed class ValueObjectTypeConverter<T, TValue> : TypeConverter
    where T : IValueObject<T, TValue>
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
       => sourceType == typeof(TValue) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is TValue typedValue)
        {
            return T.Create(typedValue);
        }

        return null;
    }
}

/// <summary>
/// Converts an object to an ValueObject.
/// </summary>
public sealed class ValueObjectTypeConverter : TypeConverter
{
    private readonly TypeConverter _converter;

    /// <summary>
    /// Constructs a <see cref="ValueObjectTypeConverter"/> for a specific type. 
    /// </summary>
    /// <param name="type">The type of the value object.</param>
    public ValueObjectTypeConverter(Type type)
    {
        var valueObjectInterface = type.GetInterfaces()
            .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IValueObject<,>))
            ?? throw new ArgumentException($"Type '{type.FullName}' does not implement IValueObject<,> interface.", nameof(type));
        var converterType = typeof(ValueObjectTypeConverter<,>).MakeGenericType(valueObjectInterface.GenericTypeArguments);
        _converter = (TypeConverter)Activator.CreateInstance(converterType)!;
    }

    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => _converter.CanConvertFrom(sourceType);

    /// <inheritdoc />
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => _converter.ConvertTo(context, culture, value, destinationType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => _converter.ConvertFrom(context, culture, value);
}
