namespace Latex2Compute.Helpers;
readonly struct SimpleParser
{
    readonly ReadOnlyMemory<char> _input;

    public SimpleParser(ReadOnlyMemory<char> input)
    {
        _input = input;
    }

    public Categorization SeparateScopes(
        ReadOnlySpan<char> header, 
        ReadOnlySpan<char> opening, 
        ReadOnlySpan<char> closing,
        Errors noEnd = 0)
    {
        int start = _input.Span.IndexOf(header);
        if (start < 0)
        {
            return new Categorization { Before = Range.All, Found = false };
        }

        // Has start
        int contentStart = start + header.Length + 1;
        int end = BracketZeroMem.FindEnd(_input.Span, opening, closing, contentStart);
        if (end < 0)
        {
            return new Categorization
            {
                Before = ..start,
                Content = contentStart..,
                After = ^0..,
                Errors = noEnd,
                Found = true
            };
        }

        return new Categorization
        {
            Before = ..start,
            Content = contentStart..end,
            After = (end + closing.Length)..,
            Found = true
        };
    }

    public Categorization Continue(
        Categorization before, 
        ReadOnlySpan<char> opening, 
        ReadOnlySpan<char> closing,
        Errors noStart = 0,
        Errors noEnd = 0)
    {
        if (before.Found is false)
        {
            throw new ArgumentException("", nameof(before));
        }

        // Validate opening bracket exists
        int start = before.After.Start.Value;
        if (_input.Span[start..].StartsWith(opening) is false)
        {
            // No start bracket
            return new Categorization
            {
                Content = before.After,
                Found = false,
                Errors = noStart
            };
        }

        // Move cursor over opening bracket
        start += opening.Length;

        int end = BracketZeroMem.FindEnd(_input.Span[start..], opening, closing);
        if (end < 0)
        {
            // No end bracket
            return new Categorization
            {
                Content = before.After,
                After = ^0..,
                Found = true,
                Errors = noEnd,
            };
        }

        int contentEnd = start + end;
        return new Categorization
        {
            Content = start..(contentEnd),
            After = (contentEnd + closing.Length)..,
            Found = true,
        };
    }


    internal readonly struct Categorization
    {
        public Categorization()
        {
        }
        public required bool Found { get; init; }
        public Range Before { get; init; } = new Range();
        public Range Content { get; init; } = new Range();
        public Range After { get; init; } = new Range();
        public Errors Errors { get; init; }
    }
}