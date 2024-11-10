using System.Text;

namespace MekLatexTranslationLibrary.Flexibility;

internal static class Builders
{
    internal static readonly Dictionary<TargetSystem, Func<string, List<List<Range>>, int, StringBuilder>> _matrixBuilders = new()
    {
        [TargetSystem.Ti] = static (latex, matrix, longestRow) =>
        {
            if (matrix.Count is 0)
            {
                return new();
            }

            StringBuilder b = new();

            if (matrix.Count is 1)
            {
                // Horizontal vector
                List<Range> columns = matrix.First();
                AddRow(ref b, columns, columns.Count);
                return b;
            }
            if (longestRow is 1)
            {
                // Vertical vector
                // Transpose to horizontal vector
                b.Append('[');
                for (int i = 0; i < matrix.Count; i++)
                {
                    if (i > 0)
                    {
                        b.Append(',');
                    }
                    b.Append(latex[matrix[i][0]]);
                }
                b.Append(']');
                return b;

            }

            // n * m matrix
            b.Append('[');
            foreach (List<Range> row in matrix)
            {
                AddRow(ref b, row, longestRow);
            }
            b.Append(']');
            return b;


            void AddRow(ref StringBuilder b, List<Range> columns, int requiredLen)
            {
                b.Append('[');
                b.AppendJoin(',', columns.Select(r => latex[r]));

                if (requiredLen > columns.Count)
                {
                    b.Append(',', requiredLen - columns.Count);
                }
                b.Append(']');
            }
        },

        [TargetSystem.Matlab] = static (latex, matrix, longestRow) =>
        {
            StringBuilder b = new();

            b.Append('[');
            for (int i = 0; i < matrix.Count; i++)
            {
                if (i > 0)
                {
                    b.Append(';');
                    b.Append("\n".PadRight(3));
                }
                List<Range> row = matrix[i];
                AddRawRow(ref b, row, longestRow);

            }
            b.Append(']');
            return b;

            void AddRawRow(ref StringBuilder b, List<Range> columns, int requiredLen)
            {
                b.AppendJoin(',', columns.Select(r => latex[r]));
                if (requiredLen > columns.Count)
                {
                    b.Append(',', requiredLen - columns.Count);
                }

            }
            
        }
    };






}