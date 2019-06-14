using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ARINC_Types
{
    [XmlType]
    [Serializable()]
    public class ARINC_Runway_Primary_Records : ARINC_OBJECT
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

        private string _Runway_Identifier;
        [XmlElement]
        public string Runway_Identifier
        {
            get { return _Runway_Identifier; }
            set { _Runway_Identifier = value; }
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

        private string _Runway_Length;
        [XmlElement]
        public string Runway_Length
        {
            get { return _Runway_Length; }
            set { _Runway_Length = value; }
        }

        private string _Runway_Magnetic_Bearing;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]

        public string Runway_Magnetic_Bearing
        {
            get { return _Runway_Magnetic_Bearing; }
            set { _Runway_Magnetic_Bearing = value; }
        }

        private string _Blank_spacing3;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing3
        {
            get { return _Blank_spacing3; }
            set { _Blank_spacing3 = value; }
        }

        private string _Runway_Latitude;
        [XmlElement]
        public string Runway_Latitude
        {
            get { return _Runway_Latitude; }
            set { _Runway_Latitude = value; }
        }

        private string _Runway_Longitude;
        [XmlElement]
        public string Runway_Longitude
        {
            get { return _Runway_Longitude; }
            set { _Runway_Longitude = value; }
        }

        private string _Runway_Gradient;
        [XmlElement]
        public string Runway_Gradient
        {
            get { return _Runway_Gradient; }
            set { _Runway_Gradient = value; }
        }

        private string _Blank_spacing4;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing4
        {
            get { return _Blank_spacing4; }
            set { _Blank_spacing4 = value; }
        }

        private string _Landing_Threshold_Elevation;
        [XmlElement]
        public string Landing_Threshold_Elevation
        {
            get { return _Landing_Threshold_Elevation; }
            set { _Landing_Threshold_Elevation = value; }
        }

        private string _Displaced_Threshold_Distance;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Displaced_Threshold_Distance
        {
            get { return _Displaced_Threshold_Distance; }
            set { _Displaced_Threshold_Distance = value; }
        }

        private string _Threshold_Crossing_Height;
        [XmlElement]
        public string Threshold_Crossing_Height
        {
            get { return _Threshold_Crossing_Height; }
            set { _Threshold_Crossing_Height = value; }
        }

        private string _Runway_Width;
        [XmlElement]
        public string Runway_Width
        {
            get { return _Runway_Width; }
            set { _Runway_Width = value; }
        }

        private string _Blank_spacing5;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing5
        {
            get { return _Blank_spacing5; }
            set { _Blank_spacing5 = value; }
        }

        private string _Localizer_MLS__GLS_Ref_PathIdentifier;
        [XmlElement]
        public string Localizer_MLS__GLS_Ref_PathIdentifier
        {
            get { return _Localizer_MLS__GLS_Ref_PathIdentifier; }
            set { _Localizer_MLS__GLS_Ref_PathIdentifier = value; }
        }

        private string _Localizer__MLS__GLS_Category_Class;
        [XmlElement]
        public string Localizer__MLS__GLS_Category_Class
        {
            get { return _Localizer__MLS__GLS_Category_Class; }
            set { _Localizer__MLS__GLS_Category_Class = value; }
        }

        private string _Stopway;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Stopway
        {
            get { return _Stopway; }
            set { _Stopway = value; }
        }

        private string _Second_Localizer__MLS__GLS_Ref_PathIdent;
        [XmlElement]
        public string Second_Localizer__MLS__GLS_Ref_PathIdent
        {
            get { return _Second_Localizer__MLS__GLS_Ref_PathIdent; }
            set { _Second_Localizer__MLS__GLS_Ref_PathIdent = value; }
        }

        private string _Second_Localizer__MLS__GLSCategory__Class;
        [XmlElement]
        public string Second_Localizer__MLS__GLSCategory__Class
        {
            get { return _Second_Localizer__MLS__GLSCategory__Class; }
            set { _Second_Localizer__MLS__GLSCategory__Class = value; }
        }

        private string _Reserved_Expansion;
        [XmlElement]
        public string Reserved_Expansion
        {
            get { return _Reserved_Expansion; }
            set { _Reserved_Expansion = value; }
        }

        private string _Runway_Description;
        [XmlElement]
        public string Runway_Description
        {
            get { return _Runway_Description; }
            set { _Runway_Description = value; }
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

        private List<ARINC_Runway_Continuation_Records> _Runway_Continuation_Records;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public List<ARINC_Runway_Continuation_Records> Runway_Continuation_Records
        {
            get { return _Runway_Continuation_Records; }
            set { _Runway_Continuation_Records = value; }
        }

        private List<ARINC_Runway_Simulation_Continuation_Records> _Runway_Simulation_Continuation_Records;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public List<ARINC_Runway_Simulation_Continuation_Records> Runway_Simulation_Continuation_Records
        {
            get { return _Runway_Simulation_Continuation_Records; }
            set { _Runway_Simulation_Continuation_Records = value; }
        }

        public ARINC_Runway_Primary_Records()
        {
            this.Runway_Continuation_Records = new List<ARINC_Runway_Continuation_Records>();
            this.Runway_Simulation_Continuation_Records = new List<ARINC_Runway_Simulation_Continuation_Records>();
        }
    }

    [XmlType]
    [Serializable()]
    public class ARINC_Runway_Continuation_Records : ARINC_OBJECT
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

        private string _Blank_spacing2;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing2
        {
            get { return _Blank_spacing2; }
            set { _Blank_spacing2 = value; }
        }

        private string _Continuation_Record_Number;
        [XmlElement]
        public string Continuation_Record_Number
        {
            get { return _Continuation_Record_Number; }
            set { _Continuation_Record_Number = value; }
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

        public ARINC_Runway_Continuation_Records()
        {

        }
    }

    [XmlType]
    [Serializable()]
    public class ARINC_Runway_Simulation_Continuation_Records : ARINC_OBJECT
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

        private string _Continuation_Record_Number;
        [XmlElement]
        public string Continuation_Record_Number
        {
            get { return _Continuation_Record_Number; }
            set { _Continuation_Record_Number = value; }
        }

        private string _Application_Type;
        [XmlElement]
        public string Application_Type
        {
            get { return _Application_Type; }
            set { _Application_Type = value; }
        }

        private string _Reserved_Spacing;
        [XmlElement]
        public string Reserved_Spacing
        {
            get { return _Reserved_Spacing; }
            set { _Reserved_Spacing = value; }
        }

        private string _Runway_True_Bearing;
        [XmlElement]
        public string Runway_True_Bearing
        {
            get { return _Runway_True_Bearing; }
            set { _Runway_True_Bearing = value; }
        }

        private string _True_Bearing_Source;
        [XmlElement]
        public string True_Bearing_Source
        {
            get { return _True_Bearing_Source; }
            set { _True_Bearing_Source = value; }
        }

        private string _Reserved_Spacing2;
        [XmlElement]
        public string Reserved_Spacing2
        {
            get { return _Reserved_Spacing2; }
            set { _Reserved_Spacing2 = value; }
        }

        private string _TDZE_Location;
        [XmlElement]
        public string TDZE_Location
        {
            get { return _TDZE_Location; }
            set { _TDZE_Location = value; }
        }

        private string _Touchdown_Zone_Elevation;
        [XmlElement]
        public string Touchdown_Zone_Elevation
        {
            get { return _Touchdown_Zone_Elevation; }
            set { _Touchdown_Zone_Elevation = value; }
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

        public ARINC_Runway_Simulation_Continuation_Records()
        {

        }
    }


}
