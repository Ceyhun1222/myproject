using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ARINC_Types
{
    [XmlType]
    [Serializable()]
    public class ARINC_LocalizerGlideSlope_Primary_Record : ARINC_OBJECT
    {

        private string _Blank_Spacing;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_Spacing
        {
            get { return _Blank_Spacing2; }
            set { _Blank_Spacing2 = value; }
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

        private string _Localizer_Identifier;
        [XmlElement]
        public string Localizer_Identifier
        {
            get { return _Localizer_Identifier; }
            set { _Localizer_Identifier = value; }
        }

        private string _ILS_Category;
        [XmlElement]
        public string ILS_Category
        {
            get { return _ILS_Category; }
            set { _ILS_Category = value; }
        }

        private string _Blank_Spacing2;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_Spacing2
        {
            get { return _Blank_Spacing2; }
            set { _Blank_Spacing2 = value; }
        }

        private string _Continuation_Record_No;
        [XmlElement]
        public string Continuation_Record_No
        {
            get { return _Continuation_Record_No; }
            set { _Continuation_Record_No = value; }
        }

        private string _Localizer_Frequency;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Localizer_Frequency
        {
            get { return _Localizer_Frequency; }
            set { _Localizer_Frequency = value; }
        }

        private string _Runway_Identifier;
        [XmlElement]
        public string Runway_Identifier
        {
            get { return _Runway_Identifier; }
            set { _Runway_Identifier = value; }
        }

        private string _Localizer_Latitude;
        [XmlElement]
        public string Localizer_Latitude
        {
            get { return _Localizer_Latitude; }
            set { _Localizer_Latitude = value; }
        }

        private string _Localizer_Longitude;
        [XmlElement]
        public string Localizer_Longitude
        {
            get { return _Localizer_Longitude; }
            set { _Localizer_Longitude = value; }
        }

        private string _Localizer_Bearing;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Localizer_Bearing
        {
            get { return _Localizer_Bearing; }
            set { _Localizer_Bearing = value; }
        }

        private string _Glide_Slope_Latitude;
        [XmlElement]
        public string Glide_Slope_Latitude
        {
            get { return _Glide_Slope_Latitude; }
            set { _Glide_Slope_Latitude = value; }
        }

        private string _Glide_Slope_Longitude;
        [XmlElement]
        public string Glide_Slope_Longitude
        {
            get { return _Glide_Slope_Longitude; }
            set { _Glide_Slope_Longitude = value; }
        }

        private string _Localizer_Position;
        [XmlElement]
        public string Localizer_Position
        {
            get { return _Localizer_Position; }
            set { _Localizer_Position = value; }
        }

        private string _Localizer_Position_Reference;
        [XmlElement]
        public string Localizer_Position_Reference
        {
            get { return _Localizer_Position_Reference; }
            set { _Localizer_Position_Reference = value; }
        }

        private string _Glide_Slope_Position;
        [XmlElement]
        public string Glide_Slope_Position
        {
            get { return _Glide_Slope_Position; }
            set { _Glide_Slope_Position = value; }
        }

        private string _Localizer_Width;
        [XmlElement]
        public string Localizer_Width
        {
            get { return _Localizer_Width; }
            set { _Localizer_Width = value; }
        }

        private string _Glide_Slope_Angle;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Glide_Slope_Angle
        {
            get { return _Glide_Slope_Angle; }
            set { _Glide_Slope_Angle = value; }
        }

        private string _Station_Declination;
        [XmlElement]
        public string Station_Declination
        {
            get { return _Station_Declination; }
            set { _Station_Declination = value; }
        }

        private string _Glide_Slope_Height_at_Landing_Threshold;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Glide_Slope_Height_at_Landing_Threshold
        {
            get { return _Glide_Slope_Height_at_Landing_Threshold; }
            set { _Glide_Slope_Height_at_Landing_Threshold = value; }
        }

        private string _Glide_Slope_Elevation;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Glide_Slope_Elevation
        {
            get { return _Glide_Slope_Elevation; }
            set { _Glide_Slope_Elevation = value; }
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

        private List<ARINC_LocalizerGlideSlope_Continuation_Record> _ARINC_LocalizerGlideSlope_Continuation_Record;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public List<ARINC_LocalizerGlideSlope_Continuation_Record> ARINC_LocalizerGlideSlope_Continuation_Record
        {
            get { return _ARINC_LocalizerGlideSlope_Continuation_Record; }
            set { _ARINC_LocalizerGlideSlope_Continuation_Record = value; }
        }


        private List<ARINC_LocalizerGlideSlope_Simulation_Continuation_Record> _ARINC_LocalizerGlideSlope_Simulation_Continuation_Record;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public List<ARINC_LocalizerGlideSlope_Simulation_Continuation_Record> ARINC_LocalizerGlideSlope_Simulation_Continuation_Record
        {
            get { return _ARINC_LocalizerGlideSlope_Simulation_Continuation_Record; }
            set { _ARINC_LocalizerGlideSlope_Simulation_Continuation_Record = value; }
        }

        public ARINC_LocalizerGlideSlope_Primary_Record()
        {
            this._ARINC_LocalizerGlideSlope_Continuation_Record = new List<ARINC_LocalizerGlideSlope_Continuation_Record>();
            this._ARINC_LocalizerGlideSlope_Simulation_Continuation_Record = new List<ARINC_LocalizerGlideSlope_Simulation_Continuation_Record>();
        }

    }

    [XmlType]
    [Serializable()]
    public class ARINC_LocalizerGlideSlope_Continuation_Record : ARINC_OBJECT
    {
        private string _Blank_Spacing;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_Spacing
        {
            get { return _Blank_Spacing2; }
            set { _Blank_Spacing2 = value; }
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

        private string _Localizer_Identifier;
        [XmlElement]
        public string Localizer_Identifier
        {
            get { return _Localizer_Identifier; }
            set { _Localizer_Identifier = value; }
        }

        private string _ILS_Category;
        [XmlElement]
        public string ILS_Category
        {
            get { return _ILS_Category; }
            set { _ILS_Category = value; }
        }

        private string _Blank_Spacing2;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_Spacing2
        {
            get { return _Blank_Spacing2; }
            set { _Blank_Spacing2 = value; }
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

        

        public ARINC_LocalizerGlideSlope_Continuation_Record()
        {

        }

    }

    [XmlType]
    [Serializable()]
    public class ARINC_LocalizerGlideSlope_Simulation_Continuation_Record : ARINC_OBJECT
    {

        private string _Blank_Spacing;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_Spacing
        {
            get { return _Blank_Spacing; }
            set { _Blank_Spacing = value; }
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

        private string _Localizer_Identifier;
        [XmlElement]
        public string Localizer_Identifier
        {
            get { return _Localizer_Identifier; }
            set { _Localizer_Identifier = value; }
        }

        private string _ILS_Category;
        [XmlElement]
        public string ILS_Category
        {
            get { return _ILS_Category; }
            set { _ILS_Category = value; }
        }

        private string _Blank_Spacing2;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_Spacing2
        {
            get { return _Blank_Spacing2; }
            set { _Blank_Spacing2 = value; }
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

        private string _Blank_spacing3;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing3
        {
            get { return _Blank_spacing3; }
            set { _Blank_spacing3 = value; }
        }

        private string _Facility_Characteristics;
        [XmlElement]
        public string Facility_Characteristics
        {
            get { return _Facility_Characteristics; }
            set { _Facility_Characteristics = value; }
        }

        private string _Blank_spacing4;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing4
        {
            get { return _Blank_spacing4; }
            set { _Blank_spacing4 = value; }
        }

        private string _Localizer_True_Bearing;
        [XmlElement]
        public string Localizer_True_Bearing
        {
            get { return _Localizer_True_Bearing; }
            set { _Localizer_True_Bearing = value; }
        }

        private string _Localizer_Bearing_Source;
        [XmlElement]
        public string Localizer_Bearing_Source
        {
            get { return _Localizer_Bearing_Source; }
            set { _Localizer_Bearing_Source = value; }
        }

        private string _Reserved_spacing;
        [XmlElement]
        public string Reserved_spacing
        {
            get { return _Reserved_spacing; }
            set { _Reserved_spacing = value; }
        }

        private string _Glide_Slope_Beam_Width;
        [XmlElement]
        public string Glide_Slope_Beam_Width
        {
            get { return _Glide_Slope_Beam_Width; }
            set { _Glide_Slope_Beam_Width = value; }
        }

        private string _Approach_Route_Ident;
        [XmlElement]
        public string Approach_Route_Ident
        {
            get { return _Approach_Route_Ident; }
            set { _Approach_Route_Ident = value; }
        }

        private string _Approach_Route_Ident2;
        [XmlElement]
        public string Approach_Route_Ident2
        {
            get { return _Approach_Route_Ident2; }
            set { _Approach_Route_Ident2 = value; }
        }

        private string _Approach_Route_Ident3;
        [XmlElement]
        public string Approach_Route_Ident3
        {
            get { return _Approach_Route_Ident3; }
            set { _Approach_Route_Ident3 = value; }
        }

        private string _Approach_Route_Ident4;
        [XmlElement]
        public string Approach_Route_Ident4
        {
            get { return _Approach_Route_Ident4; }
            set { _Approach_Route_Ident4 = value; }
        }

        private string _Approach_Route_Ident5;
        [XmlElement]
        public string Approach_Route_Ident5
        {
            get { return _Approach_Route_Ident5; }
            set { _Approach_Route_Ident5 = value; }
        }

        private string _Blank_spacing;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing
        {
            get { return _Blank_spacing; }
            set { _Blank_spacing = value; }
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

        public ARINC_LocalizerGlideSlope_Simulation_Continuation_Record()
        {

        }
    }


}
