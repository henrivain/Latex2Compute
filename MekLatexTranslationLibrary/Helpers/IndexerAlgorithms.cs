/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Structures;
using MekLatexTranslationLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MekLatexTranslationLibrary.Helpers
{
    /// <summary>
    /// Algoritms to get string or char indexes from string
    /// </summary>
    internal static class IndexerAlgorithms
    {
        private static readonly string[] OtherSeparators = { "#", "\\s", "\\c", "\\t" };     // system line change = #122#, \\s = sin, \\c = cos, \\t = tan

        /// <summary>
        /// GetBottom first indexes of operators =, &lt;, &gt;, +, -, * or ], ), }, #, \\s, \\c, \\t if no pair for them
        /// </summary>
        /// <param name="inp"></param>
        /// <returns>int list of indexes, returns empty list if none</returns>
        /// <exception cref="Exception"></exception>
        internal static List<int> GetFirstOperatorIndexes(string inp)
        {

            List<int> indexes = new();

            // normal operators
            indexes.AddRange(
                GetFirstIndexes(inp.Replace("\\cdot", "*"), CommonOperators._basicMathOperators));
            
            // equal separators
            indexes.AddRange(
                GetFirstIndexes(inp, CommonOperators._equalSigns));
            
            // brackets
            indexes.AddRange(
                GetFirstBracketIndexes(inp));

            // other separators
            indexes.AddRange(
                GetFirstIndexes(inp, OtherSeparators));

            if (indexes.Contains(-1)) throw new Exception("[IndexerAlgorithms.GetFirstOperatorIndexes] list includes -1");
            
            return indexes;
        }

        /// <summary>
        /// GetBottom IEnumerable of first appearances of given string operator
        /// </summary>
        /// <param name="inp"></param>
        /// <returns>IEnumerable of indexes, returns Empty list if none found</returns>
        private static IEnumerable<int> GetFirstIndexes(string inp, string[] operators)
        {
            List<int> indexes = new();
            foreach (var op in operators)
            {
                int index = inp.IndexOf(op);
                CheckUndef(index, ref indexes);
            }
            return indexes;
        }

        /// <summary>
        /// GetBottom IEnumerable of first appearances of given char operator
        /// </summary>
        /// <param name="inp"></param>
        /// <returns>IEnumerable of indexes, returns Empty list if none found</returns>
        private static IEnumerable<int> GetFirstIndexes(string inp, char[] operators)
        {
            List<int> indexes = new();
            foreach (var op in operators)
            {
                int index = inp.IndexOf(op);
                CheckUndef(index, ref indexes);
            }
            return indexes;
        }

        /// <summary>
        /// GetBottom Index of pairless brackets. Checks every bracket type.
        /// </summary>
        /// <param name="inp"></param>
        /// <returns>IEnumerable of indexes, returns Empty list if none found</returns>
        private static IEnumerable<int> GetFirstBracketIndexes(string inp)
        {
            List <int> indexes = new();
            foreach (var brackets in Brackets.All)
            {
                int index = BracketHandler.FindBrackets(inp, brackets.Opening, brackets.Closing);
                CheckUndef(index, ref indexes);
            }
            return indexes;
        }

        /// <summary>
        /// if index is >= 0, adds it to indexes list
        /// </summary>
        /// <param name="index"></param>
        /// <param name="indexes"></param>
        private static void CheckUndef(int index, ref List<int> indexes)
        {
            if (index >= 0)
            {
                indexes.Add(index); 
            }
        }
    }
}
