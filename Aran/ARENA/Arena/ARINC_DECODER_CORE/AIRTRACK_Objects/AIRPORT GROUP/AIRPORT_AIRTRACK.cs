using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;


namespace ARINC_DECODER_CORE.AIRTRACK_Objects //namespace ARINC_DECODER_CORE.AIRTRACK_Objects //namespace AirTrack.AIRTRACK_Objects
{
    public class AIRPORT_AIRTRACK : Object_AIRTRACK
    {
        //private ARINC_Airport_Primary_Record _ARINC_Airport;
        //public ARINC_Airport_Primary_Record ARINC_Airport
        //{
        //  get { return _ARINC_Airport; }
        //  set { _ARINC_Airport = value; }
        //}


        [System.ComponentModel.Browsable(false)]
        private List<RunWay_AIRTRACK> _LinkedRWY;
        [System.ComponentModel.Browsable(false)]
        public List<RunWay_AIRTRACK> LinkedRWY
        {
            get { return _LinkedRWY; }
            set { _LinkedRWY = value; }
        }

        [System.ComponentModel.Browsable(false)]
        private double _valElev_m;
        public double ValElev_m
        {
            get { return _valElev_m; }
            set { _valElev_m = value; }
        }

         [System.ComponentModel.Browsable(false)]
        private double _magnetic_Variation;
        public double Magnetic_Variation
        {
            get { return _magnetic_Variation; }
            set { _magnetic_Variation = value; }
        }

         [System.ComponentModel.Browsable(false)]
        private List<Procedure_AIRTRACK> _linkedProceduresList;
         [System.ComponentModel.Browsable(false)]
        public List<Procedure_AIRTRACK> LinkedProceduresList
        {
            get { return _linkedProceduresList; }
            set { _linkedProceduresList = value; }
        }

        public AIRPORT_AIRTRACK()
        {
        }

        public AIRPORT_AIRTRACK(ARINC_OBJECT arincObj)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

            this.ID_AIRTRACK = Guid.NewGuid().ToString();
            this.ARINC_OBJ = arincObj;

            ARINC_Airport_Primary_Record ARINC_Airport = (ARINC_Airport_Primary_Record)this.ARINC_OBJ;
            #region

            double dbl = 0;

            Double.TryParse(ARINC_Airport.Airport_Elevation, out dbl);
            this.ValElev_m = ArnUtil.ConvertValueToMeter(dbl.ToString(),"FT");

            string MV = ARINC_Airport.Magnetic_Variation;
            int i = 1;
            if (MV.StartsWith("W"))
            {
                i = -1;
            }
            MV = MV.Substring(1, MV.Length - 1);
            Double.TryParse(MV, out dbl);

            this.Magnetic_Variation = dbl / 10 * i;


            dbl = 0;


            if ((ARINC_Airport.Airport_Reference_Pt_Latitude.Length > 0) && (ARINC_Airport.Airport_Reference_Pt_Longitude.Trim().Length > 0))
            {

                string LonSign = ARINC_Airport.Airport_Reference_Pt_Longitude.Trim().Substring(0, 1);
                string LatSign = ARINC_Airport.Airport_Reference_Pt_Latitude.Trim().Substring(0, 1);

                string _geoLat = ARINC_Airport.Airport_Reference_Pt_Latitude.Trim();
                string _geoLong = ARINC_Airport.Airport_Reference_Pt_Longitude.Trim();

                IPoint pnt = ArnUtil.Create_ESRI_POINT(_geoLat, _geoLong, this._valElev_m.ToString(), "M");

                if (pnt != null)
                {
                    this.Shape = new Shape_AIRTRACK(_geoLat, _geoLong);
                    this.Shape.Geometry = pnt as IGeometry;
                }
            }

            this.INFO_AIRTRACK = ARINC_Airport.Airport_Name.TrimEnd();
            #endregion
        }

        #region

   
        #endregion

    }
}
