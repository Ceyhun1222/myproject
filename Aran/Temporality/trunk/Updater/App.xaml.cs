using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace Updater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static void ExtractZipFile(string archiveFilenameIn, string password, string outFolder)
        {
            ZipFile zf = null;
            try
            {
                FileStream fs = File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);
                if (!String.IsNullOrEmpty(password))
                {
                    zf.Password = password; // AES encrypted entries are handled automatically
                }
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue; // Ignore directories
                    }
                    String entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    byte[] buffer = new byte[4096]; // 4K is optimum
                    Stream zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    String fullZipToPath = Path.Combine(outFolder, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (!string.IsNullOrEmpty(directoryName))
                        Directory.CreateDirectory(directoryName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
        }


        public static void EmptyDirectory(DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles()) file.Delete();
            foreach (var subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (var dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (var file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name),true);
        }

        public static bool Update(bool emptyFolder=false)
        {
            try
            {
                var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\RISK\\Update";
                var dataFile = folder + "\\data.zip";
                if (File.Exists(dataFile))
                {
                    var folderOut = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\RISK\\Update\\Out";

                    Directory.CreateDirectory(folderOut);
                    EmptyDirectory(new DirectoryInfo(folderOut));

                    ExtractZipFile(dataFile, null, folderOut);

                    //seems all is ok, now we can make changes in current folder
                    Directory.CreateDirectory(@"..\Bin");

                    if (emptyFolder)
                    {
                        EmptyDirectory(new DirectoryInfo(@"..\Bin"));
                    }
                    CopyFilesRecursively(new DirectoryInfo(folderOut), new DirectoryInfo(@"..\Bin"));

                    EmptyDirectory(new DirectoryInfo(folderOut));
                    Directory.Delete(folderOut, true);

                    File.Delete(dataFile);
                    return true;
                }
            }
            catch 
            {

            }


            return false;
        }


        private static void KillProcesses(string name)
        {
            name = name.ToLower();
            if (name.EndsWith(".exe"))
            {
                name = name.Substring(0, name.Length - 4);
            }

            var localProcs = Process.GetProcessesByName(name);
            try
            {
                foreach (var targetProc in localProcs)
                {
                    targetProc.Kill();
                }
            }
            catch 
            {
            }
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 0) return;//nothing to do when there is no arguments

            var firstArgument = e.Args[0];//application to kill and restart
            if (!firstArgument.ToLower().EndsWith(".exe"))
            {
                firstArgument = firstArgument + ".exe";
            }
            var lastArgument = e.Args[e.Args.Length-1];//application to mention in message

            
            //kill app
            KillProcesses(firstArgument);

            //do update
            if (Update(e.Args.Any(t => t == "/d")))
            {
                const string currentLogFile = "..\\Bin\\changelog.html";

                if (File.Exists(currentLogFile))
                {
                    //show changelog
                    if (MessageBox.Show(lastArgument + " was updated. Do you want to see change log?", "Successfully Updated", MessageBoxButton.YesNo)
                        == MessageBoxResult.Yes)
                    {
                        Process.Start(currentLogFile);
                    }
                }
                else
                {
                    MessageBox.Show(lastArgument + " was updated.", "Successfully Updated", MessageBoxButton.OK);
                }
                //rerun app
                Process.Start(@"..\Bin\" + firstArgument);
            }
            //exit
            Shutdown();
        }
    }
}
