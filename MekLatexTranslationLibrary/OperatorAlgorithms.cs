/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;
using MekLatexTranslationLibrary.OperatorBuilders;
using MekLatexTranslationLibrary.Structures;

namespace MekLatexTranslationLibrary;

/// <summary>
/// translate operators from LaTeX to acsii translation
/// </summary>
public static class OperatorAlgorithms
{
    public static TranslationItem RunAll(TranslationItem item)
    {
        //runs operator change algorithms with instructions from settings

        //ready to use

        while (item.Latex.Contains("\\frac{") || item.Latex.Contains("\\dfrac{"))
        {
            item = Fraction(item);
        }
        
        if (item.Latex.Contains("\\begin{cases}"))
        {
            //while loop inside method
            item = System(item);
        }
        
        item = Logarithm(item);

        while (item.Latex.Contains("\\sqrt{"))
        {
            item = SquareRoot(item);
        }
        while (item.Latex.Contains("\\sqrt["))
        {
            item = NthRoot(item);
        }
        while (item.Latex.Contains("\\lim"))
        {
            item = Limit(item);
        }
        while (item.Latex.Contains("\\sum"))
        {
            item = Sum(item);
        }
        while (item.Latex.Contains("\\int"))
        {
            item = IntIntegral(item);
        }

        if (item.Latex.Contains("\\sin") || item.Latex.Contains("\\cos") || item.Latex.Contains("\\tan"))
        {
            item = TrigonometricOperators(item);
        }

        //keep power last
        while (item.Latex.Contains("^{"))
        {
            item = RiseToPower(item);
        }

        return item;
    }


    private static TranslationItem Fraction(TranslationItem item)
    {
        // Finds \frac{x}{y} and turns it to (x)/(y)
        //possible errors [1, 2, 3]
        int startBracket;

        while (item.Latex.Contains("\\frac{"))
        {
            startBracket = item.Latex.IndexOf("\\frac{");
            item.Latex = item.Latex.Remove(startBracket, 6);
            item = FractionBuilder.Build(item, startBracket);
        }

        while (item.Latex.Contains("\\dfrac{"))
        {
            startBracket = item.Latex.IndexOf("\\dfrac{");
            item.Latex = item.Latex.Remove(startBracket, 7);
            item = FractionBuilder.Build(item, startBracket);
        }
        return item;
    }


    private static TranslationItem RiseToPower(TranslationItem item)
    {
        //find and translate longer rise to power    ^{x} => ^(x) 
        string inp = item.Latex;
        string erCodes = item.ErrorCodes;

        //find start find index
        int startBracket = inp.IndexOf("^{");
        inp = inp.Remove(startBracket, 2);          // startindex + how many chars to remove
        inp = inp.Insert(startBracket, "^(");
        //find ending and get change symbols
        int endBracket = HandleBracket.FindBrackets(inp, "{}", startBracket);
        if (endBracket != -1)
        {
            //complete change
            inp = inp.Remove(endBracket, 1);
            inp = inp.Insert(endBracket, ")");
        }
        else
        {
            //if no ending bracket => closing = end
            inp += ")";
            erCodes += "virhe18";
            Helper.DevPrintTranslationError("virhe18");
        }
        //return values as string array
        item.ErrorCodes = erCodes;
        item.Latex = inp;
        return item;
    }

    private static TranslationItem Logarithm(TranslationItem item)
    {
        // find \log_x(y) and translate to #ot#(x,y)
        
        LogStartInfo lsi = new(item.Latex);

        while (lsi.IsFound)
        {
            item = LogarithmBuilder.Build(item, lsi);
            lsi = new(item.Latex);
        }
        return item;
    }

    private static TranslationItem System(TranslationItem item)
    {
        return CasesBuilder.Build(item);
    }

    private static TranslationItem Limit(TranslationItem item)
    {
        return LimitBuilder.Build(item);
    }

    private static TranslationItem TrigonometricOperators(TranslationItem item)
    {
        // change trigonometric functions to ti-form
        return TrigonBuilder.Build(item);
    }

    private static TranslationItem SquareRoot(TranslationItem item)
    {
        // Find and change "\\sqrt{}" => "sqrt()"
        string inp = item.Latex;

        int start = inp.IndexOf("\\sqrt{");
        inp = inp.Remove(start, 6);

        int closingBracket = HandleBracket.FindBrackets(inp, "{}", start);

        if (closingBracket != -1)
        {
            // everything okay
            item.Latex = inp[..start] + "#142#(" + inp[start..closingBracket] + ")" + inp[(closingBracket + 1)..];
            return item;
        }

        // no end bracket for \sqrt{} [end = inp.end]
        item.ErrorCodes += "virhe15";
        item.Latex = inp[..start] + "#142#(" + inp[start..] + ")";
        return item;
    }

    private static TranslationItem NthRoot(TranslationItem item)
    {
        return RootBuilder.Build(item);
    }

    private static TranslationItem Sum(TranslationItem item)
    {
        
        
        // translate \\sum_{x}^{y}z => sum(z,x,y)
        string inp = item.Latex;

        int startIndex = inp.IndexOf("\\sum");
        inp = inp.Remove(startIndex, 4);

        item.Latex = SumBuilder.Build(inp, startIndex, ref item);

        return item;
    }

    private static TranslationItem IntIntegral(TranslationItem item)
    {
        // What does this do? 
        string inp = item.Latex;

        int startIndex = inp.IndexOf("\\int");
        inp = inp.Remove(startIndex, 4);

        if (inp.Length >= startIndex + 6)
        {
            item.Latex = IntegralBuilder.Build(inp, startIndex);
            return item;
        }

        // inp ends too soon => skip
        item.ErrorCodes += "virhe25";
        Helper.DevPrintTranslationError("virhe25");

        return item;
    }

    private static TranslationItem MethodTemplate(TranslationItem item)
    {
        // What does this do?
        string inp = item.Latex;
        string erCodes = item.ErrorCodes;

        //code goes here

        item.Latex = inp;
        item.ErrorCodes = erCodes;
        return item;
    }


}
