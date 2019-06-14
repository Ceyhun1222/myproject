using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects 
{
    public class NDB_AIRTRACK : Object_AIRTRACK
    {
        public NDB_AIRTRACK()
        {
        }

        public NDB_AIRTRACK(ARINC_OBJECT arincObj)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            this.ID_AIRTRACK = Guid.NewGuid().ToString();
            this.ARINC_OBJ = arincObj;
            this.INFO_AIRTRACK = ((ARINC_Navaid_NDB_Primary_Record)this.ARINC_OBJ).NDB_Name;
            

            double dbl = 0;
            string magvar = ((ARINC_Navaid_NDB_Primary_Record)this.ARINC_OBJ).Magnetic_Variation;
            Double.TryParse(magvar.Substring(1, magvar.Length - 1), out dbl);
            if (magvar.StartsWith("W")) dbl = dbl * -1;
            this._valMagVar = dbl/10;

            dbl = 0;
            Double.TryParse(((ARINC_Navaid_NDB_Primary_Record)this.ARINC_OBJ).NDB_Frequency, out dbl);
            this._valFreq_MHZ = dbl;

            if ((((ARINC_Navaid_NDB_Primary_Record)this.ARINC_OBJ).NDB_Latitude.Length > 0) && (((ARINC_Navaid_NDB_Primary_Record)this.ARINC_OBJ).NDB_Longitude.Trim().Length > 0))
            {

                string LonSign = ((ARINC_Navaid_NDB_Primary_Record)this.ARINC_OBJ).NDB_Longitude.Trim().Substring(0, 1);
                string LatSign = ((ARINC_Navaid_NDB_Primary_Record)this.ARINC_OBJ).NDB_Latitude.Trim().Substring(0, 1);

                string _geoLat = ((ARINC_Navaid_NDB_Primary_Record)this.ARINC_OBJ).NDB_Latitude.Trim();
                string _geoLong = ((ARINC_Navaid_NDB_Primary_Record)this.ARINC_OBJ).NDB_Longitude.Trim();

                IPoint pnt = ArnUtil.Create_ESRI_POINT(_geoLat, _geoLong, "0", "FT");

                if (pnt != null)
                {
                    //_geoLong = ArnUtil.ConvertLongitudeToAIXM45COORD(pnt.X, LonSign);
                    //_geoLat = ArnUtil.ConvertLatgitudeToAIXM45COORD(pnt.Y, LatSign);

                    this.Shape = new Shape_AIRTRACK(_geoLat, _geoLong);
                    this.Shape.Geometry = pnt as IGeometry;
                }
            }
        }

        //private ARINC_Navaid_NDB_Primary_Record _ARINC_Navaid_NDB;

        //public ARINC_Navaid_NDB_Primary_Record ARINC_Navaid_NDB
        //{
        //    get { return _ARINC_Navaid_NDB; }
        //    set { _ARINC_Navaid_NDB = value; }
        //}

        private double _valFreq_MHZ;

        public double ValFreq_MHZ
        {
            get { return _valFreq_MHZ; }
            set { _valFreq_MHZ = value; }
        }

        private double _valMagVar;

        public double ValMagVar
        {
            get { return _valMagVar; }
            set { _valMagVar = value; }
        }

    }
}
