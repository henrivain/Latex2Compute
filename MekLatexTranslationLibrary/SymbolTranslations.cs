/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary;

/// <summary>
/// Change or remove LaTeX symbols in LaTeX to acsii translation
/// </summary>
internal static class SymbolTranslations
{
    /// <summary>
    /// Translates all easy to translate symbols like "\cdot" and "\alpha" and all units if needed
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    internal static TranslationItem Run(TranslationItem item)
    {
        // translate symbols to ti-nspire form

        // geometry mode changes
        if (item.Settings.GeometryMode)
        {
            item = GeometryModeSymbols(item);
        }

        // make basic everytime translations
        item = BasicSymbols(item);

        // physics mode 1 changes
        if (item.Settings.PhysicsMode1)
        {
            // Remove physics symbols like m^3, V, N
            // => do not use g,l,m,s,A,C,F,N,W,V as variables in your Latex text
            item = RemoveGreekSymbols(item);
            item = RemovePhysics1ModeSymbols(item);
        }

        // mathmode changes
        if (item.Settings.MathMode)
        {
            // remove greek letters
            item = RemoveGreekSymbols(item);
        }

        return item;
    }

    private static string[] GetPhysicsSymbols()
    {
        // storage to physics symbols
        // return [string array {symbols}]
        string[] PhysicsSymbols =
        {
            "\\min", "\\max", "lx", "lm", "Wb", "sr", 
            "mm^3", "cm^3", "dm^3", "nm^3", "km^3", "m^3",
            "mm^2", "cm^2", "dm^2", "nm^2", "km^2", "m^2",
            "mm", "cm", "dm", "nm", "pm", "km", "hm", "dak",
            "eV", "kg", "mg", "MWh", "GWh", "TWh", "kWh", "kW", "bar", "kN",
            "kJ", "mol", "°F", "°C", "kPa", "Pa", "MHz",
            "kHz", "Hz", "cd", "ml", "kA", "mA", "kV", "mV", "C",
            "ms", "ns", "cl", "dl",
            "A", "g", "K", "m", "s", "W", "J", "N", "F", "l", "V", "h", "r", "T"
        };
        return PhysicsSymbols;
    }

    private static string[] GetGreekSymbolsArray()
    {
        // storage to math and physics greek symbols
        // return [string array {symbols}]
        string[] symbolsToRemove = 
        {
            "k\\Omega", "\\Omega", "\\Gamma", "\\Delta",            
            "\\varepsilon", "\\zeta", "\\eta", "\\theta",           
            "\\vartheta", "\\iota", "\\kappa", "\\lambda",          
            "\\Lambda", "\\mu", "\\nu", "\\xi", "\\Xi", "\\Pi",     
            "\\rho", "\\sigma", "\\Sigma", "\\tau", "\\upsilon",    
            "\\Upsilon", "\\phi", "\\Phi", "\\chi", "\\psi",        
            "\\Psi", "\\omega", "\\partial", "\\varphi"             
        };
        return symbolsToRemove;
    }

    private static TranslationItem BasicSymbols(TranslationItem item)
    {
        // Translate symbols which will be translated same way every time
        string inp = item.Latex;

        // basic symbols
        inp = inp.Replace("{,}", ".");
        inp = inp.Replace("\\cdot", "*");
        inp = inp.Replace("\\infty", "infinity");
        inp = inp.Replace("\\pi", "#161#");

        TranslateAllBrackets(ref inp);

        // remove angles
        inp = inp.Replace("(rad)", "");
        inp = inp.Replace("rad", "");
        inp = inp.Replace("\\degree", "");
        inp = inp.Replace("°", "");

        // less than, greater than
        inp = inp.Replace("\\le", "<=");
        inp = inp.Replace("\\ge", ">=");
        inp = inp.Replace("\\mp", "±");
        inp = inp.Replace("\\pm", "±");
        inp = inp.Replace("\\ne ", "≠");

        // remove if not geometry mode
        inp = inp.Replace("\\alpha", "");
        inp = inp.Replace("\\beta", "");
        inp = inp.Replace("\\gamma", "");
        inp = inp.Replace("\\delta", "");

        // other symbols
        inp = inp.Replace("\\ldots", "");
        inp = inp.Replace("\\...", "");

        item.Latex = inp;

        // vectors
        item = CheckVectorBars(item);
        return item;
    }

    private static void TranslateAllBrackets(ref string inp)
    {
        // different brackets and abs
        inp = inp.Replace("\\left(", "(");
        inp = inp.Replace("\\right)", ")");
        inp = inp.Replace("\\left\\{", "(");
        inp = inp.Replace("\\right\\}", ")");
        inp = inp.Replace("\\left[", "(");
        inp = inp.Replace("\\right]", ")");
        inp = inp.Replace("\\left|", "abs(");
        inp = inp.Replace("\\right|", ")");
    }

    private static TranslationItem CheckVectorBars(TranslationItem item)
    {
        // check if \overline{ elements in inp
        string inp = item.Latex;
        string overline = "\\overline{";
        while (inp.Contains(overline))
        {
            RemoveVectorBar(ref item, ref inp, overline);
        }
        item.Latex = inp;
        return item;
    }

    private static void RemoveVectorBar(ref TranslationItem item, ref string inp, string overline)
    {
        // remove vector "\overline" element
        int start = inp.IndexOf(overline);
        inp = inp.Remove(start, 10);
        int end = HandleBracket.FindBrackets(inp, "{}", start);
        if (end != -1)
        {
            inp = inp.Remove(end, 1);
        }
        else
        {
            // no end add error code => vector bar does not have end \overline{  } <=
            item.ErrorCodes += "virhe 21";
        }
    }

    private static TranslationItem GeometryModeSymbols(TranslationItem item)
    {
        // Translate symbols which will be translated same way every time
        string inp = item.Latex;

        inp = inp.Replace("\\alpha", "α");
        inp = inp.Replace("\\beta", "β");
        inp = inp.Replace("\\gamma", "γ");
        inp = inp.Replace("\\delta", "δ");

        item.Latex = inp;
        return item;
    }

    private static TranslationItem RemoveGreekSymbols(TranslationItem item)
    {
        // Translate symbols which will be translated same way every time
        string inp = item.Latex;
        string[] SymbolsArray = GetGreekSymbolsArray();
        foreach (string symbol in SymbolsArray)
        {
            inp = inp.Replace(symbol, "");
        }
        item.Latex = inp;
        return item;
    }

    private static TranslationItem RemovePhysics1ModeSymbols(TranslationItem item)
    {
        // Translate symbols which will be translated same way every time
        string inp = item.Latex;
        string[] SymbolsArray = GetPhysicsSymbols();
        foreach (string symbol in SymbolsArray)
        {
            inp = inp.Replace(symbol, "");
        }
        item.Latex = inp;
        return item;
    }
}
