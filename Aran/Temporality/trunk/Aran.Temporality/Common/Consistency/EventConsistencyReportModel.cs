using Aran.Aim;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Temporality.Common.Consistency
{
    public class EventConsistencyReportModel
    {
        public EventConsistencyErrorType ErrorType { get; set; }

        public EventConsistency EventConsistency { get; set; }

        private const string _titles = "Category,Type,WorkPackage,FeatureType,Guid,Interpretation,SequenceNumber,CorrectionNumber,LifeTimeBegin,LifeTimeEnd,SubmitDate";
        public static string GetTitles()
        {
            return _titles;
        }

        public static List<EventConsistencyErrorType> WarningTypes { get; } = new List<EventConsistencyErrorType>
        {
            EventConsistencyErrorType.Duplicate,
        };

        public static List<EventConsistencyErrorType> ErrorTypes
        {
            get
            {
                return System.Enum.GetValues(typeof(EventConsistencyErrorType)).Cast<EventConsistencyErrorType>()
                    .Where(x => !WarningTypes.Contains(x)).ToList();
            }
        }

        public string Category => WarningTypes.Contains(ErrorType) ? "Warning" : "Error";

        public override string ToString()
        {
            var c = EventConsistency;
            return $"{Category},{ErrorType},{c.WorkPackage},{c.FeatureType},{c.Identifier},{c.Interpretation},{c.SequenceNumber},{c.CorrectionNumber},{c.ValidTimeBegin},{c.ValidTimeEnd},{c.SubmitDate}";
        }
    }
}
