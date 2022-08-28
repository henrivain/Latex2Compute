namespace MekLatexTranslationLibraryTests;
public class SquareRootTests
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
    [InlineData("\\sqrt{3}", "sqrt(3)")]
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
