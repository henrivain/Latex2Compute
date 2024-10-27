using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary.OperatorBuilders;

internal static partial class CasesBuilder
{
    const string OperatorStart = "\\begin{cases}";
    const string OperatorEnd = "\\end{cases}";
    const string NormalTag = "#121#";
    const string RowChangeTag = "#122#";
    const string PiecedTag = "#123#";

    public static string BuildAll(string input, ref List<TranslationErrors> errors)
    {
        int startIndex;
        while (true)
        {
            startIndex = input.IndexOf(OperatorStart);
            if (startIndex < 0)
            {
                return input;
            }

            input = input.Remove(startIndex, OperatorStart.Length);
            input = Build(input, startIndex, ref errors);
        }
    }

    private static string Build(string input, int startIndex, ref List<TranslationErrors> errors)
    {
        input = input.Replace(@"\\", RowChangeTag);

        Cases instance = new()
        {
            TextBefore = input[..startIndex]
        };

        int bodyEndIndex = BracketHandler.FindBrackets(input, BracketType.CasesStartEnd, startIndex);

        if (bodyEndIndex < 0)
        {
            instance.Body = BuildAll(input[startIndex..], ref errors);
            Helper.TranslationError(TranslationErrors.Cases_NoEndBracketFound, ref errors);
        }
        else
        {
            instance.Body = BuildAll(input[startIndex..(bodyEndIndex - OperatorEnd.Length)], ref errors);
            instance.TextAfter = Slicer.GetSpanSafely(input, bodyEndIndex..);
        }
        return instance.ToString();
    }
}
