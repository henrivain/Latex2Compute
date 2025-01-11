/// Copyright 2021 Henri Vainio 
namespace Latex2Compute.Parsers;

internal class TrigonParser
{
    /// <summary>
    /// Use new algorithms
    /// </summary>
    /// <param name="item"></param>
    /// <returns>translation item with one more translated \sin, \cos or \tan </returns>
    internal static string BuildAll(string input)
    {
        input = input.Replace("\\sin^{-1}", ConstSymbol.Arcsin);
        input = input.Replace("\\sin", ConstSymbol.Sin);
        input = input.Replace("\\cos^{-1}", ConstSymbol.Arccos);
        input = input.Replace("\\cos", ConstSymbol.Cos);
        input = input.Replace("\\tan^{-1}", ConstSymbol.Arctan);
        input = input.Replace("\\tan", ConstSymbol.Tan);

        input = TranslationTag.TryAddInsides(input, "#13");
        return input;
    }

}
