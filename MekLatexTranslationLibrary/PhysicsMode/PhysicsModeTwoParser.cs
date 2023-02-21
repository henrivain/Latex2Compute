using MekLatexTranslationLibrary.Helpers;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MekLatexTranslationLibraryTests")]

namespace MekLatexTranslationLibrary.PhysicsMode;

internal class PhysicsModeTwoParser : IPhysicsModeParser
{
    internal PhysicsModeTwoParser(string latex)
    {
        Latex = latex;
    }

    record struct SymbolInfo(string Value, int Index, int LengthDiff);

    public string Translate()
    {
        var input = Latex;
        var replacements = GetReplacementIndices(ref input).ToArray();


        for (int i = replacements.Length - 1; i >= 0; i--)
        {
            var value = replacements[i];
            for (int j = 0; j < i; j++)
            {
                if (replacements[j].Index > value.Index)
                {
                    replacements[j].Index += value.LengthDiff;
                }
            }
            input = $"{input[..value.Index]}{value.Value}{input[value.Index..]}";
        }
        return input;
    }

    private static List<SymbolInfo> GetReplacementIndices(ref string input)
    {
        List<SymbolInfo> replacements = new();
        foreach (var symbol in Symbols)
        {
            int index = 0;
            while (true)
            {
                index = input.IndexOf(symbol.Key, index);
                if (index < 0)
                {
                    break;
                }
                input = input.Remove(index, symbol.Key.Length);
                bool addSeparator = CheckSeparatorNeed(input, symbol.Value, index);
                
                int lengthDiff = symbol.Value.Length - symbol.Key.Length;
                replacements.Add(new(addSeparator ? $"*{symbol.Value}" : symbol.Value, index, lengthDiff));
            }
        }
        return replacements;
    }

    private static bool CheckSeparatorNeed(in string input, in string replacement, int index)
    {
        if (replacement.Length < 1)
        {
            return false;
        }
        char? charBefore = Slicer.GetCharSafely(input, index - 1);
        if (charBefore is null)
        {
            return false;
        }
        return char.IsDigit(replacement[0]) && _operators.Contains(charBefore.Value) is false;
    }

    internal string Latex { get; }

    public TranslationError[]? Errors { get; private set; } = null;

    public string? Result { get; private set; } = null;

    static readonly char[] _operators = { '/', '*', '-', '+' };

    static ImmutableArray<KeyValuePair<string, string>> Symbols { get; }
        = ImmutableArray.Create(new Dictionary<string, string>()
        {
            // Rise to power does not matter, because it will be translated automatically
            ["nm"] = "_nm",
            ["mm"] = "_mm",
            ["cm"] = "_cm",
            ["dm"] = "_dm",
            ["m"] = "_m",
            ["dam"] = "10_m",
            ["hm"] = "10^(2)_m",
            ["km"] = "_km",

            ["ml"] = "_mm",
            ["cl"] = "10^(2)_l",
            ["dl"] = "10_l",
            ["l"] = "_km",

            ["W"] = "_W",
            ["kW"] = "_kW",
            ["Wh"] = "10^(-3)_kWh",
            ["kWh"] = "_kWh",
            ["MWh"] = "10^(3)_kWh",
            ["GWh"] = "10^(6)_kWh",
            ["TWh"] = "10^(9)_kWh",
            ["PWh"] = "10^(12)_kWh",

            ["kA"] = "_kA",
            ["A"] = "_A",
            ["mA"] = "_mA",

            ["kV"] = "_kV",
            ["V"] = "_V",
            ["mV"] = "_mV",

            ["mT"] = "10^(-3)_T",
            ["T"] = "_T",

            ["C"] = "_coul",
            ["G"] = "_Gs",

            ["F"] = "_F",
            ["nF"] = "_nF",
            ["pF"] = "_pF",
            ["Wb"] = "_Wb",

            ["N"] = "_N",
            ["kN"] = "10^(3)_N",
            ["MN"] = "10^(6)_N",
            ["GN"] = "10^(9)_N",

            ["mg"] = "_mg",
            ["g"] = "_gm",
            ["kg"] = "_kg",

            ["ns"] = "_ns",
            ["ms"] = "_ms",
            ["s"] = "_s",
            ["\\min"] = "_min",
            ["h"] = "_hr",

            ["mol"] = "_mol",
            ["mmol"] = "10^(-3)_mol",

            ["cd"] = "_cd",

            ["r"] = "",
            ["rad"] = "",
            ["(rad)"] = "",

            ["eV"] = "_eV",
            ["keV"] = "10^(3)_eV",
            ["MeV"] = "10^(6)_eV",
            ["GeV"] = "10^(9)_eV",
            ["TeV"] = "10^(12)_eV",

            ["J"] = "_J",
            ["kJ"] = "10^(3)_J",
            ["MJ"] = "10^(6)_J",
            ["GJ"] = "10^(9)_J",
            ["TJ"] = "10^(12)_J",


            ["K"] = "_°k",
            ["°C"] = "_°C",
            ["°F"] = "_°F",

            ["cal"] = "_cal",
            ["kcal"] = "_kcal",

            ["Hz"] = "_Hz",
            ["kHz"] = "_kHz",
            ["MHz"] = "_MHz",
            ["GHz"] = "_GHz",

        }.OrderByDescending(x => x.Key.Length)
            .ToArray());
}
