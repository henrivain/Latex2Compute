/// Copyright 2021 Henri Vainio 
using System.Text.RegularExpressions;

namespace MekLatexTranslationLibrary.Helpers;

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
            Console.WriteLine($"[TranslationError] {error}");
        }
    }

    /// <summary>
    /// Add translationerror to list and print it to console if in debug mode
    /// </summary>
    /// <param name="error">error to add to errors list</param>
    /// <param name="errors">errors list which includes all errors in translate process</param>
    internal static void TranslationError(TranslationError error, ref List<TranslationError> errors)
    {
        errors.Add(error);
        if (Translation.LatexInDevelopment)
        {
            Console.WriteLine($"[TranslationError] {nameof(error)}");
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
