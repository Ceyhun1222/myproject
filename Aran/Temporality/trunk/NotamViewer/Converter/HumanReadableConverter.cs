using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;

namespace NotamViewer.Converter
{
    

    public class HumanReadableConverter : IValueConverter
    {
        public static HumanReadableConverter Instance=new HumanReadableConverter();

        public static HashSet<string> DescriptionList(object value, HashSet<object> processedObjects = null)
        {

            if (processedObjects == null) processedObjects = new HashSet<object>();

            var result = new HashSet<string>();

            if (!processedObjects.Contains(value))
            {
                processedObjects.Add(value);

                //do process value

                //process lists
                if (value is IList)
                {
                    var list = value as IList;
                    foreach (var item in list)
                    {
                        var preResult = DescriptionList(item, processedObjects);
                        foreach (var item1 in preResult)
                        {
                            result.Add(item1);
                        }
                    }
                    return result;
                }

                var aimObject = value as IAimObject;
                if (aimObject != null)
                {
                    var aimPropInfoArr = AimMetadata.GetAimPropInfos(aimObject);

                    foreach (var aimPropInfo in aimPropInfoArr)
                    {
                        if (String.IsNullOrEmpty(aimPropInfo.AixmName)) continue;

                        //detect val type
                        Type valEnumType = null;
                        if (aimPropInfo.PropType.SubClassType == AimSubClassType.ValClass)
                        {
                            valEnumType = AimMetadata.GetEnumType(aimPropInfo.PropType.Properties[1].PropType.Index);
                        }

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

                        var isComplex = ((myValue is IList && (myValue as IList).Count > 0) ||
                                         (myValue is AimObject && valEnumType == null));


                        if (isComplex)
                        {
                            var preResult = DescriptionList(myValue, processedObjects);
                            foreach (var item1 in preResult)
                            {
                                result.Add(item1);
                            }

                            if (myValue is TimeSlice)
                            {
                                var description = ToHuman(myValue);

                                //if (!Cache.ContainsKey(myValue))
                                //{
                                //    Cache.Add(myValue, description);
                                //}
                                
                                result.Add(description);
                            }
                        }
                        else
                        {
                            if (myValue!=null)
                            {
                                var description = ToHuman(myValue);
                                
                                //if (!Cache.ContainsKey(myValue))
                                //{
                                //   Cache.Add(myValue, description); 
                                //}
                                
                                result.Add(description);
                            }
                        }

                    }

                    return result;
                }


                //we actually should not be here
            }

            return result;
        }

        public static string ShortAimDescription(object obj)
        {
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
                return ((dynamic)obj).Name;
            }

            var s=obj.ToString();
            return !s.Equals(obj.GetType().FullName) ? s : "";
        }

        public static string ToHuman(object value)
        {
            if (value == null) return "";
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

        //private static readonly Hashtable Cache = new Hashtable(); 

        public static void ClearCache()
        {
            //Cache.Clear();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is String) return value;

            if (value == null) return null;

            string result;

            result = ToHuman(value);

            //if (!Cache.ContainsKey(value))
            //{
            //    result = ToHuman(value);
            //    Cache.Add(value, result);
            //}
            //else
            //{
            //    result = Cache[value] as string;
            //}
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
