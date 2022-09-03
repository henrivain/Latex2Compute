namespace MekLatexTranslationLibraryTests;
public class NthRootTests
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
    [InlineData("\\sqrt[1]{2}", "root(2,1)")]
    [InlineData("\\sqrt[12]{34}", "root(34,12)")]
    [InlineData("56\\sqrt[12]{34}78", "56root(34,12)78")]
    [InlineData("5\\sqrt[12]{34}6", "5root(34,12)6")]
    [InlineData("1\\sqrt[]{}2", "1root(,)2")]
    public void ValidNthRoot_ShouldReturn_ValidTranslation_Always(string input, string expectedResult)
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
    [InlineData("\\sqrt[12]{\\sqrt[56]{34}}", "root(root(34,56),12)")]
    [InlineData("7\\sqrt[12]{\\sqrt[56]{34}}8", "7root(root(34,56),12)8")]
    [InlineData("\\sqrt[12]{34}\\sqrt[56]{78}", "root(34,12)root(78,56)")]
    public void NthRoot_InsideAnother_ShouldTranslateAll_Always(string input, string expectedResult)
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
    [InlineData("\\sqrt[12]{99", "root(99,12)")]
    [InlineData("\\sqrt[12]99", "root(99,12)")]
    [InlineData("\\sqrt[1299", "root(,1299)")]
    public void NthRoot_WithBadInput_ShouldTranslate_Always(string input, string expectedResult)
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
