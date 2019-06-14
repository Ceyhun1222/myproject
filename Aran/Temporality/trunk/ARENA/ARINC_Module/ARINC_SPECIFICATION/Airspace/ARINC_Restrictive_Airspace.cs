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
    public class ARINC_Restrictive_Airspace_Primary_Records : ARINC_OBJECT
    {

        private string _ICAO_Code;
        [XmlElement]
        public string ICAO_Code
        {
            get { return _ICAO_Code; }
            set { _ICAO_Code = value; }
        }

        private string _Restrictive_Type;
        [XmlElement]
        public string Restrictive_Type
        {
            get { return _Restrictive_Type; }
            set { _Restrictive_Type = value; }
        }

        private string _Restrictive_Airspace_Designation;
        [XmlElement]
        public string Restrictive_Airspace_Designation
        {
            get { return _Restrictive_Airspace_Designation; }
            set { _Restrictive_Airspace_Designation = value; }
        }

        private string _Multiple_Code;
        [XmlElement]
        public string Multiple_Code
        {
            get { return _Multiple_Code; }
            set { _Multiple_Code = value; }
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

        private string _Level;
        [XmlElement]
        public string Level
        {
            get { return _Level; }
            set { _Level = value; }
        }

        private string _Time_Code;
        [XmlElement]
        public string Time_Code
        {
            get { return _Time_Code; }
            set { _Time_Code = value; }
        }

        private string _NOTAM;
        [XmlElement]
        public string NOTAM
        {
            get { return _NOTAM; }
            set { _NOTAM = value; }
        }

        private string _Blank_Spacing1;
        [XmlElement]
        public string Blank_Spacing1
        {
            get { return _Blank_Spacing1; }
            set { _Blank_Spacing1 = value; }
        }

        private string _Boundary_Via;
        [XmlElement]
        public string Boundary_Via
        {
            get { return _Boundary_Via; }
            set { _Boundary_Via = value; }
        }

        private string _Latitude;
        [XmlElement]
        public string Latitude
        {
            get { return _Latitude; }
            set { _Latitude = value; }
        }

        private string _Longitude;
        [XmlElement]
        public string Longitude
        {
            get { return _Longitude; }
            set { _Longitude = value; }
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

        private string _Blank_Spacing2;
        [XmlElement]
        public string Blank_Spacing2
        {
            get { return _Blank_Spacing2; }
            set { _Blank_Spacing2 = value; }
        }

        private string _Lower_Limit;
        [XmlElement]
        public string Lower_Limit
        {
            get { return _Lower_Limit; }
            set { _Lower_Limit = value; }
        }

        private string _Unit_Indicator1;
        [XmlElement]
        public string Unit_Indicator1
        {
            get { return _Unit_Indicator1; }
            set { _Unit_Indicator1 = value; }
        }

        private string _Upper_Limit;
        [XmlElement]
        public string Upper_Limit
        {
            get { return _Upper_Limit; }
            set { _Upper_Limit = value; }
        }

        private string _Unit_Indicator2;
        [XmlElement]
        public string Unit_Indicator2
        {
            get { return _Unit_Indicator2; }
            set { _Unit_Indicator2 = value; }
        }

        private string _Restrictive_Airspace_Name;
        [XmlElement]
        public string Restrictive_Airspace_Name
        {
            get { return _Restrictive_Airspace_Name; }
            set { _Restrictive_Airspace_Name = value; }
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

        public ARINC_Restrictive_Airspace_Primary_Records()
        {

        }
    }

}
