
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
        int result = BracketHandler.FindBrackets(input, BracketType.RoundLong);

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
        int roundLongResult = BracketHandler.FindBrackets(input, BracketType.RoundLong);
        int roundResult = BracketHandler.FindBrackets(input, BracketType.Round);
        int curlyLongResult = BracketHandler.FindBrackets(input, BracketType.CurlyLong);
        int curlyResult = BracketHandler.FindBrackets(input, BracketType.Curly);
        int squareLongResult = BracketHandler.FindBrackets(input, BracketType.SquareLong);
        int squareResult = BracketHandler.FindBrackets(input, BracketType.Square);

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
        ContentAndEnd result = BracketHandler.GetCharsBetweenBrackets(input, startIndex);
  
        // Assert
        Assert.Equal(expected, result.Content);
        Assert.Equal(expectedIndex, result.EndIndex);
  
    }

}
