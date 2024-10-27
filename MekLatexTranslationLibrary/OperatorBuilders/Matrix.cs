﻿using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary.OperatorBuilders;
internal readonly ref struct Matrix
{
    private Matrix(
        ReadOnlySpan<char> before, 
        List<List<string>> body, 
        ReadOnlySpan<char> after,
        TranslationErrors errors,
        int longestRow)
    {
        Before = before;
        Body = body;
        After = after;
        Errors = errors;
        LongestRow = longestRow;
    }

    private ReadOnlySpan<char> Before { get; }
    private List<List<string>> Body { get; }
    private ReadOnlySpan<char> After { get; }
    private TranslationErrors Errors { get; }
    private int LongestRow { get; }

    internal static Matrix Parse(ReadOnlySpan<char> latex)
    {
        TranslationErrors errors = TranslationErrors.None;

        ReadOnlySpan<char> matrixStart =
            stackalloc char[] { '\\', 'b', 'e', 'g', 'i', 'n', '{', 'm', 'a', 't', 'r', 'i', 'x', '}' };
        ReadOnlySpan<char> matrixEnd =
            stackalloc char[] { '\\', 'e', 'n', 'd', '{', 'm', 'a', 't', 'r', 'i', 'x', '}' };
        ReadOnlySpan<char> rowSep =
            stackalloc char[] { '\\', '\\' };
        ReadOnlySpan<char> columnSep =
            stackalloc char[] { '&' };

        int startIndex = MemoryExtensions.IndexOf(latex, matrixStart);
        if (startIndex < 0)
        {
            return new Matrix (
                latex, 
                new(), 
                ReadOnlySpan<char>.Empty, 
                errors,
                0);
        }

        ReadOnlySpan<char> chars = Slicer.GetSpanSafely(latex, (startIndex + matrixStart.Length)..);
        int endIndex = BracketZeroMem.FindEnd(chars, matrixStart, matrixEnd);
        if (endIndex < 0)
        {
            endIndex = chars.Length;
            errors |= TranslationErrors.MissinMatrixEnd;
        }

        // Parse matrix pieces
        ReadOnlySpan<char> before = latex[..startIndex];
        ReadOnlySpan<char> body = chars[..endIndex];    // TODO: Build matrix body here
        ReadOnlySpan<char> after = Slicer.GetSpanSafely(chars, (endIndex + matrixEnd.Length)..);


        int index = 0;
        int longestRow = 0;
        List<List<string>> matrix = new();
        while (true)
        {
            int rowEnd = body[index..].IndexOf(rowSep) ;
            

            ReadOnlySpan<char> row = rowEnd < 0 ? 
                body[index..] : body[index..(rowEnd + index)];

            List<string> columns = ParseRow(row, columnSep);
            matrix.Add(columns);
            if (columns.Count > longestRow)
            {
                longestRow = columns.Count;
            }
            if (rowEnd < 0)
            {
                break;
            }

            index += rowEnd + rowSep.Length;
        }
        return new Matrix(before, matrix, after, errors, longestRow);
    }

    internal string Build()
    {
        return $"{Before}{BuildMatrixBody()}{After}";
    }

    private string BuildMatrixBody()
    {
        // Empty matrix
        if (Body.Count == 0)
        {
            return string.Empty;
        }

        // Horizontal 1 row matrix
        if (Body.Count == 1)
        {
            return $"[{string.Join(',', Body[0])}]";
        }

        // Vertical 1 col matrix
        if (Body.All(x => x.Count == 1))
        {
            return $"[{string.Join(',', Body.Select(x => x.First()))}]";
        }

        int longestRow = LongestRow;
        IEnumerable<string> rows = Body.Select(x => 
        {
            if (x.Count < longestRow)
            {
                x.AddRange(Enumerable.Repeat("", longestRow - x.Count));
            }
            return $"[{string.Join(',', x)}]";
        });
        return $"[{string.Join("", rows)}]";
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
