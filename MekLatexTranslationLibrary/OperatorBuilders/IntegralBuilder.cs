/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;
using MekLatexTranslationLibrary.Structures;

namespace MekLatexTranslationLibrary.OperatorBuilders;

/// <summary>
/// Translate one integral with Build()
/// </summary>
internal class IntegralBuilder
{
    private static bool IsUndefinedIntegral(string input, int startIndex)
    {
        return Slicer.GetSpanSafely(input, startIndex, 6) is "_{}^{}";
    }

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
            return BuildUndefinedIntegral(inp, startIndex);
        }
        return BuildDefinedIntegral(inp, startIndex);
    }

    private static string BuildUndefinedIntegral(string inp, int startIndex)
    {
        // if no lower and upper values  "indefinite integral" --> ∫(c,e)

        string end = inp[(startIndex + 6)..];
        int dIndex = end.IndexOf('d');
        if (dIndex is not -1)
        {
            TwoStrings divided = SeparateBodyAndArgument(end, dIndex);
            return $"{inp[..startIndex]}∫({divided.First},{divided.Second})";
        }

        end = (end == string.Empty) ? "y" : end;
        return $"{inp[..startIndex]}∫({end},x)";
    }

    private static string BuildDefinedIntegral(string inp, int start)
    {
        //if includes lower and upper values "defined integral" --> ∫(c,e,b,a)

        IntegralInfo integralInfo = new();

        ComplexSymbolReader reader = new(inp, start, "integral");

        integralInfo.SetReaderInfo(reader);

        string integralBody = inp[(reader.End + 1)..];

        int dIndex = integralBody.IndexOf('d');
        dIndex = (dIndex == -1) ? integralBody.Length : dIndex;

        TwoStrings bodyAndArgument = SeparateBodyAndArgument(integralBody, dIndex);

        integralInfo.BeforeD = bodyAndArgument.First;
        integralInfo.AfterD = bodyAndArgument.Second;

        return $"{integralInfo.TextBefore}∫({bodyAndArgument.First},{bodyAndArgument.Second},{integralInfo.Bottom},{integralInfo.Top})";
    }

    private static TwoStrings SeparateBodyAndArgument(string input, int dIndex)
    {
        string integralBody = Slicer.GetSpanSafely(input, ..dIndex);
        string integralArgument = Slicer.GetSpanSafely(input, (dIndex + 1)..);

        if (string.IsNullOrWhiteSpace(integralBody)) integralBody = "y";
        if (string.IsNullOrWhiteSpace(integralArgument)) integralArgument = "x";

        return new(integralBody, integralArgument);
    }
}
