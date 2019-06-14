using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    public class TACAN_AIRTRACK : VHF_NAVAID_AIRTRACK
    {
        public TACAN_AIRTRACK()
        {
        }


        public TACAN_AIRTRACK(ARINC_OBJECT arincObj)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

            this.ID_AIRTRACK = Guid.NewGuid().ToString();
            this.ARINC_OBJ = arincObj;
            this.VHF_code = VHF_NAVAID_code.TACAN;
            this.INFO_AIRTRACK = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Ident;
            this.AirportIcaoID = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).Airport_ICAO_Identifier.Trim();

                double dbl = 0;
                Double.TryParse(((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Elevation_FT, out dbl);
                this.ValElevFT = dbl;

                #region position

                if ((((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Latitude.Length > 0) && (((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Longitude.Trim().Length > 0))
                {

                    string LonSign = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Longitude.Trim().Substring(0, 1);
                    string LatSign = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Latitude.Trim().Substring(0, 1);

                    string _geoLat = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Latitude.Trim();
                    string _geoLong = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Longitude.Trim();

                    IPoint pnt = ArnUtil.Create_ESRI_POINT(_geoLat, _geoLong, dbl.ToString(), "FT");

                    if (pnt != null)
                    {
                        this.Shape = new Shape_AIRTRACK(_geoLat, _geoLong);
                        this.Shape.Geometry = pnt as IGeometry;
                    }

                    
                }

                #endregion
                
            


        }

        //private ARINC_Navaid_VHF_Primary_Record _ARINC_TACAN;

        //public ARINC_Navaid_VHF_Primary_Record ARINC_TACAN
        //{
        //    get { return _ARINC_TACAN; }
        //    set { _ARINC_TACAN = value; }
        //}

        private double _valElevFT;

        public double ValElevFT
        {
            get { return _valElevFT; }
            set { _valElevFT = value; }
        }


    }
}
