﻿/// Copyright 2022 Henri Vainio 

namespace MekLatexTranslationLibrary.Parsers;

internal static class NthRootParser

{
    struct Root
    {
        public Root() { }
        public string TextBefore { get; set; } = string.Empty;
        public string TopText { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string TextAfter { get; set; } = string.Empty;
        public override readonly string ToString()
        {
            return $"{TextBefore}{ConstSymbol.Root}({Body},{TopText}){TextAfter}";
        }
    }

    const string OperatorStart = "\\sqrt[";

    public static string BuildAll(string input, ref TranslationErrors errors)
    {
        int startIndex;
        while (true)
        {
            startIndex = input.IndexOf(OperatorStart);
            if (startIndex < 0)
            {
                return input;
            }

            input = input.Remove(startIndex, OperatorStart.Length - 1);
            input = Build(input, startIndex, ref errors);
        }
    }

    internal static string Build(string input, int startIndex, ref TranslationErrors errors)
    {
        // Find and change "\\sqrt[x]{y}" => "root(y,x)"
        Root root = new()
        {
            TextBefore = input[..startIndex]
        };

        var topInfo = BracketHandler.GetCharsBetweenBrackets(input, BracketType.Square, startIndex);
        if (topInfo.EndIndex < 0)
        {
            errors |= TranslationErrors.NthRoot_NoFirstClosingBracket;
            Helper.PrintError(TranslationErrors.NthRoot_NoFirstClosingBracket);

            topInfo.Content = input[(startIndex + 1)..];
            topInfo.EndIndex = input.Length;
        }

        root.TopText = topInfo.Content;

        var bodyInfo = BracketHandler.GetCharsBetweenBrackets(input, BracketType.Curly, topInfo.EndIndex);
        if (bodyInfo.EndIndex < 0)
        {
            errors |= TranslationErrors.NthRoot_NoSecondStartBracket;
            Helper.PrintError(TranslationErrors.NthRoot_NoSecondStartBracket);

            bodyInfo.EndIndex = BracketHandler.FindBrackets(input, BracketType.Curly, topInfo.EndIndex);
            if (bodyInfo.EndIndex < 0)
            {
                errors |= TranslationErrors.NthRoot_NoEndFound;
                Helper.PrintError(TranslationErrors.NthRoot_NoEndFound);
                bodyInfo.EndIndex = input.Length;
            }
            bodyInfo.Content = Slicer.GetSpanSafely(input, topInfo.EndIndex..);
        }
        root.Body = bodyInfo.Content;
        root.TextAfter = input[bodyInfo.EndIndex..];

        return root.ToString();
    }
}