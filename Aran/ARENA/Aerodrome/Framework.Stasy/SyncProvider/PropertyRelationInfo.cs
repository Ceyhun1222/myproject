using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Stasy.SyncProvider
{
    public class PropertyRelationInfo
    {
        public PropertyRelationInfo()
        {
            RelatedFeatIdList = new List<string>();
        }
        public PropertyInfo RelatedPropertyInfo { get; set; }

        public List<string> RelatedFeatIdList { get; set; }

        public bool IsCollection { get; set; }

    }
}
