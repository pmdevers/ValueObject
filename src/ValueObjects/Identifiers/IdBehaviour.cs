using System.Diagnostics.CodeAnalysis;

namespace ValueObjects.Identifiers;

public interface IdBehaviour<T>
{
    bool Supports(object id);
    T Empty();
    T Next();

    string ToString(T id, string? format, IFormatProvider? provider);

    bool TryParse(string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out T result);
}

public class GuidIdBehaviour : IdBehaviour<Guid>
{
    public Guid Empty() => Guid.Empty;
    public Guid Next() => Guid.NewGuid();
    public string ToString(Guid id, string? format, IFormatProvider? provider) => id.ToString();
    public bool TryParse(string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Guid result)
    {
        if (Guid.TryParse(s, out var guid))
        {
            result = guid;
            return true;
        }
        result = default;
        return false;
    }
    public bool Supports(object id) => id is Guid;
}
