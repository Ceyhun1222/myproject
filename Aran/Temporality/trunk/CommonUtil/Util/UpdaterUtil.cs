using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Aran.Temporality.CommonUtil.Context;

namespace Aran.Temporality.CommonUtil.Util
{
    public class UpdaterUtil
    {
        public static bool Update()
        {
            var version = CurrentDataContext.Version;

            var newVersion = CurrentDataContext.CurrentNoAixmDataService.GetUpdateVersion(CurrentDataContext.Application + ":" + version);
            if (!string.IsNullOrEmpty(newVersion) && newVersion.ToLower().Trim() != version.ToLower().Trim())
            {
                if (MessageBox.Show(
                    "A new version " + newVersion + " of application is available. In order to install it current application should be closed. Do you want to install update now?",
                    "Update is available", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    var data = CurrentDataContext.CurrentNoAixmDataService.GetUpdate(CurrentDataContext.Application + ":" + version);
                    if (data != null)
                    {
                        var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\RISK\\Update";
                        Directory.CreateDirectory(folder);
                        var dataFile = folder + "\\data.zip";
                        using (var writer = new FileStream(dataFile, FileMode.Create))
                        {
                            writer.Write(data, 0, data.Length);
                        }

                        Process.Start(@"..\Updater\Updater.exe", CurrentDataContext.Application);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
