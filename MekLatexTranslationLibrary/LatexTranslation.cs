/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.OperatorBuilders;
using MekLatexTranslationLibrary.OtherBuilders;

namespace MekLatexTranslationLibrary;


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
        List<TranslationErrors> errors = Enumerable.Empty<TranslationErrors>().ToList();

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

    internal static string TranslateAllOperators(string input, ref List<TranslationErrors> errors)
    {
        input = Matrix.Parse(input).Build();
        input = FractionBuilder.BuildAll(input, ref errors);
        input = PropabilityOperators.BuildAll(input, ref errors);
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
