using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIP.DB;

//using AIP.DB;

namespace XHTML_WPF.Classes
{
    internal static class Lib
    {
        internal static bool IsAIPDBExists = false;
        internal static bool IsAIPConnectionExists = false;

        static Lib()
        {
            IsAIPDBExists = isAipDbExists();
            IsAIPConnectionExists = isAipDbConnectionExists();
        }
        private static bool isAipDbExists()
        {
            try
            {
                using (eAIPContext db = new eAIPContext())
                {
                    return db.Database.Exists();
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool isAipDbConnectionExists()
        {
            try
            {
                using (eAIPContext db = new eAIPContext())
                {
                    db.Database.Connection.Open();
                    db.Database.Connection.Close();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        internal static string SaveAIPFile(AIPFile aipFile)
        {
            try
            {
                if (aipFile?.AIPFileData?.Data == null) return "";
                string fileName = "";
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Temp");
                if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);
                fileName = Path.Combine(outputDir, AIPFileName(aipFile));
                File.WriteAllBytes(fileName, aipFile?.AIPFileData?.Data);

                return fileName;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        internal static string AIPFileName(AIPFile aipFile)
        {
            try
            {
                return $@"Temp_{aipFile.id}.{aipFile.FileName.GetAfterOrEmpty()}";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }
    }



    public static class CustomExtensions
    {
        public static string ToTitleString(this Version version)
        {
            try
            {
                return $"Version {version.Major}.{version.Minor}.{version.Build}, Revision {version.Revision}";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }
    }
}
