/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary.OperatorBuilders;

internal static class RootBuilder
{
    /// <summary>
    /// Translate Latex "\sqrt[]{}" to #141#(,) => translate later root(,)
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    internal static TranslationItem Build(TranslationItem item)
    {
        // Find and change "\\sqrt[x]{y}" => "root(x,y)"
        string inp = item.Latex;

        bool inpContinues = true;

        int start = inp.IndexOf("\\sqrt[");
        inp = inp.Remove(start, 6);

        int closingBracket = HandleBracket.FindBrackets(inp, "[]", start);

        if (closingBracket != -1)
        {
            int startBracket = closingBracket + 2;

            string nextChar = HelperAlgorithms.GetNextCharsSafely(inp[(closingBracket + 1)..], 1);

            switch (nextChar)
            {
                case null:
                    // inp end in first closing bracket => skip translation
                    item.ErrorCodes += "virhe24";
                    Helper.DevPrintTranslationError("virhe24");
                    inpContinues = false;
                    break;

                case "{":
                    // everything ok
                    break;

                default:
                    // no "{" -bracket => take that char to count too
                    item.ErrorCodes += "virhe23";
                    Helper.DevPrintTranslationError("virhe23");
                    startBracket = closingBracket + 1;
                    break;
            }

            if (inpContinues)
            {
                string erCodes = item.ErrorCodes;
                Compile(ref inp, ref erCodes, start, closingBracket, startBracket);
                item.ErrorCodes = erCodes;
            }
            item.Latex = inp;
            return item;
        }

        // no first closing bracket => skip
        item.ErrorCodes += "virhe16";
        Helper.DevPrintTranslationError("virhe16");
        item.Latex = inp;
        return item;
    }

    private static void Compile(ref string inp, ref string erCodes, int start, int closingBracket, int startBracket)
    {
        // complile final result
        int end = HandleBracket.FindBrackets(inp, "{}", startBracket);

        if (end != -1)
        {
            inp = $"{ inp[..start] }#141#({ inp[startBracket..end] },{ inp[start..closingBracket] }){ inp[(end + 1)..] }";
            return;
        }
        // no ending bracket => [end = inp.end]
        erCodes += "virhe17";
        Helper.DevPrintTranslationError("virhe17");
        inp = $"{ inp[..start] }#141#({ inp[startBracket..] },{ inp[start..closingBracket] })";
    }
}
