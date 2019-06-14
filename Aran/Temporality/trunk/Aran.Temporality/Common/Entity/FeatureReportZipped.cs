using System;
using Aran.Temporality.Internal.Interface;

namespace Aran.Temporality.Common.Entity
{ 
    [Serializable]
    public class FeatureReportZipped : INHibernateEntity
    {
        public FeatureReportZipped()
        {
            DateTime = DateTime.Now;
        }
        public virtual int Id { get; set; }
        public virtual DateTime DateTime { get; set; }
        public virtual string ReportType { get; set; }

        public virtual string FeatureGuid { get; set; }
        
        public virtual byte[] ReportData { get; set; }
    }
}
