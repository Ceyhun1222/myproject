using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace AIXM45Loader
{
    public class AIXM45_NDB : AIXM45_NAVAID
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

        private string _txtRmk;// AirportICAO_ID
        public string TxtRmk
        {
            get { return _txtRmk; }
            set { _txtRmk = value; }
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

        private string _codeClass;
        public string CodeClass
        {
            get { return _codeClass; }
            set { _codeClass = value; }
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

        private double _valMagVar;
        public double ValMagVar
        {
            get { return _valMagVar; }
            set { _valMagVar = value; }
        }

        private string _txtName;
        public string TxtName
        {
            get { return _txtName; }
            set { _txtName = value; }
        }

        public AIXM45_NDB(ARINC_OBJECT ARINC_Record)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            ARINC_Navaid_NDB_Primary_Record _Record = (ARINC_Navaid_NDB_Primary_Record)ARINC_Record;
            this._codeClass = _Record.NAVAID_Class;
            this._R_codeId = _Record.Navaid_Identifier;
            this._R_geoLat = _Record.NDB_Latitude;
            this._R_geoLong = _Record.NDB_Longitude;
            this._R_mid = _Record.ID;
            //this.R_OrgMID = "";
            this._txtName = _Record.Name;
            this._txtRmk = _Record.Airport_ICAO_Identifier;
            this._uomFreq = "KHz";

            double dbl = 0;
            Double.TryParse(_Record.NDB_Frequency, out dbl);
            this._valFreq = dbl/10;

            dbl = 0;
            string skl = _Record.Magnetic_Variation[0].ToString(); 
            Double.TryParse(_Record.Magnetic_Variation.Substring(1,_Record.Magnetic_Variation.Length-1), out dbl);
            this._valMagVar = dbl/10;
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

        public AIXM45_NDB()
        {
        }

    }


}
