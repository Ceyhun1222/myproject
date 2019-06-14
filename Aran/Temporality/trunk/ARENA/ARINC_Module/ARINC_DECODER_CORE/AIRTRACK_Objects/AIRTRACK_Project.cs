using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    [XmlInclude(typeof(Object_AIRTRACK))]
    [XmlInclude(typeof(AIRPORT_AIRTRACK))]
    [XmlInclude(typeof(ILS_AIRTRACK))]
    [XmlInclude(typeof(RunWay_AIRTRACK))]
    [XmlInclude(typeof(RunWay_THR_AIRTRACK))]
    [XmlInclude(typeof(DME_AIRTRACK))]
    [XmlInclude(typeof(NDB_AIRTRACK))]
    [XmlInclude(typeof(TACAN_AIRTRACK))]
    [XmlInclude(typeof(VHF_NAVAID_AIRTRACK))]
    [XmlInclude(typeof(VOR_AIRTRACK))]
    [XmlInclude(typeof(WayPoint_AIRTRACK))]
    [XmlInclude(typeof(Leg_AIRTRACK))]
    [XmlInclude(typeof(ProcedureBranch_AIRTRACK))]
    [XmlInclude(typeof(AREA_AIRTRACK))]
    [XmlInclude(typeof(Waypoint_Description_Code))]
    [XmlInclude(typeof(Procedure_AIRTRACK))]


    [XmlType]
    [Serializable()]
    public class AIRTRACK_Project
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

        private List<Object_AIRTRACK> _ListOfObjects;
        [XmlElement]
        public List<Object_AIRTRACK> ListOfObjects
        {
            get { return _ListOfObjects; }
            set { _ListOfObjects = value; }
        }

        public AIRTRACK_Project()
        {
            this.DATE = DateTime.Now.ToLongDateString();
            this.ListOfObjects = new List<Object_AIRTRACK>();
        }

        public AIRTRACK_Project(string SourceFileName, List<Object_AIRTRACK> ObjetsList)
        {
            this.DATE = DateTime.Now.ToLongDateString();
            this.SourceFile = SourceFileName;
            this.ListOfObjects = new List<Object_AIRTRACK>();
            this.ListOfObjects = ObjetsList;
        }
    }
}
