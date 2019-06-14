using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Framework;
using Microsoft.Win32;
using ArenaINIManager;
using System.IO;

namespace ARENA
{

    class RecentFilesHelper
    {
        const string RecentFileRegistryKeyPath = @"Software\ESRI\Desktop10.2\ArcMap\Recent File List";
        
        public static string[] GetRecentFiles_Mxd(IApplication app)
        {
            List<string> recentFilePaths = new List<string>();

            //Read the registry to get the recent file list
            string openKey = string.Format(RecentFileRegistryKeyPath, app.Name);
            RegistryKey recentListKey = Registry.CurrentUser.OpenSubKey(openKey);
            if (recentListKey != null)
            {
                string[] listNames = recentListKey.GetValueNames();
                foreach (string name in listNames)
                {
                    string fileName = recentListKey.GetValue(name, string.Empty).ToString();
                    if (!string.IsNullOrEmpty(fileName))
                        recentFilePaths.Add(fileName);
                }
            }

            return recentFilePaths.ToArray();
        }

        public static string[] GetRecentFiles_Pdm()
        {
            if (!File.Exists(ArenaStatic.ArenaStaticProc.GetMainFolder() + @"\ArenaSettings.ini")) ArenaStatic.ArenaStaticProc.CreateArenaIniFile();
            var ArenaIni = new IniFile(ArenaStatic.ArenaStaticProc.GetMainFolder() + @"\ArenaSettings.ini");

            string recentList = ArenaIni.Read("RecentFiles", "ARENA");
            if (recentList.EndsWith("?")) recentList = recentList.TrimEnd('?');

            string[] recentFilePaths = recentList.Split('?');

          

            return recentFilePaths;
        }

    }
}
