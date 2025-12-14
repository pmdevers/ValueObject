using AwesomeAssertions;
using System.Globalization;
using ValueObjects.Financial;

namespace ValueObjects.Tests.Finance;

public class Percentage_Test
{
    [Theory]
    [InlineData("50%", "0,5")]
    [InlineData("100%", "1,0")]
    [InlineData("0,5", "0,005")]
    [InlineData("100", "1,0")]
    [InlineData("120%", "1,2")]
    public void Parse_ShouldParseValidPercentageStrings(string percentage, string expected)
    {
        var culture = CultureInfo.GetCultureInfo("nl-NL");
        var result = Percentage.Parse(percentage);
        var expectedValue = decimal.Parse(expected, culture);

        (Percentage.ToValue(result) == expectedValue).Should().BeTrue();
    }

    [Fact]
    public void ToString_ShouldReturnPercentageString()
    {
        var culture = CultureInfo.GetCultureInfo("nl-NL");
        var percentage = Percentage.Create(0.755m);
        var result = percentage.ToString(null, culture);
        result.Should().Be("75,500%");
    }
}
