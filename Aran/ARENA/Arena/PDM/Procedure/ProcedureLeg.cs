using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;

namespace PDM
{
    [Serializable()]
    [TypeConverter(typeof(PropertySorter))]
    public class ProcedureLeg : PDMObject
    {

        private string _ProcedureIdentifier;
        [Browsable(false)]
        public string ProcedureIdentifier
        {
            get { return _ProcedureIdentifier; }
            set { _ProcedureIdentifier = value; }
        }

        private string _TransitionIdentifier;
        [Browsable(false)]
        public string TransitionIdentifier
        {
            get { return _TransitionIdentifier; }
            set { _TransitionIdentifier = value; }
        }

        private int _seqNumberARINC;

        public int SeqNumberARINC
        {
            get { return _seqNumberARINC; }
            set { _seqNumberARINC = value; }
        }

        private AltitudeUseType _altitudeInterpretation;

        public AltitudeUseType AltitudeInterpretation
        {
            get { return _altitudeInterpretation; }
            set { _altitudeInterpretation = value; }
        }

        private double? _altitudeOverrideATC = null;

        public double? AltitudeOverrideATC
        {
            get { return _altitudeOverrideATC; }
            set { _altitudeOverrideATC = value; }
        }

        private UOM_DIST_VERT _altitudeUOM;

        public UOM_DIST_VERT AltitudeUOM
        {
            get { return _altitudeUOM; }
            set { _altitudeUOM = value; }
        }

        private List<AircraftCharacteristic> _aircraftCategory;
        
        public List<AircraftCharacteristic> AircraftCategory
        {
            get { return _aircraftCategory; }
            set { _aircraftCategory = value; }
        }

        //private double? _angleField;

        //public double? AngleField
        //{
        //    get { return _angleField; }
        //    set { _angleField = value; }
        //}

        private SegmentPoint _arcCentre;
        //[Browsable(false)]
        public SegmentPoint ArcCentre
        {
            get { return _arcCentre; }
            set { _arcCentre = value; }
        }

        private SegmentPoint _startPoint;
        //[Browsable(false)]
        public SegmentPoint StartPoint
        {
            get { return _startPoint; }
            set { _startPoint = value; }
        }

        private SegmentPoint _endPoint;
        //[Browsable(false)]
        public SegmentPoint EndPoint
        {
            get { return _endPoint; }
            set { _endPoint = value; }
        }

        private double? _bankAngle = null;

        public double? BankAngle
        {
            get { return _bankAngle; }
            set { _bankAngle = value; }
        }

        private double? _course = null;

        public double? Course
        {
            get { return _course; }
            set { _course = value; }
        }

        private CodeDirectionReference _courseDirection;

        public CodeDirectionReference CourseDirection
        {
            get { return _courseDirection; }
            set { _courseDirection = value; }
        }

        private CodeCourse _courseType;

        public CodeCourse CourseType
        {
            get { return _courseType; }
            set { _courseType = value; }
        }


        private double? _duration = null;

        public double? Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        private DurationType _durationUOM;

        public DurationType DurationUOM
        {
            get { return _durationUOM; }
            set { _durationUOM = value; }
        }

        private TrajectoryType _legPathField;

        public TrajectoryType LegPathField
        {
            get { return _legPathField; }
            set { _legPathField = value; }
        }

        private CodeSegmentPath _legTypeARINC;

        public CodeSegmentPath LegTypeARINC
        {
            get { return _legTypeARINC; }
            set { _legTypeARINC = value; }
        }

        private double? length;

        public double? Length
        {
            get { return length; }
            set { length = value; }
        }

        private UOM_DIST_HORZ _lengthUOM;

        public UOM_DIST_HORZ LengthUOM
        {
            get { return _lengthUOM; }
            set { _lengthUOM = value; }
        }

        private double? _lowerLimitAltitude = null;

        public double? LowerLimitAltitude
        {
            get { return _lowerLimitAltitude; }
            set { _lowerLimitAltitude = value; }
        }

        private UOM_DIST_VERT _lowerLimitAltitudeUOM;

        public UOM_DIST_VERT LowerLimitAltitudeUOM
        {
            get { return _lowerLimitAltitudeUOM; }
            set { _lowerLimitAltitudeUOM = value; }
        }

        private CodeVerticalReference _lowerLimitReference;

        public CodeVerticalReference LowerLimitReference
        {
            get { return _lowerLimitReference; }
            set { _lowerLimitReference = value; }
        }

        private double? _speedLimit = null;

        public double? SpeedLimit
        {
            get { return _speedLimit; }
            set { _speedLimit = value; }
        }

        private SpeedType _speedUOM;

        public SpeedType SpeedUOM
        {
            get { return _speedUOM; }
            set { _speedUOM = value; }
        }

        private CodeSpeedReference _speedReference;

        public CodeSpeedReference SpeedReference
        {
            get { return _speedReference; }
            set { _speedReference = value; }
        }

        private AltitudeUseType _speedInterpritation;

        public AltitudeUseType SpeedInterpritation
        {
            get { return _speedInterpritation; }
            set { _speedInterpritation = value; }
        }

        private DirectionTurnType _turnDirection;

        public DirectionTurnType TurnDirection
        {
            get { return _turnDirection; }
            set { _turnDirection = value; }
        }

        private double? _upperLimitAltitude = null;

        public double? UpperLimitAltitude
        {
            get { return _upperLimitAltitude; }
            set { _upperLimitAltitude = value; }
        }

        private UOM_DIST_VERT _upperLimitAltitudeUOM;

        public UOM_DIST_VERT UpperLimitAltitudeUOM
        {
            get { return _upperLimitAltitudeUOM; }
            set { _upperLimitAltitudeUOM = value; }
        }

        private CodeVerticalReference _upperLimitReference;

        public CodeVerticalReference UpperLimitReference
        {
            get { return _upperLimitReference; }
            set { _upperLimitReference = value; }
        }

        private double? _verticalAngle = null;

        public double? VerticalAngle
        {
            get { return _verticalAngle; }
            set { _verticalAngle = value; }
        }

        private List<ObstacleAssessmentArea> _AssessmentArea;
        //[Browsable(false)]
        public List<ObstacleAssessmentArea> AssessmentArea
        {
            get { return _AssessmentArea; }
            set { _AssessmentArea = value; }
        }

        private SegmentLegSpecialization _LegSpecialization;

        public SegmentLegSpecialization LegSpecialization
        {
            get { return _LegSpecialization; }
            set { _LegSpecialization = value; }
        }

        private CodeSegmentTermination _endConditionDesignator;

        public CodeSegmentTermination EndConditionDesignator
        {
            get { return _endConditionDesignator; }
            set { _endConditionDesignator = value; }
        }

        private double? _requiredNavigationPerformance = null;

        public double? RequiredNavigationPerformance
        {
            get { return _requiredNavigationPerformance; }
            set { _requiredNavigationPerformance = value; }
        }

        private bool _procedureTurnRequired;

        public bool ProcedureTurnRequired
        {
            get { return _procedureTurnRequired; }
            set { _procedureTurnRequired = value; }
        }

        private string _legBlobGeometry;
        [Browsable(false)]
        public string LegBlobGeometry
        {
            get { return _legBlobGeometry; }
            set { _legBlobGeometry = value; }
        }

        private HoldingPattern _holdingUse;
        //[Browsable(false)]
        public HoldingPattern HoldingUse
        {
            get { return _holdingUse; }
            set { _holdingUse = value; }
        }

        private string _HoldingId;
        [Browsable(false)]
        public string HoldingId
        {
            get { return _HoldingId; }
            set { _HoldingId = value; }
        }

        private int _PositionFlag;
        [Browsable(false)]
        public int PositionFlag
        {
            get { return _PositionFlag; }
            set { _PositionFlag = value; }
        }

        [Browsable(false)]
        public override string Lat
        {
            get
            {
                return base.Lat;
            }
            set
            {
                base.Lat = value;
            }
        }

        [Browsable(false)]
        public override string Lon
        {
            get
            {
                return base.Lon;
            }
            set
            {
                base.Lon = value;
            }
        }

        [Browsable(false)]
        public override double? Elev
        {
            get
            {
                return base.Elev;
            }
            set
            {
                base.Elev = value;
            }
        }
        [Browsable(false)]
        public override UOM_DIST_VERT Elev_UOM
        {
            get
            {
                return base.Elev_UOM;
            }
            set
            {
                base.Elev_UOM = value;
            }
        }


        [Browsable(false)]
        public override DateTime ActualDate
        {
            get
            {
                return base.ActualDate;
            }
            set
            {
                base.ActualDate = value;
            }
        }

        [Browsable(false)]
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.ProcedureLeg;
            }
        } 

        public ProcedureLeg()
        {
        }

        public override string ToString()
        {
            return this.LegTypeARINC.ToString();

        }


        private AngleIndication _AngleIndication;
        [Browsable(false)]
        public AngleIndication AngleIndication
        {
            get { return _AngleIndication; }
            set { _AngleIndication = value; }
        }

        private DistanceIndication _DistanceIndication;
        [Browsable(false)]
        public DistanceIndication DistanceIndication
        {
            get { return _DistanceIndication; }
            set { _DistanceIndication = value; }
        }


        public override bool StoreToDB(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {

                ITable tbl = AIRTRACK_TableDic[typeof(ProcedureLeg)];

                if (EsriWorkEnvironment.EsriUtils.RowExist(tbl, this.ID)) return true;

                IRow row = tbl.CreateRow();

                #region CompileRow

                int findx = -1;

                findx = row.Fields.FindField("ID_Transition"); if (findx >= 0) row.set_Value(findx, this.TransitionIdentifier);
                findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
                findx = row.Fields.FindField("seqNumberARINC"); if (findx >= 0) row.set_Value(findx, this.SeqNumberARINC);
                findx = row.Fields.FindField("altitudeInterpretation"); if (findx >= 0) row.set_Value(findx, this.AltitudeInterpretation.ToString());
                findx = row.Fields.FindField("altitudeOverrideATC"); if (findx >= 0) row.set_Value(findx, this.AltitudeOverrideATC);
                findx = row.Fields.FindField("altitudeUOM"); if (findx >= 0) row.set_Value(findx, this.AltitudeUOM.ToString());
                findx = row.Fields.FindField("endConditionDesignator"); if (findx >= 0) row.set_Value(findx, this.EndConditionDesignator.ToString());
                findx = row.Fields.FindField("bankAngle"); if (findx >= 0) row.set_Value(findx, this.BankAngle);
                findx = row.Fields.FindField("course"); if (findx >= 0) row.set_Value(findx, this.Course);
                findx = row.Fields.FindField("courseDirection"); if (findx >= 0) row.set_Value(findx, this.CourseDirection.ToString());
                findx = row.Fields.FindField("courseType"); if (findx >= 0) row.set_Value(findx, this.CourseType.ToString());
                findx = row.Fields.FindField("procedureTurnRequired"); if (findx >= 0) row.set_Value(findx, this.ProcedureTurnRequired);
                findx = row.Fields.FindField("duration"); if (findx >= 0) row.set_Value(findx, this.Duration);
                findx = row.Fields.FindField("durationUOM"); if (findx >= 0) row.set_Value(findx, this.DurationUOM.ToString());
                findx = row.Fields.FindField("legPathField"); if (findx >= 0) row.set_Value(findx, this.LegPathField.ToString());
                findx = row.Fields.FindField("legTypeARINC"); if (findx >= 0) row.set_Value(findx, this.LegTypeARINC.ToString());
                findx = row.Fields.FindField("length"); if (findx >= 0) row.set_Value(findx, this.Length);
                findx = row.Fields.FindField("lengthUOM"); if (findx >= 0) row.set_Value(findx, this.LengthUOM.ToString());
                findx = row.Fields.FindField("lowerLimitAltitude"); if (findx >= 0) row.set_Value(findx, this.LowerLimitAltitude);
                findx = row.Fields.FindField("lowerLimitAltitudeUOM"); if (findx >= 0) row.set_Value(findx, this.LowerLimitAltitudeUOM.ToString());
                findx = row.Fields.FindField("speedLimit"); if (findx >= 0) row.set_Value(findx, this.SpeedLimit);
                findx = row.Fields.FindField("speedLimitUOM"); if (findx >= 0) row.set_Value(findx, this.SpeedUOM.ToString());
                findx = row.Fields.FindField("turnDirection"); if (findx >= 0) row.set_Value(findx, this.TurnDirection.ToString());
                findx = row.Fields.FindField("upperLimitAltitude"); if (findx >= 0) row.set_Value(findx, this.UpperLimitAltitude);
                findx = row.Fields.FindField("upperLimitAltitudeUOM"); if (findx >= 0) row.set_Value(findx, this.UpperLimitAltitudeUOM.ToString());
                findx = row.Fields.FindField("verticalAngle"); if (findx >= 0) row.set_Value(findx, this.VerticalAngle);
                findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
                findx = row.Fields.FindField("LegSpecialization"); if (findx >= 0) row.set_Value(findx, this.LegSpecialization.ToString());
                findx = row.Fields.FindField("FirstLastFlag"); if (findx >= 0) row.set_Value(findx, this.PositionFlag);

                findx = row.Fields.FindField("StartPontID"); if ((findx >= 0) && (this.StartPoint!=null)) row.set_Value(findx, this.StartPoint.ID);
                findx = row.Fields.FindField("EndPontID"); if ((findx >= 0) && (this.EndPoint != null)) row.set_Value(findx, this.EndPoint.ID);

                findx = row.Fields.FindField("LandingArea"); if ((findx >= 0) && (this.Lat.Length > 0)) row.set_Value(findx, this.Lat);
                findx = row.Fields.FindField("AerodromeName"); if ((findx >= 0) && (this.Lon.Length > 0)) row.set_Value(findx, this.Lon);

                this.Lat = "";
                this.Lon = "";

                if (this.Geo != null)
                {
                    findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);
                }

                #endregion

                row.Store();


                if (this.ArcCentre != null) { this.ArcCentre.StoreToDB(AIRTRACK_TableDic); }
                if (this.StartPoint != null) { this.StartPoint.StoreToDB(AIRTRACK_TableDic); }
                if (this.EndPoint != null) { this.EndPoint.StoreToDB(AIRTRACK_TableDic); }
                if (this.HoldingUse != null) { this.HoldingUse.StoreToDB(AIRTRACK_TableDic); }
                //if (this.AssessmentArea != null) this.AssessmentArea.StoreToDB(AIRTRACK_TableDic);


                if (this.LegSpecialization == SegmentLegSpecialization.FinalLeg)
                {
                    FinalLeg pdmFinalLeg = (FinalLeg)this;

                    tbl = AIRTRACK_TableDic[typeof(FinalLeg)];
                    row = tbl.CreateRow();

                    pdmFinalLeg.CompileRow(ref row);

                    row.Store();
                }

                if (this.LegSpecialization == SegmentLegSpecialization.MissedApproachLeg)
                {
                    MissaedApproachLeg pdmMissaedApproachLeg = (MissaedApproachLeg)this;

                    tbl = AIRTRACK_TableDic[typeof(MissaedApproachLeg)];
                    row = tbl.CreateRow();

                    pdmMissaedApproachLeg.CompileRow(ref row);

                    row.Store();
                }

                


            }
             catch(Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false;} 

            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            //int findx = -1;

            //findx = row.Fields.FindField("ID_Transition"); if (findx >= 0) row.set_Value(findx, this.TransitionIdentifier);
            //findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            //findx = row.Fields.FindField("seqNumberARINC"); if (findx >= 0) row.set_Value(findx, this.SeqNumberARINC);
            //findx = row.Fields.FindField("altitudeInterpretation"); if (findx >= 0) row.set_Value(findx, this.AltitudeInterpretation.ToString());
            //findx = row.Fields.FindField("altitudeOverrideATC"); if (findx >= 0) row.set_Value(findx, this.AltitudeOverrideATC);
            //findx = row.Fields.FindField("altitudeUOM"); if (findx >= 0) row.set_Value(findx, this.AltitudeUOM.ToString());
            //findx = row.Fields.FindField("endConditionDesignator"); if (findx >= 0) row.set_Value(findx, this.EndConditionDesignator.ToString());
            //findx = row.Fields.FindField("bankAngle"); if (findx >= 0) row.set_Value(findx, this.BankAngle);
            //findx = row.Fields.FindField("course"); if (findx >= 0) row.set_Value(findx, this.Course);
            //findx = row.Fields.FindField("courseDirection"); if (findx >= 0) row.set_Value(findx, this.CourseDirection.ToString());
            //findx = row.Fields.FindField("courseType"); if (findx >= 0) row.set_Value(findx, this.CourseType.ToString());
            //findx = row.Fields.FindField("procedureTurnRequired"); if (findx >= 0) row.set_Value(findx, this.ProcedureTurnRequired);
            //findx = row.Fields.FindField("duration"); if (findx >= 0) row.set_Value(findx, this.Duration);
            //findx = row.Fields.FindField("durationUOM"); if (findx >= 0) row.set_Value(findx, this.DurationUOM.ToString());
            //findx = row.Fields.FindField("legPathField"); if (findx >= 0) row.set_Value(findx, this.LegPathField.ToString());
            //findx = row.Fields.FindField("legTypeARINC"); if (findx >= 0) row.set_Value(findx, this.LegTypeARINC);
            //findx = row.Fields.FindField("length"); if (findx >= 0) row.set_Value(findx, this.Length);
            //findx = row.Fields.FindField("lengthUOM"); if (findx >= 0) row.set_Value(findx, this.LengthUOM.ToString());
            //findx = row.Fields.FindField("lowerLimitAltitude"); if (findx >= 0) row.set_Value(findx, this.LowerLimitAltitude);
            //findx = row.Fields.FindField("lowerLimitAltitudeUOM"); if (findx >= 0) row.set_Value(findx, this.LowerLimitAltitudeUOM.ToString());
            //findx = row.Fields.FindField("speedLimit"); if (findx >= 0) row.set_Value(findx, this.SpeedLimit);
            //findx = row.Fields.FindField("speedLimitUOM"); if (findx >= 0) row.set_Value(findx, this.SpeedUOM.ToString());
            //findx = row.Fields.FindField("turnDirection"); if (findx >= 0) row.set_Value(findx, this.TurnDirection.ToString());
            //findx = row.Fields.FindField("upperLimitAltitude"); if (findx >= 0) row.set_Value(findx, this.UpperLimitAltitude);
            //findx = row.Fields.FindField("upperLimitAltitudeUOM"); if (findx >= 0) row.set_Value(findx, this.UpperLimitAltitudeUOM.ToString());
            //findx = row.Fields.FindField("verticalAngle"); if (findx >= 0) row.set_Value(findx, this.VerticalAngle);
            //findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            //findx = row.Fields.FindField("LegSpecialization"); if (findx >= 0) row.set_Value(findx, this.LegSpecialization);


            //if (this.Geo != null)
            //{


            //    findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);

            //}
        }

        public override List<string> HideBranch(Dictionary<Type, ITable> AIRTRACK_TableDic, bool Visibility)
        {
            List<string> res = base.HideBranch(AIRTRACK_TableDic, Visibility);

            return res;
        }

        public override List<string> GetBranch(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            List<string> res = base.GetBranch(AIRTRACK_TableDic);

            if (this.StartPoint != null)
            {
                List<string> part = StartPoint.GetBranch(AIRTRACK_TableDic);
                res.AddRange(part);
            }

            if (this.EndPoint != null)
            {
                List<string> part = EndPoint.GetBranch(AIRTRACK_TableDic);
                res.AddRange(part);
            }

            return res;
        }

        public override void RebuildGeo()
        {
           
            RebuildGeo2();
            return;

        }

        public void RebuildGeo2()
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            this.Geo = ArnUtil.GetGeometry(this.ID, "ProcedureLegs", ArenaStatic.ArenaStaticProc.GetTargetDB());

            return;
            if (this.LegBlobGeometry != null)
            {

                string[] words = ((string)this.LegBlobGeometry).Split(':');

                byte[] bytes = new byte[words.Length];

                for (int i = 0; i <= words.Length - 2; i++) bytes[i] = Convert.ToByte(words[i]);


                // сконвертируем его в геометрию 
                IMemoryBlobStream memBlobStream = new MemoryBlobStream();

                IMemoryBlobStreamVariant varBlobStream = (IMemoryBlobStreamVariant)memBlobStream;

                varBlobStream.ImportFromVariant(bytes);

                IObjectStream anObjectStream = new ObjectStreamClass();
                anObjectStream.Stream = memBlobStream;

                IPropertySet aPropSet = new PropertySetClass();

                IPersistStream aPersistStream = (IPersistStream)aPropSet;
                aPersistStream.Load(anObjectStream);

                this.Geo = aPropSet.GetProperty("Leg") as IGeometry;
            }
        }

        public override string GetObjectLabel()
        {
            return this.SeqNumberARINC.ToString()+ " " + this.LegTypeARINC.ToString();
        }

    }


}
