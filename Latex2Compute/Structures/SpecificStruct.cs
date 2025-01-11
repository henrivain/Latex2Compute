/// Copyright 2021 Henri Vainio 
using Latex2Compute.Helpers;

namespace Latex2Compute.Structures;

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
/// GetBottom information of next Latex logarithm start inside given start
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
    internal SumInfo SetReaderInfo(ComplexSymbolReader reader, ref TranslationErrors errors)
    {
        TextBefore = reader.TextBefore;            
        Top = reader.TopContent;
        
        int equalDivider = reader.BottomContent.IndexOf('=');
        if (equalDivider is -1)
        {
            errors |= TranslationErrors.Sum_NoVariableFound;
            Helper.PrintError(TranslationErrors.Sum_NoVariableFound);
            Bottom = $"n,{reader.BottomContent}";
            return this;
        }
        Bottom = $"{reader.BottomContent[..equalDivider]},{reader.BottomContent[(equalDivider + 1)..]}";
        return this;
    }


    public string TextBefore { get; set; } = string.Empty;
    public string Equation { get; set; } = string.Empty;
    public string Bottom { get; set; } = string.Empty;
    public string Top { get; set; } = string.Empty;
    public string TextAfter { get; set; } = string.Empty;
}
