﻿
using Latex2Compute.Helpers;
using Latex2Compute.Structures;

namespace Latex2ComputeTests;
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
    [InlineData("", -1)]
    [InlineData(")", 1)]
    [InlineData("ggg)", 4)]
    public void FindBracket_ShouldReturn_IndexPlusOne_IfEndFound( 
        string input, int expectedIndex)
    {
        // Arrange & Act
        int result = BracketHandler.FindBrackets(input, BracketType.Round);


        // Assert
        Assert.Equal(expectedIndex, result);
    }


    [Theory]
    [InlineData("", 0, "", -1)]
    [InlineData("", 4, "", -1)]
    [InlineData("\\left(\\left(g\\right)\\right)", 0, "\\left(g\\right)", 27)]
    [InlineData("\\left(g\\right)", 0, "g", 14)]
    [InlineData("\\left(g\\right)\\left(g\\right)", 14, "g", 28)]
    public void GetCharsBetweenBrackets_ShouldReturn_ExpectedSubstring(
    string input, int startIndex, string expected, int expectedIndex)
    {
        // Arrange & Act
        ContentAndEnd result = BracketHandler.GetCharsBetweenBrackets(input, BracketType.RoundLong, startIndex);
  
        // Assert
        Assert.Equal(expectedIndex, result.EndIndex);
        Assert.Equal(expected, result.Content);

    }

    [Theory]
    [InlineData("", 0, "", -1)]
    [InlineData("", 4, "", -1)]
    [InlineData("((g))", 0, "(g)", 5)]
    [InlineData("(g)", 0, "g", 3)]
    [InlineData("(g)(y)", 0, "g", 3)]
    [InlineData("(g)(y)", 3, "y", 6)]
    [InlineData("(gy", 0, "gy", 3)]
    public void GetCharsBetweenBrackets_ShouldReturn_ExpectedSubstring_WithDifferentBracketTypeRound(
        string input, int startIndex, string expected, int expectedIndex)
    {
        // Arrange & Act
        ContentAndEnd result = BracketHandler.GetCharsBetweenBrackets(input, BracketType.Round, startIndex);
  
        // Assert
        Assert.Equal(expectedIndex, result.EndIndex);
        Assert.Equal(expected, result.Content);

    }

}
