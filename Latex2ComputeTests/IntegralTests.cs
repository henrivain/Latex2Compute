namespace MekLatexTranslationLibraryTests;
public class IntegralTests
{
 

    [Theory]
    [InlineData("\\int _{ }^{ }", "∫(y,x)")]
    [InlineData("2\\int _{ }^{ }", "2∫(y,x)")]
    public void EmptyIntegral_ShouldAdd_XAsArgument_YAsBody(
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
    [InlineData("\\int _a^b2x^2\\ dx", "∫(2x^2,x,a,b)")]
    [InlineData("\\int _{12}^{13}t\\ dt", "∫(t,t,12,13)")]
    [InlineData("\\int _{-2}^7\\left(x+2\\right)^{\\frac{1}{2}}dx+\\int _7^{10}\\left(-x+10\\right)dx", "∫((x+2)^((1)/(2)),x,-2,7)+∫((-x+10),x,7,10)")]
    [InlineData("\\int _{-3}^3\\frac{4\\cdot \\left(9-x^2\\right)^2\\cdot \\sqrt{3}}{12}dx", "∫((4*(9-x^2)^2*sqrt(3))/(12),x,-3,3)")]
    public void DefinedIntegral_ShouldBeTranslated_WithDefinisionRange_Always(
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
    [InlineData("\\int _{ }^{ }x\\ dx", "∫(x,x)")]
    [InlineData("\\int _{ }^{ }t\\ dxt", "∫(t,xt)")]
    public void UnDefinedIntegral_ShouldBeTranslated_WithoutDefinisionRange_Always(
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
    [InlineData("\\int _{ }^{11}x\\ dx", "∫(x,x,,11)")]
    [InlineData("\\int _{11}^{ }x\\ dx", "∫(x,x,11,)")]
    public void HalfDefinedIntegral_ShouldBeTranslated_WithOneRangeEmpty_Always(
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
    [InlineData("\\int _{12}^{13}tft", "∫(tft,x,12,13)")]
    public void DefinedIntegral_WithoutDSeparator_ShouldAdd_XInTheEnd_Always(
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
    [InlineData("\\int _{ }^{ }\\int _{ }^{ }2y\\ dy\\ dx", "∫(∫(2y,y),x)")]
    public void MultipleIntegrals_ShouldTranslate_All_Always(
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
