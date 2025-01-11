/// Copyright 2021 Henri Vainio 
using Latex2Compute.Structures;

namespace Latex2Compute.Helpers;

/// <summary>
/// Read complex Latex structure and return contents and end index ( compile structure "_{}^{}" ) 
/// <para/>symbolType use: "sum" or "integral"
/// </summary>
/// <returns>int EndIndex, string FirstContent, string SecondContent, string TextBefore</returns>
internal class ComplexSymbolReader
{
    internal enum SymbolType { Sum, Integral }

    readonly Dictionary<SymbolType, TwoStrings> _symbolTypes = new()
    {
        [SymbolType.Sum] = new TwoStrings("sumS", "sumE"),
        [SymbolType.Integral] = new TwoStrings("int", "int")
    };

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
    /// <returns>int EndIndex, string FirstContent, string SecondContent, string TextBefore</returns>
    internal ComplexSymbolReader(string inp, int start, SymbolType symbolType)
    {
        TwoStrings types = _symbolTypes[symbolType];
        ContentAndEnd bottomInfo = HelperAlgorithms.CheckAndGetInconsistentStart(inp, start, types.First, "_{");
        int bottomEndIndex = (bottomInfo.EndIndex is -1) ? start : bottomInfo.EndIndex;
        ContentAndEnd topInfo = HelperAlgorithms.CheckAndGetInconsistentStart(inp, bottomEndIndex + 1, types.Second, "^{");

        End = topInfo.EndIndex is -1 ? 
            bottomEndIndex : topInfo.EndIndex;
        
        TextBefore = inp[..start];
        BottomContent = bottomInfo.Content;
        TopContent = topInfo.Content;
    }
}
