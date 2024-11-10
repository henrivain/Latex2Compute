
using System.Runtime.CompilerServices;

/// Copyright 2021 Henri Vainio 
namespace MekLatexTranslationLibrary;


/// <summary>
/// Enum defining any cleanup actions to be done to the output.
/// </summary>
public enum EndChanges
{
    /// <summary>
    /// No actions to the output.
    /// </summary>
    None = 0,

    /// <summary>
    /// Make <see cref="EmptyFrac"/> changes and remove "…" symbols.
    /// </summary>
    All = 1,

    /// <summary>
    /// Remove all empty fractions without meaning from 
    /// the output like ()/() or (^2)/() or ^2/^2.
    /// </summary>
    EmptyFrac = 2
}

/// <summary>
/// Enum defining how to handle units in translation.
/// </summary>
public enum UnitTranslationMode
{
    /// <summary>
    /// [Default] Old math mode, only remove greek symbols.
    /// </summary>
    None = 0,

    /// <summary>
    /// Old Physics mode 1, remove all units.
    /// </summary>
    Remove = 1,

    /// <summary>
    /// Old physics mode 2, translate all units.
    /// </summary>
    Translate = 2
}

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

public static class TranslationItemExtensions
{
    public static bool IsSet(this TranslationArgs args, UnitTranslationMode mode)
    {
        return args.UnitTranslationMode == mode;
    }

    public static bool IsSet(this TranslationArgs args, Params param)
    {
        return (args.Params & param) == param;
    }

    public static bool IsSet(this TranslationArgs args, EndChanges mode)
    {
        return args.EndChanges == mode;
    }

    /// <summary>
    /// Enables the given parameter in the settings. Mutates the input args.
    /// If you want to enable multiple settings, use pipe or pipe assingment.
    /// <code>
    /// params.Enable(Param1 | Param2);
    /// // is same to
    /// params |= Param1 | Param2;
    /// </code>
    /// </summary>
    /// <param name="args"></param>
    /// <param name="param"></param>
    /// <returns>Same settings instance </returns>
    public static void Enable(ref this TranslationArgs args, Params param)
    {
        args.Params |= param;
    }

    /// <summary>
    /// Disables the given parameter in the settings. Mutates the input args.
    /// If you want to disable multiple settings, use pipe or pipe assingment
    /// <code>params.Disable(Param1 | Param2);</code>
    /// </summary>
    /// <param name="args"></param>
    /// <param name="param"></param>
    /// <returns>Same settings instance </returns>
    public static void Disable(ref this TranslationArgs args, Params param)
    {
        args.Params &= ~param;
    }
}


/// <summary>
/// input type to translation 
/// </summary>
/// <remarks>
/// (string text, TranslationArgs settings)
/// </remarks>
public struct TranslationItem
{
    /// <summary>
    /// Define Translation item
    /// </summary>
    /// <param name="text"></param>
    /// <param name="settings"></param>
    /// <returns>string Latex, string ErrorCodes, TranslationArgs Settings</returns>
    public TranslationItem(string text, TranslationArgs settings)
    {
        Latex = text;
        Settings = settings;
    }

    public string Latex { get; internal set; }
    public TranslationArgs Settings { get; private set; }



}
