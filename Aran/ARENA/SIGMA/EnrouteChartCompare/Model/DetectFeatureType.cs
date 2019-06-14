using System.Collections.Generic;
using ChartCompare;
using PDM;

namespace EnrouteChartCompare.Model
{
    public class DetectFeatureType
    {
        public DetectFeatureType(string featName, List<DetailedItem> featList, PDM_ENUM featType)
        {
            FeatName = featName;
            FeatureList = featList;
            FeatureType = featType;
        }

        public string FeatName { get; set; }
        public bool IsSelected { get; set; }
        public List<DetailedItem> FeatureList { get; set; }
        public PDM_ENUM FeatureType { get; set; }
    }
}