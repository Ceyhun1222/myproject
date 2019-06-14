using ArenaStatic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using ZzArchivator;
using AranSupport;
//using EsriWorkEnvironment;
using ARENA;

namespace ArenaLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = new string[] { @"D:\ARINC\DATA\qatar\sidPRJ.pdm" };
            try
            {
                FileAssociation.Associate("Arena File", System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\favicon.ico");

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            if (args.Length == 0) return;

            AlertForm alrtForm = new AlertForm();
            try
            {
                var _FileName = args[0];

                //MessageBox.Show(_FileName);

                alrtForm.FormBorderStyle = FormBorderStyle.None;
                alrtForm.Opacity = 0.5;
                alrtForm.BackgroundImage = ArenaLauncher.Properties.Resources.ArenaSplash;

                alrtForm.TopMost = true;
                alrtForm.Show();

                var tempDirName = System.IO.Path.GetTempPath();
                var dInf = Directory.CreateDirectory(tempDirName + @"\PDM\" + System.IO.Path.GetFileNameWithoutExtension(_FileName));
                tempDirName = dInf.FullName;
                var tempPdmFilename = System.IO.Path.Combine(tempDirName, "pdm.pdm");
                var tempMxdFilename = System.IO.Path.Combine(tempDirName, "pdm.mxd");
                var tempMdbFileName = System.IO.Path.Combine(tempDirName, "pdm.mdb");


                ArenaStaticProc.DecompressToDirectory(_FileName, tempDirName);

                bool waitFlaf = false;

                while (!waitFlaf)
                {
                    string[] mdbF = System.IO.Directory.GetFiles(tempDirName, "*.mdb");
                    string[] mxdF = System.IO.Directory.GetFiles(tempDirName, "*.mxd");
                    string[] pdmF = System.IO.Directory.GetFiles(tempDirName, "*.pdm");

                    waitFlaf = (mdbF != null && mdbF.Length > 0) && (mxdF != null && mxdF.Length > 0) && (pdmF != null && pdmF.Length > 0);
                }



                //string ConString = System.IO.Path.Combine(tempDirName, "pdm.mdb");
                //EsriUtils.CompactDataBase(ConString);
                Application.DoEvents();
                tempMxdFilename = System.IO.Path.Combine(tempDirName, "arena_PDM.mxd");

                ArenaStaticProc.SetTargetDB(tempDirName);

                if ((args != null) && (args.Length > 0))
                {

                    var process = new Process();
                    process.StartInfo = new ProcessStartInfo()
                    {

                        UseShellExecute = true,
                        FileName = tempMxdFilename
                    };

                    process.Start();

                    Application.DoEvents();

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                alrtForm.Close();
            }

        }
    }
}
