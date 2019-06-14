using Aerodrome.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Stasy.SyncProvider
{
    public class TypeRelationInfo
    {
        public TypeRelationInfo()
        {
            Relations = new List<PropertyRelationInfo>();
        }
        public AM_AbstractFeature RootFeature { get; set; }

        public List<PropertyRelationInfo> Relations { get; set; }
    }
}
