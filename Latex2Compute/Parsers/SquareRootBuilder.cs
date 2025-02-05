using Latex2Compute.Helpers;

namespace Latex2Compute.Parsers;
internal class SquareRootBuilder
{
    const string OperatorStart = "\\sqrt{"; 

    internal static string BuildAll(string input, ref Errors errors)
    {
        // Find and change "\\sqrt{}" => "sqrt()"
        int startIndex;
        while (true)
        {
            startIndex = input.IndexOf(OperatorStart);
            if (startIndex < 0)
            {
                return input;
            }

            input = input.Remove(startIndex, OperatorStart.Length - 1);
            input = Build(input, startIndex, ref errors);
        }
    }
    internal static string Build(string input, int startIndex, ref Errors errors)
    {
        var body = BracketHandler.GetCharsBetweenBrackets(input, BracketType.Curly, startIndex);
        if (body.EndIndex < 0)
        {
            errors |= Errors.Sqrt_NoStartBracketFound;
            Helper.PrintError(Errors.Sqrt_NoStartBracketFound);
            body.EndIndex = input.Length;
            body.Content = input[startIndex..];
        }

        string textAfter = Slicer.GetSpanSafely(input, body.EndIndex..);
        return $"{input[..startIndex]}{ConstSymbol.Sqrt}({body.Content}){textAfter}";
    }
}
