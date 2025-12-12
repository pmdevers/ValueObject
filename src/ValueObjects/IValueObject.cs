using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ValueObjects;


[JsonConverter(typeof(ValueObjectJsonConverter))]
[TypeConverter(typeof(ValueObjectTypeConverter))]
public interface IValueObject<TSelf, TValue> : IFormattable
    where TSelf : IValueObject<TSelf, TValue>
{
    public static TSelf Unknown => TSelf.TryParse("?", null, out var result) ? result : throw new FormatException();
    public static TSelf Empty => TSelf.TryParse(string.Empty, null, out var result) ? result : throw new FormatException();
    public abstract static bool TryParse(string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out TSelf result);

    public abstract static TSelf Create(TValue value);

    public abstract static implicit operator TSelf(TValue value);
}
