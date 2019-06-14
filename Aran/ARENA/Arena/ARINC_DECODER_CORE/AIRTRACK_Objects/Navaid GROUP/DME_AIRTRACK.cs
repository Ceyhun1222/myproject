using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;



namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    public class DME_AIRTRACK : VHF_NAVAID_AIRTRACK
    {
         public DME_AIRTRACK()
         {
         }

         public DME_AIRTRACK(ARINC_OBJECT arincObj)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

            this.ID_AIRTRACK = Guid.NewGuid().ToString();
            this.ARINC_OBJ = arincObj;
            this.VHF_code = VHF_NAVAID_code.DME;

            string nvdClass = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).NAVAID_Class.Substring(0, 2).Trim();
            if (nvdClass.StartsWith("I")) 
                this.VHF_code = VHF_NAVAID_code.DME_ILS;
            

            this.INFO_AIRTRACK = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Ident;
            this.AirportIcaoID = ((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).Airport_ICAO_Identifier.Trim();

                double dbl = 0;
                Double.TryParse(((ARINC_Navaid_VHF_Primary_Record)this.ARINC_OBJ).DME_Elevation_FT, out dbl);
                this.ValElevM =Math.Round( ArnUtil.ConvertValueToMeter( dbl.ToString(),"M"),2);

                #region DME position

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

         //private ARINC_Navaid_VHF_Primary_Record _ARINC_DME;

         //public ARINC_Navaid_VHF_Primary_Record ARINC_DME
         //{
         //    get { return _ARINC_DME; }
         //    set { _ARINC_DME = value; }
         //}

         private double _valElevM;

         public double ValElevM
         {
             get { return _valElevM; }
             set { _valElevM = value; }
         }

         //private double _frequencyKHz;

         //public double FrequencyKHz
         //{
         //    get { return _frequencyKHz; }
         //    set { _frequencyKHz = value; }
         //}

    }
}
