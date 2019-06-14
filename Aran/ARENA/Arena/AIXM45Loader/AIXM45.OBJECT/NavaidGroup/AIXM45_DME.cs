using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace AIXM45Loader
{
    public class AIXM45_DME : AIXM45_NAVAID
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

        private string _valFreq;
        public string ValFreq
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

        private string _R_VorMid;
        public string R_VorMid
        {
            get { return _R_VorMid; }
            set { _R_VorMid = value; }
        }

        private string _codeChannel;
        public string CodeChannel
        {
            get { return _codeChannel; }
            set { _codeChannel = value; }
        }

        public AIXM45_DME(ARINC_OBJECT ARINC_Record)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            ARINC_Navaid_VHF_Primary_Record _Record = (ARINC_Navaid_VHF_Primary_Record)ARINC_Record;
            this._codeType = _Record.NAVAID_Class.Trim();
            this._R_codeId = _Record.Navaid_Identifier;
            this._R_geoLat = _Record.DME_Latitude;
            this._R_geoLong = _Record.DME_Longitude;
            this._R_mid = Guid.NewGuid().ToString();
            this._txtName = _Record.VHF_Name;
            this._uomDistVer = "FT";
            this._uomFreq = "KHz";
            double dbl = 0;
            //Double.TryParse(_Record.DME_Elevation, out dbl);
            this._valElev = dbl;
            this._valFreq = _Record.Frequency_Protection;
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

        public AIXM45_DME()
        {
        }

    }
}
