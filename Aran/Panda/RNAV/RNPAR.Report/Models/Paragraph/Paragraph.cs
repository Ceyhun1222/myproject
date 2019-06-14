using System;
using System.Collections.Generic;
using Aran.Panda.RNAV.RNPAR.Report.Models.BasicData;
using Aran.Panda.RNAV.RNPAR.Report.Models.Conclusion;

namespace Aran.Panda.RNAV.RNPAR.Report.Models.Paragraph
{
    public class Paragraph
    {
        public string Title { get; }

        public List<SubParagraph> SubParagraphs { get; }

        public Paragraph(string title)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));

            SubParagraphs = new List<SubParagraph>();
        }
    }
}