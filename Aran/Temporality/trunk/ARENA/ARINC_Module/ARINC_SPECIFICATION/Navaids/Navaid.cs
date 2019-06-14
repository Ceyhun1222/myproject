using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ARINC_Types
{

    [XmlType]
    [Serializable()]
    public class ARINC_Navaid : ARINC_OBJECT
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

        private string _Navaid_Identifier;
        [XmlElement]
        public string Navaid_Identifier
        {
            get { return _Navaid_Identifier; }
            set { _Navaid_Identifier = value; }
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


        public ARINC_Navaid()
        {

        }

    }

}
