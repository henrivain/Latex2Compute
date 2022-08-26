namespace MekLatexTranslationLibraryTests;
public class FracTests
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
    [InlineData("\\frac{4}{3}", "(4)/(3)", false)]
    [InlineData("\\frac{22}{33}", "(22)/(33)", false)]
    [InlineData("\\frac{4}{3}", "(4)/(3)", true)]
    [InlineData("\\frac{22}{33}", "(22)/(33)", true)]
    public void ValidFraction_ShouldReturn_CorrectFormFraction_Always(
        string input, 
        string expectedResult, 
        bool isPhysicsMode)
    {
        // Arrange
        var item = new TranslationItem(input, 
            isPhysicsMode ? _physicsArgs : _normalArgs);
        
        // Act
        var result = Translation.MakeNormalTranslation(item);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }
    
    
    
    [Theory]
    [InlineData("\\frac{}{}", "", false)]
    [InlineData("12\\frac{}{}34", "1234", false)]
    [InlineData("\\frac{}{}", "", true)]
    [InlineData("12\\frac{}{}34", "1234", true)]
    public void EmptyFraction_ShouldBeRemoved_Always(
        string input, 
        string expectedResult, 
        bool isPhysicsMode)
    {
        // Arrange
        var item = new TranslationItem(input,
             isPhysicsMode ? _physicsArgs : _normalArgs);

        // Act
        var result = Translation.MakeNormalTranslation(item);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }


    [Theory]
    [InlineData("\\frac{}{", "", false)]
    [InlineData("\\frac{23}{45", "(23)/(45)", false)]
    [InlineData("12\\frac{23}{4567", "12(23)/(4567)", false)]
    [InlineData("\\frac{}{", "", true)]
    [InlineData("\\frac{23}{45", "(23)/(45)", true)]
    [InlineData("12\\frac{23}{4567", "12(23)/(4567)", true)]
    public void Fraction_WithoutEndBracket_BracketShouldBeAdded_AndTranslatedNormally_Always(
        string input, 
        string expectedResult, 
        bool isPhysicsMode)
    {
        // Arrange
        var item = new TranslationItem(input,
             isPhysicsMode ? _physicsArgs : _normalArgs);

        // Act
        var result = Translation.MakeNormalTranslation(item);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }

    [Theory]
    [InlineData("\\frac{\\frac{11}{22}}{33}", "((11)/(22))/(33)", false)]
    [InlineData("\\frac{11}{\\frac{22}{33}}", "(11)/((22)/(33))", false)]
    [InlineData("\\frac{\\frac{}{}}{\\frac{}{}}", "", false)]
    [InlineData("\\frac{\\frac{1}{2}}{\\frac{\\frac{3}{4}}{\\frac{5}{6}}}", 
        "((1)/(2))/(((3)/(4))/((5)/(6)))", false)]
    [InlineData("\\frac{\\frac{11}{22}}{33}", "((11)/(22))/(33)", true)]
    [InlineData("\\frac{11}{\\frac{22}{33}}", "(11)/((22)/(33))", true)]
    [InlineData("\\frac{\\frac{}{}}{\\frac{}{}}", "", true)]
    [InlineData("\\frac{\\frac{1}{2}}{\\frac{\\frac{3}{4}}{\\frac{5}{6}}}", 
        "((1)/(2))/(((3)/(4))/((5)/(6)))", true)]
    public void FractionInsideFraction_ShouldTranslatedNormally_Always(
     string input,
     string expectedResult,
     bool isPhysicsMode)
    {
        // Arrange
        var item = new TranslationItem(input,
             isPhysicsMode ? _physicsArgs : _normalArgs);

        // Act
        var result = Translation.MakeNormalTranslation(item);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }


    [Theory]
    [InlineData("\\frac{11}", "(11)", false)]
    [InlineData("\\frac{11}22", "(11)/(22)", false)]
    [InlineData("\\frac{1122", "(1122", false)]
    [InlineData("\\frac{11}", "(11)", true)]
    [InlineData("\\frac{11}22", "(11)/(22)", true)]
    [InlineData("\\frac{1122", "(1122", true)]
    public void Fraction_WithLostBrackets_BracketsShouldBeAdded_AndTranslatedNormally_IfPossible(
    string input,
    string expectedResult,
    bool isPhysicsMode)
    {
        // Arrange
        var item = new TranslationItem(input,
             isPhysicsMode ? _physicsArgs : _normalArgs);

        // Act
        var result = Translation.MakeNormalTranslation(item);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }



}
