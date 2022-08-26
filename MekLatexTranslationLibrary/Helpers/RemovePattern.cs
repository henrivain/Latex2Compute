
using System.Text.RegularExpressions;
/// Copyright 2021 Henri Vainio 
namespace MekLatexTranslationLibrary.Helpers
{
    internal class RemovePattern
    {
        /// <summary>
        /// Removes Latex pattern (like "\text" or "\mathrm") which is not needed
        /// <para/>use patter like "\text" and bracketType like "{}"
        /// </summary>
        /// <param name="inp"></param>
        /// <param name="pattern"></param>
        /// <returns>inp with all patterns removed</returns>
        internal static string BracketsAfterLatex(string inp, string pattern, string bracketType = "{}")
        {
            // check that bracket type is valid
            if (bracketType is not "{}" && bracketType is not "[]" && bracketType is not "()")
            {
                throw new ArgumentException($"[Helper.RemoveLatexPatternWithBrackets] bracketType is not valid [use \"{{}}\", \"[]\" or \"()\"]; was given {bracketType}");
            }
            pattern += bracketType[0];

            while (inp.Contains(pattern))
            {
                int startIndex = inp.IndexOf(pattern);
                inp = inp.Remove(startIndex, pattern.Length);
                int endIndex = HandleBracket.FindBrackets(inp, bracketType, startIndex);
                if (endIndex is not -1)
                {
                    inp = inp.Remove(endIndex, 1);
                }
            }
            return inp;
        }

        /// <summary>
        /// Remove all appearences of every string pattern in list like [ "()/()", "()/(^2)" ]
        /// </summary>
        /// <param name="inp"></param>
        /// <param name="patterns"></param>
        /// <returns>inp with patterns removed</returns>
        internal static string StringPatterns(string inp, string[] patterns)
        {
            foreach (var pattern in patterns)
            {
                // loop to remove every instance
                while (inp.Contains(pattern))
                {
                    inp = inp.Replace(pattern, "");
                }
            }
            return inp;
        }

        /// <summary>
        /// Removes all appearances of given regex patterns in given order
        /// </summary>
        /// <param name="inp"></param>
        /// <param name="patterns"></param>
        /// <returns>inp with removed pattrns</returns>
        internal static string RegexPatterns(string inp, string[] patterns)
        {
            foreach (string pattern in patterns)
            {
                while (Regex.IsMatch(inp, pattern))
                {
                    inp = Regex.Replace(inp, pattern, "");
                }
            }
            return inp;
        }
    }
}
