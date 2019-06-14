using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIP.BaseLib.Class
{
    public static class Encrypt
    {
        public static void Protect(string exeConfigName)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(exeConfigName);

                if (config.GetSection("connectionStrings") is ConnectionStringsSection section && !section.SectionInformation.IsProtected)
                {
                    // Encrypt the section.
                    section?.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                    // Save the current configuration.
                    config.Save();
                }
                // Debug.WriteLine("Protected={0}", section?.SectionInformation.IsProtected);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void UnProtect(string exeConfigName)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(exeConfigName);

                if (config.GetSection("connectionStrings") is ConnectionStringsSection section && section.SectionInformation.IsProtected)
                {
                    // Remove encryption.
                    section.SectionInformation.UnprotectSection();
                    // Save the current configuration.
                    config.Save();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ProtectBaseLib(bool isProtect = true)
        {
            try
            {
                if (isProtect) Protect("AIP.BaseLib.dll");
                else UnProtect("AIP.BaseLib.dll");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
