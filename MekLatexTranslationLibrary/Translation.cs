/// Copyright 2021 Henri Vainio 
using System.Collections;

/// <summary>
/// Call LaTeX translation and get results
/// </summary>
namespace MekLatexTranslationLibrary;

public static class Translation
{
    internal static bool LatexInDevelopment = true;

    /// <summary>
    /// Makes normal translation => Translater LaTeX for user's cas calculator like Ti-Nspire
    /// </summary>
    /// <param name="input"></param>
    /// <returns>TranslationResult (Result, ErrorCodes) with latex translated</returns>
    public static TranslationResult MakeNormalTranslation(TranslationItem input)
    {
        TranslationItem result = ConnectToAlgorithms(input);

        return new(result.Latex, result.ErrorCodes); ;
    }



    /// <summary>
    /// Run translation algorihms in right order
    /// </summary>
    /// <param name="item"></param>
    /// <returns>inp that is translated with given translation args</returns>
    private static TranslationItem ConnectToAlgorithms(TranslationItem item)
    {
        // start changes
        item = StartEdit.Run(item);

        // translate all operators
        item = OperatorAlgorithms.RunAll(item);

        // connect to symbol translations
        item = SymbolTranslations.Run(item);

        // make end changes
        item = EndEdit.Run(item);

        // finish and return value
        return item;
    }
}
