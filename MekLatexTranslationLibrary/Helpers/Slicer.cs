namespace MekLatexTranslationLibrary.Helpers;
internal static class Slicer
{
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
    internal static string GetSpanSafely(string input, int startIndex, int length)
    {
        return GetSpanSafely(input, startIndex..(startIndex + length));
    }
}
