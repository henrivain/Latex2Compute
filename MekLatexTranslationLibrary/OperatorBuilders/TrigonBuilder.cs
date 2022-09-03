/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary.OperatorBuilders;

internal class TrigonBuilder
{
    /// <summary>
    /// Use new algorithms
    /// </summary>
    /// <param name="item"></param>
    /// <returns>translation item with one more translated \sin, \cos or \tan </returns>
    internal static string BuildAll(string input)
    {
        input = input.Replace("\\sin^{-1}", "#131#");
        input = input.Replace("\\sin", "#132#");
        input = input.Replace("\\cos^{-1}", "#133#");
        input = input.Replace("\\cos", "#134#");
        input = input.Replace("\\tan^{-1}", "#135#");
        input = input.Replace("\\tan", "#136#");

        input = TranslationTag.TryAddInsides(input, "#13N#", "#13");
        return input;
    }

}
