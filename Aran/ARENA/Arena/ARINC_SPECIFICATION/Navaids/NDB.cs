using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.ComponentModel;

namespace ARINC_Types
{
    [XmlType]
    [Serializable()]
    public class ARINC_Navaid_NDB_Primary_Record : ARINC_Navaid
    {
        private string _NDB_Frequency;
        [XmlElement]
        [Browsable(false)]
        public string NDB_Frequency
        {
            get { return _NDB_Frequency; }
            set { _NDB_Frequency = value; }
        }

        private string _NAVAID_Class;
        [XmlElement]
        public string NAVAID_Class
        {
            get { return _NAVAID_Class; }
            set { _NAVAID_Class = value; }
        }

        private string _NDB_Latitude;
        [XmlElement]
        public string NDB_Latitude
        {
            get { return _NDB_Latitude; }
            set { _NDB_Latitude = value; }
        }

        private string _NDB_Longitude;
        [XmlElement]
        public string NDB_Longitude
        {
            get { return _NDB_Longitude; }
            set { _NDB_Longitude = value; }
        }


        private string _Blank_Spacing2;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_Spacing2
        {
            get { return _Blank_Spacing2; }
            set { _Blank_Spacing2 = value; }
        }


        private string _Magnetic_Variation;
        [XmlElement]
        [Browsable(false)]
        public string Magnetic_Variation
        {
            get { return _Magnetic_Variation; }
            set { _Magnetic_Variation = value; }
        }


        private string _Blank_Spacing3;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_Spacing3
        {
            get { return _Blank_Spacing3; }
            set { _Blank_Spacing3 = value; }
        }


        private string _Reserved_Expansion;
        [XmlElement]
        public string Reserved_Expansion
        {
            get { return _Reserved_Expansion; }
            set { _Reserved_Expansion = value; }
        }

        private string _Datum_Code;
        [XmlElement]
        public string Datum_Code
        {
            get { return _Datum_Code; }
            set { _Datum_Code = value; }
        }

        private string _NDB_Name;
        [XmlElement]
        public string NDB_Name
        {
            get { return _NDB_Name; }
            set { _NDB_Name = value; }
        }

        private List<ARINC_Navaid_NDB_Continuation_Record> _Continuation_Record;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public List<ARINC_Navaid_NDB_Continuation_Record> Continuation_Record
        {
            get { return _Continuation_Record; }
            set { _Continuation_Record = value; }
        }


        private List<ARINC_Navaid_NDB_Simulation_Continuation_Record> _Simulation_Continuation_Record;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public List<ARINC_Navaid_NDB_Simulation_Continuation_Record> Simulation_Continuation_Record
        {
            get { return _Simulation_Continuation_Record; }
            set { _Simulation_Continuation_Record = value; }
        }


        private List<ARINC_Navaid_NDB_Flight_Planing_Continuation_Record> _Flight_Planing_Continuation_Record;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public List<ARINC_Navaid_NDB_Flight_Planing_Continuation_Record> Flight_Planing_Continuation_Record
        {
            get { return _Flight_Planing_Continuation_Record; }
            set { _Flight_Planing_Continuation_Record = value; }
        }

        private List<ARINC_Navaid_NDB_Limitation_Continuation_Record> _Limitation_Continuation_Record;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public List<ARINC_Navaid_NDB_Limitation_Continuation_Record> Limitation_Continuation_Record
        {
            get { return _Limitation_Continuation_Record; }
            set { _Limitation_Continuation_Record = value; }
        }

        public ARINC_Navaid_NDB_Primary_Record()
        {
            this.Continuation_Record = new List<ARINC_Navaid_NDB_Continuation_Record>();
            this.Simulation_Continuation_Record = new List<ARINC_Navaid_NDB_Simulation_Continuation_Record>();
            this.Flight_Planing_Continuation_Record = new List<ARINC_Navaid_NDB_Flight_Planing_Continuation_Record>();
            this.Limitation_Continuation_Record = new List<ARINC_Navaid_NDB_Limitation_Continuation_Record>();
        }
    }

    [XmlType]
    [Serializable()]
    public class ARINC_Navaid_NDB_Continuation_Record : ARINC_Navaid
    {
        private string _Navaid_Type;
        [XmlElement]
        public string Navaid_Type
        {
            get { return _Navaid_Type; }
            set { _Navaid_Type = value; }
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

        private string _NDB_Identifier;
        [XmlElement]
        public string NDB_Identifier
        {
            get { return _NDB_Identifier; }
            set { _NDB_Identifier = value; }
        }

        private string _Blank;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank
        {
            get { return _Blank; }
            set { _Blank = value; }
        }

        private string _ICAO_Code_USA;
        [XmlElement]
        public string ICAO_Code_USA
        {
            get { return _ICAO_Code_USA; }
            set { _ICAO_Code_USA = value; }
        }

        private string _Continuation_Record_No;
        [XmlElement]
        public string Continuation_Record_No
        {
            get { return _Continuation_Record_No; }
            set { _Continuation_Record_No = value; }
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

        public ARINC_Navaid_NDB_Continuation_Record()
        {
            //throw new System.NotImplementedException();
        }


    }

    [XmlType]
    [Serializable()]
    public class ARINC_Navaid_NDB_Simulation_Continuation_Record : ARINC_Navaid
    {
        private string _Navaid_Type;
        [XmlElement]
        public string Navaid_Type
        {
            get { return _Navaid_Type; }
            set { _Navaid_Type = value; }
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

        private string _NDB_Identifier;
        [XmlElement]
        public string NDB_Identifier
        {
            get { return _NDB_Identifier; }
            set { _NDB_Identifier = value; }
        }

        private string _Blank;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank
        {
            get { return _Blank; }
            set { _Blank = value; }
        }

        private string _ICAO_Code_USA;
        [XmlElement]
        public string ICAO_Code_USA
        {
            get { return _ICAO_Code_USA; }
            set { _ICAO_Code_USA = value; }
        }

        private string _Continuation_Record_No;
        [XmlElement]
        public string Continuation_Record_No
        {
            get { return _Continuation_Record_No; }
            set { _Continuation_Record_No = value; }
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

        private string _Application_Type;
        [XmlElement]
        public string Application_Type
        {
            get { return _Application_Type; }
            set { _Application_Type = value; }
        }

        private string _Blank_Spacing;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_Spacing
        {
            get { return _Blank_Spacing; }
            set { _Blank_Spacing = value; }
        }

        private string _Facility_Characteristics;
        [XmlElement]
        public string Facility_Characteristics
        {
            get { return _Facility_Characteristics; }
            set { _Facility_Characteristics = value; }
        }

        private string _Reserved_Spacing;
        [XmlElement]
        public string Reserved_Spacing
        {
            get { return _Reserved_Spacing; }
            set { _Reserved_Spacing = value; }
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

        public ARINC_Navaid_NDB_Simulation_Continuation_Record()
        {

        }



    }

    [XmlType]
    [Serializable()]
    public class ARINC_Navaid_NDB_Flight_Planing_Continuation_Record : ARINC_Navaid
    {

        private string _Navaid_Type;
        [XmlElement]
        public string Navaid_Type
        {
            get { return _Navaid_Type; }
            set { _Navaid_Type = value; }
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

        private string _NDB_Identifier;
        [XmlElement]
        public string NDB_Identifier
        {
            get { return _NDB_Identifier; }
            set { _NDB_Identifier = value; }
        }

        private string _Blank;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank
        {
            get { return _Blank; }
            set { _Blank = value; }
        }

        private string _ICAO_Code_USA;
        [XmlElement]
        public string ICAO_Code_USA
        {
            get { return _ICAO_Code_USA; }
            set { _ICAO_Code_USA = value; }
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

        public ARINC_Navaid_NDB_Flight_Planing_Continuation_Record()
        {
            //throw new System.NotImplementedException();
        }

    }

    [XmlType]
    [Serializable()]
    public class ARINC_Navaid_NDB_Limitation_Continuation_Record : ARINC_Navaid
    {
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

        private string _NDB_Identifier;
        [XmlElement]
        public string NDB_Identifier
        {
            get { return _NDB_Identifier; }
            set { _NDB_Identifier = value; }
        }

        private string _Blank;
        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank
        {
            get { return _Blank; }
            set { _Blank = value; }
        }

        private string _ICAO_Code_USA;
        [XmlElement]
        public string ICAO_Code_USA
        {
            get { return _ICAO_Code_USA; }
            set { _ICAO_Code_USA = value; }
        }

        private string _Continuation_Record_No;
        [XmlElement]
        public string Continuation_Record_No
        {
            get { return _Continuation_Record_No; }
            set { _Continuation_Record_No = value; }
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

        private string _Reserved;

        [XmlElement]
        public string Reserved
        {
            get { return _Reserved; }
            set { _Reserved = value; }
        }
        private string _Navaid_Limitation_Code;

        [XmlElement]
        public string Navaid_Limitation_Code
        {
            get { return _Navaid_Limitation_Code; }
            set { _Navaid_Limitation_Code = value; }
        }
        private string _Component_Affected_Indicator;

        [XmlElement]
        public string Component_Affected_Indicator
        {
            get { return _Component_Affected_Indicator; }
            set { _Component_Affected_Indicator = value; }
        }
        private string _Sequence_Number;

        [XmlElement]
        public string Sequence_Number
        {
            get { return _Sequence_Number; }
            set { _Sequence_Number = value; }
        }

        private string _Sector_From_Sector_To1;

        [XmlElement]
        public string Sector_From_Sector_To1
        {
            get { return _Sector_From_Sector_To1; }
            set { _Sector_From_Sector_To1 = value; }
        }
        private string _Distance_Description1;

        [XmlElement]
        public string Distance_Description1
        {
            get { return _Distance_Description1; }
            set { _Distance_Description1 = value; }
        }
        private string _Distance_Limitation1;

        [XmlElement]
        public string Distance_Limitation1
        {
            get { return _Distance_Limitation1; }
            set { _Distance_Limitation1 = value; }
        }
        private string _Altitude_Description1;

        [XmlElement]
        public string Altitude_Description1
        {
            get { return _Altitude_Description1; }
            set { _Altitude_Description1 = value; }
        }
        private string _Altitude_Limitation1;

        [XmlElement]
        public string Altitude_Limitation1
        {
            get { return _Altitude_Limitation1; }
            set { _Altitude_Limitation1 = value; }
        }

        private string _Sector_From_Sector_To2;

        [XmlElement]
        public string Sector_From_Sector_To2
        {
            get { return _Sector_From_Sector_To2; }
            set { _Sector_From_Sector_To2 = value; }
        }
        private string _Distance_Description2;

        [XmlElement]
        public string Distance_Description2
        {
            get { return _Distance_Description2; }
            set { _Distance_Description2 = value; }
        }
        private string _Distance_Limitation2;

        [XmlElement]
        public string Distance_Limitation2
        {
            get { return _Distance_Limitation2; }
            set { _Distance_Limitation2 = value; }
        }
        private string _Altitude_Description2;

        [XmlElement]
        public string Altitude_Description2
        {
            get { return _Altitude_Description2; }
            set { _Altitude_Description2 = value; }
        }
        private string _Altitude_Limitation2;

        [XmlElement]
        public string Altitude_Limitation2
        {
            get { return _Altitude_Limitation2; }
            set { _Altitude_Limitation2 = value; }
        }

        private string _Sector_From_Sector_To3;

        [XmlElement]
        public string Sector_From_Sector_To3
        {
            get { return _Sector_From_Sector_To3; }
            set { _Sector_From_Sector_To3 = value; }
        }
        private string _Distance_Description3;

        [XmlElement]
        public string Distance_Description3
        {
            get { return _Distance_Description3; }
            set { _Distance_Description3 = value; }
        }
        private string _Distance_Limitation3;

        [XmlElement]
        public string Distance_Limitation3
        {
            get { return _Distance_Limitation3; }
            set { _Distance_Limitation3 = value; }
        }
        private string _Altitude_Description3;

        [XmlElement]
        public string Altitude_Description3
        {
            get { return _Altitude_Description3; }
            set { _Altitude_Description3 = value; }
        }
        private string _Altitude_Limitation3;

        [XmlElement]
        public string Altitude_Limitation3
        {
            get { return _Altitude_Limitation3; }
            set { _Altitude_Limitation3 = value; }
        }

        private string _Sector_From_Sector_To4;

        [XmlElement]
        public string Sector_From_Sector_To4
        {
            get { return _Sector_From_Sector_To4; }
            set { _Sector_From_Sector_To4 = value; }
        }
        private string _Distance_Description4;

        [XmlElement]
        public string Distance_Description4
        {
            get { return _Distance_Description4; }
            set { _Distance_Description4 = value; }
        }
        private string _Distance_Limitation4;

        [XmlElement]
        public string Distance_Limitation4
        {
            get { return _Distance_Limitation4; }
            set { _Distance_Limitation4 = value; }
        }
        private string _Altitude_Description4;

        [XmlElement]
        public string Altitude_Description4
        {
            get { return _Altitude_Description4; }
            set { _Altitude_Description4 = value; }
        }
        private string _Altitude_Limitation4;

        [XmlElement]
        public string Altitude_Limitation4
        {
            get { return _Altitude_Limitation4; }
            set { _Altitude_Limitation4 = value; }
        }

        private string _Sector_From_Sector_To5;

        [XmlElement]
        public string Sector_From_Sector_To5
        {
            get { return _Sector_From_Sector_To5; }
            set { _Sector_From_Sector_To5 = value; }
        }
        private string _Distance_Description5;

        [XmlElement]
        public string Distance_Description5
        {
            get { return _Distance_Description5; }
            set { _Distance_Description5 = value; }
        }
        private string _Distance_Limitation5;

        [XmlElement]
        public string Distance_Limitation5
        {
            get { return _Distance_Limitation5; }
            set { _Distance_Limitation5 = value; }
        }
        private string _Altitude_Description5;

        [XmlElement]
        public string Altitude_Description5
        {
            get { return _Altitude_Description5; }
            set { _Altitude_Description5 = value; }
        }
        private string _Altitude_Limitation5;

        [XmlElement]
        public string Altitude_Limitation5
        {
            get { return _Altitude_Limitation5; }
            set { _Altitude_Limitation5 = value; }
        }

        private string _Sequence_End_Indicator;

        [XmlElement]
        public string Sequence_End_Indicator
        {
            get { return _Sequence_End_Indicator; }
            set { _Sequence_End_Indicator = value; }
        }
        private string _Blank_spacing;

        [XmlElement]
        [System.ComponentModel.Browsable(false)]
        public string Blank_spacing
        {
            get { return _Blank_spacing; }
            set { _Blank_spacing = value; }
        }

        public ARINC_Navaid_NDB_Limitation_Continuation_Record()
        {
            //throw new System.NotImplementedException();
        }

    }


}
