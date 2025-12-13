using System.Diagnostics.CodeAnalysis;
using ValueObjects;
using ValueObjects.Identifiers;

namespace ValueObject.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var id = TestId.Next();
        var obj = TestObject.Create(id);

        TestObject test = (TestObject)id;

        TestMethod(id, obj);
        TestMethod(obj, id);

    }

    public void TestMethod(TestId id, TestObject obj)
    {

    }
}

[Id<GuidIdBehaviour, Guid>]
public partial record struct TestId { }

public partial record struct TestObject : IValueObject<TestObject, TestId>
{
    public static TestObject Create(TestId value)
    {
        return new TestObject(value);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out TestObject result)
    {
        throw new NotImplementedException();
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value.ToString();
    }
}