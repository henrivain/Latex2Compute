namespace Latex2ComputeTests.StaticSymbols;

public class StaticSymbolOverrideTests
{
    [Theory]
    [InlineData("c+g", "_c+_g")]
    [InlineData("5c+6g", "5_c+6_g")]
    [InlineData("2x", "2*10_x")]
    public void Translate_ShouldUse_UserStaticSymbols(string input, string expectedResult)
    {
        // Arrange
        var args = Testing.GetDefaultArgs();
        args.UserStaticSymbols = new Dictionary<string, string>
        {
            ["c"] = "_c",
            ["g"] = "_g",
            ["x"] = "10_x"
        };
        var item = new TranslationItem(input, args);

        // Act
        var result = LatexTranslation.Translate(item);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }

    [Theory]
    [InlineData("g+kg", "_g+_kg")]
    [InlineData("5g", "5_g")]
    public void Translate_ShouldOverride_PhysicsSymbols(string input, string expectedResult)
    {
        // Arrange
        var args = Testing.GetDefaultArgs();
        args.UnitTranslationMode = UnitTranslationMode.Translate;
        args.UserStaticSymbols = new Dictionary<string, string>
        {
            ["g"] = "_g"
        };
        var item = new TranslationItem(input, args);

        // Act
        var result = LatexTranslation.Translate(item);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }
   
    
    [Theory]
    [InlineData("\\lambda", "λ")]
    public void Translate_ShouldOverride_Physics1Symbols(string input, string expectedResult)
    {
        // Arrange
        var args = Testing.GetDefaultArgs();
        args.UnitTranslationMode = UnitTranslationMode.Remove;
        args.UserStaticSymbols = new Dictionary<string, string>
        {
            ["\\lambda"] = "λ"
        };
        var item = new TranslationItem(input, args);

        // Act
        var result = LatexTranslation.Translate(item);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }
}
