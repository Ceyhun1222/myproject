using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
using ESRI.ArcGIS.Geodatabase;
using System.Xml.Serialization;

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
    [Serializable()]
    public class StandardInstrumentArrival : Procedure
    {
        private string _ID_MasterProc;
        [Browsable(false)]
        public string ID_MasterProc
        {
            get { return _ID_MasterProc; }
            set { _ID_MasterProc = value; }
        }

        private string _designator;

        public string Designator
        {
            get { return _designator; }
            set { _designator = value; }
        }

        [Browsable(false)]
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.StandardInstrumentArrival;
            }
        } 

        public StandardInstrumentArrival()
        {
        }

        public override string GetObjectLabel()
        {
            return base.GetObjectLabel();
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("MasterProcID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("designator"); if (findx >= 0) row.set_Value(findx, this.Designator);

            row.Store();
        }

        public override string ToString()
        {
            return this.Airport_ICAO_Code + " " + this.ProcedureIdentifier;
        }
    }
}
