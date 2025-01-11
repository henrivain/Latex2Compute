namespace MekLatexTranslationLibraryTests;
public class PropabilityOperatorTests
{

    [Theory]
    [InlineData("\\binom{23}{2}", "nCr(23,2)")]
    [InlineData("\\binom{\\binom{23}{2}}{2}\\binom{23}{2}", "nCr(nCr(23,2),2)nCr(23,2)")]
    [InlineData("22\\binom{23}{24}56", "22nCr(23,24)56")]
    [InlineData("\\binom{}{}", "nCr(,)")]
    public void ValidBinom_ShouldReturn_ValidTranslation_Always(
        string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, Testing.GetDefaultArgs());
        var physicsItem = new TranslationItem(input, Testing.GetPhysics1Args());

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }

    [Theory]
    [InlineData("\\binom{23}{2", "nCr(23,2)")]
    [InlineData("22\\binom{23}{2456", "22nCr(23,2456)")]
    [InlineData("\\binom{}{", "nCr(,)")]
    public void Binom_WithoutLastBracket_ShouldInclude_EndInParameter(
        string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, Testing.GetDefaultArgs());
        var physicsItem = new TranslationItem(input, Testing.GetPhysics1Args());

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }

    [Theory]
    [InlineData("\\binom{23}2", "nCr(23,2)")]
    [InlineData("\\binom{}", "nCr(,)")]
    public void Binom_WithoutSeconStartBracket_ShouldInclude_EndInParameter_AndEndBracketIndexChar(
    string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, Testing.GetDefaultArgs());
        var physicsItem = new TranslationItem(input, Testing.GetPhysics1Args());

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }

    [Theory]
    [InlineData("\\binom232", "nCr(232,)")]
    [InlineData("\\binom", "nCr(,)")]
    public void Binom_WithoutStartBRacket_PutEverything_InEquation(
        string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, Testing.GetDefaultArgs());
        var physicsItem = new TranslationItem(input, Testing.GetPhysics1Args());

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }

    [Theory]
    [InlineData("\\binom{232", "nCr(232,)")]
    [InlineData("\\binom{", "nCr(,)")]
    public void Binom_WithoutFirstEndBracket_PutEverything_InEquation(
        string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, Testing.GetDefaultArgs());
        var physicsItem = new TranslationItem(input, Testing.GetPhysics1Args());

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }


    [Theory]
    [InlineData("\\left(33\\right)_2", "nPr(33,2)")]
    [InlineData("11\\left(33\\right)_233", "11nPr(33,2)33")]
    [InlineData("\\left(\\right)_", "nPr(,)")]
    [InlineData("\\left(33\\right)_{2}", "nPr(33,2)")]
    [InlineData("11\\left(33\\right)_{23}3", "11nPr(33,23)3")]
    [InlineData("11\\left(33\\right)_{233", "11nPr(33,233)")]   // last bracket can be missing
    public void ValidNPR_ShouldBeTranslated_Always(
        string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, Testing.GetDefaultArgs());
        var physicsItem = new TranslationItem(input, Testing.GetPhysics1Args());

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }


    [Theory]
    [InlineData("\\left(33\\right)2", "(33)2")]
    [InlineData("33_2", "33_2")]
    [InlineData("\\left(33_2", "(33_2")]
    public void NPR_WithPartMissing_ShouldNotBeTranslated(
        string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, Testing.GetDefaultArgs());
        var physicsItem = new TranslationItem(input, Testing.GetPhysics1Args());

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }

}
