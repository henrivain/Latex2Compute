﻿namespace Latex2ComputeTests;
public class SymbolTests
{

    [Fact]
    public void Pi_ShouldBeTranslated_AndSeparatedWithMultiplicationSign_IfNeeded_Always()
    {
        // Arrange
        string input = "\\pi \\pi \\frac{\\pi }{\\pi } 22\\pi";
        string expectedResult = "pi*pi(pi)/(pi)22*pi";
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
    public void MolicGasConstant_ShouldBeTranslated_InPhysicsMode()
    {
        // Arrange
        string input = @"R=8{,}314\ 51\text{ }\ \frac{\text{Pa}\cdot \text{m}^{\text{3}}}
                        {\text{mol}\cdot \text{K}}=0{,}083\ 1451\text{ }\ \frac{\text{bar}
                        \cdot \text{d}\text{m}^{\text{3}}}{\text{mol}\cdot \text{K}}";
        string expectedResult = "R=8.31451=0.0831451";
        var physicsItem = new TranslationItem(input, Testing.GetPhysics1Args());

        // Act
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedResult, physicsResult.Result);
    }


    [Theory]
    [InlineData("\\alpha \\beta \\gamma")]
    public void GreekSymbols_ShouldBeRemoved_WhenMathMode(
        string input)
    {
        // Arrange
        var normalItem = new TranslationItem(input, Testing.GetDefaultArgs());

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);

        // Assert
        Assert.Empty(normalResult.Result);
    }


    [Theory]
    [InlineData("\\frac{}{}")]
    [InlineData("\\frac{\\frac{}{}}{\\frac{}{}}")]
    [InlineData("\\frac{\\frac{1}{}}{\\frac{}{1}}")]    // also fracs with only one number up or down should be removed
    public void SomeEmptyFracs_ShouldBeRemoved_WithDefaultSettings(
        string input)
    {
        // Arrange
        var normalItem = new TranslationItem(input, Testing.GetDefaultArgs());
        var physicsItem = new TranslationItem(input, Testing.GetPhysics1Args());

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Empty(normalResult.Result);
        Assert.Empty(physicsResult.Result);
    }


    [Theory]
    [InlineData("===5", "5")]
    [InlineData("=++=5", "5")]
    [InlineData("=5", "5")]
    [InlineData("=", "")]
    [InlineData("+++", "")]
    [InlineData("+5", "5")]
    [InlineData("++5", "5")]
    [InlineData("5", "5")]
    [InlineData("-a", "-a")]
    public void PlusSign_InTheStartOfInput_ShouldBeRemoved_Always(
        string input, string expectedInput)
    {
        // Arrange
        var normalItem = new TranslationItem(input, Testing.GetDefaultArgs());
        var physicsItem = new TranslationItem(input, Testing.GetPhysics1Args());

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedInput, normalResult.Result);
        Assert.Equal(expectedInput, physicsResult.Result);
    }


    [Theory]
    [InlineData("\\left|33\\right|", "abs(33)")]
    [InlineData("\\left|\\right|", "abs()")]
    public void Abs_BothBracketsShouldBeTranslated(
    string input, string expectedInput)
    {
        // Arrange
        var normalItem = new TranslationItem(input, Testing.GetDefaultArgs());
        var physicsItem = new TranslationItem(input, Testing.GetPhysics1Args());

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedInput, normalResult.Result);
        Assert.Equal(expectedInput, physicsResult.Result);
    }


    [Theory]
    [InlineData("mm^3 cm^3 dm^3 nm^3 km^3 m^3")]
    [InlineData("mm^2 cm^2 dm^2 nm^2 km^2 m^2")]
    [InlineData("nm mm cm dm m dam hm km")]
    [InlineData("kWh MWh GWh TWh")]
    [InlineData("kA A mA")]
    [InlineData("kV V mV")]
    [InlineData("W kW")]
    [InlineData("T C G F Wb")]
    [InlineData("kN N")]
    [InlineData("kg mg g")]
    [InlineData("ms ns s h")]
    [InlineData("mol mmol")]
    [InlineData("lx lm cd")]
    [InlineData("r rad sr")]
    [InlineData("eV keV MeV GeV TeV")]
    [InlineData("J kJ MJ GJ kcal cal")]
    [InlineData("°C °F K")]
    [InlineData("MHz kHz Hz")]
    [InlineData("bar kPa Pa")]
    [InlineData("kpl \\max \\min")]
    public void SomeSymbols_ShouldBeRemoved_InPhysicsModeOne(string input)
    {
        // Arrange
        var physicsItem = new TranslationItem(input, Testing.GetPhysics1Args());

        // Act
        var result = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Empty(result.Result);
    }

    [Theory]
    [InlineData("2{,}897\\ 771\\ 955\\cdot 10^{−3}\\ \\text{m}\\cdot \\text{K}", "2.897771955*10^(−3)")]
    [InlineData("5{,}670\\ 374\\ 419\\cdot 10^{−8}\\ \\frac{\\text{W}}{\\text{m}^2\\cdot \\text{K}^4}", "5.670374419*10^(−8)")]
    [InlineData("\\frac{m+W}{s}", "")]
    public void PhysicsMode_ShouldRemove_Unnecessary_MultiplicationAndPlus_Signs(string input, string expectedReturn)
    {
        // Arrange
        var physicsItem = new TranslationItem(input, Testing.GetPhysics1Args());
        
        // Act
        var result = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedReturn, result.Result);
    }
}
