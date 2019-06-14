using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AranUpdateManager.Data;

namespace AranUpdateManager
{
    static class Global
    {
        static Global()
        {
            Settings = new Settings();
            //DbPro = new DbProvider();
            DbPro = new ManagerDbProvider();
        }

        //public static DbProvider DbPro { get; private set; }

        public static ManagerDbProvider DbPro { get; private set; }

        public static void ShowException(Exception ex)
        {
            MessageBox.Show(ex.Message, "ARAN Update Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static bool Execute7z(string arguments, int waitMilliSeconds)
        {
            var info = new ProcessStartInfo(Path.Combine(Application.StartupPath, "7z", "7z.exe"));
            info.Arguments = arguments;
            info.UseShellExecute = false;
            info.RedirectStandardError = false;
            info.RedirectStandardInput = false;
            info.RedirectStandardOutput = false;
            info.CreateNoWindow = true;
            info.ErrorDialog = false;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            var proc7z = Process.Start(info);
            if (!proc7z.WaitForExit(waitMilliSeconds))
            {
                proc7z.Kill();
                return false;
            }

            return true;
        }

        public static Settings Settings { get; private set; }
    }
}
