using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Metadata
{
    public class IniFile   // revision 10
    {
        string Path;
        string EXE;//= Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32")]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath = null)
        {
            string pp = IniPath.EndsWith("AMDBSettings.ini") ? IniPath : IniPath + @"\AMDBSettings.ini";
            Path = new FileInfo(pp).FullName.ToString();
            EXE = IniPath;
        }

        public void CreateAerodromeIni(string ArenaVersion)
        {
            Write("AmdbFile", EXE, "AERODROME");
            Write("LanguageCode", @"English", "AERODROME");
            Write("TargetDB", "", "AERODROME");
            Write("ProjectIdentifier", "AMDB", "AERODROME");
            Write("RecentFiles", "", "AERODROME");
            Write("version", ArenaVersion, "AERODROME");

        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(2024);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 2024, Path);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}
