using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace AIXM45Loader
{
    public class AIXM45_Procedure_Leg : AIXM45_Object
    {
        private string _R_RecomendedUse;

        public string R_RecomendedUse
        {
            get { return _R_RecomendedUse; }
            set { _R_RecomendedUse = value; }
        }
        private string _R_RecomendedUse_type;

        public string R_RecomendedUse_type
        {
            get { return _R_RecomendedUse_type; }
            set { _R_RecomendedUse_type = value; }
        }
        private string _R_RecomendedUse_mid;

        public string R_RecomendedUse_mid
        {
            get { return _R_RecomendedUse_mid; }
            set { _R_RecomendedUse_mid = value; }
        }
        private string _R_RecomendedUse_codeId;

        public string R_RecomendedUse_codeId
        {
            get { return _R_RecomendedUse_codeId; }
            set { _R_RecomendedUse_codeId = value; }
        }
        private string _R_RecomendedUse_geoLat;

        public string R_RecomendedUse_geoLat
        {
            get { return _R_RecomendedUse_geoLat; }
            set { _R_RecomendedUse_geoLat = value; }
        }
        private string _R_RecomendedUse_geoLong;

        public string R_RecomendedUse_geoLong
        {
            get { return _R_RecomendedUse_geoLong; }
            set { _R_RecomendedUse_geoLong = value; }
        }
        private string _R_SignificantPointGroupFix;

        public string R_SignificantPointGroupFix
        {
            get { return _R_SignificantPointGroupFix; }
            set { _R_SignificantPointGroupFix = value; }
        }
        private string _R_SignificantPointGroupFix_type;

        public string R_SignificantPointGroupFix_type
        {
            get { return _R_SignificantPointGroupFix_type; }
            set { _R_SignificantPointGroupFix_type = value; }
        }
        private string _R_SignificantPointGroupFix_mid;

        public string R_SignificantPointGroupFix_mid
        {
            get { return _R_SignificantPointGroupFix_mid; }
            set { _R_SignificantPointGroupFix_mid = value; }
        }
        private string _R_SignificantPointGroupFix_codeId;

        public string R_SignificantPointGroupFix_codeId
        {
            get { return _R_SignificantPointGroupFix_codeId; }
            set { _R_SignificantPointGroupFix_codeId = value; }
        }
        private string _R_SignificantPointGroupFix_geoLat;

        public string R_SignificantPointGroupFix_geoLat
        {
            get { return _R_SignificantPointGroupFix_geoLat; }
            set { _R_SignificantPointGroupFix_geoLat = value; }
        }
        private string _R_SignificantPointGroupFix_geoLong;

        public string R_SignificantPointGroupFix_geoLong
        {
            get { return _R_SignificantPointGroupFix_geoLong; }
            set { _R_SignificantPointGroupFix_geoLong = value; }
        }
        private string _R_SignificantPointGroupCentre;

        public string R_SignificantPointGroupCentre
        {
            get { return _R_SignificantPointGroupCentre; }
            set { _R_SignificantPointGroupCentre = value; }
        }
        private string _R_SignificantPointGroupCentre_type;

        public string R_SignificantPointGroupCentre_type
        {
            get { return _R_SignificantPointGroupCentre_type; }
            set { _R_SignificantPointGroupCentre_type = value; }
        }
        private string _R_SignificantPointGroupCentre_mid;

        public string R_SignificantPointGroupCentre_mid
        {
            get { return _R_SignificantPointGroupCentre_mid; }
            set { _R_SignificantPointGroupCentre_mid = value; }
        }
        private string _R_SignificantPointGroupCentre_codeId;

        public string R_SignificantPointGroupCentre_codeId
        {
            get { return _R_SignificantPointGroupCentre_codeId; }
            set { _R_SignificantPointGroupCentre_codeId = value; }
        }
        private string _R_SignificantPointGroupCentre_geoLat;

        public string R_SignificantPointGroupCentre_geoLat
        {
            get { return _R_SignificantPointGroupCentre_geoLat; }
            set { _R_SignificantPointGroupCentre_geoLat = value; }
        }
        private string _R_SignificantPointGroupCentre_geoLong;

        public string R_SignificantPointGroupCentre_geoLong
        {
            get { return _R_SignificantPointGroupCentre_geoLong; }
            set { _R_SignificantPointGroupCentre_geoLong = value; }
        }
        private string _codePhase;

        public string codePhase
        {
            get { return _codePhase; }
            set { _codePhase = value; }
        }
        private string _codeType;

        public string codeType
        {
            get { return _codeType; }
            set { _codeType = value; }
        }
        private double _valCourse;

        public double valCourse
        {
            get { return _valCourse; }
            set { _valCourse = value; }
        }
        private string _codeTypeCourse;

        public string codeTypeCourse
        {
            get { return _codeTypeCourse; }
            set { _codeTypeCourse = value; }
        }
        private string _codeDirTurn;

        public string codeDirTurn
        {
            get { return _codeDirTurn; }
            set { _codeDirTurn = value; }
        }
        private string _codeTurnValid;

        public string codeTurnValid
        {
            get { return _codeTurnValid; }
            set { _codeTurnValid = value; }
        }
        private string _codeDescrDistVer;

        public string codeDescrDistVer
        {
            get { return _codeDescrDistVer; }
            set { _codeDescrDistVer = value; }
        }
        private string _codeDistVerUpper;

        public string codeDistVerUpper
        {
            get { return _codeDistVerUpper; }
            set { _codeDistVerUpper = value; }
        }
        private double _valDistVerUpper;

        public double valDistVerUpper
        {
            get { return _valDistVerUpper; }
            set { _valDistVerUpper = value; }
        }
        private string _uomDistVerUpper;

        public string uomDistVerUpper
        {
            get { return _uomDistVerUpper; }
            set { _uomDistVerUpper = value; }
        }
        private string _codeDistVerLower;

        public string codeDistVerLower
        {
            get { return _codeDistVerLower; }
            set { _codeDistVerLower = value; }
        }
        private double _valDistVerLower;

        public double valDistVerLower
        {
            get { return _valDistVerLower; }
            set { _valDistVerLower = value; }
        }
        private string _uomDistVerLower;

        public string uomDistVerLower
        {
            get { return _uomDistVerLower; }
            set { _uomDistVerLower = value; }
        }
        private double _valVerAngle;

        public double valVerAngle
        {
            get { return _valVerAngle; }
            set { _valVerAngle = value; }
        }
        private double _valSpeedLimit;

        public double valSpeedLimit
        {
            get { return _valSpeedLimit; }
            set { _valSpeedLimit = value; }
        }
        private string _uomSpeed;

        public string uomSpeed
        {
            get { return _uomSpeed; }
            set { _uomSpeed = value; }
        }
        private string _codeSpeedRef;

        public string codeSpeedRef
        {
            get { return _codeSpeedRef; }
            set { _codeSpeedRef = value; }
        }
        private double _valDist;

        public double valDist
        {
            get { return _valDist; }
            set { _valDist = value; }
        }
        private double _valDur;

        public double valDur
        {
            get { return _valDur; }
            set { _valDur = value; }
        }
        private string _uomDur;

        public string uomDur
        {
            get { return _uomDur; }
            set { _uomDur = value; }
        }
        private double _valTheta;

        public double valTheta
        {
            get { return _valTheta; }
            set { _valTheta = value; }
        }
        private double _valRho;

        public double valRho
        {
            get { return _valRho; }
            set { _valRho = value; }
        }
        private double _valBankAngle;

        public double valBankAngle
        {
            get { return _valBankAngle; }
            set { _valBankAngle = value; }
        }
        private string _uomDistHorz;

        public string uomDistHorz
        {
            get { return _uomDistHorz; }
            set { _uomDistHorz = value; }
        }
        private string _codeRepAtc;

        public string codeRepAtc
        {
            get { return _codeRepAtc; }
            set { _codeRepAtc = value; }
        }
        private string _codeRoleFix;

        public string codeRoleFix
        {
            get { return _codeRoleFix; }
            set { _codeRoleFix = value; }
        }
        private string _txtRmk;

        public string txtRmk
        {
            get { return _txtRmk; }
            set { _txtRmk = value; }
        }
        private string _R_MasterProcMid;

        public string R_MasterProcMid
        {
            get { return _R_MasterProcMid; }
            set { _R_MasterProcMid = value; }
        }
        private string _R_MasterProcType;

        public string R_MasterProcType
        {
            get { return _R_MasterProcType; }
            set { _R_MasterProcType = value; }
        }
        private int _R_NOSEQ;

        public int R_NOSEQ
        {
            get { return _R_NOSEQ; }
            set { _R_NOSEQ = value; }
        }

        private string procedureIdentifier;

        public string ProcedureIdentifier
        {
            get { return procedureIdentifier; }
            set { procedureIdentifier = value; }
        }

        private string airportIdentifier;

        public string AirportIdentifier
        {
            get { return airportIdentifier; }
            set { airportIdentifier = value; }
        }

        private string transition_Identifier;

        public string Transition_Identifier
        {
            get { return transition_Identifier; }
            set { transition_Identifier = value; }
        }

        private string route_Type;

        public string Route_Type
        {
            get { return route_Type; }
            set { route_Type = value; }
        }

        private bool flag;

        public bool Flag
        {
            get { return flag; }
            set { flag = value; }
        }


        private string _fk_transition_mid;
        public string fk_transition_mid
        {
            get { return _fk_transition_mid; }
            set { _fk_transition_mid = value; }
        }

        public AIXM45_Procedure_Leg(ARINC_OBJECT ARINC_Record)
        {
            ARINC_Terminal_Procedure_Primary_Record _Record = (ARINC_Terminal_Procedure_Primary_Record)ARINC_Record;

            this.procedureIdentifier = _Record.SID_STAR_Approach_Identifier;
            this.AirportIdentifier = _Record.Airport_Identifier;
            this.transition_Identifier = _Record.Transition_Identifier;
            this.Route_Type = _Record.Route_Type;
            this.flag = false;

            this.fk_transition_mid = _Record.ID;

            ////////////////////////////////////////////////////////
            this._R_RecomendedUse_mid = _Record.Recommended_Navaid.Trim();
            this._R_SignificantPointGroupFix_mid = _Record.Fix_Identifier.Trim();
            this._R_SignificantPointGroupCentre_mid = _Record.Center_Fix.Trim();
            ////////////////////////////////////////////////////////

            if ((GetProcType(_Record.Name).CompareTo("IAP") == 0))
            {
                this.transition_Identifier = "RW" + this.procedureIdentifier.Substring(1, this.procedureIdentifier.Length - 1);
                this.procedureIdentifier = _Record.SID_STAR_Approach_Identifier.Trim() + _Record.Transition_Identifier.Trim();
               
                if ((!transition_Identifier.EndsWith("L")) || (!transition_Identifier.EndsWith("R")) || (!transition_Identifier.EndsWith("G")) || (!transition_Identifier.EndsWith("D")))
                {
                    //this.transition_Identifier = this.transition_Identifier.Substring(0, this.transition_Identifier.Length - 1)+" ";
                    if (this.transition_Identifier.Length > 5) this.transition_Identifier = this.transition_Identifier.Substring(0, 4);
                    this.transition_Identifier = this.transition_Identifier + " ";
                    
                }
            }

            

            this.codePhase = _Record.Route_Type;
            this.codeType = _Record.Path_and_Termination;

            double dbl;
            if (_Record.Magnetic_Course.Trim().Length > 0)
            {
                Double.TryParse(_Record.Magnetic_Course, out dbl);
                this.valCourse = dbl / 10;
            }
            else
                this.valCourse = Double.NaN;

            this.codeTypeCourse = "MT";
            this.codeDirTurn = _Record.Turn_Direction;
            this.codeTurnValid = GetFlyBy_FlyOverFlag(_Record.Waypoint_Description_Code);//_Record.Turn_Direction_Valid;

            this.codeDescrDistVer = ConvertAltitude_DescriptionToAIXM45(_Record.Altitude_Description);

            this.uomDistVerUpper = "FT";
            if (_Record.Altitude2.StartsWith("FL"))
            {
                _Record.Altitude2 = _Record.Altitude2.Substring(2);
                this.uomDistVerUpper = "FL";
            }

            if (_Record.Altitude2.Trim().Length > 0)
            {
                Double.TryParse(_Record.Altitude2, out dbl);
                this.valDistVerUpper = dbl;
            }
            else
                this.valDistVerUpper = Double.NaN;
            //this.codeDistVerUpper = _Record.;

            this.uomDistVerLower = "FT";
            if (_Record.Altitude1.StartsWith("FL"))
            {
                _Record.Altitude1 = _Record.Altitude1.Substring(2);
                this.uomDistVerLower = "FL";
            }

            if (_Record.Altitude1.Trim().Length > 0)
            {
                Double.TryParse(_Record.Altitude1, out dbl);
                this.valDistVerLower = dbl;
            }
            else
                this.valDistVerLower = Double.NaN;
            //this.codeDistVerLower = _Record.;

            if (_Record.Vertical_Angle.Trim().Length > 0)
            {
                Double.TryParse(_Record.Vertical_Angle, out dbl);
                this.valVerAngle = dbl / 100;
            }
            else
                this.valVerAngle = Double.NaN;


            if (_Record.Speed_Limit.Trim().Length > 0)
            {
                Double.TryParse(_Record.Speed_Limit, out dbl);
                this.valSpeedLimit = dbl;
            }
            else
                this.valSpeedLimit = Double.NaN;

            this.uomSpeed = "KT";
            this.codeSpeedRef = "IAS";


            if (_Record.Route_Distance_Holding_Distance_or_Time.Trim().Length > 0)
            {
                if (_Record.Route_Distance_Holding_Distance_or_Time.StartsWith("T"))
                {
                    _Record.Route_Distance_Holding_Distance_or_Time = _Record.Route_Distance_Holding_Distance_or_Time.Substring(1);
                    Double.TryParse(_Record.Route_Distance_Holding_Distance_or_Time, out dbl);
                    this.valDur = dbl / 10;
                    this.uomDur = "M";
                }
                else
                {
                    Double.TryParse(_Record.Route_Distance_Holding_Distance_or_Time, out dbl);
                    this.valDist = dbl / 10;
                }
            }
            else
            {
                this.valDur = Double.NaN;
                this.valDist = Double.NaN;
            }

            if (_Record.Theta.Trim().Length > 0)
            {
                Double.TryParse(_Record.Theta, out dbl);
                this.valTheta = dbl / 10;
            }
            else
                this.valTheta = Double.NaN;

            if (_Record.Rho.Trim().Length > 0)
            {
                Double.TryParse(_Record.Rho, out dbl);
                this.valRho = dbl / 10;
            }
            else
                this.valRho = Double.NaN;


            this.valBankAngle = 25;
            this.uomDistHorz = "NM";
            this.codeRepAtc = "";
            this.codeRoleFix = GetFixRole(_Record.Waypoint_Description_Code);
            this.txtRmk = "";
            //this.R_MasterProcMid = определить позднее!!!;

            this.R_MasterProcType = GetProcType( _Record.Name);
            int n;
            Int32.TryParse(_Record.Sequence_Number, out n);
            this.R_NOSEQ = n;

           
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
                    if (navaidDescription[3].CompareTo('D') == 0) res = "IAF";
                    if (navaidDescription[3].CompareTo('E') == 0) res = "OTHER";
                    if (navaidDescription[3].CompareTo('F') == 0) res = "FAF";
                    if (navaidDescription[3].CompareTo('H') == 0) res = "OTHER";
                    if (navaidDescription[3].CompareTo('I') == 0) res = "FAF";
                    if (navaidDescription[3].CompareTo('M') == 0) res = "MAPT";
                }
            }
            catch
            { res = ""; }

            return res;
        }

        private string GetProcType(string name)
        {
            string res = "";
            switch (name)
            {
                case("SIDProcedures"):
                    res = "SID";
                    break;

                case("STARProcedures"):
                    res = "STAR";
                    break;

                case ("ApproachProcedures"):
                    res = "IAP";
                    break;
            }

            return res;
        }

        private string ConvertAltitude_DescriptionToAIXM45(string Altitude_Description)
        {
            string result = "OTHER";

            switch (Altitude_Description)
            {
                case("+"):
                    result = "LA";
                    break;

                case ("C"):
                case ("G"):
                case ("I"):
                    result = "BH";
                    break;

                case ("@"):
                case (" "):
                case ("-"):
                    result = "L";
                    break;

                case("B"):
                    result = "B";
                    break;

                default: result = "OTHER";
                    break;
            }
            return result;
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


    }
}
