using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace AranUpdWinService
{
    public class Updater
    {
        private string _tempDir;
        private string _tempFile;
        private string _aranProgFiles;
        private string _baseDir;
        private string _currentUserName;

        public Updater(string baseDir, string currentUserName)
        {
            _baseDir = baseDir;
            _tempDir = Path.Combine(baseDir, "Temp");
            if (!Directory.Exists(_tempDir))
                Directory.CreateDirectory(_tempDir);
            _tempFile = Path.Combine(_tempDir, "aranbin.7z");

            _aranProgFiles = Path.GetFullPath(Path.Combine(baseDir, "..\\"));
            _currentUserName = currentUserName;
        }

        public bool UpdateFiles(byte[] data, out string errorText)
        {
            errorText = string.Empty;

            #region Check whether process is running...

            var procArr = Process.GetProcesses();
            var sa = new string[] { "mapenv", "tossm", "arcmap" };
            foreach (var s in sa)
            {
                foreach (var proc in procArr)
                {
                    if (proc.ProcessName.ToLower().StartsWith(s))
                        errorText += proc.ProcessName + "\n";
                }
            }

            if (errorText.Length > 0)
            {
                errorText = "The following applications are running: \n" + errorText;
                return false;
            }

            #endregion

            #region If temp file exists delete it.

            if (File.Exists(_tempFile))
            {
                try
                {
                    File.Delete(_tempFile);
                }
                catch (Exception ex)
                {
                    errorText = "Could not delete temp file\nDetails: " + ex.Message;
                    return false;
                }
            }

            #endregion


            #region Extract all files in data to temp dir

            using (var fs = File.Create(_tempFile))
            {
                fs.Write(data, 0, data.Length);
                fs.Close();
            }

            var archiveFilesDir = Path.Combine(_tempDir, "bin");

            if (Directory.Exists(archiveFilesDir))
            {
                try
                {
                    Directory.Delete(archiveFilesDir, true);
                }
                catch (Exception ex)
                {
                    errorText = "Could not delete prev achrive\nDetails: " + ex.Message;
                    return false;
                }
            }

            var procRes = Globals.RunProcess(
                Path.Combine(_baseDir, "7z", "7z.exe"),
                string.Format("x \"{0}\" -o\"{1}\"", _tempFile, _tempDir),
                1000 * 60 * 2);

            if (!procRes)
            {
                errorText = "7z stopped because process takes too much time";
                return false;
            }

            #endregion

            try
            {
                CopyFiles(archiveFilesDir);

                var errorsList = new List<string>();
                RunPostCopyFiles(errorsList);
                foreach (var s in errorsList)
                    errorText += s + "\n";

                return true;
            }
            catch (Exception ex)
            {
                errorText = "Could not copy files.\nDetails: " + ex.Message;
                return false;
            }
        }


        private void CopyFiles(string archiveFilesDir)
        {
            var list = Directory.GetFiles(archiveFilesDir, "*", SearchOption.AllDirectories);
            var sDir = archiveFilesDir + "\\";
            var backupFiles = new List<Tuple<string, string>>();

            foreach (var sourceFile in list)
            {
                var destFile = sourceFile.Replace(sDir, _aranProgFiles);

                if (File.Exists(destFile))
                {
                    var backupFileName = destFile + ".backup";
                    try
                    {
                        if (File.Exists(backupFileName))
                            File.Delete(backupFileName);

                        File.Move(destFile, backupFileName);
                        backupFiles.Add(new Tuple<string, string>(backupFileName, Path.GetFileName(destFile)));
                    }
                    catch (Exception ex)
                    {
                        RollbackUpdate(backupFiles);
                        throw ex;
                    }
                }

                try
                {
                    var dir = Path.GetDirectoryName(destFile);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    File.Move(sourceFile, destFile);
                }
                catch (Exception ex)
                {
                    RollbackUpdate(backupFiles);
                    throw ex;
                }
            }

            #region Delete all backup files

            foreach (var file in backupFiles)
            {
                var backupFileName = file.Item1;
                try
                {
                    File.Delete(backupFileName);
                }
                catch { }
            }
            
            #endregion

            //RollbackUpdate(backupFiles);
            Directory.Delete(_tempDir, true);
        }

        private void RunPostCopyFiles(List<string> errorList)
        {
            Func<string, bool> pred1 = delegate(string filePath)
            {
                var fileName = Path.GetFileName(filePath);
                return fileName.StartsWith("run_") && (fileName.EndsWith(".bat") || fileName.EndsWith(".exe"));
            };

            var list = Directory.EnumerateFiles(_aranProgFiles, "*.*", SearchOption.AllDirectories).Where(pred1);

            foreach (var file in list)
            {
                try
                {
                    var info = new ProcessStartInfo(file);
                    info.UseShellExecute = false;
                    info.RedirectStandardError = true;
                    info.RedirectStandardInput = false;
                    info.RedirectStandardOutput = true;
                    info.CreateNoWindow = true;
                    info.ErrorDialog = false;
                    info.WindowStyle = ProcessWindowStyle.Hidden;
                    info.UserName = _currentUserName;
                    info.LoadUserProfile = true;

                    var proc = new Process();
                    proc.StartInfo = info;

                    proc.Start();


                    if (!proc.WaitForExit(1000 * 60 * 2))
                    {
                        proc.Kill();
                        errorList.Add(file + " stopped because process takes too much time");
                        return;
                    }

                    errorList.Add(proc.StandardError.ReadToEnd());
                }
                catch (Exception ex)
                {
                    errorList.Add(string.Format("Error on running script, file: {0}, Error: {1}", file, ex.Message));
                }
            }
        }

        private void RollbackUpdate(List<Tuple<string, string>> backupFileNameList)
        {
            foreach (var tuple in backupFileNameList)
            {
                var dir = Path.GetDirectoryName(tuple.Item1);
                var destFile = Path.Combine(dir, tuple.Item2);
                File.Move(tuple.Item1, destFile);
            }
        }
    }
}
