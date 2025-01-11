/// Copyright 2021 Henri Vainio 
namespace Latex2Compute;

[Flags]
public enum Params
{
    /// <summary>
    /// Use default settings, only valid if no other settings are set.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Remove all curly brackets from the output.
    /// </summary>
    RemoveCurlyBracket = 1 << 1,

    /// <summary>
    /// Translate alpha, beta, gamma, delta to unicode α β γ δ.
    /// </summary>
    UseGeometryModeSymbols = 1 << 2,

    /// <summary>
    /// Add solve([output],vars) around the output if not already 
    /// surrounded with solve.
    /// </summary>
    AutoSolve = 1 << 3,

    /// <summary>
    /// Add derivative([output],var) around the output.
    /// </summary>
    AutoDerivative = 1 << 4,

    /// <summary>
    /// Translate special constants like e and i.
    /// </summary>
    SpecialSymbolTranslation = 1 << 5,

    /// <summary>
    /// Run translations specific to mathcha.io.
    /// </summary>
    MathchaEnabled = 1 << 6,

    /// <summary>
    /// Automatically separate operators with multiplication sign "*".
    /// </summary>
    AutoSeparateOperators = 1 << 7
}
