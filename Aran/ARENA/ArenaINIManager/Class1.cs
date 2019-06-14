using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


///
namespace ArenaINIManager
{
    public class IniFile   // revision 10
    {
        string Path;
        string EXE ;//= Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32")]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath = null)
        {
            string pp = IniPath.EndsWith("ArenaSettings.ini") ? IniPath : IniPath + @"\ArenaSettings.ini";
            Path = new FileInfo(pp).FullName.ToString();
            EXE = IniPath;
        }

        public void CreateArenaIni(string ArenaVersion)
        {
                Write("PdmFile", EXE + @"\Model", "Arena");
                Write("LanguageCode", @"English", "Arena");
                Write("TargetDB", "", "Arena");
                Write("ProjectIdentifier", "ARENA", "Arena");
                Write("RecentFiles", "", "Arena");
                Write("version", ArenaVersion, "ARENA");


                Write("AreaFile", EXE + @"\Model\ARINC\Area\area.xml", "ARINC");
                Write("Regions", EXE + @"\Model\ARINC\Area\areaList.xml", "ARINC");
                Write("SpecificationPath", EXE + @"\Model\ARINC", "ARINC");

                Write("mapFolder", @"C:\", "SIGMA");
                Write("archiveFolder", EXE + @"\MapArchive", "SIGMA");
                Write("templateFolder", EXE + @"\Model\SIGMA\Templates", "SIGMA"); //
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
