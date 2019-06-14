using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIP.GUI.Classes
{
    internal static class Permissions
    {
        private static List<string> permissionsList = new List<string> { "AipManagementApi_AipAdmin" };

        static void setPermissions(List<string> list)
        {
            permissionsList = list;
        }

        public static bool listContains(string item)
        {
            if (permissionsList.Contains(item)) return true;
            return false;
        }
        public static bool Is_Admin()
        {
            if (listContains("AipManagementApi_AipAdmin")) return true;
            return false;
        }
        public static bool ManageDataSet()
        {
            if (Is_Admin() || listContains("AipManagementApi_AipDataSet")) return true;
            return false;
        }
        public static bool ManagaAisPackages()
        {
            if (Is_Admin() || listContains("AipManagementApi_AipCreate")) return true;
            return false;
        }
        public static bool CanPublishAIP()
        {
            if (Is_Admin() || listContains("AipManagementApi_AipPublish")) return true;
            return false;
        }
        public static bool CanUploadAIPtoWebsite()
        {
            if (Is_Admin() || listContains("AipManagementApi_AipUpload")) return true;
            return false;
        }
        public static bool CanManageSupplements()
        {
            if (Is_Admin() || listContains("AipManagementApi_AipSupplement")) return true;
            return false;
        }
        public static bool CanManageOtherPages()
        {
            if (Is_Admin() || listContains("AipManagementApi_AipOtherPage")) return true;
            return false;
        }
        public static bool CanManageCirculars()
        {
            if (Is_Admin() || listContains("AipManagementApi_AipCircular")) return true;
            return false;
        }
        public static bool CanUploadFiles()
        {
            if (Is_Admin() || listContains("AipManagementApi_AipFileManager")) return true;
            return false;
        }
        public static bool CanManageAbbreviations()
        {
            if (Is_Admin() || listContains("AipManagementApi_AipAbbreviationManager")) return true;
            return false;
        }
        public static bool CanManageLanguageTexts()
        {
            if (Is_Admin() || listContains("AipManagementApi_AipLanguageManager")) return true;
            return false;
        }
    }
}
