using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChartServices.DataContract
{
    public class ChartWithReference : Chart
    {
        public virtual IList<string> FeatureIdList { get; set; }
    }
}