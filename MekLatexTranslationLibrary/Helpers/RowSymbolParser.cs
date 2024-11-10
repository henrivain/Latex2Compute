// Copyright Henri Vainio 2024

using System.Security;

namespace MekLatexTranslationLibrary.Helpers;
internal readonly struct RowSymbolParser
{
    private RowSymbolParser(
        Range before,
        Range body,
        Range after,
        TranslationErrors errors)
    {
        // If symbol exists in input
        Before = before;
        Body = body;
        After = after;
        Errors = errors;
        HasSymbol = true;
    }

    private RowSymbolParser(TranslationErrors errors)
    {
        // If symbol does not exist in input
        Before = 0..^0;
        Body = new Range();
        After = new Range();
        Errors = errors;
        HasSymbol = false;
    }

    internal Range Before { get; }
    internal Range Body { get; }
    internal Range After { get; }
    internal TranslationErrors Errors { get; }
    public bool HasSymbol { get; }

    internal static RowSymbolParser Parse(
        ReadOnlySpan<char> latex,
        ReadOnlySpan<char> symbolStart,
        ReadOnlySpan<char> symbolEnd,
        TranslationErrors endNotFoundError)
    {
        TranslationErrors errors = TranslationErrors.None;

        // Find start
        int startIndex = latex.IndexOf(symbolStart);
        if (startIndex < 0)
        {
            // No symbol in input
            // Take entire input
            return new RowSymbolParser(endNotFoundError);
        }
        
        int offset = startIndex + symbolStart.Length;

        // Find body and end
        ReadOnlySpan<char> input = latex.GetSpanSafely(offset..);
        int endIndex = BracketZeroMem.FindEnd(input, symbolStart, symbolEnd);
        if (endIndex < 0)
        {
            // End not found, use´input length as end
            endIndex = input.Length;
            errors |= endNotFoundError;
        }

        // Use offset to get correct ranges in latex instead of input

        Range before = ..startIndex;
        Range body = offset..(offset + endIndex);
        Range after = (offset + endIndex + symbolEnd.Length)..;

        return new RowSymbolParser(before, body, after, errors);
    }


    public void Deconstruct(
        out Range before, 
        out Range body, 
        out Range after, 
        out TranslationErrors errors,
        out bool hasSymbol)
    {
        before = Before;
        body = Body;
        after = After;
        errors = Errors;
        hasSymbol = HasSymbol;
    }




    


}
