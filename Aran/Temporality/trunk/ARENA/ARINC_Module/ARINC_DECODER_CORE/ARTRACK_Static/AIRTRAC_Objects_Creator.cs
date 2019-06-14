using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ARINC_DECODER_CORE.AIRTRACK_Objects;

namespace ARINC_DECODER_CORE.ARTRACK_Static
{
    public class AIRTRAC_Objects_Creator
    {
        private static Dictionary<Type, Type> elementTypes = new Dictionary<Type, Type>();

        static AIRTRAC_Objects_Creator()
        {
            elementTypes.Add(typeof(ARINC_Airport_Primary_Record), typeof(AIRPORT_AIRTRACK_CREATOR));
            elementTypes.Add(typeof(ARINC_Runway_Primary_Records), typeof(RUNWAY_THR_AIRTRACK_CREATOR));
            elementTypes.Add(typeof(ARINC_LocalizerGlideSlope_Primary_Record), typeof(ILS_AIRTRACK_CREATOR));
            elementTypes.Add(typeof(ARINC_WayPoint_Primary_Record), typeof(WayPoint_AIRTRACK_CREATOR));
            elementTypes.Add(typeof(ARINC_Navaid_NDB_Primary_Record), typeof(NDB_AIRTRACK_CREATOR));
            elementTypes.Add(typeof(ARINC_Navaid_VHF_Primary_Record), typeof(VHF_AIRTRACK_CREATOR));
            elementTypes.Add(typeof(ARINC_Terminal_Procedure_Primary_Record), typeof(LEG_AIRTRACK_CREATOR));
            elementTypes.Add(typeof(ARINC_Enroute_Airways_Primary_Record), typeof(SEGMENT_POINT_AIRTRACK_CREATOR));
            elementTypes.Add(typeof(ARINC_FIR_UIR_Primary_Records), typeof(Airspace_Segment_AIRTRACK_CREATOR));
            elementTypes.Add(typeof(ARINC_Restrictive_Airspace_Primary_Records), typeof(Airspace_Segment_AIRTRACK_CREATOR));
            elementTypes.Add(typeof(ARINC_Controlled_Airspace_Primary_Records), typeof(Airspace_Segment_AIRTRACK_CREATOR));
            elementTypes.Add(typeof(ARINC_Airport_Marker), typeof(Marker_AIRTRACK_CREATOR));


        }

        public static Object_AIRTRACK AIRTRAC_Object_Create(ARINC_OBJECT elem)
        {
            if (elementTypes.ContainsKey(elem.GetType()))
            {

                Type t = elementTypes[elem.GetType()];
                IAIRTRAC_Objects_Creator result = (IAIRTRAC_Objects_Creator)Activator.CreateInstance(t);
                Object_AIRTRACK obj = result.Create_AIRTRAC_Object(elem);
                return obj;

                
            }
            else
                return null;
        }
    }

    public interface IAIRTRAC_Objects_Creator
    {
        Object_AIRTRACK Create_AIRTRAC_Object(ARINC_OBJECT elem);
    }

    public class AIRPORT_AIRTRACK_CREATOR : IAIRTRAC_Objects_Creator
    {
        public AIRTRACK_Objects.Object_AIRTRACK Create_AIRTRAC_Object(ARINC_Types.ARINC_OBJECT elem)
        {
            AIRPORT_AIRTRACK _Object = new AIRPORT_AIRTRACK(elem);
            return _Object;
        }
    }

    public class RUNWAY_THR_AIRTRACK_CREATOR : IAIRTRAC_Objects_Creator
    {
        public AIRTRACK_Objects.Object_AIRTRACK Create_AIRTRAC_Object(ARINC_Types.ARINC_OBJECT elem)
        {
            RunWay_THR_AIRTRACK _Object = new RunWay_THR_AIRTRACK(elem);
            return _Object;
        }
    }

    public class ILS_AIRTRACK_CREATOR : IAIRTRAC_Objects_Creator
    {
        public AIRTRACK_Objects.Object_AIRTRACK Create_AIRTRAC_Object(ARINC_Types.ARINC_OBJECT elem)
        {
            ILS_AIRTRACK _Object = new ILS_AIRTRACK(elem);
            return _Object;
        }
    }

    public class WayPoint_AIRTRACK_CREATOR : IAIRTRAC_Objects_Creator
    {
        public AIRTRACK_Objects.Object_AIRTRACK Create_AIRTRAC_Object(ARINC_Types.ARINC_OBJECT elem)
        {
            WayPoint_AIRTRACK _Object = new WayPoint_AIRTRACK(elem);
            return _Object;
        }
    }

    public class NDB_AIRTRACK_CREATOR : IAIRTRAC_Objects_Creator
    {
        public AIRTRACK_Objects.Object_AIRTRACK Create_AIRTRAC_Object(ARINC_Types.ARINC_OBJECT elem)
        {
            NDB_AIRTRACK _Object = new NDB_AIRTRACK(elem);
            return _Object;
        }
    }

    public class VHF_AIRTRACK_CREATOR : IAIRTRAC_Objects_Creator
    {
        public AIRTRACK_Objects.Object_AIRTRACK Create_AIRTRAC_Object(ARINC_Types.ARINC_OBJECT elem)
        {
            Object_AIRTRACK _Object = new Object_AIRTRACK();
            string[] NavaidVorType = {"V","VD","VT","VM" };
            string[] NavaidDmeType = { "D", "I"};
            string[] NavaidTacanType = { "T" };

            ARINC_Navaid_VHF_Primary_Record ARINC_OBJ = (ARINC_Navaid_VHF_Primary_Record)elem;

            string nvdClass = ARINC_OBJ.NAVAID_Class.Substring(0, 2).Trim();
            if (NavaidVorType.Contains(nvdClass))
            {
                _Object = new VOR_AIRTRACK(ARINC_OBJ);

            }

            else if (NavaidDmeType.Contains(nvdClass))
            {
                _Object = new DME_AIRTRACK(ARINC_OBJ);
            }

            else if (NavaidTacanType.Contains(nvdClass))
            {
                _Object = new TACAN_AIRTRACK(ARINC_OBJ);
            }
            else return null;
            
            return _Object;
        }
    }

    public class LEG_AIRTRACK_CREATOR : IAIRTRAC_Objects_Creator
    {
        public AIRTRACK_Objects.Object_AIRTRACK Create_AIRTRAC_Object(ARINC_Types.ARINC_OBJECT elem)
        {
            Leg_AIRTRACK _Object = new Leg_AIRTRACK(elem);
            return _Object;
        }
    }

    public class SEGMENT_POINT_AIRTRACK_CREATOR : IAIRTRAC_Objects_Creator
    {
        public AIRTRACK_Objects.Object_AIRTRACK Create_AIRTRAC_Object(ARINC_Types.ARINC_OBJECT elem)
        {
            SEGMENT_POINT_AIRTRACK _Object = new SEGMENT_POINT_AIRTRACK(elem);
            return _Object;
        }
    }

    public class Airspace_Segment_AIRTRACK_CREATOR : IAIRTRAC_Objects_Creator
    {
        public AIRTRACK_Objects.Object_AIRTRACK Create_AIRTRAC_Object(ARINC_Types.ARINC_OBJECT elem)
        {
            AIRTRACK_Airspace_Segment _Object = new AIRTRACK_Airspace_Segment(elem);
            return _Object;
        }
    }


    public class Marker_AIRTRACK_CREATOR : IAIRTRAC_Objects_Creator
    {
        public AIRTRACK_Objects.Object_AIRTRACK Create_AIRTRAC_Object(ARINC_Types.ARINC_OBJECT elem)
        {
            Marker_AIRTRACK _Object = new Marker_AIRTRACK(elem);
            return _Object;
        }
    }
}
