/// Copyright 2024 Henri Vainio 

namespace Latex2Compute;


/// <summary>
/// input type to translation 
/// </summary>
/// <remarks>
/// (string text, TranslationArgs settings)
/// </remarks>
public readonly struct TranslationItem
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

    public string Latex { get; }
    public TranslationArgs Settings { get; }



}
