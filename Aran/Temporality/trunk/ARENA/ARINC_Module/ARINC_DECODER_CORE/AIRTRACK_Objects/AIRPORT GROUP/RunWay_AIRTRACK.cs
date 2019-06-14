using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    public class RunWay_THR_AIRTRACK : Object_AIRTRACK
    {

        public RunWay_THR_AIRTRACK()
        {

        }

        public RunWay_THR_AIRTRACK(ARINC_OBJECT arincObj)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

            this.ID_AIRTRACK = Guid.NewGuid().ToString();
            this.ARINC_OBJ = arincObj;

            ARINC_Runway_Primary_Records ARINC_Runway = (ARINC_Runway_Primary_Records)this.ARINC_OBJ;

            this.INFO_AIRTRACK = ARINC_Runway.Runway_Description;

            double dbl = 0;
            Double.TryParse(ARINC_Runway.Landing_Threshold_Elevation, out dbl);
            this.ThrElev_M = Math.Round( ArnUtil.ConvertValueToMeter(dbl.ToString(), "FT"),2);

            dbl = 0;
            Double.TryParse(ARINC_Runway.Displaced_Threshold_Distance, out dbl);
            if (dbl > 0)
            {
                this.Displaced_Threshold_Distance_M = Math.Round(ArnUtil.ConvertValueToMeter(dbl.ToString(), "FT"),2);
            }

            dbl = 0;
            Double.TryParse(ARINC_Runway.Runway_Length, out dbl);
            if (dbl > 0)
            {
                this.RWY_LENGTH_M = Math.Round(ArnUtil.ConvertValueToMeter( dbl.ToString(),"FT"),2);
            }

            dbl = 0;
            Double.TryParse(ARINC_Runway.Runway_Width, out dbl);
            if (dbl > 0)
            {
                this.RWY_WIDTH_M = Math.Round(ArnUtil.ConvertValueToMeter(dbl.ToString(), "FT"),2);
            }


            string MB = ARINC_Runway.Runway_Magnetic_Bearing;
            if (MB.EndsWith("T")) 
                MB = MB.Replace("T", "");
            dbl = 0;
            Double.TryParse(MB, out dbl);
            this.Magnetic_Bearing = dbl/10;

            this.RDN_TXT_DESIG = ARINC_Runway.Runway_Identifier;
            this.OPPOSITE_RDN_TXT_DESIG = GET_OPPOSITE_RDN_TXT_DESIG(this.RDN_TXT_DESIG.Trim());
            

            dbl = 0;
            Double.TryParse(ARINC_Runway.Stopway, out dbl);
            if (dbl > 0)
            {
                this.THR_Stopway_M = ArnUtil.ConvertValueToMeter(dbl.ToString(), "FT");

            }



            if ((ARINC_Runway.Runway_Latitude.Length > 0) && (ARINC_Runway.Runway_Longitude.Trim().Length > 0))
            {

                string LonSign = ARINC_Runway.Runway_Longitude.Trim().Substring(0, 1);
                string LatSign = ARINC_Runway.Runway_Latitude.Trim().Substring(0, 1);

                string _geoLat = ARINC_Runway.Runway_Latitude.Trim();
                string _geoLong = ARINC_Runway.Runway_Longitude.Trim();

                IPoint pnt = ArnUtil.Create_ESRI_POINT(_geoLat, _geoLong, this.ThrElev_M.ToString(), "FT");

                if (pnt != null)
                {
                    //_geoLong = ArnUtil.ConvertLongitudeToAIXM45COORD(pnt.X, LonSign);
                    //_geoLat = ArnUtil.ConvertLatgitudeToAIXM45COORD(pnt.Y, LatSign);

                    this.Shape = new Shape_AIRTRACK(_geoLat, _geoLong);
                    this.Shape.Geometry = pnt as IGeometry;
                }
            }

        }

        //private ARINC_Runway_Primary_Records _ARINC_Runway;
        //public ARINC_Runway_Primary_Records ARINC_Runway
        //{
        //    get { return _ARINC_Runway; }
        //    set { _ARINC_Runway = value; }
        //}

        private double _ThrElev_FT;
        public double ThrElev_M
        {
            get { return _ThrElev_FT; }
            set { _ThrElev_FT = value; }
        }

        private double _Magnetic_Bearing;
        public double Magnetic_Bearing
        {
            get { return _Magnetic_Bearing; }
            set { _Magnetic_Bearing = value; }
        }

        private double _True_Bearing;
        public double True_Bearing
        {
            get { return _True_Bearing; }
            set { _True_Bearing = value; }
        }

        private double _Displaced_Threshold_Distance_FT;
        public double Displaced_Threshold_Distance_M
        {
            get { return _Displaced_Threshold_Distance_FT; }
            set { _Displaced_Threshold_Distance_FT = value; }
        }

        private double _THR_Stopway_FT;
        public double THR_Stopway_M
        {
            get { return _THR_Stopway_FT; }
            set { _THR_Stopway_FT = value; }
        }

        private string _OPPOSITE_RDN_TXT_DESIG;
        public string OPPOSITE_RDN_TXT_DESIG
        {
            get { return _OPPOSITE_RDN_TXT_DESIG; }
            set { _OPPOSITE_RDN_TXT_DESIG = value; }
        }

        private string _RDN_TXT_DESIG;
        public string RDN_TXT_DESIG
        {
            get { return _RDN_TXT_DESIG; }
            set { _RDN_TXT_DESIG = value; }
        }

        private ILS_AIRTRACK _relatedIls;
        public ILS_AIRTRACK RelatedIls
        {
            get { return _relatedIls; }
            set { _relatedIls = value; }
        }

        private double _RWY_LENGTH;
        public double RWY_LENGTH_M
        {
            get { return _RWY_LENGTH; }
            set { _RWY_LENGTH = value; }
        }

        private double _RWY_WIDTH;
        public double RWY_WIDTH_M
        {
            get { return _RWY_WIDTH; }
            set { _RWY_WIDTH = value; }
        }

        private List<Object_AIRTRACK> _relatedMarker;

        public List<Object_AIRTRACK> RelatedMarker
        {
            get { return _relatedMarker; }
            set { _relatedMarker = value; }
        }


        private string GET_OPPOSITE_RDN_TXT_DESIG(string _RDN_TXT_DESIG)
        {
            if (_RDN_TXT_DESIG.Length == 0) return "RW";
            if (_RDN_TXT_DESIG.Trim().CompareTo("RW") == 0) return "RW";

            string res = _RDN_TXT_DESIG.Trim().Substring(2, _RDN_TXT_DESIG.Length - 2);
            string suf = "";
            if (res.Length == 3)
            {
                suf = res.Substring(2, 1);
                res = res.Substring(0, res.Length - 1);
            }

            switch (suf)
            {
                case ("L"):
                    suf = "R";
                    break;
                case ("R"):
                    suf = "L";
                    break;
                case ("G"):
                    suf = "G";
                    break;
                case ("D"):
                    suf = "D";
                    break;
            }



            //int i = 0;
            //Int32.TryParse(res, out i);
            //i = (180 + i * 10);
            //if ((i > 90) && (i < 270)) i /= 10;
            //else i /= 100;
            //res = i.ToString();
            //if (res.Length < 2) res = "0" + res;


            int i = 0;
            Int32.TryParse(res, out i);
            i = (180 + i * 10);
            if (i > 360)
            {
                Int32.TryParse(res, out i);
                i = i * 10 - 180;
            }
            i /= 10;
            res = i.ToString();
            if (res.Length < 2) res = "0" + res;

            return "RW" + res + suf;
        }
    }


    public class RunWay_AIRTRACK : Object_AIRTRACK
    {
        public RunWay_AIRTRACK()
        {

        }

        public RunWay_AIRTRACK(List<RunWay_THR_AIRTRACK> THRs)
        {
            this.LinkedTHR = new List<RunWay_THR_AIRTRACK>();
            this.LinkedTHR.AddRange(THRs);

            this.RWY_Designator=THRs[0].RDN_TXT_DESIG;
            if (THRs.Count > 1) this.RWY_Designator = this.RWY_Designator + " - " + THRs[1].RDN_TXT_DESIG;
            this.INFO_AIRTRACK = this.RWY_Designator;
        }

        private List<RunWay_THR_AIRTRACK> _LinkedTHR;
        [System.ComponentModel.Browsable(false)]
        public List<RunWay_THR_AIRTRACK> LinkedTHR
        {
            get { return _LinkedTHR; }
            set { _LinkedTHR = value; }
        }

        private double _RWY_Length_M;
        public double RWY_Length_M
        {
            get { return _RWY_Length_M; }
            set { _RWY_Length_M = value; }
        }

        private double _RWY_Width_M;
        public double RWY_Width_M
        {
            get { return _RWY_Width_M; }
            set { _RWY_Width_M = value; }
        }

        private string _RWY_Designator;
        public string RWY_Designator
        {
            get { return _RWY_Designator; }
            set { _RWY_Designator = value; }
        }

        //private double _RWY_Stopway_FT;

        //public double RWY_Stopway_FT
        //{
        //    get { return _RWY_Stopway_FT; }
        //    set { _RWY_Stopway_FT = value; }
        //}
    }

}
