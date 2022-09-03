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
    public static TranslationResult MakeNormalTranslation(TranslationItem item)
    {
        string input = item.Latex;
        List<TranslationError> errors = Enumerable.Empty<TranslationError>().ToList();

        // start changes
        input = StartEdit.Run(input);

        // translate all operators
        input = TranslateAllOperators(input, ref errors);

        // connect to symbol translations
        input = SymbolTranslations.Run(input, item.Settings, ref errors);

        // make end changes
        input = EndEdit.Run(input, item.Settings, ref errors);

        // finish and return value
        return new(input, string.Join(", ", errors));
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
