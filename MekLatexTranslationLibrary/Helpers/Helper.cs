/// Copyright 2021 Henri Vainio 
using System.Text.RegularExpressions;

namespace MekLatexTranslationLibrary.Helpers
{
    internal static class Helper
    {
        /// <summary>
        /// Writes error to Console if in develpment
        /// </summary>
        /// <param name="error"></param>
        internal static void DevPrintTranslationError(string error)
        {
            if (Translation.LatexInDevelopment)
            {
                Console.WriteLine(error);
            }
        }

        /// <summary>
        /// Gets smallest value of two, which is bigger than minValue
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="minValue"></param>
        /// <returns>smallest value or -1 if all two small</returns>
        internal static int GetSmallestValue(int value1, int value2, int minValue)
        {
            int bigger = Math.Max(value1, value2);
            int smaller = Math.Min(value1, value2);

            if (bigger < minValue) return -1;
            return (smaller < minValue) ? bigger : smaller;
        }
    }
}
