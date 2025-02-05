
namespace Latex2Compute;

[Flags]
public enum Errors
{
    None = 0,
    Frac_NoNumerEnd =                    1 << 1,
    Frac_NoDenomStart =                 1 << 2,
    Frac_NoDenomEnd =                   1 << 3,
    Matcha_PageConstructorEndBracketNotFound =  1 << 4,
    Log_BasisNotFound =                         1 << 5,
    Lim_NoApproachValue =                       1 << 6,
    NthRoot_NoEndFound =                        1 << 7,
    NthRoot_NoSecondStartBracket =              1 << 8,
    Sqrt_NoStartBracketFound =                  1 << 9,
    VecBar_NoEndBracketFound =                  1 << 10,
    Sum_NoVariableFound =                       1 << 11,
    NthRoot_NoFirstClosingBracket =             1 << 12,
    Power_NoEndingBracketFound =                1 << 13,
    Cases_NoEndBracketFound =                   1 << 14,
    Binom_NoFirstStart =                        1 << 15,
    BinomNoSecondStart =                        1 << 16,
    MissingMatrixEnd =                          1 << 17,
    MatrixInvalidTarget = 1 << 18,
}


public static class TranslationErrorsExtensions
{
    public static bool HasFlag(this Errors value, Errors flag)
    {
        return value.HasFlag(flag);
    }

    public static string[] GetErrors(this Errors value)
    {
        if (value is Errors.None)
        {
            return new string[] { Errors.None.ToString() };
        }

        return Enum.GetValues<Errors>()
            .Cast<Errors>()
            .Where(v => value.HasFlag(v) && v != Errors.None)
            .Select(v => v.ToString())
            .ToArray();
    }

    public static string ToErrorString(this Errors value)
    {
        return string.Join(", ", value.GetErrors());
    }
}
