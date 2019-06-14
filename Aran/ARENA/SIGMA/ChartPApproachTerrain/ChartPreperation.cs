using System;
using System.IO;
using System.Windows.Forms;
using ArenaStatic;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using EsriWorkEnvironment;
using DataModule;

namespace ChartPApproachTerrain 
{
    class PATCChartPreperation  
    {

        public static IMap ChartsPreparation(int ChartType, string ProjectName, string FolderName, string TemplatetName, IApplication m_application)
        {

		
            try
            {
                #region переместить проект в новое место, очистить индексы и relations

                var pathToTemplateFileMXD2 = Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\PATC\", TemplatetName);

                var pathToTemplateMDB = Path.Combine(Path.GetDirectoryName(ArenaStaticProc.GetTargetDB()), "pdm.mdb");

                ArenaDataModule.ClearRelations_Indexes(pathToTemplateMDB);

                File.Copy(pathToTemplateMDB, Path.Combine(FolderName, "pdm.mdb"), true);

                Application.DoEvents();
                File.Copy(pathToTemplateFileMXD2, Path.Combine(FolderName, ProjectName), true);
                Application.DoEvents();
                //
                Application.DoEvents();

                m_application.OpenDocument(Path.Combine(FolderName, ProjectName));

                IMxDocument pNewDocument = (IMxDocument)m_application.Document;
                pNewDocument.RelativePaths = true;
                m_application.SaveDocument(ProjectName);
                Application.DoEvents();

                m_application.RefreshWindow();

                Application.DoEvents();

                #endregion

                return pNewDocument.ActiveView.FocusMap;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;

            }
        }


        public static IWorkspaceEdit GetWorkspace(IMap activeMap)
        {

            ILayer _Layer = EsriUtils.getLayerByName(activeMap, "AirportCartography");
            if (_Layer == null)
            {               
                return null;
            }
            var fc = ((IFeatureLayer)_Layer).FeatureClass;
            var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;
            return workspaceEdit;

        }
    }
}
