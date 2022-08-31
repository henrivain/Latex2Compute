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


        item.Latex = FractionBuilder.BuildAll(item.Latex, ref errors);
        //while (item.Latex.Contains("\\frac{") || item.Latex.Contains("\\dfrac{"))
        //{
        //    item = Fraction(item);
        //}
        
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
        item.Latex = RiseToPowerBuilder.BuildAll(item.Latex, ref errors);
       

        return item;
    }


    

    private static TranslationItem SquareRoot(TranslationItem item)
    {
        // Find and change "\\sqrt{}" => "sqrt()"
        string inp = item.Latex;

        int start = inp.IndexOf("\\sqrt{");
        inp = inp.Remove(start, 6);

        int closingBracket = BracketHandler.FindBrackets(inp, "{}", start);

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
