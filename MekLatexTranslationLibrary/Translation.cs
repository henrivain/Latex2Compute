/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.OperatorBuilders;
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
        List<TranslationError> errors = Enumerable.Empty<TranslationError>().ToList();

        // start changes
        item = StartEdit.Run(item);

        // translate all operators
        item.Latex = TranslateAllOperators(item.Latex, ref errors);

        // connect to symbol translations
        item = SymbolTranslations.Run(item);

        // make end changes
        item = EndEdit.Run(item);

        // finish and return value
        return item;
    }


    private static string TranslateAllOperators(string input, ref List<TranslationError> errors)
    {
        input = FractionBuilder.BuildAll(input, ref errors);
        input = CasesBuilder.BuildAll(input, ref errors);
        input = LogarithmBuilder.BuildAll(input, ref errors);
        input = SquareRootBuilder.BuildAll(input, ref errors);
        input = NthRootBuilder.BuildAll(input, ref errors);
        input = LimitBuilder.BuildAll(input, ref errors);
        input = SumBuilder.BuildAll(input, ref errors);
        input = IntegralBuilder.BuildAll(input);
        input = TrigonBuilder.BuildAll(input);
        input = RiseToPowerBuilder.BuildAll(input, ref errors);   //keep this last
        return input;
    }
}
