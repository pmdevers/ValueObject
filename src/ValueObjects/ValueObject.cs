using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ValueObjects;

[DebuggerDisplay("{ToString()}")]
public readonly partial record struct ValueObject : IParsable<ValueObject>, IFormattable
{
    private readonly string _value;

#if true
    public static ValueObject Empty => new(default!);

#endif

    private ValueObject(string value)
    {
        _value = value;
    }

    public static ValueObject Parse(string s)
        => Parse(s, provider: null);

    public static ValueObject Parse(string s, IFormatProvider? provider)
        => TryParse(s, provider, out var result) ? result : throw new FormatException();

    public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out ValueObject result)
        => TryParse(s, provider: null, out result);

    public override string ToString()
        => ToString("d", CultureInfo.InvariantCulture);

    public static implicit operator ValueObject(string value)
        => new(value);
}

public readonly partial record struct ValueObject : IValueObject<ValueObject, string>
{
    public static ValueObject Create(string value)
    {
        return new(value);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ValueObject result)
    {
        if (string.IsNullOrEmpty(s))
        {
            result = Empty;
            return true;
        }

        result = Create(s);
        return true;
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value;
    }
}