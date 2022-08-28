namespace MekLatexTranslationLibraryTests;
public class RiseToPowerTests
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
    [InlineData("2^2", "2^2")]
    [InlineData("2^{34}", "2^(34)")]
    [InlineData("2^{3}", "2^(3)")]
    [InlineData("2^{3}4", "2^(3)4")]
    [InlineData("2^34", "2^34")]
    public void ValidRiseToPower_ShouldReturn_ValidTranslation_Always(
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
