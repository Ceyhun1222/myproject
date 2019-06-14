using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Leg_AIRTRACK : Object_AIRTRACK
    {
        public Leg_AIRTRACK()
        {
        }

        public Leg_AIRTRACK(ARINC_OBJECT arincObj)
        {
            //AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            this.ID_AIRTRACK = Guid.NewGuid().ToString();
            this.ARINC_ProcedureLeg = (ARINC_Terminal_Procedure_Primary_Record)arincObj;
            ARINC_Terminal_Procedure_Primary_Record _Record = (ARINC_Terminal_Procedure_Primary_Record)arincObj;

            int FLkoef = 1;

            this.Procedure_Identifier = _Record.SID_STAR_Approach_Identifier;
            //this.AirportIdentifier = _Record.Airport_Identifier;
            this.Transition_Identifier = _Record.Transition_Identifier;
            this.Route_Type = _Record.Route_Type;
            //this.flag = false;


            this.FixIdentifier = _Record.Fix_Identifier;
            this.RecomendedNavaid = _Record.Recommended_Navaid;
            this.CenterFix = _Record.Center_Fix;

            if ((GetProcType(_Record.Name).CompareTo("IAP") == 0))
            {
                this.Transition_Identifier = "RW" + this.Procedure_Identifier.Substring(1, this.Procedure_Identifier.Length - 1);
                this.Procedure_Identifier = _Record.SID_STAR_Approach_Identifier.Trim() +"_"+ _Record.Transition_Identifier.Trim();

                if ((!Transition_Identifier.EndsWith("L")) || (!Transition_Identifier.EndsWith("R")) || (!Transition_Identifier.EndsWith("G")) || (!Transition_Identifier.EndsWith("D")))
                {
                    //this.Transition_Identifier = this.Transition_Identifier.Substring(0, this.Transition_Identifier.Length - 1)+" ";
                    if (this.Transition_Identifier.Length > 5) this.Transition_Identifier = this.Transition_Identifier.Substring(0, 4);
                    this.Transition_Identifier = this.Transition_Identifier + " ";

                }
            }

            //this.codePhase = _Record.Route_Type;
            this.Path_and_Termination = _Record.Path_and_Termination;

            double dbl;
            if (_Record.Magnetic_Course.Trim().Length > 0)
            {
                Double.TryParse(_Record.Magnetic_Course, out dbl);
                this.MagneticCourse = dbl / 10;
            }
            else
                this.MagneticCourse = Double.NaN;

           
            this.TurnDirection = _Record.Turn_Direction;
            this.TurnDirectionValid = _Record.Turn_Direction_Valid.StartsWith("Y");
            this.RoleFix = GetFixRole(_Record.Waypoint_Description_Code);

            this.FlyBy_FlyOver = GetFlyBy_FlyOverFlag(_Record.Waypoint_Description_Code);

            this.Altitude_Description = _Record.Altitude_Description;

            if (_Record.Altitude2.StartsWith("FL"))
            {
                _Record.Altitude2 = _Record.Altitude2.Substring(2);
                FLkoef = 1000;
            }
            else
                FLkoef = 1;

            if (_Record.Altitude2.Trim().Length > 0)
            {
                Double.TryParse(_Record.Altitude2, out dbl);
                this.ValDistVerUpper = (dbl * FLkoef);
            }
            else
                this.ValDistVerUpper = Double.NaN;
            //this.codeDistVerUpper = _Record.;


            if (_Record.Altitude1.StartsWith("FL"))
            {
                _Record.Altitude1 = _Record.Altitude1.Substring(2);
                FLkoef = 1000;
            }
            else
                FLkoef = 1;

            if (_Record.Altitude1.Trim().Length > 0)
            {
                Double.TryParse(_Record.Altitude1, out dbl);
                this.ValDistVerLower = (dbl * FLkoef);
            }
            else
                this.ValDistVerLower = Double.NaN;

            if (_Record.Vertical_Angle.Trim().Length > 0)
            {
                Double.TryParse(_Record.Vertical_Angle, out dbl);
                this.ValVerAngle = dbl / 100;
            }
            else
                this.ValVerAngle = Double.NaN;


            if (_Record.Speed_Limit.Trim().Length > 0)
            {
                Double.TryParse(_Record.Speed_Limit, out dbl);
                this.SpeedLimin = dbl;
                
            }
            else
                this.SpeedLimin = Double.NaN;


            if (_Record.Route_Distance_Holding_Distance_or_Time.Trim().Length > 0)
            {
                double V = 388.92; //(210 KT = 388.92 KM/H)

                if (this.SpeedLimin!=Double.NaN) V = this.SpeedLimin;

                if (_Record.Route_Distance_Holding_Distance_or_Time.StartsWith("T"))
                {
                    _Record.Route_Distance_Holding_Distance_or_Time = _Record.Route_Distance_Holding_Distance_or_Time.Substring(1);
                    Double.TryParse(_Record.Route_Distance_Holding_Distance_or_Time, out dbl);
                    this.RouteDistance_HoldingDistance = dbl * 60 * (V / 3.6);

                }
                else
                {
                    Double.TryParse(_Record.Route_Distance_Holding_Distance_or_Time, out dbl);
                    this.RouteDistance_HoldingDistance = (dbl / 10);
                    //this.valDist = dbl / 10;
                }
            }
            else
            {
                this.RouteDistance_HoldingDistance = Double.NaN;
            }

            if (_Record.Theta.Trim().Length > 0)
            {
                Double.TryParse(_Record.Theta, out dbl);
                this.Theta = dbl / 10;
            }
            else
                this.Theta = Double.NaN;

            if (_Record.Rho.Trim().Length > 0)
            {
                Double.TryParse(_Record.Rho, out dbl);
                this.Rho = dbl / 10;
            }
            else
                this.Rho = Double.NaN;


            if (_Record.ARC_Radius.Trim().Length > 0)
            {
                Double.TryParse(_Record.ARC_Radius, out dbl);
                this.ARC_Radius =(dbl / 10000);
            }
            else
                this.ARC_Radius = Double.NaN;


            //this.valBankAngle = 25;
            this.Waypoint_Description_Code = new Waypoint_Description_Code(_Record.Waypoint_Description_Code);//GetFixRole(_Record.Waypoint_Description_Code);

            int n;
            Int32.TryParse(_Record.Sequence_Number, out n);
            this.No_Seq = n;

        }

        private string _Transition_Identifier;
        [System.ComponentModel.Browsable(false)]
        public string Transition_Identifier
        {
            get { return _Transition_Identifier; }
            set { _Transition_Identifier = value; }
        }

        private string _Procedure_Identifier;
        [System.ComponentModel.Browsable(false)]
        public string Procedure_Identifier
        {
            get { return _Procedure_Identifier; }
            set { _Procedure_Identifier = value; }
        }

        private string _TurnDirection;
        [System.ComponentModel.Browsable(false)]
        public string TurnDirection
        {
            get { return _TurnDirection; }
            set { _TurnDirection = value; }
        }

        private bool _TurnDirectionValid;
        [System.ComponentModel.Browsable(false)]
        public bool TurnDirectionValid
        {
            get { return _TurnDirectionValid; }
            set { _TurnDirectionValid = value; }
        }

        private string _FlyBy_FlyOver;
        [System.ComponentModel.Browsable(false)]
        public string FlyBy_FlyOver
        {
            get { return _FlyBy_FlyOver; }
            set { _FlyBy_FlyOver = value; }
        }

        private string _RoleFix;
        [System.ComponentModel.Browsable(false)]
        public string RoleFix
        {
            get { return _RoleFix; }
            set { _RoleFix = value; }
        }

        private string _FixIdentifier;
        [System.ComponentModel.Browsable(false)]
        public string FixIdentifier
        {
            get { return _FixIdentifier; }
            set { _FixIdentifier = value; }
        }

        private string _RecomendedNavaid;
        [System.ComponentModel.Browsable(false)]
        public string RecomendedNavaid
        {
            get { return _RecomendedNavaid; }
            set { _RecomendedNavaid = value; }
        }

        private string _CenterFix;
        [System.ComponentModel.Browsable(false)]
        public string CenterFix
        {
            get { return _CenterFix; }
            set { _CenterFix = value; }
        }

        private double _MagneticCourse;
        [System.ComponentModel.Browsable(false)]
        public double MagneticCourse
        {
            get { return _MagneticCourse; }
            set { _MagneticCourse = value; }
        }

        private double _ValDistVerUpper;
        [System.ComponentModel.Browsable(false)]
        public double ValDistVerUpper
        {
            get { return _ValDistVerUpper; }
            set { _ValDistVerUpper = value; }
        }

        private double _ValDistVerLower;
        [System.ComponentModel.Browsable(false)]
        public double ValDistVerLower
        {
            
            get { return _ValDistVerLower; }
            set { _ValDistVerLower = value; }
        }

        private double _ValVerAngle;
        [System.ComponentModel.Browsable(false)]
        public double ValVerAngle
        {
            get { return _ValVerAngle; }
            set { _ValVerAngle = value; }
        }

        private double _SpeedLimin;
        [System.ComponentModel.Browsable(false)]
        public double SpeedLimin
        {
            get { return _SpeedLimin; }
            set { _SpeedLimin = value; }
        }

        private double _RouteDistance_HoldingDistance;
        [System.ComponentModel.Browsable(false)]
        public double RouteDistance_HoldingDistance
        {
            get { return _RouteDistance_HoldingDistance; }
            set { _RouteDistance_HoldingDistance = value; }
        }

        private double _Theta;
        [System.ComponentModel.Browsable(false)]
        public double Theta
        {
            get { return _Theta; }
            set { _Theta = value; }
        }

        private double _Rho;
        [System.ComponentModel.Browsable(false)]
        public double Rho
        {
            get { return _Rho; }
            set { _Rho = value; }
        }

        private double _ARC_Radius;
        [System.ComponentModel.Browsable(false)]
        public double ARC_Radius
        {
            get { return _ARC_Radius; }
            set { _ARC_Radius = value; }
        }

        private int _No_Seq;
        [System.ComponentModel.Browsable(false)]
        public int No_Seq
        {
            get { return _No_Seq; }
            set { _No_Seq = value; }
        }

        private ARINC_Terminal_Procedure_Primary_Record _ARINC_ProcedureLeg;
        [System.ComponentModel.Browsable(false)]
        public ARINC_Terminal_Procedure_Primary_Record ARINC_ProcedureLeg
        {
            get { return _ARINC_ProcedureLeg; }
            set { _ARINC_ProcedureLeg = value; }
        }

        private string _Route_Type;
        [System.ComponentModel.Browsable(false)]
        public string Route_Type
        {
            get { return _Route_Type; }
            set { _Route_Type = value; }
        }


        private string _Path_and_Termination;
        [System.ComponentModel.Browsable(false)]
        public string Path_and_Termination
        {
            get { return _Path_and_Termination; }
            set { _Path_and_Termination = value; }
        }


        private string GetProcType(string name)
        {
            string res = "";
            switch (name)
            {
                case ("SIDProcedures"):
                    res = "SID";
                    break;

                case ("STARProcedures"):
                    res = "STAR";
                    break;

                case ("ApproachProcedures"):
                    res = "IAP";
                    break;
            }

            return res;
        }

        private string GetFlyBy_FlyOverFlag(string navaidDescription)
        {
            string res = "";
            try
            {
                if (navaidDescription.Length > 2)
                {
                    if (navaidDescription[1].CompareTo('B') == 0) res = "Y";
                    if (navaidDescription[1].CompareTo('E') == 0) res = "Y";
                    if (navaidDescription[1].CompareTo('U') == 0) res = "Y";
                    if (navaidDescription[1].CompareTo('Y') == 0) res = "Y";
                }
                else
                {
                    if (navaidDescription.CompareTo("B") == 0) res = "Y";
                    if (navaidDescription.CompareTo("E") == 0) res = "Y";
                    if (navaidDescription.CompareTo("U") == 0) res = "Y";
                    if (navaidDescription.CompareTo("Y") == 0) res = "Y";
                }
            }
            catch
            { res = ""; }
            return res;
        }

        private string GetFixRole(string navaidDescription)
        {
            string res = "";
            try
            {
                if (navaidDescription.Length > 3)
                {
                    if (navaidDescription[3].CompareTo('A') == 0) res = "IAF";
                    if (navaidDescription[3].CompareTo('B') == 0) res = "IF";
                    if (navaidDescription[3].CompareTo('C') == 0) res = "IAF";
                    if (navaidDescription[3].CompareTo('D') == 0) res = "IF_IAF";
                    if (navaidDescription[3].CompareTo('E') == 0) res = "OTHER";
                    if (navaidDescription[3].CompareTo('F') == 0) res = "FAF";
                    if (navaidDescription[3].CompareTo('H') == 0) res = "MAHF";
                    if (navaidDescription[3].CompareTo('I') == 0) res = "FAF";
                    if (navaidDescription[3].CompareTo('M') == 0) res = "MAPT";
                }
            }
            catch
            { res = ""; }

            return res;


        }
 
        private string _Altitude_Description;
        [System.ComponentModel.Browsable(false)]
        public string Altitude_Description
        {
            get { return _Altitude_Description; }
            set { _Altitude_Description = value; }
        }

        private Waypoint_Description_Code _Waypoint_Description_Code;
        [System.ComponentModel.Browsable(false)]
        public Waypoint_Description_Code  Waypoint_Description_Code
        {
            get { return _Waypoint_Description_Code; }
            set { _Waypoint_Description_Code = value; }
        }
    }


    public class Waypoint_Description_Code
    {

        public Waypoint_Description_Code()
        {
            this.Fix_type = Fix_type.None;
            this.Route_type = wyp_type.None;
            this.FixFunction = FixFunction.None;
            this._Fix_Role = Fix_Role.None;
        }

        public Waypoint_Description_Code(string _Description_Code)
        {
            this.Fix_type = setFixType(_Description_Code[0].ToString());
            this.Route_type = setRouteType(_Description_Code[1].ToString());
            this.FixFunction = setFixFunction(_Description_Code[2].ToString());
            this.Fix_Role = setFixRole(_Description_Code[3].ToString());
        }


        private Fix_type _Fix_type;
        public Fix_type Fix_type
        {
            get { return _Fix_type; }
            set { _Fix_type = value; }
        }

        private wyp_type _Route_type;
        public wyp_type Route_type
        {
            get { return _Route_type; }
            set { _Route_type = value; }
        }

        private FixFunction _FixFunction;
        public FixFunction FixFunction
        {
            get { return _FixFunction; }
            set { _FixFunction = value; }
        }

        private Fix_Role _Fix_Role;
        public Fix_Role Fix_Role
        {
            get { return _Fix_Role; }
            set { _Fix_Role = value; }
        }

        private Fix_type setFixType(string Col_40)
        {
            Fix_type res = Fix_type.None;
            switch (Col_40)
            {
                case ("A"):
                    res = Fix_type.Airport_as_Waypoint;
                    break;
                case ("E"):
                    res = Fix_type.Essential_Waypoint;
                    break;
                case ("F"):
                    res = Fix_type.Off_Airway_Waypoint;
                    break;
                case ("G"):
                    res = Fix_type.Runway_as_WaypointHelipad_as_Waypoint;
                    break;
                case ("H"):
                    res = Fix_type.Heliport_as_Waypoint;
                    break;
                case ("N"):
                    res = Fix_type.NDB_Navaid_as_Waypoint;
                    break;
                case ("P"):
                    res = Fix_type.Phantom_Waypoint;
                    break;
                case ("R"):
                    res = Fix_type.Non_Essential_Waypoint;
                    break;
                case ("T"):
                    res = Fix_type.Transition_Essential_Waypoint;
                    break;
                case ("V"):
                    res = Fix_type.VHF_Navaid_As_Waypoint;
                    break;
                default:
                    res = Fix_type.None;
                    break;
            }

            return res;
        }
        private wyp_type setRouteType(string Col_41)
        {
            wyp_type res = wyp_type.None;
            switch (Col_41)
            {
                case ("B"):
                    res = wyp_type.Flyover_Waypoint_End_of_STAR_Route_Type_Transition_or_Final_Approach;
                    break;
                case ("E"):
                    res = wyp_type.End_of_Airway_or_Terminal_Procedure_Route_Type;
                    break;
                case ("U"):
                    res = wyp_type.Uncharted_Airway_Intersection;
                    break;
                case ("Y"):
                    res = wyp_type.Fly_Over_Waypoint_;
                    break;
                default:
                    res = wyp_type.None;
                    break;
            }

            return res;
        }
        private FixFunction setFixFunction(string Col_42)
        {
            FixFunction res = FixFunction.None;
            switch (Col_42)
            {
                case ("A"):
                    res = FixFunction.Unnamed_Stepdown_Fix_After_Final_Approach_Fix;
                    break;
                case ("B"):
                    res = FixFunction.Unnamed_Stepdown_Fix_Before_Final_Approach_Fix;
                    break;
                case ("C"):
                    res = FixFunction.ATC_Compulsory_Waypoint;
                    break;
                case ("G"):
                    res = FixFunction.Oceanic_Gateway_Waypoint;
                    break;
                case ("M"):
                    res = FixFunction.First_Leg_of_Missed_Approach_Procedure;
                    break;
                case ("P"):
                    res = FixFunction.Path_Point_Fix;
                    break;
                case ("S"):
                    res = FixFunction.Named_Stepdown_Fix;
                    break;
                default:
                    res = FixFunction.None;
                    break;
            }

            return res;
        }
        private Fix_Role setFixRole(string Col_43)
        {
            Fix_Role res = Fix_Role.None;
            switch (Col_43)
            {
                case ("A"):
                    res = Fix_Role.Initial_Approach_Fix;
                    break;
                case ("B"):
                    res = Fix_Role.Intermediate_Approach_Fix;
                    break;
                case ("C"):
                    res = Fix_Role.Initial_Approach_Fix_with_Holding;
                    break;
                case ("D"):
                    res = Fix_Role.Initial_Approach_Fix_with_Final_Approach_Course_Fix;
                    break;
                case ("E"):
                    res = Fix_Role.Final_End_Point_Fix;
                    break;
                case ("F"):
                    res = Fix_Role.Published_Final_Approach_Fix_or_Database_Final_Approach_Fix;
                    break;
                case ("H"):
                    res = Fix_Role.Holding_Fix;
                    break;
                case ("I"):
                    res = Fix_Role.Final_Approach_Course_Fix;
                    break;
                case ("M"):
                    res = Fix_Role.Published_Missed_Approach_Point_Fix;
                    break;
                default:
                    res = Fix_Role.None;
                    break;
            }

            return res;
        }

    }
}
