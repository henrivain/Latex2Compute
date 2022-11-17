﻿using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary.OperatorBuilders;
internal static class PropabilityOperators
{

    const string BinomStart = "\\binom";
    const string BinomTag = "#202#";

    const string NPRStart = "\\left(";
    const string NPREnd = "\\right)_";
    const string NPRTag = "#201#";


    public static string BuildAll(string input, ref List<TranslationError> errors)
    {
        input = BuildAllNCRs(input, ref errors);
        input = BuildAllNPRs(input, ref errors);
        return input;
    }

    struct Binom
    {
        public Binom() { }
        public string TextBefore { get; set; } = string.Empty;
        public string Equation { get; set; } = string.Empty;
        public string Parameter { get; set; } = string.Empty;
        public string TextAfter { get; set; } = string.Empty;
        public override string ToString() => $"{TextBefore}{BinomTag}({Equation},{Parameter}){TextAfter}";
    }
    public static string BuildAllNCRs(string input, ref List<TranslationError> errors)
    {
        int startIndex;

        while (true)
        {
            startIndex = input.IndexOf(BinomStart);
            if (startIndex < 0) return input;

            input = input.Remove(startIndex, BinomStart.Length);
            input = BuildNCR(input, startIndex, ref errors);
        }
    }
    internal static string BuildNCR(string input, int startIndex, ref List<TranslationError> errors)
    {
        Binom binom;
        var firstPart = BracketHandler.GetCharsBetweenBrackets(input, BracketType.Curly, startIndex);

        if (firstPart.EndIndex < 0)
        {
            Helper.TranslationError(TranslationError.Binom_NoFirstStart, ref errors);
            binom = new()
            {
                TextBefore = input[..startIndex],
                Equation = BuildAllNCRs(input[startIndex..], ref errors)
            };
            return binom.ToString();
        }
        var secondPart = BracketHandler.GetCharsBetweenBrackets(input, BracketType.Curly, firstPart.EndIndex);
        string? param = null;
        if (secondPart.EndIndex < 0)
        {
            Helper.TranslationError(TranslationError.BinomNoSecondStart, ref errors);
            param = Slicer.GetSpanSafely(input, firstPart.EndIndex..);
        }
        binom = new()
        {
            TextBefore = input[..startIndex],
            Equation = firstPart.Content,
            Parameter = BuildAllNCRs(param is null ? secondPart.Content : param, ref errors),
            TextAfter = param is null ? BuildAllNCRs(input[secondPart.EndIndex..], ref errors) : string.Empty
        };
        return binom.ToString();
    }





    public static string BuildAllNPRs(string input, ref List<TranslationError> errors)
    {
        int indexerIndex = 0;
        int startIndex;
        int endIndex;

        while (true)
        {
            // find any possible start  '\left('
            startIndex = input.IndexOf(NPRStart, indexerIndex);
            if (startIndex < 0) return input;

            // jump indexer over
            indexerIndex = startIndex + NPRStart.Length;

            // try find end part    '\right)_'
            endIndex = BracketHandler.FindBrackets(input, BracketType.RoundLong_RoundLongUnderScore, indexerIndex);
            if (endIndex < 0) continue;

            // remove start and fix endindex to match new length
            input = input.Remove(startIndex, NPRStart.Length);
            endIndex -= NPRStart.Length;

            // build
            input = BuildNPR(input, startIndex, endIndex, ref errors);
        }
    }
    internal static string BuildNPR(string input, int startIndex, int firstEndIndex, ref List<TranslationError> errors)
    {
        string textBefore = input[..startIndex];
        string content = BuildAllNPRs(input[startIndex..(firstEndIndex - NPREnd.Length)], ref errors);
        string param = Slicer.GetCharSafely(input, firstEndIndex).ToString() ?? string.Empty;
        string textAfter = Slicer.GetSpanSafely(input, (firstEndIndex + 1)..);

        if (Slicer.GetCharSafely(input, firstEndIndex) is '{')
        {
            var paramInfo = BracketHandler.GetCharsBetweenBrackets(input, BracketType.Curly, firstEndIndex);
            if (paramInfo.EndIndex > -1)
            {
                param = BuildAllNPRs(paramInfo.Content, ref errors);
                textAfter = input[paramInfo.EndIndex..];
            }
        }
        return $"{textBefore}{NPRTag}({content},{param}){textAfter}";

    }
}