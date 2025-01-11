namespace Latex2Compute.Parsers;

internal static partial class CasesParser
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
                if (isPieced)
                {
                    ConvertBodyIntoPieced();
                }

                IsPieced = isPieced;    // IsPieced must be defined after Body, because setting Body will set IsPieced null
            }
            string tag = (IsPieced ?? false) ? 
                ConstSymbol.Piecewise : ConstSymbol.System;
            
            return $"{TextBefore}{tag}({Body}){TextAfter}"
                .Replace("&", string.Empty);
        }

        const string RangeDivider = "&{,}";

        private void ConvertBodyIntoPieced()
        {
            string[] piecedBody = Body.Split(ConstSymbol.SystemRowChange);
            for (int i = 0; i < piecedBody.Length; i++)
            {
                string line = piecedBody[i];
                int index = line.IndexOf(RangeDivider);
                if (index < 0)
                {
                    throw new InvalidOperationException($"{nameof(line)} must have range divider '{RangeDivider}'. Input was {line}");
                }
                piecedBody[i] = line
                    .Remove(index, 4)
                    .Insert(index, ",")
                    .Replace("or", ConstSymbol.Or)
                    .Replace("tai", ConstSymbol.Or);
            }
            Body = string.Join(ConstSymbol.SystemRowChange, piecedBody);
        }

        private bool CheckIfPieced()
        {
            string[] piecedBody = Body.Split(ConstSymbol.SystemRowChange);
            for (int i = 0; i < piecedBody.Length; i++)
            {
                // return false if one line does not contain range divider
                if (piecedBody[i].Contains(RangeDivider) is false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
