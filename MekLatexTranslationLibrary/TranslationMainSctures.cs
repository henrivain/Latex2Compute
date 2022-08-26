/// Copyright 2021 Henri Vainio 

namespace MekLatexTranslationLibrary;


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
        ErrorCodes = "";
        Settings = settings;
    }

    internal string Latex { get; set; }
    public string ErrorCodes { get; internal set; }
    public TranslationArgs Settings { get; private set; }
}



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

/// <summary>
/// result type to translation 
/// </summary>
/// <returns>
/// (string Result, string ErrorCodes)
/// </returns>
public struct TranslationResult
{
    /// <summary>
    /// get TranslationResult with set values
    /// </summary>
    /// <param name="result"></param>
    /// <param name="erCodes"></param>
    /// <returns>string Result, string ErrorCodes</returns>
    internal TranslationResult(string result, string erCodes)
    {
        Result = result;
        ErrorCodes = erCodes;
    }
    public string Result { get; private set; }
    public string ErrorCodes { get; private set; }
}

/// <summary>
/// Api response type for Mek
/// </summary>
public struct ApiResponse
{
    /// <summary>
    /// Api response from translation algorithms
    /// </summary>
    /// <returns>"no latex error" if not successful</returns>
    public ApiResponse(bool successfull)
    {

        Successfull = successfull;
        if (successfull is false)
        {
            // successfull = false
            // if no latex this is returned
            ErrorMsg = "Could not execute translation (no latex)";
            Text = null;
            return;
        }
        Text = null;
        ErrorMsg = null;
    }
    public bool Successfull { get; set; } = true;
    public string? ErrorMsg { get; set; }
    public string? Text { get; set; }
}