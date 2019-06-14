using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArenaStatic;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using EsriWorkEnvironment;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

namespace ChartTypeA
{
    class TypeAChartPreperation
    {

        public static IMap ChartsPreparation(int ChartType, string ProjectName, string FolderName, string TemplatetName, IApplication m_application)
        {

            try
            {

              //  SaveSourcePDMFiles(FolderName);

                #region переместить проект в новое место, очистить индексы и relations

                var pathToTemplateFileMXD2 = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\ChartTypeA\", TemplatetName);


                var pathToTemplateMdb = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\ChartTypeA\pdm.mdb");

//                ArenaDataModule.ClearRelations_Indexes(pathToTemplateMDB);

                File.Copy(pathToTemplateMdb, System.IO.Path.Combine(FolderName, "pdm.mdb"), true);


                Application.DoEvents();
                File.Copy(pathToTemplateFileMXD2, System.IO.Path.Combine(FolderName, ProjectName), true);
                Application.DoEvents();
                //

                Application.DoEvents();


                m_application.OpenDocument(System.IO.Path.Combine(FolderName, ProjectName));

                IMxDocument pNewDocument = (IMxDocument)m_application.Document;
                pNewDocument.RelativePaths = true;
               // m_application.SaveDocument(ProjectName);
                Application.DoEvents();

                m_application.RefreshWindow();

                Application.DoEvents();
                GlobalParams.Application = m_application;
                GlobalParams.ProjectName = ProjectName;

                GlobalParams.HookHelper.Hook = m_application;

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

            ILayer _Layer = EsriUtils.getLayerByName(activeMap, "ObstacleArea");
            var fc = ((IFeatureLayer)_Layer).FeatureClass;
            var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;
            return workspaceEdit;

        }

        public static void FireEvents()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ESRI.ArcGIS.Carto.IActiveViewEvents_Event activeViewEvents = GlobalParams.Map as ESRI.ArcGIS.Carto.IActiveViewEvents_Event;
            // ESRI.ArcGIS.Carto.IActiveViewEvents_Event pageLayouViewEvents = GlobalParams.HookHelper.PageLayout as ESRI.ArcGIS.Carto.IActiveViewEvents_Event;

            if (activeViewEvents != null)
            {
                // var itemReorderEventHandler= new IActiveViewEvents_ItemReorderedEventHandler(OnItemOrdered);
                //  activeViewEvents.ItemReordered += itemReorderEventHandler;

                   var m_ActiveViewEventsAfterDraw = new ESRI.ArcGIS.Carto.IActiveViewEvents_AfterDrawEventHandler(OnActiveViewEventsItemDraw);
            //    activeViewEvents.AfterDraw += m_ActiveViewEventsAfterDraw;

                var m_ActiveViewEventViewRefreshed = new ESRI.ArcGIS.Carto.IActiveViewEvents_ViewRefreshedEventHandler(ViewRefreshed);
                activeViewEvents.ViewRefreshed += m_ActiveViewEventViewRefreshed;

                //var m_PageLayoutEventViewRefreshed = new ESRI.ArcGIS.Carto.IActiveViewEvents_ViewRefreshedEventHandler(PageLayoutViewRefreshed);
                //pageLayouViewEvents.ViewRefreshed += m_PageLayoutEventViewRefreshed;
            }
        }

        private static void OnActiveViewEventsItemDraw(IDisplay Display, esriViewDrawPhase phase)
        {
            if (phase == esriViewDrawPhase.esriViewGeography)
            {
                if (GlobalParams.VerticalObstacleCreater == null || GlobalParams.GrCreater == null) return;

                var allElements = GlobalParams.GrCreater.AllElements;
                if (allElements == null)
                {
                    allElements = GetElementByName("ProfileElem") as IGroupElement;
                    GlobalParams.GrCreater.AllElements = allElements;
                }

                var env = (allElements as IElement).Geometry.Envelope;
                var yMin = env.YMin;

                if (Math.Abs(yMin - GlobalParams.GrCreater.YMin) > 0.001)
                    GlobalParams.GrCreater.FrameHeight += yMin - GlobalParams.GrCreater.YMin;

                

                if (GlobalParams.VerticalObstacleCreater != null)
                {
                    GlobalParams.VerticalObstacleCreater.FrameHeight = GlobalParams.GrCreater.FrameHeight;
                    GlobalParams.VerticalObstacleCreater.Create();
                }
                GlobalParams.GrCreater.ObstacleElements = GlobalParams.VerticalObstacleCreater?.ObstaGroupElement;
                GlobalParams.GrCreater.ReCreate();
            }
        }

        public static void ViewRefreshed(IActiveView View, esriViewDrawPhase phase, object Data, IEnvelope envelope)
        {
            if (phase == esriViewDrawPhase.esriViewGraphicSelection || phase == esriViewDrawPhase.esriViewGeography)
            {
                if (GlobalParams.VerticalObstacleCreater == null || GlobalParams.GrCreater == null) return;

                var allElements = GlobalParams.GrCreater.AllElements;
                if (allElements == null)
                {
                    allElements = GetElementByName("ProfileElem") as IGroupElement;
                    GlobalParams.GrCreater.AllElements = allElements;
                }

                var env = (allElements as IElement).Geometry.Envelope;
                var yMin = env.YMin;

                if (Math.Abs(yMin - GlobalParams.GrCreater.YMin) > 0.001)
                    GlobalParams.GrCreater.FrameHeight += yMin - GlobalParams.GrCreater.YMin;


                if (GlobalParams.VerticalObstacleCreater != null)
                {
                    
                    GlobalParams.VerticalObstacleCreater.FrameHeight = GlobalParams.GrCreater.FrameHeight;
                    GlobalParams.VerticalObstacleCreater.Create();
                }
                GlobalParams.GrCreater.ObstacleElements = GlobalParams.VerticalObstacleCreater?.ObstaGroupElement;
                GlobalParams.GrCreater.ReCreate();
                //var pt = new PointClass() {X = 30, Y = 30};
                //GlobalParams.ScaleElement?.CreateScale(pt,GlobalParams.HookHelper.FocusMap.MapScale/10);
            }
        }

        public static IElement GetElementByName(string name)
        {
            var graphicsContainer = GlobalParams.HookHelper.PageLayout as IGraphicsContainer;

            graphicsContainer.Reset();
            var element = graphicsContainer.Next();

            while (element != null)
            {
                IElementProperties3 elemProperties = element as IElementProperties3;
                if (elemProperties.Name == name)
                    return element;
            }

            return null;

        }
    }
}
