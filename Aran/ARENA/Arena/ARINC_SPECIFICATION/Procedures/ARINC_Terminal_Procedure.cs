using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ARINC_Types
{
    [XmlType]
    [Serializable()]
    public class ARINC_Terminal_Procedure_Primary_Record : ARINC_OBJECT
    {

        private string _Blank_spacing;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing
        {
            get { return _Blank_spacing; }
            set { _Blank_spacing = value; }
        }

        private string _Airport_Identifier;
        [XmlElement]
        public string Airport_Identifier
        {
            get { return _Airport_Identifier; }
            set { _Airport_Identifier = value; }
        }

        private string _ICAO_Code;
        [XmlElement]
        public string ICAO_Code
        {
            get { return _ICAO_Code; }
            set { _ICAO_Code = value; }
        }

        private string _Subsection_Code;
        [XmlElement]
        public string Subsection_Code
        {
            get { return _Subsection_Code; }
            set { _Subsection_Code = value; }
        }

        private string _SID_STAR_Approach_Identifier;
        [XmlElement]
        public string SID_STAR_Approach_Identifier
        {
            get { return _SID_STAR_Approach_Identifier; }
            set { _SID_STAR_Approach_Identifier = value; }
        }

        private string _Route_Type;
        [XmlElement]
        public string Route_Type
        {
            get { return _Route_Type; }
            set { _Route_Type = value; }
        }

        private string _Transition_Identifier;
        [XmlElement]
        public string Transition_Identifier
        {
            get { return _Transition_Identifier; }
            set { _Transition_Identifier = value; }
        }

        private string _Blank_spacing1;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing1
        {
            get { return _Blank_spacing1; }
            set { _Blank_spacing1 = value; }
        }

        private string _Sequence_Number;
        [XmlElement]
        public string Sequence_Number
        {
            get { return _Sequence_Number; }
            set { _Sequence_Number = value; }
        }

        private string _Fix_Identifier;
        [XmlElement]
        public string Fix_Identifier
        {
            get { return _Fix_Identifier; }
            set { _Fix_Identifier = value; }
        }

        private string _ICAO_Code1;
        [XmlElement]
        public string ICAO_Code1
        {
            get { return _ICAO_Code1; }
            set { _ICAO_Code1 = value; }
        }

        private string _Section_Code1;
        [XmlElement]
        public string Section_Code1
        {
            get { return _Section_Code1; }
            set { _Section_Code1 = value; }
        }

        private string _Subsection_Code1;
        [XmlElement]
        public string Subsection_Code1
        {
            get { return _Subsection_Code1; }
            set { _Subsection_Code1 = value; }
        }

        private string _Continuation_Number;
        [XmlElement]
        public string Continuation_Number
        {
            get { return _Continuation_Number; }
            set { _Continuation_Number = value; }
        }

        private string _Waypoint_Description_Code;
        [XmlElement]
        public string Waypoint_Description_Code
        {
            get { return _Waypoint_Description_Code; }
            set { _Waypoint_Description_Code = value; }
        }

        private string _Turn_Direction;
        [XmlElement]
        public string Turn_Direction
        {
            get { return _Turn_Direction; }
            set { _Turn_Direction = value; }
        }

        private string _RNP;
        [XmlElement]
        public string RNP
        {
            get { return _RNP; }
            set { _RNP = value; }
        }

        private string _Path_and_Termination;
        [XmlElement]
        public string Path_and_Termination
        {
            get { return _Path_and_Termination; }
            set { _Path_and_Termination = value; }
        }

        private string _Turn_Direction_Valid;
        [XmlElement]
        public string Turn_Direction_Valid
        {
            get { return _Turn_Direction_Valid; }
            set { _Turn_Direction_Valid = value; }
        }

        private string _Recommended_Navaid;
        [XmlElement]
        public string Recommended_Navaid
        {
            get { return _Recommended_Navaid; }
            set { _Recommended_Navaid = value; }
        }

        private string _ICAO_Code2;
        [XmlElement]
        public string ICAO_Code2
        {
            get { return _ICAO_Code2; }
            set { _ICAO_Code2 = value; }
        }

        private string _ARC_Radius;
        [XmlElement]
        public string ARC_Radius
        {
            get { return _ARC_Radius; }
            set { _ARC_Radius = value; }
        }

        private string _Theta;
        [XmlElement]
        public string Theta
        {
            get { return _Theta; }
            set { _Theta = value; }
        }

        private string _Rho;
        [XmlElement]
        public string Rho
        {
            get { return _Rho; }
            set { _Rho = value; }
        }

        private string _Magnetic_Course;
        [XmlElement]
        public string Magnetic_Course
        {
            get { return _Magnetic_Course; }
            set { _Magnetic_Course = value; }
        }

        private string _Route_Distance_Holding_Distance_or_Time;
        [XmlElement]
        public string Route_Distance_Holding_Distance_or_Time
        {
            get { return _Route_Distance_Holding_Distance_or_Time; }
            set { _Route_Distance_Holding_Distance_or_Time = value; }
        }

        private string _RECD_NAV_Sect_on;
        [XmlElement]
        public string RECD_NAV_Sect_on
        {
            get { return _RECD_NAV_Sect_on; }
            set { _RECD_NAV_Sect_on = value; }
        }

        private string _RECD_NAV_Subsection;
        [XmlElement]
        public string RECD_NAV_Subsection
        {
            get { return _RECD_NAV_Subsection; }
            set { _RECD_NAV_Subsection = value; }
        }

        private string _Reserved_expansion;
        [XmlElement]
        public string Reserved_expansion
        {
            get { return _Reserved_expansion; }
            set { _Reserved_expansion = value; }
        }

        private string _Altitude_Description;
        [XmlElement]
        public string Altitude_Description
        {
            get { return _Altitude_Description; }
            set { _Altitude_Description = value; }
        }

        private string _ATC_Indicator;
        [XmlElement]
        public string ATC_Indicator
        {
            get { return _ATC_Indicator; }
            set { _ATC_Indicator = value; }
        }

        private string _Altitude1;
        [XmlElement]
        public string Altitude1
        {
            get { return _Altitude1; }
            set { _Altitude1 = value; }
        }

        private string _Altitude2;
        [XmlElement]
        public string Altitude2
        {
            get { return _Altitude2; }
            set { _Altitude2 = value; }
        }

        private string _Transition_Altitude;
        [XmlElement]
        public string Transition_Altitude
        {
            get { return _Transition_Altitude; }
            set { _Transition_Altitude = value; }
        }

        private string _Speed_Limit;
        [XmlElement]
        public string Speed_Limit
        {
            get { return _Speed_Limit; }
            set { _Speed_Limit = value; }
        }

        private string _Vertical_Angle;
        [XmlElement]
        public string Vertical_Angle
        {
            get { return _Vertical_Angle; }
            set { _Vertical_Angle = value; }
        }

        private string _Center_Fix;
        [XmlElement]
        public string Center_Fix
        {
            get { return _Center_Fix; }
            set { _Center_Fix = value; }
        }

        private string _Multiple_Code;
        [XmlElement]
        public string Multiple_Code
        {
            get { return _Multiple_Code; }
            set { _Multiple_Code = value; }
        }

        private string _ICAO_Code3;
        [XmlElement]
        public string ICAO_Code3
        {
            get { return _ICAO_Code3; }
            set { _ICAO_Code3 = value; }
        }

        private string _Section_Code;
        [XmlElement]
        public string Section_Code
        {
            get { return _Section_Code; }
            set { _Section_Code = value; }
        }

        private string _Subsection_Code2;
        [XmlElement]
        public string Subsection_Code2
        {
            get { return _Subsection_Code2; }
            set { _Subsection_Code2 = value; }
        }

        private string _GPS_FMS_Indication;
        [XmlElement]
        public string GPS_FMS_Indication
        {
            get { return _GPS_FMS_Indication; }
            set { _GPS_FMS_Indication = value; }
        }

        private string _Blank_spacing2;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing2
        {
            get { return _Blank_spacing2; }
            set { _Blank_spacing2 = value; }
        }

        private string _Apch_Route_Qualifier_1;
        [XmlElement]
        public string Apch_Route_Qualifier_1
        {
            get { return _Apch_Route_Qualifier_1; }
            set { _Apch_Route_Qualifier_1 = value; }
        }

        private string _Apch_Route_Qualifier_2;
        [XmlElement]
        public string Apch_Route_Qualifier_2
        {
            get { return _Apch_Route_Qualifier_2; }
            set { _Apch_Route_Qualifier_2 = value; }
        }

        private string _Blank_spacing3;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing3
        {
            get { return _Blank_spacing3; }
            set { _Blank_spacing3 = value; }
        }

        private string _File_Record_Number;
        [XmlElement]
        public string File_Record_Number
        {
            get { return _File_Record_Number; }
            set { _File_Record_Number = value; }
        }

        private string _Cycle_Date;
        [XmlElement]
        public string Cycle_Date
        {
            get { return _Cycle_Date; }
            set { _Cycle_Date = value; }
        }

        private List<ARINC_Terminal_Procedure_Continuation_Record> _ARINC_ARINC_Terminal_Procedure_Continuation_Record;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public List<ARINC_Terminal_Procedure_Continuation_Record> ARINC_ARINC_Terminal_Procedure_Continuation_Record
        {
            get { return _ARINC_ARINC_Terminal_Procedure_Continuation_Record; }
            set { _ARINC_ARINC_Terminal_Procedure_Continuation_Record = value; }
        }

        private ARINC_Terminal_Procedure_Flight_Planning_Continuation_Records _ARINC_ARINC_ARINC_Terminal_Procedure_Flight_Planning_Continuation_Records;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public ARINC_Terminal_Procedure_Flight_Planning_Continuation_Records ARINC_ARINC_ARINC_Terminal_Procedure_Flight_Planning_Continuation_Records
        {
            get { return _ARINC_ARINC_ARINC_Terminal_Procedure_Flight_Planning_Continuation_Records; }
            set { _ARINC_ARINC_ARINC_Terminal_Procedure_Flight_Planning_Continuation_Records = value; }
        }

        public ARINC_Terminal_Procedure_Primary_Record()
        {
            this._ARINC_ARINC_Terminal_Procedure_Continuation_Record = new List<ARINC_Terminal_Procedure_Continuation_Record>();
            this._ARINC_ARINC_ARINC_Terminal_Procedure_Flight_Planning_Continuation_Records = new ARINC_Terminal_Procedure_Flight_Planning_Continuation_Records();
        }
    }

    [XmlType]
    [Serializable()]
    public class ARINC_Terminal_Procedure_Continuation_Record : ARINC_OBJECT
    {

        private string _Blank_spacing;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing
        {
            get { return _Blank_spacing; }
            set { _Blank_spacing = value; }
        }

        private string _Airport_Identifier;
        [XmlElement]
        public string Airport_Identifier
        {
            get { return _Airport_Identifier; }
            set { _Airport_Identifier = value; }
        }

        private string _ICAO_Code;
        [XmlElement]
        public string ICAO_Code
        {
            get { return _ICAO_Code; }
            set { _ICAO_Code = value; }
        }

        private string _Subsection_Code;
        [XmlElement]
        public string Subsection_Code
        {
            get { return _Subsection_Code; }
            set { _Subsection_Code = value; }
        }

        private string _SID_STAR_Approach_Identifier_6;
        [XmlElement]
        public string SID_STAR_Approach_Identifier_6
        {
            get { return _SID_STAR_Approach_Identifier_6; }
            set { _SID_STAR_Approach_Identifier_6 = value; }
        }

        private string _Route_Type;
        [XmlElement]
        public string Route_Type
        {
            get { return _Route_Type; }
            set { _Route_Type = value; }
        }

        private string _Transition_Identifier;
        [XmlElement]
        public string Transition_Identifier
        {
            get { return _Transition_Identifier; }
            set { _Transition_Identifier = value; }
        }

        private string _Blank_spacing2;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing2
        {
            get { return _Blank_spacing2; }
            set { _Blank_spacing2= value; }
        }

        private string _Sequence_Number;
        [XmlElement]
        public string Sequence_Number
        {
            get { return _Sequence_Number; }
            set { _Sequence_Number = value; }
        }

        private string _Fix_Identifier;
        [XmlElement]
        public string Fix_Identifier
        {
            get { return _Fix_Identifier; }
            set { _Fix_Identifier = value; }
        }

        private string _ICAO_Code2;
        [XmlElement]
        public string ICAO_Code2
        {
            get { return _ICAO_Code2; }
            set { _ICAO_Code2 = value; }
        }

        private string _Section_Code;
        [XmlElement]
        public string Section_Code
        {
            get { return _Section_Code; }
            set { _Section_Code = value; }
        }

        private string _Subsection_Code2;
        [XmlElement]
        public string Subsection_Code2
        {
            get { return _Subsection_Code2; }
            set { _Subsection_Code2 = value; }
        }

        private string _Continuation_Record_Number;
        [XmlElement]
        public string Continuation_Record_Number
        {
            get { return _Continuation_Record_Number; }
            set { _Continuation_Record_Number = value; }
        }

        private string _Reserved_spacing;
        [XmlElement]
        public string Reserved_spacing
        {
            get { return _Reserved_spacing; }
            set { _Reserved_spacing = value; }
        }

        private string _CAT_A_Decision_Height;
        [XmlElement]
        public string CAT_A_Decision_Height
        {
            get { return _CAT_A_Decision_Height; }
            set { _CAT_A_Decision_Height = value; }
        }

        private string _CAT_B_Decision_Height;
        [XmlElement]
        public string CAT_B_Decision_Height
        {
            get { return _CAT_B_Decision_Height; }
            set { _CAT_B_Decision_Height = value; }
        }

        private string _CAT_C_Decision_Height;
        [XmlElement]
        public string CAT_C_Decision_Height
        {
            get { return _CAT_C_Decision_Height; }
            set { _CAT_C_Decision_Height = value; }
        }

        private string _CAT_D_Decision_Height;
        [XmlElement]
        public string CAT_D_Decision_Height
        {
            get { return _CAT_D_Decision_Height; }
            set { _CAT_D_Decision_Height = value; }
        }

        private string _CAT_A_Minimum_Descent_Altitude;
        [XmlElement]
        public string CAT_A_Minimum_Descent_Altitude
        {
            get { return _CAT_A_Minimum_Descent_Altitude; }
            set { _CAT_A_Minimum_Descent_Altitude = value; }
        }

        private string _CAT_B_Minimum_Descent_Altitude;
        [XmlElement]
        public string CAT_B_Minimum_Descent_Altitude
        {
            get { return _CAT_B_Minimum_Descent_Altitude; }
            set { _CAT_B_Minimum_Descent_Altitude = value; }
        }

        private string _CAT_C_Minimum_Descent_Altitude;
        [XmlElement]
        public string CAT_C_Minimum_Descent_Altitude
        {
            get { return _CAT_C_Minimum_Descent_Altitude; }
            set { _CAT_C_Minimum_Descent_Altitude = value; }
        }

        private string _CAT_D_Minimum_Descent_Altitude;
        [XmlElement]
        public string CAT_D_Minimum_Descent_Altitude
        {
            get { return _CAT_D_Minimum_Descent_Altitude; }
            set { _CAT_D_Minimum_Descent_Altitude = value; }
        }

        private string _Blank_Spacing;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_Spacing
        {
            get { return _Blank_Spacing; }
            set { _Blank_Spacing = value; }
        }

        private string _Procedure_Category;
        [XmlElement]
        public string Procedure_Category
        {
            get { return _Procedure_Category; }
            set { _Procedure_Category = value; }
        }

        private string _RNP;
        [XmlElement]
        public string RNP
        {
            get { return _RNP; }
            set { _RNP = value; }
        }

        private string _Reserved_expansion;
        [XmlElement]
        public string Reserved_expansion
        {
            get { return _Reserved_expansion; }
            set { _Reserved_expansion = value; }
        }

        private string _Apch_Route_Qualifier_1;
        [XmlElement]
        public string Apch_Route_Qualifier_1
        {
            get { return _Apch_Route_Qualifier_1; }
            set { _Apch_Route_Qualifier_1 = value; }
        }

        private string _Apch_Route_Qualifier_2;
        [XmlElement]
        public string Apch_Route_Qualifier_2
        {
            get { return _Apch_Route_Qualifier_2; }
            set { _Apch_Route_Qualifier_2 = value; }
        }

        private string _Blank_spacing3;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing3
        {
            get { return _Blank_spacing3; }
            set { _Blank_spacing3 = value; }
        }

        private string _File_Record_Number;
        [XmlElement]
        public string File_Record_Number
        {
            get { return _File_Record_Number; }
            set { _File_Record_Number = value; }
        }

        private string _Cycle_Date;
        [XmlElement]
        public string Cycle_Date
        {
            get { return _Cycle_Date; }
            set { _Cycle_Date = value; }
        }

        public ARINC_Terminal_Procedure_Continuation_Record()
        {

        }
    }

    [XmlType]
    [Serializable()]
    public class ARINC_Terminal_Procedure_Flight_Planning_Continuation_Records : ARINC_OBJECT
    {

        private string _Blank_spacing;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing
        {
            get { return _Blank_spacing; }
            set { _Blank_spacing = value; }
        }

        private string _Airport_Identifier;
        [XmlElement]
        public string Airport_Identifier
        {
            get { return _Airport_Identifier; }
            set { _Airport_Identifier = value; }
        }

        private string _ICAO_Code;
        [XmlElement]
        public string ICAO_Code
        {
            get { return _ICAO_Code; }
            set { _ICAO_Code = value; }
        }

        private string _Subsection_Code;
        [XmlElement]
        public string Subsection_Code
        {
            get { return _Subsection_Code; }
            set { _Subsection_Code = value; }
        }

        private string _SID_STAR_Approach_Identifier_6;
        [XmlElement]
        public string SID_STAR_Approach_Identifier_6
        {
            get { return _SID_STAR_Approach_Identifier_6; }
            set { _SID_STAR_Approach_Identifier_6 = value; }
        }

        private string _Route_Type;
        [XmlElement]
        public string Route_Type
        {
            get { return _Route_Type; }
            set { _Route_Type = value; }
        }

        private string _Transition_Identifier;
        [XmlElement]
        public string Transition_Identifier
        {
            get { return _Transition_Identifier; }
            set { _Transition_Identifier = value; }
        }

        private string _Blank_spacing2;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing2
        {
            get { return _Blank_spacing2; }
            set { _Blank_spacing2 = value; }
        }

        private string _Sequence_Number;
        [XmlElement]
        public string Sequence_Number
        {
            get { return _Sequence_Number; }
            set { _Sequence_Number = value; }
        }

        private string _Fix_Identifier;
        [XmlElement]
        public string Fix_Identifier
        {
            get { return _Fix_Identifier; }
            set { _Fix_Identifier = value; }
        }

        private string _ICAO_Code2;
        [XmlElement]
        public string ICAO_Code2
        {
            get { return _ICAO_Code2; }
            set { _ICAO_Code2 = value; }
        }

        private string _Section_Code;
        [XmlElement]
        public string Section_Code
        {
            get { return _Section_Code; }
            set { _Section_Code = value; }
        }

        private string _Subsection_Code2;
        [XmlElement]
        public string Subsection_Code2
        {
            get { return _Subsection_Code2; }
            set { _Subsection_Code2 = value; }
        }

        private string _Continuation_Record_No;
        [XmlElement]
        public string Continuation_Record_No
        {
            get { return _Continuation_Record_No; }
            set { _Continuation_Record_No = value; }
        }

        private string _Application_Type;
        [XmlElement]
        public string Application_Type
        {
            get { return _Application_Type; }
            set { _Application_Type = value; }
        }

        private string _Start_End_Indicator;
        [XmlElement]
        public string Start_End_Indicator
        {
            get { return _Start_End_Indicator; }
            set { _Start_End_Indicator = value; }
        }

        private string _Start_End_Date;
        [XmlElement]
        public string Start_End_Date
        {
            get { return _Start_End_Date; }
            set { _Start_End_Date = value; }
        }

        private string _Blank_Spacing;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_Spacing
        {
            get { return _Blank_Spacing; }
            set { _Blank_Spacing = value; }
        }

        private string _Route_Distance;
        [XmlElement]
        public string Route_Distance
        {
            get { return _Route_Distance; }
            set { _Route_Distance = value; }
        }

        private string _Reserved_Expansion;
        [XmlElement]
        public string Reserved_Expansion
        {
            get { return _Reserved_Expansion; }
            set { _Reserved_Expansion = value; }
        }

        private string _File_Record_No;
        [XmlElement]
        public string File_Record_No
        {
            get { return _File_Record_No; }
            set { _File_Record_No = value; }
        }

        private string _Cycle_Date;
        [XmlElement]
        public string Cycle_Date
        {
            get { return _Cycle_Date; }
            set { _Cycle_Date = value; }
        }

        public ARINC_Terminal_Procedure_Flight_Planning_Continuation_Records()
        {

        }
    }

}
