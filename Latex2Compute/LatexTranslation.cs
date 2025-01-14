/// Copyright 2021 Henri Vainio 
using Latex2Compute.Parsers;
using Latex2Compute.OtherBuilders;

namespace Latex2Compute;

/// <summary>
/// Result type for translation 
/// </summary>
public readonly record struct TranslationResult(string Result, Errors ErrorFlags);

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
        Errors errors = Errors.None;

        // start changes
        input = StartEdit.Run(input);

        // translate all operators
        input = TranslateAllOperators(input, item.Settings, ref errors);

        // connect to symbol translations
        input = SymbolTranslations.Run(input, item.Settings, ref errors);

        // make end changes
        input = EndEdit.Run(input, item.Settings, ref errors);

        // finish and return value
        return new(input, errors);
    }

    internal static string TranslateAllOperators(
        string input, TranslationArgs args, ref Errors errors)
    {
        input = Matrix.BuildAll(input, args, ref errors);
        input = FractionParser.BuildAll(input, ref errors);
        input = ProbabilityOperatorParser.BuildAll(input, ref errors);
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
