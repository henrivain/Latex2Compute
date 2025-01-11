namespace MekLatexTranslationLibraryTests;
public class RiseToPowerTests
{
    


    [Theory]
    [InlineData("2^2", "2^2")]
    [InlineData("2^{}", "2^()")]
    [InlineData("2^{34}", "2^(34)")]
    [InlineData("2^{3}", "2^(3)")]
    [InlineData("2^{3}4", "2^(3)4")]
    [InlineData("2^34", "2^34")]
    public void ValidRiseToPower_ShouldReturn_ValidTranslation_Always(
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
