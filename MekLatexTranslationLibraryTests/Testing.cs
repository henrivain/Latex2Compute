namespace MekLatexTranslationLibraryTests;

class Testing
{
    public static TranslationArgs GetDefaultArgs()
    {
        return new TranslationArgs
        {
            UnitTranslationMode = UnitTranslationMode.None,
            EndChanges = EndChanges.All,
            Params =
              Params.RemoveCurlyBracket |
              Params.MathchaEnabled |
              Params.AutoSeparateOperators
        };
    }

    public static TranslationArgs GetPhysics1Args()
    {
        return new TranslationArgs
        {
            UnitTranslationMode = UnitTranslationMode.Remove,
            EndChanges = EndChanges.All,
            Params =
               Params.RemoveCurlyBracket |
               Params.MathchaEnabled |
               Params.AutoSeparateOperators
        };
    }
}
