using MekLatexTranslationLibrary.PhysicsMode;

namespace MekLatexTranslationLibraryTests.PhysicsUnits;
public class PhysicsModeTwoParserTests
{
    [Fact]
    public void Translate_ShouldRemove_Rads()
    {
        // Arrange
        var parser = new PhysicsModeTwoParser("5rad+6(rad)");

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
        var parser = new PhysicsModeTwoParser(input);

        // Act
        var result = parser.Translate();

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Null(parser.Errors);
    }

    /* Strange cases
     * "mdammm" != "_m*10_m_mm" because dam is deleted first and "mmm" is left => "mm" + "m"
     
     */

    [Theory]
    //[InlineData("mm^2+cm+dam+mm*55km", "_mm^2+_cm+10_m+_mm*55_km")]
    //[InlineData("mm^2+cm+dammm*55km", "_mm^2+_cm+10_m_mm*55_km")]
    //[InlineData("mcm+mm", "_m_cm+_mm")]
    //[InlineData("m+dammm", "_m+10_m_mm")]
    //[InlineData("mdam+mm", "_m*10_m+_mm")]
    //[InlineData("mm^2+dam77mm", "_mm^2+10_m77_mm")]
    [InlineData("mm^2+dammm", "_mm^2+10_m_mm")]
    public void Translate_ShouldTranslate_Lengths(string input, string expectedResult)
    {
        // Arrange
        var parser = new PhysicsModeTwoParser(input);

        // Act
        var result = parser.Translate();

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Null(parser.Errors);
    }

    [Theory]
    [InlineData("dam+hm+Wh", "10_m+10^(2)_m+10^(-3)_kWh")]
    [InlineData("kWhMWhGWhTWhPWh", "_kWh10^(3)_kWh10^(6)_kWh10^(9)_kWh10^(12)_kWh")]
    public void Translate_ShouldAdd_TenPowers_IfNeeded(string input, string expectedResult)
    {
        // Arrange
        var parser = new PhysicsModeTwoParser(input);

        // Act
        var result = parser.Translate();

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Null(parser.Errors);
    }


    [Theory]
    [InlineData("2dam+5hm", "2*10_m+5*10^(2)_m")]
    public void Translate_ShouldAdd_MultiplicationSign_IfNeeded(string input, string expectedResult)
    {
        // Arrange
        var parser = new PhysicsModeTwoParser(input);

        // Act
        var result = parser.Translate();

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Null(parser.Errors);
    }
}
