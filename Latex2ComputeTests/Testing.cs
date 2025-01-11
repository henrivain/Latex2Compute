namespace Latex2ComputeTests;

class Testing
{
    public static TranslationArgs GetDefaultArgs()
    {
        return new TranslationArgs
        {
            UnitTranslationMode = UnitTranslationMode.None,
            TargetSystem = TargetSystem.Ti,
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
            TargetSystem = TargetSystem.Ti,
            EndChanges = EndChanges.All,
            Params =
               Params.RemoveCurlyBracket |
               Params.MathchaEnabled |
               Params.AutoSeparateOperators
        };
    }
}
