using System.Collections.Generic;
using Aran.Panda.RNAV.RNPAR.Report.Models.Paragraph;

namespace Aran.Panda.RNAV.RNPAR.Report.Models.BasicData
{
    public class BasicDataTable : AbstractSubParagraphContent
    {
        public string ConstantColumnTitle { get; set; } = "Constant";

        public string ValueColumnTitle { get; set; } = "Value";

        public List<AbstractBasicDataTableRow> Rows { get; } = new List<AbstractBasicDataTableRow>();
    }
}
