using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using ZzArchivator;
using ArenaStatic;
using PDM;
using DataModule;
using ARENA.Enums_Const;
using ARENA.Project;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesGDB;
using EsriWorkEnvironment;
using AranSupport;
using ARENA;
using ArenaLogManager;

namespace ARENA
{
    public static class ArenaProjectOpener
    {

        public static void OpenSelectedFile(string _FileName, IApplication m_application)
        {

            var tempDirName = System.IO.Path.GetTempPath();
            var dInf = Directory.CreateDirectory(tempDirName + @"\PDM\" + System.IO.Path.GetFileNameWithoutExtension(_FileName));
            tempDirName = dInf.FullName;
            var tempPdmFilename = System.IO.Path.Combine(tempDirName, "pdm.pdm");
            var tempMxdFilename = System.IO.Path.Combine(tempDirName, "pdm.mxd");
            var tempMdbFileName = System.IO.Path.Combine(tempDirName, "pdm.mdb");
            //string zoomedLayerName = "AirportHeliport";


            try
            {

                ArenaStaticProc.DecompressToDirectory(_FileName, tempDirName);
                ArenaStaticProc.SetTargetDB(tempDirName);

                Application.DoEvents();

                OpenDecompressedFolder(_FileName, m_application, tempDirName, true);

            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name)
                    .Error(ex, "Error when opening project; Project Name: " + _FileName);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                ArenaMessageForm msgFrm = new ArenaMessageForm("ARENA", "Error when opening project.\nMaybe it's an attempt to open an old version of the project", global::ArenaToolBox.Properties.Resources.ArenaMessage);

                msgFrm.ShowDialog();
            }


            

        }

        public static string OpenDecompressedFolder(string _FileName, IApplication m_application, string tempDirName, bool saveAsRecent = false)
        {
            string tempMxdFilename;
            ARENA.Environment.Environment curEnvironment = new ARENA.Environment.Environment { mxApplication = m_application, pMap = ((IMxDocument)m_application.Document).ActiveView.FocusMap };
            ArenaProjectType prjType = ArenaProjectType.ARENA;
            //curEnvironment.Data.PdmObjectList.AddRange(ArenaDataModule.GetObjectsFromPdmFile(tempDirName, ref prjType));
            curEnvironment.Data.CurrentProjectType = prjType;
            curEnvironment.Data.CurProjectName = _FileName;
            curEnvironment.Data.MapDocumentName = System.IO.Path.GetFileNameWithoutExtension(_FileName);
            m_application.Caption = System.IO.Path.GetFileNameWithoutExtension(_FileName);


           
            if (File.Exists(tempDirName + @"\ProjectLog.txt"))
            curEnvironment.Data.ProjectLog = File.ReadLines(tempDirName + @"\ProjectLog.txt").ToList();

            string ConString = System.IO.Path.Combine(tempDirName, "pdm.mdb");
            if (File.Exists(ConString))
            {
                EsriUtils.CompactDataBase(ConString);
                tempMxdFilename = System.IO.Path.Combine(tempDirName, "arena_PDM.mxd");
                m_application.NewDocument(false, tempMxdFilename);
            }
            else
            {
                string ConStringAerodrome = System.IO.Path.Combine(tempDirName, "aerodrome.mdb");
                EsriUtils.CompactDataBase(ConStringAerodrome);
                tempMxdFilename = System.IO.Path.Combine(tempDirName, "aerodrome.mxd");
                m_application.NewDocument(false, tempMxdFilename);
            }

            #region MyRegion

            //curEnvironment.FillAirtrackTableDic();
            //curEnvironment.FillAirportDictionary();


            //DataCash.ProjectEnvironment = curEnvironment;

            //for (int i = 0; i < ((IMxDocument)m_application.Document).ContentsViewCount; i++)
            //{
            //    IContentsView cnts = ((IMxDocument)m_application.Document).get_ContentsView(i);

            //    string cntxtName = ((IMxDocument)m_application.Document).ContentsView[i].Name;


            //    if (cntxtName.StartsWith("TOCLayerFilter"))
            //    {
            //        ((IMxDocument)m_application.Document).CurrentContentsView = cnts;
            //        ((IMxDocument)m_application.Document).ContentsView[i].Refresh(cntxtName);
            //    }
            //}



            //=======
            //    string cntxtName = ((IMxDocument)m_application.Document).ContentsView[i].Name;

            //    if (cntxtName.StartsWith("TOCLayerFilter"))
            //    {
            //        ((IMxDocument)m_application.Document).CurrentContentsView = cnts;
            //        ((IMxDocument)m_application.Document).ContentsView[i].Refresh(cntxtName);
            //    }
            //}

            //>>>>>>> b8570916d65c61a2a98e9b27f2318a5966c81467

            #endregion


            switch (curEnvironment.Data.CurrentProjectType)
            {
                case (ArenaProjectType.ARENA):
                case (ArenaProjectType.PANDA):
                    curEnvironment.Data.CurrentProject = new ArenaProject(curEnvironment);
                    tempMxdFilename = System.IO.Path.Combine(tempDirName, "arena_PDM.mxd");

                    try
                    {
                        ((ArenaProject)curEnvironment.Data.CurrentProject).ProjectSettings = new ARENA.Settings.ArenaSettings();
                        ((ArenaProject)curEnvironment.Data.CurrentProject).ProjectSettings.ReadProjectSettings(tempDirName);
                    }
                    catch { }


                    break;

                case (ArenaProjectType.NOTAM):
                    curEnvironment.Data.CurrentProject = new NotamProject(curEnvironment);
                    tempMxdFilename = System.IO.Path.Combine(tempDirName, "notam_PDM.mxd");
                    //zoomedLayerName = "AirspaceVolume";
                    break;

                case (ArenaProjectType.AERODROME):
                    //curEnvironment.Data.CurrentProject = new ArenaProject(curEnvironment);
                    tempMxdFilename = System.IO.Path.Combine(tempDirName, "aerodrome.mxd");
                    break;
            }

            if (saveAsRecent) ArenaStatic.ArenaStaticProc.SaveRecentFileName(_FileName);
            return tempMxdFilename;
        }
    }
}
