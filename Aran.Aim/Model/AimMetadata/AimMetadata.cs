using System;
using System.Collections.Generic;
using Aran.Aim.Objects;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using System.Reflection;

namespace Aran.Aim
{
    public static class AimMetadata
    {
        public static List<AimClassInfo> AimClassInfoList
        {
            get
            {
                lock (_monitor)
                {


                    if (_aimClassInfoList == null)
                        LoadAimClassInfoList();

                    return _aimClassInfoList;

                }
            }
        }

        public static AimClassInfo GetClassInfoByIndex(IAimObject aimObject)
        {
            return GetClassInfoByIndex(GetAimTypeIndex(aimObject));
        }

        public static AimClassInfo GetClassInfoByIndex(int index)
        {
            lock (_monitor)
            {
                if (_aimClassInfoList == null)
                    LoadAimClassInfoList();

                AimClassInfo classInfo;
                _aimClassInfoDict.TryGetValue(index, out classInfo);
                return classInfo;
            }
        }

        public static int GetAimTypeIndex(IAimObject aimObject)
        {
            switch (aimObject.AimObjectType)
            {
                case AimObjectType.Field:
                    return (int)((AimField)aimObject).FieldType;
                case AimObjectType.Object:
                    return (int)((AObject)aimObject).ObjectType;
                case AimObjectType.Feature:
                    return (int)((Feature)aimObject).FeatureType;
                default:
                    return (int)((ADataType)aimObject).DataType;
            }
        }

        public static Type GetPropertyEnumType(int aimTypeIndex)
        {
            string typeName = GetAimTypeName(aimTypeIndex);

            string propTypeName = "Aran.Aim.PropertyEnum.Property" + typeName;
            Type enumType = Type.GetType(propTypeName);

            if (enumType == null)
            {
                return GetPropertyEnumType(AimObjectFactory.Create(aimTypeIndex));
            }
            return enumType;
        }

        public static Type GetPropertyEnumType(AimObject aimObject)
        {
            Type type = aimObject.GetType();

            while (true)
            {
                string propTypeName = "Aran.Aim.PropertyEnum.Property" + type.Name;
                Type enumType = Type.GetType(propTypeName);

                if (enumType != null)
                    return enumType;

                if (type.BaseType == null)
                    return null;

                type = type.BaseType;
            }
        }

        public static string GetAimTypeName(IAimObject aimObject)
        {
            return GetAimTypeName(GetAimTypeIndex(aimObject));
        }

        public static string GetAimTypeName(int aimTypeIndex)
        {
            AllAimObjectType allType = (AllAimObjectType)aimTypeIndex;
            return allType.ToString();
        }

        public static bool IsAbstract(IAimObject aimObject)
        {
            return IsAbstract(GetAimTypeIndex(aimObject));
        }

        public static bool IsAbstract(int aimTypeIndex)
        {
            AllAimObjectType allType = (AllAimObjectType)aimTypeIndex;
            return (allType > AllAimObjectType._5_ABSTRACT && allType < AllAimObjectType._6_ENUM);
        }

        public static bool IsValClass(int aimTypeIndex)
        {
            AllAimObjectType allType = (AllAimObjectType)aimTypeIndex;
            return (allType > AllAimObjectType._2_1_VALCLASS_BEGIN && allType < AllAimObjectType._2_1_VALCLASS_END);
        }

        public static bool IsAbstractFeatureRef(FeatureRef featureRef)
        {
            return IsAbstractFeatureRef(GetAimTypeIndex(featureRef));
        }

        public static bool IsAbstractFeatureRef(int aimTypeIndex)
        {
            AllAimObjectType allType = (AllAimObjectType)aimTypeIndex;
            return (allType > AllAimObjectType._2_2_ABSTRACTCLASS_BEGIN &&
                  allType < AllAimObjectType._2_2_ABSTRACTCLASS_END);
        }

        public static bool IsAbstractFeatureRefObject(int aimTypeIndex)
        {
            var allType = (AllAimObjectType)aimTypeIndex;
            return (allType > AllAimObjectType.FeatureRefObject && allType < AllAimObjectType._3_2_METADATA_BEGIN);
        }

        public static bool IsEnum(int aimTypeIndex)
        {
            AllAimObjectType allType = (AllAimObjectType)aimTypeIndex;
            return (allType > AllAimObjectType._6_ENUM);
        }

        public static bool IsChoice(IAimObject aimObject)
        {
            return IsChoice(AimMetadata.GetAimTypeIndex(aimObject));
        }

        public static bool IsChoice(int aimTypeIndex)
        {
            AllAimObjectType allType = (AllAimObjectType)aimTypeIndex;

            switch (allType)
            {
                case AllAimObjectType.ArrestingGearExtent:
                case AllAimObjectType.MarkingExtent:
                case AllAimObjectType.EquipmentChoice:
                case AllAimObjectType.SignificantPoint:
                case AllAimObjectType.GuidanceService:
                case AllAimObjectType.VerticalStructurePartGeometry:
                case AllAimObjectType.ObstacleAreaOrigin:
                case AllAimObjectType.HoldingPatternLength:
                case AllAimObjectType.FlightConditionElementChoice:
                case AllAimObjectType.FlightRoutingElementChoice:
                    return true;
                default:
                    return false;
            }
        }

        public static AimObjectType GetAimObjectType(IAimObject aimObject)
        {
            return GetAimObjectType(GetAimTypeIndex(aimObject));
        }

        public static AimObjectType GetAimObjectType(int aimTypeIndex)
        {
            AllAimObjectType allType = (AllAimObjectType)aimTypeIndex;

            if (allType < AllAimObjectType._2_DATATYPE || allType > AllAimObjectType._6_ENUM)
                return AimObjectType.Field;

            if (allType < AllAimObjectType._3_OBJECT)
                return AimObjectType.DataType;

            if (allType < AllAimObjectType._4_FEATURE)
                return AimObjectType.Object;

            if (allType < AllAimObjectType._5_ABSTRACT)
                return AimObjectType.Feature;

            if (allType < AllAimObjectType._5_1_ABSTRACT_FEATURE)
                return AimObjectType.Object;

            if (allType < AllAimObjectType._6_ENUM)
                return AimObjectType.Feature;

            return 0;

            throw new Exception("AranTypeIndex is invalid.");
        }

        public static AimPropInfo[] GetAimPropInfos(IAimObject aimObject)
        {
            return GetAimPropInfos(GetAimTypeIndex(aimObject));
        }

        public static AimPropInfo[] GetAimPropInfos(int aimTypeIndex)
        {
            AimClassInfo classInfo = GetClassInfoByIndex(aimTypeIndex);
            return classInfo.Properties.ToArray();
        }

        public static string GetEnumValueAsString(int enumValue, int enumTypeIndex)
        {
            EnumType enumTypeItem = (EnumType)enumTypeIndex;
            Type enumType = Type.GetType("Aran.Aim.Enums." + enumTypeItem);
            object enumObject = Enum.ToObject(enumType, enumValue);
            return enumObject.ToString();
        }

        public static Type GetEnumType(int enumTypeIndex)
        {
            EnumType enumTypeItem = (EnumType)enumTypeIndex;
            return Type.GetType("Aran.Aim.Enums." + enumTypeItem);
        }

        public static bool AimDescriptionLoaded { get; private set; }

        public static AimClassInfo GetClassInfoByName(string name)
        {
            if (name == "Point")
                return GetClassInfoByIndex((int)ObjectType.AixmPoint);

            return AimClassInfoList.Find(ci => ci.Name == name);
        }


        private static void LoadAimClassInfoList()
        {

            _aimClassInfoList = new List<AimClassInfo>();
            _aimClassInfoDict = new Dictionary<int, AimClassInfo>();

            Array values = Enum.GetValues(typeof(AllAimObjectType));
            foreach (object enumItem in values)
            {
                if (enumItem.ToString().StartsWith("_"))
                    continue;

                AimClassInfo aci = ToAimClassInfo(enumItem);
                _aimClassInfoList.Add(aci);
                _aimClassInfoDict.Add(aci.Index, aci);
            }

            SetParents();

            foreach (AimClassInfo classInfo in _aimClassInfoList)
            {
                if (classInfo.AimObjectType != AimObjectType.Field)
                {
                    classInfo.Properties.AddRange(AimMetadata.GetAimPropInfos_private(classInfo.Index));
                }
            }

            foreach (AimClassInfo classInfo in _aimClassInfoList)
            {
                foreach (AimPropInfo propInfo in classInfo.Properties)
                {
                    propInfo.PropType = _aimClassInfoDict[propInfo.TypeIndex];
                }
            }

            var appLocation = typeof(AimMetadata).Assembly.Location;
            var appDir = System.IO.Path.GetDirectoryName(appLocation);
            var aimDescFileName = appDir + "\\AIMDescription.xml";

            var adl = new Aran.Aim.AimDescriptionLoader();
            AimDescriptionLoaded = adl.Load(aimDescFileName);

        }

        private static AimClassInfo ToAimClassInfo(object enumItem)
        {
            AimClassInfo aci = new AimClassInfo();
            aci.Name = enumItem.ToString();
            aci.Index = (int)enumItem;
            aci.AimObjectType = Aim.AimMetadata.GetAimObjectType(aci.Index);
            aci.IsAbstract = Aim.AimMetadata.IsAbstract(aci.Index);

            if (Aim.AimMetadata.IsChoice(aci.Index))
                aci.SubClassType = AimSubClassType.Choice;
            else if (Aim.AimMetadata.IsAbstractFeatureRef(aci.Index))
                aci.SubClassType = AimSubClassType.AbstractFeatureRef;
            else if (Aim.AimMetadata.IsValClass(aci.Index))
                aci.SubClassType = AimSubClassType.ValClass;
            else if (Aim.AimMetadata.IsEnum(aci.Index))
                aci.SubClassType = AimSubClassType.Enum;
            else
                aci.SubClassType = AimSubClassType.None;


            if (aci.AimObjectType != AimObjectType.Field)
            {
                if (!aci.Name.StartsWith("FeatureRef") && !aci.Name.StartsWith("Abstract"))
                {
                    if (aci.AimObjectType == AimObjectType.Object && aci.Name == "AixmPoint")
                        aci.AixmName = "Point";
                    else
                        aci.AixmName = aci.Name;
                }
            }

            return aci;
        }

        private static AimPropInfo[] GetAimPropInfos_private(int aimTypeIndex)
        {
            string objectName = GetAimTypeName(aimTypeIndex);
            string typeName = "Aran.Aim.PropertyEnum.Metadata" + objectName;
            Type type = Type.GetType(typeName);

            if (type == null)
            {
                if (AimMetadata.IsAbstractFeatureRef(aimTypeIndex) &&
                    objectName.StartsWith("Abstract") &&
                    objectName.EndsWith("Ref"))
                {
                    typeName = "Aran.Aim.PropertyEnum.MetadataAbstractFeatureRef`1";
                    type = Type.GetType(typeName);

                    objectName = objectName.Substring(8);
                    objectName = objectName.Substring(0, objectName.Length - 3);
                    objectName = "Aran.Aim." + objectName + "Type";
                    type = type.MakeGenericType(Type.GetType(objectName));
                }
                else if (objectName.StartsWith("Abstract") &&
                    objectName.EndsWith("RefObject"))
                {
                    typeName = "Aran.Aim.PropertyEnum.MetadataAbstractFeatureRefObject`1";
                    type = Type.GetType(typeName);

                    objectName = "Aran.Aim.DataTypes." + objectName.Substring(0, objectName.Length - 6);
                    type = type.MakeGenericType(Type.GetType(objectName));
                }
            }

            FieldInfo fieldInfo = type.GetField("PropInfoList");
            AimPropInfoList aranPropInfoList = (AimPropInfoList)fieldInfo.GetValue(null);
            return aranPropInfoList.ToArray();
        }

        private static void SetParents()
        {
            AimClassInfo approachLegClassInfo = null;
            AimClassInfo segmentLegClassInfo = null;

            foreach (AimClassInfo classInfo in _aimClassInfoList)
            {
                if (AimMetadata.IsAbstract(classInfo.Index))
                {
                    string absEnumType = "Aran.Aim." + classInfo.Name + "Type";
                    Array values = Enum.GetValues(Type.GetType(absEnumType));

                    foreach (int subTypeIndex in values)
                    {
                        foreach (AimClassInfo classInfoSub in _aimClassInfoList)
                        {
                            if (classInfoSub.Index == subTypeIndex)
                            {
                                classInfoSub.Parent = classInfo;
                            }
                        }
                    }
                }

                if (approachLegClassInfo == null && classInfo.Name == "ApproachLeg")
                {
                    approachLegClassInfo = classInfo;
                }

                if (segmentLegClassInfo == null && classInfo.Name == "SegmentLeg")
                {
                    segmentLegClassInfo = classInfo;
                }
            }

            approachLegClassInfo.Parent = segmentLegClassInfo;
        }

        private static List<AimClassInfo> _aimClassInfoList = null;
        private static Dictionary<int, AimClassInfo> _aimClassInfoDict;
        private static object _monitor = new object();
    }
}
