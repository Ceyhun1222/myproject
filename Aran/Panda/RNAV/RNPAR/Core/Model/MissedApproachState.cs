using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    class MissedApproachState : State<MissedApproachState>
    {
        public MissedApproachState()
        {
            _type = RNPAR.Model.StateType.MissedApproach;
        }

        protected override MissedApproachState CreateNewState()
        {
            return new MissedApproachState();
        }

        public override MissedApproachState Copy()
        {
            var intermediateState = BaseCopy();
            intermediateState.CurrlegObsatclesList = CurrlegObsatclesList;
            intermediateState._MASmaxRadius = _MASmaxRadius;
            intermediateState._MASminRadius = _MASminRadius;
            intermediateState._MASturndir = _MASturndir;
            intermediateState._CurrMASLeg = _CurrMASLeg;
            intermediateState._MASLegs = new List<RFLeg>();
            intermediateState._MASLegs.AddRange(_MASLegs);
            intermediateState.TrackDistance = TrackDistance;
            intermediateState._ptNextprj = _ptNextprj;
            intermediateState._hNext = _hNext;
            intermediateState._dg = _dg;

            return intermediateState;
        }

        public override void Commit()
        {
            AddLeg();
        }


        public override void Clear()
        {
            int n = _MASLegs.Count;

            for (int i = 0; i < n; i++)
            {
                Env.Current.AranGraphics.SafeDeleteGraphic(_MASLegs[i].FixElem);
                Env.Current.AranGraphics.SafeDeleteGraphic(_MASLegs[i].NominalElem);
                Env.Current.AranGraphics.SafeDeleteGraphic(_MASLegs[i].ProtectionElem);
            }

            Env.Current.AranGraphics.SafeDeleteGraphic(_CurrMASLeg.FixElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_CurrMASLeg.NominalElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_CurrMASLeg.ProtectionElem);
        }

        public override void ReCreate()
        {
            int n = _MASLegs.Count;
            for (int i = 0; i < n; i++)
            {
                var leg = _MASLegs[i];
                Draw(ref leg);
                _MASLegs[i] = leg;
            }

            Draw(ref _CurrMASLeg);

            TurnDirectionChanged(TurnDirection);
        }



        public ObstacleContainer CurrlegObsatclesList;

        public double DGMax => 2.5;
        public double DGInit => 2.5;

        public double _MASminRadius;
        public double _MASmaxRadius;
        public double _dg;

        public int _MASturndir;
        public RFLeg _CurrMASLeg;
        public List<RFLeg> _MASLegs;
        public double TrackDistance;
        public double CurrAdhpElev => Env.Current.DataContext.CurrADHP.Elev;
        public Point _ptNextprj;
        private double _hNext;

        private const double _MaxDistance = 15.0 * 1852.0;
        private const int FlyByBankAngle = 18;

        public bool RfLegEnabled => NextLeg.legType != LegType.FlyBy;
        public bool FlyByLegEnabled => (NextLeg.legType == LegType.Straight) && FlyByDTACorrect() && _MASLegs.Count > 0;
        public bool StraightLegEnabled { get; private set; } = true;
        public double FlyByCourseDeltaMax => 120;
        public double RfCourseDeltaMax => 240;


        public PreFinalState PreFinalState => Env.Current.RNPContext.PreFinalPhase.CurrentState;
        public IntermediateState IntermediateState => Env.Current.RNPContext.IntermediatePhase.CurrentState;
        public FinalState FinalState => Env.Current.RNPContext.FinalPhase.CurrentState;

        public int TurnDirection => (1 - _MASturndir) / 2;

        public RFLeg NextLeg => _MASLegs.Count > 0 ? _MASLegs[_MASLegs.Count - 1] : default(RFLeg);


        public void Init()
        {


            _ptNextprj = PreFinalState.ptMas0;
            _ptNextprj.Z = ARANFunctions.ReturnDistanceInMeters(PreFinalState.ptSOCprj, _ptNextprj) * System.Math.Tan(ARANMath.DegToRad(DGInit));
            _hNext = _ptNextprj.Z;
            //=========================================================================================
            //GlobalVars.constants.AircraftCategory[aircraftCategoryData.VatMax].Value[_Category]
            //GlobalVars.constants.AircraftCategory[aircraftCategoryData.VatMax].Value[_Category]
            //ViafMin, ViafMax


            double ias = Env.Current.UnitContext.Constants.AircraftCategory[aircraftCategoryData.ViafMax].Value[PreFinalState._Category];

            double tas = 3.6 * ARANMath.IASToTAS(ias, CurrAdhpElev + _hNext, 0.0);

            _MASminRadius = CalcMASRadius(tas, _hNext - CurrAdhpElev, 20.0);
            if (_MASminRadius <= 2.0 * PreFinalState._FASRNPval)
                _MASminRadius = Math.Ceiling(2.01 * PreFinalState._FASRNPval);

            _MASmaxRadius = FinalState.CalcRadius(_hNext - CurrAdhpElev, 1.0);

            //=========================================================================================
            _MASLegs = new List<RFLeg>();
            _dg = DGInit;
            _CurrMASLeg = new RFLeg
            {
                DescentGR = Math.Tan(ARANMath.DegToRad(DGInit)),
                IAS = ias,
                TAS = tas,
                BankAngle = FinalState._FASmaxBank,
                DistToNext = 1852.0,
                RollOutAltitude = _hNext,
                Radius = _MASmaxRadius,
                RollOutDir = NativeMethods.Modulus(ARANMath.C_PI + PreFinalState._ArDir, ARANMath.C_2xPI),
                RollOutPrj = new Point(PreFinalState.ptMas0),
                RNPvalue = PreFinalState._MARNPval,
                MOC = 0
            };
            _CurrMASLeg.RollOutGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(_CurrMASLeg.RollOutPrj);
            _CurrMASLeg.DistToNext = _CurrMASLeg.RNPvalue;
            _CurrMASLeg.Nominal = new MultiLineString();
            _CurrMASLeg.Protection = new MultiPolygon();
            

            TurnDirectionChanged(TurnDirection);

            LegTypeChanged(LegType.FixedRadius);

            CheckMASRadius();

            EntryCourseChanged(_CurrMASLeg.Course);

            PreFinalState.DrawMasProtArea();
            CreateMASLeg();

        }

        public void IasChanged(double ias)
        {

            _CurrMASLeg.IAS = ias;

            double iasMin = Env.Current.UnitContext.Constants.AircraftCategory[aircraftCategoryData.ViafMin].Value[PreFinalState._Category];
            double iasMax = Env.Current.UnitContext.Constants.AircraftCategory[aircraftCategoryData.ViafMax].Value[PreFinalState._Category];

            if (_CurrMASLeg.IAS < iasMin)
                _CurrMASLeg.IAS = iasMin;

            if (_CurrMASLeg.IAS > iasMax)
                _CurrMASLeg.IAS = iasMax;


            _CurrMASLeg.TAS = 3.6 * ARANMath.IASToTAS(_CurrMASLeg.IAS, CurrAdhpElev + _hNext, 0.0);

            _MASminRadius = CalcMASRadius(_CurrMASLeg.TAS, _CurrMASLeg.StartAltitude - CurrAdhpElev, 20.0);
            if (_MASminRadius <= 2.0 * PreFinalState._FASRNPval)
                _MASminRadius = Math.Ceiling(2.01 * PreFinalState._FASRNPval);

            _MASmaxRadius = FinalState.CalcRadius(_CurrMASLeg.StartAltitude - CurrAdhpElev, 1.0);

            CheckMASRadius();

        }

        public void SegmentRnpValueChanged(double rnpNM)
        {

            double RNPval = rnpNM;

            if (RNPval < 0.1)
                RNPval = 0.1;

            if (RNPval > 1.0)
                RNPval = 1.0;

            _CurrMASLeg.RNPvalue = Env.Current.UnitContext.UnitConverter.DistanceFromNM(RNPval);
            if (_CurrMASLeg.DistToNext < _CurrMASLeg.RNPvalue)
            {
                _CurrMASLeg.DistToNext = _CurrMASLeg.RNPvalue;
            }
        }

        public void WptAltitudeChanged(double wpt)
        {


            _CurrMASLeg.StartAltitude = wpt;

            double maxAlt = _CurrMASLeg.RollOutAltitude + _CurrMASLeg.Nominal.Length * _CurrMASLeg.DescentGR;

            if (_CurrMASLeg.StartAltitude < _hNext)
                _CurrMASLeg.StartAltitude = _hNext;

            if (_CurrMASLeg.StartAltitude > maxAlt)
                _CurrMASLeg.StartAltitude = maxAlt;


            _CurrMASLeg.StartPrj.Z = _CurrMASLeg.StartAltitude;
            //_CurrMASLeg.StartGeo.Z = _CurrMASLeg.StartAltitude;

            _MASminRadius = CalcMASRadius(_CurrMASLeg.TAS, _CurrMASLeg.StartAltitude - CurrAdhpElev, 20.0);
            if (_MASminRadius <= 2.0 * PreFinalState._FASRNPval)
                _MASminRadius = Math.Ceiling(2.01 * PreFinalState._FASRNPval);

            _MASmaxRadius = FinalState.CalcRadius(_CurrMASLeg.StartAltitude - CurrAdhpElev, 1.0);
            CheckMASRadius();
        }

        public void MocChanged(double moc)
        {

            _CurrMASLeg.MOC = moc;

            //if (_CurrMASLeg.MOC < 150.0)
            //    _CurrMASLeg.MOC = 150.0;

            //if (_CurrMASLeg.MOC > 300.0)
            //    _CurrMASLeg.MOC = 300.0;
        }

        public void TrackDistanceChanged(double distance)
        {
            _CurrMASLeg.IsWpt = false;
            _CurrMASLeg.DistToNext = distance;
            if (_CurrMASLeg.DistToNext < _CurrMASLeg.RNPvalue)
                _CurrMASLeg.DistToNext = _CurrMASLeg.RNPvalue;

            if (_CurrMASLeg.DistToNext > _MaxDistance)
                _CurrMASLeg.DistToNext = _MaxDistance;

            //_CurrMASLeg.EndPrj = ARANFunctions.LocalToPrj(_ptNextprj, _CurrMASLeg.ExitDir, -_CurrMASLeg.DistToNext);
            //_CurrMASLeg.EndGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(_CurrMASLeg.EndPrj);
            //_CurrMASLeg.StartPrj = new Point(_CurrMASLeg.EndPrj);

            ////_CurrMASLeg.EndAltitude = FAPDist2hFAP(_CurrFASLeg.DistToNext, _hNext);
            ////textBox0203.Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_CurrFASLeg.EndAltitude).ToString();

            //_CurrMASLeg.EndPrj.Z = _CurrMASLeg.EndAltitude;
            //_CurrMASLeg.EndGeo.Z = _CurrMASLeg.EndAltitude;

        }

        public void RadiusChanged(double radius)
        {
            _CurrMASLeg.IsWpt = false;
            _CurrMASLeg.Radius = radius;
            CheckMASRadius();
        }

        public void CheckMASRadius()
        {
            double fTmp = _CurrMASLeg.Radius;

            if (_CurrMASLeg.Radius < _MASminRadius)
                _CurrMASLeg.Radius = _MASminRadius;

            if (_CurrMASLeg.Radius > _MASmaxRadius)
                _CurrMASLeg.Radius = _MASmaxRadius;

            //if (fTmp != _CurrMASLeg.Radius)
            //{
            //    CreateMASLeg();
            //}

            fTmp = _CurrMASLeg.BankAngle;
            _CurrMASLeg.BankAngle = CalcMASBank(_CurrMASLeg.TAS, _CurrMASLeg.RollOutAltitude - CurrAdhpElev, _CurrMASLeg.Radius);

            if (fTmp != _CurrMASLeg.BankAngle)
            {
                CheckMASBank(false);
            }
        }

        public double CalcMASBank(double Vtas, double height, double radius)
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
            _CurrMASLeg.IsWpt = false;
            _CurrMASLeg.BankAngle = angle;
            CheckMASBank();
        }

        public void CheckMASBank(bool create = true)
        {
            if (_CurrMASLeg.BankAngle < 1.0)
                _CurrMASLeg.BankAngle = 1.0;

            if (_CurrMASLeg.BankAngle > FinalState._FASmaxBank)   //_MASmaxBank
                _CurrMASLeg.BankAngle = FinalState._FASmaxBank;

            double fTmp = _CurrMASLeg.Radius;
            _CurrMASLeg.Radius = CalcMASRadius(_CurrMASLeg.TAS, _CurrMASLeg.RollOutAltitude - CurrAdhpElev, _CurrMASLeg.BankAngle);
        }

        public void LegTypeChanged(LegType type)
        {
            _CurrMASLeg.IsWpt = false;
            _CurrMASLeg.legType = type;
            ;
            if (_CurrMASLeg.legType == LegType.FlyBy)
            {

                BankAngleChanged(FlyByBankAngle);
                EntryCourseChanged();
            }

            if (_CurrMASLeg.legType == LegType.FixedRadius)
            {
                MocChanged(0);
                RadiusChanged(_CurrMASLeg.Radius);
                EntryCourseChanged();
            }

            if (_CurrMASLeg.legType == LegType.Straight)
            {
                MocChanged(0);
                TrackDistanceChanged(_CurrMASLeg.DistToNext);
            }

        }

        public void TurnDirectionChanged(int direction)
        {
            _MASturndir = 1 - 2 * direction;
            _CurrMASLeg.TurnDir = (TurnDirection)_MASturndir;
            EntryCourseChanged();
        }

        public void EntryCourseChanged()
        {
            EntryCourseChanged(_CurrMASLeg.Course);
        }

        public void EntryCourseChanged(double course)
        {
            _CurrMASLeg.IsWpt = false;
            _CurrMASLeg.Course = course;
            double ExitCourse = ARANFunctions.DirToAzimuth(_ptNextprj, _CurrMASLeg.RollOutDir, Env.Current.SpatialContext.SpatialReferenceProjection, Env.Current.SpatialContext.SpatialReferenceGeo);
            double dAzt = NativeMethods.Modulus(_MASturndir * (_CurrMASLeg.Course - ExitCourse));

            double dAngle;
            if (_CurrMASLeg.legType == LegType.FixedRadius)
                dAngle = RfCourseDeltaMax;
            else
            {
                dAngle = CalcFlyByEntryCourseDelta();
            }

            if (dAzt > dAngle || dAzt == 0)
                _CurrMASLeg.Course = NativeMethods.Modulus(ExitCourse + _MASturndir * dAngle);


            Point EndGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(_ptNextprj);
            _CurrMASLeg.StartDir = ARANFunctions.AztToDirection(EndGeo, _CurrMASLeg.Course, Env.Current.SpatialContext.SpatialReferenceGeo, Env.Current.SpatialContext.SpatialReferenceProjection);

            if(LegType.FlyBy == _CurrMASLeg.legType)
                FlyByMocChanged(_CurrMASLeg.Course);

        }

        private double CalcFlyByEntryCourseDelta()
        {
            return Math.Min(FlyByCourseDeltaMax, CalcMaxFlyByCourseDeltaDTA());
        }

        private double CalcMaxFlyByCourseDeltaDTA()
        {
            var dMax = ARANMath.RadToDeg(NativeMethods.Modulus(2 * Math.Atan(NextLeg.DistToNext / _CurrMASLeg.Radius)));
            return dMax;
        }

        private double FlyByDTA()
        {
            double ExitCourse = ARANFunctions.DirToAzimuth(_ptNextprj, _CurrMASLeg.RollOutDir, Env.Current.SpatialContext.SpatialReferenceProjection, Env.Current.SpatialContext.SpatialReferenceGeo);
            double dAzt = NativeMethods.Modulus(_MASturndir * (_CurrMASLeg.Course - ExitCourse));
            return _CurrMASLeg.Radius * Math.Tan(dAzt / 2);
        }

        private double FlyByDTA(double radius, double deltaAz)
        {
            return radius * Math.Tan(deltaAz / 2);
        }

        private bool FlyByDTACorrect()
        {
            //double r = CalcMASRadius(_CurrMASLeg.TAS, _CurrMASLeg.RollOutAltitude - CurrAdhpElev, FlyByBankAngle);
            //return NextLeg.DistToNext >= r * Math.Tan(ARANMath.DegToRad(FlyByCourseDeltaMax / 2));
            return true;
        }

        public double CalcMASRadius(double Vtas, double height, double bank)
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

        public void CreateMASLeg()
        {
            Env.Current.AranGraphics.SafeDeleteGraphic(_CurrMASLeg.FixElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_CurrMASLeg.NominalElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_CurrMASLeg.ProtectionElem);

            //==================================================
            _CurrMASLeg.Nominal.Clear();
            _CurrMASLeg.Protection.Clear();

            if (_CurrMASLeg.legType == LegType.FixedRadius)
            {

                _CurrMASLeg.RollOutPrj = (Point)_ptNextprj.Clone();
                _CurrMASLeg.RollOutGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(_CurrMASLeg.RollOutPrj);

                _CurrMASLeg.RollOutPrj.Z = _CurrMASLeg.RollOutAltitude;
                _CurrMASLeg.RollOutGeo.Z = _CurrMASLeg.RollOutAltitude;
                //===

                if (_MASLegs.Count == 0)
                {
                    //RFLeg next = _CurrFASLeg;// _FASLegs[_FASLegs.Count - 1];
                    CreateMASRFLeg(default(RFLeg), ref _CurrMASLeg);
                }
                else
                {
                    RFLeg next = _MASLegs[_MASLegs.Count - 1];
                    CreateMASRFLeg(next, ref _CurrMASLeg);

                    
                    if (_MASLegs.Count > 1)
                    {
                        RFLeg nextNext;
                        nextNext = _MASLegs[_MASLegs.Count - 2];

                        next.Protection.Clear();
                        next.Nominal.Clear();

                        if (next.legType == LegType.FixedRadius)
                            CreateMASRFLeg(nextNext, ref next, _CurrMASLeg, true);
                        else if (next.legType == LegType.FlyBy)
                            CreateIMASFlyByLeg(nextNext, ref next, _CurrMASLeg, true);
                        else
                            CreateMASStraightLeg(nextNext, ref next, _CurrMASLeg, true);
                        Clear(next);

                        Draw(ref next);

                        _MASLegs[_MASLegs.Count - 1] = next;
                    }

                    
                }


                TrackDistance = _CurrMASLeg.Nominal.Length - _CurrMASLeg.RNPvalue;
            }
            else if (_CurrMASLeg.legType == LegType.FlyBy)
            {
                double turnangle = (_CurrMASLeg.RollOutDir - _CurrMASLeg.StartDir) * (int)_CurrMASLeg.TurnDir;

                double ptDist = _CurrMASLeg.Radius * Math.Tan(0.5 * turnangle);

                _CurrMASLeg.RollOutPrj = ARANFunctions.LocalToPrj(_ptNextprj, _CurrMASLeg.RollOutDir, ptDist);        // (Point)_ptNextprj.Clone();
                _CurrMASLeg.RollOutGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(_CurrMASLeg.RollOutPrj);

                _CurrMASLeg.RollOutPrj.Z = _CurrMASLeg.RollOutAltitude;
                _CurrMASLeg.RollOutGeo.Z = _CurrMASLeg.RollOutAltitude;
                //===

                if (_MASLegs.Count == 0)
                    CreateIMASFlyByLeg(new RFLeg{StartPrj = PreFinalState.ptMas0.Clone() as Point}, ref _CurrMASLeg);
                else
                {
                    RFLeg next = _MASLegs[_MASLegs.Count - 1];
                    CreateIMASFlyByLeg(next, ref _CurrMASLeg);

                    //_CurrMASLeg.ProtectionElem =Env.Current.AranGraphics.DrawMultiPolygon(_CurrMASLeg.Protection, -1, eFillStyle.sfsForwardDiagonal);
                    //Application.DoEvents();


                    if (_MASLegs.Count > 1)
                    {
                        RFLeg nextNext;
                        nextNext = _MASLegs[_MASLegs.Count - 2];

                        next.Protection.Clear();
                        next.Nominal.Clear();

                        if (next.legType == LegType.FixedRadius)
                            CreateMASRFLeg(nextNext, ref next, _CurrMASLeg, true);
                        else if (next.legType == LegType.FlyBy)
                            CreateIMASFlyByLeg(nextNext, ref next, _CurrMASLeg, true);
                        else
                            CreateMASStraightLeg(nextNext, ref next, _CurrMASLeg, true);
                        Clear(next);

                        Draw(ref next);

                        _MASLegs[_MASLegs.Count - 1] = next;

                    }

                   
                }

                TrackDistance = _CurrMASLeg.Nominal.Length - _CurrMASLeg.RNPvalue;
            }
            else
            {
                _CurrMASLeg.RollOutPrj = _CurrMASLeg.IsWpt ? _CurrMASLeg.startWpt.pPtPrj : ARANFunctions.LocalToPrj(_ptNextprj, _CurrMASLeg.RollOutDir, -_CurrMASLeg.DistToNext);
                _CurrMASLeg.RollOutGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(_CurrMASLeg.RollOutPrj);
                _CurrMASLeg.StartPrj = new Point(_CurrMASLeg.RollOutPrj);
                _CurrMASLeg.StartGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(_CurrMASLeg.StartPrj);

                _CurrMASLeg.StartDir = _CurrMASLeg.RollOutDir;

                //_CurrMASLeg.EndAltitude = FAPDist2hFAP(_CurrFASLeg.DistToNext, _hNext);
                //textBox0203.Text = Env.Current.UnitContext.UnitConverter.HeightToDisplayUnits(_CurrFASLeg.EndAltitude).ToString();

                _CurrMASLeg.RollOutPrj.Z = _CurrMASLeg.RollOutAltitude;
                _CurrMASLeg.RollOutGeo.Z = _CurrMASLeg.RollOutAltitude;

                if (_MASLegs.Count == 0)
                    CreateMASStraightLeg(default(RFLeg), ref _CurrMASLeg);
                else
                {
                    RFLeg next = _MASLegs[_MASLegs.Count - 1];
                    CreateMASStraightLeg(next, ref _CurrMASLeg);


                    
                    if (_MASLegs.Count > 1)
                    {
                        RFLeg nextNext;
                        nextNext = _MASLegs[_MASLegs.Count - 2];
                        next.Protection.Clear();
                        next.Nominal.Clear();
                        if (next.legType == LegType.FixedRadius)
                            CreateMASRFLeg(nextNext, ref next, _CurrMASLeg, true);
                        else if (next.legType == LegType.FlyBy)
                            CreateIMASFlyByLeg(nextNext, ref next, _CurrMASLeg, true);
                        else
                            CreateMASStraightLeg(nextNext, ref next, _CurrMASLeg, true);
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

                        _MASLegs[_MASLegs.Count - 1] = next;
                    }


                
                }
                TrackDistance = _CurrMASLeg.DistToNext;
            }

            _CurrMASLeg.CopyGeometry();
            _CurrMASLeg.ProtectionElem = Env.Current.AranGraphics.DrawMultiPolygon(_CurrMASLeg.Protection, eFillStyle.sfsHollow, ARANFunctions.RGB(0, 0, 255));
            _CurrMASLeg.NominalElem = Env.Current.AranGraphics.DrawMultiLineString(_CurrMASLeg.Nominal, 2, 255);
            if (_CurrMASLeg.legType != LegType.FlyBy)
                _CurrMASLeg.FixElem = Env.Current.AranGraphics.DrawPointWithText(_CurrMASLeg.StartPrj, "WPT", ARANFunctions.RGB(0, 127, 255));




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
                from = ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, (int)nextLeg.TurnDir * 2.0 * currLeg.RNPvalue);
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

        private void CreateMASStraightLeg(RFLeg nextLeg, ref RFLeg currLeg, RFLeg prevLeg = default(RFLeg), bool prevIsValid = false)
        {
            double startDir;
            Point startPrj;
            if (_MASLegs.Count > 0)
            {
                CreateNextLeg(nextLeg, ref currLeg);
                startPrj = nextLeg.StartPrj;
                startDir = nextLeg.StartDir;
            }
            else
            {
                startPrj = PreFinalState.ptMas0.Clone() as Point;
                startDir = _CurrMASLeg.StartDir;
            }

            Ring ring;
            Polygon pPoly;
            LineString ls;

            ring = new Ring();
            ring.Add(ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, 2.0 * currLeg.RNPvalue));
            ring.Add(ARANFunctions.LocalToPrj(startPrj, startDir, 0.0, 2.0 * currLeg.RNPvalue));
            ring.Add(ARANFunctions.LocalToPrj(startPrj, startDir, 0.0, -2.0 * currLeg.RNPvalue));
            ring.Add(ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, -2.0 * currLeg.RNPvalue));

            pPoly = new Polygon();
            pPoly.ExteriorRing = ring;
            currLeg.Protection.Add(pPoly);

            //===================================================
            ls = new LineString();
            ls.Add(currLeg.RollOutPrj);
            ls.Add(startPrj);
            currLeg.Nominal.Add(ls);
            currLeg.StartAltitude = currLeg.RollOutAltitude + ls.Length * currLeg.DescentGR;

            if (!prevIsValid)
                return;

            CreatePrevLeg(ref currLeg, prevLeg);
        }

        private void CreateMASRFLeg(RFLeg nextLeg, ref RFLeg currLeg, RFLeg prevLeg = default(RFLeg), bool prevIsValid = false)
        {
            if (_MASLegs.Count > 0)
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

        private void CreateIMASFlyByLeg(RFLeg nextLeg, ref RFLeg currLeg, RFLeg prevLeg = default(RFLeg), bool prevIsValid = false)
        {
            //CreateNextLeg(nextLeg, ref currLeg);
            Ring ring;
            Polygon pPoly;
            LineString ls;

            int turnDir = (int)currLeg.TurnDir;
            //===================================================

            currLeg.Center = ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, turnDir * currLeg.Radius);
            currLeg.StartPrj = ARANFunctions.LocalToPrj(currLeg.Center, currLeg.StartDir, 0.0, -turnDir * currLeg.Radius);
            currLeg.StartGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(currLeg.StartPrj);
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
            _CurrMASLeg.DescentGR = System.Math.Tan(ARANMath.DegToRad(dg));

        }
        public void AddLeg(bool next = true)
        {

            /*********************************************************************************************/
            NativeMethods.ShowPandaBox(Context.AppEnvironment.Current.SystemContext.EnvWin32Window.ToInt32());

            Functions.GetObstaclesByPolygonWithDecomposition(Env.Current.DataContext.ObstacleList, out CurrlegObsatclesList, _CurrMASLeg.Protection);

            int n = CurrlegObsatclesList.Parts.Length;
            for (int i = 0; i < n; i++)
            {
                CurrlegObsatclesList.Parts[i].MOC = _CurrMASLeg.MOC;
                CurrlegObsatclesList.Parts[i].ReqH = CurrlegObsatclesList.Parts[i].Height + _CurrMASLeg.MOC;
                CurrlegObsatclesList.Parts[i].Elev = CurrlegObsatclesList.Parts[i].Height + PreFinalState.ptTHRprj.Z;
            }

            _CurrMASLeg.ObstaclesList = CurrlegObsatclesList;

          //  Env.Current.RNPContext.ReportForm.FillPage07(CurrlegObsatclesList);
            NativeMethods.HidePandaBox();

            /************************************************************************************************************/


            _MASLegs.Add(_CurrMASLeg);

            if (next)
            {
                RFLeg nextLeg = _CurrMASLeg;
                _ptNextprj = nextLeg.StartPrj;
                _ptNextprj.Z = nextLeg.StartAltitude;
                _hNext = _ptNextprj.Z;

                if (_MASLegs.Count == 1)
                    _dg = DGInit;
                _CurrMASLeg = new RFLeg
                {
                    Nominal = new MultiLineString(),
                    Protection = new MultiPolygon(),
                    DescentGR = _MASLegs.Count > 1 ? nextLeg.DescentGR : Math.Tan(ARANMath.DegToRad(_dg)),
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
                CreateMASLeg();
            }
        }

        public void RemoveLeg()
        {
            if (_MASLegs.Any())
            {
                _MASLegs.RemoveAt(_MASLegs.Count - 1);
            }
        }

        public List<WPT_FIXType> GetRfWptList(WPT_FIXType[] waypoints)
        {
            double exitCourse = ARANFunctions.DirToAzimuth(_CurrMASLeg.RollOutPrj, _CurrMASLeg.RollOutDir,
                Env.Current.SpatialContext.SpatialReferenceProjection, Env.Current.SpatialContext.SpatialReferenceGeo);
            return Functions.GetRfWptList(_CurrMASLeg, waypoints,
                CalcMASRadius(_CurrMASLeg.TAS, _CurrMASLeg.RollOutAltitude - CurrAdhpElev, FinalState._FASmaxBank),
                CalcMASRadius(_CurrMASLeg.TAS, _CurrMASLeg.RollOutAltitude - CurrAdhpElev, 1.0), exitCourse, _MASturndir);
        }

        public List<WPT_FIXType> GetFlyByWptList(WPT_FIXType[] waypoints)
        {

            double fE = Env.Current.RNPContext.ErrorAngle;
            var result = new List<WPT_FIXType>();
            double exitCourse = ARANFunctions.DirToAzimuth(_CurrMASLeg.RollOutPrj, _CurrMASLeg.RollOutDir,
                Env.Current.SpatialContext.SpatialReferenceProjection, Env.Current.SpatialContext.SpatialReferenceGeo);
            var dMax = CalcFlyByEntryCourseDelta();
            foreach (var wpt in waypoints)
            {
                double course, inverseCourse;
                ARANFunctions.ReturnGeodesicAzimuth(_CurrMASLeg.RollOutGeo, wpt.pPtGeo, out course, out inverseCourse);
                double dAzt = NativeMethods.Modulus(_MASturndir * (inverseCourse - exitCourse));

                if (dAzt > dMax || (System.Math.Abs(Math.Sin(dAzt)) <= fE) && (Math.Cos(dAzt) > 0))
                    continue;
                result.Add(wpt);
            }

            return result;

        }


        public List<WPT_FIXType> GetStraightWptList(WPT_FIXType[] waypoints)
        {
            return Functions.GetStraightWptList(_ptNextprj, _CurrMASLeg.RollOutDir, waypoints, _CurrMASLeg.RNPvalue,
                _MaxDistance);
        }

        public void RfWptSelected(WPT_FIXType wpt)
        {
            _CurrMASLeg.IsWpt = true;
            _CurrMASLeg.startWpt = wpt;

            double dX = (_CurrMASLeg.RollOutPrj.X + wpt.pPtPrj.X) / 2;
            double dY = (_CurrMASLeg.RollOutPrj.Y + wpt.pPtPrj.Y) / 2;
            var wptDir = ARANFunctions.ReturnAngleInRadians(_CurrMASLeg.RollOutPrj, wpt.pPtPrj);
            var center = ARANFunctions.LineLineIntersect(_CurrMASLeg.RollOutPrj,
                _CurrMASLeg.RollOutDir + _MASturndir * ARANMath.C_PI_2,
                new Point(dX, dY), wptDir + ARANMath.C_PI_2) as Point;
            double r = ARANFunctions.ReturnDistanceInMeters(_CurrMASLeg.RollOutPrj, center);


            _CurrMASLeg.Radius = r;
            _CurrMASLeg.BankAngle = CalcMASBank(_CurrMASLeg.TAS, _CurrMASLeg.RollOutAltitude - CurrAdhpElev, _CurrMASLeg.Radius);
            double dir = ARANFunctions.ReturnAngleInRadians(center, wpt.pPtPrj) + _MASturndir * ARANMath.C_PI_2;
            var course = ARANFunctions.DirToAzimuth(center, dir, Env.Current.SpatialContext.SpatialReferenceProjection,
                Env.Current.SpatialContext.SpatialReferenceGeo);
            _CurrMASLeg.StartDir = dir;
            _CurrMASLeg.Course = course;
        }

        public void FlyByWptSelected(WPT_FIXType wpt)
        {
            _CurrMASLeg.IsWpt = true;
            _CurrMASLeg.startWpt = wpt;

            var wptDir = ARANFunctions.ReturnAngleInRadians(wpt.pPtPrj, NextLeg.StartPrj);
            var course = ARANFunctions.DirToAzimuth(_CurrMASLeg.RollOutPrj, wptDir, Env.Current.SpatialContext.SpatialReferenceProjection,
                Env.Current.SpatialContext.SpatialReferenceGeo);
            //return NextLeg.DistToNext >= r * Math.Tan(ARANMath.DegToRad(FlyByCourseDeltaMax / 2));
            _CurrMASLeg.StartDir = wptDir;
            _CurrMASLeg.Course = course;
            FlyByMocChanged(course);
        }

        private void FlyByMocChanged(double course)
        {
            if (Math.Abs(NativeMethods.Modulus(course) - NativeMethods.Modulus(NextLeg.Course)) > 15)
            {
                MocChanged(50);
            }
            else
                MocChanged(30);
        }

        public void StraightWptSelected(WPT_FIXType wpt)
        {
            _CurrMASLeg.IsWpt = true;
            _CurrMASLeg.startWpt = wpt;
            _CurrMASLeg.DistToNext = _MASLegs.Count > 0? ARANFunctions.ReturnDistanceInMeters(_MASLegs[_MASLegs.Count - 1].StartPrj, wpt.pPtPrj)
                : ARANFunctions.ReturnDistanceInMeters(PreFinalState.ptMas0, wpt.pPtPrj);
        }



        //public void CreateMAProtArea()
        //{

        //    //RNPAR 4.1.7
        //    double FASsemiWidth = (double)FASRegmentWithRule.Calculate(new object[] { _FASRNPval });
        //    double ThrToSplStartDist = (PreFinalState._PrelDHval - PreFinalState._GP_RDH) * PreFinalState._CoTanGPA;

        //    //RNPAR 4.1.7
        //    double MAsemiWidth = (double)MASRegmentWithRule.Calculate(new object[] { PreFinalState._MARNPval });
        //    double SplStartToEndDist = PreFinalState.coTan15 * 2.0 * (PreFinalState._MARNPval - PreFinalState._FASRNPval);

        //    _FapToThrDist = PreFinalState.hFAP2FAPDist(PreFinalState._PrelFAPalt - PreFinalState.ptTHRgeo.Z);

        //    ptFAPprj = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -_FapToThrDist);

        //    Env.Current.AranGraphics.SafeDeleteGraphic(_FAPElem);
        //    Env.Current.AranGraphics.SafeDeleteGraphic(_MASegmentElem);

        //    _FAPElem = Env.Current.AranGraphics.DrawPointWithText(ptFAPprj, "FAP", ARANFunctions.RGB(192, 127, 192));

        //    Point pt0l = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -_FapToThrDist - 5.0 * 1852.0, FASsemiWidth);
        //    Point pt0r = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -_FapToThrDist - 5.0 * 1852.0, -FASsemiWidth);

        //    Point pt1l = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -ThrToSplStartDist, FASsemiWidth);
        //    Point pt1r = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -ThrToSplStartDist, -FASsemiWidth);

        //    Point pt2l = ARANFunctions.LocalToPrj(pt1l, _ArDir, SplStartToEndDist, MAsemiWidth - FASsemiWidth);
        //    Point pt2r = ARANFunctions.LocalToPrj(pt1r, _ArDir, SplStartToEndDist, -MAsemiWidth + FASsemiWidth);

        //    Point pt3l = ARANFunctions.LocalToPrj(pt2l, _ArDir, 27780.0);
        //    Point pt3r = ARANFunctions.LocalToPrj(pt2r, _ArDir, 27780.0);

        //    Ring MASegmentRing = new Ring();
        //    MASegmentRing.Add(pt0l);
        //    MASegmentRing.Add(pt1l);
        //    MASegmentRing.Add(pt2l);
        //    MASegmentRing.Add(pt3l);

        //    MASegmentRing.Add(pt3r);
        //    MASegmentRing.Add(pt2r);
        //    MASegmentRing.Add(pt1r);
        //    MASegmentRing.Add(pt0r);

        //    Polygon pPolygon = new Polygon();
        //    pPolygon.ExteriorRing = MASegmentRing;
        //    _MASegmentPolygon = new MultiPolygon();
        //    _MASegmentPolygon.Add(pPolygon);
        //    _MASegmentElem = Env.Current.AranGraphics.DrawMultiPolygon(_MASegmentPolygon, eFillStyle.sfsHollow, ARANFunctions.RGB(0, 245, 245));
        //    NativeMethods.ShowPandaBox(Env.Current.SystemContext.EnvWin32Window.ToInt32());
        //    Functions.GetObstaclesByPolygonWithDecomposition(Env.Current.DataContext.ObstacleList, out OASObstacleList, _MASegmentPolygon);

        //    FillFAMAObstaclesFields(_PrelFAPalt);
        //    NativeMethods.HidePandaBox();

        //}
    }
}
