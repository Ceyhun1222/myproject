using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    public class VOR_AIRTRACK : VHF_NAVAID_AIRTRACK
    {
        public VOR_AIRTRACK()
        {
        }

        public VOR_AIRTRACK(ARINC_OBJECT arincObj)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

            this.ID_AIRTRACK = Guid.NewGuid().ToString();
            this.ARINC_OBJ = arincObj;
            this.VHF_code = VHF_NAVAID_code.VOR;
            this.INFO_AIRTRACK = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).VHF_Name;
            this.AirportIcaoID = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).Airport_ICAO_Identifier.Trim();


            string nvdClass = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).NAVAID_Class.Substring(0, 2).Trim();

            #region VOR position

            if ((((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).VOR_Latitude.Length > 0) && (((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).VOR_Longitude.Trim().Length > 0))
            {

                string LonSign = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).VOR_Longitude.Trim().Substring(0, 1);
                string LatSign = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).VOR_Latitude.Trim().Substring(0, 1);

                string _geoLat = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).VOR_Latitude.Trim();
                string _geoLong = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).VOR_Longitude.Trim();

                IPoint pnt = ArnUtil.Create_ESRI_POINT(_geoLat, _geoLong, "0", "FT");

                if (pnt != null)
                {
                    this.Shape = new Shape_AIRTRACK(_geoLat, _geoLong);
                    this.Shape.Geometry = pnt as IGeometry;
                }
            }

            #endregion

            if (nvdClass.StartsWith("VD"))
            {
                this.DME = new DME_AIRTRACK();
                this.DME.ID_AIRTRACK = Guid.NewGuid().ToString();
                this.VHF_code = VHF_NAVAID_code.VOR_DME;
                ARINC_Navaid_VHF_Primary_Record ARINC_Navaid_VOR = (ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ;
                this.DME.INFO_AIRTRACK = ARINC_Navaid_VOR.DME_Ident;

                double dbl = 0;
                Double.TryParse(((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Elevation_FT, out dbl);
                this.DME.ValElevM = Math.Round( ArnUtil.ConvertValueToMeter( dbl.ToString(),"M"),2);

                #region DME position

                if ((ARINC_Navaid_VOR.DME_Latitude.Length > 0) && (ARINC_Navaid_VOR.DME_Longitude.Trim().Length > 0))
                {

                    string LonSign = ARINC_Navaid_VOR.DME_Longitude.Trim().Substring(0, 1);
                    string LatSign = ARINC_Navaid_VOR.DME_Latitude.Trim().Substring(0, 1);

                    string _geoLat = ARINC_Navaid_VOR.DME_Latitude.Trim();
                    string _geoLong = ARINC_Navaid_VOR.DME_Longitude.Trim();

                    IPoint pnt = ArnUtil.Create_ESRI_POINT(_geoLat, _geoLong, dbl.ToString(), "FT");

                    if (pnt != null)
                    {
                        this.DME.Shape = new Shape_AIRTRACK(_geoLat, _geoLong);
                        this.DME.Shape.Geometry = pnt as IGeometry;
                    }

                    
                }

                #endregion
                
            }

            if ((nvdClass.StartsWith("VT")) || (nvdClass.StartsWith("VM")))
            {
                this.TACAN = new TACAN_AIRTRACK();
                this.TACAN.ID_AIRTRACK = Guid.NewGuid().ToString();
                this.VHF_code = VHF_NAVAID_code.VOR_TACAN;
                this.TACAN.INFO_AIRTRACK = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Ident;

                double dbl = 0;
                Double.TryParse(((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Elevation_FT, out dbl);
                this.TACAN.ValElevFT = dbl;

                #region TACAN position

                if ((((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Latitude.Length > 0) && (((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Longitude.Trim().Length > 0))
                {

                    string LonSign = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Longitude.Trim().Substring(0, 1);
                    string LatSign = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Latitude.Trim().Substring(0, 1);

                    string _geoLat = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Latitude.Trim();
                    string _geoLong = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Longitude.Trim();

                    IPoint pnt = ArnUtil.Create_ESRI_POINT(_geoLat, _geoLong, dbl.ToString(), "FT");

                    if (pnt != null)
                    {
                        this.TACAN.Shape = new Shape_AIRTRACK(_geoLat, _geoLong);
                        this.TACAN.Shape.Geometry = pnt as IGeometry;
                    }


                }

                #endregion

            }


        }

        //private ARINC_Navaid_VHF_Primary_Record _ARINC_Navaid_VOR;
        //[System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
        //public ARINC_Navaid_VHF_Primary_Record ARINC_Navaid_VOR
        //{
        //    get { return _ARINC_Navaid_VOR; }
        //    set { _ARINC_Navaid_VOR = value; }
        //}


        private DME_AIRTRACK _DME;
        [System.ComponentModel.Browsable(false)]
        public DME_AIRTRACK DME
        {
            get { return _DME; }
            set { _DME = value; }
        }

        private TACAN_AIRTRACK _TACAN;
        [System.ComponentModel.Browsable(false)]
        public TACAN_AIRTRACK TACAN
        {
            get { return _TACAN; }
            set { _TACAN = value; }
        }

        private double _ValElevM;

        public double ValElevM
        {
            get { return _ValElevM; }
            set { _ValElevM = value; }
        }
    }
}
