using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace ValueObjects.Financial;

public readonly partial record struct Percentage :
    IValueObject<Percentage, decimal>,
    IParsable<Percentage>,
    IFormattable,
    IIncrementOperators<Percentage>,
    IDecrementOperators<Percentage>,
    IUnaryPlusOperators<Percentage, Percentage>,
    IUnaryNegationOperators<Percentage, Percentage>,
    IAdditionOperators<Percentage, Percentage, Percentage>,
    ISubtractionOperators<Percentage, Percentage, Percentage>,
    IDivisionOperators<Percentage, decimal, Percentage>,
    IDivisionOperators<Percentage, int, Percentage>
{
    public static bool IsValid(decimal value)
    {
        return value > 0 && value < 1;
    }

    public static Percentage Parse(string s, IFormatProvider? provider = null)
        => TryParse(s, provider, out var result) ? result : throw new FormatException();

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Percentage result)
    {
        if (PercentageParser.TryParse(s, provider, out var value))
        {
            result = new Percentage(value);
            return true;
        }

        result = default;
        return false;
    }

    public override string ToString()
        => ToString(string.Empty);

    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        if (string.IsNullOrEmpty(format))
        {
            format = (_value * 100) % 1 == 0 ? "P0" : "P";
        }

        return _value.ToString(format, formatProvider);
    }

    /// <summary>
    /// Explicitly converts a <see cref="Percentage"/> value to a double.
    /// </summary>
    /// <param name="val">The <see cref="Percentage"/> value.</param>
    public static explicit operator double(Percentage val) => (double)(val._value);

    /// <summary>
    /// Increments the <see cref="Percentage"/> value.
    /// </summary>
    /// <param name="value">The <see cref="Percentage"/> value.</param>
    /// <returns>The incremented <see cref="Percentage"/> value.</returns>
    public static Percentage operator ++(Percentage value)
        => new(value._value + 1);

    /// <summary>
    /// Decrements the <see cref="Percentage"/> value.
    /// </summary>
    /// <param name="value">The <see cref="Percentage"/> value.</param>
    /// <returns>The decremented <see cref="Percentage"/> value.</returns>
    public static Percentage operator --(Percentage value)
        => new(value._value - 1);

    /// <summary>
    /// Returns the unary plus of the <see cref="Percentage"/> value.
    /// </summary>
    /// <param name="value">The <see cref="Percentage"/> value.</param>
    /// <returns>The unary plus of the <see cref="Percentage"/> value.</returns>
    public static Percentage operator +(Percentage value)
        => new(+value._value);

    /// <summary>
    /// Returns the unary negation of the <see cref="Percentage"/> value.
    /// </summary>
    /// <param name="value">The <see cref="Percentage"/> value.</param>
    /// <returns>The unary negation of the <see cref="Percentage"/> value.</returns>
    public static Percentage operator -(Percentage value)
        => new(-value._value);

    /// <summary>
    /// Adds two <see cref="Percentage"/> values.
    /// </summary>
    /// <param name="left">The left <see cref="Percentage"/> value.</param>
    /// <param name="right">The right <see cref="Percentage"/> value.</param>
    /// <returns>The sum of the two <see cref="Percentage"/> values.</returns>
    public static Percentage operator +(Percentage left, Percentage right)
        => new(left._value + right._value);

    /// <summary>
    /// Subtracts one <see cref="Percentage"/> value from another.
    /// </summary>
    /// <param name="left">The left <see cref="Percentage"/> value.</param>
    /// <param name="right">The right <see cref="Percentage"/> value.</param>
    /// <returns>The difference between the two <see cref="Percentage"/> values.</returns>
    public static Percentage operator -(Percentage left, Percentage right)
        => new(left._value - right._value);

    /// <summary>
    /// Multiplies two <see cref="Percentage"/> values.
    /// </summary>
    /// <param name="left">The left <see cref="Percentage"/> value.</param>
    /// <param name="right">The right <see cref="Percentage"/> value.</param>
    /// <returns>The product of the two <see cref="Percentage"/> values.</returns>
    public static Percentage operator *(Percentage left, Percentage right)
        => new(left._value * right._value);

    /// <summary>
    /// Divides one <see cref="Percentage"/> value by another.
    /// </summary>
    /// <param name="left">The left <see cref="Percentage"/> value.</param>
    /// <param name="right">The right <see cref="Percentage"/> value.</param>
    /// <returns>The quotient of the two <see cref="Percentage"/> values.</returns>
    public static Percentage operator /(Percentage left, Percentage right)
        => new(left._value / right._value);

    /// <summary>
    /// Divides a <see cref="Percentage"/> value by a decimal.
    /// </summary>
    /// <param name="left">The <see cref="Percentage"/> value.</param>
    /// <param name="right">The decimal value.</param>
    /// <returns>The quotient of the <see cref="Percentage"/> value and the decimal value.</returns>
    public static Percentage operator /(Percentage left, decimal right)
        => new((decimal)left / right);

    /// <summary>
    /// Divides a <see cref="Percentage"/> value by an integer.
    /// </summary>
    /// <param name="left">The <see cref="Percentage"/> value.</param>
    /// <param name="right">The integer value.</param>
    /// <returns>The quotient of the <see cref="Percentage"/> value and the integer value.</returns>
    public static Percentage operator /(Percentage left, int right)
        => new((decimal)left / right);
}


/// <summary>
/// Provides parsing and formatting methods for <see cref="Percentage"/> values.
/// </summary>
internal static class PercentageParser
{
    private const int _procent_factor = 100;
    private const int _promile_factor = 1000;

    /// <summary>
    /// Converts a decimal value to a percentage string.
    /// </summary>
    /// <param name="value">The decimal value.</param>
    /// <returns>A percentage string.</returns>
    public static string ToString(decimal value, IFormatProvider? formatProvider = null)
    {
        return value.ToString("P2", formatProvider);
    }

    /// <summary>
    /// Tries to parse a percentage string to a decimal value.
    /// </summary>
    /// <param name="s">The percentage string.</param>
    /// <param name="result">The parsed decimal value.</param>
    /// <returns>true if the string was parsed successfully; otherwise, false.</returns>
    public static bool TryParse(string s, IFormatProvider? provider, out decimal result)
    {
        result = 0;
        var value = Normalize(s);

        if (decimal.TryParse(value, provider, out var tmp))
        {
            result = tmp / _procent_factor;

            if (s.Contains('‰'))
            {
                result = tmp / _promile_factor;
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Normalizes a percentage string by removing percentage and permille symbols.
    /// </summary>
    /// <param name="s">The percentage string.</param>
    /// <returns>The normalized string.</returns>
    private static string Normalize(string s)
        => s.Replace("%", string.Empty)
            .Replace("‰", string.Empty);
}