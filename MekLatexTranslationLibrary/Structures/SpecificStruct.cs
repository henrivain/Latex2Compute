/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary.Structures;

/// <summary>
/// Store values to during the logarithm translation
/// </summary>
/// <returns>
/// string TextBefore, string Basis, string Body, string TextAfter
/// </returns>
internal struct LogItem
{
    public LogItem() { }
    internal string TextBefore { get; set; } = string.Empty;
    internal string Base { get; set; } = string.Empty;
    internal string Body { get; set; } = string.Empty;
    internal string TextAfter { get; set; } = string.Empty;
}

/// <summary>
/// Get information of next Latex logarithm start inside given start
/// </summary>
/// <returns>bool IsFound, bool HasBase, int Index, string StartType, string Base</returns>
internal struct LogStartInfo
{
    /// <summary>
    /// Check if any defined log operators in input
    /// </summary>
    /// <param name="input"></param>
    /// <returns>LogStartInfo with defined values</returns>
    internal LogStartInfo(string input)
    {
        string[] logs = { "\\log", "\\lg", "\\lb", "\\ln" };
        Validate(input, logs);
    }

    private void Validate(string input, string[] starts)
    {
        // change properties if has found any start
        foreach (string start in starts)
        {
            int i = input.IndexOf(start);
            if (i != -1)
            {
                Index = i;
                StartType = start;
                IsFound = true;
                GetSubstitute(start);
                return;
            }
        }

    }

    private void GetSubstitute(string start)
    {
        (Base, HasBase) = start switch
        {
            "\\lg" => ("10", true),
            "\\lb" => ("2", true),
            "\\ln" => ("none", true),
            _ => (string.Empty, false)
        };


    }

    internal bool IsFound { get; private set; } = false;
    internal bool HasBase { get; private set; } = false;
    internal int Index { get; private set; } = 0;
    internal string StartType { get; private set; } = string.Empty;
    internal string Base { get; private set; } = string.Empty;

}

/// <summary>
/// Save data during sum translation
/// </summary>
/// <returns>[all string] TextBefore, Equation, Bottom, Top, TextAfter</returns>
internal struct SumInfo
{
    public SumInfo() { }

    /// <summary>
    /// Set Start, Bottom and Top values automatically from given ComplexSymbolReader
    /// <para/>Changes bottom to right format to use variable
    /// </summary>
    /// <param name="reader"></param>
    internal void SetReaderInfo(ComplexSymbolReader reader, ref TranslationItem item)
    {
        TextBefore = reader.TextBefore;            
        Top = reader.TopContent;
        SetBottom(reader.BottomContent, ref item);
    }

    /// <summary>
    /// Set Bottom with extracted variable
    /// </summary>
    /// <param name="bottomContent"></param>
    /// <param name="item"></param>
    private void SetBottom(string bottomContent, ref TranslationItem item)
    {
        int equalDivider = bottomContent.IndexOf('=');
        if (equalDivider is -1)
        {
            if (Translation.LatexInDevelopment) Console.WriteLine("[SumInfo.SetBottom] virhe11");
            Bottom = $"n,{bottomContent}";
            item.ErrorCodes += "virhe11";
            return;
        }
        Bottom = $"{bottomContent[..equalDivider]},{bottomContent[(equalDivider + 1)..]}";
    }

    public string TextBefore { get; set; } = string.Empty;
    public string Equation { get; set; } = string.Empty;
    public string Bottom { get; set; } = string.Empty;
    public string Top { get; set; } = string.Empty;
    public string TextAfter { get; set; } = string.Empty;
}
