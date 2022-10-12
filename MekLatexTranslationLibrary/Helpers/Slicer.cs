namespace MekLatexTranslationLibrary.Helpers;
internal static class Slicer
{
    /// <returns>substring of input, if fails returns string.Empty</returns>
    internal static string GetSpanSafely(string input, Range range)
    {
        try
        {
            return input[range];
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine($"[Translation] Index out of range, Range '{range}' can't be read from input: '{input}', returns empty string");
            return string.Empty;
        }
    }


    /// <returns>substring of input, if fails returns ""</returns>
    internal static string GetSpanSafely(string input, int startIndex, int length)
    {
        return GetSpanSafely(input, startIndex..(startIndex + length));
    }

    internal static char? GetCharSafely(string input, int startIndex)
    {
        try
        {
            return input[startIndex];
        }
        catch (IndexOutOfRangeException)
        {
            Console.WriteLine($"[Translation] Index out of range, Can't read char with index '{startIndex}' from input: '{input}', returns null");
            return null;
        }
    }

    internal static char? GetLastCharSafely(string input)
    {
        if (string.IsNullOrEmpty(input)) return null;
        return input[^1];
    }
}
