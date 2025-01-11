namespace Latex2ComputeTests.PhysicsUnits;
public class PhysicsMode2Tests
{
    public PhysicsMode2Tests()
    {
        _args = Testing.GetDefaultArgs();
        _args.UnitTranslationMode = UnitTranslationMode.Translate;
    }

    readonly TranslationArgs _args;

    [Theory]
    [InlineData(@"\frac{J}{kg}", "(_J)/(_kg)")]
    [InlineData(@"29\cdot 10^{65}\frac{J}{kg}", "29*10^(65)(_J)/(_kg)")]
    [InlineData(@"\frac{450\cdot 10^6W}{29\cdot 10^{65}\frac{J}{kg}}", "(450*10^6_W)/(29*10^(65)(_J)/(_kg))")]
    [InlineData(@"\frac{450\cdot 10^6W}{\frac{29\cdot 10^6\frac{J}{kg}\cdot 40{,}28kg}{s}}",
        "(450*10^6_W)/((29*10^6(_J)/(_kg)*40.28_kg)/(_s))")]
    [InlineData(@"\frac{\pi }{100}m^2+16{,}00\cdot 10^{-6}\frac{1}{K}\cdot \frac{\pi }{100}m^2\cdot \left(13K-308{,}15K\right)",
        "(pi)/(100)_m^2+16.00*10^(-6)(1)/(_°K)*(pi)/(100)_m^2*(13_°K-308.15_°K)")]
    [InlineData(@"\frac{290W}{1\frac{kg}{l}\cdot 490\cdot 10^{-3}l\cdot 0{,}1427\frac{K}{s}}",
        "(290_W)/(1(_kg)/(_l)*490*10^(-3)_l*0.1427(_°K)/(_s))")]
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
    [InlineData(@"\frac{550K-18K}{550K}", "(550_°K-18_°K)/(550_°K)")]
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
    [InlineData(@"l", "_l")]
    [InlineData("0°C", "0_°C")]
    [InlineData("0°K", "0_°K")]
    [InlineData("0°F", "0_°F")]
    [InlineData("C", "_coul")]
    [InlineData("mbar+barkbar", "_mbar+_bar*10^(3)_bar")]
    [InlineData("PakPa", "_Pa_kPa")]
    [InlineData("MPa", "10^(6)_Pa")]
    [InlineData("GPa", "10^(9)_Pa")]
    [InlineData("mW", "10^(-3)_W")]
    [InlineData("lykpcpc", "_ltyr*10^(3)_pc_pc")]
    [InlineData("molmmol", "_mol*10^(-3)_mol")]
    [InlineData("kWhGWh", "_kWh*10^(6)_kWh")]
    [InlineData("CmC", "_coul*10^(-3)_coul")]
    [InlineData("3pm", "3*10^(-12)_m")]
    [InlineData("3a", "3_yr")]
    [InlineData("3Bq+4kBq", "3(1/_s)+4*10^(3)*(1/_s)")]
    [InlineData("3c", "3_c")]
    [InlineData("3u", "3_u")]
    [InlineData("3d", "3_day")]
    [InlineData("3nC+66\\mu C", "3*10^(-9)_coul+66*10^(-6)_coul")]
    [InlineData("1mN", "1*10^(-3)_N")]
    [InlineData("3MW+4GW+5TW", "3*10^(6)_W+4*10^(9)_W+5*10^(12)_W")]
    public void PhysicsModeTranslate_ShouldTranslate_SimpleUnits(string input, string expectedResult)
    {
        // Arrange
        TranslationItem item = new(input, _args);

        // Act
        var result = LatexTranslation.Translate(item);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }

    [Theory]
    [InlineData("r")]
    [InlineData("rad")]
    [InlineData("(rad)")]
    [InlineData(@"\degree")]
    [InlineData(@"°")]
    public void PhysicsModeTranslate_Remove_SomeUnits(string input)
    {
        // Arrange
        TranslationItem item = new(input, _args);

        // Act
        var result = LatexTranslation.Translate(item);

        // Assert
        Assert.Empty(result.Result);
    }

    [Theory]
    [InlineData(@"2\Omega +3k\Omega -4m\Omega -4M\Omega ", "2_ohm+3_kΩ-4*10^(-3)_Ω-4_MΩ")]
    public void PhysicsModeTranslate_LatexUnits(string input, string expectedResult)
    {
        TranslationItem item = new(input, _args);

        // Act
        var result = LatexTranslation.Translate(item);

        // Assert
        Assert.Equal(expectedResult, result.Result);
    }

}
