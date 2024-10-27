/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;
using MekLatexTranslationLibrary.Structures;

namespace MekLatexTranslationLibrary.OperatorBuilders;

internal static class SumBuilder
{
    const string OperatorStart = "\\sum";
    const string Tag = "∑";

    internal static string Build(string input, int startIndex, ref TranslationErrors errors)
    {
        ComplexSymbolReader reader = new(input, startIndex, "sum");
        SumInfo sumInfo = new SumInfo().SetReaderInfo(reader, ref errors);
        sumInfo = AddExpressionAfter(input, sumInfo, reader, ref errors);
        return $"{sumInfo.TextBefore}{Tag}({sumInfo.Equation},{sumInfo.Bottom},{sumInfo.Top}){sumInfo.TextAfter}";
    }

    public static string BuildAll(string input, ref TranslationErrors errors)
    {
        while (true)
        {
            int startIndex = input.IndexOf(OperatorStart);
            if (startIndex < 0) return input;

            input = input.Remove(startIndex, 4);
            input = Build(input, startIndex, ref errors);
        }
    }

    /// <summary>
    /// Checks if input can be read onward
    /// </summary>
    /// <param name="inp"></param>
    /// <param name="sumInfo"></param>
    /// <param name="reader"></param>
    /// <returns>updated sumInfo if needed</returns>
    private static SumInfo AddExpressionAfter(
        string inp, SumInfo sumInfo, ComplexSymbolReader reader, ref TranslationErrors errors)
    {
        if (inp.Length > reader.End + 1)
        {
            ContentAndEnd endEquation = HelperAlgorithms.GetExpressionAfterOperator(inp[(reader.End + 1)..]);
            sumInfo.Equation = BuildAll(endEquation.Content, ref errors);
            sumInfo.TextAfter = inp[(reader.End + endEquation.EndIndex + 1)..];
        }
        return sumInfo;
    }
} 
