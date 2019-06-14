using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Aim.Data.Filters;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Aim.Utilities;
using Aran.Converters;
using Aran.Temporality.Common.Util;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Aran.Temporality.Common.Mongo
{
    internal static class MongoFilter
    {
        public const double EqualTolerance = 0.0000001;

        private static bool TryFindProperty(AimClassInfo classInfo, string propName, out AimPropInfo propInfo, out int index)
        {
            propInfo = null;
            index = -1;

            for (int i = 0; i < classInfo.Properties.Count; i++)
            {
                if (classInfo.Properties[i].Name.Equals(propName, StringComparison.OrdinalIgnoreCase))
                {
                    propInfo = classInfo.Properties[i];
                    index = i;
                    return true;
                }
            }

            return false;
        }

        private static object GetPropertyValue(Feature data, string property)
        {
            var propInfos = AimMetadataUtility.GetInnerProps((int)data.FeatureType, property);
            var propInfo = propInfos.LastOrDefault();

            if (propInfo == null)
                return null;

            var propValList = AimMetadataUtility.GetInnerPropertyValue(data, propInfos, false);
            var propVal = propValList.FirstOrDefault();

            if (propVal is IEditAimField)
                return (propVal as IEditAimField).FieldValue;
            if (propVal is FeatureRef)
                return (propVal as FeatureRef).Identifier;

            return propVal;
        }

        public static string GenerateMongoPath(int featureTypeId, string path)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex(featureTypeId);
            var stringBuilder = new StringBuilder("Value.p2_Feature");

            foreach (var propName in path.Split('.'))
            {
                if (!TryFindProperty(classInfo, propName, out var propInfo, out var index))
                    throw new ArgumentException($"Filter path is incorrect. Property '{propName}' from path '{path}' was not found in {(FeatureType)featureTypeId}");

                stringBuilder.Append(".p").Append(index).Append('_').Append(propInfo.Name);

                if (propInfo.PropType.AimObjectType == AimObjectType.Field)
                    break;

                if (propInfo.IsFeatureReference)
                {
                    stringBuilder.Append(".p2_Identifier");
                    break;
                }

                if (propInfo.PropType.AimObjectType == AimObjectType.DataType)
                {
                    stringBuilder.Append(".p3_SI");
                    break;
                }

                classInfo = AimMetadata.GetClassInfoByIndex(propInfo.TypeIndex);
            }

            return stringBuilder.ToString();
        }

        public static void GenerateMongoPath(int featureTypeId, string path, out string mongoPath)
        {
            mongoPath = GenerateMongoPath(featureTypeId, path);
        }

        private static FilterDefinition<MongoWrapper<TDataType>> ListInFilterDefinition<TDataType>(string mongoPath, IList value)
        {
            var builder = new FilterDefinitionBuilder<MongoWrapper<TDataType>>();
            var list = value;
            if (list == null || list.Count == 0)
                return FilterDefinition<MongoWrapper<TDataType>>.Empty;

            var type = list[0].GetType();

            if (type == typeof(bool))
                return builder.In(mongoPath, list.Cast<bool>());
            if (type == typeof(DateTime))
                return builder.In(mongoPath, list.Cast<DateTime>());
            if (type == typeof(double))
                return builder.In(mongoPath, list.Cast<double>());
            if (type == typeof(Guid))
                return builder.In(mongoPath, list.Cast<Guid>());
            if (type.IsSubclassOf(typeof(System.Enum)))
                return builder.In(mongoPath, list.Cast<int>());
            if (type == typeof(int))
                return builder.In(mongoPath, list.Cast<int>());
            if (type == typeof(long))
                return builder.In(mongoPath, list.Cast<long>());
            if (type == typeof(string))
                return builder.In(mongoPath, list.Cast<string>());
            if (type == typeof(UInt32))
                return builder.In(mongoPath, list.Cast<UInt32>());

            throw new NotSupportedException("Unsupported operation");
        }

        private static FilterDefinition<MongoWrapper<TDataType>> ComparisonFilterDefinition<TDataType>(int featureTypeId, ComparisonOps comparisonOps)
        {
            GenerateMongoPath(featureTypeId, comparisonOps.PropertyName, out var mongoPath);

            var builder = new FilterDefinitionBuilder<MongoWrapper<TDataType>>();

            var value = comparisonOps.Value;

            if (value is ADataType)
                value = ConverterToSI.Convert(value, 0);

            switch (comparisonOps.OperationType)
            {
                case ComparisonOpType.EqualTo:
                    double numericValue = value is double d ? d : value is float f ? f : double.NaN;

                    if (double.IsNaN(numericValue))
                        return builder.Eq(mongoPath, value);

                    return builder.And(new List<FilterDefinition<MongoWrapper<TDataType>>>
                    {
                        builder.Gt(mongoPath, numericValue - EqualTolerance),
                        builder.Lt(mongoPath, numericValue + EqualTolerance)
                    });
                case ComparisonOpType.GreaterThan:
                    return builder.Gt(mongoPath, value);
                case ComparisonOpType.GreaterThanOrEqualTo:
                    return builder.Gte(mongoPath, value);
                case ComparisonOpType.In:
                    var iList = value as IList;
                    if (iList == null)
                        throw new Exception("For operation IN the value should be IList");
                    return ListInFilterDefinition<TDataType>(mongoPath, iList);
                case ComparisonOpType.Is:
                    //TODO: implement it
                    throw new NotSupportedException("Unsupported operation: " + comparisonOps.OperationType);
                case ComparisonOpType.LessThan:
                    return builder.Lt(mongoPath, value);
                case ComparisonOpType.LessThanOrEqualTo:
                    return builder.Lte(mongoPath, value);
                case ComparisonOpType.Like:
                    {
                        var text = Regex.Escape(value.ToString());
                        var regex = new Regex(text, RegexOptions.IgnoreCase);
                        return builder.Regex(mongoPath, new BsonRegularExpression(regex));
                    }
                case ComparisonOpType.NotEqualTo:
                    return builder.Ne(mongoPath, value);
                case ComparisonOpType.NotLike:
                    {
                        var text = "^((?!" + Regex.Escape(value.ToString()) + ").)*$";
                        var regex = new Regex(text, RegexOptions.IgnoreCase);
                        return builder.Regex(mongoPath, new BsonRegularExpression(regex));
                    }
                case ComparisonOpType.NotNull:
                    return builder.Ne(mongoPath, BsonNull.Value);
                case ComparisonOpType.Null:
                    return builder.Eq(mongoPath, BsonNull.Value);
            }

            throw new NotSupportedException("Unsupported operation: " + comparisonOps.OperationType);
        }

        private static FilterDefinition<MongoWrapper<TDataType>> SpatialFilterDefinition<TDataType>(int featureTypeId, SpatialOps spatialOps, HashSet<string> geoPathes)
        {
            GenerateMongoPath(featureTypeId, spatialOps.PropertyName, out var mongoPath);

            geoPathes.Add(mongoPath);

            var builder = new FilterDefinitionBuilder<MongoWrapper<TDataType>>();

            if (spatialOps is InExtend)
            {
                var op = spatialOps as InExtend;
                var polygon = new GeoJsonPolygon<GeoJson2DGeographicCoordinates>(
                    new GeoJsonPolygonCoordinates<GeoJson2DGeographicCoordinates>(
                        new GeoJsonLinearRingCoordinates<GeoJson2DGeographicCoordinates>(
                            new List<GeoJson2DGeographicCoordinates>
                            {
                                new GeoJson2DGeographicCoordinates(op.MinX, op.MinY),
                                new GeoJson2DGeographicCoordinates(op.MinX, op.MaxY),
                                new GeoJson2DGeographicCoordinates(op.MaxX, op.MaxY),
                                new GeoJson2DGeographicCoordinates(op.MaxX, op.MinY),
                                new GeoJson2DGeographicCoordinates(op.MinX, op.MinY)
                            }
                            )));
                return builder.GeoWithin(mongoPath, polygon);
            }

            if (spatialOps is Within)
            {
                var op = spatialOps as Within;
                GeoJsonGeometry<GeoJson3DGeographicCoordinates> geometry = null;
                if (op.Geometry is Geometries.Polygon polygon)
                    geometry = GeoJsonConverter.GetGeoJsonPolygon(polygon);
                else if (op.Geometry is Geometries.MultiPolygon multiPolygon)
                    geometry = GeoJsonConverter.GetGeoJsonMultiPolygon(multiPolygon);

                if (geometry == null)
                    throw new ArgumentException("Invalid geometry for Within filter");

                return builder.GeoWithin(mongoPath, geometry);
            }

            if (spatialOps is DWithin)
            {
                var op = spatialOps as DWithin;
                var distance = ConverterToSI.Convert(op.Distance, 0) / 6378100;
                if (distance == 0)
                    throw new ArgumentException("Distance for DWithin filter is 0");
                if (op.Point == null || op.Point.IsEmpty)
                    throw new ArgumentException("Invalid point for DWithin filter");
                return builder.GeoWithinCenterSphere(mongoPath, op.Point.X, op.Point.Y, distance);
            }
            throw new NotSupportedException("not supported Filter.Operation");
        }

        private static FilterDefinition<MongoWrapper<TDataType>> GetMongoFilter<TDataType>(OperationChoice operationChoice, int featureTypeId, HashSet<string> geoPathes)
        {
            if (operationChoice == null)
                return FilterDefinition<MongoWrapper<TDataType>>.Empty;

            var classInfo = AimMetadata.GetClassInfoByIndex(featureTypeId);
            if (classInfo == null)
                throw new ArgumentException("featureTypeId");

            switch (operationChoice.Choice)
            {
                case OperationChoiceType.Comparison:
                    return ComparisonFilterDefinition<TDataType>(featureTypeId, operationChoice.ComparisonOps);

                case OperationChoiceType.Logic:
                    var builder = new FilterDefinitionBuilder<MongoWrapper<TDataType>>();
                    if (operationChoice.LogicOps is BinaryLogicOp ops)
                    {
                        switch (ops.Type)
                        {
                            case BinaryLogicOpType.And:
                                return builder.And(ops.OperationList.Select(x => GetMongoFilter<TDataType>(x, featureTypeId, geoPathes)));
                            case BinaryLogicOpType.Or:
                                return builder.Or(ops.OperationList.Select(x => GetMongoFilter<TDataType>(x, featureTypeId, geoPathes)));
                            default:
                                throw new NotSupportedException("not supported logical condition");
                        }
                    }

                    throw new NotSupportedException("not supported logical operation");
                case OperationChoiceType.Spatial:
                    return SpatialFilterDefinition<TDataType>(featureTypeId, operationChoice.SpatialOps, geoPathes);
                default:
                    throw new NotSupportedException("not supported Filter.Operation");
            }
        }

        public static FilterDefinition<MongoWrapper<TDataType>> GetMongoFilter<TDataType>(this Filter filter, int featureTypeId, out HashSet<string> geoPathes)
        {
            geoPathes = new HashSet<string>();
            if (filter == null || filter.Operation == null)
                return FilterDefinition<MongoWrapper<TDataType>>.Empty;
            return GetMongoFilter<TDataType>(filter.Operation, featureTypeId, geoPathes);
        }
    }
}
