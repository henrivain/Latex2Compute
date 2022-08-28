namespace MekLatexTranslationLibraryTests;
public class TrigonometryTests
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
    [InlineData("\\sin \\left(8\\right)", "sin(8)")]
    [InlineData("\\cos \\left(8\\right)", "cos(8)")]
    [InlineData("\\tan \\left(8\\right)", "tan(8)")]
    [InlineData("\\sin 8", "sin(8)")]
    [InlineData("\\cos 8", "cos(8)")]
    [InlineData("\\tan 8", "tan(8)")]
    [InlineData("22+\\sin 8-4", "22+sin(8)-4")]
    [InlineData("22+\\cos 8-4", "22+cos(8)-4")]
    [InlineData("22+\\tan 8-4", "22+tan(8)-4")]
    [InlineData("\\sin 238", "sin(238)")]
    [InlineData("\\cos 238", "cos(238)")]
    [InlineData("\\tan 238", "tan(238)")]
    [InlineData("\\sin 8 \\sin 8", "sin(8)sin(8)")]
    [InlineData("\\cos 8 \\cos 8", "cos(8)cos(8)")]
    [InlineData("\\tan 8 \\tan 8", "tan(8)tan(8)")]
    [InlineData("\\sin \\left(\\sin \\left(8\\right)\\right)", "sin(sin(8))")]
    [InlineData("\\cos \\left(\\cos \\left(8\\right)\\right)", "cos(cos(8))")]
    [InlineData("\\tan \\left(\\tan \\left(8\\right)\\right)", "tan(tan(8))")]
    public void NormalForms_ShouldBeTranslated_Always(
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
    [InlineData("\\sin ^{-1}\\left(8\\right)", "arcsin(8)")]
    [InlineData("\\cos ^{-1}\\left(8\\right)", "arccos(8)")]
    [InlineData("\\tan ^{-1}\\left(8\\right)", "arctan(8)")]
    [InlineData("\\sin ^{-1}28", "arcsin(28)")]
    [InlineData("\\cos ^{-1}28", "arccos(28)")]
    [InlineData("\\tan ^{-1}28", "arctan(28)")]
    public void ArcForms_ShouldBeTranslated_Always(
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
    [InlineData("x\\cos43", "x*cos(43)")]
    [InlineData("x\\tan43", "x*tan(43)")]
    [InlineData("x\\sin43", "x*sin(43)")]
    [InlineData("x\\cos ^{-1}43", "x*arccos(43)")]
    [InlineData("x\\tan ^{-1}43", "x*arctan(43)")]
    [InlineData("x\\sin ^{-1}43", "x*arcsin(43)")]
    public void TriFuncs_ShouldBeSeparated_FromOthersByAsterisk(
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
