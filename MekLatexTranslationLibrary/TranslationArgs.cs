/// Copyright 2024 Henri Vainio 

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

    /// <summary>
    /// Sets the way the units are translated.
    /// </summary>
    public UnitTranslationMode UnitTranslationMode { get; set; } = UnitTranslationMode.None;

    /// <summary>
    /// Additional parameters for the translation.
    /// </summary>
    public Params Params { get; set; } = Params.Default;

    /// <summary>
    /// Which math system to target like Ti-Nspire or Matlab.
    /// </summary>
    public TargetSystem TargetSystem { get; set; } = TargetSystem.Ti;


    /// <summary>
    /// Choose which cleanup actions are made to the output.
    /// </summary>
    public EndChanges EndChanges { get; set; } = EndChanges.All;


    public static TranslationArgs GetDefault()
    {
        return new TranslationArgs
        {
            UnitTranslationMode = UnitTranslationMode.None,
            TargetSystem = TargetSystem.Ti,
            EndChanges = EndChanges.All,
            Params =
              Params.RemoveCurlyBracket |
              Params.MathchaEnabled |
              Params.AutoSeparateOperators
        };
    }
}
