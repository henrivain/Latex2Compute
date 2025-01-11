namespace Latex2ComputeTests;
public class LimTests
{

    [Theory]
    [InlineData("\\lim _{x\\to 4}\\left(3x\\right)", "lim(3x,x,4)")]
    [InlineData("\\lim _{x\\to 4}3x", "lim(3x,x,4)")]
    [InlineData("\\lim _{x\\to 4}", "lim(,x,4)")]
    public void ValidLim_ShouldReturn_ValidTranslation_Always(string input, string expectedResult)
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
    [InlineData("\\lim _{x\\to 4}\\left(3\\right)x", "lim(3,x,4)x")]
    [InlineData("\\lim _{x\\to 4}\\left(22x\\right)xx", "lim(22x,x,4)xx")]
    public void ValidLim_IfBodyWithBrackets_BodyShouldBeSeparated_FromEnd_Always(string input, string expectedResult)
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


    [Fact]
    public void Lim_WithoutRange_ShouldAdd_XtoÅ_Always()
    {
        // Arrange
        string input = "\\lim \\left(y\\right)";
        string expectedResult = "lim(y,x,å)";

        var normalItem = new TranslationItem(input, Testing.GetDefaultArgs());
        var physicsItem = new TranslationItem(input, Testing.GetPhysics1Args());

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }

    [Fact]
    public void Lim_WithoutVariable_ShouldAdd_XAsVar_Always()
    { 
        // Arrange
        string input = "\\lim _{\\to å}y";
        string expectedResult = "lim(y,x,å)";

        var normalItem = new TranslationItem(input, Testing.GetDefaultArgs());
        var physicsItem = new TranslationItem(input, Testing.GetPhysics1Args());

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }

    [Fact]
    public void Lim_WithoutApproachable_ShouldAdd_ÅAsApproachable_Always()
    {
        // Arrange
        string input = "\\lim _{x\\to}y";
        string expectedResult = "lim(y,x,å)";

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
    [InlineData("\\lim _{x\\to å}\\left(\\lim _{y\\to o}b\\right)", "lim(lim(b,y,o),x,å)")]
    [InlineData("\\lim _{x\\to å}\\left(\\lim _{y\\to o}\\left(b\\right)\\right)", "lim(lim(b,y,o),x,å)")]
    [InlineData("\\lim _{x\\to å}\\left(3\\cdot x\\right)\\lim _{y\\to å}\\left(3\\cdot y\\right)", "lim(3*x,x,å)lim(3*y,y,å)")]
    public void Lim_WithLim_ShouldBeTranslated_Normally_Always(string input, string expectedResult)
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
