namespace Latex2Compute.Parsers;

internal static class LogarithmParser
{
    public static string BuildAll(string input, ref TranslationErrors errors)
    {
        LogStartInfo lsi;
        while (true)
        {
            lsi = new(input);
            if (lsi.IsFound is false)
            {
                return input;
            }

            input = input.Remove(lsi.Index, lsi.StartType.Length);
            input = Build(input, lsi, ref errors);
        }
    }

    internal static string Build(string input, LogStartInfo lsi, ref TranslationErrors errors)
    {
        if (lsi.StartType == "\\ln")
        {
            return TranslateNaturalLog(input, lsi);
        }

        ContentAndEnd baseInfo = GetBase(input, lsi, ref errors);
        ContentAndEnd antilogInfo = HelperAlgorithms.GetExpressionAfterOperator(input[baseInfo.EndIndex..]);

        LogItem log = new()
        {
            TextBefore = input[..lsi.Index],
            Base = baseInfo.Content,
            Body = BuildAll(antilogInfo.Content, ref errors),
            TextAfter = input[(antilogInfo.EndIndex + baseInfo.EndIndex)..]
        };

        return $"{log.TextBefore}{ConstSymbol.Log}({log.Body},{log.Base}){log.TextAfter}";
    }

    private static string TranslateNaturalLog(string inp, LogStartInfo lsi)
    {
        
        ContentAndEnd antilogInfo = HelperAlgorithms.GetExpressionAfterOperator(inp[lsi.Index..]);

        LogItem logItem = new()
        {
            TextBefore = inp[..lsi.Index],
            Body = antilogInfo.Content,
            TextAfter = inp[(antilogInfo.EndIndex + lsi.Index)..]
        };

        return $"{logItem.TextBefore}{ConstSymbol.NaturalLog}({logItem.Body}){logItem.TextAfter}";
    }

    /// <summary>
    /// GetBottom Base
    /// </summary>
    /// <param name="inp"></param>
    /// <param name="lsi"></param>
    /// <param name="item"></param>
    /// <returns>(Content, EndIndex)</returns>
    private static ContentAndEnd GetBase(string inp, LogStartInfo lsi, ref TranslationErrors errors)
    {
        // gets logarithm base from input string
        if (lsi.HasBase is false)
        {
            ContentAndEnd temp = HelperAlgorithms.CheckAndGetInconsistentStart(inp, lsi.Index, "log", "_{");
            return ValidateBase(temp, lsi, ref errors);
        }
        return new(lsi.Index, lsi.Base);
    }
    private static ContentAndEnd ValidateBase(ContentAndEnd temp, LogStartInfo lsi, ref TranslationErrors errors)
    {
        // validates or edits base if needed
        if (temp.EndIndex != -1)
        {
            // move to next index from start
            return new ContentAndEnd(temp.EndIndex + 1, temp.Content);
        }

        errors |= TranslationErrors.Log_BasisNotFound;
        Helper.PrintError(TranslationErrors.Log_BasisNotFound);

        return new(lsi.Index + 1, "10");    // set base to 10

    }
}
