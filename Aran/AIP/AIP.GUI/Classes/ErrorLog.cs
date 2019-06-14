using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIP.GUI
{
    public static class ErrorLog
    {
        private static string errorLogFileName;
        public static string ErrorLogFileName
        {
            get
            {
                return Lib.CurrentDir + @"\Error.log";
            }
        }
        static ErrorLog()
        {
            if (!File.Exists(ErrorLogFileName))
                File.Create(ErrorLogFileName);
        }

        public static void Write(string Message, string Exception = null)
        {
            using (StreamWriter writer = new StreamWriter(ErrorLogFileName, true))
            {
                string dateTime = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                if (Exception != null)
                {
                    writer.WriteLine($@"[{dateTime}]: " + Message + Environment.NewLine +
                                     "System error: " + Exception);
                }
                else
                {
                    writer.WriteLine($@"[{dateTime}]: " + Message);
                }

            }
        }

        public static void ShowMessage(string Message, bool AddToLogFile = false, MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            MessageBox.Show(Message,
            "Error", MessageBoxButtons.OK,
                        icon);
            if(AddToLogFile)
                Write(Message);
        }

        public static void ShowWarning(string Message)
        {
            ShowMessage(Message, false, MessageBoxIcon.Warning);
        }

        public static void ShowInfo(string Message)
        {
            ShowMessage(Message, false, MessageBoxIcon.Information);
        }

        public static void ShowException(string Message, Exception ex = null, bool AddToLogFile = false)
        {
            if (ex != null) Message += $@"{Environment.NewLine} {ex.GetBaseException()}"; 
            MessageBox.Show(Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (AddToLogFile) Write(Message);
        }

        public static void Add(string Message, Exception ex = null, bool ShowMessage = false)
        {
            if (ex != null) Message += $@"{Environment.NewLine} {ex.GetBaseException()}";
            if(ShowMessage) MessageBox.Show(Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Write(Message);
        }

    }
}
