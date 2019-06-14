using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ARINC_Types
{
    [XmlType]
    [Serializable()]
    public class ARINC_Airport_Marker : ARINC_OBJECT
    {

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

        private string _Marker_Type;
        [XmlElement]
        public string Marker_Type
        {
            get { return _Marker_Type; }
            set { _Marker_Type = value; }
        }

        private string _Blank_Spacing1;
        [XmlElement]
        public string Blank_Spacing1
        {
            get { return _Blank_Spacing1; }
            set { _Blank_Spacing1 = value; }
        }

        private string _Continuation_Record_No;
        [XmlElement]
        public string Continuation_Record_No
        {
            get { return _Continuation_Record_No; }
            set { _Continuation_Record_No = value; }
        }

        private string _Locator_Frequency;
        [XmlElement]
        public string Locator_Frequency
        {
            get { return _Locator_Frequency; }
            set { _Locator_Frequency = value; }
        }

        private string _Runway_Identifier;
        [XmlElement]
        public string Runway_Identifier
        {
            get { return _Runway_Identifier; }
            set { _Runway_Identifier = value; }
        }

        private string _Marker_Latitude;
        [XmlElement]
        public string Marker_Latitude
        {
            get { return _Marker_Latitude; }
            set { _Marker_Latitude = value; }
        }

        private string _Marker_Longitude;
        [XmlElement]
        public string Marker_Longitude
        {
            get { return _Marker_Longitude; }
            set { _Marker_Longitude = value; }
        }

        private string _Minor_Axis_Bearing;
        [XmlElement]
        public string Minor_Axis_Bearing
        {
            get { return _Minor_Axis_Bearing; }
            set { _Minor_Axis_Bearing = value; }
        }

        private string _Locator_Latitude;
        [XmlElement]
        public string Locator_Latitude
        {
            get { return _Locator_Latitude; }
            set { _Locator_Latitude = value; }
        }

        private string _Locator_Longitude;
        [XmlElement]
        public string Locator_Longitude
        {
            get { return _Locator_Longitude; }
            set { _Locator_Longitude = value; }
        }

        private string _Locator_Class;
        [XmlElement]
        public string Locator_Class
        {
            get { return _Locator_Class; }
            set { _Locator_Class = value; }
        }

        private string _Locator_Facility_Characteristics;
        [XmlElement]
        public string Locator_Facility_Characteristics
        {
            get { return _Locator_Facility_Characteristics; }
            set { _Locator_Facility_Characteristics = value; }
        }

        private string _Locator_Identifier;
        [XmlElement]
        public string Locator_Identifier
        {
            get { return _Locator_Identifier; }
            set { _Locator_Identifier = value; }
        }

        private string _Blank_Spacing2;
        [XmlElement]
        public string Blank_Spacing2
        {
            get { return _Blank_Spacing2; }
            set { _Blank_Spacing2 = value; }
        }

        private string _Magnetic_Variation;
        [XmlElement]
        public string Magnetic_Variation
        {
            get { return _Magnetic_Variation; }
            set { _Magnetic_Variation = value; }
        }

        private string _Blank_Spacing3;
        [XmlElement]
        public string Blank_Spacing3
        {
            get { return _Blank_Spacing3; }
            set { _Blank_Spacing3 = value; }
        }

        private string _Facility_Elevation;
        [XmlElement]
        public string Facility_Elevation
        {
            get { return _Facility_Elevation; }
            set { _Facility_Elevation = value; }
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

        public ARINC_Airport_Marker()
        {

        }
    }

}
