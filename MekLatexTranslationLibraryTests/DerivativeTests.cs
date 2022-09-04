namespace MekLatexTranslationLibraryTests;
public class DerivativeTests
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
    [InlineData("D\\ ", "derivative(,)")]
    [InlineData("D\\ 2x^2", "derivative(2x^2,x)")]
    [InlineData("D", "derivative(,)")]
    [InlineData("Dx", "derivative(x,x)")]
    public void Derivative_ShouldTrigger_IfStartsWithBig_D_Always(
        string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, _normalArgs);
        var physicsItem = new TranslationItem(input, _physicsArgs);

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }

    [Theory]
    [InlineData("D\\ 2x^2D", "derivative(2x^2D,x)")]
    [InlineData("DD2x^2D", "derivative(D2x^2D,x)")]
    [InlineData("D\\ D2x^2D", "derivative(D2x^2D,x)")]
    public void Derivative_ShouldTrigger_OnlyOnce_WithManyBigDs_Always(
        string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, _normalArgs);
        var physicsItem = new TranslationItem(input, _physicsArgs);

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }

    [Theory]
    [InlineData("", "derivative(,)")]
    [InlineData("2x^2", "derivative(2x^2,x)")]
    [InlineData("x", "derivative(x,x)")]
    public void Derivative_ShouldTrigger_IfArgsSaySo_Always(
        string input, string expectedResult)
    {
        // Arrange
        var normalArgs = _normalArgs;
        normalArgs.AutoDerivative = true;
        var physicsArgs = _physicsArgs;
        physicsArgs.AutoDerivative = true;

        var normalItem = new TranslationItem(input, normalArgs);
        var physicsItem = new TranslationItem(input, physicsArgs);

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }


    [Theory]
    [InlineData("D\\ g", "derivative(,)")]
    [InlineData("D\\ m2x^2", "derivative(2x^2,x)")]
    public void Derivative_InPhysicsMode_ShouldNotAdd_UnitAsArgument(
        string input, string expectedResult)
    {
        // Arrange
        var physicsItem = new TranslationItem(input, _physicsArgs);

        // Act
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedResult, physicsResult.Result);
    }

    [Theory]
    [InlineData("D\\ ax", "derivative(ax,x)")]
    [InlineData("D\\ by", "derivative(by,y)")]
    [InlineData("D\\ cz", "derivative(cz,z)")]
    [InlineData("D\\ abcdefghijklmnopqrstuvwxyz", "derivative(abcdefghijklmnopqrstuvwxyz,x)")]
    public void Derivative_ShouldFind_XYZ_ArgsBeforeOthers_InMathMode(
        string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, _normalArgs);

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
    }
}
