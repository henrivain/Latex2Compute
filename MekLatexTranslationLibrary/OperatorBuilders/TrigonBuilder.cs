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
    internal static TranslationItem Build(TranslationItem item)
    {
        string inp = item.Latex;

        inp = inp.Replace("\\sin^{-1}", "#131#");
        inp = inp.Replace("\\sin", "#132#");
        inp = inp.Replace("\\cos^{-1}", "#133#");
        inp = inp.Replace("\\cos", "#134#");
        inp = inp.Replace("\\tan^{-1}", "#135#");
        inp = inp.Replace("\\tan", "#136#");

        inp = TranslationTag.TryAddInsides(inp, "#13N#", "#13");

        item.Latex = inp;
        return item;
    }

}
