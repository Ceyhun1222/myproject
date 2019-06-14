using AIP.DB;
using AIP.XML;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Ionic.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xaml;
using System.Xml.Linq;
using AIP.BaseLib.Airac;
using AIP.BaseLib.Class;
using AIP.GUI.Classes;
using AIP.GUI.Properties;
using Aran.Geometries;
using Aran.Temporality.CommonUtil.Context;
using EntityFramework.Extensions;
using HtmlAgilityPack;
using Npgsql;
using AirportHeliport = Aran.Aim.Features.AirportHeliport;
using Amendment = AIP.DB.Amendment;
using AmendmentType = AIP.DB.AmendmentType;
using Crc32 = AIP.GUI.Classes.Crc32;
using Description = AIP.DB.Description;
using eAIP = AIP.DB.eAIP;
using Group = AIP.DB.Group;
using HtmlDocument = System.Windows.Forms.HtmlDocument;
using Point = Aran.Geometries.Point;

namespace AIP.GUI
{

    internal static class Lib
    {
        internal static string CurrentDir, CurrentDataDir, LayoutFile, SourceDir, SourceDirCat, SourceDirTemplate, SourceCatDirTemplate, TargetDir, TargetPdfDir, SourcePdfDir, TargetDirTemplate, OutputDir, RemoteOutputDir, OutputDirTemplate, MakeAIPFile, MakeAIPWorkingDir, ExtMakeAIPFile, ExtMakeAIPWorkingDir, AIPLanguage;

        internal static language AIXMLanguage;
        internal static eAIPOptions DBOptions;
        internal static eAIP CurrentAIP, PrevousAIP;
        internal static eAISpackage CurrentAIS, PrevousAIS;
        internal static List<Feature> PermDelta;
        internal static Dictionary<string, string> MenuLang, Lang, TplLang;
        internal static List<Guid> Airspace_FIR_Guid_List;
        internal static bool ShowDBLog = false;
        internal static string DBLogFile = Path.Combine(Directory.GetCurrentDirectory(), "Log", "DB.log");
        public static List<string> Sections = new List<string>(); //{ "GEN-0.1", "GEN-0.2", "GEN-0.3", "GEN-0.4", "GEN-0.5", "GEN-0.6", "GEN-1.1", "GEN-1.2", "GEN-1.3", "GEN-1.4", "GEN-1.5", "GEN-1.6", "GEN-1.7", "GEN-2.1", "GEN-2.2", "GEN-2.3", "GEN-2.4", "GEN-2.5", "GEN-2.6", "GEN-2.7", "GEN-3.1", "GEN-3.2", "GEN-3.3", "GEN-3.4", "GEN-3.5", "GEN-3.6", "GEN-4.1", "GEN-4.2", "ENR-0.1", "ENR-0.2", "ENR-0.3", "ENR-0.4", "ENR-0.5", "ENR-0.6", "ENR-1.1", "ENR-1.2", "ENR-1.3", "ENR-1.4", "ENR-1.5", "ENR-1.6", "ENR-1.7", "ENR-1.8", "ENR-1.9", "ENR-1.10", "ENR-1.11", "ENR-1.12", "ENR-1.13", "ENR-1.14", "ENR-2.1", "ENR-2.2", "ENR-3.1", "ENR-3.2", "ENR-3.3", "ENR-3.4", "ENR-3.5", "ENR-3.6", "ENR-4.1", "ENR-4.2", "ENR-4.3", "ENR-4.4", "ENR-4.5", "ENR-5.1", "ENR-5.2", "ENR-5.3", "ENR-5.4", "ENR-5.5", "ENR-5.6", "ENR-6", "AD-0.1", "AD-0.2", "AD-0.3", "AD-0.4", "AD-0.5", "AD-0.6", "AD-1.1", "AD-1.2", "AD-1.3", "AD-1.4", "AD-1.5" };
        public static Dictionary<string, string> SectionList = new Dictionary<string, string>();
        public static Dictionary<int, List<AirportHeliport>> AirportHeliports = new Dictionary<int, List<AirportHeliport>>();
        public static int TempAdNum = 24;
        public static System.Windows.Application XhtmlControl;
        public static int sectionsDebug = Properties.Settings.Default.SectionNumberToGen; // 0 to off, or set first N
        public static Dictionary<SectionParameter, List<SectionName>> SectionWithAttribute = new Dictionary<SectionParameter, List<SectionName>>();
        
        static Lib()
        {
            try
            {

                //foreach (CodeRuleProcedureTitle suit in (CodeRuleProcedureTitle[])Enum.GetValues(typeof(CodeRuleProcedureTitle)))
                //{
                //    Console.WriteLine($@"[Description(""GEN 1.2, {("*" + suit.ToString()).Replace("*OTHER_", "OTHER:").Replace("*", "")}"")]");
                //    Console.WriteLine(suit.ToString() + @",");
                //}

                InitDir();
                Sections = TextSectionList();
                Regex rgx = new Regex("[^a-zA-Z0-9]");
                foreach (string sec in Sections)
                {
                    SectionList.Add(rgx.Replace(sec, ""), sec);
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }



        private static void InitDir()
        {
            try
            {
                CurrentDir = CurrentDirFunc();
                LayoutFile = Path.Combine(CurrentDir, "UserLayout.xml");
                CurrentDataDir = Path.Combine(CurrentDir, "Data");
                //SourceDirTemplate = CurrentDataDir + @"eAIP-source\{DATE}\";
                SourceDirTemplate = Path.Combine(CurrentDataDir, "eAIP-source", "{DATE}");
                SourceCatDirTemplate = Path.Combine(SourceDirTemplate, "{CAT}");
                OutputDirTemplate = Path.Combine(CurrentDataDir, "eAIP-output", "{DATE}");
                TargetDirTemplate = Path.Combine(OutputDirTemplate, "html", "{CAT}");
                MakeAIPWorkingDir = Path.Combine(CurrentDataDir, "eAIP-pack");
                ExtMakeAIPWorkingDir = Path.Combine(CurrentDataDir, "eAIP-extended");

                if (!Directory.Exists(CurrentDataDir)) Directory.CreateDirectory(CurrentDataDir);
                if (IsHashChanged(Properties.Resources.Data, Path.Combine(CurrentDataDir, "data.hash")))
                {
                    ExtractFiles(Properties.Resources.Data, CurrentDataDir);
                }
                //if (!Directory.Exists(MakeAIPWorkingDir))
                //{
                //    if (Directory.Exists(CurrentDir + @"Data\"))
                //    {
                //        ExtractFiles(Properties.Resources.eAIP_pack, CurrentDir + @"Data\");
                //    }
                //}
                //if (!Directory.Exists(ExtMakeAIPWorkingDir))
                //{
                //    if (Directory.Exists(CurrentDir + @"Data\"))
                //    {
                //        ExtractFiles(Properties.Resources.eAIP_extpack, CurrentDir + @"Data\");
                //    }
                //}
                MakeAIPFile = Path.Combine(MakeAIPWorkingDir, "MakeAIP-core.bat");
                ExtMakeAIPFile = Path.Combine(ExtMakeAIPWorkingDir, "MakeAIP-extended.bat");
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static bool IsHashChanged(byte[] file, string fileName)
        {
            try
            {
                string hash = Hash.GetMd5(file);
                if (!File.Exists(fileName) || File.ReadAllText(fileName) != hash)
                {
                    File.WriteAllText(fileName, hash);
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        public static string CurrentDirFunc()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static void Use<T>(this T item, Action<T> act)
        {
            act(item);
        }

        public enum coordtype { DDMMSSN_1, NDDMMSS_1, DDMMSS_2, NDDMMSS_2 }
        public static string LonToDDMMSS(string X_Lon, coordtype coorT, int FracNum)
        {
            string res = "";
            try
            {
                double Coord = Convert.ToDouble(X_Lon);
                if (Coord == 0) Coord = Convert.ToDouble(X_Lon);
                string sign = "E";
                if (Coord < 0)
                {
                    sign = "W";
                    Coord = Math.Abs(Coord);
                }

                double X = Math.Round(Coord, 10);

                int deg = (int)X;
                double delta = Math.Round((X - deg) * 60, 8);

                int min = (int)delta;
                delta = Math.Round((delta - min) * 60, FracNum);

                string degSTR = "0";
                string minSTR = "0";
                string secSTR = "0";

                degSTR = deg < 10 ? "0" + degSTR : "0";
                degSTR = deg < 100 ? degSTR + deg : deg.ToString();
                minSTR = min < 10 ? minSTR + min : min.ToString();
                secSTR = delta < 10 ? secSTR + delta : delta.ToString();

                switch (coorT)
                {
                    case coordtype.DDMMSSN_1:
                        res = degSTR + "°" + minSTR + "'" + secSTR + "\"" + sign;
                        break;
                    case coordtype.NDDMMSS_1:
                        res = sign + " " + degSTR + "°" + minSTR + "'" + secSTR + "\"";
                        break;
                    case coordtype.DDMMSS_2:
                        res = degSTR + minSTR + secSTR + sign;
                        break;
                    case coordtype.NDDMMSS_2:
                        res = sign + " " + degSTR + minSTR + secSTR;
                        break;
                    default:
                        break;
                }

                //res = degSTR + "°" + minSTR + "'" + secSTR + "'" + "'" + sign;

            }
            catch (Exception ex) { }

            return res;
        }

        public static string LatToDDMMSS(string Y_Lat, coordtype coorT, int FracNum)
        {
            string res = "";
            try
            {
                double Coord = Convert.ToDouble(Y_Lat);
                if (Coord == 0) Coord = Convert.ToDouble(Y_Lat);

                string sign = "N";
                if (Coord < 0)
                {
                    sign = "S";
                    Coord = Math.Abs(Coord);
                }

                double Y = Math.Round(Coord, 10);

                int deg = (int)Y;
                double delta = Math.Round((Y - deg) * 60, 8);

                int min = (int)delta;
                delta = Math.Round((delta - min) * 60, FracNum);

                string degSTR = "0";
                string minSTR = "0";
                string secSTR = "0";

                degSTR = deg < 10 ? degSTR + deg.ToString() : deg.ToString();
                minSTR = min < 10 ? minSTR + min.ToString() : min.ToString();
                secSTR = delta < 10 ? secSTR + delta.ToString() : delta.ToString();

                switch (coorT)
                {
                    case coordtype.DDMMSSN_1:
                        res = degSTR + "°" + minSTR + "'" + secSTR + "\"" + sign;
                        break;
                    case coordtype.NDDMMSS_1:
                        res = sign + " " + degSTR + "°" + minSTR + "'" + secSTR + "\"";
                        break;
                    case coordtype.DDMMSS_2:
                        res = degSTR + minSTR + secSTR + sign;
                        break;
                    case coordtype.NDDMMSS_2:
                        res = sign + " " + degSTR + minSTR + secSTR;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex) { }

            return res;
        }

        public static string ConvertAIXMEnum2AIP<T>(Nullable<T> enumToConvert) where T : struct
        {
            switch (typeof(T).ToString())
            {
                case "Aran.Aim.Enums.CodeDirection":
                    if (enumToConvert.ToString() == "FORWARD") return "Forwards";
                    else if (enumToConvert.ToString() == "BACKWARD") return "Backwards";
                    else if (enumToConvert.ToString() == "BOTH") return "Both";
                    break;
                case "Aran.Aim.Enums.CodeATCReporting":
                    if (enumToConvert.ToString() == "COMPULSORY") return "Compulsory";
                    else if (enumToConvert.ToString() == "ON_REQUEST") return "Request";
                    else if (enumToConvert.ToString() == "NO_REPORT") return "No-report";
                    break;
            }
            return "";
        }

        public static List<RouteSegment> SortSegments(List<RouteSegment> relatedSegments, bool ShowErrors = true)
        {
            try
            {
                if (relatedSegments == null || relatedSegments.Count < 2) return relatedSegments;

                //first is the one that not second
                var endIds = relatedSegments.Select(t => GetPointId(t.End)).ToList();

                var independent = relatedSegments.Where(t => !endIds.Contains(GetPointId(t.Start))).ToList();
                if (independent.Count == 0)
                {
                    if (ShowErrors)
                        throw new Exception("Can not find first segment");
                }
                if (independent.Count > 1)
                {
                    if (ShowErrors)
                        throw new Exception("Several first segments detected");
                }

                var result = new List<RouteSegment>();
                var current = independent.First();
                while (true)
                {
                    result.Add(current);
                    var id = GetPointId(current.End);

                    var next = relatedSegments.Where(t => GetPointId(t.Start) == id).ToList();
                    if (next.Count == 0)
                    {
                        break;
                    }

                    if (next.Count > 1)
                    {
                        if (ShowErrors)
                            throw new Exception("Several next segments detected");
                    }

                    current = next.First();
                }

                if (result.Count != relatedSegments.Count)
                {
                    if (ShowErrors)
                        throw new Exception("Can not recreate segment sequence");
                }

                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static Guid? GetPointId(EnRouteSegmentPoint point)
        {
            try
            {
                if (point != null)
                {
                    switch (point.PointChoice.Choice)
                    {
                        case SignificantPointChoice.DesignatedPoint:
                            return point.PointChoice.FixDesignatedPoint.Identifier;
                        case SignificantPointChoice.Navaid:
                            return point.PointChoice.NavaidSystem.Identifier;
                        case SignificantPointChoice.TouchDownLiftOff:
                            return point.PointChoice.AimingPoint.Identifier;
                        case SignificantPointChoice.RunwayCentrelinePoint:
                            return point.PointChoice.RunwayPoint.Identifier;
                        case SignificantPointChoice.AirportHeliport:
                            return point.PointChoice.AirportReferencePoint.Identifier;
                        case SignificantPointChoice.AixmPoint:
                            //no id here
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }


        public static bool IsOnLanguage(RulesProcedures rp)
        {
            try
            {
                //if (rp.Annotation?.FirstOrDefault().PropertyName.Contains("Language") && 
                //    rp.Annotation?.FirstOrDefault().TranslatedNote?.FirstOrDefault().Note?.Lang == language.ENG)
                return true;

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        //public static void FileGenerate_(string filename, eAISpackage ais, string path)
        //{
        //    if (filename == "eAIP")
        //    {
        //        filename = ais.ICAOcountrycode.ToUpperInvariant() + @"-" +
        //            filename + @"-" +
        //            ais.lang + ".xml";
        //        List<string> lines = new List<string>();

        //        string text = Properties.Resources.Speciman_eAIP;
        //        text = text
        //            .Replace("{COUNTRY}", ais.ICAOcountrycode.ToUpperInvariant())
        //            .Replace("{LANG}", ais.lang)
        //            .Replace("{PUBDATE}", ais.Publicationdate?.ToString("yyyy-MM-dd"))
        //            .Replace("{COMPANY}", ais.Publishingorganisation)
        //            .Replace("{EFDATE}", ais.Effectivedate.ToString("yyyy-MM-dd"));
        //        File.WriteAllText(path + @"\" + filename, text);

        //        text = Properties.Resources.Locales;
        //        File.WriteAllText(path + @"\..\" + ais.ICAOcountrycode.ToUpperInvariant() + @"-locales.xml", text);

        //        foreach (string xmlfile in Sections)
        //        {
        //            filename = ais.ICAOcountrycode.ToUpperInvariant() + @"-" +
        //            xmlfile + @"-" +
        //            ais.lang + ".xml";
        //            File.Create(path + @"\" + filename);
        //        }
        //    }
        //}

        public static void FileGenerate(string filename, eAISpackage ais, string path)
        {
            if (filename == "eAIP")
            {
                filename = ais.ICAOcountrycode.ToUpperInvariant() + @"-" +
                           filename + @"-" +
                           ais.lang + ".xml";
                string ICAOCountryCode = ais.ICAOcountrycode.ToUpperInvariant();

                List<string> airList = GetAIXMAirportHeliport()
                    .Where(x => x.Type == CodeAirportHeliport.AD || x.Type == CodeAirportHeliport.AH).Select(x => x.LocationIndicatorICAO).ToList();
                List<string> helList = GetAIXMAirportHeliport()
                    .Where(x => x.Type == CodeAirportHeliport.HP).Select(x => x.LocationIndicatorICAO).ToList();
                IEnumerable<string> airhelList = airList.Concat(helList);

                eAIPMainXml xml = new eAIPMainXml();
                xml.Company = ais.Publishingorganisation;
                xml.Lang = ais.lang;
                xml.Edition = ais.eAIPpackage.eAIP.Edition;
                xml.Country = ais.ICAOcountrycode.ToUpperInvariant();
                xml.PublicationDate = ais.Publicationdate?.ToString("yyyy-MM-dd");
                xml.EffectiveDate = ais.Effectivedate.ToString("yyyy-MM-dd");
                xml.Sections = Sections.ToList();

                xml.AirportList = airList;
                xml.HeliportList = helList;

                string output = Razor.Run(xml);
                File.WriteAllText(Path.Combine(path, filename), output);

                if (IsHashChanged(Encoding.UTF8.GetBytes(Properties.Resources.locales),
                    Path.Combine(Path.Combine(Directory.GetParent(path).FullName, "locales.hash"))))
                {
                    string text = Properties.Resources.locales;
                    File.WriteAllText(Path.Combine(Directory.GetParent(path).FullName, "locales.xml"), text);
                }
                // Generate all files except for AD2 and AD3
                foreach (string xmlfile in Sections)
                {
                    CreateEmptyFile(Path.Combine(path, $@"{ICAOCountryCode}-{xmlfile}-{ais.lang}.xml"));
                }

                // Generate AD2 and AD3 files
                foreach (string airHel in airhelList)
                {
                    CreateEmptyFile(Path.Combine(path, $@"{ICAOCountryCode}-AD-{airHel.ToUpperInvariant()}-{ais.lang}.xml"));
                }
            }
        }

        //public static void FileGenerate(string filename, eAISpackage ais, string path)
        //{
        //    if (filename == "eAIP")
        //    {
        //        filename = ais.ICAOcountrycode.ToUpperInvariant() + @"-" +
        //                   filename + @"-" +
        //                   ais.lang + ".xml";
        //        List<string> lines = new List<string>();
        //        string ICAOCountryCode = ais.ICAOcountrycode.ToUpperInvariant();

        //        List<string> airList = GetAIXMAirportHeliport()
        //            .Where(x => x.Type == CodeAirportHeliport.AD || x.Type == CodeAirportHeliport.AH).Select(x => x.LocationIndicatorICAO).ToList();
        //        List<string> helList = GetAIXMAirportHeliport()
        //            .Where(x => x.Type == CodeAirportHeliport.HP).Select(x => x.LocationIndicatorICAO).ToList();
        //        IEnumerable<string> airhelList = airList.Concat(helList);

        //        Speciman spec = new Speciman();
        //        spec.Company = ais.Publishingorganisation;
        //        spec.Lang = ais.lang;
        //        spec.Edition = ais.eAIPpackage.eAIP.Edition;
        //        spec.Country = ais.ICAOcountrycode.ToUpperInvariant();
        //        spec.PublicationDate = ais.Publicationdate?.ToString("yyyy-MM-dd");
        //        spec.EffectiveDate = ais.Effectivedate.ToString("yyyy-MM-dd");
        //        spec.Sections = Sections.ToList();

        //        spec.AirportList = airList;
        //        spec.HeliportList = helList;

        //        Templates.Speciman tpl = new Templates.Speciman();
        //        tpl.Session = new Dictionary<string, object>();
        //        tpl.Session.Add("Data", spec);
        //        tpl.Initialize();
        //        string output = tpl.TransformText();

        //        File.WriteAllText(Path.Combine(path, filename), output);

        //        string text = Properties.Resources.Locales;
        //        //File.WriteAllText(path + @"\..\" + ais.ICAOcountrycode.ToUpperInvariant() + @"-locales.xml", text);
        //        File.WriteAllText(Path.Combine(Directory.GetParent(path).FullName, "locales.xml"), text);

        //        // Generate all files except for AD2 and AD3
        //        foreach (string xmlfile in Sections)
        //        {
        //            CreateEmptyFile(Path.Combine(path, $@"{ICAOCountryCode}-{xmlfile}-{ais.lang}.xml"));
        //        }

        //        // Generate AD2 and AD3 files
        //        foreach (string airHel in airhelList)
        //        {
        //            CreateEmptyFile(Path.Combine(path, $@"{ICAOCountryCode}-AD-{airHel.ToUpperInvariant()}-{ais.lang}.xml"));
        //        }
        //    }
        //}

        public static bool CreateEmptyFile(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    File.Create(fileName);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        public static void ExtractFiles(byte[] byte_array, string OutputPath)
        {
            try
            {
                using (ZipFile zip = ZipFile.Read(new MemoryStream(byte_array)))
                {
                    foreach (ZipEntry x in zip)
                    {
                        string newPath = Path.Combine(OutputPath, x.FileName);

                        if (x.IsDirectory)
                        {
                            Directory.CreateDirectory(newPath);
                        }
                        else
                        {
                            using (FileStream stream = new FileStream(newPath, FileMode.Create))
                                x.Extract(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowMessage(ex.Message, true);
            }
        }

        public static List<Feature> GetPermDelta()
        {
            try
            {
                Feature feat = Globals.GetFeaturesByED(FeatureType.RulesProcedures).OfType<RulesProcedures>().Where(n => n.Category == CodeRuleProcedure.RULE).FirstOrDefault();
                (feat as RulesProcedures).Content = (feat as RulesProcedures).Content.Replace("Standards and Recommended", "Recommended and Standards").Replace("types of information subjects.", "types of information subjects. This old sentence must be removed!").Replace("Integrated ", "");
                return new List<Feature>() { feat };
            }
            catch (Exception ex)
            {
                ErrorLog.ShowMessage(ex.Message, true);
                return null;
            }
        }

        public enum PathType
        {
            None,
            OutputFolder,
            OutputIndex,
            SourceFolder,
        }

        public static string GetPath(PathType PathType, eAISpackage cais = null)
        {
            try
            {
                eAISpackage ais = cais ?? Lib.CurrentAIS;
                if (ais == null) return null;
                string txt_airac = IsAIRAC(ais.Effectivedate) ? "-AIRAC" : "";
                string yyyyMMdd = ais.Effectivedate.ToString("yyyy-MM-dd");

                if (PathType == PathType.SourceFolder)
                {
                    return SourceDirTemplate.Replace("{DATE}", yyyyMMdd + txt_airac);
                }
                else if (PathType == PathType.OutputFolder || PathType == PathType.OutputIndex)
                {
                    string str = OutputDirTemplate.Replace("{DATE}", yyyyMMdd + txt_airac);
                    if (PathType == PathType.OutputFolder) return str;
                    else return Path.Combine(str, "html", $@"index-{Lib.CurrentAIP.lang}.html");
                }
                return "";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static string AdditionalParams(string Arguments)
        {
            try
            {
                if (Properties.Settings.Default.MakeAIPDebug)
                    return Arguments + " -debug";
                else
                    return Arguments;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return Arguments;
            }
        }

        public static string PdfLang(string lang)
        {
            try
            {
                return lang.Substring(0, 2);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static bool IsAIRAC(DateTime dt)
        {
            try
            {
                return AiracCycle.AiracCycleList.Any(d => d.Airac == dt);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        public static string GetComparedContent(string from, string to, string IDName, string Ref)
        {
            try
            {
                HtmlDiff.HtmlDiff diff = new HtmlDiff.HtmlDiff(@from, to);
                diff.IgnoreWhitespaceDifferences = true;
                diff.GenerateID = true;
                diff.IDName = IDName;
                diff.Ref = Ref;

                // Lets add a block expression to group blocks we care about (such as dates)
                // diffHelper.AddBlockExpression(new Regex(@"[\d]{1,2}[\s]*(Jan|Feb)[\s]*[\d]{4}", RegexOptions.IgnoreCase));

                return diff.BuildXml("e");
            }
            catch (Exception ex)
            {
                ErrorLog.ShowMessage(ex.Message, true);
                return null;
            }
        }

        public static bool ComparePdfdContent(string from, string to)
        {
            try
            {

                HtmlDiff.HtmlDiff diff = new HtmlDiff.HtmlDiff(@from, to);
                diff.IgnoreWhitespaceDifferences = true;
                diff.GenerateID = true;
                diff.IDName = "1";
                diff.Ref = "1";

                // Lets add a block expression to group blocks we care about (such as dates)
                // diffHelper.AddBlockExpression(new Regex(@"[\d]{1,2}[\s]*(Jan|Feb)[\s]*[\d]{4}", RegexOptions.IgnoreCase));

                return diff.Build("e") == to;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowMessage(ex.Message, true);
                return false;
            }
        }

        /// <summary>
        /// Find differencies in the 2 files and insert inserted and deleted tags entire result file
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <param name="IDName"></param>
        /// <param name="Ref"></param>
        /// <returns>true, if changes are available. Else return false</returns>
        public static bool GetComparedFile(string file1, string file2, string IDName, string Ref)
        {
            try
            {
                if (File.Exists(file1) && File.Exists(file2))
                {
                    string from = File.ReadAllText(file1);
                    string to = File.ReadAllText(file2);
                    string res = GetComparedContent(@from, to, IDName, Ref);
                    string file_res = file1.Substring(0, file1.Length - 2);
                    File.WriteAllText(file_res, res);

                    // By conception not possible that last AIP
                    // and file with changes can have equal length.
                    // file_res should be bigger, if some changes made
                    //return (file2.Length != file_res.Length); // true if files aren`t equal
                    return (FileLength(file2) != FileLength(file_res)); // true if files aren`t equal

                    //return file_res;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowMessage(ex.Message, true);
                return false;
            }
        }

        public static long FileLength(string path)
        {
            try
            {
                if (File.Exists(path))
                    return new FileInfo(path).Length;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return 0;
            }
        }

        public static SectionName GetSectionName(string section)
        {
            try
            {
                SectionName sn = SectionName.None;
                if (section?.Contains(".") == true)
                {
                    Enum.TryParse(section.GetBeforeOrEmpty(), out sn);
                }
                else
                    Enum.TryParse(section, out sn);
                //if (sn == SectionName.None)
                //    ErrorLog.ShowWarning($@"Error in the GetSectionName. No such class added: {section}");

                return sn;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return SectionName.None;
            }
        }



        public static DB.AirportHeliport GetAirportHeliport(string Name)
        {
            try
            {
                string airHelTxt = "";
                if (Name.Contains(".")) // It is AD2.XXXX or AD3.XXXX
                {
                    string[] arr = Name.Split('.');
                    if (arr.Length == 2) airHelTxt = arr[1];
                }
                else
                    return null;

                using (eAIPContext db = new eAIPContext())
                {
                    DB.AirportHeliport airHel = db.AirportHeliport.FirstOrDefault(x => x.LocationIndicatorICAO == airHelTxt);
                    return airHel;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static SubClassType GetSubclassName(string subclass)
        {
            try
            {
                SubClassType sn = SubClassType.None;
                Enum.TryParse(subclass, out sn);
                //if (sn == SectionName.None)
                //    ErrorLog.ShowWarning($@"Error in the GetSectionName. No such class added: {section}");

                return sn;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return SubClassType.None;
            }
        }



        public static eAIP CreateAmendment(eAIP eaip, eAIPContext db)
        {
            try
            {
                eAIP aip = db.eAIP.Where(n => n.id == eaip.id).Include("Amendment.Group.Description").FirstOrDefault();

                if (aip?.Amendment == null)
                {
                    //db.Database.Log = Console.WriteLine;
                    Amendment amdt = new Amendment();
                    Group grp = new Group();
                    List<Group> GroupList = new List<Group>();
                    List<Description> DescriptionList = new List<Description>();
                    Description dscr = new Description();

                    amdt.SubClassType = SubClassType.Amendment;
                    amdt.Type = (AiracCycle.AiracCycleList.Any(d => d.Airac == eaip.Effectivedate)) ? AmendmentType.AIRAC : AmendmentType.NonAIRAC;

                    amdt.Number = ""; // Empty while not recognized as available status
                    amdt.AmendmentStatus = AmendmentStatus.NotAvailable;

                    amdt.Year = eaip.Effectivedate.Year.ToString();
                    amdt.Publicationdate = eaip.Publicationdate?.ToString("yyyy-MM-dd");
                    amdt.Effectivedate = eaip.Effectivedate.ToString("yyyy-MM-dd");
                    amdt.Group = new List<Group>();
                    amdt.eAIPID = eaip.id;
                    amdt.Description = dscr;

                    dscr.Title = "Miscellaneous";
                    grp.Description = dscr;
                    grp.Type = GroupType.Misc;
                    grp.eAIPID = eaip.id;
                    GroupList.Add(grp);
                    DescriptionList.Add(dscr);

                    grp = new Group();
                    dscr = new Description();
                    dscr.Title = "GEN";
                    grp.Description = dscr;
                    grp.Type = GroupType.GEN;
                    grp.eAIPID = eaip.id;
                    GroupList.Add(grp);
                    DescriptionList.Add(dscr);

                    grp = new Group();
                    dscr = new Description();
                    dscr.Title = "ENR";
                    grp.Description = dscr;
                    grp.Type = GroupType.ENR;
                    grp.eAIPID = eaip.id;
                    GroupList.Add(grp);
                    DescriptionList.Add(dscr);

                    grp = new Group();
                    dscr = new Description();
                    dscr.Title = "AD";
                    grp.Description = dscr;
                    grp.Type = GroupType.AD;
                    grp.eAIPID = eaip.id;

                    GroupList.Add(grp);
                    DescriptionList.Add(dscr);
                    amdt.Group = GroupList;
                    aip.Amendment = amdt;

                    db.Description.AddRange(DescriptionList);
                    db.Group.AddRange(GroupList);
                    db.Amendment.Add(amdt);

                    // Giving number
                    DB.eAIP PreviousAmdtAIP = db.eAIP.Where(n => n.Amendment != null && n.Effectivedate < aip.Effectivedate && n.lang == aip.lang)
                        .OrderByDescending(n => n.Effectivedate)
                        .Include("Amendment.Group.Description")
                        .FirstOrDefault();

                    aip.Amendment.AmendmentStatus = AmendmentStatus.Available;
                    if (PreviousAmdtAIP?.Amendment == null || PreviousAmdtAIP.Effectivedate.Year != aip.Effectivedate.Year)
                    {
                        aip.Amendment.Number = "001";
                    }
                    else
                    {
                        int number = (PreviousAmdtAIP.Amendment.Number == "") ? 0 : System.Convert.ToInt32(PreviousAmdtAIP.Amendment.Number);
                        number++;
                        aip.Amendment.Number = number.ToString("D3");
                    }

                    db.SaveChanges();
                    return aip;

                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        //public static XML.GEN01 Dataset2XML(eAIPContext db, string sectionT, Dataset.eAIP eAIP)
        //{
        //    XML.GEN01 XML_AIP = new XML.GEN01();
        //    Dataset.GEN01 Dataset_AIP = db.GEN01.Where(n => n.eAIPID == eAIP.id && n.SectionStatus == SectionStatusEnum.Filled).FirstOrDefault();

        //    if (Dataset_AIP == null)
        //    {
        //        return null;
        //    }
        //    XML_AIP.id = sectionT;
        //    XML_AIP.Title = new Title();
        //    string title = Dataset_AIP.Title?.ToString() ?? "PREFACE";
        //    XML_AIP.Title.Items = new string[] { title };
        //    XML_AIP.Title.Updated = "No";


        //    List<XML.Subsection> SubsectionList = new List<XML.Subsection>();
        //    List<XML.Designatedpoint> lst = new List<XML.Designatedpoint>();
        //    foreach (Dataset.Subsection item in Dataset_AIP.Children.OrderBy(n => n.OrderNumber).ToList())
        //    {
        //        XML.Subsection ent = new XML.Subsection();
        //        ent.Title = SetValue<XML.Title>(item.Title);
        //        ent.Items = AddSubsectionText(item.Content);
        //        SubsectionList.Add(ent);
        //    }
        //    XML_AIP.Subsection = SubsectionList.ToArray();
        //    return XML_AIP;
        //}


        public static T SetValue<T>(string value) where T : class, new()
        {
            if (value == null) return null;
            T newprop = new T();
            PropertyInfo propInfo = newprop.GetType().GetProperty("Items");
            propInfo.SetValue(newprop, new object[] { value });
            return newprop;
        }

        public static object[] AddSubsectionText(string text)
        {
            return new object[] { new div() { Items = new object[] { text } } };
        }


        //public static string AIPSectionToClass(string sectionname)
        //{
        //    try
        //    {
        //        return SectionList.FirstOrDefault(x => x.Value == sectionname).Key;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
        //        return null;
        //    }
        //}

        public static string AIPClassToSection(string classname)
        {
            try
            {
                if (classname.Contains(".")) // AD2
                    return classname;
                else
                    return SectionList.FirstOrDefault(x => x.Key == classname).Value;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }
        public static void InstallTexts(bool isNewInstallation = false)
        {
            try
            {
                LanguageText LT;
                List<LanguageText> LTList;


                // Removing old entries
                using (eAIPContext lang = new eAIPContext())
                {
                    // Slow method
                    //lang.LanguageTexts.RemoveRange(lang.LanguageTexts);
                    //lang.SaveChanges();

                    // Faster method
                    if (lang.LanguageReference.Any() && isNewInstallation)
                    {
                        DialogResult dialogResult = MessageBox.Show(@"Database is already contains language texts. Do you want to clear all and install default phrases?", @"Question", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (eAIPContext.ConnectionFactory == DefaultConnectionFactory.MSSQL)
                                lang.Database.ExecuteSqlCommand("TRUNCATE TABLE [LanguageText]");
                            else
                                lang.Database.ExecuteSqlCommand("TRUNCATE TABLE public.\"LanguageText\"");
                        }
                        else
                            return;
                    }


                    // Inserting all menu texts from xml files
                    LTList = new List<LanguageText>();
                    List<LanguageReference> LRList = lang.LanguageReference.AsNoTracking().ToList();
                    List<LanguageText> ExistingList = (isNewInstallation)
                        ? new List<LanguageText>()
                        : lang.LanguageTexts.AsNoTracking().ToList();
                    foreach (LanguageReference lng in LRList)
                    {
                        // Installing from Menu file
                        string file = Path.Combine(Lib.CurrentDir, "Settings", $@"menu_{lng.Value}.xml");// $@"{Lib.CurrentDir}\Settings\menu_{lng.Value}.xml";
                        if (File.Exists(file))
                        {
                            XDocument doc = XDocument.Load(file);
                            foreach (XElement node in doc.Descendants("Nodes").Where(n => n.Attribute("Name") != null && n.Attribute("Text") != null))
                            {
                                string name = node.Attribute("Name")?.Value.Trim();

                                LT = new LanguageText();
                                LT.LanguageCategory = LanguageCategory.Menu; // Menu
                                LT.LanguageReferenceId = lng.id;
                                LT.Name = name;
                                if (!isNewInstallation && NameExist(ExistingList, LT)) continue;
                                //Regex withoutCategory = new Regex(@"(GEN|ENR|AD) [0-9]\.?[0-9]{0,2}? ");
                                //LT.Value = withoutCategory.Replace(node.Attribute("Text")?.Value.Trim() ?? "", "");
                                LT.Value = (string)node.Attribute("Text")?.Value.Trim();
                                LTList.Add(LT);
                            }
                        }

                        // Istalling from Lang file
                        file = Path.Combine(Lib.CurrentDir, "Settings", $@"lang_{lng.Value}.xml"); //$@"{Lib.CurrentDir}\Settings\lang_{lng.Value}.xml";
                        if (File.Exists(file))
                        {
                            XDocument doc = XDocument.Load(file);
                            foreach (XElement node in doc.Descendants("Lang").Where(n => n.Attribute("Name") != null && n.Attribute("Text") != null))
                            {
                                LT = new LanguageText();
                                LT.LanguageCategory = LanguageCategory.Text; // Text
                                LT.LanguageReferenceId = lng.id;
                                LT.Name = (string)node.Attribute("Name")?.Value.Trim();
                                if (!isNewInstallation && NameExist(ExistingList, LT)) continue;
                                LT.Value = (string)node.Attribute("Text")?.Value.Trim();
                                LTList.Add(LT);
                            }
                        }

                        // Istalling from Lang file
                        file = Path.Combine(Lib.CurrentDir, "Settings", $@"template_{lng.Value}.xml");
                        if (File.Exists(file))
                        {
                            XDocument doc = XDocument.Load(file);
                            foreach (XElement node in doc.Descendants("Lang").Where(n => n.Attribute("Name") != null && n.Value != null))
                            {
                                LT = new LanguageText();
                                LT.LanguageCategory = LanguageCategory.Template; // Text
                                LT.LanguageReferenceId = lng.id;
                                LT.Name = (string)node.Attribute("Name")?.Value.Trim();
                                if (!isNewInstallation && NameExist(ExistingList, LT)) continue;
                                LT.Value = (string)node.Value.Trim();
                                LTList.Add(LT);
                            }
                        }
                    }
                    lang.LanguageTexts.AddRange(LTList);
                    lang.SaveChanges();
                    GetLanguageTexts();
                    ErrorLog.ShowInfo("Language texts successfully saved");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static bool NameExist(List<LanguageText> existingList, LanguageText lt)
        {
            try
            {
                if (existingList.Any(x =>
                        x.LanguageCategory == lt.LanguageCategory &&
                        x.LanguageReferenceId == lt.LanguageReferenceId &&
                        x.Name == lt.Name))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return true;
            }
        }

        public static void GetLanguageTexts()
        {
            try
            {
                using (eAIPContext lang = new eAIPContext())
                {
                    int? LR_ID = lang.LanguageReference.Where(n => n.Value == Lib.AIPLanguage)?.AsNoTracking().FirstOrDefault()?.id;

                    Lib.MenuLang = new Dictionary<string, string>();
                    Lib.Lang = new Dictionary<string, string>();
                    Lib.TplLang = new Dictionary<string, string>();

                    if (LR_ID != null)
                    {

                        // lang.Database.Log = Console.WriteLine;
                        List<LanguageText> lt = lang.LanguageTexts?.Where(
                            n => n.LanguageCategory == LanguageCategory.Menu &&
                                 n.LanguageReference.id == LR_ID).AsNoTracking().ToList();
                        Lib.MenuLang = lt?.
                                 ToDictionary(n => n.Name, n => n.Value);

                        lt = lang.LanguageTexts?.Where(
                            n => n.LanguageCategory == LanguageCategory.Text &&
                                 n.LanguageReference.id == LR_ID).AsNoTracking().ToList();
                        Lib.Lang = lt?.
                                 ToDictionary(n => n.Name, n => n.Value);

                        lt = lang.LanguageTexts?.Where(
                            n => n.LanguageCategory == LanguageCategory.Template &&
                                 n.LanguageReference.id == LR_ID).AsNoTracking().ToList();
                        Lib.TplLang = lt?.
                            ToDictionary(n => n.Name, n => n.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static string GetText(string key, string notFoundPhrase = "")
        {
            try
            {
                if (Lib.Lang.ContainsKey(key))
                    return Lib.Lang[key];
                else
                    return notFoundPhrase;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }


        /// <summary>
        /// Get text for translate
        /// Text is getting by name: EnumName_Value
        /// If no entry found, empty string returns
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumText(Enum enumValue)
        {
            try
            {
                string key = $@"{enumValue.GetType().Name}_{enumValue}";
                if (Lib.Lang.ContainsKey(key))
                    return Lib.Lang[key];
                else
                    return String.Empty;

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }
        

        /// <summary>
        /// Translate section,
        /// For example: GEN311 must be translated into "GEN 3.1.1 LangText"
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string HeaderWithNumbering(string key)
        {
            try
            {
                string text = GetText(key);
                var index = key.StartsWith("AD") ? 2 : 3; // Else GEN or ENR
                var name = key.Substring(0, index) + " " + string.Join(".", key.Substring(index).ToCharArray());
                return $@"<span class=""Numbering"">{name} </span>  {text}";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string TemplateReplace(string section, Dictionary<string, string> data)
        {
            try
            {
                string file = @"Templates\AIP\" + section + ".html";
                if (File.Exists(file))
                {
                    string sb = File.ReadAllText(file);
                    foreach (string k in data.Keys)
                    {
                        sb = sb.Replace(k, data[k]);
                    }
                    return sb;
                }
                else return "";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        private static string ReplaceImages(string xhtml, DB.eAIP eAIP, string section = null)
        {
            try
            {
                var doc = new HtmlAgilityPack.HtmlDocument()
                {
                    OptionOutputOriginalCase = true,
                    OptionCheckSyntax = false,
                    OptionOutputAsXml = false,
                    OptionFixNestedTags = true,
                    OptionWriteEmptyNodes = true,
                    OptionAutoCloseOnEnd = true
                };
                doc.LoadHtml(xhtml);
                var imgs = doc.DocumentNode.SelectNodes("//img");
                if (imgs != null)
                    foreach (HtmlNode img in imgs)
                    {
                        var base64str = img.Attributes["src"]?.Value.GetAfterOrEmpty(";base64,");
                        if (base64str == null) break;
                        var _type = img.Attributes["src"]?.Value.ToStringBetween("data:", ";base64,");
                        var type = img.Attributes["src"]?.Value == null ? "" : $"Type=\"{_type}\"";
                        var fileExt = _type.GetAfterOrEmpty("/");
                        var width = img.Attributes["width"]?.Value == null ? "" : $"Width=\"{ImgDimensionsRepair(img.Attributes["width"]?.Value)}\"";

                        var height = img.Attributes["height"]?.Value == null ? "" : $"Height=\"{ImgDimensionsRepair(img.Attributes["height"]?.Value)}\"";
                        var hash = DbUtility.GetMd5Hash(base64str);
                        var fileName = section == null ? $@"IMG_{hash}.{fileExt}" : $@"{section}_{hash}.{fileExt}";
                        string txt_airac = Lib.IsAIRAC(eAIP.Effectivedate) ? "-AIRAC" : "";
                        string dateAip = eAIP.Effectivedate.ToString("yyyy-MM-dd") + txt_airac;

                        string sourceDir = Path.Combine(Lib.SourceDirTemplate.Replace("{DATE}", dateAip), "graphics", "eAIP");
                        string outputDir = Path.Combine(Lib.OutputDirTemplate.Replace("{DATE}", dateAip), "graphics", "eAIP");

                        if (!Directory.Exists(sourceDir)) Directory.CreateDirectory(sourceDir);
                        if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);
                        var spath = Path.Combine(sourceDir, fileName);
                        var tpath = Path.Combine(outputDir, fileName);

                        byte[] imgarr = Convert.FromBase64String(base64str);
                        File.WriteAllBytes(spath, imgarr);
                        File.Copy(spath, tpath, true);

                        //Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imgarr));
                        //if (img.Attributes["width"]?.Value == image.Width.ToString() && img.Attributes["height"]?.Value == image.Height.ToString())
                        //{
                        //    width = height = "";
                        //}
                        var GraphicFile =
                            doc.CreateTextNode(
                                $"<e:Graphic-file xlink:href=\"../graphics/eAIP/{fileName}\" {type} {width} {height} xlink:show=\"embed\" />");
                        img.ParentNode.ReplaceChild(GraphicFile, img);
                    }
                // <e:Graphic-file xlink:href="../graphics/eAIP/ENR-6-AerialActivities.svg" Type="image/png" Width="300" Height="200" xlink:show="embed"/>
                return doc.DocumentNode.InnerHtml; // InnerHtml, because without <xml> header
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        private static string ImgDimensionsRepair(string dmn)
        {
            try
            {
                return Math.Round(dmn.ToDouble()).ToString();

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static string ConvertToAIPXhtml(string xhtml, DB.eAIP eAIP, string section = null)
        {
            try
            {
                //xhtml = Parser.Export(xhtml); // Convert to export format for both html and pdf
                xhtml = ReplaceImages(xhtml, eAIP, section); // Convert data/image to file
                xhtml = ReplaceSpecialObjects(xhtml, eAIP, section); // Convert AIP File links to links with saving files to disk

                return xhtml
                    .Replace("<p>&nbsp;</p>", "<br />") // Unknown &nbsp; for xml
                    .Replace("<span>&nbsp;</span>", "<br />")// Unknown &nbsp; for xml
                    .Replace("&nbsp;", " ")// Unknown &nbsp; for xml
                    .Replace("<", "<x:")
                    .Replace("<x:/", "</x:")
                    .Replace("<x:x:", "<x:")
                    .Replace("<x:e:", "<e:")
                    .Replace("</x:e:", "</e:")
                    .Replace("<x:div xmlns=\"http://www.w3.org/1999/xhtml\">", "<x:div>"); // Fixes for xhtml
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static string AIXM_GetNotes(List<Note> annotation)
        {
            try
            {
                if (annotation != null)
                {
                    List<string> notesList = annotation.SelectManyNullSafe(n =>
                            n.TranslatedNote.Where(c => c.Note?.Lang == Lib.AIXMLanguage).Select(x => x.Note.Value))
                            .Distinct()
                            .ToList();
                    string notes = String.Join("<br />", notesList);
                    return notes.ConvertBreak();
                }
                else return "";

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string AIXM_GetNotesByPurpose(List<Note> annotation, CodeNotePurpose purpose, bool? isNotEqual = null)
        {
            try
            {
                if (annotation != null)
                {
                    bool Condition(Note x) => isNotEqual == true ? (int?)x.Purpose != (int)purpose : (int?)x.Purpose == (int)purpose;

                    List<string> notesList = annotation
                        .Where(Condition)
                        .SelectManyNullSafe(n => n.TranslatedNote.Where(c => c.Note?.Lang == Lib.AIXMLanguage))
                        .Select(n => n.Note.Value)
                        .Distinct()
                        .ToList();
                    string notes = String.Join("<br />", notesList);
                    return notes.ConvertBreak();
                }
                else return "";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }



        public static string AIXM_GetNotesByPurpose(List<Note> annotation, List<CodeNotePurpose?> purposeList)
        {
            try
            {
                if (annotation != null)
                {
                    List<string> notes_list = annotation
                    .Where(x => purposeList.Contains(x.Purpose))
                    .SelectManyNullSafe(n => n.TranslatedNote.Where(c => c.Note?.Lang == Lib.AIXMLanguage)
                    .Select(x => x.Note.Value))
                    .Distinct()
                    .ToList();

                    string notes = String.Join("<br />", notes_list);
                    return notes.ConvertBreak();
                }
                else return "";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string AIXM_GetNotesByCategory(List<CategoryWithNotes> Data)
        {
            try
            {
                string output = "";
                foreach (CategoryWithNotes val in Data)
                {
                    output += $@"{val.Category}: ";
                    List<string> notes_list = val.Annotation
                        .SelectManyNullSafe(n => n.TranslatedNote.Where(c => c.Note?.Lang == Lib.AIXMLanguage)
                            .Select(x => x.Note.Value))
                        .Distinct()
                        .ToList();
                    output += String.Join("<br />", notes_list);
                }
                return output.ConvertBreak();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string AIXM_GetNotesByPropertyNameAndPurpose(List<Note> annotation, string propertyName, List<CodeNotePurpose?> purposeList)
        {
            try
            {
                if (annotation != null)
                {
                    List<string> notes_list = annotation
                        .Where(n => n.PropertyName.ToLowerInvariant().Contains(propertyName.ToLowerInvariant()) &&
                                    purposeList.Contains(n.Purpose))
                        .SelectManyNullSafe(n => n.TranslatedNote.Where(c => c.Note?.Lang == Lib.AIXMLanguage)
                        .Select(x => x.Note.Value))
                        .Distinct()
                        .ToList();
                    string notes = String.Join("<br />", notes_list);
                    return notes.ConvertBreak();
                }
                else return "";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string AIXM_GetNotesByPropertyName(List<Note> annotation, string propertyName)
        {
            try
            {
                if (annotation != null)
                {
                    List<string> notes_list = annotation
                        .Where(n => n.PropertyName != null && n.PropertyName.ToLowerInvariant().Contains(propertyName.ToLowerInvariant()))
                        .SelectManyNullSafe(n => n.TranslatedNote?.Where(c => c.Note?.Lang == Lib.AIXMLanguage)
                        .Select(x => x.Note?.Value))
                        .Distinct()
                        .ToList();
                    string notes = String.Join("<br />", notes_list);
                    return notes.ConvertBreak();
                }
                else return "";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string AIXM_GetNotesByPropertyName(List<Note> annotation, List<string> propertyNameList)
        {
            try
            {
                if (annotation != null)
                {
                    List<string> notes_list = annotation
                        .Where(n => n.PropertyName != null &&
                               propertyNameList.Select(x => x.ToLowerInvariant()).Contains(n.PropertyName.ToLowerInvariant()))
                        .SelectManyNullSafe(n => n.TranslatedNote?.Where(c => c.Note?.Lang == Lib.AIXMLanguage)
                        .Select(x => x.Note?.Value))
                        .Distinct()
                        .ToList();
                    string notes = String.Join("<br />", notes_list);
                    return notes.ConvertBreak();
                }
                else return "";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }


        public static string w(string text)
        {
            return (text == null) ? "" : text;
        }

        public static List<Unit> GetUnitByType(CodeUnit type)
        {
            try
            {
                return Globals.GetFeaturesByED(FeatureType.Unit).Cast<Unit>().Where(n => n.Type == type && IsLanguageNote(n.Annotation)).ToList();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static bool IsLanguageNote(List<Note> Annotation)
        {
            try
            {
                //return true;
                if (Annotation?.AsEnumerable().SelectManyNullSafe(n => n.TranslatedNote)?.Any(x => x.Note.Lang == Lib.AIXMLanguage) == true)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        public static string GetAvailabilities(List<PropertiesWithSchedule> PropertiesWithScheduleList)
        {
            string output = "";
            try
            {
                if (PropertiesWithScheduleList == null || PropertiesWithScheduleList.Count == 0 || PropertiesWithScheduleList.All(n => n.TimeInterval == null))
                    return "H24";
                else
                {
                    foreach (PropertiesWithSchedule Pws in PropertiesWithScheduleList)
                    {
                        output += GetHoursOfOperations(Pws.TimeInterval);
                    }
                }
                return output;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return output;
            }
        }

        public static string GetHoursOfOperations(List<Timesheet> TimesheetList)
        {
            string output = "";
            try
            {
                if (TimesheetList == null || TimesheetList.Count == 0)
                    return "H24";
                else
                {
                    foreach (Timesheet ts in TimesheetList)
                    {
                        if (ts.StartDate != "" || ts.EndDate != "")
                        {
                            output += $@"{ts.StartDate}-{ts.EndDate} ";
                        }
                        if (ts.Day != null || ts.DayTil != null)
                        {
                            if (ts.Day != null && ts.DayTil != null)
                            {
                                output += $@"{ts.Day?.ToString()}-{ts.DayTil?.ToString()} ";
                            }
                            else if (ts.Day != null)
                            {
                                output += $@"{ts.Day?.ToString()} ";
                            }

                        }
                        if (ts.StartTime != null || ts.EndTime != null)
                        {
                            if (ts.StartTime == ts.EndTime || (ts.StartTime == "00:00" && ts.EndTime == "24:00"))
                            {
                                output += "H24 ";
                            }
                            else
                                output += $@"{ts.StartTime}-{ts.EndTime} ";
                        }
                    }
                }
                return output;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return output;
            }
        }

        //public static string GetServiceOperationalStatus(List<Timesheet> TimesheetList)
        //{
        //    string output = "";
        //    try
        //    {
        //        if (TimesheetList == null || TimesheetList.Count == 0)
        //            return "H24";
        //        else
        //        {
        //            foreach (Timesheet ts in TimesheetList)
        //            {
        //                if (ts.StartDate != "" || ts.EndDate != "")
        //                {
        //                    output += $@"{ts.StartDate}-{ts.EndDate} ";

        //                }
        //                if (ts.Day != null || ts.DayTil != null)
        //                {
        //                    if (ts.Day != null && ts.DayTil != null)
        //                    {
        //                        output += $@"{ts.Day?.ToString()}-{ts.DayTil?.ToString()} ";
        //                    }
        //                    else if (ts.Day != null)
        //                    {
        //                        output += $@"{ts.Day?.ToString()} ";
        //                    }

        //                }
        //                if (ts.StartTime != null || ts.EndTime != null)
        //                {
        //                    if (ts.StartTime == ts.EndTime || (ts.StartTime == "00:00" && ts.EndTime == "24:00"))
        //                    {
        //                        output += "H24 ";
        //                    }
        //                    else
        //                        output += $@"{ts.StartTime}-{ts.EndTime} ";
        //                }
        //            }
        //        }
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
        //        return "";
        //    }
        //}


        public static List<AirportHeliport> GetAIXMAirportHeliport()
        {
            try
            {
                if (CurrentAIP != null && !AirportHeliports.ContainsKey(CurrentAIP.id))
                {
                    AirportHeliports[CurrentAIP.id] = Globals.GetFeaturesByED(FeatureType.AirportHeliport)?.Cast<Aran.Aim.Features.AirportHeliport>().OrderBy(x => x.LocationIndicatorICAO)
                        .ToList();
                    return AirportHeliports[CurrentAIP.id];
                }
                else if (CurrentAIP != null && AirportHeliports.ContainsKey(CurrentAIP.id))
                {
                    return AirportHeliports[CurrentAIP.id];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }


        public static string CRC32(string filePath)
        {
            try
            {
                Crc32 crc32 = new Crc32();
                String hash = String.Empty;

                using (FileStream fs = File.Open(filePath, FileMode.Open))
                    foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToLower();

                return hash;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static string SHA1(string filePath)
        {
            string hash = "";
            try
            {
                Stream stream = (Stream)File.Open(filePath, FileMode.Open);

                using (var bufferedStream = new BufferedStream(stream, 1024 * 64))
                {
                    var sha = new System.Security.Cryptography.SHA1Managed();// SHA1Managed(); //SHA256Managed();
                    byte[] checksum = sha.ComputeHash(bufferedStream);
                    hash = BitConverter.ToString(checksum).Replace("-", String.Empty);
                }
                return hash;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string SHA1(byte[] byteArray)
        {
            string hash = "";
            try
            {
                Stream stream = new MemoryStream(byteArray);

                using (var bufferedStream = new BufferedStream(stream, 1024 * 64))
                {
                    var sha = new System.Security.Cryptography.SHA1Managed();// SHA1Managed(); //SHA256Managed();
                    byte[] checksum = sha.ComputeHash(bufferedStream);
                    hash = BitConverter.ToString(checksum).Replace("-", String.Empty);
                }
                return hash;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static DateTime? GetServerDate()
        {
            try
            {
#if PGSQL
                return Globals.GetServerDate();
#else
                string connectionString =
                        System.Configuration.ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand(@"SELECT GETUTCDATE() as curdate", con))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                return reader.GetDateTime(0);
                            }
                        }
                    }
                }
                return null;
#endif
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        internal static string GetLang(int lang, bool fullName = true)
        {
            try
            {
                using (eAIPContext db = new eAIPContext())
                {
                    DbQuery<LanguageReference> tmp = db.LanguageReference
                        .AsNoTracking();
                    var Langs = (fullName) ?
                        tmp.Select(x => new { x.id, x.Name }).ToDictionary(x => x.id, x => x.Name) :
                        tmp.Select(x => new { x.id, x.Value }).ToDictionary(x => x.id, x => x.Value)
                        ;

                    if (Langs.ContainsKey(lang)) return (fullName) ? Langs[lang] : Langs[lang].GetBeforeOrEmpty("-").ToUpperInvariant();
                }
                return "";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }

        }

        internal static int? GetLangIdByValue(string langValue)
        {
            try
            {
                using (eAIPContext db = new eAIPContext())
                {
                    return (db.LanguageReference?
                        .AsNoTracking())?
                        .FirstOrDefault(x => x.Value == langValue)?
                        .id;
                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }

        }

        internal static string SaveAIPFile(AIPFile aipFile, bool autoDestination = true, bool overWrite = false)
        {
            try
            {
                if (aipFile?.AIPFileData?.Data == null) return "";
                string fileName = "";
                if (autoDestination) // "../graphics/eAIP/"
                {
                    string outputDir = Path.Combine(OutputDir, "graphics", "eAIP");
                    if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);
                    fileName = Path.Combine(outputDir, AIPFileName(aipFile));
                    //if(!overWrite) fileName = GetUniqueFilePath(fileName);
                    File.WriteAllBytes(fileName, aipFile?.AIPFileData?.Data);

                    // for pdf version of webpage - pdf should be later generated and include existing pdf file
                    string sourceDir = Path.Combine(SourceDir, "graphics", "eAIP");
                    if (!Directory.Exists(sourceDir)) Directory.CreateDirectory(sourceDir);
                    string sourceFileName = Path.Combine(sourceDir, AIPFileName(aipFile));
                    //if (!overWrite) sourceFileName = GetUniqueFilePath(sourceFileName);
                    File.WriteAllBytes(sourceFileName, aipFile?.AIPFileData?.Data);
                }
                else
                {
                    FolderBrowserDialog folderDlg = new FolderBrowserDialog { ShowNewFolderButton = true };
                    DialogResult result = folderDlg.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        File.WriteAllBytes(Path.Combine(folderDlg.SelectedPath, AIPFileName(aipFile)), aipFile?.AIPFileData?.Data);
                    }
                }

                return fileName;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        /// <summary>
        /// If filename is "Test (1).pdf" then it will become "Test (2).pdf".
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string GetUniqueFilePath(string filepath)
        {
            if (File.Exists(filepath))
            {
                string folder = Path.GetDirectoryName(filepath);
                string filename = Path.GetFileNameWithoutExtension(filepath);
                string extension = Path.GetExtension(filepath);
                int number = 1;

                Match regex = Regex.Match(filepath, @"(.+) \((\d+)\)\.\w+");

                if (regex.Success)
                {
                    filename = regex.Groups[1].Value;
                    number = int.Parse(regex.Groups[2].Value);
                }

                do
                {
                    number++;
                    if (folder != null) filepath = Path.Combine(folder, $"{filename} ({number}){extension}");
                }
                while (File.Exists(filepath));
            }

            return filepath;
        }

        public static bool IsAIPSelected()
        {
            try
            {
                if (Lib.CurrentAIS == null)
                {
                    ErrorLog.ShowInfo($@"No active eAIS package selected.{Environment.NewLine}Please select eAIS package from ""eAIS packages"" window and click ""Active""");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        public static string XhtmlNil()
        {
            return "<p>NIL</p>";
        }

        internal static string AIPFileName(AIPFile aipFile)
        {
            try
            {
                string air = string.IsNullOrEmpty(aipFile.AirportHeliport) ? "" : $@"_{aipFile.AirportHeliport}";
                string langCode = aipFile.LanguageReferenceId == null
                    ? "en-GB"
                    : Lib.GetLang(aipFile.LanguageReferenceId.GetValueOrDefault(), false);
                return $@"{aipFile.id}_{Lib.CurrentAIP.ICAOcountrycode}{air}_{aipFile.SectionName}_{langCode}.{aipFile.FileName.GetAfterOrEmpty()}";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        internal static string GetFileType(string fileName)
        {
            try
            {
                string ext = fileName.GetAfterOrEmpty(".").ToLowerInvariant();
                if (ext == "pdf") return "application/pdf";
                else return "";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }

        }

        public static Dictionary<string, GridViewOption> GridViewAttributes<T>()
        {
            try
            {
                Dictionary<string, GridViewOption> dict = new Dictionary<string, GridViewOption>();
                PropertyInfo[] props = typeof(T).GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    List<GridViewOption> attrs = prop.GetCustomAttributes(true).OfType<GridViewOption>().ToList();
                    foreach (GridViewOption attr in attrs)
                    {
                        dict.Add(prop.Name, attr);
                    }
                }
                return dict;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static List<SectionName> SectionByAttribute(SectionParameter flag)
        {
            try
            {
                // Cache return
                //if (SectionWithAttribute.ContainsKey(flag))
                //{
                //    return SectionWithAttribute[flag];
                //}
                var sections = typeof(SectionName)
                    .GetFields()
                    .Select(x => new
                    {
                        att = x.GetCustomAttributes(false)
                            .OfType<SectionOptionAttribute>()
                            .FirstOrDefault(),
                        member = x
                    })
                    .Where(x => x.att != null && x.att.ValidOn.HasFlag(flag))
                    .OrderBy(x => x.att.GenerateOrder)
                    .Select(x => (SectionName)x.member.GetValue(null))
                    .ToList();
                // Cache fill
                //SectionWithAttribute.Add(flag, sectionsDebug == 0 ? sections : sections.Take(sectionsDebug).ToList());
                //return SectionWithAttribute[flag];
                return sectionsDebug == 0 ? sections : sections.Take(sectionsDebug).ToList();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static List<string> TextSectionList()
        {
            try
            {
                List<SectionName> sectionList = typeof(SectionName)
                    .GetFields()
                    .Select(x => new
                    {
                        att = x.GetCustomAttributes(false)
                            .OfType<SectionOptionAttribute>()
                            .FirstOrDefault(),
                        member = x
                    })
                    .Where(x => x.att != null && x.att.ValidOn.HasFlag(SectionParameter.TextSection))
                    .OrderBy(x => x.att.ShowOrder)
                    .Select(x => (SectionName)x.member.GetValue(null))
                    .ToList();
                List<string> testList = new List<string>();
                foreach (SectionName section in sectionList)
                {
                    testList.Add(section.ToName());
                }
                return testList;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static List<SectionName> AllSectionsWithAttributes()
        {
            try
            {
                return typeof(SectionName)
                    .GetFields()
                    .Select(x => new
                    {
                        att = x.GetCustomAttributes(false)
                            .OfType<SectionOptionAttribute>()
                            .FirstOrDefault(),
                        member = x
                    })
                    .Where(x => x.att != null && x.att.ValidOn.HasFlag(SectionParameter.None))
                    .Select(x => (SectionName)x.member.GetValue(null))
                    .ToList();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        /// <summary>
        /// Check that template`s class is empty
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsNull<T>(T data)
        {
            try
            {
                Type type = typeof(T);
                // Is List have 0 elements?
                if (data.IsGenericList())
                {
                    if (data is ICollection col)
                        return col.Count == 0;
                }
                else // Is any field in the class have non empty, non-Nil, or non-H24 value (H24 may be removed, if it is require)
                {
                    foreach (FieldInfo pi in type.GetFields())
                    {
                        object value = pi.GetValue(data);
                        if ((value is string && !string.IsNullOrEmpty(value.ToString()) && value != "NIL" && value != "H24"))
                            return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        public static Control FindControl(Control root, string id)
        {
            try
            {
                if (root.Name == id) return root;
                foreach (Control c in root.Controls)
                {
                    Control t = FindControl(c, id);
                    if (t != null) return t;
                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static void LoadXhtmlControlResources()
        {
            try
            {
                if (XhtmlControl == null)
                {
                    XhtmlControl = new System.Windows.Application
                    {
                        ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown
                    };
                    List<string> dict = new List<string>()
                    {
                        "System.Windows.xaml",
                        "Telerik.Windows.Controls.xaml",
                        "Telerik.Windows.Controls.RibbonView.xaml",
                        "Telerik.Windows.Controls.RichTextBoxUI.xaml",
                        "Telerik.Windows.Controls.GridView.xaml"
                    };
                    foreach (var dic in dict)
                    {
                        XhtmlControl.Resources.MergedDictionaries.Add(
                            System.Windows.Application.LoadComponent(
                                new Uri($@"/Telerik.Windows.Themes.Windows7;component/Themes/{dic}",
                                    UriKind.Relative)) as System.Windows.ResourceDictionary);
                    }
                }
                else
                    XhtmlControl = System.Windows.Application.Current;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Telerik.Windows.Themes.Windows7.dll is requiring.{Environment.NewLine} Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static string ReplaceSpecialObjects(string xhtml, DB.eAIP eAIP, string section = null)
        {
            try
            {
                // Replace link with new one
                // Todo: check file hash
                var doc = new HtmlAgilityPack.HtmlDocument()
                {
                    OptionOutputOriginalCase = true,
                    OptionCheckSyntax = false,
                    OptionOutputAsXml = false,
                    OptionFixNestedTags = true,
                    OptionWriteEmptyNodes = true,
                    OptionAutoCloseOnEnd = true
                };
                doc.LoadHtml(xhtml);
                var links = doc.DocumentNode.SelectNodes("//a");
                if (links == null) return xhtml;

                List<HtmlNode> linkList = links.Where(x => x.GetAttributeValue("href", "").StartsWith("eaippro://"))
                    .ToList();
                if (linkList.Count == 0) return xhtml;

                using (eAIPContext db = new eAIPContext())
                {
                    foreach (HtmlNode link in linkList)
                    {
                        string urlAddress = link.Attributes["href"]?.Value;
                        if (urlAddress?.StartsWith("eaippro://") == true)
                        {
                            var param = Lib.GetParams(link.Attributes["href"]?.Value);
                            if (param.ContainsKey("id") && param.ContainsKey("identifier"))
                            {
                                Int32.TryParse(param["id"], out int id);
                                Guid.TryParse(param["identifier"], out Guid identifier);
                                if (id != 0)
                                {
                                    // Processing File object
                                    /////////////////////////////
                                    if (urlAddress.Contains("/OpenFile/"))
                                    {
                                        var file = db.AIPFile
                                            .AsNoTracking()
                                            .Where(x => x.IsCanceled != true
                                                        && x.Identifier == identifier
                                                        && (eAIP.Effectivedate >= x.EffectivedateFrom && (x.EffectivedateTo == null || eAIP.Effectivedate <= x.EffectivedateTo))
                                            )
                                            .OrderByDescending(x => x.Version)
                                            .Include(x => x.AIPFileData.AIPFileDataHash)
                                            .FirstOrDefault();

                                        if (file != null)
                                        {
                                            var path = Lib.SaveAIPFile(file);
                                            var fileName = Path.GetFileName(path);
                                            var type = Lib.GetFileType(file.FileName);
                                            var fileList = PDFManager.GetGraphicFiles(file, path, true);
                                            var graphicJoinedList = String.Join(" ", fileList.ToArray());
                                            var GraphicFile = doc.CreateTextNode(graphicJoinedList);
                                            link.ParentNode.ReplaceChild(GraphicFile, link);
                                        }
                                        else
                                        {
                                            // ToDo: What if file not found by criterias - clear it?
                                            link.ParentNode.ReplaceChild(doc.CreateTextNode(), link);
                                        }
                                    }
                                    // Processing Abbreviation object
                                    /////////////////////////////
                                    else if (urlAddress.Contains("/OpenAbbrev/"))
                                    {
                                        var abbrev = db.Abbreviation
                                            .AsNoTracking()
                                            .Where(x => x.IsCanceled != true
                                                        && x.Identifier == identifier
                                                        && (eAIP.Effectivedate >= x.EffectivedateFrom
                                                            && (x.EffectivedateTo == null ||
                                                                eAIP.Effectivedate <= x.EffectivedateTo))
                                            )
                                            .OrderByDescending(x => x.Version)
                                            .FirstOrDefault();
                                        if (abbrev != null)
                                        {
                                            var AbbreviationLink =
                                                doc.CreateTextNode($"<e:Abbreviation id=\"{abbrev.id}-{abbrev.Identifier}-{abbrev.Version}\" Ref=\"{abbrev.IdKey}\" Updated=\"No\"/>");
                                            link.ParentNode.ReplaceChild(AbbreviationLink, link);
                                        }
                                        else
                                        {
                                            // ToDo: What if abbrev not found by criterias - clear it?
                                            link.ParentNode.ReplaceChild(doc.CreateTextNode(), link);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return doc.DocumentNode.InnerHtml;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return xhtml;
            }
        }

        public static Dictionary<string, string> GetParams(string uri)
        {
            var matches = Regex.Matches(uri, @"[\/](([^\/=]+)=([^\/=#]*))", RegexOptions.Compiled);
            return matches.Cast<Match>().ToDictionary(
                m => Uri.UnescapeDataString(m.Groups[2].Value),
                m => Uri.UnescapeDataString(m.Groups[3].Value)
            );
        }

        public static bool SafeCopy(string path, string newPath)
        {
            try
            {
                using (var inputFile = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var outputFile = new FileStream(newPath, FileMode.Create))
                    {
                        inputFile.CopyTo(outputFile, 0x10000);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

    }

    public enum PathCategory
    {
        eAIP,
        eSUP,
        eAIC
    }
}
