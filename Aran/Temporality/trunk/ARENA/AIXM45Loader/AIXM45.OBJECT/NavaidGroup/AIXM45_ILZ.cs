using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;
//using ESRI.ArcGIS.esriSystem;

namespace AIXM45Loader
{
    public class AIXM45_ILZ : AIXM45_NAVAID
    {
        private string _codeId;
        public string CodeId
        {
            get { return _codeId; }
            set { _codeId = value; }
        }

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

        private double _valMagBrg;
        public double ValMagBrg
        {
            get { return _valMagBrg; }
            set { _valMagBrg = value; }
        }

        private double _valTrueBrg;
        public double ValTrueBrg
        {
            get { return _valTrueBrg; }
            set { _valTrueBrg = value; }
        }

        //private double _valMagVar;
        //public double ValMagVar
        //{
        //    get { return _valMagVar; }
        //    set { _valMagVar = value; }
        //}

        private string _dateMagVar;
        public string DateMagVar
        {
            get { return _dateMagVar; }
            set { _dateMagVar = value; }
        }

        private double _valWidCourse;
        public double ValWidCourse
        {
            get { return _valWidCourse; }
            set { _valWidCourse = value; }
        }

        private string _codeTypeUseBack;
        public string CodeTypeUseBack
        {
            get { return _codeTypeUseBack; }
            set { _codeTypeUseBack = value; }
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

        //private double _valElev;
        //public double ValElev
        //{
        //    get { return _valElev; }
        //    set { _valElev = value; }
        //}

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

        private double _valElev;

        public double ValElev
        {
            get { return _valElev; }
            set { _valElev = value; }
        }

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

        public AIXM45_ILZ(ARINC_OBJECT ARINC_Record)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            ARINC_LocalizerGlideSlope_Primary_Record _Record = (ARINC_LocalizerGlideSlope_Primary_Record)ARINC_Record;

            //this._codeDatum = "";
            //this._codeEm = "";
            this._codeId = _Record.Localizer_Identifier;
            //this._codeTypeUseBack = "";
            //this._dateMagVar = "";
            this._geoLat = _Record.Localizer_Latitude;
            this._geoLong = _Record.Localizer_Longitude;
            this._R_ILSMID = _Record.ID;
            this._R_mid = Guid.NewGuid().ToString();
            this._uomDistVer = "FT";
            //this._valElev = _Record.

            this._uomFreq = "KHZ";
            double dbl = 0;

            Double.TryParse(_Record.Localizer_Frequency,out dbl);
            this._valFreq = dbl;
            if (_Record.Localizer_Frequency.Contains(".")) this._uomFreq = "MHZ";

            if (_Record.Localizer_Bearing.EndsWith("T"))
            {
                _Record.Localizer_Bearing = _Record.Localizer_Bearing.Remove(_Record.Localizer_Bearing.Length - 1, 1);
                Double.TryParse(_Record.Localizer_Bearing, out dbl);
                this._valTrueBrg = dbl / 10;
                this._valMagBrg = Double.NaN;
            }
            else if (_Record.Localizer_Bearing.Trim().Length>0)
            {
                Double.TryParse(_Record.Localizer_Bearing, out dbl);
                this._valMagBrg = dbl / 10;
                this._valTrueBrg = Double.NaN;
            }
            else 
            {
                this._valMagBrg = Double.NaN;
                this._valTrueBrg = Double.NaN;
            }


            if (_Record.Localizer_Width.Trim().Length > 0)
            {
                Double.TryParse(_Record.Localizer_Width, out dbl);
                this._valWidCourse = dbl;
            }
            else
                this._valWidCourse = Double.NaN;

            if ((this.GeoLat.Trim().Length > 0) && (this.GeoLong.Trim().Length > 0))
                this.Geometry = ArnUtil.Create_ESRI_POINT(this.GeoLat, this.GeoLong, "", "FT");

            if (this.Geometry != null)
            {
                string LonSign = this.GeoLong.Substring(0, 1);
                string LatSign = this.GeoLat.Substring(0, 1);

                ////this._geoLong = AIXM45_Decoder.ConvertLongitudeToAIXM45COORD(((IPoint)this.Geometry).X, LonSign);
                //this._geoLat = AIXM45_Decoder.ConvertLatgitudeToAIXM45COORD(((IPoint)this.Geometry).Y, LatSign);

            }
        }

        public AIXM45_ILZ()
        {
        }
    }
}
