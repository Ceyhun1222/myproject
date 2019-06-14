using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Metadata.UI;

namespace Aran.Temporality.CommonUtil.Util
{
    public class RelationUtil
    {
        private static readonly List<int> FeatureTypes = new List<int>(Enum.GetValues(typeof(FeatureType)).Cast<int>());

        public static string GetConnectionProperty(FeatureType from, FeatureType to)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex((int)from);
            var uiClassInfo = classInfo.UiInfo();
            var link=uiClassInfo.RefInfoList.Where(t => t.Direction == PropertyDirection.Sub && t.ClassInfo.Index==(int)to).FirstOrDefault();
            if (link == null) return null;

            var pathString = link.PropertyPath.Aggregate(string.Empty, (current, next) => current + (next.Name + "\n"));
            if (pathString.EndsWith("\n")) pathString = pathString.Substring(0, pathString.Length - 1);
            return pathString;
        }

        public static List<FeatureType> MayRefereTo(FeatureType me)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex((int)me);
            var uiClassInfo = classInfo.UiInfo();
            var list = uiClassInfo.RefInfoList.Where(t => t.Direction == PropertyDirection.Ref).Select(t => t.ClassInfo.Index).ToList();
            return list.Intersect(FeatureTypes).Cast<FeatureType>().ToList();
        }

        public static List<FeatureType> MayRefereFrom(FeatureType me)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex((int)me);
            var uiClassInfo = classInfo.UiInfo();
            var list = uiClassInfo.RefInfoList.Where(t => t.Direction == PropertyDirection.Sub).Select(t => t.ClassInfo.Index).ToList();
            return list.Intersect(FeatureTypes).Cast<FeatureType>().ToList();
        }

        public static void AddSubProperties(List<string> list, string parentPath, AimPropInfo info)
        {
            var newParentPath = parentPath==null?info.Name:parentPath+"\\"+info.Name;
            list.Add(newParentPath);

            if (info.PropType.SubClassType == AimSubClassType.ValClass) return;
            if (info.PropType.Properties == null) return;

            foreach (var prop in info.PropType.Properties.Where(prop => !prop.IsFeatureReference && prop.Name != "Id" && 
                prop.PropType!=info.PropType))
            {
                AddSubProperties(list, newParentPath, prop);
            }
        }

        public static List<string> GetAllPropertyPath(FeatureType featureType)
        {
            var result=new List<string>();
            var classInfo = AimMetadata.GetClassInfoByIndex((int)featureType);

            foreach (var prop in classInfo.Properties.Where(prop => !prop.IsFeatureReference && prop.Name!="Id" && prop.Name != "Metadata"))
            {
                AddSubProperties(result, null, prop);
            }

            return result;
        }
    }
}
