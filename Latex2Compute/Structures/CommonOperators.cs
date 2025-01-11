/// Copyright 2021 Henri Vainio 

namespace Latex2Compute.Structures;

/// <summary>
/// GetBottom char array of some type of basic math operator
/// <para/>EqualSigns, BasicMathOperators, StartBrackets, EndBrackets
/// </summary>
internal readonly struct CommonOperators
{
    internal static readonly char[] _equalSigns = { '=', '<', '>' };
    internal static readonly char[] _basicMathOperators = { '+', '-', '*' };     // does not include /, because most of the time it is not significant
    internal static readonly char[] _startBrackets = { '(', '[', '{' };
    internal static readonly char[] _endBrackets = { ')', ']', '}' };
}
