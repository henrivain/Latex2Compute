/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Structures;

namespace MekLatexTranslationLibrary.Helpers;

internal static class HelperAlgorithms
{
    /// <summary>
    /// GetBottom content and end index of latex span (example _{34} => (5, 34))
    /// </summary>
    /// <param name="input"></param>
    /// <param name="startIndex"></param>
    /// <param name="symbol"></param>
    /// <param name="separator"></param>
    /// <returns>(int EndIndex, string Content), if no content, returns (Length or -1, Placeholder / inputEnd)</returns>
    internal static ContentAndEnd CheckAndGetInconsistentStart(string input, int startIndex, string symbol, string separator = "_{")
    {
        /*  get starting of more complex operators, for example log, lim, integral, sum.
            input [inp, symbol, separator]  symbol => (lim, log, int, sumS, sumE)   
            separator => ["^{" or "_{"]
            output [content, endpoint(-1 if none)] */

        string shortSeparator = GetShortSeparator(separator);

        //check if starts with long version
        if (Slicer.GetSpanSafely(input, startIndex, 2) == separator)
        {
            return LongInconsistentStart(input, startIndex, symbol);
        }

        //check if starts with short version
        if (Slicer.GetSpanSafely(input, startIndex, 1) == shortSeparator)
        {
            return ShortInconsistentStart(input, startIndex, symbol);
        }
        //if E.g. base is none
        return new(-1, GetPlaceHolderSymbol(symbol));
    }

    /// <summary>
    /// GetBottom expression that belongs to some Latex operator 
    /// <para/> DO NOT INCLUDE START! (wrong: "\log_2(234)" right: "(234)" ) use inp[index..]
    /// </summary>
    /// <remarks>
    /// example: 
    /// <para/>"23" => "23" 
    /// <para/>"(234)" => "234" 
    /// <para/>"23=4" => "23"
    /// </remarks>
    /// <param name="croppedInput"></param>
    /// <returns>(int endIndex, string content) if found, else substring of inp and length of inp (endIndex is always 0 or bigger)</returns>
    internal static ContentAndEnd GetExpressionAfterOperator(string croppedInput)
    {

        ContentAndEnd info = BracketHandler.GetCharsBetweenBrackets(croppedInput, BracketType.RoundLong, 0);
        if (info.EndIndex != -1) return info;
        return SeparateByOperator(croppedInput);
    }



    /// <summary>
    /// Try to get index of pattern. If ArgumentOutOfRange returns -1
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="startIndex"></param>
    /// <returns>startindex of pattern, -1 if not found, if ArgumentOutOfRange</returns>
    internal static int TryGetIndexOf(string input, string pattern, int startIndex = 0)
    {
        try
        {
            return input.IndexOf(pattern, startIndex);
        }
        catch (ArgumentOutOfRangeException)
        {
            Helper.PrintError($"Given startIndex was out of range; input: {input}, startIndex: {startIndex}");
            return -1;
        }
    }

    /// <summary>
    /// GetBottom text until next =, &lt;, &gt;, +, -, * or ], ), } operator
    /// </summary>
    /// <param name="inp"></param>
    /// <returns>(int EndIndex, string content) if found, else returns input length and input</returns>
    private static ContentAndEnd SeparateByOperator(string inp)
    {

        List<int> values = IndexerAlgorithms.GetFirstOperatorIndexes(inp);

        if (values.Count > 0)
        {
            int end = values.Min();
            string content = inp[..end];
            return new(end, content);
        }
        return new(inp.Length, inp);
    }

    /// <summary>
    /// GetBottom short version of inconsistent start (startsWith _ or ^ )
    /// </summary>
    /// <param name="input"></param>
    /// <param name="startIndex"></param>
    /// <returns>(int EndIndex, string Content), if no content, returns (Length, Placeholder) </returns>
    private static ContentAndEnd ShortInconsistentStart(string input, int startIndex, string symbol)
    {
        startIndex++;   // exclude _ or ^ from input

        string content = Slicer.GetSpanSafely(input, startIndex, 1);
        if (string.IsNullOrEmpty(content))
        {
            // does not continue after _ or ^
            return new(startIndex, GetPlaceHolderSymbol(symbol));
        }
        return new(startIndex, content);
    }

    /// <summary>
    /// GetBottom long version of inconsistent start (startsWith _{ or ^{ )
    /// </summary>
    /// <param name="input"></param>
    /// <param name="startIndex"></param>
    /// <param name="symbol"></param>
    /// <returns>(int EndIndex, string Content), if no content, returns (Length or -1, Placeholder) </returns>
    private static ContentAndEnd LongInconsistentStart(string input, int startIndex, string symbol)
    {
        startIndex += 2;    // exclude  ^{ or _{ from string 
        int endPoint = BracketHandler.FindBrackets(input, BracketType.Curly, startIndex);
        endPoint--;

        if (endPoint < 0)
        {
            //decide if returns -1 or input Length
            if (IsEndPart(symbol))
            {
                return new(input.Length, input[startIndex..]);
            }
            return new(-1, GetPlaceHolderSymbol(symbol));
        }
        string content = input[startIndex..endPoint];
        return new(endPoint, content);
    }

    /// <summary>
    /// Check if symbol is end part of symbol (sumE)
    /// </summary>
    /// <param name="symbol"></param>
    /// <returns>true if shown in the list, else false</returns>
    private static bool IsEndPart(string symbol)
    {
        return symbol switch
        {
            "sumE" => true,
            _ => false
        };
    }

    /// <summary>
    /// Gets placeholder string for Latex operator (example log => 10)
    /// </summary>
    /// <param name="symbol"></param>
    /// <returns>placeholder string for given symbol</returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static string GetPlaceHolderSymbol(string symbol)
    {
        return symbol switch
        {
            "log" => "10",
            "lim" => "x",
            "int" => "",
            "sumS" => "ö",
            "sumE" => "å",
            _ => throw new InvalidOperationException("[HelperAlgorithms.GetPlaceHolderSymbol] given symbols does not match")
        };
    }

    /// <summary>
    /// get short version of Latex separator (no {} around value)
    /// </summary>
    /// <param name="separator"></param>
    /// <returns>_ or ^</returns>
    private static string GetShortSeparator(string separator)
    {
        //get starting separator in both forms
        return separator switch
        {
            "_{" => "_",
            "^{" => "^",
            _ => "_"
        };
    }
}
