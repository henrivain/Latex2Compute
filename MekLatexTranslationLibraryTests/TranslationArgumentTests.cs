namespace MekLatexTranslationLibraryTests;
public class TranslationArgumentTests
{
    [Theory]
    [InlineData("x=0", "solve(x=0,x)")]
    [InlineData("x+b=0", "solve(x+b=0,b,x)")]
    [InlineData("xyz=0", "solve(xyz=0,x,y,z)")]
    [InlineData("b=0", "solve(b=0,b)")]
    [InlineData("=y", "solve(=y,y)")]
    [InlineData("xyz", "xyz")]
    [InlineData("3\\cdotx<0", "solve(3*x<0,x)")]
    [InlineData("3x<=0", "solve(3x<=0,x)")]
    [InlineData("55\\cdotx>0", "solve(55*x>0,x)")]
    [InlineData("55x>=0", "solve(55x>=0,x)")]
    [InlineData("solve(3x,x)", "solve(3x,x)")]
    public void AutoSolve_ShouldAdd_Solve_AndVariable_IfHasEqualityOperator_AndVariable(string input, string expectedResult)
    {
        // Arrange
        TranslationArgs args = Testing.GetDefaultArgs();
        args.Enable(Params.AutoSolve);

        TranslationItem normalItem = new(input, args);

        // Act
        var result = LatexTranslation.Translate(normalItem);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }


    [Theory]
    [InlineData("Dx^2", "derivative(x^2,x)")]
    [InlineData("D14b", "derivative(14b,b)")]
    [InlineData("D14abx", "derivative(14abx,x)")]   // xyz are chosen first
    [InlineData("14abx", "14abx")]   // no D
    [InlineData("DD 3x", "derivative(D3x,x)")]   // should not run twice
    [InlineData("D derivative(3x,x)", "derivative(derivative(3x,x),x)")]   // can derivative already derivated 
    public void AutoDerivative_ShouldAdd_Derivative_AndVariable_IfStartsWithD(string input, string expectedResult)
    {
        // Arrange
        TranslationItem normalItem = new(input, Testing.GetDefaultArgs());

        // Act
        var result = LatexTranslation.Translate(normalItem);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }

    [Theory]
    [InlineData("x^2", "derivative(x^2,x)")]
    [InlineData("14b", "derivative(14b,b)")]
    [InlineData("14abx", "derivative(14abx,x)")]   // xyz are chosen first
    [InlineData("14", "derivative(14,)")]   // no D
    [InlineData("derivative(3x,x)", "derivative(derivative(3x,x),x)")]   // can derivative already derivated 
    public void AutoDerivativeSetting_DerivatesAlways(string input, string expectedResult)
    {
        // Arrange
        var args = Testing.GetDefaultArgs();
        args.Enable(Params.AutoDerivative);

        TranslationItem normalItem = new(input, args);

        // Act
        var result = LatexTranslation.Translate(normalItem);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }


    [Theory]
    [InlineData("\\pi\\tan\\left(2\\right)", "pi*tan(2)")]
    [InlineData("\\pi=\\tan\\left(2\\right)", "pi=tan(2)")]     // has operator symbol in front
    public void AutoSeparateOperators_ShouldSeparateOperators_ByAsterisk(string input, string expectedResult)
    {
        // Arrange
        TranslationArgs args = Testing.GetDefaultArgs();
        args.Enable(Params.AutoSeparateOperators);

        TranslationItem normalItem = new(input, args);

        // Act
        var result = LatexTranslation.Translate(normalItem);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }


    [Theory]
    [InlineData("\\frac{}{}", "")]
    [InlineData("\\frac{mol}{\\frac{km}{h}}", "")]
    public void EmptyFracs_ShouldBeRemoved_IfEndChanges_IsAll(string input, string expectedResult)
    {
        // Arrange
        TranslationArgs args = Testing.GetPhysics1Args();
        args.EndChanges = EndChanges.All;

        var normalItem = new TranslationItem(input, args);

        // Act
        var result = LatexTranslation.Translate(normalItem);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }


}
