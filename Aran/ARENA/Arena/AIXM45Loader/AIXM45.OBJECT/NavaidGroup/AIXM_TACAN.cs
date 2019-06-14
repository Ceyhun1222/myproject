using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace AIXM45Loader
{
    public class AIXM45_TACAN : AIXM45_NAVAID
    {
        private string _R_mid;

        public string R_mid
        {
            get { return _R_mid; }
            set { _R_mid = value; }
        }
        private string _R_codeId;

        public string R_codeId
        {
            get { return _R_codeId; }
            set { _R_codeId = value; }
        }
        private string _R_geoLat;

        public string R_geoLat
        {
            get { return _R_geoLat; }
            set { _R_geoLat = value; }
        }
        private string _R_geoLong;

        public string R_geoLong
        {
            get { return _R_geoLong; }
            set { _R_geoLong = value; }
        }
        private string _txtName;

        public string txtName
        {
            get { return _txtName; }
            set { _txtName = value; }
        }
        private string _codeChannel;

        public string codeChannel
        {
            get { return _codeChannel; }
            set { _codeChannel = value; }
        }
        private double _valDeclination;

        public double valDeclination
        {
            get { return _valDeclination; }
            set { _valDeclination = value; }
        }
        private double _valMagVar;

        public double valMagVar
        {
            get { return _valMagVar; }
            set { _valMagVar = value; }
        }
        private string _dateMagVar;

        public string dateMagVar
        {
            get { return _dateMagVar; }
            set { _dateMagVar = value; }
        }
        private string _codeEm;

        public string codeEm
        {
            get { return _codeEm; }
            set { _codeEm = value; }
        }
        private string _codeDatum;

        public string codeDatum
        {
            get { return _codeDatum; }
            set { _codeDatum = value; }
        }
        private string _valGeoAccuracy;

        public string valGeoAccuracy
        {
            get { return _valGeoAccuracy; }
            set { _valGeoAccuracy = value; }
        }
        private string _uomGeoAccuracy;

        public string uomGeoAccuracy
        {
            get { return _uomGeoAccuracy; }
            set { _uomGeoAccuracy = value; }
        }
        private double _valElev;

        public double valElev
        {
            get { return _valElev; }
            set { _valElev = value; }
        }
        private double _valElevAccuracy;

        public double valElevAccuracy
        {
            get { return _valElevAccuracy; }
            set { _valElevAccuracy = value; }
        }
        private double _valGeoidUndulation;

        public double valGeoidUndulation
        {
            get { return _valGeoidUndulation; }
            set { _valGeoidUndulation = value; }
        }
        private string _uomDistVer;

        public string uomDistVer
        {
            get { return _uomDistVer; }
            set { _uomDistVer = value; }
        }
        private string _txtVerDatum;

        public string txtVerDatum
        {
            get { return _txtVerDatum; }
            set { _txtVerDatum = value; }
        }
        private string _txtRmk;

        public string txtRmk
        {
            get { return _txtRmk; }
            set { _txtRmk = value; }
        }

        private string _R_VorMid;

        public string R_VorMid
        {
            get { return _R_VorMid; }
            set { _R_VorMid = value; }
        }

         public AIXM45_TACAN(ARINC_OBJECT ARINC_Record)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            ARINC_Navaid_VHF_Primary_Record _Record = (ARINC_Navaid_VHF_Primary_Record)ARINC_Record;

             //this._codeChannel = _Record.

           
            this._R_codeId = _Record.Navaid_Identifier;
            this._R_geoLat = _Record.DME_Latitude;
            this._R_geoLong = _Record.DME_Longitude;
            this._R_mid = Guid.NewGuid().ToString();
            this._txtName = _Record.VHF_Name;
            this._uomDistVer = "FT";
            double dbl = 0;
            Double.TryParse(_Record.DME_Elevation_FT, out dbl);
            this._valElev = dbl;
            this.R_VorMid = "";



            if ((this.R_geoLat.Trim().Length > 0) && (this.R_geoLong.Trim().Length > 0))
                this.Geometry = ArnUtil.Create_ESRI_POINT(this.R_geoLat, this.R_geoLong, "0", "FT");

            if (this.Geometry != null)
            {
                string LonSign = this.R_geoLong.Substring(0, 1);
                string LatSign = this.R_geoLat.Substring(0, 1);

                //this.R_geoLong = AIXM45_Decoder.ConvertLongitudeToAIXM45COORD(((IPoint)this.Geometry).X, LonSign);
                //this.R_geoLat = AIXM45_Decoder.ConvertLatgitudeToAIXM45COORD(((IPoint)this.Geometry).Y, LatSign);

            }
        }

    }


}
