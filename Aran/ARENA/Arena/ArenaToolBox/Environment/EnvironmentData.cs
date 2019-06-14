using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using ARENA.Project;
using ARENA.Enums_Const;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;
using GeometryFunctions;
using PDM;
using Aran.Temporality.CommonUtil.Context;
using Aran.Aim.Data;
using Aran.Aim;
using Aran.Aim.Enums;
using TimeSlice = Aran.Temporality.Common.MetaData.TimeSlice;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Aim.Extension.Property;
using System.Windows.Forms;
using Aran.Aim.AixmMessage;
using Aran.Aim.DataTypes;
using ArenaErrorReport;
using System.Windows.Interop;
using ArenaLogManager;

namespace ARENA.Environment
{
    public class EnvironmentData
    {
        #region Environment

        public Environment Environment { get; set; }

        #endregion

        #region Auto-Initialized properties and collections 

        private List<PDMObject> _pdmObjectList;
        public List<PDMObject> PdmObjectList
        {
            get { return _pdmObjectList??(_pdmObjectList=new List<PDMObject>()); }
        }

        private Dictionary<string, AirportHeliport> _airdromeHeliportDictionary;
        public Dictionary<string, AirportHeliport> AirdromeHeliportDictionary
        {
            get { return _airdromeHeliportDictionary??(_airdromeHeliportDictionary=new Dictionary<string, AirportHeliport>()); }
        }

        private TapFunctions _geometryFunctions;
        public TapFunctions GeometryFunctions
        {
            get { return _geometryFunctions??(_geometryFunctions=new TapFunctions()); }
        }

        private Dictionary<Type, ITable> _tableDictionary;
        public Dictionary<Type, ITable> TableDictionary
        {
            get { return _tableDictionary ?? (_tableDictionary = new Dictionary<Type, ITable>()); }
            set { _tableDictionary = value; }
        }

        private static bool TossConnectionInited;

        //#region AIM_LIST

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_ADHP_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_ADHP_LIST
        //{
        //    get { return _AIM_ADHP_LIST; }
        //    set { _AIM_ADHP_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_RWY_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_RWY_LIST
        //{
        //    get { return _AIM_RWY_LIST; }
        //    set { _AIM_RWY_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_RDN_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_RDN_LIST
        //{
        //    get { return _AIM_RDN_LIST; }
        //    set { _AIM_RDN_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_CLP_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_CLP_LIST
        //{
        //    get { return _AIM_CLP_LIST; }
        //    set { _AIM_CLP_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_NVD_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_NVD_LIST
        //{
        //    get { return _AIM_NVD_LIST; }
        //    set { _AIM_NVD_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_VOR_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_VOR_LIST
        //{
        //    get { return _AIM_VOR_LIST; }
        //    set { _AIM_VOR_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_DME_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_DME_LIST
        //{
        //    get { return _AIM_DME_LIST; }
        //    set { _AIM_DME_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_NDB_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_NDB_LIST
        //{
        //    get { return _AIM_NDB_LIST; }
        //    set { _AIM_NDB_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_TACAN_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_TACAN_LIST
        //{
        //    get { return _AIM_TACAN_LIST; }
        //    set { _AIM_TACAN_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_LOC_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_LOC_LIST
        //{
        //    get { return _AIM_LOC_LIST; }
        //    set { _AIM_LOC_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_GLD_P_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_GLD_P_LIST
        //{
        //    get { return _AIM_GLD_P_LIST; }
        //    set { _AIM_GLD_P_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_WYP_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_WYP_LIST
        //{
        //    get { return _AIM_WYP_LIST; }
        //    set { _AIM_WYP_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_ARSP_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_ARSP_LIST
        //{
        //    get { return _AIM_ARSP_LIST; }
        //    set { _AIM_ARSP_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_ENRT_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_ENRT_LIST
        //{
        //    get { return _AIM_ENRT_LIST; }
        //    set { _AIM_ENRT_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_ROUTE_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_ROUTE_LIST
        //{
        //    get { return _AIM_ROUTE_LIST; }
        //    set { _AIM_ROUTE_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_IAP_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_IAP_LIST
        //{
        //    get { return _AIM_IAP_LIST; }
        //    set { _AIM_IAP_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_SID_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_SID_LIST
        //{
        //    get { return _AIM_SID_LIST; }
        //    set { _AIM_SID_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_STAR_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_STAR_LIST
        //{
        //    get { return _AIM_STAR_LIST; }
        //    set { _AIM_STAR_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_ArrivalFeederLeg_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_ArrivalFeederLeg_LIST
        //{
        //    get { return _AIM_ArrivalFeederLeg_LIST; }
        //    set { _AIM_ArrivalFeederLeg_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_ArrivalLeg_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_ArrivalLeg_LIST
        //{
        //    get { return _AIM_ArrivalLeg_LIST; }
        //    set { _AIM_ArrivalLeg_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_DepartureLeg_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_DepartureLeg_LIST
        //{
        //    get { return _AIM_DepartureLeg_LIST; }
        //    set { _AIM_DepartureLeg_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_FinalLeg_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_FinalLeg_LIST
        //{
        //    get { return _AIM_FinalLeg_LIST; }
        //    set { _AIM_FinalLeg_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_InitialLeg_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_InitialLeg_LIST
        //{
        //    get { return _AIM_InitialLeg_LIST; }
        //    set { _AIM_InitialLeg_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_IntermediateLeg_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_IntermediateLeg_LIST
        //{
        //    get { return _AIM_IntermediateLeg_LIST; }
        //    set { _AIM_IntermediateLeg_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_MissedApproachLeg_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_MissedApproachLeg_LIST
        //{
        //    get { return _AIM_MissedApproachLeg_LIST; }
        //    set { _AIM_MissedApproachLeg_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_AngleIndication_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_AngleIndication_LIST
        //{
        //    get { return _AIM_AngleIndication_LIST; }
        //    set { _AIM_AngleIndication_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_DistanceIndication_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_DistanceIndication_LIST
        //{
        //    get { return _AIM_DistanceIndication_LIST; }
        //    set { _AIM_DistanceIndication_LIST = value; }
        //}

        //private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_VerticalStructure_LIST;
        //public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_VerticalStructure_LIST
        //{
        //    get { return _AIM_VerticalStructure_LIST; }
        //    set { _AIM_VerticalStructure_LIST = value; }
        //}

        //#endregion

        #region AIXM_LIST

        private List<Intermediate_AIXM51_Arena> _Intermediate_AIXM51_Arena_Features;
        public List<Intermediate_AIXM51_Arena> Intermediate_AIXM51_Arena_Features
        {
            get { return _Intermediate_AIXM51_Arena_Features; }
            set { _Intermediate_AIXM51_Arena_Features = value; }
        }

        #endregion

        #endregion

        #region Current status

        public AbstractProject CurrentProject { get; set; }
        public ArenaProjectType CurrentProjectType { get; set; }
        //public DateTime CurrentProjectDataLoadingTime { get; set; }

        public ISpatialReference SpatialReference { get; set; }
        public string MapDocumentName { get; set; }
        public string CurProjectName { get; set; }

        private List<string> _CurrentLog;

        public List<string> ProjectLog
        {
            get { return _CurrentLog ?? (_CurrentLog = new List<string>()); }
            set { _CurrentLog = value; }
        }
        
        #endregion
       
        #region Layer

        public ILayer GetLinkedLayer(PDMObject sellObj)
        {
            var map = Environment.pMap;
            var tp = sellObj.GetType().Name;
            ILayer selectedLayer = null;
            try
            {
                switch (tp)
                {
                    case ("AirportHeliport"):
                        selectedLayer = EsriUtils.getLayerByName(map, "AirportHeliport");
                        break;
                    case ("RunwayDirection"):
                        selectedLayer = EsriUtils.getLayerByName(map, "RunwayDirection");
                        break;
                    case ("RunwayCenterLinePoint"):
                        selectedLayer = EsriUtils.getLayerByName(map, "RunwayDirectionCenterLinePoint");
                        break;
                    case ("GlidePath"):
                        selectedLayer = EsriUtils.getLayerByName(map, "GlidePath");
                        break;
                    case ("Localizer"):
                        selectedLayer = EsriUtils.getLayerByName(map, "Localizer");
                        break;
                    case ("VOR"):
                        selectedLayer = EsriUtils.getLayerByName(map, "VOR");
                        break;
                    case ("DME"):
                        selectedLayer = EsriUtils.getLayerByName(map, "DME");
                        break;
                    case ("NDB"):
                        selectedLayer = EsriUtils.getLayerByName(map, "NDB");
                        break;
                    case ("TACAN"):
                        selectedLayer = EsriUtils.getLayerByName(map, "TACAN");
                        break;
                    case ("WayPoint"):
                        selectedLayer = EsriUtils.getLayerByName(map, "WayPoint");
                        break;
                    case ("Enroute"):
                    case ("RouteSegment"):
                        selectedLayer = EsriUtils.getLayerByName(map, "RouteSegment");
                        break;
                    case ("Airspace"):
                    case ("AirspaceVolume"):
                        selectedLayer = EsriUtils.getLayerByName(map, "AirspaceVolume");
                        break;
                    //case ("VerticalStructure"):
                    case ("VerticalStructurePart"):
                        if (sellObj.Geo != null)
                        {
                            switch (sellObj.Geo.GeometryType)
                            {
                                case (esriGeometryType.esriGeometryPoint):
                                    selectedLayer = EsriUtils.getLayerByName(map, "VerticalStructure_Point");
                                    break;

                                case (esriGeometryType.esriGeometryPolyline):
                                case (esriGeometryType.esriGeometryLine):
                                    selectedLayer = EsriUtils.getLayerByName(map, "VerticalStructure_Curve");
                                    break;

                                case (esriGeometryType.esriGeometryPolygon):
                                    selectedLayer = EsriUtils.getLayerByName(map, "VerticalStructure_Surface");
                                    break;
                            }
                        }
                        break;

                    case ("InstrumentApproachProcedure"):
                    case ("StandardInstrumentArrival"):
                    case ("StandardInstrumentDeparture"):
                    case ("ProcedureTransitions"):
                    case ("FinalLeg"):
                    case ("ProcedureLeg"):
                    case ("MissaedApproachLeg"):
                        selectedLayer = EsriUtils.getLayerByName(map, "ProcedureLegs");

                        break;

                    case ("FacilityMakeUp"):
                        selectedLayer = EsriUtils.getLayerByName(map, "FacilityMakeUp");

                        break;
                    case ("GeoBorder"):
                        selectedLayer = EsriUtils.getLayerByName(map, "GeoBorder");

                        break;
                    case ("SegmentPoint"):
                        switch (((SegmentPoint)sellObj).PointChoice)
                        {
                            case PointChoice.DesignatedPoint:
                                selectedLayer = EsriUtils.getLayerByName(map, "WayPoint");
                                break;
                            case PointChoice.Navaid:
                                //selectedLayer = EsriUtils.getLayerByName(map, "WayPoint");
                                //sellObj = DataCash.GetPDMObject(((SegmentPoint)sellObj).PointChoiceID, PDM_ENUM.NavaidSystem);
                                break;
                            case PointChoice.RunwayCentrelinePoint:
                                selectedLayer = EsriUtils.getLayerByName(map, "RunwayDirectionCenterLinePoint");
                                break;
                            case PointChoice.AirportHeliport:
                                selectedLayer = EsriUtils.getLayerByName(map, "AirportHeliport");
                                break;

                        }
                        break;
                    case ("HoldingPattern"):
                        selectedLayer = EsriUtils.getLayerByName(map, "HoldingPattern");

                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name)
                    .Error(ex, "Type: " + sellObj.GetType().Name + "; ID: " + sellObj.ID);
                selectedLayer = null;
            }
            

            return selectedLayer;
        }

        public PDMObject GetObjectLinkedWithLayer(string LayerName)
        {
            PDMObject res = null;
            switch (LayerName)
            {
                case ("AirportHeliport"):
                    res = new AirportHeliport();
                    break;
                case ("RunwayDirection"):
                    res = new AirportHeliport();
                    break;
                case ("RunwayDirectionCenterLinePoint"):
                    res = new RunwayDirection();
                    break;
                case ("GlidePath"):
                    res = new GlidePath();
                    break;
                case ("Localizer"):
                    res = new Localizer();
                    break;
                case ("VOR"):
                    res = new VOR();
                    break;
                case ("DME"):
                    res = new DME();
                    break;
                case ("NDB"):
                    res = new NDB();
                    break;
                case ("TACAN"):
                    res = new TACAN();
                    break;
                case ("WayPoint"):
                    res = new WayPoint();
                    break;
                case ("Enroute"):
                case ("RouteSegment"):
                    res = new RouteSegment();
                    break;
                case ("Airspace"):
                case ("AirspaceVolume"):
                    res = new AirspaceVolume();
                    break;
                case ("VerticalStructure_Point"):
                case ("VerticalStructure_Curve"):
                case ("VerticalStructure_Surface"):
                   res = new VerticalStructurePart();
                    break;
            }

            return res;
        }


        #endregion

        #region Images

        public Dictionary<string, int> LayersSymbolInImageList = new Dictionary<string, int>();

        public Bitmap ConvertLayersSymbolToBitmap(ILayer selectedLayer)
        {
            Bitmap bitmap;
            try
            {
                //var graphics = Graphics.FromImage(global::ArenaToolBox.Properties.Resources.info);
                Bitmap flag = new Bitmap(16, 16);
                Graphics graphics = Graphics.FromImage(flag);

                var featureLayer = selectedLayer as IFeatureLayer;
                if (featureLayer == null) return null;
                var geoFeatureLayer = (IGeoFeatureLayer)featureLayer;
                var simpleRenderer = (ISimpleRenderer)geoFeatureLayer.Renderer;

                var symbol = simpleRenderer.Symbol;
                

                bitmap = EsriUtils.SymbolToBitmap(
                        symbol,
                        new Size(16, 16),
                        graphics,
                        ColorTranslator.ToWin32(System.Drawing.Color.Transparent));

            }
            catch(Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name)
                    .Error(ex, "Layer Name: " + selectedLayer.Name);
                bitmap = null;
            }

            return bitmap;
        }

        public int GetObjectImageIndex(PDMObject pdmObj)
        {
            var res = 0;

            if (!LayersSymbolInImageList.ContainsKey(pdmObj.GetType().Name))
            {
                var lyr = GetLinkedLayer(pdmObj);
                if (lyr != null)
                {
                    var bmp = ConvertLayersSymbolToBitmap(lyr);
                    if (bmp!=null) Environment.TreeViewImageList.Images.Add(bmp);
                    LayersSymbolInImageList.Add(pdmObj.GetType().Name, Environment.TreeViewImageList.Images.Count - 1);
                }
            }

            if (LayersSymbolInImageList.ContainsKey(pdmObj.GetType().Name)) res = LayersSymbolInImageList[pdmObj.GetType().Name];

            return res++;
        }

        #endregion

        //private static IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> GetData(Aran.Aim.FeatureType featureType, DateTime initialDate)
        //{
        //    if (!TossConnectionInited)
        //    {
        //        return null;

        //    }

            
        //    return CurrentDataContext.CurrentService.GetActualDataByDate(new Aran.Temporality.Common.Id.FeatureId { FeatureTypeId = (int)featureType }, false, initialDate);



        //}

        public void CloseToss()
        {
            TossConnectionInited = false;
            ConnectionProvider.Close();
        }

        public bool OpenToss()
        {
            ConnectionProvider.MainAction = () =>
            {
                TossConnectionInited = true;
            };
            
           return ConnectionProvider.Open();

        }

        public bool ReadAIXM51Data(string AIXM_FileName)
        {
            AObjectListConfig.IgnoreNotes = true;

            var featTypeList = new[] { FeatureType.AirportHeliport };

            switch (CurrentProjectType)
            {
                case ArenaProjectType.ARENA:
                    featTypeList = new[] { FeatureType.AirportHeliport, FeatureType.Runway, FeatureType.RunwayDirection, FeatureType.RunwayCentrelinePoint,FeatureType.RunwayElement,FeatureType.Taxiway,
                               FeatureType.TaxiwayElement,FeatureType.Navaid, FeatureType.DME, FeatureType.VOR,FeatureType.Localizer, FeatureType.Glidepath, FeatureType.NDB,FeatureType.MarkerBeacon,
                               FeatureType.TACAN,FeatureType.DesignatedPoint,FeatureType.VerticalStructure, FeatureType.Airspace, FeatureType.Route, FeatureType.RouteSegment,
                               FeatureType.InstrumentApproachProcedure,FeatureType.StandardInstrumentArrival, FeatureType.StandardInstrumentDeparture,
                               FeatureType.ArrivalFeederLeg,FeatureType.FinalLeg,FeatureType.InitialLeg,FeatureType.IntermediateLeg,FeatureType.MissedApproachLeg,FeatureType.ArrivalLeg,FeatureType.DepartureLeg,
                               FeatureType.AngleIndication, FeatureType.DistanceIndication,FeatureType.HoldingPattern,FeatureType.SafeAltitudeArea,FeatureType.GeoBorder,
                               FeatureType.AirTrafficControlService, FeatureType.AirTrafficManagementService,FeatureType.GroundTrafficControlService,
                               FeatureType.RadioCommunicationChannel,FeatureType.InformationService, FeatureType.OrganisationAuthority, FeatureType.ObstacleArea,FeatureType.ApproachLightingSystem};
                    break;
                //case ArenaProjectType.ARENA:
                //    featTypeList = new[] { FeatureType.HoldingPattern };
                //    break;

                case ArenaProjectType.AERODROME:
                    featTypeList = new[] { FeatureType.AirportHeliport, FeatureType.Runway,FeatureType.Apron,FeatureType.ApronMarking,FeatureType.ApronElement,FeatureType.TaxiwayElement,FeatureType.Taxiway,
                               FeatureType.ApproachLightingSystem,FeatureType.ApronLightSystem,FeatureType.RunwayDirection,FeatureType.RunwayCentrelinePoint,FeatureType.DeicingArea,FeatureType.DeicingAreaMarking,
                               FeatureType.Runway, FeatureType.RunwayElement, FeatureType.RunwayMarking,FeatureType.TaxiwayMarking, FeatureType.AircraftStand,FeatureType.StandMarking, FeatureType.GuidanceLine,
                               FeatureType.GuidanceLineLightSystem, FeatureType.GuidanceLineMarking, FeatureType.AirTrafficControlService, FeatureType.RadioCommunicationChannel, FeatureType.InformationService,
                               FeatureType.Navaid,FeatureType.DME, FeatureType.Localizer, FeatureType.Glidepath, FeatureType.VerticalStructure, FeatureType.VOR, FeatureType.CheckpointINS, FeatureType.CheckpointVOR,
                               FeatureType.TaxiwayLightSystem, FeatureType.RunwayProtectArea, FeatureType.RunwayProtectAreaLightSystem,FeatureType.RunwayVisualRange,FeatureType.AirportHotSpot,FeatureType.TaxiHoldingPosition,
                               FeatureType.TaxiHoldingPositionLightSystem, FeatureType .TaxiHoldingPositionMarking,FeatureType.VisualGlideSlopeIndicator,FeatureType.TouchDownLiftOff,FeatureType.TouchDownLiftOffMarking,
                               FeatureType.TouchDownLiftOffLightSystem,FeatureType.TouchDownLiftOffSafeArea,FeatureType.WorkArea, FeatureType.Road,FeatureType.Unit,FeatureType.NonMovementArea, FeatureType.Airspace,
                               FeatureType.RadioFrequencyArea };
                    break;


                case ArenaProjectType.PANDA:
                    featTypeList = new[] { FeatureType.AirportHeliport, FeatureType.Runway, FeatureType.RunwayDirection, FeatureType.RunwayCentrelinePoint,FeatureType.RunwayElement,FeatureType.Taxiway,FeatureType.TaxiwayElement,
                               FeatureType.Navaid, FeatureType.DME, FeatureType.VOR,FeatureType.Localizer, FeatureType.Glidepath, FeatureType.NDB,FeatureType.MarkerBeacon,FeatureType.TACAN,FeatureType.DesignatedPoint,
                               FeatureType.VerticalStructure, FeatureType.InstrumentApproachProcedure,FeatureType.StandardInstrumentArrival, FeatureType.StandardInstrumentDeparture,
                               FeatureType.ArrivalFeederLeg,FeatureType.FinalLeg,FeatureType.InitialLeg,FeatureType.IntermediateLeg,FeatureType.MissedApproachLeg,FeatureType.ArrivalLeg,FeatureType.DepartureLeg,
                               FeatureType.AngleIndication, FeatureType.DistanceIndication,FeatureType.HoldingPattern,FeatureType.SafeAltitudeArea,FeatureType.GeoBorder, 
                               FeatureType.AirTrafficControlService,FeatureType.RadioCommunicationChannel,FeatureType.InformationService, FeatureType.OrganisationAuthority, FeatureType.ObstacleArea, FeatureType.ApproachLightingSystem};
                    break;
                default:
                    featTypeList = new[] { FeatureType.AirportHeliport, FeatureType.Runway, FeatureType.RunwayDirection, FeatureType.RunwayCentrelinePoint,FeatureType.RunwayElement,FeatureType.Taxiway,FeatureType.TaxiwayElement, 
                               FeatureType.Navaid, FeatureType.DME, FeatureType.VOR,FeatureType.Localizer, FeatureType.Glidepath, FeatureType.NDB,FeatureType.MarkerBeacon,FeatureType.TACAN,FeatureType.DesignatedPoint,
                               FeatureType.VerticalStructure, FeatureType.Airspace, FeatureType.Route, FeatureType.RouteSegment,
                               FeatureType.InstrumentApproachProcedure,FeatureType.StandardInstrumentArrival, FeatureType.StandardInstrumentDeparture,
                               FeatureType.ArrivalFeederLeg,FeatureType.FinalLeg,FeatureType.InitialLeg,FeatureType.IntermediateLeg,FeatureType.MissedApproachLeg,FeatureType.ArrivalLeg,FeatureType.DepartureLeg,
                               FeatureType.AngleIndication, FeatureType.DistanceIndication,FeatureType.SafeAltitudeArea,FeatureType.GeoBorder, 
                               FeatureType.AirTrafficControlService,FeatureType.RadioCommunicationChannel,FeatureType.InformationService, FeatureType.OrganisationAuthority, FeatureType.ObstacleArea, FeatureType.ApproachLightingSystem};
                    break;
            }

            try
            {
                var parser = new ParseXmlFile();
                parser.Parse(AIXM_FileName);


                #region Parser

                if (!System.Diagnostics.Debugger.IsAttached)
                {

                    //var errorInfoList = new List<DeserializedErrorInfo>(parser.ErrorInfoList);

                    //errorInfoList.ForEach(ei =>
                    //{
                    //    if (string.IsNullOrEmpty(ei.Action))
                    //        ei.Action = "Property Ignored";
                    //});


                    //#region Check Non Existing Feature Reference

                    //var nonExistingRefFeaturePropList = CheckNonExistingFeatureRef(parser.Features);

                    //if (nonExistingRefFeaturePropList.Count > 0)
                    //{
                    //    var sss = string.Empty;

                    //    foreach (var item in nonExistingRefFeaturePropList)
                    //    {
                    //        Aran.Aim.Utilities.AimMetadataUtility.SetValue(item.Feature, item.PropInfoList.ToArray(), item.ListPropIndexes, null);

                    //        var propPath = string.Empty;

                    //        if (item.PropInfoList.Count > 0)
                    //        {
                    //            item.PropInfoList.ForEach(pi => propPath += pi.AixmName + "/");
                    //            propPath = propPath.Remove(propPath.Length - 1);
                    //        }

                    //        errorInfoList.Add(new DeserializedErrorInfo
                    //        {
                    //            FeatureType = item.Feature.FeatureType,
                    //            Identifier = item.Feature.Identifier,
                    //            PropertyName = propPath,
                    //            ErrorMessage = string.Format("Non existing reference detected: ({0}: {1})",
                    //                (item.PropInfoList.Count == 0 ? "" : item.PropInfoList.Last().ReferenceFeature.ToString()),
                    //                item.RefIdentifier),
                    //            Action = "Property Cleared",
                    //            ErrorType = CodeErrorType.NonExistingReference
                    //        });

                    //        #region NON_GUID_IDENTIFIER

                    //        if (Aran.Aim.DataTypes.FeatureRef.NonGuidIdentifier)
                    //        {
                    //            foreach (var pair in FeatureRef.GuidAssociteList)
                    //            {
                    //                if (pair.Value == item.RefIdentifier)
                    //                {
                    //                    sss += pair.Key + "\r\n";
                    //                    break;
                    //                }
                    //            }
                    //        }

                    //        #endregion
                    //    }
                    //}

                    //#endregion

                    //if (errorInfoList.Count > 0)
                    //{
                    //    //*** Show Error Info.

                    //    var window = new MainWindow(errorInfoList);
                    //    var helper = new WindowInteropHelper(window);
                    //    helper.Owner = new IntPtr(this.Environment.mxApplication.hWnd);
                    //    //ElementHost.EnableModelessKeyboardInterop(window);
                    //    window.ShowInTaskbar = false;

                    //    if (!window.ShowDialog() ?? false)
                    //        return false;
                    //}


                }

                #endregion

                this.Intermediate_AIXM51_Arena_Features = new List<Intermediate_AIXM51_Arena>();

                foreach (var feat in parser.Features)
                {
                    try
                    {
                        //System.Diagnostics.Debug.WriteLine(feat.FeatureType.ToString() + " " + feat.Identifier);
                        //if (feat.Identifier.ToString().CompareTo("1bed6938-40dd-41c5-bc69-d8c203d69ffb") == 0) continue;
                        if (featTypeList.Contains(feat.FeatureType))
                        {
                            var myEvent = new AimEvent
                            {
                                WorkPackage = 0,
                                Interpretation = feat.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA ? Interpretation.TempDelta : Interpretation.PermanentDelta,
                                TimeSlice = new TimeSlice(DateTime.Now),
                                LifeTimeBegin = (feat.TimeSlice.FeatureLifetime != null) && (feat.TimeSlice.FeatureLifetime.BeginPosition <= DateTime.Now) ? feat.TimeSlice.FeatureLifetime.BeginPosition : DateTime.Now,
                                LifeTimeEnd = feat.TimeSlice.FeatureLifetime != null ? feat.TimeSlice.FeatureLifetime.EndPosition : DateTime.Now,
                                Data = feat,

                            };

                            myEvent.Data.InitEsriExtension();
                            var geoDataList = myEvent.Data.PropertyExtensions.OfType<EsriPropertyExtension>().Select(t => t.EsriObject).ToList();

                            this.Intermediate_AIXM51_Arena_Features.Add(new Intermediate_AIXM51_Arena { AIXM51_Feature = feat, AixmGeo = geoDataList });
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name)
                            .Error(ex, "Type: " + feat.FeatureType + "; ID: "+ feat.Identifier);                      
                    }
                    
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name)
                    .Error(ex, "Error when read AIXM 5.1 Data ");
                return false;
            }

            return true;
        }

        public bool ReadAIXM51Data(Aran.Temporality.Common.Entity.PublicSlot _Slot)
        {
            AObjectListConfig.IgnoreNotes = true;

            var featTypeList = new[] { FeatureType.AirportHeliport };

            switch (CurrentProjectType)
            {
                case ArenaProjectType.ARENA:
                    featTypeList = new[] { FeatureType.AirportHeliport, FeatureType.Runway, FeatureType.RunwayDirection, FeatureType.RunwayCentrelinePoint,FeatureType.RunwayElement,FeatureType.Taxiway,
                               FeatureType.TaxiwayElement,FeatureType.Navaid, FeatureType.DME, FeatureType.VOR,FeatureType.Localizer, FeatureType.Glidepath, FeatureType.NDB,FeatureType.MarkerBeacon,
                               FeatureType.TACAN,FeatureType.DesignatedPoint,FeatureType.VerticalStructure, FeatureType.Airspace, FeatureType.Route, FeatureType.RouteSegment,
                               FeatureType.InstrumentApproachProcedure,FeatureType.StandardInstrumentArrival, FeatureType.StandardInstrumentDeparture,
                               FeatureType.ArrivalFeederLeg,FeatureType.FinalLeg,FeatureType.InitialLeg,FeatureType.IntermediateLeg,FeatureType.MissedApproachLeg,FeatureType.ArrivalLeg,FeatureType.DepartureLeg,
                               FeatureType.AngleIndication, FeatureType.DistanceIndication,FeatureType.HoldingPattern,FeatureType.SafeAltitudeArea,FeatureType.GeoBorder,
                               FeatureType.AirTrafficControlService, FeatureType.AirTrafficManagementService,FeatureType.GroundTrafficControlService,
                               FeatureType.RadioCommunicationChannel,FeatureType.InformationService, FeatureType.OrganisationAuthority, FeatureType.ObstacleArea,FeatureType.ApproachLightingSystem};
                    break;
                case ArenaProjectType.AERODROME:
                    featTypeList = new[] { FeatureType.AirportHeliport, FeatureType.Runway,FeatureType.Apron,FeatureType.ApronMarking,FeatureType.ApronElement,FeatureType.TaxiwayElement,FeatureType.Taxiway,
                               FeatureType.ApproachLightingSystem,FeatureType.ApronLightSystem,FeatureType.RunwayDirection,FeatureType.RunwayCentrelinePoint,FeatureType.DeicingArea,FeatureType.DeicingAreaMarking,
                               FeatureType.Runway, FeatureType.RunwayElement, FeatureType.RunwayMarking,FeatureType.TaxiwayMarking, FeatureType.AircraftStand,FeatureType.StandMarking, FeatureType.GuidanceLine,
                               FeatureType.GuidanceLineLightSystem, FeatureType.GuidanceLineMarking, FeatureType.AirTrafficControlService, FeatureType.RadioCommunicationChannel, FeatureType.InformationService,
                               FeatureType.Navaid,FeatureType.DME, FeatureType.Localizer, FeatureType.Glidepath, FeatureType.VerticalStructure, FeatureType.VOR, FeatureType.CheckpointINS, FeatureType.CheckpointVOR,
                               FeatureType.TaxiwayLightSystem, FeatureType.RunwayProtectArea, FeatureType.RunwayProtectAreaLightSystem,FeatureType.RunwayVisualRange,FeatureType.AirportHotSpot,FeatureType.TaxiHoldingPosition,
                               FeatureType.TaxiHoldingPositionLightSystem, FeatureType .TaxiHoldingPositionMarking,FeatureType.VisualGlideSlopeIndicator,FeatureType.TouchDownLiftOff,FeatureType.TouchDownLiftOffMarking,
                               FeatureType.TouchDownLiftOffLightSystem,FeatureType.TouchDownLiftOffSafeArea,FeatureType.WorkArea, FeatureType.Road,FeatureType.Unit,FeatureType.NonMovementArea, FeatureType.Airspace,
                               FeatureType.RadioFrequencyArea };
                    break;


                case ArenaProjectType.PANDA:
                    featTypeList = new[] { FeatureType.AirportHeliport, FeatureType.Runway, FeatureType.RunwayDirection, FeatureType.RunwayCentrelinePoint,FeatureType.RunwayElement,FeatureType.Taxiway,FeatureType.TaxiwayElement,
                               FeatureType.Navaid, FeatureType.DME, FeatureType.VOR,FeatureType.Localizer, FeatureType.Glidepath, FeatureType.NDB,FeatureType.MarkerBeacon,FeatureType.TACAN,FeatureType.DesignatedPoint,
                               FeatureType.VerticalStructure, FeatureType.InstrumentApproachProcedure,FeatureType.StandardInstrumentArrival, FeatureType.StandardInstrumentDeparture,
                               FeatureType.ArrivalFeederLeg,FeatureType.FinalLeg,FeatureType.InitialLeg,FeatureType.IntermediateLeg,FeatureType.MissedApproachLeg,FeatureType.ArrivalLeg,FeatureType.DepartureLeg,
                               FeatureType.AngleIndication, FeatureType.DistanceIndication,FeatureType.HoldingPattern,FeatureType.SafeAltitudeArea,FeatureType.GeoBorder,
                               FeatureType.AirTrafficControlService,FeatureType.RadioCommunicationChannel,FeatureType.InformationService, FeatureType.OrganisationAuthority, FeatureType.ObstacleArea, FeatureType.ApproachLightingSystem};
                    break;
                default:
                    featTypeList = new[] { FeatureType.AirportHeliport, FeatureType.Runway, FeatureType.RunwayDirection, FeatureType.RunwayCentrelinePoint,FeatureType.RunwayElement,FeatureType.Taxiway,FeatureType.TaxiwayElement,
                               FeatureType.Navaid, FeatureType.DME, FeatureType.VOR,FeatureType.Localizer, FeatureType.Glidepath, FeatureType.NDB,FeatureType.MarkerBeacon,FeatureType.TACAN,FeatureType.DesignatedPoint,
                               FeatureType.VerticalStructure, FeatureType.Airspace, FeatureType.Route, FeatureType.RouteSegment,
                               FeatureType.InstrumentApproachProcedure,FeatureType.StandardInstrumentArrival, FeatureType.StandardInstrumentDeparture,
                               FeatureType.ArrivalFeederLeg,FeatureType.FinalLeg,FeatureType.InitialLeg,FeatureType.IntermediateLeg,FeatureType.MissedApproachLeg,FeatureType.ArrivalLeg,FeatureType.DepartureLeg,
                               FeatureType.AngleIndication, FeatureType.DistanceIndication,FeatureType.SafeAltitudeArea,FeatureType.GeoBorder,
                               FeatureType.AirTrafficControlService,FeatureType.RadioCommunicationChannel,FeatureType.InformationService, FeatureType.OrganisationAuthority, FeatureType.ObstacleArea, FeatureType.ApproachLightingSystem};
                    break;
            }

            try
            {

                this.Intermediate_AIXM51_Arena_Features = new List<Intermediate_AIXM51_Arena>();

                foreach (var feat in featTypeList)
                {
                    try
                    {

                       var _data = GetData(feat, _Slot.EndEffectiveDate);
                    
                        if (_data !=null)
                        {
                            foreach (var aimFeat in _data)
                            {
                                try
                                {
                                    aimFeat.Data.InitEsriExtension();
                                    var geoDataList = aimFeat.Data.PropertyExtensions.OfType<EsriPropertyExtension>().Select(t => t.EsriObject).ToList();
                                    this.Intermediate_AIXM51_Arena_Features.Add(new Intermediate_AIXM51_Arena { AIXM51_Feature = aimFeat.Data.Feature, AixmGeo = geoDataList });
                                }
                                catch (Exception ex)
                                {
                                    LogManager.GetLogger(ex.TargetSite.Name)
                                        .Error(ex, "Type: " + aimFeat.Data.Feature.FeatureType + "; ID: " + aimFeat.Data.Feature.Identifier);
                                }
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name)
                            .Error(ex, "Type: " + feat.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name)
                    .Error(ex, "Error when read AIXM 5.1 TOSS Data ");
                return false;
            }

            return true;
        }

        private static IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> GetData(Aran.Aim.FeatureType featureType, DateTime initialDate)
        {
            if (!TossConnectionInited)
            {
                return null;

            }


            return CurrentDataContext.CurrentService.GetActualDataByDate(new Aran.Temporality.Common.Id.FeatureId { FeatureTypeId = (int)featureType }, false, initialDate);

        }

        private List<Aran.Aim.Utilities.RefFeatureProp> CheckNonExistingFeatureRef(List<Aran.Aim.Features.Feature> featureList)
        {
            var guidList = new List<Guid>();
            foreach (var feat in featureList)
                guidList.Add(feat.Identifier);

            var nonExistingRefFeaturePropList = new List<Aran.Aim.Utilities.RefFeatureProp>();

            foreach (var feat in featureList)
            {
                var refFeaturePropList = new List<Aran.Aim.Utilities.RefFeatureProp>();
                Aran.Aim.Utilities.AimMetadataUtility.GetReferencesFeatures(feat, refFeaturePropList);

                foreach (var refFeatureProp in refFeaturePropList)
                {
                    if (!guidList.Contains(refFeatureProp.RefIdentifier))
                    {

                        nonExistingRefFeaturePropList.Add(refFeatureProp);
                        refFeatureProp.Feature = feat;

                    }
                }
            }

            return nonExistingRefFeaturePropList;
        }

    }
}
