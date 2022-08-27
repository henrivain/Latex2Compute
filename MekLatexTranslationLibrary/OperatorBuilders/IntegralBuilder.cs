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

    /// <summary>
    /// Translate one Integral starting from startIndex
    /// </summary>
    /// <param name="inp"></param>
    /// <param name="startIndex"></param>
    /// <returns>inp with one Integral translated</returns>
    internal static string Build(string inp, int startIndex)
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
        int dIndex = integralBody.IndexOf('d');
        if (dIndex is -1) dIndex = integralBody.Length;
        return SeparateBodyAndArgument(integralBody, dIndex);
    }
    private static TwoStrings SeparateBodyAndArgument(string input, int dIndex)
    {
        string integralBody = Slicer.GetSpanSafely(input, ..dIndex);
        string integralArgument = Slicer.GetSpanSafely(input, (dIndex + 1)..);

        if (string.IsNullOrWhiteSpace(integralBody)) integralBody = "y";
        if (string.IsNullOrWhiteSpace(integralArgument)) integralArgument = "x";

        return new(integralBody, integralArgument);
    }
    private static bool IsUndefinedIntegral(string input, int startIndex) => Slicer.GetSpanSafely(input, startIndex, 6) is "_{}^{}";

}
