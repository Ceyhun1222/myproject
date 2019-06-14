using System;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{
    public enum ReportType
    {
        BusinessRulesReport = 1,
        FeatureDependencyReport = 2,
		MissingLinkReport = 3,
        SyntaxReport = 4,
        PublishingError =5 
    }

    [Serializable]
    public class ProblemReport : INHibernateEntity
    {
        public ProblemReport()
        {
            DateTime = DateTime.Now;
        }
        public virtual int Id { get; set; }
        public virtual DateTime DateTime { get; set; }
        public virtual int ReportType { get; set; }
        public virtual int ConfigId { get; set; }
        public virtual int PublicSlotId { get; set; }
        public virtual int PrivateSlotId { get; set; }
        public virtual int ErrorsFound { get; set; }
        public virtual int FeaturesProcessed { get; set; }
        public virtual byte[] ReportData { get; set; }
    }
}
