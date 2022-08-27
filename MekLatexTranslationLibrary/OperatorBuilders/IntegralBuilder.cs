/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;
using MekLatexTranslationLibrary.Structures;

namespace MekLatexTranslationLibrary.OperatorBuilders;

/// <summary>
/// Translate one integral with Build()
/// </summary>
internal class IntegralBuilder
{
    private static string Tag { get; } = "#181#";
    private static string OperatorStart { get; } = "\\int";

    /// <summary>
    /// Translate all integrals inside input 
    /// (this is also called recursively as part of integral build process)
    /// </summary>
    /// <param name="input"></param>
    /// <returns>input with all integrals translated</returns>
    internal static string BuildAllInside(string input)
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
    private static string Build(string inp, int startIndex)
    {
        if (IsUndefinedIntegral(inp, startIndex))
        {
            // "indefinite integral" --> ∫(c,e)
            var divided = ReadIntegralBody(inp, startIndex + 6);
            return $"{inp[..startIndex]}{Tag}({divided.First},{divided.Second})";
        }

        // "definite integral" --> ∫(c,e,b,a)
        ComplexSymbolReader definedRange = new(inp, startIndex, "integral");
        var body = ReadIntegralBody(inp, definedRange.End + 1);
        
        return $"{definedRange.TextBefore}{Tag}({body.First},{body.Second}," +
               $"{definedRange.BottomContent},{definedRange.TopContent})";
    }
    private static TwoStrings ReadIntegralBody(string inp, int startIndex)
    {
        string integralBody = inp[startIndex..];
        int dIndex = integralBody.LastIndexOf('d');
        if (dIndex is -1) dIndex = integralBody.Length;
        return SeparateBodyAndArgument(integralBody, dIndex);
    }
    private static TwoStrings SeparateBodyAndArgument(string input, int dIndex)
    {
        string integralBody = BuildAllInside(Slicer.GetSpanSafely(input, ..dIndex));
        string integralArgument = Slicer.GetSpanSafely(input, (dIndex + 1)..);
        if (string.IsNullOrWhiteSpace(integralBody)) integralBody = "y";
        if (string.IsNullOrWhiteSpace(integralArgument)) integralArgument = "x";
        return new(integralBody, integralArgument);
    }
    private static bool IsUndefinedIntegral(string input, int startIndex) => Slicer.GetSpanSafely(input, startIndex, 6) is "_{}^{}";
    private static bool CanHaveDefinitiveRange(string input, int startIndex) => input.Length >= startIndex + 6;
}
