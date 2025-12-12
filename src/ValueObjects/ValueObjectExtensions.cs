using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ValueObjects;

public static class ValueObjectExtensions
{
    extension<T, TValue>(IValueObject<T, TValue> self)
        where T : IValueObject<T, TValue>
    {
        public string ToString()
            => self.ToString("n", CultureInfo.InvariantCulture);

        public static T Unknown => IValueObject<T, TValue>.Unknown;

        public static T Empty => IValueObject<T, TValue>.Empty;

        public static T Parse(string value)
            => T.TryParse(value, null, out var result) ? result : throw new FormatException();

        public static T Parse(string value, IFormatProvider? provider)
            => T.TryParse(value, provider, out var result) ? result : throw new FormatException();

        public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out T result)
            => T.TryParse(s, null, out result);
    }
}
