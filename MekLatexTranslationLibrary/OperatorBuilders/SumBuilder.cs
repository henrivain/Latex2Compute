/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;
using MekLatexTranslationLibrary.Structures;

namespace MekLatexTranslationLibrary.OperatorBuilders;

internal static class SumBuilder
{
    /// <summary>
    /// Translate one Sum
    /// </summary>
    /// <param name="inp"></param>
    /// <param name="startIndex"></param>
    /// <param name="item"></param>
    /// <returns>inp with one more translated Sum</returns>
    internal static string Build(string inp, int startIndex, ref TranslationItem item)
    {
        // removed er codes: virhe10

        SumInfo sumInfo = new();

        // get textBefore, bottom and top
        ComplexSymbolReader reader = new(inp, startIndex, "sum");
        sumInfo.SetReaderInfo(reader, ref item);    // ref item for error: "virhe11", means "no var in bottom" => use n as var
        DefineExpressionAfter(inp, ref sumInfo, reader);

        return $"{sumInfo.TextBefore}∑({sumInfo.Equation},{sumInfo.Bottom},{sumInfo.Top}){sumInfo.TextAfter}";
    }


    /// <summary>
    /// Checks if input can be read onward
    /// </summary>
    /// <param name="inp"></param>
    /// <param name="sumInfo"></param>
    /// <param name="reader"></param>
    /// <returns>updated sumInfo if needed</returns>
    private static void DefineExpressionAfter(string inp, ref SumInfo sumInfo, ComplexSymbolReader reader)
    {
        // if input can be read onward
        if (inp.Length > reader.End + 1)
        {
            ContentAndEnd endEquation = HelperAlgorithms.GetExpressionAfterOperator(inp[(reader.End + 1)..]);
            sumInfo.Equation = endEquation.Content;
            sumInfo.TextAfter = inp[(reader.End + endEquation.EndIndex + 1)..];
            return;
        }
        // don't add anything
    }
} 
