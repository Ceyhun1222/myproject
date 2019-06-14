using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Mongo;
using Aran.Temporality.Internal.MetaData.Offset;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;

namespace Aran.Temporality.Common.TossConverter
{
    public enum MessageCauseType
    {
        SystemMessage,
        UnknownGeometryError,
        SelfIntersect,
        DuplicateVertices,
        IncorrectLatLon,
        Less3UniqueVertices,
        PolygonOpened,
        PolygonReverse,
        PolygonShort,
        CommonError,
        UnknownError,
        LineHasOneUniqueVertices,
        RingHasOneUniqueVertices,
    }

    class ConverterUtil
    {
        private static bool AimFieldPredicate(IAimObject aimObject, Func<AimField, bool> predicate)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex(aimObject);

            foreach (var propInfo in classInfo.Properties)
            {
                var propValue = aimObject.GetValue(propInfo.Index);

                if (propValue == null)
                    continue;

                if (propValue.PropertyType == AimPropertyType.AranField)
                {
                    if (propValue is AimField aimField)
                        if (predicate(aimField))
                            return true;
                }
                else if (propInfo.IsList)
                {
                    foreach (var obj in (IList)propValue)
                        if (obj is IAimObject aimObj)
                            if (AimFieldPredicate(aimObj, predicate))
                                return true;
                }
                else if (propValue.PropertyType == AimPropertyType.Object)
                {
                    if (AimFieldPredicate(propValue as IAimObject, predicate))
                        return true;
                }
            }

            return false;
        }

        private static void AimFieldAction(IAimObject aimObject, Action<AimField> action)
        {
            if (aimObject == null)
                return;

            var classInfo = AimMetadata.GetClassInfoByIndex(aimObject);

            foreach (var propInfo in classInfo.Properties)
            {
                var propValue = aimObject.GetValue(propInfo.Index);

                if (propValue == null)
                    continue;

                if (propValue.PropertyType == AimPropertyType.AranField)
                {
                    if (propValue is AimField aimField)
                        action(aimField);
                }
                else if (propInfo.IsList)
                {
                    foreach (var obj in (IList)propValue)
                        if (obj is IAimObject aimObj)
                            AimFieldAction(aimObj, action);
                }
                else if (propValue.PropertyType == AimPropertyType.Object || propValue.PropertyType == AimPropertyType.DataType)
                {
                    AimFieldAction(propValue as IAimObject, action);
                }
            }
        }

        private static bool HasOpened(IAimObject aimObject)
        {
            return AimFieldPredicate(aimObject, aimField =>
            {
                if (aimField.FieldType == AimFieldType.GeoPolygon)
                    if ((aimField as IEditAimField).FieldValue is MultiPolygon multiPolygon)
                        if (!multiPolygon.IsClose)
                            return true;
                return false;
            });
        }

        private static bool HasTM(IAimObject aimObject)
        {
            return AimFieldPredicate(aimObject, aimField =>
            {
                if ((aimField as IEditAimField).FieldValue is MultiPolygon multiPolygon)
                {
                    foreach (Polygon polygon in multiPolygon)
                    {
                        foreach (Point point in polygon.ExteriorRing)
                            if (point.T != 0.0 || point.M != 0.0)
                                return true;

                        foreach (var interior in polygon.InteriorRingList)
                            foreach (Point point in interior)
                                if (point.T != 0.0 || point.M != 0.0)
                                    return true;
                    }
                }
                else if ((aimField as IEditAimField).FieldValue is MultiLineString multiLineString)
                {
                    foreach (LineString lineString in multiLineString)
                        foreach (Point point in lineString)
                            if (point.T != 0.0 || point.M != 0.0)
                                return true;
                }
                else if ((aimField as IEditAimField).FieldValue is Point point)
                    if (point.T != 0.0 || point.M != 0.0)
                        return true;

                return false;
            });
        }

        private static bool HasShort(IAimObject aimObject)
        {
            return AimFieldPredicate(aimObject, aimField =>
            {
                if (aimField.FieldType == AimFieldType.GeoPolygon)
                    if ((aimField as IEditAimField).FieldValue is MultiPolygon multiPolygon)
                        foreach (Polygon polygon in multiPolygon)
                        {
                            if (polygon.ExteriorRing.Count < 4)
                                return true;
                            foreach (var interior in polygon.InteriorRingList)
                                if (interior.Count < 4)
                                    return true;
                        }
                return false;
            });
        }

        private static void Close(IAimObject aimObject)
        {
            AimFieldAction(aimObject, aimField =>
            {
                if (aimField.FieldType == AimFieldType.GeoPolygon)
                    if ((aimField as IEditAimField).FieldValue is MultiPolygon multiPolygon)
                        multiPolygon.Close();
            });
        }

        private static void RemoveTM(IAimObject aimObject)
        {
            AimFieldAction(aimObject, aimField =>
            {
                if ((aimField as IEditAimField).FieldValue is MultiPolygon multiPolygon)
                {
                    foreach (Polygon polygon in multiPolygon)
                    {
                        foreach (Point point in polygon.ExteriorRing)
                            point.T = point.M = 0.0;

                        foreach (var interior in polygon.InteriorRingList)
                            foreach (Point point in interior)
                                point.T = point.M = 0.0;
                    }
                }
                else if ((aimField as IEditAimField).FieldValue is MultiLineString multiLineString)
                {
                    foreach (LineString lineString in multiLineString)
                        foreach (Point point in lineString)
                            point.T = point.M = 0.0;
                }
                else if ((aimField as IEditAimField).FieldValue is Point point)
                    point.T = point.M = 0.0;
            });
        }

        private static double MultiPolygonSum(IAimObject aimObject, double sum = 0)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex(aimObject);

            foreach (var propInfo in classInfo.Properties)
            {
                var propValue = aimObject.GetValue(propInfo.Index);
                if (propValue == null)
                    continue;

                if (propValue.PropertyType == AimPropertyType.AranField)
                {
                    if (propValue is AimField aimField)
                    {
                        if (aimField.FieldType == AimFieldType.GeoPolygon)
                        {
                            if ((propValue as IEditAimField).FieldValue is MultiPolygon multiPolygon)
                            {
                                foreach (Polygon polygon in multiPolygon)
                                {
                                    sum += polygon.ExteriorRing.Sum(point => point.X + point.Y);
                                    sum += polygon.InteriorRingList.Sum(ring => ring.Sum(point => point.X + point.Y));
                                }
                            }
                        }
                    }
                }
                else if (propInfo.IsList)
                {
                    foreach (var obj in (IList)propValue)
                        if (obj is IAimObject aimObj)
                            return MultiPolygonSum(aimObj, sum);
                }
                else if (propValue.PropertyType == AimPropertyType.Object)
                    return MultiPolygonSum(propValue as IAimObject, sum);
            }

            return sum;
        }

        private static void ClearLines(IAimObject aimObject)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex(aimObject);

            foreach (var propInfo in classInfo.Properties)
            {
                var propValue = aimObject.GetValue(propInfo.Index);
                if (propValue == null)
                    continue;

                if (propValue.PropertyType == AimPropertyType.AranField)
                {
                    if (propValue is AimField aimField)
                    {
                        if (aimField.FieldType == AimFieldType.GeoPolyline)
                        {
                            if ((propValue as IEditAimField).FieldValue is MultiLineString multiLineString)
                            {
                                var newMultiLineString = new MultiLineString();
                                foreach (var lineString in multiLineString)
                                {
                                    if (lineString.Distinct(new PointEqualityComparer()).Count() > 1)
                                        newMultiLineString.Add(lineString);
                                }
                                (propValue as IEditAimField).FieldValue = newMultiLineString;
                            }
                        }
                    }
                }
                else if (propInfo.IsList)
                {
                    foreach (var obj in (IList)propValue)
                        if (obj is IAimObject aimObj)
                            ClearLines(aimObj);
                }
                else if (propValue.PropertyType == AimPropertyType.Object)
                    ClearLines(propValue as IAimObject);
            }
        }

        private static void ClearRings(IAimObject aimObject)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex(aimObject);

            foreach (var propInfo in classInfo.Properties)
            {
                var propValue = aimObject.GetValue(propInfo.Index);
                if (propValue == null)
                    continue;

                if (propValue.PropertyType == AimPropertyType.AranField)
                {
                    if (propValue is AimField aimField)
                    {
                        if (aimField.FieldType == AimFieldType.GeoPolygon)
                        {
                            if ((propValue as IEditAimField).FieldValue is MultiPolygon multiPolygon)
                            {
                                var newMultiPolygon = new MultiPolygon();
                                foreach (Polygon polygon in multiPolygon)
                                {
                                    if (polygon.ExteriorRing.Distinct(new PointEqualityComparer()).Count() > 1)
                                    {
                                        var newPolygon = new Polygon();
                                        newPolygon.ExteriorRing = polygon.ExteriorRing;
                                        foreach (Ring ring in polygon.InteriorRingList)
                                        {
                                            if (ring.Distinct(new PointEqualityComparer()).Count() > 1)
                                                newPolygon.InteriorRingList.Add(ring);
                                        }
                                        newMultiPolygon.Add(newPolygon);
                                    }
                                }
                                (propValue as IEditAimField).FieldValue = newMultiPolygon;
                            }
                        }
                    }
                }
                else if (propInfo.IsList)
                {
                    foreach (var obj in (IList)propValue)
                        if (obj is IAimObject aimObj)
                            ClearRings(aimObj);
                }
                else if (propValue.PropertyType == AimPropertyType.Object)
                    ClearRings(propValue as IAimObject);
            }
        }

        private static void ClearMultiPolygons(IAimObject aimObject)
        {
            AimFieldAction(aimObject, aimField =>
            {
                if (aimField.FieldType == AimFieldType.GeoPolygon)
                    (aimField as IEditAimField).FieldValue = new MultiPolygon();
            });
        }

        private static bool EqualWithoutTM(AimObject aimObject1, AimObject aimObject2)
        {
            RemoveTM(aimObject1);
            RemoveTM(aimObject2);

            return AimObject.IsEquals(aimObject1, aimObject2, true);
        }

        private static MessageCauseType InequalityCauses(AimObject aimObject1, AimObject aimObject2)
        {
            if (aimObject1 != null && aimObject2 != null)
            {
                if (HasShort(aimObject1) || HasShort(aimObject2))
                    return MessageCauseType.PolygonShort;

                Close(aimObject1);
                Close(aimObject2);

                if (AimObject.IsEquals(aimObject1, aimObject2, true))
                    return MessageCauseType.PolygonOpened;

                ClearRings(aimObject1);
                ClearRings(aimObject2);
                if (AimObject.IsEquals(aimObject1, aimObject2, true))
                    return MessageCauseType.RingHasOneUniqueVertices;

                ClearLines(aimObject1);
                ClearLines(aimObject2);
                if (AimObject.IsEquals(aimObject1, aimObject2, true))
                    return MessageCauseType.LineHasOneUniqueVertices;

                if (Math.Abs(MultiPolygonSum(aimObject1) - MultiPolygonSum(aimObject2)) < 0.000001)
                {
                    ClearMultiPolygons(aimObject1);
                    ClearMultiPolygons(aimObject2);

                    if (AimObject.IsEquals(aimObject1, aimObject2, true))
                        return MessageCauseType.PolygonReverse;
                }
            }

            return MessageCauseType.UnknownError;
        }

        public static JsonWriterSettings CustomJsonWriterSettings => new JsonWriterSettings { Indent = true };

        public static void HandleMongoException(int workPackage, int number, MongoException mongoException, AbstractEvent<AimFeature> abstractEvent, Action<MessageCauseType, string, string> logger, out MessageCauseType type)
        {
            string log = null;
            string errorMessage = null;
            type = MessageCauseType.UnknownGeometryError;

            if (mongoException.Message.Contains("Can't extract geo keys"))
            {
                var objectIdIndex = mongoException.Message.IndexOf("ObjectId('") + 10;
                if (objectIdIndex > 10)
                {
                    var objectId = mongoException.Message.Substring(objectIdIndex, 24);

                    var feature = abstractEvent?.Data?.Feature;

                    if (feature == null)
                        return;

                    log = $"{workPackage}.{number}, {feature.Identifier}, {feature.FeatureType}, {feature.TimeSlice?.Interpretation}, " +
                          $"{feature.TimeSlice?.SequenceNumber}.{feature.TimeSlice?.CorrectionNumber}, " +
                          $"{feature.TimeSlice?.ValidTime?.BeginPosition} - {feature.TimeSlice?.ValidTime?.EndPosition?.ToString() ?? "~"}";

                    errorMessage = mongoException.Message;

                    var reasons = new Dictionary<MessageCauseType, string>
                                {
                                    {MessageCauseType.DuplicateVertices, "Duplicate vertices:"},
                                    {MessageCauseType.SelfIntersect, "Edges "},
                                    {MessageCauseType.IncorrectLatLon, "longitude/latitude is out of bounds"},
                                    {MessageCauseType.Less3UniqueVertices, "Loop must have at least 3 different vertices:"},
                                    {MessageCauseType.LineHasOneUniqueVertices, "GeoJSON LineString must have at least 2 vertices"},
                                };

                    foreach (var reason in reasons)
                    {
                        var messageIndex = errorMessage.IndexOf(reason.Value);
                        if (messageIndex > 0)
                        {
                            var additionalMessage = errorMessage.Substring(messageIndex);
                            log += $"\n{additionalMessage}";
                            type = reason.Key;
                            break;
                        }
                    }

                    if (type == MessageCauseType.UnknownGeometryError)
                        log += $"\n{errorMessage.Substring(Math.Max(0, errorMessage.Length - 200))}";
                }
                else
                {
                    log = $"Unknown Can't extract geo keys error: {mongoException.Message}";
                    type = MessageCauseType.UnknownGeometryError;
                }
            }
            else
            {
                log = $"Unknown mongo exception: {mongoException.Message}";
                type = MessageCauseType.UnknownGeometryError;
                errorMessage = mongoException.Message;
            }

            logger.Invoke(type, log, errorMessage);
        }

        public static void CheckFeatures(AimFeature first, AimFeature second, int workPackage, int number, Action<MessageCauseType, string, string> logger)
        {
            if (logger == null)
                return;

            if (first?.Feature == null || second?.Feature == null)
            {
                if (first?.Feature == null && second?.Feature != null)
                    logger.Invoke(MessageCauseType.CommonError, $"{workPackage}.{number}. First feature is null.", second.Feature.ToJson(CustomJsonWriterSettings));
                else if (first?.Feature != null && second?.Feature == null)
                    logger.Invoke(MessageCauseType.CommonError, $"{workPackage}.{number}. Second feature is null.", first.Feature.ToJson(CustomJsonWriterSettings));

                return;
            }

            var result = AimObject.IsEquals(first.Feature, second.Feature, true);

            if (!result && !EqualWithoutTM(first.Feature, second.Feature))
            {
                var json = first.Feature.ToJson(CustomJsonWriterSettings);
                var cause = InequalityCauses(first.Feature, second.Feature);
                string metainfo = $"{workPackage}.{number}, {first.Identifier}, {first.FeatureType}, {first.Feature.TimeSlice?.Interpretation}, " +
                                  $"{first.Feature.TimeSlice?.SequenceNumber}.{first.Feature.TimeSlice?.CorrectionNumber}, " +
                                  $"{first.Feature.TimeSlice?.ValidTime?.BeginPosition} - {first.Feature.TimeSlice?.ValidTime?.EndPosition?.ToString() ?? "~"}";
                logger.Invoke(cause, metainfo, json);
            }
        }

        public static void ApplyTimeSlice(AbstractEvent<AimFeature> abstractEvent)
        {

            if (abstractEvent?.Data?.Feature != null)
            {
                abstractEvent.Data.Feature.TimeSlice = new TimeSlice
                {
                    ValidTime = new TimePeriod
                    {
                        BeginPosition = abstractEvent.TimeSlice.BeginPosition,
                        EndPosition = abstractEvent.TimeSlice.EndPosition
                    },
                    FeatureLifetime = new TimePeriod
                    {
                        BeginPosition = abstractEvent.LifeTimeBegin ?? default(DateTime),
                        EndPosition = abstractEvent.LifeTimeEnd
                    },
                    Interpretation = ConvertInterpretation(abstractEvent.Interpretation),
                    SequenceNumber = abstractEvent.Version.SequenceNumber,
                    CorrectionNumber = abstractEvent.Version.CorrectionNumber
                };
            }
        }

        private static TimeSliceInterpretationType ConvertInterpretation(Interpretation interpretation)
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
    }
}
