using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Panda.RNAV.RNPAR.Report.Models.Paragraph
{
    public class TextSubParagraphContent : AbstractSubParagraphContent
    {
        public string Text { get; }

        public TextSubParagraphContent(string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }
    }
}
