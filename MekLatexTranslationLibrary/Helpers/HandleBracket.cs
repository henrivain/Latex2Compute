/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Structures;

namespace MekLatexTranslationLibrary.Helpers
{
    internal static class HandleBracket
    {
        /// <summary>
        /// Find closing bracket of element, if start not given starts from index 0
        /// </summary>
        /// <param name="input"></param>
        /// <param name="bracketType"></param>
        /// <param name="startPoint"></param>
        /// <returns>index of end bracket, if not found returns -1</returns>
        public static int FindBrackets(string input, string bracketType, int startPoint = 0)
        {
            //finds and returns index of start bracket's pair
            //(bracket type doesn't have to be brackets, can also be "¤&")
            //if None => -1
            //Example: 236{7}33}33 => 8
            char opening = bracketType[0];
            char closing = bracketType[1];
            
            int x = 1;
            int endBracket = -1;



            for (int i = startPoint; i < input.Length; i++)
            {
                if (input[i] == opening)
                {
                    x++;
                }
                else if (input[i] == closing)
                {
                    x--;
                    if (x == 0)
                    {
                        endBracket = i++;
                        break;
                    }
                }
            };
            return endBracket;
        }



        /// <summary>
        /// get everything inside \left( \right) brackets
        /// </summary>
        /// <param name="inp"></param>
        /// <param name="startIndex"></param>
        /// <returns>string[insides, int end index as string], if end not found, end = -1</returns>
        public static ContentAndEnd GetCharsBetweenBrackets(string inp, int startIndex)
        {
            string startChars = HelperAlgorithms.CheckNextCharsSafely(inp, 6, startIndex);

            if (startChars is "\\left(")
            {
                return FindEndBracket(inp, startIndex);
            }

            // if is not continuing or does not start with bracket
            return new(-1, "");
        }
        private static ContentAndEnd FindEndBracket(string inp, int startIndex)
        {
            // leave opening bracket
            string tempInp = GetTempInp(inp, startIndex);
            int endBracket = FindBrackets(tempInp, "()", 0);

            if (endBracket is -1)
            {
                // no end bracket => end = content
                return new(inp.Length, tempInp);
            }

            // end bracket => create content
            int endIndex = startIndex + endBracket + "\\left(".Length + "\\right)".Length;
            return new(endIndex, tempInp[..endBracket]);
        }
        private static string GetTempInp(string inp, int startIndex)
        {
            string tempInp = inp[(startIndex + 6)..];
            tempInp = tempInp.Replace("\\left(", "(");
            return tempInp.Replace("\\right)", ")");
        }
    }
}
