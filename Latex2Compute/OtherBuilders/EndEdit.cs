﻿/// Copyright 2021 Henri Vainio 
using System.Text.RegularExpressions;

namespace Latex2Compute.OtherBuilders;

static internal class EndEdit
{
    /// <summary>
    /// Translates all operator tags to ti-nspire form and deletes empty operators. Do Matcha changes
    /// <para/>most of the operator names are seen as units with physics mode => uses tags (example "s" = second => "system" [problem] => uses "#121#")
    /// <para/>Removes empty divisions ["()/()" => ""], Adds derivative or solve if needed [solve(input,x)]
    /// </summary>
    /// <param name="input"></param>
    /// <returns>input with made end change</returns>
    internal static string Run(string input, TranslationArgs args, ref TranslationErrors errors)
    {
        // remove matcha document constructors (do before curly bracket removal)
        if (args.IsSet(Params.MathchaEnabled))
        {
            input = Mathcha.MakeMathchaChanges(input, ref errors);
        }

        // change special
        if (args.IsSet(Params.SpecialSymbolTranslation))
        {
            input = SpecialSymbols(input);
        }

        // remove unused curly brackets
        if (args.IsSet(Params.RemoveCurlyBracket))
        {
            input = input.Replace("{", "");
            input = input.Replace("}", "");
        }
        // remove not useful
        input = input.Replace("\\", "");

        if (args.IsSet(Params.AutoSeparateOperators))
        {
            input = SeparateOperatorsWithCdot(input);
        }

        // auto solve
        if (args.IsSet(Params.AutoSolve) && 
            input.Contains(ConstSymbol.Solve) is false)
        {
            // if auto solve is on => call autosolve method and check if conditions are true
            input = RunAutoSolve(input);
        }

        // auto derivative
        if (args.IsSet(Params.AutoDerivative))
        {
            input = RunAutoDerivative(input);
        }
        if (input.StartsWith('D'))
        {
            // Skip first character
            input = RunAutoDerivative(input.AsSpan(1));
        }


        input = TranslationTag.ToNspireOperator(input);

        switch (args.EndChanges)
        {
            case EndChanges.All:
                // remove all not wanted items
                input = RemoveEmptyFractions(input);
                input = input.Replace("…", "");
                break;

            case EndChanges.EmptyFrac:
                //remove empty fractions
                input = RemoveEmptyFractions(input);
                break;
        }

        return RemoveStartEqualSigns(input).ToString();
    }

    private static ReadOnlySpan<char> RemoveStartEqualSigns(ReadOnlySpan<char> input)
    {
        if (input.IsEmpty)
        {
            return ReadOnlySpan<char>.Empty;
        }
        if (input[0] is '=' or '+')
        {
            return RemoveStartEqualSigns(input[1..]);
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

        // TODO: source gen regex
        /*lang=regex*/
        string[] patterns =
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
        ReadOnlySpan<char> operators = stackalloc char[]{ '+', '-', '/', '*' };
        if (operators.Contains(Slicer.GetLastCharSafely(inp) ?? '\0'))
        {
            inp = inp[..^1];
        }

        // replace patterns
        // TODO: source gen regex
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
        if (inp.Contains("#13") is false && inp.Contains(ConstSymbol.Pi) is false)
        {
            return inp;
        }

        inp = TranslationTag.SeparateOperatorByTag(inp, ConstSymbol.AnyTrigon);
        inp = TranslationTag.SeparateOperatorByTag(inp, ConstSymbol.Pi);

        return inp;
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
        if (symbols.Any(inp.Contains) is false)
        {
            return inp;
        }

        string? variables = string
            .Concat(GetVariablesInInput(inp)
            .Select(x => $",{x}")
            );

        inp = $"#192#({inp}{variables})";
        return inp;
    }

    /// <summary>
    /// Put input inside derivative() and add vars if found in input
    /// </summary>
    /// <param name="inp"></param>
    /// <returns></returns>
    private static string RunAutoDerivative(ReadOnlySpan<char> inp)
    {
        char? variable = GetFirstVariableCharInInput(inp);
        if (variable is null)
        {
            return $"{ConstSymbol.Derivative}({inp},)";
        }
        return $"{ConstSymbol.Derivative}({inp},{variable})";
    }



    /// <summary>
    /// Get first letter that appears to be in input (x, y and z are checked first)
    /// </summary>
    /// <param name="inp"></param>
    /// <returns>char "variable" or null if no chars in input</returns>
    private static char? GetFirstVariableCharInInput(ReadOnlySpan<char> input)
    {
        foreach (char c in WeightedVariableLetters)
        {
            if (input.Contains(c))
            {
                return c;
            }
        }
        return null;
    }

    /// <summary>
    /// Get all lowercase letters that appear in input
    /// </summary>
    /// <param name="input"></param>
    /// <returns>array of lowercase letters that are found in input or empty array if none found</returns>
    private static char[] GetVariablesInInput(ReadOnlySpan<char> input)
    {
        List<char> variables = new();
        foreach (char c in LowerCaseLetters)
        {
            if (input.Contains(c))
            {
                variables.Add(c);
            }
        }
        return variables.ToArray();
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



    static char[] WeightedVariableLetters { get; } = new char[] { 'x', 'y', 'z' }.Concat(GetLowerCaseChars()).ToArray();
    static char[] LowerCaseLetters { get; } = GetLowerCaseChars();
    static char[] GetLowerCaseChars() => Enumerable.Range('a', 26).Select(x => (char)x).ToArray();
}