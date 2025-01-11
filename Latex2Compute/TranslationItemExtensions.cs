/// Copyright 2024 Henri Vainio 

namespace Latex2Compute;

public static class TranslationItemExtensions
{
    public static bool IsSet(this TranslationArgs args, UnitTranslationMode mode)
    {
        return args.UnitTranslationMode == mode;
    }

    public static bool IsSet(this TranslationArgs args, Params param)
    {
        return (args.Params & param) == param;
    }

    public static bool IsSet(this TranslationArgs args, EndChanges mode)
    {
        return args.EndChanges == mode;
    }

    /// <summary>
    /// Enables the given parameter in the settings. Mutates the input args.
    /// If you want to enable multiple settings, use pipe or pipe assingment.
    /// <code>
    /// params.Enable(Param1 | Param2);
    /// // is same to
    /// params |= Param1 | Param2;
    /// </code>
    /// </summary>
    /// <param name="args"></param>
    /// <param name="param"></param>
    /// <returns>Same settings instance </returns>
    public static void Enable(ref this TranslationArgs args, Params param)
    {
        args.Params |= param;
    }

    /// <summary>
    /// Disables the given parameter in the settings. Mutates the input args.
    /// If you want to disable multiple settings, use pipe or pipe assingment
    /// <code>params.Disable(Param1 | Param2);</code>
    /// </summary>
    /// <param name="args"></param>
    /// <param name="param"></param>
    /// <returns>Same settings instance </returns>
    public static void Disable(ref this TranslationArgs args, Params param)
    {
        args.Params &= ~param;
    }
}
