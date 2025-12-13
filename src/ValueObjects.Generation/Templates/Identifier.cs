// Automatically generated code. DO NOT EDIT!
#nullable enable
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;
using ValueObjects;

namespace @Namespace;

[DebuggerDisplay("{DebuggerDisplay}")]
[JsonConverter(typeof(ValueObjectJsonConverter))]
[TypeConverter(typeof(ValueObjectTypeConverter))]
public readonly partial record struct @RawName : IValueObject<@RawName, @Value>
{
    private readonly @Value _value;
    private static readonly @Behaviour _behaviour = new();

    public static @RawName Empty => new(_behaviour.Empty());

    private @RawName(@Value value)
    {
        _value = value;
    }

    public static @RawName Next() => new(_behaviour.Next());

    public static @RawName Create(@Value value)
        => _behaviour.Supports(value) ?
        new @RawName(value) :
        throw new ArgumentException("Invalid identifier type.", nameof(value));

    public static bool TryParse(string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out @RawName result)
    {
        if (string.IsNullOrEmpty(s))
        {
            result = @RawName.Empty;
            return true;
        }
        else if (_behaviour.TryParse(s, provider, out var id))
        {
            result = new(id);
            return true;
        }

        result = default!;
        return false;
    }

    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _behaviour.ToString(_value, format, formatProvider);
    }

    public static @Value ToValue(@RawName self)
        => self._value;

    public static implicit operator @RawName(@Value value)
        => Create(value);

    public static implicit operator @Value(@RawName value)
        => ToValue(value);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => _value.ToString();
}