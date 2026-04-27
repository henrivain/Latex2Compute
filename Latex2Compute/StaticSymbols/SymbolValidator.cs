namespace Latex2Compute.StaticSymbols;

internal class SymbolValidator 
{
    readonly Dictionary<string, string> _symbolCatalog;

    readonly HashSet<string> _characterCombinations;

    public SymbolValidator(Dictionary<string, string> symbolCatalog)
    {
        _symbolCatalog = symbolCatalog;
        _characterCombinations = SymbolKeysToHashSet(symbolCatalog);
    }

    internal bool IsPossibleStart(string key)
    {
        return _characterCombinations.Contains(key);
    }

    internal bool IsFullMatch(string key)
    {
        return _symbolCatalog.ContainsKey(key);
    }

    internal string GetSubstitute(string key)
    {
        return _symbolCatalog[key];
    }

    internal bool AddOrOverrideSymbol(string key, string value)
    {
        if (_symbolCatalog.ContainsKey(key))
        {
            _symbolCatalog[key] = value;
            return false;
        }
        _symbolCatalog.Add(key, value);

        string span = string.Empty;
        foreach (var c in key)
        {
            span += c;
            _characterCombinations.Add(span);
        }
        return true;
    }

    internal bool AddIfNotExists(string key, string value)
    {
        if (_symbolCatalog.ContainsKey(key))
        {
            return false;
        }
        return AddOrOverrideSymbol(key, value);
    }

    /// <summary>
    /// Get hash set containing all possible combinations of characters 
    /// that can lead to complete Symbols dictionary symbol key.
    /// </summary>
    /// <returns>Hashset of substrings from Symbols dictionary</returns>
    private static HashSet<string> SymbolKeysToHashSet(Dictionary<string, string> symbolCatalog)
    {
        // Hashset containing all possible combinations that can lead to complete symbol key
        HashSet<string> result = [];
        foreach (var symbol in symbolCatalog)
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
