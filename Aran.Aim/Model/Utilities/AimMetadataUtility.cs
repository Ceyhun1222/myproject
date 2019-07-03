using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.PropertyEnum;

namespace Aran.Aim.Utilities
{
    public static class AimMetadataUtility
    {
        public static int GetPropertyIndexByName(AimObject obj, string name)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex(obj);
            foreach (var pI in classInfo.Properties)
            {
                if (pI.Name == name)
                    return pI.Index;
            }
            return -1;
        }


        /// <summary>
        /// Return Inner properties as AimPropInfo Array.
        /// </summary>
        /// <param name="aimTypeIndex"></param>
        /// <param name="propretyName">show inner properties with '/' symbol</param>
        /// <returns></returns>
        public static AimPropInfo[] GetInnerProps(int aimTypeIndex, string propertyName)
        {
            var propInfoList = new List<AimPropInfo>();
            string[] propNameArr = propertyName.Split("/.;\\".ToArray(), StringSplitOptions.RemoveEmptyEntries);

            AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex(aimTypeIndex);

            foreach (string propName in propNameArr)
            {
                AimPropInfo propInfo = classInfo.Properties[propName];
                if (propInfo == null)
                    break;

                propInfoList.Add(propInfo);

                classInfo = propInfo.PropType;
                if (classInfo == null)
                    break;
            }

            return propInfoList.ToArray();
        }

        public static List<IAimProperty> GetInnerPropertyValue(IAimObject aimObject, AimPropInfo[] innerPropInfos, bool loadComplexProperty = true)
        {
            List<IAimProperty> aimPropValList = new List<IAimProperty>();
            GetPropValue(aimObject, innerPropInfos, 0, loadComplexProperty, aimPropValList);
            return aimPropValList;
        }

        public static IEnumerable<AimClassInfo> GetChoiceSubTypes(AimClassInfo classInfo)
        {
            Type choiceEnumType = Type.GetType("Aran.Aim." + classInfo.Name + "Choice");
            Array values = Enum.GetValues(choiceEnumType);

            List<AimClassInfo> list = new List<AimClassInfo>();

            foreach (int choiceIndex in values)
                list.Add(AimMetadata.GetClassInfoByIndex(choiceIndex));

            return list;
        }

        public static AimPropInfo GetChoicePropInfo(IEditChoiceClass choiceClass)
        {
            AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex((int)((AObject)choiceClass).ObjectType);

            foreach (var propInfo in classInfo.Properties)
            {
                if (propInfo.IsFeatureReference && (int)propInfo.ReferenceFeature == choiceClass.RefType)
                    return propInfo;
                else if (propInfo.TypeIndex == choiceClass.RefType)
                    return propInfo;
            }

            return null;
        }

        private static void GetPropValue(IAimObject aimObject, AimPropInfo[] propInfos, int index, bool loadComplexProperty, List<IAimProperty> resultList)
        {
            if (index == propInfos.Length)
                return;

            if (AimMetadata.IsChoice(aimObject))
            {
                IEditChoiceClass editChoiceClass = (ChoiceClass)aimObject;

                if (editChoiceClass.RefValue == null ||
                    (editChoiceClass.RefType != propInfos[index].TypeIndex) &&
                    (!propInfos[index].IsFeatureReference || (int)propInfos[index].ReferenceFeature != editChoiceClass.RefType))
                {
                    return;
                }

                if (index == propInfos.Length - 1)
                {
                    resultList.Add(editChoiceClass.RefValue);
                    return;
                }

                aimObject = (IAimObject)editChoiceClass.RefValue;
                index++;
            }

            AimPropInfo propInfo = propInfos[index];
            IAimProperty aimPropVal = aimObject.GetValue(propInfo.Index);

            if (aimPropVal == null && loadComplexProperty && aimObject is DBEntity)
            {
                Type dbEntityType = aimObject.GetType();
                System.Reflection.PropertyInfo sysPropInfo = dbEntityType.GetProperty(propInfo.Name);
                object obj = sysPropInfo.GetValue(aimObject, null);
                aimPropVal = aimObject.GetValue(propInfo.Index);
            }

            if (aimPropVal == null)
                return;

            if (index == propInfos.Length - 1)
            {
                resultList.Add(aimPropVal);
            }
            else
            {
                if (aimPropVal.PropertyType == AimPropertyType.List)
                {
                    System.Collections.IList listValue = (System.Collections.IList)aimPropVal;
                    foreach (AObject aObj in listValue)
                    {
                        GetPropValue(aObj, propInfos, index + 1, loadComplexProperty, resultList);
                    }
                }
                else if (aimPropVal.PropertyType == AimPropertyType.Object)
                {
                    GetPropValue((IAimObject)aimPropVal, propInfos, index + 1, loadComplexProperty, resultList);
                }
            }
        }

        public static List<AimClassInfo> GetAbstractChilds(AimClassInfo classInfo)
        {
            if (classInfo.SubClassType == AimSubClassType.AbstractFeatureRef)
            {
                string name = classInfo.Name.Substring("Abstract".Length, classInfo.Name.Length - "AbstractRef".Length);

                AllAimObjectType aaot = (AllAimObjectType)Enum.Parse(typeof(AllAimObjectType), name);
                classInfo = AimMetadata.GetClassInfoByIndex((int)aaot);
            }


            var list = new List<AimClassInfo>();
            string abstractFeatName = "";
            if (AimMetadata.IsAbstractFeatureRefObject(classInfo.Index))
            {
                abstractFeatName = classInfo.Name.Substring("Abstract".Length, classInfo.Name.Length - "Abstract".Length - "RefObject".Length);
            }
            foreach (var ci in AimMetadata.AimClassInfoList)
            {


                if (ci.Parent != null)
                {
                    if (ci.Parent == classInfo || ci.Parent.Name == abstractFeatName)
                    {
                        if (ci.IsAbstract)
                        {
                            var lv = GetAbstractChilds(ci);
                            list.AddRange(lv);
                        }
                        else
                        {
                            list.Add(ci);
                        }
                    }
                }
            }

            return list;
        }

        public static bool SetValue(IAimObject aimObject, AimPropInfo[] innerPropInfos, List<int> listIndexs, IAimProperty value)
        {
            if (innerPropInfos.Length == 0)
                return false;

            IAimObject propAimObject = aimObject;
            int nexListItemIndex = -1;
            if (listIndexs.Count > 0)
                nexListItemIndex = 0;

            for (int i = 0; i < innerPropInfos.Length - 1; i++)
            {
                var propInfo = innerPropInfos[i];
                var aimPropVal = propAimObject.GetValue(propInfo.Index);
                if (aimPropVal == null || propInfo.PropType.AimObjectType != AimObjectType.Object)
                    return false;

                if (aimPropVal.PropertyType == AimPropertyType.List)
                {
                    if (nexListItemIndex == -1)
                        return false;
                    var listPropVal = aimPropVal as System.Collections.IList;
                    if (listIndexs[nexListItemIndex] >= listPropVal.Count)
                        return false;

                    propAimObject = listPropVal[listIndexs[nexListItemIndex]] as IAimObject;

                    nexListItemIndex++;
                    if (nexListItemIndex >= listIndexs.Count)
                        nexListItemIndex = -1;
                }
                else
                {
                    if (propInfo.PropType.SubClassType == AimSubClassType.Choice &&
                        value == null &&
                        i == innerPropInfos.Length - 2)
                    {

                        propAimObject.SetValue(propInfo.Index, null);
                        return true;
                    }
                    else
                    {
                        propAimObject = aimPropVal as IAimObject;
                    }
                }
            }

            propAimObject.SetValue(innerPropInfos.Last().Index, value);
            return true;
        }

        public static void GetReferencesFeatures(IAimObject aimObject, List<RefFeatureProp> refFeaturePropList)
        {
            GetReferencesFeatures(aimObject, refFeaturePropList, new List<AimPropInfo>(), new List<int>());
        }

        public static IAimProperty GetChoicePropValue(IAimObject aimObj, AimPropInfo aimPropInfo)
        {
            var editChoice = aimObj as IEditChoiceClass;
            if (editChoice == null)
                return null;

            if (editChoice.RefType != (aimPropInfo.IsFeatureReference ? (int)aimPropInfo.ReferenceFeature : aimPropInfo.TypeIndex))
                return null;

            return editChoice.RefValue;
        }

        public static bool IsGeoPropInfo(AimPropInfo propInfo)
        {
            switch (propInfo.TypeIndex)
            {
                case (int)AimFieldType.GeoPoint:
                case (int)AimFieldType.GeoPolyline:
                case (int)AimFieldType.GeoPolygon:
                    return true;
                default:
                    return false;
            }
        }

        public static List<Tuple<FeatureType, string>> GetUsedByFeatureTypeList(
            ObjectType objType, string seperator = "/", bool byAixmName = false, bool addObjectName = false)
        {
            var result = new List<Tuple<FeatureType, string>>();

            foreach (var cInfo in AimMetadata.AimClassInfoList)
            {
                if (cInfo.AimObjectType == AimObjectType.Feature)
                {
                    var propNameList = new List<string>();
                    var cashe = new Dictionary<ObjectType, List<string>>();
                    var history = new HashSet<int>();

                    GetPropNameInObject(cInfo, (int)objType, seperator,
                        byAixmName, addObjectName, string.Empty, propNameList, cashe, history);

                    foreach (var propName in propNameList)
                        result.Add(new Tuple<FeatureType, string>((FeatureType)cInfo.Index, propName));
                }
            }

            return result;
        }

        private static void GetPropNameInObject(
            AimClassInfo cInfo,
            int typeIndex,
            string seperator,
            bool byAixmName,
            bool addObjectName,
            string prevPropPath,
            List<string> result,
            Dictionary<ObjectType, List<string>> cashe,
            HashSet<int> history)
        {
            if (!history.Add(cInfo.Index))
                return;

            foreach (var pInfo in cInfo.Properties)
            {
                if (pInfo.TypeIndex == typeIndex)
                {
                    if (result == null)
                        result = new List<string>();

                    var s = prevPropPath + (byAixmName ? pInfo.AixmName : pInfo.Name);
                    if (addObjectName)
                        s += seperator + (byAixmName ? pInfo.PropType.AixmName : pInfo.PropType.Name);
                    result.Add(s);
                }
                else if (pInfo.PropType.AimObjectType == AimObjectType.Object)
                {
                    string newPrevPropPath = prevPropPath + (byAixmName ? pInfo.AixmName : pInfo.Name) + seperator;
                    if (addObjectName)
                        newPrevPropPath += (byAixmName ? pInfo.PropType.AixmName : pInfo.PropType.Name) + seperator;

                    GetPropNameInObject(
                        pInfo.PropType,
                        typeIndex,
                        seperator,
                        byAixmName,
                        addObjectName,
                        newPrevPropPath,
                        result,
                        cashe,
                        history);
                }
            }
        }


        public static bool ClearMissingLinks(Feature feature, Dictionary<string, List<Guid>> links, Feature lastCorrection = null)
        {
            if (feature == null || links == null || links.Count == 0)
                return false;

            var result = false;

            var problemPropIndexes = new HashSet<int>();

            foreach (var link in links)
            {
                var propertyPath = link.Key;
                var guids = link.Value;

                var problemPropInfoList = GetInnerProps(AimMetadata.GetAimTypeIndex(feature), propertyPath).Select(x => x.Index).ToList();

                if (problemPropInfoList.Count == 0)
                    continue;

                var linkResult = ClearMissingLinks(feature, guids, problemPropInfoList, false);

                if (linkResult)
                {
                    result = true;

                    problemPropIndexes.Add(problemPropInfoList.First());

                    if (problemPropInfoList.Count == 1)
                        feature.SetNilReason(problemPropInfoList.First(), NilReason.unknown);
                }
            }

            if (!result)
                return false;

            var classInfo = AimMetadata.GetClassInfoByIndex(feature);

            foreach (var property in classInfo.Properties)
            {
                if (property.Index < (int)PropertyFeature.NEXT_CLASS)
                    continue;

                if (!problemPropIndexes.Contains(property.Index))
                {
                    if (lastCorrection != null && (feature as IAimObject).GetValue(property.Index) != null && (lastCorrection as IAimObject).GetValue(property.Index) == null)
                    {
                        (feature as IAimObject).SetValue(property.Index, null);
                    }
                }
            }


            return result;
        }

        private static bool ClearMissingLinks(IAimObject aimObject, List<Guid> guids, List<int> propInfoList, bool result)
        {
            var propIndex = propInfoList.First();
            var aimPropVal = aimObject.GetValue(propIndex);
            propInfoList = propInfoList.Skip(1).ToList();

            if (aimPropVal == null)
                return result;

            if (propInfoList.Count > 0)
            {
                if (aimPropVal.PropertyType == AimPropertyType.List && aimPropVal is System.Collections.IList list)
                {
                    var count = list.Count;
                    for (int i = 0; i < count; i++)
                    {
                        var aimObjectItem = list[i] as IAimObject;
                        result = ClearMissingLinks(aimObjectItem, guids, propInfoList, result);
                    }
                }
                else if (aimPropVal.PropertyType == AimPropertyType.Object)
                {
                    result = ClearMissingLinks(aimPropVal as IAimObject, guids, propInfoList, result);
                }

                return result;
            }

            if (aimPropVal is IEditChoiceClass editChoiceClass)
            {
                if (AimMetadata.GetAimObjectType(editChoiceClass.RefType) == AimObjectType.Feature &&
                    editChoiceClass.RefValue is FeatureRef featureRef && guids.Contains(featureRef.Identifier))
                {
                    aimObject.SetValue(propIndex, null);
                    result = true;
                }
            }
            else if (aimPropVal.PropertyType == AimPropertyType.DataType && aimPropVal is FeatureRef featureRef &&
                     guids.Contains(featureRef.Identifier))
            {
                aimObject.SetValue(propIndex, null);
                result = true;
            }
            else if (aimPropVal.PropertyType == AimPropertyType.List)
            {
                var list = aimPropVal as System.Collections.IList;
                var count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    var aimObjectItem = list[i] as IAimObject;
                    if (aimObjectItem is FeatureRefObject featureRefObject &&
                        guids.Contains(featureRefObject.Feature.Identifier))
                    {
                        list.RemoveAt(i);
                        count--;
                        result = true;
                    }
                    else if (aimObjectItem is IEditChoiceClass editChoiceClassItem)
                    {
                        if (AimMetadata.GetAimObjectType(editChoiceClassItem.RefType) == AimObjectType.Feature &&
                            editChoiceClassItem.RefValue is FeatureRef featureRefItem && guids.Contains(featureRefItem.Identifier))
                        {
                            list.RemoveAt(i);
                            count--;
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        private static void GetReferencesFeatures(IAimObject aimObject, List<RefFeatureProp> refFeaturePropList, List<AimPropInfo> propInfoList, List<int> listItemList)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex(aimObject);

            if (aimObject is IEditChoiceClass)
            {
                var editCC = aimObject as IEditChoiceClass;
                if (AimMetadata.GetAimObjectType(editCC.RefType) == AimObjectType.Feature && editCC.RefValue is FeatureRef)
                {
                    var rfp = new RefFeatureProp();
                    rfp.RefIdentifier = (editCC.RefValue as FeatureRef).Identifier;
                    rfp.FeatureType = (FeatureType)editCC.RefType;
                    rfp.PropInfoList.AddRange(propInfoList);

                    var propInfo = classInfo.Properties.Where(pi => pi.TypeIndex == editCC.RefType).FirstOrDefault();
                    if (propInfo != null)
                        rfp.PropInfoList.Add(propInfo);

                    rfp.ListPropIndexes.AddRange(listItemList);
                    refFeaturePropList.Add(rfp);
                }
                return;
            }

            foreach (var propInfo in classInfo.Properties)
            {
                var aimPropVal = aimObject.GetValue(propInfo.Index);
                if (aimPropVal != null)
                {
                    switch (aimPropVal.PropertyType)
                    {
                        case AimPropertyType.DataType:
                            {
                                if (aimPropVal is FeatureRef)
                                {
                                    var rfp = new RefFeatureProp();

                                    var featRef = aimPropVal as FeatureRef;
                                    rfp.RefIdentifier = featRef.Identifier;

                                    if (featRef is IAbstractFeatureRef)
                                        rfp.FeatureType = (FeatureType)(featRef as IAbstractFeatureRef).FeatureTypeIndex;
                                    else
                                        rfp.FeatureType = propInfo.ReferenceFeature;

                                    rfp.PropInfoList.AddRange(propInfoList);
                                    rfp.PropInfoList.Add(propInfo);
                                    rfp.ListPropIndexes.AddRange(listItemList);
                                    refFeaturePropList.Add(rfp);
                                }
                            }
                            break;
                        case AimPropertyType.List:
                            {
                                var newPropInfoList = new List<AimPropInfo>(propInfoList);
                                newPropInfoList.Add(propInfo);

                                var list = aimPropVal as System.Collections.IList;
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var newListItemList = new List<int>(listItemList);
                                    var aimObjectItem = list[i] as IAimObject;
                                    newListItemList.Add(i);

                                    if (propInfo.TypeIndex == (int)ObjectType.FeatureRefObject)
                                    {
                                        var rfp = new RefFeatureProp();

                                        var featRefObj = aimObjectItem as FeatureRefObject;
                                        rfp.RefIdentifier = featRefObj.Feature.Identifier;
                                        rfp.FeatureType = propInfo.ReferenceFeature;

                                        rfp.PropInfoList.AddRange(propInfoList);
                                        rfp.PropInfoList.Add(propInfo);
                                        rfp.ListPropIndexes.AddRange(newListItemList);
                                        refFeaturePropList.Add(rfp);
                                    }
                                    else
                                    {
                                        GetReferencesFeatures(aimObjectItem, refFeaturePropList, newPropInfoList, newListItemList);
                                    }
                                }
                            }
                            break;
                        case AimPropertyType.Object:
                            {
                                var newPropInfoList = new List<AimPropInfo>(propInfoList);
                                newPropInfoList.Add(propInfo);

                                GetReferencesFeatures(aimPropVal as IAimObject, refFeaturePropList, newPropInfoList, new List<int>(listItemList));
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// This method gets the list all non-abstract features' types, referenced from the current feature.
        /// </summary>
        /// <param name="featureType">Type of the currecnt feature.</param>
        /// <param name="checkedFeatTypeList">The list of already checked feature types.</param>
        /// <returns></returns>
        public static List<FeatureType> GetReferencesFeatures(int featureType)
        {
            return GetReferencesFeatures(featureType, new List<int>());
        }

        /// <summary>
        /// This method gets the list all non-abstract features' types, referenced from the current feature.
        /// </summary>
        /// <param name="featureType">Type of the currecnt feature</param>
        /// <param name="checkedFeatTypeList">The list of already checked feature types</param>
        /// <returns></returns>
        private static List<FeatureType> GetReferencesFeatures(int featureType, List<int> checkedFeatTypeList)
        {
            List<FeatureType> featureTypeList = new List<FeatureType>();
            var classInfo = AimMetadata.GetClassInfoByIndex(featureType);

            foreach (var propInfo in classInfo.Properties)
            {
                switch (propInfo.PropType.AimObjectType)
                {
                    case AimObjectType.Field:
                        continue;
                    case AimObjectType.DataType:
                        {
                            if (propInfo.IsFeatureReference)
                            {
                                if (propInfo.ReferenceFeature != 0)
                                {
                                    AddToList(featureTypeList, propInfo.ReferenceFeature);
                                }
                                else
                                {
                                    var tmp = AimMetadata.GetClassInfoByIndex(propInfo.TypeIndex);
                                    var resultList = GetAbstractChilds(tmp);
                                    foreach (var item in resultList)
                                        AddToList(featureTypeList, (FeatureType)item.Index);
                                }
                            }
                        }
                        break;
                    case AimObjectType.Object:
                        {
                            if (propInfo.IsFeatureReference)
                            {
                                AddToList(featureTypeList, propInfo.ReferenceFeature);
                            }
                            else if (!checkedFeatTypeList.Contains(propInfo.PropType.Index))
                            {
                                checkedFeatTypeList.Add(propInfo.PropType.Index);

                                if (AimMetadata.IsAbstractFeatureRefObject(propInfo.TypeIndex))
                                {
                                    var tmp = AimMetadata.GetClassInfoByIndex(propInfo.TypeIndex);
                                    var resultList = GetAbstractChilds(tmp);
                                    foreach (var item in resultList)
                                        AddToList(featureTypeList, (FeatureType)item.Index);
                                }
                                else
                                {
                                    var resultFeatList = GetReferencesFeatures(propInfo.PropType.Index, checkedFeatTypeList);
                                    foreach (var item in resultFeatList)
                                        AddToList(featureTypeList, item);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return featureTypeList;
        }

        private static void AddToList(List<FeatureType> list, FeatureType value)
        {
            if (list.Contains(value))
                return;
            list.Add(value);
        }

        public static List<string> GetGeometryPathes(FeatureType featureType)
        {
            return GetGeometryPathes((int)featureType);
        }

        public static List<string> GetGeometryPathes(int typeIndex)
        {
            var pathes = new List<string>();
            FindGeometryPathes(typeIndex, string.Empty, pathes);
            pathes = pathes.Select(x => x.Trim('.')).ToList();
            return pathes;
        }

        public static void FindGeometryPathes(int typeIndex, string name, List<string> pathes, List<int> types = null)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex(typeIndex);

            if (classInfo?.Properties == null) return;

            if (types == null)
                types = new List<int>();

            types.Add(typeIndex);

            foreach (var propInfo in classInfo.Properties)
            {
                if (propInfo.PropType.AimObjectType == AimObjectType.Field)
                {
                    if ((AimFieldType)propInfo.TypeIndex == AimFieldType.GeoPolyline || (AimFieldType)propInfo.TypeIndex == AimFieldType.GeoPolygon)
                        pathes.Add(name + "." + propInfo.Name);
                }
                else
                {
                    if (types.Contains(propInfo.TypeIndex))
                        continue;

                    FindGeometryPathes(propInfo.TypeIndex, name + "." + propInfo.Name, pathes, types);
                }
            }

            types.Remove(typeIndex);
        }
    }



    public class RefFeatureProp
    {
        public RefFeatureProp()
        {
            PropInfoList = new List<AimPropInfo>();
            ListPropIndexes = new List<int>();
        }

        public Guid RefIdentifier { get; set; }

        public FeatureType FeatureType { get; set; }

        public List<AimPropInfo> PropInfoList { get; private set; }

        public List<int> ListPropIndexes { get; private set; }

        public Features.Feature Feature { get; set; }
    }
}
