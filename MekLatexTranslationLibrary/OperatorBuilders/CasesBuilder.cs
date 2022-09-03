using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary.OperatorBuilders;

internal static class CasesBuilder
{
    const string OperatorStart = "\\begin{cases}";
    const string OperatorEnd = "\\end{cases}";
    const string Tag = "#121#";

    struct Cases
    {
        public Cases() { }

        public string TextBefore { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string TextAfter { get; set; } = string.Empty;

        public override string ToString() => $"{TextBefore}{Tag}({Body}){TextAfter}";
    }

    public static string BuildAll(string input, ref List<TranslationError> errors)
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

    private static string Build(string input, int startIndex, ref List<TranslationError> errors)
    {
        input = input.Replace("\\\\", "#122#");
        input = input.Replace("&", string.Empty);

        Cases instance = new()
        {
            TextBefore = input[..startIndex]
        };

        int bodyEndIndex = BracketHandler.FindBrackets(input, BracketType.CasesStartEnd, startIndex);

        if (bodyEndIndex < 0)
        {
            instance.Body = BuildAll(input[startIndex..], ref errors);
            Helper.TranslationError(TranslationError.Cases_NoEndBracketFound, ref errors);
        }
        else
        {
            instance.Body = BuildAll(input[startIndex..(bodyEndIndex - OperatorEnd.Length)], ref errors);
            instance.TextAfter = Slicer.GetSpanSafely(input, bodyEndIndex..);
        }

        return instance.ToString();
    }


   
}
