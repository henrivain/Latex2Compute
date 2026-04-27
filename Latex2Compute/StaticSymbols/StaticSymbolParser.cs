namespace Latex2Compute.StaticSymbols;

internal class StaticSymbolParser 
{
    readonly SymbolValidator _validator;
    readonly char[] _operators;

    public StaticSymbolParser(Dictionary<string, string> symbolCatalog, char[] operators)
    {
        _validator = new SymbolValidator(symbolCatalog);
        _operators = operators;
    }

    public virtual string Translate(string latex)
    {
        if (_validator.IsEmpty())
        {
            return latex;
        }

        int lastValidIndex = -1;
        string symbolKey = string.Empty;
        for (int index = 0; index < latex.Length; index++)
        {
            string currentKey;
            if (lastValidIndex is -1)
            {
                currentKey = latex[index].ToString();
                if (_validator.IsPossibleStart(currentKey) is false)
                {
                    continue;
                }
                lastValidIndex = index;
            }
            else
            {
                currentKey = latex[lastValidIndex..(index + 1)];
                if (_validator.IsPossibleStart(currentKey) is false)
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
            bool isSymbol = _validator.IsFullMatch(currentKey);
            if (isSymbol)
            {
                symbolKey = currentKey;
            }
        }
        return Build(latex, symbolKey, lastValidIndex, out _);
    }

    public void OverrideSymbols(Dictionary<string, string> overrides)
    {
        SymbolValidator validator = GetValidator();
        foreach (var d in overrides)
        {
            validator.AddOrOverrideSymbol(d.Key, d.Value);
        }
    }

    protected SymbolValidator GetValidator()
    {
        return _validator;
    }

    private void Reset(ref string symbolKey, ref int lastValidIndex, ref int index)
    {
        index = string.IsNullOrEmpty(symbolKey) 
            ? lastValidIndex 
            : lastValidIndex + _validator.GetSubstitute(symbolKey).Length - 1;

        symbolKey = string.Empty;
        lastValidIndex = -1;
    }

    private string Build(string latex, string symbolKey, int startIndex, out bool addedSeparator)
    {
        addedSeparator = false;
        if (string.IsNullOrEmpty(symbolKey) ||
            _validator.IsFullMatch(symbolKey) is false)
        {
            return latex;
        }
        string replacement = _validator.GetSubstitute(symbolKey);
        if (IsSeparatorNeeded(latex, startIndex, replacement))
        {
            replacement = $"*{replacement}";
            addedSeparator = true;
        }
        return $"{latex[..startIndex]}{replacement}{latex[(startIndex + symbolKey.Length)..]}";
    }

    /// <summary>
    /// Check if separator '*' is needed. Check if replacement[0] is digit and 
    /// char before replacement in latex is not operator.
    /// </summary>
    /// <param name="latex"></param>
    /// <param name="replacementIndex"></param>
    /// <param name="replacement"></param>
    /// <returns>True if separator '*' is needed, othewise false.</returns>
    private bool IsSeparatorNeeded(ReadOnlySpan<char> latex, int replacementIndex, ReadOnlySpan<char> replacement)
    {
        char? charBefore = Slicer.GetCharSafely(latex, replacementIndex - 1);
        if (charBefore is null)
        {
            return false;
        }

        char? firstOfReplacement = Slicer.GetCharSafely(replacement, 0);
        if (firstOfReplacement is null)
        {
            return false;
        }

        bool hasTenPower = char.IsDigit(firstOfReplacement.Value);
        bool isOperator = _operators.Contains(charBefore.Value);
        return hasTenPower && isOperator is false;
    }

}
