using AwesomeAssertions;
using ValueObjects.Financial;

namespace ValueObjects.Tests.Finance;

public class Percentage_Test
{
    [Theory]
    [InlineData("50%", 0.5)]
    [InlineData("100%", 1.0)]
    [InlineData("0.5", 0.05)]
    [InlineData("100", 1.0)]
    [InlineData("120%", 1.2)]
    public void Parse_ShouldParseValidPercentageStrings(string percentage, double expected)
    {
        var result = Percentage.Parse(percentage);

        (result == (decimal)expected).Should().BeTrue();
    }

    [Fact]
    public void ToString_ShouldReturnPercentageString()
    {
        var percentage = Percentage.Create(0.755m);
        var result = percentage.ToString();
        result.Should().Be("75.50%");
    }
}
