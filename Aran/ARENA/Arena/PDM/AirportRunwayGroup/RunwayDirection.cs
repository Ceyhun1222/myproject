using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
using PDM.PropertyExtension;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using AranSupport;

namespace PDM
{
    [Serializable()]
    [TypeConverter(typeof(PropertySorter))]
    public class RunwayDirection : PDMObject
    {
        private string _ID_Runway;
        [Browsable(false)]
        public string ID_Runway
        {
            get { return _ID_Runway; }
            set { _ID_Runway = value; }
        }

        private string _ID_AirportHeliport;
        [Browsable(false)]
        public string ID_AirportHeliport
        {
            get { return _ID_AirportHeliport; }
            set { _ID_AirportHeliport = value; }
        }

        private string _designator;
        [Mandatory(true)]
        [PropertyOrder(10)]
        [Description("The full textual designator of the landing and take-off direction.")]
        public string Designator
        {
            get { return _designator; }
            set { _designator = value; }
        }

        private double? _truebearing = null;
        [Mandatory(true)]
        [PropertyOrder(20)]
        [Description("The true bearing of the Runway direction.")]
        public double? TrueBearing
        {
            get { return _truebearing; }
            set { _truebearing = value; }
        }

        private double? _magBearing = null;
        [Mandatory(true)]
        [PropertyOrder(30)]
        [Description("The magnetic bearing.")]
        public double? MagBearing
        {
            get { return _magBearing; }
            set { _magBearing = value; }
        }

        private double? _landingThresholdElevation = null;
        [Mandatory(true)]
        [PropertyOrder(40)]
        [Description("The value of the elevation of the RDN.")]
        public double? LandingThresholdElevation
        {
            get { return _landingThresholdElevation; }
            set { _landingThresholdElevation = value; }
        }

        private double _displacedThresholdDistance;
        [PropertyOrder(50)]
        [Description("The value of the displaced threshold distance.")]
        public double DisplacedThresholdDistance
        {
            get { return _displacedThresholdDistance; }
            set { _displacedThresholdDistance = value; }
        }

        private double? _ClearWay = null;
        [PropertyOrder(50)]
        public double? ClearWayLength
        {
            get { return _ClearWay; }
            set { _ClearWay = value; }
        }

        private double? _ClearWayWidth = null;
        [PropertyOrder(50)]
        public double? ClearWayWidth
        {
            get { return _ClearWayWidth; }
            set { _ClearWayWidth = value; }
        }

        private List<DeclaredDistance> _RdnDeclaredDistance;
        public List<DeclaredDistance> RdnDeclaredDistance
        {
            get { return _RdnDeclaredDistance; }
            set { _RdnDeclaredDistance = value; }
        }

        private double? _stopway = null;
        [PropertyOrder(60)]
        [Description("The value of the stopway.")]
        public double? Stopway
        {
            get { return _stopway; }
            set { _stopway = value; }
        }

        private UOM_DIST_HORZ _uom;
        [Mandatory(true)]
        [PropertyOrder(70)]
        [Description("The unit of measurement for the horizontal dimensions of the Runway direction.")]
        public UOM_DIST_HORZ Uom
        {
            get { return _uom; }
            set { _uom = value; }
        }

        private List<NavaidSystem> _Related_NavaidSystem;
        [Browsable(false)]
        public List<NavaidSystem> Related_NavaidSystem
        {
            get { return _Related_NavaidSystem; }
            set { _Related_NavaidSystem = value; }
        }



        private List<RunwayCenterLinePoint> _CenterLinePoints;
        [Browsable(false)]
        public List<RunwayCenterLinePoint> CenterLinePoints
        {
            get { return _CenterLinePoints; }
            set { _CenterLinePoints = value; }
        }

        private List<RunwayProtectArea> _RwyProtectArea;
        //[Browsable(false)]
        public List<RunwayProtectArea> RwyProtectArea
        {
            get { return _RwyProtectArea; }
            set { _RwyProtectArea = value; }
        }

        private List<RunwayVisualRange> _RwyVisualRange;
        //[Browsable(false)]
        public List<RunwayVisualRange> RwyVisualRange
        {
            get { return _RwyVisualRange; }
            set { _RwyVisualRange = value; }
        }

        private ApproachLightingSystem _RdnLightSystem;
        [DisplayName("Lighting system")]
        public ApproachLightingSystem RdnLightSystem { get => _RdnLightSystem; set => _RdnLightSystem = value; }

        private VisualGlideSlopeIndicator _VisualGlideSlope;
        [DisplayName("VisualGlideSlopeIndicator")]
        public VisualGlideSlopeIndicator VisualGlideSlope { get => _VisualGlideSlope; set => _VisualGlideSlope = value; }

        [Browsable(false)]
        public override PDM_ENUM PDM_Type => PDM_ENUM.RunwayDirection;


        public RunwayDirection()
        {
            //throw new System.NotImplementedException();
        }
  
        public override bool StoreToDB(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {
                if (!AIRTRACK_TableDic.ContainsKey(this.GetType())) return false;
                ITable tbl = AIRTRACK_TableDic[this.GetType()];

                if (EsriWorkEnvironment.EsriUtils.RowExist(tbl, this.ID)) return true;

                IRow row = tbl.CreateRow();

                CompileRow(ref row);

                row.Store();

                if (this.Related_NavaidSystem != null)
                {

                    foreach (NavaidSystem nvd in this.Related_NavaidSystem)
                    {
                        nvd.ID_AirportHeliport = this.ID_AirportHeliport;
                        nvd.ID_RunwayDirection = this.ID;
                        nvd.StoreToDB(AIRTRACK_TableDic);
                    }
                }

                if (this.CenterLinePoints != null)
                {

                    foreach (RunwayCenterLinePoint clp in this.CenterLinePoints)
                    {
                        clp.ID_RunwayDirection = this.ID;
                        clp.StoreToDB(AIRTRACK_TableDic);
                    }
                }

                if (this.RwyProtectArea != null)
                {

                    foreach (RunwayProtectArea rpa in this.RwyProtectArea)
                    {
                        rpa.ID_RunwayDirection = this.ID;
                        rpa.StoreToDB(AIRTRACK_TableDic);
                    }
                }

                if (this.RwyVisualRange != null)
                {

                    foreach (RunwayVisualRange rvr in this.RwyVisualRange)
                    {
                        rvr.StoreToDB(AIRTRACK_TableDic);
                    }
                }

                if (this.RdnLightSystem != null)
                {
                    this.RdnLightSystem.RunwayDirection_ID = this.ID;
                    this.RdnLightSystem.StoreToDB(AIRTRACK_TableDic);

                }

                if (this.VisualGlideSlope != null)
                {
                    this.VisualGlideSlope.ID_RunwayDirection = this.ID;
                    this.VisualGlideSlope.StoreToDB(AIRTRACK_TableDic);

                }

            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;}
            
           return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("ID_Runway"); if (findx >= 0) row.set_Value(findx, this.ID_Runway);
            findx = row.Fields.FindField("ID_AirportHeliport"); if (findx >= 0) row.set_Value(findx, this.ID_AirportHeliport);
            findx = row.Fields.FindField("designator"); if (findx >= 0) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("magBearing"); if (findx >= 0 && this.MagBearing.HasValue) row.set_Value(findx, this.MagBearing);
            findx = row.Fields.FindField("trueBearing"); if (findx >= 0 && this.TrueBearing.HasValue) row.set_Value(findx, this.TrueBearing);
            findx = row.Fields.FindField("landingThresholdElevation"); if (findx >= 0 && this.LandingThresholdElevation.HasValue) row.set_Value(findx, this.LandingThresholdElevation);
            //findx = row.Fields.FindField("displacedThresholdDistance"); if (findx >= 0) row.set_Value(findx, this.DisplacedThresholdDistance);
            findx = row.Fields.FindField("stopway"); if (findx >= 0 && this.Stopway.HasValue) row.set_Value(findx, this.Stopway);
            findx = row.Fields.FindField("Lat"); if (findx >= 0) row.set_Value(findx, this.Lat);
            findx = row.Fields.FindField("Lon"); if (findx >= 0) row.set_Value(findx, this.Lon);
            findx = row.Fields.FindField("Uom"); if (findx >= 0) row.set_Value(findx, this.Uom.ToString());
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);
            if (this.Geo != null)
            {
                findx = row.Fields.FindField("LatCoord"); if (findx >= 0) row.set_Value(findx, this.Y_to_NS_DDMMSS());
                findx = row.Fields.FindField("LonCoord"); if (findx >= 0) row.set_Value(findx, this.X_to_EW_DDMMSS());

                //findx = row.Fields.FindField("LatCoord"); if (findx >= 0) row.set_Value(findx, this.Y_to_DDMMSS_NS());
                //findx = row.Fields.FindField("LonCoord"); if (findx >= 0) row.set_Value(findx, this.X_to_DDMMSS_EW_());

                findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);

            }
        }

        public override string X_to_EW_DDMMSS()
        {
            string res = "";

            try
            {
                if (this.Geo.GeometryType != esriGeometryType.esriGeometryPoint) return res;
                if (this.Geo.IsEmpty) return res;

                double Coord = ((IPoint)this.Geo).X;
                this.X = ((IPoint)this.Geo).X;

                string sign = "E";
                if (Coord < 0)
                {
                    sign = "W";
                    Coord = Math.Abs(Coord);
                }

                double X = Math.Round(Coord, 10);

                int deg = (int)X;
                double delta = Math.Round((X - deg) * 60, 8);

                int min = (int)delta;
                delta = Math.Round((delta - min) * 60, 2);

                string degSTR = "0";
                string minSTR = "0";
                string secSTR = "0";

                degSTR = deg < 10 ? "0" + degSTR : "0";
                degSTR = deg < 100 ? degSTR + deg.ToString() : deg.ToString();
                minSTR = min < 10 ? minSTR + min.ToString() : min.ToString();
                secSTR = delta < 10 ? secSTR + delta.ToString() : delta.ToString();

                this.Lon = degSTR + minSTR + secSTR + sign;

                res = sign + degSTR + minSTR + secSTR;

                //res = degSTR + "°" + minSTR + "'" + secSTR + "'" + "'" + sign;

            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = ""; }
            return res;
        }

        public override string Y_to_NS_DDMMSS()
        {

            string res = "";

            try
            {
                if (this.Geo.GeometryType != esriGeometryType.esriGeometryPoint) return res;
                if (this.Geo.IsEmpty) return res;

                double Coord = ((IPoint)this.Geo).Y;
                this.Y = ((IPoint)this.Geo).Y;

                string sign = "N";
                if (Coord < 0)
                {
                    sign = "S";
                    Coord = Math.Abs(Coord);
                }

                double Y = Math.Round(Coord, 10);
                //X = RealMode(X, 360);

                int deg = (int)Y;
                double delta = Math.Round((Y - deg) * 60, 8);

                int min = (int)delta;
                delta = Math.Round((delta - min) * 60, 2);


                string degSTR = "0";
                string minSTR = "0";
                string secSTR = "0";

                degSTR = deg < 10 ? degSTR + deg.ToString() : deg.ToString();
                minSTR = min < 10 ? minSTR + min.ToString() : min.ToString();
                secSTR = delta < 10 ? secSTR + delta.ToString() : delta.ToString();

                this.Lat = degSTR + minSTR + secSTR + sign;

                res = sign + degSTR + minSTR + secSTR;
            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = ""; }

            return res;

        }

        public override string GetObjectLabel()
        {
            return "Rdn " + this.Designator;
        }

        public override int DeleteObject(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            if (AIRTRACK_TableDic.ContainsKey(this.GetType())) return 0;
            ITable tbl = AIRTRACK_TableDic[this.GetType()];



            if (this.Related_NavaidSystem != null)
            {
                foreach (NavaidSystem ns in this.Related_NavaidSystem)
                {
                    ns.DeleteObject(AIRTRACK_TableDic);
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

             if (this.Related_NavaidSystem != null)
             {

                 foreach (NavaidSystem nvd in this.Related_NavaidSystem)
                 {
                     List<string> part =nvd.HideBranch(AIRTRACK_TableDic, Visibility);
                     res.AddRange(part);
                 }
             }

             return res;
        }

        public override List<string> GetBranch(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            List<string> res = base.GetBranch(AIRTRACK_TableDic);

            if (this.Related_NavaidSystem != null)
            {

                foreach (NavaidSystem nvd in this.Related_NavaidSystem)
                {
                    List<string> part = nvd.GetBranch(AIRTRACK_TableDic);
                    res.AddRange(part);
                }
            }

            return res;
        }

        public override string ToString()
        {
            return "Rdn " + this.Designator;
        }


        [DisplayName("Elev TdZ")]
        public override double? Elev
        {
            get
            {
                return base.Elev;
            }
            set
            {
                base.Elev = value;
            }
        }

        [DisplayName("Elev TdZ UOM")]
        public override UOM_DIST_VERT Elev_UOM
        {
            get
            {
                return base.Elev_UOM;
            }
            set
            {
                base.Elev_UOM = value;
            }
        }

       

        public override bool CompareId(string AnotherID)
        {
            bool res = base.CompareId(AnotherID);

            if (this.Related_NavaidSystem != null)
                foreach (var item in Related_NavaidSystem)
                {
                    res = res || item.CompareId(AnotherID);
                }

            if (this.CenterLinePoints != null)
                foreach (var item in CenterLinePoints)
                {
                    res = res || item.CompareId(AnotherID);
                }

            return res;
        }
       
    }

    [Serializable()]
    public class DeclaredDistance
    {
        private CodeDeclaredDistance _distanceType;
        public CodeDeclaredDistance DistanceType
        {
            get { return _distanceType; }
            set { _distanceType = value; }
        }

        private double _distanceValue;
        public double DistanceValue
        {
            get { return _distanceValue; }
            set { _distanceValue = value; }
        }

        private UOM_DIST_HORZ _distanceUOM;
        public UOM_DIST_HORZ DistanceUOM
        {
            get { return _distanceUOM; }
            set { _distanceUOM = value; }
        }

        public DeclaredDistance()
        {
        }

        public override string ToString()
        {
            return this.DistanceType.ToString();
        }
    }

    [Serializable()]
    public class RunwayElement : PDMObject
    {
        private string _ID_Runway;
        [Browsable(false)]
        public string ID_Runway
        {
            get { return _ID_Runway; }
            set { _ID_Runway = value; }
        }

        private string _designator;
        [Description("The full textual designator of the runway, used to uniquely identify it at an aerodrome which has more than one.")]
        [Mandatory(true)]
        [PropertyOrder(20)]
        public string Designator
        {
            get { return _designator; }
            set { _designator = value; }
        }

        private string ID_AirportHeliport;
        [Browsable(false)]
        public string ID_AirportHeliport1
        {
            get { return ID_AirportHeliport; }
            set { ID_AirportHeliport = value; }
        }

        private string _codeComposition;
        public string CodeComposition
        {
            get { return _codeComposition; }
            set { _codeComposition = value; }
        }

        private double? _length;
        public double? Length
        {
            get { return _length; }
            set { _length = value; }
        }

        private double? width;
        public double? Width
        {
            get { return width; }
            set { width = value; }
        }

        private UOM_DIST_HORZ _lengthUom;
        public UOM_DIST_HORZ LengthUom
        {
            get { return _lengthUom; }
            set { _lengthUom = value; }
        }

        private UOM_DIST_HORZ _widthUom;
        public UOM_DIST_HORZ WidthUom
        {
            get { return _widthUom; }
            set { _widthUom = value; }
        }

        private CodeRunwayElementType _runwayElementType;
        public CodeRunwayElementType RunwayElementType
        {
            get { return _runwayElementType; }
            set { _runwayElementType = value; }
        }

        [Browsable(false)]
        public override double? Elev
        {
            get
            {
                return base.Elev;
            }
            set
            {
                base.Elev = value;
            }
        }

        [Browsable(false)]
        public override UOM_DIST_VERT Elev_UOM
        {
            get
            {
                return base.Elev_UOM;
            }
            set
            {
                base.Elev_UOM = value;
            }
        }

        [Browsable(false)]
        public override string Lat
        {
            get
            {
                return base.Lat;
            }
            set
            {
                base.Lat = value;
            }
        }

        [Browsable(false)]
        public override string Lon
        {
            get
            {
                return base.Lon;
            }
            set
            {
                base.Lon = value;
            }
        }


        private byte[] _rwyGeometry;

        [Browsable(false)]
        public byte[] RwyGeometry
        {
            get { return _rwyGeometry; }
            set { _rwyGeometry = value; }
        }

        public SurfaceCharacteristics SurfaceProperties { get; set; }

        public override PDM_ENUM PDM_Type => PDM_ENUM.RunwayElement;

        public override void RebuildGeo()
        {

            //string[] words = ((string)this.RwyGeometry).Split(':');

            //byte[] bytes = new byte[words.Length];

            //for (int i = 0; i <= words.Length - 2; i++) bytes[i] = Convert.ToByte(words[i]);


            //// сконвертируем его в геометрию 
            //IMemoryBlobStream memBlobStream = new MemoryBlobStream();

            //IMemoryBlobStreamVariant varBlobStream = (IMemoryBlobStreamVariant)memBlobStream;

            //varBlobStream.ImportFromVariant(bytes);

            //IObjectStream anObjectStream = new ObjectStreamClass();
            //anObjectStream.Stream = memBlobStream;

            //IPropertySet aPropSet = new PropertySetClass();

            //IPersistStream aPersistStream = (IPersistStream)aPropSet;
            //aPersistStream.Load(anObjectStream);

            //this.Geo = aPropSet.GetProperty("Border") as IGeometry;

            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            this.Geo = ArnUtil.GetGeometry(this.ID, this.PDM_Type.ToString(), ArenaStatic.ArenaStaticProc.GetTargetDB());

            if (this.Geo == null && this.RwyGeometry != null)
            {
                this.Geo = (IGeometry)HelperClass.GetObjectFromBlob(this.RwyGeometry, "Border");
            }

        }

        public override string GetObjectLabel()
        {
            string res = this.CodeComposition;
            res = this.Length.HasValue ? res + "L" + this.Length.Value.ToString() + this.LengthUom.ToString() : "";
            res = this.Width.HasValue ? res + "W" + this.Width.Value.ToString() + this.WidthUom.ToString() : "";

            return base.GetObjectLabel();
        }

        public override bool StoreToDB(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {
                if (!AIRTRACK_TableDic.ContainsKey(this.GetType())) return false;
                ITable tbl = AIRTRACK_TableDic[this.GetType()];

                if (EsriWorkEnvironment.EsriUtils.RowExist(tbl, this.ID)) return true;

                IRow row = tbl.CreateRow();


                CompileRow(ref row);

                row.Store();

                SurfaceProperties?.StoreToDB(AIRTRACK_TableDic);

            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }
            {
                res = false;
            }

            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("ID_AirportHeliport"); if (findx >= 0) row.set_Value(findx, this.ID_AirportHeliport);//
            findx = row.Fields.FindField("ID_Runway"); if (findx >= 0) row.set_Value(findx, this.ID_Runway);
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("Designator"); if (findx >= 0) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("Length"); if (findx >= 0 && this.Length.HasValue) row.set_Value(findx, this.Length.Value);
            findx = row.Fields.FindField("Width"); if (findx >= 0 && this.Width.HasValue) row.set_Value(findx, this.Width.Value);
            findx = row.Fields.FindField("widthUom"); if (findx >= 0 && this.Width.HasValue) row.set_Value(findx, this.WidthUom.ToString());
            findx = row.Fields.FindField("lengthUom"); if (findx >= 0 && this.Length.HasValue) row.set_Value(findx, this.LengthUom.ToString());
            findx = row.Fields.FindField("type"); if (findx >= 0 ) row.set_Value(findx, this.RunwayElementType.ToString());

            findx = row.Fields.FindField("CodeComposition"); if (findx >= 0) row.set_Value(findx, this.CodeComposition);
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);//
            findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);
        }

    }
}
