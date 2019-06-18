﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIP.GUI.Classes;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Geometries;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Aim.MetaData;

namespace AIP.DataSet.Classes
{
    public static class SUP
    {
        public static Dictionary<string, Tuple<string, string>> GetStateDelta(AimFeature oldState, AimFeature newState)
        {
            var result = new Dictionary<string, Tuple<string, string>>();

            FindAimObjectDelta(oldState?.Feature, newState?.Feature, result);

            return result;
        }

        static void FindAimObjectDelta(IAimObject oldValue, IAimObject newValue, Dictionary<string, Tuple<string, string>> result, string name = "")
        {
            if (newValue == null && oldValue == null)
                return;

            var classInfo = AimMetadata.GetClassInfoByIndex(newValue ?? oldValue);

            if (classInfo?.Properties != null)
                foreach (var propInfo in classInfo.Properties)
                {
                    var prop1 = oldValue?.GetValue(propInfo.Index);
                    var prop2 = newValue?.GetValue(propInfo.Index);

                    if (prop1 == null && prop2 == null)
                        continue;

                    try
                    {
                        FindPropertyDelta(propInfo, prop1, prop2, result, name);
                    }
                    catch
                    {
                        //TODO: ?
                    }
                }
        }
        static string AimFieldDescription(AimField value)
        {
            var editableValue = value as IEditAimField;
            if (value.FieldType == AimFieldType.GeoPoint)
            {
                // TODO: point to string
                var point = (Aran.Geometries.Point)editableValue.FieldValue;
                return point.ToPointString();
            }

            return editableValue.FieldValue.ToString();
        }

        static void FindAimFieldDelta(AimField oldValue, AimField newValue, Dictionary<string, Tuple<string, string>> result, string name)
        {
            if (oldValue == null && newValue == null)
                return;

            var type = oldValue?.FieldType ?? newValue.FieldType;

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

                if (!oldPoint.Equals2D(newPoint, 0.0000001))
                    result[name] = Tuple.Create(AimFieldDescription(oldValue), AimFieldDescription(newValue));
            }
            else if (!editableOldValue.FieldValue.Equals(editableNewValue.FieldValue))
                result[name] = Tuple.Create(AimFieldDescription(oldValue), AimFieldDescription(newValue));
        }

        static void FindValClassDelta(ValClassBase oldValue, ValClassBase newValue, Dictionary<string, Tuple<string, string>> result, string name)
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

        private static void FindFeatureRefDelta(FeatureRef oldValue, FeatureRef newValue, Dictionary<string, Tuple<string, string>> result, string name)
        {
            if ((oldValue ?? newValue) == null)
                return;

            if (oldValue != null && newValue == null)
                result[name] = Tuple.Create(oldValue.Identifier.ToString(), (string)null);
            else if (oldValue == null)
                result[name] = Tuple.Create((string)null, newValue.Identifier.ToString());
            else if (!oldValue.Identifier.Equals(newValue.Identifier))
                result[name] = Tuple.Create(oldValue.Identifier.ToString(), newValue.Identifier.ToString());
        }

        private static void FindListDelta(IList oldValue, IList newValue, Dictionary<string, Tuple<string, string>> result, string name)
        {
            if ((oldValue ?? newValue) == null)
                return;

            int index = 0;

            if (oldValue != null && newValue == null)
                foreach (var value in oldValue)
                    FindAimObjectDelta(value as IAimObject, null, result, $"{name}[{index++}]");
            else if (oldValue == null)
                foreach (var value in newValue)
                    FindAimObjectDelta(null, value as IAimObject, result, $"{name}[{index++}]");
            else
            {
                foreach (var pair in oldValue.Cast<AObject>().Zip(newValue.Cast<AObject>(), (oldVal, newVal) => new { oldVal, newVal }))
                    FindAimObjectDelta(pair.oldVal, pair.newVal, result, $"{name}[{index++}]");

                int indexBak = index;

                foreach (var value in oldValue.Cast<AObject>().Skip(index))
                    FindAimObjectDelta(value, null, result, $"{name}[{index++}]");

                foreach (var value in newValue.Cast<AObject>().Skip(indexBak))
                    FindAimObjectDelta(null, value, result, $"{name}[{indexBak++}]");
            }
        }
        private static char _delimeter = '.';

        static string GetName(string path, AimPropInfo propInfo)
        {
            if (string.IsNullOrEmpty(path))
                return propInfo.Name;

            return $"{path}{_delimeter}{propInfo.Name}";
        }

        static void FindPropertyDelta(AimPropInfo propInfo, IAimProperty oldValue, IAimProperty newValue, Dictionary<string, Tuple<string, string>> result, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                if (propInfo.Name == "Id")
                    return;
                if (propInfo.Name == "Identifier")
                    return;
                if (propInfo.Name == "TimeSlice")
                    return;
            }

            if (propInfo.PropType.AimObjectType == AimObjectType.Field)
                FindAimFieldDelta(oldValue as AimField, newValue as AimField, result, GetName(name, propInfo));
            else if (propInfo.PropType.SubClassType == AimSubClassType.ValClass)
                FindValClassDelta(oldValue as ValClassBase, newValue as ValClassBase, result, GetName(name, propInfo));
            else if (propInfo.IsList)
                FindListDelta(oldValue as IList, newValue as IList, result, GetName(name, propInfo));
            else if (propInfo.IsFeatureReference)
                FindFeatureRefDelta(oldValue as FeatureRef, newValue as FeatureRef, result, GetName(name, propInfo));
            else
                FindAimObjectDelta(oldValue as IAimObject, newValue as IAimObject, result, GetName(name, propInfo));
        }
    }

    public class SUPEntry
    {
        public SUPEntry(Guid guid, string featType, string fieldName, string oldValue, string newValue, string begin, string end)
        {
            Identifier = guid;
            FeatureType = featType;
            Identifier = guid;
            FieldName = fieldName;
            OldValue = oldValue;
            NewValue = newValue;
            BeginPosition = begin;
            EndPosition = end;
        }

        [Description("Identifier")]
        public Guid Identifier { get; set; }

        [Description("Feature type")]
        public string FeatureType { get; set; }

        [Description("Field Name")]
        public string FieldName { get; set; }

        [Description("Old Value")]
        public string OldValue { get; set; }

        [Description("New Value")]
        public string NewValue { get; set; }

        [Description("Begin Position")]
        public string BeginPosition { get; set; }

        [Description("End Position")]
        public string EndPosition { get; set; }
    }
    
}