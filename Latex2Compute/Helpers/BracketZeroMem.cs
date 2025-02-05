namespace Latex2Compute.Helpers;

internal static class BracketZeroMem
{
    /// <summary>
    /// Finds the end bracket start index.
    /// Counts opening and closing brackets to find correct pair.
    /// DO NOT INCLUDE START BRACKET IN THE INPUT.
    /// </summary>  
    /// <param name="input">Input text where return index is searched from.</param>
    /// <param name="start">Start bracket, for example "{"</param>
    /// <param name="end">End bracket, for example "}"</param>
    /// <returns>Bracket index if found, otherwise -1.</returns>
    internal static int FindEnd(
            ReadOnlySpan<char> input,
            ReadOnlySpan<char> start,
            ReadOnlySpan<char> end, 
            int startIndex = 0)
    {
        int startCursor = startIndex;
        int endCursor = startIndex;
        while (true)
        {
            int nextStart = input[startCursor..].IndexOf(start);
            int nextEnd = input[endCursor..].IndexOf(end);

            if (nextEnd < 0)
            {
                return -1;
            }

            if (nextStart < 0)
            {
                return nextEnd + endCursor;
            }


            if (nextStart + startCursor < nextEnd + endCursor)
            {
                startCursor += nextStart + start.Length;
                endCursor += nextEnd + end.Length;
            }
            else
            {
                return endCursor + nextEnd;
            }
        }
    }



    internal static ReadOnlyMemory<char> GetSpanSafely(ReadOnlyMemory<char> input, Range range)
    {
        // Is start index out of bounds
        if (range.Start.Value >= input.Length)
        {
            return Memory<char>.Empty;
        }
        // Is end index out of bounds
        if (range.End.Value >= input.Length)
        {
            return input[range.Start..];
        }
        return input[range];
    }
}