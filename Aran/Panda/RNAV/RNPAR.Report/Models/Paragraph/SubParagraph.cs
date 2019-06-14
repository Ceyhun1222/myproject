using System;
using System.Collections.Generic;

namespace Aran.Panda.RNAV.RNPAR.Report.Models.Paragraph
{
    public class SubParagraph
    {
        public string Title { get; }

        public List<AbstractSubParagraphContent> Contents { get; }

        public SubParagraph(string title)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));

            Contents = new List<AbstractSubParagraphContent>();
        }
    }
}