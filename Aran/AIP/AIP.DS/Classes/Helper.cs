using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AIP.DataSet.Lib;
using Aran.Aim.Features;

namespace AIP.DataSet.Classes
{
    public static class DateTimeExtention
    {
        public static string ToLongFormatString(this DateTime dt)
        {
            try
            {
                return dt.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static string ToShortFormatString(this DateTime dt)
        {
            try
            {
                return dt.ToString("yyyy'-'MM'-'dd");
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }
    }

    public static class StringExtention
    {
        public static string RemoveInvalidPathChars(this string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return "";
                string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                Regex r = new Regex($"[{Regex.Escape(regexSearch)}]");
                return r.Replace(path, "");
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }
    }

    public static class EnumExtention
    {
        public static string GetEnumDescription(this Enum value)
        {
            var attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }
}
