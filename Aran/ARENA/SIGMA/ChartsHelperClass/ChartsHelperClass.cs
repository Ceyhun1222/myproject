using ANCOR.MapCore;
using ArenaStatic;
using PDM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ARENA;
using ESRI.ArcGIS.Carto;
using EsriWorkEnvironment;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using System.Runtime.InteropServices;
using ANCOR.MapElements;
using DataModule;
using ARENA.Enums_Const;
using Excel = Microsoft.Office.Interop.Excel;
using ChartCompare;

namespace SigmaChart
{
    public static class ChartsHelperClass
    {
        public static string MakeText(object obj, int Rounder = 0, int minLength = 0, double? delta = 0, string DefaultText = "NaN", bool CourseFlag = false, string AdditionalEndText = "")
        {
            string res = DefaultText;
            
            double d = 0;
            d = delta.HasValue ? delta.Value : 0;
            if (obj != null)
            {
                if (obj is Double)
                {
                    obj = (Double)obj - d;
                    if (CourseFlag && ((Double)obj) < 0) obj = (Double)obj + 360;


                    obj = Math.Round((Double)obj, Rounder);
                }
            }

            res = obj == null ? DefaultText : obj.ToString();
            if (res.CompareTo(DefaultText) == 0) return res;

            if (Rounder > 0 && !res.Contains("."))
                res = res + ".";

            if (Rounder > 0)
            {
                int pntPos = res.IndexOf(".");
                pntPos = res.Length - pntPos - 1;
                for (int i = 0; i < Rounder - pntPos; i++)
                {
                    res = res + "0";
                }
            }

            if (minLength > 0 && res.Length < minLength)
                for (int i = 0; i <= minLength - res.Length; i++)
                {
                    res = "0" + res;
                }
            if (CourseFlag) res = res + "°";
            if (AdditionalEndText.Length > 0) res = res + AdditionalEndText;
            return res + " ";
        }

        public static string MakeText(PDMObject pdmOBJ, AncorDataSource dataSource, int Rounder = 0, int minLength = 0, double? delta = 0, string DefaultText = "NaN", bool CourseFlag = false, string AdditionalEndText ="")
        {
            string res = DefaultText;

            
            var obj = ArenaStaticProc.GetObjectValue(pdmOBJ, dataSource.Value, false);

            

            double d =0;
            d = delta.HasValue ? delta.Value : 0;
            if (obj != null)
            {
                if (obj is Double)
                {
                    obj = (Double)obj - d;
                    if (CourseFlag && ((Double)obj) < 0) obj = (Double)obj + 360;

                   if (Rounder >=0)
                    obj = Math.Round((Double)obj, Rounder);
                }
            }

            res = obj == null || obj.ToString().Length ==0 ? DefaultText : obj.ToString();
            if (res.CompareTo(DefaultText) == 0) return res;

            if (Rounder > 0 && !res.Contains("."))
                res = res + ".";
           

            if (Rounder > 0)
            {
                int pntPos = res.IndexOf(".");
                pntPos = res.Length - pntPos -1;
                for (int i = 0; i < Rounder - pntPos; i++)
                {
                    res = res + "0";
                }
                string c = res.Substring(0, res.IndexOf("."));
                if (c.Length < 3) res = "0" + res;
            }

            if (minLength > 0 && res.Length < minLength)
                for (int i = 0; i <= minLength - res.Length; i++)
                {
                    res = "0" + res;
                }
            
           

            if (CourseFlag && !res.EndsWith("°"))
                res = res + "°";


            if (AdditionalEndText.Length > 0) res = res + AdditionalEndText;
            return   (dataSource.Condition.Contains(res)) ? "" : res +" ";
        }

        public static string MakeText(PDMObject pdmOBJ, AncorDataSource dataSource, coordtype _coordTP, int Rounder = 0, string DefaultText = "NaN")
        {
            string res = DefaultText;
            var obj = ArenaStaticProc.GetObjectValue(pdmOBJ, dataSource.Value, false);
            if (obj != null)
            {
                if (obj is Double) obj = Math.Round((Double)obj, Rounder);
            }

            if (dataSource.Value.StartsWith("Y") && obj != null) obj = ArenaStaticProc.LatToDDMMSS(obj.ToString(), _coordTP);
            if (dataSource.Value.StartsWith("X") && obj != null) obj = ArenaStaticProc.LonToDDMMSS(obj.ToString(), _coordTP);

            if (dataSource.Value.StartsWith("Lat") && obj != null) obj = ArenaStaticProc.LatToDDMMSS(obj.ToString(), _coordTP);
            if (dataSource.Value.StartsWith("Lon") && obj != null) obj = ArenaStaticProc.LonToDDMMSS(obj.ToString(), _coordTP);

            res = obj == null ? DefaultText : obj.ToString();

            return (dataSource.Condition.Contains(res)) ? "" : res + " ";
        }

        public static string MakeText_UOM(PDMObject pdmOBJ, AncorDataSource dataSource,string resultUom, int Rounder = 0, int minLength = 0, double? delta = 0, string DefaultText = "NaN", bool CourseFlag = false)
        {
            string res = DefaultText;
            var obj = ArenaStaticProc.GetObjectValue(pdmOBJ, dataSource.Value, false);



            double d = 0;
            d = delta.HasValue ? delta.Value : 0;
            if (obj != null)
            {
                if (obj is Double)
                {
                    obj = (Double)obj - d;
                    if (CourseFlag && ((Double)obj) < 0) obj = (Double)obj + 360;

                    if (resultUom.Length > 0)
                    {
                        string uom = ArenaStaticProc.GetObjectUomString(pdmOBJ, dataSource.Value);
                        obj = ArenaStaticProc.UomTransformation(uom, resultUom, (Double)obj, 10);
                    }

                    obj = Math.Round((Double)obj, Rounder);

                    
                }
            }

            res = obj == null ? DefaultText : obj.ToString();

            if (res.CompareTo(DefaultText) == 0) return res;

            if (Rounder > 0 && !res.Contains("."))
                res = res + ".";

            if (Rounder > 0)
            {
                int pntPos = res.IndexOf(".");
                pntPos = res.Length - pntPos - 1;
                for (int i = 0; i < Rounder - pntPos; i++)
                {
                    res = res + "0";
                }
            }

            if (minLength > 0 && res.Length < minLength)
                for (int i = 0; i <= minLength - res.Length; i++)
                {
                    res = "0" + res;
                }

            return (dataSource.Condition.Contains(res)) ? "" : res + " ";
        }



        public static IMap ChartsPreparation(int ChartType, string ProjectName, string FolderName, string TemplatetName, IApplication m_application)
        {
            
            try
            {


                //SaveSourcePDMFiles(FolderName);


                #region переместить проект в новое место, очистить индексы и relations

                var pathToTemplateFileMXD2 = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\Enroute\", TemplatetName);
                

                switch (ChartType)
                {
                    case(1):
                        pathToTemplateFileMXD2 = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\Enroute\", TemplatetName);
                        break;
                    case (2):
                        pathToTemplateFileMXD2 = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\SID\", TemplatetName);
                        break;
                    case (4):
                        pathToTemplateFileMXD2 = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\STAR\", TemplatetName);
                        break;
                    case (5):
                        pathToTemplateFileMXD2 = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\IAP\", TemplatetName);
                        break;
                    case (7):
                        pathToTemplateFileMXD2 = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\AreaChart\", TemplatetName);
                        break;
                    case (8):
                        pathToTemplateFileMXD2 = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\AerodromeElectronicChart\", TemplatetName);
                        break;
                    case (9):
                        pathToTemplateFileMXD2 = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\AerodromeParkingDockingChart\", TemplatetName);
                        break;
                    case (10):
                        pathToTemplateFileMXD2 = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\AerodromeGroundMovementChart\", TemplatetName);
                        break;
                    case (11):
                        pathToTemplateFileMXD2 = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\AerodromeBirdChart\", TemplatetName);
                        break;
                    case (12):
                        pathToTemplateFileMXD2 = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\AerodromeChart\", TemplatetName);
                        break;
                    case (6):
                        pathToTemplateFileMXD2 = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\PATC\", TemplatetName);
                        break;
                    case (13):
                        pathToTemplateFileMXD2 = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\MinimumAltitudeChart\", TemplatetName);
                        break;
                    default:
                        break;
                }


                string pathToTemplateMDB = "";

                switch (ChartType)
                {
                    case (1):
                    case (2):
                    case (4):
                    case (5):
                    case (7):
                    case (13):
                    default:
                        pathToTemplateMDB = ArenaStaticProc.GetTargetDB();
                        ArenaDataModule.ClearRelations_Indexes(pathToTemplateMDB);
                        File.Copy(pathToTemplateMDB, System.IO.Path.Combine(FolderName, "pdm.mdb"), true);
                        Application.DoEvents();
                        File.Copy(pathToTemplateFileMXD2, System.IO.Path.Combine(FolderName, ProjectName), true);
                        Application.DoEvents();

                        break;
                    case (8):
                    case (9):
                    case (10):
                    case (11):
                    case (12):
                        pathToTemplateMDB = ArenaStaticProc.GetTargetDB();
                        ArenaDataModule.ClearRelations_Indexes(pathToTemplateMDB);
                        File.Copy(pathToTemplateMDB, System.IO.Path.Combine(FolderName, "aerodrome.mdb"), true);
                        Application.DoEvents();
                        File.Copy(pathToTemplateFileMXD2, System.IO.Path.Combine(FolderName, ProjectName), true);
                        Application.DoEvents();

                        break;
                   
                   
                }

                
                //

                m_application.OpenDocument(System.IO.Path.Combine(FolderName, ProjectName));

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

        public static void SetMapGridVisibilityState(IActiveView activeView, bool visibilityState)
        {
            IMap map = activeView.FocusMap;
            IGraphicsContainer graphicsContainer = activeView as IGraphicsContainer;
            IFrameElement frameElement = graphicsContainer.FindFrame(map);
            IMapFrame mapFrame = frameElement as IMapFrame;
            IMapGrids mapGrids = mapFrame as IMapGrids;

            IMapGrid mapGrid = null;
            if (mapGrids.MapGridCount > 0)
            {
                mapGrid = mapGrids.get_MapGrid(0);
                mapGrid.Visible = visibilityState;
            }
        }

        public static void ChartsFinalisation(IApplication m_application, IMap FocusMap, int ChartType, string PN ,string AreaOfInterestLayer = "", IAOIBookmark _mapBookmarks = null, string whereClause = "OBJECTID >0")
        {
           
            #region Стереть следы арена

            try
            {
                DataCash.ProjectEnvironment = null;

                var projName = ArenaStaticProc.GetTargetDB();//System.IO.Path.GetDirectoryName(ArenaStaticProc.GetTargetDB()) + @"\pdm.pdm";
                string[] FN = Directory.GetFiles(System.IO.Path.GetDirectoryName(projName), "*.pdm");
                foreach (var fl in FN)
                {
                    System.IO.File.Delete(fl);
                    Application.DoEvents();
                }

                ArenaStaticProc.SetTargetDB("");

            }
            catch { MessageBox.Show("Finalisation error"); }

            Application.DoEvents();

            #endregion


            #region refresh Annotation Feature Layers data source

            ILayer _Layer = EsriUtils.getLayerByName(FocusMap, "AirportHeliport");
            if (_Layer == null) _Layer = EsriUtils.getLayerByName(FocusMap, "AirportCartography");


            var fc = ((IFeatureLayer)_Layer).FeatureClass;
            var workspace = (IFeatureWorkspace)fc.FeatureDataset.Workspace;

            ILayer AnnoLayer = EsriUtils.getLayerByName(FocusMap, "Annotations");
            if (AnnoLayer != null)
            {
                ICompositeLayer Complayer = (ICompositeLayer)AnnoLayer;
                for (int j = 0; j <= Complayer.Count - 1; j++)
                {
                    try
                    {
                        ILayer Layer2 = Complayer.get_Layer(j);
                        string fcName = Layer2.Name;
                        var pFeatureClass = workspace.OpenFeatureClass(fcName);
                        var pFeatureLayer = (IFeatureLayer)Layer2;
                        pFeatureLayer.FeatureClass = pFeatureClass;
                    }
                    catch 
                    {
                        continue;
                    }
                }
            }

            _Layer = EsriUtils.getLayerByName(FocusMap, "ProcedureLegs");
            if (_Layer != null) FocusMap.DeleteLayer(_Layer);

            _Layer = EsriUtils.getLayerByName(FocusMap, "RouteSegment");
            if (_Layer != null) FocusMap.DeleteLayer(_Layer);

            _Layer = EsriUtils.getLayerByName(FocusMap, "AirspaceVolume");
            if (_Layer != null) FocusMap.DeleteLayer(_Layer);

            _Layer = EsriUtils.getLayerByName(FocusMap, "AirportHeliport");
            if (_Layer != null) FocusMap.DeleteLayer(_Layer);

            _Layer = EsriUtils.getLayerByName(FocusMap, "WayPoint");
            if (_Layer != null) FocusMap.DeleteLayer(_Layer);

            #endregion

            if (AreaOfInterestLayer.Length > 0)
                ZoomToAreaOfInterestLayer(m_application, FocusMap, AreaOfInterestLayer, whereClause);


            //((FocusMap as IActiveView) as IPageLayout).ZoomToWhole();

            if (_mapBookmarks != null)
            {
                IMapBookmarks mapBookmarks = (IMapBookmarks)FocusMap;
                //Add the bookmark to the bookmarks collection
                mapBookmarks.RemoveAllBookmarks();
                mapBookmarks.AddBookmark(_mapBookmarks);

                ISpatialBookmark spatialBookmark = _mapBookmarks;


                //Zoom to the bookmark
                spatialBookmark.ZoomTo(FocusMap);
                
            }

            

            FocusMap.Description = ChartType.ToString();
            m_application.SaveDocument();

            #region DataCash.ProjectEnvironment

            string selectedChart = ((IMapDocument)m_application.Document).DocumentFilename;
            var tempDirName = System.IO.Path.GetDirectoryName(selectedChart);
            ArenaProjectType prjType = ArenaProjectType.ARENA;

            ARENA.Environment.Environment curEnvironment = new ARENA.Environment.Environment { mxApplication = m_application, pMap = ((IMxDocument)m_application.Document).ActiveView.FocusMap };
            curEnvironment.Data.PdmObjectList.AddRange(ArenaDataModule.GetObjectsFromPdmFile(tempDirName, ref prjType));

            DataCash.ProjectEnvironment = curEnvironment;


            #endregion


            for (int i = 0; i < ((IMxDocument)m_application.Document).ContentsViewCount; i++)
            {
                IContentsView cnts = ((IMxDocument)m_application.Document).get_ContentsView(i);

                string cntxtName = ((IMxDocument)m_application.Document).ContentsView[i].Name;

                if (cntxtName.StartsWith("ANCORTOCLayerView"))
                {
                    //((IMxDocument)m_application.Document).CurrentContentsView = cnts;
                    ((IMxDocument)m_application.Document).ContentsView[i].Refresh(cntxtName);
                }
                if (cntxtName.StartsWith("TOCLayerFilter")) ((IMxDocument)m_application.Document).ContentsView[i].Refresh(cntxtName);

            }

            /////////////////////////////////
            if (!((IMxDocument)m_application.Document).CurrentContentsView.Name.StartsWith("ANCORTOCLayerView"))
            {
                ArenaStaticProc.BringToFrontToc((IMxDocument)m_application.Document, "ANCORTOCLayerView");
            }
            /////////////////////////////////

            Create_CEFID_File(System.IO.Path.GetDirectoryName(((IMapDocument)m_application.Document).DocumentFilename));

            ((IMxDocument)m_application.Document).RelativePaths = true;

            m_application.Caption = System.IO.Path.GetFileNameWithoutExtension(PN);

            Application.DoEvents();
        }

        public static void Create_CEFID_File(string DocumentFileName)
        {
            string selectedChart = DocumentFileName;
            var tempDirName = DocumentFileName;//System.IO.Path.GetDirectoryName(selectedChart);

            List<string> CEFID = new List<string>();

            foreach (var item in SigmaDataCash.AnnotationFeatureClassList.Values)
            {
                try
                {
                    if (!(item is IFeatureClass)) continue;

                    IFeatureClass annoFC = (IFeatureClass)item;

                    if (annoFC.FindField("PdmUID") < 0) continue;

                    if (annoFC.AliasName.StartsWith("FreeAnno")) continue;

                    IQueryFilter featFilter = new QueryFilterClass();

                    featFilter.WhereClause = "PdmUID <> ''";

                    IFeatureCursor featCur = annoFC.Search(featFilter, false);

                    IFeature feat;
                    while ((feat = featCur.NextFeature()) != null)
                    {
                        if (feat != null)
                        {
                            int fINDX = feat.Fields.FindField("PdmUID");
                            if (fINDX >= 0)
                            {
                                string PdmUID = feat.get_Value(feat.Fields.FindField("PdmUID")).ToString();
                                if (CEFID.IndexOf(PdmUID) < 0) CEFID.Add(PdmUID);
                            }


                        }
                    }
                    Marshal.ReleaseComObject(featCur);
                }

                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

            }

            if (CEFID.Count > 0)
                System.IO.File.WriteAllLines(tempDirName + @"\CEFID.txt", CEFID);
        }

        public static List<string> Read_CEFID(string path)
        {
            if (!File.Exists(path + @"\CEFID.txt"))
            {
                Create_CEFID_File(path);
            }

                if (File.Exists(path + @"\CEFID.txt"))
            {
                return File.ReadLines(path + @"\CEFID.txt").ToList();
            }
            else
                return null;
        }

        public static void ZoomToAreaOfInterestLayer(IApplication m_application,IMap pMap, string AreaOfInterestLayer, string WhereClause = "OBJECTID >0")
        {
            try
            {
                //double mpScl = pMap.MapScale;

                pMap.ClearSelection();
                ILayer layer = EsriUtils.getLayerByName(pMap, AreaOfInterestLayer);
                if (layer == null) return;
                var pSelect = layer as IFeatureSelection;


                if (pSelect != null)
                {
                    pSelect.CombinationMethod = esriSelectionResultEnum.esriSelectionResultAdd;
                    

                    IQueryFilter queryFilter = new QueryFilterClass();
                    queryFilter.WhereClause = WhereClause;

                    pSelect.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultAdd, false);

                    UID menuID = new UIDClass();

                    menuID.Value = "{AB073B49-DE5E-11D1-AA80-00C04FA37860}"; //zoomToSelected

                    ICommandItem pCmdItem = m_application.Document.CommandBars.Find(menuID);
                    pCmdItem.Execute();
                    Marshal.ReleaseComObject(pCmdItem);
                    Marshal.ReleaseComObject(menuID);

                    pMap.ClearSelection();

                }

                //pMap.MapScale = mpScl;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
                throw;
            }

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

        }

        public static void SaveSourcePDMFiles(string projName, List<PDMObject> pdmList =null)
        {

            PDM_ObjectsList Tmp_PdmObjectList = pdmList == null ?  new PDM_ObjectsList(DataCash.ProjectEnvironment.Data.PdmObjectList, "sigma_enroute_chart") : 
                                                                   new PDM_ObjectsList(pdmList, "sigma_enroute_chart");


            string[] FN = pdmList == null ? Directory.GetFiles(projName, "*.*") : Directory.GetFiles(projName, "*.obj");

            foreach (var fl in FN)
            {
                try
                {
                    System.IO.File.Delete(fl);
                }
                catch { continue; }
            }

            projName = projName + @"\pdm.obj";


            #region PDM objects files

            if (Tmp_PdmObjectList.PDMObject_list.Count > 0)
            {
                int ObjCount = Tmp_PdmObjectList.PDMObject_list.Count;
                int Size = 1000;

                int Steps = ObjCount / Size;
                int part = ObjCount - Steps * Size;
                int index = 1;

                PDMObject[] newList = new PDMObject[part];
                Tmp_PdmObjectList.PDMObject_list.CopyTo(0, newList, 0, part);
                PDM_ObjectsList ResPart = pdmList == null ? new PDM_ObjectsList(newList.ToList(), DataCash.ProjectEnvironment.Data.CurrentProjectType.ToString()) :
                                                            new PDM_ObjectsList(newList.ToList(), ARENA.Enums_Const.ArenaProjectType.SIGMA.ToString());

                ArenaStatic.ArenaStaticProc.Serialize(ResPart, projName);
                index++;

                for (int i = 0; i <= Steps - 1; i++)
                {
                    newList = new PDMObject[Size];
                    Tmp_PdmObjectList.PDMObject_list.CopyTo(part + Size * i, newList, 0, Size);
                    ResPart = pdmList == null ? new PDM_ObjectsList(newList.ToList(), DataCash.ProjectEnvironment.Data.CurrentProjectType.ToString()) :
                                                new PDM_ObjectsList(newList.ToList(), ARENA.Enums_Const.ArenaProjectType.SIGMA.ToString());
                    string newFN = projName.Replace("pdm.obj", (i + 1).ToString() + "pdm.obj");
                    ArenaStatic.ArenaStaticProc.Serialize(ResPart, newFN);
                    index++;
                }
                newList = null;
                ResPart = null;

            }
            Tmp_PdmObjectList = null;

            #endregion
        }

        public static void SaveAirspace_ChartGeo(IFeatureClass featureClass, PDM.AirspaceVolume arspsVol)
        {
            if (featureClass == null) return;

            int fID = -1;
            IFeature pFeat = featureClass.CreateFeature();

            if (arspsVol.Geo == null) arspsVol.RebuildGeo2();

            if (arspsVol.Geo is ESRI.ArcGIS.Geometry.IZ2)
            {
                ESRI.ArcGIS.Geometry.IZ2 z = (ESRI.ArcGIS.Geometry.IZ2)arspsVol.Geo;
                z.SetNonSimpleZs(0.0);
            }
            IZAware zAware = (ESRI.ArcGIS.Geometry.IZAware)arspsVol.Geo;
            zAware.ZAware = false;


            fID = pFeat.Fields.FindField("SHAPE"); if (fID > 0) pFeat.set_Value(fID, arspsVol.Geo);
            fID = pFeat.Fields.FindField("FeatureGUID"); if (fID > 0) pFeat.set_Value(fID, arspsVol.ID);
            fID = pFeat.Fields.FindField("codeId"); if (fID > 0) pFeat.set_Value(fID, arspsVol.CodeId);
            fID = pFeat.Fields.FindField("txtName"); if (fID > 0) pFeat.set_Value(fID, arspsVol.TxtName);
            fID = pFeat.Fields.FindField("codeClass"); if (fID > 0) pFeat.set_Value(fID, arspsVol.CodeClass);
            fID = pFeat.Fields.FindField("codeLocInd"); if (fID > 0) pFeat.set_Value(fID, arspsVol.CodeLocInd);
            fID = pFeat.Fields.FindField("codeDistVerUpper"); if (fID > 0) pFeat.set_Value(fID, arspsVol.CodeDistVerUpper.ToString());
            fID = pFeat.Fields.FindField("valDistVerUpper"); if (fID > 0) pFeat.set_Value(fID, arspsVol.ValDistVerUpper);
            fID = pFeat.Fields.FindField("uomDistVerUpper"); if (fID > 0) pFeat.set_Value(fID, arspsVol.UomValDistVerUpper.ToString());
            fID = pFeat.Fields.FindField("codeDistVerLower"); if (fID > 0) pFeat.set_Value(fID, arspsVol.CodeDistVerLower);
            fID = pFeat.Fields.FindField("valDistVerLower"); if (fID > 0) pFeat.set_Value(fID, arspsVol.ValDistVerLower);
            fID = pFeat.Fields.FindField("uomDistVerLower"); if (fID > 0) pFeat.set_Value(fID, arspsVol.UomValDistVerLower.ToString());
            fID = pFeat.Fields.FindField("codeDistVerMax"); if (fID > 0) pFeat.set_Value(fID, arspsVol.CodeDistVerMax.ToString());
            fID = pFeat.Fields.FindField("valDistVerMax"); if (fID > 0) pFeat.set_Value(fID, arspsVol.ValDistVerMax);
            fID = pFeat.Fields.FindField("uomDistVerMax"); if (fID > 0) pFeat.set_Value(fID, arspsVol.UomValDistVerMax.ToString());
            fID = pFeat.Fields.FindField("valDistVerMnm"); if (fID > 0) pFeat.set_Value(fID, arspsVol.ValDistVerMnm);
            fID = pFeat.Fields.FindField("codeDistVerMnm"); if (fID > 0) pFeat.set_Value(fID, arspsVol.CodeDistVerMnm.ToString());
            fID = pFeat.Fields.FindField("valLowerLimit"); if (fID > 0) pFeat.set_Value(fID, arspsVol.ValLowerLimit);
            fID = pFeat.Fields.FindField("codeType"); if (fID > 0) pFeat.set_Value(fID, arspsVol.CodeType.ToString());
            fID = pFeat.Fields.FindField("uomDistVerMnm"); if (fID > 0) pFeat.set_Value(fID, arspsVol.UomValDistVerMnm.ToString());
            fID = pFeat.Fields.FindField("localtype"); if (fID > 0) pFeat.set_Value(fID, arspsVol.TxtLocalType);




            pFeat.Store();

        }

        public static void UpdateAirspace_ChartGeo(IFeatureClass featureClass, PDM.AirspaceVolume arspsVol)
        {
            if (featureClass == null) return;
            IFeatureCursor featCur = null;

            if (arspsVol.Geo == null) arspsVol.RebuildGeo();
            IGeometry arspsVolGeo = arspsVol.Geo;

            //try
            {


                //if (arspsVolGeo is ESRI.ArcGIS.Geometry.IZ2)
                {
                    ESRI.ArcGIS.Geometry.IZ2 z = (ESRI.ArcGIS.Geometry.IZ2)arspsVolGeo;
                    z.SetNonSimpleZs(0.0);
                }
                IZAware zAware = (ESRI.ArcGIS.Geometry.IZAware)arspsVolGeo;
                zAware.ZAware = false;

            }
            //catch (Exception ex)
            //{

            //    System.Diagnostics.Debug.WriteLine(ex.Message);
            //}

            IQueryFilter featFilter = new QueryFilterClass();

            string id2 = arspsVol.ID;
            if (arspsVol.ID.Length > 36)
                id2 = arspsVol.ID.Remove(36);
            featFilter.WhereClause = "FeatureGUID = '" + arspsVol.ID + "' OR FeatureGUID = '"  + id2 + "'";

            featCur = featureClass.Update(featFilter, false);

            IFeature pFeat = featCur.NextFeature();
            int fID = -1;
            if (pFeat != null)
            {
                fID = pFeat.Fields.FindField("SHAPE"); if (fID > 0) pFeat.set_Value(fID, arspsVolGeo);
                fID = pFeat.Fields.FindField("FeatureGUID"); if (fID > 0) pFeat.set_Value(fID, arspsVol.ID);
                fID = pFeat.Fields.FindField("codeId"); if (fID > 0) pFeat.set_Value(fID, arspsVol.CodeId);
                fID = pFeat.Fields.FindField("txtName"); if (fID > 0) pFeat.set_Value(fID, arspsVol.TxtName);
                fID = pFeat.Fields.FindField("codeClass"); if (fID > 0) pFeat.set_Value(fID, arspsVol.CodeClass);
                fID = pFeat.Fields.FindField("codeLocInd"); if (fID > 0) pFeat.set_Value(fID, arspsVol.CodeLocInd);
                fID = pFeat.Fields.FindField("codeDistVerUpper"); if (fID > 0) pFeat.set_Value(fID, arspsVol.CodeDistVerUpper.ToString());
                fID = pFeat.Fields.FindField("valDistVerUpper"); if (fID > 0) pFeat.set_Value(fID, arspsVol.ValDistVerUpper);
                fID = pFeat.Fields.FindField("uomDistVerUpper"); if (fID > 0) pFeat.set_Value(fID, arspsVol.UomValDistVerUpper.ToString());
                fID = pFeat.Fields.FindField("codeDistVerLower"); if (fID > 0) pFeat.set_Value(fID, arspsVol.CodeDistVerLower);
                fID = pFeat.Fields.FindField("valDistVerLower"); if (fID > 0) pFeat.set_Value(fID, arspsVol.ValDistVerLower);
                fID = pFeat.Fields.FindField("uomDistVerLower"); if (fID > 0) pFeat.set_Value(fID, arspsVol.UomValDistVerLower.ToString());
                fID = pFeat.Fields.FindField("codeDistVerMax"); if (fID > 0) pFeat.set_Value(fID, arspsVol.CodeDistVerMax.ToString());
                fID = pFeat.Fields.FindField("valDistVerMax"); if (fID > 0) pFeat.set_Value(fID, arspsVol.ValDistVerMax);
                fID = pFeat.Fields.FindField("uomDistVerMax"); if (fID > 0) pFeat.set_Value(fID, arspsVol.UomValDistVerMax.ToString());
                fID = pFeat.Fields.FindField("valDistVerMnm"); if (fID > 0) pFeat.set_Value(fID, arspsVol.ValDistVerMnm);
                fID = pFeat.Fields.FindField("codeDistVerMnm"); if (fID > 0) pFeat.set_Value(fID, arspsVol.CodeDistVerMnm.ToString());
                fID = pFeat.Fields.FindField("valLowerLimit"); if (fID > 0) pFeat.set_Value(fID, arspsVol.ValLowerLimit);
                fID = pFeat.Fields.FindField("codeType"); if (fID > 0) pFeat.set_Value(fID, arspsVol.CodeType.ToString());
                fID = pFeat.Fields.FindField("uomDistVerMnm"); if (fID > 0) pFeat.set_Value(fID, arspsVol.UomValDistVerMnm.ToString());
                pFeat.Store();

            }

            Marshal.ReleaseComObject(featCur);
        }


        public static void UpdateHoldingPattern_Direction(IFeatureClass featureClass, PDM.HoldingPattern hlngPttrn)
        {
            if (featureClass == null) return;
            IFeatureCursor featCur = null;

            IQueryFilter featFilter = new QueryFilterClass();
            featFilter.WhereClause = "FeatureGUID = '" + hlngPttrn.ID+"'";

            featCur = featureClass.Update(featFilter, false);

            IFeature pFeat = featCur.NextFeature();
            int fID = -1;
            if (pFeat != null)
            {
                fID = pFeat.Fields.FindField("turnDirection"); if (fID > 0) pFeat.set_Value(fID, hlngPttrn.TurnDirection.ToString());
                pFeat.Store();

            }

            Marshal.ReleaseComObject(featCur);

        }


        public static void SavePocedureLeg_ChartGeo(IFeatureClass featureClass, PDM.ProcedureLeg procLeg)
        {
            int fID = -1;
            IFeature pFeat = featureClass.CreateFeature();

            if (procLeg.Geo == null) procLeg.RebuildGeo2();

            IZAware zAware = (ESRI.ArcGIS.Geometry.IZAware)procLeg.Geo;
            zAware.ZAware = true;


            fID = pFeat.Fields.FindField("SHAPE"); if (fID > 0) pFeat.set_Value(fID, procLeg.Geo);
            fID =  pFeat.Fields.FindField("ID_Transition"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.TransitionIdentifier);
            fID =  pFeat.Fields.FindField("FeatureGUID"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.ID);
            fID =  pFeat.Fields.FindField("seqNumberARINC"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.SeqNumberARINC);
            fID =  pFeat.Fields.FindField("altitudeInterpretation"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.AltitudeInterpretation.ToString());
            fID =  pFeat.Fields.FindField("altitudeOverrideATC"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.AltitudeOverrideATC);
            fID =  pFeat.Fields.FindField("altitudeUOM"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.AltitudeUOM.ToString());
            fID =  pFeat.Fields.FindField("endConditionDesignator"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.EndConditionDesignator.ToString());
            fID =  pFeat.Fields.FindField("bankAngle"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.BankAngle);
            fID =  pFeat.Fields.FindField("course"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.Course);
            fID =  pFeat.Fields.FindField("courseDirection"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.CourseDirection.ToString());
            fID =  pFeat.Fields.FindField("courseType"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.CourseType.ToString());
            fID =  pFeat.Fields.FindField("procedureTurnRequired"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.ProcedureTurnRequired);
            fID =  pFeat.Fields.FindField("duration"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.Duration);
            fID =  pFeat.Fields.FindField("durationUOM"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.DurationUOM.ToString());
            fID =  pFeat.Fields.FindField("legPathField"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.LegPathField.ToString());
            fID =  pFeat.Fields.FindField("legTypeARINC"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.LegTypeARINC.ToString());
            fID =  pFeat.Fields.FindField("length"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.Length);
            fID =  pFeat.Fields.FindField("lengthUOM"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.LengthUOM.ToString());
            fID =  pFeat.Fields.FindField("lowerLimitAltitude"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.LowerLimitAltitude);
            fID =  pFeat.Fields.FindField("lowerLimitAltitudeUOM"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.LowerLimitAltitudeUOM.ToString());
            fID =  pFeat.Fields.FindField("speedLimit"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.SpeedLimit);
            fID =  pFeat.Fields.FindField("speedLimitUOM"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.SpeedUOM.ToString());
            fID =  pFeat.Fields.FindField("turnDirection"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.TurnDirection.ToString());
            fID =  pFeat.Fields.FindField("upperLimitAltitude"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.UpperLimitAltitude);
            fID =  pFeat.Fields.FindField("upperLimitAltitudeUOM"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.UpperLimitAltitudeUOM.ToString());
            fID =  pFeat.Fields.FindField("verticalAngle"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.VerticalAngle);
            fID =  pFeat.Fields.FindField("ActualDate"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.ActualDate);
            fID =  pFeat.Fields.FindField("LegSpecialization"); if (fID >= 0)  pFeat.set_Value(fID, procLeg.LegSpecialization.ToString());

            fID =  pFeat.Fields.FindField("StartPontID"); if ((fID >= 0) && (procLeg.StartPoint != null))  pFeat.set_Value(fID, procLeg.StartPoint.ID);
            fID = pFeat.Fields.FindField("EndPontID"); if ((fID >= 0) && (procLeg.EndPoint != null)) pFeat.set_Value(fID, procLeg.EndPoint.ID);

            fID = pFeat.Fields.FindField("StartPontDesignator");
            if (fID >= 0)
                pFeat.set_Value(fID,  procLeg.StartPoint != null? procLeg.StartPoint.SegmentPointDesignator : " - ");

            fID = pFeat.Fields.FindField("EndPontDesignator");
            if (fID >= 0)
                pFeat.set_Value(fID, procLeg.EndPoint != null? procLeg.EndPoint.SegmentPointDesignator : " - ");


            fID = pFeat.Fields.FindField("ArcCenterPontID"); if ((fID >= 0) && (procLeg.ArcCentre != null)) pFeat.set_Value(fID, procLeg.ArcCentre.ID);
            fID = pFeat.Fields.FindField("FirstLastFlag"); if (fID >= 0) pFeat.set_Value(fID, procLeg.PositionFlag);
            fID = pFeat.Fields.FindField("ProcName");
            if (fID >= 0)
            {
                string pN = procLeg.ProcedureIdentifier;
                if (pN != null && procLeg.ProcedureIdentifier.StartsWith("ignore")) pN = procLeg.ProcedureIdentifier.Remove(0, 6);
                pFeat.set_Value(fID, pN);
            }

            fID = pFeat.Fields.FindField("LandingArea"); if (fID >= 0 && procLeg.Notes !=null && procLeg.Notes.Count > 1) pFeat.set_Value(fID, procLeg.Notes[0]);
            fID = pFeat.Fields.FindField("AerodromeName"); if (fID >= 0 && procLeg.Notes != null && procLeg.Notes.Count > 1) pFeat.set_Value(fID, procLeg.Notes[1]);

            pFeat.Store();

        }


        public static void SavePocedureLeg_ChartGeo(IFeatureClass featureClass, PDM.ProcedureLeg procLeg, PDM.HoldingPattern procHolding)
        {
            int fID = -1;
            IFeature pFeat = featureClass.CreateFeature();

            if (procHolding.Geo == null) procHolding.RebuildGeo2();

            IZAware zAware = (ESRI.ArcGIS.Geometry.IZAware)procHolding.Geo;
            zAware.ZAware = true;


            fID = pFeat.Fields.FindField("SHAPE"); if (fID > 0) pFeat.set_Value(fID, procHolding.Geo);
            fID = pFeat.Fields.FindField("ID_Transition"); if (fID >= 0 && procHolding.ProcedureLegID!=null) pFeat.set_Value(fID, procHolding.ProcedureLegID);
            fID = pFeat.Fields.FindField("FeatureGUID"); if (fID >= 0) pFeat.set_Value(fID, procHolding.ID);
            fID = pFeat.Fields.FindField("seqNumberARINC"); if (fID >= 0 && procLeg!=null) pFeat.set_Value(fID, procLeg.SeqNumberARINC);
            //fID = pFeat.Fields.FindField("altitudeInterpretation"); if (fID >= 0) pFeat.set_Value(fID, procHolding.AltitudeInterpretation.ToString());
            //fID = pFeat.Fields.FindField("altitudeOverrideATC"); if (fID >= 0) pFeat.set_Value(fID, procHolding.AltitudeOverrideATC);
            //fID = pFeat.Fields.FindField("altitudeUOM"); if (fID >= 0) pFeat.set_Value(fID, procHolding.AltitudeUOM.ToString());
            //fID = pFeat.Fields.FindField("endConditionDesignator"); if (fID >= 0) pFeat.set_Value(fID, procHolding.EndConditionDesignator.ToString());
            //fID = pFeat.Fields.FindField("bankAngle"); if (fID >= 0) pFeat.set_Value(fID, procHolding.BankAngle);
            fID = pFeat.Fields.FindField("course"); if (fID >= 0) pFeat.set_Value(fID, procHolding.InboundCourse);
            //fID = pFeat.Fields.FindField("courseDirection"); if (fID >= 0) pFeat.set_Value(fID, procHolding.CourseDirection.ToString());
            //fID = pFeat.Fields.FindField("courseType"); if (fID >= 0) pFeat.set_Value(fID, procHolding.CourseType.ToString());
            //fID = pFeat.Fields.FindField("procedureTurnRequired"); if (fID >= 0) pFeat.set_Value(fID, procHolding.ProcedureTurnRequired);
            //fID = pFeat.Fields.FindField("duration"); if (fID >= 0) pFeat.set_Value(fID, procHolding.Duration);
            //fID = pFeat.Fields.FindField("durationUOM"); if (fID >= 0) pFeat.set_Value(fID, procHolding.DurationUOM.ToString());
            //fID = pFeat.Fields.FindField("legPathField"); if (fID >= 0) pFeat.set_Value(fID, procHolding.LegPathField.ToString());
            //fID = pFeat.Fields.FindField("legTypeARINC"); if (fID >= 0) pFeat.set_Value(fID, procHolding.LegTypeARINC.ToString());
            //fID = pFeat.Fields.FindField("length"); if (fID >= 0) pFeat.set_Value(fID, procHolding.Length);
            //fID = pFeat.Fields.FindField("lengthUOM"); if (fID >= 0) pFeat.set_Value(fID, procHolding.LengthUOM.ToString());
            fID = pFeat.Fields.FindField("lowerLimitAltitude"); if (fID >= 0) pFeat.set_Value(fID, procHolding.LowerLimit);
            fID = pFeat.Fields.FindField("lowerLimitAltitudeUOM"); if (fID >= 0) pFeat.set_Value(fID, procHolding.LowerLimitUOM.ToString());
            fID = pFeat.Fields.FindField("speedLimit"); if (fID >= 0) pFeat.set_Value(fID, procHolding.SpeedLimit);
            fID = pFeat.Fields.FindField("speedLimitUOM"); if (fID >= 0) pFeat.set_Value(fID, procHolding.SpeedLimitUOM.ToString());
            fID = pFeat.Fields.FindField("turnDirection"); if (fID >= 0) pFeat.set_Value(fID, procHolding.TurnDirection.ToString());
            fID = pFeat.Fields.FindField("upperLimitAltitude"); if (fID >= 0) pFeat.set_Value(fID, procHolding.UpperLimit);
            fID = pFeat.Fields.FindField("upperLimitAltitudeUOM"); if (fID >= 0) pFeat.set_Value(fID, procHolding.UpperLimitUOM.ToString());
            //fID = pFeat.Fields.FindField("verticalAngle"); if (fID >= 0) pFeat.set_Value(fID, procHolding.VerticalAngle);
            fID = pFeat.Fields.FindField("ActualDate"); if (fID >= 0) pFeat.set_Value(fID, procHolding.ActualDate);
            //fID = pFeat.Fields.FindField("LegSpecialization"); if (fID >= 0) pFeat.set_Value(fID, procHolding.LegSpecialization.ToString());

            fID = pFeat.Fields.FindField("StartPontID"); if ((fID >= 0) && (procHolding.HoldingPoint != null)) pFeat.set_Value(fID, procHolding.HoldingPoint.ID);
            //fID = pFeat.Fields.FindField("EndPontID"); if ((fID >= 0) && (procHolding.EndPoint != null)) pFeat.set_Value(fID, procHolding.EndPoint.ID);
            //fID = pFeat.Fields.FindField("ArcCenterPontID"); if ((fID >= 0) && (procHolding.ArcCentre != null)) pFeat.set_Value(fID, procHolding.ArcCentre.ID);
            fID = pFeat.Fields.FindField("FirstLastFlag"); if (fID >= 0) pFeat.set_Value(fID, 2);


            pFeat.Store();

        }

        public static void SelectChartTemplate(ref AxPageLayoutControl axPageLayoutControl, string TemplateFolder, ref ListBox lstBx, out double mapSize_Width, out double mapSize_Height)
        {
            axPageLayoutControl.LoadMxFile(TemplateFolder);
            lstBx.Items.Clear();
            string PageOrientation = "Portrait";
            if (axPageLayoutControl.PageLayout.Page.Orientation == 2) PageOrientation = "Landscape";
            lstBx.Items.Add("Page orientation = " + PageOrientation);
            lstBx.Items.Add("Page Size = " + ToString(axPageLayoutControl.PageLayout.Page.FormID));

            esriUnits unts = axPageLayoutControl.PageLayout.Page.Units;
            axPageLayoutControl.PageLayout.Page.Units = esriUnits.esriMeters;

            //получить mapSize_Width и mapSize_Height строго в метрах
            IGraphicsContainer graphics = (IGraphicsContainer)axPageLayoutControl.PageLayout;
            IFrameElement frameElement = graphics.FindFrame(axPageLayoutControl.ActiveView.FocusMap);
            IMapFrame mapFrame = (IMapFrame)frameElement;
            IElement mapElement = (IElement)mapFrame;
            IGeometry frameGmtr = mapElement.Geometry;
            mapSize_Width = frameGmtr.Envelope.Width;
            mapSize_Height = frameGmtr.Envelope.Height;

            //поменять единицы измерения и вывести значения mapSize_Width и mapSize_Height на экран
            axPageLayoutControl.PageLayout.Page.Units = unts;

            frameElement = graphics.FindFrame(axPageLayoutControl.ActiveView.FocusMap);
            mapFrame = (IMapFrame)frameElement;
            mapElement = (IElement)mapFrame;
            frameGmtr = mapElement.Geometry;


            lstBx.Items.Add("Map Frame Width = " + Math.Round(frameGmtr.Envelope.Width, 2).ToString());
            lstBx.Items.Add("Map Frame Height = " + Math.Round(frameGmtr.Envelope.Height, 2).ToString());

            lstBx.Items.Add("Page Units = " + ToString(axPageLayoutControl.PageLayout.Page.Units));
        }

        private static string ToString(ESRI.ArcGIS.esriSystem.esriUnits esriUnits)
        {
            string res = "";

            switch (esriUnits)
            {
                case ESRI.ArcGIS.esriSystem.esriUnits.esriCentimeters:
                    res = "Centimeters";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriDecimalDegrees:
                    res = "DecimalDegrees";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriDecimeters:
                    res = "Decimeters";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriFeet:
                    res = "Feet";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriInches:
                    res = "Inches";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriKilometers:
                    res = "Kilometers";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriMeters:
                    res = "Meters";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriMiles:
                    res = "Miles";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriMillimeters:
                    res = "Millimeters";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriNauticalMiles:
                    res = "Nautical Miles";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriPoints:
                    res = "Points";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriUnitsLast:
                    res = "Units Last";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriUnknownUnits:
                    res = "Unknown Units";
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriYards:
                    res = "Yards";
                    break;
                default:
                    res = "";
                    break;
            }

            return res;
        }

        private static string ToString(ESRI.ArcGIS.Carto.esriPageFormID esriPageFormID)
        {
            string res = "A0";
            switch (esriPageFormID)
            {
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormA0:
                    res = "A0";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormA1:
                    res = "A1";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormA2:
                    res = "A2";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormA3:
                    res = "A3";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormA4:
                    res = "A4";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormA5:
                    res = "A5";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormC:
                    res = "C";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormCUSTOM:
                    res = "CUSTOM";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormD:
                    res = "D";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormE:
                    res = "E";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormLegal:
                    res = "Legal";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormLetter:
                    res = "Letter";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormSameAsPrinter:
                    res = "SameAsPrinter";
                    break;
                case ESRI.ArcGIS.Carto.esriPageFormID.esriPageFormTabloid:
                    res = "Tabloid";
                    break;
                default:
                    res = "A0";
                    break;
            }
            return res;
        }

        public static double Calc_Scale(double mapHeight, double mapWidth, PDM_ENUM pDM_ENUM,IMap FcsMp, ISpatialReference pSpatialReference)
        {
            var ENRT_featureList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == pDM_ENUM) select element).ToList();
            object Missing = Type.Missing;
            IGeometryCollection bag = new GeometryBagClass() as IGeometryCollection;
            double res = 1000000;

            if (ENRT_featureList != null && ENRT_featureList.Count > 0)
            {


                foreach (Enroute enrt in ENRT_featureList)
                {
                    foreach (RouteSegment rtseg in enrt.Routes)
                    {
                        if (rtseg.StartPoint.Geo == null) rtseg.StartPoint.RebuildGeo();
                        bag.AddGeometry(rtseg.StartPoint.Geo, ref Missing, ref Missing);
                        if (rtseg.EndPoint.Geo == null) rtseg.EndPoint.RebuildGeo();
                        bag.AddGeometry(rtseg.EndPoint.Geo, ref Missing, ref Missing);

                    }
                }

                IGeometry geom = EsriUtils.ToProject(((IGeometryBag)bag).Envelope, FcsMp, pSpatialReference);

                mapHeight = 1/ (mapHeight / geom.Envelope.Height);
                mapWidth = 1 / (mapWidth / geom.Envelope.Width);

                res = mapWidth;
                if (mapHeight < mapWidth) res = mapHeight;
                
            }

            return res;
        }

        public static double Calc_Scale(double mapHeight, double mapWidth, IGeometryCollection bag, IMap FcsMp, ISpatialReference pSpatialReference)
        {
            object Missing = Type.Missing;
            double res = 1000000;

            if (bag != null && !((IGeometryBag)bag).Envelope.IsEmpty)
            {
                
                IGeometry geom = EsriUtils.ToProject(((IGeometryBag)bag).Envelope, FcsMp, pSpatialReference);

                mapHeight = 1 / (mapHeight / geom.Envelope.Height);
                mapWidth = 1 / (mapWidth / geom.Envelope.Width);

                res = mapWidth;
                if (mapHeight < mapWidth) res = mapHeight;

            }

            return res;
        }

        public static void SaveRouteSegmentPoint_ChartSegmentPointGeo(IFeatureClass featureClass, PDM.SegmentPoint SegP, IGeometry geo, bool fafFlag = false)
        {
            int fID = -1;
            IFeature pFeat = featureClass.CreateFeature();
            IPoint pnt = new PointClass();
            pnt.PutCoords(((IPoint)geo).X, ((IPoint)geo).Y);

            fID = pFeat.Fields.FindField("SHAPE"); if (fID > 0) pFeat.set_Value(fID, pnt);
            fID = pFeat.Fields.FindField("FeatureGUID"); if (fID > 0) pFeat.set_Value(fID, SegP.ID);
            //fID = pFeat.Fields.FindField("Route_LEG_ID");  if (fID > 0) pFeat.set_Value(fID, );
            //fID = pFeat.Fields.FindField("PointUse");  if (fID > 0) pFeat.set_Value(fID, SegP.p);
            fID = pFeat.Fields.FindField("radarGuidance");
            if (fID > 0)
            {
                if (SegP.RadarGuidance == true) pFeat.set_Value(fID, 1);
                else pFeat.set_Value(fID, 0);
            }


            fID = pFeat.Fields.FindField("waypoint"); if (fID > 0) pFeat.set_Value(fID, SegP.IsWaypoint);
            fID = pFeat.Fields.FindField("flyOver");
            if (fID > 0)
            {
                if (SegP.FlyOver == true) pFeat.set_Value(fID, 1);
                else pFeat.set_Value(fID, 0);
            }

            fID = pFeat.Fields.FindField("reportingATC");
            if (fID > 0 && !fafFlag) pFeat.set_Value(fID, SegP.ReportingATC.ToString());
            else if (fID > 0 && fafFlag)
            {
                pFeat.set_Value(fID, "FAF");
                fID = pFeat.Fields.FindField("flyOver");
                if (fID > 0)
                {
                    pFeat.set_Value(fID, 1);
                }
            }

            fID = pFeat.Fields.FindField("SegmentPointDesignator"); if (fID > 0) pFeat.set_Value(fID, SegP.SegmentPointDesignator);
            fID = pFeat.Fields.FindField("CoordX"); if (fID > 0) pFeat.set_Value(fID, pnt.X);
            fID = pFeat.Fields.FindField("CoordY"); if (fID > 0) pFeat.set_Value(fID, pnt.Y);

            fID = pFeat.Fields.FindField("PointRole"); if (fID > 0 && SegP.PointRole.HasValue) pFeat.set_Value(fID, SegP.PointRole.Value.ToString());
            fID = pFeat.Fields.FindField("PointChoice"); if (fID > 0) pFeat.set_Value(fID, SegP.PointChoice.ToString());

            pFeat.Store();

            //SigmaChart.SigmaDataCash
            
            Application.DoEvents();

        }

        public static void saveAirportHeliport_ChartGeo(IFeatureClass featureClass, PDM.AirportHeliport ADHP)
        {
            int fID = -1;
            IFeature pFeat = featureClass.CreateFeature();

            try
            {
                fID = pFeat.Fields.FindField("FeatureGUID"); if (fID >= 0) pFeat.set_Value(fID, ADHP.ID);
                fID = pFeat.Fields.FindField("designator"); if (fID >= 0) pFeat.set_Value(fID, ADHP.Designator);
                fID = pFeat.Fields.FindField("designatorIATA"); if (fID >= 0) pFeat.set_Value(fID, ADHP.DesignatorIATA);
                fID = pFeat.Fields.FindField("magneticVariation"); if (fID >= 0 && ADHP.MagneticVariation.HasValue) pFeat.set_Value(fID, ADHP.MagneticVariation);
                fID = pFeat.Fields.FindField("name"); if (fID >= 0) pFeat.set_Value(fID, ADHP.Name);
                fID = pFeat.Fields.FindField("Airport_ReferencePt_Latitude"); if (fID >= 0) pFeat.set_Value(fID, ADHP.Lat);
                fID = pFeat.Fields.FindField("Airport_ReferencePt_Longitude"); if (fID >= 0) pFeat.set_Value(fID, ADHP.Lon);
                fID = pFeat.Fields.FindField("Elev"); if (fID >= 0 && ADHP.Elev.HasValue) pFeat.set_Value(fID, ADHP.Elev);
                fID = pFeat.Fields.FindField("ElevUom"); if (fID >= 0) pFeat.set_Value(fID, ADHP.Elev_UOM.ToString());
                fID = pFeat.Fields.FindField("ActualDate"); if (fID >= 0) pFeat.set_Value(fID, ADHP.ActualDate);
                fID = pFeat.Fields.FindField("AirportHeliportType"); if (fID >= 0) pFeat.set_Value(fID, ADHP.AirportHeliportType.ToString());

                if (ADHP.Geo == null) ADHP.RebuildGeo();

                if (ADHP.Geo != null)
                {
                    fID = pFeat.Fields.FindField("LonCoord"); if (fID >= 0) pFeat.set_Value(fID, ADHP.X_to_EW_DDMMSS());
                    fID = pFeat.Fields.FindField("LatCoord"); if (fID >= 0) pFeat.set_Value(fID, ADHP.Y_to_NS_DDMMSS());
                    fID = pFeat.Fields.FindField("Shape"); pFeat.set_Value(fID, ADHP.Geo);

                }
           
           
            pFeat.Store();
        }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            Application.DoEvents();

        }

        public static void UpdateSegmentPoint_ChartGeo(IFeatureClass dpnGeo_featClass, string FID, string DESIGNATOR, string fieldName, string newValueText)
        {
            try
            {

                int fID = -1;
                IQueryFilter featFilter = new QueryFilterClass();
                if (dpnGeo_featClass == null) return;
                IFeatureCursor featCur = null;

                featFilter.WhereClause = "FeatureGUID = '" + FID + "'";

                featCur = dpnGeo_featClass.Update(featFilter, false);

                IFeature pFeat = featCur.NextFeature();
                if (pFeat == null)
                {
                    featFilter.WhereClause = "SegmentPointDesignator = '" + DESIGNATOR + "'";
                    featCur = dpnGeo_featClass.Update(featFilter, false);
                    pFeat = featCur.NextFeature();
                }

                if (pFeat != null)
                {
                    if (fieldName.ToLower().Contains("lat") || fieldName.ToLower().Contains("lon"))
                    {
                        IGeometry gm = pFeat.Shape;
                        IPoint pnt = (IPoint)gm;

                        AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();


                        if (fieldName.ToLower().Contains("lon")) pnt.X = ArnUtil.GetLONGITUDEFromAIXMString(newValueText);
                        if (fieldName.ToLower().Contains("lat")) pnt.Y = ArnUtil.GetLATITUDEFromAIXMString(newValueText);

                        pFeat.Shape = pnt;

                        ArnUtil = null;
                    }

                    fID = pFeat.Fields.FindField("radarGuidance");
                    if (fID > 0 && fieldName.ToLower().Contains("radarguidance"))
                    {
                        if (newValueText.CompareTo("True") == 0) pFeat.set_Value(fID, 1);
                        else pFeat.set_Value(fID, 0);
                    }
                    fID = pFeat.Fields.FindField("waypoint");
                    if (fID > 0 && fieldName.ToLower().Contains("waypoint"))
                    {
                        if (newValueText.CompareTo("True") == 0) pFeat.set_Value(fID, 1);
                        else pFeat.set_Value(fID, 0);
                    }

                    fID = pFeat.Fields.FindField("flyOver");
                    if (fID > 0 && fieldName.ToLower().Contains("flyover"))
                    {
                        if (newValueText.CompareTo("True") == 0) pFeat.set_Value(fID, 1);
                        else pFeat.set_Value(fID, 0);
                    }

                    fID = pFeat.Fields.FindField("reportingATC");
                    if (fID > 0 && fieldName.ToLower().Contains("reportingatc"))
                    {
                        pFeat.set_Value(fID, newValueText);
                    }


                    fID = pFeat.Fields.FindField("SegmentPointDesignator");
                    if (fID > 0 && fieldName.ToLower().Contains("segmentpointdesignator"))
                    {
                        pFeat.set_Value(fID, newValueText);
                    }


                    fID = pFeat.Fields.FindField("PointRole");
                    if (fID > 0 && fieldName.ToLower().Contains("pointrole"))
                    {
                        pFeat.set_Value(fID, newValueText);
                    }

                    fID = pFeat.Fields.FindField("PointChoice");
                    if (fID > 0 && fieldName.ToLower().Contains("pointchoice"))
                    {
                        pFeat.set_Value(fID, newValueText);
                    }


                    pFeat.Store();
                }

                Marshal.ReleaseComObject(featCur);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
            }
        }

        public static void SaveNavaidSegmentPoint_ChartSegmentPointGeo(IFeatureClass featureClass, PDM.NavaidSystem SegP)
        {
            int fID = -1;
            List<PDM.NavaidComponent> NAVAID_MKR_LST = null;
            IFeature pFeat = featureClass.CreateFeature();

            fID = pFeat.Fields.FindField("FeatureGUID"); if (fID > 0) pFeat.set_Value(fID, SegP.ID);
            fID = pFeat.Fields.FindField("name"); if (fID > 0) pFeat.set_Value(fID, SegP.Name);
            fID = pFeat.Fields.FindField("nav_type"); if (fID > 0) pFeat.set_Value(fID, SegP.CodeNavaidSystemType.ToString());

            fID = pFeat.Fields.FindField("designator"); if (fID > 0) pFeat.set_Value(fID, SegP.Designator.ToString());
            fID = pFeat.Fields.FindField("elevation"); if (fID > 0 && SegP.Elev != null && SegP.Elev.HasValue) pFeat.set_Value(fID, SegP.Elev.ToString());
            fID = pFeat.Fields.FindField("elevetion_uom"); if (fID > 0) pFeat.set_Value(fID, SegP.Elev_UOM.ToString());


            try
            {
                string _lat = "";
                string _lon = "";
                IPoint _shape = null;
                string _frequency = "";
                string _frequency_uom = "";
                double _magvar = 0; ;
                switch (SegP.CodeNavaidSystemType)
                {
                    case NavaidSystemType.VOR:
                    case NavaidSystemType.VOR_DME:
                    case NavaidSystemType.VORTAC:
                  
                        PDM.VOR NAVAID_VOR = (PDM.VOR)(from element in SegP.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.VOR) select element).FirstOrDefault();

                        if (NAVAID_VOR != null)
                        {
                            if (NAVAID_VOR.Geo == null) NAVAID_VOR.RebuildGeo();
                            _shape = new PointClass();
                            _shape.PutCoords(((IPoint)NAVAID_VOR.Geo).X, ((IPoint)NAVAID_VOR.Geo).Y);
                            _lon = NAVAID_VOR.Lon.ToString();
                            _lat = NAVAID_VOR.Lat.ToString();
                            _frequency = NAVAID_VOR.Frequency != null ? NAVAID_VOR.Frequency.ToString() : "";
                            _frequency_uom = NAVAID_VOR.Frequency != null ? NAVAID_VOR.Frequency_UOM.ToString() : "";
                            _magvar = NAVAID_VOR.MagVar.HasValue ? NAVAID_VOR.MagVar.Value : 0;
                        }
                        else if (NAVAID_VOR == null && SegP.CodeNavaidSystemType == NavaidSystemType.VORTAC)
                        {
                            PDM.TACAN NAVAID_TAC = (PDM.TACAN)(from element in SegP.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.TACAN) select element).FirstOrDefault();
                            if (NAVAID_TAC != null)
                            {
                                if (NAVAID_TAC.Geo == null) NAVAID_TAC.RebuildGeo();
                                _shape = new PointClass();
                                _shape.PutCoords(((IPoint)NAVAID_TAC.Geo).X, ((IPoint)NAVAID_TAC.Geo).Y);
                                _lon = NAVAID_TAC.Lon.ToString();
                                _lat = NAVAID_TAC.Lat.ToString();
                                _frequency = NAVAID_TAC.Frequency != null ? NAVAID_TAC.Frequency.ToString() : "";
                                _frequency_uom = NAVAID_TAC.Frequency != null ? NAVAID_TAC.Frequency_UOM.ToString() : "";
                                _magvar = NAVAID_TAC.MagVar.HasValue ? NAVAID_TAC.MagVar.Value : 0;
                            }
                        }
                        break;

                    case NavaidSystemType.DME:
                        PDM.DME NAVAID_DME = (PDM.DME)(from element in SegP.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.DME) select element).FirstOrDefault();
                        if (NAVAID_DME != null)
                        {
                            if (NAVAID_DME.Geo == null) NAVAID_DME.RebuildGeo();
                            _shape = new PointClass();
                            _shape.PutCoords(((IPoint)NAVAID_DME.Geo).X, ((IPoint)NAVAID_DME.Geo).Y);
                            _lon = NAVAID_DME.Lon.ToString();
                            _lat = NAVAID_DME.Lat.ToString();
                        }

                        break;

                    case NavaidSystemType.NDB:
                    case NavaidSystemType.NDB_MKR:
                    case NavaidSystemType.NDB_DME:
                        PDM.NDB NAVAID_NDB = (PDM.NDB)(from element in SegP.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.NDB) select element).FirstOrDefault();
                        if (NAVAID_NDB != null)
                        {
                            if (NAVAID_NDB.Geo == null) NAVAID_NDB.RebuildGeo();
                            _shape = new PointClass();
                            _shape.PutCoords(((IPoint)NAVAID_NDB.Geo).X, ((IPoint)NAVAID_NDB.Geo).Y);
                            _lon = NAVAID_NDB.Lon.ToString();
                            _lat = NAVAID_NDB.Lat.ToString();
                            _frequency = NAVAID_NDB.Frequency != null ? NAVAID_NDB.Frequency.ToString() : "";
                            _frequency_uom = NAVAID_NDB.Frequency != null ? NAVAID_NDB.Frequency_UOM.ToString() : "";
                        }

                        NAVAID_MKR_LST = (from element in SegP.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.Marker) select element).ToList();

                        break;

                    case NavaidSystemType.TACAN:
                        PDM.TACAN NAVAID_TACAN = (PDM.TACAN)(from element in SegP.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.TACAN) select element).FirstOrDefault();
                        if (NAVAID_TACAN != null)
                        {
                            if (NAVAID_TACAN.Geo == null) NAVAID_TACAN.RebuildGeo();
                            _shape = new PointClass();
                            _shape.PutCoords(((IPoint)NAVAID_TACAN.Geo).X, ((IPoint)NAVAID_TACAN.Geo).Y);
                            _lon = NAVAID_TACAN.Lon.ToString();
                            _lat = NAVAID_TACAN.Lat.ToString();

                            _frequency = NAVAID_TACAN.Frequency != null ? NAVAID_TACAN.Frequency.ToString() : "";
                            _frequency_uom = NAVAID_TACAN.Frequency != null ? NAVAID_TACAN.Frequency_UOM.ToString() : "";
                        }
                        break;

                    case NavaidSystemType.ILS_DME:
                    case NavaidSystemType.ILS:
                        PDM.Localizer NAVAID_LOC = (PDM.Localizer)(from element in SegP.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.Localizer) select element).FirstOrDefault();
                        if (NAVAID_LOC != null)
                        {
                            if (NAVAID_LOC.Geo == null) NAVAID_LOC.RebuildGeo();
                            _shape = new PointClass();
                            _shape.PutCoords(((IPoint)NAVAID_LOC.Geo).X, ((IPoint)NAVAID_LOC.Geo).Y);
                            _lon = NAVAID_LOC.Lon.ToString();
                            _lat = NAVAID_LOC.Lat.ToString();

                            _frequency = NAVAID_LOC.Frequency != null ? NAVAID_LOC.Frequency.ToString() : "";
                            _frequency_uom = NAVAID_LOC.Frequency != null ? NAVAID_LOC.Frequency_UOM.ToString() : "";


                            fID = pFeat.Fields.FindField("nav_type"); if (fID > 0) pFeat.set_Value(fID, NavaidSystemType.LOC.ToString());
                        }

                        NAVAID_MKR_LST = (from element in SegP.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.Marker) select element).ToList();


                        break;
                    

                }


                fID = pFeat.Fields.FindField("SHAPE"); if (fID > 0) pFeat.set_Value(fID, _shape);

                fID = pFeat.Fields.FindField("location_x"); if (fID > 0) pFeat.set_Value(fID, _shape.X);
                fID = pFeat.Fields.FindField("location_y"); if (fID > 0) pFeat.set_Value(fID, _shape.Y);
                fID = pFeat.Fields.FindField("frequency"); if (fID > 0 && _frequency.Length >0 && _frequency.ToUpper().CompareTo("NAN")!=0) pFeat.set_Value(fID, _frequency);
                fID = pFeat.Fields.FindField("frequency_uom"); if (fID > 0 && _frequency_uom.Length > 0) pFeat.set_Value(fID, _frequency_uom);
                fID = pFeat.Fields.FindField("navaidMagVar"); if (fID > 0 ) pFeat.set_Value(fID, _magvar);
            }
            catch { }

            pFeat.Store();



            if (NAVAID_MKR_LST !=null)
            {

                foreach (PDM.Marker _mkr in NAVAID_MKR_LST)
                {

                    pFeat = featureClass.CreateFeature();

                    fID = pFeat.Fields.FindField("FeatureGUID"); if (fID > 0) pFeat.set_Value(fID, SegP.ID);
                    fID = pFeat.Fields.FindField("name"); if (fID > 0) pFeat.set_Value(fID, SegP.Name);
                    fID = pFeat.Fields.FindField("nav_type"); if (fID > 0) pFeat.set_Value(fID, SegP.CodeNavaidSystemType.ToString());

                    fID = pFeat.Fields.FindField("designator"); if (fID > 0) pFeat.set_Value(fID, SegP.Designator.ToString());
                    fID = pFeat.Fields.FindField("elevation"); if (fID > 0 && SegP.Elev != null && SegP.Elev.HasValue) pFeat.set_Value(fID, SegP.Elev.ToString());
                    fID = pFeat.Fields.FindField("elevetion_uom"); if (fID > 0) pFeat.set_Value(fID, SegP.Elev_UOM.ToString());

                    string _lat = "";
                    string _lon = "";
                    IPoint _shape = null;
                    string _frequency = "";
                    string _frequency_uom = "";

                    if (_mkr.Geo == null) _mkr.RebuildGeo();
                    _shape = new PointClass();
                    _shape.PutCoords(((IPoint)_mkr.Geo).X, ((IPoint)_mkr.Geo).Y);
                    _lon = _mkr.Lon.ToString();
                    _lat = _mkr.Lat.ToString();

                    _frequency = _mkr.Frequency != null ? _mkr.Frequency.ToString() : "";
                    _frequency_uom = _mkr.Frequency != null ? _mkr.Frequency_UOM.ToString() : "";

                    fID = pFeat.Fields.FindField("SHAPE"); if (fID > 0) pFeat.set_Value(fID, _shape);
                    fID = pFeat.Fields.FindField("nav_type"); if (fID > 0) pFeat.set_Value(fID, NavaidSystemType.MKR.ToString());
                    fID = pFeat.Fields.FindField("location_x"); if (fID > 0) pFeat.set_Value(fID, _shape.X);
                    fID = pFeat.Fields.FindField("location_y"); if (fID > 0) pFeat.set_Value(fID, _shape.Y);
                    fID = pFeat.Fields.FindField("frequency"); if (fID > 0 && _frequency!= null && _frequency.Length > 0 && _frequency.ToUpper().CompareTo("NAN") != 0) pFeat.set_Value(fID, _frequency);
                    fID = pFeat.Fields.FindField("frequency_uom"); if (fID > 0 && _frequency_uom.Length > 0) pFeat.set_Value(fID, _frequency_uom);
                    fID = pFeat.Fields.FindField("navaidMagVar"); if (fID > 0 && _mkr.Axis_Bearing!=null && _mkr.Axis_Bearing.HasValue) pFeat.set_Value(fID, _mkr.Axis_Bearing.Value);

                    pFeat.Store();
                }
                NAVAID_MKR_LST = null;
            }

            Application.DoEvents();

        }

        public static void UpdateRouteSegmant_ChartGeo(IFeatureClass featureClass, DetailedItem item)
        {
            if (item.Feature.Geo == null) item.Feature.RebuildGeo();

            if (item.Feature.Geo == null) return;

            IQueryFilter featFilter = new QueryFilterClass();

            featFilter.WhereClause = "FeatureGUID = '" + item.Feature.ID + "'";

            IFeatureCursor featCur = featureClass.Update(featFilter, false);

            IFeature feat = featCur.NextFeature();


            if (feat != null)
            {
                if (item.Feature.Geo == null) item.Feature.RebuildGeo();

                if (item.Feature.Geo != null)
                {
                    
                    IGeometry geo = item.Feature.Geo;

                    if (((ESRI.ArcGIS.Geometry.IZAware)geo).ZAware)
                    {
                        ESRI.ArcGIS.Geometry.IZ2 z = (ESRI.ArcGIS.Geometry.IZ2)geo;
                        z.SetNonSimpleZs(0.0);
                        IZAware zAware = (ESRI.ArcGIS.Geometry.IZAware)geo;
                        zAware.ZAware = false;
                    }
                    int fID = feat.Fields.FindField("SHAPE"); if (fID > 0) feat.set_Value(fID, geo);

                    feat.Store();
                }

            }

            Marshal.ReleaseComObject(featCur);

            Application.DoEvents();

        }
    

        public static void SaveNavaidSegmentPoint_ChartSegmentPointGeo(IFeatureClass featureClass, PDM.NavaidComponent navComp)
        {
            int fID = -1;
            IFeature pFeat = featureClass.CreateFeature();

            fID = pFeat.Fields.FindField("FeatureGUID"); if (fID > 0) pFeat.set_Value(fID, navComp.ID);
            fID = pFeat.Fields.FindField("name"); if (fID > 0) pFeat.set_Value(fID, navComp.Designator);
            fID = pFeat.Fields.FindField("nav_type");
            if (fID > 0)
            {
                if (navComp.PDM_Type == PDM_ENUM.Localizer)
                    pFeat.set_Value(fID, "LOC");
                else
                    pFeat.set_Value(fID, navComp.PDM_Type.ToString());
            }

            fID = pFeat.Fields.FindField("designator"); if (fID > 0) pFeat.set_Value(fID, navComp.Designator.ToString());
            fID = pFeat.Fields.FindField("elevation"); if (fID > 0 && navComp.Elev != null && navComp.Elev.HasValue) pFeat.set_Value(fID, navComp.Elev.ToString());
            fID = pFeat.Fields.FindField("elevetion_uom"); if (fID > 0) pFeat.set_Value(fID, navComp.Elev_UOM.ToString());


            try
            {
                string _lat = "";
                string _lon = "";
                IPoint _shape = null;
                string _frequency = "";
                string _frequency_uom = "";
                double _magvar = 0;

                if (navComp.Geo == null) navComp.RebuildGeo();
                _shape = new PointClass();
                _shape.PutCoords(((IPoint)navComp.Geo).X, ((IPoint)navComp.Geo).Y);
                _lon = navComp.Lon.ToString();
                _lat = navComp.Lat.ToString();
                _frequency = navComp.Frequency != null ? navComp.Frequency.ToString() : "";
                _frequency_uom = navComp.Frequency != null ? navComp.Frequency_UOM.ToString() : "";
                _magvar = navComp.MagVar.HasValue ? navComp.MagVar.Value : 0;

                fID = pFeat.Fields.FindField("SHAPE"); if (fID > 0) pFeat.set_Value(fID, _shape);

                fID = pFeat.Fields.FindField("location_x"); if (fID > 0) pFeat.set_Value(fID, _shape.X);
                fID = pFeat.Fields.FindField("location_y"); if (fID > 0) pFeat.set_Value(fID, _shape.Y);
                fID = pFeat.Fields.FindField("frequency"); if (fID > 0 && _frequency.Length > 0) pFeat.set_Value(fID, _frequency);
                fID = pFeat.Fields.FindField("frequency_uom"); if (fID > 0 && _frequency_uom.Length > 0) pFeat.set_Value(fID, _frequency_uom);
                fID = pFeat.Fields.FindField("navaidMagVar"); if (fID > 0) pFeat.set_Value(fID, _magvar);
            }
            catch { }

            pFeat.Store();
            Application.DoEvents();

        }


        public static IElement CreateSegmentPointAnno(PDM.ProcedureLeg Leg, AbstractChartElement chartEl, bool Start, string distUom, bool Rnav)
        {

            ChartElement_SigmaCollout_Designatedpoint chrtEl_Sign = (ChartElement_SigmaCollout_Designatedpoint)chartEl;
            SegmentPoint routeSegmentPoint = Start ? Leg.StartPoint : Leg.EndPoint;

            IElement el_SegPnt = null;
            IPoint pnt = new PointClass();
            if (routeSegmentPoint.Geo == null) routeSegmentPoint.RebuildGeo();
            pnt.PutCoords(((IPoint)routeSegmentPoint.Geo).X - chrtEl_Sign.Anchor.X, ((IPoint)routeSegmentPoint.Geo).Y - chrtEl_Sign.Anchor.Y);


            chrtEl_Sign.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(routeSegmentPoint, chrtEl_Sign.TextContents[0][0].DataSource, chrtEl_Sign.CoordType);
            chrtEl_Sign.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(routeSegmentPoint, chrtEl_Sign.TextContents[1][0].DataSource, chrtEl_Sign.CoordType);
            chrtEl_Sign.CaptionTextLine[0][0].TextValue = ChartsHelperClass.MakeText(routeSegmentPoint, chrtEl_Sign.CaptionTextLine[0][0].DataSource);

            chrtEl_Sign.TextContents[2][0].Visible = false;
            chrtEl_Sign.TextContents[2][1].Visible = false;
            chrtEl_Sign.TextContents[3][0].Visible = false;
            chrtEl_Sign.TextContents[3][1].Visible = false;


            chrtEl_Sign.CaptionTextLine[0][0].Visible = !chrtEl_Sign.CaptionTextLine[0][0].TextValue.Contains("InnerText");

            if (chrtEl_Sign.CaptionTextLine.Count > 1)
            {
                chrtEl_Sign.CaptionTextLine[1][0].TextValue = ChartsHelperClass.MakeText(routeSegmentPoint, chrtEl_Sign.CaptionTextLine[1][0].DataSource);
                chrtEl_Sign.CaptionTextLine[1][0].Visible = !chrtEl_Sign.CaptionTextLine[1][0].TextValue.Contains("InnerText");
            }

            chrtEl_Sign.Anchor = new AncorPoint(((IPoint)routeSegmentPoint.Geo).X, ((IPoint)routeSegmentPoint.Geo).Y);


            if (routeSegmentPoint.PointFacilityMakeUp!=null && !Rnav && (routeSegmentPoint.PointFacilityMakeUp.DistanceIndication != null || routeSegmentPoint.PointFacilityMakeUp.AngleIndication != null))
            {

                string Nav = routeSegmentPoint.PointFacilityMakeUp.DistanceIndication!= null? routeSegmentPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointID : routeSegmentPoint.PointFacilityMakeUp.AngleIndication.SignificantPointID;

                var nav = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is NavaidSystem) && 
                           (((NavaidSystem)element).ID.CompareTo(Nav) == 0 || ((NavaidSystem)element).ID_Feature.CompareTo(Nav) == 0) select element).FirstOrDefault();
                if (nav == null)
                    nav = DataCash.GetNavaidComponentByID(Nav);

                if (nav != null)
                {

                    if (((NavaidSystem)nav).Components != null && ((NavaidSystem)nav).Components.Count > 0)
                    {
                        foreach (var navComp in ((NavaidSystem)nav).Components)
                        {

                            var obj = ArenaStaticProc.GetObjectValue(navComp, "Frequency", false);
                            if (obj != null)
                            {
                                chrtEl_Sign.TextContents[2][0].DataSource.Value = "Designator";
                                chrtEl_Sign.TextContents[2][1].DataSource.Value = obj.ToString();
                                chrtEl_Sign.TextContents[2][0].TextValue = navComp.Designator;
                                chrtEl_Sign.TextContents[2][1].TextValue = obj.ToString();

                                chrtEl_Sign.TextContents[2][0].Visible = !chrtEl_Sign.TextContents[2][0].Visible;
                                chrtEl_Sign.TextContents[2][1].Visible = !chrtEl_Sign.TextContents[2][1].Visible;

                                
                                chrtEl_Sign.TextContents[3][0].TextValue = routeSegmentPoint.PointFacilityMakeUp.AngleIndication!=null? ChartsHelperClass.MakeText(routeSegmentPoint.PointFacilityMakeUp.AngleIndication, chrtEl_Sign.TextContents[3][0].DataSource, Rounder:1, minLength: 5): "";
                                chrtEl_Sign.TextContents[3][1].TextValue = routeSegmentPoint.PointFacilityMakeUp.DistanceIndication!=null ? ChartsHelperClass.MakeText_UOM(routeSegmentPoint.PointFacilityMakeUp.DistanceIndication, chrtEl_Sign.TextContents[3][1].DataSource,distUom,1) : "";

                                chrtEl_Sign.TextContents[3][0].Visible = routeSegmentPoint.PointFacilityMakeUp.AngleIndication != null;//true;
                                chrtEl_Sign.TextContents[3][1].Visible = routeSegmentPoint.PointFacilityMakeUp.DistanceIndication != null;//true;


                                //chrtEl_Sign.TextContents[3][0].DataSource.Value = "AngleIndication";
                                //chrtEl_Sign.TextContents[3][1].DataSource.Value = "DistanceIndication";
                            }
                        }
                    }
                }

            }
            else
            {
                chrtEl_Sign.TextContents[2][0].Visible = false;
                chrtEl_Sign.TextContents[2][1].Visible = false;
                chrtEl_Sign.TextContents[3][0].Visible = false;
                chrtEl_Sign.TextContents[3][1].Visible = false;

            }



            el_SegPnt = (IElement)chrtEl_Sign.ConvertToIElement();

            if (el_SegPnt is IGroupElement)
            {
                IGroupElement GrEl = el_SegPnt as IGroupElement;
                for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                {
                    GrEl.get_Element(i).Geometry = pnt as IGeometry;
                }
            }
            else el_SegPnt.Geometry = pnt;

            return el_SegPnt;

        }

        public static IElement CreateSegmentPointAnno_STAR(PDM.ProcedureLeg Leg, AbstractChartElement chartEl, bool Start, string distUom, bool Rnav)
        {

            ChartElement_SigmaCollout_Designatedpoint chrtEl_Sign = (ChartElement_SigmaCollout_Designatedpoint)chartEl;
            SegmentPoint routeSegmentPoint = Start ? Leg.StartPoint : Leg.EndPoint;

            IElement el_SegPnt = null;
            IPoint pnt = new PointClass();
            if (routeSegmentPoint.Geo == null) routeSegmentPoint.RebuildGeo();
            pnt.PutCoords(((IPoint)routeSegmentPoint.Geo).X - chrtEl_Sign.Anchor.X, ((IPoint)routeSegmentPoint.Geo).Y - chrtEl_Sign.Anchor.Y);


            chrtEl_Sign.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(routeSegmentPoint, chrtEl_Sign.TextContents[0][0].DataSource, chrtEl_Sign.CoordType);
            chrtEl_Sign.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(routeSegmentPoint, chrtEl_Sign.TextContents[1][0].DataSource, chrtEl_Sign.CoordType);
            chrtEl_Sign.CaptionTextLine[0][0].TextValue = ChartsHelperClass.MakeText(routeSegmentPoint, chrtEl_Sign.CaptionTextLine[0][0].DataSource);
            chrtEl_Sign.CaptionTextLine[1][0].TextValue = ChartsHelperClass.MakeText(routeSegmentPoint, chrtEl_Sign.CaptionTextLine[1][0].DataSource);

            chrtEl_Sign.CaptionTextLine[0][0].Visible = routeSegmentPoint.PointRole.HasValue && routeSegmentPoint.PointRole == ProcedureFixRoleType.IAF;
            if (chrtEl_Sign.CaptionTextLine[0][0].Visible)
            {
                chrtEl_Sign.Frame.FrameMargins.TopMargin = -7;
                chrtEl_Sign.Frame.FrameMargins.HeaderHorizontalMargin = 3;
            }
            else
                chrtEl_Sign.CaptionTextLine.Remove(chrtEl_Sign.CaptionTextLine[0]);

            chrtEl_Sign.Anchor = new AncorPoint(((IPoint)routeSegmentPoint.Geo).X, ((IPoint)routeSegmentPoint.Geo).Y);


                chrtEl_Sign.TextContents[2][0].Visible = false;
                chrtEl_Sign.TextContents[2][1].Visible = false;
                chrtEl_Sign.TextContents[3][0].Visible = false;
                chrtEl_Sign.TextContents[3][1].Visible = false;


            if (routeSegmentPoint.PointFacilityMakeUp != null && !Rnav && (routeSegmentPoint.PointFacilityMakeUp.DistanceIndication != null || routeSegmentPoint.PointFacilityMakeUp.AngleIndication != null))
            {

                string Nav = routeSegmentPoint.PointFacilityMakeUp.DistanceIndication != null ? routeSegmentPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointID : routeSegmentPoint.PointFacilityMakeUp.AngleIndication.SignificantPointID;

                var nav = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is NavaidSystem) && 
                           (((NavaidSystem)element).ID.CompareTo(Nav) == 0 || ((NavaidSystem)element).ID_Feature.CompareTo(Nav) == 0) select element).FirstOrDefault();
                if (nav == null)
                    nav = DataCash.GetNavaidComponentByID(Nav);

                if (nav != null)
                {

                    if (((NavaidSystem)nav).Components != null && ((NavaidSystem)nav).Components.Count > 0)
                    {
                        foreach (var navComp in ((NavaidSystem)nav).Components)
                        {

                            var obj = ArenaStaticProc.GetObjectValue(navComp, "Frequency", false);
                            if (obj != null)
                            {
                                chrtEl_Sign.TextContents[2][0].DataSource.Value = "Designator";
                                chrtEl_Sign.TextContents[2][1].DataSource.Value = obj.ToString();
                                chrtEl_Sign.TextContents[2][0].TextValue = navComp.Designator;
                                chrtEl_Sign.TextContents[2][1].TextValue = obj.ToString();

                                chrtEl_Sign.TextContents[2][0].Visible = !chrtEl_Sign.TextContents[2][0].Visible;
                                chrtEl_Sign.TextContents[2][1].Visible = !chrtEl_Sign.TextContents[2][1].Visible;


                                chrtEl_Sign.TextContents[3][0].TextValue = routeSegmentPoint.PointFacilityMakeUp.AngleIndication != null ? ChartsHelperClass.MakeText(routeSegmentPoint.PointFacilityMakeUp.AngleIndication, chrtEl_Sign.TextContents[3][0].DataSource, Rounder: 1, minLength: 5) : "";
                                chrtEl_Sign.TextContents[3][1].TextValue = routeSegmentPoint.PointFacilityMakeUp.DistanceIndication != null ? ChartsHelperClass.MakeText_UOM(routeSegmentPoint.PointFacilityMakeUp.DistanceIndication, chrtEl_Sign.TextContents[3][1].DataSource, distUom, 1) : "";

                                chrtEl_Sign.TextContents[3][0].Visible = routeSegmentPoint.PointFacilityMakeUp.AngleIndication != null;//true;
                                chrtEl_Sign.TextContents[3][1].Visible = routeSegmentPoint.PointFacilityMakeUp.DistanceIndication != null;//true;


                                //chrtEl_Sign.TextContents[3][0].DataSource.Value = "AngleIndication";
                                //chrtEl_Sign.TextContents[3][1].DataSource.Value = "DistanceIndication";
                            }
                        }
                    }
                }

            }



            el_SegPnt = (IElement)chrtEl_Sign.ConvertToIElement();

            if (el_SegPnt is IGroupElement)
            {
                IGroupElement GrEl = el_SegPnt as IGroupElement;
                for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                {
                    GrEl.get_Element(i).Geometry = pnt as IGeometry;
                }
            }
            else el_SegPnt.Geometry = pnt;

            return el_SegPnt;

        }

        public static IElement CreateSegmentPointAnno(PDM.SegmentPoint routeSegmentPoint, AbstractChartElement chartEl)
        {

            ChartElement_SigmaCollout_Designatedpoint chrtEl_Sign = (ChartElement_SigmaCollout_Designatedpoint)chartEl;

            IElement el_SegPnt = null;
            IPoint pnt = new PointClass();
            if (routeSegmentPoint.Geo == null) routeSegmentPoint.RebuildGeo();
            pnt.PutCoords(((IPoint)routeSegmentPoint.Geo).X - chrtEl_Sign.Anchor.X, ((IPoint)routeSegmentPoint.Geo).Y - chrtEl_Sign.Anchor.Y);


            chrtEl_Sign.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(routeSegmentPoint, chrtEl_Sign.TextContents[0][0].DataSource, chrtEl_Sign.CoordType);
            chrtEl_Sign.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(routeSegmentPoint, chrtEl_Sign.TextContents[1][0].DataSource, chrtEl_Sign.CoordType);
            chrtEl_Sign.CaptionTextLine[0][0].TextValue = ChartsHelperClass.MakeText(routeSegmentPoint, chrtEl_Sign.CaptionTextLine[0][0].DataSource);

            chrtEl_Sign.CaptionTextLine[0][0].Visible = !chrtEl_Sign.CaptionTextLine[0][0].TextValue.StartsWith("InnerText");
            if (chrtEl_Sign.CaptionTextLine.Count > 1) chrtEl_Sign.CaptionTextLine[1][0].Visible = !chrtEl_Sign.CaptionTextLine[1][0].TextValue.StartsWith("InnerText");

            if (chrtEl_Sign.TextContents.Count > 2) chrtEl_Sign.TextContents[2][0].Visible = false;
            if (chrtEl_Sign.TextContents.Count > 2) chrtEl_Sign.TextContents[2][1].Visible = false;
            if (chrtEl_Sign.TextContents.Count > 3) chrtEl_Sign.TextContents[3][0].Visible = false;
            if (chrtEl_Sign.TextContents.Count > 3) chrtEl_Sign.TextContents[3][1].Visible = false;


            chrtEl_Sign.Anchor = new AncorPoint(((IPoint)routeSegmentPoint.Geo).X, ((IPoint)routeSegmentPoint.Geo).Y);

            el_SegPnt = (IElement)chrtEl_Sign.ConvertToIElement();

            if (el_SegPnt is IGroupElement)
            {
                IGroupElement GrEl = el_SegPnt as IGroupElement;
                for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                {
                    GrEl.get_Element(i).Geometry = pnt as IGeometry;
                }
            }
            else el_SegPnt.Geometry = pnt;

            return el_SegPnt;

        }

        public static IElement CreateWayPointAnno(PDM.SegmentPoint routeSegmentPoint, AbstractChartElement chartEl)
        {
            ChartElement_TextArrow chrtEl_Sign = (ChartElement_TextArrow)chartEl;

            IElement el_SegPnt = null;
            IPoint pnt = new PointClass();
            if (routeSegmentPoint.Geo == null) routeSegmentPoint.RebuildGeo();
            pnt.PutCoords(((IPoint)routeSegmentPoint.Geo).X - chrtEl_Sign.Anchor.X, ((IPoint)routeSegmentPoint.Geo).Y - chrtEl_Sign.Anchor.Y);

            chrtEl_Sign.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(routeSegmentPoint, chrtEl_Sign.TextContents[0][0].DataSource);
            chrtEl_Sign.Anchor = new AncorPoint(((IPoint)routeSegmentPoint.Geo).X, ((IPoint)routeSegmentPoint.Geo).Y);

            el_SegPnt = (IElement)chrtEl_Sign.ConvertToIElement();

            if (el_SegPnt is IGroupElement)
            {
                IGroupElement GrEl = el_SegPnt as IGroupElement;
                for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                {
                    GrEl.get_Element(i).Geometry = pnt as IGeometry;
                }
            }
            else el_SegPnt.Geometry = pnt;

            return el_SegPnt;

        }

        public static IElement CreateSegmentPointAnno(PDM.NavaidSystem navaidSegmentPoint, AbstractChartElement chartEl, string vertUom, bool findDME = false)
        {
            IElement el_SegPnt = null;
            PDM.PDMObject dme_tac = null;

            ChartElement_SigmaCollout_Navaid chrtEl_Sign = (ChartElement_SigmaCollout_Navaid)chartEl;
            PDM.NavaidComponent NAVAID = (PDM.VOR)(from element in navaidSegmentPoint.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.VOR) select element).FirstOrDefault();

            if (navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.NDB || navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.NDB_MKR)
                NAVAID = (PDM.NDB)(from element in navaidSegmentPoint.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.NDB) select element).FirstOrDefault();
            if (navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.ILS_DME)
            {
                NAVAID = (PDM.Localizer)(from element in navaidSegmentPoint.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.Localizer) select element).FirstOrDefault();
                if (findDME)
                    NAVAID = (PDM.DME)(from element in navaidSegmentPoint.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.DME) select element).FirstOrDefault();
            }

            if (navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.VOR_DME)
                dme_tac = (PDM.DME)(from element in navaidSegmentPoint.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.DME) select element).FirstOrDefault();
            if (navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.VORTAC)
                dme_tac = (PDM.TACAN)(from element in navaidSegmentPoint.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.TACAN) select element).FirstOrDefault();

            if (dme_tac != null) 
            {
                NAVAID = (PDM.NavaidComponent)dme_tac;
            }

            if (NAVAID == null) return null;

            if (NAVAID.Geo == null) NAVAID.RebuildGeo();
            IPoint pnt = new PointClass();
            pnt.PutCoords(((IPoint)NAVAID.Geo).X - chrtEl_Sign.Anchor.X, ((IPoint)NAVAID.Geo).Y - chrtEl_Sign.Anchor.Y);


            #region InnerText

            chrtEl_Sign.TextContents[0][0].TextValue = navaidSegmentPoint.CodeNavaidSystemType.ToString().Replace("_", "/");

            if (navaidSegmentPoint.Components != null && navaidSegmentPoint.Components.Count > 0)
            {
                foreach (var item in navaidSegmentPoint.Components)
                {
                    if (item.PDM_Type == PDM_ENUM.VOR)
                    {
                        if (((VOR)item).VorType.HasValue && ((VOR)item).VorType.Value == CodeVOR.DVOR)
                        {
                            chrtEl_Sign.TextContents[0][0].TextValue = chrtEl_Sign.TextContents[0][0].TextValue.Replace("VOR", "DVOR") +" ";
                           
                        }

                        chrtEl_Sign.TextContents[0][1].TextValue = ((VOR)item).Frequency.HasValue ? ((VOR)item).Frequency.Value.ToString() : "NAN";
                        break;
                    }
                }
            }

           



            chrtEl_Sign.TextContents[1][0].TextValue = NAVAID.Designator != null ? NAVAID.Designator : navaidSegmentPoint.Designator;
            chrtEl_Sign.MorseTextLine.MorseText = NAVAID.Designator != null ? NAVAID.Designator : navaidSegmentPoint.Designator;

            if (NAVAID is Localizer) chrtEl_Sign.MorseTextLine = null;
            if (NAVAID is DME && findDME) chrtEl_Sign.MorseTextLine = null;

            if (!(NAVAID is Localizer) &&  !findDME)
            {
                chrtEl_Sign.TextContents[3][0].TextValue = ArenaStaticProc.LatToDDMMSS(((IPoint)NAVAID.Geo).Y.ToString(), chrtEl_Sign.CoordType);
                chrtEl_Sign.TextContents[4][0].TextValue = ArenaStaticProc.LonToDDMMSS(((IPoint)NAVAID.Geo).X.ToString(), chrtEl_Sign.CoordType);
            }
            else
            {
                chrtEl_Sign.TextContents.Remove(chrtEl_Sign.TextContents[chrtEl_Sign.TextContents.Count - 1]);
                chrtEl_Sign.TextContents.Remove(chrtEl_Sign.TextContents[chrtEl_Sign.TextContents.Count - 1]);
            }

            chrtEl_Sign.TextContents[2][0].TextValue = "";

            if (navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.VOR_DME && !findDME)
            {

                var dme = (from element in navaidSegmentPoint.Components where (element != null) && (element.PDM_Type == PDM_ENUM.DME) select element).FirstOrDefault();
                if (dme != null)
                {
                    chrtEl_Sign.TextContents[2][0].TextValue = ((DME)dme).Channel != null ? ((DME)dme).Channel : "";
                }

            }

            else if (navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.VORTAC)
            {


                var tac = (from element in navaidSegmentPoint.Components where (element != null) && (element.PDM_Type == PDM_ENUM.TACAN) select element).FirstOrDefault();
                if (tac != null)
                {
                    chrtEl_Sign.TextContents[2][0].TextValue = ((TACAN)tac).Channel != null ? ((TACAN)tac).Channel : "";
                }

            }

            else if (navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.NDB || navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.NDB_MKR)
            {
                chrtEl_Sign.TextContents[0][1].Visible = false;
                chrtEl_Sign.TextContents[2][0].StartSymbol.Text = "";
                chrtEl_Sign.TextContents[2][0].TextValue = "";
                //chrtEl_Sign.TextContents[2][1].TextValue = "";

            }
            else
            {
                chrtEl_Sign.TextContents.Remove(chrtEl_Sign.TextContents[2]);
            }

            if (chrtEl_Sign.TextContents.Count>2 && chrtEl_Sign.TextContents[2][0].TextValue.Trim().Length == 0) chrtEl_Sign.TextContents.Remove(chrtEl_Sign.TextContents[2]);

            chrtEl_Sign.Anchor = new AncorPoint(((IPoint)NAVAID.Geo).X, ((IPoint)NAVAID.Geo).Y);


            #endregion

            #region Caption

            if (!(NAVAID is Localizer) && !findDME)
            {
                chrtEl_Sign.CaptionTextLine[0][0].TextValue = navaidSegmentPoint.Name.ToString().Trim();
            }
            else if (NAVAID is Localizer)
            {
                chrtEl_Sign.CaptionTextLine[0][0].TextValue = "LOC";
                chrtEl_Sign.TextContents[0][0].TextValue = navaidSegmentPoint.Designator;
                chrtEl_Sign.TextContents[0][1].TextValue = NAVAID.Frequency.HasValue ? NAVAID.Frequency.Value.ToString() : "NAN";
            }
            else if (NAVAID is DME && findDME)
            {
                chrtEl_Sign.CaptionTextLine[0][0].TextValue = "DME";
                chrtEl_Sign.TextContents[0][0].TextValue = navaidSegmentPoint.Designator;
                chrtEl_Sign.TextContents[0][1].TextValue = ((DME)NAVAID).Channel.Length > 0 ? ((DME)NAVAID).Channel : "NAN";
            }


            if (chrtEl_Sign.CaptionTextLine.Count == 2)
            {
                chrtEl_Sign.CaptionTextLine.Remove(chrtEl_Sign.CaptionTextLine[1]);
            }
            //else
            //{
            //    chrtEl_Sign.CaptionTextLine[0][0].TextValue = "";
            //}

            #endregion

            #region Bottom

            if (!(NAVAID is Localizer) && !findDME)
            {
                if (dme_tac != null)
                {
                    int el = 0;
                    string ElevStr = "";
                    string ElevStrUOM = "";

                    if (dme_tac.Elev != null && dme_tac.Elev.HasValue && !dme_tac.Elev.ToString().StartsWith("NaN")) el = (int)Math.Round((double)dme_tac.Elev, 0);

                    //if (el != 0)
                    {

                        // convert el value to FT
                       // if (dme_tac.Elev_UOM != UOM_DIST_VERT.FT)
                        {
                            //var elevDme = dme_tac.ConvertValueToFeet((double)el, dme_tac.Elev_UOM.ToString());
                            string uom = dme_tac.Elev_UOM.ToString();
                            double elevDme = ArenaStaticProc.UomTransformation(uom, vertUom, (double)el);
                            var ff = Math.DivRem((int)elevDme, 100, out el);
                            el = 100 * (ff + 1);
                        }

                        ElevStr = el.ToString() +" ";
                        ElevStrUOM = vertUom.ToString(); //dme_tac.Elev_UOM.ToString();
                    }

                    chrtEl_Sign.BottomTextLine[0][0].TextValue = ElevStr;
                    chrtEl_Sign.BottomTextLine[0][1].TextValue = ElevStrUOM;
                }
                else
                {
                    chrtEl_Sign.BottomTextLine[0][0].TextValue = "";
                    chrtEl_Sign.BottomTextLine[0][1].TextValue = "";
                    chrtEl_Sign.BottomTextLine.Remove(chrtEl_Sign.BottomTextLine[0]);
                    chrtEl_Sign.HasFooter = false;

                }

                
            }
            else
            {
                chrtEl_Sign.BottomTextLine.Remove(chrtEl_Sign.BottomTextLine[0]);
                chrtEl_Sign.Frame.FrameMargins.BottomMargin = 5;
                chrtEl_Sign.Frame.FrameMargins.TopMargin = 0;
                chrtEl_Sign.Frame.FrameMargins.FooterHorizontalMargin = 0;
                chrtEl_Sign.Frame.FrameMargins.HeaderHorizontalMargin = 0;
                chrtEl_Sign.TextContents.RemoveAt(chrtEl_Sign.TextContents.Count - 1);
                chrtEl_Sign.HasFooter = false;

            }

            #endregion


            el_SegPnt = (IElement)chrtEl_Sign.ConvertToIElement();

            if (el_SegPnt is IGroupElement)
            {
                IGroupElement GrEl = el_SegPnt as IGroupElement;
                for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                {
                    GrEl.get_Element(i).Geometry = pnt as IGeometry;
                }
            }
            else el_SegPnt.Geometry = pnt;

            return el_SegPnt;

        }

        public static void SaveHoldingPath(IFeatureClass HoldingPathFeatureClass, HoldingPattern Holding)
        {
            if (HoldingPathFeatureClass == null) return;
            if (Holding.Geo == null) Holding.RebuildGeo2();
            if (Holding.Geo == null) return;

            IPointCollection hldG = (IPointCollection)Holding.Geo;

            Polyline PolyLn1 = new PolylineClass();
            (PolyLn1 as IPointCollection).AddPoint(hldG.Point[hldG.PointCount - 2]);
            for (int i = 0; i <= hldG.PointCount / 2 - 1; i++)
                (PolyLn1 as IPointCollection).AddPoint(hldG.Point[i]);


            IFeature pFeat = HoldingPathFeatureClass.CreateFeature();


            int fID = pFeat.Fields.FindField("SHAPE"); if (fID > 0) pFeat.set_Value(fID, PolyLn1);
            fID = pFeat.Fields.FindField("FeatureGUID"); if (fID > 0) pFeat.set_Value(fID, Holding.ID);
            fID = pFeat.Fields.FindField("SegmentPointID"); if (fID > 0) pFeat.set_Value(fID, Holding.HoldingPoint.SegmentPointDesignator);
            fID = pFeat.Fields.FindField("type"); if (fID > 0) pFeat.set_Value(fID, Holding.Type.ToString());
            fID = pFeat.Fields.FindField("outboundCourse"); if (fID > 0) pFeat.set_Value(fID, Holding.OutboundCourse);
            fID = pFeat.Fields.FindField("outboundCourseType"); if (fID > 0) pFeat.set_Value(fID, Holding.OutboundCourseType.ToString());
            fID = pFeat.Fields.FindField("inboundCourse"); if (fID > 0) pFeat.set_Value(fID, Holding.InboundCourse);
            fID = pFeat.Fields.FindField("turnDirection"); if (fID > 0) pFeat.set_Value(fID, Holding.TurnDirection.ToString());
            fID = pFeat.Fields.FindField("upperLimit"); if (fID > 0) pFeat.set_Value(fID, Holding.UpperLimit);
            fID = pFeat.Fields.FindField("upperLimitUOM"); if (fID > 0) pFeat.set_Value(fID, Holding.UpperLimitUOM.ToString());
            fID = pFeat.Fields.FindField("upperLimitReference"); if (fID > 0) pFeat.set_Value(fID, Holding.UpperLimitReference.ToString());
            fID = pFeat.Fields.FindField("lowerLimit"); if (fID > 0) pFeat.set_Value(fID, Holding.LowerLimit);
            fID = pFeat.Fields.FindField("lowerLimitUOM"); if (fID > 0) pFeat.set_Value(fID, Holding.LowerLimitUOM.ToString());
            fID = pFeat.Fields.FindField("lowerLimitReference"); if (fID > 0) pFeat.set_Value(fID, Holding.LowerLimitReference.ToString());
            fID = pFeat.Fields.FindField("speedLimit"); if (fID > 0) pFeat.set_Value(fID, Holding.SpeedLimit);
            fID = pFeat.Fields.FindField("speedLimitUOM"); if (fID > 0) pFeat.set_Value(fID, Holding.SpeedLimitUOM.ToString());
            fID = pFeat.Fields.FindField("instruction"); if (fID > 0) pFeat.set_Value(fID, Holding.Instruction);
            fID = pFeat.Fields.FindField("nonStandardHolding"); if (fID > 0) pFeat.set_Value(fID, Holding.NonStandardHolding);
            fID = pFeat.Fields.FindField("duration_Time"); if (fID > 0) pFeat.set_Value(fID, Holding.Duration_Distance);
            fID = pFeat.Fields.FindField("duration_Time_UOM"); if (fID > 0) pFeat.set_Value(fID, Holding.Duration_Distance_UOM);

            pFeat.Store();


            Polyline PolyLn2 = new PolylineClass();
            for (int i = hldG.PointCount / 2 - 1; i <= hldG.PointCount - 2; i++)
                (PolyLn2 as IPointCollection).AddPoint(hldG.Point[i]);



            pFeat = HoldingPathFeatureClass.CreateFeature();


            fID = pFeat.Fields.FindField("SHAPE"); if (fID > 0) pFeat.set_Value(fID, PolyLn2);
            fID = pFeat.Fields.FindField("FeatureGUID"); if (fID > 0) pFeat.set_Value(fID, Holding.ID);
            fID = pFeat.Fields.FindField("SegmentPointID"); if (fID > 0) pFeat.set_Value(fID, Holding.HoldingPoint.SegmentPointDesignator);
            fID = pFeat.Fields.FindField("type"); if (fID > 0) pFeat.set_Value(fID, Holding.Type.ToString());
            fID = pFeat.Fields.FindField("outboundCourse"); if (fID > 0) pFeat.set_Value(fID, Holding.OutboundCourse);
            fID = pFeat.Fields.FindField("outboundCourseType"); if (fID > 0) pFeat.set_Value(fID, Holding.OutboundCourseType.ToString());
            fID = pFeat.Fields.FindField("inboundCourse"); if (fID > 0) pFeat.set_Value(fID, Holding.InboundCourse);
            fID = pFeat.Fields.FindField("turnDirection"); if (fID > 0) pFeat.set_Value(fID, Holding.TurnDirection.ToString());
            fID = pFeat.Fields.FindField("upperLimit"); if (fID > 0) pFeat.set_Value(fID, Holding.UpperLimit);
            fID = pFeat.Fields.FindField("upperLimitUOM"); if (fID > 0) pFeat.set_Value(fID, Holding.UpperLimitUOM.ToString());
            fID = pFeat.Fields.FindField("upperLimitReference"); if (fID > 0) pFeat.set_Value(fID, Holding.UpperLimitReference.ToString());
            fID = pFeat.Fields.FindField("lowerLimit"); if (fID > 0) pFeat.set_Value(fID, Holding.LowerLimit);
            fID = pFeat.Fields.FindField("lowerLimitUOM"); if (fID > 0) pFeat.set_Value(fID, Holding.LowerLimitUOM.ToString());
            fID = pFeat.Fields.FindField("lowerLimitReference"); if (fID > 0) pFeat.set_Value(fID, Holding.LowerLimitReference.ToString());
            fID = pFeat.Fields.FindField("speedLimit"); if (fID > 0) pFeat.set_Value(fID, Holding.SpeedLimit);
            fID = pFeat.Fields.FindField("speedLimitUOM"); if (fID > 0) pFeat.set_Value(fID, Holding.SpeedLimitUOM.ToString());
            fID = pFeat.Fields.FindField("instruction"); if (fID > 0) pFeat.set_Value(fID, Holding.Instruction);
            fID = pFeat.Fields.FindField("nonStandardHolding"); if (fID > 0) pFeat.set_Value(fID, Holding.NonStandardHolding);
            fID = pFeat.Fields.FindField("duration_Time"); if (fID > 0) pFeat.set_Value(fID, Holding.Duration_Distance);
            fID = pFeat.Fields.FindField("duration_Time_UOM"); if (fID > 0) pFeat.set_Value(fID, Holding.Duration_Distance_UOM);

            pFeat.Store();
        }

        public static IElement CreateSegmentPointAnno_STAR(PDM.NavaidSystem navaidSegmentPoint, AbstractChartElement chartEl, string vertUom, ProcedureFixRoleType _PointRole,  bool findDME = false)
        {
            IElement el_SegPnt = null;
            PDM.PDMObject dme_tac = null;

            ChartElement_SigmaCollout_Navaid chrtEl_Sign = (ChartElement_SigmaCollout_Navaid)chartEl;
            PDM.NavaidComponent NAVAID = (PDM.VOR)(from element in navaidSegmentPoint.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.VOR) select element).FirstOrDefault();

            if (navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.NDB || navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.NDB_MKR)
                NAVAID = (PDM.NDB)(from element in navaidSegmentPoint.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.NDB) select element).FirstOrDefault();
            if (navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.ILS_DME)
            {
                NAVAID = (PDM.Localizer)(from element in navaidSegmentPoint.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.Localizer) select element).FirstOrDefault();
                if (findDME)
                    NAVAID = (PDM.DME)(from element in navaidSegmentPoint.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.DME) select element).FirstOrDefault();
            }

            if (navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.VOR_DME)
                dme_tac = (PDM.DME)(from element in navaidSegmentPoint.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.DME) select element).FirstOrDefault();
            if (navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.VORTAC)
                dme_tac = (PDM.TACAN)(from element in navaidSegmentPoint.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.TACAN) select element).FirstOrDefault();

            if (dme_tac != null)
            {
                NAVAID = (PDM.NavaidComponent)dme_tac;
            }

            if (NAVAID == null) return null;

            if (NAVAID.Geo == null) NAVAID.RebuildGeo();
            IPoint pnt = new PointClass();
            pnt.PutCoords(((IPoint)NAVAID.Geo).X - chrtEl_Sign.Anchor.X, ((IPoint)NAVAID.Geo).Y - chrtEl_Sign.Anchor.Y);


            #region InnerText

            chrtEl_Sign.TextContents[0][0].TextValue = navaidSegmentPoint.CodeNavaidSystemType.ToString().Replace("_", "/");

            if (navaidSegmentPoint.Components != null && navaidSegmentPoint.Components.Count > 0)
            {
                foreach (var item in navaidSegmentPoint.Components)
                {
                    if (item.PDM_Type == PDM_ENUM.VOR)
                    {
                        if (((VOR)item).VorType.HasValue && ((VOR)item).VorType.Value == CodeVOR.DVOR)
                        {
                            chrtEl_Sign.TextContents[0][0].TextValue = chrtEl_Sign.TextContents[0][0].TextValue.Replace("VOR", "DVOR") + " ";

                        }

                        chrtEl_Sign.TextContents[0][1].TextValue = ((VOR)item).Frequency.HasValue ? ((VOR)item).Frequency.Value.ToString() : "NAN";
                        break;
                    }
                }
            }





            chrtEl_Sign.TextContents[1][0].TextValue = NAVAID.Designator != null ? NAVAID.Designator : navaidSegmentPoint.Designator;
            chrtEl_Sign.MorseTextLine.MorseText = NAVAID.Designator != null ? NAVAID.Designator : navaidSegmentPoint.Designator;

            if (NAVAID is Localizer) chrtEl_Sign.MorseTextLine = null;
            if (NAVAID is DME && findDME) chrtEl_Sign.MorseTextLine = null;

            if (!(NAVAID is Localizer) && !findDME)
            {
                chrtEl_Sign.TextContents[3][0].TextValue = ArenaStaticProc.LatToDDMMSS(((IPoint)NAVAID.Geo).Y.ToString(), chrtEl_Sign.CoordType);
                chrtEl_Sign.TextContents[4][0].TextValue = ArenaStaticProc.LonToDDMMSS(((IPoint)NAVAID.Geo).X.ToString(), chrtEl_Sign.CoordType);
            }
            else
            {
                chrtEl_Sign.TextContents.Remove(chrtEl_Sign.TextContents[chrtEl_Sign.TextContents.Count - 1]);
                chrtEl_Sign.TextContents.Remove(chrtEl_Sign.TextContents[chrtEl_Sign.TextContents.Count - 1]);
            }

            chrtEl_Sign.TextContents[2][0].TextValue = "";

            if (navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.VOR_DME && !findDME)
            {

                var dme = (from element in navaidSegmentPoint.Components where (element != null) && (element.PDM_Type == PDM_ENUM.DME) select element).FirstOrDefault();
                if (dme != null)
                {
                    chrtEl_Sign.TextContents[2][0].TextValue = ((DME)dme).Channel != null ? ((DME)dme).Channel : "";
                }

            }

            else if (navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.VORTAC)
            {


                var tac = (from element in navaidSegmentPoint.Components where (element != null) && (element.PDM_Type == PDM_ENUM.TACAN) select element).FirstOrDefault();
                if (tac != null)
                {
                    chrtEl_Sign.TextContents[2][0].TextValue = ((TACAN)tac).Channel != null ? ((TACAN)tac).Channel : "";
                }

            }

            else if (navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.NDB || navaidSegmentPoint.CodeNavaidSystemType == NavaidSystemType.NDB_MKR)
            {
                chrtEl_Sign.TextContents[0][1].Visible = false;
                chrtEl_Sign.TextContents[2][0].StartSymbol.Text = "";
                chrtEl_Sign.TextContents[2][0].TextValue = "";
                //chrtEl_Sign.TextContents[2][1].TextValue = "";

            }
            else
            {
                chrtEl_Sign.TextContents.Remove(chrtEl_Sign.TextContents[2]);
            }

            if (chrtEl_Sign.TextContents.Count > 2 && chrtEl_Sign.TextContents[2][0].TextValue.Trim().Length == 0) chrtEl_Sign.TextContents.Remove(chrtEl_Sign.TextContents[2]);

            chrtEl_Sign.Anchor = new AncorPoint(((IPoint)NAVAID.Geo).X, ((IPoint)NAVAID.Geo).Y);


            #endregion

            #region Caption

            chrtEl_Sign.CaptionTextLine[0][0].TextValue = _PointRole.ToString();
            chrtEl_Sign.CaptionTextLine[0][0].Visible = _PointRole == ProcedureFixRoleType.IAF;
            

            if (!(NAVAID is Localizer) && !findDME)
            {
                chrtEl_Sign.CaptionTextLine[1][0].TextValue = navaidSegmentPoint.Name.ToString();
            }
            else if (NAVAID is Localizer)
            {
                chrtEl_Sign.CaptionTextLine[1][0].TextValue = "LOC";
                chrtEl_Sign.TextContents[1][0].TextValue = navaidSegmentPoint.Designator;
                chrtEl_Sign.TextContents[1][1].TextValue = NAVAID.Frequency.HasValue ? NAVAID.Frequency.Value.ToString() : "NAN";
            }
            else if (NAVAID is DME && findDME)
            {
                chrtEl_Sign.CaptionTextLine[1][0].TextValue = "DME";
                chrtEl_Sign.TextContents[1][0].TextValue = navaidSegmentPoint.Designator;
                chrtEl_Sign.TextContents[1][1].TextValue = ((DME)NAVAID).Channel.Length > 0 ? ((DME)NAVAID).Channel : "NAN";
            }


            if (chrtEl_Sign.CaptionTextLine[0][0].Visible)
            {
                chrtEl_Sign.Frame.FrameMargins.TopMargin = -7;
                chrtEl_Sign.Frame.FrameMargins.HeaderHorizontalMargin = 3;
            }
            else
                chrtEl_Sign.CaptionTextLine.Remove(chrtEl_Sign.CaptionTextLine[0]);

            #endregion

            #region Bottom

            if (!(NAVAID is Localizer) && !findDME)
            {
                if (dme_tac != null)
                {
                    int el = 0;
                    string ElevStr = "";
                    string ElevStrUOM = "";

                    if (dme_tac.Elev != null && dme_tac.Elev.HasValue && !dme_tac.Elev.ToString().StartsWith("NaN")) el = (int)Math.Round((double)dme_tac.Elev, 0);

                    if (el != 0)
                    {

                        // convert el value to FT
                        // if (dme_tac.Elev_UOM != UOM_DIST_VERT.FT)
                        {
                            //var elevDme = dme_tac.ConvertValueToFeet((double)el, dme_tac.Elev_UOM.ToString());
                            string uom = dme_tac.Elev_UOM.ToString();
                            double elevDme = ArenaStaticProc.UomTransformation(uom, vertUom, (double)el);
                            var ff = Math.DivRem((int)elevDme, 100, out el);
                            el = 100 * (ff + 1);
                        }

                        ElevStr = el.ToString() + " ";
                        ElevStrUOM = "FT"; //dme_tac.Elev_UOM.ToString();
                    }

                    chrtEl_Sign.BottomTextLine[0][0].TextValue = ElevStr;
                    chrtEl_Sign.BottomTextLine[0][1].TextValue = ElevStrUOM;
                }
                else
                {
                    chrtEl_Sign.BottomTextLine[0][0].TextValue = "";
                    chrtEl_Sign.BottomTextLine[0][1].TextValue = "";
                    chrtEl_Sign.BottomTextLine.Remove(chrtEl_Sign.BottomTextLine[0]);
                    chrtEl_Sign.HasFooter = false;

                }


            }
            else
            {
                chrtEl_Sign.BottomTextLine.Remove(chrtEl_Sign.BottomTextLine[0]);
                chrtEl_Sign.Frame.FrameMargins.BottomMargin = 5;
                chrtEl_Sign.Frame.FrameMargins.TopMargin = 0;
                chrtEl_Sign.Frame.FrameMargins.FooterHorizontalMargin = 0;
                chrtEl_Sign.Frame.FrameMargins.HeaderHorizontalMargin = 0;
                chrtEl_Sign.TextContents.RemoveAt(chrtEl_Sign.TextContents.Count - 1);
                chrtEl_Sign.HasFooter = false;

            }

            #endregion


            el_SegPnt = (IElement)chrtEl_Sign.ConvertToIElement();

            if (el_SegPnt is IGroupElement)
            {
                IGroupElement GrEl = el_SegPnt as IGroupElement;
                for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                {
                    GrEl.get_Element(i).Geometry = pnt as IGeometry;
                }
            }
            else el_SegPnt.Geometry = pnt;

            return el_SegPnt;

        }

        public static IElement CreateAngleIdicationAnno(IGeometry NavPosition, PDM.SegmentPoint sPnt, string NAVAID_Designator, double ARP_MagneticVariation, AbstractChartElement chartEl, NavaidSystemType navType)
        {
            IElement el_SegPnt = null;
            ChartElement_Radial chrtEl_Sign = (ChartElement_Radial)chartEl;

            IPoint pnt = new PointClass();
            pnt.PutCoords(((IPoint)NavPosition).X , ((IPoint)NavPosition).Y);

            #region InnerText

            chrtEl_Sign.TextContents[0][0].TextValue = NAVAID_Designator != null ? NAVAID_Designator : "";

            if (sPnt != null && sPnt.PointFacilityMakeUp != null && sPnt.PointFacilityMakeUp.AngleIndication != null)

                if (navType.ToString().StartsWith("NDB"))
                {
                    sPnt.PointFacilityMakeUp.AngleIndication.Angle = sPnt.PointFacilityMakeUp.AngleIndication.Angle + 180;
                    sPnt.PointFacilityMakeUp.AngleIndication.Angle = sPnt.PointFacilityMakeUp.AngleIndication.Angle > 360 ? sPnt.PointFacilityMakeUp.AngleIndication.Angle - 360 : sPnt.PointFacilityMakeUp.AngleIndication.Angle;
                }

                if (sPnt.PointFacilityMakeUp.AngleIndication.AngleType == BearingType.MAG)
                    chrtEl_Sign.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(sPnt.PointFacilityMakeUp.AngleIndication, chrtEl_Sign.TextContents[0][1].DataSource);
                else
                    chrtEl_Sign.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(sPnt.PointFacilityMakeUp.AngleIndication, chrtEl_Sign.TextContents[0][1].DataSource,0,0,ARP_MagneticVariation);

            #endregion

            if (navType.ToString().StartsWith("NDB"))
            {

                chrtEl_Sign.TextContents[0][1].StartSymbol.Text = "";
                AncorChartElementWord wrd = (AncorChartElementWord)chrtEl_Sign.TextContents[0][1].Clone();
                chrtEl_Sign.TextContents[0].Remove(chrtEl_Sign.TextContents[0][1]);
                chrtEl_Sign.TextContents[0].Insert(0, wrd);

            }

            el_SegPnt = (IElement)chrtEl_Sign.ConvertToIElement();

            if (el_SegPnt is IGroupElement)
            {
                IGroupElement GrEl = el_SegPnt as IGroupElement;
                for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                {
                    GrEl.get_Element(i).Geometry = pnt as IGeometry;
                }
            }
            else el_SegPnt.Geometry = pnt;

            return el_SegPnt;

        }

        public static IElement CreateAngleIdicationAnno(IGeometry NavPosition, AngleIndication AngleInd, string NAVAID_Designator, double ARP_MagneticVariation, AbstractChartElement chartEl, NavaidSystemType navType)
        {
            IElement el_SegPnt = null;
            ChartElement_Radial chrtEl_Sign = (ChartElement_Radial)chartEl;

            IPoint pnt = new PointClass();
            pnt.PutCoords(((IPoint)NavPosition).X, ((IPoint)NavPosition).Y);

            #region InnerText

            chrtEl_Sign.TextContents[0][0].TextValue = NAVAID_Designator != null ? NAVAID_Designator : "";

            if (AngleInd != null)
            {

                if (navType.ToString().StartsWith("NDB"))
                {
                    AngleInd.Angle = AngleInd.Angle + 180;
                    AngleInd.Angle = AngleInd.Angle > 360 ? AngleInd.Angle - 360 : AngleInd.Angle;
                }

                if (AngleInd.AngleType == BearingType.MAG)
                    chrtEl_Sign.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(AngleInd, chrtEl_Sign.TextContents[0][1].DataSource);
                else
                    chrtEl_Sign.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(AngleInd, chrtEl_Sign.TextContents[0][1].DataSource, 0, 0, ARP_MagneticVariation);

                if (navType.ToString().StartsWith("NDB"))
                {

                    chrtEl_Sign.TextContents[0][1].StartSymbol.Text = "";
                    AncorChartElementWord wrd = (AncorChartElementWord)chrtEl_Sign.TextContents[0][1].Clone();
                    chrtEl_Sign.TextContents[0].Remove(chrtEl_Sign.TextContents[0][1]);
                    chrtEl_Sign.TextContents[0].Insert(0, wrd);

                }
            }
            #endregion

            el_SegPnt = (IElement)chrtEl_Sign.ConvertToIElement();

            if (el_SegPnt is IGroupElement)
            {
                IGroupElement GrEl = el_SegPnt as IGroupElement;
                for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                {
                    GrEl.get_Element(i).Geometry = pnt as IGeometry;
                }
            }
            else el_SegPnt.Geometry = pnt;

            return el_SegPnt;

        }

        public static IElement CreateDistanceIdicationAnno(IGeometry NavPosition, PDM.SegmentPoint sPnt, string NAVAID_Designator, AbstractChartElement chartEl)
        {
            IElement el_SegPnt = null;
            ChartElement_SimpleText chrtEl_Sign = (ChartElement_SimpleText)chartEl;

            IPoint pnt = new PointClass();
            pnt.PutCoords(((IPoint)NavPosition).X, ((IPoint)NavPosition).Y);



            #region InnerText

            chrtEl_Sign.TextContents[0][0].TextValue = NAVAID_Designator != null ? NAVAID_Designator : "";

            if (sPnt != null && sPnt.PointFacilityMakeUp != null && sPnt.PointFacilityMakeUp.DistanceIndication != null)
                chrtEl_Sign.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(sPnt.PointFacilityMakeUp.DistanceIndication, chrtEl_Sign.TextContents[0][1].DataSource, 1);



            #endregion

            el_SegPnt = (IElement)chrtEl_Sign.ConvertToIElement();

            if (el_SegPnt is IGroupElement)
            {
                IGroupElement GrEl = el_SegPnt as IGroupElement;
                for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                {
                    GrEl.get_Element(i).Geometry = pnt as IGeometry;
                }
            }
            else el_SegPnt.Geometry = pnt;

            return el_SegPnt;

        }

        public static IElement CreateDistanceIdicationAnno(IGeometry NavPosition, DistanceIndication _Dist, string NAVAID_Designator, AbstractChartElement chartEl, string resultUom)
        {
            IElement el_SegPnt = null;
            ChartElement_SimpleText chrtEl_Sign = (ChartElement_SimpleText)chartEl;

            IPoint pnt = new PointClass();
            pnt.PutCoords(((IPoint)NavPosition).X, ((IPoint)NavPosition).Y);



            #region InnerText

            chrtEl_Sign.TextContents[0][0].TextValue = NAVAID_Designator != null ? NAVAID_Designator : "";

            if (_Dist != null)
                chrtEl_Sign.TextContents[0][1].TextValue = ChartsHelperClass.MakeText_UOM(_Dist, chrtEl_Sign.TextContents[0][1].DataSource, resultUom,1);

            #endregion

            el_SegPnt = (IElement)chrtEl_Sign.ConvertToIElement();

            if (el_SegPnt is IGroupElement)
            {
                IGroupElement GrEl = el_SegPnt as IGroupElement;
                for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                {
                    GrEl.get_Element(i).Geometry = pnt as IGeometry;
                }
            }
            else el_SegPnt.Geometry = pnt;

            return el_SegPnt;

        }

        public static void SaveBuffer(List<Bagel> polygons, IFeatureClass featureClass)
        {

            foreach (var polygon in polygons)
            {

                try
                {

                    IFeature feat = featureClass.CreateFeature();
                    feat.Shape = polygon.BagelPoly;

                    int codeTypeIndex = feat.Fields.FindField("codeType");
                    if (codeTypeIndex > 0)
                    {
                        feat.set_Value(codeTypeIndex, polygon.BagelCodeId);
                    }

                    codeTypeIndex = feat.Fields.FindField("codeClass");
                    if (codeTypeIndex > 0)
                    {
                        feat.set_Value(codeTypeIndex, polygon.BagelCodeClass);
                    }

                    codeTypeIndex = feat.Fields.FindField("codeId");
                    if (codeTypeIndex > 0)
                    {
                        feat.set_Value(codeTypeIndex, polygon.BagelCodeId);
                    }

                    codeTypeIndex = feat.Fields.FindField("txtName");
                    if (codeTypeIndex > 0)
                    {
                        feat.set_Value(codeTypeIndex, polygon.BagelTxtName);
                    }

                    codeTypeIndex = feat.Fields.FindField("MasterID");
                    if (codeTypeIndex > 0)
                    {
                        feat.set_Value(codeTypeIndex, polygon.MasterID);
                    }

                    codeTypeIndex = feat.Fields.FindField("localType");
                    if (codeTypeIndex > 0)
                    {
                        feat.set_Value(codeTypeIndex, polygon.BagelLocalType);
                    }

                    feat.Store();
                    Application.DoEvents();
                }
                catch (Exception ex )
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

            }
        }

        public static void UpdateBuffer(Bagel polygon, IFeatureClass featureClass)
        {
            IQueryFilter featFilter = new QueryFilterClass();

            featFilter.WhereClause = "MasterID = '" + polygon.MasterID + "'";

            IFeatureCursor featCur = featureClass.Update(featFilter, false);

            IFeature feat = featCur.NextFeature();

            if (feat != null)
            {
   
                feat.Shape = polygon.BagelPoly;
                int codeTypeIndex = feat.Fields.FindField("codeType");
                if (codeTypeIndex > 0)
                {
                    feat.set_Value(codeTypeIndex, polygon.BagelCodeId);
                }

                codeTypeIndex = feat.Fields.FindField("codeClass");
                if (codeTypeIndex > 0)
                {
                    feat.set_Value(codeTypeIndex, polygon.BagelCodeClass);
                }

                codeTypeIndex = feat.Fields.FindField("codeId");
                if (codeTypeIndex > 0)
                {
                    feat.set_Value(codeTypeIndex, polygon.BagelCodeId);
                }

                codeTypeIndex = feat.Fields.FindField("txtName");
                if (codeTypeIndex > 0)
                {
                    feat.set_Value(codeTypeIndex, polygon.BagelTxtName);
                }

                codeTypeIndex = feat.Fields.FindField("MasterID");
                if (codeTypeIndex > 0)
                {
                    feat.set_Value(codeTypeIndex, polygon.MasterID);
                }

                feat.Store();

            }

            Marshal.ReleaseComObject(featCur);

            Application.DoEvents();

        }


        public static void SaveVerticalStructPoint_ChartGeo(IFeatureClass Anno_ObstacleGeo_featClass, VerticalStructure VStruct, double LimitM = 0)
        {
           
            foreach (VerticalStructurePart vsPart in VStruct.Parts)
            {

                if (vsPart.Geo == null) vsPart.RebuildGeo();
                if (vsPart.Geo.GeometryType != esriGeometryType.esriGeometryPoint) continue;

                if (vsPart.ConvertValueToMeter(vsPart.Elev, vsPart.Elev_UOM.ToString()) < LimitM) continue;

                int fID = -1;
                IFeature pFeat = Anno_ObstacleGeo_featClass.CreateFeature();

                if (vsPart.Geo == null) vsPart.RebuildGeo();

                try
                {
                    IZAware zAware = (ESRI.ArcGIS.Geometry.IZAware)vsPart.Geo;
                    zAware.ZAware = false;
                    IMAware mAware = (ESRI.ArcGIS.Geometry.IMAware)vsPart.Geo;
                    mAware.MAware = false;

                    fID = pFeat.Fields.FindField("SHAPE"); if (fID > 0) pFeat.set_Value(fID, vsPart.Geo);
                    fID = pFeat.Fields.FindField("FeatureGUID"); if (fID > 0) pFeat.set_Value(fID, vsPart.ID);
                    fID = pFeat.Fields.FindField("NAME"); if (fID > 0) pFeat.set_Value(fID, VStruct.Name);
                    fID = pFeat.Fields.FindField("type1"); if (fID > 0) pFeat.set_Value(fID, VStruct.Type.ToString());

                    fID = pFeat.Fields.FindField("Lighted");
                    if (fID > 0)
                    {
                        int fl = VStruct.Lighted ? 1 : 0;
                        pFeat.set_Value(fID, fl);
                    }
                    
                    fID = pFeat.Fields.FindField("GroupFlag");
                    if (fID > 0)
                    {
                        if (VStruct.Group && vsPart.Height.HasValue && vsPart.ConvertValueToMeter(vsPart.Height.Value, vsPart.Height_UOM.ToString()) < 300)
                            pFeat.set_Value(fID, 1);
                        else if (VStruct.Group && vsPart.Height.HasValue && vsPart.ConvertValueToMeter(vsPart.Height.Value, vsPart.Height_UOM.ToString()) >= 300)
                            pFeat.set_Value(fID, 1);
                        else
                            pFeat.set_Value(fID, 0);

                    }


                    fID = pFeat.Fields.FindField("High");
                    if (fID > 0 && vsPart.Height.HasValue)
                    {
                        int fl = vsPart.ConvertValueToMeter(vsPart.Height.Value, vsPart.Elev_UOM.ToString()) >= 300 ? 1 : 0;
                        pFeat.set_Value(fID, fl);
                    }


                    fID = pFeat.Fields.FindField("type2"); if (fID > 0) pFeat.set_Value(fID, vsPart.Type.ToString());
                    fID = pFeat.Fields.FindField("verticalExtent"); if (fID > 0) pFeat.set_Value(fID, vsPart.VerticalExtent);
                    fID = pFeat.Fields.FindField("verticalExtentUom"); if (fID > 0) pFeat.set_Value(fID, vsPart.VerticalExtent.ToString());
                    fID = pFeat.Fields.FindField("designator"); if (fID > 0) pFeat.set_Value(fID, vsPart.Designator);
                    fID = pFeat.Fields.FindField("Lighting"); if (fID > 0) pFeat.set_Value(fID, vsPart.MarkingFirstColour.ToString());
                    fID = pFeat.Fields.FindField("Elevation"); if (fID > 0) pFeat.set_Value(fID, vsPart.Elev);
                    fID = pFeat.Fields.FindField("ElevationUom"); if (fID > 0) pFeat.set_Value(fID, vsPart.Elev_UOM.ToString());
                    fID = pFeat.Fields.FindField("Height"); if (fID > 0) pFeat.set_Value(fID, vsPart.Height);
                    fID = pFeat.Fields.FindField("HeightUom"); if (fID > 0) pFeat.set_Value(fID, vsPart.Height_UOM.ToString()); 
                    

                    pFeat.Store();
                }
                catch(Exception ex)
                {
                    //System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                Application.DoEvents();
            }
           
        }

        public static void SaveVerticalStructPoint_ChartGeo(IFeatureClass Anno_ObstacleGeo_featClass, VerticalStructure VStruct, VerticalStructurePart vsPart, double LimitM = 0)
        {

            //foreach (VerticalStructurePart vsPart in VStruct.Parts)
            {

                if (vsPart.Geo == null) vsPart.RebuildGeo();
                if (vsPart.Geo.GeometryType != esriGeometryType.esriGeometryPoint) return;

                if (vsPart.ConvertValueToMeter(vsPart.Elev, vsPart.Elev_UOM.ToString()) < LimitM) return;

                int fID = -1;
                IFeature pFeat = Anno_ObstacleGeo_featClass.CreateFeature();

                if (vsPart.Geo == null) vsPart.RebuildGeo();

                try
                {
                    IZAware zAware = (ESRI.ArcGIS.Geometry.IZAware)vsPart.Geo;
                    zAware.ZAware = false;
                    IMAware mAware = (ESRI.ArcGIS.Geometry.IMAware)vsPart.Geo;
                    mAware.MAware = false;

                    fID = pFeat.Fields.FindField("SHAPE"); if (fID > 0) pFeat.set_Value(fID, vsPart.Geo);
                    fID = pFeat.Fields.FindField("FeatureGUID"); if (fID > 0) pFeat.set_Value(fID, vsPart.ID);
                    fID = pFeat.Fields.FindField("NAME"); if (fID > 0) pFeat.set_Value(fID, VStruct.Name);
                    fID = pFeat.Fields.FindField("type1"); if (fID > 0) pFeat.set_Value(fID, VStruct.Type.ToString());

                    fID = pFeat.Fields.FindField("Lighted");
                    if (fID > 0)
                    {
                        int fl = VStruct.Lighted ? 1 : 0;
                        pFeat.set_Value(fID, fl);
                    }

                    fID = pFeat.Fields.FindField("GroupFlag");
                    if (fID > 0)
                    {
                        if (VStruct.Group && vsPart.Height.HasValue && vsPart.ConvertValueToMeter(vsPart.Height.Value, vsPart.Height_UOM.ToString()) < 300)
                            pFeat.set_Value(fID, 1);
                        else if (VStruct.Group && vsPart.Height.HasValue && vsPart.ConvertValueToMeter(vsPart.Height.Value, vsPart.Height_UOM.ToString()) >= 300)
                            pFeat.set_Value(fID, 1);
                        else
                            pFeat.set_Value(fID, 0);

                    }


                    fID = pFeat.Fields.FindField("High");
                    if (fID > 0 && vsPart.Height.HasValue)
                    {
                        int fl = vsPart.ConvertValueToMeter(vsPart.Height.Value, vsPart.Height_UOM.ToString()) >= 300 ? 1 : 0;
                        pFeat.set_Value(fID, fl);
                    }


                    fID = pFeat.Fields.FindField("type2"); if (fID > 0) pFeat.set_Value(fID, vsPart.Type.ToString());
                    fID = pFeat.Fields.FindField("verticalExtent"); if (fID > 0) pFeat.set_Value(fID, vsPart.VerticalExtent);
                    fID = pFeat.Fields.FindField("verticalExtentUom"); if (fID > 0) pFeat.set_Value(fID, vsPart.VerticalExtent.ToString());
                    fID = pFeat.Fields.FindField("designator"); if (fID > 0) pFeat.set_Value(fID, vsPart.Designator);
                    fID = pFeat.Fields.FindField("Lighting"); if (fID > 0) pFeat.set_Value(fID, vsPart.MarkingFirstColour.ToString());
                    fID = pFeat.Fields.FindField("Elevation"); if (fID > 0) pFeat.set_Value(fID, vsPart.Elev);
                    fID = pFeat.Fields.FindField("ElevationUom"); if (fID > 0) pFeat.set_Value(fID, vsPart.Elev_UOM.ToString());
                    fID = pFeat.Fields.FindField("Height"); if (fID > 0) pFeat.set_Value(fID, vsPart.Height);
                    fID = pFeat.Fields.FindField("HeightUom"); if (fID > 0) pFeat.set_Value(fID, vsPart.Height_UOM.ToString());


                    pFeat.Store();
                }
                catch (Exception ex)
                {
                    //System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                Application.DoEvents();
            }

        }


        public static PDMObject SaveHolding_PointChartGeo(IFeatureClass featureClass, PDM.HoldingPattern Hldng, double ADHP_MagVar = 0)
        {
            PDM.PDMObject obj = null;
            if (Hldng.HoldingPoint == null) return null;
            if (Hldng.HoldingPoint.PointChoiceID == null) return null;

            switch (Hldng.HoldingPoint.PointChoice)
            {
                case PDM.PointChoice.DesignatedPoint:
                    obj = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                           where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.WayPoint)
                               && (element.ID.StartsWith(Hldng.HoldingPoint.PointChoiceID))
                           select element).FirstOrDefault();
                    break;
                case PDM.PointChoice.Navaid:
                    obj = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                           where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.NavaidSystem)
                               && (element.ID.StartsWith(Hldng.HoldingPoint.PointChoiceID) || ((NavaidSystem)element).ID_Feature.StartsWith(Hldng.HoldingPoint.PointChoiceID))
                           select element).FirstOrDefault();
                    break;

            }

            if (obj == null) return null;

            double magVar = 0;
            try
            {
                double? altitude = Hldng.ConvertValueToMeter(Hldng.LowerLimit.Value, Hldng.LowerLimitUOM.ToString()) / 1000;


                magVar = IsNumeric(Hldng.HoldingPoint.Lat) ? ChartValidator.ExternalMagVariation.MagVar(Convert.ToDouble(Hldng.HoldingPoint.Lat), Convert.ToDouble(Hldng.HoldingPoint.Lon), altitude.Value,
                                    Hldng.ActualDate.Day, Hldng.ActualDate.Month, Hldng.ActualDate.Year, 1) :
                                     ChartValidator.ExternalMagVariation.MagVar(Convert.ToDouble(Hldng.HoldingPoint.Y), Convert.ToDouble(Hldng.HoldingPoint.X), altitude.Value,
                                    Hldng.ActualDate.Day, Hldng.ActualDate.Month, Hldng.ActualDate.Year, 1);
            }
            catch { magVar = ADHP_MagVar; }


            if (obj.Geo == null) obj.RebuildGeo();
            if (obj.Geo == null && obj.PDM_Type == PDM_ENUM.NavaidSystem)
            {
                
                NavaidSystem ns = (NavaidSystem)obj;
                if (ns.Components != null && ns.Components.Count > 0)
                {
                    IPoint pp = new PointClass();
                    pp.PutCoords (((IPoint)ns.Components[0].Geo).X, ((IPoint)ns.Components[0].Geo).Y);
                    obj.Geo = pp;

                }

            }

            int fID = -1;
            IFeature pFeat = featureClass.CreateFeature();

            IPoint pnt = new PointClass();
            pnt.PutCoords(((IPoint)obj.Geo).X, ((IPoint)obj.Geo).Y);

            Hldng.HoldingPoint.ID = obj.ID;

            fID = pFeat.Fields.FindField("SHAPE"); if (fID > 0) pFeat.set_Value(fID, pnt);
            fID = pFeat.Fields.FindField("FeatureGUID"); if (fID > 0) pFeat.set_Value(fID, Hldng.ID);
            fID = pFeat.Fields.FindField("SegmentPointID"); if (fID > 0) pFeat.set_Value(fID, Hldng.HoldingPoint.SegmentPointDesignator);
            fID = pFeat.Fields.FindField("type"); if (fID > 0) pFeat.set_Value(fID, Hldng.Type.ToString());
            fID = pFeat.Fields.FindField("outboundCourse"); if (fID > 0) pFeat.set_Value(fID, Hldng.OutboundCourse);
            fID = pFeat.Fields.FindField("outboundCourseType"); if (fID > 0) pFeat.set_Value(fID, Hldng.OutboundCourseType.ToString());
            fID = pFeat.Fields.FindField("inboundCourse"); if (fID > 0) pFeat.set_Value(fID, Hldng.InboundCourse + magVar);
            fID = pFeat.Fields.FindField("turnDirection"); if (fID > 0) pFeat.set_Value(fID, Hldng.TurnDirection.ToString());
            fID = pFeat.Fields.FindField("upperLimit"); if (fID > 0) pFeat.set_Value(fID, Hldng.UpperLimit);
            fID = pFeat.Fields.FindField("upperLimitUOM"); if (fID > 0) pFeat.set_Value(fID, Hldng.UpperLimitUOM.ToString());
            fID = pFeat.Fields.FindField("upperLimitReference"); if (fID > 0) pFeat.set_Value(fID, Hldng.UpperLimitReference.ToString());
            fID = pFeat.Fields.FindField("lowerLimit"); if (fID > 0) pFeat.set_Value(fID, Hldng.LowerLimit);
            fID = pFeat.Fields.FindField("lowerLimitUOM"); if (fID > 0) pFeat.set_Value(fID, Hldng.LowerLimitUOM.ToString());
            fID = pFeat.Fields.FindField("lowerLimitReference"); if (fID > 0) pFeat.set_Value(fID, Hldng.LowerLimitReference.ToString());
            fID = pFeat.Fields.FindField("speedLimit"); if (fID > 0) pFeat.set_Value(fID, Hldng.SpeedLimit);
            fID = pFeat.Fields.FindField("speedLimitUOM"); if (fID > 0) pFeat.set_Value(fID, Hldng.SpeedLimitUOM.ToString());
            fID = pFeat.Fields.FindField("instruction"); if (fID > 0) pFeat.set_Value(fID, Hldng.Instruction);
            fID = pFeat.Fields.FindField("nonStandardHolding"); if (fID > 0) pFeat.set_Value(fID, Hldng.NonStandardHolding);
            fID = pFeat.Fields.FindField("duration_Time"); if (fID > 0) pFeat.set_Value(fID, Hldng.Duration_Distance);
            fID = pFeat.Fields.FindField("duration_Time_UOM"); if (fID > 0) pFeat.set_Value(fID, Hldng.Duration_Distance_UOM);
            //fID = pFeat.Fields.FindField("endPoint_SegmentPointID"); if (fID > 0) pFeat.set_Value(fID, hlng.);

            pFeat.Store();


            if (Hldng.EndPoint != null && Hldng.EndPoint.X.HasValue && Hldng.EndPoint.Y.HasValue)
            {
                pnt = new PointClass();
                pnt.PutCoords(Hldng.EndPoint.X.Value, Hldng.EndPoint.Y.Value);

                fID = -1;
                pFeat = featureClass.CreateFeature();

                double angle = Hldng.EndPoint.PointFacilityMakeUp != null && Hldng.EndPoint.PointFacilityMakeUp.AngleIndication != null && Hldng.EndPoint.PointFacilityMakeUp.AngleIndication.Angle.HasValue ?
                    Hldng.EndPoint.PointFacilityMakeUp.AngleIndication.Angle.Value : 0;

                fID = pFeat.Fields.FindField("SHAPE"); if (fID > 0) pFeat.set_Value(fID, pnt);
                fID = pFeat.Fields.FindField("FeatureGUID"); if (fID > 0) pFeat.set_Value(fID, Hldng.ID);
                fID = pFeat.Fields.FindField("SegmentPointID"); if (fID > 0) pFeat.set_Value(fID, Hldng.EndPoint.SegmentPointDesignator);
                fID = pFeat.Fields.FindField("type"); if (fID > 0) pFeat.set_Value(fID, "LegSpan_endPoint");
                fID = pFeat.Fields.FindField("outboundCourse"); if (fID > 0) pFeat.set_Value(fID, angle);

                //fID = pFeat.Fields.FindField("outboundCourseType"); if (fID > 0) pFeat.set_Value(fID, hlng.OutboundCourseType.ToString());
                //fID = pFeat.Fields.FindField("inboundCourse"); if (fID > 0) pFeat.set_Value(fID, hlng.InboundCourse);
                //fID = pFeat.Fields.FindField("turnDirection"); if (fID > 0) pFeat.set_Value(fID, hlng.TurnDirection.ToString());
                //fID = pFeat.Fields.FindField("upperLimit"); if (fID > 0) pFeat.set_Value(fID, hlng.UpperLimit);
                //fID = pFeat.Fields.FindField("upperLimitUOM"); if (fID > 0) pFeat.set_Value(fID, hlng.UpperLimitUOM.ToString());
                //fID = pFeat.Fields.FindField("upperLimitReference"); if (fID > 0) pFeat.set_Value(fID, hlng.UpperLimitReference.ToString());
                //fID = pFeat.Fields.FindField("lowerLimit"); if (fID > 0) pFeat.set_Value(fID, hlng.LowerLimit);
                //fID = pFeat.Fields.FindField("lowerLimitUOM"); if (fID > 0) pFeat.set_Value(fID, hlng.LowerLimitUOM.ToString());
                //fID = pFeat.Fields.FindField("lowerLimitReference"); if (fID > 0) pFeat.set_Value(fID, hlng.LowerLimitReference.ToString());
                //fID = pFeat.Fields.FindField("speedLimit"); if (fID > 0) pFeat.set_Value(fID, hlng.SpeedLimit);
                //fID = pFeat.Fields.FindField("speedLimitUOM"); if (fID > 0) pFeat.set_Value(fID, hlng.SpeedLimitUOM.ToString());
                //fID = pFeat.Fields.FindField("instruction"); if (fID > 0) pFeat.set_Value(fID, hlng.Instruction);
                //fID = pFeat.Fields.FindField("nonStandardHolding"); if (fID > 0) pFeat.set_Value(fID, hlng.NonStandardHolding);
                //fID = pFeat.Fields.FindField("duration_Time"); if (fID > 0) pFeat.set_Value(fID, hlng.Duration_Distance);
                //fID = pFeat.Fields.FindField("duration_Time_UOM"); if (fID > 0) pFeat.set_Value(fID, hlng.Duration_Distance_UOM);
                //fID = pFeat.Fields.FindField("endPoint_SegmentPointID"); if (fID > 0) pFeat.set_Value(fID, hlng.);

                pFeat.Store();

            }

            return obj;
        }


        public static bool IsNumeric(string anyString)
        {
            if (anyString == null)
            {
                anyString = "";
            }
            if (anyString.Length > 0)
            {
                double dummyOut = new double();
                System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US", true);
                bool res = Double.TryParse(anyString, out dummyOut) && !Double.IsNaN(dummyOut);
                return res;
            }
            else
            {
                return false;
            }
        }
        public static void saveRouteSegment_ChartRouteGeo(IFeatureClass featureClass, string RouteFormed, PDM.RouteSegment RTE, IGeometry geo)
        {
            int fID = -1;

            fID = featureClass.Fields.FindField("FeatureGUID_OLD");
            //if (fID >= 0)
            //{
            //    featureClass.DeleteField(featureClass.Fields.Field[fID]);
                
            //}

            IFeature pFeat = featureClass.CreateFeature();

            fID = pFeat.Fields.FindField("SHAPE"); if (fID > 0) pFeat.set_Value(fID, geo);
            fID = pFeat.Fields.FindField("FeatureGUID"); if (fID > 0) pFeat.set_Value(fID, RTE.ID);
            fID = pFeat.Fields.FindField("designator"); if (fID > 0) pFeat.set_Value(fID, RTE.GetObjectLabel());
            fID = pFeat.Fields.FindField("codeType"); if (fID > 0) pFeat.set_Value(fID, RTE.CodeType);
            fID = pFeat.Fields.FindField("RouteFormed"); if (fID > 0) pFeat.set_Value(fID, RouteFormed);

            fID = pFeat.Fields.FindField("RouteFormedID");
            if (fID > 0 && RTE.SourceDetail != null) pFeat.set_Value(fID, RTE.SourceDetail);
            if (fID < 0 && RTE.SourceDetail != null) pFeat.set_Value(pFeat.Fields.FindField("RouteFormed"), RTE.SourceDetail);

            fID = pFeat.Fields.FindField("codeLvl"); if (fID > 0) pFeat.set_Value(fID, RTE.CodeLvl.ToString());
            fID = pFeat.Fields.FindField("codeIntl"); if (fID > 0) pFeat.set_Value(fID, RTE.CodeIntl.ToString());
            fID = pFeat.Fields.FindField("codeDir"); if (fID > 0) pFeat.set_Value(fID, RTE.CodeDir.ToString());
            fID = pFeat.Fields.FindField("navigationType");
            if (fID > 0)
            {
                if (RTE.NavigationType == PDM.CodeRouteNavigation.RNAV)
                    pFeat.set_Value(fID, RTE.NavigationType.ToString());
                else
                    pFeat.set_Value(fID, PDM.CodeRouteNavigation.CONV.ToString());

            }
            fID = pFeat.Fields.FindField("valDistVerUpper"); if (fID > 0) pFeat.set_Value(fID, RTE.ValDistVerUpper);
            fID = pFeat.Fields.FindField("uomDistVerUpper"); if (fID > 0) pFeat.set_Value(fID, RTE.UomValDistVerUpper.ToString());
            fID = pFeat.Fields.FindField("codeDistVerUpper"); if (fID > 0) pFeat.set_Value(fID, RTE.CodeDistVerUpper.ToString());
            fID = pFeat.Fields.FindField("valDistVerLower"); if (fID > 0) pFeat.set_Value(fID, RTE.ValDistVerLower);
            fID = pFeat.Fields.FindField("uomDistVerLower"); if (fID > 0) pFeat.set_Value(fID, RTE.UomValDistVerLower.ToString());
            fID = pFeat.Fields.FindField("codeDistVerLower"); if (fID > 0) pFeat.set_Value(fID, RTE.CodeDistVerLower.ToString());
            fID = pFeat.Fields.FindField("valDistVerMnm"); if (fID > 0) pFeat.set_Value(fID, RTE.ValDistVerMnm);
            fID = pFeat.Fields.FindField("uomDistVerMnm"); if (fID > 0) pFeat.set_Value(fID, RTE.UomValDistVerMnm.ToString());
            fID = pFeat.Fields.FindField("codeDistVerMnm"); if (fID > 0) pFeat.set_Value(fID, RTE.CodeDistVerMnm.ToString());
            fID = pFeat.Fields.FindField("valDistVerLowerOvrde"); if (fID > 0) pFeat.set_Value(fID, RTE.ValDistVerLowerOvrde);
            fID = pFeat.Fields.FindField("uomDistVerLowerOvrde"); if (fID > 0) pFeat.set_Value(fID, RTE.UomValDistVerLowerOvrde.ToString());
            fID = pFeat.Fields.FindField("codeDistVerLowerOvrde"); if (fID > 0) pFeat.set_Value(fID, RTE.CodeDistVerLowerOvrde.ToString());
            fID = pFeat.Fields.FindField("ValWidRight"); if (fID > 0) pFeat.set_Value(fID, RTE.ValWidRight);
            fID = pFeat.Fields.FindField("ValWidLeft"); if (fID > 0) pFeat.set_Value(fID, RTE.ValWidLeft);
            fID = pFeat.Fields.FindField("uomWid"); if (fID > 0) pFeat.set_Value(fID, RTE.UomValWid.ToString());
            fID = pFeat.Fields.FindField("valTrueTrack"); if (fID > 0) pFeat.set_Value(fID, RTE.ValTrueTrack);
            fID = pFeat.Fields.FindField("valMagTrack"); if (fID > 0) pFeat.set_Value(fID, RTE.ValMagTrack);
            fID = pFeat.Fields.FindField("valReversTrueTrack"); if (fID > 0) pFeat.set_Value(fID, RTE.ValReversTrueTrack);
            fID = pFeat.Fields.FindField("valReversMagTrack"); if (fID > 0) pFeat.set_Value(fID, RTE.ValReversMagTrack);
            fID = pFeat.Fields.FindField("valLen"); if (fID > 0) pFeat.set_Value(fID, RTE.ValLen);
            fID = pFeat.Fields.FindField("uomValLen"); if (fID > 0) pFeat.set_Value(fID, RTE.UomValLen.ToString());


            pFeat.Store();
            Application.DoEvents();

        }

        public static void WrapText(ref ChartElement_SimpleText chrtEl_legName)
        {
            string[] lst = chrtEl_legName.TextContents[0][0].TextValue.Split('/');
            chrtEl_legName.TextContents[0][0].TextValue = lst[0];

            if (lst.Length > 1)
            {
                for (int i = 1; i <= lst.Length - 1; i++)
                {
                    List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
                    AncorChartElementWord wrd = new AncorChartElementWord(lst[i]);//создаем слово
                    wrd.Font.Size = chrtEl_legName.TextContents[0][0].Font.Size;//false;
                    wrd.Font.Name = chrtEl_legName.TextContents[0][0].Font.Name;//false;
                    wrd.Font.Bold = chrtEl_legName.TextContents[0][0].Font.Bold;//false;
                    txtLine.Add(wrd); // добавим его в строку
                    chrtEl_legName.TextContents.Add(txtLine);
                }

                chrtEl_legName.Placed = false;
            }
        }

        public static void WrapText(ref ChartElement_TextArrow chrtEl_legName)
        {
            string[] lst = chrtEl_legName.TextContents[0][0].TextValue.Split('/');
            chrtEl_legName.TextContents[0][0].TextValue = lst[0];

            if (lst.Length > 1)
            {
                for (int i = 1; i <= lst.Length - 1; i++)
                {
                    List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
                    AncorChartElementWord wrd = new AncorChartElementWord(lst[i]);//создаем слово
                    wrd.Font.Size = chrtEl_legName.TextContents[0][0].Font.Size;//false;
                    wrd.Font.Name = chrtEl_legName.TextContents[0][0].Font.Name;//false;
                    wrd.Font.Bold = chrtEl_legName.TextContents[0][0].Font.Bold;//false;
                    txtLine.Add(wrd); // добавим его в строку
                    chrtEl_legName.TextContents.Insert(i,txtLine);
                }
            }
        }

        public static int Round_Hundred(double _val)
        {
            int el = 0;
            var ff = Math.DivRem((int)_val, 100, out el);
            el = 100 * ff + (el >= 50 ? 100 : 0);

            return el;
        }

        public static IElement CreateSegmentPointLegHeightAnno(ProcedureLeg Legs, AbstractChartElement chartEl, string vertUom, string distUom, IMap FocusMap, ISpatialReference pSpatialReference, string ProcName =null)
        {

            ChartElement_TextArrow txtArrow = (ChartElement_TextArrow)chartEl;

            double _LowerLimitAltitude = Double.NaN;
            double _UpperLimitAltitude = Double.NaN;

            #region  format elemnet

            #region Округлить значение высот

            if (Legs.LowerLimitAltitude.HasValue && !Double.IsNaN(Legs.LowerLimitAltitude.Value) && Legs.LowerLimitAltitudeUOM == UOM_DIST_VERT.M)
            {
                _LowerLimitAltitude = Legs.LowerLimitAltitude.Value;

                var obj = ArenaStaticProc.UomTransformation(Legs.LowerLimitAltitudeUOM.ToString(), vertUom, (Double)Legs.LowerLimitAltitude.Value, 10);
                int el = Round_Hundred(obj);
                Legs.LowerLimitAltitude = el;
            }
            if (Legs.UpperLimitAltitude.HasValue && !Double.IsNaN(Legs.UpperLimitAltitude.Value) && Legs.UpperLimitAltitudeUOM == UOM_DIST_VERT.M)
            {
                _UpperLimitAltitude = Legs.UpperLimitAltitude.Value;

                var obj = ArenaStaticProc.UomTransformation(Legs.UpperLimitAltitudeUOM.ToString(), vertUom, (Double)Legs.UpperLimitAltitude.Value, 10);
                int el = Round_Hundred(obj);
                Legs.UpperLimitAltitude = el;

            }

            #endregion

            switch (Legs.AltitudeInterpretation)
            {
                case AltitudeUseType.ABOVE_LOWER:

                    #region 
                    
                        txtArrow.TextContents.RemoveRange(1, 2);

                        if (Legs.LowerLimitAltitudeUOM != PDM.UOM_DIST_VERT.FL)
                        {
                            txtArrow.TextContents[0][0].DataSource.Value = "LowerLimitAltitude";
                            txtArrow.TextContents[0][0].Font.UnderLine = true;
                            txtArrow.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][0].DataSource);

                            txtArrow.TextContents[0].RemoveRange(1, 1);
                        }
                        else
                        {
                            txtArrow.TextContents[0][0].DataSource.Value = "LowerLimitAltitudeUOM";
                            txtArrow.TextContents[0][0].Font.UnderLine = true;
                            txtArrow.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][0].DataSource);

                            txtArrow.TextContents[0][1].Font.UnderLine = true;
                            txtArrow.TextContents[0][1].DataSource.Value = "LowerLimitAltitude";
                            txtArrow.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][1].DataSource);

                        }

                    #endregion

                    break;

                case AltitudeUseType.BELOW_UPPER:

                    #region 
                    
                            txtArrow.TextContents.RemoveRange(2, 1);
                            txtArrow.TextContents[0][0].Font.UnderLine = true;
                            txtArrow.TextContents[0][0].TextValue = "______";
                            txtArrow.TextContents[0][0].TextPosition = ANCOR.MapCore.textPosition.Subscript;
                            txtArrow.TextContents[0].RemoveRange(1, 1);

                            if (Legs.UpperLimitAltitudeUOM != PDM.UOM_DIST_VERT.FL)
                            {
                                txtArrow.TextContents[1][0].DataSource.Value = "UpperLimitAltitude";
                                txtArrow.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][0].DataSource);
                                txtArrow.TextContents[1].RemoveRange(1, 1);
                            }
                            else
                            {
                                txtArrow.TextContents[1][0].DataSource.Value = "UpperLimitAltitudeUOM";
                                txtArrow.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][0].DataSource);
                                txtArrow.TextContents[1][1].DataSource.Value = "UpperLimitAltitude";
                                txtArrow.TextContents[1][1].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][1].DataSource);
                            }

                    #endregion


                    break;

                case AltitudeUseType.AT_LOWER:

                    #region 
                    
                    txtArrow.TextContents.RemoveRange(2, 1);
                    txtArrow.TextContents[0][0].Font.UnderLine = true;
                    txtArrow.TextContents[0][0].TextValue = "______";
                    txtArrow.TextContents[0][0].TextPosition = ANCOR.MapCore.textPosition.Subscript;
                    txtArrow.TextContents[0].RemoveRange(1, 1);

                    if (Legs.UpperLimitAltitudeUOM != PDM.UOM_DIST_VERT.FL)
                    {
                        txtArrow.TextContents[1][0].DataSource.Value = "LowerLimitAltitude";
                        txtArrow.TextContents[1][0].Font.UnderLine = true;
                        txtArrow.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][0].DataSource);
                        txtArrow.TextContents[1].RemoveRange(1, 1);
                    }
                    else
                    {
                        txtArrow.TextContents[1][0].DataSource.Value = "LowerLimitAltitudeUOM";
                        txtArrow.TextContents[1][0].Font.UnderLine = true;
                        txtArrow.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][0].DataSource);

                        txtArrow.TextContents[1][1].DataSource.Value = "LowerLimitAltitude";
                        txtArrow.TextContents[1][1].Font.UnderLine = true;
                        txtArrow.TextContents[1][1].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][1].DataSource);
                    }

                    #endregion


                    break;

                case AltitudeUseType.BETWEEN:

                    #region 

                    txtArrow.TextContents[0][0].Font.UnderLine = true;
                    txtArrow.TextContents[0][0].TextValue = "______";
                    txtArrow.TextContents[0][0].TextPosition = ANCOR.MapCore.textPosition.Subscript;
                    txtArrow.TextContents[0].RemoveRange(1, 1);

                    if (Legs.UpperLimitAltitudeUOM != PDM.UOM_DIST_VERT.FL)
                    {

                        txtArrow.TextContents[1][0].DataSource.Value = "UpperLimitAltitude";
                        txtArrow.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][0].DataSource);
                        txtArrow.TextContents[1].RemoveRange(1, 1);

                        txtArrow.TextContents[2][0].DataSource.Value = "LowerLimitAltitude";
                        txtArrow.TextContents[2][0].Font.UnderLine = true;
                        txtArrow.TextContents[2][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[2][0].DataSource);
                        txtArrow.TextContents[2].RemoveRange(1, 1);

                        if (txtArrow.TextContents[1][0].TextValue.CompareTo(txtArrow.TextContents[2][0].TextValue) == 0)
                        {
                            txtArrow.TextContents.Remove(txtArrow.TextContents[1]);
                        }
                        else
                            txtArrow.Leading = -0.7;

                    }
                    else
                    {
                        txtArrow.TextContents[1][0].DataSource.Value = "UpperLimitAltitude";
                        txtArrow.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][0].DataSource);
                        txtArrow.TextContents[1][1].DataSource.Value = "UpperLimitAltitudeUOM";
                        txtArrow.TextContents[1][1].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][1].DataSource);

                        txtArrow.TextContents[2][0].DataSource.Value = "LowerLimitAltitude";
                        txtArrow.TextContents[2][0].Font.UnderLine = true;
                        txtArrow.TextContents[2][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[2][0].DataSource);
                        txtArrow.TextContents[2][1].DataSource.Value = "LowerLimitAltitudeUOM";
                        txtArrow.TextContents[2][1].Font.UnderLine = true;
                        txtArrow.TextContents[2][1].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[2][1].DataSource);
                    }
                    
                    #endregion
                    
                    break;

                case AltitudeUseType.RECOMMENDED:

                    #region 
                    
                    txtArrow.TextContents.RemoveRange(1, 2);

                    if (Legs.UpperLimitAltitudeUOM != PDM.UOM_DIST_VERT.FL)
                    {
                        txtArrow.TextContents[0][0].DataSource.Value = "LowerLimitAltitude";
                        //txtArrow.TextContents[0][0].Font.UnderLine = true;
                        txtArrow.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][0].DataSource);

                        txtArrow.TextContents[0].RemoveRange(1, 1);
                    }
                    else
                    {
                        txtArrow.TextContents[0][0].DataSource.Value = "LowerLimitAltitudeUOM";
                        //txtArrow.TextContents[0][0].Font.UnderLine = true;
                        txtArrow.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][0].DataSource);

                        //txtArrow.TextContents[0][1].Font.UnderLine = true;
                        txtArrow.TextContents[0][1].DataSource.Value = "LowerLimitAltitude";
                        txtArrow.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][1].DataSource);

                    }

                    #endregion

                    break;

                case AltitudeUseType.EXPECT_LOWER:

                    #region 
                    
                    txtArrow.TextContents.RemoveRange(1, 2);

                    if (Legs.UpperLimitAltitudeUOM != PDM.UOM_DIST_VERT.FL)
                    {
                        txtArrow.TextContents[0][0].DataSource.Value = "LowerLimitAltitude";
                        //txtArrow.TextContents[0][0].Font.UnderLine = true;
                        txtArrow.TextContents[0][0].StartSymbol.Text = "EXPECT ";
                        txtArrow.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][0].DataSource);

                        txtArrow.TextContents[0].RemoveRange(1, 1);
                    }
                    else
                    {
                        txtArrow.TextContents[0][0].DataSource.Value = "LowerLimitAltitudeUOM";
                        //txtArrow.TextContents[0][0].Font.UnderLine = true;
                        txtArrow.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][0].DataSource);

                        //txtArrow.TextContents[0][1].Font.UnderLine = true;
                        txtArrow.TextContents[0][0].StartSymbol.Text = "EXPECT ";
                        txtArrow.TextContents[0][1].DataSource.Value = "LowerLimitAltitude";
                        txtArrow.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][1].DataSource);

                    }
                    #endregion

                    break;

                case AltitudeUseType.AS_ASSIGNED:
                case AltitudeUseType.OTHER:
                default:

                    break;
            }
            #endregion

            if (ProcName != null && txtArrow.TextContents.Count >= 1 && !txtArrow.TextContents[0][0].TextValue.Trim().StartsWith("NaN"))
            {
                List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
                AncorChartElementWord wrd = new AncorChartElementWord(ProcName, txtArrow.Font);//создаем слово
               
                txtLine.Add(wrd);
                txtArrow.TextContents.Insert(0,txtLine);
            }

            #region FormatElement
            if (txtArrow.TextContents.Count == 3 && txtArrow.TextContents[1][0].TextValue.StartsWith("__"))
            {
                txtArrow.Leading = -3;
                txtArrow.TextContents[1][0].TextPosition = textPosition.Superscript;
            }
            #endregion

            if (txtArrow.TextContents[0][0].TextValue.Contains("/")) WrapText(ref txtArrow);


            #region new

            if (Legs.Geo == null) Legs.RebuildGeo();

            //if (Legs.Geo != null)
            
                Polyline PolyLn = new PolylineClass();
                IPointCollection RealShape = Legs.Geo as IPointCollection;

                IPoint startPoint = RealShape.get_Point(0);
                IPoint endPoint = RealShape.get_Point(RealShape.PointCount - 1);
                {
                    (PolyLn as IPointCollection).AddPointCollection(RealShape);
                }

                ILine ln = new LineClass();
                ln.FromPoint = startPoint;
                ln.ToPoint = endPoint;

                double angl = ChartElementsManipulator.GetLineSlopE(ln, FocusMap, pSpatialReference) * 180 / Math.PI;
                angl = angl % 360;
                angl = angl < 0 ? angl + 360 : angl;


                if (angl > 90 && angl <= 270)
                {
                    ln.FromPoint = endPoint;
                    ln.ToPoint = startPoint;

                    angl = ChartElementsManipulator.GetLineSlopE(ln, FocusMap, pSpatialReference) * 180 / Math.PI;
                }

                #endregion

                if (Legs.EndPoint == null)
                {
                    // если лег прерыватеся не на waypoint, например на высоте, тогда необходимо создать фиктивную точку и записать ее в слой DPN, чтобы аннотации было вокруг чего двигаться
                    //    // устанавливаем ьакой признак
                    chartEl.Tag = "fix";
                }
            txtArrow.Slope = txtArrow.Anchor.X == 0 && txtArrow.Anchor.Y == 0 ? angl : 0;


            endPoint = new PointClass {X = endPoint.X + txtArrow .Anchor.X, Y = endPoint.Y + txtArrow.Anchor.Y };

            txtArrow.Anchor = new AncorPoint(endPoint.X, endPoint.Y);
            
            IElement el_SegPnt = null;
            el_SegPnt = (IElement)txtArrow.ConvertToIElement();

            if (el_SegPnt is IGroupElement)
            {
                IGroupElement GrEl = el_SegPnt as IGroupElement;
                for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                {
                    GrEl.get_Element(i).Geometry = endPoint as IGeometry;
                }
            }
            else el_SegPnt.Geometry = endPoint;


            if (!Double.IsNaN(_LowerLimitAltitude)) Legs.LowerLimitAltitude = _LowerLimitAltitude;
            if (!Double.IsNaN(_UpperLimitAltitude)) Legs.UpperLimitAltitude = _UpperLimitAltitude;

            return el_SegPnt;


        }


        public static IElement CreateSegmentPointLegHeightAnno_AlongPoint(ProcedureLeg Legs, AbstractChartElement chartEl, string vertUom, string distUom, string ProcName = null)
        {

            ChartElement_TextArrow txtArrow = (ChartElement_TextArrow)chartEl;

            #region  format elemnet


            switch (Legs.AltitudeInterpretation)
            {
                case AltitudeUseType.ABOVE_LOWER:

                    #region 

                    txtArrow.TextContents.RemoveRange(1, 2);

                    if (Legs.UpperLimitAltitudeUOM != PDM.UOM_DIST_VERT.FL)
                    {
                        txtArrow.TextContents[0][0].DataSource.Value = "LowerLimitAltitude";
                        txtArrow.TextContents[0][0].Font.UnderLine = true;
                        txtArrow.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][0].DataSource);

                        txtArrow.TextContents[0].RemoveRange(1, 1);
                    }
                    else
                    {
                        txtArrow.TextContents[0][0].DataSource.Value = "LowerLimitAltitudeUOM";
                        txtArrow.TextContents[0][0].Font.UnderLine = true;
                        txtArrow.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][0].DataSource);

                        txtArrow.TextContents[0][1].Font.UnderLine = true;
                        txtArrow.TextContents[0][1].DataSource.Value = "LowerLimitAltitude";
                        txtArrow.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][1].DataSource);

                    }

                    #endregion

                    break;

                case AltitudeUseType.BELOW_UPPER:

                    #region 

                    txtArrow.TextContents.RemoveRange(2, 1);
                    txtArrow.TextContents[0][0].Font.UnderLine = true;
                    txtArrow.TextContents[0][0].TextValue = "______";
                    txtArrow.TextContents[0][0].TextPosition = ANCOR.MapCore.textPosition.Subscript;
                    txtArrow.TextContents[0].RemoveRange(1, 1);

                    if (Legs.UpperLimitAltitudeUOM != PDM.UOM_DIST_VERT.FL)
                    {
                        txtArrow.TextContents[1][0].DataSource.Value = "UpperLimitAltitude";
                        txtArrow.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][0].DataSource);
                        txtArrow.TextContents[1].RemoveRange(1, 1);
                    }
                    else
                    {
                        txtArrow.TextContents[1][0].DataSource.Value = "UpperLimitAltitudeUOM";
                        txtArrow.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][0].DataSource);
                        txtArrow.TextContents[1][1].DataSource.Value = "UpperLimitAltitude";
                        txtArrow.TextContents[1][1].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][1].DataSource);
                    }

                    #endregion


                    break;

                case AltitudeUseType.AT_LOWER:

                    #region 

                    txtArrow.TextContents.RemoveRange(2, 1);
                    txtArrow.TextContents[0][0].Font.UnderLine = true;
                    txtArrow.TextContents[0][0].TextValue = "______";
                    txtArrow.TextContents[0][0].TextPosition = ANCOR.MapCore.textPosition.Subscript;
                    txtArrow.TextContents[0].RemoveRange(1, 1);

                    if (Legs.UpperLimitAltitudeUOM != PDM.UOM_DIST_VERT.FL)
                    {
                        txtArrow.TextContents[1][0].DataSource.Value = "LowerLimitAltitude";
                        txtArrow.TextContents[1][0].Font.UnderLine = true;
                        txtArrow.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][0].DataSource);
                        txtArrow.TextContents[1].RemoveRange(1, 1);
                    }
                    else
                    {
                        txtArrow.TextContents[1][0].DataSource.Value = "LowerLimitAltitudeUOM";
                        txtArrow.TextContents[1][0].Font.UnderLine = true;
                        txtArrow.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][0].DataSource);

                        txtArrow.TextContents[1][1].DataSource.Value = "LowerLimitAltitude";
                        txtArrow.TextContents[1][1].Font.UnderLine = true;
                        txtArrow.TextContents[1][1].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][1].DataSource);
                    }

                    #endregion


                    break;

                case AltitudeUseType.BETWEEN:

                    #region 

                    txtArrow.TextContents[0][0].Font.UnderLine = true;
                    txtArrow.TextContents[0][0].TextValue = "______";
                    txtArrow.TextContents[0][0].TextPosition = ANCOR.MapCore.textPosition.Subscript;
                    txtArrow.TextContents[0].RemoveRange(1, 1);

                    if (Legs.UpperLimitAltitudeUOM != PDM.UOM_DIST_VERT.FL)
                    {

                        txtArrow.TextContents[1][0].DataSource.Value = "UpperLimitAltitude";
                        txtArrow.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][0].DataSource);
                        txtArrow.TextContents[1].RemoveRange(1, 1);

                        txtArrow.TextContents[2][0].DataSource.Value = "LowerLimitAltitude";
                        txtArrow.TextContents[2][0].Font.UnderLine = true;
                        txtArrow.TextContents[2][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[2][0].DataSource);
                        txtArrow.TextContents[2].RemoveRange(1, 1);

                        if (txtArrow.TextContents[1][0].TextValue.CompareTo(txtArrow.TextContents[2][0].TextValue) == 0)
                        {
                            txtArrow.TextContents.Remove(txtArrow.TextContents[1]);
                        }
                        else
                            txtArrow.Leading = -0.7;

                    }
                    else
                    {
                        txtArrow.TextContents[1][0].DataSource.Value = "UpperLimitAltitude";
                        txtArrow.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][0].DataSource);
                        txtArrow.TextContents[1][1].DataSource.Value = "UpperLimitAltitudeUOM";
                        txtArrow.TextContents[1][1].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[1][1].DataSource);

                        txtArrow.TextContents[2][0].DataSource.Value = "LowerLimitAltitude";
                        txtArrow.TextContents[2][0].Font.UnderLine = true;
                        txtArrow.TextContents[2][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[2][0].DataSource);
                        txtArrow.TextContents[2][1].DataSource.Value = "LowerLimitAltitudeUOM";
                        txtArrow.TextContents[2][1].Font.UnderLine = true;
                        txtArrow.TextContents[2][1].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[2][1].DataSource);
                    }

                    #endregion

                    break;

                case AltitudeUseType.RECOMMENDED:

                    #region 

                    txtArrow.TextContents.RemoveRange(1, 2);

                    if (Legs.UpperLimitAltitudeUOM != PDM.UOM_DIST_VERT.FL)
                    {
                        txtArrow.TextContents[0][0].DataSource.Value = "LowerLimitAltitude";
                        //txtArrow.TextContents[0][0].Font.UnderLine = true;
                        txtArrow.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][0].DataSource);

                        txtArrow.TextContents[0].RemoveRange(1, 1);
                    }
                    else
                    {
                        txtArrow.TextContents[0][0].DataSource.Value = "LowerLimitAltitudeUOM";
                        //txtArrow.TextContents[0][0].Font.UnderLine = true;
                        txtArrow.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][0].DataSource);

                        //txtArrow.TextContents[0][1].Font.UnderLine = true;
                        txtArrow.TextContents[0][1].DataSource.Value = "LowerLimitAltitude";
                        txtArrow.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][1].DataSource);

                    }

                    #endregion

                    break;

                case AltitudeUseType.EXPECT_LOWER:

                    #region 

                    txtArrow.TextContents.RemoveRange(1, 2);

                    if (Legs.UpperLimitAltitudeUOM != PDM.UOM_DIST_VERT.FL)
                    {
                        txtArrow.TextContents[0][0].DataSource.Value = "LowerLimitAltitude";
                        //txtArrow.TextContents[0][0].Font.UnderLine = true;
                        txtArrow.TextContents[0][0].StartSymbol.Text = "EXPECT ";
                        txtArrow.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][0].DataSource);

                        txtArrow.TextContents[0].RemoveRange(1, 1);
                    }
                    else
                    {
                        txtArrow.TextContents[0][0].DataSource.Value = "LowerLimitAltitudeUOM";
                        //txtArrow.TextContents[0][0].Font.UnderLine = true;
                        txtArrow.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][0].DataSource);

                        //txtArrow.TextContents[0][1].Font.UnderLine = true;
                        txtArrow.TextContents[0][0].StartSymbol.Text = "EXPECT ";
                        txtArrow.TextContents[0][1].DataSource.Value = "LowerLimitAltitude";
                        txtArrow.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(Legs, txtArrow.TextContents[0][1].DataSource);

                    }
                    #endregion

                    break;

                case AltitudeUseType.AS_ASSIGNED:
                case AltitudeUseType.OTHER:
                default:

                    break;
            }
            #endregion

            if (ProcName != null && txtArrow.TextContents.Count >= 1 && !txtArrow.TextContents[0][0].TextValue.Trim().StartsWith("NaN"))
            {
                List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
                AncorChartElementWord wrd = new AncorChartElementWord(ProcName, txtArrow.Font);//создаем слово

                txtLine.Add(wrd);
                txtArrow.TextContents.Insert(0, txtLine);
            }

            #region FormatElement
            if (txtArrow.TextContents.Count == 3 && txtArrow.TextContents[1][0].TextValue.StartsWith("__"))
            {
                txtArrow.Leading = -3;
                txtArrow.TextContents[1][0].TextPosition = textPosition.Superscript;
            }
            #endregion

            if (txtArrow.TextContents[0][0].TextValue.Contains("/")) WrapText(ref txtArrow);

            IElement el_SegPnt = null;
            IPoint pnt = new PointClass();
            IGeometry gm = null;
            if (Legs.EndPoint != null) { Legs.EndPoint.RebuildGeo(); }
            if (Legs.EndPoint != null && Legs.EndPoint.Geo != null) { gm = Legs.EndPoint.Geo; }
            else
            {
                if (Legs.Geo == null) Legs.RebuildGeo2();
                gm = ((IPointCollection)Legs.Geo).get_Point(((IPointCollection)Legs.Geo).PointCount - 1);

                // если лег прерыватеся не на waypoint, например на высоте, тогда необходимо создать фиктивную точку и записать ее в слой DPN, чтобы аннотации было вокруг чего двигаться
                // устанавливаем ьакой признак
                chartEl.Tag = "fix";

            }

            pnt.PutCoords(((IPoint)gm).X - txtArrow.Anchor.X, ((IPoint)gm).Y - txtArrow.Anchor.Y);

            txtArrow.Anchor = new AncorPoint(((IPoint)gm).X, ((IPoint)gm).Y);


            el_SegPnt = (IElement)txtArrow.ConvertToIElement();

            if (el_SegPnt is IGroupElement)
            {
                IGroupElement GrEl = el_SegPnt as IGroupElement;
                for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                {
                    GrEl.get_Element(i).Geometry = pnt as IGeometry;
                }
            }
            else el_SegPnt.Geometry = pnt;


            return el_SegPnt;


        }


        public static string GetTargetText(string target_prop, AirportHeliport selARP, List<RunwayDirection> selRwyDir, List<PDMObject> list, string vertUom, string distUom, int RNP = -1)
        {
            string res = "";

            try
            {
                switch (target_prop)
                {
                    //case ("Sigma_AirportHeliport_TransitionAltitude"):

                    //    string uom = selARP.TransitionAltitudeUOM.ToString();
                    //    //double val = ArenaStaticProc.UomTransformation(uom, vertUom.ToString(), selARP.TransitionAltitude.Value, 0);
                    //    double val = selARP.TransitionAltitude.Value;
                    //    res = val.ToString() + " " + selARP.TransitionAltitudeUOM.ToString();//ArenaStaticProc.GetObjectValueAsString(selARP, "TransitionAltitude");

                    //    break;
                    case ("Sigma_AirportDesignator"):
                        //res = ArenaStaticProc.GetObjectValueAsString(selARP, "ServedCity") + "/" + ArenaStaticProc.GetObjectValueAsString(selARP, "Name") + " (" + ArenaStaticProc.GetObjectValueAsString(selARP, "Designator") + ")";
                        res = ArenaStaticProc.GetObjectValueAsString(selARP, "Name") + " (" + ArenaStaticProc.GetObjectValueAsString(selARP, "Designator") + ")";
                        break;
                    case ("Sigma_AirportHeliport_Frequency"):
                        //res = ArenaStaticProc.GetObjectValueAsString(targetObj, "TransitionAltitude");
                        break;
                    case ("Sigma_LandingArea"):
                        res = (RNP > 0) ? res + "RNP RWY " : res + "RWY ";
                        foreach (var rwy in selRwyDir)
                        {
                            res = (RNP > 0)? res + " " + ArenaStaticProc.GetObjectValueAsString(rwy, "Designator") :   res + " " + ArenaStaticProc.GetObjectValueAsString(rwy, "Designator");
                            res = res + "/";
                        }

                        res = res.Remove(res.Length - 1, 1);
                        break;
                    case ("Sigma_ProceduresList"):
                        int i = 0;
                        list = list.OrderBy(name => name.GetObjectLabel()).ToList();
                        res = "";
                        foreach (var item in list)
                        {
                            //if (i < 3)
                            //    res = res + ArenaStaticProc.GetObjectValueAsString(item, "ProcedureIdentifier") + ", ";
                            //else
                            //{
                            //    res = res + System.Environment.NewLine + ArenaStaticProc.GetObjectValueAsString(item, "ProcedureIdentifier") + ", ";
                            //    i = 0;
                            //}
                            string procName = ArenaStaticProc.GetObjectValueAsString(item, "ProcedureIdentifier");
                            string[] words = procName.Split(' ');
                            string procStart = words[0];

                            if (res.Contains(procStart))
                                res = res + "/" + words[1];
                            else
                            {
                                res = res.Length > 0 ? res + ", " + ArenaStaticProc.GetObjectValueAsString(item, "ProcedureIdentifier")  : res + ArenaStaticProc.GetObjectValueAsString(item, "ProcedureIdentifier");
                            }

                            i++;
                        }
                        if (res.EndsWith(","))
                        res = res.Remove(res.Length - 1, 1);
                        
                        break;

                    
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            if (res == null) res = "";
            return res;

        }

        public static void SaveRWY_ChartGeo(IFeatureClass AnnoRWYGeo_featClass, IGeometry rwyGeo, string rwyDesignator)
        {
            IFeature pFeat = AnnoRWYGeo_featClass.CreateFeature();

            int fIndx = pFeat.Fields.FindField("SHAPE"); 
            if (fIndx > 0) pFeat.set_Value(fIndx, rwyGeo);

            fIndx = pFeat.Fields.FindField("txtName");
            if (fIndx > 0) pFeat.set_Value(fIndx, rwyDesignator);

            pFeat.Store();
            Application.DoEvents();
        }

        public static void SaveFacilityMakeUp_Geo(IFeatureClass AnnoFacilitymakeUpGeo_featClass, string facilityMakeUpID, IGeometry gm)
        {

            IZAware zAware = (IZAware)gm;
            zAware.ZAware = false;
            IMAware mAware = (IMAware)gm;
            mAware.MAware = false;


            IFeature pFeat = AnnoFacilitymakeUpGeo_featClass.CreateFeature();


            int fIndx = pFeat.Fields.FindField("FeatureGUID");
            if (fIndx > 0) pFeat.set_Value(fIndx, facilityMakeUpID);
            
            fIndx = pFeat.Fields.FindField("SHAPE");
            if (fIndx > 0) pFeat.set_Value(fIndx, gm);

            pFeat.Store();
            Application.DoEvents();
        }

        public static void SaveGP_ChartGeo(IFeatureClass AnnoGPGeo_featClass, NavaidComponent cmpnt, AirportHeliport selARP, RunwayDirection selRwyDir)
        {
            if (cmpnt.Geo == null) cmpnt.RebuildGeo();

            IFeature pFeat = AnnoGPGeo_featClass.CreateFeature();

            int fIndx = pFeat.Fields.FindField("SHAPE");
            if (fIndx > 0) pFeat.set_Value(fIndx, cmpnt.Geo);

            fIndx = pFeat.Fields.FindField("angle");
            if (fIndx > 0 && selRwyDir.TrueBearing.HasValue)
            {
                double angle = selRwyDir.TrueBearing.Value + 180;
                angle = angle > 360 ? angle - 360 : angle;
                pFeat.set_Value(fIndx, angle);//pFeat.set_Value(fIndx, ((GlidePath)cmpnt).Angle);
            }
            fIndx = pFeat.Fields.FindField("relADHP");
            if (fIndx > 0) pFeat.set_Value(fIndx, selARP.Designator);

            fIndx = pFeat.Fields.FindField("relRWY");
            if (fIndx > 0) pFeat.set_Value(fIndx, selRwyDir.Designator);

            pFeat.Store();
            Application.DoEvents();
        }

        public static IAOIBookmark CreateBookmark(IMap _FocusMap, string bookmarkName)
        {

            IActiveView activeView = (IActiveView)_FocusMap;

            IAOIBookmark _bookmark = new AOIBookmarkClass();

            _bookmark.Location = activeView.Extent;
            _bookmark.Name = bookmarkName;

            return _bookmark;
        }

        public static AbstractChartElement getPrototypeChartElement(List<AbstractChartElement> _prototype_anno_lst, string PrototypeElementName)
        {
            //AbstractChartElement res = _prototype_anno_lst.FirstOrDefault(x => x.Name.StartsWith(PrototypeElementName)).Clone() as AbstractChartElement;
            AbstractChartElement res = _prototype_anno_lst.FirstOrDefault(x => x.Name.StartsWith(PrototypeElementName)) as AbstractChartElement;
            if (res != null) return res.Clone() as AbstractChartElement;
            else return null;
        }

        public static double CheckedAngle(double angle, ref bool RollFlag)
        {
            double res = angle % 360;
            if (res < 0) res = res + 360;


            if (res > 90 && res <= 270)
            {
                res = res + 180;
                if (res > 360) res = res - 360;
                RollFlag = true;
            }

            return res;
        }

        public static IEnvelope GetLayerExtent(IFeatureLayer2 _Layer, string _DefinitionExpression)
        {

            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = _DefinitionExpression;
            IFeatureCursor featureCursor = _Layer.Search(queryFilter, false);


            IFeature feature;
            IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();

            while ((feature = featureCursor.NextFeature()) != null)
            {
                if (feature.Shape == null) continue;
                IGeometry geometry = feature.Shape;
                IEnvelope featureExtent = geometry.Envelope;
                envelope.Union(featureExtent);
            }

            Marshal.ReleaseComObject(featureCursor);

            return envelope;
        }

        public static List<PDMObject> BuildMSAList(AirportHeliport airportHeliport)
        {
            List<PDMObject>  msa_list = new List<PDMObject>();

            var list_safeArea = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                 where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.SafeAltitudeArea) &&
                                     (((SafeAltitudeArea)element).CentrePoint != null) &&
                                     (((SafeAltitudeArea)element).CentrePoint.PointChoice == PointChoice.AirportHeliport) &&
                                     (((SafeAltitudeArea)element).CentrePoint.Route_LEG_ID != null) &&
                                     (((SafeAltitudeArea)element).CentrePoint.Route_LEG_ID.StartsWith(airportHeliport.ID))
                                 select element).ToList();

            if (list_safeArea !=null && list_safeArea.Count > 0)
                msa_list.AddRange(list_safeArea);


            /////////////////////////////////////////////////////

            var nav = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(_navSys => _navSys.PDM_Type == PDM_ENUM.NavaidSystem && ((NavaidSystem)_navSys).ID_AirportHeliport != null && ((NavaidSystem)_navSys).ID_AirportHeliport.StartsWith(airportHeliport.ID)).ToList();
            if (nav != null && nav.Count > 0)
            {
                foreach (var item in nav)
                {

                    list_safeArea = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                     where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.SafeAltitudeArea) &&
                                        (((SafeAltitudeArea)element).CentrePoint != null) &&
                                        (((SafeAltitudeArea)element).CentrePoint.PointChoice == PointChoice.Navaid) &&
                                        (((SafeAltitudeArea)element).CentrePoint.Route_LEG_ID != null) &&
                                        (((SafeAltitudeArea)element).CentrePoint.Route_LEG_ID.StartsWith(item.ID))
                                     select element).ToList();

                    if (list_safeArea != null && list_safeArea.Count > 0)
                    {
                        foreach (var sa in list_safeArea)
                        {
                            if (msa_list.IndexOf(sa) < 0) msa_list.Add(sa);
                        }
                    }
                }
            }


            return msa_list;

        }

        public static string ToString(List<string> list)
        {
            string res = "";

            foreach (var item in list)
            {
                res = res + item + ",";
            }

            return res.Remove(res.Length-1,1);
        }

        public static double NormalizeSlope(double Slope)
        {
            double res = Slope < 0 ? Slope + 360 : Slope;

            if (res > 90 && res <= 270)
            {
                res += 180;
                res = res > 360 ? res - 360 : res;
            }

            return res;
        }

        public static void CreateAerodromeTable(string path, AirportHeliport selectedAirport)
        {
            Excel.Application excelDoc;
            Excel.Worksheet curWorksheet;
            Excel.Workbook curWorkbook;
            
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            excelDoc = new Excel.Application();
            curWorkbook = excelDoc.Workbooks.Add();

            #region Runway Direction

            curWorksheet = (Excel.Worksheet)curWorkbook.Worksheets.Item[1];
            curWorksheet.Name = "Runway Direction";
            curWorksheet.Cells[1, 1] = "Runway";
            curWorksheet.Cells[1, 2] = "Direction\n(Magnetic)";
            curWorksheet.Cells[1, 3] = "Threshold";
            curWorksheet.Cells[1, 4] = "Bearing\nStrength";
            curWorksheet.Cells.ColumnWidth = 15;
            curWorksheet.Cells.RowHeight = 30;

            int i = 2;
            if (selectedAirport.RunwayList != null)
            {
                foreach (var rwy in selectedAirport.RunwayList)
                {
                    foreach (var rwyDir in rwy.RunwayDirectionList)
                    {
                        curWorksheet.Cells[i, 1] = rwyDir.Designator;

                        var mag = rwyDir.MagBearing ?? rwyDir.TrueBearing - selectedAirport.MagneticVariation;
                        if (mag != null)
                            curWorksheet.Cells[i, 2] = mag;
                        else
                            curWorksheet.Cells[i, 2] = "-";

                        if (rwyDir.Lat != "" || rwyDir.Lon != "")
                        {
                            curWorksheet.Cells[i, 3] = ArenaStaticProc.LatToDDMMSS(rwyDir.Lat, coordtype.DDMMSS_SS_2) +
                                                       "\n" +
                                                       ArenaStaticProc.LonToDDMMSS(rwyDir.Lon, coordtype.DDMMSS_SS_2);
                        }
                        else
                        {
                            curWorksheet.Cells[i, 3] = "-";
                        }
                        string bearing = CreateSurfProp(rwy.SurfaceProperties);

                        curWorksheet.Cells[i, 4] = bearing;

                        i++;
                    }
                }
            }

            #endregion

            #region Aprons Surface

            curWorksheet = curWorkbook.Worksheets.Add();
            curWorksheet.Name = "Apron";
            curWorksheet.Cells[1, 1] = "APRONS SURFACE & STRENGTH";

            curWorksheet.Cells[1, 1].ColumnWidth = 40;
            curWorksheet.Cells.RowHeight = 15;

            curWorksheet.Cells.Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            i = 2;
            if (selectedAirport.ApronList != null)
            {
                foreach (var apron in selectedAirport.ApronList)
                {
                    if (apron.SurfaceProperties != null)
                    {
                        string prop = "";
                        prop += apron.Name != "" ? apron.Name : "-";
                        prop += "  -";
                        prop += apron.SurfaceProperties.Composition != CodeSurfaceCompositionType.OTHER
                            ? apron.SurfaceProperties.Composition.ToString()
                            : "-";
                        prop += ", ";
                        prop += CreateSurfProp(apron.SurfaceProperties);
                        curWorksheet.Cells[i, 1] = prop;

                        i++;
                    }
                }
            }

            #endregion

            #region Runway

            curWorksheet = curWorkbook.Worksheets.Add();
            curWorksheet.Name = "Runway";
            curWorksheet.Cells[1, 1].ColumnWidth = 15;
            curWorksheet.Cells[1, 2].ColumnWidth = 25;
            curWorksheet.Cells.RowHeight = 15;

            i = 1;
            if (selectedAirport.RunwayList != null)
            {
                foreach (var rwy in selectedAirport.RunwayList)
                {
                    curWorksheet.Cells[i, 1] = "RWY " + rwy.Designator;

                    curWorksheet.Cells[++i, 1] = "WIDTH";
                    string width = "- ";
                    width += rwy.Width != null ? rwy.Width.ToString() : "-";
                    curWorksheet.Cells[i, 2] = width;

                    if (rwy.SurfaceProperties != null)
                    {
                        curWorksheet.Cells[++i, 1] = "SURFACE";
                        string surf = "- ";
                        surf += rwy.SurfaceProperties.Composition != CodeSurfaceCompositionType.OTHER
                            ? rwy.SurfaceProperties.Composition.ToString()
                            : "-";
                        curWorksheet.Cells[i, 2] = surf;

                        curWorksheet.Cells[++i, 1] = "STRENGTH";
                        string prop = "- PCN ";

                        prop += CreateSurfProp(rwy.SurfaceProperties);

                        curWorksheet.Cells[i, 2] = prop;
                    }
                    i += 2;
                }
            }

            #endregion

            #region Vor Check Point (Legend)

            curWorksheet = curWorkbook.Worksheets.Add();
            curWorksheet.Name = "VOR(Legend)";

            curWorksheet.Cells[1, 1].ColumnWidth = 20;
            curWorksheet.Cells[1, 2].ColumnWidth = 5;
            curWorksheet.Cells[1, 3].ColumnWidth = 5;
            curWorksheet.Range["C1", "C10"].Font.Name = "AeroSigma";
            curWorksheet.Range["C1", "C10"].Font.Size = 16;
            curWorksheet.Cells.RowHeight = 30;

            curWorksheet.Cells[1, 1] = "VOR CHECK POINT\nAND FREQUENCY";
            List<VOR> vorList = new List<VOR>();
            i = 1;
            var checkpointVorList = selectedAirport.NavSystemCheckpoints.Where(c => c is CheckpointVOR);
            if (checkpointVorList.Any())
            {
                foreach (var checkVor in checkpointVorList)
                {
                    var vor = (VOR)DataCash.GetNavaidComponentByID(((CheckpointVOR)checkVor).ID_VOR);
                    if (vor == null)
                        continue;
                    if (!vorList.Contains(vor))
                        vorList.Add(vor);
                }
            }
            if (vorList != null)
            {
                foreach (var vor in vorList)
                {
                    if (vor.Designator != null && vor.Frequency != null)
                    {
                        curWorksheet.Cells[i, 1] = "VOR CHECK POINT AND FREQUENCY";
                        curWorksheet.Cells[i, 2] = vor.Designator + "\n" + vor.Frequency;
                        curWorksheet.Cells[i, 3] = "P";

                        i++;
                    }
                }
            }

            #endregion

            #region Taxiways

            curWorksheet = curWorkbook.Worksheets.Add();
            curWorksheet.Name = "Taxiways";
            curWorksheet.Cells[1, 1].ColumnWidth = 15;
            curWorksheet.Cells[1, 2].ColumnWidth = 30;

            i = 2;
            curWorksheet.Cells[1, 1] = "TAXIWAYS WIDTH & STRENGTH";
            if (selectedAirport.TaxiwayList != null)
            {
                foreach (var taxiway in selectedAirport.TaxiwayList)
                {
                    if (taxiway.SurfaceProperties != null)
                    {
                        var wordList = taxiway.Designator.Split(' ').ToList();
                        if (wordList[0] == "TWY")
                            wordList.Remove(wordList[0]);

                        if (wordList[0].Any(char.IsDigit))
                            continue;

                        curWorksheet.Cells[i, 1] = string.Join(" ", wordList.ToArray()) ?? "-";
                        string surf = "- ";
                        surf += taxiway.Width != null ? taxiway.Width.ToString() + taxiway.WidthUom : "-";
                        surf += ", ";
                        surf += "PCN ";
                        surf += CreateSurfProp(taxiway.SurfaceProperties);
                        curWorksheet.Cells[i, 2] = surf;
                        i++;
                    }
                }
            }

            #endregion

            #region AircraftStand

            curWorksheet = curWorkbook.Worksheets.Add();
            curWorksheet.Name = "AircraftStand";

            curWorksheet.Cells[1, 1].ColumnWidth = 10;
            curWorksheet.Cells[1, 2].ColumnWidth = 20;

            i = 1;
            if (selectedAirport.ApronList != null)
            {
                foreach (var apron in selectedAirport.ApronList)
                {
                    if(apron.ApronElementList==null)
                        continue;
                    foreach (var apronElement in apron.ApronElementList)
                    {
                        if(apronElement.AircrafrStandList==null)
                            continue;
                        foreach (var aircraftStand in apronElement.AircrafrStandList)
                        {
                            if(aircraftStand.Lat=="" ||aircraftStand.Lon=="")
                                continue;
                            curWorksheet.Cells[i, 1] = aircraftStand.Designator ?? "-";
                            curWorksheet.Cells[i, 2] =
                                ArenaStaticProc.LatToDDMMSS(aircraftStand.Lat, coordtype.DDMMSS_SS_2) +
                                " " +
                                ArenaStaticProc.LonToDDMMSS(aircraftStand.Lon, coordtype.DDMMSS_SS_2);
                            i++;

                        }
                    }
                }
            }

            #endregion

            curWorkbook.SaveAs(path);
            curWorkbook.Close();

        }

        public static string CreateSurfProp(SurfaceCharacteristics prop)
        {
            string surf = "";
            if (prop == null)
                return "-";

            surf += prop.ClassPCN != null
                ? prop.ClassPCN.ToString()
                : "-";
            surf += "/";
            surf += prop.PavementTypePCN != CodePCNPavementType.OTHER
                ? prop.PavementTypePCN.ToString().Substring(0, 1)
                : "-";
            surf += "/";
            surf += prop.PavementSubgradePCN != CodePCNSubgradeType.OTHER
                ? prop.PavementSubgradePCN.ToString().Substring(0, 1)
                : "-";
            surf += "/";
            surf += prop.MaxTyrePressurePCN != CodePCNTyrePressureType.OTHER
                ? prop.MaxTyrePressurePCN.ToString().Substring(0, 1)
                : "-";
            surf += "/";
            surf += prop.EvaluationMethodPCN != CodePCNMethodType.OTHER
                ? prop.EvaluationMethodPCN.ToString().Substring(0, 1)
                : "-";

            return surf;
        }
    }
    
}
