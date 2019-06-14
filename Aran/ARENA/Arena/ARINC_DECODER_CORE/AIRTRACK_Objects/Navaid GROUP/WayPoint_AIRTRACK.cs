using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    public class WayPoint_AIRTRACK :Object_AIRTRACK
    {
        public WayPoint_AIRTRACK()
        {
        }

        public WayPoint_AIRTRACK(ARINC_OBJECT arincObj)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

            this.ID_AIRTRACK = Guid.NewGuid().ToString();
            this.ARINC_OBJ = arincObj;
            this.INFO_AIRTRACK = ((ARINC_WayPoint_Primary_Record)this.ARINC_OBJ).Name;

            if ((((ARINC_WayPoint_Primary_Record)this.ARINC_OBJ).Waypoint_Latitude.Length > 0) && (((ARINC_WayPoint_Primary_Record)this.ARINC_OBJ).Waypoint_Longitude.Trim().Length > 0))
            {

                string LonSign = ((ARINC_WayPoint_Primary_Record)this.ARINC_OBJ).Waypoint_Longitude.Trim().Substring(0, 1);
                string LatSign = ((ARINC_WayPoint_Primary_Record)this.ARINC_OBJ).Waypoint_Latitude.Trim().Substring(0, 1);

                string _geoLat = ((ARINC_WayPoint_Primary_Record)this.ARINC_OBJ).Waypoint_Latitude.Trim();
                string _geoLong = ((ARINC_WayPoint_Primary_Record)this.ARINC_OBJ).Waypoint_Longitude.Trim();

                IPoint pnt = ArnUtil.Create_ESRI_POINT(_geoLat, _geoLong, "0", "FT");

                if (pnt != null)
                {
                    //_geoLong = ArnUtil.ConvertLongitudeToAIXM45COORD(pnt.X, LonSign);
                    //_geoLat = ArnUtil.ConvertLatgitudeToAIXM45COORD(pnt.Y, LatSign);

                    this.Shape = new Shape_AIRTRACK(_geoLat, _geoLong);
                    this.Shape.Geometry = pnt as IGeometry;
                }
            }
            this.Terminal_Enroute = ((ARINC_WayPoint_Primary_Record)this.ARINC_OBJ).Region_Code;
            if (((ARINC_WayPoint_Primary_Record)this.ARINC_OBJ).Region_Code.StartsWith("ENRT")) this.Terminal_Enroute = "ENROUTE";

            this.Using = DefineWypntUsing(((ARINC_WayPoint_Primary_Record)this.ARINC_OBJ).Waypoint_Type).ToString();
            this.Type = DefineWypntType(((ARINC_WayPoint_Primary_Record)this.ARINC_OBJ).Waypoint_Type, this.Terminal_Enroute.StartsWith("ENROUTE"));
            this.Function = DefineWypntFunction(((ARINC_WayPoint_Primary_Record)this.ARINC_OBJ).Waypoint_Type[1].ToString(), this.Terminal_Enroute.StartsWith("ENROUTE"));
        }

        private string DefineWypntFunction(string WaypointTypebool, bool ENROUTE)
        {
            string res = "";

            if (!ENROUTE)
            {
                if (Type.StartsWith("A")) res = "Final Approach Fix";
                if (Type.StartsWith("B")) res = "Initial Approach Fix and Final Approach Fix";
                if (Type.StartsWith("C")) res = "Final Approach Course Fix";
                if (Type.StartsWith("D")) res = "Intermediate Approach Fix";
                if (Type.StartsWith("I")) res = "Initial Approach Fix";
                if (Type.StartsWith("K")) res = "Final Approach Course Fix at Initial Approach Fix";
                if (Type.StartsWith("L")) res = "Final Approach Course Fix at Intermediate Approach Fix";
                if (Type.StartsWith("M")) res = "Missed Approach Fix Initial Approach Fix and Missed Approach Fix";
                if (Type.StartsWith("N")) res = "Unnamed Stepdown Fix";
                if (Type.StartsWith("P")) res = "Named Stepdown Fix";
                if (Type.StartsWith("S")) res = "FIR/UIR or Controlled";
                if (Type.StartsWith("U")) res = "Airspace Intersection";


            }
            else
            {
                if (Type.StartsWith("A")) res = "Final Approach Fix";
                if (Type.StartsWith("B")) res = "Initial and Final Approach Fix";
                if (Type.StartsWith("C")) res = "Final Approach Course Fix";
                if (Type.StartsWith("D")) res = "Intermediate Approach Fix";
                if (Type.StartsWith("F")) res = "Off-Route Intersection";
                if (Type.StartsWith("I")) res = "Initial Approach Fix";
                if (Type.StartsWith("K")) res = "Final Approach Course Fix at Initial Approach Fix";
                if (Type.StartsWith("L")) res = "Final Approach Course Fix at Intermediate Approach Fix";
                if (Type.StartsWith("M")) res = "Missed Approach Fix";
                if (Type.StartsWith("N")) res = "Initial Approach Fix and Missed Approach Fix";
                if (Type.StartsWith("O")) res = "Oceanic Entry_Exit Waypoint";
                if (Type.StartsWith("U")) res = "FIR_UIR or Controlled Airspace Intersection";
                if (Type.StartsWith("V")) res = "Latitude_Longitude Intersection,Full Degree of Latitude";
                if (Type.StartsWith("W")) res = "Latitude_Longitude Intersection,Half Degree of Latitude";


            }

            return res;
        }

        private string DefineWypntType(string Type, bool ENROUTE)
        {
            string res = "";

            if (!ENROUTE)
            {
                if (Type.StartsWith("A")) res = "ARC Center Fix Waypoint";
                if (Type.StartsWith("C")) res = "Combined Named Intersection and RNAV Waypoint";
                if (Type.StartsWith("I")) res = "Unnamed, Charted Intersection";
                if (Type.StartsWith("M")) res = "Middle Marker as Waypoint";
                if (Type.StartsWith("N")) res = "Terminal NDB Navaid as Waypoint";
                if (Type.StartsWith("O")) res = "Outer Marker as Waypoint";
                if (Type.StartsWith("R")) res = "Named Intersection";
                if (Type.StartsWith("W")) res = "RNAV Waypoint";

            }
            else
            {
                if (Type.StartsWith("C")) res = "Combined Named Intersection and RNAV";
                if (Type.StartsWith("I")) res = "Unnamed, Charted Intersection";
                if (Type.StartsWith("N")) res = "NDB Navaid as Waypoint";
                if (Type.StartsWith("R")) res = "Named Intersection";
                if (Type.StartsWith("U")) res = "Uncharted Airway Intersection";
                if (Type.StartsWith("W")) res = "RNAV Waypoint";

            }

            return res;
        }

        private PROC_TYPE_code DefineWypntUsing(string WaypointType)
        {
            PROC_TYPE_code res = PROC_TYPE_code.Multiple;
            if (WaypointType.Length < 3) return res;

            if (WaypointType.EndsWith("D")) res = PROC_TYPE_code.SID;
            if (WaypointType.EndsWith("E")) res = PROC_TYPE_code.STAR;
            if (WaypointType.EndsWith("F")) res = PROC_TYPE_code.Approach;
            if (WaypointType.EndsWith("Z")) res = PROC_TYPE_code.Multiple;
            return res;
        }

        //private ARINC_WayPoint_Primary_Record _ARINC_WayPoint;

        //public ARINC_WayPoint_Primary_Record ARINC_WayPoint
        //{
        //    get { return _ARINC_WayPoint; }
        //    set { _ARINC_WayPoint = value; }
        //}

        //private Fix_type _fixType;

        //public Fix_type FixType
        //{
        //    get { return _fixType; }
        //    set { _fixType = value; }
        //}

        private string _terminal_Enroute;

        public string Terminal_Enroute
        {
            get { return _terminal_Enroute; }
            set { _terminal_Enroute = value; }
        }

        private string _Using;

        public string Using
        {
            get { return _Using; }
            set { _Using = value; }
        }

        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _function;

        public string Function
        {
            get { return _function; }
            set { _function = value; }
        }
    }
}
