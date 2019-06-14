using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;
//using ESRI.ArcGIS.esriSystem;

namespace AIXM45Loader
{
    public class AIXM45_IGP : AIXM45_NAVAID
    {
        private double _valFreq;
        public double ValFreq
        {
            get { return _valFreq; }
            set { _valFreq = value; }
        }

        private string _uomFreq;
        public string UomFreq
        {
            get { return _uomFreq; }
            set { _uomFreq = value; }
        }

        private string _codeEm;
        public string CodeEm
        {
            get { return _codeEm; }
            set { _codeEm = value; }
        }

        private double _valSlope;
        public double ValSlope
        {
            get { return _valSlope; }
            set { _valSlope = value; }
        }

        private double _valRdh;
        public double ValRdh
        {
            get { return _valRdh; }
            set { _valRdh = value; }
        }

        private string _uomRdh;
        public string UomRdh
        {
            get { return _uomRdh; }
            set { _uomRdh = value; }
        }

        private string _geoLat;
        public string GeoLat
        {
            get { return _geoLat; }
            set { _geoLat = value; }
        }

        private string _geoLong;
        public string GeoLong
        {
            get { return _geoLong; }
            set { _geoLong = value; }
        }

        private string _codeDatum;
        public string CodeDatum
        {
            get { return _codeDatum; }
            set { _codeDatum = value; }
        }

        //private double _valGeoAccuracy;
        //public double ValGeoAccuracy
        //{
        //    get { return _valGeoAccuracy; }
        //    set { _valGeoAccuracy = value; }
        //}

        //private string _uomGeoAccuracy;
        //public string UomGeoAccuracy
        //{
        //    get { return _uomGeoAccuracy; }
        //    set { _uomGeoAccuracy = value; }
        //}

        private double _valElev;
        public double ValElev
        {
            get { return _valElev; }
            set { _valElev = value; }
        }

        //private double _valElevAccuracy;
        //public double ValElevAccuracy
        //{
        //    get { return _valElevAccuracy; }
        //    set { _valElevAccuracy = value; }
        //}

        //private double _valGeoidUndulation;
        //public double ValGeoidUndulation
        //{
        //    get { return _valGeoidUndulation; }
        //    set { _valGeoidUndulation = value; }
        //}

        private string _uomDistVer;
        public string UomDistVer
        {
            get { return _uomDistVer; }
            set { _uomDistVer = value; }
        }

        private string _R_ILSMID;
        public string R_ILSMID
        {
            get { return _R_ILSMID; }
            set { _R_ILSMID = value; }
        }

        private string _R_mid;
        public string R_mid
        {
            get { return _R_mid; }
            set { _R_mid = value; }
        }

        public AIXM45_IGP(ARINC_OBJECT ARINC_Record)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            ARINC_LocalizerGlideSlope_Primary_Record _Record = (ARINC_LocalizerGlideSlope_Primary_Record)ARINC_Record;


            this._geoLat = _Record.Glide_Slope_Latitude.Trim();
            if (this._geoLat.Trim().Length == 0) this._geoLat = _Record.Localizer_Latitude;
            this._geoLong = _Record.Glide_Slope_Longitude;
            if (this._geoLong.Trim().Length == 0) this._geoLong = _Record.Localizer_Longitude;

            this._R_ILSMID = _Record.ID;
            this._R_mid = Guid.NewGuid().ToString();
            this._uomDistVer = "FT";
            this._uomFreq = "KHZ";
            this._uomRdh = "FT";
            double dbl;
             Double.TryParse(_Record.Glide_Slope_Elevation,out dbl);
             this._valElev = dbl;
             Double.TryParse(_Record.Glide_Slope_Height_at_Landing_Threshold, out dbl);
             this._valRdh = dbl;
             Double.TryParse(_Record.Glide_Slope_Angle, out dbl);
             this._valSlope = dbl/100;

             if ((this.GeoLat.Trim().Length > 0) && (this.GeoLong.Trim().Length > 0))
                 this.Geometry = ArnUtil.Create_ESRI_POINT(this.GeoLat, this.GeoLong, this.ValElev.ToString(), "FT");

             if (this.Geometry != null)
             {
                 string LonSign = this.GeoLong.Substring(0, 1);
                 string LatSign = this.GeoLat.Substring(0, 1);

                 ////this._geoLong = AIXM45_Decoder.ConvertLongitudeToAIXM45COORD(((IPoint)this.Geometry).X, LonSign);
                 //this._geoLat = AIXM45_Decoder.ConvertLatgitudeToAIXM45COORD(((IPoint)this.Geometry).Y, LatSign);

             }
        }

        public AIXM45_IGP()
        {
        }

    }
}
