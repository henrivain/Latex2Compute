using MekLatexTranslationLibrary.Helpers;
using MekLatexTranslationLibrary.Structures;

namespace MekLatexTranslationLibrary.OperatorBuilders;

internal static class LogarithmBuilder
{
    internal static TranslationItem Build(TranslationItem item, LogStartInfo lsi)
    {
        string inp = item.Latex;

        // before, basis, antilog, after
        LogItem logItem = new();

        inp = inp.Remove(lsi.Index, lsi.StartType.Length);
        
        if (lsi.StartType == "\\ln")
        {
            return TranslateNaturalLog(inp, item, lsi);
        }

        logItem.TextBefore = inp[..lsi.Index];

        ContentAndEnd baseInfo = GetBase(inp, lsi, ref item);
        logItem.Base = baseInfo.Content;

        ContentAndEnd antilogInfo = HelperAlgorithms.GetExpressionAfterOperator(inp[(baseInfo.EndIndex)..]);    // increment by one to skip to next char

        logItem.Antilog = antilogInfo.Content;
        logItem.TextAfter = inp[(antilogInfo.EndIndex + baseInfo.EndIndex)..];    // starting from the end of antilog end

        item.Latex = $"{logItem.TextBefore}#111#({logItem.Antilog},{logItem.Base}){logItem.TextAfter}";
        return item;
    }


    private static TranslationItem TranslateNaturalLog(string inp, TranslationItem item, LogStartInfo lsi)
    {
        LogItem logItem = new();
        logItem.TextBefore = inp[..lsi.Index];
        
        ContentAndEnd antilogInfo = HelperAlgorithms.GetExpressionAfterOperator(inp[lsi.Index..]);
        
        logItem.Antilog = antilogInfo.Content;
        logItem.TextAfter = inp[(antilogInfo.EndIndex + lsi.Index)..];

        item.Latex = $"{logItem.TextBefore}#112#({logItem.Antilog}){logItem.TextAfter}";
        return item;
    }

    /// <summary>
    /// Get Base
    /// </summary>
    /// <param name="inp"></param>
    /// <param name="lsi"></param>
    /// <param name="item"></param>
    /// <returns>(Content, EndIndex)</returns>
    private static ContentAndEnd GetBase(string inp, LogStartInfo lsi, ref TranslationItem item)
    {
        // gets logarithm base from input string
        if (!lsi.HasBase)
        {
            ContentAndEnd temp = HelperAlgorithms.CheckAndGetInconsistentStart(inp, lsi.Index, "log", "_{");
            return ValidateBase(temp, lsi, ref item);
        }
        return new(lsi.Index, lsi.Base);
    }

    private static ContentAndEnd ValidateBase(ContentAndEnd temp, LogStartInfo lsi, ref TranslationItem item)
    {
        // validates or edits base if needed
        if (temp.EndIndex != -1)
        {
            // move to next index from start
            return new ContentAndEnd(temp.EndIndex + 1, temp.Content);
        }
        item.ErrorCodes += "virhe5";
        if (Translation.LatexInDevelopment) Console.WriteLine("virhe5");
        return new(lsi.Index + 1, "10");    // set base to 10

    }
}
