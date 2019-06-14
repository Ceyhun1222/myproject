using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ARENA.Project;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;
using PDM;
using PDM.PropertyExtension;
using Path = System.IO.Path;
using ArenaStatic;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ARENA.Enums_Const;
using DataModule;
using ArenaLogManager;

namespace ARENA.Environment
{

    public class Environment
    {
        #region TreeView and its Events

        private void TreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            if (Data.CurrentProject!=null)
            {
                Data.CurrentProject.TreeViewAfterSelect(sender,e);

            }
        }

        private void TreeViewAfterCheck(object sender, TreeViewEventArgs e)
        {
            if (Data.CurrentProject != null)
            {
                Data.CurrentProject.TreeViewAfterCheck(sender, e);
            }
        }

        //private void MapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        //{
        //    if (Data.CurrentProject != null)
        //    {
        //        Data.CurrentProject.MapControl_OnMouseDown(sender, e);
        //    }
        //}

        private void InitTreeViewEvents()
        {
            FeatureTreeView.AfterSelect += TreeViewAfterSelect;
            FeatureTreeView.AfterCheck += TreeViewAfterCheck;
        }

        private void InitMapControlEvents()
        {
            //MapControl.OnMouseDown += MapControl_OnMouseDown;

        }



        private TreeView _featureTreeView;
        public TreeView FeatureTreeView
        {
            get { return _featureTreeView; }
            set { 
                _featureTreeView = value;
                InitTreeViewEvents();
            }
        }

        #endregion

        #region UI properties

        private IApplication m_application;
        public IApplication mxApplication
        {
            get { return m_application; }
            set { m_application = value; }
        }

        private IMap _mapControl;
        public IMap pMap
        {
            get { return _mapControl; }
            set
            {
                _mapControl = value;
                //mapControl = (IMap)MapControl.Object;
                InitMapControlEvents();
            }
        }

        public ImageList TreeViewImageList { get; set; }

        #region  удалить блок

        public ReadOnlyPropertyGrid ReadOnlyPropertyGrid { get; set; }
        //public IMapControl3 mapControl { get; set; }
        
        public ContextMenuStrip FeatureTreeViewContextMenuStrip;
        public ToolStrip EnvironmentToolStrip;
        public ToolStrip FeatureTreeViewToolStrip;
        public ContextMenuStrip mapControlContextMenuStrip;
        public StatusStrip statusStrip;
        public ToolStripButton PlayButton;
        public MenuStrip MaimMenu;
        public Panel PandaToolBox;
        #endregion
        
        #endregion

        #region Data

        private EnvironmentData _data;
        public EnvironmentData Data
        {
            get { return _data??(_data=new EnvironmentData { Environment = this } ); }
        }

        #endregion
        
        #region Logic

        public void ZoomToLayerByName(string name)
        {

            var zoomLayer = EsriUtils.getLayerByName(pMap, name);

            ((IActiveView)pMap).Extent = zoomLayer.AreaOfInterest;
            ((IActiveView)pMap).Refresh();

        }

        public void SetCenter_and_Projection(ArenaProjectType prjType = ArenaProjectType.ARENA)
        {
            string zoomedLayerName = "";
            ILayer _Layer = null;

            switch (prjType)
            {
                case ArenaProjectType.ARENA:
                case ArenaProjectType.NOTAM:
                case ArenaProjectType.PANDA:
                case ArenaProjectType.SIGMA:

                    zoomedLayerName = "AirportHeliport";
                    _Layer = EsriUtils.getLayerByName(pMap, zoomedLayerName);

                    if (IsEmptyLayer(_Layer))
                    {
                        zoomedLayerName = "AirspaceVolume";
                        _Layer = EsriUtils.getLayerByName(pMap, zoomedLayerName);
                    }

                    if (IsEmptyLayer(_Layer))
                    {
                        zoomedLayerName = "WayPoint";
                        _Layer = EsriUtils.getLayerByName(pMap, zoomedLayerName);
                    }

                    if (IsEmptyLayer(_Layer))
                    {
                        zoomedLayerName = "AirportHeliport";
                        _Layer = EsriUtils.getLayerByName(pMap, zoomedLayerName);
                    }


                    if (_Layer != null)
                    {

                        ZoomToLayerByName(zoomedLayerName);

                    }

                    break;
                case ArenaProjectType.AERODROME:

                    zoomedLayerName = "RunwayElement"; 
                    _Layer = EsriUtils.getLayerByName(pMap, zoomedLayerName);

                    if (IsEmptyLayer(_Layer))
                    {
                        zoomedLayerName = "AirportHeliport";
                        _Layer = EsriUtils.getLayerByName(pMap, zoomedLayerName);
                    }
                    break;
                default:
                    break;
            }
            

           
        }

        private bool IsEmptyLayer(ILayer _Layer)
        {
            if (_Layer == null) return true;

            bool res = ((IFeatureLayer)_Layer).FeatureClass.FeatureDataset == null;

            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "OBJECTID >0";
            res = ((IFeatureLayer)_Layer).FeatureClass.FeatureCount(queryFilter) <=0;
             
            return res;
        }

        public void ClearGraphics()
        {
            try
            {
                var graphicsContainer = (IGraphicsContainer)pMap;

                graphicsContainer.Reset();

                var thisElement = graphicsContainer.Next();
                while (thisElement != null)
                {
                    var docElementProperties = thisElement as IElementProperties;
                    if (docElementProperties != null && docElementProperties.Name.StartsWith("ARENA"))
                    {
                        graphicsContainer.DeleteElement(thisElement);
                    }
                    thisElement = graphicsContainer.Next();
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name)
                    .Error(ex, "Errer in ClearGraphics method");

            }
        }

        public void FillGeo(PDMObject sellObj)
        {
            try
            {
                string tp = sellObj.GetType().Name;
                switch (tp)
                {
                    case ("AirportHeliport"):
                    case ("RunwayDirection"):
                    case ("VOR"):
                    case ("DME"):
                    case ("NDB"):
                    case ("TACAN"):
                    case ("Localizer"):
                    case ("GlidePath"):
                    case ("RouteSegment"):
                    case ("WayPoint"):
                    case ("AirspaceVolume"):
                    case ("VerticalStructurePart"):
                        sellObj.RebuildGeo();
                        break;
                    case ("NavaidSystem"):
                        foreach (PDMObject comp in (sellObj as NavaidSystem).Components)
                        {
                            comp.RebuildGeo();
                        }

                        break;
                    case ("Enroute"):
                        foreach (PDMObject rte in (sellObj as Enroute).Routes)
                        {
                            rte.RebuildGeo();
                        }
                        break;
                    case ("Airspace"):
                        foreach (AirspaceVolume vol in (sellObj as Airspace).AirspaceVolumeList)
                        {
                            if (vol.Geo == null) vol.RebuildGeo();
                        }
                        break;

                }

            }
            catch
            {
            }

        }

        public void FillAirtrackTableDic()
        {
            {
                //var lyr = pMap.get_Layer(0);
                var lyr = EsriUtils.getLayerByName(this.pMap, "AirportHeliport");
                if (lyr == null) lyr = EsriUtils.getLayerByName(this.pMap, "AirportCartography");
                if (lyr == null) return;
                var fc = ((IFeatureLayer)lyr).FeatureClass;
                if (fc != null)
                {
                    var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;
                    workspaceEdit.StopEditOperation();
                    workspaceEdit.StopEditing(false);


                    if (Data.TableDictionary.Count == 0)
                    {
                        FillAirtrackTableDic(workspaceEdit);
                    }
                }
            }
        }

        public void FillAirportDictionary()
        {
             var arpList = (from element in this.Data.PdmObjectList where (element != null) && (element is AirportHeliport) select element).ToList();

             foreach (AirportHeliport arp in arpList)
             {
                 if (!this.Data.AirdromeHeliportDictionary.ContainsKey(arp.ID)) this.Data.AirdromeHeliportDictionary.Add(arp.ID, arp);
             }

        }

        public void FillAirtrackTableDic(IWorkspaceEdit workspaceEdit)
        {
            try
            {
                Data.TableDictionary = new Dictionary<Type, ITable>();
                Dictionary<string, Type> tables = new Dictionary<string, Type>();
                switch (Data.CurrentProjectType)
                {
                    case ArenaProjectType.ARENA:
                        tables.Add("AirportHeliport", typeof(AirportHeliport));
                        tables.Add("Runway", typeof(Runway));
                        tables.Add("RunwayDirection", typeof(RunwayDirection));
                        tables.Add("RunwayDirectionCenterLinePoint", typeof(RunwayCenterLinePoint));
                        tables.Add("GlidePath", typeof(GlidePath));
                        tables.Add("Localizer", typeof(Localizer));
                        tables.Add("MarkerBeacon", typeof(Marker));
                        tables.Add("NDB", typeof(NDB));
                        tables.Add("VOR", typeof(VOR));
                        tables.Add("DME", typeof(DME));
                        tables.Add("TACAN", typeof(TACAN));
                        tables.Add("WayPoint", typeof(WayPoint));
                        tables.Add("NavaidSystem", typeof(NavaidSystem));
                        tables.Add("Procedures", typeof(Procedure));
                        tables.Add("InstrumentApproachProcedure", typeof(InstrumentApproachProcedure));
                        tables.Add("StandardInstrumentArrival", typeof(StandardInstrumentArrival));
                        tables.Add("StandardInstrumentDeparture", typeof(StandardInstrumentDeparture));
                        tables.Add("ProcedureTransitions", typeof(ProcedureTransitions));
                        tables.Add("ProcedureLegs", typeof(ProcedureLeg));
                        tables.Add("FinalLeg", typeof(FinalLeg));
                        tables.Add("MissaedApproachLeg", typeof(MissaedApproachLeg));
                        tables.Add("SegmentPoint", typeof(SegmentPoint));
                        //tables.Add("SegmentPoint", typeof(RouteSegmentPoint));
                        tables.Add("FacilityMakeUp", typeof(FacilityMakeUp));
                        tables.Add("AngleIndication", typeof(AngleIndication));
                        tables.Add("DistanceIndication", typeof(DistanceIndication));
                        tables.Add("Enroute", typeof(Enroute));
                        tables.Add("RouteSegment", typeof(RouteSegment));
                        tables.Add("HoldingPattern", typeof(HoldingPattern));
                        tables.Add("AirspaceVolume", typeof(AirspaceVolume));
                        tables.Add("Airspace", typeof(Airspace));
                        tables.Add("VerticalStructure", typeof(VerticalStructure));
                        tables.Add("VerticalStructurePart", typeof(VerticalStructurePart));
                        tables.Add("VerticalStructure_Point", typeof(PointClass));
                        tables.Add("VerticalStructure_Curve", typeof(LineClass));
                        tables.Add("VerticalStructure_Surface", typeof(PolygonClass));
                        tables.Add("Area", typeof(AREA_PDM));
                        tables.Add("SafeArea", typeof(SafeAltitudeArea));
                        tables.Add("SafeAreaSector", typeof(SafeAltitudeAreaSector));
                        tables.Add("GeoBorder", typeof(GeoBorder));                        
                        tables.Add("RunwayElement", typeof(RunwayElement));
                        tables.Add("Taxiway", typeof(Taxiway));
                        tables.Add("TaxiwayElement", typeof(TaxiwayElement));
                        tables.Add("GroundLightSystem", typeof(LightSystem));
                        tables.Add("LightElement", typeof(LightElement));
                        
                        break;
                    case ArenaProjectType.NOTAM:
                        break;
                    case ArenaProjectType.PANDA:
                        break;
                    case ArenaProjectType.SIGMA:
                        break;
                    case ArenaProjectType.AERODROME:
                        tables.Add("AirportHeliport", typeof(AirportHeliport));
                        tables.Add("Runway", typeof(Runway));
                        tables.Add("RunwayDirection", typeof(RunwayDirection));
                        tables.Add("RunwayDirectionCenterLinePoint", typeof(RunwayCenterLinePoint));
                        tables.Add("GlidePath", typeof(GlidePath));
                        tables.Add("Localizer", typeof(Localizer));
                        tables.Add("MarkerBeacon", typeof(Marker));
                        tables.Add("NDB", typeof(NDB));
                        tables.Add("VOR", typeof(VOR));
                        tables.Add("DME", typeof(DME));
                        tables.Add("TACAN", typeof(TACAN));
                        tables.Add("WayPoint", typeof(WayPoint));
                        tables.Add("NavaidSystem", typeof(NavaidSystem));
                        tables.Add("VerticalStructure", typeof(VerticalStructure));
                        tables.Add("VerticalStructurePart", typeof(VerticalStructurePart));
                        tables.Add("VerticalStructure_Point", typeof(PointClass));
                        tables.Add("VerticalStructure_Curve", typeof(LineClass));
                        tables.Add("VerticalStructure_Surface", typeof(PolygonClass));
                        tables.Add("RunwayElement", typeof(RunwayElement));
                        tables.Add("Taxiway", typeof(Taxiway));
                        tables.Add("TaxiwayElement", typeof(TaxiwayElement));
                        tables.Add("ApproachLightSystem", typeof(ApproachLightingSystem));
                        tables.Add("LightElement", typeof(LightElement));
                        tables.Add("Apron", typeof(Apron));
                        tables.Add("ApronMarking", typeof(ApronMarking));
                        tables.Add("Marking_Point", typeof(Marking_Point));
                        tables.Add("Marking_Curve", typeof(Marking_Curve));
                        tables.Add("Marking_Surface", typeof(Marking_Surface));
                        tables.Add("ApronElement", typeof(ApronElement));
                        tables.Add("ApronLightSystem", typeof(ApronLightSystem));
                        tables.Add("DeicingArea", typeof(DeicingArea));
                        tables.Add("DeicingAreaMarking", typeof(DeicingAreaMarking));
                        tables.Add("RunwayMarking", typeof(RunwayMarking));
                        tables.Add("TaxiwayMarking", typeof(TaxiwayMarking));
                        tables.Add("AircraftStand", typeof(AircraftStand));
                        tables.Add("AircraftStandExtent", typeof(AircraftStandExtent));
                        tables.Add("AirportHeliportExtent", typeof(AirportHeliportExtent));
                        tables.Add("StandMarking", typeof(StandMarking));
                        tables.Add("GuidanceLine", typeof(GuidanceLine));
                        tables.Add("GuidanceLineLightSystem", typeof(GuidanceLineLightSystem));
                        tables.Add("GuidanceLineMarking", typeof(GuidanceLineMarking));
                        tables.Add("RadioCommunicationChanel", typeof(RadioCommunicationChanel));
                        tables.Add("CheckpointVOR", typeof(CheckpointVOR));
                        tables.Add("CheckpointINS", typeof(CheckpointINS));
                        tables.Add("TaxiwayLightSystem", typeof(TaxiwayLightSystem));
                        tables.Add("RunwayProtectArea", typeof(RunwayProtectArea));
                        tables.Add("RunwayProtectAreaLightSystem", typeof(RunwayProtectAreaLightSystem));
                        tables.Add("RunwayVisualRange", typeof(RunwayVisualRange));
                        tables.Add("AirportHotSpot", typeof(AirportHotSpot));
                        tables.Add("TaxiHoldingPosition", typeof(TaxiHoldingPosition));
                        tables.Add("TaxiHoldingPositionLightSystem", typeof(TaxiHoldingPositionLightSystem));
                        tables.Add("TaxiHoldingPositionMarking", typeof(TaxiHoldingPositionMarking));
                        tables.Add("VisualGlideSlopeIndicator", typeof(VisualGlideSlopeIndicator));
                        tables.Add("TouchDownLiftOff", typeof(TouchDownLiftOff));
                        tables.Add("TouchDownLiftOffAimingPoint", typeof(TouchDownLiftOffAimingPoint));
                        tables.Add("TouchDownLiftOffMarking", typeof(TouchDownLiftOffMarking));
                        tables.Add("TouchDownLiftOffLightSystem", typeof(TouchDownLiftOffLightSystem));
                        tables.Add("TouchDownLiftOffSafeArea", typeof(TouchDownLiftOffSafeArea));
                        tables.Add("WorkArea", typeof(WorkArea));
                        tables.Add("SurfaceCharacteristics", typeof(SurfaceCharacteristics));
                        tables.Add("Road", typeof(Road));
                        tables.Add("Unit", typeof(Unit));
                        tables.Add("NonMovementArea", typeof(NonMovementArea));
                        tables.Add("AirspaceVolume", typeof(AirspaceVolume));
                        tables.Add("Airspace", typeof(Airspace));
                        tables.Add("RadioFrequencyArea", typeof(RadioFrequencyArea));

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                foreach (var table in tables)
                {
                    if (EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, table.Key) != null)
                    {
                        Data.TableDictionary.Add(table.Value,
                            EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit, table.Key));
                    }
                }
                
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name)
                    .Error(ex, "Error when filling Table Dictionaty. Project type: " + Data.CurrentProjectType);

            }

        }



        public bool CreateEmptyProject(ArenaProjectType projectType)
        {
            Data.CurrentProjectType = projectType;

            var pathToTemplateFile = ArenaStaticProc.GetPathToTemplateFile();
            try
            {

                if (Directory.Exists(pathToTemplateFile))
                {

                    //FeatureTreeView.Nodes.Clear();
                    var tempDirName = Path.GetTempPath();
                    var dInf = Directory.CreateDirectory(tempDirName + @"\PDM\" + Guid.NewGuid().ToString() + @"\");
                    tempDirName = dInf.FullName;

                    var pathToTemplateFileMxdSource = Path.Combine(pathToTemplateFile, "pdm.mxd");
                    var pathToTemplateFileMxdDest = Path.Combine(tempDirName, "pdm.mxd");

                    var pathToTemplateFileMdbSource = Path.Combine(pathToTemplateFile, "pdm.mdb");
                    var pathToTemplateFileMdbDest = Path.Combine(tempDirName, "pdm.mdb");

                    switch (projectType)
                    {
                        case (ArenaProjectType.ARENA):
                        case (ArenaProjectType.PANDA):
                            //FeatureTreeView.Nodes.Add(new TreeNode("ARP/RWY/RDN/ILS"));
                            //FeatureTreeView.Nodes.Add(new TreeNode("Navaids"));

                            pathToTemplateFileMxdSource = Path.Combine(pathToTemplateFile, "arena_PDM.mxd");
                            pathToTemplateFileMxdDest = Path.Combine(tempDirName, "arena_PDM.mxd");

                            break;

                        case (ArenaProjectType.NOTAM):
                            //FeatureTreeView.Nodes.Add(new TreeNode("Airspaces"));

                            pathToTemplateFileMxdSource = Path.Combine(pathToTemplateFile, "notam_PDM.mxd");
                            pathToTemplateFileMxdDest = Path.Combine(tempDirName, "notam_PDM.mxd");

                            break;
                        case (ArenaProjectType.AERODROME):
                            //FeatureTreeView.Nodes.Add(new TreeNode("Airspaces"));

                            pathToTemplateFileMxdSource = Path.Combine(pathToTemplateFile, "aerodrome.mxd");
                            pathToTemplateFileMxdDest = Path.Combine(tempDirName, "aerodrome.mxd");

                            pathToTemplateFileMdbSource = Path.Combine(pathToTemplateFile, "aerodrome.mdb");
                            pathToTemplateFileMdbDest = Path.Combine(tempDirName, "aerodrome.mdb");

                            break;
                    }

                    File.Copy(pathToTemplateFileMdbSource, pathToTemplateFileMdbDest, true);
                    File.Copy(pathToTemplateFileMxdSource, pathToTemplateFileMxdDest, true);

                    while (Directory.GetFiles(tempDirName).Length < 2) System.Threading.Thread.Sleep(1);

                    mxApplication.NewDocument(false, pathToTemplateFileMxdDest);
                    Application.DoEvents();


                    IMxDocument pNewDocument = (IMxDocument) mxApplication.Document;
                    Application.DoEvents();
                    pMap = pNewDocument.ActiveView.FocusMap;

                    var lyr = pMap.get_Layer(0);
                    var fc = ((IFeatureLayer) lyr).FeatureClass;

                    var workspaceEdit = (IWorkspaceEdit) fc.FeatureDataset.Workspace;

                    workspaceEdit.StopEditOperation();
                    workspaceEdit.StopEditing(false);


                    if (Data.TableDictionary.Count == 0)
                    {
                        FillAirtrackTableDic(workspaceEdit);
                    }

                    switch (projectType)
                    {
                        case (ArenaProjectType.ARENA):
                            Data.CurrentProject = new ArenaProject(this);
                            Data.CurrentProjectType = ArenaProjectType.ARENA;
                            break;

                        case (ArenaProjectType.AERODROME):
                            Data.CurrentProject = new ArenaProject(this);
                            Data.CurrentProjectType = ArenaProjectType.AERODROME;
                            break;

                        case (ArenaProjectType.NOTAM):
                            Data.CurrentProject = new NotamProject(this);
                            break;

                        case (ArenaProjectType.PANDA):
                            Data.CurrentProject = new PandaProject(this);
                            Data.CurrentProjectType = ArenaProjectType.PANDA;
                            break;
                    }

                    Data.CurrentProject.OnCreate();
                    ArenaStaticProc.SetTargetDB(tempDirName);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name)
                    .Error(ex, "Error when creating empty " + projectType + " project. Path: " + pathToTemplateFile);
                return false;
            }

        }

 
        public void SaveLog()
        {
            try
            {
                string filePath = System.IO.Path.GetDirectoryName(ArenaStaticProc.GetPathToARINCSpecificationFile()) + @"\ARENA_ResultsInfo.txt";
                if (File.Exists(filePath)) File.Delete(filePath);

                List<string> tmp = new List<string>();

               
                tmp.Add("Primary Info");
                foreach (var item in DataCash.ProjectEnvironment.Data.ProjectLog)
                {
                    tmp.Add(item);
                }
                


                #region Log


                tmp.Add("");
                tmp.Add("Results");

                var queryObjectsGroup = from MyGroup in DataCash.ProjectEnvironment.Data.PdmObjectList group MyGroup by new { pdm_Type = MyGroup.PDM_Type } into GroupOfObjects orderby GroupOfObjects.Key.pdm_Type select GroupOfObjects;

                string CurType = queryObjectsGroup.First().Key.pdm_Type.ToString(); ;
                string TypeNode = CurType;


                foreach (var ARINCGroup in queryObjectsGroup)
                {

                    tmp.Add(ARINCGroup.Key.pdm_Type.ToString() + " " + ARINCGroup.Count().ToString());

                    foreach (var item in ARINCGroup)
                    {
                        tmp.Add((char)9 + item.GetType().Name + (char)9 + item.GetObjectLabel() + (char)9 + item.ID  + (char)9 + item.SourceDetail );
                    }

                }

                #endregion


                #region ErrorLog

                tmp.Add("");
                tmp.Add("Errors");

                tmp.Add("Type" + (char)9 + "Name" + (char)9 + "ID" + (char)9 + "Message" + (char)9 + "Source");
                foreach (var item in this.Data.PdmObjectList)
                {
                    var lst = item.GetExceptionMessage();
                    if (lst != null)
                        tmp.AddRange(lst);
                }

                #endregion

                if (DataCash.ProjectEnvironment.Data.ProjectLog != null) DataCash.ProjectEnvironment.Data.ProjectLog.AddRange(tmp);

                System.IO.File.WriteAllLines(filePath, tmp.ToArray());

            }
            catch { }
        }

        #endregion

        public Environment()
        {
        }

        public Environment(IApplication arcView_MxApplication)
        {
            this.mxApplication = arcView_MxApplication;
            this.pMap = ((IMxDocument)m_application.Document).ActiveView.FocusMap;


            if (pMap.LayerCount <= 0)
            {
                MessageBox.Show("Load ARENA project", "Project not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ILayer _Layer = EsriUtils.getLayerByName(this.pMap, "AirportHeliport");
            if (_Layer == null)
            {
                MessageBox.Show("Load ARENA project", "Project not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var fc = ((IFeatureLayer)_Layer).FeatureClass;
            var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;
            string _FilePath = Path.GetFullPath(((IWorkspace)workspaceEdit).PathName);

            ArenaProjectType prjType = ArenaProjectType.ARENA;
            this.Data.PdmObjectList.AddRange(ArenaDataModule.GetObjectsFromPdmFile(Path.GetDirectoryName(_FilePath), ref prjType));
            this.Data.CurrentProjectType = prjType;

            string[] words = _FilePath.Split('\\');
            this.Data.CurProjectName = words[words.Length-2];
            this.Data.MapDocumentName = words[words.Length - 2];

            FillAirtrackTableDic(workspaceEdit);
            FillAirportDictionary();


        }
    }
}
