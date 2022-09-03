using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MekLatexTranslationLibraryTests;
public class CasesTests
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
    [InlineData("\\begin{cases}7&\\\\8&\\end{cases}", "system(7,8)")]
    [InlineData("\\begin{cases}77&\\\\88&\\end{cases}", "system(77,88)")]
    [InlineData("\\begin{cases}77&\\\\88&ee", "system(77,88ee)")]
    [InlineData("\\begin{cases}\r\n\\begin{cases}\r\n1&2\\\\\r\n3&4\r\n\\end{cases}\\\\\r\n56\r\n\\end{cases}", "system(system(12,34),56)")]
    public void NaturalLogarithm_ShouldReturn_ValidTranslation_Always(
        string input, string expectedResult)
    {
        // Arrange
        var normalItem = new TranslationItem(input, _normalArgs);
        var physicsItem = new TranslationItem(input, _physicsArgs);

        // Act
        var normalResult = Translation.MakeNormalTranslation(normalItem);
        var physicsResult = Translation.MakeNormalTranslation(physicsItem);

        // Assert
        Assert.Equal(expectedResult, normalResult.Result);
        Assert.Equal(expectedResult, physicsResult.Result);
    }
}
