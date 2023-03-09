using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MekLatexTranslationLibraryTests;
public class TranslationArgumentTests
{
    readonly TranslationArgs _normalArgs = new()
    {
        MathMode = true,
        PhysicsMode1 = false,
        PhysicsMode2 = false,
        GeometryMode = true
    };

    readonly TranslationArgs _physicsArgs = new()
    {
        MathMode = false,
        PhysicsMode1 = true,
        PhysicsMode2 = false,
        GeometryMode = true
    };

    // Physics mode and math mode are both enabled in most of these tests, to make the highest possiblity to break tests
    // Both modes should not be enabled in production code at the same time



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
        var normalItem = new TranslationItem(input, new()
        {
            PhysicsMode1 = true,
            MathMode = true,
            AutoSolve = true,
        });

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
        var normalItem = new TranslationItem(input, new()
        {
            PhysicsMode1 = true,
            MathMode = true,
        });

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
        var normalItem = new TranslationItem(input, new()
        {
            PhysicsMode1 = true,
            MathMode = true,
            AutoDerivative = true,
        });

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
        var normalItem = new TranslationItem(input, new()
        {
            PhysicsMode1 = true,
            MathMode = true,
            AutoSeparateOperators = true
        });

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
        var normalItem = new TranslationItem(input, new()
        {
            PhysicsMode1 = true,
            MathMode = true,
            EndChanges = "all"
        });

        // Act
        var result = LatexTranslation.Translate(normalItem);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }


}
