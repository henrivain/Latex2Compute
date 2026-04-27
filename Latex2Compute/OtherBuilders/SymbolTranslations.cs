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

        // Remove greek symbols if needed
        if (args.IsSet(UnitTranslationMode.None) || args.IsSet(UnitTranslationMode.Remove))
        {
            input = Erase(input, SymbolCatalog.GreekSymbols);
        }

        // physics mode 1 changes
        if (args.IsSet(UnitTranslationMode.Remove))
        {
            // Remove physics symbols like m^3, V, N
            // => do not use g,l,m,s,A,C,F,N,W,V as variables in your Latex text
            input = Erase(input, SymbolCatalog.PhysicsEraseable);
        }

        // Get parser for mode specific translation or for user symbols only
        StaticSymbolParser parser = args.IsSet(UnitTranslationMode.Translate)
            ? new PhysicsParser() 
            : new StaticSymbolParser([], SymbolCatalog.MathOperators);
        
        // Add user defined overrides
        parser.OverrideSymbols(args.UserStaticSymbols);
        
        // Translate symbols
        return parser.Translate(input);
    }

    private static string BasicSymbols(string input, in TranslationArgs args)
    {
        // Translate symbols which will be translated same way every time

        // basic symbols
        input = new CommonSymbolParser().Translate(input);

        // remove angles
        if (args.IsSet(UnitTranslationMode.Translate) is false)
        {
            input = Erase(input, ["°", "(rad)", "rad", "\\degree"]);
        }

        // other symbols
        return Erase(input, ["\\alpha", "\\beta", "\\gamma", "\\delta", "\\ldots", "\\..."]);
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
