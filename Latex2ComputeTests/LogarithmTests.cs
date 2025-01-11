namespace MekLatexTranslationLibraryTests;
public class LogarithmTests
{

    [Theory]
    [InlineData("\\log _345", "log(45,3)")]
    [InlineData("\\log _3\\left(45\\right)", "log(45,3)")]
    [InlineData("22\\log _3\\left(45\\right)33", "22log(45,3)33")]
    [InlineData("\\log _{32}45", "log(45,32)")]
    [InlineData("\\log _{327}45", "log(45,327)")]
    [InlineData("\\log _{327}\\left(45=823\\right)", "log(45=823,327)")]
    public void ValidLogarithm_ShouldReturn_ValidTranslation_Always(
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
    [InlineData("\\log _{327}45=823", "log(45,327)=823")]
    [InlineData("\\log _{327}45*823", "log(45,327)*823")]
    [InlineData("\\log _{327}45+823", "log(45,327)+823")]
    [InlineData("\\log _{327}45-823", "log(45,327)-823")]
    [InlineData("\\log _{327}45<823", "log(45,327)<823")]
    [InlineData("\\log _{327}45>823", "log(45,327)>823")]
    public void Logarithm_ShouldSeparateBody_IfBeforeOperator_Always(
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
    [InlineData("\\ln \\left(3\\right)444\\cdot \\ln 33=\\ln 555", "ln(3)444*ln(33)=ln(555)")]
    [InlineData("\\frac{\\ln 9\\cdot \\ln 5}{\\ln 8}", "(ln(9)*ln(5))/(ln(8))")]
    public void NaturalLogarithm_ShouldReturn_ValidTranslation_Always(
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
    [InlineData("\\lg 2", "log(2,10)")]
    [InlineData("\\lg \\lg2", "log(log(2,10),10)")]
    public void TenBaseLogarithm_ShouldAdd_TenAsBase_Always(
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
