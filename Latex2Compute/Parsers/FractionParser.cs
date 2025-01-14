// Copyright 2025 Henri Vainio 

namespace Latex2Compute.Parsers;

internal class FractionParser
{
    const string OperatorStart = "\\frac";
    const string OperatorStartVariantD = "\\dfrac";


    internal static string BuildAll(string input, ref Errors errors)
    {
        input = input.Replace(OperatorStartVariantD, OperatorStart);
        while (true)
        {
            var fraction = Build(input.AsMemory(), OperatorStart, ref errors);
            if (fraction is null)
            {
                return input;
            }
            input = fraction.Value.ToString();
        }
    }

    private struct Fraction
    {
        public Fraction() { }
        public ReadOnlyMemory<char> Latex { get; init; }
        public Range Before { get; set; }
        public Range Numerator { get; set; }
        public Range Denominator { get; set; }
        public Range After { get; set; }

        /// <summary>
        /// Get struct as parsed fraction representation ()/()
        /// </summary>
        /// <returns>Parsed fraction {before}({top})/({bottom}){after}</returns>
        public override readonly string ToString()
        {
            if (Numerator.Start.Value == Numerator.End.Value && 
                Denominator.End.Value == Denominator.End.Value)
            {
                return $"{Latex[Before]}{Latex[After]}";
            }
            return $"{Latex[Before]}({Latex[Numerator]})/({Latex[Denominator]}){Latex[After]}";
        }
    }
    
    private static Fraction? Build(ReadOnlyMemory<char> input, ReadOnlySpan<char> opening,  ref Errors errors)
    {
        var parser = new SimpleParser(input);
        var numerator = parser.SeparateScopes(opening, "{", "}", Errors.Frac_NoNumerEnd);
        if (numerator.Found is false)
        {
            return null;
        }

        var denominator = parser.Continue(numerator, "{", "}", Errors.Frac_NoDenomStart, Errors.Frac_NoDenomEnd);

        errors |= numerator.Errors | denominator.Errors;
        return new Fraction
        {
            Latex = input,
            Before = numerator.Before,
            Numerator = numerator.Content,
            Denominator = denominator.Content,
            After = denominator.After
        };
    }
}
