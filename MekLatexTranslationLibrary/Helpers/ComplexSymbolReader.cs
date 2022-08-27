/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Structures;

namespace MekLatexTranslationLibrary.Helpers;

/// <summary>
/// Read complex Latex structure and return contents and end index ( compile structure "_{}^{}" ) 
/// <para/>symbolType use: "sum" or "integral"
/// </summary>
/// <returns>int End, string FirstContent, string SecondContent, string TextBefore</returns>
internal class ComplexSymbolReader
{
    internal int End { get; private set; } = -1;
    internal string TextBefore { get; private set; } = string.Empty;
    internal string BottomContent { get; private set; } = string.Empty;
    internal string TopContent { get; private set; } = string.Empty;

    /// <summary>
    /// Read complex Latex structure and return contents and end index ( compile structure "_{}^{}" ) 
    /// <para/>symbolType use: "sum" or "integral"
    /// </summary>
    /// <param name="inp"></param>
    /// <param name="start"></param>
    /// <param name="symbolType"></param>
    /// <returns>int End, string FirstContent, string SecondContent, string TextBefore</returns>
    internal ComplexSymbolReader(string inp, int start, string symbolType)
    {
        TwoStrings types = GetSymbolType(symbolType);
        ContentAndEnd bottomInfo = HelperAlgorithms.CheckAndGetInconsistentStart(inp, start, types.First, "_{");
        int bottomEndIndex = (bottomInfo.End is -1) ? start : bottomInfo.End;
        ContentAndEnd topInfo = HelperAlgorithms.CheckAndGetInconsistentStart(inp, bottomEndIndex + 1, types.Second, "^{");

        End = (topInfo.End is -1) ? bottomEndIndex : topInfo.End;
        TextBefore = inp[..start];
        BottomContent = bottomInfo.Content;
        TopContent = topInfo.Content;
    }
    private static TwoStrings GetSymbolType(string symbolType)
    {
        return symbolType switch
        {
            "sum" => new("sumS", "sumE"),
            "integral" => new("int", "int"),
            _ => throw new InvalidOperationException("[ComplexSymbolInfo] Given symbol type is not correct; use string value: 'sum' or 'integral' ")
        };
    }
}
