/// Copyright 2021 Henri Vainio 

namespace MekLatexTranslationLibrary;




/// <summary>
/// Argument properties are:
/// <para/>
/// CurlyBracket, GeometryMode, Physics1Mode, Physics2Mode, MathMode, AutoSolve
/// EndChanges, SpecialSymbolTranslation, MatchaEnabled, AutoSeparateOperators
/// </summary>
public struct TranslationArgs
{
    /// <summary>
    /// new Translation args with default values
    /// </summary>
    public TranslationArgs() { }

    public UnitTranslationMode UnitTranslationMode { get; set; } = UnitTranslationMode.None;
    public Params Params { get; set; } = Params.Default;
    public EndChanges EndChanges { get; set; } = EndChanges.All;

 

    public static TranslationArgs GetDefault()
    {
        return new TranslationArgs
        {
            UnitTranslationMode = UnitTranslationMode.None,
            EndChanges = EndChanges.All,
            Params =
              Params.RemoveCurlyBracket |
              Params.MathchaEnabled |
              Params.AutoSeparateOperators
        };
    }
}
