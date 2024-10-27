/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;
using MekLatexTranslationLibrary.Structures;

namespace MekLatexTranslationLibrary.OperatorBuilders;

internal static class LimitBuilder
{
    // call recursion after body.Content is initialized (like integral)

    const string Tag = "#151#";
    const string OperatorStart = "\\lim";

    public static string BuildAll(string input, ref TranslationErrors errors)
    {
        int startIndex;

        while (true)
        {
            startIndex = input.IndexOf(OperatorStart);
            if (startIndex < 0) return input;

            input = input.Remove(startIndex, OperatorStart.Length);
            input = Build(input, startIndex, ref errors);
        }
    }

    internal static string Build(string input, int startIndex, ref TranslationErrors errors)
    {

        var bottomInfo = HelperAlgorithms.CheckAndGetInconsistentStart(input, startIndex, "lim", "_{");
        int bottomEnd = bottomInfo.EndIndex;
        string bottom = bottomInfo.Content;

        if (bottomEnd is -1)
        {
            bottomEnd = startIndex - 1;
            errors |= TranslationErrors.Lim_NoApproachValue;
            Helper.PrintError(TranslationErrors.Lim_NoApproachValue);
        }

        bottom = ReplaceArrowWithComma(bottom);
        bottom = AddMissingParts(bottom);

        ContentAndEnd body = HelperAlgorithms.GetExpressionAfterOperator(input[(bottomEnd + 1)..]);
        string afterBody = body.EndIndex is -1 ? string.Empty : input[(bottomEnd + body.EndIndex + 1)..];
        
        return $"{input[..startIndex]}{Tag}({BuildAll(body.Content, ref errors)},{bottom}){afterBody}";
    }
    
    private static string ReplaceArrowWithComma(string bottom)
    {
        string[] arrows = new string[]
        {
            "\\rightarrow", "\\to"
        };

        foreach (var arrow in arrows)
        {
            int arrowIndex = bottom.IndexOf(arrow);
            if (arrowIndex >= 0)
            {
                return bottom
                    .Remove(arrowIndex, arrow.Length)
                    .Insert(arrowIndex, ",");
            }
        }
        return bottom;
    }
    private static string AddMissingParts(string bottom)
    {
        if (Slicer.GetSpanSafely(bottom, ^1..) == ",")
        {
            bottom += "å";
        }
        if (Slicer.GetSpanSafely(bottom, ..1) == ",")
        {
            bottom = $"x{bottom}";
        }
        if (bottom.IndexOf(",") is -1)
        {
            bottom += ",å";
        }
        return bottom;
    }
}
