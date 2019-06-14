using Aran.Aim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOSSM.Report.Model
{
    public class AccuracyReportModel
    {
        public FeatureType FeatureType { get; set; }

        public Guid Identifier { get; set; }

        public int? SequenceNumber { get; set; }

        public int? CorrectionNumber { get; set; }

        public string BeginValidTime { get; set; }

        public string EndValidTime { get; set; }

        public string Description { get; set; }

        public string AccuracyPath { get; set; }

        public string AccuracyValue { get; set; }

        public string AccuracyMeasurement { get; set; }
    }
}
