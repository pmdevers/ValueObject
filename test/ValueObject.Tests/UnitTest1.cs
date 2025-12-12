using AwesomeAssertions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using ValueObjects;
using ValueObjects.Identifiers;

namespace ValueObject.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        SampleValueObject.Empty.ToString().Should().Be(string.Empty);
        SampleValueObject.Unknown.ToString().Should().Be("?");

        SampleValueObject.Parse("Hello").ToString().Should().Be("Hello");
        SampleValueObject.TryParse("World", null, out var vo).Should().BeTrue();
        vo.ToString().Should().Be("World");
    }

    [Fact]
    public void TestJsonSerialization()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new ValueObjectJsonConverter());

        var vo = SampleValueObject.Parse("JsonValue");

        var json = JsonSerializer.Serialize(vo, options);

        json.Should().Be("\"JsonValue\"");

        var deserialized = JsonSerializer.Deserialize<SampleValueObject>(json, options);

        deserialized.Should().Be(vo);
    }


    [Fact]
    public void Number()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new ValueObjectJsonConverter());

        var vo = NumberObject.Create(666);

        var json = JsonSerializer.Serialize(vo, options);

        json.Should().Be("666");

        var deserialized = JsonSerializer.Deserialize<NumberObject>(json, options);

        deserialized.Should().Be(vo);
    }

    [Fact]
    public void Operators()
    {
        SampleValueObject vo = "ImplicitValue";
        vo.ToString().Should().Be("ImplicitValue");
    }


    [Fact]
    public void IdentifierGuid()
    {

        var id1 = Identifier.Create(Guid.NewGuid());
        var id2 = Identifier.Parse(id1.ToString());
        id2.Should().Be(id1);
    }

    [Fact]
    public void TypeConverter()
    {
        var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(SampleValueObject));
        var vo = (SampleValueObject)converter.ConvertFrom(122)!;
        (vo == "122").Should().BeTrue();
    }


    [Fact]
    public void IntTypeConverter()
    {
        var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(NumberObject));
        var vo = (NumberObject)converter.ConvertFrom(122)!;
        (vo == 122).Should().BeTrue();
    }

    [Fact]
    public void GuidIdTypeConverter()
    {

        var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(TestId));
        var guid = Guid.NewGuid();
        var vo = (TestId)converter.ConvertFrom(guid.ToString())!;
        (vo.ToString() == guid.ToString()).Should().BeTrue();
    }
}

[Id<GuidIdBehaviour, Guid>]
public partial record struct TestId { }


public record struct NumberObject : IValueObject<NumberObject, int>
{
    private int _value;
    public static NumberObject Create(int value)
    {
        return new() { _value = value };
    }
    public static bool TryParse(string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out NumberObject result)
    {
        if (int.TryParse(s, out var value))
        {
            result = new() { _value = value };
            return true;
        }
        result = default;
        return false;
    }
    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value.ToString(formatProvider);
    }
    public static implicit operator NumberObject(int value)
        => Create(value);
}

[DebuggerDisplay("{DebuggerDisplay}")]
public record struct SampleValueObject() : IValueObject<SampleValueObject, string>
{
    public static bool TryParse(string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out SampleValueObject result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }
        result = new() { _value = s };
        return true;
    }

    private string _value = string.Empty;
    public static SampleValueObject Create(string value)
    {
        return new() { _value = value };
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => _value;

    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        return _value;
    }

    public static implicit operator SampleValueObject(string value)
        => Create(value);
}