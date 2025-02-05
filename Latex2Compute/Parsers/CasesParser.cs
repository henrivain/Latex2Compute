using Latex2Compute.Helpers;

namespace Latex2Compute.Parsers;

internal static partial class CasesParser
{
    const string OperatorStart = "\\begin{cases}";
    const string OperatorEnd = "\\end{cases}";



    public static string BuildAll(string input, ref Errors errors)
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

    private static string Build(string input, int startIndex, ref Errors errors)
    {
        input = input.Replace(@"\\", ConstSymbol.SystemRowChange);

        Cases instance = new()
        {
            TextBefore = input[..startIndex]
        };

        int bodyEndIndex = BracketHandler.FindBrackets(input, BracketType.CasesStartEnd, startIndex);

        if (bodyEndIndex < 0)
        {
            instance.Body = BuildAll(input[startIndex..], ref errors);

            errors |= Errors.Cases_NoEndBracketFound;
            Helper.PrintError(Errors.Cases_NoEndBracketFound);
        }
        else
        {
            instance.Body = BuildAll(input[startIndex..(bodyEndIndex - OperatorEnd.Length)], ref errors);
            instance.TextAfter = Slicer.GetSpanSafely(input, bodyEndIndex..);
        }
        return instance.ToString();
    }
}
