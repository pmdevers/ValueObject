using ValueObjects;

namespace ValueObject.Tests;

public partial record struct Username : IValueObject<Username, string>
{
    public static Username Create(string value)
        => new(value);
}
