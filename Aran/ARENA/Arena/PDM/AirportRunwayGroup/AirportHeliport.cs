using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
//using System.Diagnostics.Debug;
using PDM.PropertyExtension;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geometry;
using System.Xml.Serialization;

namespace PDM
{

     [Serializable()]
    [TypeConverter(typeof(PropertySorter))]
    public class AirportHeliport : PDMObject
    {

        private string _designator;
        [Description("The four letter ICAO code of the aerodrome.")]
        [Category("Airport")]
        [PropertyOrder(10)]
        [Mandatory(true)]
        public string Designator
        {
            get { return _designator; }
            set { _designator = value; }
        }

        private string _designatorIATA;
        [PropertyOrder(20)]
        [Category("Airport")]
        [Description("The three letter IATA code of the aerodrome.")]
        public string DesignatorIATA
        {
            get { return _designatorIATA; }
            set { _designatorIATA = value; }
        }

        private string _name;
        [Description("The full free text name of the aerodrome.")]
        [Category("Airport")]
        [PropertyOrder(30)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _servedCity;
        [Description("The location that is served by the airport.")]
        [Category("Airport")]
        [PropertyOrder(35)]
        public string ServedCity
        {
            get { return _servedCity; }
            set { _servedCity = value; }
        }

        private AirportHeliportType _airportHeliportType;
        [Category("Airport")]
        [PropertyOrder(36)]
        public AirportHeliportType AirportHeliportType
        {
            get { return _airportHeliportType; }
            set { _airportHeliportType = value; }
        }


        private bool _certifiedICAO;
        [Category("Airport")]
        [PropertyOrder(37)]
        public bool CertifiedICAO { get => _certifiedICAO; set => _certifiedICAO = value; }

        private double? _magneticVariation = null;
        [Description("The angular difference between True North and Magnetic North.")]
        [Category("Magnetic Variation")]
        [PropertyOrder(40)]
        [Mandatory(true)]
        public double? MagneticVariation
        {
            get { return _magneticVariation; }
            set { _magneticVariation = value; }
        }

        private double? _magneticVariationChange = null;
        [Description("The annual rate of change of the magnetic variation")]
        [Category("Magnetic Variation")]
        [PropertyOrder(45)]
        [Mandatory(true)]
        public double? MagneticVariationChange
        {
            get { return _magneticVariationChange; }
            set { _magneticVariationChange = value; }
        }

        private string _dateMagneticVariation;
        [Description("The date on which the magnetic variation had this value.")]
        [PropertyOrder(50)]
        [Category("Magnetic Variation")]
        [Mandatory(false)]
        public string DateMagneticVariation
        {
            get { return _dateMagneticVariation; }
            set { _dateMagneticVariation = value; }
        }

        private double? _transitionAltitude = null;
        [PropertyOrder(50)]
        [Description("The value of the transition altitude.")]
        public double? TransitionAltitude
        {
            get { return _transitionAltitude; }
            set { _transitionAltitude = value; }
        }

        private UOM_DIST_VERT _transitionAltitudeUOM;
        [PropertyOrder(60)]
        [Description("The value of the transition altitude.")]
        public UOM_DIST_VERT TransitionAltitudeUOM
        {
            get { return _transitionAltitudeUOM; }
            set { _transitionAltitudeUOM = value; }
        }



        public CodeMilitaryOperationsType ControlType { get; set; }

        private List<Runway> _runwayList;
        [Description("Runways")]
        [Browsable(false)]
        [PropertyOrder(70)]
        public List<Runway> RunwayList
        {
            get { return _runwayList; }
            set { _runwayList = value; }
        }

        //[ReadOnly(true)]
        //public string Country { get; set; }

        public List<NavigationSystemCheckpoint> NavSystemCheckpoints { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public IGeometry Extent { get; set; }

        private List<Apron> _apronList;
        [Description("Aprons")]
        //[Browsable(false)]
        [PropertyOrder(70)]
        public List<Apron> ApronList 
        {
            get { return _apronList; }
            set { _apronList = value; }
        }

        
        [Description("Roads")]
        [Browsable(false)]
        [PropertyOrder(70)]
        public List<Road> RoadList { get; set; }

        [Description("Units")]
        [Browsable(false)]
        [PropertyOrder(70)]
        public List<Unit> UnitList { get; set; }

        [Description("NonMovementArea")]
        [Browsable(false)]
        [PropertyOrder(70)]
        public List<NonMovementArea> NonMovementAreaList { get; set; }


        private double? _referenceTemperature;
         [Category("Reference Temperature")]
         [PropertyOrder(80)]
        public double? ReferenceTemperature
        {
            get { return _referenceTemperature; }
            set { _referenceTemperature = value; }
        }

         private Uom_Temperature _temperatureUOM;
         [Category("Reference Temperature")]
         [PropertyOrder(90)]
         public Uom_Temperature TemperatureUOM
         {
             get { return _temperatureUOM; }
             set { _temperatureUOM = value; }
         }

        private List<RadioCommunicationChanel> _communicationChanels;
        public List<RadioCommunicationChanel> CommunicationChanels
        {
            get => _communicationChanels;
            set => _communicationChanels = value;
        }

       
        private List<Taxiway> _taxiwayList;
        [Description("Taxiways")]
        //[Browsable(false)]
        [PropertyOrder(110)]
        public List<Taxiway> TaxiwayList
        {
            get { return _taxiwayList; }
            set { _taxiwayList = value; }
        }

        private List<AirportHotSpot> _airportHotSpotList;
        [Description("AirportHotSpot")]
        //[Browsable(false)]
        [PropertyOrder(110)]
        public List<AirportHotSpot> AirportHotSpotList
        {
            get { return _airportHotSpotList; }
            set { _airportHotSpotList = value; }
        }

        private List<TouchDownLiftOff> _touchDownLiftOffList;
        [Description("TouchDownLiftOff")]
        //[Browsable(false)]
        [PropertyOrder(110)]
        public List<TouchDownLiftOff> TouchDownLiftOffList
        {
            get { return _touchDownLiftOffList; }
            set { _touchDownLiftOffList = value; }
        }

        public List<WorkArea> WorkAreaList { get; set; }

        
        [Browsable(false)]
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.AirportHeliport;
            }
        }

        private string _organisationAuthority;
        [ReadOnly(true)]
        public string OrganisationAuthority
        {
            get { return _organisationAuthority; }
            set { _organisationAuthority = value; }
        }


        public AirportHeliport()
        {
            
        }

        public override bool StoreToDB(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
           bool res =true;
           try
           {
                if (!AIRTRACK_TableDic.ContainsKey(this.GetType())) return false;
                ITable tbl = AIRTRACK_TableDic[this.GetType()];
                if (EsriWorkEnvironment.EsriUtils.RowExist(tbl, this.ID)) return true;


               IRow row = tbl.CreateRow();

               CompileRow(ref row);

               row.Store();


                if (this.RunwayList != null)
                {

                    foreach (Runway rwy in this.RunwayList)
                    {
                        rwy.ID_AirportHeliport = this.ID;
                        rwy.StoreToDB(AIRTRACK_TableDic);

                    }
                }

                if (this.TaxiwayList != null)
                {

                    foreach (Taxiway twy in this.TaxiwayList)
                    {
                        twy.ID_AirportHeliport = this.ID;
                        twy.StoreToDB(AIRTRACK_TableDic);

                    }
                }

                if (this.ApronList != null)
                {

                    foreach (Apron apron in this.ApronList)
                    {
                        apron.ID_AirportHeliport = this.ID;
                        apron.StoreToDB(AIRTRACK_TableDic);

                    }
                }

                if (this.NavSystemCheckpoints != null)
                {

                    foreach (NavigationSystemCheckpoint nav in this.NavSystemCheckpoints)
                    {
                        nav.ID_AirportHeliport = this.ID;
                        if (nav is CheckpointVOR) ((CheckpointVOR)nav).StoreToDB(AIRTRACK_TableDic);
                        //if (nav is CheckpointINS) ((CheckpointINS)nav).StoreToDB(AIRTRACK_TableDic);

                    }
                }

                if (this.AirportHotSpotList != null)
                {

                    foreach (AirportHotSpot airHotSpot in this.AirportHotSpotList)
                    {
                        airHotSpot.ID_AirportHeliport = this.ID;
                        airHotSpot.StoreToDB(AIRTRACK_TableDic);

                    }
                }

                if (this.TouchDownLiftOffList != null)
                {

                    foreach (TouchDownLiftOff touchDown in this.TouchDownLiftOffList)
                    {
                        touchDown.ID_AirportHeliport = this.ID;
                        touchDown.StoreToDB(AIRTRACK_TableDic);

                    }
                }

                if (this.WorkAreaList != null)
                {

                    foreach (WorkArea workArea in this.WorkAreaList)
                    {
                        workArea.ID_AirportHeliport = this.ID;
                        workArea.StoreToDB(AIRTRACK_TableDic);

                    }
                }

                if (this.RoadList != null)
                {

                    foreach (Road road in this.RoadList)
                    {
                        road.ID_AirportHeliport = this.ID;
                        road.StoreToDB(AIRTRACK_TableDic);

                    }
                }

                if (this.UnitList != null)
                {

                    foreach (Unit unit in this.UnitList)
                    {
                        unit.ID_AirportHeliport = this.ID;
                        unit.StoreToDB(AIRTRACK_TableDic);

                    }
                }

                if (this.NonMovementAreaList != null)
                {

                    foreach (NonMovementArea unit in this.NonMovementAreaList)
                    {
                        unit.ID_AirportHeliport = this.ID;
                        unit.StoreToDB(AIRTRACK_TableDic);

                    }
                }

                if (this.CommunicationChanels != null)
                {

                    foreach (RadioCommunicationChanel radCom in this.CommunicationChanels)
                    {
                        radCom.StoreToDB(AIRTRACK_TableDic);
                    }
                }

                if (this.Extent != null)
                {
                    int findx = -1;
                    ITable tblGeoPnt = AIRTRACK_TableDic[typeof(AirportHeliportExtent)];
                    IRow rowGeoExtent = tblGeoPnt.CreateRow();
                    findx = -1;
                    findx = rowGeoExtent.Fields.FindField("FeatureGUID");
                    if (findx >= 0) rowGeoExtent.set_Value(findx, this.ID);

                    findx = rowGeoExtent.Fields.FindField("Shape");
                    rowGeoExtent.set_Value(findx, this.Extent);

                    rowGeoExtent.Store();
                }

            }
           catch(Exception ex)
           {
               this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace };
               res = false;
           }

            
          
           return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            //findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, "f");
            findx = row.Fields.FindField("designator"); if (findx >= 0) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("designatorIATA"); if (findx >= 0) row.set_Value(findx, this.DesignatorIATA);
            findx = row.Fields.FindField("magneticVariation"); if (findx >= 0 && this.MagneticVariation.HasValue) row.set_Value(findx, this.MagneticVariation);
            findx = row.Fields.FindField("name"); if (findx >= 0) row.set_Value(findx, this.Name);
            findx = row.Fields.FindField("Airport_ReferencePt_Latitude"); if (findx >= 0) row.set_Value(findx, this.Lat);
            findx = row.Fields.FindField("Airport_ReferencePt_Longitude"); if (findx >= 0) row.set_Value(findx, this.Lon);
            findx = row.Fields.FindField("Elev"); if (findx >= 0 && this.Elev.HasValue) row.set_Value(findx, this.Elev);
            findx = row.Fields.FindField("GUND"); if (findx >= 0 && this.GeoProperties.GeoidUndulation.HasValue) row.set_Value(findx, this.GeoProperties.GeoidUndulation);
            findx = row.Fields.FindField("ElevUom"); if (findx >= 0) row.set_Value(findx, this.Elev_UOM.ToString());
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            findx = row.Fields.FindField("AirportHeliportType"); if (findx >= 0) row.set_Value(findx, this.AirportHeliportType.ToString());
            findx = row.Fields.FindField("ControlType"); if (findx >= 0) row.set_Value(findx, this.ControlType.ToString());


            if (this.Geo != null)
            {
                findx = row.Fields.FindField("LonCoord"); if (findx >= 0) row.set_Value(findx, this.X_to_EW_DDMMSS());
                findx = row.Fields.FindField("LatCoord"); if (findx >= 0) row.set_Value(findx, this.Y_to_NS_DDMMSS());

                //findx = row.Fields.FindField("LonCoord"); if (findx >= 0) row.set_Value(findx, this.X_to_DDMMSS_EW_());
                //findx = row.Fields.FindField("LatCoord"); if (findx >= 0) row.set_Value(findx, this.Y_to_DDMMSS_NS());
                findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);

            }

        }

        public override string GetObjectLabel()
        {
            return "ADHP " + this.Designator;
        }

        public override int DeleteObject(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {

            if (base.DeleteObject(AIRTRACK_TableDic) == 0) return 0;
            if (AIRTRACK_TableDic.ContainsKey(this.GetType())) return 0;
            ITable tbl = AIRTRACK_TableDic[this.GetType()];


            if (this.RunwayList != null)
            {

                foreach (Runway rwy in this.RunwayList)
                {
                    if (rwy.RunwayDirectionList != null)
                    {
                        foreach (RunwayDirection rdn in rwy.RunwayDirectionList)
                        {
                            if (rdn.Related_NavaidSystem != null)
                            {
                                foreach (NavaidSystem ns in rdn.Related_NavaidSystem)
                                {
                                    ns.DeleteObject(AIRTRACK_TableDic);
                                }
                            }
                        }
                    }
                }
            }

            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)(tbl as FeatureClass).Workspace;

            if (!workspaceEdit.IsBeingEdited())
            {
                workspaceEdit.StartEditing(false);
                workspaceEdit.StartEditOperation();
            }
            
            IQueryFilter qry = new QueryFilterClass();
            qry.WhereClause = "FeatureGUID = '" + this.ID + "'";
            tbl.DeleteSearchedRows(qry);
            
            Marshal.ReleaseComObject(qry);

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            return 1;
        }

        public override List<string> HideBranch(Dictionary<Type, ITable> AIRTRACK_TableDic, bool Visibility)
        {

            List<string> res = base.HideBranch(AIRTRACK_TableDic, Visibility);

            if (this.RunwayList != null)
            {

                foreach (Runway rwy in this.RunwayList)
                {
                  List<string> part=  rwy.HideBranch(AIRTRACK_TableDic, Visibility);
                  res.AddRange(part);
                }
            }

            return res;
        }

        public override List<string> GetBranch(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            List<string> res = base.GetBranch(AIRTRACK_TableDic);

            if (this.RunwayList != null)
            {

                foreach (Runway rwy in this.RunwayList)
                {
                    List<string> part = rwy.GetBranch(AIRTRACK_TableDic);
                    res.AddRange(part);
                }
            }

            return res;
        }

        public override string ToString()
        {
            return "ADHP " + this.Designator;
        }

        public override bool CompareId(string AnotherID)
        {
            bool res = base.CompareId(AnotherID);

            if (this.RunwayList != null)
                foreach (var item in this.RunwayList)
                {
                    res = res || item.CompareId(AnotherID);
                }

            if (this.TaxiwayList != null)
                foreach (var item in this.TaxiwayList)
                {
                    res = res || item.CompareId(AnotherID);
                }

            return res;
        }
        

    }

    public class AirportHeliportExtent
    {

    }

}
