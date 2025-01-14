using System.Collections.ObjectModel;

namespace Latex2Compute.Structures;
internal static class OperatorMap
{








    internal static readonly ReadOnlyDictionary<TargetSystem, ReadOnlyDictionary<string, string>> Operators =
        new(new Dictionary<TargetSystem, ReadOnlyDictionary<string, string>>()
        {
            [TargetSystem.Ti] = new(new Dictionary<string, string>()
            {
                [ConstSymbol.Log] = "log",
                [ConstSymbol.NaturalLog] = "ln",
                [ConstSymbol.System] = "system",
                [ConstSymbol.SystemRowChange] = ",",
                [ConstSymbol.Piecewise] = "piecewise",
                [ConstSymbol.Or] = "or",
                [ConstSymbol.Log] = "log",
                [ConstSymbol.Log] = "log",
                [ConstSymbol.Log] = "log",
                [ConstSymbol.Log] = "log",
            }),

            [TargetSystem.Matlab] = new(new Dictionary<string, string>()
            {
                [ConstSymbol.Log] = "log",
                [ConstSymbol.NaturalLog] = "log",

            })
        });
}
