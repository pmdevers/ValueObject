using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ValueObjects.Identifiers;

[DebuggerDisplay("{DebuggerDisplay}")]
[JsonConverter(typeof(ValueObjectJsonConverter))]
[TypeConverter(typeof(ValueObjectTypeConverter))]
public readonly partial record struct Identifier : IValueObject<Identifier, Guid>
{
    private readonly Guid _value;
    private static readonly GuidIdBehaviour _behaviour = new();

    private Identifier(Guid value)
    {
        _value = value;
    }

    public static Identifier Next() => new(_behaviour.Next());

    public static Identifier Create(Guid value)
        => _behaviour.Supports(value) ?
        new Identifier(value) :
        throw new ArgumentException("Invalid identifier type.", nameof(value));

    public static bool TryParse(string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Identifier result)
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

    public static implicit operator Identifier(Guid value)
        => Create(value);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => ToString();
}