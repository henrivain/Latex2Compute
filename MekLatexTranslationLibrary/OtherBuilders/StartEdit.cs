using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary.OtherBuilders;

static internal class StartEdit
{
    /// <summary>
    /// makes start changes, remove unnecessary chars and "\text" "\mathrm"
    /// </summary>
    /// <param name="input"></param>
    /// <returns>input with some symbols removed or manipulated</returns>
    internal static string Run(string input)
    {
        //Changes to LaTexInput
        input = input
            .Replace("\\ ", "")
            .Replace(" ", "")
            .Replace("\n", "")
            .Replace("\r", "")
            .Replace("derivative", "#191#")
            .Replace("solve", "#192#");

        // remove \text and \mathrm
        input = RemovePattern.RemovePatternAndBrackets(input, "\\text");
        input = RemovePattern.RemovePatternAndBrackets(input, "\\mathrm");

        input = input
            .Replace("m/s", "\\frac{m}{s}")
            .Replace("C/mol", "\\frac{C}{mol}");

        if (input.StartsWith('+'))
        {
            input = input.Remove(0, 1);
        }

        return input;
    }
}
