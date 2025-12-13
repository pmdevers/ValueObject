// Automatically generated code. DO NOT EDIT!
#nullable enable
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;
using ValueObjects;

namespace @namespace;

[DebuggerDisplay("{DebuggerDisplay}")]
[JsonConverter(typeof(ValueObjectJsonConverter))]
[TypeConverter(typeof(ValueObjectTypeConverter))]
public readonly partial record struct @RawName
{
    private readonly @Value _value;

    public static @RawName Empty => new(default!);

    private @RawName(@Value value)
    {
        _value = value;
    }

    public static @Value ToValue(@RawName self)
        => self._value;

    public static explicit operator @RawName(@Value value)
        => Create(value);

    public static implicit operator @Value(@RawName value)
        => ToValue(value);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string DebuggerDisplay => _value.ToString();
}
