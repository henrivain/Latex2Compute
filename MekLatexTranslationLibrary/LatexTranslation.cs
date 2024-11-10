/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Parsers;
using MekLatexTranslationLibrary.OtherBuilders;

namespace MekLatexTranslationLibrary;

/// <summary>
/// Result type for translation 
/// </summary>
public readonly record struct TranslationResult(string Result, TranslationErrors ErrorFlags);

public static class LatexTranslation
{
    /// <summary>
    /// Translate latex string into form that your symbolic calculator understands.
    /// (mainly developed (and tested) for ti-nspire, but also works in many occasions with other calculators)
    /// Translation process prints errors in the console if entry assembly is in debug mode
    /// </summary>
    /// <param name="item"><see cref="TranslationItem"/> containing translation arguments and latex string.</param>
    /// <returns><see cref="TranslationResult"/> containing any errors in translation process.</returns>
    public static TranslationResult Translate(TranslationItem item)
    {
        string input = item.Latex;
        TranslationErrors errors = TranslationErrors.None;

        // start changes
        input = StartEdit.Run(input);

        // translate all operators
        input = TranslateAllOperators(input, ref errors);

        // connect to symbol translations
        input = SymbolTranslations.Run(input, item.Settings, ref errors);

        // make end changes
        input = EndEdit.Run(input, item.Settings, ref errors);

        // finish and return value
        return new(input, errors);
    }

    internal static string TranslateAllOperators(string input, ref TranslationErrors errors)
    {
        input = Matrix.BuildAll(input, ref errors).ToString();
        input = FractionParser.BuildAll(input, ref errors);
        input = PropabilityOperatorParser.BuildAll(input, ref errors);
        input = CasesParser.BuildAll(input, ref errors);
        input = LogarithmParser.BuildAll(input, ref errors);
        input = SquareRootBuilder.BuildAll(input, ref errors);
        input = NthRootParser.BuildAll(input, ref errors);
        input = LimitParser.BuildAll(input, ref errors);
        input = SumParser.BuildAll(input, ref errors);
        input = IntegralParser.BuildAll(input);
        input = TrigonParser.BuildAll(input);
        input = ExponentParser.BuildAll(input, ref errors);   //keep this last
        return input;
    }
}
