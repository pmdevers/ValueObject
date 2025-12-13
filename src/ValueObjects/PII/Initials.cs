using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ValueObjects.PII;

public readonly partial record struct Initials :
    IValueObject<Initials, string>,
    IParsable<Initials>
{
    private const char _dot = '.';

    public static Initials Create(string value)
        => new(value);

    public static Initials Parse(string s, IFormatProvider? provider = null)
        => TryParse(s, provider, out var result) ? result : throw new FormatException();

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Initials result)
    {
        if (string.IsNullOrEmpty(s))
        {
            result = Empty;
            return true;
        }

        var value = s.Contains(_dot)
            ? s
            : string.Join(_dot, s.ToUpper(CultureInfo.InvariantCulture).ToCharArray()) + _dot;

        result = Create(value);
        return true;
    }

    public static Initials FromNames(string names)
    {
        var initials = string.Join("", names.Split(' ').Select(s => s.First()));
        return TryParse(initials, null, out var results) ? results : Empty;
    }
}
