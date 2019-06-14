using Aran.Temporality.CommonUtil.Util;
using ArenaStatic;
using Microsoft.Win32;

namespace AmdbManager
{
    public class Config
    {

        static Config()
        {
            var registry = new ModifyRegistry
            {
                ShowError = false,
                BaseRegistryKey = Registry.CurrentUser,
                SubKey = ArenaStaticProc.GetMainFolder()
            };
            AddressTCP = registry.Read(nameof(AddressTCP));
            Username = registry.Read(nameof(Username));
            Password = registry.Read(nameof(Password));
            CertificateName = "ChartManagerTempCert";
        }

        public static string AddressTCP { get; set; }
        public static string CertificateName { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }
    }
}
