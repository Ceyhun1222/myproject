using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
//using PDM;

namespace PDM
{
    [XmlInclude(typeof(PDMObject))]
    [XmlInclude(typeof(AirportHeliport))]
    [XmlInclude(typeof(Runway))]
    [XmlInclude(typeof(RunwayDirection))]
    [XmlInclude(typeof(RunwayCenterLinePoint))]
    [XmlInclude(typeof(DeclaredDistance))]

    [XmlInclude(typeof(NavaidSystem))]
    [XmlInclude(typeof(NavaidComponent))]
    [XmlInclude(typeof(Localizer))]
    [XmlInclude(typeof(GlidePath))]
    [XmlInclude(typeof(VOR))]
    [XmlInclude(typeof(DME))]
    [XmlInclude(typeof(TACAN))]
    [XmlInclude(typeof(NDB))]
    [XmlInclude(typeof(WayPoint))]
    [XmlInclude(typeof(Marker))]

    [XmlInclude(typeof(InstrumentApproachProcedure))]
    [XmlInclude(typeof(StandardInstrumentArrival))]
    [XmlInclude(typeof(StandardInstrumentDeparture))]
    [XmlInclude(typeof(AircraftCharacteristic))]
    [XmlInclude(typeof(Procedure))]

    [XmlInclude(typeof(ProcedureTransitions))]

    [XmlInclude(typeof(FinalLeg))]
    [XmlInclude(typeof(MissaedApproachLeg))]
    [XmlInclude(typeof(ProcedureLeg))]

    [XmlInclude(typeof(ObstacleAssessmentArea))]
    [XmlInclude(typeof(Obstruction))]
    [XmlInclude(typeof(ApproachCondition))]

    [XmlInclude(typeof(SegmentPoint))]
    [XmlInclude(typeof(FacilityMakeUp))]
    [XmlInclude(typeof(DistanceIndication))]
    [XmlInclude(typeof(AngleIndication))]
    
    [XmlInclude(typeof(Enroute))]
    [XmlInclude(typeof(RouteSegment))]
    [XmlInclude(typeof(RouteSegmentPoint))]

    [XmlInclude(typeof(AirspaceVolume))]
    [XmlInclude(typeof(Airspace))]

    [XmlInclude(typeof(VerticalStructure))]
    [XmlInclude(typeof(VerticalStructurePart))]

    [XmlInclude(typeof(AREA_PDM))]

    //[XmlInclude(typeof(ARINC_Navaid_VHF_Primary_Record))]
    //[XmlInclude(typeof(ARINC_Navaid_NDB_Primary_Record))]
    //[XmlInclude(typeof(ARINC_LocalizerGlideSlope_Primary_Record))]
    //[XmlInclude(typeof(ARINC_Airport_Primary_Record))]
    //[XmlInclude(typeof(ARINC_Runway_Primary_Records))]
    //[XmlInclude(typeof(ARINC_WayPoint_Primary_Record))]
    //[XmlInclude(typeof(ARINC_Enroute_Airways_Primary_Record))]
    //[XmlInclude(typeof(ARINC_FIR_UIR_Primary_Records))]
    //[XmlInclude(typeof(ARINC_Controlled_Airspace_Primary_Records))]
    //[XmlInclude(typeof(ARINC_Restrictive_Airspace_Primary_Records))]
    //[XmlInclude(typeof(ARINC_Terminal_Procedure_Primary_Record))]
    //[XmlInclude(typeof(ARINC_Airport_Marker))]


    [XmlType]
    [Serializable()]
    public class PDM_ObjectsList
    {
        private string _date;
        [XmlElement]
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private List<PDMObject> _PDMObject_list;
        [XmlElement]
        public List<PDMObject> PDMObject_list
        {
            get { return _PDMObject_list; }
            set { _PDMObject_list = value; }
        }

        private string _projectType;
        [XmlElement]
        public string ProjectType
        {
            get { return _projectType; }
            set { _projectType = value; }
        }

        private List<string> _filterList;
        [XmlElement]
        public List<string> FilterList
        {
            get { return _filterList; }
            set { _filterList = value; }
        }
        

        public PDM_ObjectsList()
        {
            this.Date = DateTime.Now.ToLongDateString();
            this.PDMObject_list = new List<PDMObject>();
            this.ProjectType = ArenaProjectType.ARENA.ToString();
        }

        public PDM_ObjectsList(List<PDMObject> objects,ArenaProjectType prjType)
        {
            this.Date = DateTime.Now.ToLongDateString();
            this.PDMObject_list = new List<PDMObject>();
            this.PDMObject_list.AddRange(objects.GetRange(0,objects.Count));
            this.ProjectType = prjType.ToString();
        }

    }

    public enum ArenaProjectType
    {
        ARENA = 0,
        NOTAM = 1,
        PANDA =2,
    }


}
