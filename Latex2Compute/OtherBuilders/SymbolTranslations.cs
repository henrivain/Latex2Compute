// Copyright 2021 Henri Vainio 
using Latex2Compute.Helpers;
using Latex2Compute.StaticSymbols;
using System.Text;

namespace Latex2Compute.OtherBuilders;

/// <summary>
/// Change or remove LaTeX symbols in LaTeX to acsii translation
/// </summary>
internal static class SymbolTranslations
{
    /// <summary>
    /// Translates all easy to translate symbols like "\cdot" and "\alpha" and all units if needed
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    internal static string Run(string input, TranslationArgs args, ref Errors errors)
    {
        // translate symbols to ti-nspire form

        // geometry mode changes
        if (args.IsSet(Params.UseGeometryModeSymbols))
        {
            input = new GeometryLetterParser().Translate(input);
        }

        // make basic everytime translations
        input = BasicSymbols(input, in args);

        // vectors
        input = CheckVectorBars(input, ref errors);

        // Get parser for mode specific translation or for user symbols only
        StaticSymbolParser parser = GetParser(args);
        
        // Add user defined overrides
        parser.OverrideSymbols(args.UserStaticSymbols);
        
        // Translate symbols
        return parser.Translate(input);
    }

    private static StaticSymbolParser GetParser(TranslationArgs args)
    {
        // Physics 2
        if (args.IsSet(UnitTranslationMode.Translate)) 
            return new PhysicsParser();
        
        // Physics 1
        if (args.IsSet(UnitTranslationMode.Remove))
            return new PhycicsEraserParser();

        // Math mode
        return new MathModeParser();
    }

    private static string BasicSymbols(string input, in TranslationArgs args)
    {
        // Translate symbols which will be translated same way every time
        // basic symbols
        CommonSymbolParser parser = new ();

        if (args.IsSet(UnitTranslationMode.Translate) is false)
        {
            parser.OverrideSymbols(
                SymbolCatalog.AngleSingns.ToDictionary(x => x, x => ""));
        }

        return parser.Translate(input);
    }

    private static string CheckVectorBars(string input, ref Errors errors)
    {
        // remove vector "\overline{}" element
        string overline = "\\overline{";

        while (true)
        {
            int start = input.IndexOf(overline);
            if (start < 0)
            {
                return input;
            }

            input = input.Remove(start, 10);
            int end = BracketHandler.FindBrackets(input, BracketType.Curly, start);
            end--;
            if (end is not -1)
            {
                input = input.Remove(end, 1);
            }
            else
            {
                // no end add error code => vector bar does not have end \overline{  } <=
                errors |= Errors.VecBar_NoEndBracketFound;
                Helper.PrintError(Errors.VecBar_NoEndBracketFound);
            }
        }
    }

    private static string Erase(string input, string[] symbolsToErase)
    {
        StringBuilder builder = new(input);

        // Erase all symbols in symbolsToErase from input
        foreach (string symbol in symbolsToErase)
        {
            builder.Replace(symbol, string.Empty);
        }
        return builder.ToString();
    }
}
