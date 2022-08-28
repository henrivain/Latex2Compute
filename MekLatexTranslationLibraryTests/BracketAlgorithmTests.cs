
using MekLatexTranslationLibrary.Helpers;
using MekLatexTranslationLibrary.Structures;

namespace MekLatexTranslationLibraryTests;
public class BracketAlgorithmTests
{
    [Theory]
    [InlineData("\\right)", 7)]
    [InlineData("\\left(\\right)\\right)", 20)]
    [InlineData("ggg\\right)", 10)]
    [InlineData("ggg\\right)ggg", 10)]
    [InlineData("ggg\\right)\\left(\\right)\\right)", 10)]
    // start bracket is removed from input
    public void FindBrackets_WithRoundBrackets_ShouldReturn_IndexOf_EndOfClosingBracket(
        string input, int expectedIndex)
    {
        // Arrange & Act
        int result = HandleBracket.FindBrackets(input, BracketType.RoundLong);

        // Assert
        Assert.Equal(expectedIndex, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("\\left(\\right)")]
    [InlineData("\\left[\\right]")]
    [InlineData("\\left{\\right}")]
    [InlineData("jkashdkjashjk")]
    public void FindBracket_EndBracketNotFound_ShouldReturn_MinusOne(
        string input)
    {
        // Arrange & Act
        int roundLongResult = HandleBracket.FindBrackets(input, BracketType.RoundLong);
        int roundResult = HandleBracket.FindBrackets(input, BracketType.Round);
        int curlyLongResult = HandleBracket.FindBrackets(input, BracketType.CurlyLong);
        int curlyResult = HandleBracket.FindBrackets(input, BracketType.Curly);
        int squareLongResult = HandleBracket.FindBrackets(input, BracketType.SquareLong);
        int squareResult = HandleBracket.FindBrackets(input, BracketType.Square);

        // Assert
        Assert.Equal(-1, roundLongResult);
        Assert.Equal(-1, roundResult);
        Assert.Equal(-1, curlyLongResult);
        Assert.Equal(-1, curlyResult);
        Assert.Equal(-1, squareLongResult);
        Assert.Equal(-1, squareResult);
    }



    [Theory]
    [InlineData("", 0, "", -1)]
    [InlineData("\\left(\\left(g\\right)\\right)", 0, "\\left(g\\right)", 27)]
    [InlineData("\\left(g\\right)", 0, "g", 14)]
    [InlineData("\\left(g\\right)\\left(g\\right)", 14, "g", 28)]
    public void GetCharsBetweenBrackets_ShouldReturn_ExpectedSubstring(
    string input, int startIndex, string expected, int expectedIndex)
    {
        // Arrange & Act
        ContentAndEnd result = HandleBracket.GetCharsBetweenBrackets(input, startIndex);
  
        // Assert
        Assert.Equal(expected, result.Content);
        Assert.Equal(expectedIndex, result.EndIndex);
  
    }

}
