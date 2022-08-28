/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;
using MekLatexTranslationLibrary.OperatorBuilders;

namespace MekLatexTranslationLibrary;

/// <summary>
/// translate operators from LaTeX to acsii translation
/// </summary>
public static class OperatorAlgorithms
{
    public static TranslationItem RunAll(TranslationItem item)
    {
        //runs operator change algorithms with instructions from settings
        List<TranslationError> errors = Enumerable.Empty<TranslationError>().ToList();

        while (item.Latex.Contains("\\frac{") || item.Latex.Contains("\\dfrac{"))
        {
            item = Fraction(item);
        }
        
        if (item.Latex.Contains("\\begin{cases}"))
        {
            item = CasesBuilder.Build(item);
        }

        item.Latex = LogarithmBuilder.BuildAll(item.Latex, ref errors);

        while (item.Latex.Contains("\\sqrt{"))
        {
            item = SquareRoot(item);
        }
        while (item.Latex.Contains("\\sqrt["))
        {
            item = RootBuilder.Build(item);
        }

        item.Latex = LimitBuilder.BuildAll(item.Latex, ref errors);
        item.Latex = SumBuilder.BuildAll(item.Latex, ref errors);


        // error 25 does not exist anymore
        item.Latex = IntegralBuilder.BuildAllInside(item.Latex);
        item = TrigonBuilder.Build(item);

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
