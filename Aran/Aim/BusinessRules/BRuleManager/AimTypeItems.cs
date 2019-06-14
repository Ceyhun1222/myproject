using Aran.Aim;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRuleManager
{
    static class AimTypeItems
    {
        static AimTypeItems()
        {
            var list = AimMetadata.AimClassInfoList.Where(ci => 
                (!string.IsNullOrEmpty(ci.AixmName) && (ci.AimObjectType == AimObjectType.Feature || ci.AimObjectType == AimObjectType.Object))).ToList();

            list.Sort((a, b) => { return string.Compare(a.AixmName, b.AixmName); }); 

            ClassInfoList = new ReadOnlyCollection<AimClassInfo>(list);
        }

        public static ReadOnlyCollection<AimClassInfo> ClassInfoList { get; private set; }

        public static List<AimPropInfo> GetProperties(AimClassInfo cInfo)
        {
            var list = new List<AimPropInfo>();

            if (cInfo.Index == (int)DataType.FeatureRef || 
                cInfo.Index == (int)ObjectType.FeatureRefObject ||
                cInfo.SubClassType == AimSubClassType.AbstractFeatureRef)
            {
                return list;
            }

            foreach (var pInfo in cInfo.Properties)
            {
                if (pInfo.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                if (!string.IsNullOrEmpty(pInfo.AixmName))
                {
                    list.Add(pInfo);
                }
                else
                {
                    if (cInfo.SubClassType == AimSubClassType.ValClass && pInfo.Name == "Value")
                        list.Add(pInfo);
                }
            }
            return list;
        }
    }

}
