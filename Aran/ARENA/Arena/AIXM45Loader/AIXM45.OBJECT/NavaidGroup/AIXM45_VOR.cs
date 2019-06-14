using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace AIXM45Loader
{
    public class AIXM45_VOR : AIXM45_NAVAID
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
        public string TxtName
        {
            get { return _txtName; }
            set { _txtName = value; }
        }

        private string _codeType;
        public string CodeType
        {
            get { return _codeType; }
            set { _codeType = value; }
        }

        private double _valFreq;
        public double ValFreq
        {
            get { return _valFreq; }
            set { _valFreq = value; }
        }

        private double _valMagVar;
        public double ValMagVar
        {
            get { return _valMagVar; }
            set { _valMagVar = value; }
        }

        private string _uomFreq;
        public string UomFreq
        {
            get { return _uomFreq; }
            set { _uomFreq = value; }
        }

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

        private AIXM45_DME _DME;
        public AIXM45_DME DME
        {
            get { return _DME; }
            set { _DME = value; }
        }


        private AIXM45_TACAN _TACAN;
        public AIXM45_TACAN TACAN
        {
            get { return _TACAN; }
            set { _TACAN = value; }
        }

        public AIXM45_VOR(ARINC_OBJECT ARINC_Record)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            ARINC_Navaid_VHF_Primary_Record _Record = (ARINC_Navaid_VHF_Primary_Record)ARINC_Record;
            this._codeType = _Record.NAVAID_Class.Trim();
            if (this.CodeType.StartsWith("VD") )
            {
                this._DME = new AIXM45_DME(_Record);
                this._DME.R_VorMid = _Record.ID;
            }


            if (this.CodeType.StartsWith("VT") || this.CodeType.StartsWith("VM"))
            {
                this._TACAN = new AIXM45_TACAN(_Record);
                this._TACAN.R_VorMid = _Record.ID;
            }

            this._R_codeId = _Record.Navaid_Identifier;
            this._R_geoLat = _Record.VOR_Latitude;
            this._R_geoLong = _Record.VOR_Longitude;
            this._R_mid = _Record.ID;
            this._txtName = _Record.VHF_Name;
            this._uomFreq = "MHz";
            double dbl = 0;
            Double.TryParse(_Record.VOR_Frequency, out dbl);
            this._valFreq = dbl/100;

            string skl = _Record.Station_Declination[0].ToString();
            Double.TryParse(_Record.Station_Declination.Substring(1, _Record.Station_Declination.Length - 1), out dbl);
            this._valMagVar = dbl / 10;
            if (skl.StartsWith("W")) this._valMagVar = this._valMagVar * -1;

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

        public AIXM45_VOR()
        {
        }

    }
}
