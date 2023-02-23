namespace MekLatexTranslationLibraryTests.PhysicsUnits;
public class PhysicsMode2Tests
{
    public PhysicsMode2Tests()
    {
        _args = new()
        {
            MathMode = false,
            PhysicsMode1 = false, 
            PhysicsMode2 = true
        };
    }

    readonly TranslationArgs _args;

    [Theory]
    [InlineData(@"\frac{J}{kg}", "(_J)/(_kg)")]
    [InlineData(@"29\cdot 10^{65}\frac{J}{kg}", "29*10^(65)(_J)/(_kg)")]
    [InlineData(@"\frac{450\cdot 10^6W}{29\cdot 10^{65}\frac{J}{kg}}", "(450*10^6_W)/(29*10^(65)(_J)/(_kg))")]
    [InlineData(@"\frac{450\cdot 10^6W}{\frac{29\cdot 10^6\frac{J}{kg}\cdot 40{,}28kg}{s}}",
        "(450*10^6_W)/((29*10^6(_J)/(_kg)*40.28_kg)/(_s))")]
    [InlineData(@"\frac{\pi }{100}m^2+16{,}00\cdot 10^{-6}\frac{1}{K}\cdot \frac{\pi }{100}m^2\cdot \left(13K-308{,}15K\right)",
        "(pi)/(100)_m^2+16.00*10^(-6)(1)/(_°k)*(pi)/(100)_m^2*(13_°k-308.15_°k)")]
    [InlineData(@"\frac{290W}{1\frac{kg}{l}\cdot 490\cdot 10^{-3}l\cdot 0{,}1427\frac{K}{s}}",
        "(290_W)/(1(_kg)/(_km)*490*10^(-3)_km*0.1427(_°k)/(_s))")]
    public void PhysicsModeTranslate_ShouldTranslate_ComplexUnits(string input, string expectedResult)
    {
        // Arrange
        TranslationItem item = new(input, _args);

        // Act
        var result = LatexTranslation.Translate(item);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }

    [Theory]
    [InlineData(@"\frac{550K-18K}{550K}", "(550_°k-18_°k)/(550_°k)")]
    public void PhysicsModeTranslate_ShouldTranslate_Temperatures(string input, string expectedResult)
    {
        // Arrange
        TranslationItem item = new(input, _args);

        // Act
        var result = LatexTranslation.Translate(item);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }

    [Theory]
    [InlineData(@"\text{kg/m}^3", "_kg/_m^3")]
    public void PhysicsModeTranslate_ShouldTranslate_SimpleUnits(string input, string expectedResult)
    {
        // Arrange
        TranslationItem item = new(input, _args);

        // Act
        var result = LatexTranslation.Translate(item);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }

}
