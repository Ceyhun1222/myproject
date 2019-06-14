using Aran.Temporality.Common.Entity;

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IProblemReportStorage : ICrudStorage<ProblemReport>
    {
        bool UpdateProblemReport(ProblemReport report);
        ProblemReport GetProblemReport(int publicSlotId, int privateSlotId, int configId, ReportType reportType);
    }
}
