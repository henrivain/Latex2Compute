/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;
using MekLatexTranslationLibrary.Structures;

namespace MekLatexTranslationLibrary.OperatorBuilders;

internal static class LimitBuilder
{
    internal static TranslationItem Build(TranslationItem item)
    {
        // Find limit and translate it "\\lim_(x\to y)(z)" => lim(z,x,y)
        string inp = item.Latex;

        int start = inp.IndexOf("\\lim");
        inp = inp.Remove(start, 4);

        ContentAndEnd bottomInfo = HelperAlgorithms.CheckAndGetInconsistentStart(inp, start, "lim", "_{");
        string bottom = bottomInfo.Content;
        int bottomEnd = bottomInfo.End;
        int separatorIndex;
        if (bottomEnd == -1)
        {
            // no bottom ending bracket "\\right)" 
            // possible bottom => content
            bottom = "x,å";
            bottomEnd = start;
            item.ErrorCodes += "virhe6";
            Helper.DevPrintTranslationError("virhe6");
        }

        if (bottom.Contains("\\rightarrow"))
        {
            separatorIndex = bottom.IndexOf("\\rightarrow");
            bottom = bottom.Remove(separatorIndex, 11);
            bottom = bottom.Insert(separatorIndex, ",");
        }
        else if (bottom.Contains("\\to"))
        {
            separatorIndex = bottom.IndexOf("\\to");
            bottom = bottom.Remove(separatorIndex, 3);
            bottom = bottom.Insert(separatorIndex, ",");
        }

        ContentAndEnd bracketContentInfo = HelperAlgorithms.GetExpressionAfterOperator(inp[(bottomEnd + 1)..]);
            
            //HandleBracket.GetCharsBetweenBrackets(inp, bottomEnd + 1);
        string content = bracketContentInfo.Content;
        int end = bracketContentInfo.End;

        if (end != -1)
        {
            // if all found [with content]
            item.Latex = $"{inp[..start]}#151#({content},{bottom}){inp[(bottomEnd + end + 1)..]}";
            return item;
        }
        // if no content brackets
        item.Latex = $"{inp[..start]}#151#({inp[(bottomEnd + 1)..]},{bottom})";
        return item;

    }
}
