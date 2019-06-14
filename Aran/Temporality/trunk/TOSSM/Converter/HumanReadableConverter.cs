using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Aim.PropertyPrecision;
using Aran.Geometries;
using Aran.Temporality.Common.Aim.MetaData;
using TOSSM.Util.Wrapper;

namespace TOSSM.Converter
{
    

    public class HumanReadableConverter : IValueConverter
    {
        public static HumanReadableConverter Instance=new HumanReadableConverter();

        public static IEnumerable<string> DescriptionList(object value)
        {
            var readonlyFeatureWrapper = value as ReadonlyFeatureWrapper;
            if (readonlyFeatureWrapper != null)
            {
                return readonlyFeatureWrapper.DescriptionList;
            }
            return new SortedSet<string>();
        }

        public static IEnumerator<string> EnumDescriptions(object value, ISet<object> processedObjects = null)
        {
            if (processedObjects == null) processedObjects = new HashSet<object>();
            if (!processedObjects.Add(value)) yield break;

            //process lists
            var list = value as IList;
            if (list != null)
            {
                foreach (var enumerator in from object item in list select EnumDescriptions(item, processedObjects))
                {
                    while (enumerator.MoveNext())
                    {
                        yield return enumerator.Current;
                    }
                }
            }

            var aimObject = value as IAimObject;
            if (aimObject == null) yield break;

            var aimPropInfoArr = AimMetadata.GetAimPropInfos(aimObject);
            foreach (var aimPropInfo in aimPropInfoArr)
            {
                if (String.IsNullOrEmpty(aimPropInfo.AixmName)) continue;
                
                //get value
                object myValue = aimObject.GetValue(aimPropInfo.Index);
                if (myValue is IEditAimField)
                {
                    myValue = (myValue as IEditAimField).FieldValue;
                }
                if (myValue is IEditChoiceClass)
                {
                    myValue = (myValue as IEditChoiceClass).RefValue;
                }
                if (myValue == null) continue;

                //detect val type
                Type valEnumType = null;
                if (aimPropInfo.PropType.SubClassType == AimSubClassType.ValClass)
                {
                    valEnumType = AimMetadata.GetEnumType(aimPropInfo.PropType.Properties[1].PropType.Index);
                }
                var isComplex = ((myValue is IList && (myValue as IList).Count > 0) ||//not empty list OR
                                 (myValue is AimObject && valEnumType == null));//aim object but not enum

                if (isComplex)
                {
                    var enumerator=EnumDescriptions(myValue, processedObjects);
                    while (enumerator.MoveNext())
                    {
                        yield return enumerator.Current;
                    }
                    if (!(myValue is TimeSlice)) continue;//skip timeslice
                    yield return ToHuman(myValue);
                }
                else
                {
                    yield return ToHuman(myValue);
                }
            }
        }

        public static string ShortAimDescription(object obj)
        {
            if (obj is AimFeature)
            {
                return ShortAimDescription((obj as AimFeature).Feature);
            }

            if (obj is Feature)
            {
                //return UIUtilities.GetFeatureDescription(obj as Feature);
            }

            if (obj == null) return "";

            if (obj.GetType().GetProperty("Designator")!=null)
            {
                return ((dynamic)obj).Designator;
            }
            if (obj.GetType().GetProperty("Name") != null)
            {
				//return ( ( dynamic ) obj ).Name;
				//string n = ( ( dynamic ) obj ).Name;				
				var name = ((dynamic)obj).Name;
				if ( name != null )
					return name.ToString ( );
				else
					return "";
            }

            var s=obj.ToString();
            return !s.Equals(obj.GetType().FullName) ? s : "";
        }

        public static string GetDescription<T>(T e) where T : IConvertible
        {
            string description = null;

            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descriptionAttributes.Length > 0)
                        {
                            // we're only getting the first description we find
                            // others will be ignored
                            description = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                        }

                        break;
                    }
                }
            }

            return description;
        }

        public static string ToHuman(object value)
        {
            if (value == null) return "";

            if (value is Enum enumValue)
            {
                var description = GetDescription(enumValue) ?? value.ToString();
                if (description.StartsWith("_")) description = description.Substring(1);
                description = description.Replace("_minus_", "-");
                description = description.Replace("_plus_", "+");
                description = description.Replace("_", " ");
                return description;
            }

            if (value is IList)
            {
                var list = value as IList;
                var count = list.Count;
                if (count==0) return "{}";
                if (count == 1) return "{ " + ToHuman(list[0]) + " }";
                return count + " items: {" + ToHuman(list[0]) + ", ... }";
            }

            if (value is TimeSlice)
            {
                var ts = value as TimeSlice;
                var result = ts.SequenceNumber + "." + ts.CorrectionNumber;

                if (ts.FeatureLifetime!=null && ts.ValidTime!=null && ts.FeatureLifetime.EndPosition==ts.ValidTime.BeginPosition)
                {
                    result+=" decommissioning at "+String.Format("{0:yyyy/MM/dd HH:mm}", ts.ValidTime.BeginPosition);
                    return result;
                }

                if (ts.ValidTime!=null)
                {
                    result += " from " + String.Format("{0:yyyy/MM/dd HH:mm}", ts.ValidTime.BeginPosition);
                    if (ts.ValidTime.EndPosition!=null)
                    {
                        result += " to " + String.Format("{0:yyyy/MM/dd HH:mm}", ts.ValidTime.EndPosition);
                    }
                }
                else
                {
                    result += " no valid time";
                }
                return result;
            }


            if (value is Point)
            {
                return "Point";
                //var point = value as Point;
                //if (point.Z>0)
                //    return "Point { X=" + point.X + " Y=" + point.Y+" Z="+point.Z+" }";
                //return "Point { X=" + point.X + " Y=" + point.Y + " }";
            }
            if (value is MultiLineString)
            {
                //var geo = value as MultiLineString;
                return "Curve";
            }
            if (value is MultiPolygon)
            {
                //var geo = value as MultiPolygon;
                return "Surface";
            }

            

            if (value is DateTime)
            {
                return String.Format("{0:yyyy/MM/dd HH:mm}", value);
            }


            var fullName = value.GetType().FullName;
            if (fullName != null && (fullName.StartsWith("System.") || value.GetType().IsEnum))
            {
                return value.ToString();
            }

            if (value is IEditValClass)
            {
                var val = (IEditValClass)value;
                var baseType = value.GetType().BaseType;
                if (baseType != null)
                {
                    var valEnumType=baseType.GetGenericArguments()[0];
                    return val.Value + " " + Enum.ToObject(valEnumType, val.Uom);
                }
            }

            return value.GetType().Name + " " + ShortAimDescription(value);
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is String) return value;

            if (value == null) return null;

            string result;

            result = ToHuman(value);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private const string Zeros="00000000000000000000000000000000000000000000000000000000000000000000000";


        public static string ToFormat(double doubleValue, PrecisionFormat format)
        {
            var formatString = format.FractionalPart > 0 ?
                   Zeros.Substring(0, Math.Max(1, format.IntegerPart)) + "."
                                                       + Zeros.Substring(0, format.FractionalPart) :
                   Zeros.Substring(0, Math.Max(1, format.IntegerPart)) + ".#####################";

            return doubleValue.ToString(formatString, CultureInfo.InvariantCulture);
        }

        public static string ToHuman(object value, PropertyConfiguration propertyConfiguration)
        {
            var doublePropertyConfiguration = propertyConfiguration as DoublePropertyConfiguration;
            if (doublePropertyConfiguration != null && value is double)
            {
                return ToFormat((double) value, new PrecisionFormat(doublePropertyConfiguration.PrecisionFormat));
            }

            var valPropertyConfiguration = propertyConfiguration as ValPropertyConfiguration;
            var valClass = value as IEditValClass;
            if (valPropertyConfiguration!=null && valClass != null)
            {
                if (valPropertyConfiguration.EnumProperties.ContainsKey(valClass.Uom))
                {
                    var baseType = value.GetType().BaseType;
                    if (baseType != null)
                    {
                        var valEnumType = baseType.GetGenericArguments()[0];
                        return ToFormat(valClass.Value,
                        new PrecisionFormat(
                            valPropertyConfiguration.EnumProperties[valClass.Uom].PrecisionFormat)) + " " + Enum.ToObject(valEnumType, valClass.Uom);
                    }
                } 
            }

            return ToHuman(value);
        }
    }
}
