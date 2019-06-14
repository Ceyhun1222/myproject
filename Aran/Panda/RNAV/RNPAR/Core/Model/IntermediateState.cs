using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Features;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Panda.RNAV.RNPAR.Model;
using Aran.Panda.RNAV.RNPAR.Utils;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Env = Aran.Panda.RNAV.RNPAR.Context.AppEnvironment;

namespace Aran.Panda.RNAV.RNPAR.Core.Model
{
    class IntermediateState : State<IntermediateState>
    {

        public IntermediateState()
        {
            _type = RNPAR.Model.StateType.Intermediate;
        }

        #region Cvverides
        protected override IntermediateState CreateNewState()
        {
            return new IntermediateState();
        }

        public override IntermediateState Copy()
        {
            var intermediateState = BaseCopy();
            intermediateState.CurrlegObsatclesList = CurrlegObsatclesList;
            intermediateState._ImASmaxRadius = _ImASmaxRadius;
            intermediateState._ImASminRadius = _ImASminRadius;
            intermediateState._ImASturndir = _ImASturndir;
            intermediateState._CurrImASLeg = _CurrImASLeg;
            intermediateState._ImASLegs = new List<RFLeg>();
            intermediateState._ImASLegs.AddRange(_ImASLegs);
            intermediateState.TrackDistance = TrackDistance;
            intermediateState._ptNextprj = _ptNextprj;
            intermediateState._hNext = _hNext;
            intermediateState._dg = _dg;

            return intermediateState;
        }

        public override void Commit()
        {
            AddLeg(!LastLeg);
        }


        public override void Clear()
        {
            int n = _ImASLegs.Count;

            for (int i = 0; i < n; i++)
            {
                Env.Current.AranGraphics.SafeDeleteGraphic(_ImASLegs[i].FixElem);
                Env.Current.AranGraphics.SafeDeleteGraphic(_ImASLegs[i].NominalElem);
                Env.Current.AranGraphics.SafeDeleteGraphic(_ImASLegs[i].ProtectionElem);
            }

            Env.Current.AranGraphics.SafeDeleteGraphic(_CurrImASLeg.FixElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_CurrImASLeg.NominalElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_CurrImASLeg.ProtectionElem);
        }

        public override void ReCreate()
        {
            int n = _ImASLegs.Count;
            for (int i = 0; i < n; i++)
            {
                var leg = _ImASLegs[i];
                Draw(ref leg);
                _ImASLegs[i] = leg;
            }

            Draw(ref _CurrImASLeg);

            TurnDirectionChanged(TurnDirection);
        }

        #endregion

        public ObstacleContainer CurrlegObsatclesList;

        public double DGMax => _ImASLegs.Count > 0 ? 4.7 : PreFinalState._VPA;
        public double DGInit => _ImASLegs.Count > 0 ? 2.4 : Math.Min(PreFinalState._VPA, 1.4);

        public double _ImASminRadius;
        public double _ImASmaxRadius;
        public double _dg;

        public int _ImASturndir;
        public RFLeg _CurrImASLeg;
        public List<RFLeg> _ImASLegs;
        public double TrackDistance;
        public double CurrAdhpElev => Env.Current.DataContext.CurrADHP.Elev;
        public Point _ptNextprj;
        private double _hNext;
       
        private const double _MaxDistance = 15.0 * 1852.0;
        private const int FlyByBankAngle = 18;

        public bool RfLegEnabled => NextLeg.legType != LegType.FlyBy;
        public bool FlyByLegEnabled => (NextLeg.legType == LegType.Straight) && FlyByDTACorrect();
        public bool StraightLegEnabled { get; private set; } = true;
        public double FlyByCourseDeltaMax => _ImASLegs.Count > 0 ? 90.0 : 15.0;
        public double RfCourseDeltaMax => 240;

        public bool LastLeg { get; set; }
        public PreFinalState PreFinalState => Env.Current.RNPContext.PreFinalPhase.CurrentState;
        public FinalState FinalState => Env.Current.RNPContext.FinalPhase.CurrentState;

        public int TurnDirection => (1 - _ImASturndir) / 2;

        public RFLeg NextLeg => _ImASLegs.Count > 0 ? _ImASLegs[_ImASLegs.Count - 1] : FinalState._CurrFASLeg;
       

        public void Init()
        {

            _ptNextprj = FinalState._ptNextprj;
            _ptNextprj.Z = FinalState._ptNextprj.Z;
            _hNext = FinalState._hNext;
            //=========================================================================================
            //GlobalVars.constants.AircraftCategory[aircraftCategoryData.VatMax].Value[_Category]
            //GlobalVars.constants.AircraftCategory[aircraftCategoryData.VatMax].Value[_Category]
            //ViafMin, ViafMax


            double ias = Env.Current.UnitContext.Constants.AircraftCategory[aircraftCategoryData.ViafMax].Value[PreFinalState._Category];

            double tas = 3.6 * ARANMath.IASToTAS(ias, CurrAdhpElev + _hNext, 0.0);

            _ImASminRadius = CalcImASRadius(tas, _hNext - CurrAdhpElev, 20.0);
            if (_ImASminRadius <= 2.0 * PreFinalState._FASRNPval)
                _ImASminRadius = Math.Ceiling(2.01 * PreFinalState._FASRNPval);

            _ImASmaxRadius = FinalState.CalcRadius(_hNext - CurrAdhpElev, 1.0);

            //=========================================================================================
            _ImASLegs = new List<RFLeg>();
            _dg = DGInit;
            _CurrImASLeg = new RFLeg
            {
                DescentGR = Math.Tan(ARANMath.DegToRad(DGInit)),
                IAS = ias,
                TAS = tas,
                BankAngle = FinalState._FASmaxBank,
                DistToNext = 0.0,
                RollOutAltitude = _hNext,
                Radius = _ImASmaxRadius,
                RollOutDir = FinalState._CurrFASLeg.StartDir,
                RollOutPrj = new Point(FinalState._CurrFASLeg.StartPrj),
                RNPvalue = Env.Current.UnitContext.UnitConverter.DistanceToInternalUnits(1),
                MOC = 150
            };
            _CurrImASLeg.RollOutGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(_CurrImASLeg.RollOutPrj);
            _CurrImASLeg.DistToNext = _CurrImASLeg.RNPvalue;
            _CurrImASLeg.Nominal = new MultiLineString();
            _CurrImASLeg.Protection = new MultiPolygon();




            

            TurnDirectionChanged(TurnDirection);

            LegTypeChanged(LegType.FixedRadius);

            CheckImASRadius();

            EntryCourseChanged(_CurrImASLeg.Course);

            CreateImASLeg();
        }

        public void IasChanged(double ias)
        {


            _CurrImASLeg.IAS = ias;

            double iasMin = Env.Current.UnitContext.Constants.AircraftCategory[aircraftCategoryData.ViafMin].Value[PreFinalState._Category];
            double iasMax = Env.Current.UnitContext.Constants.AircraftCategory[aircraftCategoryData.ViafMax].Value[PreFinalState._Category];

            if (_CurrImASLeg.IAS < iasMin)
                _CurrImASLeg.IAS = iasMin;

            if (_CurrImASLeg.IAS > iasMax)
                _CurrImASLeg.IAS = iasMax;


            _CurrImASLeg.TAS = 3.6 * ARANMath.IASToTAS(_CurrImASLeg.IAS, CurrAdhpElev + _hNext, 0.0);

            _ImASminRadius = CalcImASRadius(_CurrImASLeg.TAS, _CurrImASLeg.StartAltitude - CurrAdhpElev, 20.0);
            if (_ImASminRadius <= 2.0 * PreFinalState._FASRNPval)
                _ImASminRadius = Math.Ceiling(2.01 * PreFinalState._FASRNPval);

            _ImASmaxRadius = FinalState.CalcRadius(_CurrImASLeg.StartAltitude - CurrAdhpElev, 1.0);

            CheckImASRadius();

        }

        public void SegmentRnpValueChanged(double rnpNM)
        {

            double RNPval = rnpNM;

            if (RNPval < 0.1)
                RNPval = 0.1;

            if (RNPval > 1.0)
                RNPval = 1.0;

            _CurrImASLeg.RNPvalue = Env.Current.UnitContext.UnitConverter.DistanceFromNM(RNPval);
            if (_CurrImASLeg.DistToNext < _CurrImASLeg.RNPvalue)
            {
                _CurrImASLeg.DistToNext = _CurrImASLeg.RNPvalue;
            }
        }

        public void WptAltitudeChanged(double wpt)
        {


            _CurrImASLeg.StartAltitude = wpt;

            double maxAlt = _CurrImASLeg.RollOutAltitude + _CurrImASLeg.Nominal.Length * _CurrImASLeg.DescentGR;

            if (_CurrImASLeg.StartAltitude < _hNext)
                _CurrImASLeg.StartAltitude = _hNext;

            if (_CurrImASLeg.StartAltitude > maxAlt)
                _CurrImASLeg.StartAltitude = maxAlt;


            _CurrImASLeg.StartPrj.Z = _CurrImASLeg.StartAltitude;
            //_CurrImASLeg.StartGeo.Z = _CurrImASLeg.StartAltitude;

            _ImASminRadius = CalcImASRadius(_CurrImASLeg.TAS, _CurrImASLeg.StartAltitude - CurrAdhpElev, 20.0);
            if (_ImASminRadius <= 2.0 * PreFinalState._FASRNPval)
                _ImASminRadius = Math.Ceiling(2.01 * PreFinalState._FASRNPval);

            _ImASmaxRadius = FinalState.CalcRadius(_CurrImASLeg.StartAltitude - CurrAdhpElev, 1.0);
            CheckImASRadius();
        }

        public void MocChanged(double moc)
        {

            _CurrImASLeg.MOC = moc;

            if (_CurrImASLeg.MOC < 150.0)
                _CurrImASLeg.MOC = 150.0;

            if (_CurrImASLeg.MOC > 300.0)
                _CurrImASLeg.MOC = 300.0;
        }

        public void TrackDistanceChanged(double distance)
        {
            _CurrImASLeg.IsWpt = false;
            _CurrImASLeg.DistToNext = distance;
            if (_CurrImASLeg.DistToNext < _CurrImASLeg.RNPvalue)
                _CurrImASLeg.DistToNext = _CurrImASLeg.RNPvalue;

            if (_CurrImASLeg.DistToNext > _MaxDistance)
                _CurrImASLeg.DistToNext = _MaxDistance;

            //_CurrImASLeg.EndPrj = ARANFunctions.LocalToPrj(_ptNextprj, _CurrImASLeg.ExitDir, -_CurrImASLeg.DistToNext);
            //_CurrImASLeg.EndGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(_CurrImASLeg.EndPrj);
            //_CurrImASLeg.StartPrj = new Point(_CurrImASLeg.EndPrj);

            ////_CurrImASLeg.EndAltitude = FAPDist2hFAP(_CurrFASLeg.DistToNext, _hNext);
            ////textBox0203.Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_CurrFASLeg.EndAltitude).ToString();

            //_CurrImASLeg.EndPrj.Z = _CurrImASLeg.EndAltitude;
            //_CurrImASLeg.EndGeo.Z = _CurrImASLeg.EndAltitude;

        }

        public void RadiusChanged(double radius)
        {
            _CurrImASLeg.IsWpt = false;
            _CurrImASLeg.Radius = radius;
            CheckImASRadius();
        }

        public void CheckImASRadius()
        {
            double fTmp = _CurrImASLeg.Radius;

            if (_CurrImASLeg.Radius < _ImASminRadius)
                _CurrImASLeg.Radius = _ImASminRadius;

            if (_CurrImASLeg.Radius > _ImASmaxRadius)
                _CurrImASLeg.Radius = _ImASmaxRadius;

            //if (fTmp != _CurrImASLeg.Radius)
            //{
            //    CreateImASLeg();
            //}

            fTmp = _CurrImASLeg.BankAngle;
            _CurrImASLeg.BankAngle = CalcImASBank(_CurrImASLeg.TAS, _CurrImASLeg.RollOutAltitude - CurrAdhpElev, _CurrImASLeg.Radius);

            if (fTmp != _CurrImASLeg.BankAngle)
            {
                CheckImASBank(false);
            }
        }

        public double CalcImASBank(double Vtas, double height, double radius)
        {
            //double Vtas = 3.6 * ARANMath.IASToTAS(IAS, GlobalVars.CurrADHP.Elev + height, 0.0);

            //int ix = (int)Math.Ceiling((height - 152.4) / 152.4);
            //if (ix < 0) ix = 0;
            //else if (ix > 21) ix = 21;

            double Vwind = 3.6 * PreFinalState._CurrTWC;
            double V = Vtas + Vwind;

            //double R = (6355.0 * Math.Tan(ARANMath.DegToRad(bank))) / (Math.PI * V);
            //		bank = ARANMath.RadToDeg(Math.Atan(3.0 * Math.PI * V / 6355.0));
            //double bank = ARANMath.RadToDeg(Math.Atan(V * V / (127094.0 * radius)));

            double R = 50.0 * V / (radius * Math.PI);

            if (R > 3.0)
                R = 3.0;

            double bank = ARANMath.RadToDeg(Math.Atan(R * Math.PI * V / 6355.0));

            if (bank > 20.0)
                bank = 20.0;
            return bank;
        }

        public void BankAngleChanged(double angle)
        {
            _CurrImASLeg.IsWpt = false;
            _CurrImASLeg.BankAngle = angle;
            CheckImASBank();
        }

        public void CheckImASBank(bool create = true)
        {
            if (_CurrImASLeg.BankAngle < 1.0)
                _CurrImASLeg.BankAngle = 1.0;

            if (_CurrImASLeg.BankAngle > FinalState._FASmaxBank)   //_ImASmaxBank
                _CurrImASLeg.BankAngle = FinalState._FASmaxBank;

            if (_CurrImASLeg.legType == LegType.FlyBy)
                _CurrImASLeg.BankAngle = FlyByBankAngle;

            double fTmp = _CurrImASLeg.Radius;
            _CurrImASLeg.Radius = CalcImASRadius(_CurrImASLeg.TAS, _CurrImASLeg.RollOutAltitude - CurrAdhpElev, _CurrImASLeg.BankAngle);
        }

        public void LegTypeChanged(LegType type)
        {
            _CurrImASLeg.IsWpt = false;
            _CurrImASLeg.legType = type;
            ;
            if (_CurrImASLeg.legType == LegType.FlyBy)
            {
                
                BankAngleChanged(FlyByBankAngle);
                EntryCourseChanged();
            }

            if (_CurrImASLeg.legType == LegType.FixedRadius)
            {
                RadiusChanged(_CurrImASLeg.Radius);
                EntryCourseChanged();
            }

            if (_CurrImASLeg.legType == LegType.Straight)
                TrackDistanceChanged(_CurrImASLeg.DistToNext);

        }

        public void TurnDirectionChanged(int direction)
        {
            _ImASturndir = 1 - 2 * direction;
            _CurrImASLeg.TurnDir = (TurnDirection)_ImASturndir;
            EntryCourseChanged();
        }

        public void EntryCourseChanged()
        {
            EntryCourseChanged(_CurrImASLeg.Course);
        }

        public void EntryCourseChanged(double course)
        {
            _CurrImASLeg.IsWpt = false;
            _CurrImASLeg.Course = course;
            double ExitCourse = ARANFunctions.DirToAzimuth(_ptNextprj, _CurrImASLeg.RollOutDir, Env.Current.SpatialContext.SpatialReferenceProjection, Env.Current.SpatialContext.SpatialReferenceGeo);
            double dAzt = NativeMethods.Modulus(_ImASturndir * (_CurrImASLeg.Course - ExitCourse));

            double dAngle;
            if (_CurrImASLeg.legType == LegType.FixedRadius)
                dAngle = RfCourseDeltaMax;
            else
            {
                dAngle = CalcFlyByEntryCourseDelta();
            }

            if (dAzt > dAngle)
                _CurrImASLeg.Course = NativeMethods.Modulus(ExitCourse + _ImASturndir * dAngle);


            Point EndGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(_ptNextprj);
            _CurrImASLeg.StartDir = ARANFunctions.AztToDirection(EndGeo, _CurrImASLeg.Course, Env.Current.SpatialContext.SpatialReferenceGeo, Env.Current.SpatialContext.SpatialReferenceProjection);

                
        }

        private double CalcFlyByEntryCourseDelta()
        {
            return Math.Min(FlyByCourseDeltaMax, CalcMaxFlyByCourseDeltaDTA());
        }

        private double CalcMaxFlyByCourseDeltaDTA()
        {
            var dMax = ARANMath.RadToDeg(NativeMethods.Modulus(2 * Math.Atan(NextLeg.DistToNext / _CurrImASLeg.Radius)));
            return dMax;
        }

        private double FlyByDTA()
        {
            double ExitCourse = ARANFunctions.DirToAzimuth(_ptNextprj, _CurrImASLeg.RollOutDir, Env.Current.SpatialContext.SpatialReferenceProjection, Env.Current.SpatialContext.SpatialReferenceGeo);
            double dAzt = NativeMethods.Modulus(_ImASturndir * (_CurrImASLeg.Course - ExitCourse));
            return _CurrImASLeg.Radius * Math.Tan(dAzt/2);
        }

        private double FlyByDTA(double radius, double deltaAz)
        {
            return radius * Math.Tan(deltaAz / 2);
        }

        private bool FlyByDTACorrect()
        {
            //double r = CalcImASRadius(_CurrImASLeg.TAS, _CurrImASLeg.RollOutAltitude - CurrAdhpElev, FlyByBankAngle);
            //return NextLeg.DistToNext >= r * Math.Tan(ARANMath.DegToRad(FlyByCourseDeltaMax / 2));
            return true;
        }

        public double CalcImASRadius(double Vtas, double height, double bank)
        {
            //double Vtas = 3.6 * ARANMath.IASToTAS(IAS, GlobalVars.CurrADHP.Elev + height, 0.0);

            //int ix = (int)Math.Ceiling((height - 152.4) / 152.4);
            //if (ix < 0) ix = 0;
            //else if (ix > 21) ix = 21;

            double Vwind = 3.6 * PreFinalState._CurrTWC;
            double V = Vtas + Vwind;
            double R = (6355.0 * Math.Tan(ARANMath.DegToRad(bank))) / (Math.PI * V);

            if (R > 3.0)
                R = 3.0;

            return 1000.0 * V / (20 * Math.PI * R);
        }

        public void CreateImASLeg()
        {
            Env.Current.AranGraphics.SafeDeleteGraphic(_CurrImASLeg.FixElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_CurrImASLeg.NominalElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_CurrImASLeg.ProtectionElem);

            //==================================================
            _CurrImASLeg.Nominal.Clear();
            _CurrImASLeg.Protection.Clear();

            if (_CurrImASLeg.legType == LegType.FixedRadius)
            {

                _CurrImASLeg.RollOutPrj = (Point)_ptNextprj.Clone();
                _CurrImASLeg.RollOutGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(_CurrImASLeg.RollOutPrj);

                _CurrImASLeg.RollOutPrj.Z = _CurrImASLeg.RollOutAltitude;
                _CurrImASLeg.RollOutGeo.Z = _CurrImASLeg.RollOutAltitude;
                //===

                if (_ImASLegs.Count == 0)
                {
                    //RFLeg next = _CurrFASLeg;// _FASLegs[_FASLegs.Count - 1];
                    CreateImASRFLeg(FinalState._CurrFASLeg, ref _CurrImASLeg);
                }
                else
                {
                    RFLeg next = _ImASLegs[_ImASLegs.Count - 1];
                    CreateImASRFLeg(next, ref _CurrImASLeg);

                    RFLeg nextNext;
                    if (_ImASLegs.Count == 1)
                        nextNext = FinalState._CurrFASLeg;
                    else
                        nextNext = _ImASLegs[_ImASLegs.Count - 2];

                    next.Protection.Clear();
                    next.Nominal.Clear();

                    if (next.legType == LegType.FixedRadius)
                        CreateImASRFLeg(nextNext, ref next, _CurrImASLeg, true);
                    else if (next.legType == LegType.FlyBy)
                        CreateIImASFlyByLeg(nextNext, ref next, _CurrImASLeg, true);
                    else
                        CreateImASStraightLeg(nextNext, ref next, _CurrImASLeg, true);

                    Clear(next);

                    Draw(ref next);

                    _ImASLegs[_ImASLegs.Count - 1] = next;
                }


                TrackDistance = _CurrImASLeg.Nominal.Length - _CurrImASLeg.RNPvalue;
            }
            else if (_CurrImASLeg.legType == LegType.FlyBy)
            {
                double turnangle = (_CurrImASLeg.RollOutDir - _CurrImASLeg.StartDir) * (int)_CurrImASLeg.TurnDir;
     
                double ptDist = _CurrImASLeg.Radius * Math.Tan(0.5 * turnangle);

                _CurrImASLeg.RollOutPrj = ARANFunctions.LocalToPrj(_ptNextprj, _CurrImASLeg.RollOutDir, ptDist);        // (Point)_ptNextprj.Clone();
                _CurrImASLeg.RollOutGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(_CurrImASLeg.RollOutPrj);

                _CurrImASLeg.RollOutPrj.Z = _CurrImASLeg.RollOutAltitude;
                _CurrImASLeg.RollOutGeo.Z = _CurrImASLeg.RollOutAltitude;
                //===

                if (_ImASLegs.Count == 0)
                    CreateIImASFlyByLeg(FinalState._CurrFASLeg, ref _CurrImASLeg);
                else
                {
                    RFLeg next = _ImASLegs[_ImASLegs.Count - 1];
                    CreateIImASFlyByLeg(next, ref _CurrImASLeg);

                    //_CurrImASLeg.ProtectionElem =Env.Current.AranGraphics.DrawMultiPolygon(_CurrImASLeg.Protection, -1, eFillStyle.sfsForwardDiagonal);
                    //Application.DoEvents();

                    RFLeg nextNext;
                    if (_ImASLegs.Count == 1)
                        nextNext = FinalState._CurrFASLeg;
                    else
                        nextNext = _ImASLegs[_ImASLegs.Count - 2];

                    next.Protection.Clear();
                    next.Nominal.Clear();

                    if (next.legType == LegType.FixedRadius)
                        CreateImASRFLeg(nextNext, ref next, _CurrImASLeg, true);
                    else if (next.legType == LegType.FlyBy)
                        CreateIImASFlyByLeg(nextNext, ref next, _CurrImASLeg, true);
                    else
                        CreateImASStraightLeg(nextNext, ref next, _CurrImASLeg, true);



                    Clear(next);

                    Draw(ref next);

                    _ImASLegs[_ImASLegs.Count - 1] = next;
                }

                TrackDistance = _CurrImASLeg.Nominal.Length - _CurrImASLeg.RNPvalue;
            }
            else
            {
                _CurrImASLeg.RollOutPrj = _CurrImASLeg.IsWpt?_CurrImASLeg.startWpt.pPtPrj:ARANFunctions.LocalToPrj(_ptNextprj, _CurrImASLeg.RollOutDir, -_CurrImASLeg.DistToNext);
                _CurrImASLeg.RollOutGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(_CurrImASLeg.RollOutPrj);
                _CurrImASLeg.StartPrj = new Point(_CurrImASLeg.RollOutPrj);
                _CurrImASLeg.StartGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(_CurrImASLeg.StartPrj);

                _CurrImASLeg.StartDir = _CurrImASLeg.RollOutDir;

                //_CurrImASLeg.EndAltitude = FAPDist2hFAP(_CurrFASLeg.DistToNext, _hNext);
                //textBox0203.Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_CurrFASLeg.EndAltitude).ToString();

                _CurrImASLeg.RollOutPrj.Z = _CurrImASLeg.RollOutAltitude;
                _CurrImASLeg.RollOutGeo.Z = _CurrImASLeg.RollOutAltitude;

                if (_ImASLegs.Count == 0)
                    CreateImASStraightLeg(FinalState._CurrFASLeg, ref _CurrImASLeg);
                else
                {
                    RFLeg next = _ImASLegs[_ImASLegs.Count - 1];
                    CreateImASStraightLeg(next, ref _CurrImASLeg);


                    RFLeg nextNext;
                    if (_ImASLegs.Count == 1)
                        nextNext = FinalState._CurrFASLeg;
                    else
                        nextNext = _ImASLegs[_ImASLegs.Count - 2];

                    next.Protection.Clear();
                    next.Nominal.Clear();

                    if (next.legType == LegType.FixedRadius)
                        CreateImASRFLeg(nextNext, ref next, _CurrImASLeg, true);
                    else if (next.legType == LegType.FlyBy)
                        CreateIImASFlyByLeg(nextNext, ref next, _CurrImASLeg, true);
                    else
                        CreateImASStraightLeg(nextNext, ref next, _CurrImASLeg, true);

                    Clear(next);


                    //Application.DoEvents();

                    next.ProtectionElem = -1;
                    next.NominalElem = -1;
                    next.FixElem = -1;

                    if (next.legType != LegType.FlyBy)
                    {
                        Draw(ref next);
                    }
                    //Application.DoEvents();

                    _ImASLegs[_ImASLegs.Count - 1] = next;
                }
                TrackDistance = _CurrImASLeg.DistToNext;
            }

            _CurrImASLeg.CopyGeometry();

            _CurrImASLeg.ProtectionElem = Env.Current.AranGraphics.DrawMultiPolygon(_CurrImASLeg.Protection, eFillStyle.sfsHollow, ARANFunctions.RGB(0, 0, 255));
            _CurrImASLeg.NominalElem = Env.Current.AranGraphics.DrawMultiLineString(_CurrImASLeg.Nominal, 2, 255);
            if (_CurrImASLeg.legType != LegType.FlyBy)
                _CurrImASLeg.FixElem = Env.Current.AranGraphics.DrawPointWithText(_CurrImASLeg.StartPrj, "WPT", ARANFunctions.RGB(0, 127, 255));



       
        }

        private static void Clear(RFLeg next)
        {
            Env.Current.AranGraphics.SafeDeleteGraphic(next.FixElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(next.NominalElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(next.ProtectionElem);
        }

        private static void Draw(ref RFLeg leg)
        {
            leg.ProtectionElem =
                Env.Current.AranGraphics.DrawMultiPolygon(leg.Protection, eFillStyle.sfsHollow, ARANFunctions.RGB(0, 0, 255));
            leg.NominalElem = Env.Current.AranGraphics.DrawMultiLineString(leg.Nominal, 2, 255);
            if (leg.legType != LegType.FlyBy)
                leg.FixElem = Env.Current.AranGraphics.DrawPointWithText(leg.StartPrj, "WPT", ARANFunctions.RGB(0, 127, 255));
        }

        private void CreateNextLeg(RFLeg nextLeg, ref RFLeg currLeg)    //CreateNextPart
        {
            Ring ring;
            Polygon pPoly;
            LineString ls;
            Point from, to;

            if (nextLeg.legType == LegType.Straight)
            {
                ring = new Ring();
                //ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, currLeg.RollOutDir, currLeg.RNPvalue, 2.0 * currLeg.RNPvalue));
                //ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, 2.0 * currLeg.RNPvalue));
                //ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, -2.0 * currLeg.RNPvalue));
                //ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, currLeg.RollOutDir, currLeg.RNPvalue, -2.0 * currLeg.RNPvalue));

                ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.RollOutDir, currLeg.RNPvalue, 2.0 * currLeg.RNPvalue));
                ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.RollOutDir, 0.0, 2.0 * currLeg.RNPvalue));
                ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.RollOutDir, 0.0, -2.0 * currLeg.RNPvalue));
                ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.RollOutDir, currLeg.RNPvalue, -2.0 * currLeg.RNPvalue));
                //ring.IsExterior = true;

                pPoly = new Polygon();
                pPoly.ExteriorRing = ring;
                //Env.Current.AranGraphics.DrawPolygon(pPoly, -1, eFillStyle.sfsBackwardDiagonal);
                ///Env.Current.AranGraphics.DrawPointWithText(nextLeg.StartPrj, -1, "nextLeg.StartPrj");
                //while(true)
                //Application.DoEvents();

                currLeg.Protection.Add(pPoly);

                //Env.Current.AranGraphics.DrawMultiPolygon(currLeg.Protection, -1, eFillStyle.sfsBackwardDiagonal);
                //Application.DoEvents();
                //===================================================
                ls = new LineString();
                ls.Add(nextLeg.StartPrj);
                ls.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, currLeg.RNPvalue));

                //Env.Current.AranGraphics.DrawLineString(ls, -1, 2);
                //Application.DoEvents();

                currLeg.Nominal.Add(ls);
            }
            else if (nextLeg.legType == LegType.FixedRadius)
            {
                ring = new Ring();

                double alpha = currLeg.RNPvalue / (nextLeg.Radius - 2.0 * currLeg.RNPvalue);
                from = ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, (int)nextLeg.TurnDir*2.0 * currLeg.RNPvalue);
                to = ARANFunctions.LocalToPrj(nextLeg.Center, nextLeg.StartDir - (int)nextLeg.TurnDir * (ARANMath.C_PI_2 - alpha), nextLeg.Radius - 2.0 * currLeg.RNPvalue, 0.0);

                //Env.Current.AranGraphics.DrawPointWithText(nextLeg.Center, "center");
                //Env.Current.AranGraphics.DrawPointWithText(from, "from");

                ls = ARANFunctions.CreateArcAsPartPrj(nextLeg.Center, from, to, nextLeg.TurnDir);
                ring.AddMultiPoint(ls);

                alpha = currLeg.RNPvalue / (nextLeg.Radius + 2.0 * currLeg.RNPvalue);
                from = ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, -(int)nextLeg.TurnDir * 2.0 * currLeg.RNPvalue);
                to = ARANFunctions.LocalToPrj(nextLeg.Center, nextLeg.StartDir - (int)nextLeg.TurnDir * (ARANMath.C_PI_2 - alpha), nextLeg.Radius + 2.0 * currLeg.RNPvalue, 0.0);

                ls = ARANFunctions.CreateArcAsPartPrj(nextLeg.Center, from, to, nextLeg.TurnDir);

                ring.AddReverse(ls);

                pPoly = new Polygon();
                pPoly.ExteriorRing = ring;

                currLeg.Protection.Add(pPoly);

                //===================================================
                alpha = currLeg.RNPvalue / nextLeg.Radius;

                to = ARANFunctions.LocalToPrj(nextLeg.Center, nextLeg.StartDir - (int)nextLeg.TurnDir * (ARANMath.C_PI_2 - alpha), nextLeg.Radius, 0.0);
                ls = ARANFunctions.CreateArcAsPartPrj(nextLeg.Center, nextLeg.StartPrj, to, nextLeg.TurnDir);

                currLeg.Nominal.Add(ls);
            }
            else
            {
                int turnDir = (int)nextLeg.TurnDir;
                Point ptFIX = (Point)ARANFunctions.LineLineIntersect(nextLeg.StartPrj, nextLeg.StartDir, nextLeg.RollOutPrj, nextLeg.RollOutDir);

                double turnangle = turnDir * (nextLeg.RollOutDir - nextLeg.StartDir);
                if (turnangle < 0.0)
                    turnangle += 2.0 * Math.PI;

                double bisect = nextLeg.RollOutDir + 0.5 * turnDir * (Math.PI - turnangle);

                double rIn = nextLeg.Radius + nextLeg.RNPvalue;
                double l1 = (nextLeg.Radius + 3.0 * currLeg.RNPvalue) / Math.Cos(0.5 * turnangle);
                Point ptCnt1 = ARANFunctions.LocalToPrj(ptFIX, bisect, l1);

                //Env.Current.AranGraphics.DrawPointWithText(ptCnt1, -1, "Cnt-1");
                //Application.DoEvents();

                //================================================================================================
                Point ptInStart = ARANFunctions.LocalToPrj(ptCnt1, nextLeg.StartDir, 0, -turnDir * rIn);

                //Env.Current.AranGraphics.DrawPointWithText(ptInStart, -1, "Inner Start");
                //Application.DoEvents();
                //ptInStart = ARANFunctions.LocalToPrj(ptCnt1, currLeg.StartDir, 0, -turnDir * rIn);

                double x = Math.Sqrt(rIn * rIn - nextLeg.RNPvalue * nextLeg.RNPvalue);
                Point ptInEnd = ARANFunctions.LocalToPrj(ptCnt1, bisect, -x, -turnDir * currLeg.RNPvalue);

                //Env.Current.AranGraphics.DrawPointWithText(ptInStart, -1, "Inner Start");
                //Env.Current.AranGraphics.DrawPointWithText(ptInEnd, -1, "Inner End");
                ////while(true)
                //Application.DoEvents();

                //Point ptOutStart = ARANFunctions.LocalToPrj(ptFIX, currLeg.ExitDir + Math.PI, 2.0 * currLeg.RNPvalue);
                Point ptOutStart = ARANFunctions.LocalToPrj(ptFIX, currLeg.StartDir, 0, -2.0 * turnDir * currLeg.RNPvalue);
                //Env.Current.AranGraphics.DrawPointWithText(ptOutStart, -1, "Outer Start");
                //Application.DoEvents();

                x = currLeg.RNPvalue * ARANMath.C_SQRT3;
                Point ptOutEnd = ARANFunctions.LocalToPrj(ptFIX, bisect, -x, -turnDir * currLeg.RNPvalue);
                //Env.Current.AranGraphics.DrawPointWithText(ptOutEnd, -1, "Outer End");
                //Application.DoEvents();

                Point ptOutPrev = ARANFunctions.LocalToPrj(ptCnt1, currLeg.StartDir, 0, -turnDir * (rIn + 4.0 * currLeg.RNPvalue));
                //Env.Current.AranGraphics.DrawPointWithText(ptOutPrev, -1, "Outer Prev");
                //Application.DoEvents();

                ring = new Ring();
                ls = ARANFunctions.CreateArcAsPartPrj(ptCnt1, ptInStart, ptInEnd, nextLeg.TurnDir);

                //Env.Current.AranGraphics.DrawLineString(ls, -1, 2);
                //Application.DoEvents();

                ring.AddMultiPoint(ls);

                ls = ARANFunctions.CreateArcAsPartPrj(ptFIX, ptOutStart, ptOutEnd, nextLeg.TurnDir);

                //Env.Current.AranGraphics.DrawLineString(ls, -1, 2);
                //Application.DoEvents();

                ring.AddReverse(ls);
                ring.Add(ptOutPrev);


                pPoly = new Polygon();
                pPoly.ExteriorRing = ring;
                currLeg.Protection.Add(pPoly);

                //Env.Current.AranGraphics.DrawPolygon(pPoly, -1, eFillStyle.sfsBackwardDiagonal);
                ////while(true)
                //Application.DoEvents();

                // cutter Polygon ==================================

                Ring cutRing = new Ring();
                cutRing.Add(ARANFunctions.LocalToPrj(ptFIX, nextLeg.StartDir, currLeg.RNPvalue, 2.0 * currLeg.RNPvalue));
                cutRing.Add(ARANFunctions.LocalToPrj(ptFIX, nextLeg.StartDir, currLeg.RNPvalue, -2.0 * currLeg.RNPvalue));
                cutRing.Add(ARANFunctions.LocalToPrj(ptFIX, nextLeg.StartDir, 5.0 * currLeg.RNPvalue, -2.0 * currLeg.RNPvalue));
                cutRing.Add(ARANFunctions.LocalToPrj(ptFIX, nextLeg.StartDir, 5.0 * currLeg.RNPvalue, 2.0 * currLeg.RNPvalue));

                Polygon cutPoly = new Polygon();
                cutPoly.ExteriorRing = cutRing;

                //Env.Current.AranGraphics.DrawPolygon(cutPoly, -1, eFillStyle.sfsHorizontal);
                //Application.DoEvents();

                // cutter Line ==================================
                LineString cutLine = new LineString();
                cutLine.Add(ptCnt1);
                cutLine.Add(ARANFunctions.LocalToPrj(ptFIX, bisect + Math.PI, 5.0 * nextLeg.RNPvalue));

                //Env.Current.AranGraphics.DrawLineString(cutLine, 255, 2);
                //Application.DoEvents();

                GeometryOperators geoOps = new GeometryOperators();
                //geoOps.CurrentGeometry = cutPoly;

                if (geoOps.Crosses(cutPoly, cutLine))
                {
                    Geometry geomLeft, geomRight;
                    geoOps.Cut(cutPoly, cutLine, out geomLeft, out geomRight);

                    if (nextLeg.TurnDir == PANDA.Common.TurnDirection.CCW)
                        cutPoly = ((MultiPolygon)geomLeft)[0];
                    else
                        cutPoly = ((MultiPolygon)geomRight)[0];
                }

                //Env.Current.AranGraphics.DrawPolygon(cutPoly, -1, eFillStyle.sfsVertical);
                //Application.DoEvents();

                //geoOps.CurrentGeometry = cutPoly;
                if (geoOps.Crosses(cutPoly, currLeg.Protection))
                {
                    //MultiPolygon result = (MultiPolygon)geoOps.Difference(cutPoly, currLeg.Protection);
                    //Env.Current.AranGraphics.DrawMultiPolygon(result, -1, eFillStyle.sfsCross);
                    //while(true)
                    //Application.DoEvents();

                    MultiPolygon result1 = (MultiPolygon)geoOps.Difference(currLeg.Protection, cutPoly);
                    //Env.Current.AranGraphics.DrawMultiPolygon(result1 , -1, eFillStyle.sfsDiagonalCross);
                    //Application.DoEvents();

                    currLeg.Protection = result1;
                }

                //===================================================

                ls = ARANFunctions.CreateArcAsPartPrj(nextLeg.Center, nextLeg.StartPrj, nextLeg.RollOutPrj, nextLeg.TurnDir);

                //Env.Current.AranGraphics.DrawLineString(ls, ARANFunctions.RGB(0, 255,0), 2);
                //Application.DoEvents();

                currLeg.Nominal.Add(ls);
                currLeg.StartAltitude = currLeg.RollOutAltitude + ls.Length * currLeg.DescentGR;
            }
        }

        private void CreatePrevLeg(ref RFLeg currLeg, RFLeg prevLeg)    //CreatePrevPart
        {
            Ring ring;
            Polygon pPoly;
            LineString ls;
            Point from, to;

            if (prevLeg.legType == LegType.Straight)
            {
                ring = new Ring();
                ring.Add(ARANFunctions.LocalToPrj(currLeg.StartPrj, prevLeg.RollOutDir, 0.0, 2.0 * currLeg.RNPvalue));
                ring.Add(ARANFunctions.LocalToPrj(currLeg.StartPrj, prevLeg.RollOutDir, -prevLeg.RNPvalue, 2.0 * currLeg.RNPvalue));
                ring.Add(ARANFunctions.LocalToPrj(currLeg.StartPrj, prevLeg.RollOutDir, -prevLeg.RNPvalue, -2.0 * currLeg.RNPvalue));
                ring.Add(ARANFunctions.LocalToPrj(currLeg.StartPrj, prevLeg.RollOutDir, 0.0, -2.0 * currLeg.RNPvalue));

                pPoly = new Polygon();
                pPoly.ExteriorRing = ring;

                //Env.Current.AranGraphics.DrawPolygon(pPoly, -1, eFillStyle.sfsBackwardDiagonal);
                //Application.DoEvents();

                currLeg.Protection.Add(pPoly);
                //===================================================
                ls = new LineString();
                ls.Add(ARANFunctions.LocalToPrj(currLeg.StartPrj, prevLeg.RollOutDir, -prevLeg.RNPvalue));
                ls.Add(currLeg.StartPrj);
                currLeg.Nominal.Add(ls);
            }
            else if (prevLeg.legType == LegType.FixedRadius)
            {
                ring = new Ring();


                //Env.Current.AranGraphics.DrawPointWithText(currLeg.StartPrj, -1, "currLeg.StartPrj");
                //Env.Current.AranGraphics.DrawPointWithText(prevLeg.RollOutPrj, -1, "prevLeg.RollOutPrj");
                ////while(true)
                //Application.DoEvents();

                double alpha = prevLeg.RNPvalue / (prevLeg.Radius - 2.0 * currLeg.RNPvalue);
                double fTurnDir = (int)prevLeg.TurnDir;

                to = ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.StartDir, 0.0, 2.0 * fTurnDir * currLeg.RNPvalue);
                from = ARANFunctions.LocalToPrj(prevLeg.Center, currLeg.StartDir - fTurnDir * (ARANMath.C_PI_2 + alpha), prevLeg.Radius - 2.0 * currLeg.RNPvalue, 0.0);
                //from = ARANFunctions.LocalToPrj(prevLeg.RollOutPrj, prevLeg.StartDir, 0.0, 2.0 * prevLeg.RNPvalue);
                //to = ARANFunctions.LocalToPrj(prevLeg.Center, prevLeg.StartDir + (int)prevLeg.TurnDir * (alpha + ARANMath.C_PI_2), prevLeg.Radius + 2.0 * prevLeg.RNPvalue, 0.0);

                //Env.Current.AranGraphics.DrawPointWithText(from, -1, "from");
                //Env.Current.AranGraphics.DrawPointWithText(to, -1, "to");
                //Application.DoEvents();

                //double alpha = currLeg.RNPvalue / prevLeg.Radius;


                ls = ARANFunctions.CreateArcAsPartPrj(prevLeg.Center, from, to, prevLeg.TurnDir);
                ring.AddMultiPoint(ls);


                alpha = prevLeg.RNPvalue / (prevLeg.Radius + 2.0 * currLeg.RNPvalue);

                to = ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.StartDir, 0.0, -2.0 * fTurnDir * currLeg.RNPvalue);
                from = ARANFunctions.LocalToPrj(prevLeg.Center, currLeg.StartDir - fTurnDir * (ARANMath.C_PI_2 + alpha), prevLeg.Radius + 2.0 * currLeg.RNPvalue, 0.0);
                //from = ARANFunctions.LocalToPrj(prevLeg.RollOutPrj, prevLeg.StartDir, 0.0, -2.0 * prevLeg.RNPvalue);
                //to = ARANFunctions.LocalToPrj(prevLeg.Center, prevLeg.StartDir + (int)prevLeg.TurnDir * (alpha + ARANMath.C_PI_2), prevLeg.Radius - 2.0 * prevLeg.RNPvalue, 0.0);

                //while(true)
                //Application.DoEvents();

                //Env.Current.AranGraphics.DrawPointWithText(from, -1, "from");
                //Env.Current.AranGraphics.DrawPointWithText(to, -1, "to");
                //Application.DoEvents();

                ls = ARANFunctions.CreateArcAsPartPrj(prevLeg.Center, from, to, prevLeg.TurnDir);
                ring.AddReverse(ls);

                pPoly = new Polygon();
                pPoly.ExteriorRing = ring;

                //Env.Current.AranGraphics.DrawPolygon(pPoly, -1, eFillStyle.sfsForwardDiagonal);
                //Application.DoEvents();

                currLeg.Protection.Add(pPoly);
                //===================================================
                alpha = prevLeg.RNPvalue / prevLeg.Radius;

                from = ARANFunctions.LocalToPrj(prevLeg.Center, currLeg.StartDir - fTurnDir * (ARANMath.C_PI_2 + alpha), prevLeg.Radius, 0.0);
                ls = ARANFunctions.CreateArcAsPartPrj(prevLeg.Center, from, prevLeg.RollOutPrj, prevLeg.TurnDir);

                //Env.Current.AranGraphics.DrawLineString(ls, -1, 2);
                //Application.DoEvents();

                currLeg.Nominal.Add(ls);
            }
            else
            {
                int turnDir = (int)prevLeg.TurnDir;
                Point ptFIX = currLeg.StartPrj;

                //Env.Current.AranGraphics.DrawPointWithText(ptFIX, -1, "FIX");
                //while(true)
                //Application.DoEvents();

                double turnangle = turnDir * (prevLeg.RollOutDir - prevLeg.StartDir);
                if (turnangle < 0.0)
                    turnangle += 2.0 * Math.PI;

                double bisect = prevLeg.RollOutDir + 0.5 * turnDir * (Math.PI - turnangle);

                double rIn = prevLeg.Radius + currLeg.RNPvalue;
                double l1 = (prevLeg.Radius + 3.0 * currLeg.RNPvalue) / Math.Cos(0.5 * turnangle);
                Point ptCnt1 = ARANFunctions.LocalToPrj(ptFIX, bisect, l1);
                //================================================================================================
                //Env.Current.AranGraphics.DrawPointWithText(ptCnt1, -1, "Cnt-1");
                //Application.DoEvents();

                //ls = new LineString();
                //ls.Add(currLeg.RollOutPrj);
                //ls.Add(ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 10000.0));
                //Env.Current.AranGraphics.DrawLineString(ls, -1, 2);
                //Application.DoEvents();

                //ls = new LineString();
                //ls.Add(currLeg.StartPrj);
                //ls.Add(ARANFunctions.LocalToPrj(currLeg.StartPrj, prevLeg.StartDir, -10000.0));
                //Env.Current.AranGraphics.DrawLineString(ls, -1, 2);
                //Application.DoEvents();

                //ls = new LineString();
                //ls.Add(ptFIX);
                //ls.Add(ARANFunctions.LocalToPrj(ptFIX, bisect, 10000.0));
                //Env.Current.AranGraphics.DrawLineString(ls, -1, 2);
                //Application.DoEvents();

                Point ptInEnd = ARANFunctions.LocalToPrj(ptCnt1, currLeg.StartDir, 0, -turnDir * rIn);

                //Env.Current.AranGraphics.DrawPointWithText(ptInEnd, -1, "Inner End");
                //Application.DoEvents();

                double x = Math.Sqrt(rIn * rIn - prevLeg.RNPvalue * prevLeg.RNPvalue);
                Point ptInStart = ARANFunctions.LocalToPrj(ptCnt1, bisect, -x, turnDir * prevLeg.RNPvalue);
                //Env.Current.AranGraphics.DrawPointWithText(ptInStart, -1, "Inner Start-1");
                //while(true)
                //Application.DoEvents();

                //Point ptOutStart = ARANFunctions.LocalToPrj(ptFIX, currLeg.ExitDir + Math.PI, 2.0 * currLeg.RNPvalue);

                Point ptOutEnd = ARANFunctions.LocalToPrj(ptFIX, currLeg.StartDir, 0, -2.0 * turnDir * currLeg.RNPvalue);
                //Env.Current.AranGraphics.DrawPointWithText(ptOutEnd, -1, "Outer End");
                //Application.DoEvents();

                x = Math.Sqrt(4 * currLeg.RNPvalue * currLeg.RNPvalue - prevLeg.RNPvalue * prevLeg.RNPvalue);
                Point ptOutStart = ARANFunctions.LocalToPrj(ptFIX, bisect, -x, turnDir * prevLeg.RNPvalue);
                //Env.Current.AranGraphics.DrawPointWithText(ptOutStart, -1, "Outer Start-1");
                //Application.DoEvents();

                Point ptOutPrev = ARANFunctions.LocalToPrj(ptCnt1, currLeg.StartDir, 0, -turnDir * (rIn + 4.0 * currLeg.RNPvalue));
                //Env.Current.AranGraphics.DrawPointWithText(ptOutPrev, -1, "Outer Prev");
                //Application.DoEvents();

                //Env.Current.AranGraphics.DrawPointWithText(ptCnt1, -1, "ptCnt1");
                //Application.DoEvents();

                ring = new Ring();
                ls = ARANFunctions.CreateArcAsPartPrj(ptCnt1, ptInStart, ptInEnd, prevLeg.TurnDir);
                ring.AddReverse(ls);

                ls = ARANFunctions.CreateArcAsPartPrj(ptFIX, ptOutStart, ptOutEnd, prevLeg.TurnDir);
                ring.AddMultiPoint(ls);
                ring.Add(ptOutPrev);

                pPoly = new Polygon();
                pPoly.ExteriorRing = ring;

                //Env.Current.AranGraphics.DrawPolygon(pPoly, -1, eFillStyle.sfsBackwardDiagonal);
                ////while(true)
                //Application.DoEvents();

                currLeg.Protection.Add(pPoly);

                //Env.Current.AranGraphics.DrawPolygon(pPoly, -1, eFillStyle.sfsBackwardDiagonal);
                ////while(true)
                //Application.DoEvents();

                // cutter Line ==================================

                Point lineStart = ARANFunctions.LocalToPrj(ptCnt1, bisect, 0, turnDir * prevLeg.RNPvalue);
                Point lineEnd = ARANFunctions.LocalToPrj(ptFIX, bisect, -2.0 * (prevLeg.Radius + prevLeg.RNPvalue + currLeg.RNPvalue), turnDir * prevLeg.RNPvalue);

                //Env.Current.AranGraphics.DrawPointWithText(lineStart, -1, "line Start");
                //Env.Current.AranGraphics.DrawPointWithText(lineEnd, -1, "line End ");
                ///while(true)
                //Application.DoEvents();

                LineString cutLine = new LineString();
                cutLine.Add(lineStart);
                cutLine.Add(lineEnd);

                //Env.Current.AranGraphics.DrawLineString(cutLine, 255, 2);
                //Application.DoEvents();

                GeometryOperators geoOps = new GeometryOperators();
                //geoOps.CurrentGeometry = currLeg.Protection;

                //if (!geoOps.Disjoint(currLeg.Protection, cutLine))
                if (geoOps.Crosses(currLeg.Protection, cutLine))
                {
                    MultiPolygon cutPoly, cutPoly0, cutPoly1;
                    Geometry geomLeft, geomRight;

                    geoOps.Cut(currLeg.Protection, cutLine, out geomLeft, out geomRight);
                    cutPoly0 = ((MultiPolygon)geomLeft);
                    cutPoly1 = ((MultiPolygon)geomRight);

                    if (prevLeg.TurnDir == PANDA.Common.TurnDirection.CCW)
                        cutPoly = cutPoly0;
                    else
                        cutPoly = cutPoly1;

                    //if (cutPoly0.Area > cutPoly1.Area)
                    //    cutPoly = cutPoly0;
                    //else
                    //    cutPoly = cutPoly1;

                    //Env.Current.AranGraphics.DrawMultiPolygon(cutPoly, -1, eFillStyle.sfsHorizontal);
                    //Application.DoEvents();

                    currLeg.Protection = cutPoly;
                }

                // appendix ==================================

                //Env.Current.AranGraphics.DrawPointWithText(ptOutStart, -1, "Outer Start");
                //Env.Current.AranGraphics.DrawPointWithText(ptOutEnd, -1, "Outer End");
                //Application.DoEvents();

                //ptOutEnd = ptOutStart;
                //Env.Current.AranGraphics.DrawPointWithText(ptOutEnd, -1, "Outer End");
                //Application.DoEvents();
                ptOutStart = ARANFunctions.LocalToPrj(ptFIX, prevLeg.StartDir, 0, -2.0 * turnDir * currLeg.RNPvalue);
                //Env.Current.AranGraphics.DrawPointWithText(ptOutStart, -1, "Outer Start-2");
                ////while(true)
                //Application.DoEvents();

                //x = currLeg.RNPvalue * ARANMath.C_SQRT3;
                ring = new Ring();

                ls = ARANFunctions.CreateArcAsPartPrj(ptFIX, ptOutStart, ptOutEnd, prevLeg.TurnDir);
                //Env.Current.AranGraphics.DrawLineString(ls, -1, 2);
                //Application.DoEvents();
                ring.AddMultiPoint(ls);
                ring.Add(ptFIX);
                ring.Add(ARANFunctions.LocalToPrj(ptFIX, prevLeg.StartDir, 0.0, turnDir * 2.0 * currLeg.RNPvalue));

                ring.Add(ARANFunctions.LocalToPrj(ptFIX, prevLeg.StartDir, -prevLeg.RNPvalue, turnDir * 2.0 * currLeg.RNPvalue));
                ring.Add(ARANFunctions.LocalToPrj(ptFIX, prevLeg.StartDir, -prevLeg.RNPvalue, -turnDir * 2.0 * currLeg.RNPvalue));

                //Env.Current.AranGraphics.DrawPointWithText(ARANFunctions.LocalToPrj(ptFIX, prevLeg.StartDir, -prevLeg.RNPvalue, -turnDir * 2.0 * currLeg.RNPvalue), -1, "ptr");
                //Application.DoEvents();
                //Env.Current.AranGraphics.DrawRing(ring, -1, eFillStyle.sfsHorizontal);
                //while(true)
                //Application.DoEvents();

                Polygon appendix = new Polygon();
                appendix.ExteriorRing = ring;

                currLeg.Protection = (MultiPolygon)geoOps.UnionGeometry(currLeg.Protection, appendix);

                //Point ptOutStart = ARANFunctions.LocalToPrj(ptFIX, bisect, -x, turnDir * currLeg.RNPvalue);

                //Env.Current.AranGraphics.DrawPolygon(cutPoly, -1, eFillStyle.sfsVertical);
                //Application.DoEvents();

                //geoOps.CurrentGeometry = currLeg.Protection;
                //if (!geoOps.Disjoint(currLeg.Protection))
                //{
                //MultiPolygon result = (MultiPolygon)geoOps.Difference(cutPoly, currLeg.Protection);
                //Env.Current.AranGraphics.DrawMultiPolygon(result, -1, eFillStyle.sfsCross);
                //while(true)
                //Application.DoEvents();

                //MultiPolygon result1 = (MultiPolygon)geoOps.Difference(currLeg.Protection, cutPoly);
                //currLeg.Protection = result1;
                //Env.Current.AranGraphics.DrawMultiPolygon(result1 , -1, eFillStyle.sfsDiagonalCross);
                //Application.DoEvents();
                //currLeg.Protection
                //}

                //===================================================

                ls = currLeg.Nominal[1];

                //Env.Current.AranGraphics.DrawLineString(ls, -1, 2);
                //Application.DoEvents();

                //currLeg.Nominal.Remove(1);
                //if (prevLeg.TurnDir== TurnDirection.CCW)
                ls[0] = prevLeg.RollOutPrj;
                //else
                //	ls[1] = prevLeg.RollOutPrj;

                //Env.Current.AranGraphics.DrawLineString(ls, -1, 2);
                //Application.DoEvents();

                currLeg.Nominal.Clear();
                currLeg.Nominal.Add(ls);

                //Env.Current.AranGraphics.DrawMultiLineString(currLeg.Nominal, 255, 2);
                //Application.DoEvents();
                x = Math.Sqrt(prevLeg.Radius * prevLeg.Radius - currLeg.RNPvalue * currLeg.RNPvalue);
                Point ptStart = ARANFunctions.LocalToPrj(prevLeg.Center, bisect, -x, turnDir * currLeg.RNPvalue);

                //Env.Current.AranGraphics.DrawPointWithText(ptStart, -1, "");
                //Application.DoEvents();

                ls = ARANFunctions.CreateArcAsPartPrj(prevLeg.Center, prevLeg.StartPrj, prevLeg.RollOutPrj, prevLeg.TurnDir);

                //Env.Current.AranGraphics.DrawLineString(ls, 255, 2);
                //Application.DoEvents();

                currLeg.Nominal.Add(ls);
                currLeg.StartAltitude = currLeg.RollOutAltitude + ls.Length * currLeg.DescentGR;

                //===================================================

            }
        }

        private void CreateImASStraightLeg(RFLeg nextLeg, ref RFLeg currLeg, RFLeg prevLeg = default(RFLeg), bool prevIsValid = false)
        {
            CreateNextLeg(nextLeg, ref currLeg);

            Ring ring;
            Polygon pPoly;
            LineString ls;

            ring = new Ring();
            ring.Add(ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, 2.0 * currLeg.RNPvalue));
            ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, 2.0 * currLeg.RNPvalue));
            ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, -2.0 * currLeg.RNPvalue));
            ring.Add(ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, -2.0 * currLeg.RNPvalue));

            pPoly = new Polygon();
            pPoly.ExteriorRing = ring;
            currLeg.Protection.Add(pPoly);

            //===================================================
            ls = new LineString();
            ls.Add(currLeg.RollOutPrj);
            ls.Add(nextLeg.StartPrj);
            currLeg.Nominal.Add(ls);
            currLeg.StartAltitude = currLeg.RollOutAltitude + ls.Length * currLeg.DescentGR;

            if (!prevIsValid)
                return;

            CreatePrevLeg(ref currLeg, prevLeg);
        }

        private void CreateImASRFLeg(RFLeg nextLeg, ref RFLeg currLeg, RFLeg prevLeg = default(RFLeg), bool prevIsValid = false)
        {
            CreateNextLeg(nextLeg, ref currLeg);

            Ring ring;
            Polygon pPoly;
            LineString ls;
            Point from, to;
            int turnDir = (int)currLeg.TurnDir;
            //===================================================
            currLeg.Center = ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, turnDir * currLeg.Radius);
            currLeg.StartPrj = ARANFunctions.LocalToPrj(currLeg.Center, currLeg.StartDir, 0.0, -turnDir * currLeg.Radius);
            currLeg.StartGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(currLeg.StartPrj);

            ring = new Ring();

            from = ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.StartDir, 0.0, 2.0 * currLeg.RNPvalue);
            to = ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, 2.0 * currLeg.RNPvalue);

            //Env.Current.AranGraphics.DrawPointWithText(from,  "from");
            //Env.Current.AranGraphics.DrawPointWithText(to, "_|_");
            //Env.Current.AranGraphics.DrawPointWithText(currLeg.Center, "_C_");
            //Env.Current.AranGraphics.DrawMultiPolygon(currLeg.Protection, eFillStyle.sfsBackwardDiagonal);
      //      Application.DoEvents();

            ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, from, to, currLeg.TurnDir);
            ring.AddMultiPoint(ls);

            from = ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.StartDir, 0.0, -2.0 * currLeg.RNPvalue);
            to = ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, -2.0 * currLeg.RNPvalue);

            ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, from, to, currLeg.TurnDir);
            ring.AddReverse(ls);

            //ring.IsExterior = true;

            pPoly = new Polygon();
            pPoly.ExteriorRing = ring;

            //Env.Current.AranGraphics.DrawPolygon(pPoly, -1, eFillStyle.sfsBackwardDiagonal);
            //Application.DoEvents();

            currLeg.Protection.Add(pPoly);

            //===================================================

            ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, currLeg.StartPrj, currLeg.RollOutPrj, currLeg.TurnDir);

            //Env.Current.AranGraphics.DrawLineString(ls, -1, 2);
            //Application.DoEvents();

            currLeg.Nominal.Add(ls);
            currLeg.StartAltitude = currLeg.RollOutAltitude + ls.Length * currLeg.DescentGR;
            //===================================================
            if (!prevIsValid)
                return;

            //Env.Current.AranGraphics.DrawMultiPolygon(currLeg.Protection, -1, eFillStyle.sfsForwardDiagonal);
            //Application.DoEvents();

            CreatePrevLeg(ref currLeg, prevLeg);
        }

        private void CreateIImASFlyByLeg(RFLeg nextLeg, ref RFLeg currLeg, RFLeg prevLeg = default(RFLeg), bool prevIsValid = false)
        {
            //CreateNextLeg(nextLeg, ref currLeg);
            Ring ring;
            Polygon pPoly;
            LineString ls;

            int turnDir = (int)currLeg.TurnDir;
            //===================================================

            currLeg.Center = ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, turnDir * currLeg.Radius);
            currLeg.StartPrj = ARANFunctions.LocalToPrj(currLeg.Center, currLeg.StartDir, 0.0, -turnDir * currLeg.Radius);
            currLeg.StartGeo =  Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(currLeg.StartPrj);
            //Env.Current.AranGraphics.DrawPointWithText(currLeg.Center, "Center");

            Point ptFIX = nextLeg.StartPrj;

            double turnangle = turnDir * (currLeg.RollOutDir - currLeg.StartDir);// +2 * Math.PI;
            if (turnangle < 0.0)
                turnangle += 2.0 * Math.PI;

            double bisect = currLeg.RollOutDir + 0.5 * turnDir * (Math.PI - turnangle);

            //double bisect = 0.5 * (currLeg.StartDir + currLeg.RollOutDir + turnDir * Math.PI);
            //bisect = 0.5 * (currLeg.StartDir + currLeg.RollOutDir + Math.PI) + Math.PI;
            //bisect = currLeg.RollOutDir + 0.5 * turnDir * (Math.PI - turnangle);
            //ls = new LineString();
            //ls.Add(ptFIX);
            //ls.Add(ARANFunctions.LocalToPrj(ptFIX, bisect, 10000.0));
            //Env.Current.AranGraphics.DrawLineString(ls, -1, 2);
            //Env.Current.AranGraphics.DrawPointWithText(ptFIX, -1, "FIX");
            ////while(true)
            //Application.DoEvents();

            double rIn = currLeg.Radius + currLeg.RNPvalue;
            double l1 = (currLeg.Radius + 3.0 * currLeg.RNPvalue) / Math.Cos(0.5 * turnangle);
            Point ptCnt1 = ARANFunctions.LocalToPrj(ptFIX, bisect, l1);

            //MultiPolygon innerCircle = ARANFunctions.CreateCircleAsMultiPolyPrj(ptCnt1, currLeg.Radius + currLeg.RNPvalue);
            //Env.Current.AranGraphics.DrawMultiPolygon(innerCircle, -1, eFillStyle.sfsForwardDiagonal);
            //MultiPolygon outerCircle = ARANFunctions.CreateCircleAsMultiPolyPrj(ptFIX, 2.0 * currLeg.RNPvalue);
            //Env.Current.AranGraphics.DrawMultiPolygon(outerCircle, -1, eFillStyle.sfsBackwardDiagonal);

            //Env.Current.AranGraphics.DrawPointWithText(currLeg.EndPrj, -1, "End");
            //Env.Current.AranGraphics.DrawPointWithText(currLeg.StartPrj, -1, "Start");

            //Env.Current.AranGraphics.DrawPointWithText(ptFIX, -1, "FIX");
            //Env.Current.AranGraphics.DrawPointWithText(ptCnt1, -1, "Cnt-1");
            ////while(true)
            //Application.DoEvents();

            Point ptInStart = ARANFunctions.LocalToPrj(ptCnt1, currLeg.StartDir, 0, -turnDir * rIn);

            double x = Math.Sqrt(rIn * rIn - currLeg.RNPvalue * currLeg.RNPvalue);
            Point ptInEnd = ARANFunctions.LocalToPrj(ptCnt1, bisect, -x, -turnDir * currLeg.RNPvalue);

            //Env.Current.AranGraphics.DrawPointWithText(ptInStart, -1, "Inner Start");
            //Env.Current.AranGraphics.DrawPointWithText(ptInEnd, -1, "Inner End");
            ////while(true)
            //Application.DoEvents();

            //Point ptOutStart = ARANFunctions.LocalToPrj(ptFIX, currLeg.ExitDir + Math.PI, 2.0 * currLeg.RNPvalue);
            Point ptOutStart = ARANFunctions.LocalToPrj(ptFIX, currLeg.StartDir, 0, -2.0 * turnDir * currLeg.RNPvalue);
            //Env.Current.AranGraphics.DrawPointWithText(ptOutStart, -1, "Outer Start");
            //Application.DoEvents();

            x = currLeg.RNPvalue * ARANMath.C_SQRT3;
            Point ptOutEnd = ARANFunctions.LocalToPrj(ptFIX, bisect, -x, -turnDir * currLeg.RNPvalue);
            //Env.Current.AranGraphics.DrawPointWithText(ptOutEnd, -1, "Outer End");
            //Application.DoEvents();

            Point ptOutPrev = ARANFunctions.LocalToPrj(ptCnt1, currLeg.StartDir, 0, -turnDir * (rIn + 4.0 * currLeg.RNPvalue));
            //Env.Current.AranGraphics.DrawPointWithText(ptOutPrev, -1, "Outer Prev");
            //Application.DoEvents();

            ring = new Ring();
            ls = ARANFunctions.CreateArcAsPartPrj(ptCnt1, ptInStart, ptInEnd, currLeg.TurnDir);

            //Env.Current.AranGraphics.DrawLineString(ls, -1, 2);
            //Application.DoEvents();

            ring.AddMultiPoint(ls);

            ls = ARANFunctions.CreateArcAsPartPrj(ptFIX, ptOutStart, ptOutEnd, currLeg.TurnDir);

            //Env.Current.AranGraphics.DrawLineString(ls, -1, 2);
            //Application.DoEvents();

            ring.AddReverse(ls);
            ring.Add(ptOutPrev);


            pPoly = new Polygon();
            pPoly.ExteriorRing = ring;
            currLeg.Protection.Add(pPoly);

            //Env.Current.AranGraphics.DrawPolygon(pPoly, -1, eFillStyle.sfsBackwardDiagonal);
            //while(true)
            //Application.DoEvents();

            // cutter Polygon ==================================

            Ring cutRing = new Ring();
            cutRing.Add(ARANFunctions.LocalToPrj(ptFIX, currLeg.StartDir, currLeg.RNPvalue, 2.0 * currLeg.RNPvalue));
            cutRing.Add(ARANFunctions.LocalToPrj(ptFIX, currLeg.StartDir, currLeg.RNPvalue, -2.0 * currLeg.RNPvalue));
            cutRing.Add(ARANFunctions.LocalToPrj(ptFIX, currLeg.StartDir, 5.0 * currLeg.RNPvalue, -2.0 * currLeg.RNPvalue));
            cutRing.Add(ARANFunctions.LocalToPrj(ptFIX, currLeg.StartDir, 5.0 * currLeg.RNPvalue, 2.0 * currLeg.RNPvalue));

            Polygon cutPoly = new Polygon();
            cutPoly.ExteriorRing = cutRing;

            //Env.Current.AranGraphics.DrawPolygon(cutPoly, -1, eFillStyle.sfsHorizontal);
            //Application.DoEvents();

            // cutter Line ==================================
            LineString cutLine = new LineString();
            cutLine.Add(ptCnt1);
            cutLine.Add(ARANFunctions.LocalToPrj(ptFIX, bisect + Math.PI, 5.0 * currLeg.RNPvalue));

            //Env.Current.AranGraphics.DrawLineString(cutLine, 255, 2);
            //Application.DoEvents();

            GeometryOperators geoOps = new GeometryOperators();
            //geoOps.CurrentGeometry = cutPoly;

            if (geoOps.Crosses(cutPoly, cutLine))
            {
                Geometry geomLeft, geomRight;
                geoOps.Cut(cutPoly, cutLine, out geomLeft, out geomRight);

                if (currLeg.TurnDir == Aran.PANDA.Common.TurnDirection.CCW)
                    cutPoly = ((MultiPolygon)geomLeft)[0];
                else
                    cutPoly = ((MultiPolygon)geomRight)[0];
            }

            //Env.Current.AranGraphics.DrawPolygon(cutPoly, -1, eFillStyle.sfsVertical);
            //Application.DoEvents();

            //geoOps.CurrentGeometry = cutPoly;
            if (geoOps.Crosses(cutPoly, currLeg.Protection))
            {
                //MultiPolygon result = (MultiPolygon)geoOps.Difference(cutPoly, currLeg.Protection);
                //Env.Current.AranGraphics.DrawMultiPolygon(result, -1, eFillStyle.sfsCross);
                //while(true)
                //Application.DoEvents();

                MultiPolygon result1 = (MultiPolygon)geoOps.Difference(currLeg.Protection, cutPoly);
                currLeg.Protection = result1;
                //Env.Current.AranGraphics.DrawMultiPolygon(result1 , -1, eFillStyle.sfsDiagonalCross);
                //Application.DoEvents();
                //currLeg.Protection
            }

            //Ring eraliestBisectorRing = new Ring();
            //eraliestBisectorRing.Add(ARANFunctions.LocalToPrj() ); 

            //Polygon eraliestBisectorPoly = new Polygon();

            //Env.Current.AranGraphics.DrawMultiPolygon(innerCircle, -1, eFillStyle.sfsForwardDiagonal);
            //Env.Current.AranGraphics.DrawMultiPolygon(outerCircle, -1, eFillStyle.sfsBackwardDiagonal);
            //Application.DoEvents();

            //ring = new Ring();

            //from = ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.EntryDir, 0.0, 2.0 * currLeg.RNPvalue);
            //to = ARANFunctions.LocalToPrj(currLeg.EndPrj, currLeg.ExitDir, 0.0, 2.0 * currLeg.RNPvalue);

            //ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, from, to, currLeg.TurnDir);
            //ring.AddMultiPoint(ls);

            //from = ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.EntryDir, 0.0, -2.0 * currLeg.RNPvalue);
            //to = ARANFunctions.LocalToPrj(currLeg.EndPrj, currLeg.ExitDir, 0.0, -2.0 * currLeg.RNPvalue);

            //ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, from, to, currLeg.TurnDir);
            //ring.AddReverse(ls);


            //===================================================

            ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, currLeg.StartPrj, currLeg.RollOutPrj, currLeg.TurnDir);

            //Env.Current.AranGraphics.DrawLineString(ls, 255, 2);
            //Application.DoEvents();

            currLeg.Nominal.Add(ls);
            currLeg.StartAltitude = currLeg.RollOutAltitude + ls.Length * currLeg.DescentGR;

            //===================================================
            //if (!prevIsValid)
            //	return;
            //CreatePrevLeg(ref currLeg, prevLeg);
        }

        public void SetDescentGradient(double dg)
        {
            _dg = dg;
            _CurrImASLeg.DescentGR = System.Math.Tan(ARANMath.DegToRad(dg));

        }
        public void AddLeg(bool next = true)
        {
       
            /*********************************************************************************************/
            NativeMethods.ShowPandaBox(Context.AppEnvironment.Current.SystemContext.EnvWin32Window.ToInt32());

            Functions.GetObstaclesByPolygonWithDecomposition(Env.Current.DataContext.ObstacleList, out CurrlegObsatclesList, _CurrImASLeg.Protection);

            int n = CurrlegObsatclesList.Parts.Length;
            for (int i = 0; i < n; i++)
            {
                CurrlegObsatclesList.Parts[i].MOC = _CurrImASLeg.MOC;
                CurrlegObsatclesList.Parts[i].ReqH = CurrlegObsatclesList.Parts[i].Height + _CurrImASLeg.MOC;
                CurrlegObsatclesList.Parts[i].Elev = CurrlegObsatclesList.Parts[i].Height + PreFinalState.ptTHRprj.Z;
            }

            _CurrImASLeg.ObstaclesList = CurrlegObsatclesList;

            Env.Current.RNPContext.ReportForm.FillPage07(CurrlegObsatclesList);
            NativeMethods.HidePandaBox();

            /************************************************************************************************************/


            _ImASLegs.Add(_CurrImASLeg);

            if (next)
            {
                RFLeg nextLeg = _CurrImASLeg;
                _ptNextprj = nextLeg.StartPrj;
                _ptNextprj.Z = nextLeg.StartAltitude;
                _hNext = _ptNextprj.Z;

                if (_ImASLegs.Count == 1)
                    _dg = DGInit;
                _CurrImASLeg = new RFLeg
                {
                    Nominal = new MultiLineString(),
                    Protection = new MultiPolygon(),
                    DescentGR = _ImASLegs.Count > 1 ? nextLeg.DescentGR : Math.Tan(ARANMath.DegToRad(_dg)),
                    RNPvalue = nextLeg.RNPvalue,
                    BankAngle = nextLeg.BankAngle,
                    DistToNext = nextLeg.DistToNext,
                    Radius = nextLeg.Radius,
                    TurnDir = nextLeg.TurnDir,
                    IAS = nextLeg.IAS,
                    TAS = nextLeg.TAS,
                    RollOutDir = nextLeg.StartDir,
                    RollOutAltitude = nextLeg.StartAltitude,
                    legType = nextLeg.legType,
                    MOC = nextLeg.MOC,
                    IsEndWpt = nextLeg.IsWpt,
                    endWpt = nextLeg.startWpt
                };



                switch (nextLeg.legType)
                {
                    case LegType.FlyBy:
                    case LegType.FixedRadius:
                        LegTypeChanged(LegType.Straight);
                        break;
                }

                TurnDirectionChanged(TurnDirection);
                CreateImASLeg();
            }
        }

        public void RemoveLeg()
        {
            if (_ImASLegs.Any())
            {
                _ImASLegs.RemoveAt(_ImASLegs.Count - 1);
            }
        }

        public List<WPT_FIXType> GetRfWptList(WPT_FIXType[] waypoints)
        {
            double exitCourse = ARANFunctions.DirToAzimuth(_CurrImASLeg.RollOutPrj, _CurrImASLeg.RollOutDir,
                Env.Current.SpatialContext.SpatialReferenceProjection, Env.Current.SpatialContext.SpatialReferenceGeo);
            return Functions.GetRfWptList(_CurrImASLeg, waypoints,
                CalcImASRadius(_CurrImASLeg.TAS, _CurrImASLeg.RollOutAltitude - CurrAdhpElev, FinalState._FASmaxBank),
                CalcImASRadius(_CurrImASLeg.TAS, _CurrImASLeg.RollOutAltitude - CurrAdhpElev, 1.0), exitCourse, _ImASturndir);
        }

        public List<WPT_FIXType> GetFlyByWptList(WPT_FIXType[] waypoints)
        {

            double fE = Env.Current.RNPContext.ErrorAngle;
            var result = new List<WPT_FIXType>();
            double exitCourse = ARANFunctions.DirToAzimuth(_CurrImASLeg.RollOutPrj, _CurrImASLeg.RollOutDir,
                Env.Current.SpatialContext.SpatialReferenceProjection, Env.Current.SpatialContext.SpatialReferenceGeo);
            var dMax = CalcFlyByEntryCourseDelta();
            foreach (var wpt in waypoints)
            {
                double course, inverseCourse;
                ARANFunctions.ReturnGeodesicAzimuth(_CurrImASLeg.RollOutGeo, wpt.pPtGeo, out course, out inverseCourse);
                double dAzt = NativeMethods.Modulus(_ImASturndir * (inverseCourse - exitCourse));

                if (dAzt > dMax || (System.Math.Abs(Math.Sin(dAzt)) <= fE) && (Math.Cos(dAzt) > 0))
                    continue;
                result.Add(wpt);
            }

            return result;

        }


        public List<WPT_FIXType> GetStraightWptList(WPT_FIXType[] waypoints)
        {
            return Functions.GetStraightWptList(_ptNextprj, _CurrImASLeg.RollOutDir, waypoints, _CurrImASLeg.RNPvalue,
                _MaxDistance);
        }

        public void RfWptSelected(WPT_FIXType wpt)
        {
            _CurrImASLeg.IsWpt = true;
            _CurrImASLeg.startWpt = wpt;

            double dX = (_CurrImASLeg.RollOutPrj.X + wpt.pPtPrj.X) / 2;
            double dY = (_CurrImASLeg.RollOutPrj.Y + wpt.pPtPrj.Y) / 2;
            var wptDir = ARANFunctions.ReturnAngleInRadians(_CurrImASLeg.RollOutPrj, wpt.pPtPrj);
            var center = ARANFunctions.LineLineIntersect(_CurrImASLeg.RollOutPrj,
                _CurrImASLeg.RollOutDir + _ImASturndir * ARANMath.C_PI_2,
                new Point(dX, dY), wptDir + ARANMath.C_PI_2) as Point;
            double r = ARANFunctions.ReturnDistanceInMeters(_CurrImASLeg.RollOutPrj, center);


            _CurrImASLeg.Radius = r;
            _CurrImASLeg.BankAngle = CalcImASBank(_CurrImASLeg.TAS, _CurrImASLeg.RollOutAltitude - CurrAdhpElev, _CurrImASLeg.Radius);
            double dir = ARANFunctions.ReturnAngleInRadians(center, wpt.pPtPrj) + _ImASturndir * ARANMath.C_PI_2;
            var course = ARANFunctions.DirToAzimuth(center, dir, Env.Current.SpatialContext.SpatialReferenceProjection,
                Env.Current.SpatialContext.SpatialReferenceGeo);
            _CurrImASLeg.StartDir = dir;
            _CurrImASLeg.Course = course;
        }

        public void FlyByWptSelected(WPT_FIXType wpt)
        {
            _CurrImASLeg.IsWpt = true;
            _CurrImASLeg.startWpt = wpt;

            var wptDir = ARANFunctions.ReturnAngleInRadians(wpt.pPtPrj, NextLeg.StartPrj);
            var course = ARANFunctions.DirToAzimuth(_CurrImASLeg.RollOutPrj, wptDir, Env.Current.SpatialContext.SpatialReferenceProjection,
                Env.Current.SpatialContext.SpatialReferenceGeo);
            _CurrImASLeg.StartDir = wptDir;
            _CurrImASLeg.Course = course;
        }

        public void StraightWptSelected(WPT_FIXType wpt)
        {
            _CurrImASLeg.IsWpt = true;
            _CurrImASLeg.startWpt = wpt;
            var leg = _ImASLegs.Count > 0 ? _ImASLegs[_ImASLegs.Count - 1] : FinalState._CurrFASLeg;
            _CurrImASLeg.DistToNext = ARANFunctions.ReturnDistanceInMeters(leg.StartPrj, wpt.pPtPrj);
        }
    
    }
}
