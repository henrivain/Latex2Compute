using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary.OperatorBuilders;
internal class RiseToPowerBuilder
{
    public static string BuildAll(string input, ref TranslationErrors errors)
    {
        //find and translate longer rise to power    ^{x} => ^(x) 

        while (true)
        {
            int startBracket = input.IndexOf("^{");
            if (startBracket < 0) return input;

            input = input.Remove(startBracket, 2)
                         .Insert(startBracket, "^(");

            int endBracket = BracketHandler.FindBrackets(input, BracketType.Curly, startBracket);
            endBracket--;

            if (endBracket > -1)
            {
                input = input.Remove(endBracket, 1)
                             .Insert(endBracket, ")");
            }
            else
            {
                //if no ending bracket => closing = end
                input += ")";
                errors |= TranslationErrors.Power_NoEndingBracketFound;
                Helper.PrintError(TranslationErrors.Power_NoEndingBracketFound);
            }
        }
    }
}
