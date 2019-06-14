using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AIP.DataSet.Classes;
using AIP.DataSet.Lib;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Temporality.Common.Enum;

namespace AIP.DataSet
{
    public static class AIP
    {
        public static DateTime EffectiveDate;
        public static DateTime CancelDate;
        public static bool IsAIRAC;
        public static bool IsUnpublished;
        public static bool IsVersion = true;
        public static string ProjectName;
        public static string SpaceName;
        public static string Company;
        public static string Language;
        public static string Country;
        public static string MainFolder;
        public static string DataSetFile;
        public static string DataSetFileFullPath;
        public static string ReportFileFullPath;
        public static string DataSetFolder;
        public static InterpretationTypes Interpretation = InterpretationTypes.Snapshot;
        public static List<ClassFilter> FilterList;

        static AIP()
        {
            MainFolder = Path.Combine(AssemblyDirectory(), "Data", "AIPDataSet");
            if (!Directory.Exists(MainFolder)) Directory.CreateDirectory(MainFolder);
        }

        public static void Initialize()
        {
            try
            {
                InitDataSetPath();
                InitFilter();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }

        }

        private static void InitFilter()
        {
            FilterList = new List<ClassFilter>
            {
                new ClassFilter(
                    FeatureType.Airspace,
                    new List<Filter>
                    {
                        new Filter("Designator", AIP.Country, FilterOperation.StartsWith)
                    })
            };
        }

        public static string AssemblyDirectory()
        {
            try
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        private static void InitDataSetPath()
        {
            try
            {
                string airacPart = IsAIRAC && (int)Interpretation < (int)InterpretationTypes.AllStatesInRange ? "_AIRAC" : "";
                string toDatePart = (int)Interpretation >= (int)InterpretationTypes.AllStatesInRange ? $@"_{CancelDate:yyyyMMdd}" : "";
                string rootFolder = $@"{EffectiveDate:yyyy-MM-dd}{airacPart}";
                string unpublishPart = "";
                string versionPart = (IsVersion) ? "_v1" : "";
                string interpretationPart = "";
                if(Interpretation == InterpretationTypes.Snapshot) interpretationPart = "FULL";
                else if(Interpretation == InterpretationTypes.AllStatesInRange) interpretationPart = "FULL_NOTAM";
                else if(Interpretation == InterpretationTypes.TempDeltaStatesInRange) interpretationPart = "NOTAM";

                if (IsUnpublished)
                {
                    string projectPart = string.IsNullOrEmpty(ProjectName) ? "" : $@"_{ProjectName.RemoveInvalidPathChars()}";
                    string spacePart = string.IsNullOrEmpty(SpaceName) ? "" : $@"_{SpaceName.RemoveInvalidPathChars()}";
                    unpublishPart = $@"_PENDING_{ProjectName.RemoveInvalidPathChars()}_{SpaceName.RemoveInvalidPathChars()}";
                }

                string rootDatasetFolder = Path.Combine(MainFolder, rootFolder);
                string intFolder = $@"{interpretationPart}_{EffectiveDate:yyyyMMdd}{airacPart}";
                string datasetFolder = Path.Combine(rootDatasetFolder, $@"{intFolder}{versionPart}");
                if (!Directory.Exists(datasetFolder)) Directory.CreateDirectory(datasetFolder);
                else if (IsVersion)
                {
                    int max = GetMaxVersionDirectory(rootDatasetFolder, intFolder) ?? 1;
                    datasetFolder = datasetFolder.Replace($@"_v1", $@"_v{(max + 1)}");
                    Directory.CreateDirectory(datasetFolder);
                }

                // Filling properies
                DataSetFolder = Path.Combine(MainFolder, datasetFolder);
                string fileMainPart = $@"{AIP.Country}_AIP_DS_{interpretationPart}_{EffectiveDate:yyyyMMdd}{airacPart}{toDatePart}{unpublishPart}";
                DataSetFile = $@"{fileMainPart}.xml";
                DataSetFileFullPath = Path.Combine(DataSetFolder, $@"{DataSetFile}");
                ReportFileFullPath = Path.Combine(DataSetFolder, $@"REPORT_{fileMainPart}.log");
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static int? GetMaxVersionDirectory(string rootDirectory, string pattern)
        {
            try
            {
                return Directory.GetDirectories(rootDirectory, $@"{pattern}_v*")
                        .Select(Path.GetFileName)
                        .Select(s => ParseInt32(s.Replace($@"{pattern}_v", "")))
                        .DefaultIfEmpty()
                        .Max();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return 0;
            }
        }

        private static int ParseInt32(string str, int defaultValue = 0)
        {
            int result;
            return Int32.TryParse(str, out result) ? result : defaultValue;
        }
    }


}
