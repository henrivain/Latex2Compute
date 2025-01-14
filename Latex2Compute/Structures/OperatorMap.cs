using System.Collections.ObjectModel;

namespace Latex2Compute.Structures;
internal static class OperatorMap
{
    static readonly Dictionary<string, string> _operatorsTi = new()
    {
        [ConstSymbol.Log] = "log",
        [ConstSymbol.NaturalLog] = "ln",
        [ConstSymbol.System] = "system",
        [ConstSymbol.SystemRowChange] = ",",
        [ConstSymbol.Piecewise] = "piecewise",
        [ConstSymbol.Or] = " or ",
        [ConstSymbol.Arcsin] = "arcsin",
        [ConstSymbol.Arccos] = "arccos",
        [ConstSymbol.Arctan] = "arctan",
        [ConstSymbol.Sin] = "sin",
        [ConstSymbol.Cos] = "cos",
        [ConstSymbol.Tan] = "tan",
        [ConstSymbol.Sqrt] = "sqrt",
        [ConstSymbol.Root] = "root",
        [ConstSymbol.Limit] = "lim",
        [ConstSymbol.Pi] = "pi",
        [ConstSymbol.Abs] = "abs",
        [ConstSymbol.Integral] = "∫",
        [ConstSymbol.Derivative] = "derivative",
        [ConstSymbol.Solve] = "solve",
        [ConstSymbol.Npr] = "nPr",
        [ConstSymbol.Binom] = "nCr",
    };

    static readonly Dictionary<string, string> _operatorsMatlab = new()
    {
        [ConstSymbol.NaturalLog] = "log",
        [ConstSymbol.Arcsin] = "asin",
        [ConstSymbol.Arccos] = "acos",
        [ConstSymbol.Arctan] = "atan",
        [ConstSymbol.Root] = "nthroot",
        [ConstSymbol.Integral] = "int",
        [ConstSymbol.Derivative] = "diff",
    };


    static readonly Dictionary<Symbol, string> _symbolsTi = new()
    {
        [Symbol.ImaginaryUnit] = "@i",
    };

    static readonly Dictionary<Symbol, string> _symbolsMatlab = new()
    {
        [Symbol.ImaginaryUnit] = "i",
    };

    internal enum Symbol
    {
        ImaginaryUnit,
    }


    internal static readonly ReadOnlyDictionary<TargetSystem, ReadOnlyDictionary<string, string>> Operators =
        new(new Dictionary<TargetSystem, ReadOnlyDictionary<string, string>>()
        {
            // Default contains all possible operators (keys)
            [TargetSystem.Default] = _operatorsTi.AsReadOnly(), 
            [TargetSystem.Ti] = _operatorsTi.AsReadOnly(), 
            [TargetSystem.Matlab] = _operatorsMatlab.AsReadOnly(), 
        });

    internal static readonly ReadOnlyDictionary<TargetSystem, ReadOnlyDictionary<Symbol, string>> Symbols =
        new(new Dictionary<TargetSystem, ReadOnlyDictionary<Symbol, string>>()
        {
            [TargetSystem.Default] = _symbolsTi.AsReadOnly(),
            [TargetSystem.Ti] = _symbolsTi.AsReadOnly(),
            [TargetSystem.Matlab] = _symbolsMatlab.AsReadOnly(),
        });

    internal static string GetOperatorOrDefault(string key, TargetSystem target = TargetSystem.Default)
    {
        // Get value for target, if not found, get default
        if (Operators.TryGetValue(target, out var dict))
        {
            if (dict.TryGetValue(key, out var value))
            {
                return value;
            }
        }
        if (Operators[TargetSystem.Default].TryGetValue(key, out var defaultValue))
        {
            return defaultValue;
        }
        throw new NotImplementedException($"Operator {key} not found (for target {target})");
    }
    internal static string GetSymbolOrDefault(Symbol symbol, TargetSystem target = TargetSystem.Default)
    {
        if (Symbols.TryGetValue(target, out var dict))
        {
            if (dict.TryGetValue(symbol, out var value))
            {
                return value;
            }
        }
        if (Symbols[TargetSystem.Default].TryGetValue(symbol, out var defaultValue))
        {
            return defaultValue;
        }
        throw new NotImplementedException($"Symbol {symbol} not found (for target {target})");
    }

    internal static IEnumerable<string> GetAllOperatorKeys()
    {
        return Operators[TargetSystem.Default].Keys;
    }

}
