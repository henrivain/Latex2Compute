namespace Latex2Compute.StaticSymbols;
internal class SymbolParsers : StaticSymbolParser
{
    public SymbolParsers() 
        : base(SymbolCatalog.PhysicsTranslations, SymbolCatalog.MathOperators)
    {
    }
}
