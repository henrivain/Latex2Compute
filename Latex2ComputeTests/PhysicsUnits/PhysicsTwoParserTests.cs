using MekLatexTranslationLibrary.PhysicsMode;

namespace MekLatexTranslationLibraryTests.PhysicsUnits;
public class PhysicsTwoParserTests
{
    [Fact]
    public void Translate_ShouldRemove_Rads()
    {
        // Arrange
        var parser = new PhysicsTwoParser("5rad+6(rad)");

        // Act
        var result = parser.Translate();

        // Assert
        Assert.Equal("5+6", result);
    }

    [Theory]
    [InlineData("20h+3\\min+7s+5ms+4ns", "20_hr+3_min+7_s+5_ms+4_ns")]
    [InlineData("\\minhhsnsms", "_min_hr_hr_s_ns_ms")]
    [InlineData("snsms", "_s_ns_ms")]
    [InlineData("hsnsms", "_hr_s_ns_ms")]
    public void Translate_ShouldTranslate_TimeUnits(string input, string expectedResult)
    {
        // Arrange
        var parser = new PhysicsTwoParser(input);

        // Act
        var result = parser.Translate();

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(TranslationErrors.None, parser.Errors);
    }

    [Theory]
    [InlineData("mcm+mm", "_m_cm+_mm")]
    [InlineData("m+dammm", "_m+10_m_mm")]
    [InlineData("mdam+mm", "_m*10_m+_mm")]
    [InlineData("mm^2+dam77mm", "_mm^2+10_m77_mm")]
    [InlineData("mm^2+dam+mm", "_mm^2+10_m+_mm")]
    [InlineData("mm^2+dammm", "_mm^2+10_m_mm")]
    public void Translate_ShouldTranslate_Lengths(string input, string expectedResult)
    {
        // Arrange
        var parser = new PhysicsTwoParser(input);

        // Act
        var result = parser.Translate();

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(TranslationErrors.None, parser.Errors);
    }

    [Theory]
    [InlineData("dam+hm+Wh", "10_m+10^(2)_m+10^(-3)_kWh")]
    public void Translate_ShouldAdd_TenPowers_IfNeeded(string input, string expectedResult)
    {
        // Arrange
        var parser = new PhysicsTwoParser(input);

        // Act
        var result = parser.Translate();

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(TranslationErrors.None, parser.Errors);
    }

    [Theory]
    [InlineData("2dam+5hm", "2*10_m+5*10^(2)_m")]
    [InlineData("kWhMWhGWhTWhPWh", "_kWh*10^(3)_kWh*10^(6)_kWh*10^(9)_kWh*10^(12)_kWh")]
    public void Translate_ShouldAdd_MultiplicationSign_IfNeeded(string input, string expectedResult)
    {
        // Arrange
        var parser = new PhysicsTwoParser(input);

        // Act
        var result = parser.Translate();

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(TranslationErrors.None, parser.Errors);
    }

    [Theory]
    [InlineData("kJkgK", "10^(3)_J_kg_°K")]
    [InlineData("0°C", "0_°C")]
    [InlineData("0K", "0_°K")]
    [InlineData("0°F", "0_°F")]
    public void Translate_ShouldNotAdd_PowersIfNotNeeded(string input, string expectedResult)
    {
        // Arrange
        var parser = new PhysicsTwoParser(input);

        // Act
        var result = parser.Translate();

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(TranslationErrors.None, parser.Errors);
    }
}
