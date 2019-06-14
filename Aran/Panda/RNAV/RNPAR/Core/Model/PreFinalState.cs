using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.Panda.RNAV.RNPAR.Core.Intefrace;
using Aran.Panda.RNAV.RNPAR.Model;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aran.Geometries.Operators;
using Aran.Panda.RNAV.RNPAR.Rules.Core;
using Aran.Panda.RNAV.RNPAR.Rules.DOC9905;
using Aran.Panda.RNAV.RNPAR.Utils;
using Env = Aran.Panda.RNAV.RNPAR.Context.AppEnvironment;

namespace Aran.Panda.RNAV.RNPAR.Core.Model
{
    class PreFinalState : State<PreFinalState>
    {
        public double MARNPvalMin => 185.2;
        public double MARNPvalMax => 1852;

        public double coTan15 => 3.7320508075688772935274463415059;
        public double rdhToler => 1.524;

        //new TWC { height =	   0.00, speed =	 28.000},
        public TWC[] TWCTable => _twcTable;
        private TWC[] _twcTable = new TWC[]
        {
            new TWC { height =   152.40, speed =     46.300},
            new TWC { height =   304.80, speed =     70.376},
            new TWC { height =   457.20, speed =     92.600},
            new TWC { height =   609.60, speed =     92.600},
            new TWC { height =   762.00, speed =     92.600},
            new TWC { height =   914.40, speed =     92.600},
            new TWC { height =  1066.80, speed =    101.860},
            new TWC { height =  1219.20, speed =    111.120},
            new TWC { height =  1371.60, speed =    120.380},
            new TWC { height =  1524.00, speed =    129.640},
            new TWC { height =  1676.40, speed =    138.900},
            new TWC { height =  1828.80, speed =    148.160},
            new TWC { height =  1981.20, speed =    157.420},
            new TWC { height =  2133.60, speed =    166.680},
            new TWC { height =  2286.00, speed =    175.940},
            new TWC { height =  2438.40, speed =    185.200},
            new TWC { height =  2590.80, speed =    194.460},
            new TWC { height =  2743.20, speed =    203.720},
            new TWC { height =  2895.60, speed =    212.980},
            new TWC { height =  3048.00, speed =    222.240},
            new TWC { height =  3200.40, speed =    231.500},
            new TWC { height =  3352.80, speed =    240.760}
        };


        public ObstacleContainer OFZObstacleList;
        public ObstacleContainer OASObstacleList;

        public ObstacleContainer FASObstacleList;
        public ObstacleContainer MASObstacleList;

        public MultiPolygon _MASFASegmentPolygon;
        public MultiPolygon _MASegmentPolygon;
        public MultiPolygon _MASSplaySegmentPolygon;
        public MultiLineString _MASSplaySegmentNominal;
        public MultiPolygon _FASegmentPolygon;
        public MultiLineString _FASegmentNominal;
        public MultiLineString _pOasLine;
        public MultiLineString _pZorigin;

        public double _ArCourse;
        public double _RWYDir;
        public double _ArDir;

        public double _CoTanGPA;
        public double _GP_RDH;
        public double _TanGPA;

        public double _AlignP_THRMax;
        public double _AlignP_THRMin;
        public double _FapToThrDist;
        public double _AlignP_THR;

        public double _OCHbyAlignment;
        public double _OCHbyObctacle;
        public double _OASgradient;
        public double _HeightLoss;
        public double _OASorigin;
        public double _GPAngle;
        public double _VPA;
        public double _OCHMin;
        public double _MOCfap;
        public double _MOC75;
        public double _Tmin;

        public double _zOrigin;
        public double _TrD;
        public double _IAS;

        public double _MaxTWC;
        public double _CurrTWC;

        public double _PrelFAPalt;
        public double _FASRNPval;
        public double _PrelDHval;
        public double _MARNPval;

        public Point ptFas0;
        public Point ptFas0l;
        public Point ptFas0r;
        public Point ptFas1;
        public Point ptFas1l;
        public Point ptFas1r;

        public Point ptMas0;
        public Point ptMas0l;
        public Point ptMas0r;
        public Point ptMas1l;
        public Point ptMas1r;

        public Point ptMas2l;
        public Point ptMas2r;

        public Point ptTHRgeo;
        public Point ptTHRprj;
        public Point ptFAPprj;
        public Point ptSOCprj;

        public RWYType _SelectedRWY;

        public int _ZoriginLineElem;
        public int _MASegmentElem;
        public int _FASegmentElem;
        public int _MASplaySegmentElem;
        public int _OasLineElem;
        public int _Category;
        public int _FAPElem;
        public int _SOCElem;
        public decimal _VPAMin;
        public decimal _VPAMax;
        public decimal MAGUpDwn;

        #region  Rules

        public RNPSegmentWidth FASRegmentWithRule { get; } = new RNPSegmentWidth(SegmentType.Final, null);
        public RNPSegmentWidth MASRegmentWithRule { get; } = new RNPSegmentWidth(SegmentType.MissedApproach, null);
        public LateralProtection FASLateralProtectionRule { get; } = new LateralProtection(SegmentType.Final, "");
        public LateralProtection MASLateralProtectionRule { get; } = new LateralProtection(SegmentType.MissedApproach, "");
        public CalculatingTAS CalculatingTASRule { get; } = new CalculatingTAS(SegmentType.Final, "");

        #endregion


        public override PreFinalState Copy()
        {
            var preFinalState = BaseCopy();

            preFinalState.OFZObstacleList = OFZObstacleList;
            preFinalState.OASObstacleList = OASObstacleList;
            preFinalState._MASFASegmentPolygon = _MASFASegmentPolygon.Clone() as MultiPolygon;
            preFinalState._FASegmentPolygon = _FASegmentPolygon.Clone() as MultiPolygon;
            preFinalState._MASegmentPolygon = _MASegmentPolygon.Clone() as MultiPolygon;
            preFinalState._MASSplaySegmentPolygon = _MASSplaySegmentPolygon.Clone() as MultiPolygon;

            preFinalState.ptFas0 = ptFas1.Clone() as Point;
            preFinalState.ptFas1 = ptFas0.Clone() as Point;
            preFinalState.ptFas0l = ptFas0l.Clone() as Point;
            preFinalState.ptFas0r = ptFas0r.Clone() as Point;
            preFinalState.ptFas1l = ptFas1l.Clone() as Point;
            preFinalState.ptFas1r = ptFas1r.Clone() as Point;

            preFinalState.ptMas0 = ptMas0.Clone() as Point;
            preFinalState.ptMas0l = ptMas0l.Clone() as Point;
            preFinalState.ptMas0r = ptMas0r.Clone() as Point;
            preFinalState.ptMas1l = ptMas1l.Clone() as Point;
            preFinalState.ptMas1r = ptMas1r.Clone() as Point;


            preFinalState._pOasLine = _pOasLine.Clone() as MultiLineString;
            preFinalState._pZorigin = _pZorigin.Clone() as MultiLineString;

            preFinalState._ArCourse = _ArCourse;
            preFinalState._RWYDir = _RWYDir;
            preFinalState._ArDir = _ArDir;

            preFinalState._VPA = _VPA;
            preFinalState._CoTanGPA = _CoTanGPA;
            preFinalState._GP_RDH = _GP_RDH;
            preFinalState._TanGPA = _TanGPA;

            preFinalState._AlignP_THRMax = _AlignP_THRMax;
            preFinalState._AlignP_THRMin = _AlignP_THRMin;
            preFinalState._FapToThrDist = _FapToThrDist;
            preFinalState._AlignP_THR = _AlignP_THR;

            preFinalState._OCHbyAlignment = _OCHbyAlignment;
            preFinalState._OCHbyObctacle = _OCHbyObctacle;
            preFinalState._OASgradient = _OASgradient;
            preFinalState._HeightLoss = _HeightLoss;
            preFinalState._OASorigin = _OASorigin;
            preFinalState._GPAngle = _GPAngle;
            preFinalState._OCHMin = _OCHMin;
            preFinalState._MOCfap = _MOCfap;
            preFinalState._MOC75 = _MOC75;
            preFinalState._Tmin = _Tmin;

            preFinalState._zOrigin = _zOrigin;
            preFinalState._TrD = _TrD;
            preFinalState._IAS = _IAS;

            preFinalState._MaxTWC = _MaxTWC;
            preFinalState._CurrTWC = _CurrTWC;

            preFinalState._PrelFAPalt = _PrelFAPalt;
            preFinalState._FASRNPval = _FASRNPval;
            preFinalState._PrelDHval = _PrelDHval;
            preFinalState._MARNPval = _MARNPval;

            preFinalState.ptTHRgeo = ptTHRgeo.Clone() as Point;
            preFinalState.ptTHRprj = ptTHRprj.Clone() as Point;
            preFinalState.ptFAPprj = ptFAPprj?.Clone() as Point;
            preFinalState.ptSOCprj = ptSOCprj?.Clone() as Point;

            preFinalState._SelectedRWY = _SelectedRWY;

            preFinalState._ZoriginLineElem = _ZoriginLineElem;
            preFinalState._MASegmentElem = _MASegmentElem;
            preFinalState._FASegmentElem = _FASegmentElem;
            preFinalState._MASplaySegmentElem = _MASplaySegmentElem;
            preFinalState._OasLineElem = _OasLineElem;
            preFinalState._Category = _Category;
            preFinalState._FAPElem = _FAPElem;
            preFinalState._SOCElem = _SOCElem;
            preFinalState._VPAMin = _VPAMin;
            preFinalState._VPAMax = _VPAMax;
            preFinalState.MAGUpDwn = MAGUpDwn;


            return preFinalState;

        }

        protected override PreFinalState CreateNewState()
        {
            return new PreFinalState();
        }

        public PreFinalState()
        {
            _type = StateType.PreFinal;
        }


        public double SetAlitemertTHR(double altimeterTHR)
        {
            _AlignP_THR = altimeterTHR;

            if (_AlignP_THR < _AlignP_THRMin)
                _AlignP_THR = _AlignP_THRMin;

            if (_AlignP_THR > _AlignP_THRMax)
                _AlignP_THR = _AlignP_THRMax;

            if (altimeterTHR != _AlignP_THR)
                altimeterTHR = System.Math.Round(_AlignP_THR, 2);
            return altimeterTHR;
        }

        public void CalcOCHMin()
        {

            if (_OCHbyAlignment > _OCHbyObctacle)
                _OCHMin = _OCHbyAlignment;
            else
                _OCHMin = _OCHbyObctacle;
        }

        public void CalcOCHByAlighment()
        {
            _OCHbyAlignment = _GP_RDH + _AlignP_THR * _TanGPA;
        }

        public double CalcTWC(double twc)
        {
            _CurrTWC = twc;
            if (_CurrTWC < 0.0)
                _CurrTWC = 0.0;

            if (_CurrTWC > _MaxTWC)
                _CurrTWC = _MaxTWC;

            return _CurrTWC;
        }



        public double hFAP2FAPDist(double Hrel)
        {
            return Env.Current.SpatialContext.MeanEarthRadius * Math.Log((Env.Current.SpatialContext.MeanEarthRadius + Hrel + ptTHRgeo.Z) / (Env.Current.SpatialContext.MeanEarthRadius + _GP_RDH + ptTHRgeo.Z)) * _CoTanGPA;
        }

        public double FAPDist2hFAP(double Dist, double hStart)
        {
            return Math.Exp(Dist * _TanGPA / Env.Current.SpatialContext.MeanEarthRadius) * (Env.Current.SpatialContext.MeanEarthRadius + hStart) - Env.Current.SpatialContext.MeanEarthRadius;
        }


        public void SetFASRNPValue(double val)
        {
            _FASRNPval = (double)FASLateralProtectionRule.Calculate(val);
        }


        public void SetMapRNPValue(double val)
        {
            _MARNPval = (double)MASLateralProtectionRule.Calculate(val);
            if (_MARNPval < _FASRNPval)
                _MARNPval = (double)MASLateralProtectionRule.Calculate(_FASRNPval);

        }

        public void SetFAOffsetAngle(double angle)
        {
            _ArCourse = ptTHRgeo.M + angle; //RNPAR 4.5.5
            _ArDir = ARANFunctions.AztToDirection(ptTHRgeo, _ArCourse, Env.Current.SpatialContext.SpatialReferenceGeo, Env.Current.SpatialContext.SpatialReferenceProjection);

        }

        public void SetDH(double value)
        {
            _PrelDHval = value;
            if (_PrelDHval < _OCHMin)
                _PrelDHval = _OCHMin;

            if (_PrelDHval > 4.0 * 75.0)
                _PrelDHval = 4.0 * 75.0;
        }

        public void SetDatumHeight(double dh)
        {
            double[] _RDHNom = new double[] { 12.192, 13.716, 15.24, 15.24, 15.24 };

            _GP_RDH = dh;

            if (_GP_RDH < _RDHNom[_Category] - rdhToler)
                _GP_RDH = _RDHNom[_Category] - rdhToler;

            if (_GP_RDH > _RDHNom[_Category] + rdhToler)
                _GP_RDH = _RDHNom[_Category] + rdhToler;
        }


        public void CalcFASProtArea()
        {

            Ring FASegmentRing = new Ring();
            FASegmentRing.Add(ptFas0l.Clone() as Point);
            FASegmentRing.Add(ptFas1l.Clone() as Point);
            FASegmentRing.Add(ptFas1r.Clone() as Point);
            FASegmentRing.Add(ptFas0r.Clone() as Point);

            Polygon FASPolygon = new Polygon();
            FASPolygon.ExteriorRing = FASegmentRing;
            _FASegmentPolygon = new MultiPolygon();
            _FASegmentPolygon.Add(FASPolygon);

            _FASegmentNominal = new MultiLineString();
            LineString ls = new LineString();
            ls.Add(ptFas1);
            ls.Add(ptFas0);
            _FASegmentNominal.Add(ls);
        }


        public void CalcMASPlayProtArea()
        {
            Ring MASplayegmentRing = new Ring();
            MASplayegmentRing.Add(ptFas1l.Clone() as Point);
            MASplayegmentRing.Add(ptMas0l.Clone() as Point);
            MASplayegmentRing.Add(ptMas0r.Clone() as Point);
            MASplayegmentRing.Add(ptFas1r.Clone() as Point);


            Polygon MASplayPolygon = new Polygon();
            MASplayPolygon.ExteriorRing = MASplayegmentRing;
            _MASSplaySegmentPolygon = new MultiPolygon();
            _MASSplaySegmentPolygon.Add(MASplayPolygon);


            _MASSplaySegmentNominal = new MultiLineString();
            LineString ls = new LineString();
            ls.Add(ptFas1);
            ls.Add(ptMas0);
            _MASSplaySegmentNominal.Add(ls);
        }


        public void CalcMASProtArea()
        {
            Ring MASegmentRing = new Ring();
            MASegmentRing.Add(ptMas0l.Clone() as Point);
            MASegmentRing.Add(ptMas1l.Clone() as Point);
            MASegmentRing.Add(ptMas1r.Clone() as Point);
            MASegmentRing.Add(ptMas0r.Clone() as Point);


            Polygon MASPolygon = new Polygon();
            MASPolygon.ExteriorRing = MASegmentRing;
            _MASegmentPolygon = new MultiPolygon();
            _MASegmentPolygon.Add(MASPolygon);
        }

        public void CalcMASFASProtArea()
        {
            Ring MASFASegmentRing = new Ring();
            MASFASegmentRing.Add(ptFas0l.Clone() as Point);
            MASFASegmentRing.Add(ptFas1l.Clone() as Point);
            MASFASegmentRing.Add(ptMas0l.Clone() as Point);
            MASFASegmentRing.Add(ptMas1l.Clone() as Point);

            MASFASegmentRing.Add(ptMas1r.Clone() as Point);
            MASFASegmentRing.Add(ptMas0r.Clone() as Point);
            MASFASegmentRing.Add(ptFas1r.Clone() as Point);
            MASFASegmentRing.Add(ptFas0r.Clone() as Point);

            Polygon MASFASPolygon = new Polygon();
            MASFASPolygon.ExteriorRing = MASFASegmentRing;
            _MASFASegmentPolygon = new MultiPolygon();
            _MASFASegmentPolygon.Add(MASFASPolygon);


        }

        public void CreateMAProtArea(double fasDistance = 9260.0, double masDistance = 27780.0)
        {
            CalcProtAreaParams(fasDistance, masDistance);

            ClearMapPropArea();

            CalcFASProtArea();
            CalcMASProtArea();
            CalcMASPlayProtArea();
            CalcMASFASProtArea();


            DrawFAP();
            DrawMasSegment();
            DrawFasSegment();
            DrawMasSplaySegment();

            NativeMethods.ShowPandaBox(Env.Current.SystemContext.EnvWin32Window.ToInt32());
            Functions.GetObstaclesByPolygonWithDecomposition(Env.Current.DataContext.ObstacleList, out OASObstacleList, _MASFASegmentPolygon);
            FillFAMAObstaclesFields(_PrelFAPalt);
            NativeMethods.HidePandaBox();

        }

        public void DrawMAProtArea()
        {

            ClearMapPropArea();
            DrawFAP();
            DrawMasSegment();
            DrawFasSegment();
            DrawMasSplaySegment();
        }

        public void CreateFasProtArea(double distance)
        {
            ClearMapPropArea();
            CalcFasParams(distance, true);

            CalcFASProtArea();
            CalcMASFASProtArea();

            DrawMasSegment();
            DrawFasSegment();
            DrawMasSplaySegment();

            NativeMethods.ShowPandaBox(Env.Current.SystemContext.EnvWin32Window.ToInt32());
            Functions.GetObstaclesByPolygonWithDecomposition(Env.Current.DataContext.ObstacleList, out OASObstacleList, _MASFASegmentPolygon);
            FillFAMAObstaclesFields(_PrelFAPalt);
            Functions.GetObstaclesByPolygonWithDecomposition(OASObstacleList, out FASObstacleList, _FASegmentPolygon);
            NativeMethods.HidePandaBox();
        }


        public void DrawMasProtArea()
        {
            ClearMapPropArea();

            DrawFasSegment();
            DrawMasSplaySegment();

            NativeMethods.ShowPandaBox(Env.Current.SystemContext.EnvWin32Window.ToInt32());
            Functions.GetObstaclesByPolygonWithDecomposition(OASObstacleList, out MASObstacleList, _MASSplaySegmentPolygon);
            NativeMethods.HidePandaBox();
        }


        public void DrawMasSegment()
        {
            _MASegmentElem =
                Env.Current.AranGraphics.DrawMultiPolygon(_MASegmentPolygon, eFillStyle.sfsHollow,
                    ARANFunctions.RGB(0, 245, 245));
        }

        public void DrawFasSegment()
        {
            _FASegmentElem =
                Env.Current.AranGraphics.DrawMultiPolygon(_FASegmentPolygon, eFillStyle.sfsHollow,
                    ARANFunctions.RGB(0, 245, 245));
        }

        public void DrawFAP()
        {
            _FAPElem = Env.Current.AranGraphics.DrawPointWithText(ptFAPprj, "FAP", ARANFunctions.RGB(192, 127, 192));
        }

        public void DrawMasSplaySegment()
        {
            _MASplaySegmentElem =
                Env.Current.AranGraphics.DrawMultiPolygon(_MASSplaySegmentPolygon, eFillStyle.sfsHollow,
                    ARANFunctions.RGB(0, 245, 245));
        }


        private void CalcProtAreaParams(double fasDistance = 9260.0, double masDistance = 27780.0)
        {
            var FASsemiWidth = CalcFasParams(fasDistance);


            CalcMasParams(masDistance, FASsemiWidth);
        }

        private void CalcMasParams(double masDistance, double FASsemiWidth)
        {
            //RNPAR 4.1.7
            double MAsemiWidth = (double) MASRegmentWithRule.Calculate(new object[] {_MARNPval});
            double SplStartToEndDist = coTan15 * 2.0 * (_MARNPval - _FASRNPval);
            ptMas0 = ARANFunctions.LocalToPrj(ptFas1, _ArDir, SplStartToEndDist);
            ptMas0l = ARANFunctions.LocalToPrj(ptFas1l, _ArDir, SplStartToEndDist, MAsemiWidth - FASsemiWidth);
            ptMas0r = ARANFunctions.LocalToPrj(ptFas1r, _ArDir, SplStartToEndDist, -MAsemiWidth + FASsemiWidth);

            ptMas1l = ARANFunctions.LocalToPrj(ptMas0l, _ArDir, masDistance);
            ptMas1r = ARANFunctions.LocalToPrj(ptMas0r, _ArDir, masDistance);
        }

        private double CalcFasParams(double fasDistance, bool fromThr = false)
        {
            //RNPAR 4.1.7
            double FASsemiWidth = (double) FASRegmentWithRule.Calculate(new object[] {_FASRNPval});
            double ThrToSplStartDist = (_PrelDHval - _GP_RDH) * _CoTanGPA;

            _FapToThrDist = hFAP2FAPDist(_PrelFAPalt - ptTHRgeo.Z);

            ptFAPprj = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -_FapToThrDist);

            ptFas0 = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, fromThr?- fasDistance: -_FapToThrDist - fasDistance);
            ptFas0l = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, fromThr?- fasDistance: -_FapToThrDist - fasDistance, FASsemiWidth);
            ptFas0r = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, fromThr ? -fasDistance : -_FapToThrDist - fasDistance, -FASsemiWidth);

            ptFas1 = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -ThrToSplStartDist);
            ptFas1l = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -ThrToSplStartDist, FASsemiWidth);
            ptFas1r = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -ThrToSplStartDist, -FASsemiWidth);
            return FASsemiWidth;
        }

        public void MAGUpDwnChanged(decimal value)
        {
            MAGUpDwn = value;
        }

        public void FillFAMAObstaclesFields(double FAPaltitude, bool isStraight = true)
        {
            double _MAMOC = 0.0;

            CalcAreaParams(FAPaltitude, isStraight);

            int n = OASObstacleList.Parts.Length;
            double MAG = 0.01 * (double)MAGUpDwn;
            double CoTanZ = 1.0 / MAG;

            double InvMeanEarthRadius = 1.0 / Env.Current.SpatialContext.MeanEarthRadius;

            for (int i = 0; i < n; i++)
            {
                double x, y;
                ARANFunctions.PrjToLocal(ptTHRprj, _ArDir, OASObstacleList.Parts[i].pPtPrj, out x, out y);
                OASObstacleList.Parts[i].Dist = -x;
                OASObstacleList.Parts[i].DistStar = y;
                OASObstacleList.Parts[i].EffectiveHeight = 0.0;

                if (OASObstacleList.Parts[i].Dist > _OASorigin)
                {
                    OASObstacleList.Parts[i].Plane = (int)eOAS.WPlane;
                    double p = _OASgradient * (OASObstacleList.Parts[i].Dist - _OASorigin) * InvMeanEarthRadius;
                    double q = _TanGPA * OASObstacleList.Parts[i].Dist * InvMeanEarthRadius;
                    OASObstacleList.Parts[i].hSurface = (Env.Current.SpatialContext.MeanEarthRadius + ptTHRprj.Z) * (Math.Exp(p) - 1.0);
                    OASObstacleList.Parts[i].MOC = (Env.Current.SpatialContext.MeanEarthRadius + ptTHRprj.Z + _GP_RDH) * Math.Exp(p) - Env.Current.SpatialContext.MeanEarthRadius - OASObstacleList.Parts[i].hSurface;

                    OASObstacleList.Parts[i].hPenet = OASObstacleList.Parts[i].Height - OASObstacleList.Parts[i].hSurface;
                    if (OASObstacleList.Parts[i].hPenet > 0.0)
                        OASObstacleList.Parts[i].ReqOCH = OASObstacleList.Parts[i].Height + _HeightLoss;
                }
                else if (x > _zOrigin)
                {
                    OASObstacleList.Parts[i].Plane = (int)eOAS.XlPlane;
                    OASObstacleList.Parts[i].hSurface = MAG * (x - _zOrigin);
                    OASObstacleList.Parts[i].hPenet = OASObstacleList.Parts[i].Height - OASObstacleList.Parts[i].hSurface;

                    if (OASObstacleList.Parts[i].hPenet > 0.0)
                    {
                        double fTmp = ((OASObstacleList.Parts[i].Height + _MAMOC) * CoTanZ + (OASObstacleList.Parts[i].Dist + _zOrigin)) / (CoTanZ + _CoTanGPA);
                        OASObstacleList.Parts[i].EffectiveHeight = fTmp;
                        OASObstacleList.Parts[i].ReqOCH = fTmp + _HeightLoss;
                    }
                }
                else
                {
                    OASObstacleList.Parts[i].Plane = (int)eOAS.ZeroPlane;
                    OASObstacleList.Parts[i].hSurface = 0.0;
                    OASObstacleList.Parts[i].hPenet = OASObstacleList.Parts[i].Height - OASObstacleList.Parts[i].hSurface;

                    if (OASObstacleList.Parts[i].hPenet > 0.0)
                        OASObstacleList.Parts[i].ReqOCH = OASObstacleList.Parts[i].Height + _HeightLoss;
                }
            }

            Env.Current.RNPContext.ReportForm.FillPage02(OASObstacleList);
        }

        private void CalcAreaParams(double FAPaltitude, bool isStraight)
        {

            Env.Current.AranGraphics.SafeDeleteGraphic(_OasLineElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_ZoriginLineElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_SOCElem);

            _MOC75 = FASMOCatAltitude(75.0 + ptTHRprj.Z, isStraight);
            _MOCfap = FASMOCatAltitude(FAPaltitude, isStraight);

            _OASgradient = ((FAPaltitude - ptTHRprj.Z - _MOCfap) - (75.0 - _MOC75)) / ((FAPaltitude - ptTHRprj.Z - 75.0) * _CoTanGPA);
            _OASorigin = (75.0 - _GP_RDH) * _CoTanGPA - (75.0 - _MOC75) / _OASgradient;

            double anpe = 1.225 * _MARNPval;
            double wpr = 18.288;
            double fte = 22.86 * _CoTanGPA;
            //double dISA = _Tmin - 15.0;
            double Vtas = (double)CalculatingTASRule.Calculate(_IAS, Env.Current.DataContext.CurrADHP.Elev, 15.0);
            double t = 15;

            _TrD = t * (Vtas + 18.52 / 3.6) + 4.0 / 3.0 * Math.Sqrt(anpe * anpe + wpr * wpr + fte * fte);
            _zOrigin = _TrD - (_HeightLoss - _GP_RDH) * _CoTanGPA;

            double xSOC = (_PrelDHval - _GP_RDH) * _CoTanGPA - _TrD;
            ptSOCprj = ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -xSOC);
            ptSOCprj.Z = _PrelDHval - _HeightLoss;

            _SOCElem = Env.Current.AranGraphics.DrawPointWithText(ptSOCprj, "SOC", ARANFunctions.RGB(192, 127, 192));

            GeometryOperators geomOp = new GeometryOperators();
            geomOp.CurrentGeometry = _MASFASegmentPolygon;

            LineString ls = new LineString();
            ls.Add(ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -_OASorigin, 100000.0));
            ls.Add(ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, -_OASorigin, -100000.0));

            _pOasLine = (MultiLineString)geomOp.Intersect(ls);
            _OasLineElem = Env.Current.AranGraphics.DrawMultiLineString(_pOasLine, 2, ARANFunctions.RGB(0, 255, 0));

            ls.Clear();
            ls.Add(ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, _zOrigin, 100000.0));
            ls.Add(ARANFunctions.LocalToPrj(ptTHRprj, _ArDir, _zOrigin, -100000.0));

            _pZorigin = (MultiLineString)geomOp.Intersect(ls);
            _ZoriginLineElem = Env.Current.AranGraphics.DrawMultiLineString(_pZorigin, 2, ARANFunctions.RGB(0, 0, 255));

        }

        public void CreateOFZPlanes()
        {

            NativeMethods.ShowPandaBox(Context.AppEnvironment.Current.SystemContext.EnvWin32Window.ToInt32());
            Functions.CreateOFZPlanes(ptTHRprj, _RWYDir, 45.0, ref Env.Current.RNPContext.OFZPlanes);

            int n = Env.Current.RNPContext.OFZPlanes.Length;

            for (int i = 0; i < n; i++)
            {
                Env.Current.AranGraphics.SafeDeleteGraphic(Env.Current.RNPContext.OFZPlanesElement[i]);
                Env.Current.RNPContext.OFZPlanesElement[i] = Env.Current.AranGraphics.DrawMultiPolygon(Env.Current.RNPContext.OFZPlanes[i].Poly, AranEnvironment.Symbols.eFillStyle.sfsHollow, -1, Env.Current.RNPContext.OFZPlanesState);
            }

            _OCHbyObctacle = 75.0;
            Functions.AnaliseObstacles(Env.Current.DataContext.ObstacleList, out OFZObstacleList, ptTHRprj, _ArDir,
                Env.Current.RNPContext.OFZPlanes);
            //if (Functions.AnaliseObstacles(Env.Current.DataContext.ObstacleList, out OFZObstacleList, ptTHRprj, _ArDir, Env.Current.RNPContext.OFZPlanes) > 0)
            //    _OCHbyObctacle = 90.0; // RNPAR 2.2.2 ?.?.? old edition 

            // GlobalVars.VisibilityBar.SetEnabled(GlobalVars.VisibilityBar.OFZ, true);
            Env.Current.RNPContext.ReportForm.FillPage01(OFZObstacleList);
            NativeMethods.HidePandaBox();

        }


        private double FASMOCatAltitude(double elev, bool isStraight)
        {
            double dISA = _Tmin - (15.0 - 0.0065 * ptTHRprj.Z);

            double bg = isStraight ? 8.0 : 12.360679774997896964091736687313;   //40.0 * sin(18.0);
            double fte = 23.0;
            double atis = 6.0;

            double anpe = 1.225 * _FASRNPval * _TanGPA;
            double wpr = 18.0 * _TanGPA;
            double ase = -2.887e-7 * elev * elev + 6.5e-3 * elev + 15;
            double tan001 = Math.Tan(0.01 * ARANMath.DegToRadValue);
            double vae = (elev - ptTHRprj.Z) * _CoTanGPA * (_TanGPA - (_TanGPA - tan001) / (1.0 + _TanGPA * tan001));
            double isad = (elev - ptTHRprj.Z) * dISA / (288 + dISA - 0.5 * 0.0065 * elev);

            double result = bg - isad + 4.0 / 3.0 * Math.Sqrt(anpe * anpe + wpr * wpr + fte * fte + ase * ase + vae * vae + atis * atis);
            return result;
        }


        public void SetAltimeterMargin(int type)
        {
            double fMOCCorrH, fMOCCorrGP, fMargin;

            if (type == 0)                        //Radio	//	fMargin = 0.096 / 0.277777777777778 * cVatMax.Values(k) - 3.2
                fMargin = 0.34406047516199 * Env.Current.UnitContext.Constants.AircraftCategory[aircraftCategoryData.VatMax].Value[_Category] - 3.2;                 // 0.3456
            else                                                        //Baro	//	fMargin = 0.068 / 0.277777777777778 * cVatMax.Values(k) + 28.3
                fMargin = 0.24298056155508 * Env.Current.UnitContext.Constants.AircraftCategory[aircraftCategoryData.VatMax].Value[_Category] + 28.3;                    // 0.2448

            if (Env.Current.DataContext.CurrADHP.pPtGeo.Z > 900.0)
                fMOCCorrH = Env.Current.DataContext.CurrADHP.pPtGeo.Z * fMargin / 1500.0;
            else
                fMOCCorrH = 0.0;

            if (_GPAngle > ARANMath.DegToRad(3.2))
                fMOCCorrGP = (_GPAngle - ARANMath.DegToRad(3.2)) * fMargin * 0.5;
            else
                fMOCCorrGP = 0.0;

            _HeightLoss = fMargin + fMOCCorrGP + fMOCCorrH;
        }

        public void SetMaxFAPAlt(double val)
        {

            _PrelFAPalt = val;
            double minval = 2.0 * _PrelDHval + ptTHRprj.Z;

            if (_PrelFAPalt < minval)
                _PrelFAPalt = minval;


            int ix = (int)Math.Ceiling((_PrelFAPalt - 152.4) / 152.4);
            if (ix < 0) ix = 0;
            else if (ix > 21) ix = 21;

            _CurrTWC = _MaxTWC = TWCTable[ix].speed / 3.6; //RNPAR 3.2.4
        }

        public void SetVPA(double vpa)
        {

            _VPA = vpa;
            _GPAngle = (double)vpa * ARANMath.DegToRadValue; //RNPAR 4.5.19 RNPAR 4.5.20 RNPAR 4.5.21
            _TanGPA = System.Math.Tan(_GPAngle);
            _CoTanGPA = 1.0 / _TanGPA;
        }

        public void SetCategory(int category)
        {
            _Category = category;
            _IAS = Env.Current.UnitContext.Constants.AircraftCategory[aircraftCategoryData.VfafMax].Value[_Category];
            SetVPAMinMax();
        }

        public void SetVPAMinMax()
        {
            decimal[] MaxGPAngle = new decimal[] { 6.4m, 4.2m, 3.6m, 3.1m, 3.1m };

            _VPAMin = (decimal)ARANMath.RadToDeg(Env.Current.UnitContext.Constants.Pansops[ePANSOPSData.arOptGPAngle].Value);
            _VPAMax = MaxGPAngle[_Category];
        }

        public void SetMinTemperature(double value)
        {

            _Tmin = value;

            if (_Tmin < -100.0)
                _Tmin = -100.0;

            if (_Tmin > 15.0)
                _Tmin = 15.0;
        }

        public override void Commit()
        {
            Rules.Clear();
            Rules.Add(FASRegmentWithRule);
            Rules.Add(MASRegmentWithRule);
            Rules.Add(MASLateralProtectionRule);
            Rules.Add(FASLateralProtectionRule);
            Rules.Add(CalculatingTASRule);
        }


        public override void Clear()
        {
            ClearMapPropArea();
            ClearAreaParams();
        }

        public void ClearAreaParams()
        {
            Env.Current.AranGraphics.SafeDeleteGraphic(_OasLineElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_ZoriginLineElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_SOCElem);
        }

        public void ClearMapPropArea()
        {
            Env.Current.AranGraphics.SafeDeleteGraphic(_FAPElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_MASegmentElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_FASegmentElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_MASplaySegmentElem);
        }

        public void ClearMasPropArea()
        {
            Env.Current.AranGraphics.SafeDeleteGraphic(_MASegmentElem);
            Env.Current.AranGraphics.SafeDeleteGraphic(_MASplaySegmentElem);
        }

        public override void ReCreate()
        {
            ClearMapPropArea();
            CreateMAProtArea();
        }
    }
}

