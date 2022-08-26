/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;
using MekLatexTranslationLibrary.Structures;

namespace MekLatexTranslationLibrary.OperatorBuilders;

/// <summary>
/// Translate one integral with Build()
/// </summary>
internal class IntegralBuilder
{

    /// <summary>
    /// Translate one Integral starting from startIndex
    /// </summary>
    /// <param name="inp"></param>
    /// <param name="startIndex"></param>
    /// <returns>inp with one Integral translated</returns>
    internal static string Build(string inp, int startIndex)
    {
        if (HelperAlgorithms.CheckNextCharsSafely(inp, 6, startIndex) is "_{}^{}")
        {
            return BuildUndefinedIntegral(inp, startIndex);
        }
        //if includes lower and upper values "defined integral" --> ∫(c,e,b,a)
        return BuildDefinedIntegral(inp, startIndex);
    }

    private static string BuildUndefinedIntegral(string inp, int startIndex)
    {
        // if no lower and upper values  "indefinite integral" --> ∫(c,e)

        string end = inp[(startIndex + 6)..];
        int dIndex = end.IndexOf('d');

        if (dIndex is not -1)
        {
            // split d in end
            TwoStrings divided = CheckIntegralContents(end, dIndex);
            return $"{inp[..startIndex]}∫{divided.First},{divided.Second})";
        }

        // no d in end
        end = (end is "") ? "y" : end;

        return $"{inp[..startIndex]}∫({end},x)";
    }

    private static string BuildDefinedIntegral(string inp, int start)
    {
        IntegralInfo integralInfo = new();

        ComplexSymbolReader reader = new(inp, start, "integral");

        integralInfo.SetReaderInfo(reader);

        string integralBody = inp[(reader.End + 1)..];

        int dIndex = integralBody.IndexOf('d');
        dIndex = (dIndex == -1) ? integralBody.Length : dIndex;

        TwoStrings halved = CheckIntegralContents(integralBody, dIndex);

        integralInfo.BeforeD = halved.First;
        integralInfo.AfterD = halved.Second;
        // compile

        return $"{integralInfo.TextBefore}∫({integralInfo.BeforeD},{integralInfo.AfterD},{integralInfo.Bottom},{integralInfo.Top})";
    }

    private static TwoStrings CheckIntegralContents(string input, int index)
    {
        string firstPart = input[..index];
        string SecondPart = input[(index + 1)..];

        if (firstPart == "")
        {
            firstPart = "y";
        }
        if (SecondPart == "")
        {
            SecondPart = "x";
        }
        return new(firstPart, SecondPart);
    }
}
