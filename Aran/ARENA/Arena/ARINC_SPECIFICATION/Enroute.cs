using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ARINC_Types
{
    [XmlType]
    [Serializable()]
    public class ARINC_Enroute_Airways_Primary_Record : ARINC_OBJECT
    {

    //private string _Subsection_Code;
    //[XmlElement]
    //public  string Subsection_Code
    //{
    //   get { return _Subsection_Code; }
    //   set { _Subsection_Code=value; }
    //}

    private string _Blank_Spacing;
    [XmlElement]
    public string Blank_Spacing
    {
        get { return _Blank_Spacing; }
        set { _Blank_Spacing = value; }
    }

    private string _Route_Identifier;
    [XmlElement]
    public  string Route_Identifier
    {
       get { return _Route_Identifier; }
       set { _Route_Identifier=value; }
    }

    private string _Reserved_1;
    [XmlElement]
    public  string Reserved_1
    {
       get { return _Reserved_1; }
       set { _Reserved_1=value; }
    }

    private string _Blank_Spacing1;
    [XmlElement]
    public  string Blank_Spacing1
    {
       get { return _Blank_Spacing1; }
       set { _Blank_Spacing1=value; }
    }

    private string _Sequence_Number;
    [XmlElement]
    public  string Sequence_Number
    {
       get { return _Sequence_Number; }
       set { _Sequence_Number=value; }
    }

    private string _Fix_Identifier;
    [XmlElement]
    public  string Fix_Identifier
    {
       get { return _Fix_Identifier; }
       set { _Fix_Identifier=value; }
    }

    private string _ICAO_Code1;
    [XmlElement]
    public  string ICAO_Code1
    {
       get { return _ICAO_Code1; }
       set { _ICAO_Code1=value; }
    }

    private string _Section_Code;
    [XmlElement]
    public  string Section_Code
    {
       get { return _Section_Code; }
       set { _Section_Code=value; }
    }

    private string _Subsection;
    [XmlElement]
    public  string Subsection
    {
       get { return _Subsection; }
       set { _Subsection=value; }
    }

    private string _Continuation_Record_No;
    [XmlElement]
    public  string Continuation_Record_No
    {
       get { return _Continuation_Record_No; }
       set { _Continuation_Record_No=value; }
    }

    private string _Waypoint_Description_Code;
    [XmlElement]
    public  string Waypoint_Description_Code
    {
       get { return _Waypoint_Description_Code; }
       set { _Waypoint_Description_Code=value; }
    }

    private string _Boundary_Code;
    [XmlElement]
    public  string Boundary_Code
    {
       get { return _Boundary_Code; }
       set { _Boundary_Code=value; }
    }

    private string _Route_Type;
    [XmlElement]
    public  string Route_Type
    {
       get { return _Route_Type; }
       set { _Route_Type=value; }
    }

    private string _Level;
    [XmlElement]
    public  string Level
    {
       get { return _Level; }
       set { _Level=value; }
    }

    private string _Direction_Restriction;
    [XmlElement]
    public  string Direction_Restriction
    {
       get { return _Direction_Restriction; }
       set { _Direction_Restriction=value; }
    }

    private string _Cruise_Table_Indicator;
    [XmlElement]
    public  string Cruise_Table_Indicator
    {
       get { return _Cruise_Table_Indicator; }
       set { _Cruise_Table_Indicator=value; }
    }

    private string _EU_Indicator;
    [XmlElement]
    public  string EU_Indicator
    {
       get { return _EU_Indicator; }
       set { _EU_Indicator=value; }
    }

    private string _Recommended_NAVAID;
    [XmlElement]
    public  string Recommended_NAVAID
    {
       get { return _Recommended_NAVAID; }
       set { _Recommended_NAVAID=value; }
    }

    private string _ICAO_Code2;
    [XmlElement]
    public  string ICAO_Code2
    {
       get { return _ICAO_Code2; }
       set { _ICAO_Code2=value; }
    }

    private string _RNP;
    [XmlElement]
    public  string RNP
    {
       get { return _RNP; }
       set { _RNP=value; }
    }

    private string _Blank_Spacing2;
    [XmlElement]
    public  string Blank_Spacing2
    {
       get { return _Blank_Spacing2; }
       set { _Blank_Spacing2=value; }
    }

    private string _Theta;
    [XmlElement]
    public  string Theta
    {
       get { return _Theta; }
       set { _Theta=value; }
    }

    private string _Rho;
    [XmlElement]
    public  string Rho
    {
       get { return _Rho; }
       set { _Rho=value; }
    }

    private string _Outbound_Magnetic_Course;
    [XmlElement]
    public  string Outbound_Magnetic_Course
    {
       get { return _Outbound_Magnetic_Course; }
       set { _Outbound_Magnetic_Course=value; }
    }

    private string _Route_Distance_From;
    [XmlElement]
    public  string Route_Distance_From
    {
       get { return _Route_Distance_From; }
       set { _Route_Distance_From=value; }
    }

    private string _Inbound_Magnetic_Course;
    [XmlElement]
    public  string Inbound_Magnetic_Course
    {
       get { return _Inbound_Magnetic_Course; }
       set { _Inbound_Magnetic_Course=value; }
    }

    private string _Blank_Spacing3;
    [XmlElement]
    public  string Blank_Spacing3
    {
       get { return _Blank_Spacing3; }
       set { _Blank_Spacing3=value; }
    }

    private string _Minimum_Altitude1;
    [XmlElement]
    public  string Minimum_Altitude1
    {
       get { return _Minimum_Altitude1; }
       set { _Minimum_Altitude1=value; }
    }

    private string _Minimum_Altitude2;
    [XmlElement]
    public  string Minimum_Altitude2
    {
       get { return _Minimum_Altitude2; }
       set { _Minimum_Altitude2=value; }
    }

    private string _Maximum_Altitude;
    [XmlElement]
    public  string Maximum_Altitude
    {
       get { return _Maximum_Altitude; }
       set { _Maximum_Altitude=value; }
    }

    private string _Reserved_Expansion;
    [XmlElement]
    public  string Reserved_Expansion
    {
       get { return _Reserved_Expansion; }
       set { _Reserved_Expansion=value; }
    }

    private string _File_Record_No;
    [XmlElement]
    public  string File_Record_No
    {
       get { return _File_Record_No; }
       set { _File_Record_No=value; }
    }

    private string _Cycle_Date;
    [XmlElement]
    public  string Cycle_Date
    {
       get { return _Cycle_Date; }
       set { _Cycle_Date=value; }
    }

    private List<ARINC_Enroute_Airways_Continuation_Record> _ARINC_Enroute_Airways_Continuation_Record;
    [XmlElement]
    [System.ComponentModel.Browsable(false)]
    public List<ARINC_Enroute_Airways_Continuation_Record> ARINC_Enroute_Airways_Continuation_Record
    {
        get { return _ARINC_Enroute_Airways_Continuation_Record; }
        set { _ARINC_Enroute_Airways_Continuation_Record = value; }
    }
     
    

    public ARINC_Enroute_Airways_Primary_Record()
    {

    }

    }


    [XmlType]
    [Serializable()]
    public class ARINC_Enroute_Airways_Continuation_Record : ARINC_OBJECT
    {
        private string _Blank_Spacing;
        [XmlElement]
        public string Blank_Spacing
        {
            get { return _Blank_Spacing1; }
            set { _Blank_Spacing1 = value; }
        }

        private string _Route_Identifier;
        [XmlElement]
        public string Route_Identifier
        {
            get { return _Route_Identifier; }
            set { _Route_Identifier = value; }
        }

        private string _Reserved_1;
        [XmlElement]
        public string Reserved_1
        {
            get { return _Reserved_1; }
            set { _Reserved_1 = value; }
        }

        private string _Blank_Spacing1;
        [XmlElement]
        public string Blank_Spacing1
        {
            get { return _Blank_Spacing1; }
            set { _Blank_Spacing1 = value; }
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

        private string _Section_Code;
        [XmlElement]
        public string Section_Code
        {
            get { return _Section_Code; }
            set { _Section_Code = value; }
        }

        private string _Subsection;
        [XmlElement]
        public string Subsection
        {
            get { return _Subsection; }
            set { _Subsection = value; }
        }

        private string _Continuation_Record_No;
        [XmlElement]
        public string Continuation_Record_No
        {
            get { return _Continuation_Record_No; }
            set { _Continuation_Record_No = value; }
        }

        private string _Notes;
        [XmlElement]
        public string Notes
        {
            get { return _Notes; }
            set { _Notes = value; }
        }

        private string _Reserved;
        [XmlElement]
        public string Reserved
        {
          get { return _Reserved; }
          set { _Reserved = value; }
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
    }

}
