using MekLatexTranslationLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MekLatexTranslationLibrary;

static internal class StartEdit
{
    /// <summary>
    /// makes start changes, remove unnecessary chars and "\text" "\mathrm"
    /// </summary>
    /// <param name="item"></param>
    /// <returns>TranslationItem item with changes made</returns>
    internal static TranslationItem Run(TranslationItem item)
    {
        string inp = item.Latex;

        //Changes to LaTexInput
        inp = inp.Replace("\\ ", "");
        inp = inp.Replace(" ", "");
        inp = inp.Replace("\n", "");
        inp = inp.Replace("\r", "");

        // remove \text and \mathrm
        inp = RemovePattern.BracketsAfterLatex(inp, "\\text");
        inp = RemovePattern.BracketsAfterLatex(inp, "\\mathrm");

        inp = inp.Replace("m/s", "\\frac{m}{s}");

        item.Latex = inp;
        return item;
    }
}
