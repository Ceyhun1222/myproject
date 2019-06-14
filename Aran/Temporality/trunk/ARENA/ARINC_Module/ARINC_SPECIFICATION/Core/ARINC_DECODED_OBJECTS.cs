using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace  ARINC_Types
{
    [XmlInclude(typeof(ARINC_OBJECT))]
    [XmlInclude(typeof(ARINC_Navaid))]

    [XmlInclude(typeof(ARINC_Navaid_VHF_Continuation_Record))]
    [XmlInclude(typeof(ARINC_Navaid_VHF_Flight_Planing_Continuation_Record))]
    [XmlInclude(typeof(ARINC_Navaid_VHF_Limitation_Continuation_Record))]
    [XmlInclude(typeof(ARINC_Navaid_VHF_Primary_Record))]
    [XmlInclude(typeof(ARINC_Navaid_VHF_Simulation_Continuation_Record))]

    [XmlInclude(typeof(ARINC_Navaid_NDB_Continuation_Record))]
    [XmlInclude(typeof(ARINC_Navaid_NDB_Flight_Planing_Continuation_Record))]
    [XmlInclude(typeof(ARINC_Navaid_NDB_Limitation_Continuation_Record))]
    [XmlInclude(typeof(ARINC_Navaid_NDB_Primary_Record))]
    [XmlInclude(typeof(ARINC_Navaid_NDB_Simulation_Continuation_Record))]

    [XmlInclude(typeof(ARINC_WayPoint_Primary_Record))]
    [XmlInclude(typeof(ARINC_WayPoint_Continuation_Record))]
    [XmlInclude(typeof(ARINC_WayPoint_Flight_Planing_Continuation_Record))]

    [XmlInclude(typeof(ARINC_Airport_Primary_Record))]
    [XmlInclude(typeof(ARINC_Airport_Continuation_Record))]
    [XmlInclude(typeof(ARINC_Airport_Flight_Planning_Continuation_Records))]

    [XmlInclude(typeof(ARINC_Runway_Primary_Records))]
    [XmlInclude(typeof(ARINC_Runway_Continuation_Records))]
    [XmlInclude(typeof(ARINC_Runway_Simulation_Continuation_Records))]


    [XmlInclude(typeof(ARINC_LocalizerGlideSlope_Primary_Record))]
    [XmlInclude(typeof(ARINC_LocalizerGlideSlope_Continuation_Record))]
    [XmlInclude(typeof(ARINC_LocalizerGlideSlope_Simulation_Continuation_Record))]

    [XmlInclude(typeof(ARINC_Airport_Marker))]

    [XmlInclude(typeof(ARINC_Terminal_Procedure_Primary_Record))]
    [XmlInclude(typeof(ARINC_Terminal_Procedure_Continuation_Record))]
    [XmlInclude(typeof(ARINC_Terminal_Procedure_Flight_Planning_Continuation_Records))]

    [XmlInclude(typeof(ARINC_Enroute_Airways_Primary_Record))]
    [XmlInclude(typeof(ARINC_Enroute_Airways_Continuation_Record))]


    [XmlInclude(typeof(ARINC_FIR_UIR_Primary_Records))]
    [XmlInclude(typeof(ARINC_Restrictive_Airspace_Primary_Records))]
    [XmlInclude(typeof(ARINC_Controlled_Airspace_Primary_Records))]


    [XmlType]
    [Serializable()]
    public class ARINC_DECODED_OBJECTS
    {
        private string _DATE;
        [XmlElement]
        public string DATE
        {
            get { return _DATE; }
            set { _DATE = value; }
        }

        private string _SourceFile;
        [XmlElement]
        public string SourceFile
        {
            get { return _SourceFile; }
            set { _SourceFile = value; }
        }

        private List<ARINC_OBJECT> _ListOfObjects;
        [XmlElement]
        public List<ARINC_OBJECT> ListOfObjects
        {
            get { return _ListOfObjects; }
            set { _ListOfObjects = value; }
        }

        public ARINC_DECODED_OBJECTS()
        {
            this.DATE = DateTime.Now.ToLongDateString();
            this.ListOfObjects = new List<ARINC_OBJECT>();
        }

        public ARINC_DECODED_OBJECTS(string SourceFileName, List<ARINC_OBJECT> ObjetsList)
        {
            this.DATE = DateTime.Now.ToLongDateString();
            this.SourceFile = SourceFileName;
            this.ListOfObjects = new List<ARINC_OBJECT>();
            this.ListOfObjects = ObjetsList;
        }
    }

}
