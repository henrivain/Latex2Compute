/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Structures;
using System.Text.RegularExpressions;

namespace MekLatexTranslationLibrary.Helpers;

internal static class TranslationTag
{
    const char DigitWildCard = 'N';
    const char CharWildCard = 'X';
    
    /// <summary>
    /// Translate Mek translation tags to Ti-nspire operator form
    /// </summary>
    /// <param name="inp"></param>
    /// <returns>inp with tags translated into nspire form</returns>
    internal static string ToNspireOperator(string inp)
    {

        inp = inp

            //log
            .Replace("#111#", "log")
            .Replace("#112#", "ln")
            //system
            .Replace("#121#", "system")
            .Replace("#122#", ",")
            //trigonometry
            .Replace("#131#", "arcsin")
            .Replace("#132#", "sin")
            .Replace("#133#", "arccos")
            .Replace("#134#", "cos")
            .Replace("#135#", "arctan")
            .Replace("#136#", "tan")
            //sqrt 
            .Replace("#142#", "sqrt")
            .Replace("#141#", "root")
            //lim
            .Replace("#151#", "lim")
            // pi
            .Replace("#161#", "pi")       
            // abs
            .Replace("#171#", "abs")      
            // integral
            .Replace("#181#", "∫");
        
        return inp;
    }

    /// <summary>
    /// Check if given tag appears and get insides for it
    /// </summary>
    /// <param name="input"></param>
    /// <returns>string with insides initialized for some tag</returns>
    internal static string TryAddInsides(string input, string tag, string charsBeforeWildCard)
    {
        int index = input.IndexOf(charsBeforeWildCard);

        while (index >= 0)
        {
            input = SetTagOperatorInsides(input, index + 5);

            index = HelperAlgorithms.TryGetIndexOf(input, charsBeforeWildCard, index + 1);
        }
        return input;
    }


    /// <summary>
    /// Separates Operator with "*" if it does not have any of operator symbol before it
    /// <para/>operator symbols: * + - / = > < or ) ] }
    /// </summary>
    /// <param name="inp"></param>
    /// <param name="tag"></param>
    /// <returns>inp with given opeartor separated with "*"</returns>
    internal static string SeparateOperatorByTag(string inp, string tag)
    {
        ValidateTag(tag);
        int index = 0;
        while (true)
        {
            index = GetFirstAvailableTagIndex(inp, tag, index);

            if (index < 0) break;

            if (IsOperatorSymbol(inp, index) is false)
            {
                inp = $"{inp[..index]}*{inp[index..]}";
            }
            index++;
        }
        return inp;
    }

    private static int GetFirstAvailableTagIndex(string inp, string tag, int startIndex)
    {
        string TagStart = TagCharsBeforeWildCard(tag);

        while (true)
        {
            int tagCandidateIndex = HelperAlgorithms.TryGetIndexOf(inp, TagStart, startIndex);

            if (tagCandidateIndex < 0) return -1;

            if (ArgsMatch(inp[tagCandidateIndex..], tag))
            {
                return tagCandidateIndex;
            }

            startIndex = tagCandidateIndex + 1;
        }
    }





















    // tag validaters

    /// <summary>
    /// Checks if tag is in given format
    /// <para/>type #ccc#, where c is any digit, N or X
    /// <para/>wildcards N = any digit, X = any char
    /// <para/>throws Exception if not valid
    /// </summary>
    /// <param name="tag"></param>
    /// <exception cref="Exception"></exception>
    private static void ValidateTag(string tag)
    {
        if (tag.Length is not 5) throw new Exception($"[TranslationTag.ValidateTag] tag length must be 5; was given {tag})");

        string pattern = @"#(\d|N|X){3}#";      // matches "#ccc#" where c is any digit, N or X
        if (Regex.IsMatch(tag, pattern))
        {
            return;
        }

        throw new Exception($"[TranslationTag.ValidateTag] tag can only be type #ccc#, where c is any digit or wildcard N or X ; was given {tag})");
    }

    /// <summary>
    /// Get Tag start before first wildcard
    /// <para/>"#12N#" => "#12" ( N is wild card)
    /// </summary>
    /// <param name="tag"></param>
    /// <returns>tag start before wildcards</returns>
    private static string TagCharsBeforeWildCard(string tag)
    {
        int index2 = tag.IndexOf(DigitWildCard);
        int index1 = tag.IndexOf(CharWildCard);

        int index = Helper.GetSmallestValue(index1, index2, 0);

        return (index is -1) ? tag : tag[..index];
    }




    // compare to tag

    /// <summary>
    /// Check if Char matches given tag char or wildcard
    /// <para/> in tag: 'N' = digit, 'X' = letter
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="input"></param>
    /// <returns>true if matches otherwise false</returns>
    private static bool DoesCharMatchTag(char tag, char input)
    {
        switch (tag)
        {
            case DigitWildCard:
                return char.IsDigit(input);

            case CharWildCard:
                return char.IsLetter(input);

            default:
                return (tag == input);
        }
    }


    // arg validaters

    /// <summary>
    /// Check if Args' lengths are valid
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tag"></param>
    /// <returns>true if valid</returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static bool ArgsLongEnough(string input, string tag)
    {
        if (tag.Length is not 5)
        {
            throw new InvalidOperationException($"[HelperAlgorithms.DoesMatchFiveCharTag] Given tag is not length of five (5); given tag {tag}");
        }
        if (input.Length < 5)
        {
            Helper.DevPrintTranslationError($"[HelperAlgorithms.DoesMatchFiveCharTag] Given input is not at least length of five (5); given input {input}");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Checks is next chars in string match given translation tag. Wild cards: (N = any number, X = any char)
    /// <para/> tag format "#ccc#" (example "#12N#")
    /// <para/>input "#132#654", tag "#13N#" (output: true because "#132#" matches)
    /// <para/>input "1#132#654", tag "#13N#" (output: false does not start with tag)
    /// <para/>input "#142#654", tag "#13N#" (output: false char in index 2 is not 3)
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tag"></param>
    /// <returns>true if matches, false if not</returns>
    private static bool ArgsMatch(string input, string tag)
    {
        if (ArgsLongEnough(input, tag) is false) return false;

        char[] inputSpan = input[..5].ToCharArray();

        for (short i = 0; i < inputSpan.Length; i++)
        {
            if (DoesCharMatchTag(tag[i], inputSpan[i]) is false) return false;
        }
        return true;
    }


    // other

    /// <summary>
    /// Gets content after symbol and formats it right way inside input if has any 
    /// <para/>
    /// </summary>
    /// <param name="input"></param>
    /// <param name="startIndex"></param>
    /// <returns>input with right formatted content</returns>
    private static string SetTagOperatorInsides(string input, int startIndex)
    {
        ContentAndEnd contentInfo = HelperAlgorithms.GetExpressionAfterOperator(input[startIndex..]);

        input = $"{input[..startIndex]}({contentInfo.Content}){input[(startIndex + contentInfo.End)..]}";

        return input;
    }

    /// <summary>
    /// Check if char before index is operator symbol or does not exist (negative index)
    /// </summary>
    /// <param name="inp"></param>
    /// <param name="index"></param>
    /// <returns>false if is not operatorSymbol, else true</returns>
    private static bool IsOperatorSymbol(string inp, int index)
    {
        if (index <= 0) return true;   // can't get index -1

        char[] symbolToCheck = CommonOperators.EqualSigns
                                    .Union(CommonOperators.EndBrackets)
                                        .Union(CommonOperators.BasicMathOperators)
                                            .Union(CommonOperators.StartBrackets)
                                                .ToArray();

        return Array.Exists(symbolToCheck, element => element == inp[index - 1]);
    }
}
