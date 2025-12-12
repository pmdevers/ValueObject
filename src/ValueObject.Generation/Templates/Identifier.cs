// Automatically generated code. DO NOT EDIT!
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace @namespace;

[DebuggerDisplay("{DebuggerDisplay}")]
[JsonConverter(typeof(ValueObjectJsonConverter))]
[TypeConverter(typeof(ValueObjectTypeConverter))]
public readonly partial record struct @RawName : IValueObject<@RawName, @Type>
{
    private readonly @Type _value;
    private static readonly @Behaviour _behaviour = new();

    private @RawName(@Type value)
    {
        _value = value;
    }

    public static @RawName Next() => new(_behaviour.Next());

    public static @RawName Create(@Type value)
        => _behaviour.Supports(value) ?
        new @RawName(value) :
        throw new ArgumentException("Invalid identifier type.", nameof(value));

    public static bool TryParse(string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out @RawName result)
    {
        if (string.IsNullOrEmpty(s))
        {
            result = Identifier.Empty;
            return true;
        }
        else if (_behaviour.TryParse(s, provider, out var id))
        {
            result = new(id);
            return true;
        }

        result = Identifier.Unknown;
        return false;
    }

    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _behaviour.ToString(_value, format, formatProvider);
    }

    public static implicit operator Identifier(@Type value)
        => Create(value);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => ToString();
}