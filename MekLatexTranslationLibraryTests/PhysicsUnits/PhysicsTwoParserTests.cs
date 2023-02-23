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
        Assert.Null(parser.Errors);
    }
}
