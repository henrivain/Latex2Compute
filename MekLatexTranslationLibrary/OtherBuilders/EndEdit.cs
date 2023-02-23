/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;
using System.Text.RegularExpressions;

namespace MekLatexTranslationLibrary.OtherBuilders;

static internal class EndEdit
{
    /// <summary>
    /// Translates all operator tags to ti-nspire form and deletes empty operators. Do Matcha changes
    /// <para/>most of the operator names are seen as units with physics mode => uses tags (example "s" = second => "system" [problem] => uses "#121#")
    /// <para/>Removes empty divisions ["()/()" => ""], Adds derivative or solve if needed [solve(input,x)]
    /// </summary>
    /// <param name="input"></param>
    /// <returns>input with made end change</returns>
    internal static string Run(string input, TranslationArgs args, ref List<TranslationError> errors)
    {

        // remove matcha document constructors (do before curly bracket removal)
        if (args.MatchaEnabled)
        {
            input = Matcha.MakeMatchaChanges(input, ref errors);
        }

        // change special
        if (args.SpecialSymbolTranslation)
        {
            input = SpecialSymbols(input);
        }

        // remove unused curly brackets
        if (args.CurlyBracket)
        {
            input = input.Replace("{", "");
            input = input.Replace("}", "");
        }
        // remove not useful
        input = input.Replace("\\", "");

        if (args.AutoSeparateOperators)
        {
            input = SeparateOperatorsWithCdot(input);
        }

        if (args.AutoSolve && input.Contains("#192#") is false)
        {
            // if auto solve is on => call autosolve method and check if conditions are true
            input = RunAutoSolve(input);
        }
        input = CheckDerivative(input, args);


        input = TranslationTag.ToNspireOperator(input);

        switch (args.EndChanges)
        {
            case "all":
                // remove all not wanted items
                input = RemoveEmptyFractions(input);
                input = RemoveEndChangesSymbols(input);
                break;

            case "emptyFrac":
                //remove empty fractions
                input = RemoveEmptyFractions(input);
                break;
        }

        

        return input;
    }

    /// <summary>
    /// Removes all empty fractions from input like ()/() or (^2)/() or ^2/^2
    /// </summary>
    /// <param name="inp"></param>
    /// <returns>input with fractions removed</returns>
    private static string RemoveEmptyFractions(string inp)
    {
        inp = RemoveUnnecessaryOperatorSigns(inp);

        /*lang=regex*/string[] patterns =
        {
            // [,+,-,*,^2] means, that can be '+', '-', '*', '^2' or empty ''
            // 2 can be any number 
            @"\((\*|\+|\-|\^[0-9]|\^\([0-9]?\))?\)/\((\*|\+|\-|\^[0-9]|\^\([0-9]?\))?\)",       // ([,+,-,*,^2,(^2)])/([,+,-,*,^2,^(2),^()])
            @"(\*|\+|\-|\(\))\^([0-9]|\([0-9]\))",      // [+,-,*,()]^(2)  or  [+,-,*,()]^2
            @"\(\)/\([0-9]\)",                          // ()/(2)
            @"\([0-9]\)/\(\)",                          // (2)/()
            @"(\^[0-9])?/\^[0-9]",                      // ^2/^2 or /^2
            @"\^[0-9]/",                                // ^2/
            @"\(\)/\(\)",                               // ()/() 
        };
        return RemovePattern.RegexPatterns(inp, patterns);
    }


    private record struct ReplacableRegexPattern(string Pattern, string Replacement);

    private static string RemoveUnnecessaryOperatorSigns(string inp)
    {
        inp ??= string.Empty;

        // if inp ends with any operator, remove it
        char[] operators = { '+', '-', '/', '*' };
        if (operators.Contains(Slicer.GetLastCharSafely(inp) ?? '\0'))
        {
            inp = inp[..^1];
        }

        // replace patterns
        ReplacableRegexPattern[] patterns =
        {
            new($@"\((\*|\+|\-|/)\^", "(^"),        // '(*^', '(+^', '(-^' or '(/^'      
        };
        foreach (var pattern in patterns)
        {
            while (Regex.IsMatch(inp, pattern.Pattern))
            {
                inp = Regex.Replace(inp, pattern.Pattern, pattern.Replacement);
            }
        }
        return inp;
    }

    private static string SeparateOperatorsWithCdot(string inp)
    {
        if (inp.Contains("#13") is false && inp.Contains("#161#") is false) return inp;

        inp = TranslationTag.SeparateOperatorByTag(inp, "#13N#");
        inp = TranslationTag.SeparateOperatorByTag(inp, "#161#");

        return inp;
    }





    /// <summary>
    /// Wrap input inside ti-nspire derivative block and add variable [any char, check "xyz" first]
    /// <para/> Runs if input.Settings.Autoderivative is true or input starts with 'D'
    /// </summary>
    /// <param name="input"></param>
    /// <param name="args"></param>
    /// <returns>derivative(input,x) wrapped input</returns>
    private static string CheckDerivative(string input, TranslationArgs args)
    {
        // check if needed to add "derivative()"
        if (args.AutoDerivative)
        {
            return RunAutoDerivative(input);
        }
        if (input.StartsWith('D'))
        {
            return RunAutoDerivative(input.Remove(0, 1));
        }
        return input;
    }

    /// <summary>
    /// Removes unesessary symbols like "…"
    /// </summary>
    /// <param name="inp"></param>
    /// <returns>input with unnesessary symbols removed</returns>
    private static string RemoveEndChangesSymbols(string inp)
    {
        // remove non needed symbols
        return inp.Replace("…", "");
    }

    /// <summary>
    /// Wrap input inside ti-nspire solve block and add variable x, y or z if found like "solve(input,x)"
    /// <para/>Runs in any case
    /// </summary>
    /// <param name="inp"></param>
    /// <returns>solve(input,x) wrapped input</returns>
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
            inp = $"#192#({inp}{variables})";
        }
        return inp;
    }

    /// <summary>
    /// Put input inside derivative() and add vars if found in input
    /// </summary>
    /// <param name="inp"></param>
    /// <returns></returns>
    private static string RunAutoDerivative(string inp)
    {
        char? variable = FindVariable(inp);
        if (variable is null)
        {
            return $"#191#({inp},)";
        }
        return $"#191#({inp},{variable})";
    }

    /// <summary>
    /// GetBottom first character that appears to be in input (x, y and z are checked first)
    /// </summary>
    /// <param name="inp"></param>
    /// <returns>char "variable" or null if no chars in input</returns>
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
    /// <returns>input with translated i and e</returns>
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
    /// if input[index] is 'i', make checks and add @ if needed
    /// </summary>
    /// <param name="inp"></param>
    /// <param name="index"></param>
    /// <returns>input with one more @i translated</returns>
    private static string TranslateImaginaryUnit(string inp, ref int index)
    {
        if (inp[index] is not 'i') return inp;

        if (index == 0)
        {
            index++;
            return $"@{inp}";       // add @-symbol in front of input
        }

        char charBeforeIndex = inp[index - 1];

        if (charBeforeIndex is not 'p')
        {
            inp = $"{inp[..index]}@{inp[index..]}";     // add @-symbol before index
            index++;
            return inp;
        }
        return inp;
    }
}
