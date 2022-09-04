using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary.OperatorBuilders;
internal class SquareRootBuilder
{
    const string Tag = "#142#";

    const string OperatorStart = "\\sqrt{"; 

    internal static string BuildAll(string input, ref List<TranslationError> errors)
    {
        // Find and change "\\sqrt{}" => "sqrt()"
        int startIndex;
        while (true)
        {
            startIndex = input.IndexOf(OperatorStart);
            if(startIndex < 0) return input;

            input = input.Remove(startIndex, OperatorStart.Length - 1);
            input = Build(input, startIndex, ref errors);
        }
    }
    internal static string Build(string input, int startIndex, ref List<TranslationError> errors)
    {
        var body = BracketHandler.GetCharsBetweenBrackets(input, BracketType.Curly, startIndex);
        if (body.EndIndex < 0)
        {
            Helper.TranslationError(TranslationError.Sqrt_NoStartBracketFound, ref errors);
            body.EndIndex = input.Length;
            body.Content = input[startIndex..];
        }

        string textAfter = Slicer.GetSpanSafely(input, body.EndIndex..);
        return $"{input[..startIndex]}{Tag}({body.Content}){textAfter}";
    }
}
