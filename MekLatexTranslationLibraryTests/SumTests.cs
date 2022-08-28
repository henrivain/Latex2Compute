namespace MekLatexTranslationLibraryTests;
public class SumTests
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
    [InlineData("\\sum _{n=33}^{22}44", "∑(44,n,33,22)")]
    [InlineData("\\sum _{n=33}^{22}", "∑(,n,33,22)")]
    [InlineData("11\\sum _{n=33}^{22}44", "11∑(44,n,33,22)")]
    [InlineData("1\\sum _3^24", "1∑(4,n,3,2)")]
    public void ValidSum_ShouldReturn_ValidTranslation_Always(
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
    [InlineData("\\sum _{ }^{ }", "∑(,n,,)")]
    [InlineData("11\\sum _{33}^{22}44", "11∑(44,n,33,22)")] // no variable => add n
    public void IncompeteSum_ShouldAdd_Values_IfNeeded(
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
    [InlineData("1\\sum _{33}^{2244", "1∑(,n,33,2244)")]    // no end end bracket
    public void Sum_IncompeteBracket_ShouldAdd_Values_IfPossible(
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
    [InlineData("\\sum _{n=2}^1\\left(a\\cdot \\sum _{n=4}^35a\\right)", "∑(a*∑(5a,n,4,3),n,2,1)")]
    public void Sum_InsideAnother_ShouldTranslateAll_Always(
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
