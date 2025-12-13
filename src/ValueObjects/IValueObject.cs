using System.Diagnostics.CodeAnalysis;

namespace ValueObjects;

public interface IValueObject<TSelf, TValue>
{
    static abstract TSelf Create(TValue value);
    static abstract bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out TSelf result);
    string ToString(string? format, IFormatProvider? formatProvider);
}
