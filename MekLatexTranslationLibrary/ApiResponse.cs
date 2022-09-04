/// Copyright 2021 Henri Vainio 

namespace MekLatexTranslationLibrary;

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