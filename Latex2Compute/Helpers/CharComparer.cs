namespace Latex2Compute.Helpers;

internal static class CharComparer
{
    /// <summary>
    /// Get index of first char that is not a letter
    /// </summary>
    /// <param name="input"></param>
    /// <param name="startIndex"></param>
    /// <returns>index of first not letter char, returns length of input if every one is a letter</returns>
    internal static int GetIndexOfFirstNonLetter(ReadOnlySpan<char> input, int startIndex = 0)
    {
        ReadOnlySpan<char> alphabet = stackalloc char[]
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };

        for (int i = startIndex; i < input.Length; i++)
        {
            if (alphabet.Contains(char.ToLower(input[i])) is false)
            {
                return i;
            }
        }
        return input.Length;
    }
}