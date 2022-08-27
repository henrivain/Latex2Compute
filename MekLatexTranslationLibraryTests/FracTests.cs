﻿namespace MekLatexTranslationLibraryTests;
public class FracTests
{
    readonly TranslationArgs _normalArgs = new()
    {
        MathMode = true,
        PhysicsMode1 = false,
        PhysicsMode2 = false,
    };
    
    readonly TranslationArgs _physicsArgs = new()
    {
        MathMode = false,
        PhysicsMode1 = true,
        PhysicsMode2 = false,
    };


    [Theory]
    [InlineData("\\frac{4}{3}", "(4)/(3)")]
    [InlineData("\\frac{22}{33}", "(22)/(33)")]
    public void ValidFraction_ShouldReturn_CorrectFormFraction_Always(
        string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, _normalArgs);
        var physicsItem = new TranslationItem(input, _physicsArgs);

        // Act
        var normalResult = Translation.MakeNormalTranslation(normalItem);
        var physicsResult = Translation.MakeNormalTranslation(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }
    
    
    
    [Theory]
    [InlineData("\\frac{}{}", "")]
    [InlineData("12\\frac{}{}34", "1234")]
    public void EmptyFraction_ShouldBeRemoved_Always(
        string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, _normalArgs);
        var physicsItem = new TranslationItem(input, _physicsArgs);

        // Act
        var normalResult = Translation.MakeNormalTranslation(normalItem);
        var physicsResult = Translation.MakeNormalTranslation(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }


    [Theory]
    [InlineData("\\frac{}{", "")]
    [InlineData("\\frac{23}{45", "(23)/(45)")]
    [InlineData("12\\frac{23}{4567", "12(23)/(4567)")]
    public void Fraction_WithoutEndBracket_BracketShouldBeAdded_AndTranslatedNormally_Always(
        string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, _normalArgs);
        var physicsItem = new TranslationItem(input, _physicsArgs);

        // Act
        var normalResult = Translation.MakeNormalTranslation(normalItem);
        var physicsResult = Translation.MakeNormalTranslation(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }

    [Theory]
    [InlineData("\\frac{\\frac{11}{22}}{33}", "((11)/(22))/(33)")]
    [InlineData("\\frac{11}{\\frac{22}{33}}", "(11)/((22)/(33))")]
    [InlineData("\\frac{\\frac{}{}}{\\frac{}{}}", "")]
    [InlineData("\\frac{\\frac{1}{2}}{\\frac{\\frac{3}{4}}{\\frac{5}{6}}}", 
        "((1)/(2))/(((3)/(4))/((5)/(6)))")]
    public void FractionInsideFraction_ShouldTranslatedNormally_Always(
        string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, _normalArgs);
        var physicsItem = new TranslationItem(input, _physicsArgs);

        // Act
        var normalResult = Translation.MakeNormalTranslation(normalItem);
        var physicsResult = Translation.MakeNormalTranslation(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }


    [Theory]
    [InlineData("\\frac{11}", "(11)")]
    [InlineData("\\frac{11}22", "(11)/(22)")]
    [InlineData("\\frac{1122", "(1122")]
    public void Fraction_WithLostBrackets_BracketsShouldBeAdded_AndTranslatedNormally_IfPossible(
        string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, _normalArgs);
        var physicsItem = new TranslationItem(input, _physicsArgs);

        // Act
        var normalResult = Translation.MakeNormalTranslation(normalItem);
        var physicsResult = Translation.MakeNormalTranslation(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }



}
