using MekLatexTranslationLibrary.Helpers;

namespace MekLatexTranslationLibrary.OperatorBuilders;

internal static partial class CasesBuilder
{
    class Cases
    {
        string _body = string.Empty;

        static readonly string[] _pieceRangeDefiners = new string[] { "{,}x", "{,}y", "{,}z" };
        

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
                bool isPieced = TryConvertIntoPiecedFunction();
                if (isPieced) ConvertBodyIntoPieced();
                IsPieced = isPieced;    // IsPieced must be defined after Body, because setting Body will set IsPieced null
            }
            string tag = (IsPieced ?? false) ? PiecedTag : NormalTag;
            return $"{TextBefore}{tag}({Body}){TextAfter}";
        }

        private void ConvertBodyIntoPieced()
        {
            string[] piecedBody = Body.Split(RowChangeTag);
            for (int i = 0; i < piecedBody.Length; i++)
            {
                int index = -1;
                foreach (var definer in _pieceRangeDefiners)
                {
                    index = Helper.GetSmallestValue(index, piecedBody[i].IndexOf(definer), 0);
                }
                if (index < 0)
                {
                    piecedBody[i] = $"{piecedBody[i]},";
                }
                else
                {
                    piecedBody[i] = piecedBody[i]
                        .Remove(index, 3)
                        .Insert(index, ",");
                }
            }
            Body = string.Join(RowChangeTag, piecedBody);
        }

        private bool TryConvertIntoPiecedFunction()
        {
            string[] piecedBody = Body.Split(RowChangeTag);
            for (int i = 0; i < piecedBody.Length; i++)
            {
                bool isPiecePieced = _pieceRangeDefiners.Any(x => piecedBody[i].Contains(x));
                if (isPiecePieced is false) return false;
            }
            return true;
        }
    }
}
