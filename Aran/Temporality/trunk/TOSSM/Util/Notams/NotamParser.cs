using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Aran.Temporality.Common.Entity;
using TOSSM.Util.Notams.Xml;
using NotamType = Aran.Temporality.Common.Entity.NotamType;

namespace TOSSM.Utils
{
    public class NotamParser
    {
        private static readonly string InvalidMessageFormat = "Invalid message format";

        public static Notam ParseTxt(string text)
        {
            Notam notam = new Notam { Text = text };
            SetNotamHeaders(notam, text);
            SetQ(notam, text);
            SetICAOCode(notam, text);
            SetBeginDate(notam, text);
            SetEndDate(notam, text);
            SetText(notam, text);
            SetLower(notam, text);
            SetUpper(notam, text);
            SetSchedule(notam, text);
            return notam;
        }

        public static Notam ParseFile(string path)
        {
            if(path.EndsWith(".txt"))
                return ParseTxtFile(path);
            if(path.EndsWith(".xml"))
                return ParseXMLFile(path);
            try
            {
                return ParseTxtFile(path);
            }
            catch (Exception e)
            {
                return ParseXMLFile(path);
            }
        }

        private static Notam ParseXMLFile(string path)
        {
            string text = ReadFile(path);
            NotamQueryResponseMessageType notamXml = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(NotamQueryResponseMessageType));
                using (StreamReader reader = new StreamReader(path))
                {
                    notamXml = (NotamQueryResponseMessageType) serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                throw new FormatException(InvalidMessageFormat);
            }

            Notam notam = new Notam { Text = text, Format = (int)NotamFormat.Xml};
            
            if(notamXml.NotamQuery?.Notam != null)
            {
                notam.Series = notamXml.NotamQuery.Notam.Series;
                notam.Number = notamXml.NotamQuery.Notam.Number;
                SetYear(notam, notamXml.NotamQuery.Notam.Year);
                switch (notamXml.NotamQuery.Notam.Type)
                {
                    case NotamTypeType.N:
                        notam.Type = (int) NotamType.N;
                        break;
                    case NotamTypeType.R:
                        notam.Type = (int)NotamType.R;
                        break;
                    case NotamTypeType.C:
                        notam.Type = (int)NotamType.C;
                        break;
                }
                ;
                notam.FIR = notamXml.NotamQuery.Notam.QLine.FIR;
                notam.Code23 = notamXml.NotamQuery.Notam.QLine.Code23;
                notam.Code45 = notamXml.NotamQuery.Notam.QLine.Code45;
                notam.Traffic = notamXml.NotamQuery.Notam.QLine.Traffic.ToString();
                notam.Purpose = notamXml.NotamQuery.Notam.QLine.Purpose.ToString();
                notam.Scope= notamXml.NotamQuery.Notam.QLine.Scope.ToString();
                notam.Lower = notamXml.NotamQuery.Notam.QLine.Lower;
                notam.Upper = notamXml.NotamQuery.Notam.QLine.Upper;
                notam.Coordinates = notamXml.NotamQuery.Notam.Coordinates;
                notam.Radius = notamXml.NotamQuery.Notam.Radius;
                notam.ICAO = notamXml.NotamQuery.Notam.ItemA.Length > 0? notamXml.NotamQuery.Notam.ItemA[0]:"";
                notam.StartValidity = ConvertToDateTime(notamXml.NotamQuery.Notam.StartValidity);
                notam.EndValidity = ConvertToDateTime(notamXml.NotamQuery.Notam.EndValidity);
                notam.EndValidityEst = notamXml.NotamQuery.Notam.EstimationSpecified;
                notam.ItemE = notamXml.NotamQuery.Notam.ItemE;
                notam.ItemF = notamXml.NotamQuery.Notam.ItemF;
                notam.ItemG = notamXml.NotamQuery.Notam.ItemG;
                notam.Schedule= notamXml.NotamQuery.Notam.ItemD;
                return notam;
            }

            throw new FormatException(InvalidMessageFormat);

        }

        private static Notam ParseTxtFile(string path)
        {
            string text = ReadFile(path);

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            return ParseTxt(regex.Replace(text.Trim(), " "));
        }

        private static string ReadFile(string path)
        {
            if (!File.Exists(path))
                throw new FormatException("File is empty");
            string text = File.ReadAllText(path);
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
                throw new FormatException("File is empty");
            return text;
        }


        private static void SetSchedule(Notam notam, string text)
        {
            try
            {
                SetValues(text, @"D\)( DAILY)?(( [0-2]\d[0-6]\d-[0-2]\d[0-6]\d)+|( SR-SS))", (result) =>
                {
                    notam.Schedule = result.Contains(" DAILY") ? result.Substring(9) : result.Substring(3);
                });
            }
            catch
            {
                // ignored
            }
        }


        private static void SetUpper(Notam notam, string text)
        {
            try
            {
                SetValues(text, @"G\) (UNL|(\d+(M|FT) (AMSL|AGL))|(FL\d+))", (result) =>
                {
                    notam.ItemG = result.Substring(3);
                });
            }
            catch
            {
                // ignored
            }
        }

        private static void SetLower(Notam notam, string text)
        {
            try
            {
                SetValues(text, @"F\) (GND|SFC|(\d+(M|FT) (AMSL|AGL))|(FL\d+))", (result) =>
                {
                    notam.ItemF = result.Substring(3);
                });
            }
            catch
            {
                // ignored
            }
        }

        private static void SetText(Notam notam, string text)
        {
            SetValues(text, @"E\) (.|\n)+", (result) =>
            {
                var index = result.IndexOf("F) ");
                notam.ItemE = index > 0 ? result.Substring(0, index).Substring(3) : result.Substring(3);
            });
        }

        private static void SetBeginDate(Notam notam, string text)
        {
            SetValues(text, @"B\) [0-9]{2}[01][0-9][0-3][0-9][0-2][0-9][0-6][0-9]", (result) =>
            {
                notam.StartValidity = ConvertToDateTime(result.Split(' ')[1]);
            });
        }

        private static void SetEndDate(Notam notam, string text)
        {

            if (notam.Type != (int)NotamType.C)
                SetValues(text, @"C\) [0-9]{2}[01][0-9][0-3][0-9][0-2][0-9][0-6][0-9]( EST)?", (result) =>
                {
                    var parts = result.Split(' ');
                    notam.EndValidity = ConvertToDateTime(result.Split(' ')[1]);
                    if (parts.Length > 2)
                        notam.EndValidityEst = true;
                });

        }

        private static void SetICAOCode(Notam notam, string text)
        {
            SetValues(text, @"A\) [A-Z]{4}", (result) =>
            {
                notam.ICAO = result.Split(' ')[1];
            });
        }

        private static void SetNotamHeaders(Notam notam, string text)
        {
            SetValues(text, @"(?![ST])[A-Z]\d{4}/\d{2} NOTAM[N,R,C]( (?![ST])[A-Z]\d{4}/\d{2})?", (result) =>
            {
                var split = result.Split(' ');
                SetId(notam, split[0]);
                SetType(notam, split[1]);
                if (split.Length == 3)
                    notam.RefNotam = split[2];
            });



        }

        private static void SetId(Notam notam, string text)
        {
            SetValues(text, @"(?![ST])[A-Z]\d{4}/\d{2}", (result) =>
            {
                var parts = result.Split('/');
                notam.Series = parts[0].Substring(0, 1);
                notam.Number = parts[0].Substring(1, 4);
                SetYear(notam, parts[1]);
            });
        }

        private static void SetYear(Notam notam, string txt)
        {
            var year = Int32.Parse(txt);
            notam.Year = year >= 70 ? (year + 1900) : (year + 2000);
        }

        private static void SetType(Notam notam, string text)
        {
            SetValues(text, @"NOTAM[N,R,C]", (result) =>
            {
                notam.Type = (int)Enum.Parse(typeof(NotamType),result.Substring(5));
            });

        }

        private static void SetQ(Notam notam, string text)
        {
            SetValues(text, @"Q\) *[A-Z\d]{4} */ *Q[A-Z]{4} */ *[IVK]{1,3} */ *[NBOMK]{1,5} */ *[AEWK]{1,4} */ *\d+ */ *\d+ */ *[0-9]{1,5}[NS][0-9]{1,5}[EW][0-9]{1,3}", (result) =>
            {
                var qString = result.Substring(3).Trim();
                var parts = qString.Split('/');
                notam.FIR = parts[0].Trim();
                notam.Code23 = parts[1].Trim().Substring(1, 2);
                notam.Code45 = parts[1].Trim().Substring(3);
                notam.Traffic = parts[2].Trim();
                notam.Purpose = parts[3].Trim();
                notam.Scope = parts[4].Trim();
                notam.Lower = parts[5].Trim();
                notam.Upper = parts[6].Trim();
                notam.Coordinates = parts[7].Trim().Substring(0, 11);
                notam.Radius = parts[7].Trim().Substring(11);
            });

        }

        private static void SetValues(string text, string regEx, Action<string> action)
        {
            try
            {
                Regex regex = new Regex(regEx);
                var match = regex.Match(text);
                if (match.Success)
                {
                    action(match.Value);
                }
                else
                    throw new FormatException(InvalidMessageFormat);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public static DateTime ConvertToDateTime(string text)
        {
            var year = Int32.Parse(text.Substring(0, 2));
            year = (year >= 70 ? (year + 1900) : (year + 2000));
            System.DateTime dtDateTime = new DateTime(year, Int32.Parse(text.Substring(2, 2)), Int32.Parse(text.Substring(4, 2)), Int32.Parse(text.Substring(6, 2)), Int32.Parse(text.Substring(8)), 0, 0, System.DateTimeKind.Utc);
            return dtDateTime;
        }

    }
}
