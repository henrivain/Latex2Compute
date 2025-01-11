namespace Latex2ComputeTests;
public class GeometryModeTests
{
    // GeometryMode is enabled in these tests

    [Theory]
    [InlineData("\\alpha", "α")]
    [InlineData("\\beta", "β")]
    [InlineData("\\gamma", "γ")]
    [InlineData("\\delta", "δ")]
    public void GeometryMode_ShouldTranslate_FourGreekLetters_Always(string input, string expectedResult)
    {
        // Arrange
        TranslationArgs normalArgs = Testing.GetDefaultArgs();
        normalArgs.Enable(Params.UseGeometryModeSymbols);
        
        TranslationArgs physicsArgs = Testing.GetDefaultArgs();
        physicsArgs.Enable(Params.UseGeometryModeSymbols);

        var normalItem = new TranslationItem(input, normalArgs);
        var physicsItem = new TranslationItem(input, physicsArgs);

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }
}
