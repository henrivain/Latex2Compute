/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.OperatorBuilders;
using MekLatexTranslationLibrary.OtherBuilders;

namespace MekLatexTranslationLibrary;


public static class LatexTranslation
{
    // Summary:
    //          Translate latex string into form that your symbolic calculator understands
    //          (mainly developed (and tested) for ti-nspire, but also works in many occasions with other calculators)
    //          Translation process prints errors in the console if entry assembly is in debug mode
    // 
    //  Params: 
    //          TranslationItem item which includes:
    //              Latex (latex string to be translated)
    //              TranslationArgs (Settings that algorithms will follow when doing the translation process)
    //
    //  Returns: 
    //          Translation result which includes:
    //              Result (string representation of all latex translated)
    //              ErrorCodes (any error that appeared during the translation)
    public static TranslationResult Translate(TranslationItem item)
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

    internal static string TranslateAllOperators(string input, ref List<TranslationError> errors)
    {
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
