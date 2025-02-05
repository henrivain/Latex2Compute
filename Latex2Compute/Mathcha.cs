/// Copyright 2021 Henri Vainio 
using Latex2Compute.Helpers;

namespace Latex2Compute;

/// <summary>
/// Translation algorithms for Matcha io symbols and constructors
/// </summary>
internal static class Mathcha
{
    public static string MakeMathchaChanges(string input, ref Errors errors)
    {
        // method removes unuseful latex groups that matcha.io adds to their latex documents
        input = input.Replace("\\displaystyle", "");
        input = input.Replace("$", "");

        while (input.Contains("\\end{") || input.Contains("\\begin{"))
        {
            int startIndex = input.IndexOf("\\end{");
            if (startIndex == -1)
            {
                // if contains begin instead of end
                startIndex = input.IndexOf("\\begin{");
                input = input.Remove(startIndex, 7);
            }
            else
            {
                // remove end
                input = input.Remove(startIndex, 5);
            }
            input = RemoveConstructorArgs(input, startIndex, ref errors);
        }
        return input;
    }

    private static string RemoveConstructorArgs(string input, int startIndex, ref Errors errors)
    {
        // remove arguments from macha io document constructors or math field definers
        int end = BracketHandler.FindBrackets(input, '{', '}', startIndex);

        if (end == -1)
        {
            // no end to matcha page constructor => skip
            errors |= Errors.Matcha_PageConstructorEndBracketNotFound;
            Helper.PrintError(Errors.Matcha_PageConstructorEndBracketNotFound.ToString());
        }
        else
        {
            input = input.Remove(startIndex, end - startIndex + 1);
        }
        return input;
    }
}
