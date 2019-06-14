using Aran.Aim;
using Aran.Temporality.Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOSSM.Report.Model
{
    public class ReportViewModel
    {
        public PublicSlot PublicSlot { get; set; }
        public PrivateSlot PrivateSlot { get; set; }
        public List<FeatureReportViewModel> Features { get; set; }

        public SortedDictionary<string, Tuple<int, int, int>> TotalCounts
        {
            get
            {
                var result = new SortedDictionary<string, Tuple<int, int, int>>();

                foreach (var featureType in Enum.GetNames(typeof(FeatureType)))
                {
                    if (Features.Any(x => x.FeatureType == featureType))
                    {
                        var currentElements = Features.Where(x => x.FeatureType == featureType);
                        result[featureType] = Tuple.Create(
                            currentElements.Count(x => x.Operation == SlotReportOperation.N),
                            currentElements.Count(x => x.Operation == SlotReportOperation.U),
                            currentElements.Count(x => x.Operation == SlotReportOperation.W));
                    }
                }

                return result;
            }
        }

        public int New => Features.Count(x => x.Operation == SlotReportOperation.N);
        public int Updated => Features.Count(x => x.Operation == SlotReportOperation.U);
        public int Withdrawn => Features.Count(x => x.Operation == SlotReportOperation.W);
    }


    public class FeatureReportViewModel
    {
        public string FeatureType { get; set; }
        public string UUID { get; set; }
        public SlotReportOperation Operation { get; set; }
        public Dictionary<string, Tuple<string, string>> Delta { get; set; }
        public string OldDescription { get; set; }
        public string NewDescription { get; set; }
    }

    public enum SlotReportOperation { N, U, W }
}
