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
    public bool CurlyBracket { get; set; } = true;
    public bool GeometryMode { get; set; } = false;
    public bool PhysicsMode1 { get; set; } = false;
    public bool PhysicsMode2 { get; set; } = false;
    public bool MathMode { get; set; } = true;
    public bool AutoSolve { get; set; } = false;
    public bool AutoDerivative { get; set; } = false;
    public string EndChanges { get; set; } = "all";
    public bool SpecialSymbolTranslation { get; set; } = false;
    public bool MatchaEnabled { get; set; } = true;
    public bool AutoSeparateOperators { get; set; } = true;
}
