using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace AIP.XML
{
    

    internal static class StringExtensions
    {

        /// <summary>
        /// Return text before char, ex AD2.EVGA => AD2
        /// </summary>
        /// <param name="text"></param>
        /// <param name="stopAt"></param>
        /// <returns></returns>
        internal static string GetBeforeOrEmpty(this string text, string stopAt = ".")
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
        internal static int ToInt(this string textNum)
        {
            try
            {
                Int32.TryParse(textNum, out int num);
                return num;
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex}");
                throw;
            }
        }

        /// <summary>
        /// Returns double from text
        /// </summary>
        /// <returns></returns>
        internal static double ToDouble(this string textNum)
        {
            try
            {
                return double.Parse(textNum, System.Globalization.CultureInfo.InvariantCulture); ;
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex}");
                throw;
            }
        }

        /// <summary>
        /// Return text after char, ex AD2.EVGA => EVGA
        /// </summary>
        /// <param name="text"></param>
        /// <param name="stopAt"></param>
        /// <returns></returns>
        internal static string GetAfterOrEmpty(this string text, string stopAt = ".")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                return text.Substring(text.LastIndexOf(stopAt, StringComparison.Ordinal) + stopAt.Length);
            }

            return String.Empty;
        }

        internal static string ToSeparatedValues(this List<string> text, string dividedBy = ", ", int maxValues = 0, string moreEntries = null)
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
                Console.WriteLine($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex}");
                throw;
            }
        }

        internal static string TrimEndChar(this string text, int numberChars = 1)
        {
            return text.Remove(text.Length - numberChars);
        }


        internal static string NilIfEmpty(this string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                return Nil();
            else
                return text;
        }

        internal static string Nil()
        {
            return "NIL";
        }

        internal static string ToYesNoNil(this bool? flag)
        {
            if (flag == null)
                return "NIL";
            else
                return flag == true ? "Yes" : "No";
        }

        internal static string Get(this Dictionary<string, string> dict, string key)
        {
            if (dict != null && dict.ContainsKey(key))
                return dict[key];
            else
                return "";
        }

        internal static string ToStringBetween(this string text, string start, string end)
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
        internal static string UpperFirst(this string word)
        {
            try
            {
                return char.ToUpper(word.First()) + word.Substring(1).ToLower();
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex}");
                throw;
            }
        }
    }

    internal static class CollectionExtentions
    {
        internal static bool IsNull<T>(this T obj)
        {
            return obj == null;
        }

        internal static bool IsNullOrEmpty<T>(this List<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        internal static List<string> ToStringList<T>(this List<T> collection)
        {
            return collection.Select(x => x.ToString()).ToList();
        }

        internal static bool IsGenericList(this object o)
        {
            var oType = o.GetType();
            return (oType.IsGenericType && (oType.GetGenericTypeDefinition() == typeof(List<>)));
        }

        internal static bool ContainsValue<T>(this List<T> collection, T value)
        {
            return collection != null && collection.Count != 0 && value != null && collection.Contains(value);
        }

        internal static IEnumerable<TResult> SelectManyNullSafe<TSource, TResult>(
            this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            return source.Select(selector)
                .Where(sequence => sequence != null)
                .SelectMany(x => x);
            //.Where(item => item != null);
        }

    }
}
