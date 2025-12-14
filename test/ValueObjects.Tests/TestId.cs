using ValueObjects;

namespace ValueObject.Tests;

public partial record struct TestId : IValueObject<TestId, Guid>
{
    public static TestId Next()
        => new(Guid.NewGuid());
    public static bool IsValid(Guid value)
        => true;
}