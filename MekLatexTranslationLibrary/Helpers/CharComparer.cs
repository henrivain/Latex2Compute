namespace MekLatexTranslationLibrary.Helpers;

internal static class CharComparer
{
    internal static readonly char[] LowerLetters = Enumerable.Range('a', 26).Select(x => (char)x).ToArray(); 

    /// <summary>
    /// Get index of first char that is not a letter
    /// </summary>
    /// <param name="input"></param>
    /// <param name="startIndex"></param>
    /// <returns>index of first not letter char, returns length of input if every one is a letter</returns>
    internal static int GetIndexOfFirstNonChar(ReadOnlySpan<char> input, int startIndex = 0)
    {
        for (int i = startIndex; i < input.Length; i++)
        {
            if (LowerLetters.Contains(char.ToLower(input[i])) is false) return i;
        }
        return input.Length;
    }
}