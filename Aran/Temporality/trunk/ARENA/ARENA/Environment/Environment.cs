using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ARENA.Project;
using ARINC_DECODER_CORE;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;
using PDM;
using PDM.PropertyExtension;
using Path = System.IO.Path;

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

        private void MapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (Data.CurrentProject != null)
            {
                Data.CurrentProject.MapControl_OnMouseDown(sender, e);
            }
        }

        private void InitTreeViewEvents()
        {
            FeatureTreeView.AfterSelect += TreeViewAfterSelect;
            FeatureTreeView.AfterCheck += TreeViewAfterCheck;
        }

        private void InitMapControlEvents()
        {
            MapControl.OnMouseDown += MapControl_OnMouseDown;

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

        private AxMapControl _mapControl;
        public AxMapControl MapControl
        {
            get { return _mapControl; }
            set
            {
                _mapControl = value;
                mapControl = (IMapControl3)MapControl.Object;
                InitMapControlEvents();
            }
        }


        public ReadOnlyPropertyGrid ReadOnlyPropertyGrid { get; set; }
        public IMapControl3 mapControl { get; set; }
        public ImageList TreeViewImageList { get; set; }
        public ContextMenuStrip FeatureTreeViewContextMenuStrip;
        public ToolStrip EnvironmentToolStrip;
        public ToolStrip FeatureTreeViewToolStrip;
        public ContextMenuStrip mapControlContextMenuStrip;
        public StatusStrip statusStrip;
        public ToolStripButton PlayButton;
        public MenuStrip MaimMenu;
        public Panel PandaToolBox;
        
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

            var zoomLayer = EsriUtils.getLayerByName(mapControl.Map, name);

            ((IActiveView)MapControl.Map).Extent = zoomLayer.AreaOfInterest;
            ((IActiveView)MapControl.Map).Refresh();

        }

        public void SetCenter_and_Projection()
        {
            string zoomedLayerName = "Area";
            ILayer _Layer = EsriUtils.getLayerByName(mapControl.Map, zoomedLayerName);

            if (IsEmptyLayer(_Layer))
            {
                zoomedLayerName = "AirspaceVolume";
                _Layer = EsriUtils.getLayerByName(mapControl.Map, zoomedLayerName);
            }

            if (IsEmptyLayer(_Layer))
            {
                zoomedLayerName = "WayPoint";
                _Layer = EsriUtils.getLayerByName(mapControl.Map, zoomedLayerName);
            }


            if ((_Layer != null) && (Data.AirdromeHeliportDictionary.Count > 0))
            {
               
                AirportHeliport firstADHP = Data.AirdromeHeliportDictionary.First().Value;

                if ((firstADHP != null) && (firstADHP.Geo != null) && (((IPoint)firstADHP.Geo) != null))
                {
                    double CMD = ((IPoint)firstADHP.Geo).X;

                    EsriUtils.ChangeProjectionAndMeredian(CMD, mapControl.Map);
                }

                ZoomToLayerByName(zoomedLayerName);

            }
        }

        private bool IsEmptyLayer(ILayer _Layer)
        {
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
                var graphicsContainer = (IGraphicsContainer)MapControl.Map;

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
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);

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

        public void FillAirtrackTableDic(IWorkspaceEdit workspaceEdit)
        {
            Data.TableDictionary = new Dictionary<Type, ITable>
            {
                {
                    typeof (AirportHeliport),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "AirportHeliport")
                },
                {
                    typeof (Runway),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit, "Runway")
                },
                {
                    typeof (RunwayDirection),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "RunwayDirection")
                },
                {
                    typeof (RunwayCenterLinePoint),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "RunwayDirectionCenterLinePoint")
                },
                {
                    typeof (GlidePath),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "GlidePath")
                },
                {
                    typeof (Localizer),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "Localizer")
                },
                 {
                    typeof (Marker),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "MarkerBeacon")
                },
                {
                    typeof (NDB),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit, "NDB")
                },
                {
                    typeof (VOR),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit, "VOR")
                },
                {
                    typeof (DME),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit, "DME")
                },
                {
                    typeof (TACAN),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit, "TACAN")
                },
                {
                    typeof (WayPoint),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit, "WayPoint")
                },
                {
                    typeof (NavaidSystem),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "NavaidSystem")
                },
                {
                    typeof (Procedure),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "Procedures")
                },
                {
                    typeof (InstrumentApproachProcedure),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "InstrumentApproachProcedure")
                },
                {
                    typeof (StandardInstrumentArrival),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "StandardInstrumentArrival")
                },
                {
                    typeof (StandardInstrumentDeparture),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "StandardInstrumentDeparture")
                },
                {
                    typeof (ProcedureTransitions),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "ProcedureTransitions")
                },
                {
                    typeof (ProcedureLeg),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "ProcedureLegs")
                },
                {
                    typeof (FinalLeg),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "FinalLeg")
                },
                {
                    typeof (MissaedApproachLeg),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "MissaedApproachLeg")
                },
                {
                    typeof (SegmentPoint),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "SegmentPoint")
                },
                                {
                    typeof (RouteSegmentPoint),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "SegmentPoint")
                },
                {
                    typeof (FacilityMakeUp),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "FacilityMakeUp")
                },
                {
                    typeof (AngleIndication),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "AngleIndication")
                },
                {
                    typeof (DistanceIndication),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "DistanceIndication")
                },
                {
                    typeof (Enroute),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit, "Enroute")
                },
                {
                    typeof (RouteSegment),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "RouteSegment")
                },
                {
                    typeof (AirspaceVolume),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "AirspaceVolume")
                },
                {
                    typeof (Airspace),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit, "Airspace")
                },
                {
                    typeof (VerticalStructure),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "VerticalStructure")
                },
                {
                    typeof (VerticalStructurePart),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "VerticalStructurePart")
                },
                {
                    typeof (PointClass),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "VerticalStructure_Point")
                },
                {
                    typeof (LineClass),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "VerticalStructure_Curve")
                },
                {
                    typeof (PolygonClass),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit,
                                            "VerticalStructure_Surface")
                },
                {
                    typeof (AREA_PDM),
                    EsriUtils.getTableByname((IFeatureWorkspace) workspaceEdit, "Area")
                },

            };
        }

        public void CreateEmptyProject(ArenaProjectType projectType)
        {
            try
            {
                var pathToTemplateFile = Static_Proc.GetPathToTemplateFile();

                if (Directory.Exists(pathToTemplateFile))
                {
  

                    FeatureTreeView.Nodes.Clear();
                    var tempDirName = Path.GetTempPath();
                    var dInf = Directory.CreateDirectory(tempDirName + @"\PDM");
                    tempDirName = dInf.FullName;

                    var pathToTemplateFileMxdSource = Path.Combine(pathToTemplateFile, "pdm.mxd");
                    var pathToTemplateFileMxdDest = Path.Combine(tempDirName, "pdm.mxd");

                    switch (projectType)
                    {
                        case (ArenaProjectType.ARENA):
                        case (ArenaProjectType.PANDA):
                            FeatureTreeView.Nodes.Add(new TreeNode("ARP/RWY/RDN/ILS"));
                            FeatureTreeView.Nodes.Add(new TreeNode("Navaids"));

                            pathToTemplateFileMxdSource = Path.Combine(pathToTemplateFile, "arena_PDM.mxd");
                            pathToTemplateFileMxdDest = Path.Combine(tempDirName, "arena_PDM.mxd");

                            break;

                        case (ArenaProjectType.NOTAM):
                            FeatureTreeView.Nodes.Add(new TreeNode("Airspaces"));

                            pathToTemplateFileMxdSource = Path.Combine(pathToTemplateFile, "notam_PDM.mxd");
                            pathToTemplateFileMxdDest = Path.Combine(tempDirName, "notam_PDM.mxd");

                            break;
                    }



                    var pathToTemplateFileMdbSource = Path.Combine(pathToTemplateFile, "pdm.mdb");
                    var pathToTemplateFileMdbDest = Path.Combine(tempDirName, "pdm.mdb");

                    File.Copy(pathToTemplateFileMdbSource, pathToTemplateFileMdbDest, true);
                    File.Copy(pathToTemplateFileMxdSource, pathToTemplateFileMxdDest, true);
                    
                    while (Directory.GetFiles(tempDirName).Length < 2) System.Threading.Thread.Sleep(1);

                    if (MapControl.CheckMxFile(pathToTemplateFileMxdDest))
                    {
                        MapControl.LoadMxFile(pathToTemplateFileMxdDest);

                        var lyr = MapControl.get_Layer(0);
                        var fc = ((IFeatureLayer)lyr).FeatureClass;
                        var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;
                        workspaceEdit.StopEditOperation();
                        workspaceEdit.StopEditing(false);


                        if (Data.TableDictionary.Count == 0)
                        {
                            FillAirtrackTableDic(workspaceEdit);
                        }
                    }


                    switch (projectType)
                    {
                        case (ArenaProjectType.ARENA):
                            Data.CurrentProject = new ArenaProject(this);
                            break;

                        case (ArenaProjectType.NOTAM):
                            Data.CurrentProject = new NotamProject(this);
                            break;

                        case (ArenaProjectType.PANDA):
                            Data.CurrentProject = new PandaProject(this);
                            Data.CurrentProjectType = ArenaProjectType.PANDA;
                            break;
                    }

                    //Data.CurrentProject.
                    
                    Data.CurrentProject.OnCreate();
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);

            }

        }

        public void SetObjectVisibility(PDMObject sellObj, bool objVisibilityFlag)
        {
            sellObj.VisibilityFlag = objVisibilityFlag;

            ILayer lyr = Data.GetLinkedLayer(sellObj);
            if (lyr == null) return;
            IFeatureLayer2 FL = (ESRI.ArcGIS.Carto.IFeatureLayer2)lyr;
            IFeatureLayerDefinition pFlyrDef = (IFeatureLayerDefinition)FL;

            

            if (objVisibilityFlag)
            {
                if (pFlyrDef.DefinitionExpression.Length <= 0) pFlyrDef.DefinitionExpression = "FeatureGUID NOT IN ('0') OR FeatureGUID IN ('1')";
                    //pFlyrDef.DefinitionExpression = "FeatureGUID NOT IN ('0') OR FeatureGUID IN ('" + sellObj.ID + "')";
                //else
                {
                    pFlyrDef.DefinitionExpression = pFlyrDef.DefinitionExpression.Replace("'" + sellObj.ID + "',", "");
                    pFlyrDef.DefinitionExpression = pFlyrDef.DefinitionExpression.Replace("'1'", "'" + sellObj.ID + "','1'");
                }
            }
            else
            {
                if (pFlyrDef.DefinitionExpression.Length <= 0) pFlyrDef.DefinitionExpression = "FeatureGUID NOT IN ('0') OR FeatureGUID IN ('1')";
                //    pFlyrDef.DefinitionExpression = "FeatureGUID NOT IN ('" + sellObj.ID + "', '0') OR FeatureGUID IN ('1')";
                //else
                {
                    pFlyrDef.DefinitionExpression = pFlyrDef.DefinitionExpression.Replace("'" + sellObj.ID + "',", "");
                    pFlyrDef.DefinitionExpression = pFlyrDef.DefinitionExpression.Replace("'0'", "'" + sellObj.ID + "','0'");
                }
            }
            

            ClearGraphics();

        }

        public void SaveLog()
        {
            try
            {
                string filePath = System.IO.Path.GetDirectoryName(Static_Proc.GetPathToARINCSpecificationFile()) + @"\ARENA_ResultsInfo.txt";
                if (File.Exists(filePath)) File.Delete(filePath);

                var errorMessages = (from element in this.Data.PdmObjectList where (element != null) && (element.ExeptionDetails != null) select element).ToList();
                if ((errorMessages != null) && (errorMessages.Count > 0))
                {
                    List<string> tmp = new List<string>();
                    tmp.Add(DateTime.Now.ToString());

                    tmp.Add("Type" + (char)9 + "Name" + (char)9 + "ID" + (char)9 + "Message" + (char)9 + "Source" + (char)9 + "Stack trace");
                    foreach (var item in errorMessages)
                    {
                        tmp.Add(item.GetType().Name + (char)9 + item.GetObjectLabel() + (char)9 + item.ID + (char)9 + item.ExeptionDetails.Message + (char)9 + item.ExeptionDetails.Source + (char)9 + item.ExeptionDetails.StackTrace);

                    }

                    

                    System.IO.File.WriteAllLines(filePath, tmp.ToArray());
                }
            }
            catch { }
        }

        #endregion
    }
}
