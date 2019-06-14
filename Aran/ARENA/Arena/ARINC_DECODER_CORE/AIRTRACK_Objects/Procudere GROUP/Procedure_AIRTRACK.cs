using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    public class ProcedureBranch_AIRTRACK 
    {

        private string _PROC_BRANCH_ID;
        [System.ComponentModel.Browsable(false)]
        public string PROC_BRANCH_ID
        {
            get { return _PROC_BRANCH_ID; }
            set { _PROC_BRANCH_ID = value; }
        }

        private PROC_TYPE_code _proc_type;
        [System.ComponentModel.Browsable(false)]
        public PROC_TYPE_code Proc_type
        {
            get { return _proc_type; }
            set { _proc_type = value; }
        }

        private string _AirportIdentifier;
        //[System.ComponentModel.Browsable(false)]
        public string AirportIdentifier
        {
            get { return _AirportIdentifier; }
            set { _AirportIdentifier = value; }
        }

        private string _Transition_Identifier;
        //[System.ComponentModel.Browsable(false)]
        public string Transition_Identifier
        {
            get { return _Transition_Identifier; }
            set { _Transition_Identifier = value; }
        }

        private string _proc_branch_Identifier;
        [System.ComponentModel.Browsable(false)]
        public string Proc_Branch_Identifier
        {
            get { return _proc_branch_Identifier; }
            set { _proc_branch_Identifier = value; }
        }

        private List<Leg_AIRTRACK> _ProcedureLegs;
        [System.ComponentModel.Browsable(false)]
        public List<Leg_AIRTRACK> ProcedureLegs
        {
            get { return _ProcedureLegs; }
            set { _ProcedureLegs = value; }
        }

        private List<string> _DebugMessages;
        [System.ComponentModel.Browsable(false)]
        public List<string> DebugMessages
        {
            get { return _DebugMessages; }
            set { _DebugMessages = value; }
        }

        private Route_Type _route_type;
        //[System.ComponentModel.Browsable(false)]
        public Route_Type Route_type
        {
            get { return _route_type; }
            set { _route_type = value; }
        }

        private string _Proc_Identifier;
       // [System.ComponentModel.Browsable(false)]
        public string Proc_Identifier
        {
            get { return _Proc_Identifier; }
            set { _Proc_Identifier = value; }
        }

        public ProcedureBranch_AIRTRACK()
        {
        }

        public ProcedureBranch_AIRTRACK(string ARINC_PROC_OBJ_NAME)
        {
            if (ARINC_PROC_OBJ_NAME.CompareTo("STARProcedures") == 0) 
                this._proc_type = PROC_TYPE_code.STAR;
            if (ARINC_PROC_OBJ_NAME.CompareTo("SIDProcedures") == 0) 
                this._proc_type = PROC_TYPE_code.SID;
            if (ARINC_PROC_OBJ_NAME.CompareTo("ApproachProcedures") == 0) this._proc_type = PROC_TYPE_code.Approach;

            this.PROC_BRANCH_ID = Guid.NewGuid().ToString();
        }

        public void DefineType()
        {


            if (this.ProcedureLegs.Count > 0)
            {
                string legType = this.ProcedureLegs[0].Route_Type[0].ToString();
                switch (this.Proc_type)
                {
                    case (PROC_TYPE_code.Approach):

                        if (legType.StartsWith("A")) this.Route_type = Route_Type.Approach_Transition;
                        if (legType.StartsWith("B")) this.Route_type = Route_Type.Localizer_Backcourse_Approach;
                        if (legType.StartsWith("E")) this.Route_type = Route_Type.RNAV_GPS_Required_Approach;
                        if (legType.StartsWith("F")) this.Route_type = Route_Type.Flight_Management_System_Approach;
                        if (legType.StartsWith("G")) this.Route_type = Route_Type.Instrument_Guidance_System_Approach;
                        if (legType.StartsWith("I")) this.Route_type = Route_Type.Instrument_Landing_System_Approach;
                        if (legType.StartsWith("J")) this.Route_type = Route_Type.LAAS_GPS_GLS_Approach;
                        if (legType.StartsWith("K")) this.Route_type = Route_Type.WAAS_GPS_Approach;
                        if (legType.StartsWith("L")) this.Route_type = Route_Type.Localizer_Only_Approach;
                        if (legType.StartsWith("M")) this.Route_type = Route_Type.Microwave_Landing_System_Approach;
                        if (legType.StartsWith("N")) this.Route_type = Route_Type.Non_Directional_Beacon_Approach;
                        if (legType.StartsWith("P")) this.Route_type = Route_Type.Global_Positioning_System_Approach;
                        if (legType.StartsWith("R")) this.Route_type = Route_Type.Area_Navigation_Approach;
                        if (legType.StartsWith("T")) this.Route_type = Route_Type.TACAN_Approach;
                        if (legType.StartsWith("U")) this.Route_type = Route_Type.Simplified_Directional_Facility_Approach;
                        if (legType.StartsWith("V")) this.Route_type = Route_Type.VOR_Approach;
                        if (legType.StartsWith("W")) this.Route_type = Route_Type.Microwave_Landing_System_Type_A_Approach;
                        if (legType.StartsWith("X")) this.Route_type = Route_Type.Localizer_Directional_Aid_Approach;
                        if (legType.StartsWith("Y")) this.Route_type = Route_Type.Microwave_Landing_System_Type_B_and_C_Approach;
                        if (legType.StartsWith("Z")) this.Route_type = Route_Type.Missed_Approach;


                        break;

                    case (PROC_TYPE_code.SID):

                        if (legType.StartsWith("0")) this.Route_type = Route_Type.Engine_Out_SID;
                        if (legType.StartsWith("1")) this.Route_type = Route_Type.SID_Runway_Transition;
                        if (legType.StartsWith("2")) this.Route_type = Route_Type.SID_or_SID_Common_Route;
                        if (legType.StartsWith("3")) this.Route_type = Route_Type.SID_Enroute_Transition;
                        if (legType.StartsWith("4")) this.Route_type = Route_Type.RNAV_SID_Runway_Transition;
                        if (legType.StartsWith("5")) this.Route_type = Route_Type.RNAV_SID_or_SID_Common_Route;
                        if (legType.StartsWith("6")) this.Route_type = Route_Type.RNAV_SID_Enroute_Transition;
                        if (legType.StartsWith("F")) this.Route_type = Route_Type.FMS_SID_Runway_Transition;
                        if (legType.StartsWith("M")) this.Route_type = Route_Type.FMS_SID_or_SID_Common_Route;
                        if (legType.StartsWith("S")) this.Route_type = Route_Type.FMS_SID_Enroute_Transition;
                        if (legType.StartsWith("T")) this.Route_type = Route_Type.Vector_SID_Runway_Transition;
                        if (legType.StartsWith("V")) this.Route_type = Route_Type.Vector_SID_Enroute_Transition;

                        break;

                    case (PROC_TYPE_code.STAR):

                        if (legType.StartsWith("1")) this.Route_type = Route_Type.STAR_Enroute_Transition;
                        if (legType.StartsWith("2")) this.Route_type = Route_Type.STAR_or_STAR_Common_Route;
                        if (legType.StartsWith("3")) this.Route_type = Route_Type.STAR_Runway_Transition;
                        if (legType.StartsWith("4")) this.Route_type = Route_Type.RNAV_STAR_Enroute_Transition;
                        if (legType.StartsWith("5")) this.Route_type = Route_Type.RNAV_STAR_or_STAR_Common_Route;
                        if (legType.StartsWith("6")) this.Route_type = Route_Type.RNAV_STAR_Runway_Transition;
                        if (legType.StartsWith("7")) this.Route_type = Route_Type.Profile_Descent_Enroute_Transition;
                        if (legType.StartsWith("8")) this.Route_type = Route_Type.Profile_Descent_Common_Route;
                        if (legType.StartsWith("9")) this.Route_type = Route_Type.Profile_Descent_Runway_Transition;
                        if (legType.StartsWith("F")) this.Route_type = Route_Type.FMS_STAR_Enroute_Transition;
                        if (legType.StartsWith("M")) this.Route_type = Route_Type.FMS_STAR_or_STAR_Common_Route;
                        if (legType.StartsWith("S")) this.Route_type = Route_Type.FMS_STAR_Runway_Transition;


                        break;

                }

            }
            
        }
    }

    public class Procedure_AIRTRACK : Object_AIRTRACK
    {
        public Procedure_AIRTRACK()
        {
        }

        public Procedure_AIRTRACK(List<ProcedureBranch_AIRTRACK> ProcBranches)
        {
            this.PROC_ID = Guid.NewGuid().ToString();
            this.AirportIdentifier = ProcBranches[0].AirportIdentifier;
            this.Proc_type = ProcBranches[0].Proc_type;
            this.Proc_Identifier = ProcBranches[0].Proc_Identifier;
            this.Branches = ProcBranches;
        }

        private string _PROC_ID;
        [System.ComponentModel.Browsable(false)]
        public string PROC_ID
        {
            get { return _PROC_ID; }
            set { _PROC_ID = value; }
        }

        private PROC_TYPE_code _proc_type;
        [System.ComponentModel.Browsable(false)]
        public PROC_TYPE_code Proc_type
        {
            get { return _proc_type; }
            set { _proc_type = value; }
        }

        private string _AirportIdentifier;
        //[System.ComponentModel.Browsable(false)]
        public string AirportIdentifier
        {
            get { return _AirportIdentifier; }
            set { _AirportIdentifier = value; }
        }

        private string _Proc_Identifier;
        // [System.ComponentModel.Browsable(false)]
        public string Proc_Identifier
        {
            get { return _Proc_Identifier; }
            set { _Proc_Identifier = value; }
        }

        private List<ProcedureBranch_AIRTRACK> _Branches;
        [System.ComponentModel.Browsable(false)]
        public List<ProcedureBranch_AIRTRACK> Branches
        {
            get { return _Branches; }
            set { _Branches = value; }
        }

        
    }
}
