using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ESRI.ArcGIS.Geometry;

//using PDM;

namespace PDM
{
    [XmlInclude(typeof(PDMObject))]
    [XmlInclude(typeof(AirportHeliport))]
    [XmlInclude(typeof(Runway))]
    [XmlInclude(typeof(RunwayDirection))]
    [XmlInclude(typeof(RunwayCenterLinePoint))]
    [XmlInclude(typeof(DeclaredDistance))]
    [XmlInclude(typeof(RunwayElement))]
    [XmlInclude(typeof(Taxiway))]
    [XmlInclude(typeof(TaxiwayElement))]
    [XmlInclude(typeof(LightSystem))]
    [XmlInclude(typeof(LightElement))]

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

    [XmlInclude(typeof(FinalProfile))]
    [XmlInclude(typeof(ApproachAltitude))]
    [XmlInclude(typeof(ApproachDistance))]
    [XmlInclude(typeof(ApproachTiming))]
    [XmlInclude(typeof(ApproachMinima))]

    [XmlInclude(typeof(Enroute))]
    [XmlInclude(typeof(RouteSegment))]
    [XmlInclude(typeof(RouteSegmentPoint))]

    [XmlInclude(typeof(AirspaceVolume))]
    [XmlInclude(typeof(Airspace))]
    [XmlInclude(typeof(AirspaceClass))]
    [XmlInclude(typeof(AssociatedLevels))]

    [XmlInclude(typeof(VerticalStructure))]
    [XmlInclude(typeof(VerticalStructurePart))]

    [XmlInclude(typeof(HoldingPattern))]

    [XmlInclude(typeof(SafeAltitudeArea))]
    [XmlInclude(typeof(SafeAltitudeAreaSector))]

    [XmlInclude(typeof(GeoBorder))]

    [XmlInclude(typeof(AREA_PDM))]

    [XmlInclude(typeof(RadioCommunicationChanel))]
    
    [XmlInclude(typeof(AircraftStand))]
    [XmlInclude(typeof(AirportHotSpot))]
    [XmlInclude(typeof(Marking))]
    [XmlInclude(typeof(Apron))]
    [XmlInclude(typeof(ApronElement))]
    [XmlInclude(typeof(ApronLightSystem))]
    [XmlInclude(typeof(ApronMarking))]
    [XmlInclude(typeof(DeicingArea))]
    [XmlInclude(typeof(DeicingAreaMarking))]
    [XmlInclude(typeof(GuidanceLine))]
    [XmlInclude(typeof(GuidanceLineLightSystem))]
    [XmlInclude(typeof(GuidanceLineMarking))]
    [XmlInclude(typeof(RunwayMarking))]
    [XmlInclude(typeof(RunwayProtectArea))]
    [XmlInclude(typeof(RunwayProtectAreaLightSystem))]
    [XmlInclude(typeof(RunwayVisualRange))]
    [XmlInclude(typeof(StandMarking))]
    [XmlInclude(typeof(SurfaceCharacteristics))]
    [XmlInclude(typeof(TaxiHoldingPosition))]
    [XmlInclude(typeof(TaxiHoldingPositionLightSystem))]
    [XmlInclude(typeof(TaxiHoldingPositionMarking))]
    [XmlInclude(typeof(TaxiwayLightSystem))]
    [XmlInclude(typeof(TaxiwayMarking))]
    [XmlInclude(typeof(TouchDownLiftOff))]
    [XmlInclude(typeof(TouchDownLiftOffLightSystem))]
    [XmlInclude(typeof(TouchDownLiftOffMarking))]
    [XmlInclude(typeof(TouchDownLiftOffSafeArea))]
    [XmlInclude(typeof(VisualGlideSlopeIndicator))]
    [XmlInclude(typeof(WorkArea))]
    [XmlInclude(typeof(ApproachLightingSystem))]
    [XmlInclude(typeof(NavigationSystemCheckpoint))]
    [XmlInclude(typeof(CheckpointINS))]
    [XmlInclude(typeof(CheckpointVOR))]
    [XmlInclude(typeof(MarkingElement))]
    [XmlInclude(typeof(Marking_Point))]
    [XmlInclude(typeof(Marking_Curve))]
    [XmlInclude(typeof(Marking_Surface))]
    [XmlInclude(typeof(AirportHeliportProtectionArea))]
    [XmlInclude(typeof(AirportHeliportExtent))]
    [XmlInclude(typeof(TouchDownLiftOffAimingPoint))]
    [XmlInclude(typeof(AircraftStandExtent))]
    [XmlInclude(typeof(Road))]
    [XmlInclude(typeof(Unit))]
    [XmlInclude(typeof(NonMovementArea))]
    [XmlInclude(typeof(RunwayStrip))]
    [XmlInclude(typeof(RadioFrequencyArea))]
    [XmlInclude(typeof(VolumeGeometryComponent))]

    [XmlInclude(typeof(FeatureMetadata))]
    [XmlInclude(typeof(GeoProperties))]
    [XmlInclude(typeof(TimePeriod))]

    [XmlType]
    [Serializable()]
    public class PDM_ObjectsList
    {
        private DateTime _date;
        [XmlElement]
        public DateTime Date
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

        public string VersionInfo { get; set; }

        public PDM_ObjectsList()
        {
            this.Date = DateTime.Now;
            this.PDMObject_list = new List<PDMObject>();
            this.ProjectType = "ARENA";
            this.VersionInfo = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public PDM_ObjectsList(List<PDMObject> objects,string prjType)
        {
            this.Date = DateTime.Now;
            this.PDMObject_list = new List<PDMObject>();
            this.PDMObject_list.AddRange(objects.GetRange(0,objects.Count));
            this.ProjectType = prjType;
            this.VersionInfo = this.VersionInfo==null || this.VersionInfo.Length <=0 ?  System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() : this.VersionInfo;
        }

    }


}
