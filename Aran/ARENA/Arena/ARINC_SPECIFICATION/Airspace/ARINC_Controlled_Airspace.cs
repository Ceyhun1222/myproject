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
    public class ARINC_Controlled_Airspace_Primary_Records : ARINC_OBJECT
    {

        private string _ICAO_Code;
        [XmlElement]
        public string ICAO_Code
        {
            get { return _ICAO_Code; }
            set { _ICAO_Code = value; }
        }

        private string _Airspace_Type;
        [XmlElement]
        public string Airspace_Type
        {
            get { return _Airspace_Type; }
            set { _Airspace_Type = value; }
        }

        private string _Airspace_Center;
        [XmlElement]
        public string Airspace_Center
        {
            get { return _Airspace_Center; }
            set { _Airspace_Center = value; }
        }

        private string _Section_Code;
        [XmlElement]
        public string Section_Code
        {
            get { return _Section_Code; }
            set { _Section_Code = value; }
        }

        private string _Subsection_Code;
        [XmlElement]
        public string Subsection_Code
        {
            get { return _Subsection_Code; }
            set { _Subsection_Code = value; }
        }

        private string _Airspace_Classification;
        [XmlElement]
        public string Airspace_Classification
        {
            get { return _Airspace_Classification; }
            set { _Airspace_Classification = value; }
        }

        private string _Reserved_spacing;
        [XmlElement]
        public string Reserved_spacing
        {
            get { return _Reserved_spacing; }
            set { _Reserved_spacing = value; }
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

        private string _Continuation_Record_Number;
        [XmlElement]
        public string Continuation_Record_Number
        {
            get { return _Continuation_Record_Number; }
            set { _Continuation_Record_Number = value; }
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

        private string _Blank_spacing;
        [XmlElement]
        public string Blank_spacing
        {
            get { return _Blank_spacing; }
            set { _Blank_spacing = value; }
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

        private string _RNP;
        [XmlElement]
        public string RNP
        {
            get { return _RNP; }
            set { _RNP = value; }
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

        private string _Controlled_Airspace_Name;
        [XmlElement]
        public string Controlled_Airspace_Name
        {
            get { return _Controlled_Airspace_Name; }
            set { _Controlled_Airspace_Name = value; }
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

        public ARINC_Controlled_Airspace_Primary_Records()
        {

        }
    }

}
