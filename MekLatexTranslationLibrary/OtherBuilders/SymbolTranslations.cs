/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary.OtherBuilders;

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
    internal static string Run(string input, TranslationArgs args, ref List<TranslationError> errors)
    {
        // translate symbols to ti-nspire form

        // geometry mode changes
        if (args.GeometryMode)
        {
            input = GeometryModeSymbols(input);
        }

        // make basic everytime translations
        input = BasicSymbols(input);

        // vectors
        input = CheckVectorBars(input, ref errors);

        // physics mode 1 changes
        if (args.PhysicsMode1)
        {
            // Remove physics symbols like m^3, V, N
            // => do not use g,l,m,s,A,C,F,N,W,V as variables in your Latex text
            input = RemoveGreekSymbols(input);
            input = RemovePhysics1ModeSymbols(input);
        }

        // mathmode changes
        if (args.MathMode)
        {
            // remove greek letters
            input = RemoveGreekSymbols(input);
        }

        return input;
    }

    private static string[] GetPhysicsSymbols()
    {
        // storage to physics symbols
        // return [string array {symbols}]
        string[] PhysicsSymbols =
        {
            "mm^3", "cm^3", "dm^3", "nm^3", "km^3", "m^3",
            "mm^2", "cm^2", "dm^2", "nm^2", "km^2", "m^2",
            "keV", "MeV", "GeV", "TeV", "eV",
            "MHz", "kHz", "Hz",
            "MWh", "GWh", "TWh", "kWh", "kW",
            "mol", 
            "kpl",
            "\\min", "\\max",
            "kPa", "bar", "Pa",
            "dak", "cm", "dm", "nm", "pm", "km", "hm", "mm",
            "lx", "lm", "Wb", "sr",
            "kN",
            "kJ", 
            "kg", "mg",
            "°F", "°C",
            "cd", 
            "kA", "mA", 
            "kV", "mV", 
            "ms", "ns",
            "ml", "cl", "dl",
            "A","C", "F", "g", "G", "h", "J", "K", "l", 
            "m", "N", "r", "s", "T", "V", "W",  
        };
        return PhysicsSymbols;
    }

    private static string[] GetGreekSymbolsArray()
    {
        // storage to math and physics greek symbols
        // return [string array {symbols}]
        return new[]
        {
            "k\\Omega", "\\Omega", "\\Gamma", "\\Delta",
            "\\varepsilon", "\\zeta", "\\eta", "\\theta",
            "\\vartheta", "\\iota", "\\kappa", "\\lambda",
            "\\Lambda", "\\mu", "\\nu", "\\xi", "\\Xi", "\\Pi",
            "\\rho", "\\sigma", "\\Sigma", "\\tau", "\\upsilon",
            "\\Upsilon", "\\phi", "\\Phi", "\\chi", "\\psi",
            "\\Psi", "\\omega", "\\partial", "\\varphi"
        };
    }

    private static string BasicSymbols(string input)
    {
        // Translate symbols which will be translated same way every time

        // basic symbols
        input = input.Replace("{,}", ".")
            .Replace("\\cdot", "*")
            .Replace("\\infty", "infinity")
            .Replace("\\pi", "#161#");

        input = TranslateAllBrackets(input);

        // remove angles
        input = input
            .Replace("(rad)", string.Empty)
            .Replace("rad", string.Empty)
            .Replace("\\degree", string.Empty)
            .Replace("°", string.Empty)

        // less than, greater han
            .Replace("\\le", "<=")
            .Replace("\\ge", ">=")
            .Replace("\\mp", "±")
            .Replace("\\pm", "±")
            .Replace("\\ne", "≠")

        // remove if not geometrymode
            .Replace("\\alpha", string.Empty)
            .Replace("\\beta", string.Empty)
            .Replace("\\gamma", string.Empty)
            .Replace("\\delta", string.Empty)

        // other symbols
            .Replace("\\ldots", string.Empty)
            .Replace("\\...", string.Empty);

        return input;
    }

    private static string TranslateAllBrackets(string input)
    {
        // different brackets and abs
        return input
            .Replace("\\left(", "(")
            .Replace("\\right)", ")")
            .Replace("\\left\\{", "(")
            .Replace("\\right\\}", ")")
            .Replace("\\left[", "(")
            .Replace("\\right]", ")")
            .Replace("\\left|", "#171#(")
            .Replace("\\right|", ")");
    }

    private static string CheckVectorBars(string input, ref List<TranslationError> errors)
    {
        // remove vector "\overline{}" element
        string overline = "\\overline{";

        while (true)
        {
            int start = input.IndexOf(overline);
            if (start < 0) return input;

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
                Helper.TranslationError(TranslationError.VecBar_NoEndBracketFound, ref errors);
            }
        }
    }



    private static string GeometryModeSymbols(string input)
    {
        // Translate symbols which will be translated same way every time
        return input
            .Replace("\\alpha", "α")
            .Replace("\\beta", "β")
            .Replace("\\gamma", "γ")
            .Replace("\\delta", "δ");
    }

    private static string RemoveGreekSymbols(string input)
    {
        // Translate symbols which will be translated same way every time
        string[] SymbolsArray = GetGreekSymbolsArray();
        foreach (string symbol in SymbolsArray)
        {
            input = input.Replace(symbol, string.Empty);
        }
        return input;
    }

    private static string RemovePhysics1ModeSymbols(string input)
    {
        // Translate symbols which will be translated same way every time
        string[] SymbolsArray = GetPhysicsSymbols();
        foreach (string symbol in SymbolsArray)
        {
            input = input.Replace(symbol, string.Empty);
        }
        return input;
    }
}
