/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Structures;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MekLatexTranslationLibraryTests")]

namespace MekLatexTranslationLibrary.Helpers;

internal static class HandleBracket
{
    const string LeftBracket = "\\left(";
    const string RightBracket = "\\right)";

    private static (string Opening, string Closing) GetBracketsFromType(BracketType type) => type switch
    {
        BracketType.Round => ("(", ")"),
        BracketType.RoundLong => (LeftBracket, RightBracket),
        BracketType.Square => ("[", "]"),
        BracketType.SquareLong => ("\\left[", "\\right]"),
        BracketType.Curly => ("{", "}"),
        BracketType.CurlyLong => ("\\left{", "\\right}"),
        BracketType.CurrencySign_And => ("¤", "&"),
        _ => throw new NotImplementedException($"{nameof(BracketType)} {type} is not implemented")
    };

    /// <summary>
    /// Find index of matching end bracket
    /// </summary>
    /// <param name="input">The string which is looped throught</param>
    /// <param name="type"></param>
    /// <param name="startPoint"></param>
    /// <returns>index of ending bracket, -1 if not found</returns>
    public static int FindBrackets(string input, BracketType type, int startIndex = 0)
    {
        (string opening, string closing) = GetBracketsFromType(type);
        
        int x = 1;
        for (int i = startIndex; i < input.Length; i++)
        {
            string next = Slicer.GetSpanSafely(input, i, opening.Length);
            string next2 = Slicer.GetSpanSafely(input, i, closing.Length);
            if (next == opening)
            {
                // You don't need to read the rest of the opening
                i += opening.Length - 1;
                x++;
            }
            else if (next2 == closing)
            {
                x--;
                i += closing.Length - 1;
                if (x is 0) return i + 1;
            }
        }
        return -1;
    }




    /// <summary>
    /// Find closing bracket of element, if start not given starts from index 0
    /// </summary>
    /// <param name="input"></param>
    /// <param name="bracketType"></param>
    /// <param name="startPoint"></param>
    /// <returns>index of end bracket, if not found returns -1</returns>
    public static int FindBrackets(string input, string bracketType, int startPoint = 0)
    {
        //finds and returns index of start bracket's pair
        //(bracket type doesn't have to be brackets, can also be "¤&")
        //if None => -1
        //Example: 236{7}33}33 => 8
        char opening = bracketType[0];
        char closing = bracketType[1];
        
        int x = 1;
        int endBracket = -1;

        for (int i = startPoint; i < input.Length; i++)
        {
            if (input[i] == opening)
            {
                x++;
            }
            else if (input[i] == closing)
            {
                x--;
                if (x == 0)
                {
                    endBracket = i++;
                    break;
                }
            }
        };
        return endBracket;
    }



    /// <summary>
    /// get everything inside \left( \right) brackets
    /// </summary>
    /// <param name="input"></param>
    /// <param name="startIndex"></param>
    /// <returns>string[insides, int end index as string], if end not found, end = -1</returns>
    public static ContentAndEnd GetCharsBetweenBrackets(string input, int startIndex)
    {
        // leave opening bracket
        if (Slicer.GetSpanSafely(input, startIndex, LeftBracket.Length) is LeftBracket)
        {
            startIndex += LeftBracket.Length;
            input = input[startIndex..];
            int endBracket = FindBrackets(input, BracketType.RoundLong);
            if (endBracket is -1)
            {
                return new(input.Length, input);
            }
            int endIndex = startIndex + endBracket;
            string output = input[..(endBracket - RightBracket.Length)];
            return new(endIndex, output);
        }
        // if is not continuing or does not start with bracket
        return new(-1, "");
    }
}
