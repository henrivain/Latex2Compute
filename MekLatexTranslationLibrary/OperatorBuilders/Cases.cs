using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary.OperatorBuilders;

internal static partial class CasesBuilder
{
    class Cases
    {
        string _body = string.Empty;

        

        public string TextBefore { get; set; } = string.Empty;
        public string Body
        {
            get => _body;
            set
            {
                IsPieced = null;
                _body = value;
            }
        }
        public string TextAfter { get; set; } = string.Empty;
        private bool? IsPieced { get; set; } = null;


        public override string ToString()
        {
            if (IsPieced is null)
            {
                bool isPieced = CheckIfPieced();
                if (isPieced) ConvertBodyIntoPieced();
                IsPieced = isPieced;    // IsPieced must be defined after Body, because setting Body will set IsPieced null
            }
            string tag = (IsPieced ?? false) ? PiecedTag : NormalTag;
            
            return $"{TextBefore}{tag}({Body}){TextAfter}"
                .Replace("&", string.Empty);
        }

        const string _rangeDivider = "&{,}";

        private void ConvertBodyIntoPieced()
        {
            string[] piecedBody = Body.Split(RowChangeTag);
            for (int i = 0; i < piecedBody.Length; i++)
            {
                string line = piecedBody[i];
                int index = line.IndexOf(_rangeDivider);
                if (index < 0)
                {
                    throw new InvalidOperationException($"{nameof(line)} must have range divider '{_rangeDivider}'. Input was {line}");
                }
                piecedBody[i] = line
                    .Remove(index, 4)
                    .Insert(index, ",")
                    .Replace("or", "#124#")
                    .Replace("tai", "#124#");
            }
            Body = string.Join(RowChangeTag, piecedBody);
        }

        private bool CheckIfPieced()
        {
            string[] piecedBody = Body.Split(RowChangeTag);
            for (int i = 0; i < piecedBody.Length; i++)
            {
                // return false if one line does not contain range divider
                if (piecedBody[i].Contains(_rangeDivider) is false) return false;
            }
            return true;
        }
    }
}
