namespace MekLatexTranslationLibraryTests;
public class SymbolTests
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

    [Fact]
    public void Pi_ShouldBeTranslated_AndSeparatedWithMultiplicationSign_IfNeeded_Always()
    {
        // Arrange
        string input = "\\pi \\pi \\frac{\\pi }{\\pi } 22\\pi";
        string expectedResult = "pi*pi(pi)/(pi)22*pi";
        var normalItem = new TranslationItem(input, _normalArgs);
        var physicsItem = new TranslationItem(input, _physicsArgs);

        // Act
        var normalResult = Translation.MakeNormalTranslation(normalItem);
        var physicsResult = Translation.MakeNormalTranslation(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }

    [Fact]
    public void MolicGasConstant_ShouldBeTranslated_InPhysicsMode()
    {
        // Arrange
        string input = @"R=8{,}314\ 51\text{ }\ \frac{\text{Pa}\cdot \text{m}^{\text{3}}}
                        {\text{mol}\cdot \text{K}}=0{,}083\ 1451\text{ }\ \frac{\text{bar}
                        \cdot \text{d}\text{m}^{\text{3}}}{\text{mol}\cdot \text{K}}";
        string expectedResult = "R=8.31451=0.0831451";
        var physicsItem = new TranslationItem(input, _physicsArgs);

        // Act
        var physicsResult = Translation.MakeNormalTranslation(physicsItem);

        // Assert
        Assert.Equal(expectedResult, physicsResult.Result);
    }


    [Theory]
    [InlineData("\\alpha \\beta \\gamma")]
    public void GreekSymbols_ShouldBeRemoved_WhenMathMode(
        string input)
    {
        // Arrange
        var normalItem = new TranslationItem(input, _normalArgs);

        // Act
        var normalResult = Translation.MakeNormalTranslation(normalItem);

        // Assert
        Assert.Empty(normalResult.Result);
    }


    [Theory]
    [InlineData("\\frac{}{}")]
    [InlineData("\\frac{\\frac{}{}}{\\frac{}{}}")]
    [InlineData("\\frac{\\frac{1}{}}{\\frac{}{1}}")]    // also fracs with only one number up or down should be removed
    public void SomeEmptyFracs_ShouldBeRemoved_WithDefaultSettings(
        string input)
    {
        // Arrange
        var normalItem = new TranslationItem(input, _normalArgs);
        var physicsItem = new TranslationItem(input, _physicsArgs);

        // Act
        var normalResult = Translation.MakeNormalTranslation(normalItem);
        var physicsResult = Translation.MakeNormalTranslation(physicsItem);

        // Assert
        Assert.Empty(normalResult.Result);
        Assert.Empty(physicsResult.Result);
    }
}
