namespace Aran.Panda.RNAV.RNPAR.Report.Models.Conclusion
{
    public class ConclusionTableRow
    {
        public string Description { get; set; }

        public ConclusionType ConclusionType { get; set; } = ConclusionType.Empty;

        public string Comment { get; set; }
    }
}