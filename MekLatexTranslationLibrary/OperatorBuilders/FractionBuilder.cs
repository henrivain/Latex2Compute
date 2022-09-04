/// Copyright 2021 Henri Vainio 
using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary.OperatorBuilders;

internal class FractionBuilder
{
    const string OperatorStart = "\\frac{";
    const string OperatorStartVariantD = "\\dfrac{";




    public static string BuildAll(string input, ref List<TranslationError> errors)
    {
        // Finds \frac{x}{y} and turns it to (x)/(y)
        //possible errors [1, 2, 3]
        int startIndex;
        int normalStartIndex;
        int dStartIndex;
        while (true)
        {
            normalStartIndex = input.IndexOf(OperatorStart);
            dStartIndex = input.IndexOf(OperatorStartVariantD);
            startIndex = GetSmallerButBiggerThan(dStartIndex, normalStartIndex, minValue:0);
            if (startIndex < 0) break;
            if (startIndex == normalStartIndex)
            {
                input = input.Remove(startIndex, OperatorStart.Length);
            }
            else
            {
                input = input.Remove(startIndex, OperatorStartVariantD.Length);
            }

            input = Build(input, startIndex, ref errors);
        }
        return input;
    }

    struct Fraction
    {
        public Fraction() { }
        public string TextBefore { get; set; } = string.Empty;
        public string TopContent { get; set; } = string.Empty;
        public string BottomContent { get; set; } = string.Empty;
        public string TextAfter { get; set; } = string.Empty;

        /// <summary>
        /// Get struct as parsed fraction representation ()/()
        /// </summary>
        /// <returns>Parsed fraction {before}({top})/({bottom}){after}</returns>
        public override string ToString()
        {
            return $"{TextBefore}({TopContent})/({BottomContent}){TextAfter}";
        }
    }

    internal static string Build(string input, int startIndex, ref List<TranslationError> errors)
    {
        Fraction f = new()
        {
            TextBefore = input[..startIndex]
        };

        (f.TopContent, int topEndIndex)= GetTop(input, startIndex, ref errors);
       
        if (topEndIndex >= 0)
        {
            (f.BottomContent, int bottomEndIndex) = GetBottom(input, topEndIndex, ref errors);
            if (bottomEndIndex >= 0)
            {
                f.TextAfter = input[bottomEndIndex..];
            }
        }
        return f.ToString();
    }
    private static (string content, int endIndex) GetTop(string input, int startIndex, ref List<TranslationError> errors)
    {
        int endIndex = BracketHandler.FindBrackets(input, BracketType.Curly, startIndex);
        if (endIndex < 0)
        {
            Helper.TranslationError(TranslationError.Frac_NoFirstEndBracket, ref errors);
            return (input[startIndex..], -1);
        }
        return (input[startIndex..endIndex], endIndex);
    }


    private static (string content, int endIndex) GetBottom(string input, int startIndex, ref List<TranslationError> errors)
    {
        bool useOffset = Slicer.GetSpanSafely(input, startIndex, 1) is "{";
        if (useOffset )
        {
            startIndex++;
        }
        else
        {
            Helper.TranslationError(TranslationError.Frac_NoSecondStartBracket, ref errors);
        }

        int endIndex = BracketHandler.FindBrackets(input, BracketType.Curly, startIndex);
        if (endIndex < 0) 
        {
            Helper.TranslationError(TranslationError.Frac_NoSecondEndBracket, ref errors);
            endIndex = input.Length;
        }
        return (input[startIndex..endIndex], endIndex);
    }
    private static int GetSmallerButBiggerThan(int val1, int val2, int minValue)
    {
        if (val1 < minValue) return val2;
        if (val2 < minValue) return val1;
        return Math.Min(val1, val2);
    }


}
