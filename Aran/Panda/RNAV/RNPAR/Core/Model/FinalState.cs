using Aran.Panda.RNAV.RNPAR.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Panda.RNAV.RNPAR.Context;
using Aran.Panda.RNAV.RNPAR.Rules.DOC9905;
using Aran.Panda.RNAV.RNPAR.Utils;
using Aran.PANDA.Common;
using Env = Aran.Panda.RNAV.RNPAR.Context.AppEnvironment;

namespace Aran.Panda.RNAV.RNPAR.Core.Model
{
    class FinalState : State<FinalState>
    {
        public FinalState()
        {
            _type = StateType.Final;
        }


        public PreFinalState PreFinalState => AppEnvironment.Current.RNPContext.PreFinalPhase.CurrentState;
        private double Elevation => Env.Current.DataContext.CurrADHP.Elev;

        public ObstacleContainer FASObstaclesList;

        private double _maObstacleStraightAltitude;

        public bool LastLeg { get; set; }
        public int _FASturndir;
        public int _FROPElem;

        public double _MinDistFROPtoNEXT;
        public double _MinFROPAltitude;
        public double _FASminRadius;
        public double _FASmaxRadius;
        public double _FASmaxBank;
        public double _totalLenght;
        public double _hNext;
        public int _FAPElem;
        public bool _isRfLeg = true;


        public Point _ptNextprj;
        public RFLeg _CurrFASLeg;
        public double Length;
        public List<RFLeg> _FASLegs { get; private set; } = new List<RFLeg>();

        public int TurnDirection => (1 - _FASturndir) / 2;

        public StraightSegmentPriorToOCH StraightSegmentPriorToOCH { get; } = new StraightSegmentPriorToOCH(SegmentType.Final, null);
        public BankAngleRF BankAngleRFRule { get; } = new BankAngleRF(SegmentType.Final, null);
        public CalcRadius CalcRadiusFRule { get; } = new CalcRadius(SegmentType.Final, null);


        #region Overrides

        public override FinalState Copy()
        {
            var finalState = BaseCopy();
            finalState.FASObstaclesList = FASObstaclesList;
            finalState._maObstacleStraightAltitude = _maObstacleStraightAltitude;
            finalState._FASturndir = _FASturndir;
            finalState._FROPElem = _FROPElem;
            finalState._MinDistFROPtoNEXT = _MinDistFROPtoNEXT;
            finalState._MinFROPAltitude = _MinFROPAltitude;
            finalState._FASminRadius = _FASminRadius;
            finalState._FASmaxRadius = _FASmaxRadius;
            finalState._FASmaxBank = _FASmaxBank;

            finalState._totalLenght = _totalLenght;
            finalState._hNext = _hNext;
            finalState._FAPElem = _FAPElem;
            finalState._isRfLeg = _isRfLeg;
            finalState._ptNextprj = _ptNextprj;
            finalState._CurrFASLeg = _CurrFASLeg;
            finalState._CurrFASLeg.Nominal = _CurrFASLeg.Nominal.Clone() as MultiLineString;
            finalState._CurrFASLeg.Protection = _CurrFASLeg.Protection.Clone() as MultiPolygon;
            finalState.Length = Length;
            _FASLegs.ForEach(t => finalState._FASLegs.Add(t));
            return finalState;
        }

        protected override FinalState CreateNewState()
        {
            return new FinalState();
        }

        public override void Clear()
        {
            Env.Current.AranGraphics.SafeDeleteGraphic(_FROPElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_FAPElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_CurrFASLeg.NominalElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_CurrFASLeg.ProtectionElem);

            int n = _FASLegs.Count;

            for (int i = 0; i < n; i++)
            {
                Env.Current.AranGraphics.SafeDeleteGraphic(_FASLegs[i].NominalElem);
                Env.Current.AranGraphics.SafeDeleteGraphic(_FASLegs[i].ProtectionElem);
            }
        }

        public override void ReCreate()
        {
            base.ReCreate();

            if (_isRfLeg)
            {
                if (_FASLegs.Count == 0)
                {
                    _FROPElem = DrawFapFrop(_CurrFASLeg.RollOutPrj, "FROP");
                }
                else
                {
                    _FROPElem = DrawFapFrop(_FASLegs[0].RollOutPrj, "FROP");
                }
                _FAPElem = DrawFapFrop(_CurrFASLeg.StartPrj, "FAP");
            }
            else
            {
                _FAPElem = DrawFapFrop(_CurrFASLeg.StartPrj, "FAP");
                if (_FASLegs.Count > 0)
                {
                    _FROPElem = DrawFapFrop(_FASLegs[0].RollOutPrj, "FROP");
                }

            }

            int n = _FASLegs.Count;
            for (int i = 0; i < n; i++)
            {
                var leg = _FASLegs[i];
                leg.ProtectionElem = Env.Current.AranGraphics.DrawMultiPolygon(_FASLegs[i].Protection,
                    eFillStyle.sfsHollow, ARANFunctions.RGB(0, 0, 255));
                leg.NominalElem = Env.Current.AranGraphics.DrawMultiLineString(_FASLegs[i].Nominal, 2, 255);
                _FASLegs[i] = leg;
            }

            _CurrFASLeg.ProtectionElem = Env.Current.AranGraphics.DrawMultiPolygon(_CurrFASLeg.Protection,
                eFillStyle.sfsHollow, ARANFunctions.RGB(0, 0, 255));
            _CurrFASLeg.NominalElem = Env.Current.AranGraphics.DrawMultiLineString(_CurrFASLeg.Nominal, 2, 255);


            if (!_CurrFASLeg.IsWpt && _isRfLeg)
            {
                CalcFropDistance(_CurrFASLeg.DistToNext);
                TurnDirectionChanged();
            }

        }

        public override void Commit()
        {
            Rules.Add(BankAngleRFRule);
            Rules.Add(CalcRadiusFRule);
            Rules.Add(StraightSegmentPriorToOCH);

            if (_isRfLeg)
            {
                if (LastLeg)
                {
                    Env.Current.AranGraphics.SafeDeleteGraphic(_FAPElem);
                    _FAPElem = DrawFapFrop(_CurrFASLeg.StartPrj, "FAP");
                    AddLastRfLeg();
                }
                else
                    AddRfLeg();
            }
            else
                AddStraightLeg();
        }

        #endregion

        public void Init()
        {
            _FASLegs.Clear();
            _FAPElem = PreFinalState._FAPElem;
            //DBModule.GetObstaclesByDist(out Env.Current.DataContext.ObstacleList, GlobalVars.CurrADHP.pPtPrj, GlobalVars.ModellingRadius, ptTHRgeo.Z);

            double dt, Vtas = 3.6 * ARANMath.IASToTAS(PreFinalState._IAS, Env.Current.DataContext.CurrADHP.Elev, 0.0);

            //RNPAR 4.5.12
            //if (PreFinalState._MARNPval >= 1852.0)
            //{
            //    double d15 = (PreFinalState._PrelDHval - PreFinalState._GP_RDH) * PreFinalState._CoTanGPA +
            //                 4.1666666666666666 * Vtas + 4.1666666666666666 * 27.78; //115.75
            //    dt = d15;
            //}
            //else
            //{
            //    double d50 = (PreFinalState._PrelDHval - PreFinalState._GP_RDH) * PreFinalState._CoTanGPA +
            //                 (Vtas + 27.78) * 13.89;
            //    dt = d50;
            //}

            ////RNPAR 4.5.11
            //double D150 = (150.0 - PreFinalState._GP_RDH) * PreFinalState._CoTanGPA;

            //_MinDistFROPtoNEXT = Math.Max(D150, dt);
            _MinDistFROPtoNEXT = (double)StraightSegmentPriorToOCH.Calculate(PreFinalState._PrelDHval, PreFinalState._GP_RDH, PreFinalState._GPAngle, Vtas, PreFinalState._MARNPval);
            //================================================================================

            //RNPAR 3.2.7 ?? what about 3.2.11 
            double Vwind = 3.6 * PreFinalState._CurrTWC;
            double V = Vtas + Vwind;

            _FASmaxBank = Math.Min(ARANMath.RadToDeg(Math.Atan(3.0 * Math.PI * V / 6355.0)), 20) ;

            //if (_FASmaxBank > 18.0)				_FASmaxBank = 18.0;		//??????????????????????
            //================================================================================
            _hNext = PreFinalState._GP_RDH + PreFinalState.ptTHRgeo.Z;
            _MinFROPAltitude = PreFinalState.FAPDist2hFAP(_MinDistFROPtoNEXT, _hNext);

            //RNPAR 4.1 ???? 
            _FASminRadius = CalcRadius(_MinFROPAltitude - Elevation, _FASmaxBank);
            if (_FASminRadius <= 2.0 * PreFinalState._FASRNPval)
                _FASminRadius = Math.Ceiling(2.01 * PreFinalState._FASRNPval);

            _FASmaxRadius = CalcRadius(PreFinalState._PrelFAPalt - Elevation, 1.0);

            //=========================================================================================
            _totalLenght = 0.0;
            _FASLegs.Clear();
            _ptNextprj = PreFinalState.ptTHRprj;

            //_hNext = ptTHRprj.Z;
            //_FROPtoTHRdistance = _MinDistFROPtoTHR;

            _CurrFASLeg = new RFLeg
            {
                DescentGR = PreFinalState._TanGPA,
                RNPvalue = PreFinalState._FASRNPval,
                BankAngle = _FASmaxBank,
                DistToNext = _MinDistFROPtoNEXT,
                RollOutAltitude = _MinFROPAltitude,
                Radius = CalcRadius(PreFinalState._PrelFAPalt - Elevation, _FASmaxBank),
                RollOutDir = PreFinalState._ArDir,
                Nominal = new MultiLineString(),
                Protection = new MultiPolygon()
            };

            ReCalc();
        }

        private void ReCalc()
        {
            CalcFropDistance(_CurrFASLeg.DistToNext);
            CalcEntryCourse(_CurrFASLeg.Course);

            TurnDirectionChanged(false);
            LegTypeChanged(true, false);

            CheckRadius(false);
            CreateFASLeg();
        }

        public double CalcRadius(double height, double bank)
        {
            //RNPAR 3.2.7 ?? what about 3.2.11 
            double Vtas = ARANMath.IASToTAS(PreFinalState._IAS, Elevation + height, 0.0);

            double Vwind = PreFinalState._CurrTWC;
            //double V = 3.6 * (Vtas + Vwind);
            //double R = (6355.0 * Math.Tan(ARANMath.DegToRad(bank))) / (Math.PI * V);

            //if (R > 3.0)
            //    R = 3.0;
            //double radius = 1000.0 * V / (20 * Math.PI * R);

            return (double)CalcRadiusFRule.Calculate(Vtas, Vwind, bank);
        }

        public double CalcBank(double height, double radius)
        {
            //RNPAR 4.1 3.2.8  ?? what about 3.2.11 
            double Vtas = ARANMath.IASToTAS(PreFinalState._IAS, Elevation + height, 0.0);


            double Vwind = PreFinalState._CurrTWC;
            double V = 3.6 * (Vtas + Vwind);

            //double R = (6355.0 * Math.Tan(ARANMath.DegToRad(bank))) / (Math.PI * V);
            //		bank = ARANMath.RadToDeg(Math.Atan(3.0 * Math.PI * V / 6355.0));
            //double bank = ARANMath.RadToDeg(Math.Atan(V * V / (127094.0 * radius)));

            //double R = 50.0 * V / (radius * Math.PI);

            //if (R > 3.0)
            //    R = 3.0;

            //double bank = ARANMath.RadToDeg(Math.Atan(R * Math.PI * V / 6355.0));

            //if (bank > 18.0)				bank = 18.0;		//???????????????????????
            return  (double)BankAngleRFRule.Calculate(Vtas, Vwind, radius);
        }

        public void CheckRadius(bool create = true)
        {
            double fTmp = _CurrFASLeg.Radius;

            if (_CurrFASLeg.Radius < _FASminRadius)
                _CurrFASLeg.Radius = _FASminRadius;

            if (_CurrFASLeg.Radius > _FASmaxRadius)
                _CurrFASLeg.Radius = _FASmaxRadius;

            fTmp = _CurrFASLeg.BankAngle;
            _CurrFASLeg.BankAngle = CalcBank(_CurrFASLeg.RollOutAltitude - Elevation, _CurrFASLeg.Radius);

            if (Math.Abs(fTmp - _CurrFASLeg.BankAngle) > ARANMath.EpsilonDegree)
            {
                CheckBank();
            }
        }

        public void IsWpt(bool yes)
        {
            _CurrFASLeg.IsWpt = yes;
        }

        public void SetFropWpt(WPT_FIXType wpt)
        {
            _CurrFASLeg.IsEndWpt = true;
            _CurrFASLeg.endWpt = wpt;
        }

        public void RemoveFropWpt()
        {
            _CurrFASLeg.IsEndWpt = false;
        }

        public void CalcFropDistance(double dist)
        {
          

            _CurrFASLeg.DistToNext = dist;

            double minDist = _FASLegs.Count > 0 ? 0 : _MinDistFROPtoNEXT;
            if (_CurrFASLeg.DistToNext < minDist)
                _CurrFASLeg.DistToNext = minDist;

            if (_CurrFASLeg.DistToNext > PreFinalState._FapToThrDist)
                _CurrFASLeg.DistToNext = PreFinalState._FapToThrDist;

            if (_FASLegs.Count == 0)
                PreFinalState.CreateFasProtArea(_CurrFASLeg.DistToNext);

            _CurrFASLeg.RollOutPrj =
                ARANFunctions.LocalToPrj(_ptNextprj, _CurrFASLeg.RollOutDir, -_CurrFASLeg.DistToNext);
            _CurrFASLeg.RollOutGeo =
                Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(_CurrFASLeg.RollOutPrj);
            _CurrFASLeg.RollOutAltitude = PreFinalState.FAPDist2hFAP(_CurrFASLeg.DistToNext, _hNext);


            _CurrFASLeg.RollOutPrj.Z = _CurrFASLeg.RollOutAltitude;
            _CurrFASLeg.RollOutGeo.Z = _CurrFASLeg.RollOutAltitude;

            if (_FASLegs.Count == 0)
            {
                Env.Current.AranGraphics.SafeDeleteGraphic(_FROPElem);
                _FROPElem = DrawFapFrop(_CurrFASLeg.RollOutPrj, "FROP");
            }


        }

        public void FillObstacles()
        {
            _maObstacleStraightAltitude = _CurrFASLeg.StartAltitude;
        }

        public void CalcEntryCourse(double course)
        {
            _CurrFASLeg.IsWpt = false;
            _CurrFASLeg.Course = course;
            double ExitCourse = ARANFunctions.DirToAzimuth(_CurrFASLeg.RollOutPrj, _CurrFASLeg.RollOutDir,
                Env.Current.SpatialContext.SpatialReferenceProjection, Env.Current.SpatialContext.SpatialReferenceGeo);
            double dAzt = NativeMethods.Modulus(_FASturndir * (_CurrFASLeg.Course - ExitCourse));

            if (dAzt > 240 || dAzt == 0)
                _CurrFASLeg.Course = NativeMethods.Modulus(ExitCourse + _FASturndir * 240.0);

            _CurrFASLeg.StartDir = ARANFunctions.AztToDirection(_CurrFASLeg.RollOutGeo, _CurrFASLeg.Course,
                Env.Current.SpatialContext.SpatialReferenceGeo, Env.Current.SpatialContext.SpatialReferenceProjection);

        }

        public void BankAngleChanged(double angle)
        {
            _CurrFASLeg.BankAngle = angle;
            CheckBank();

        }

        private void CheckBank()
        {
            double fTmp = _CurrFASLeg.BankAngle;

            if (_CurrFASLeg.BankAngle < 1.0)
                _CurrFASLeg.BankAngle = 1.0;

            if (_CurrFASLeg.BankAngle > _FASmaxBank)
                _CurrFASLeg.BankAngle = _FASmaxBank;

            fTmp = _CurrFASLeg.Radius;
            _CurrFASLeg.Radius = CalcRadius(_CurrFASLeg.RollOutAltitude - Elevation, _CurrFASLeg.BankAngle);
        }

        public void CreateFASLeg()
        {
            if (_FASturndir == 0)
                return;

            Env.Current.AranGraphics.SafeDeleteGraphic(_CurrFASLeg.NominalElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_CurrFASLeg.ProtectionElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_FAPElem);

            //==================================================
            _CurrFASLeg.Nominal.Clear();
            _CurrFASLeg.Protection.Clear();

            if (_FASLegs.Count != 0)
                CreateStraightLeg(_FASLegs[_FASLegs.Count - 1], ref _CurrFASLeg);


            if (_isRfLeg)
            {
                CreateRFLeg(_FASturndir, ref _CurrFASLeg);
            }

            _FAPElem = DrawFapFrop(_CurrFASLeg.StartPrj, "FAP");

            //===================================================
            _CurrFASLeg.ProtectionElem = Env.Current.AranGraphics.DrawMultiPolygon(_CurrFASLeg.Protection,
                eFillStyle.sfsHollow, ARANFunctions.RGB(0, 0, 255));
            _CurrFASLeg.NominalElem = Env.Current.AranGraphics.DrawMultiLineString(_CurrFASLeg.Nominal, 2, 255);

            _CurrFASLeg.CopyGeometry();

            Length = _totalLenght + _CurrFASLeg.Nominal.Length;

            if (_FASLegs.Count == 0)
                Length += _CurrFASLeg.DistToNext;

        }

        private void CreateStraightLeg(RFLeg nextLeg, ref RFLeg currLeg)
        {
            //currLeg.StartPrj = new Point(currLeg.RollOutPrj);
            //currLeg.StartDir = currLeg.RollOutDir;

            Ring ring = new Ring();
            ring.Add(ARANFunctions.LocalToPrj(currLeg.RollOutPrj, nextLeg.StartDir, 0.0, 2.0 * currLeg.RNPvalue));
            ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, 2.0 * currLeg.RNPvalue));
            ring.Add(ARANFunctions.LocalToPrj(nextLeg.StartPrj, nextLeg.StartDir, 0.0, -2.0 * currLeg.RNPvalue));
            ring.Add(ARANFunctions.LocalToPrj(currLeg.RollOutPrj, nextLeg.StartDir, 0.0, -2.0 * currLeg.RNPvalue));

            Polygon pPoly = new Polygon();
            pPoly.ExteriorRing = ring;

            currLeg.Protection.Add(pPoly);
            //===================================================
            LineString ls = new LineString();
            ls.Add(currLeg.RollOutPrj);
            ls.Add(nextLeg.StartPrj);
            currLeg.Nominal.Add(ls);
        }

        private void CreateRFLeg(int turnDir, ref RFLeg currLeg)
        {
            currLeg.Center =
                ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, turnDir * currLeg.Radius);
            currLeg.StartPrj =
                ARANFunctions.LocalToPrj(currLeg.Center, currLeg.StartDir, 0.0, -turnDir * currLeg.Radius);

            currLeg.StartGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(currLeg.StartPrj);

            Ring ring = new Ring();

            Point from = ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.StartDir, 0.0, 2.0 * currLeg.RNPvalue);
            Point to = ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, 2.0 * currLeg.RNPvalue);

            LineString ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, from, to, currLeg.TurnDir);
            ring.AddMultiPoint(ls);

            from = ARANFunctions.LocalToPrj(currLeg.StartPrj, currLeg.StartDir, 0.0, -2.0 * currLeg.RNPvalue);
            to = ARANFunctions.LocalToPrj(currLeg.RollOutPrj, currLeg.RollOutDir, 0.0, -2.0 * currLeg.RNPvalue);

            ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, from, to, currLeg.TurnDir);
            ring.AddReverse(ls);

            Polygon pPoly = new Polygon();
            pPoly.ExteriorRing = ring;
            currLeg.Protection.Add(pPoly);
            //===================================================

            ls = ARANFunctions.CreateArcAsPartPrj(currLeg.Center, currLeg.StartPrj, currLeg.RollOutPrj,
                currLeg.TurnDir);
            currLeg.Nominal.Add(ls);

            currLeg.StartAltitude = currLeg.RollOutAltitude + ls.Length * currLeg.DescentGR;
        }

        public void TurnDirectionChanged(int direction)
        {

            _FASturndir = 1 - 2 * direction;
            _CurrFASLeg.TurnDir = (TurnDirection)_FASturndir;

            CalcEntryCourse(_CurrFASLeg.Course);
        }

        private void TurnDirectionChanged(bool create = true)
        {
            TurnDirectionChanged(TurnDirection);
        }

        public void LegTypeChanged(bool rf, bool create = true)
        {
            _isRfLeg = rf;
            if (rf)
            {
                _CurrFASLeg.legType = LegType.FixedRadius;
                Env.Current.AranGraphics.SafeDeleteGraphic(_FAPElem);
                _FAPElem = DrawFapFrop(PreFinalState.ptFAPprj, "FAP");
                CalcFropDistance(_CurrFASLeg.DistToNext);
            }
            else
            {
                _CurrFASLeg.legType = LegType.Straight;
                if (_FASLegs.Count == 0)
                    Env.Current.AranGraphics.SafeDeleteGraphic(_FROPElem);
                CalcFapDistance(_CurrFASLeg.DistToNext);
            }
        }

        private int DrawFapFrop(Point point, string name)
        {
            return Env.Current.AranGraphics.DrawPointWithText(point, name,
                ARANFunctions.RGB(192, 127, 192));
        }

        public void CalcFapDistance(double distToNext, bool create = true)
        {
            _CurrFASLeg.DistToNext = distToNext;


            if (_CurrFASLeg.DistToNext < _MinDistFROPtoNEXT)
                _CurrFASLeg.DistToNext = _MinDistFROPtoNEXT;

            if (_CurrFASLeg.DistToNext > PreFinalState._FapToThrDist)
                _CurrFASLeg.DistToNext = PreFinalState._FapToThrDist;

            var start = ARANFunctions.LocalToPrj(_ptNextprj, _CurrFASLeg.RollOutDir, -_CurrFASLeg.DistToNext);

            CalcStraightLeg(start);
        }

        private void CalcStraightLeg(Point start)
        {
            _CurrFASLeg.StartAltitude = PreFinalState.FAPDist2hFAP(_CurrFASLeg.DistToNext, _hNext);
            _CurrFASLeg.StartPrj = start;
            _CurrFASLeg.StartPrj.Z = _CurrFASLeg.StartAltitude;

            _CurrFASLeg.StartGeo =
                Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo<Point>(_CurrFASLeg.StartPrj);
            _CurrFASLeg.StartGeo.Z = _CurrFASLeg.StartAltitude;

            _CurrFASLeg.RollOutPrj = _CurrFASLeg.StartPrj;
            _CurrFASLeg.RollOutGeo = _CurrFASLeg.StartGeo;
            _CurrFASLeg.RollOutAltitude = _CurrFASLeg.StartAltitude;

            _CurrFASLeg.StartPrj = new Point(_CurrFASLeg.RollOutPrj);

            Env.Current.AranGraphics.SafeDeleteGraphic(_FAPElem);
            _FAPElem = DrawFapFrop(_CurrFASLeg.RollOutPrj, "FAP");

        }

        public void RadiusChanged(double radius)
        {
            _CurrFASLeg.IsWpt = false;
            _CurrFASLeg.Radius = radius;
            CheckRadius();
        }

        public void AddRfLeg()
        {

            RFLeg nextLeg = _CurrFASLeg;

            AddLastRfLeg();


            _CurrFASLeg = new RFLeg
            {
                Nominal = new MultiLineString(),
                Protection = new MultiPolygon(),
                DescentGR = PreFinalState._TanGPA,
                RNPvalue = PreFinalState._FASRNPval,
                BankAngle = nextLeg.BankAngle,
                Radius = nextLeg.Radius,
                TurnDir = nextLeg.TurnDir,
                RollOutDir = nextLeg.StartDir,
                RollOutAltitude = nextLeg.StartAltitude,
                DistToNext = nextLeg.DistToNext,
                Course = nextLeg.Course,
                IsEndWpt = nextLeg.IsWpt,
                endWpt = nextLeg.startWpt
            };



            CalcFropDistance(_CurrFASLeg.DistToNext);
            //CalcEntryCourse(_CurrFASLeg.Course);

            TurnDirectionChanged();
            _totalLenght += nextLeg.Nominal.Length;
        }

        public void AddLastRfLeg()
        {
            NativeMethods.ShowPandaBox(Context.AppEnvironment.Current.SystemContext.EnvWin32Window.ToInt32());

            if (_FASLegs.Count == 0)
                PreFinalState.FillFAMAObstaclesFields(_maObstacleStraightAltitude, false);

            _ptNextprj = _CurrFASLeg.StartPrj;
            _ptNextprj.Z = _CurrFASLeg.StartAltitude;
            _hNext = _CurrFASLeg.StartAltitude;

            MultiPolygon mlp = (MultiPolygon)_CurrFASLeg.Protection.Clone(); //  new MultiPolygon();
            MultiLineString mls = (MultiLineString)_CurrFASLeg.Nominal.Clone(); // new MultiLineString();
            GeometryOperators geoOp = new GeometryOperators();
            double InvMeanEarthRadius = 1.0 / Env.Current.SpatialContext.MeanEarthRadius;

            foreach (var leg in _FASLegs)
            {
                mlp = (MultiPolygon)geoOp.UnionGeometry(mlp, leg.Protection);
                mls = (MultiLineString)geoOp.UnionGeometry(mls, leg.Nominal);
            }

            Functions.GetObstaclesByPolygonWithDecomposition(Env.Current.DataContext.ObstacleList, out FASObstaclesList, mlp);

            geoOp.CurrentGeometry = mls;

            int n = FASObstaclesList.Parts.Length;
            for (int i = 0; i < n; i++)
            {
                if (mls.IsEmpty)
                    FASObstaclesList.Parts[i].Dist = 0.0;
                else
                {
                    Point ptTan = geoOp.GetNearestPoint(FASObstaclesList.Parts[i].pPtPrj);
                    LineString ls = (LineString)mls[0].Clone();

                    double x0, y0;
                    double x1, y1;
                    double distance = 0.0;

                    while (ls.Count > 1)
                    {
                        Point pt0 = ls[0];
                        Point pt1 = ls[1];
                        double dir = ARANFunctions.ReturnAngleInRadians(pt0, pt1);
                        double dist = ARANFunctions.ReturnDistanceInMeters(pt0, pt1);

                        ARANFunctions.PrjToLocal(pt0, dir, ptTan, out x0, out y0);
                        if (y0 * y0 < ARANMath.Epsilon_2Distance)
                        {
                            ARANFunctions.PrjToLocal(pt1, dir, ptTan, out x1, out y1);
                            if (x0 * x1 < 0.0)
                            {
                                ls.Remove(0);
                                ls.Insert(0, ptTan);
                                distance = ls.Length;
                                break;
                            }
                        }
                        ls.Remove(0);
                    }
                    double commonL = _totalLenght + distance;

                    if (_FASLegs.Count == 0)
                        commonL += _CurrFASLeg.DistToNext;


                    FASObstaclesList.Parts[i].Dist = commonL;// _totalLenght += nextLeg.Nominal.Length;	//??????????????????????
                }

                double p = PreFinalState._OASgradient * (FASObstaclesList.Parts[i].Dist - PreFinalState._OASorigin) * InvMeanEarthRadius;
                double q = PreFinalState._TanGPA * FASObstaclesList.Parts[i].Dist * InvMeanEarthRadius;

                FASObstaclesList.Parts[i].hSurface = (Env.Current.SpatialContext.MeanEarthRadius + PreFinalState.ptTHRprj.Z) * (Math.Exp(p) - 1.0);
                //FASObstaclesList.Parts[i].MOC = (GlobalVars.MeanEarthRadius + ptTHRprj.Z + _GP_RDH) * Math.Exp(p) - GlobalVars.MeanEarthRadius - FASObstaclesList.Parts[i].hSurface;

                FASObstaclesList.Parts[i].hPenet = FASObstaclesList.Parts[i].Height - FASObstaclesList.Parts[i].hSurface;
                //if (FASObstaclesList.Parts[i].hPenet > 0.0)
                //	FASObstaclesList.Parts[i].ReqOCH = FASObstaclesList.Parts[i].Height + _HeightLoss;
            }

            _CurrFASLeg.ObstaclesList = FASObstaclesList;
            _FASLegs.Add(_CurrFASLeg);

            Env.Current.RNPContext.ReportForm.FillPage03(FASObstaclesList);
            NativeMethods.HidePandaBox();
        }
        private void AddStraightLeg()
        {
            _CurrFASLeg.StartDir = _CurrFASLeg.RollOutDir;
            _ptNextprj = _CurrFASLeg.StartPrj;
            _ptNextprj.Z = _hNext = _CurrFASLeg.StartAltitude;
            _CurrFASLeg.ObstaclesList = FASObstaclesList;
            _FASLegs.Add(_CurrFASLeg);
        }


        public List<WPT_FIXType> GetRfWptList(WPT_FIXType[] waypoints)
        {
            double exitCourse = ARANFunctions.DirToAzimuth(_CurrFASLeg.RollOutPrj, _CurrFASLeg.RollOutDir,
                Env.Current.SpatialContext.SpatialReferenceProjection, Env.Current.SpatialContext.SpatialReferenceGeo);
            return Functions.GetRfWptList(_CurrFASLeg, waypoints,
                CalcRadius(_CurrFASLeg.RollOutAltitude - Elevation, _FASmaxBank),
                CalcRadius(_CurrFASLeg.RollOutAltitude - Elevation, 1.0), exitCourse, _FASturndir);
        }

        public List<WPT_FIXType> GetStraightWptList(WPT_FIXType[] waypoints)
        {
            return Functions.GetStraightWptList(_ptNextprj, _CurrFASLeg.RollOutDir, waypoints, _MinDistFROPtoNEXT,
                PreFinalState._FapToThrDist);
        }

        public void RfWptSelected(WPT_FIXType wpt)
        {
            _CurrFASLeg.IsWpt = true;
            _CurrFASLeg.startWpt = wpt;

            double dX = (_CurrFASLeg.RollOutPrj.X + wpt.pPtPrj.X) / 2;
            double dY = (_CurrFASLeg.RollOutPrj.Y + wpt.pPtPrj.Y) / 2;
            var wptDir = ARANFunctions.ReturnAngleInRadians(_CurrFASLeg.RollOutPrj, wpt.pPtPrj);
            var center = ARANFunctions.LineLineIntersect(_CurrFASLeg.RollOutPrj,
                _CurrFASLeg.RollOutDir + _FASturndir * ARANMath.C_PI_2,
                new Point(dX, dY), wptDir + ARANMath.C_PI_2) as Point;
            double r = ARANFunctions.ReturnDistanceInMeters(_CurrFASLeg.RollOutPrj, center);

            //double x1 = _CurrFASLeg.RollOutPrj.X;
            //double y1 = _CurrFASLeg.RollOutPrj.Y;
            //double x2 = wpt.pPtPrj.X;
            //double y2 = wpt.pPtPrj.Y;
            //double a1 = 1;
            //double a2 = (x1 - x2);
            //double b1 = Math.Tan(_CurrFASLeg.RollOutDir);
            //double b2 = y1 - y2;
            //double c1 = a1 * x1 + b1 * y1;
            //double c2 = 0.5 * (x1 * x1 + y1 * y1 - x2 * x2 - y2 * y2);
            //double x = (c1 * b2 - b1 * c2) / (a1 * b2 - b1 * a2);
            //double y = (a1 * c2 - c1 * a2) / (a1 * b2 - b1 * a2);

            //double r = ARANFunctions.ReturnDistanceInMeters(_CurrFASLeg.RollOutPrj, new Point(x, y));

            _CurrFASLeg.Radius = r;
            _CurrFASLeg.BankAngle = CalcBank(_CurrFASLeg.RollOutAltitude - Elevation, _CurrFASLeg.Radius);
            double dir = ARANFunctions.ReturnAngleInRadians(center, wpt.pPtPrj) + _FASturndir * ARANMath.C_PI_2;
            var course = ARANFunctions.DirToAzimuth(center, dir, Env.Current.SpatialContext.SpatialReferenceProjection,
                Env.Current.SpatialContext.SpatialReferenceGeo);
            _CurrFASLeg.StartDir = dir;
            _CurrFASLeg.Course = course;
        }

        public void StraightWptSelected(WPT_FIXType wpt)
        {
            _CurrFASLeg.IsWpt = true;
            _CurrFASLeg.startWpt = wpt;
            _CurrFASLeg.DistToNext = ARANFunctions.ReturnDistanceInMeters(_ptNextprj, wpt.pPtPrj);
            CalcStraightLeg(wpt.pPtPrj);
        }


        public List<WPT_FIXType> GetFropWptList(WPT_FIXType[] waypoints)
        {
            return Functions.GetStraightWptList(_ptNextprj, _CurrFASLeg.RollOutDir, waypoints, _MinDistFROPtoNEXT,
                PreFinalState._FapToThrDist);
        }

        public void FropWptSelected(WPT_FIXType wpt)
        {
            if (_FASLegs.Count == 0)
            {
                SetFropWpt(wpt);
            }
            _CurrFASLeg.DistToNext = ARANFunctions.ReturnDistanceInMeters(_ptNextprj, wpt.pPtPrj);
            CalcFropDistance(_CurrFASLeg.DistToNext);
        }

    }


    public struct RFLeg
    {
        public Point StartPrj;
        public Point StartGeo;
        public double Course;
        public double StartDir;
        public double StartAltitude;

        public Point RollOutPrj;
        public Point RollOutGeo;
        public double RollOutDir;
        public double RollOutAltitude;

        //public Point EndPrj;
        //public Point EndGeo;
        //public double EndDir;
        //public double EndAltitude;

        public TurnDirection TurnDir;
        public Point Center;
        public double Radius;
        public double BankAngle;
        public double TAS;
        public double IAS;

        public double MOC;
        public double RNPvalue;
        public double DescentGR;
        public double DistToNext;

        public MultiLineString Nominal;
        public MultiPolygon Protection;

        public MultiLineString NominalOriginal;
        public MultiPolygon ProtectionOriginal;

        public ObstacleContainer ObstaclesList;

        public LegType legType;
        public int FixElem;
        public int NominalElem;
        public int ProtectionElem;

        public WPT_FIXType startWpt;
        public bool IsWpt;

        public WPT_FIXType endWpt;
        public bool IsEndWpt;

        public void CopyGeometry()
        {
            NominalOriginal = Nominal.Clone() as MultiLineString;
            ProtectionOriginal = Protection.Clone() as MultiPolygon;
        }
    }
}

