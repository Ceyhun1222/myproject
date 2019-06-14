using System;
using System.Collections;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Geometries;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Aran.Temporality.Common.Mongo.Serializer
{
    internal class FeatureSerializer<T> : SerializerBase<T> where T : Feature, new()
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, T value)
        {
            FeatureSerializer.SerializeAimObject(value, context.Writer, true);
        }

        public override T Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return FeatureSerializer.DeserializeFeature(context.Reader) as T;
        }
    }

    internal static class FeatureSerializer
    {
        internal const string FeatureTypeField = "_t";
        internal const string AimTypeIndexField = "ai";
        internal const string NilReasonsField = "nr";

        internal static void SerializeAimObject(IAimObject value, IBsonWriter writer, bool isAbstract = false)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex(value);

            writer.WriteStartDocument();

            if (value is Feature)
            {
                writer.WriteName(FeatureTypeField);
                writer.WriteString(AimMetadata.GetAimTypeName(value) ?? "");
            }

            if (isAbstract)
            {
                writer.WriteName(AimTypeIndexField);
                writer.WriteInt32(AimMetadata.GetAimTypeIndex(value));
            }

            if (value is Feature feature)
            {
                SerializeNilReasons(feature, classInfo, writer);
            }

            if (value != null)
            {
                for (int i = 0; i < classInfo.Properties.Count; i++)
                {
                    var propInfo = classInfo.Properties[i];
                    var propValue = value.GetValue(propInfo.Index);
                    if (propValue == null || (propValue.PropertyType == AimPropertyType.List &&
                                              ((IList)propValue).Count == 0))
                        continue;

                    // For TimeSlice
                    if (value is Feature && i == 2)
                        continue;

                    writer.WriteName(GetPropertyName(i, propInfo.Name));
                    SerializeProperty(propValue, propInfo, writer);
                }
            }

            writer.WriteEndDocument();
        }

        private static void SerializeProperty(IAimProperty propValue, AimPropInfo propInfo, IBsonWriter writer)
        {
            if (propInfo.PropType.AimObjectType == AimObjectType.Field)
                SerializeAimField(propValue as AimField, writer);
            else if (propInfo.PropType.SubClassType == AimSubClassType.ValClass)
                SerializeValClass(propValue as IEditValClass, writer);
            else if (propInfo.IsList)
                SerializeList(propValue as IEnumerable, writer, propInfo.PropType.IsAbstract);
            else if (propInfo.IsFeatureReference)
                if (propValue is AbstractFeatureRefBase abstractFeatureRef)
                    SerializeAbstractFeatureRef(abstractFeatureRef, writer);
                else
                    SerializeFeatureRef(propValue as FeatureRef, writer);
            else if (propInfo.PropType.SubClassType == AimSubClassType.Choice)
                SerializeChoiceObject(propValue as ChoiceClass, writer);
            else
                SerializeAimObject(propValue as IAimObject, writer, propInfo.PropType.IsAbstract);
        }

        public static Feature DeserializeFeature(IBsonReader reader)
        {
            reader.ReadStartDocument();

            if (reader.ReadName() != FeatureTypeField)
                throw new Exception("Missing a required field: _t");
            reader.SkipValue();

            if (reader.ReadName() != AimTypeIndexField)
                throw new Exception("Missing a required field: ai");

            var ai = reader.ReadInt32();
            var feature = AimObjectFactory.Create(ai) as Feature;

            if (reader.ReadName() != NilReasonsField)
                throw new Exception("Missing a required field: NilReasons");
            DeserializeNilReason(feature, reader);

            DeserializeAimObject(feature, reader);

            reader.ReadEndDocument();

            return feature;
        }

        internal static void DeserializeAimObject(IAimObject value, IBsonReader reader)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex(value);

            while (reader.ReadBsonType() != BsonType.EndOfDocument)
            {
                var name = reader.ReadName();

                var index = GetIndex(name);

                if (index == -1)
                {
                    reader.SkipValue();
                    continue;
                }

                AimPropInfo propInfo = classInfo.Properties[index];

                var property = DeserializeProperty(propInfo, reader);

                value.SetValue(propInfo.Index, property);
            }
        }

        private static IAimProperty DeserializeProperty(AimPropInfo propInfo, IBsonReader reader)
        {
            if (propInfo.PropType.AimObjectType == AimObjectType.Field)
            {
                var fieldValue = AimObjectFactory.Create(propInfo.TypeIndex);
                DeserializeAimField(fieldValue as AimField, reader);
                return fieldValue as IAimProperty;
            }

            if (propInfo.PropType.SubClassType == AimSubClassType.ValClass)
            {
                var valClassValue = AimObjectFactory.Create(propInfo.TypeIndex);
                DeserializeValClass(valClassValue as IEditValClass, reader);
                return valClassValue as IAimProperty;
            }

            if (propInfo.IsList)
            {
                IList list = AimObjectFactory.CreateList(propInfo.TypeIndex);
                DeserializeList(list, reader, propInfo);
                return list as IAimProperty;
            }

            if (propInfo.IsFeatureReference)
            {
                var featureRef = AimObjectFactory.Create(propInfo.TypeIndex);
                if (featureRef is AbstractFeatureRefBase abstractFeatureRef)
                    DeserializeAbstractFeatureRef(abstractFeatureRef, reader);
                else
                    DeserializeFeatureRef(featureRef as FeatureRef, reader);
                return featureRef as IAimProperty;
            }

            if (propInfo.PropType.SubClassType == AimSubClassType.Choice)
            {
                var choiceObject = AimObjectFactory.Create(propInfo.TypeIndex);
                DeserializeChoice(choiceObject as ChoiceClass, reader);
                return choiceObject as IAimProperty;
            }

            if (!propInfo.PropType.IsAbstract)
            {
                var aimObject = AimObjectFactory.Create(propInfo.TypeIndex);
                reader.ReadStartDocument();
                DeserializeAimObject(aimObject, reader);
                reader.ReadEndDocument();
                return aimObject as IAimProperty;
            }

            return DeserializeAbstractAimObject(reader) as IAimProperty;
        }

        #region Serializers

        private static void SerializeChoiceObject(ChoiceClass value, IBsonWriter writer)
        {
            writer.WriteStartDocument();

            var choiceClassInfo = AimMetadata.GetClassInfoByIndex(value);
            var propInfo = Aran.Aim.Utilities.AimMetadataUtility.GetChoicePropInfo(value);

            int index = -1;
            for (int i = 0; i < choiceClassInfo.Properties.Count; i++)
                if (choiceClassInfo.Properties[i].Index == propInfo.Index)
                {
                    index = i;
                    break;
                }

            writer.WriteName(GetPropertyName(index, propInfo.Name));

            SerializeProperty((value as IEditChoiceClass).RefValue, propInfo, writer);

            writer.WriteEndDocument();
        }

        private static void SerializeList(IEnumerable list, IBsonWriter writer, bool isAbstract)
        {
            writer.WriteStartArray();

            foreach (var obj in list)
            {
                if (obj is ChoiceClass choiceClass)
                    SerializeChoiceObject(choiceClass, writer);
                else if (obj is FeatureRef featureRef)
                    SerializeFeatureRef(featureRef, writer);
                else if (obj is IAbstractFeatureRef abstractFeatureRef)
                    SerializeAbstractFeatureRef(abstractFeatureRef, writer);
                else
                    SerializeAimObject(obj as IAimObject, writer, isAbstract);
            }

            writer.WriteEndArray();
        }

        private static void SerializeAimField(AimField value, IBsonWriter writer)
        {
            var ev = value as IEditAimField;

            switch (value.FieldType)
            {
                case AimFieldType.SysBool:
                    writer.WriteBoolean((bool)ev.FieldValue);
                    break;
                case AimFieldType.SysDateTime:
                    BsonSerializer.Serialize(writer, (DateTime)ev.FieldValue);
                    break;
                case AimFieldType.SysDouble:
                    writer.WriteDouble((Double)ev.FieldValue);
                    break;
                case AimFieldType.SysGuid:
                    BsonSerializer.Serialize(writer, (Guid)ev.FieldValue);
                    break;
                case AimFieldType.SysEnum:
                case AimFieldType.SysInt32:
                    writer.WriteInt32((Int32)ev.FieldValue);
                    break;
                case AimFieldType.SysInt64:
                    writer.WriteInt64((Int64)ev.FieldValue);
                    break;
                case AimFieldType.SysString:
                    writer.WriteString(ev.FieldValue as string ?? "");
                    break;
                case AimFieldType.SysUInt32:
                    writer.WriteInt64((UInt32)ev.FieldValue);
                    break;
                case AimFieldType.GeoPoint:
                case AimFieldType.GeoPolyline:
                case AimFieldType.GeoPolygon:
                    SerializeGeometry(ev.FieldValue as Geometries.Geometry, writer);
                    break;
            }
        }

        private static void SerializeGeometry(Geometries.Geometry value, IBsonWriter writer)
        {
            switch (value.Type)
            {
                case GeometryType.MultiPolygon:
                    BsonSerializer.Serialize(writer, GeoJsonConverter.GetGeoJsonMultiPolygon(value as MultiPolygon));
                    break;
                case GeometryType.MultiLineString:
                    BsonSerializer.Serialize(writer, GeoJsonConverter.GetGeoJsonMultiLineString(value as MultiLineString));
                    break;
                case GeometryType.Point:
                    BsonSerializer.Serialize(writer, GeoJsonConverter.GetGeoJsonPoint(value as Point));
                    break;
                default:
                    writer.WriteNull();
                    break;
            }
        }

        private static void SerializeValClass(IEditValClass value, IBsonWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteDouble("p1_Value", value.Value);
            writer.WriteInt32("p2_Uom", value.Uom);
            writer.WriteDouble("p3_SI", Converters.ConverterToSI.Convert(value, 0));
            writer.WriteEndDocument();
        }

        private static void SerializeAbstractFeatureRef(IAbstractFeatureRef value, IBsonWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteInt64("p1_Id", value.Id);
            writer.WriteName("p2_Identifier");
            BsonSerializer.Serialize(writer, value.Identifier);
            writer.WriteInt32("p3_FeatureTypeIndex", value.FeatureTypeIndex);
            writer.WriteEndDocument();
        }

        private static void SerializeFeatureRef(FeatureRef value, IBsonWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteInt64("p1_Id", value.Id);
            writer.WriteName("p2_Identifier");
            BsonSerializer.Serialize(writer, value.Identifier);
            if (value.FeatureType != null)
                writer.WriteInt32("p3_FeatureType", (int)value.FeatureType);
            writer.WriteEndDocument();
        }

        private static void SerializeNilReasons(Feature feature, AimClassInfo classInfo, IBsonWriter writer)
        {
            writer.WriteName(NilReasonsField);
            writer.WriteStartDocument();
            foreach (var propInfo in classInfo.Properties)
            {
                if (feature.GetNilReason(propInfo.Index) is NilReason nilReason)
                {
                    writer.WriteInt32(propInfo.Index.ToString(), (int)nilReason);
                }
            }
            writer.WriteEndDocument();
        }

        #endregion

        #region Deserializers

        private static void DeserializeValClass(IEditValClass value, IBsonReader reader)
        {
            reader.ReadStartDocument();
            while (reader.ReadBsonType() != BsonType.EndOfDocument)
            {
                var name = reader.ReadName();
                var index = GetIndex(name);
                if (index == 1)
                    value.Value = reader.ReadDouble();
                else if (index == 2)
                    value.Uom = reader.ReadInt32();
                else
                    reader.SkipValue();
            }
            reader.ReadEndDocument();
        }

        private static void DeserializeChoice(ChoiceClass choiceObject, IBsonReader reader)
        {
            var choiceClassInfo = AimMetadata.GetClassInfoByIndex(choiceObject);

            reader.ReadStartDocument();

            if (reader.State == BsonReaderState.Type)
            {
                var t = reader.ReadBsonType();
                if (t == BsonType.EndOfDocument)
                {
                    reader.ReadEndDocument();
                    return;
                }
            }

            var choiceName = reader.ReadName();
            var choiceIndex = GetIndex(choiceName);

            var choicePropInfo = choiceClassInfo.Properties[choiceIndex];

            var editChoiceObject = choiceObject as IEditChoiceClass;
            if (choicePropInfo.IsFeatureReference)
                editChoiceObject.RefType = (int)choicePropInfo.ReferenceFeature;
            else
                editChoiceObject.RefType = choicePropInfo.TypeIndex;

            editChoiceObject.RefValue = DeserializeProperty(choicePropInfo, reader);

            reader.ReadEndDocument();
        }

        private static void DeserializeList(IList list, IBsonReader reader, AimPropInfo propInfo)
        {
            reader.ReadStartArray();

            while (reader.ReadBsonType() != BsonType.EndOfDocument)
            {
                if (propInfo.PropType.SubClassType == AimSubClassType.Choice)
                {
                    var choiceObject = AimObjectFactory.Create(propInfo.TypeIndex);
                    DeserializeChoice(choiceObject as ChoiceClass, reader);
                    list.Add(choiceObject);
                }
                else if (!propInfo.PropType.IsAbstract)
                {
                    var aimObject = AimObjectFactory.Create(propInfo.TypeIndex);
                    reader.ReadStartDocument();
                    DeserializeAimObject(aimObject, reader);
                    reader.ReadEndDocument();
                    list.Add(aimObject);
                }
                else
                {
                    list.Add(DeserializeAbstractAimObject(reader));
                }
            }

            reader.ReadEndArray();
        }

        private static void DeserializeAimField(AimField value, IBsonReader reader)
        {
            var editField = value as IEditAimField;
            switch (value.FieldType)
            {
                case AimFieldType.SysBool:
                    editField.FieldValue = reader.ReadBoolean();
                    break;
                case AimFieldType.SysDateTime:
                    editField.FieldValue = BsonSerializer.Deserialize<DateTime>(reader);
                    break;
                case AimFieldType.SysDouble:
                    editField.FieldValue = reader.ReadDouble();
                    break;
                case AimFieldType.SysGuid:
                    editField.FieldValue = BsonSerializer.Deserialize<Guid>(reader);
                    break;
                case AimFieldType.SysEnum:
                case AimFieldType.SysInt32:
                    editField.FieldValue = reader.ReadInt32();
                    break;
                case AimFieldType.SysInt64:
                    editField.FieldValue = reader.ReadInt64();
                    break;
                case AimFieldType.SysString:
                    editField.FieldValue = reader.ReadString();
                    break;
                case AimFieldType.SysUInt32:
                    editField.FieldValue = (UInt32)reader.ReadInt64();
                    break;
                case AimFieldType.GeoPoint:
                    editField.FieldValue = GeoJsonConverter.GetAranPoint(
                        BsonSerializer.Deserialize<GeoJsonPoint<GeoJson3DGeographicCoordinates>>(reader));
                    break;
                case AimFieldType.GeoPolyline:
                    var multiLine = GeoJsonConverter.GetAranMultiLineString(
                        BsonSerializer.Deserialize<GeoJsonMultiLineString<GeoJson3DGeographicCoordinates>>(reader));
                    if (multiLine != null)
                        editField.FieldValue = multiLine;
                    break;
                case AimFieldType.GeoPolygon:
                    var multiPolygon = GeoJsonConverter.GetAranMultiPolygon(
                        BsonSerializer.Deserialize<GeoJsonMultiPolygon<GeoJson3DGeographicCoordinates>>(reader));
                    if (multiPolygon != null)
                        editField.FieldValue = multiPolygon;
                    break;
            }
        }

        private static AimObject DeserializeAbstractAimObject(IBsonReader reader)
        {
            AimObject aimObject = null;

            reader.ReadStartDocument();

            var ai = -1;

            if (reader.ReadBsonType() != BsonType.EndOfDocument)
                ai = reader.ReadInt32(AimTypeIndexField);

            if (ai > 0)
            {
                aimObject = AimObjectFactory.Create(ai);
                DeserializeAimObject(aimObject, reader);
            }

            reader.ReadEndDocument();

            return aimObject;
        }

        private static void DeserializeAbstractFeatureRef(IAbstractFeatureRef value, IBsonReader reader)
        {
            reader.ReadStartDocument();
            while (reader.ReadBsonType() != BsonType.EndOfDocument)
            {
                var name = reader.ReadName();
                var index = GetIndex(name);
                if (index == 1)
                    value.Id = reader.ReadInt64();
                else if (index == 2)
                    value.Identifier = BsonSerializer.Deserialize<Guid>(reader);
                else if (index == 3)
                    value.FeatureTypeIndex = reader.ReadInt32();
                else
                    reader.SkipValue();
            }
            reader.ReadEndDocument();
        }

        private static void DeserializeFeatureRef(FeatureRef value, IBsonReader reader)
        {
            reader.ReadStartDocument();
            while (reader.ReadBsonType() != BsonType.EndOfDocument)
            {
                var name = reader.ReadName();
                var index = GetIndex(name);
                if (index == 1)
                    value.Id = reader.ReadInt64();
                else if (index == 2)
                    value.Identifier = BsonSerializer.Deserialize<Guid>(reader);
                else if (index == 3)
                    value.FeatureType = (FeatureType)reader.ReadInt32();
                else
                    reader.SkipValue();
            }
            reader.ReadEndDocument();
        }

        public static void DeserializeNilReason(Feature feature, IBsonReader reader)
        {
            reader.ReadStartDocument();

            while (reader.ReadBsonType() != BsonType.EndOfDocument)
            {
                if (int.TryParse(reader.ReadName(), out var propIndex))
                {
                    var nilReason = (NilReason)reader.ReadInt32();
                    feature?.SetNilReason(propIndex, nilReason);
                }
            }

            reader.ReadEndDocument();
        }

        #endregion

        #region Index methods

        private static int GetIndex(string name)
        {
            if (name[0] != 'p') return -1;

            int sum = 0;
            int i = 1;
            int length = name.Length;

            while (i < length && char.IsNumber(name[i]))
            {
                sum = sum * 10 + (int)char.GetNumericValue(name[i]);
                i++;
            }

            return sum;

        }

        private static string GetPropertyName(int index, string name = "")
        {
            return $"p{index}_{name}";
        }

        #endregion
    }
}
