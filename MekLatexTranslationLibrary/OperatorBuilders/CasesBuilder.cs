using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary.OperatorBuilders;

internal static class CasesBuilder
{
    internal static TranslationItem Build(TranslationItem item)
    {
        // Find \cases and turn it into system()
        string inp = item.Latex;
        string erCodes = item.ErrorCodes;

        string newCasesStart = "¤";
        string newCasesEnd = "&";

        inp = inp.Replace("\\\\", "#122#");
        inp = inp.Replace("&", "");
        inp = inp.Replace("¤", "");
        inp = inp.Replace("\\begin{cases}", newCasesStart);
        inp = inp.Replace("\\end{cases}", newCasesEnd);

        while (inp.Contains(newCasesStart) && inp.Contains(newCasesEnd))
        {
            inp = Compile(inp, ref erCodes, newCasesStart);
        }

        inp = inp.Replace("&", "");  //remove ends before start
        item.Latex = inp;
        item.ErrorCodes = erCodes;
        return item;
    }


    private static string Compile(string inp, ref string erCodes, string newCasesStart)
    {
        // can't reach this point if doesn't have both: \begin and \end
        int start = inp.IndexOf(newCasesStart);
        int end = HandleBracket.FindBrackets(inp, "¤&", start + 1);
        if (end != -1)
        {
            //compile normal
            return $"{inp[..start]}#121#({inp.AsSpan(start + 1, end - start - 1)}){inp[(end + 1)..]}";
        }
        //if end is set before start (end = inp.end)
        erCodes += "virhe26";
        
        Helper.DevPrintTranslationError("virhe26");
        
        return $"{inp[..start]}#121#({inp[(start + 1)..]})";
    }
}
