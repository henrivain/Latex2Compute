
namespace MekLatexTranslationLibrary;
public enum TranslationError
{
    Frac_NoSecondStartBracket = 2,
    Frac_NoSecondEndBracket = 3,
    Log_BasisNotFound = 5,
    Lim_NoApproachValue = 6,
    Sum_NoVariableFound = 11,
    Frac_NoFirstEndBracket = 12,
    Frac_NoEndFound = 15,
    NthRoot_NoFirstClosingBracket = 16,
    Power_NoEndingBracketFound = 18,
    NthRoot_NoSecondStartBracket = 23,
    NthRoot_NoSecondEndBracket = 25,
    Sqrt_InputDoesNotStartWithBracket = 26,
    Cases_EndBeforeStart = 27,
    Cases_NoEndBracketFound = 28,
}
