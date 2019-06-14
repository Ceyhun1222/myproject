using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Common.Aim.Extension.Message;
using Aran.Temporality.Common.Aim.Extension.Property;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver.GeoJsonObjectModel;
using TimeSlice = Aran.Temporality.Common.MetaData.TimeSlice;

namespace Aran.Temporality.Common.Mongo.Serializer
{
    class AimEventSerializer : SerializerBase<AimEvent>
    {
        private static int GetIndex(string name)
        {
            if (name[0] != 'p') return -1;

            int sum = 0;
            int i = 1;
            int length = name.Length;

            while (i < length && char.IsNumber(name[i]))
            {
                sum = sum * 10 + (int) char.GetNumericValue(name[i]);
                i++;
            }

            return sum;

        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, AimEvent value)
        {
            context.Writer.WriteStartDocument();

            SerializeMetaData(value, context.Writer);

            SerializeData(value?.Data, context.Writer);

            context.Writer.WriteEndDocument();
        }

        private void SerializeMetaData(AimEvent value, IBsonWriter writer)
        {
            if (value == null)
                return;

            writer.WriteStartDocument("p1_MetaData");

            writer.WriteInt32("p1_FeatureTypeId", value.FeatureTypeId);

            writer.WriteName("p2_Guid");
            BsonSerializer.Serialize(writer, value.Guid);

            writer.WriteInt32("p3_Interpretation", (int) value.Interpretation);

            writer.WriteName("p4_ValidTimeBegin");
            BsonSerializer.Serialize(writer, value.TimeSlice?.BeginPosition);
            writer.WriteName("p5_ValidTimeEnd");
            BsonSerializer.Serialize(writer, value.TimeSlice?.EndPosition);

            if (value.Version?.SequenceNumber != null)
                writer.WriteInt32("p6_SequenceNumber", value.Version.SequenceNumber);
            if (value.Version?.CorrectionNumber != null)
                writer.WriteInt32("p7_CorrectionNumber", value.Version.CorrectionNumber);


            writer.WriteName("p8_LifeTimeBegin");
            BsonSerializer.Serialize(writer, value.LifeTimeBegin);
            writer.WriteName("p9_LifeTimeEnd");
            BsonSerializer.Serialize(writer, value.LifeTimeEnd);

            writer.WriteName("p10_SubmitDate");
            BsonSerializer.Serialize(writer, value.SubmitDate);

            writer.WriteInt32("p11_WorkPackage", value.WorkPackage);
            writer.WriteString("p12_User", value.User ?? "");
            writer.WriteBoolean("p13_IsCanceled", value.IsCanceled);
            writer.WriteBoolean("p14_ApplyTimeSliceToFeatureLifeTime", value.ApplyTimeSliceToFeatureLifeTime);
            writer.WriteBoolean("p15_IsCreatedInWorkPackage", value.IsCreatedInWorkPackage);

            writer.WriteEndDocument();
        }

        private void SerializeData(AimFeature value, IBsonWriter writer)
        {
            if (value == null)
                return;

            if (value.Feature != null)
            {
                writer.WriteName("p2_Feature");
                BsonSerializer.Serialize(writer, value.Feature);
            }

            if (value.MessageExtensions.Count > 0)
            {
                writer.WriteStartArray("p3_MessageExtensions");
                foreach (var extension in value.MessageExtensions)
                    BsonSerializer.Serialize(writer, extension);
                writer.WriteEndArray();
            }

            if (value.PropertyExtensions.Count > 0)
            {
                writer.WriteStartArray("p4_PropertyExtensions");
                foreach (var extension in value.PropertyExtensions)
                    BsonSerializer.Serialize(writer, extension);
                writer.WriteEndArray();
            }
        }

        public override AimEvent Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var reader = context.Reader;

            reader.ReadStartDocument();

            var value = new AimEvent
            {
                TimeSlice = new TimeSlice(),
                Version = new TimeSliceVersion(),
                Data = new AimFeature()
            };

            while (reader.State != BsonReaderState.EndOfDocument && (reader.State != BsonReaderState.Type || reader.ReadBsonType() != BsonType.EndOfDocument))
            {
                var name = reader.ReadName();
                var index = GetIndex(name);
                switch (index)
                {
                    case 1:
                        DeserializeMetaData(value, reader);
                        break;
                    case 2:
                        value.Data.Feature = BsonSerializer.Deserialize<Feature>(reader);
                        break;
                    case 3:
                    {
                        reader.ReadStartArray();
                        while (reader.State != BsonReaderState.EndOfArray && (reader.State != BsonReaderState.Type || reader.ReadBsonType() != BsonType.EndOfDocument))
                        {
                            var extension = BsonSerializer.Deserialize<MessageExtension>(reader);
                            value.Data.MessageExtensions.Add(extension);
                        }
                        reader.ReadEndArray();
                        break;
                    }
                    case 4:
                    {
                        reader.ReadStartArray();
                        while (reader.State != BsonReaderState.EndOfArray && (reader.State != BsonReaderState.Type || reader.ReadBsonType() != BsonType.EndOfDocument))
                        {
                            var extension = BsonSerializer.Deserialize<PropertyExtension>(reader);
                            value.Data.PropertyExtensions.Add(extension);
                        }
                        reader.ReadEndArray();
                        break;
                    }
                    default:
                        reader.SkipValue();
                        break;
                }
            }

            reader.ReadEndDocument();

            if (value.Data == null)
                return value;

            typeof(AimFeature).GetProperty("FeatureType")?.SetValue(value.Data, (FeatureType) value.FeatureTypeId);
            value.Data.Identifier = value.Guid ?? Guid.Empty;

            if (value.Data.Feature != null)
            {
                value.Data.Feature.TimeSlice = new Aran.Aim.DataTypes.TimeSlice
                {
                    ValidTime = new TimePeriod
                    {
                        BeginPosition = value.TimeSlice.BeginPosition,
                        EndPosition = value.TimeSlice.EndPosition
                    },
                    SequenceNumber = value.Version.SequenceNumber,
                    CorrectionNumber = value.Version.CorrectionNumber,
                    Interpretation = ConvertInterpretation(value.Interpretation),
                    FeatureLifetime = new TimePeriod
                    {
                        BeginPosition = value.LifeTimeBegin ?? default(DateTime),
                        EndPosition = value.LifeTimeEnd
                    }
                };

            }

            return value;
        }

        private TimeSliceInterpretationType ConvertInterpretation(Interpretation interpretation)
        {
            switch (interpretation)
            {
                case Interpretation.TempDelta:
                    return TimeSliceInterpretationType.TEMPDELTA;
                case Interpretation.BaseLine:
                    return TimeSliceInterpretationType.BASELINE;
                case Interpretation.Snapshot:
                    return TimeSliceInterpretationType.SNAPSHOT;
                default:
                    return TimeSliceInterpretationType.PERMDELTA;
            }
        }

        public void DeserializeMetaData(AimEvent value, IBsonReader reader)
        {
            reader.ReadStartDocument();

            while (reader.State != BsonReaderState.EndOfDocument && (reader.State != BsonReaderState.Type || reader.ReadBsonType() != BsonType.EndOfDocument))
            {
                var name = reader.ReadName();
                var index = GetIndex(name);

                if (index == 1)
                    value.FeatureTypeId = reader.ReadInt32();
                else if (index == 2)
                    value.Guid = BsonSerializer.Deserialize<Guid>(reader);
                else if (index == 3)
                    value.Interpretation = (Interpretation) reader.ReadInt32();
                else if (index == 4)
                    value.TimeSlice.BeginPosition = BsonSerializer.Deserialize<DateTime>(reader);
                else if (index == 5)
                    value.TimeSlice.EndPosition = BsonSerializer.Deserialize<DateTime?>(reader);
                else if (index == 6)
                    value.Version.SequenceNumber = reader.ReadInt32();
                else if (index == 7)
                    value.Version.CorrectionNumber = reader.ReadInt32();
                else if (index == 8)
                    value.LifeTimeBegin = BsonSerializer.Deserialize<DateTime?>(reader);
                else if (index == 9)
                    value.LifeTimeEnd = BsonSerializer.Deserialize<DateTime?>(reader);
                else if (index == 10)
                    value.SubmitDate = BsonSerializer.Deserialize<DateTime>(reader);
                else if (index == 11)
                    value.WorkPackage = reader.ReadInt32();
                else if (index == 12)
                    value.User = reader.ReadString();
                else if (index == 13)
                    value.IsCanceled = reader.ReadBoolean();
                else if (index == 14)
                    value.ApplyTimeSliceToFeatureLifeTime = reader.ReadBoolean();
                else if (index == 15)
                    value.IsCreatedInWorkPackage = reader.ReadBoolean();
                else
                    reader.SkipValue();
            }

            reader.ReadEndDocument();
        }

        private void DeserializeData(AimFeature value, IBsonReader reader)
        {
            reader.ReadStartDocument();

            while (reader.State != BsonReaderState.EndOfDocument && (reader.State != BsonReaderState.Type || reader.ReadBsonType() != BsonType.EndOfDocument))
            {
                var name = reader.ReadName();

                switch (name.FirstOrDefault())
                {
                    case 'F':
                        value.Feature = BsonSerializer.Deserialize<Feature>(reader);
                        break;
                    case 'M':
                    {
                        reader.ReadStartArray();
                        while (reader.State != BsonReaderState.EndOfArray && (reader.State != BsonReaderState.Type || reader.ReadBsonType() != BsonType.EndOfDocument))
                        {
                            var extension = BsonSerializer.Deserialize<MessageExtension>(reader);
                            value.MessageExtensions.Add(extension);
                        }
                        reader.ReadEndArray();
                        break;
                    }
                    case 'P':
                    {
                        reader.ReadStartArray();
                        while (reader.State != BsonReaderState.EndOfArray && (reader.State != BsonReaderState.Type || reader.ReadBsonType() != BsonType.EndOfDocument))
                        {
                            var extension = BsonSerializer.Deserialize<PropertyExtension>(reader);
                            value.PropertyExtensions.Add(extension);
                        }
                        reader.ReadEndArray();
                            break;
                    }
                    default:
                        reader.SkipValue();
                        break;
                }
            }

            reader.ReadEndDocument();
        }
    }
}
