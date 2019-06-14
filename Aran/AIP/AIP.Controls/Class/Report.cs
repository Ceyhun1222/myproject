using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIP.BaseLib.Class
{
    public static class Report
    {
        private static string _errorLogFileName;
        public static string ErrorLogFileName
        {
            get => Path.Combine(CurrentDir(), !String.IsNullOrEmpty(_errorLogFileName) ? _errorLogFileName : @"Report.log");
            set => _errorLogFileName = value;
        }

        static Report()
        {
            if (!Directory.Exists(CurrentDir())) Directory.CreateDirectory(CurrentDir());
            if (!File.Exists(ErrorLogFileName)) File.Create(ErrorLogFileName);
        }

        private static string CurrentDir()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Report");
        }

        public static void Write(string Message)
        {
            //if (!File.Exists(ErrorLogFileName)) File.Create(ErrorLogFileName);
            using (StreamWriter writer = new StreamWriter(ErrorLogFileName, true))
            {
                string dateTime = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                writer.WriteLine($@"[{dateTime}]: " + Message);
            }
        }
    }
}
