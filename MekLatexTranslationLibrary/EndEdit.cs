/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary;

static internal class EndEdit
{
    /// <summary>
    /// Translates all operator tags to ti-nspire form and deletes empty operators. Do Matcha changes
    /// <para/>most of the operator names are seen as units with physics mode => uses tags (example "s" = second => "system" [problem] => uses "#121#")
    /// <para/>Removes empty divisions ["()/()" => ""], Adds derivative or solve if needed [solve(inp,x)]
    /// </summary>
    /// <param name="item"></param>
    /// <returns>inp with made end change</returns>
    internal static TranslationItem Run(TranslationItem item)
    {

        // remove matcha document constructors (do before curly bracket removal)
        item = Matcha.ConnectToMathchaChanges(item);

        string inp = item.Latex;

        // change special
        if (item.Settings.SpecialSymbolTranslation)
        {
            inp = SpecialSymbols(inp);
        }

        // remove unused curly brackets
        if (item.Settings.CurlyBracket)
        {
            inp = inp.Replace("{", "");
            inp = inp.Replace("}", "");
        }
        // remove not useful
        inp = inp.Replace("\\", "");

        if (item.Settings.AutoSeparateOperators)
        {
            inp = SeparateOperatorsWithCdot(inp);
        }

        inp = TranslationTag.ToNspireOperator(inp);

        // changes from settings

        switch (item.Settings.EndChanges)
        {
            case "all":
                // remove all not wanted items
                inp = RemoveEmptyFractions(inp);
                inp = RemoveEndChangesSymbols(inp);
                break;

            case "emptyFrac":
                //remove empty fractions
                inp = RemoveEmptyFractions(inp);
                break;

            default:
                break;
        }

        if (item.Settings.AutoSolve)
        {
            // if auto solve is on => call autosolve method and check if conditions are true
            inp = RunAutoSolve(inp);
        }

        inp = CheckDerivative(inp, item);

        item.Latex = inp;
        return item;
    }

    /// <summary>
    /// Removes all empty fractions from inp like ()/() or (^2)/() or ^2/^2
    /// </summary>
    /// <param name="inp"></param>
    /// <returns>inp with fractions removed</returns>
    private static string RemoveEmptyFractions(string inp)
    {
        string[] patterns =
        {
            @"\(\)/\(\)",                   // ()/() 
            @"\*\^\([0-9]\)",
            @"\(\)\^[0-9]",                 // ()^2
            @"\(\^[0-9]\)/\(\)",            // (^2)/()      2 can be any number
            @"\(\^[0-9]\)/\(\^[0-9]\)",     // (^2)/(^2)      
            @"\(\)/\(\^[0-9]\)",            // ()/(^2)
            @"\(\)/\([0-9]\)",              // ()/(2)
            @"\([0-9]\)/\(\)",              // (2)/()
            @"\^[0-9]/\^[0-9]",             // ^2/^2
            @"\^[0-9]/",                    // ^2/
            @"/\^[0-9]",                    // /^2
            @"\(\*\)/\(\*\)",                 // (*)/(*) 
            @"\(\*\)/\(\)",                  // (*)/() 
            @"\(\)/\(\*\)",                  // ()/(*) 
            @"\(\)/\(\)",                   // ()/() 
        };
        return RemovePattern.RegexPatterns(inp, patterns);
    }

    private static string SeparateOperatorsWithCdot(string inp)
    {
        if (inp.Contains("#13") is false && inp.Contains("#161#") is false) return inp;

        inp = TranslationTag.SeparateOperatorByTag(inp, "#13N#");
        inp = TranslationTag.SeparateOperatorByTag(inp, "#161#");

        return inp;
    }





    /// <summary>
    /// Wrap inp inside ti-nspire derivative block and add variable [any char, check "xyz" first]
    /// <para/> Runs if item.Settings.Autoderivative == true or inp starts with 'D'
    /// </summary>
    /// <param name="inp"></param>
    /// <param name="item"></param>
    /// <returns>derivative(inp,x) wrapped inp</returns>
    private static string CheckDerivative(string inp, TranslationItem item)
    {
        // check if needed to add "derivative()"
        if (item.Settings.AutoDerivative)
        {
            return RunAutoDerivative(inp);
        }

        if (inp.StartsWith('D'))
        {
            return RunAutoDerivative(inp.Remove(0, 1));
        }

        return inp;
    }

    /// <summary>
    /// Removes unesessary symbols like "…"
    /// </summary>
    /// <param name="inp"></param>
    /// <returns>inp with unnesessary symbols removed</returns>
    private static string RemoveEndChangesSymbols(string inp)
    {
        // remove non needed symbols
        return inp.Replace("…", "");
    }

    /// <summary>
    /// Wrap inp inside ti-nspire solve block and add variable x, y or z if found like "solve(inp,x)"
    /// <para/>Runs in any case
    /// </summary>
    /// <param name="inp"></param>
    /// <returns>solve(inp,x) wrapped inp</returns>
    private static string RunAutoSolve(string inp)
    {
        string[] symbols = { ">", "<", "=" };
        if (symbols.Any(inp.Contains))
        {
            string variables = "";
            if (inp.Contains('x'))
            {
                variables += ",x";
            }
            if (inp.Contains('y'))
            {
                variables += ",y";
            }
            if (inp.Contains('z'))
            {
                variables += ",z";
            }
            inp = $"solve({inp}{variables})";
        }
        return inp;
    }

    /// <summary>
    /// Put inp inside derivative() and add vars if found in inp
    /// </summary>
    /// <param name="inp"></param>
    /// <returns></returns>
    private static string RunAutoDerivative(string inp)
    {
        char? variable = FindVariable(inp);
        if (variable is null)
        {
            return $"derivative({inp},)";
        }
        return $"derivative({inp},{variable})";
    }

    /// <summary>
    /// GetBottom first character that appears to be in inp (x, y and z are checked first)
    /// </summary>
    /// <param name="inp"></param>
    /// <returns>char "variable" or null if no chars in inp</returns>
    private static char? FindVariable(string inp)
    {
        char[] variables = { 'x', 'y', 'z' };
        variables = variables.Concat(GetLowerCaseChars()).ToArray();

        foreach (char c in variables)
        {
            if (inp.Contains(c))
            {
                return c;
            }
        }
        return null;
    }

    /// <summary>
    /// GetBottom array of all lower case chars
    /// </summary>
    /// <returns></returns>
    private static char[] GetLowerCaseChars()
    {
        // get all lowercase chars
        return Enumerable.Range('a', 26).Select(x => (char)x).ToArray();
    }

    /// <summary>
    /// Translate i and e (neper and imaginary unit)
    /// </summary>
    /// <param name="inp"></param>
    /// <returns>inp with translated i and e</returns>
    private static string SpecialSymbols(string inp)
    {
        // change special symbols
        inp = inp.Replace("e", "@e");

        if (inp.Contains('i') is false) return inp;

        for (int i = 0; i < inp.Length; i++)
        {
            inp = TranslateImaginaryUnit(inp, ref i);
        }

        return inp;
    }

    /// <summary>
    /// if inp[index] is 'i', make checks and add @ if needed
    /// </summary>
    /// <param name="inp"></param>
    /// <param name="index"></param>
    /// <returns>inp with one more @i translated</returns>
    private static string TranslateImaginaryUnit(string inp, ref int index)
    {
        if (inp[index] is not 'i') return inp;

        if (index == 0)
        {
            index++;
            return $"@{inp}";       // add @-symbol in front of inp
        }

        char charBeforeIndex = inp[(index - 1)];

        if (charBeforeIndex is not 'p')
        {
            inp = $"{inp[..index]}@{inp[index..]}";     // add @-symbol before index
            index++;
            return inp;
        }
        return inp;
    }
}
