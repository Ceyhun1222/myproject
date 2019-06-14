using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    public class Marker_AIRTRACK : Object_AIRTRACK
    {

        public Marker_AIRTRACK()
        {
        }

        public Marker_AIRTRACK(ARINC_OBJECT arincObj)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

            this.ID_AIRTRACK = Guid.NewGuid().ToString();
            this.ARINC_OBJ = arincObj;
            ARINC_Airport_Marker ARINC_Marker = (ARINC_Airport_Marker)this.ARINC_OBJ;

            #region Marker position

            if ((ARINC_Marker.Marker_Latitude.Length > 0) && (ARINC_Marker.Marker_Longitude.Trim().Length > 0))
            {

                string LonSign = ARINC_Marker.Marker_Longitude.Trim().Substring(0, 1);
                string LatSign = ARINC_Marker.Marker_Latitude.Trim().Substring(0, 1);

                string _geoLat = ARINC_Marker.Marker_Latitude.Trim();
                string _geoLong = ARINC_Marker.Marker_Longitude.Trim();

                IPoint pnt = ArnUtil.Create_ESRI_POINT(_geoLat, _geoLong, "0", "FT");

                if (pnt != null)
                {
                    //_geoLong = ArnUtil.ConvertLongitudeToAIXM45COORD(pnt.X, LonSign);
                    //_geoLat = ArnUtil.ConvertLatgitudeToAIXM45COORD(pnt.Y, LatSign);

                    this.Shape = new Shape_AIRTRACK(_geoLat, _geoLong);
                    this.Shape.Geometry = pnt as IGeometry;
                }
            }

            #endregion


            this.AirportCode = ((ARINC_Airport_Marker)this.ARINC_OBJ).Airport_Identifier;
            this.RdnCode = ((ARINC_Airport_Marker)this.ARINC_OBJ).Runway_Identifier;
            this.LoalizerCode = ((ARINC_Airport_Marker)this.ARINC_OBJ).Localizer_Identifier;
 
            this.INFO_AIRTRACK = ARINC_Marker.Name;
        }

        private string _airportCode;
        public string AirportCode
        {
            get { return _airportCode; }
            set { _airportCode = value; }
        }

        private string _rdnCode;
        public string RdnCode
        {
            get { return _rdnCode; }
            set { _rdnCode = value; }
        }

        private string _loalizerCode;
        public string LoalizerCode
        {
            get { return _loalizerCode; }
            set { _loalizerCode = value; }
        }

    }
}
