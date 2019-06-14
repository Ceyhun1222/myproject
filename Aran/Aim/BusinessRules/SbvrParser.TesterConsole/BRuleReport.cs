using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SbvrParser.TesterConsole
{
    [DataContract]
    public class BRuleReport
    {
        public BRuleReport()
        {
            Features = new List<BRuleFeatureReport>();
        }

        [DataMember(Name = "UID")]
        public string Uid { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Profile { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public List<BRuleFeatureReport> Features { get; private set; }
    }

    [DataContract]
    public class BRuleFeatureReport
    {
        public BRuleFeatureReport()
        {
            Identifiers = new List<Guid>();
        }

        [DataMember]
        public string FeatureType { get; set; }

        [DataMember]
        public List<Guid> Identifiers { get; private set; }
    }

    [DataContract]
    public class BRuleReportList
    {
        public BRuleReportList()
        {
            Items = new List<BRuleReport>();
        }

        [DataMember]
        public List<BRuleReport> Items { get; private set; }
    }
}
