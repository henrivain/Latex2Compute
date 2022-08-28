/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;
using MekLatexTranslationLibrary.Structures;

namespace MekLatexTranslationLibrary.OperatorBuilders;

internal static class LimitBuilder
{
    // call recursion after body.Content is initialized (like integral)

    private static string Tag { get; } = "#151#";
    private static string OperatorStart { get; } = "\\lim";
    
    private static string Build(string input, int startIndex, ref List<TranslationError> errors)
    {

        var bottomInfo = HelperAlgorithms.CheckAndGetInconsistentStart(input, startIndex, "lim", "_{");
        int bottomEnd = bottomInfo.EndIndex;
        string bottom = bottomInfo.Content;

        if (bottomEnd is -1)
        {
            bottomEnd = startIndex - 1;
            errors.Add(TranslationError.Lim_NoApproachValue);
            Helper.DevPrintTranslationError(nameof(TranslationError.Lim_NoApproachValue));
        }

        bottom = ReplaceArrowWithComma(bottom);
        bottom = AddMissingParts(bottom);

        ContentAndEnd body = HelperAlgorithms.GetExpressionAfterOperator(input[(bottomEnd + 1)..]);
        string afterBody = body.EndIndex is -1 ? string.Empty : input[(bottomEnd + body.EndIndex + 1)..];
        
        return $"{input[..startIndex]}{Tag}({BuildAll(body.Content, ref errors)},{bottom}){afterBody}";
    }
    

    internal static TranslationItem Build(TranslationItem item)
    {
        // Find limit and translate it "\\lim_(x\to y)(z)" => lim(z,x,y)
        string inp = item.Latex;

        int start = inp.IndexOf("\\lim");
        inp = inp.Remove(start, 4);

        ContentAndEnd bottomInfo = HelperAlgorithms.CheckAndGetInconsistentStart(inp, start, "lim", "_{");
        string bottom = bottomInfo.Content;
        int bottomEnd = bottomInfo.EndIndex;
        if (bottomEnd is -1)
        {
            bottomEnd = start - 1;
            item.ErrorCodes += "virhe6";
            Helper.DevPrintTranslationError("virhe6");
        }

        bottom = ReplaceArrowWithComma(bottom);
        bottom = AddMissingParts(bottom);

        ContentAndEnd bracketContentInfo = HelperAlgorithms.GetExpressionAfterOperator(inp[(bottomEnd + 1)..]);

        string content = bracketContentInfo.Content;
        int end = bracketContentInfo.EndIndex;

        if (end != -1)
        {
            // if all found [with content]
            item.Latex = $"{inp[..start]}{Tag}({content},{bottom}){inp[(bottomEnd + end + 1)..]}";
            return item;
        }
        // if no content brackets
        item.Latex = $"{inp[..start]}{Tag}({inp[(bottomEnd + 1)..]},{bottom})";
        return item;
    }
    
    internal static string BuildAll(string input, ref List<TranslationError> errors)
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
