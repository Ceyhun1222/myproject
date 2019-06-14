using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ARINC_Types;

namespace ARINC_Types
{
    [XmlType]
    [Serializable()]
    public class ARINC_FIR_UIR_Primary_Records : ARINC_OBJECT
    {

        private string _FIR_UIR_Identifier;
        [XmlElement]
        public string FIR_UIR_Identifier
        {
            get { return _FIR_UIR_Identifier; }
            set { _FIR_UIR_Identifier = value; }
        }

        private string _FIR_UIR_Address;
        [XmlElement]
        public string FIR_UIR_Address
        {
            get { return _FIR_UIR_Address; }
            set { _FIR_UIR_Address = value; }
        }

        private string _FIR_UIR_Indicator;
        [XmlElement]
        public string FIR_UIR_Indicator
        {
            get { return _FIR_UIR_Indicator; }
            set { _FIR_UIR_Indicator = value; }
        }

        private string _Sequence_Number;
        [XmlElement]
        public string Sequence_Number
        {
            get { return _Sequence_Number; }
            set { _Sequence_Number = value; }
        }

        private string _Continuation_Record_No;
        [XmlElement]
        public string Continuation_Record_No
        {
            get { return _Continuation_Record_No; }
            set { _Continuation_Record_No = value; }
        }

        private string _Adjacent_FIR_Identifier;
        [XmlElement]
        public string Adjacent_FIR_Identifier
        {
            get { return _Adjacent_FIR_Identifier; }
            set { _Adjacent_FIR_Identifier = value; }
        }

        private string _Adjacent_UIR_Identifier;
        [XmlElement]
        public string Adjacent_UIR_Identifier
        {
            get { return _Adjacent_UIR_Identifier; }
            set { _Adjacent_UIR_Identifier = value; }
        }

        private string _Reporting_Units_Speed;
        [XmlElement]
        public string Reporting_Units_Speed
        {
            get { return _Reporting_Units_Speed; }
            set { _Reporting_Units_Speed = value; }
        }

        private string _Reporting_Units_Altitude;
        [XmlElement]
        public string Reporting_Units_Altitude
        {
            get { return _Reporting_Units_Altitude; }
            set { _Reporting_Units_Altitude = value; }
        }

        private string _Entry_Report;
        [XmlElement]
        public string Entry_Report
        {
            get { return _Entry_Report; }
            set { _Entry_Report = value; }
        }

        private string _Blank_Spacing;
        [XmlElement]
        public string Blank_Spacing
        {
            get { return _Blank_Spacing; }
            set { _Blank_Spacing = value; }
        }

        private string _Boundary_Via;
        [XmlElement]
        public string Boundary_Via
        {
            get { return _Boundary_Via; }
            set { _Boundary_Via = value; }
        }

        private string _FIR_UIR_Latitude;
        [XmlElement]
        public string FIR_UIR_Latitude
        {
            get { return _FIR_UIR_Latitude; }
            set { _FIR_UIR_Latitude = value; }
        }

        private string _FIR_UIR_Longitude;
        [XmlElement]
        public string FIR_UIR_Longitude
        {
            get { return _FIR_UIR_Longitude; }
            set { _FIR_UIR_Longitude = value; }
        }

        private string _Arc_Origin_Latitude;
        [XmlElement]
        public string Arc_Origin_Latitude
        {
            get { return _Arc_Origin_Latitude; }
            set { _Arc_Origin_Latitude = value; }
        }

        private string _Arc_Origin_Longitude;
        [XmlElement]
        public string Arc_Origin_Longitude
        {
            get { return _Arc_Origin_Longitude; }
            set { _Arc_Origin_Longitude = value; }
        }

        private string _Arc_Distance;
        [XmlElement]
        public string Arc_Distance
        {
            get { return _Arc_Distance; }
            set { _Arc_Distance = value; }
        }

        private string _Arc_Bearing;
        [XmlElement]
        public string Arc_Bearing
        {
            get { return _Arc_Bearing; }
            set { _Arc_Bearing = value; }
        }

        private string _FIR_Upper_Limit;
        [XmlElement]
        public string FIR_Upper_Limit
        {
            get { return _FIR_Upper_Limit; }
            set { _FIR_Upper_Limit = value; }
        }

        private string _UIR_Lower_Limit;
        [XmlElement]
        public string UIR_Lower_Limit
        {
            get { return _UIR_Lower_Limit; }
            set { _UIR_Lower_Limit = value; }
        }

        private string _UIR_Upper_Limit;
        [XmlElement]
        public string UIR_Upper_Limit
        {
            get { return _UIR_Upper_Limit; }
            set { _UIR_Upper_Limit = value; }
        }

        private string _Cruise_Table_Ind;
        [XmlElement]
        public string Cruise_Table_Ind
        {
            get { return _Cruise_Table_Ind; }
            set { _Cruise_Table_Ind = value; }
        }

        private string _Reserved_Expansion;
        [XmlElement]
        public string Reserved_Expansion
        {
            get { return _Reserved_Expansion; }
            set { _Reserved_Expansion = value; }
        }

        private string _FIR_UIR_Name;
        [XmlElement]
        public string FIR_UIR_Name
        {
            get { return _FIR_UIR_Name; }
            set { _FIR_UIR_Name = value; }
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

        public ARINC_FIR_UIR_Primary_Records()
        {

        }
    }

}
