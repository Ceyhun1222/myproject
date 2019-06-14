using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
using ESRI.ArcGIS.Geodatabase;
using System.Xml.Serialization;
using System.Drawing.Design;
using System.Windows.Forms.Design;

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
    public class InstrumentApproachProcedure : Procedure
    {

        private string _ID_MasterProc;
        [Browsable(false)]
        public string ID_MasterProc
        {
            get { return _ID_MasterProc; }
            set { _ID_MasterProc = value; }
        }

        private CodeApproachPrefix _approachPrefix;

        public CodeApproachPrefix ApproachPrefix
        {
            get { return _approachPrefix; }
            set { _approachPrefix = value; }
        }
        private ApproachType _approachType;

        public ApproachType ApproachType
        {
            get { return _approachType; }
            set { _approachType = value; }
        }
        private string _multipleIdentification;

        public string MultipleIdentification
        {
            get { return _multipleIdentification; }
            set { _multipleIdentification = value; }
        }
        private double? _copterTrack = null;

        public double? CopterTrack
        {
            get { return _copterTrack; }
            set { _copterTrack = value; }
        }
        private string _circlingIdentification;

        public string CirclingIdentification
        {
            get { return _circlingIdentification; }
            set { _circlingIdentification = value; }
        }
        private string _courseReversalInstruction;

        public string CourseReversalInstruction
        {
            get { return _courseReversalInstruction; }
            set { _courseReversalInstruction = value; }
        }
        private CodeApproachEquipmentAdditional _additionalEquipment;

        public CodeApproachEquipmentAdditional AdditionalEquipment
        {
            get { return _additionalEquipment; }
            set { _additionalEquipment = value; }
        }
        private double? _channelGNSS = null;

        public double? ChannelGNSS
        {
            get { return _channelGNSS; }
            set { _channelGNSS = value; }
        }
        private bool _WAASReliable;

        public bool WAASReliable
        {
            get { return _WAASReliable; }
            set { _WAASReliable = value; }
        }

        private string _missedInstruction;
        public string MissedInstruction
        {
            get { return _missedInstruction; }
            set { _missedInstruction = value; }
        }

        private FinalProfile _Profile;
        [Editor(typeof(ApproachProfileEditor), typeof(UITypeEditor))]
        public FinalProfile Profile
        {
            get { return _Profile; }
            set { _Profile = value; }
        }

        [Browsable(false)]
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.InstrumentApproachProcedure;
            }
        } 

        public InstrumentApproachProcedure()
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
            findx = row.Fields.FindField("approachPrefix"); if (findx >= 0) row.set_Value(findx, this.ApproachPrefix.ToString());
            findx = row.Fields.FindField("approachType"); if (findx >= 0) row.set_Value(findx, this.ApproachType.ToString());
            findx = row.Fields.FindField("multipleIdentification"); if (findx >= 0) row.set_Value(findx, this.MultipleIdentification);
            findx = row.Fields.FindField("copterTrack"); if (findx >= 0) row.set_Value(findx, this.CopterTrack);
            findx = row.Fields.FindField("circlingIdentification"); if (findx >= 0) row.set_Value(findx, this.CirclingIdentification);
            findx = row.Fields.FindField("courseReversalInstruction"); if (findx >= 0) row.set_Value(findx, this.CourseReversalInstruction);
            findx = row.Fields.FindField("additionalEquipment"); if (findx >= 0) row.set_Value(findx, this.AdditionalEquipment);
            findx = row.Fields.FindField("channelGNSS"); if (findx >= 0) row.set_Value(findx, this.ChannelGNSS);
            findx = row.Fields.FindField("WAASReliable"); if (findx >= 0) row.set_Value(findx, this.WAASReliable);
            findx = row.Fields.FindField("MasterProcID"); if (findx >= 0) row.set_Value(findx, this.ID);

            //findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);

            row.Store();
        }

        public override string ToString()
        {
            return this.Airport_ICAO_Code + " " + this.ProcedureIdentifier;
        }

    }

    [Serializable()]
    public class ApproachAltitude
    {
        private double? _altitude;
        public double? Altitude
        {
            get { return _altitude; }
            set { _altitude = value; }
        }

        private UOM_DIST_VERT _altitudeUOM;

        public UOM_DIST_VERT AltitudeUOM
        {
            get { return _altitudeUOM; }
            set { _altitudeUOM = value; }
        }

        private CodeProcedureDistance _measurementPoint;
        public CodeProcedureDistance MeasurementPoint
        {
            get { return _measurementPoint; }
            set { _measurementPoint = value; }
        }

        private CODE_DIST_VER _altitudeReference;
        public CODE_DIST_VER AltitudeReference
        {
            get { return _altitudeReference; }
            set { _altitudeReference = value; }
        }

        public ApproachAltitude()
        {
        }

    }

    [Serializable()]
    public class ApproachDistance
    {
        private CodeProcedureDistance _startingMeasurementPoint;
        public CodeProcedureDistance StartingMeasurementPoint
        {
            get { return _startingMeasurementPoint; }
            set { _startingMeasurementPoint = value; }
        }

        private double? _valueHAT;
        public double? ValueHAT
        {
            get { return _valueHAT; }
            set { _valueHAT = value; }
        }

        private UOM_DIST_VERT _valueHATUOM;
        public UOM_DIST_VERT ValueHATUOM
        {
            get { return _valueHATUOM; }
            set { _valueHATUOM = value; }
        }

        private CodeProcedureDistance _endingMeasurementPoint;
        public CodeProcedureDistance EndingMeasurementPoint
        {
            get { return _endingMeasurementPoint; }
            set { _endingMeasurementPoint = value; }
        }

        private double? _distance;
        public double? Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        private UOM_DIST_HORZ _distanceUOM;
        public UOM_DIST_HORZ DistanceUOM
        {
            get { return _distanceUOM; }
            set { _distanceUOM = value; }
        }

        public ApproachDistance()
        {
        }

    }

    [Serializable()]
    public class ApproachTiming
    {
        private CodeProcedureDistance _startingMeasurementPoint;
        public CodeProcedureDistance StartingMeasurementPoint
        {
            get { return _startingMeasurementPoint; }
            set { _startingMeasurementPoint = value; }
        }

        private CodeProcedureDistance _endingMeasurementPoint;
        public CodeProcedureDistance EndingMeasurementPoint
        {
            get { return _endingMeasurementPoint; }
            set { _endingMeasurementPoint = value; }
        }

        private double _time;
        public double Time
        {
            get { return _time; }
            set { _time = value; }
        }

        private DurationType _timeDuration;
        public DurationType TimeDuration
        {
            get { return _timeDuration; }
            set { _timeDuration = value; }
        }

        private double? _speed;
        public double? Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private SpeedType _speedUOM;
        public SpeedType SpeedUOM
        {
            get { return _speedUOM; }
            set { _speedUOM = value; }
        }

        public ApproachTiming()
        {
        }
    }

    [Serializable()]
    public class ApproachMinima
    {
        private double _minima;
        public double Minima { get => _minima; set => _minima = value; }

        private UOM_DIST_VERT _minimaUom;
        public UOM_DIST_VERT MinimaUom { get => _minimaUom; set => _minimaUom = value; }

        private string _profileSegmnetDesignator;
        public string ProfileSegmnetDesignator { get => _profileSegmnetDesignator; set => _profileSegmnetDesignator = value; }

        public ApproachMinima()
        {
        }

    }

    [Serializable()]
    public class FinalProfile
    {
        public FinalProfile()
        {
        }

        private List<ApproachAltitude> _ApproachAltitudeTable;
        public List<ApproachAltitude> ApproachAltitudeTable
        {
            get { return _ApproachAltitudeTable; }
            set { _ApproachAltitudeTable = value; }
        }

        private List<ApproachDistance> _ApproachDistancetable;
        public List<ApproachDistance> ApproachDistancetable
        {
            get { return _ApproachDistancetable; }
            set { _ApproachDistancetable = value; }
        }

        private List<ApproachTiming> _ApproachTimingTable;
        public List<ApproachTiming> ApproachTimingTable
        {
            get { return _ApproachTimingTable; }
            set { _ApproachTimingTable = value; }
        }

        private List<ApproachMinima> _ApproachMinimaTable;
        public List<ApproachMinima> ApproachMinimaTable { get => _ApproachMinimaTable; set => _ApproachMinimaTable = value; }





    }


    public class ApproachProfileEditor : UITypeEditor
    {

        public override System.Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, System.Object value)
        {
            if ((context != null) && (provider != null))
            {

                IWindowsFormsEditorService svc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (svc != null)
                {


                    using (ApproachProfileEditorForm frm = new ApproachProfileEditorForm())
                    {
                        System.Object OldVal = new System.Object();
                        OldVal = value;


                        if (context.Instance is InstrumentApproachProcedure)
                        {
                            if (((InstrumentApproachProcedure)context.Instance).Profile.ApproachAltitudeTable != null)
                            {
                                frm.dataGridView1.DataSource = ((InstrumentApproachProcedure)context.Instance).Profile.ApproachAltitudeTable;
                            }
                            else
                                frm.dataGridView1.DataSource = null;

                            if (((InstrumentApproachProcedure)context.Instance).Profile.ApproachDistancetable != null)
                            {
                                frm.dataGridView2.DataSource = ((InstrumentApproachProcedure)context.Instance).Profile.ApproachDistancetable;
                            }
                            else
                                frm.dataGridView2.DataSource = null;

                            if (((InstrumentApproachProcedure)context.Instance).Profile.ApproachTimingTable != null)
                                frm.dataGridView3.DataSource = ((InstrumentApproachProcedure)context.Instance).Profile.ApproachTimingTable;
                            else
                                frm.dataGridView3.DataSource = null;

                            if (((InstrumentApproachProcedure)context.Instance).Profile.ApproachMinimaTable != null)
                                frm.dataGridView4.DataSource = ((InstrumentApproachProcedure)context.Instance).Profile.ApproachMinimaTable;
                            else
                                frm.dataGridView4.DataSource = null;
                        }

                        if (svc.ShowDialog(frm) == System.Windows.Forms.DialogResult.OK)
                        {
                        }
                        else
                        {
                            value = OldVal;
                        }

                    }


                }
            }


            return base.EditValue(context, provider, value);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null)
                return UITypeEditorEditStyle.Modal;
            else
                return base.GetEditStyle(context);
        }



    }

}
