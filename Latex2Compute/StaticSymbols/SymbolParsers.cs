namespace Latex2Compute.StaticSymbols;

using static ParserHelpers;

internal class PhysicsParser : StaticSymbolParser
{
    public PhysicsParser() 
        : base(SymbolCatalog.PhysicsTranslations, SymbolCatalog.MathOperators)
    {
    }
}

internal class PhycicsEraserParser : StaticSymbolParser
{
    public PhycicsEraserParser()
        : base(GetSymbols(), [])
    {
    }
    
    private static Dictionary<string, string> GetSymbols()
    {
        // This should no be cached as same dictionary reference is used 
        // internally, so if you add to it, it will affect all instances
        return MergeDictionaries(
                MakeEraseable(SymbolCatalog.PhysicsEraseable),
                MakeEraseable(SymbolCatalog.GreekSymbols));
    }
}

internal class MathModeParser : StaticSymbolParser
{
    public MathModeParser()
       : base(GetGreekSymbols(), [])
    {
    }

    private static Dictionary<string, string> GetGreekSymbols()
    {
        return MakeEraseable(SymbolCatalog.GreekSymbols);
    }
}

internal class GeometryLetterParser : StaticSymbolParser
{
    public GeometryLetterParser()
        : base(SymbolCatalog.GeometryGreekLetters, [])
    {
    }
}

internal class CommonSymbolParser : StaticSymbolParser
{
    public CommonSymbolParser()
        : base(GetTarget(), [])
    {
    }

    static Dictionary<string, string> GetTarget()
    {
        return ParserHelpers.MergeDictionaries(
            SymbolCatalog.CommonSymbols,
            SymbolCatalog.ComparisonOperators,
            SymbolCatalog.Brackets,
            MakeEraseable(SymbolCatalog.CommonRemovableSymbols)
        );
    }
}

file static class ParserHelpers
{
    internal static Dictionary<string, string> MergeDictionaries(params Dictionary<string, string>[] dictionaries)
    {
        return dictionaries
            .SelectMany(x => x)
            .ToDictionary();
    }

    internal static Dictionary<string, string> MakeEraseable(params string[] symbols)
    {
        return symbols.ToDictionary(x => x, x => "");
    }
}
