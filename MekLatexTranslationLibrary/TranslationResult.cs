/// Copyright 2021 Henri Vainio 

namespace MekLatexTranslationLibrary;

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
    internal TranslationResult(string result, TranslationErrors erCodes)
    {
        Result = result;
        ErrorFlags = erCodes;
    }
    public string Result { get; private set; }
    public TranslationErrors ErrorFlags { get; private set; }
}
