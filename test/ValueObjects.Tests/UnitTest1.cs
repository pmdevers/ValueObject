using System.Text.Json;
using ValueObjects;
using ValueObjects.Financial;

namespace ValueObject.Tests;


public class ValueObject_Tests
{
    public class Serialization
    {
        [Fact]
        public void Should_Serialize_String_Value()
        {
            var test = TestId.Empty;
            var userId = UserId.Empty;

            var obj = Username.Create("testuser");
            var json = JsonSerializer.Serialize(obj);
            var deserializedObj = JsonSerializer.Deserialize<Username>(json);
            Assert.Equal(obj, deserializedObj);
        }

        [Fact]
        public void Should_Serialize_Decimal_Value()
        {
            var result = Amount.Parse("100.000");

            var obj = 100_000m; //Amount.Create(100_000);
            var json = JsonSerializer.Serialize(obj);
            var deserializedObj = JsonSerializer.Deserialize<Amount>(json);
            Assert.Equal(obj, deserializedObj);
        }
    }
}


public partial record struct UserId : IValueObject<UserId, Guid>
{
    public static UserId Next()
        => new(Guid.NewGuid());

    public static UserId Create(Guid value)
        => new(value);
}