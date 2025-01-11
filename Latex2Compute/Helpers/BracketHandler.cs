/// Copyright 2021 Henri Vainio 
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Latex2ComputeTests")]

namespace Latex2Compute.Helpers;

internal static class BracketHandler
{
    const string LeftBracket = "\\left(";
    const string RightBracket = "\\right)";

    private static Dictionary<BracketType, (string Opening, string Closing)> BracketAsString { get; set; } = new()
    {
        [BracketType.Round] = ("(", ")"),
        [BracketType.RoundLong] = (LeftBracket, RightBracket),
        [BracketType.Square] = ("[", "]"),
        [BracketType.SquareLong] = ("\\left[", "\\right]"),
        [BracketType.Curly] = ("{", "}"),
        [BracketType.CurlyLong] = ("\\left{", "\\right}"),
        [BracketType.CurrencySign_And] = ("¤", "&"),
        [BracketType.CasesStartEnd] = ("\\begin{cases}", "\\end{cases}"),
        [BracketType.IntegralBody_D] = ("\\int", "d"),
        [BracketType.RoundLong_RoundLongUnderScore] = (LeftBracket, $"{RightBracket}_")
    };

    /// <summary>
    /// Add new string representation for brackettype, if it doesn't already exist
    /// </summary>
    /// <param name="type"></param>
    /// <param name="opening"></param>
    /// <param name="closing"></param>
    /// <returns>true if value is added, otherwise false</returns>
    internal static bool TryAddBracketAsString(BracketType type, string opening, string closing)
    {
        if (BracketAsString.ContainsKey(type))
        {
            return false;
        }
        BracketAsString[type] = (opening, closing);
        return true;
    }

    /// <summary>
    /// Find index of matching end bracket
    /// <para/>example: input='log(33)*42', bracketType.Round, startIndex=5 => returns 8
    /// <para/>return index if end index + 1, because it makes reading spans easier (it is start of next span and end of this one)
    /// <para/>START BRACKET SHOULD NOT BE INCLUDED IN THE INPUT (or startIndex should be start bracket index + 1)
    /// </summary>
    /// <param name="input">The string which is looped throught</param>
    /// <param name="type"></param>
    /// <param name="startIndex"></param>
    /// <param name="matchValidatorFunc">Function to be used to validate if opening and closing match index</param>
    /// <returns>index of ending bracket+1, returns -1 if not found</returns>
    public static int FindBrackets(string input, BracketType type = BracketType.Curly, int startIndex = 0, Func<string, string, int, bool>? matchValidatorFunc = null)
    {
        var (opening, closing) = BracketAsString[type];

        // use this validation method if no other is given
        matchValidatorFunc ??= (input, reference, index) =>
        {
            return Slicer.GetSpanSafely(input, index, reference.Length) == reference;
        };

        int x = 1;
        for (int i = startIndex; i < input.Length; i++)
        {
            if (matchValidatorFunc(input, opening, i))
            {
                i += opening.Length - 1;
                x++;
            }
            else if (matchValidatorFunc(input, closing, i))
            {
                x--;
                i += closing.Length - 1;
                if (x is 0)
                {
                    return i + 1;   // offset by one for easier span handling
                }
            }
        }
        return -1;
    }


    public static int FindBrackets(ReadOnlySpan<char> input, Brackets brackets, int startIndex = 0)
        => FindBrackets(input, brackets.Opening, brackets.Closing, startIndex);

    /// <summary>
    /// Find pair closing bracket of element, if start not given starts from index 0.
    /// <para/>Example: input='(3*(-3))*42', bracketType.Round, startPoint=0 => returns 7
    /// </summary>
    /// <param name="input"></param>
    /// <param name="bracketType"></param>
    /// <param name="startPoint"></param>
    /// <returns>index of end bracket, if not found returns -1</returns>
    public static int FindBrackets(ReadOnlySpan<char> input, char opening, char closing, int startPoint = 0)
    {
        // finds and returns index of start bracket's pair
        // (bracket type doesn't have to be brackets, can also be "¤&")
        // if None => -1
        // Example: 236{7}33}33 => 8

        int bracketDepth = 1;
        for (int i = startPoint; i < input.Length; i++)
        {
            if (input[i] == opening)
            {
                bracketDepth++;
            }
            else if (input[i] == closing)
            {
                bracketDepth--;
                if (bracketDepth == 0)
                {
                    return i++;
                }
            }
        };
        return -1;
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
        var (opening, closing) = BracketAsString[type];

        // leave opening bracket in input
        if (Slicer.GetSpanSafely(input, startIndex, opening.Length) == opening)
        {
            int endIndex = FindBrackets(input, type, startIndex + opening.Length);
            if (endIndex < 0)
            {
                return new(input.Length, input[(startIndex + 1)..]);
            }

            string output = input[(startIndex + opening.Length)..(endIndex - closing.Length)];
            return new(endIndex, output);
        }
        // if is not continuing or does not start with bracket
        return new(-1, string.Empty);
    }
}
