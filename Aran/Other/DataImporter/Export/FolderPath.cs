using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataImporter.Export
{
    class FolderPath
    {
        public static string GetFolderPath()
        {
            string folderPath;
            var saveFolderDialog = new FolderBrowserDialog();
            DialogResult dialogResult = saveFolderDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
                folderPath = saveFolderDialog.SelectedPath;
            else
                return "";

            return folderPath;
        }
    }
}
