using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MekLatexTranslationLibraryTests;
public class GeometryModeTests
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

    // GeometryMode is enabled in these tests

    [Theory]
    [InlineData("\\alpha", "α")]
    [InlineData("\\beta", "β")]
    [InlineData("\\gamma", "γ")]
    [InlineData("\\delta", "δ")]
    public void GeometryMode_ShouldTranslate_FourGreekLetters_Always(string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, _normalArgs);
        var physicsItem = new TranslationItem(input, _physicsArgs);

        // Act
        var normalResult = LatexTranslation.Translate(normalItem);
        var physicsResult = LatexTranslation.Translate(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }
}
