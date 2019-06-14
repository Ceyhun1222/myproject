using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ARINC_Types
{
    [XmlType]
    [Serializable()]
    public class ARINC_WayPoint_Primary_Record : ARINC_OBJECT
    {
        private string _Region_Code;
        [XmlElement]
        public string Region_Code
        {
            get { return _Region_Code; }
            set { _Region_Code = value; }
        }


        private string _ICAO_Code;
        [XmlElement]
        public string ICAO_Code
        {
            get { return _ICAO_Code; }
            set { _ICAO_Code = value; }
        }


        private string _Subsection_1;
        [XmlElement]
        public string Subsection_1
        {
            get { return _Subsection_1; }
            set { _Subsection_1 = value; }
        }


        private string _Waypoint_Identifier;
        [XmlElement]
        public string Waypoint_Identifier
        {
            get { return _Waypoint_Identifier; }
            set { _Waypoint_Identifier = value; }
        }


        private string _Blank_Spacing1;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_Spacing1
        {
            get { return _Blank_Spacing1; }
            set { _Blank_Spacing1 = value; }
        }


        private string _ICAO_Code2;
        [XmlElement]
        public string ICAO_Code2
        {
            get { return _ICAO_Code2; }
            set { _ICAO_Code2 = value; }
        }


        private string _Continuation_Record_No;
        [XmlElement]
        public string Continuation_Record_No
        {
            get { return _Continuation_Record_No; }
            set { _Continuation_Record_No = value; }
        }


        private string _Blank_Spacing2;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_Spacing2
        {
            get { return _Blank_Spacing2; }
            set { _Blank_Spacing2 = value; }
        }


        private string _Waypoint_Type;
        [XmlElement]
        public string Waypoint_Type
        {
            get { return _Waypoint_Type; }
            set { _Waypoint_Type = value; }
        }


        private string _Waypoint_Usage;
        [XmlElement]
        public string Waypoint_Usage
        {
            get { return _Waypoint_Usage; }
            set { _Waypoint_Usage = value; }
        }


        private string _Blank_Spacing3;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_Spacing3
        {
            get { return _Blank_Spacing3; }
            set { _Blank_Spacing3 = value; }
        }


        private string _Waypoint_Latitude;
        [XmlElement]
        public string Waypoint_Latitude
        {
            get { return _Waypoint_Latitude; }
            set { _Waypoint_Latitude = value; }
        }


        private string _Waypoint_Longitude;
        [XmlElement]
        public string Waypoint_Longitude
        {
            get { return _Waypoint_Longitude; }
            set { _Waypoint_Longitude = value; }
        }


        private string _Blank_Spacing4;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_Spacing4
        {
            get { return _Blank_Spacing4; }
            set { _Blank_Spacing4 = value; }
        }


        private string _Dynamic_Mag_Variation;
        [XmlElement]
        public string Dynamic_Mag_Variation
        {
            get { return _Dynamic_Mag_Variation; }
            set { _Dynamic_Mag_Variation = value; }
        }


        private string _Reserved1;
        [XmlElement]
        public string Reserved1
        {
            get { return _Reserved1; }
            set { _Reserved1 = value; }
        }


        private string _Datum_Code;
        [XmlElement]
        public string Datum_Code
        {
            get { return _Datum_Code; }
            set { _Datum_Code = value; }
        }


        private string _Reserved2;
        [XmlElement]
        public string Reserved2
        {
            get { return _Reserved2; }
            set { _Reserved2 = value; }
        }


        private string _Name_Format_Indicator;
        [XmlElement]
        public string Name_Format_Indicator
        {
            get { return _Name_Format_Indicator; }
            set { _Name_Format_Indicator = value; }
        }


        private string _Waypoint_Name_Description;
        [XmlElement]
        public string Waypoint_Name_Description
        {
            get { return _Waypoint_Name_Description; }
            set { _Waypoint_Name_Description = value; }
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


        private List<ARINC_WayPoint_Continuation_Record> _Continuation_Record;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public List<ARINC_WayPoint_Continuation_Record> Continuation_Record
        {
            get { return _Continuation_Record; }
            set { _Continuation_Record = value; }
        }


        private List<ARINC_WayPoint_Flight_Planing_Continuation_Record> _Flight_Planing_Continuation_Record;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public List<ARINC_WayPoint_Flight_Planing_Continuation_Record> Flight_Planing_Continuation_Record
        {
            get { return _Flight_Planing_Continuation_Record; }
            set { _Flight_Planing_Continuation_Record = value; }
        }

        public ARINC_WayPoint_Primary_Record()
        {
            this.Continuation_Record = new List<ARINC_WayPoint_Continuation_Record>();
            this.Flight_Planing_Continuation_Record = new List<ARINC_WayPoint_Flight_Planing_Continuation_Record>();

        }

    }

    [XmlType]
    [Serializable()]
    public class ARINC_WayPoint_Continuation_Record : ARINC_OBJECT
    {

        private string _Region_Code;
        [XmlElement]
        public string Region_Code
        {
            get { return _Region_Code; }
            set { _Region_Code = value; }
        }


        private string _ICAO_Code;
        [XmlElement]
        public string ICAO_Code
        {
            get { return _ICAO_Code; }
            set { _ICAO_Code = value; }
        }


        private string _Subsection_1;
        [XmlElement]
        public string Subsection_1
        {
            get { return _Subsection_1; }
            set { _Subsection_1 = value; }
        }


        private string _Waypoint_Identifier;
        [XmlElement]
        public string Waypoint_Identifier
        {
            get { return _Waypoint_Identifier; }
            set { _Waypoint_Identifier = value; }
        }


        private string _Blan_Spacing1;
        [XmlElement]
        public string Blan_Spacing1
        {
            get { return _Blan_Spacing1; }
            set { _Blan_Spacing1 = value; }
        }


        private string _ICAO_Code2;
        [XmlElement]
        public string ICAO_Code2
        {
            get { return _ICAO_Code2; }
            set { _ICAO_Code2 = value; }
        }


        private string _Continuation_Record_No;
        [XmlElement]
        public string Continuation_Record_No
        {
            get { return _Continuation_Record_No; }
            set { _Continuation_Record_No = value; }
        }


        private string _Reserved_1;
        [XmlElement]
        public string Reserved_1
        {
            get { return _Reserved_1; }
            set { _Reserved_1 = value; }
        }


        private string _Notes;
        [XmlElement]
        public string Notes
        {
            get { return _Notes; }
            set { _Notes = value; }
        }


        private string _Reserved_2;
        [XmlElement]
        public string Reserved_2
        {
            get { return _Reserved_2; }
            set { _Reserved_2 = value; }
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

        public ARINC_WayPoint_Continuation_Record()
        {
           // throw new System.NotImplementedException();
        }

    }

    [XmlType]
    [Serializable()]
    public class ARINC_WayPoint_Flight_Planing_Continuation_Record : ARINC_OBJECT
    {

        private string _Region_Code;
        [XmlElement]
        public string Region_Code
        {
            get { return _Region_Code; }
            set { _Region_Code = value; }
        }


        private string _ICAO_Code;
        [XmlElement]
        public string ICAO_Code
        {
            get { return _ICAO_Code; }
            set { _ICAO_Code = value; }
        }


        private string _Subsection_1;
        [XmlElement]
        public string Subsection_1
        {
            get { return _Subsection_1; }
            set { _Subsection_1 = value; }
        }


        private string _Waypoint_Identifier;
        [XmlElement]
        public string Waypoint_Identifier
        {
            get { return _Waypoint_Identifier; }
            set { _Waypoint_Identifier = value; }
        }


        private string _Blank_Spacing1;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_Spacing1
        {
            get { return _Blank_Spacing1; }
            set { _Blank_Spacing1 = value; }
        }


        private string _ICAO_Code2;
        [XmlElement]
        public string ICAO_Code2
        {
            get { return _ICAO_Code2; }
            set { _ICAO_Code2 = value; }
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

        public ARINC_WayPoint_Flight_Planing_Continuation_Record()
        {
           // throw new System.NotImplementedException();
        }

    }
}
