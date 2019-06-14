using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ARENA.Project;
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

        #region AIM_LIST

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_ADHP_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_ADHP_LIST
        {
            get { return _AIM_ADHP_LIST; }
            set { _AIM_ADHP_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_RWY_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_RWY_LIST
        {
            get { return _AIM_RWY_LIST; }
            set { _AIM_RWY_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_RDN_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_RDN_LIST
        {
            get { return _AIM_RDN_LIST; }
            set { _AIM_RDN_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_CLP_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_CLP_LIST
        {
            get { return _AIM_CLP_LIST; }
            set { _AIM_CLP_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_NVD_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_NVD_LIST
        {
            get { return _AIM_NVD_LIST; }
            set { _AIM_NVD_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_VOR_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_VOR_LIST
        {
            get { return _AIM_VOR_LIST; }
            set { _AIM_VOR_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_DME_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_DME_LIST
        {
            get { return _AIM_DME_LIST; }
            set { _AIM_DME_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_NDB_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_NDB_LIST
        {
            get { return _AIM_NDB_LIST; }
            set { _AIM_NDB_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_TACAN_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_TACAN_LIST
        {
            get { return _AIM_TACAN_LIST; }
            set { _AIM_TACAN_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_LOC_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_LOC_LIST
        {
            get { return _AIM_LOC_LIST; }
            set { _AIM_LOC_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_GLD_P_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_GLD_P_LIST
        {
            get { return _AIM_GLD_P_LIST; }
            set { _AIM_GLD_P_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_WYP_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_WYP_LIST
        {
            get { return _AIM_WYP_LIST; }
            set { _AIM_WYP_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_ARSP_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_ARSP_LIST
        {
            get { return _AIM_ARSP_LIST; }
            set { _AIM_ARSP_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_ENRT_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_ENRT_LIST
        {
            get { return _AIM_ENRT_LIST; }
            set { _AIM_ENRT_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_ROUTE_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_ROUTE_LIST
        {
            get { return _AIM_ROUTE_LIST; }
            set { _AIM_ROUTE_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_IAP_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_IAP_LIST
        {
            get { return _AIM_IAP_LIST; }
            set { _AIM_IAP_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_SID_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_SID_LIST
        {
            get { return _AIM_SID_LIST; }
            set { _AIM_SID_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_STAR_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_STAR_LIST
        {
            get { return _AIM_STAR_LIST; }
            set { _AIM_STAR_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_ArrivalFeederLeg_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_ArrivalFeederLeg_LIST
        {
            get { return _AIM_ArrivalFeederLeg_LIST; }
            set { _AIM_ArrivalFeederLeg_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_ArrivalLeg_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_ArrivalLeg_LIST
        {
            get { return _AIM_ArrivalLeg_LIST; }
            set { _AIM_ArrivalLeg_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_DepartureLeg_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_DepartureLeg_LIST
        {
            get { return _AIM_DepartureLeg_LIST; }
            set { _AIM_DepartureLeg_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_FinalLeg_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_FinalLeg_LIST
        {
            get { return _AIM_FinalLeg_LIST; }
            set { _AIM_FinalLeg_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_InitialLeg_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_InitialLeg_LIST
        {
            get { return _AIM_InitialLeg_LIST; }
            set { _AIM_InitialLeg_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_IntermediateLeg_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_IntermediateLeg_LIST
        {
            get { return _AIM_IntermediateLeg_LIST; }
            set { _AIM_IntermediateLeg_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_MissedApproachLeg_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_MissedApproachLeg_LIST
        {
            get { return _AIM_MissedApproachLeg_LIST; }
            set { _AIM_MissedApproachLeg_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_AngleIndication_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_AngleIndication_LIST
        {
            get { return _AIM_AngleIndication_LIST; }
            set { _AIM_AngleIndication_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_DistanceIndication_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_DistanceIndication_LIST
        {
            get { return _AIM_DistanceIndication_LIST; }
            set { _AIM_DistanceIndication_LIST = value; }
        }

        private IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> _AIM_VerticalStructure_LIST;
        public IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> AIM_VerticalStructure_LIST
        {
            get { return _AIM_VerticalStructure_LIST; }
            set { _AIM_VerticalStructure_LIST = value; }
        }

        #endregion

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

        public ISpatialReference SpatialReference { get; set; }
        public string MapDocumentName { get; set; }
        public string CurProjectName { get; set; }

        private List<string> _CurrentFiltrsList;

        public List<string> CurrentFiltrsList
        {
            get { return _CurrentFiltrsList ?? (_CurrentFiltrsList = new List<string>()); }
            set { _CurrentFiltrsList = value; }
        }
        
        #endregion
       
        #region Layer

        public ILayer GetLinkedLayer(PDMObject sellObj)
        {
            var map = Environment.mapControl.Map;
            var tp = sellObj.GetType().Name;
            ILayer selectedLayer = null;

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
                    selectedLayer = EsriUtils.getLayerByName(map, "ProceduresLegs");

                    break;

                case("FacilityMakeUp"):
                    selectedLayer = EsriUtils.getLayerByName(map, "FacilityMakeUp");

                    break;
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
                var graphics = Environment.MapControl.CreateGraphics();

                var featureLayer = selectedLayer as IFeatureLayer;
                if (featureLayer == null) return null;
                var geoFeatureLayer = (IGeoFeatureLayer)featureLayer;
                var simpleRenderer = (ISimpleRenderer)geoFeatureLayer.Renderer;

                var symbol = simpleRenderer.Symbol;

                bitmap = EsriUtils.SymbolToBitmap(
                        symbol,
                        new Size(16, 16),
                        graphics,
                        ColorTranslator.ToWin32(Environment.FeatureTreeView.BackColor));

            }
            catch
            {
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
                    Environment.TreeViewImageList.Images.Add(bmp);
                    LayersSymbolInImageList.Add(pdmObj.GetType().Name, Environment.TreeViewImageList.Images.Count - 1);
                }
            }

            if (LayersSymbolInImageList.ContainsKey(pdmObj.GetType().Name)) res = LayersSymbolInImageList[pdmObj.GetType().Name];

            return res++;
        }

        #endregion

        private static IList<Aran.Temporality.Common.Abstract.State.AbstractState<Aran.Temporality.Common.Aim.MetaData.AimFeature>> GetData(Aran.Aim.FeatureType featureType)
        {
            if (!TossConnectionInited)
            {
                return null;

            }

            return CurrentDataContext.CurrentService.GetActualDataByDate(new Aran.Temporality.Common.Id.FeatureId { FeatureTypeId = (int)featureType }, false, DateTime.Now);



        }

        public void CloseToss()
        {
            TossConnectionInited = false;
            ConnectionProvider.Close();
        }

        public void OpenToss()
        {
            ConnectionProvider.MainAction = () =>
            {
                TossConnectionInited = true;
            };
            
            ConnectionProvider.Open();

            switch (CurrentProjectType)
            {
                case ArenaProjectType.ARENA:

                    #region

                    this.AIM_ADHP_LIST = GetData(Aran.Aim.FeatureType.AirportHeliport);
                    this.AIM_RWY_LIST = GetData(Aran.Aim.FeatureType.Runway);
                    this.AIM_RDN_LIST = GetData(Aran.Aim.FeatureType.RunwayDirection);
                    this.AIM_CLP_LIST = GetData(Aran.Aim.FeatureType.RunwayCentrelinePoint);
                    this.AIM_NVD_LIST = GetData(Aran.Aim.FeatureType.Navaid);
                    this.AIM_VOR_LIST = GetData(Aran.Aim.FeatureType.VOR);
                    this.AIM_DME_LIST = GetData(Aran.Aim.FeatureType.DME);
                    this.AIM_NDB_LIST = GetData(Aran.Aim.FeatureType.NDB);
                    this.AIM_TACAN_LIST = GetData(Aran.Aim.FeatureType.TACAN);
                    this.AIM_LOC_LIST = GetData(Aran.Aim.FeatureType.Localizer);
                    this.AIM_GLD_P_LIST = GetData(Aran.Aim.FeatureType.Glidepath);
                    this.AIM_WYP_LIST = GetData(Aran.Aim.FeatureType.DesignatedPoint);
                    this.AIM_ARSP_LIST = GetData(Aran.Aim.FeatureType.Airspace);
                    this.AIM_ENRT_LIST = GetData(Aran.Aim.FeatureType.Route);
                    this.AIM_ROUTE_LIST = GetData(Aran.Aim.FeatureType.RouteSegment);
                    this.AIM_IAP_LIST = GetData(Aran.Aim.FeatureType.InstrumentApproachProcedure);
                    this.AIM_STAR_LIST = GetData(Aran.Aim.FeatureType.StandardInstrumentArrival);
                    this.AIM_SID_LIST = GetData(Aran.Aim.FeatureType.StandardInstrumentDeparture);
                    this.AIM_ArrivalFeederLeg_LIST = GetData(Aran.Aim.FeatureType.ArrivalFeederLeg);
                    this.AIM_ArrivalLeg_LIST = GetData(Aran.Aim.FeatureType.ArrivalLeg);
                    this.AIM_DepartureLeg_LIST = GetData(Aran.Aim.FeatureType.DepartureLeg);
                    this.AIM_FinalLeg_LIST = GetData(Aran.Aim.FeatureType.FinalLeg);
                    this.AIM_InitialLeg_LIST = GetData(Aran.Aim.FeatureType.InitialLeg);
                    this.AIM_IntermediateLeg_LIST = GetData(Aran.Aim.FeatureType.IntermediateLeg);
                    this.AIM_MissedApproachLeg_LIST = GetData(Aran.Aim.FeatureType.MissedApproachLeg);
                    this.AIM_AngleIndication_LIST = GetData(Aran.Aim.FeatureType.AngleIndication);
                    this.AIM_DistanceIndication_LIST = GetData(Aran.Aim.FeatureType.DistanceIndication);
                    this.AIM_VerticalStructure_LIST = GetData(Aran.Aim.FeatureType.VerticalStructure);

                    #endregion

                    break;

                case ArenaProjectType.PANDA:

                    #region

                    this.AIM_ADHP_LIST = GetData(Aran.Aim.FeatureType.AirportHeliport);
                    this.AIM_RWY_LIST = GetData(Aran.Aim.FeatureType.Runway);
                    this.AIM_RDN_LIST = GetData(Aran.Aim.FeatureType.RunwayDirection);
                    this.AIM_CLP_LIST = GetData(Aran.Aim.FeatureType.RunwayCentrelinePoint);
                    this.AIM_NVD_LIST = GetData(Aran.Aim.FeatureType.Navaid);
                    this.AIM_VOR_LIST = GetData(Aran.Aim.FeatureType.VOR);
                    this.AIM_DME_LIST = GetData(Aran.Aim.FeatureType.DME);
                    this.AIM_NDB_LIST = GetData(Aran.Aim.FeatureType.NDB);
                    this.AIM_TACAN_LIST = GetData(Aran.Aim.FeatureType.TACAN);
                    this.AIM_LOC_LIST = GetData(Aran.Aim.FeatureType.Localizer);
                    this.AIM_GLD_P_LIST = GetData(Aran.Aim.FeatureType.Glidepath);
                    this.AIM_WYP_LIST = GetData(Aran.Aim.FeatureType.DesignatedPoint);
                    this.AIM_IAP_LIST = GetData(Aran.Aim.FeatureType.InstrumentApproachProcedure);
                    this.AIM_STAR_LIST = GetData(Aran.Aim.FeatureType.StandardInstrumentArrival);
                    this.AIM_SID_LIST = GetData(Aran.Aim.FeatureType.StandardInstrumentDeparture);
                    this.AIM_ArrivalFeederLeg_LIST = GetData(Aran.Aim.FeatureType.ArrivalFeederLeg);
                    this.AIM_ArrivalLeg_LIST = GetData(Aran.Aim.FeatureType.ArrivalLeg);
                    this.AIM_DepartureLeg_LIST = GetData(Aran.Aim.FeatureType.DepartureLeg);
                    this.AIM_FinalLeg_LIST = GetData(Aran.Aim.FeatureType.FinalLeg);
                    this.AIM_InitialLeg_LIST = GetData(Aran.Aim.FeatureType.InitialLeg);
                    this.AIM_IntermediateLeg_LIST = GetData(Aran.Aim.FeatureType.IntermediateLeg);
                    this.AIM_MissedApproachLeg_LIST = GetData(Aran.Aim.FeatureType.MissedApproachLeg);
                    this.AIM_AngleIndication_LIST = GetData(Aran.Aim.FeatureType.AngleIndication);
                    this.AIM_DistanceIndication_LIST = GetData(Aran.Aim.FeatureType.DistanceIndication);
                    this.AIM_VerticalStructure_LIST = GetData(Aran.Aim.FeatureType.VerticalStructure);

                    #endregion

                    break;

                default:

                    #region 

                    this.AIM_ADHP_LIST = GetData(Aran.Aim.FeatureType.AirportHeliport);
                    this.AIM_RWY_LIST = GetData(Aran.Aim.FeatureType.Runway);
                    this.AIM_RDN_LIST = GetData(Aran.Aim.FeatureType.RunwayDirection);
                    this.AIM_CLP_LIST = GetData(Aran.Aim.FeatureType.RunwayCentrelinePoint);
                    this.AIM_NVD_LIST = GetData(Aran.Aim.FeatureType.Navaid);
                    this.AIM_VOR_LIST = GetData(Aran.Aim.FeatureType.VOR);
                    this.AIM_DME_LIST = GetData(Aran.Aim.FeatureType.DME);
                    this.AIM_NDB_LIST = GetData(Aran.Aim.FeatureType.NDB);
                    this.AIM_TACAN_LIST = GetData(Aran.Aim.FeatureType.TACAN);
                    this.AIM_LOC_LIST = GetData(Aran.Aim.FeatureType.Localizer);
                    this.AIM_GLD_P_LIST = GetData(Aran.Aim.FeatureType.Glidepath);
                    this.AIM_WYP_LIST = GetData(Aran.Aim.FeatureType.DesignatedPoint);
                    this.AIM_ARSP_LIST = GetData(Aran.Aim.FeatureType.Airspace);
                    this.AIM_ENRT_LIST = GetData(Aran.Aim.FeatureType.Route);
                    this.AIM_ROUTE_LIST = GetData(Aran.Aim.FeatureType.RouteSegment);
                    this.AIM_IAP_LIST = GetData(Aran.Aim.FeatureType.InstrumentApproachProcedure);
                    this.AIM_STAR_LIST = GetData(Aran.Aim.FeatureType.StandardInstrumentArrival);
                    this.AIM_SID_LIST = GetData(Aran.Aim.FeatureType.StandardInstrumentDeparture);
                    this.AIM_ArrivalFeederLeg_LIST = GetData(Aran.Aim.FeatureType.ArrivalFeederLeg);
                    this.AIM_ArrivalLeg_LIST = GetData(Aran.Aim.FeatureType.ArrivalLeg);
                    this.AIM_DepartureLeg_LIST = GetData(Aran.Aim.FeatureType.DepartureLeg);
                    this.AIM_FinalLeg_LIST = GetData(Aran.Aim.FeatureType.FinalLeg);
                    this.AIM_InitialLeg_LIST = GetData(Aran.Aim.FeatureType.InitialLeg);
                    this.AIM_IntermediateLeg_LIST = GetData(Aran.Aim.FeatureType.IntermediateLeg);
                    this.AIM_MissedApproachLeg_LIST = GetData(Aran.Aim.FeatureType.MissedApproachLeg);
                    this.AIM_AngleIndication_LIST = GetData(Aran.Aim.FeatureType.AngleIndication);
                    this.AIM_DistanceIndication_LIST = GetData(Aran.Aim.FeatureType.DistanceIndication);
                    this.AIM_VerticalStructure_LIST = GetData(Aran.Aim.FeatureType.VerticalStructure);

                    #endregion
                   
                    break;
            }



        }

        public void ReadAIXM51Data(string AIXM_FileName)
        {

            var provider = DbProviderFactory.Create("Aran.Aim.Data.XmlProvider");
            provider.DefaultEffectiveDate = DateTime.Now;


            AObjectListConfig.IgnoreNotes = true;

            provider.Open(AIXM_FileName);

            var list = new[] { FeatureType.AirportHeliport};

            switch (CurrentProjectType)
            {
                case ArenaProjectType.ARENA:
                    list = new[] { FeatureType.AirportHeliport, FeatureType.Runway, FeatureType.RunwayDirection, FeatureType.RunwayCentrelinePoint, 
                               FeatureType.Navaid, FeatureType.DME, FeatureType.VOR,FeatureType.Localizer, FeatureType.Glidepath, FeatureType.NDB,FeatureType.MarkerBeacon,FeatureType.TACAN,FeatureType.DesignatedPoint,
                               FeatureType.VerticalStructure, FeatureType.Airspace, FeatureType.Route, FeatureType.RouteSegment,
                               FeatureType.InstrumentApproachProcedure,FeatureType.StandardInstrumentArrival, FeatureType.StandardInstrumentDeparture,
                               FeatureType.ArrivalFeederLeg,FeatureType.FinalLeg,FeatureType.InitialLeg,FeatureType.IntermediateLeg,FeatureType.MissedApproachLeg,FeatureType.ArrivalLeg,FeatureType.DepartureLeg,
                               FeatureType.AngleIndication, FeatureType.DistanceIndication};
                    break;
                case ArenaProjectType.PANDA:
                    list = new[] { FeatureType.AirportHeliport, FeatureType.Runway, FeatureType.RunwayDirection, FeatureType.RunwayCentrelinePoint, 
                               FeatureType.Navaid, FeatureType.DME, FeatureType.VOR,FeatureType.Localizer, FeatureType.Glidepath, FeatureType.NDB,FeatureType.MarkerBeacon,FeatureType.TACAN,FeatureType.DesignatedPoint,
                               FeatureType.VerticalStructure, FeatureType.InstrumentApproachProcedure,FeatureType.StandardInstrumentArrival, FeatureType.StandardInstrumentDeparture,
                               FeatureType.ArrivalFeederLeg,FeatureType.FinalLeg,FeatureType.InitialLeg,FeatureType.IntermediateLeg,FeatureType.MissedApproachLeg,FeatureType.ArrivalLeg,FeatureType.DepartureLeg,
                               FeatureType.AngleIndication, FeatureType.DistanceIndication};
                    break;
                default:
                    list = new[] { FeatureType.AirportHeliport, FeatureType.Runway, FeatureType.RunwayDirection, FeatureType.RunwayCentrelinePoint, 
                               FeatureType.Navaid, FeatureType.DME, FeatureType.VOR,FeatureType.Localizer, FeatureType.Glidepath, FeatureType.NDB,FeatureType.MarkerBeacon,FeatureType.TACAN,FeatureType.DesignatedPoint,
                               FeatureType.VerticalStructure, FeatureType.Airspace, FeatureType.Route, FeatureType.RouteSegment,
                               FeatureType.InstrumentApproachProcedure,FeatureType.StandardInstrumentArrival, FeatureType.StandardInstrumentDeparture,
                               FeatureType.ArrivalFeederLeg,FeatureType.FinalLeg,FeatureType.InitialLeg,FeatureType.IntermediateLeg,FeatureType.MissedApproachLeg,FeatureType.ArrivalLeg,FeatureType.DepartureLeg,
                               FeatureType.AngleIndication, FeatureType.DistanceIndication};
                    break;
            }


            //var list = new[] { FeatureType.AirportHeliport };

            this.Intermediate_AIXM51_Arena_Features = new List<Intermediate_AIXM51_Arena>();

            foreach (var featureType in list)
            {
                //get data from provider
                var result = provider.GetVersionsOf((FeatureType)featureType, TimeSliceInterpretationType.BASELINE);
                if (result.IsSucceed)
                {
                    if (result.List.Count > 0)
                    {

                        foreach (Aran.Aim.Features.Feature xml_data in result.List)
                        {
                            var myEvent = new AimEvent
                            {
                                WorkPackage = 0,
                                Interpretation = xml_data.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA ? Interpretation.TempDelta : Interpretation.PermanentDelta,
                                TimeSlice = new TimeSlice(DateTime.Now),
                                LifeTimeBegin = (xml_data.TimeSlice.FeatureLifetime != null) && (xml_data.TimeSlice.FeatureLifetime.BeginPosition <= DateTime.Now) ? xml_data.TimeSlice.FeatureLifetime.BeginPosition : DateTime.Now,
                                LifeTimeEnd = xml_data.TimeSlice.FeatureLifetime!=null ? xml_data.TimeSlice.FeatureLifetime.EndPosition : DateTime.Now,
                                Data = xml_data,
                            };


                            myEvent.Data.InitEsriExtension();
                            var geoDataList = myEvent.Data.PropertyExtensions.OfType<EsriPropertyExtension>().Select(t => t.EsriObject).ToList();

                            this.Intermediate_AIXM51_Arena_Features.Add( new Intermediate_AIXM51_Arena { AIXM51_Feature = xml_data, AixmGeo = geoDataList });
                        }


                    }
                }
            }



        }

    }
}
