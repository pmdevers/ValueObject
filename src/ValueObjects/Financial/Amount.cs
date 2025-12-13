using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ValueObjects.Financial;

public partial record struct Amount :
    IValueObject<Amount, decimal>,
    IParsable<Amount>
{
    public static Amount Create(decimal value)
        => new(value);

    public static Amount Parse(string s, IFormatProvider? provider = null)
        => TryParse(s, provider, out var result) ? result : throw new FormatException();

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Amount result)
    {
        if (string.IsNullOrEmpty(s))
        {
            result = Empty;
            return true;
        }

        if (decimal.TryParse(s, NumberStyles.Any, provider, out decimal value))
        {
            result = Create(value);
            return true;
        }

        result = default;
        return false;
    }
}
