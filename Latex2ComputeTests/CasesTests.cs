namespace Latex2ComputeTests;
public class CasesTests
{

    [Theory]
    [InlineData("\\begin{cases}7&\\\\8&\\end{cases}", "system(7,8)")]
    [InlineData("\\begin{cases}77&\\\\88&\\end{cases}", "system(77,88)")]
    [InlineData("\\begin{cases}77&\\\\88&ee", "system(77,88ee)")]
    [InlineData("\\begin{cases}\r\n\\begin{cases}\r\n1&2\\\\\r\n3&4\r\n\\end{cases}\\\\\r\n56\r\n\\end{cases}", "system(system(12,34),56)")]
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
    [InlineData("""
        \begin{cases}
        3x&{,}x<0\\
        x&\\
        4x^2&{,}x>0
        \end{cases}
        """, "system(3x.x<0,x,4x^2.x>0)")]
    public void PiecedFunction_ShouldBeTranslated_AsSystem_IfRowLacksRange(string input, string expectedResult)
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
    [InlineData("\\begin{cases}3x&{,}x>0\\\\x&{,}x=0\\end{cases}", "piecewise(3x,x>0,x,x=0)")]  // Two rows
    [InlineData("\\begin{cases}3x&{,}x<0\\\\x&{,}x=0\\\\4x^2&{,}x>0\\end{cases}", "piecewise(3x,x<0,x,x=0,4x^2,x>0)")]   //Three rows
    public void PiecedFunction_ShouldBeTranslated_ToPiecewiceFunction_IfEveryRowIsPieced(
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
    [InlineData(""" 
        \begin{cases}
        0&{,}x<0\\
        \frac{1}{20}&{,}0\le x\le 20\\
        0&{,}x>20
        \end{cases}
        """, "piecewise(0,x<0,(1)/(20),0<=x<=20,0,x>20)")]   // x is between "0<=x<=20"
    public void PiecedFunction_ShouldAccept_VarBetweenNumbers_InRange(
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
    [InlineData(""" 
        \begin{cases}
        0&{,}x<0\ tai\ x>20\\
        \frac{1}{20}&{,}0\le x\le 20
        \end{cases}
        """, "piecewise(0,x<0 or x>20,(1)/(20),0<=x<=20)")]   // includes "tai" => or operator
    [InlineData(""" 
        \begin{cases}
        0&{,}x<0\or\ x>20\\
        \frac{1}{20}&{,}0\le x\le 20
        \end{cases}
        """, "piecewise(0,x<0 or x>20,(1)/(20),0<=x<=20)")]   // includes or operator
    public void PiecedFunction_OR_ShouldBeTranslated(
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
