using Latex2Compute.Helpers;

namespace Latex2ComputeTests;

public class BracketZeroMemTests
{
    [Theory]
    [InlineData("{12}", "{", "}")]
    [InlineData("12", "{", "}")]
    [InlineData("{{12}3", "{", "}")]
    [InlineData("startend", "start", "end")]
    [InlineData("12start34", "start", "end")]
    public void FindEnd_ShouldReturnMinusOne_WhenNoMatchingEnd(string input, string start, string end)
    {
        // Arrange & Act
        int result = BracketZeroMem.FindEnd(input, start, end);

        // Assert
        Assert.Equal(-1, result);
    }


    [Theory]
    [InlineData("12}", "{", "}", 2)]
    [InlineData("1{}2}", "{", "}", 4)]
    [InlineData("12}3", "{", "}", 2)]
    [InlineData("12{3}4}5", "{", "}", 6)]
    [InlineData("11end", "start", "end", 2)]
    [InlineData("start11endend", "start", "end", 10)]
    public void FindEnd_ShouldReturnIndex_WhenEndFound(string input, string start, string end, int expected)
    {
        // Arrange & Act
        int result = BracketZeroMem.FindEnd(input, start, end);

        // Assert
        Assert.Equal(expected, result);
    }


}
