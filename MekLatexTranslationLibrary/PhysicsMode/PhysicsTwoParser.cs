﻿using MekLatexTranslationLibrary.OperatorBuilders;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MekLatexTranslationLibrary.PhysicsMode;
internal class PhysicsTwoParser : IPhysicsModeParser
{
    public PhysicsTwoParser(string latex)
    {
        Latex = latex;
    }
    private string Latex { get; }

    public TranslationError[]? Errors => null;

    public string? Result => null;



    public string Translate()
    {
        var latex = Latex;
        int lastValidIndex = -1;
        string symbolKey = string.Empty;
        for (int index = 0; index < latex.Length; index++)
        {
            string current = latex[index].ToString();

            if (lastValidIndex is -1)
            {
                bool isValidChar = CharacterCombinations.Contains(current);
                if (isValidChar)
                {
                    lastValidIndex = index;
                }
                else
                {
                    continue;
                }
            }
            else
            {

            }

            var currentKey = latex[lastValidIndex..(index+1)];
            bool isSymbol = Symbols.ContainsKey(currentKey);
            if (isSymbol)
            {
                symbolKey = currentKey;
                continue;
            }
            bool isValidSpan = CharacterCombinations.Contains(currentKey);
            if (index >= latex.Length - 1 || isValidSpan is false)
            {
                latex = Build(latex, symbolKey, lastValidIndex);
                Reset(ref symbolKey, ref lastValidIndex, ref index);
                continue;
            }
        }
        return latex;
    }

    private static void Reset(ref string symbolKey, ref int lastValidIndex, ref int index)
    {
        if (string.IsNullOrEmpty(symbolKey) is false)
        {
            index = lastValidIndex + Symbols[symbolKey].Length;
        }
        symbolKey = string.Empty;
        lastValidIndex = -1;
    }

    private static string Build(string latex, string symbolKey, int startIndex)
    {
        if (string.IsNullOrEmpty(symbolKey))
        {
            return latex;
        }
        return $"{latex[..startIndex]}{Symbols[symbolKey]}{latex[(startIndex + symbolKey.Length)..]}";
    }



    static ReadOnlyDictionary<string, string> Symbols { get; } = new(new Dictionary<string, string>()
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

    });

    static HashSet<string> CharacterCombinations { get; } = SymbolKeysToHashSet();

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