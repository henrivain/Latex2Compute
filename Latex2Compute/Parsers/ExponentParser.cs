﻿namespace Latex2Compute.Parsers;
internal class ExponentParser
{
    public static string BuildAll(string input, ref Errors errors)
    {
        //find and translate longer rise to power    ^{x} => ^(x) 

        while (true)
        {
            int startBracket = input.IndexOf("^{");
            if (startBracket < 0)
            {
                return input;
            }

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
                errors |= Errors.Power_NoEndingBracketFound;
                Helper.PrintError(Errors.Power_NoEndingBracketFound);
            }
        }
    }
}
