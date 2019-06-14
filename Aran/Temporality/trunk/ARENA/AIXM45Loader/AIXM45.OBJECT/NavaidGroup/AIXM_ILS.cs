using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;
//using ESRI.ArcGIS.esriSystem;

namespace AIXM45Loader
{
    public class AIXM45_ILS : AIXM45_NAVAID
    {
        private string _R_mid;

        public string R_mid
        {
            get { return _R_mid; }
            set { _R_mid = value; }
        }
        private string _codeCat;

        public string CodeCat
        {
            get { return _codeCat; }
            set { _codeCat = value; }
        }

        private string _R_DmeMid;

        public string R_DmeMid
        {
            get { return _R_DmeMid; }
            set { _R_DmeMid = value; }
        }
        private string _R_RdnTxt;

        public string R_RdnTxt
        {
            get { return _R_RdnTxt; }
            set { _R_RdnTxt = value; }
        }
        private string _R_RwyTxt;

        public string R_RwyTxt
        {
            get { return _R_RwyTxt; }
            set { _R_RwyTxt = value; }
        }
        private string _R_codeIdTxt;

        public string R_codeIdTxt
        {
            get { return _R_codeIdTxt; }
            set { _R_codeIdTxt = value; }
        }

        private string _txtRmk;

        public string txtRmk
        {
            get { return _txtRmk; }
            set { _txtRmk = value; }
        }


        private AIXM45_ILZ _ILZ;

        public AIXM45_ILZ ILZ
        {
            get { return _ILZ; }
            set { _ILZ = value; }
        }
        private AIXM45_IGP _IGP;

        public AIXM45_IGP IGP
        {
            get { return _IGP; }
            set { _IGP = value; }
        }


        private string _geoLat;
        private string _geoLong;
        public string _Airport_Identifier;

        public AIXM45_ILS(ARINC_OBJECT ARINC_Record)
        {
           ARINC_LocalizerGlideSlope_Primary_Record _Record = (ARINC_LocalizerGlideSlope_Primary_Record)ARINC_Record;
           AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();


           this._R_mid = _Record.ID;
           this._codeCat = _Record.ILS_Category;
           this._R_codeIdTxt = _Record.ICAO_Code;
           this._R_RdnTxt = _Record.Runway_Identifier;
           this._IGP = new AIXM45_IGP(ARINC_Record);
           this._ILZ = new AIXM45_ILZ(ARINC_Record);
           this.txtRmk = "ILS_" + _Record.Runway_Identifier + "(" + _Record.Airport_Identifier + "):";
            
           this._geoLat = _Record.Glide_Slope_Latitude;
           if (this._geoLat.Trim().Length == 0) this._geoLat = _Record.Localizer_Latitude;
           this._geoLong = _Record.Glide_Slope_Longitude;
           if (this._geoLong.Trim().Length == 0) this._geoLong = _Record.Localizer_Longitude;

           if ((this._geoLat.Trim().Length > 0) && (this._geoLong.Trim().Length > 0))
               this.Geometry = ArnUtil.Create_ESRI_POINT(this._geoLat, this._geoLong, "0", "FT");

           if (this.Geometry != null)
           {
               string LonSign = this._geoLong.Substring(0, 1);
               string LatSign = this._geoLat.Substring(0, 1);

               ////this._geoLong = AIXM45_Decoder.ConvertLongitudeToAIXM45COORD(((IPoint)this.Geometry).X, LonSign);
               //this._geoLat = AIXM45_Decoder.ConvertLatgitudeToAIXM45COORD(((IPoint)this.Geometry).Y, LatSign);

           }

           this._Airport_Identifier = _Record.Airport_Identifier;

        }

        public double CalcValWidCourse(double DistanceRdn_ILZ)
        {
            double res = Double.NaN;

            try
            {
                res = 2 * Math.Atan(105 / DistanceRdn_ILZ);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("");
            }

            return res;
        }

        public AIXM45_ILS()
        {
        }

    }
}
