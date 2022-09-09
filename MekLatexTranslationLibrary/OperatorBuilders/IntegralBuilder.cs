/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;
using MekLatexTranslationLibrary.Structures;

namespace MekLatexTranslationLibrary.OperatorBuilders;

/// <summary>
/// Translate one integral with BuildAll()
/// </summary>
internal class IntegralBuilder
{
    const string Tag = "#181#";
    const string OperatorStart = "\\int";

    /// <summary>
    /// Translate all integrals inside input 
    /// (this is also called recursively as part of integral build process)
    /// </summary>
    /// <param name="input"></param>
    /// <returns>input with all integrals translated</returns>
    public static string BuildAll(string input)
    {
        while (true)
        {
            int startIndex = input.IndexOf(OperatorStart);
            if (startIndex < 0) break;
            input = input.Remove(startIndex, OperatorStart.Length);
            if (CanHaveDefinitiveRange(input, startIndex))
            {
                input = Build(input, startIndex);
            }
        }
        return input;
    }

    /// <summary>
    /// Translate first integral from startindex and all integrals inside it
    /// </summary>
    /// <param name="inp"></param>
    /// <param name="startIndex"></param>
    /// <returns>inp with some Integral translated</returns>
    internal static string Build(string inp, int startIndex)
    {
        string body, argument, textAfter;
        if (IsUndefinedIntegral(inp, startIndex))
        {
            // "indefinite integral" --> ∫(c,e)
            (body, argument, textAfter) = ReadIntegralBody(inp, startIndex + 6);
            return $"{inp[..startIndex]}{Tag}({body},{argument}){textAfter}";
        }

        // "definite integral" --> ∫(c,e,b,a)
        ComplexSymbolReader definedRange = new(inp, startIndex, "integral");
        (body, argument, textAfter) = ReadIntegralBody(inp, definedRange.End + 1);
        
        return $"{definedRange.TextBefore}{Tag}({body},{argument}," +
               $"{definedRange.BottomContent},{definedRange.TopContent}){textAfter}";
    }
    private static (string Body, string Argument, string TextAfter) ReadIntegralBody(string inp, int startIndex)
    {
        string integralBody = inp[startIndex..];
        int dIndex = BracketHandler.FindBrackets(integralBody, BracketType.IntegralBody_D);
        if (dIndex > 0) dIndex--;
        if (dIndex is -1) dIndex = integralBody.Length;
        return SeparateBodyAndArgument(integralBody, dIndex);
    }
    private static (string Body, string Argument, string TextAfter) SeparateBodyAndArgument(string input, int dIndex)
    {
        string integralBody = BuildAll(Slicer.GetSpanSafely(input, ..dIndex));

        int argEndIndex = CharComparer.GetIndexOfFirstNonChar(input.AsSpan(), dIndex + 1);
        
        string integralArgument = Slicer.GetSpanSafely(input, (dIndex + 1)..argEndIndex);          // !!! this line takes all until end
        if (string.IsNullOrWhiteSpace(integralBody)) integralBody = "y";
        if (string.IsNullOrWhiteSpace(integralArgument)) integralArgument = "x";
        return (integralBody, integralArgument, Slicer.GetSpanSafely(input, argEndIndex..));
    }
    private static bool IsUndefinedIntegral(string input, int startIndex) => Slicer.GetSpanSafely(input, startIndex, 6) is "_{}^{}";
    private static bool CanHaveDefinitiveRange(string input, int startIndex) => input.Length >= startIndex + 6;
}
