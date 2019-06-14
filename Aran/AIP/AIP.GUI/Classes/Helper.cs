using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIP.DB;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Temporality.CommonUtil.Context;
using Point = Aran.Geometries.Point;

namespace AIP.GUI.Classes
{

    public static class RichTextBoxExtensions
    {
        public static void AppendColorText(this RichTextBox box, string text, Color? color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            if (color != null) box.SelectionColor = (Color)color;
            box.AppendText(text);
            if (color != null) box.SelectionColor = box.ForeColor;
        }
    }

    public static class StringExtensions
    {


        /// <summary>
        /// Return path with category (eAIP, eAIC, eSUP)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pathCategory"></param>
        /// <returns></returns>
        public static string WithCategory(this string path, PathCategory pathCategory = PathCategory.eAIP)
        {
            try
            {
                return path.Replace("{CAT}", pathCategory.ToString());
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        /// <summary>
        /// Returns int from text
        /// </summary>
        /// <returns></returns>
        public static int ToInt(this string textNum)
        {
            try
            {
                Int32.TryParse(textNum, out int num);
                return num;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return 0;
            }
        }

        /// <summary>
        /// Returns double from text
        /// </summary>
        /// <returns></returns>
        public static double ToDouble(this string textNum)
        {
            try
            {
                return double.Parse(textNum, System.Globalization.CultureInfo.InvariantCulture); ;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return 0;
            }
        }

        /// <summary>
        /// Return text after char, ex AD2.EVGA => EVGA
        /// </summary>
        /// <param name="text"></param>
        /// <param name="stopAt"></param>
        /// <returns></returns>
        public static string GetAfterOrEmpty(this string text, string stopAt = ".")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                return text.Substring(text.LastIndexOf(stopAt, StringComparison.Ordinal) + stopAt.Length);
            }

            return String.Empty;
        }

        /// <summary>
        /// Return first number of chars, ex RIGID => R
        /// </summary>
        /// <param name="text"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string FirstOrEmpty(this string text, int number = 1)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                return text.Substring(0, number);
            }
            return String.Empty;
        }

        public static string ToSeparatedValues(this List<string> text, string dividedBy = ", ", int maxValues = 0, string moreEntries = null)
        {
            try
            {
                if (maxValues == 0 || maxValues >= text.Count)
                {
                    return String.Join(dividedBy, text.ToArray());
                }
                else
                {
                    if (moreEntries == null)
                        return String.Join(dividedBy, text.ToArray(), 0, maxValues);
                    else
                        return String.Join(dividedBy, text.ToArray(), 0, maxValues) + dividedBy + moreEntries;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string TrimEndChar(this string text, int numberChars = 1)
        {
            return text.Remove(text.Length - numberChars);
        }


        public static string NilIfEmpty(this string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                return Nil();
            else
                return text;
        }

        public static string ToXhtml(this string rulesProcedures_Content)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(rulesProcedures_Content))
                    return String.Empty;
                else
                {
                    rulesProcedures_Content = AIP.XML.Parser.Export(rulesProcedures_Content);
                    return rulesProcedures_Content;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string Nil()
        {
            return "NIL";
        }

        public static string ToYesNoNil(this bool? flag)
        {
            if (flag == null)
                return "NIL";
            else
                return flag == true ? "Yes" : "No";
        }

        public static string Get(this Dictionary<string, string> dict, string key)
        {
            if (dict != null && dict.ContainsKey(key))
                return dict[key];
            else
                return "";
        }

        public static string ToStringBetween(this string text, string start, string end)
        {
            string str = "";
            Regex r = new Regex(Regex.Escape(start) + "(.*?)" + Regex.Escape(end));
            MatchCollection matches = r.Matches(text);
            foreach (Match match in matches)
            {
                str = match.Groups[1].Value;
                break;
            }
            return str.Trim();
        }

        /// <summary>
        /// Convert into Data tag for save back path in to AIXM entity
        /// </summary>
        /// <param name="text"></param>
        /// <param name="feature"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public static string ToDataTag(this string text, Feature feature, string FieldName = null)
        {
            string field = FieldName != null ? FieldName : "";
            string storageName = CurrentDataContext.StorageName;
            return $@"<div data-feature='{storageName},{feature.FeatureType},{feature.Identifier},{feature.TimeSlice.SequenceNumber},{feature.TimeSlice.CorrectionNumber},{field}' style=""display:inline"">{text}</div>";
        }

        /// <summary>
        /// Returns int from text
        /// </summary>
        /// <returns></returns>
        public static string UpperFirst(this string word)
        {
            try
            {
                return char.ToUpper(word.First()) + word.Substring(1).ToLower();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string ConvertBreak(this string annotation)
        {
            try
            {
                return Regex.Replace(annotation, @"\r\n?|\n", "<br />");
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }
    }

    public static class CollectionExtentions
    {
        public static bool IsNull<T>(this T obj)
        {
            return obj == null;
        }

        public static bool IsNullOrEmpty<T>(this List<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        public static List<string> ToStringList<T>(this List<T> collection)
        {
            return collection.Select(x => x.ToString()).ToList();
        }

        public static bool IsGenericList(this object o)
        {
            var oType = o.GetType();
            return (oType.IsGenericType && (oType.GetGenericTypeDefinition() == typeof(List<>)));
        }

        public static bool ContainsValue<T>(this List<T> collection, T value)
        {
            return collection != null && collection.Count != 0 && value != null && collection.Contains(value);
        }

        public static IEnumerable<TResult> SelectManyNullSafe<TSource, TResult>(
            this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            return source.Select(selector)
                .Where(sequence => sequence != null)
                .SelectMany(x => x);
            //.Where(item => item != null);
        }

        public static T GetAttribute<T>(this SectionName value)
            where T : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name) // I prefer to get attributes this way
                .GetCustomAttributes(false)
                .OfType<T>()
                .FirstOrDefault();
        }
    }


    public class SectionNameComparer : IComparer<SectionName>
    {
        public int Compare(SectionName section1, SectionName section2)
        {
            try
            {
                int section1order = section1.GetAttribute<SectionOptionAttribute>()?.ShowOrder ?? 0;
                int section2order = section2.GetAttribute<SectionOptionAttribute>()?.ShowOrder ?? 0;
                if (section1order < section2order) return -1;
                if (section1order > section2order) return 1;
                return 0;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return 0;
            }
        }
    }

    public class NumericComparer : Comparer<string>
    {
        [DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string x, string y);

        public override int Compare(string x, string y)
        {
            return StrCmpLogicalW(x, y);
        }
    }

    public static class SectionNameExtentsions
    {
        public static string ToName(this SectionName section)
        {
            string name = section.ToString();
            if (name.StartsWith("GEN") || name.StartsWith("ENR"))
            {
                name = (String.IsNullOrEmpty(name.Substring(4))) ?
                        name.Substring(0, 3) + "-" + name.Substring(3, 1) :
                        name.Substring(0, 3) + "-" + name.Substring(3, 1) + "." + name.Substring(4);
            }
            else if (name.StartsWith("AD0") || name.StartsWith("AD1"))
            {
                name = name.Substring(0, 2) + "-" + name.Substring(2, 1) + "." + name.Substring(3);
            }

            return name;
        }

        public static bool HasParameterFlag(this SectionName section, SectionParameter parameter)
        {
            var field = typeof(SectionName).GetField(section.ToString());
            if (field?.GetCustomAttribute<SectionOptionAttribute>() == null) return false;
            return (field.GetCustomAttribute<SectionOptionAttribute>()?.ValidOn & parameter) != 0;
        }

    }

    public static class AIXMExtensions
    {
        public static NumberFormatInfo f = CultureInfo.InvariantCulture.NumberFormat;
        public enum coordtype { DDMMSSN_1, NDDMMSS_1, DDMMSS_2, NDDMMSS_2 }
        public static string ToPointString(this Aran.Geometries.Point point, int FracNum = 0)
        {
            try
            {
                //return Lib.LonToDDMMSS(point.X.ToString(), Lib.coordtype.DDMMSS_2, 2) + " " +
                //       Lib.LatToDDMMSS(point.Y.ToString(), Lib.coordtype.DDMMSS_2, 2);
                double CoordX = point.X; double CoordY = point.Y;
                string resX = ""; string resY = ""; string signX = "E";
                if (CoordX < 0)
                {
                    signX = "W"; CoordX = Math.Abs(CoordX);
                }
                string signY = "N";
                if (CoordY < 0)
                {
                    signY = "S"; CoordY = Math.Abs(CoordY);
                }

                double X = Math.Round(CoordX, 10); int degX = (int)X;
                double deltaX = Math.Round((X - degX) * 60, 8); int minX = (int)deltaX;
                deltaX = Math.Round((deltaX - minX) * 60, FracNum);

                double Y = Math.Round(CoordY, 10); int degY = (int)Y;
                double deltaY = Math.Round((Y - degY) * 60, 8); int minY = (int)deltaY;
                deltaY = Math.Round((deltaY - minY) * 60, FracNum);

                string degXSTR = "0"; string minXSTR = "0"; string secXSTR = "0";

                degXSTR = degX < 10 ? "0" + degXSTR : "0";
                degXSTR = degX < 100 ? degXSTR + degX : degX.ToString(f);
                minXSTR = minX < 10 ? minXSTR + minX : minX.ToString(f);
                secXSTR = deltaX < 10 ? secXSTR + deltaX : deltaX.ToString(f);

                string degYSTR = "0"; string minYSTR = "0"; string secYSTR = "0";

                degYSTR = degY < 10 ? degYSTR + degY : degY.ToString(f);
                minYSTR = minY < 10 ? minYSTR + minY : minY.ToString(f);
                secYSTR = deltaY < 10 ? secYSTR + deltaY : deltaY.ToString(f);

                resX = degXSTR + minXSTR + secXSTR + signX;
                resY = degYSTR + minYSTR + secYSTR + signY;

                return resY + " " + resX;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string ToPointString(this List<Aran.Geometries.Point> pointList, string dividedBy = " - ")
        {
            try
            {
                string output = "";
                List<string> strArr = new List<string>();
                foreach (Aran.Geometries.Point point in pointList) strArr.Add(point.ToPointString());
                output += string.Join(dividedBy, strArr);
                return output;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }


        public static string ToPointString(this List<Aran.Geometries.MultiPoint> multiPointList, string dividedBy = " - ")
        {
            try
            {
                string output = "";
                foreach (MultiPoint pointList in multiPointList)
                {
                    foreach (Point point in pointList)
                    {
                        output += string.Join(dividedBy, point.ToPointString());
                    }
                    output += dividedBy;
                }
                return output.TrimEndChar(dividedBy.Length);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string ToSurfacePropertiesList(this SurfaceCharacteristics sf, SectionName section = SectionName.AD212)
        {
            try
            {
                if (sf == null) return String.Empty;

                if (section == SectionName.AD216)
                    return $@"{sf.SurfaceCondition} {sf.ClassPCN}/{sf.PavementTypePCN.ToString().FirstOrEmpty()}/{sf.PavementSubgradePCN.ToString().FirstOrEmpty()}/{sf.MaxTyrePressurePCN.ToString().FirstOrEmpty()}/{sf.EvaluationMethodPCN.ToString().FirstOrEmpty()}/{sf.WeightAUW}".Replace("//", "");
                else
                    return $@"{sf.SurfaceCondition} {sf.ClassPCN}/{sf.PavementTypePCN.ToString().FirstOrEmpty()}/{sf.PavementSubgradePCN.ToString().FirstOrEmpty()}/{sf.MaxTyrePressurePCN.ToString().FirstOrEmpty()}/{sf.EvaluationMethodPCN.ToString().FirstOrEmpty()}".Replace("//", "");
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string ToUpperLower(this AirspaceVolume av)
        {
            try
            {
                if (av == null) return String.Empty;
                return $@"{av.UpperLimit?.ToValString(av.UpperLimitReference)} / {av.LowerLimit?.ToValString(av.LowerLimitReference)}";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string GetHorizontalProjectionAnnotation(this Airspace air)
        {
            try
            {
                List<string> notesList = air?.GeometryComponent?
                    .SelectManyNullSafe(x => x?.TheAirspaceVolume?
                        .Annotation?
                        .Where(n => n.PropertyName?.ToLowerInvariant().Contains("horizontalprojection") == true &&
                                    n.Purpose == CodeNotePurpose.DESCRIPTION))
                    .SelectManyNullSafe(n =>
                        n.TranslatedNote.Where(c => c.Note?.Lang == Lib.AIXMLanguage).Select(y => y.Note.Value))
                    .Distinct()
                    .ToList();
                if(notesList == null || notesList.Count == 0) return null;
                string notes = String.Join("<br />", notesList);
                return notes.ConvertBreak();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string ToHoursOfOperations(this List<Timesheet> timesheetList)
        {
            try
            {
                return Lib.GetHoursOfOperations(timesheetList);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static List<Guid?> ToGuidList<T>(this List<T> featList) where T : Feature
        {
            return featList?
                .Select(x => x?.Identifier)
                .ToList();
        }


        public static string ToValString(this ValFrequency frequency)
        {
            return $@"{frequency.Value:#.000} {frequency.Uom}";
        }

        public static string ToValString(this ValDistanceVertical val, CodeVerticalReference? cvr = null)
        {
            return val.Value == 0 && cvr == CodeVerticalReference.SFC ?
                   "GND" :
                   val.Value == 999 && val.Uom == UomDistanceVertical.FL ?
                   "UNL" :
                   val.Uom == UomDistanceVertical.FL ?
                    $@"{val.Uom} {val.Value:00#}" :
                    $@"{val.Value} {val.Uom} {cvr}";
        }
    }

    public static class OtherExtentions
    {
        /// <summary>
        /// Returns degree from double
        /// </summary>
        /// <returns></returns>
        public static string ToDegree(this double? degree)
        {
            try
            {

                if (degree == null) return "NIL";
                else
                {

                    return degree?.ToString("00#.##") + "º";
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static bool IsFileLocked(this FileInfo file)
        {
            FileStream stream = null;
            if (!file.Exists) return false; // Not existing file - not locked
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                stream?.Close();
            }

            //file is not locked
            return false;
        }

        public static int ToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }

    }
}
