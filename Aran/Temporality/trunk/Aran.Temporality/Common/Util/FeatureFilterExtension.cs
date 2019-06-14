using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Aim.Data.Filters;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Aim.Utilities;
using Aran.Converters;
using Aran.Geometries;

namespace Aran.Temporality.Common.Util
{
    public static class FeatureFilterExtension
    {
        public const double EqualTolerance = 0.0000001;

        public static bool IsFilterOk(this Feature data, Filter filter)
        {
            return IsFilterOk(data, filter.Operation);
        }

        private static bool IsNumber(this object value)
        {
            return value is sbyte
                   || value is byte
                   || value is short
                   || value is ushort
                   || value is int
                   || value is uint
                   || value is long
                   || value is ulong
                   || value is float
                   || value is double
                   || value is decimal;
        }

        public static bool GetNumericalData(object value, out double numericalValue)
        {
            numericalValue = 0;

            try
            {
                if (IsNumber(value))
                {
                    numericalValue = Convert.ToDouble(value);
                    return true;
                }

                if (value is IEditValClass)
                {
                    numericalValue = ConverterToSI.Convert(value, double.NaN);
                    return true;
                }

                if (value is IEditValClass)
                {
                    numericalValue = ConverterToSI.Convert(value, 0);
                    return true;
                }

                if (value is AimField aimField)
                {
                    var aimFieldValue = ((IEditAimField) aimField).FieldValue;

                    if (IsNumber(aimFieldValue))
                    {
                        numericalValue = Convert.ToDouble(aimFieldValue);
                        return true;
                    }
                }
            }
            catch
            {
                // ignored
            }

            return false;
        }

        private static bool GetDateTimeData(object value, out DateTime dateTimeValue)
        {
            dateTimeValue = DateTime.MinValue;

            if (value is DateTime time)
            {
                dateTimeValue = time;
                return true;
            }

            try
            {
                if (value is AimField aimField)
                {
                    var aimFieldValue = ((IEditAimField) aimField).FieldValue;

                    if (aimField.FieldType == AimFieldType.SysDateTime)
                    {
                        dateTimeValue = (DateTime) aimFieldValue;
                        return true;
                    }
                }
            }
            catch
            {
                // ignored
            }

            return false;
        }

        static bool IsComparisonOk(object value, object actualValue, OperationChoice operation)
        {
            try
            {
                var isNumericalData = GetNumericalData(actualValue, out var actualNumericalValue);
                GetNumericalData(value, out var numericalValue);

                var isDateTimeValue = GetDateTimeData(actualValue, out var actualDateTimeValue);
                GetDateTimeData(value, out var dateTimeValue);

                switch (operation.ComparisonOps.OperationType)
                {
                    case ComparisonOpType.EqualTo:
                        return (actualValue == null && value == null) ||
                               (actualValue != null && actualValue.Equals(value)) ||
                               (isNumericalData && Math.Abs(actualNumericalValue - numericalValue) < EqualTolerance);
                    case ComparisonOpType.GreaterThan:
                        if (isNumericalData)
                            return actualNumericalValue > numericalValue;
                        else if (isDateTimeValue)
                            return actualDateTimeValue.CompareTo(dateTimeValue) > 0;
                        throw new Exception("Property is not comparable");
                    case ComparisonOpType.GreaterThanOrEqualTo:
                        if (isNumericalData)
                            return actualNumericalValue >= numericalValue;
                        else if (isDateTimeValue)
                            return actualDateTimeValue.CompareTo(dateTimeValue) >= 0;
                        throw new Exception("Property is not comparable");
                    case ComparisonOpType.In:
                        return value is IList list && list.Contains(actualValue);
                    case ComparisonOpType.Is:
                        //TODO: implement it
                        return true;
                    case ComparisonOpType.LessThan:
                        if (isNumericalData)
                            return actualNumericalValue < numericalValue;
                        else if (isDateTimeValue)
                            return actualDateTimeValue.CompareTo(dateTimeValue) < 0;
                        throw new Exception("Property is not comparable");
                    case ComparisonOpType.LessThanOrEqualTo:
                        if (isNumericalData)
                            return actualNumericalValue <= numericalValue;
                        else if (isDateTimeValue)
                            return actualDateTimeValue.CompareTo(dateTimeValue) <= 0;
                        throw new Exception("Property is not comparable");
                    case ComparisonOpType.Like:
                        return actualValue.ToString().IndexOf(value.ToString(), StringComparison.CurrentCultureIgnoreCase) >= 0;
                    case ComparisonOpType.NotEqualTo:
                        return !((actualValue == null && value == null) ||
                               (actualValue != null && actualValue.Equals(value)) ||
                               (isNumericalData && Math.Abs(actualNumericalValue - numericalValue) < EqualTolerance));
                    case ComparisonOpType.NotLike:
                        return actualValue.ToString().IndexOf(value.ToString(), StringComparison.CurrentCultureIgnoreCase) < 0;
                    case ComparisonOpType.NotNull:
                        return actualValue != null;
                    case ComparisonOpType.Null:
                        return actualValue == null;
                    default:
                        throw new Exception("Unsupported operation.ComparisonOps.OperationType");
                }
            }
            catch (Exception)
            {
                return true;
            }
        }

        private static bool IsFilterOk(Feature data, OperationChoice operation)
        {
            if (operation == null)
                return true;

            if (data == null)
                return false;
            
            switch (operation.Choice)
            {
                case OperationChoiceType.Comparison:
                    var actualValue = GetPropertyValue(data, operation.ComparisonOps.PropertyName);
                    var value = operation.ComparisonOps.Value;
                    if (value is Guid)
                    {
                        if (actualValue is FeatureRef)
                        {
                            actualValue = (actualValue as FeatureRef).Identifier;
                        }
                        else if (actualValue is FeatureRefObject)
                        {
                            actualValue = (actualValue as FeatureRefObject).Feature.Identifier;
                        }

                        var actualList = actualValue as IList;
                        if (actualList != null)//at least one element of list should pass filter to proceed
                        {
                            foreach (var item in actualList)
                            {
                                var subActualValue = item;

                                if (subActualValue is FeatureRef)
                                {
                                    subActualValue = (subActualValue as FeatureRef).Identifier;
                                }
                                else if (subActualValue is FeatureRefObject)
                                {
                                    subActualValue = (subActualValue as FeatureRefObject).Feature.Identifier;
                                }

                                if (IsComparisonOk(value, subActualValue, operation))
                                {
                                    return true;
                                }
                            }
                            return false;
                        }
                    }

                    return IsComparisonOk(value, actualValue, operation);

                case OperationChoiceType.Logic:
                    if (operation.LogicOps is BinaryLogicOp)
                    {
                        var ops = operation.LogicOps as BinaryLogicOp;
                        switch (ops.Type)
                        {
                            case BinaryLogicOpType.And:
                                return ops.OperationList.All(op => IsFilterOk(data, op));
                            case BinaryLogicOpType.Or:
                                return ops.OperationList.Any(op => IsFilterOk(data, op));
                            default:
                                throw new Exception("not supported logical condition");
                        }
                    }

                    throw new Exception("not supported logical operation");
                case OperationChoiceType.Spatial:
                    if (operation.SpatialOps is InExtend)
                    {
                        var op = operation.SpatialOps as InExtend;
                        var geoValue = GetPropertyValue(data, op.PropertyName);

                        return IsGeoInExtend(geoValue, op.MinX, op.MinY, op.MaxX, op.MaxY);
                    }

                    if (operation.SpatialOps is Within)
                    {
                        //return false;
                        var op = operation.SpatialOps as Within;
                        var geoValue = GetPropertyValue(data, op.PropertyName);

                        return IsGeoWithin(geoValue, op.Geometry);
                    }

                    if (operation.SpatialOps is DWithin)
                    {
                        var op = operation.SpatialOps as DWithin;
                        var geoValue = GetPropertyValue(data, op.PropertyName);
                        return IsGeoDWithin(geoValue, op.Point, op.Distance);
                    }

                    throw new Exception("not supported spatial filter");
                default:
                    throw new Exception("not supported operation");
            }
        }

        private static bool IsGeoInExtend(object geoValue, double minX, double minY, double maxX, double maxY)
        {
            if (geoValue == null)
                return false;

            if (!GeometrySimplifier.GetExtend(geoValue as Geometry, out var geoMinX, out var geoMinY, out var geoMaxX, out var geoMaxY))
            {
                return false;
            }

            //geoMinY and geoMaxY should be in [minY..maxY] interval
            if (geoMinY < minY) return false;
            if (geoMinY > maxY) return false;
            if (geoMaxY < minY) return false;
            if (geoMaxY > maxY) return false;

            //find appropriate semisphere
            int count = 0;
            while (
                Math.Abs(minX) > 90 ||
                Math.Abs(maxX) > 90 ||
                Math.Abs(geoMinX) > 90 ||
                Math.Abs(geoMaxX) > 90)
            {
                minX++;
                maxX++;
                geoMinX++;
                geoMaxX++;

                if (count++ > 360) break;

                //keep X in [-180..180] interval
                if (minX > 180) minX -= 360;
                if (maxX > 180) maxX -= 360;
                if (geoMinX > 180) geoMinX -= 360;
                if (geoMaxX > 180) geoMaxX -= 360;

                if (minX < -180) minX += 360;
                if (maxX < -180) maxX += 360;
                if (geoMinX < -180) geoMinX += 360;
                if (geoMaxX < -180) geoMaxX += 360;
            }

            if (count > 360) return false;

            //geoMinX and geoMaxX should be in [minX..maxX] interval
            if (geoMinX < minX) return false;
            if (geoMinX > maxX) return false;
            if (geoMaxX < minX) return false;
            if (geoMaxX > maxX) return false;

            return true;
        }


        private static bool IsGeoDWithin(object geoValue, Geometry geometry, ValDistance distance)
        {
            if (geoValue == null)
                return false;

            //return true;

            //TODO: apply negative X
#warning apply negative X
            GeometrySimplifier.Simplify(geometry, out var center1, out var radius1);
            GeometrySimplifier.Simplify(geoValue as Geometry, out var center2, out var radius2);

            if (center1 == null) return false;
            if (center2 == null) return false;

            var radius3 = NativeMethods.ReturnGeodesicDistance(center1.X, center1.Y, center2.X, center2.Y);
            var radius4 = ConverterToSI.Convert(distance, double.NaN);

            return (radius3 < radius2 + radius1 + radius4);
        }

        private static bool IsGeoWithin(object geoValue, Geometry geometry)
        {
            return IsGeoDWithin(geoValue, geometry, new ValDistance(1, UomDistance.M));
        }

        static object GetPropertyValue(object data, string property)
        {
            var propInfos = AimMetadataUtility.GetInnerProps((int)(data as Feature).FeatureType, property);
            var propInfo = propInfos.LastOrDefault();

            if (propInfo == null)
                return null;

            var propValList = AimMetadataUtility.GetInnerPropertyValue((data as IAimObject), propInfos, false);
            var propVal = propValList.FirstOrDefault();

            if (propVal is IEditAimField)
                return (propVal as IEditAimField).FieldValue;
            if (propVal is Aran.Aim.DataTypes.FeatureRef)
                return (propVal as Aran.Aim.DataTypes.FeatureRef).Identifier;




            return propVal;


            var i = property.IndexOf("/");
            if (i == -1) i = property.IndexOf(".");
            var currentProperty = i > -1 ? property.Substring(0, i) : property;
            var remainingProperty = i > -1 ? property.Substring(i + 1, property.Length - i - 1) : null;

            var pi = data.GetType().GetProperties().Where(t => t.Name.ToLower() == currentProperty.ToLower()).FirstOrDefault();
            if (pi == null) throw new Exception("wrong property name");
            object currentValue = null;
            try
            {
                currentValue = pi.GetValue(data, null);
            }
            catch
            {
            }

            if (currentValue is IList)
            {
                var list = currentValue as IList;
                if (list.Count > 0)
                {
                    currentValue = list[0];
                }
                else
                {
                    currentValue = null;
                }
            }

            if (currentValue == null) return null;

            if (remainingProperty == null) return currentValue;


            return GetPropertyValue(currentValue, remainingProperty);
        }
    }
}
