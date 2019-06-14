using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aran.Omega.Models;
using Aran.PANDA.Constants;

namespace Aran.Omega.Export
{
    class ExportFolderPath
    {
        public static string GetFolderPath(RwyDirClass rwyDir, EnumName<RunwayClassificationType> clasification, EnumName<CategoryNumber> category)
        {
            string folderPath;
            var saveFolderDialog = new FolderBrowserDialog();
            DialogResult dialogResult = saveFolderDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
                folderPath = saveFolderDialog.SelectedPath;
            else
                return "";

            folderPath += @"\" + GlobalParams.Database.AirportHeliport.Designator.Substring(2) + "_" +
                          rwyDir.Name;

            if (clasification.Enum == RunwayClassificationType.NonInstrument)
                folderPath += "NI";
            else if (clasification.Enum == RunwayClassificationType.NonPrecisionApproach)
                folderPath += "NPA";
            else
            {
                folderPath += "PA";
                if (category.Enum == CategoryNumber.One)
                    folderPath += "I";
                else
                    folderPath += "II";
            }

            folderPath += "_" + DateTime.Now.ToString("yyMMdd_Hmm");

            if (Directory.Exists(folderPath))
                folderPath += $"_{Guid.NewGuid()}";

            Directory.CreateDirectory(folderPath);

            return folderPath;

        }
    }
}
