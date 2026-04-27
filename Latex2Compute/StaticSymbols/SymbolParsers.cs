namespace Latex2Compute.StaticSymbols;

internal class PhysicsParser : StaticSymbolParser
{
    public PhysicsParser() 
        : base(SymbolCatalog.PhysicsTranslations, SymbolCatalog.MathOperators)
    {
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
            SymbolCatalog.Brackets
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
}
