using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using XHTML_WPF.Classes;

namespace XHTML_WPF.Classes
{
    

    public static class StringExtensions
    {

        /// <summary>
        /// Return text before char, ex AD2.EVGA => AD2
        /// </summary>
        /// <param name="text"></param>
        /// <param name="stopAt"></param>
        /// <returns></returns>
        public static string GetBeforeOrEmpty(this string text, string stopAt = ".")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return String.Empty;
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

    }
}
