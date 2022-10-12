
namespace MekLatexTranslationLibrary;
public enum TranslationError
{
    Frac_NoFirstEndBracket = 1,
    Frac_NoSecondStartBracket = 2,
    Frac_NoSecondEndBracket = 3,
    Matcha_PageConstructorEndBracketNotFound = 4,
    Log_BasisNotFound = 5,
    Lim_NoApproachValue = 6,
    NthRoot_NoEndFound = 7,
    NthRoot_NoSecondStartBracket = 8,
    Sqrt_NoStartBracketFound = 9,
    VecBar_NoEndBracketFound = 10,
    Sum_NoVariableFound = 11,
    NthRoot_NoFirstClosingBracket = 12,
    Power_NoEndingBracketFound = 13,
    Cases_NoEndBracketFound = 14,
    Binom_NoFirstStart = 15,
    BinomNoSecondStart = 16,
}
