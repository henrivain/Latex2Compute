/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Structures;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MekLatexTranslationLibraryTests")]

namespace MekLatexTranslationLibrary.Helpers;

internal static class BracketHandler
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
        BracketType.CasesStartEnd => ("\\begin{cases}", "\\end{cases}"),
        BracketType.IntegralBody_D => ("\\int", "d"),
        _ => throw new NotImplementedException($"{nameof(BracketType)} {type} is not implemented")
    };

    /// <summary>
    /// Find index of matching end bracket
    /// <para/>example: input='log(33)*42', bracketType.Round, startIndex=5 => returns 8
    /// <para/>return index if end index + 1, because it makes reading spans easier (it is start of next span and end of this one)
    /// <para/>START BRACKET SHOULD NOT BE INCLUDED IN THE INPUT (or startIndex should be start bracket index + 1)
    /// </summary>
    /// <param name="input">The string which is looped throught</param>
    /// <param name="type"></param>
    /// <param name="startPoint"></param>
    /// <returns>index of ending bracket+1, returns -1 if not found</returns>
    public static int FindBrackets(string input, BracketType type = BracketType.Curly, int startIndex = 0)
    {
        (string opening, string closing) = GetBracketsFromType(type);
        
        int x = 1;
        for (int i = startIndex; i < input.Length; i++)
        {
            string next = Slicer.GetSpanSafely(input, i, opening.Length);
            string next2 = Slicer.GetSpanSafely(input, i, closing.Length);
            if (next == opening)
            {
                i += opening.Length - 1;
                x++;
            }
            else if (next2 == closing)
            {
                x--;
                i += closing.Length - 1;
                if (x is 0) return i + 1;   // offset by one for easier span handling
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
    /// get everything inside brackets ( ) and end index of closing bracket (+1)
    /// <para/>Include opening bracket in input     example: '(33)/()', startIndex = 0, BracketType.Round => returns ('33', 4)
    /// </summary>
    /// <param name="input"></param>
    /// <param name="startIndex"></param>
    /// <returns>default: (content, index of end bracket+1)
    /// <para/>if end bracket not found: (input[startIndex..], input.Length)
    /// <para/>if no start bracket: (-1, string.Empty)</returns>
    public static ContentAndEnd GetCharsBetweenBrackets(string input, BracketType type = BracketType.RoundLong, int startIndex = 0)
    {
        (string opening, string closing) = GetBracketsFromType(type);

        // leave opening bracket in input
        if (Slicer.GetSpanSafely(input, startIndex, opening.Length) == opening)
        {
            int endIndex = FindBrackets(input, type, startIndex + opening.Length);
            if (endIndex < 0) return new(input.Length, input[(startIndex + 1)..]);

            string output = input[(startIndex + opening.Length)..(endIndex - closing.Length)];
            return new(endIndex, output);
        }
        // if is not continuing or does not start with bracket
        return new(-1, string.Empty);
    }
}
