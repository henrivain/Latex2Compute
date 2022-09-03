using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary;

static internal class StartEdit
{
    /// <summary>
    /// makes start changes, remove unnecessary chars and "\text" "\mathrm"
    /// </summary>
    /// <param name="item"></param>
    /// <returns>TranslationItem item with changes made</returns>
    internal static string Run(string input)
    {
  

        //Changes to LaTexInput
        input = input.Replace("\\ ", "");
        input = input.Replace(" ", "");
        input = input.Replace("\n", "");
        input = input.Replace("\r", "");

        // remove \text and \mathrm
        input = RemovePattern.BracketsAfterLatex(input, "\\text");
        input = RemovePattern.BracketsAfterLatex(input, "\\mathrm");

        input = input.Replace("m/s", "\\frac{m}{s}");

        return input;
    }
}
