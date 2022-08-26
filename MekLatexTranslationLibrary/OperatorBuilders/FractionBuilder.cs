/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary.OperatorBuilders;

internal class FractionBuilder
{
    internal static TranslationItem Build(TranslationItem item, int startBracket)
    {
        // translates end of the fraction with information about input and starting bracket
        // called from method Fraction
        string inp = item.Latex;
        inp = inp.Insert(startBracket, "(");
        int pairForFirst = HandleBracket.FindBrackets(inp, "{}", startBracket);
        if (pairForFirst != -1)
        {
            // first bracket and pair for it was found
            inp = inp.Remove(pairForFirst, 1);
            inp = inp.Insert(pairForFirst, ")");
            try
            {
                if (inp.Substring(pairForFirst + 1, 1) == "{")
                {
                    // end found
                    inp = inp.Remove(pairForFirst + 1, 1);
                    inp = inp.Insert(pairForFirst + 1, "/(");
                    int pairForSecond = HandleBracket.FindBrackets(inp, "{}", pairForFirst + 2);
                    if (pairForSecond != -1)
                    {
                        inp = inp.Remove(pairForSecond, 1);
                        inp = inp.Insert(pairForSecond, ")");
                    }
                    else
                    {
                        //no start for second part => closing = inpEnd
                        item.ErrorCodes += "virhe3";
                        Helper.DevPrintTranslationError("virhe3");
                        inp += ")";
                    }
                }
                else
                {
                    //no start bracket for second part => compile first part and don't do second
                    item.ErrorCodes += "virhe2";
                    Helper.DevPrintTranslationError("virhe2");
                    inp = inp.Insert(pairForFirst + 1, "/(");
                    int pairForSecond = HandleBracket.FindBrackets(inp, "{}", pairForFirst + 3);
                    if (pairForSecond != -1)
                    {
                        // if still has ending bracket
                        inp = inp.Remove(pairForSecond, 1);
                        inp = inp.Insert(pairForSecond, ")");
                    }
                    else
                    {
                        inp += ")";
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // inp too short => can't translate
                item.ErrorCodes += "virhe22";
                Helper.DevPrintTranslationError("virhe22");
            }
            item.Latex = inp;
            return item;
        }
        //no pair for start bracket => stop translation for this fraction
        item.ErrorCodes += "virhe1";
        Helper.DevPrintTranslationError("virhe1");

        item.Latex = inp;
        return item;
    }

}
