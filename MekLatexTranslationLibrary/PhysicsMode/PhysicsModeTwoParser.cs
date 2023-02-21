using MekLatexTranslationLibrary.Helpers;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MekLatexTranslationLibraryTests")]

namespace MekLatexTranslationLibrary.PhysicsMode;

internal class PhysicsModeTwoParser : IPhysicsModeParser
{
    internal PhysicsModeTwoParser(string latex)
    {
        Latex = latex;
    }


    static ImmutableArray<KeyValuePair<string, string>> SortedSymbols { get; }
        = new(new Dictionary<string, string>()
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

                ["\\min"] = "_min",
            }.OrderByDescending(x => x.Key.Length).ToArray().ToArray();


    record struct SymbolInfo(string Value, int Index);

    public string Translate()
    {
        var input = Latex;
        var replacements = GetReplacementIndices(ref input)
            .OrderBy(x => x.Index)
            .ToArray();


        foreach (var replacement in replacements)
        {
            input = $"{input[..replacement.Index]}{replacement.Value}{input[replacement.Index..]}";
        }
        return input;
    }

    private List<SymbolInfo> GetReplacementIndices(ref string input)
    {
        List<SymbolInfo> replacements = new();
        foreach (var symbol in SortedSymbols)
        {
            int index = 0;
            while (true)
            {
                index = input.IndexOf(symbol.Key, index);
                if (index < 0)
                {
                    index = 0;
                    break;
                }
                bool hasPowerRise = char.IsDigit(Slicer.GetCharSafely(symbol.Value, 0) ?? '\0');
                bool lastCharOperator = char.IsLetter(Slicer.GetCharSafely(input, index - 1) ?? '\0');
                bool addStar = hasPowerRise && lastCharOperator is false;
                replacements.Add(new(addStar ? $"*{symbol.Value}" : symbol.Value, index + 1));
                input = input.Remove(index, symbol.Key.Length);
                replacements = ShiftInices(replacements, symbol, index);
            }
        }
        return replacements;
    }

    private static List<SymbolInfo> ShiftInices(List<SymbolInfo> replacements, KeyValuePair<string, string> symbol, int index)
    {
        int lengthDiff = symbol.Value.Length - symbol.Key.Length;
        if (lengthDiff != 0)
        {
            replacements = replacements.Select(x =>
            {
                if (x.Index > index)
                {
                    x.Index += lengthDiff;
                }
                return x;
            }).ToList();
        }

        return replacements;
    }

    internal string Latex { get; }

    public TranslationError[]? Errors { get; private set; } = null;

    public string? Result { get; private set; } = null;

}
