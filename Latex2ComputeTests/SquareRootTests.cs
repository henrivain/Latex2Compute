namespace Latex2ComputeTests;
public class SquareRootTests
{

    [Theory]
    [InlineData("\\sqrt{3}", "sqrt(3)")]
    [InlineData("\\sqrt{34", "sqrt(34)")]
    [InlineData("\\sqrt{12}", "sqrt(12)")]
    [InlineData("\\sqrt{123}", "sqrt(123)")]
    [InlineData("\\sqrt{}", "sqrt()")]
    [InlineData("13\\sqrt{}24", "13sqrt()24")]
    [InlineData("1\\sqrt{34}2", "1sqrt(34)2")]
    [InlineData("1\\sqrt{\\sqrt{34}}2", "1sqrt(sqrt(34))2")]
    [InlineData("\\sqrt{12}\\sqrt{34}", "sqrt(12)sqrt(34)")]
    public void ValidSquareRoot_ShouldReturn_ValidTranslation_Always(
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
