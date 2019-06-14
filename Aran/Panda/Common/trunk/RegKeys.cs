using System;
using System.IO;
using Microsoft.Win32;

namespace Aran.PANDA.Common
{
	public static class RegFuncs
	{
		public const string Panda = "SOFTWARE\\RISK\\PANDA";
		public const string Conventional = Panda + "\\Conventional";
		public const string RNAV = Panda + "\\RNAV";

		public const string PandaRegKey = Panda;
		public const string RnavRegKey = RNAV;
		public const string OmegaKey = "SOFTWARE\\RISK\\Omega";

		public const string LicenseKeyName = "Acar";
		public const string ConstKeyName = "ConstDir";
		public const string InstallDirKeyName = "InstallDir";

        //public static T RegRead<T>(RegistryKey hKey, string key, string valueName, T defaultValue)
        //{
        //    try
        //    {
        //        RegistryKey regKey = hKey.OpenSubKey(key, false);
        //        if (regKey != null)
        //        {
        //            object value = regKey.GetValue(valueName);
        //            if (value != null)
        //            {
        //                try
        //                {
        //                    return (T)Convert.ChangeType(value, typeof(T));
        //                }
        //                catch { }
        //            }
        //        }
        //    }
        //    catch { }

        //    return defaultValue;
        //}

        //public static int RegWrite(Microsoft.Win32.RegistryKey HKey, string key, string valueName, object value)
        //{
        //    try
        //    {
        //        Microsoft.Win32.RegistryKey regKey = HKey.OpenSubKey(key, true);
        //        if (regKey == null)
        //            return -1;

        //        regKey.SetValue(valueName, value);
        //        return 0;
        //    }
        //    catch
        //    {
        //    }

        //    return -1;
        //}

        public static string GetConstantsDir(out bool isExists)
        {
            var assemblyDir = Path.GetDirectoryName(typeof(RegFuncs).Assembly.Location);
            var dir = Path.Combine(assemblyDir, @"PANDA\Constants");
            //var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "R.I.S.K\\PANDA\\Constants");
            isExists = Directory.Exists(dir);
            return dir;
        }

        public static T ReadConfig<T>(string key, T defaultValue)
        {
            return CommonUtils.Config.ReadConfig<T>(@"RISK/PANDA", key, defaultValue);
        }

        public static void WriteConfig(string key, object value)
        {
            CommonUtils.Config.WriteConfig(@"RISK/PANDA", key, value);
        }

        public static string GetConventionalAcar(string moduleName)
        {
            return ReadConfig("Conventional/" + moduleName + "/Acar", string.Empty);
        }

        public static void SetConventionalAcar(string moduleName, string lcode)
        {
            WriteConfig("Conventional/" + moduleName + "/Acar", lcode);
        }

        public static string GetRNAVAcar(string moduleName)
        {
            return ReadConfig("RNAV/" + moduleName + "/Acar", string.Empty);
        }

        public static void SetRNAVAcar(string moduleName, string lcode)
        {
            WriteConfig("RNAV/" + moduleName + "/Acar", lcode);
        }

	}
}