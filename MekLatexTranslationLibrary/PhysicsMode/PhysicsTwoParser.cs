using MekLatexTranslationLibrary.Helpers;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace MekLatexTranslationLibrary.PhysicsMode;
internal sealed class PhysicsTwoParser : IPhysicsModeParser
{
    public PhysicsTwoParser(string latex)
    {
        Latex = latex;
    }
    private string Latex { get; }

    public TranslationError[]? Errors => null;

    public string? Result { get; private set; }


    public string Translate()
    {
        var latex = Latex;
        int lastValidIndex = -1;
        string symbolKey = string.Empty;
        for (int index = 0; index < latex.Length; index++)
        {
            string currentKey;
            if (lastValidIndex is -1)
            {
                currentKey = latex[index].ToString();
                if (CharacterCombinations.Contains(currentKey) is false)
                {
                    continue;
                }
                lastValidIndex = index;
            }
            else
            {
                currentKey = latex[lastValidIndex..(index + 1)];
                if (CharacterCombinations.Contains(currentKey) is false)
                {
                    latex = Build(latex, symbolKey, lastValidIndex, out bool isSeparatorAdded);
                    Reset(ref symbolKey, ref lastValidIndex, ref index);
                    if (isSeparatorAdded)
                    {
                        index++;
                    }
                    continue;
                }
            }
            bool isSymbol = Symbols.ContainsKey(currentKey);
            if (isSymbol)
            {
                symbolKey = currentKey;
            }
        }
        latex = Build(latex, symbolKey, lastValidIndex, out _);
        Result = latex;
        return latex;
    }

    private static void Reset(ref string symbolKey, ref int lastValidIndex, ref int index)
    {
        if (string.IsNullOrEmpty(symbolKey))
        {
            index = lastValidIndex;
        }
        else
        {
            index = lastValidIndex + Symbols[symbolKey].Length - 1;
        }
        symbolKey = string.Empty;
        lastValidIndex = -1;
    }
    private static string Build(string latex, string symbolKey, int startIndex, out bool addedSeparator)
    {
        addedSeparator = false;
        if (string.IsNullOrEmpty(symbolKey) || 
            Symbols.ContainsKey(symbolKey) is false)
        {
            return latex;
        }
        string replacement = Symbols[symbolKey];
        if (IsSeparatorNeeded(latex, startIndex, replacement))
        {
            replacement = $"*{replacement}";
            addedSeparator = true;
        }
        return $"{latex[..startIndex]}{replacement}{latex[(startIndex + symbolKey.Length)..]}";
    }


    static ReadOnlyDictionary<string, string> Symbols { get; } =
        new(new Dictionary<string, string>()
        {
            // Rise to power does not matter, because it is translated automatically
            
            // constants
            ["c"] = "_c",   // light speed
            ["u"] = "_u",   // atom mass unit

            // TODO: Add "prefer constants over units"- setting
            // ["h"] = "_h",   // planck constant   (already hour)
            // ["g"] = "_g",   // gravitational acceleration    (already gram)
            
            ["pm"] = "10^(-12)_m",
            ["nm"] = "_nm",
            ["mm"] = "_mm",
            ["cm"] = "_cm",
            ["dm"] = "_dm",
            ["m"] = "_m",
            ["dam"] = "10_m",
            ["hm"] = "10^(2)_m",
            ["km"] = "_km",
            ["pc"] = "_pc",
            ["kpc"] = "10^(3)_pc",
            ["ly"] = "_ltyr",

            ["ml"] = "_ml",
            ["cl"] = "10^(2)_l",
            ["dl"] = "10_l",
            ["l"] = "_l",

            ["mW"] = "10^(-3)_W",
            ["W"] = "_W",
            ["kW"] = "_kW",
            ["MW"] = "10^(6)_W",
            ["GW"] = "10^(9)_W",
            ["TW"] = "10^(12)_W",
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

            ["nC"] = "10^(-9)_coul",
            ["\\muC"] = "10^(-6)_coul",
            ["mC"] = "10^(-3)_coul",
            ["C"] = "_coul",
            ["G"] = "_Gs",

            ["F"] = "_F",
            ["nF"] = "_nF",
            ["pF"] = "_pF",
            ["Wb"] = "_Wb",

            ["mN"] = "10^(-3)_N",
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
            ["d"] = "_day",
            ["a"] = "_yr",

            ["mol"] = "_mol",
            ["mmol"] = "10^(-3)_mol",

            ["cd"] = "_cd",

            ["r"] = "",
            ["rad"] = "",
            ["(rad)"] = "",
            ["\\degree"] = "",
            ["°"] = "",

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


            ["K"] = "_°K",
            ["°C"] = "_°C",
            ["°F"] = "_°F",

            ["cal"] = "_cal",
            ["kcal"] = "_kcal",

            ["Hz"] = "_Hz",
            ["kHz"] = "_kHz",
            ["MHz"] = "_MHz",
            ["GHz"] = "_GHz",


            ["mbar"] = "_mbar",
            ["bar"] = "_bar",
            ["kbar"] = "10^(3)_bar",
            ["Pa"] = "_Pa",
            ["kPa"] = "_kPa",
            ["MPa"] = "10^(6)_Pa",
            ["GPa"] = "10^(9)_Pa",

            [@"m\Omega"] = "10^(-3)_Ω",
            [@"\Omega"] = "_ohm",
            [@"k\Omega"] = "_kΩ",
            [@"M\Omega"] = "_MΩ",

            ["Bq"] = "(1/_s)",
            ["kBq"] = "10^(3)*(1/_s)",
            ["MBq"] = "10^(6)*(1/_s)",
            ["GBq"] = "10^(9)*(1/_s)",
        });

    static HashSet<string> CharacterCombinations { get; } = SymbolKeysToHashSet();
    
    static readonly char[] _operators = { '/', '*', '-', '+', '(', ')', '[', ']', '{', '}' };

    /// <summary>
    /// Check if separator '*' is needed. Check if replacement[0] is digit and 
    /// char before replacement in latex is not operator.
    /// </summary>
    /// <param name="latex"></param>
    /// <param name="replacementIndex"></param>
    /// <param name="replacement"></param>
    /// <returns>True if separator '*' is needed, othewise false.</returns>
    private static bool IsSeparatorNeeded(in string latex, int replacementIndex, in string replacement)
    {
        char? charBefore = Slicer.GetCharSafely(in latex, replacementIndex - 1);
        char? firstOfReplacement = Slicer.GetCharSafely(in replacement, 0);
        if (charBefore is null)
        {
            return false;
        }
        if (firstOfReplacement is null)
        {
            return false;
        }

        bool hasTenPower = char.IsDigit(firstOfReplacement.Value);
        bool isOperator = _operators.Contains(charBefore.Value);
        return hasTenPower && isOperator is false;
    }

    /// <summary>
    /// Get hash set containing all possible combinations of characters 
    /// that can lead to complete Symbols dictionary symbol key.
    /// </summary>
    /// <returns>Hashset of substrings from Symbols dictionary</returns>
    private static HashSet<string> SymbolKeysToHashSet()
    {
        // Hashset containing all possible combinations that can lead to complete symbol key
        HashSet<string> result = new();
        foreach (var symbol in Symbols)
        {
            string charsBefore = string.Empty;
            foreach (var c in symbol.Key)
            {
                charsBefore += c;
                result.Add(charsBefore);
            }
        }
        return result;
    }
}
