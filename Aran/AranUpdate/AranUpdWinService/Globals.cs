using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AranUpdWinService
{
    static class Globals
    {
        public static bool RunProcess(string procName, string arg, int waitMillisecond)
        {
            var info = new ProcessStartInfo(procName);
            if (arg != null)
                info.Arguments = arg;
            info.UseShellExecute = false;
            info.RedirectStandardError = false;
            info.RedirectStandardInput = false;
            info.RedirectStandardOutput = false;
            info.CreateNoWindow = true;
            info.ErrorDialog = false;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            var proc7z = Process.Start(info);

            if (waitMillisecond > 0)
            {
                if (!proc7z.WaitForExit(waitMillisecond))
                {
                    proc7z.Kill();
                    return false;
                }
            }

            return true;
        }
    }
}
