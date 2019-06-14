using System.Collections.Generic;
using Aran.Panda.RNAV.RNPAR.Report.Models.Paragraph;

namespace Aran.Panda.RNAV.RNPAR.Report.Models.Conclusion
{
    public class ConclusionTable : AbstractSubParagraphContent
    {
        public string Object { get; set; }

        public string ReferenceDocumentation { get; set; }

        public string DocumentationVersion { get; set; }

        public string Conclusion { get; set; }

        public List<List<ConclusionTableRow>> RowGroups { get; }

        public ConclusionTable()
        {
            RowGroups = new List<List<ConclusionTableRow>>();
        }
    }
}
