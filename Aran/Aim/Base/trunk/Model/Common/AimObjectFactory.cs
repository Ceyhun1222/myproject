using System;
using System.Collections;
using System.Collections.Generic;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Aim.Package;
using Aran.Aixm;

namespace Aran.Aim
{
    public static partial class AimObjectFactory
    {
        public static IAimProperty CreateAimProperty (AimPropInfo aranPropInfo)
        {
            if ((aranPropInfo.TypeCharacter & PropertyTypeCharacter.List) == PropertyTypeCharacter.List)
                return CreateList (aranPropInfo.TypeIndex) as IAimProperty;

            return Create (aranPropInfo.TypeIndex) as IAimProperty;
        }

        #region Create
        
        public static AimObject Create (int objectType)
        {
            AllAimObjectType allType = (AllAimObjectType) objectType;

            if (allType < AllAimObjectType._2_DATATYPE)
                return CreateAimField ((AimFieldType) allType);

            if (allType < AllAimObjectType._3_OBJECT)
                return CreateADataType ((DataType) allType);

            if (allType < AllAimObjectType._4_FEATURE)
                return CreateAObject ((ObjectType) allType);

            if (allType < AllAimObjectType._5_ABSTRACT)
                return CreateFeature ((FeatureType) allType);

            if (allType > AllAimObjectType._6_ENUM)
                return CreateEnumType ((EnumType) allType);

            throw new Exception ("Factory Create not supported type: " + allType);
        }

        public static AimField CreateAimField (AimFieldType systemType, object defaultValue = null)
        {
            switch (systemType)
            {
                case AimFieldType.SysBool:
                    return new AimField<bool> (defaultValue == null ? default (bool) : Convert.ToBoolean (defaultValue));
                case AimFieldType.SysDateTime:
                    return new AimField<DateTime> (defaultValue == null ? default (DateTime) : (DateTime) defaultValue);
                case AimFieldType.SysDouble:
                    return new AimField<double> (defaultValue == null ? default (double) : Convert.ToDouble (defaultValue));
                case AimFieldType.SysGuid:
                    return new AimField<Guid> (defaultValue == null ? default (Guid) : (Guid) defaultValue);
                case AimFieldType.SysInt32:
                    return new AimField<Int32> (defaultValue == null ? default (Int32) : Convert.ToInt32 (defaultValue));
                case AimFieldType.SysInt64:
                    return new AimField<Int64> (defaultValue == null ? default (Int64) : Convert.ToInt64 (defaultValue));
                case AimFieldType.SysString:
                    return new AimField<String> (defaultValue == null ? String.Empty : defaultValue.ToString ());
                case AimFieldType.SysUInt32:
                    return new AimField<UInt32> (defaultValue == null ? default (UInt32) : Convert.ToUInt32 (defaultValue));
                case AimFieldType.SysEnum:
                    return new AimField<Int32> (defaultValue == null ? default (Int32) : Convert.ToInt32 (defaultValue), AimFieldType.SysEnum);
                case AimFieldType.GeoPoint:
                    return new AimField<Aran.Geometries.Point> (defaultValue == null ? new Aran.Geometries.Point () : defaultValue as Aran.Geometries.Point);
                case AimFieldType.GeoPolyline:
                    return new AimField<Aran.Geometries.MultiLineString> (defaultValue == null ? new Aran.Geometries.MultiLineString () : defaultValue as Aran.Geometries.MultiLineString);
                case AimFieldType.GeoPolygon:
                    return new AimField<Aran.Geometries.MultiPolygon> (defaultValue == null ? new Aran.Geometries.MultiPolygon () : defaultValue as Aran.Geometries.MultiPolygon);
                default:
                    throw new Exception ("CreateSystemType is not supported for type: " + systemType);
            }
        }

        #endregion

        #region CreateList

        public static IList CreateList (int objectType)
        {
            AllAimObjectType allType = (AllAimObjectType) objectType;

            if (allType < AllAimObjectType._2_DATATYPE)
                return CreateAimFieldList ((AimFieldType) allType);

            if (allType < AllAimObjectType._3_OBJECT)
                return CreateADataTypeList ((DataType) allType);

            if (allType < AllAimObjectType._4_FEATURE)
                return CreateAObjectList ((ObjectType) allType);

            if (allType < AllAimObjectType._5_ABSTRACT)
                return CreateFeatureList ((FeatureType) allType);

            if (allType > AllAimObjectType._6_ENUM)
                return CreateEnumTypeList ((EnumType) allType);

            return CreateAbstractList ((AbstractType) allType);
        }

        private static IList CreateAimFieldList (AimFieldType systemType)
        {
            switch (systemType)
            {
                case AimFieldType.SysBool:
                    return new List<AimField<bool>> ();
                case AimFieldType.SysDateTime:
                    return new List<AimField<DateTime>> ();
                case AimFieldType.SysDouble:
                    return new List<AimField<double>> ();
                case AimFieldType.SysGuid:
                    return new List<AimField<Guid>> ();
                case AimFieldType.SysInt32:
                    return new List<AimField<Int32>> ();
                case AimFieldType.SysInt64:
                    return new List<AimField<Int64>> ();
                case AimFieldType.SysString:
                    return new List<AimField<string>> ();
                case AimFieldType.SysUInt32:
                    return new List<AimField<UInt32>> ();
                case AimFieldType.GeoPoint:
                    return new List<AimField<Aran.Geometries.Point>> ();
                case AimFieldType.GeoPolyline:
                    return new List<AimField<Aran.Geometries.LineString>> ();
                case AimFieldType.GeoPolygon:
                    return new List<AimField<Aran.Geometries.MultiPolygon>> ();
                default:
                    throw new Exception ("CreateSystemTypeList is not supported for type: " + systemType);
            }
        }

        #endregion

    }
}
