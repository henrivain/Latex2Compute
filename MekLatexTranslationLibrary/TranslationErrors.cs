
namespace MekLatexTranslationLibrary;

[Flags]
public enum TranslationErrors
{
    None = 0,
    Frac_NoFirstEndBracket =                    1 << 1,
    Frac_NoSecondStartBracket =                 1 << 2,
    Frac_NoSecondEndBracket =                   1 << 3,
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
    MissinMatrixEnd =                           1 << 17,
}
