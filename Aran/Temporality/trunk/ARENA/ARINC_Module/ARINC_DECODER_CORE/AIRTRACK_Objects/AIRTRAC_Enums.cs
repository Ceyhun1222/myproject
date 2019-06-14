using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{

    public enum Fix_type
    {
        None,
        Airport_as_Waypoint,
        Essential_Waypoint,
        Off_Airway_Waypoint,
        Runway_as_WaypointHelipad_as_Waypoint,
        Heliport_as_Waypoint,
        NDB_Navaid_as_Waypoint,
        Phantom_Waypoint,
        Non_Essential_Waypoint,
        Transition_Essential_Waypoint,
        VHF_Navaid_As_Waypoint
    }

    public enum wyp_type
    {
        None,
        Flyover_Waypoint_End_of_STAR_Route_Type_Transition_or_Final_Approach,
        End_of_Airway_or_Terminal_Procedure_Route_Type,
        Uncharted_Airway_Intersection,
        Fly_Over_Waypoint_
    }

    public enum FixFunction
    {
        None,
        Unnamed_Stepdown_Fix_After_Final_Approach_Fix,
        Unnamed_Stepdown_Fix_Before_Final_Approach_Fix,
        ATC_Compulsory_Waypoint,
        Oceanic_Gateway_Waypoint,
        First_Leg_of_Missed_Approach_Procedure,
        Path_Point_Fix,
        Named_Stepdown_Fix
    }

    public enum Fix_Role
    {
        None,
        Initial_Approach_Fix,
        Intermediate_Approach_Fix,
        Initial_Approach_Fix_with_Holding,
        Initial_Approach_Fix_with_Final_Approach_Course_Fix,
        Final_End_Point_Fix,
        Published_Final_Approach_Fix_or_Database_Final_Approach_Fix,
        Holding_Fix,
        Final_Approach_Course_Fix,
        Published_Missed_Approach_Point_Fix
    }

    public enum Route_Type
    {
        None,

        #region SID

        Engine_Out_SID,
        SID_Runway_Transition,
        SID_or_SID_Common_Route,
        SID_Enroute_Transition,
        RNAV_SID_Runway_Transition,
        RNAV_SID_or_SID_Common_Route,
        RNAV_SID_Enroute_Transition,
        FMS_SID_Runway_Transition,
        FMS_SID_or_SID_Common_Route,
        FMS_SID_Enroute_Transition,
        Vector_SID_Runway_Transition,
        Vector_SID_Enroute_Transition,

        #endregion

        #region STAR

        STAR_Enroute_Transition,
        STAR_or_STAR_Common_Route,
        STAR_Runway_Transition,
        RNAV_STAR_Enroute_Transition,
        RNAV_STAR_or_STAR_Common_Route,
        RNAV_STAR_Runway_Transition,
        Profile_Descent_Enroute_Transition,
        Profile_Descent_Common_Route,
        Profile_Descent_Runway_Transition,
        FMS_STAR_Enroute_Transition,
        FMS_STAR_or_STAR_Common_Route,
        FMS_STAR_Runway_Transition,

        #endregion

        #region IAP

        Approach_Transition,
        Localizer_Backcourse_Approach,
        RNAV_GPS_Required_Approach,
        Flight_Management_System_Approach,
        Instrument_Guidance_System_Approach,
        Instrument_Landing_System_Approach,
        LAAS_GPS_GLS_Approach,
        WAAS_GPS_Approach,
        Localizer_Only_Approach,
        Microwave_Landing_System_Approach,
        Non_Directional_Beacon_Approach,
        Global_Positioning_System_Approach,
        Area_Navigation_Approach,
        TACAN_Approach,
        Simplified_Directional_Facility_Approach,
        VOR_Approach,
        Microwave_Landing_System_Type_A_Approach,
        Localizer_Directional_Aid_Approach,
        Microwave_Landing_System_Type_B_and_C_Approach,
        Missed_Approach,


        #endregion

    }

    public enum PROC_TYPE_code
    {
        SID = 0,
        STAR = 1,
        Approach = 2,
        Multiple =3,
        
    }
}
