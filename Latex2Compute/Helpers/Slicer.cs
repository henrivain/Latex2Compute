namespace Latex2Compute.Helpers;
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

    /// <returns>Defined span of input, if fails returns <see cref="ReadOnlySpan{T}.Empty"/></returns>
    internal static ReadOnlySpan<char> GetSpanSafely(this ReadOnlySpan<char> input, Range range)
    {
        try
        {
            return input[range];
        }
        catch (ArgumentOutOfRangeException)
        {
            return ReadOnlySpan<char>.Empty;
        }
    }

    internal static string GetSpanSafely(string input, int startIndex, int length)
    {
        return GetSpanSafely(input, startIndex..(startIndex + length));
    }

    internal static char? GetCharSafely(in string input, int startIndex)
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

    internal static char? GetLastCharSafely(ReadOnlySpan<char> input)
    {
        return input.IsEmpty ? null : input[^1];
    }

    internal static List<Range> Split(
        this ReadOnlySpan<char> latex,
        ReadOnlySpan<char> separator,
        Range? splittableSpan = null)
    {
        // If no range is given, start from the beginning and search entire span.
        int last = splittableSpan?.Start.Value ?? 0;
        int endIndex = splittableSpan?.End.Value ?? latex.Length;

        List<Range> result = new();
        while (true)
        {

            int find = latex[last..].IndexOf(separator);
            int current = find + last;
            // Find is -1 if not found, current is the real index.
            if (find < 0 || endIndex < current)
            {
                result.Add(last..endIndex);
                return result;
            }

            result.Add(last..current);
            last = current + separator.Length;
        }
    }
}
