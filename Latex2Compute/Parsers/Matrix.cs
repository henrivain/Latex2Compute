using MekLatexTranslationLibrary.Flexibility;

namespace MekLatexTranslationLibrary.Parsers;




internal readonly struct Matrix
{
    private Matrix(
        string latex,
        Range before,
        List<List<Range>> body,
        Range after,
        TranslationErrors errors,
        int longestRow)
    {
        Latex = latex;
        Before = before;
        Body = body;
        After = after;
        Errors = errors;
        LongestRow = longestRow;
    }
    private string Latex { get; }
    private Range Before { get; }
    private List<List<Range>> Body { get; }
    private Range After { get; }
    private TranslationErrors Errors { get; }
    private int LongestRow { get; }


    internal static Matrix Parse(in string latex)
    {
        ReadOnlySpan<char> matrixStart =
            stackalloc char[] { '\\', 'b', 'e', 'g', 'i', 'n', '{', 'm', 'a', 't', 'r', 'i', 'x', '}' };
        ReadOnlySpan<char> matrixEnd =
            stackalloc char[] { '\\', 'e', 'n', 'd', '{', 'm', 'a', 't', 'r', 'i', 'x', '}' };
        ReadOnlySpan<char> rowSep =
            stackalloc char[] { '\\', '\\' };
        ReadOnlySpan<char> columnSep =
            stackalloc char[] { '&' };

        RowSymbolParser parser = RowSymbolParser.Parse(
            latex, matrixStart, matrixEnd, TranslationErrors.MissingMatrixEnd);

        if (parser.HasSymbol is false)
        {
            return new Matrix(latex, before: 0..^0,
                body: new(), after: 0..0, parser.Errors, longestRow: 0);
        }

        // Parse matrix pieces
        var (beforeRange, bodyRange, afterRange, _, _) = parser;
        ReadOnlySpan<char> body = latex[bodyRange];    // TODO: Build matrix body here

        int longestRow = 0;
        List<List<Range>> matrixRanges = new();

        foreach (Range rowRange in latex.AsSpan().Split(rowSep, bodyRange))
        {
            List<Range> columns = latex.AsSpan().Split(columnSep, rowRange).ToList();
            if (longestRow < columns.Count)
            {
                longestRow = columns.Count;
            }
            matrixRanges.Add(columns);
        }

        List<List<string>> matrix = new();
        foreach (List<Range> row in matrixRanges)
        {
            List<string> columns = new();
            foreach (Range column in row)
            {
                columns.Add(latex[column].ToString());
            }
            matrix.Add(columns);
        }

        return new Matrix(latex, beforeRange, matrixRanges,
            afterRange, parser.Errors, longestRow);
    }


    /// <summary>
    /// Build all matrices in the input string.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="errors"></param>
    /// <returns>Input string with all matrices translated.</returns>
    internal static string BuildAll(string input, TranslationArgs args, ref TranslationErrors errors)
    {
        ReadOnlySpan<char> matrixStart =
            stackalloc char[] { '\\', 'b', 'e', 'g', 'i', 'n', '{', 'm', 'a', 't', 'r', 'i', 'x', '}' };

        while (true)
        {
            input = Parse(input).Build(args);
            if (input.AsSpan().Contains(matrixStart, StringComparison.Ordinal) is false)
            {
                return input;
            }
        }
    }

    internal string Build(TranslationArgs args)
    {
        return $"{Latex[Before]}{BuildMatrixBody(args)}{Latex[After]}";
    }

    private string BuildMatrixBody(TranslationArgs args)
    {
        if (Builders._matrixBuilders.ContainsKey(args.TargetSystem) is false)
        {
            throw new NotSupportedException($"Invalid math system target {args.TargetSystem}",
                new KeyNotFoundException(args.TargetSystem.ToString()));
        }

        var builder = Builders._matrixBuilders[args.TargetSystem];
        return builder(Latex, Body, LongestRow).ToString();
    }

    private static List<string> ParseRow(ReadOnlySpan<char> row, ReadOnlySpan<char> separator)
    {
        List<string> result = new();
        int index = 0;
        while (true)
        {
            int columnEnd = row.GetSpanSafely(index..).IndexOf(separator);
            if (columnEnd < 0)
            {
                result.Add(row[index..].ToString());
                break;
            }

            result.Add(row[index..(index + columnEnd)].ToString());
            index += columnEnd + separator.Length;
        }
        return result;
    }
}
