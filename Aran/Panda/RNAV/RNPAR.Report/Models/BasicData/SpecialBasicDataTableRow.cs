using System;
using System.Collections.Generic;

namespace Aran.Panda.RNAV.RNPAR.Report.Models.BasicData
{
    public class SpecialBasicDataTableRow : AbstractBasicDataTableRow
    {
        public string Title { get; }

        public string FirstColumnTitle { get; }

        public string SecondColumnTitle { get; }

        public List<Tuple<string, string, string>> Rows { get; }

        public SpecialBasicDataTableRow(string title, string firstColumnTitle, string secondColumnTitle)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            FirstColumnTitle = firstColumnTitle ?? throw new ArgumentNullException(nameof(firstColumnTitle));
            SecondColumnTitle = secondColumnTitle ?? throw new ArgumentNullException(nameof(secondColumnTitle));

            Rows = new List<Tuple<string, string, string>>();
        }
    }
}
