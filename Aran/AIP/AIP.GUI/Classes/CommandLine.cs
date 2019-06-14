using AIP.DB;
using AIP.XML;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AIP.GUI.Classes
{
    internal static class CommandLine
    {
        /// <summary>
        /// Object collect all messages to send into Main form for the output with the AddOutput method
        /// </summary>
        internal static Queue<BaseLib.Struct.Output> OutputQueue = new Queue<BaseLib.Struct.Output>();

        /// <summary>
        /// Events for command line commands
        /// </summary>
        private static AutoResetEvent outputWaitHandle = new AutoResetEvent(false);
        private static AutoResetEvent errorWaitHandle = new AutoResetEvent(false);
        
        internal static void Run(string Arguments, bool isPdf = false)
        {
            using (Process p = new Process())
            {
                try
                {
                    p.StartInfo.Arguments = Arguments;
                    p.StartInfo.FileName = (isPdf) ? Lib.ExtMakeAIPFile : Lib.MakeAIPFile;
                    p.StartInfo.WorkingDirectory = (isPdf) ? Lib.ExtMakeAIPWorkingDir : Lib.MakeAIPWorkingDir;

                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.CreateNoWindow = true;

                    List<string> output = new List<string>();
                    List<string> warning = new List<string>();
                    List<string> error = new List<string>();

                    p.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data == null) outputWaitHandle?.Set();
                        else if(Properties.Settings.Default.ShowOutput) output.Add(e.Data);
                    };
                    p.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data == null) errorWaitHandle?.Set();
                        else
                        {

                            if (e.Data.ToLowerInvariant().Contains("warning:") || e.Data.ToLowerInvariant().Contains("[error] no space")) // Custom warning
                            {
                                if (Properties.Settings.Default.ShowWarnings) warning.Add(e.Data.Replace("[error]","WARNING: "));
                            }
                            else if (e.Data.ToLowerInvariant().Contains("info:")) // Custom info
                            {
                                if (Properties.Settings.Default.ShowWarnings) warning.Add(e.Data);
                            }
                            else
                                if (Properties.Settings.Default.ShowErrors && !e.Data.ToLowerInvariant().Contains("org.apache.fop.events.loggingeventlistener processevent"))
                                    error.Add(e.Data);
                        }
                    };

                    p.Start();

                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                    int timeoutSec = 5 * 60 * 1000; // TimeOut 300 seconds
                    if (p.WaitForExit(timeoutSec) &&
                        outputWaitHandle.WaitOne(timeoutSec) &&
                        errorWaitHandle.WaitOne(timeoutSec))
                    {
                        // Process completed. Checking p.ExitCode
                        //p.ExitCode
                    }
                    else
                    {
                        // Timeout occur
                        SendOutput("Timeout in long operation. You can increase timeout in the settings.", Color.Red);
                    }
                    p.Close();
                    if (output.Count > 0)
                        SendOutput(output.ToSeparatedValues(Environment.NewLine,
                                    Properties.Settings.Default.MaxLogMessages,
                                    $@"Shown first {Properties.Settings.Default.MaxLogMessages} messages from {output.Count}"));
                    if (warning.Count > 0)
                        SendOutput(warning.ToSeparatedValues(Environment.NewLine,
                            Properties.Settings.Default.MaxLogMessages,
                            $@"Shown first {Properties.Settings.Default.MaxLogMessages} warnings from {warning.Count}"), Color.Orange);
                    if (error.Count > 0)
                        SendOutput(error.ToSeparatedValues(Environment.NewLine,
                            Properties.Settings.Default.MaxLogMessages,
                            $@"Shown first {Properties.Settings.Default.MaxLogMessages} errors from {error.Count}"), Color.Red);
                    //SendOutput(GenMessage, 99);
                }
                catch (Exception ex)
                {
                    ErrorLog.ShowException("Error in the GenerateHTML", ex, true);
                }
            }
        }
        private static void SendOutput(string Message, Color? Color = null, int? Percent = null)
        {
            try
            {
                BaseLib.Struct.Output output = new BaseLib.Struct.Output();
                output.Message = Message;
                output.Color = Color ?? System.Drawing.Color.Black;
                output.Percent = Percent;
                OutputQueue.Enqueue(output);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
    }
}
