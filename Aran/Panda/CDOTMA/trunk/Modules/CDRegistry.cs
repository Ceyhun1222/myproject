using System;
using Microsoft.Win32;

namespace CDOTMA
{
	public static class CDRegistry
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

		public static T RegRead<T>(RegistryKey hKey, string key, string valueName, T defaultValue)
		{
			try
			{
				RegistryKey regKey = hKey.OpenSubKey(key, false);
				if (regKey != null)
				{
					object value = regKey.GetValue(valueName);
					if (value != null)
					{
						try
						{
							return (T)Convert.ChangeType(value, typeof(T));
						}
						catch { }
					}
				}
			}
			catch { }

			return defaultValue;
		}

		public static int RegWrite(Microsoft.Win32.RegistryKey HKey, string key, string valueName, object value)
		{
			try
			{
				Microsoft.Win32.RegistryKey regKey = HKey.OpenSubKey(key, true);
				if (regKey == null)
					return -1;

				regKey.SetValue(valueName, value);
				return 0;
			}
			catch
			{
			}

			return -1;
		}

	}
}