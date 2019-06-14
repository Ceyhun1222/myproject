using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace AIXM45Loader
{
    public class AIXM45_WayPoint : AIXM45_Object
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

        private string _codeType;
        public string CodeType
        {
            get { return _codeType; }
            set { _codeType = value; }
        }

        private string _txtName;
        public string TxtName
        {
            get { return _txtName; }
            set { _txtName = value; }
        }

        private string _R_Ahpmid;

        public string R_Ahpmid
        {
            get { return _R_Ahpmid; }
            set { _R_Ahpmid = value; }
        }

        public AIXM45_WayPoint(ARINC_OBJECT ARINC_Record)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            ARINC_WayPoint_Primary_Record _Record = (ARINC_WayPoint_Primary_Record)ARINC_Record;

            this._codeType = "ICAO";//_Record.Waypoint_Type;
            this._R_codeId = _Record.Waypoint_Identifier;

            if (_R_codeId.StartsWith("KARMA"))
                System.Diagnostics.Debug.WriteLine("");

            this._R_geoLat = _Record.Waypoint_Latitude;
            this._R_geoLong = _Record.Waypoint_Longitude;
            this._R_mid = _Record.ID;

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

        public AIXM45_WayPoint()
        {
        }
       

    }
}
