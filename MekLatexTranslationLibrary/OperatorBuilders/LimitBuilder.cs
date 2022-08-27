/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;
using MekLatexTranslationLibrary.Structures;

namespace MekLatexTranslationLibrary.OperatorBuilders;

internal static class LimitBuilder
{
    // call recursion after bracketContentInfo.Content is initialized (like integral)

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
        if (bottomEnd is -1)
        {
            bottomEnd = start - 1;
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
        
       
        if (Slicer.GetSpanSafely(bottom, ^1..) == ",")
        {
            bottom += "å";
        }
        if (Slicer.GetSpanSafely(bottom, ..1) == ",")
        {
            bottom = $"x{bottom}";
        }
        if (bottom.IndexOf(",") is -1)
        {

            bottom += ",å";
        }
        string body = inp[(bottomEnd + 1)..];
        ContentAndEnd bracketContentInfo = HelperAlgorithms.GetExpressionAfterOperator(body);
       
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
