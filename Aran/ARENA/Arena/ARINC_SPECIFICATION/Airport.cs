using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ARINC_Types
{
    [XmlType]
    [Serializable()]
    [System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class ARINC_Airport_Primary_Record : ARINC_OBJECT
    {
        private string _Blank_spacing;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing
        {
            get { return _Blank_spacing; }
            set { _Blank_spacing = value; }
        }

        private string _Airport_ICAO_Identifier;
        [XmlElement]
        public string Airport_ICAO_Identifier
        {
            get { return _Airport_ICAO_Identifier; }
            set { _Airport_ICAO_Identifier = value; }
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

        private string _ATA_IATA_Designator;
        [XmlElement]
        public string ATA_IATA_Designator
        {
            get { return _ATA_IATA_Designator; }
            set { _ATA_IATA_Designator = value; }
        }

        private string _Reserved_Expansion2;
        [XmlElement]
        public string Reserved_Expansion2
        {
            get { return _Reserved_Expansion2; }
            set { _Reserved_Expansion2 = value; }
        }

        private string _Blank_spacing1;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing1
        {
            get { return _Blank_spacing1; }
            set { _Blank_spacing1 = value; }
        }

        private string _Continuation_Record_Number;
        [XmlElement]
        public string Continuation_Record_Number
        {
            get { return _Continuation_Record_Number; }
            set { _Continuation_Record_Number = value; }
        }

        private string _Speed_Limit_Altitude;
        [XmlElement]
        public string Speed_Limit_Altitude
        {
            get { return _Speed_Limit_Altitude; }
            set { _Speed_Limit_Altitude = value; }
        }

        private string _Longest_Runway;
        [XmlElement]
        public string Longest_Runway
        {
            get { return _Longest_Runway; }
            set { _Longest_Runway = value; }
        }

        private string _IFR_Capability;
        [XmlElement]
        public string IFR_Capability
        {
            get { return _IFR_Capability; }
            set { _IFR_Capability = value; }
        }

        private string _Longest_Runway_Surface_Code;
        [XmlElement]
        public string Longest_Runway_Surface_Code
        {
            get { return _Longest_Runway_Surface_Code; }
            set { _Longest_Runway_Surface_Code = value; }
        }

        private string _Airport_Reference_Pt_Latitude;
        [XmlElement]
        public string Airport_Reference_Pt_Latitude
        {
            get { return _Airport_Reference_Pt_Latitude; }
            set { _Airport_Reference_Pt_Latitude = value; }
        }

        private string _Airport_Reference_Pt_Longitude;
        [XmlElement]
        public string Airport_Reference_Pt_Longitude
        {
            get { return _Airport_Reference_Pt_Longitude; }
            set { _Airport_Reference_Pt_Longitude = value; }
        }

        private string _Magnetic_Variation;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Magnetic_Variation
        {
            get { return _Magnetic_Variation; }
            set { _Magnetic_Variation = value; }
        }

        private string _Airport_Elevation;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Airport_Elevation
        {
            get { return _Airport_Elevation; }
            set { _Airport_Elevation = value; }
        }

        private string _Speed_Limit;
        [XmlElement]
        public string Speed_Limit
        {
            get { return _Speed_Limit; }
            set { _Speed_Limit = value; }
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

        private string _Transitions_Altitude;
        [XmlElement]
        public string Transitions_Altitude
        {
            get { return _Transitions_Altitude; }
            set { _Transitions_Altitude = value; }
        }

        private string _Transition_Level;
        [XmlElement]
        public string Transition_Level
        {
            get { return _Transition_Level; }
            set { _Transition_Level = value; }
        }

        private string _Public_Military_Indicator;
        [XmlElement]
        public string Public_Military_Indicator
        {
            get { return _Public_Military_Indicator; }
            set { _Public_Military_Indicator = value; }
        }

        private string _Time_Zone;
        [XmlElement]
        public string Time_Zone
        {
            get { return _Time_Zone; }
            set { _Time_Zone = value; }
        }

        private string _Daylight_Indicator;
        [XmlElement]
        public string Daylight_Indicator
        {
            get { return _Daylight_Indicator; }
            set { _Daylight_Indicator = value; }
        }

        private string _Magnetic_True_Indicator;
        [XmlElement]
        public string Magnetic_True_Indicator
        {
            get { return _Magnetic_True_Indicator; }
            set { _Magnetic_True_Indicator = value; }
        }

        private string _Datum_Code;
        [XmlElement]
        public string Datum_Code
        {
            get { return _Datum_Code; }
            set { _Datum_Code = value; }
        }

        private string _Reserved_Expansion;
        [XmlElement]
        public string Reserved_Expansion
        {
            get { return _Reserved_Expansion; }
            set { _Reserved_Expansion = value; }
        }

        private string _Airport_Name;
        [XmlElement]
        public string Airport_Name
        {
            get { return _Airport_Name; }
            set { _Airport_Name = value; }
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

        private List<ARINC_Airport_Continuation_Record> _ARINC_Airport_Continuation_Record;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public List<ARINC_Airport_Continuation_Record> ARINC_Airport_Continuation_Record
        {
            get { return _ARINC_Airport_Continuation_Record; }
            set { _ARINC_Airport_Continuation_Record = value; }
        }

        private List<ARINC_Airport_Flight_Planning_Continuation_Records> _ARINC_Airport_Flight_Planning_Continuation_Records;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public List<ARINC_Airport_Flight_Planning_Continuation_Records> ARINC_Airport_Flight_Planning_Continuation_Records
        {
            get { return _ARINC_Airport_Flight_Planning_Continuation_Records; }
            set { _ARINC_Airport_Flight_Planning_Continuation_Records = value; }
        }

        public ARINC_Airport_Primary_Record()
        {
            this.ARINC_Airport_Continuation_Record = new List<ARINC_Airport_Continuation_Record>();
            this.ARINC_Airport_Flight_Planning_Continuation_Records = new List<ARINC_Airport_Flight_Planning_Continuation_Records>();
        }


    }

    [XmlType]
    [Serializable()]

    public class ARINC_Airport_Continuation_Record : ARINC_OBJECT
    {

        private string _Blank_spacing;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing
        {
            get { return _Blank_spacing; }
            set { _Blank_spacing = value; }
        }

        private string _Airport_ICAO_Identifier;
        [XmlElement]
        public string Airport_ICAO_Identifier
        {
            get { return _Airport_ICAO_Identifier; }
            set { _Airport_ICAO_Identifier = value; }
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

        private string _ATA_IATA_Designator;
        [XmlElement]
        public string ATA_IATA_Designator
        {
            get { return _ATA_IATA_Designator; }
            set { _ATA_IATA_Designator = value; }
        }

        private string _Reserved_Expansion;
        [XmlElement]
        public string Reserved_Expansion
        {
            get { return _Reserved_Expansion; }
            set { _Reserved_Expansion = value; }
        }

        private string _Blank_spacing2;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing2
        {
            get { return _Blank_spacing2; }
            set { _Blank_spacing2 = value; }
        }

        private string _Continuation_Record_No;
        [XmlElement]
        public string Continuation_Record_No
        {
            get { return _Continuation_Record_No; }
            set { _Continuation_Record_No = value; }
        }

        private string _Reserved_Spacing;
        [XmlElement]
        public string Reserved_Spacing
        {
            get { return _Reserved_Spacing; }
            set { _Reserved_Spacing = value; }
        }

        private string _Notes;
        [XmlElement]
        public string Notes
        {
            get { return _Notes; }
            set { _Notes = value; }
        }

        private string _Reserved_Expansion2;
        [XmlElement]
        public string Reserved_Expansion2
        {
            get { return _Reserved_Expansion2; }
            set { _Reserved_Expansion2 = value; }
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

        public ARINC_Airport_Continuation_Record()
        {
            //throw new System.NotImplementedException();
        }
    }

    [XmlType]
    [Serializable()]
    public class ARINC_Airport_Flight_Planning_Continuation_Records : ARINC_OBJECT
    {

        private string _Blank_spacing;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing
        {
            get { return _Blank_spacing; }
            set { _Blank_spacing = value; }
        }

        private string _Airport_ICAO_Identifier;
        [XmlElement]
        public string Airport_ICAO_Identifier
        {
            get { return _Airport_ICAO_Identifier; }
            set { _Airport_ICAO_Identifier = value; }
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

        private string _ATA_IATA_Designator;
        [XmlElement]
        public string ATA_IATA_Designator
        {
            get { return _ATA_IATA_Designator; }
            set { _ATA_IATA_Designator = value; }
        }

        private string _Reserved_Expansion;
        [XmlElement]
        public string Reserved_Expansion
        {
            get { return _Reserved_Expansion; }
            set { _Reserved_Expansion = value; }
        }

        private string _Blank_spacing2;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing2
        {
            get { return _Blank_spacing2; }
            set { _Blank_spacing2 = value; }
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

        private string _FIR_Identifier;
        [XmlElement]
        public string FIR_Identifier
        {
            get { return _FIR_Identifier; }
            set { _FIR_Identifier = value; }
        }

        private string _UIR_Identifier;
        [XmlElement]
        public string UIR_Identifier
        {
            get { return _UIR_Identifier; }
            set { _UIR_Identifier = value; }
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

        private string _Blank_spacing3;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing3
        {
            get { return _Blank_spacing3; }
            set { _Blank_spacing3 = value; }
        }

        private string _Controlled_A_S_Indicator;
        [XmlElement]
        public string Controlled_A_S_Indicator
        {
            get { return _Controlled_A_S_Indicator; }
            set { _Controlled_A_S_Indicator = value; }
        }

        private string _Controlled_A_S_Arpt_Ident;
        [XmlElement]
        public string Controlled_A_S_Arpt_Ident
        {
            get { return _Controlled_A_S_Arpt_Ident; }
            set { _Controlled_A_S_Arpt_Ident = value; }
        }

        private string _Controlled_A_S_Arpt_ICAO;
        [XmlElement]
        public string Controlled_A_S_Arpt_ICAO
        {
            get { return _Controlled_A_S_Arpt_ICAO; }
            set { _Controlled_A_S_Arpt_ICAO = value; }
        }

        private string _Blank_spacing4;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing4
        {
            get { return _Blank_spacing4; }
            set { _Blank_spacing4 = value; }
        }

        private string _File_Record_No;
        [XmlElement]
        public string File_Record_No
        {
            get { return _File_Record_No; }
            set { _File_Record_No = value; }
        }

        private string _Cycle_Data;
        [XmlElement]
        public string Cycle_Data
        {
            get { return _Cycle_Data; }
            set { _Cycle_Data = value; }
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

        public ARINC_Airport_Flight_Planning_Continuation_Records()
        {

        }
    }


}
