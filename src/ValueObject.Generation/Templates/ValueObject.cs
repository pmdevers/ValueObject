using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace @namespace;

[DebuggerDisplay("{ToString()}")]
public readonly partial record struct @RawName : IParsable<@RawName>, IFormattable
{
    private readonly @Value _value;

#if true
    public static @RawName Empty => new(default!);

#endif

    private @RawName(@Value value)
    {
        _value = value;
    }

    public static @RawName Parse(string s)
        => Parse(s, provider: null);

    public static @RawName Parse(string s, IFormatProvider? provider)
        => TryParse(s, provider, out var result) ? result : throw new FormatException();

    public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out @RawName result)
        => TryParse(s, provider: null, out result);

    public override string ToString()
        => ToString("d", CultureInfo.InvariantCulture);

    public static explicit operator @RawName(@Value value)
        => Create(value);

    public static implicit operator @Value(@RawName value)
        => value._value;
}
