/// Copyright 2021 Henri Vainio 

namespace MekLatexTranslationLibrary.Structures;

/// <summary>
/// Get char array of some type of basic math operator
/// <para/>EqualSigns, BasicMathOperators, StartBrackets, EndBrackets
/// </summary>
internal struct CommonOperators
{
    internal static readonly char[] EqualSigns = { '=', '<', '>' };
    internal static readonly char[] BasicMathOperators = { '+', '-', '*' };     // does not include /, because most of the time it is not significant
    internal static readonly char[] StartBrackets = { '(', '[', '{' };
    internal static readonly char[] EndBrackets = { ')', ']', '}' };
}
