using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Geometries;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Aim.MetaData;
using TOSSM.Geometry;

namespace TOSSM.Util
{
    internal class StateDeltaUtil
    {
        internal static Func<Guid?, FeatureType?, int, string> GetFeatureRefDescription { get; set; }

        private const char Delimeter = '.';

        private static string AimFieldDescription(AimField value)
        {
            if (!(value is IEditAimField editableValue))
                return "";

            if (value.FieldType == AimFieldType.GeoPoint)
            {
                var point = (Point)editableValue.FieldValue;
                return $"{GeoFormatter.FormatXdms(point.X)}, {GeoFormatter.FormatYdms(point.Y)}, {point.Z}";
            }

            if (value.FieldType == AimFieldType.SysDateTime)
            {
                var date = (DateTime)editableValue.FieldValue;
                return date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            return editableValue.FieldValue.ToString();
        }

        private static string AimFieldDescription(Guid? guid, FeatureType? featureType, int workPackage)
        {
            var description = guid.ToString();

            var temp = GetFeatureRefDescription?.Invoke(guid, featureType, workPackage) ?? guid.ToString();

            if (temp != description)
                description += ", " + temp;

             return description;
        }

        private static void FindAimFieldDelta(AimField oldValue, AimField newValue, Dictionary<string, Tuple<string, string>> result, string name)
        {
            if (oldValue == null && newValue == null)
                return;

            var type = newValue?.FieldType ?? oldValue.FieldType;

            if (type == AimFieldType.GeoPolygon || type == AimFieldType.GeoPolyline)
                return;

            var editableOldValue = oldValue as IEditAimField;
            var editableNewValue = newValue as IEditAimField;

            if (oldValue != null && newValue == null)
                result[name] = Tuple.Create(AimFieldDescription(oldValue), (string)null);
            else if (oldValue == null)
                result[name] = Tuple.Create((string)null, AimFieldDescription(newValue));
            else if (type == AimFieldType.GeoPoint)
            {
                var oldPoint = (Point)editableOldValue.FieldValue;
                var newPoint = (Point)editableNewValue.FieldValue;

                if (oldPoint.IsEmpty && newPoint.IsEmpty)
                    return;

                if (!oldPoint.Equals2D(newPoint, 0.0000001))
                    result[name] = Tuple.Create(AimFieldDescription(oldValue), AimFieldDescription(newValue));
            }
            else if (!editableOldValue.FieldValue.Equals(editableNewValue.FieldValue))
                result[name] = Tuple.Create(AimFieldDescription(oldValue), AimFieldDescription(newValue));
        }

        private static void FindValClassDelta(ValClassBase oldValue, ValClassBase newValue, Dictionary<string, Tuple<string, string>> result, string name)
        {
            if (oldValue == null && newValue == null)
                return;

            if (oldValue != null && newValue == null)
                result[name] = Tuple.Create(oldValue.ToString(), (string)null);
            else if (oldValue == null)
                result[name] = Tuple.Create((string)null, newValue.ToString());
            else
            {
                var editableOldValue = oldValue as IEditValClass;
                var editableNewValue = newValue as IEditValClass;

                if (editableOldValue.Uom == editableNewValue.Uom && editableOldValue.Value == editableNewValue.Value)
                    return;

                result[name] = Tuple.Create(oldValue.ToString(), newValue.ToString());
            }
        }

        private static void FindFeatureRefDelta(AimPropInfo propInfo, FeatureRef oldValue, FeatureRef newValue, 
                                                Dictionary<string, Tuple<string, string>> result, string name)
        {
            if ((newValue ?? oldValue) == null)
                return;

            var oldIdentifier = oldValue.Identifier;
            var newIdentifier = newValue.Identifier;

            var oldFeatureType = GetFeatureType(propInfo, oldValue);
            var newFeatureType = GetFeatureType(propInfo, newValue);

            if (oldValue != null && newValue == null)
            {
                result[name] = Tuple.Create(AimFieldDescription(oldIdentifier, oldFeatureType, -1), (string)null);
            }
            else if (oldValue == null && newValue != null)
            {
                result[name] = Tuple.Create((string)null, AimFieldDescription(newIdentifier, newFeatureType, 0));
            }
            else
            {
                if(!oldIdentifier.Equals(newIdentifier))
                    result[name] = Tuple.Create(AimFieldDescription(oldIdentifier, oldFeatureType, -1),
                    AimFieldDescription(newIdentifier, newFeatureType, 0));
            }

            
        }

        private static FeatureType GetFeatureType(AimPropInfo propInfo, FeatureRef value)
        {
            FeatureType featureType;
            var aimPropVal = ((IAimObject)value).GetValue(propInfo.Index);
            
            if (value is IAbstractFeatureRef)
                featureType = (FeatureType)(value as IAbstractFeatureRef).FeatureTypeIndex;
            else
                featureType = propInfo.ReferenceFeature;
            
            return featureType;
        }

        private static void FindListDelta(IList oldValue, IList newValue, Dictionary<string, Tuple<string, string>> result, string name)
        {
            if ((newValue ?? oldValue) == null)
                return;

            int index = 0;

            if (newValue == null)
            {
                foreach (var value in oldValue)
                    FindAimObjectDelta(value as IAimObject, null, result, $"{name}[{index++}]");

                return;
            }

            if (oldValue == null)
            {
                foreach (var value in newValue)
                    FindAimObjectDelta(null, value as IAimObject, result, $"{name}[{index++}]");

                return;
            }

            foreach (var pair in oldValue.Cast<AObject>().Zip(newValue.Cast<AObject>(), (oldVal, newVal) => new { oldVal, newVal }))
                FindAimObjectDelta(pair.oldVal, pair.newVal, result, $"{name}[{index++}]");

            int indexBak = index;

            foreach (var value in oldValue.Cast<AObject>().Skip(index))
                FindAimObjectDelta(value, null, result, $"{name}[{index++}]");

            index = indexBak;
            foreach (var value in newValue.Cast<AObject>().Skip(index))
                FindAimObjectDelta(null, value, result, $"{name}[{index++}]");
        }

        private static void FindFeatureRefListDelta(AimPropInfo propInfo, IList oldValue, IList newValue, Dictionary<string, Tuple<string, string>> result, string name)
        {
            if ((newValue ?? oldValue) == null)
                return;

            var oldValues = new List<string>();
            var newValues = new List<string>(); 

            if (oldValue != null)
            {
                foreach (FeatureRefObject value in oldValue)
                {
                    if (newValue == null || !newValue.Cast<FeatureRefObject>().Any(x => x.Feature.Identifier == value.Feature.Identifier))
                        oldValues.Add(AimFieldDescription(value.Feature.Identifier, GetFeatureType(propInfo, value.Feature), -1));
                }
            }

            if (newValue != null)
            {
                foreach (FeatureRefObject value in newValue)
                {
                    if (oldValue == null || !oldValue.Cast<FeatureRefObject>().Any(x => x.Feature.Identifier == value.Feature.Identifier))
                        newValues.Add(AimFieldDescription(value.Feature.Identifier, GetFeatureType(propInfo, value.Feature), 0));
                }
            }

            var index = 0;

            foreach (var pair in oldValues.Zip(newValues, (oldVal, newVal) => new { oldVal, newVal }))
                result[$"{name}[{index++}].Feature.Identifier"] = Tuple.Create(pair.oldVal, pair.newVal);

            int indexBak = index;
            foreach (var value in oldValues.Skip(index))
                result[$"{name}[{index++}].Feature.Identifier"] = Tuple.Create(value, "");

            index = indexBak;
            foreach (var value in newValues.Skip(index))
                result[$"{name}[{index++}].Feature.Identifier"] = Tuple.Create("", value);
        }

        private static string GetName(string path, AimPropInfo propInfo)
        {
            if (string.IsNullOrEmpty(path))
                return propInfo.Name;

            return $"{path}{Delimeter}{propInfo.Name}";
        }

        private static void FindAimObjectDelta(IAimObject oldValue, IAimObject newValue, Dictionary<string, Tuple<string, string>> result, string name = "")
        {
            if (newValue == null && oldValue == null)
                return;

            var classInfo = AimMetadata.GetClassInfoByIndex(newValue ?? oldValue);

            if (classInfo?.Properties == null)
                return;

            foreach (var propInfo in classInfo.Properties)
            {
                var oldProp = oldValue?.GetValue(propInfo.Index);
                var newProp = newValue?.GetValue(propInfo.Index);

                if ((newValue ?? oldValue) is Feature)
                {
                    if (propInfo.Name == "Id" || propInfo.Name == "Identifier" || propInfo.Name == "TimeSlice")
                        continue;
                }

                if (oldProp == null && newProp == null)
                    continue;

                try
                {
                    if (propInfo.PropType.AimObjectType == AimObjectType.Field)
                    {
                        FindAimFieldDelta(oldProp as AimField, newProp as AimField, result, GetName(name, propInfo));
                    }
                    else if (oldProp is FeatureRef || newValue is FeatureRef)
                    {
                        FindFeatureRefDelta(propInfo, oldProp as FeatureRef, newProp as FeatureRef, result, GetName(name, propInfo));
                    }
                    else if (propInfo.PropType.SubClassType == AimSubClassType.ValClass)
                    {
                        FindValClassDelta(oldProp as ValClassBase, newProp as ValClassBase, result, GetName(name, propInfo));
                    }
                    else if (propInfo.IsList)
                    {
                        if (propInfo.PropType.Index == (int)ObjectType.FeatureRefObject)
                            FindFeatureRefListDelta(propInfo, oldProp as IList, newProp as IList, result, GetName(name, propInfo));
                        else
                            FindListDelta(oldProp as IList, newProp as IList, result, GetName(name, propInfo));
                    }
                    else
                    {
                        FindAimObjectDelta(oldProp as IAimObject, newProp as IAimObject, result, GetName(name, propInfo));
                    }
                }
                catch
                {
                    //TODO: ?
                }
            }
        }

        private static void TimeSliceDateGenerate(TimeSlice value, out string version, out string validTime, out string lifeTime)
        {
            if (value == null)
            {
                version = validTime = lifeTime = "";
                return;
            }

            version = $"{value.SequenceNumber}.{value.CorrectionNumber}";

            validTime = $"{value.ValidTime.BeginPosition.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} - "
                + (value.ValidTime.EndPosition?.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) ?? "~");

            lifeTime = $"{value.FeatureLifetime.BeginPosition.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} - "
                + (value.FeatureLifetime.EndPosition?.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) ?? "~");
        }

        private static void TimeSliceDelta(TimeSlice oldValue, TimeSlice newValue, Dictionary<string, Tuple<string, string>> result)
        {
            TimeSliceDateGenerate(oldValue, out var oldVersion, out var oldValidTime, out var oldLifeTime);
            TimeSliceDateGenerate(newValue, out var newVersion, out var newValidTime, out var newLifeTime);

            result["Version"] = Tuple.Create(oldVersion, newVersion);
            result["ValidTime"] = Tuple.Create(oldValidTime, newValidTime);
            //result["FeatureLifetime"] = Tuple.Create(oldLifeTime, newLifeTime);
        }

        public static Dictionary<string, Tuple<string, string>> GetStateDelta(AbstractState<AimFeature> oldState, AbstractState<AimFeature> newState)
        {
            var result = new Dictionary<string, Tuple<string, string>>();

            TimeSliceDelta(oldState?.Data?.Feature?.TimeSlice, newState?.Data?.Feature?.TimeSlice, result);
            FindAimObjectDelta(oldState?.Data?.Feature, newState?.Data?.Feature, result);

            if (result.Count == 3 && newState?.Data?.Feature?.TimeSlice?.FeatureLifetime.EndPosition == null)
                return new Dictionary<string, Tuple<string, string>>();

            return result;
        }
    }
}
