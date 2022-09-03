/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;
namespace MekLatexTranslationLibrary;

/// <summary>
/// Translation algorithms for Matcha io symbols and constructors
/// </summary>
public static class Matcha
{
    public static TranslationItem ConnectToMathchaChanges(TranslationItem item)
    {
        // connect to matcha changes if enabled in settings
        if (item.Settings.MatchaEnabled)
        {
            MakeMatchaChanges(item);
        }
        return item;
    }

    private static TranslationItem MakeMatchaChanges(TranslationItem item)
    {
        // method removes unuseful latex groups that matcha.io adds to their latex documents

        string inp = item.Latex;

        inp = inp.Replace("\\displaystyle", "");
        inp = inp.Replace("$", "");
        item.Latex = inp;

        while (item.Latex.Contains("\\end{") || item.Latex.Contains("\\begin{"))
        {
            int start = item.Latex.IndexOf("\\end{");
            if (start == -1)
            {
                // if contains begin instead of end
                start = item.Latex.IndexOf("\\begin{");
                item.Latex = item.Latex.Remove(start, 7);
            }
            else
            {
                // remove end
                item.Latex = item.Latex.Remove(start, 5);
            }
            item = RemoveConstructorArgs(item, start);
        }
        return item;
    }

    private static TranslationItem RemoveConstructorArgs(TranslationItem item, int start)
    {
        // remove arguments from macha io document constructors or math field definers
        int end = BracketHandler.FindBrackets(item.Latex, "{}", start);

        if (end == -1)
        {
            // no end to matcha page constructor => skip
            item.ErrorCodes += "virhe4";
        }
        else
        {
            item.Latex = item.Latex.Remove(start, end - start + 1);
        }


        return item;
    }
}
