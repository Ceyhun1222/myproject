using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using System.IO;
using Aran.Package;
using AranGeom = Aran.Geometries;
using System.Collections;
using Aran.Aim.DataTypes;
using Aran.Aim.Objects;
using Aran.Aim.Data.Filters;
using GeoAPI.CoordinateSystems;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using GeoAPI.CoordinateSystems.Transformations;
using JtsGeom = GeoAPI.Geometries;
using NtsGeom = NetTopologySuite.Geometries;
using Aran.Converters.ConverterJtsGeom;
using NetTopologySuite.CoordinateSystems.Transformations;

namespace Aran.Aim.Data.Local
{
    internal class Packer
    {
        #region Writer

        public static byte[] ToBytes(IAimObject aimObj)
        {
            using (BinaryPackageWriter writer = new BinaryPackageWriter())
            {
                int aimTypeIndex = AimMetadata.GetAimTypeIndex(aimObj);
                writer.PutInt32(aimTypeIndex);

                WriteAimObject(aimObj, writer);

                byte[] buffer = writer.ToArray();
                return buffer;
            }
        }

        private static void WriteAimObject(IAimObject aimObj, BinaryPackageWriter writer)
        {
            AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex(aimObj);

            int propIndexStartPos = writer.CurrentPosition();

            //--- *     Put zero as each property start index [sizeof (Int32)]
            //          for reserve and update this each cell later when position known.
            writer.Writer.Write(new byte[(classInfo.Properties.Count + 1) * sizeof(Int32)]);

            int pos;

            for (int i = 0; i < classInfo.Properties.Count; i++)
            {
                pos = writer.CurrentPosition();
                //--- o     Go according property index.
                writer.Seek(propIndexStartPos + (i * sizeof(Int32)), SeekOrigin.Begin);
                //--- *     Set property value start position.
                writer.PutInt32(pos);
                //--- o     Return back position.
                writer.Seek(0, SeekOrigin.End);

                var propInfo = classInfo.Properties[i];
                IAimProperty propVal = aimObj.GetValue(propInfo.Index);
                if (propVal != null)
                    WritePropertyValue(propVal, writer);
            }

            pos = writer.CurrentPosition();
            //--- o     Go last property index.
            writer.Seek(propIndexStartPos + (classInfo.Properties.Count * sizeof(Int32)), SeekOrigin.Begin);
            //--- *     Set current position.
            writer.PutInt32(pos);
            //--- o     Return back position.
            writer.Seek(0, SeekOrigin.End);
        }

        private static void WritePropertyValue(IAimProperty propValue, BinaryPackageWriter writer)
        {
            switch (propValue.PropertyType)
            {
                case AimPropertyType.AranField:
                    WriteAimField(propValue as AimField, writer);
                    break;
                case AimPropertyType.DataType:
                    WriteDataType(propValue as ADataType, writer);
                    break;
                case AimPropertyType.Object:
                    {
                        IEditChoiceClass choiceClass = propValue as IEditChoiceClass;
                        if (choiceClass != null)
                            WriteChoiceClass(choiceClass, writer);
                        else
                            WriteAimObject(propValue as IAimObject, writer);
                    }
                    break;
                case AimPropertyType.List:
                    WriteObjectList(propValue as IList, writer);
                    break;
            }
        }

        private static void WriteAimField(AimField aimField, PackageWriter writer)
        {
            object value = (aimField as IEditAimField).FieldValue;

            switch (aimField.FieldType)
            {
                case AimFieldType.SysBool:
                    writer.PutBool((bool)value);
                    break;
                case AimFieldType.SysDateTime:
                    writer.PutDateTime((DateTime)value);
                    break;
                case AimFieldType.SysDouble:
                    writer.PutDouble((double)value);
                    break;
                case AimFieldType.SysGuid:
                    writer.PutString(value.ToString());
                    break;
                case AimFieldType.SysEnum:
                case AimFieldType.SysInt32:
                    writer.PutInt32((Int32)value);
                    break;
                case AimFieldType.SysInt64:
                    writer.PutInt64((Int64)value);
                    break;
                case AimFieldType.SysString:
                    writer.PutString(value as string);
                    break;
                case AimFieldType.SysUInt32:
                    writer.PutUInt32((UInt32)value);
                    break;
                case AimFieldType.GeoPoint:
                case AimFieldType.GeoPolyline:
                case AimFieldType.GeoPolygon:
                    ((AranGeom.Geometry)value).Pack(writer);
                    break;
            }
        }

        private static void WriteDataType(ADataType dataTypeObject, BinaryPackageWriter writer)
        {
            if (dataTypeObject.DataType == DataType.TextNote)
            {
                TextNote textNote = dataTypeObject as TextNote;
                if (textNote.Lang != 0 && textNote.Value != null)
                {
                    writer.PutInt32((int)textNote.Lang);
                    writer.PutString(textNote.Value);
                }
                return;
            }

            if (dataTypeObject.DataType == DataType.FeatureRef)
            {
                FeatureRef featRef = dataTypeObject as FeatureRef;
                writer.PutString(featRef.Identifier.ToString());
                return;
            }

            IEditValClass editValClass = dataTypeObject as IEditValClass;
            if (editValClass != null)
            {
                writer.PutDouble(editValClass.Value);
                writer.PutInt32(editValClass.Uom);
                return;
            }

            IAbstractFeatureRef absFeatRef = dataTypeObject as IAbstractFeatureRef;
            if (absFeatRef != null)
            {
                writer.PutString(absFeatRef.Identifier.ToString());
                writer.PutInt32(absFeatRef.FeatureTypeIndex);
                return;
            }

            WriteAimObject(dataTypeObject, writer);
        }

        private static void WriteChoiceClass(IEditChoiceClass choiceClass, BinaryPackageWriter writer)
        {
            writer.PutInt32(choiceClass.RefType);
            int refValueAimTypeIndex = AimMetadata.GetAimTypeIndex(choiceClass.RefValue as IAimObject);
            writer.PutInt32(refValueAimTypeIndex);
            WritePropertyValue(choiceClass.RefValue, writer);
        }

        private static void WriteObjectList(IList objList, BinaryPackageWriter writer)
        {
            writer.PutInt32(objList.Count);
            int listItemPosSector = writer.CurrentPosition();
            writer.Writer.Write(new byte[objList.Count * sizeof(Int32)]);

            for (int i = 0; i < objList.Count; i++)
            {
                int pos = writer.CurrentPosition();
                writer.Seek(listItemPosSector + (i * sizeof(Int32)), SeekOrigin.Begin);
                writer.PutInt32(pos);
                writer.Seek(0, SeekOrigin.End);

                AObject aObj = objList[i] as AObject;
                writer.PutInt32((int)aObj.ObjectType);
                WriteAimObject(aObj, writer);
            }
        }

        #endregion

        #region Reader

        public static IAimObject FromBytes(byte[] buffer)
        {
            using (var reader = new BinaryPackageReader(buffer))
            {
                int aimTypeIndex = reader.GetInt32();
                var aimObj = AimObjectFactory.Create(aimTypeIndex);

                return ReadAimObject(aimObj, reader);
            }
        }


        private static IAimObject ReadAimObject(IAimObject aimObj, BinaryPackageReader reader)
        {
            AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex(aimObj);

            int[] propIndexStartPosArr = new int[classInfo.Properties.Count + 1];

            for (int i = 0; i < propIndexStartPosArr.Length; i++)
            {
                //--- *     Get each property value start position.
                propIndexStartPosArr[i] = reader.GetInt32();
            }

            for (int i = 0; i < propIndexStartPosArr.Length - 1; i++)
            {
                if (propIndexStartPosArr[i] < propIndexStartPosArr[i + 1])
                {
                    reader.Seek(propIndexStartPosArr[i], SeekOrigin.Begin);

                    var propInfo = classInfo.Properties[i];
                    IAimProperty aimPropValue;

                    if (propInfo.IsList)
                        aimPropValue = AimObjectFactory.CreateList(propInfo.TypeIndex) as IAimProperty;
                    else
                        aimPropValue = AimObjectFactory.Create(propInfo.TypeIndex) as IAimProperty;

                    ReadPropertyValue(aimPropValue, reader);

                    aimObj.SetValue(propInfo.Index, aimPropValue);
                }
            }

            return aimObj;
        }

        private static void ReadPropertyValue(IAimProperty propValue, BinaryPackageReader reader)
        {
            switch (propValue.PropertyType)
            {
                case AimPropertyType.AranField:
                    ReadAimField(propValue as AimField, reader);
                    break;
                case AimPropertyType.DataType:
                    ReadDataType(propValue as ADataType, reader);
                    break;
                case AimPropertyType.Object:
                    {
                        IEditChoiceClass choiceClass = propValue as IEditChoiceClass;
                        if (choiceClass != null)
                            ReadChoiceClass(choiceClass, reader);
                        else
                            ReadAimObject(propValue as IAimObject, reader);
                    }
                    break;
                case AimPropertyType.List:
                    ReadObjectList(propValue as IList, reader);
                    break;
            }
        }

        private static void ReadAimField(AimField aimField, BinaryPackageReader reader)
        {
            IEditAimField editAimField = aimField as IEditAimField;

            switch (aimField.FieldType)
            {
                case AimFieldType.SysBool:
                    editAimField.FieldValue = reader.GetBool();
                    break;
                case AimFieldType.SysDateTime:
                    editAimField.FieldValue = reader.GetDateTime();
                    break;
                case AimFieldType.SysDouble:
                    editAimField.FieldValue = reader.GetDouble();
                    break;
                case AimFieldType.SysGuid:
                    editAimField.FieldValue = new Guid(reader.GetString());
                    break;
                case AimFieldType.SysEnum:
                case AimFieldType.SysInt32:
                    editAimField.FieldValue = reader.GetInt32();
                    break;
                case AimFieldType.SysInt64:
                    editAimField.FieldValue = reader.GetInt64();
                    break;
                case AimFieldType.SysString:
                    editAimField.FieldValue = reader.GetString();
                    break;
                case AimFieldType.SysUInt32:
                    editAimField.FieldValue = reader.GetUInt32();
                    break;
                case AimFieldType.GeoPoint:
                case AimFieldType.GeoPolyline:
                case AimFieldType.GeoPolygon:
                    ((AranGeom.Geometry)editAimField.FieldValue).Unpack(reader);
                    break;
            }
        }

        private static void ReadDataType(ADataType dataTypeObject, BinaryPackageReader reader)
        {
            if (dataTypeObject.DataType == DataType.TextNote)
            {
                TextNote textNote = dataTypeObject as TextNote;
                textNote.Lang = (Aran.Aim.Enums.language)reader.GetInt32();
                textNote.Value = reader.GetString();
                return;
            }

            if (dataTypeObject.DataType == DataType.FeatureRef)
            {
                FeatureRef featRef = dataTypeObject as FeatureRef;
                featRef.Identifier = new Guid(reader.GetString());
                return;
            }

            IEditValClass editValClass = dataTypeObject as IEditValClass;
            if (editValClass != null)
            {
                editValClass.Value = reader.GetDouble();
                editValClass.Uom = reader.GetInt32();
                return;
            }

            IAbstractFeatureRef absFeatRef = dataTypeObject as IAbstractFeatureRef;
            if (absFeatRef != null)
            {
                absFeatRef.Identifier = new Guid(reader.GetString());
                absFeatRef.FeatureTypeIndex = reader.GetInt32();
                return;
            }

            ReadAimObject(dataTypeObject, reader);
        }

        private static void ReadChoiceClass(IEditChoiceClass choiceClass, BinaryPackageReader reader)
        {
            choiceClass.RefType = reader.GetInt32();
            int refValueAimTypeIndex = reader.GetInt32();
            IAimProperty propValue = AimObjectFactory.Create(refValueAimTypeIndex) as IAimProperty;
            ReadPropertyValue(propValue, reader);
            choiceClass.RefValue = propValue;
        }

        private static void ReadObjectList(IList objList, BinaryPackageReader reader)
        {
            int count = reader.GetInt32();
            //--- * Skip List Item Position sector.
            reader.Seek(count * sizeof(Int32), SeekOrigin.Current);

            for (int i = 0; i < count; i++)
            {
                ObjectType objType = (ObjectType)reader.GetInt32();
                AObject aObj = AimObjectFactory.CreateAObject(objType);
                ReadAimObject(aObj, reader);
                objList.Add(aObj);
            }
        }

        #endregion

        #region GetPropValues

        public static IList GetPropValues(byte[] buffer, string propertyName)
        {
            var typeIndex = BitConverter.ToInt32(buffer, 0);
            var propIndexes = GetSearchPropInfos(typeIndex, propertyName);

            return GetPropValues(buffer, propIndexes);
        }

        public static IList GetPropValues(byte[] buffer, int[] propIndexes)
        {
            using (var reader = new BinaryPackageReader(buffer))
            {
                var typeIndex = reader.GetInt32();
                var classInfo = AimMetadata.GetClassInfoByIndex(typeIndex);
                return GetPropValues(reader, classInfo, propIndexes, 0);
            }
        }

        private static IList GetPropValues(BinaryPackageReader reader, AimClassInfo classInfo, int[] propIndexes, int propIdexStart)
        {
            if (propIdexStart >= propIndexes.Length)
                return null;

            var propIndex = propIndexes[propIdexStart];
            reader.Seek(propIndex * sizeof(Int32), SeekOrigin.Current);
            var startIndex = reader.GetInt32();
            var endIndex = reader.GetInt32();
            if (startIndex == endIndex)
                return null;

            reader.Seek(startIndex, SeekOrigin.Begin);

            var propInfo = classInfo.Properties[propIndex];

            switch (propInfo.PropType.AimObjectType)
            {
                case AimObjectType.Field:
                    {
                        return GetFieldPropValue(reader, propInfo.PropType);
                    }
                case AimObjectType.DataType:
                    {
                        return GetDataTypePropValue(reader, propInfo.PropType);
                    }
                case AimObjectType.Object:
                    {
                        if (propInfo.IsList)
                            return GetObjectListValues(reader, propInfo.PropType, propIndexes, propIdexStart + 1);
                        else if (propInfo.PropType.SubClassType == AimSubClassType.Choice)
                            return GetChoicePropValues(reader, propInfo.PropType, propIndexes, propIdexStart + 1);
                        else
                            return GetPropValues(reader, propInfo.PropType, propIndexes, propIdexStart + 1);
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        private static IList GetObjectListValues(BinaryPackageReader reader, AimClassInfo classInfo, int[] propIndexes, int propIdexStart)
        {
            var count = reader.GetInt32();
            int[] startPosArr = new int[count];

            for (int i = 0; i < count; i++)
                startPosArr[i] = reader.GetInt32();

            var list = new List<object>();
            var propIndex = propIndexes[propIdexStart];
            var propInfo = classInfo.Properties[propIndex];

            for (int i = 0; i < count; i++)
            {
                var pos = startPosArr[i];
                reader.Seek(pos, SeekOrigin.Begin);

                var itemTypeIndex = reader.GetInt32();

                var itemList = GetPropValues(reader, classInfo, propIndexes, propIdexStart);
                if (itemList != null)
                {
                    foreach (var item in itemList)
                        list.Add(item);
                }
            }

            return list;
        }

        private static IList GetChoicePropValues(BinaryPackageReader reader, AimClassInfo classInfo, int[] propIndexes, int propIdexStart)
        {
            var propIndex = propIndexes[propIdexStart];
            var propInfo = classInfo.Properties[propIndex];
            var refType = reader.GetInt32();

            if (refType != propInfo.TypeIndex)
                return null;

            int refValueAimTypeIndex = reader.GetInt32();
            return GetPropValues(reader, propInfo.PropType, propIndexes, propIdexStart + 1);
        }

        private static IList GetFieldPropValue(BinaryPackageReader reader, AimClassInfo classInfo)
        {
            var fieldType = (AimFieldType)classInfo.Index;
            object res = null;

            switch (fieldType)
            {
                case AimFieldType.SysBool:
                    res = reader.GetBool();
                    break;
                case AimFieldType.SysDateTime:
                    res = reader.GetDateTime();
                    break;
                case AimFieldType.SysDouble:
                    res = reader.GetDouble();
                    break;
                case AimFieldType.SysGuid:
                    res = new Guid(reader.GetString());
                    break;
                case AimFieldType.SysEnum:
                case AimFieldType.SysInt32:
                    res = reader.GetInt32();
                    break;
                case AimFieldType.SysInt64:
                    res = reader.GetInt64();
                    break;
                case AimFieldType.SysString:
                    res = reader.GetString();
                    break;
                case AimFieldType.SysUInt32:
                    res = reader.GetUInt32();
                    break;
                case AimFieldType.GeoPoint:
                case AimFieldType.GeoPolyline:
                case AimFieldType.GeoPolygon:
                    return GetGeomPropValue(reader, classInfo);
            }

            var objList = new List<object>();
            objList.Add(res);
            return objList;
        }

        private static IList GetDataTypePropValue(BinaryPackageReader reader, AimClassInfo classInfo)
        {
            var dataType = (DataType)classInfo.Index;
            object res = null;

            if (dataType == DataType.TextNote)
            {
                var lang = (Aran.Aim.Enums.language)reader.GetInt32();
                var text = reader.GetString();
                res = string.Format("({0},{1})", lang, text);
            }
            else if (dataType == DataType.FeatureRef)
            {
                res = new Guid(reader.GetString());
            }
            else if (classInfo.SubClassType == AimSubClassType.ValClass)
            {
                var val = reader.GetDouble();
                var uom = reader.GetInt32();
                res = string.Format("({0},{1})", val, uom);
            }
            else if (classInfo.SubClassType == AimSubClassType.AbstractFeatureRef)
            {
                var identifier = new Guid(reader.GetString());
                var featureTypeIndex = reader.GetInt32();
                res = string.Format("({0},{1})", identifier, featureTypeIndex);
            }
            else
            {
                return null;
            }

            var list = new List<object>();
            list.Add(res);
            return list;
        }

        private static IList GetGeomPropValue(BinaryPackageReader reader, AimClassInfo classInfo)
        {
            return new List<object>();
        }

        #endregion

        #region Filter Value

        public static bool IsConditionSatisfied(byte[] buffer, OperationChoice operChoice)
        {
            if (_centralMeridian == null)
                return false;

            operChoice = CloneFilter(operChoice);
            ChangeValClassValToSi(operChoice);

            _filterPrjGeom = null;

            bool result;

            using (var reader = new BinaryPackageReader(buffer))
            {
                var typeIndex = reader.GetInt32();
                var classInfo = AimMetadata.GetClassInfoByIndex(typeIndex);
                result = IsConditionSatisfied(reader, classInfo, null, -1, operChoice);
            }

            _filterOriginalGeom = null;
            _filterPrjGeom = null;

            return result;
        }

        public static double? CentralMeridian
        {
            get
            {
                return _centralMeridian;
            }
            set
            {
                _centralMeridian = value;

                if (value == null)
                    return;

                var csFact = new CoordinateSystemFactory();
                var utm = csFact.CreateFromWkt(
                    "PROJCS[\"ETRS89 / ETRS-TM35\"," +
                        "GEOGCS[\"GCS_WGS_1984\"," +
                            "DATUM[\"D_WGS_1984\"," +
                                "SPHEROID[\"WGS_1984\",6378137.0,298.257223563]]," +
                            "PRIMEM[\"Greenwich\",0]," +
                            "UNIT[\"Degree\",0.017453292519943295]]," +
                        "PROJECTION[\"Transverse_Mercator\"]," +
                        "PARAMETER[\"latitude_of_origin\",0]," +
                        "PARAMETER[\"central_meridian\"," + value.Value.ToString("#.##") + "]," +
                        "PARAMETER[\"scale_factor\",0.9996]," +
                        "PARAMETER[\"false_easting\",500000]," +
                        "PARAMETER[\"false_northing\",0]," +
                        "UNIT[\"Meter\",1]]");

                var ctFact = new CoordinateTransformationFactory();
                var trans = ctFact.CreateFromCoordinateSystems(GeographicCoordinateSystem.WGS84, utm);

                _mathTransform = trans.MathTransform;
            }
        }

        private static bool IsConditionSatisfied(BinaryPackageReader reader, AimClassInfo classInfo, int[] propIndexes, int propIdexStart, OperationChoice operChoice)
        {
            if (operChoice.Choice == OperationChoiceType.Logic)
            {
                var blo = operChoice.LogicOps as BinaryLogicOp;

                foreach (var boOperChoice in blo.OperationList)
                {
                    var currPos = reader.CurrentPosition();
                    var b = IsConditionSatisfied(reader, classInfo, propIndexes, propIdexStart, boOperChoice);
                    reader.Seek(currPos, SeekOrigin.Begin);
                    _filterPrjGeom = null;

                    if (blo.Type == BinaryLogicOpType.And)
                    {
                        if (!b)
                            return false;
                    }
                    else
                    {
                        if (b)
                            return true;
                    }
                }

                return (blo.Type == BinaryLogicOpType.And);
            }


            #region Project Spatial filter geometry

            if (_filterPrjGeom == null && operChoice.Choice == OperationChoiceType.Spatial)
            {
                if (operChoice.SpatialOps is DWithin)
                {
                    var dWithin = operChoice.SpatialOps as DWithin;
                    _filterOriginalGeom = dWithin.Point;
                    _filterPrjGeom = ProjectGeom(dWithin.Point) as JtsGeom.IPoint;
                    _dWithinDistanceInMeter = Converters.ConverterToSI.Convert(dWithin.Distance, dWithin.Distance.Value);
                }
                else if (operChoice.SpatialOps is Within)
                {
                    var within = operChoice.SpatialOps as Within;
                    _filterOriginalGeom = within.Geometry;
                    _filterPrjGeom = ProjectGeom(within.Geometry);
                }
            }

            #endregion


            if (propIndexes == null)
            {
                propIndexes = GetSearchPropInfos(classInfo.Index, operChoice.PropertyName);
                propIdexStart = 0;
            }
            else if (propIdexStart >= propIndexes.Length)
            {
                return false;
            }

            var propIndex = propIndexes[propIdexStart];
            reader.Seek(propIndex * sizeof(Int32), SeekOrigin.Current);
            var startIndex = reader.GetInt32();
            var endIndex = reader.GetInt32();

            //*** Is null...
            if (startIndex == endIndex)
                return false;

            reader.Seek(startIndex, SeekOrigin.Begin);

            var propInfo = classInfo.Properties[propIndex];

            switch (propInfo.PropType.AimObjectType)
            {
                case AimObjectType.Field:
                    {
                        if (operChoice.Choice == OperationChoiceType.Spatial)
                            return IsSpacialConditionSatisfied(reader, propInfo.PropType, operChoice.SpatialOps);
                        else
                            return IsFieldConditionSatisfied(reader, propInfo.PropType, operChoice.ComparisonOps.OperationType, operChoice.ComparisonOps.Value);
                    }
                case AimObjectType.DataType:
                    {
                        if (operChoice.Choice != OperationChoiceType.Comparison)
                            return false;

                        return IsDataTypeConditionSatisfied(reader, propInfo.PropType, operChoice.ComparisonOps.OperationType, operChoice.ComparisonOps.Value);
                    }
                case AimObjectType.Object:
                    {
                        if (propInfo.IsList)
                            return IsObjectListSatisfied(reader, propInfo.PropType, propIndexes, propIdexStart + 1, operChoice);
                        else if (propInfo.PropType.SubClassType == AimSubClassType.Choice)
                            return IsChoiceSatisfied(reader, propInfo.PropType, propIndexes, propIdexStart + 1, operChoice);
                        else
                            return IsConditionSatisfied(reader, propInfo.PropType, propIndexes, propIdexStart + 1, operChoice);
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        private static bool IsFieldConditionSatisfied(BinaryPackageReader reader, AimClassInfo classInfo, ComparisonOpType operType, object value)
        {
            var fieldType = (AimFieldType)classInfo.Index;
            object fieldValue = null;

            if (classInfo.SubClassType == AimSubClassType.Enum)
                fieldType = AimFieldType.SysInt32;

            switch (fieldType)
            {
                case AimFieldType.SysBool:
                    fieldValue = reader.GetBool();
                    break;
                case AimFieldType.SysDateTime:
                    fieldValue = reader.GetDateTime();
                    break;
                case AimFieldType.SysDouble:
                    fieldValue = reader.GetDouble();
                    break;
                case AimFieldType.SysGuid:
                    fieldValue = new Guid(reader.GetString());
                    break;
                case AimFieldType.SysEnum:
                case AimFieldType.SysInt32:
                    fieldValue = reader.GetInt32();
                    break;
                case AimFieldType.SysInt64:
                    fieldValue = reader.GetInt64();
                    break;
                case AimFieldType.SysString:
                    fieldValue = reader.GetString();
                    break;
                case AimFieldType.SysUInt32:
                    fieldValue = reader.GetUInt32();
                    break;
                default:
                    return false;
            }

            return IsConditionSatisfied(fieldType, fieldValue, operType, value);
        }

        private static bool IsSpacialConditionSatisfied(BinaryPackageReader reader, AimClassInfo classInfo, SpatialOps spatialOps)
        {
            var fieldType = (AimFieldType)classInfo.Index;

            var aimFieldValue = AimObjectFactory.CreateAimField((AimFieldType)classInfo.Index) as IEditAimField;
            var aranGeom = aimFieldValue.FieldValue as AranGeom.Geometry;
            aranGeom.Unpack(reader);

            if (spatialOps is DWithin)
            {
                var jtsGeom = ProjectGeom(aranGeom);
                
                var dist = jtsGeom.Distance(_filterPrjGeom);

                return (dist <= _dWithinDistanceInMeter);
            }
            else if (spatialOps is Within)
            {
                if (!aranGeom.Extend.IsIntersected(_filterOriginalGeom.Extend))
                    return false;

                var jtsGeom = ProjectGeom(aranGeom);
                return jtsGeom.Intersects(_filterPrjGeom);
            }

            return false;
        }

        private static bool IsDataTypeConditionSatisfied(BinaryPackageReader reader, AimClassInfo classInfo, ComparisonOpType operType, object value)
        {
            var dataType = (DataType)classInfo.Index;

            if (dataType == DataType.TextNote)
            {
                #region TextNote

                if (operType != ComparisonOpType.EqualTo && operType != ComparisonOpType.NotEqualTo && !(value is object[]))
                    return false;

                var lang = (Aran.Aim.Enums.language)reader.GetInt32();
                var text = reader.GetString();

                var objArr = value as object[];
                var isEqual = lang.Equals(objArr[0]) && text.Equals(objArr[1].ToString(), StringComparison.InvariantCultureIgnoreCase);

                if (operType == ComparisonOpType.EqualTo)
                    return isEqual;
                else
                    return !isEqual;

                #endregion
            }
            else if (dataType == DataType.FeatureRef)
            {
                var val = new Guid(reader.GetString());
                return IsConditionSatisfied(AimFieldType.SysGuid, val, operType, value);
            }
            else if (classInfo.SubClassType == AimSubClassType.ValClass)
            {
                var valClassVal = AimObjectFactory.CreateADataType((DataType)classInfo.Index) as IEditValClass;
                valClassVal.Value = reader.GetDouble();
                valClassVal.Uom = reader.GetInt32();

                var siVal = Aran.Converters.ConverterToSI.Convert(valClassVal, double.NaN);

                return IsConditionSatisfied(AimFieldType.SysDouble, siVal, operType, value);
            }
            else if (classInfo.SubClassType == AimSubClassType.AbstractFeatureRef)
            {
                var identifier = new Guid(reader.GetString());
                var featureTypeIndex = reader.GetInt32();

                var absFeatRef = value as IAbstractFeatureRef;
                if (absFeatRef.FeatureTypeIndex != featureTypeIndex)
                    return false;

                return IsConditionSatisfied(AimFieldType.SysGuid, identifier, operType, absFeatRef.Identifier);
            }

            return false;
        }

        private static bool IsChoiceSatisfied(BinaryPackageReader reader, AimClassInfo classInfo, int[] propIndexes, int propIdexStart, OperationChoice operChoice)
        {
            var propIndex = propIndexes[propIdexStart];
            var propInfo = classInfo.Properties[propIndex];
            var refType = reader.GetInt32();

            if (refType != propInfo.TypeIndex)
                return false;

            int refValueAimTypeIndex = reader.GetInt32();
            return IsConditionSatisfied(reader, propInfo.PropType, propIndexes, propIdexStart + 1, operChoice);
        }

        private static bool IsObjectListSatisfied(BinaryPackageReader reader, AimClassInfo classInfo, int[] propIndexes, int propIdexStart, OperationChoice operChoice)
        {
            var count = reader.GetInt32();
            int[] startPosArr = new int[count];

            for (int i = 0; i < count; i++)
                startPosArr[i] = reader.GetInt32();

            for (int i = 0; i < count; i++)
            {
                var pos = startPosArr[i];
                reader.Seek(pos, SeekOrigin.Begin);

                var itemTypeIndex = reader.GetInt32();

                if (classInfo.Index == (int)ObjectType.FeatureRefObject)
                {
                    Array.Resize<int>(ref propIndexes, propIndexes.Length + 1);
                    propIndexes[propIndexes.Length - 1] = 1;
                }

                var isSatisfied = IsConditionSatisfied(reader, classInfo, propIndexes, propIdexStart, operChoice);

                if (isSatisfied)
                    return true;
            }

            return false;
        }

        private static bool IsConditionSatisfied(AimFieldType fieldType, object fieldValue, ComparisonOpType operType, object filterValue)
        {
            switch (fieldType)
            {
                case AimFieldType.SysBool:
                    #region Bool
                    {
                        var fVal = (bool)filterValue;
                        var val = (bool)fieldValue;

                        switch (operType)
                        {
                            case ComparisonOpType.EqualTo:
                                return (val == fVal);
                            case ComparisonOpType.NotEqualTo:
                                return (val != fVal);
                            default:
                                return false;
                        }
                    }
                    #endregion
                case AimFieldType.SysDateTime:
                    #region DateTime
                    {
                        var fVal = (DateTime)filterValue;
                        var val = (DateTime)fieldValue;

                        switch (operType)
                        {
                            case ComparisonOpType.EqualTo:
                                return (val == fVal);
                            case ComparisonOpType.GreaterThan:
                                return (val > fVal);
                            case ComparisonOpType.GreaterThanOrEqualTo:
                                return (val >= fVal);
                            case ComparisonOpType.LessThan:
                                return (val < fVal);
                            case ComparisonOpType.LessThanOrEqualTo:
                                return (val <= fVal);
                            case ComparisonOpType.NotEqualTo:
                                return (val != fVal);
                            default:
                                return false;
                        }
                    }
                    #endregion
                case AimFieldType.SysDouble:
                    #region Double
                    {
                        var fVal = Convert.ToDouble(filterValue);
                        var val = (double)fieldValue;

                        switch (operType)
                        {
                            case ComparisonOpType.EqualTo:
                                return (val == fVal);
                            case ComparisonOpType.GreaterThan:
                                return (val > fVal);
                            case ComparisonOpType.GreaterThanOrEqualTo:
                                return (val >= fVal);
                            case ComparisonOpType.LessThan:
                                return (val < fVal);
                            case ComparisonOpType.LessThanOrEqualTo:
                                return (val <= fVal);
                            case ComparisonOpType.NotEqualTo:
                                return (val != fVal);
                            default:
                                return false;
                        }
                    }
                    #endregion
                case AimFieldType.SysGuid:
                    #region Guid
                    {
                        var val = (Guid)fieldValue;

                        if (operType == ComparisonOpType.In)
                        {
                            var guids = filterValue as IEnumerable<Guid>;
                            foreach (var guid in guids)
                            {
                                if (val == guid)
                                    return true;
                            }
                            return false;
                        }

                        var fVal = (Guid)filterValue;

                        switch (operType)
                        {
                            case ComparisonOpType.EqualTo:
                                return (val == fVal);
                            case ComparisonOpType.NotEqualTo:
                                return (val != fVal);
                            default:
                                return false;
                        }
                    }
                    #endregion
                case AimFieldType.SysEnum:
                    #region Enum
                    {
                        var fVal = (int)filterValue;
                        var val = (int)fieldValue;

                        switch (operType)
                        {
                            case ComparisonOpType.EqualTo:
                                return (val == fVal);
                            case ComparisonOpType.NotEqualTo:
                                return (val != fVal);
                            default:
                                return false;
                        }
                    }
                    #endregion
                case AimFieldType.SysInt32:
                    #region Int32
                    {
                        var fVal = Convert.ToInt32(filterValue);
                        var val = (int)fieldValue;

                        switch (operType)
                        {
                            case ComparisonOpType.EqualTo:
                                return (val == fVal);
                            case ComparisonOpType.GreaterThan:
                                return (val > fVal);
                            case ComparisonOpType.GreaterThanOrEqualTo:
                                return (val >= fVal);
                            case ComparisonOpType.LessThan:
                                return (val < fVal);
                            case ComparisonOpType.LessThanOrEqualTo:
                                return (val <= fVal);
                            case ComparisonOpType.NotEqualTo:
                                return (val != fVal);
                            default:
                                return false;
                        }
                    }
                    #endregion
                case AimFieldType.SysInt64:
                    #region Int64
                    {
                        var fVal = Convert.ToInt64(filterValue);
                        var val = (long)fieldValue;

                        switch (operType)
                        {
                            case ComparisonOpType.EqualTo:
                                return (val == fVal);
                            case ComparisonOpType.GreaterThan:
                                return (val > fVal);
                            case ComparisonOpType.GreaterThanOrEqualTo:
                                return (val >= fVal);
                            case ComparisonOpType.LessThan:
                                return (val < fVal);
                            case ComparisonOpType.LessThanOrEqualTo:
                                return (val <= fVal);
                            case ComparisonOpType.NotEqualTo:
                                return (val != fVal);
                            default:
                                return false;
                        }
                    }
                    #endregion
                case AimFieldType.SysString:
                    #region String
                    {
                        var fVal = (string)filterValue;
                        var val = (string)fieldValue;

                        switch (operType)
                        {
                            case ComparisonOpType.EqualTo:
                                return val.Equals(fVal, StringComparison.InvariantCultureIgnoreCase);
                            case ComparisonOpType.GreaterThan:
                                return (val.CompareTo(fVal) > 0);
                            case ComparisonOpType.GreaterThanOrEqualTo:
                                return (val.CompareTo(fVal) >= 0);
                            case ComparisonOpType.LessThan:
                                return (val.CompareTo(fVal) < 0);
                            case ComparisonOpType.LessThanOrEqualTo:
                                return (val.CompareTo(fVal) <= 0);
                            case ComparisonOpType.Like:
                                return val.StartsWith(fVal, StringComparison.InvariantCultureIgnoreCase);
                            case ComparisonOpType.NotEqualTo:
                                return !(val.Equals(fVal, StringComparison.InvariantCultureIgnoreCase));
                            case ComparisonOpType.NotLike:
                                return !(val.StartsWith(fVal, StringComparison.InvariantCultureIgnoreCase));
                            default:
                                return false;
                        }
                    }
                    #endregion
                case AimFieldType.SysUInt32:
                    #region UInt32
                    {
                        var fVal = Convert.ToUInt32(filterValue);
                        var val = (uint)fieldValue;

                        switch (operType)
                        {
                            case ComparisonOpType.EqualTo:
                                return (val == fVal);
                            case ComparisonOpType.GreaterThan:
                                return (val > fVal);
                            case ComparisonOpType.GreaterThanOrEqualTo:
                                return (val >= fVal);
                            case ComparisonOpType.LessThan:
                                return (val < fVal);
                            case ComparisonOpType.LessThanOrEqualTo:
                                return (val <= fVal);
                            case ComparisonOpType.NotEqualTo:
                                return (val != fVal);
                            default:
                                return false;
                        }
                    }
                    #endregion
                default:
                    return false;
            }
        }

        #endregion

        public static int[] GetSearchPropInfos(int typeIndex, string propName)
        {
            var propInfos = Aran.Aim.Utilities.AimMetadataUtility.GetInnerProps(typeIndex, propName);
            var list = new List<int>();
            var classInfo = AimMetadata.GetClassInfoByIndex(typeIndex);

            foreach (var propInfo in propInfos)
            {
                list.Add(classInfo.Properties.IndexOf(propInfo));
                classInfo = propInfo.PropType;
            }

            return list.ToArray();
        }

        private static void ChangeValClassValToSi(OperationChoice operChoice)
        {
            if (operChoice.Choice == OperationChoiceType.Comparison)
                ChangeValClassValToSi(operChoice.ComparisonOps);
            else if (operChoice.Choice == OperationChoiceType.Logic)
            {
                var blo = operChoice.LogicOps as BinaryLogicOp;
                foreach (var oc in blo.OperationList)
                    ChangeValClassValToSi(oc);
            }
        }

        private static void ChangeValClassValToSi(ComparisonOps compOp)
        {
            if (compOp.Value != null && compOp.Value is IEditValClass)
            {
                var siVal = Aran.Converters.ConverterToSI.Convert(compOp.Value as IEditValClass, double.NaN);
                compOp.Value = siVal;
            }
        }

        private static OperationChoice CloneFilter(OperationChoice operChoice)
        {
            byte[] data = null;
            using (var bpw = new BinaryPackageWriter())
            {
                operChoice.Pack(bpw);
                data = bpw.ToArray();
            }

            using (var bpr = new BinaryPackageReader(data))
            {
                return OperationChoice.UnpackOperationChoice(bpr);
            }
        }

        private static JtsGeom.IGeometry ProjectGeom(AranGeom.Geometry aranGeom)
        {
            var jtsGeom = ConvertToJtsGeo.FromGeometry(aranGeom);
            var prjJtsGeom = GeometryTransform.TransformGeometry(NtsGeom.GeometryFactory.Default, jtsGeom, _mathTransform);
            return prjJtsGeom;
        }


        private static AranGeom.Geometry _filterOriginalGeom = null;
        private static JtsGeom.IGeometry _filterPrjGeom = null;
        private static double _dWithinDistanceInMeter = double.NaN;
        private static IMathTransform _mathTransform = null;
        private static double? _centralMeridian = null;
    }
}
