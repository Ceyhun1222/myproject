using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    public class AirspaceVolume_AIRTRACK : Object_AIRTRACK
    {
        private string _codeId;

        public string CodeId
        {
            get { return _codeId; }
            set { _codeId = value; }
        }

        private string _codeType;

        public string CodeType
        {
            get { return _codeType; }
            set { _codeType = value; }
        }

        private string _valLowerLimit;

        public string ValLowerLimit
        {
            get { return _valLowerLimit; }
            set { _valLowerLimit = value; }
        }

        //private string _uomDistVerMnm;

        //public string UomDistVerMnm
        //{
        //    get { return _uomDistVerMnm; }
        //    set { _uomDistVerMnm = value; }
        //}

        //private string _valDistVerMnm;

        //public string ValDistVerMnm
        //{
        //    get { return _valDistVerMnm; }
        //    set { _valDistVerMnm = value; }
        //}

        //private string _codeDistVerMnm;

        //public string CodeDistVerMnm
        //{
        //    get { return _codeDistVerMnm; }
        //    set { _codeDistVerMnm = value; }
        //}

        //private string _uomDistVerMax;

        //public string UomDistVerMax
        //{
        //    get { return _uomDistVerMax; }
        //    set { _uomDistVerMax = value; }
        //}

        private string _valDistVerMax;

        public string ValDistVerMax
        {
            get { return _valDistVerMax; }
            set { _valDistVerMax = value; }
        }

        //private string _codeDistVerMax;

        //public string CodeDistVerMax
        //{
        //    get { return _codeDistVerMax; }
        //    set { _codeDistVerMax = value; }
        //}

        //private string _uomDistVerLower;

        //public string UomDistVerLower
        //{
        //    get { return _uomDistVerLower; }
        //    set { _uomDistVerLower = value; }
        //}

        private string _valDistVerLower;

        public string ValDistVerLower
        {
            get { return _valDistVerLower; }
            set { _valDistVerLower = value; }
        }

        private string _codeDistVerLower;

        public string CodeDistVerLower
        {
            get { return _codeDistVerLower; }
            set { _codeDistVerLower = value; }
        }

        //private string _uomDistVerUpper;

        //public string UomDistVerUpper
        //{
        //    get { return _uomDistVerUpper; }
        //    set { _uomDistVerUpper = value; }
        //}

        private string _valDistVerUpper;

        public string ValDistVerUpper
        {
            get { return _valDistVerUpper; }
            set { _valDistVerUpper = value; }
        }

        private string _codeDistVerUpper;

        public string CodeDistVerUpper
        {
            get { return _codeDistVerUpper; }
            set { _codeDistVerUpper = value; }
        }

        //private string _codeMil;

        //public string CodeMil
        //{
        //    get { return _codeMil; }
        //    set { _codeMil = value; }
        //}

        //private string _codeActivity;

        //public string CodeActivity
        //{
        //    get { return _codeActivity; }
        //    set { _codeActivity = value; }
        //}

        //private string _codeLocInd;

        //public string CodeLocInd
        //{
        //    get { return _codeLocInd; }
        //    set { _codeLocInd = value; }
        //}

        //private string _codeClass;

        //public string CodeClass
        //{
        //    get { return _codeClass; }
        //    set { _codeClass = value; }
        //}

        private string _txtName;

        public string TxtName
        {
            get { return _txtName; }
            set { _txtName = value; }
        }

        private string _vertUom;

        public string VertUom
        {
            get { return _vertUom; }
            set { _vertUom = value; }
        }

        private string _airspaceCenter;

        public string AirspaceCenter
        {
            get { return _airspaceCenter; }
            set { _airspaceCenter = value; }
        }

        private string _airspaceCenterType;

        public string AirspaceCenterType
        {
            get { return _airspaceCenterType; }
            set { _airspaceCenterType = value; }
        }

        private string _airspaceVolumeID;

        public string AirspaceVolumeID
        {
            get { return _airspaceVolumeID; }
            set { _airspaceVolumeID = value; }
        }

        private List<AirspaceSegmen_GeometryProperties> _geomComponent;

        public List<AirspaceSegmen_GeometryProperties> GeomComponent
        {
            get { return _geomComponent; }
            set { _geomComponent = value; }
        }

        public AirspaceVolume_AIRTRACK()
        {
        }

        public AirspaceVolume_AIRTRACK(List<AIRTRACK_Airspace_Segment> segmentsList)
        {
            this.GeomComponent = new List<AirspaceSegmen_GeometryProperties>();
            int i = 0;

            this.ID_AIRTRACK = Guid.NewGuid().ToString();
            this.AirspaceCenter = "";
            this.AirspaceCenterType = "";
            this.CodeDistVerLower = "";
            this.CodeDistVerUpper = "";
            this.CodeId = "";
            this.CodeType = "";
            this.TxtName = "";
            this.ValDistVerLower = "";
            this.ValDistVerMax = "";
            this.ValDistVerUpper = "";
            this.ValLowerLimit = "";
            this.VertUom = "";


            switch (segmentsList[0].AisrpaceARINC_Type)
            {
                case ("FIR_UIR"):
                    i = 0;
                    foreach (AIRTRACK_Airspace_Segment seg in segmentsList)
                    {
                       ARINC_FIR_UIR_Primary_Records FIR_SEG = (ARINC_FIR_UIR_Primary_Records)seg.ARINC_OBJ;

                        #region Fill object properties

                       if (i == 0)
                       {
                           this.ValDistVerLower = FIR_SEG.UIR_Lower_Limit;

                           if (FIR_SEG.FIR_UIR_Indicator.Trim().Contains("F"))
                           {
                               this.ValDistVerMax = FIR_SEG.UIR_Upper_Limit;
                               this.CodeType = "FIR";
                           }
                           else if (FIR_SEG.FIR_UIR_Indicator.Trim().Contains("U"))
                           {
                               this.ValDistVerMax = FIR_SEG.UIR_Upper_Limit;
                               this.CodeType = "UIR";
                           }
                           else if (FIR_SEG.FIR_UIR_Indicator.Trim().Contains("B"))
                           {
                               this.ValDistVerMax = FIR_SEG.UIR_Upper_Limit;
                               this.CodeType = "UIR_P";
                           }

                           this.CodeId = FIR_SEG.FIR_UIR_Identifier;
                           this.TxtName = FIR_SEG.FIR_UIR_Name;

                           if (FIR_SEG.Reporting_Units_Altitude.Trim().StartsWith("2")) this.VertUom = "M";

                           this.AirspaceVolumeID = FIR_SEG.FIR_UIR_Identifier.Trim() + "_" + FIR_SEG.FIR_UIR_Name.Trim();
                       }



                        #endregion

                       i++;
                       AirspaceSegmen_GeometryProperties gp = new AirspaceSegmen_GeometryProperties(FIR_SEG.FIR_UIR_Latitude, FIR_SEG.FIR_UIR_Longitude, FIR_SEG.Arc_Origin_Latitude, FIR_SEG.Arc_Origin_Longitude, FIR_SEG.Arc_Distance, FIR_SEG.Arc_Bearing, FIR_SEG.Boundary_Via);
                       gp.Tag = FIR_SEG;
                       this.GeomComponent.Add(gp);
                    }

                    break;

                case ("ControlledAirspace"):
                    i = 0;
                    foreach (AIRTRACK_Airspace_Segment seg in segmentsList)
                    {
                        ARINC_Controlled_Airspace_Primary_Records Controlled_SEG = (ARINC_Controlled_Airspace_Primary_Records)seg.ARINC_OBJ;

                        #region Fill object properties

                        if (i == 0)
                        {
                            this.ValDistVerLower = Controlled_SEG.Lower_Limit;
                            this.ValDistVerUpper = Controlled_SEG.Upper_Limit;
                            this.CodeDistVerLower = Controlled_SEG.Unit_Indicator1;
                            this.CodeDistVerUpper = Controlled_SEG.Unit_Indicator2;
                            this.TxtName = Controlled_SEG.Controlled_Airspace_Name;
                            this.CodeType = DecodeAirspaceType( Controlled_SEG.Airspace_Type);
                            this.CodeId = this.CodeType.Trim() + "_" + Controlled_SEG.Airspace_Center.Trim();
                            this.AirspaceCenter = Controlled_SEG.Airspace_Center.Trim();
                            this.AirspaceCenterType = Controlled_SEG.Section_Code + Controlled_SEG.Subsection_Code;
                            this.AirspaceVolumeID = Controlled_SEG.Airspace_Center.Trim() + "_" + Controlled_SEG.Section_Code.Trim() + Controlled_SEG.Subsection_Code.Trim() + "_" + Controlled_SEG.Airspace_Type.Trim();

                        }

                        #endregion

                        i++;
                        AirspaceSegmen_GeometryProperties gp = new AirspaceSegmen_GeometryProperties(Controlled_SEG.Latitude, Controlled_SEG.Longitude, Controlled_SEG.Arc_Origin_Latitude, Controlled_SEG.Arc_Origin_Longitude, Controlled_SEG.Arc_Distance, Controlled_SEG.Arc_Bearing, Controlled_SEG.Boundary_Via);
                        gp.Tag = Controlled_SEG;
                        this.GeomComponent.Add(gp);
 


                    }

                    break;

                case ("RestrictiveAirspace"):
                    i = 0;
                    foreach (AIRTRACK_Airspace_Segment seg in segmentsList)
                    {
                        ARINC_Restrictive_Airspace_Primary_Records Restrictive_SEG =(ARINC_Restrictive_Airspace_Primary_Records)seg.ARINC_OBJ;


                        #region Fill object properties

                        if (i == 0)
                        {
                            this.ValDistVerLower = Restrictive_SEG.Lower_Limit;
                            this.ValDistVerUpper = Restrictive_SEG.Upper_Limit;
                            this.CodeDistVerLower = Restrictive_SEG.Unit_Indicator1;
                            this.CodeDistVerUpper = Restrictive_SEG.Unit_Indicator2;
                            this.TxtName = Restrictive_SEG.Restrictive_Airspace_Name;
                            this.CodeType =ConvertRestrictiveType(Restrictive_SEG.Restrictive_Type);
                            this.CodeId = Restrictive_SEG.Restrictive_Airspace_Designation;
                            this.AirspaceVolumeID = Restrictive_SEG.ICAO_Code.Trim() + "_" + Restrictive_SEG.Restrictive_Type.Trim() + "_" + Restrictive_SEG.Restrictive_Airspace_Designation.Trim() + "_" + Restrictive_SEG.Restrictive_Airspace_Name.Trim();

                        }

                        #endregion

                        i++;
                        AirspaceSegmen_GeometryProperties gp = new AirspaceSegmen_GeometryProperties(Restrictive_SEG.Latitude, Restrictive_SEG.Longitude, Restrictive_SEG.Arc_Origin_Latitude, Restrictive_SEG.Arc_Origin_Longitude, Restrictive_SEG.Arc_Distance, Restrictive_SEG.Arc_Bearing, Restrictive_SEG.Boundary_Via);
                        gp.Tag = Restrictive_SEG;
                        this.GeomComponent.Add(gp);


                    }

                    break;
            }


            //this.BuildAirspaceBorder();


        }

        private string ConvertRestrictiveType(string RestrictiveType)
        {
            string res = RestrictiveType;
            switch (RestrictiveType)
            {
                case ("M"): res = "MTR";
                    break;
                case ("T"): res = "TRA";
                    break;
                case ("U"): res = "OTHER";
                    break;
                default: res = RestrictiveType;
                    break;
            }

            return res;
        }

        private string DecodeAirspaceType(string aspsType)
        {
            string res="";
            switch (aspsType)
            {
                case "A": res = "ASR";
                    break;
                case "C": res = "CTA";
                    break;
                case "M": res = "TMA";
                    break;
                case "R": res = "TRA";
                    break;
                case "T": res = "TCA";
                    break;
                case "Z": res = "CTR";
                    break;
 
            }

            return res;
        }

        
    }

    public class AirspaceSegmen_GeometryProperties
    {
        private string _lat;

        public string Lat
        {
            get { return _lat; }
            set { _lat = value; }
        }

        private string _lon;

        public string Lon
        {
            get { return _lon; }
            set { _lon = value; }
        }

        private string _Arc_Origin_Latitude;

        public string Arc_Origin_Latitude
        {
            get { return _Arc_Origin_Latitude; }
            set { _Arc_Origin_Latitude = value; }
        }


        private string _Arc_Origin_Longitude;

        public string Arc_Origin_Longitude
        {
            get { return _Arc_Origin_Longitude; }
            set { _Arc_Origin_Longitude = value; }
        }


        private string _Arc_Distance;

        public string Arc_Distance
        {
            get { return _Arc_Distance; }
            set { _Arc_Distance = value; }
        }

        private string _Arc_Bearing;

        public string Arc_Bearing
        {
            get { return _Arc_Bearing; }
            set { _Arc_Bearing = value; }
        }

        private string _Boundary_Via;

        public string Boundary_Via
        {
            get { return _Boundary_Via; }
            set { _Boundary_Via = value; }
        }

        private object _tag;

        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public AirspaceSegmen_GeometryProperties()
        {
        }

        public AirspaceSegmen_GeometryProperties(string _lat, string _lon, string _Arc_Origin_Latitude, string _Arc_Origin_Longitude, string _Arc_Distance, string _Arc_Bearing, string _Boundary_Via)
        {
            this.Lat = _lat;
            this.Lon = _lon;
            this.Arc_Bearing = _Arc_Bearing;
            this.Arc_Distance = _Arc_Distance;
            this.Arc_Origin_Latitude = _Arc_Origin_Latitude;
            this.Arc_Origin_Longitude = _Arc_Origin_Longitude;
            this.Boundary_Via = _Boundary_Via;
           
        }
    }

    public class airspaceSupport
    {
        private double _Lat;

        public double Lat
        {
            get { return _Lat; }
            set { _Lat = value; }
        }
        private double _Lon;

        public double Lon
        {
            get { return _Lon; }
            set { _Lon = value; }
        }
        private double _Arc_Bearing;

        public double Arc_Bearing
        {
            get { return _Arc_Bearing; }
            set { _Arc_Bearing = value; }
        }
        private double _Arc_Distance;

        public double Arc_Distance
        {
            get { return _Arc_Distance; }
            set { _Arc_Distance = value; }
        }
        private double _Arc_Origin_Latitude;

        public double Arc_Origin_Latitude
        {
            get { return _Arc_Origin_Latitude; }
            set { _Arc_Origin_Latitude = value; }
        }
        private double _Arc_Origin_Longitude;

        public double Arc_Origin_Longitude
        {
            get { return _Arc_Origin_Longitude; }
            set { _Arc_Origin_Longitude = value; }
        }
        //private string _Boundary_Via;

        //public string Boundary_Via
        //{
        //    get { return _Boundary_Via; }
        //    set { _Boundary_Via = value; }
        //}

        public airspaceSupport()
        {
        }

        public airspaceSupport(AirspaceSegmen_GeometryProperties segment)
        {
           AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

            if (segment.Lat.Trim().Length > 0) this.Lat = ArnUtil.GetLATITUDEFromAIXMString(segment.Lat.Trim());

            if (segment.Lon.Trim().Length > 0) this.Lon = ArnUtil.GetLONGITUDEFromAIXMString(segment.Lon.Trim());

            if (segment.Arc_Origin_Latitude.Trim().Length > 0) this.Arc_Origin_Latitude = ArnUtil.GetLATITUDEFromAIXMString(segment.Arc_Origin_Latitude.Trim());

            if (segment.Arc_Origin_Longitude.Trim().Length > 0) this.Arc_Origin_Longitude = ArnUtil.GetLONGITUDEFromAIXMString(segment.Arc_Origin_Longitude.Trim());

            double ArcDistance;
            Double.TryParse(segment.Arc_Distance.Trim(), out ArcDistance);
            if (ArcDistance != 0) this.Arc_Distance = ArnUtil.ConvertValueToMeter((ArcDistance / 10).ToString(), "NM");

            double ArcBearing;
            Double.TryParse(segment.Arc_Bearing.Trim(), out ArcBearing);
            this.Arc_Bearing = ArcBearing / 10;

            

            ArnUtil = null;
            
        }

   
    }

    public class Airspace_AIRTRACK : Object_AIRTRACK
    {
        private string _codeId;

        public string CodeId
        {
            get { return _codeId; }
            set { _codeId = value; }
        }

        private string _txtName;

        public string TxtName
        {
            get { return _txtName; }
            set { _txtName = value; }
        }

        private List<AirspaceVolume_AIRTRACK> _AirspaceVolumeList;

        public List<AirspaceVolume_AIRTRACK> AirspaceVolumeList
        {
            get { return _AirspaceVolumeList; }
            set { _AirspaceVolumeList = value; }
        }

        public Airspace_AIRTRACK()
        {
        }

        public Airspace_AIRTRACK(List<AirspaceVolume_AIRTRACK> Volums)
        {
            this.AirspaceVolumeList = new List<AirspaceVolume_AIRTRACK>();
            this.AirspaceVolumeList.AddRange(Volums.GetRange(0,Volums.Count));
        }
    }
}
