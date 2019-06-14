using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.AranEnvironment.Symbols;
using Aran.Converters;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Point = Aran.Geometries.Point;

namespace Aran.Omega.Models
{
    public class Annex14Surfaces
    {
        private const double CodeLetterFInnerEdge=140;

        #region -->Fields

        //private List<RunwayCentrelinePoint> _rwyCntlnPtList;
        private List<RwyCenterlineClass> _rwyCntlnClassList;
        private RunwayCentrelinePoint _startCntlnPt, _endCntlnPt;
        private Point _startCntPrj, _endCntPrj;
        private SpatialReferenceOperation _spatialRefOper;
        private Aran.Geometries.Operators.IGeometryOperators _geomOperators;
        private double _direction;
        private Aran.AranEnvironment.IAranGraphics _ui;
        private ElevationDatum _elevDatum;
        private CategoryNumber _catNumber;
        private int _codeNumber;
        private Aran.PANDA.Constants.RunwayConstansList _runwayConstantList;
        private double _refH;
        private RunwayClassificationType _rwyClassifationType;
        private Strip _strip;
        private double _innerHorElevation;
        private RwyDirClass _rwyDir;
        private double _axis;
        private double _clearWay;
        private int InTransPlaneCount;
        private Aran.Geometries.Point _ptRefBalked;
        private IList<RwyClass> _selectedRwyClassList;
        private double _outerHorRadius;
        private bool _isAnnex4Spesification;
        private double _lengthOffInnerEdge;

        #endregion

        #region -->Ctor
        // ReSharper disable once TooManyDependencies
        public Annex14Surfaces(IList<RwyClass> rwyClassList, RwyDirClass rwyDir, ElevationDatum elevDatum,
            RunwayClassificationType rwyClassifactionType, CategoryNumber catNumber, int codeNumber,bool isAnnex4Specification,
            double lengthOffInnnerEdge)
        {
            try
            {
                _selectedRwyClassList = rwyClassList;
                _rwyDir = rwyDir;
                _elevDatum = elevDatum;
                _refH = elevDatum.Height;
                _rwyClassifationType = rwyClassifactionType;
                _catNumber = catNumber;
                _codeNumber = codeNumber;
                _lengthOffInnerEdge = lengthOffInnnerEdge;

                _spatialRefOper = GlobalParams.SpatialRefOperation;
                _geomOperators = GlobalParams.GeomOperators;

                _runwayConstantList = GlobalParams.Constant.RunwayConstants;
                //_rwyCntlnPtList = rwyDir.RwyCntrPtList;
                _rwyCntlnClassList = rwyDir.CenterLineClassList;
                _startCntlnPt = rwyDir.StartCntlPt;
                _endCntlnPt = rwyDir.EndCntlPt;
                _startCntPrj = _spatialRefOper.ToPrj(_startCntlnPt.Location.Geo);
                _startCntPrj.Z = ConverterToSI.Convert(_startCntlnPt.Location.Elevation, 0);
                _endCntPrj = _spatialRefOper.ToPrj(_endCntlnPt.Location.Geo);
                _endCntPrj.Z = ConverterToSI.Convert(_endCntlnPt.Location.Elevation, 0);
                _clearWay = _rwyDir.ClearWay;

                ProfileLinePoint = new List<Point>();

                _direction = ARANFunctions.ReturnAngleInRadians(_startCntPrj, _endCntPrj);
                _axis = _direction + Math.PI;
                _ui = GlobalParams.UI;
                _isAnnex4Spesification = isAnnex4Specification;

                RwyPoints = GetRwyPoints();
                CreateProfileLinePoint();
                //   AnalysesAirportSurfaces();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        #endregion

        #region -->Property
        public InnerHorizontal InnerHorizontalSurface { get; private set; }
        public ConicalSurface ConicalSurface { get; private set; }
        public Strip StripSurface { get; set; }
        public InnerApproach InnerApproach { get; private set; }
        public BalkedLanding BalkedLandingSurface { get; private set; }
        public MultiPoint RwyPoints { get; set; }

        #endregion

        #region -->Methods

        public InnerHorizontal CreateInnerHorizontal()
        {
            Aran.Geometries.Polygon geom = new Polygon();

            RunwayConstants tmpConstants = _runwayConstantList[SurfaceType.InnerHorizontal, DimensionType.Height];
            double height = tmpConstants.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstants = _runwayConstantList[SurfaceType.InnerHorizontal, DimensionType.Radius];
            double radius = tmpConstants.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            _innerHorElevation = height + _elevDatum.Height;

            Ring innerGeo = new Ring();
            foreach (var rwyClass in _selectedRwyClassList)
            {
                //Calculate Arc with start point
                Point starPtPrj = _spatialRefOper.ToPrj(rwyClass.RwyDirClassList[0].StartCntlPt.Location.Geo);
                Point endPrj = _spatialRefOper.ToPrj(rwyClass.RwyDirClassList[0].EndCntlPt.Location.Geo);
                starPtPrj.Z = endPrj.Z = _innerHorElevation;
                innerGeo.Add(starPtPrj);
                innerGeo.Add(endPrj);
            }

            Aran.Geometries.Polygon poly = new Polygon();
            Aran.Geometries.MultiPolygon resultGeo = null;
            if (_selectedRwyClassList.Count > 1)
            {

                poly.ExteriorRing = innerGeo;
                resultGeo = _geomOperators.ConvexHull(poly) as MultiPolygon;
                resultGeo = _geomOperators.Buffer(resultGeo, radius) as MultiPolygon;
            }
            else
            {
                LineString ln = ARANFunctions.ConvertPointsToTrackLIne(innerGeo);
                resultGeo = _geomOperators.Buffer(ln, radius) as MultiPolygon;
            }
            //End

            foreach (Aran.Geometries.Point pt in resultGeo.ToMultiPoint())
                pt.Z = _innerHorElevation;

            InnerHorizontalSurface = new InnerHorizontal()
                .BuildDrawing(new Strategy.UI.SinglePlaneDrawing())
                .BuildObstacleCalculation(new Strategy.ObstacleCalculation.StraightSurfaceObstacleCalculation())
                .BuildStyles() as InnerHorizontal;

            

            InnerHorizontalSurface.GeoPrj = resultGeo;
            InnerHorizontalSurface.Elevation = _innerHorElevation;
            InnerHorizontalSurface.Radius = radius;
            InnerHorizontalSurface.StartPoint = _startCntPrj;
            InnerHorizontalSurface.Height = height;
            InnerHorizontalSurface.Direction = _direction;

            return InnerHorizontalSurface;
        }

        public ConicalSurface CreateConicalPlane()
        {
            try
            {
                RunwayConstants slopeConstant = _runwayConstantList[SurfaceType.CONICAL, DimensionType.Slope];
                double slope = slopeConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

                //Get slope Params
                RunwayConstants heightConstant = _runwayConstantList[SurfaceType.CONICAL, DimensionType.Height];
                double heightConst = heightConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

                double r_conic = heightConst / (slope * 0.01);
                if (InnerHorizontalSurface == null)
                {
                    InnerHorizontalSurface = CreateInnerHorizontal();
                }

                var innerHorizontalGeo = InnerHorizontalSurface.GeoPrj[0];

                var buffer = _geomOperators.Buffer(innerHorizontalGeo, r_conic);
                var resultGeo = (MultiPolygon)_geomOperators.Difference(buffer, innerHorizontalGeo);

                double elevConic = InnerHorizontalSurface.Elevation + heightConst;
                foreach (Aran.Geometries.Polygon polygon in resultGeo)
                {
                    foreach (Aran.Geometries.Ring ring in polygon.InteriorRingList)
                        foreach (Aran.Geometries.Point pt in ring)
                            pt.Z = InnerHorizontalSurface.Elevation;

                    foreach (Aran.Geometries.Point pt in polygon.ExteriorRing)
                        pt.Z = elevConic;
                }

                ConicalSurface = new ConicalSurface
                {
                    GeoPrj = resultGeo,
                    StartPoint = _startCntPrj,
                    Height = heightConst,
                    ElevInnerHor = InnerHorizontalSurface.Elevation,
                    Elevation = elevConic,
                    Slope = slope,
                    EndCntlnPt = _endCntPrj,
                    Radius = r_conic,
                    Direction = _direction,
                    InnerHorRadius = InnerHorizontalSurface.Radius,
                    InnerHorGeo = InnerHorizontalSurface.GeoPrj
                }.BuildDrawing(new Strategy.UI.SinglePlaneDrawing())
                .BuildObstacleCalculation(new Strategy.ObstacleCalculation.SingleSurfaceCalculation())
                .BuildStyles() as ConicalSurface;

                return ConicalSurface;
            }
            catch (Exception e)
            {
                throw new Exception("Error when creating Conical plane:"+e.ToString());
            }
        }

        public OuterHorizontal CreateOuterHorizontal()
        {
            RunwayConstants radiusConstant = _runwayConstantList[SurfaceType.OuterHorizontal, DimensionType.Radius];
            _outerHorRadius = radiusConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            RunwayConstants heightConstant = _runwayConstantList[SurfaceType.CONICAL, DimensionType.Height];
            double heightConic = heightConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            var adhp = GlobalParams.Database.AirportHeliport;

            var adhpPrj = _spatialRefOper.ToPrj(adhp.ARP.Geo);

            


            var outerFull = ARANFunctions.CreateCirclePrj(adhpPrj, _outerHorRadius);
            var tmp = new Polygon();
            tmp.ExteriorRing =outerFull;

            var conicExteriorPolygon = new Polygon();
            conicExteriorPolygon.ExteriorRing = ConicalSurface.GeoPrj[0].ExteriorRing;

            var resultGeo = (MultiPolygon)_geomOperators.Difference(tmp, conicExteriorPolygon);

            foreach (Aran.Geometries.Polygon poly in resultGeo)
            {
                foreach (Aran.Geometries.Ring interiorRing in poly.InteriorRingList)
                {
                    foreach (Aran.Geometries.Point interiorPt in interiorRing)
                    {
                        interiorPt.Z = ConicalSurface.Elevation;
                    }
                }
                foreach (Aran.Geometries.Point exterirorPt in poly.ExteriorRing)
                {
                    exterirorPt.Z = ConicalSurface.Elevation;
                }
            }

            var outerHorizontal = new OuterHorizontal().
                BuildDrawing(new Strategy.UI.SinglePlaneDrawing())
                .BuildObstacleCalculation(new Strategy.ObstacleCalculation.StraightSurfaceObstacleCalculation())
                .BuildStyles() as OuterHorizontal;

            outerHorizontal.GeoPrj = resultGeo;
            outerHorizontal.Radius = _outerHorRadius;
            outerHorizontal.Elevation = ConicalSurface.Elevation;
            outerHorizontal.ConicRadius = ConicalSurface.Radius + InnerHorizontalSurface.Radius;
            outerHorizontal.StartPoint = _startCntPrj;
            outerHorizontal.SurfaceType = SurfaceType.OuterHorizontal;
            outerHorizontal.Conical = ConicalSurface;
            outerHorizontal.InnerHorizontal = InnerHorizontalSurface;
            return outerHorizontal;
        }

        private void CreateProfileLinePoint()
        {
            RunwayConstants tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.DistanceFromThreshold];
            double distanceFromThreshold = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);


            var selectedRwy =_selectedRwyClassList.FirstOrDefault(rwy => rwy.Checked
                                                                  && rwy.RwyDirClassList.Contains(_rwyDir));

            if (selectedRwy==null) throw  new ArgumentNullException($"Selected Runway is empty!");

            var rwyDir2 = selectedRwy.RwyDirClassList.FirstOrDefault(rwydir => rwydir != _rwyDir);

            if (rwyDir2==null) throw new ArgumentNullException($"Runway direction is empty!");

            double maxDistanceFromThreshold = _isAnnex4Spesification ? rwyDir2.ClearWay : rwyDir2.StopWay;
            maxDistanceFromThreshold = Math.Max(maxDistanceFromThreshold, distanceFromThreshold);


            Point startOffSet = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -maxDistanceFromThreshold, 0);
            startOffSet.Z = _startCntPrj.Z;
            ProfileLinePoint.Add(startOffSet);

            for (int i = 0; i < _rwyCntlnClassList.Count; i++)
            {
                Point tmpPtPrj = _rwyCntlnClassList[i].PtPrj;
                tmpPtPrj.Z = _rwyCntlnClassList[i].Elevation;
                ProfileLinePoint.Add(tmpPtPrj);

                //var tmpProfilePt = ARANFunctions.LocalToPrj(_rwyCntlnClassList[i].PtPrj,
                //    _direction+Math.PI/2,
                //   5000 + _rwyCntlnClassList[i].Elevation,0);

                //GlobalParams.UI.DrawPoint(tmpProfilePt, 100);

            }

            maxDistanceFromThreshold = _isAnnex4Spesification ? _rwyDir.ClearWay : _rwyDir.StopWay;
            maxDistanceFromThreshold = Math.Max(maxDistanceFromThreshold, distanceFromThreshold);

            Point endOffSet = ARANFunctions.LocalToPrj(_endCntPrj, _direction, maxDistanceFromThreshold, 0);
            endOffSet.Z = _endCntPrj.Z;
            ProfileLinePoint.Add(endOffSet);
        }

        public Approach CreateApproach()
        {
            try
            {
                double secondSectionSlopeConst = 0;
                double horizontalSecLenConst = 0;
                double secondSectionLenConst = 0, firstSectionSlope;

                Approach approach = new Approach()
                    .BuildDrawing(new Strategy.UI.ApproachDrawing())
                    .BuildObstacleCalculation(new Strategy.ObstacleCalculation.ApproachObstacleCalculation())
                    .BuildStyles() as Approach;

                Aran.Geometries.Ring geoForReport = new Ring();
                Aran.Geometries.Ring firstSectoinPlane = new Ring();

                //first section of Approcah
                //RunwayConstants tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.LengthOfInnerEdge];
                //double lengthOfInnerEdgeConst = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

                var tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.Divergence];
                double divergenceConst = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

                tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.DistanceFromThreshold];
                double distanceFromThresholdConst = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

                Aran.Geometries.Point firstSecPt1 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -distanceFromThresholdConst, _lengthOffInnerEdge / 2);
                Aran.Geometries.Point firstSecPt2 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -distanceFromThresholdConst, -_lengthOffInnerEdge / 2);
                firstSecPt1.Z = firstSecPt2.Z = _startCntPrj.Z;

                firstSectoinPlane.Add(firstSecPt1);
                firstSectoinPlane.Add(firstSecPt2);
                //It is used for drawing and calculating obstacle
                geoForReport.Add(firstSecPt1);
                geoForReport.Add(firstSecPt2);

                tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.FirstSectionLength];
                double firstSectionLenConst = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

                tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.FirstSectionSlope];
                firstSectionSlope = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);


                double widthSec1 = _lengthOffInnerEdge / 2 + firstSectionLenConst * divergenceConst / 100;
                double lengthFromStartToSec1 = distanceFromThresholdConst + firstSectionLenConst;

                Aran.Geometries.Point firstSecPt3 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -lengthFromStartToSec1, -widthSec1);
                Aran.Geometries.Point firstSecPt4 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -lengthFromStartToSec1, widthSec1);
                firstSecPt3.Z = firstSecPt4.Z = (firstSectionLenConst * firstSectionSlope) / 100 + _startCntPrj.Z;

                firstSectoinPlane.Add(firstSecPt3);
                firstSectoinPlane.Add(firstSecPt4);

                approach.Section1 = new Plane();
                approach.Section1.Geo = firstSectoinPlane;

                //It is for Calculating plane equatin.Get the three point of first section
                //Then calculate param.And it will be used Approach class when calculating obstruction

                var planePt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, firstSecPt2);
                var planePt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, firstSecPt3);
                var planePt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, firstSecPt4);

                approach.Section1.Param = CommonFunctions.CalcPlaneParam(planePt1, planePt2, planePt3);

                //second section of Approach

                bool secondPlane = true;
                if (_rwyClassifationType == RunwayClassificationType.NonInstrument || (_rwyClassifationType == RunwayClassificationType.NonPrecisionApproach && _codeNumber == 1))
                {
                    secondPlane = false;
                    geoForReport.Add(firstSecPt3);
                    geoForReport.Add(firstSecPt4);
                    geoForReport.Add(firstSecPt1);
                }

                if (secondPlane)
                {
                    Aran.Geometries.Ring secondSectionPlane = new Ring();

                    tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.SecondSectionLength];
                    secondSectionLenConst = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

                    secondSectionPlane.Add(firstSecPt4);
                    secondSectionPlane.Add(firstSecPt3);

                    double lengthSec2 = lengthFromStartToSec1 + secondSectionLenConst;
                    double widthSec2 = _lengthOffInnerEdge / 2 + (firstSectionLenConst + secondSectionLenConst) * divergenceConst / 100;
                    Aran.Geometries.Point secondSecPt3 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -lengthSec2, -widthSec2);
                    Aran.Geometries.Point secondSecPt4 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -lengthSec2, widthSec2);

                    tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.SecondSectionSlope];
                    secondSectionSlopeConst = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

                    secondSecPt3.Z = secondSecPt4.Z = (secondSectionLenConst * secondSectionSlopeConst) / 100 + firstSecPt4.Z;
                    secondSectionPlane.Add(secondSecPt3);
                    secondSectionPlane.Add(secondSecPt4);

                    //It is for Calculating second section plane equation.Get the two point from first section.
                    //Because it is sharing points both first and second section
                    //Then calculate param.And it will be used Approach class when calculating obstruction
                    approach.Section2 = new Plane();
                    approach.Section2.Geo = secondSectionPlane;

                    planePt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, secondSecPt3);
                    approach.Section2.Param = CommonFunctions.CalcPlaneParam(planePt1, planePt2, planePt3);

                    //horizontal section of Approcah
                    Ring horizontalSectionPlane = new Ring();

                    tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.HorizontalSectoinLength];
                    horizontalSecLenConst = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

                    horizontalSectionPlane.Add(secondSecPt4);
                    horizontalSectionPlane.Add(secondSecPt3);

                    double lengthHorizontalSec = lengthSec2 + horizontalSecLenConst;
                    double widthHorizontalSec = _lengthOffInnerEdge / 2 + (firstSectionLenConst + secondSectionLenConst + horizontalSecLenConst) * divergenceConst / 100;

                    Aran.Geometries.Point horizontalSecPt3 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -lengthHorizontalSec, -widthHorizontalSec);
                    Aran.Geometries.Point horizontalSecPt4 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -lengthHorizontalSec, widthHorizontalSec);
                    horizontalSecPt3.Z = horizontalSecPt4.Z = secondSecPt3.Z;

                    horizontalSectionPlane.Add(horizontalSecPt3);
                    horizontalSectionPlane.Add(horizontalSecPt4);

                    //It is for Calculating second section plane equation.Get the two point from first section.
                    //Because it is sharing points both first and second section
                    //Then calculate param.And it will be used Approach class when calculating obstruction

                    approach.Section3 = new Plane();
                    approach.Section3.Geo = horizontalSectionPlane;
                    approach.Section3.Param = new PlaneParam();
                    approach.Section3.Param.D = horizontalSecPt3.Z;
                    // approach.Section3.Param = CommonFunctions.CalcPlaneParam(horizontalSectionPlane[1], horizontalSectionPlane[2], horizontalSectionPlane[3]);

                    geoForReport.Add(horizontalSecPt3);
                    geoForReport.Add(horizontalSecPt4);
                    geoForReport.Add(geoForReport[0]);
                }

                Polygon poly = new Polygon();
                poly.ExteriorRing = geoForReport;
                MultiPolygon resultGeo = new MultiPolygon();
                resultGeo.Add(poly);

                approach.GeoPrj = resultGeo;
                approach.Direction = _direction;
                approach.StartPoint = _startCntPrj;
                approach.SecondPlane = secondPlane;
                approach.LengthOfInnerEdge = _lengthOffInnerEdge;
                approach.DistanceFromThreshold = distanceFromThresholdConst;
                approach.FirstSectionSlope = firstSectionSlope;
                approach.FirstSectionLength = firstSectionLenConst;
                approach.Divergence = divergenceConst;
                approach.SecondSectionSlope = secondSectionSlopeConst;
                approach.SecondSectionLength = secondSectionLenConst;
                approach.HorizontalSectionLength = horizontalSecLenConst;
                approach.SecondSectionSlope = secondSectionSlopeConst;
                return approach;
            }
            catch (Exception)
            {
                throw new Exception("Error when creating Approach surface");
            }

        }

        public Strip CreateStrip()
        {
            _strip = new Strip().
                 BuildDrawing(new Strategy.UI.MultiPlaneDrawing())
                .BuildObstacleCalculation(new Strategy.ObstacleCalculation.MultiplePlaneSurfaceCalculation())
                .BuildStyles() as Strip;

            var leftMltPt = new MultiPoint();
            var rightMltPt = new MultiPoint();

            //RunwayConstants lengthOfInnerEdgeConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.LengthOfInnerEdge];
            //double lengthOfInnerEdge = lengthOfInnerEdgeConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            var stripElem = new Ring();

            var rightBackPt = ARANFunctions.LocalToPrj(ProfileLinePoint[0], _direction, 0, -_lengthOffInnerEdge / 2);
            var leftBackPt = ARANFunctions.LocalToPrj(ProfileLinePoint[0], _direction, 0, _lengthOffInnerEdge / 2);

            leftBackPt.Z = rightBackPt.Z = ProfileLinePoint[0].Z+_strip.Height;

            stripElem.Add(rightBackPt);
            stripElem.Add(leftBackPt);

            leftMltPt.Add(leftBackPt);
            rightMltPt.Add(rightBackPt);

            Aran.Geometries.Point profilinePt;
            for (int i = 1; i < ProfileLinePoint.Count; i++)
            {
                profilinePt = ProfileLinePoint[i];

                Point leftPt = ARANFunctions.LocalToPrj(profilinePt, _direction, 0, _lengthOffInnerEdge / 2);
                Point rightPt = ARANFunctions.LocalToPrj(profilinePt, _direction, 0, -_lengthOffInnerEdge / 2);
                leftPt.Z = rightPt.Z = profilinePt.Z+_strip.Height;

                stripElem.Add(leftPt);
                stripElem.Add(rightPt);

                //It is for calculation Transitional surface
                leftMltPt.Add(leftPt);
                rightMltPt.Add(rightPt);

                //Create Plane for calculating 
                Aran.Geometries.Point planePt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, stripElem[0]);
                Aran.Geometries.Point planePt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, stripElem[1]);
                Aran.Geometries.Point planePt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, stripElem[2]);

                PlaneParam planeParam = CommonFunctions.CalcPlaneParam(planePt1, planePt2, planePt3);
                Plane tmpPlane = new Plane();
                tmpPlane.Geo = stripElem;
                tmpPlane.Param = planeParam;

                _strip.Planes.Add(tmpPlane);

                stripElem = new Ring();
                stripElem.Add(rightPt);
                stripElem.Add(leftPt);
            }

            var resultGeo = new MultiPolygon();
            foreach (var plane in _strip.Planes)
            {
                var poly = new Polygon {ExteriorRing = plane.Geo};
                resultGeo.Add(poly);
            }

            resultGeo = GlobalParams.GeomOperators.SimplifyGeometry(resultGeo) as Aran.Geometries.MultiPolygon;

            _strip.GeoPrj = resultGeo ;
            _strip.Direction = _direction;
            _strip.StartPoint = _startCntPrj;
            _strip.LeftPts = leftMltPt;
            _strip.RightPts = rightMltPt;
            _strip.LengthOfInnerEdge = _lengthOffInnerEdge;
            return _strip;
        }

        public Area2A CreateArea2A()
        {
            var area2A = new Area2A().
                BuildDrawing(new Strategy.UI.MultiPlaneDrawing())
                .BuildObstacleCalculation(new Strategy.ObstacleCalculation.MultiplePlaneSurfaceCalculation())
                .BuildStyles() as Area2A;
            //RunwayConstants lengthOfInnerEdgeConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.LengthOfInnerEdge];
            //var lengthOfInnerEdge = lengthOfInnerEdgeConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            var stripElem = new Ring();

            var rightBackPt = ARANFunctions.LocalToPrj(ProfileLinePoint[0], _direction, 0, -_lengthOffInnerEdge / 2);
            var leftBackPt = ARANFunctions.LocalToPrj(ProfileLinePoint[0], _direction, 0, _lengthOffInnerEdge / 2);

            leftBackPt.Z = rightBackPt.Z = ProfileLinePoint[0].Z + 3;

            stripElem.Add(rightBackPt);
            stripElem.Add(leftBackPt);

            Aran.Geometries.Point profilinePt;
            for (int i = 1; i < ProfileLinePoint.Count; i++)
            {
                profilinePt = ProfileLinePoint[i];

                Point leftPt = ARANFunctions.LocalToPrj(profilinePt, _direction, 0, _lengthOffInnerEdge / 2);
                Point rightPt = ARANFunctions.LocalToPrj(profilinePt, _direction, 0, -_lengthOffInnerEdge / 2);
                leftPt.Z = rightPt.Z = profilinePt.Z + 3;

                stripElem.Add(leftPt);
                stripElem.Add(rightPt);

                //Create Plane for calculating 
                Aran.Geometries.Point planePt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, stripElem[0]);
                Aran.Geometries.Point planePt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, stripElem[1]);
                Aran.Geometries.Point planePt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, stripElem[2]);

                stripElem.Add(stripElem[0]);
                PlaneParam planeParam = CommonFunctions.CalcPlaneParam(planePt1, planePt2, planePt3);
                Plane tmpPlane = new Plane();
                tmpPlane.Geo = stripElem;
                tmpPlane.Param = planeParam;
                tmpPlane.JtsGeo = new NetTopologySuite.Geometries.Polygon(Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromRing(stripElem));
                area2A.Planes.Add(tmpPlane);

                stripElem = new Ring();
                stripElem.Add(rightPt);
                stripElem.Add(leftPt);
            }

            var resultGeo = new MultiPolygon();
            foreach (var plane in area2A.Planes)
            {
                var poly = new Polygon { ExteriorRing = plane.Geo };
                resultGeo.Add(poly);
            }

            resultGeo = GlobalParams.GeomOperators.SimplifyGeometry(resultGeo) as Aran.Geometries.MultiPolygon;

            area2A.GeoPrj =resultGeo;
            area2A.Direction = _direction;
            area2A.StartPoint = _startCntPrj;
            area2A.LengthOfInnerEdge = _lengthOffInnerEdge;
            return area2A;
        }

        public InnerApproach CreateInnerApproach(CodeLetter codeLetter)
        {
            InnerApproach = new InnerApproach()
                .BuildDrawing(new Strategy.UI.SinglePlaneDrawing())
                .BuildObstacleCalculation(new Strategy.ObstacleCalculation.SingleSurfaceCalculation())
                .BuildStyles() as InnerApproach;

            RunwayConstants tmpConstant = _runwayConstantList[SurfaceType.InnerApproach, DimensionType.DistanceFromThreshold];
            double distanceFromThreshold = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.InnerApproach, DimensionType.Length];
            double length = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.InnerApproach, DimensionType.Width];
            double width = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.InnerApproach, DimensionType.Slope];
            double slope = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            double H_OFZ = 45;

            if (_codeNumber > 2 && codeLetter == CodeLetter.F)
                width = CodeLetterFInnerEdge;

            var side = _direction < 0 ? 1 : -1;

            Aran.Geometries.Point pt1 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -distanceFromThreshold,side * width / 2);
            Aran.Geometries.Point pt2 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -distanceFromThreshold,-(side * width / 2));

            double dist = distanceFromThreshold + length + (H_OFZ - 45) / (slope / 100);

            pt1.Z = pt2.Z = _startCntPrj.Z;

            Aran.Geometries.Point pt3 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -dist,-(side * width / 2));
            Aran.Geometries.Point pt4 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -dist,side * width / 2);

            pt3.Z = pt4.Z = _startCntPrj.Z + ((dist - distanceFromThreshold) * (slope / 100)) + (H_OFZ - 45);

            var geo = CommonFunctions.CreateMultipolygonFromPoints(new Aran.Geometries.Point[] { pt1, pt2, pt3, pt4 });

            //It is for Calculating  plane equation.Get the thres point of inner approach.
            //Then calculate param.And it will be used Inner Approach class when calculating obstruction

            Aran.Geometries.Point planePt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt1);
            Aran.Geometries.Point planePt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt2);

            Aran.Geometries.Point planePt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt3);

            //End calculating
            InnerApproach.PlaneParam = CommonFunctions.CalcPlaneParam(planePt1, planePt2, planePt3);
            InnerApproach.GeoPrj = geo;
            InnerApproach.Length = length;
            InnerApproach.Slope = slope;
            InnerApproach.Direction = _direction;
            InnerApproach.StartPoint = _startCntPrj;
            InnerApproach.DistanceFromThreshold = distanceFromThreshold;
            InnerApproach.Width = width;
            InnerApproach.H_OFZ = H_OFZ;
            return InnerApproach;
        }

        public Transitional CreateTransitionalSurface()
        {
            var transitional = new Transitional()
                .BuildDrawing(new Strategy.UI.MultiPlaneDrawing())
                .BuildObstacleCalculation(new Strategy.ObstacleCalculation.MultiplePlaneSurfaceCalculation())
                .BuildStyles() as Transitional;

            //Load all constants from Constant tables
            RunwayConstants tmpConstant = _runwayConstantList[SurfaceType.Transitional, DimensionType.Slope];
            double transSlope = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.DistanceFromThreshold];
            double distanceFromThreshold = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.Divergence];
            double appDiver = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.FirstSectionSlope];
            double appSlope = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.InnerHorizontal, DimensionType.Height];
            double innerHorHeight = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            double appLenInnerEdge = _lengthOffInnerEdge;
            //End getting constants

            var planes = new List<Plane>();

            for (int i = 0; i < ProfileLinePoint.Count - 1; i++)
            {
                var profilePointStart = ProfileLinePoint[i];
                var profilePointEnd = ProfileLinePoint[i+1];
                if (i == 0)
                {
                    profilePointStart = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -distanceFromThreshold, 0);
                    profilePointEnd = _startCntPrj;

                    profilePointStart.Z = profilePointEnd.Z = _startCntPrj.Z;
                }

                var startPt = ARANFunctions.LocalToPrj(profilePointStart, _direction, 0, _lengthOffInnerEdge / 2);
                var endPt = ARANFunctions.LocalToPrj(profilePointEnd, _direction, 0, _lengthOffInnerEdge / 2);

                startPt.Z = profilePointStart.Z;
                endPt.Z = profilePointEnd.Z;

                planes.Add(CalculateTransitionalSector(transSlope,startPt,endPt, SideDirection.sideLeft));

                startPt = ARANFunctions.LocalToPrj(profilePointStart, _direction, 0, - _lengthOffInnerEdge / 2);
                endPt = ARANFunctions.LocalToPrj(profilePointEnd, _direction, 0, - _lengthOffInnerEdge / 2);

                startPt.Z = profilePointStart.Z;
                endPt.Z = profilePointEnd.Z;

                planes.Add(CalculateTransitionalSector(transSlope, startPt,endPt, SideDirection.sideRight));
            }

            var app45LineLeft = new Ring();
            double appLength = distanceFromThreshold + (InnerHorizontalSurface.Elevation-_startCntPrj.Z) * 100 / appSlope;
            double appWidth = (appLength - distanceFromThreshold) * appDiver / 100 + appLenInnerEdge / 2;

            Aran.Geometries.Point leftAppPt = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -appLength, appWidth);
            Aran.Geometries.Point rightAppPt = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -appLength, -appWidth);
            leftAppPt.Z = rightAppPt.Z = _innerHorElevation;

            var leftPlane = new Plane();
            app45LineLeft.Add(leftAppPt);
            app45LineLeft.Add(ARANFunctions.LocalToPrj(_startCntPrj, _direction, -distanceFromThreshold, _lengthOffInnerEdge/2));
            app45LineLeft.Add(planes[0].Geo[1]);
            app45LineLeft[1].Z = _startCntPrj.Z;
            

            var localApp1Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, app45LineLeft[0]);
            var localApp2Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, app45LineLeft[1]);
            var localApp3Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, app45LineLeft[2]);

            var leftAppPlaneParam = CommonFunctions.CalcPlaneParam(localApp1Pt, localApp2Pt, localApp3Pt);
            leftPlane.Param = leftAppPlaneParam;
            leftPlane.Geo = app45LineLeft;
            planes.Add(leftPlane);

            var rightPlane = new Plane();
            var app45LineRight = new Ring();
            app45LineRight.Add(rightAppPt);
            app45LineRight.Add(ARANFunctions.LocalToPrj(_startCntPrj, _direction, -distanceFromThreshold, -_lengthOffInnerEdge / 2));
            app45LineRight[1].Z = _startCntPrj.Z;
            app45LineRight.Add(planes[1].Geo[1]);

            var localAppRight1Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, app45LineRight[0]);
            var localAppRight2Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, app45LineRight[1]);
            var localAppRight3Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, app45LineRight[2]);

            var rightPlaneParam = CommonFunctions.CalcPlaneParam(localAppRight1Pt, localAppRight2Pt, localAppRight3Pt);

            rightPlane.Param = rightPlaneParam;
            rightPlane.Geo = app45LineRight;
            planes.Add(rightPlane);

            var resultGeo = new MultiPolygon();
            foreach (var plane in planes)
            {
                var poly = new Polygon { ExteriorRing = plane.Geo };
                resultGeo.Add(poly);
            }
            transitional.GeoPrj = resultGeo;
            transitional.Slope = transSlope;
            transitional.Planes = planes;
            transitional.StartPoint = _startCntPrj;
            transitional.Direction = _direction;
            return transitional;
        }

        private Plane CalculateTransitionalSector(double transSlope,Aran.Geometries.Point startPt,Aran.Geometries.Point endPt,SideDirection sideDirection)
        {
            double withInnerEdgeCur, withInnerEdgeNext;
            Point ptCurLeft, ptNextLeft, ptCurLeftLocal, ptNextCurLocal;

            withInnerEdgeCur = (_innerHorElevation - startPt.Z) * 100 / transSlope;
            withInnerEdgeNext = (_innerHorElevation - endPt.Z) * 100 / transSlope;

            //Left Plane Calculation
            ptCurLeft = ARANFunctions.LocalToPrj(startPt, _direction, 0,(int)sideDirection * withInnerEdgeCur);
            ptNextLeft = ARANFunctions.LocalToPrj(endPt, _direction, 0,(int)sideDirection * withInnerEdgeNext);
            ptCurLeft.Z = ptNextLeft.Z = _innerHorElevation;

            ptCurLeftLocal = ARANFunctions.PrjToLocal(_startCntPrj, _axis, ptCurLeft);
            ptCurLeftLocal.Z = ptCurLeft.Z;
            ptNextCurLocal = ARANFunctions.PrjToLocal(_startCntPrj, _axis, ptNextLeft);
            ptNextCurLocal.Z = ptNextLeft.Z;
            var leftPtLocal = ARANFunctions.PrjToLocal(_startCntPrj, _axis, startPt);
            leftPtLocal.Z = startPt.Z;

            var tmpPlaneLeft = new Plane { Geo = new Ring { startPt, ptCurLeft, ptNextLeft, endPt }, Param = CommonFunctions.CalcPlaneParam(leftPtLocal, ptCurLeftLocal, ptNextCurLocal) };
            return tmpPlaneLeft;
        }

        public BalkedLanding CreateBalkedLandingSurface(CodeLetter codeLetter)
        {
            BalkedLandingSurface = new BalkedLanding()
                .BuildDrawing(new Strategy.UI.SinglePlaneDrawing())
                .BuildObstacleCalculation(new Strategy.ObstacleCalculation.SingleSurfaceCalculation())
                .BuildStyles() as BalkedLanding;

            //Load all constants from Constant tables
            RunwayConstants tmpConstant = _runwayConstantList[SurfaceType.BalkedLanding, DimensionType.DistanceFromThreshold];
            double distanceFromThreshold = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.BalkedLanding, DimensionType.LengthOfInnerEdge];
            double lenghtOfInnerEdge = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.BalkedLanding, DimensionType.Divergence];
            double divergence = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.BalkedLanding, DimensionType.Slope];
            double slope = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);


            double runwayLength = _rwyDir.Length;
            if (_codeNumber > 2 && codeLetter == CodeLetter.F)
                lenghtOfInnerEdge = CodeLetterFInnerEdge;

            if (_codeNumber < 3)
                distanceFromThreshold = runwayLength;
            else
                if (runwayLength < 1800)
                    distanceFromThreshold = runwayLength;

            _ptRefBalked = ARANFunctions.LocalToPrj(_startCntPrj, _direction, distanceFromThreshold, 0);

            //Find the distance reference balked landing point
            if (distanceFromThreshold == runwayLength)
            {
                _ptRefBalked.Z = ConverterToSI.Convert(_rwyDir.EndCntlPt.Location.Elevation, 0);
            }
            else
            {
                SideDirection side1 = ARANMath.SideDef(_ptRefBalked, _direction + Math.PI / 2, ProfileLinePoint[0]);

                for (int i = 1; i < ProfileLinePoint.Count - 1; i++)
                {
                    SideDirection side2 = ARANMath.SideDef(_ptRefBalked, _direction + Math.PI / 2, ProfileLinePoint[i]);

                    if ((int)side1 * (int)side2 < 0)
                    {
                        Aran.Geometries.Point tmpPt1 = ProfileLinePoint[i - 1];
                        Aran.Geometries.Point tmpPt2 = ProfileLinePoint[i];
                        double fTmp = ARANFunctions.ReturnDistanceInMeters(tmpPt1, tmpPt2);
                        double fTmp1 = ARANFunctions.ReturnDistanceInMeters(tmpPt1, _ptRefBalked);

                        _ptRefBalked.Z = tmpPt1.Z + fTmp1 * (tmpPt2.Z - tmpPt1.Z) / fTmp;
                        InTransPlaneCount = i;
                        break;
                    }
                }
            }

            Aran.Geometries.Point pt1 = ARANFunctions.LocalToPrj(_ptRefBalked, _direction, 0, lenghtOfInnerEdge / 2);
            Aran.Geometries.Point pt2 = ARANFunctions.LocalToPrj(_ptRefBalked, _direction, 0, -lenghtOfInnerEdge / 2);

            pt1.Z = pt2.Z = _ptRefBalked.Z;

            double lengthBalkedLanding = (_innerHorElevation - _ptRefBalked.Z) * 100 / slope;

            double widthBalkedLanding = lenghtOfInnerEdge / 2 + ((_innerHorElevation - _ptRefBalked.Z) / slope) * divergence;

            Aran.Geometries.Point pt3 = ARANFunctions.LocalToPrj(_ptRefBalked, _direction, lengthBalkedLanding, -widthBalkedLanding);
            Aran.Geometries.Point pt4 = ARANFunctions.LocalToPrj(_ptRefBalked, _direction, lengthBalkedLanding, widthBalkedLanding);
            pt3.Z = pt4.Z = _innerHorElevation;
            MultiPolygon geo = CommonFunctions.CreateMultipolygonFromPoints(new Aran.Geometries.Point[] { pt1, pt2, pt3, pt4 });

            //GlobalParams.UI.DrawPointWithText(_ptRefBalked, 200, "Ref Balked");
            //GlobalParams.UI.DrawPointWithText(pt1, 200, "pt1");
            //GlobalParams.UI.DrawPointWithText(pt2, 200, "pt2");
            //GlobalParams.UI.DrawPointWithText(pt3, 200, "pt3");
            //GlobalParams.UI.DrawPointWithText(pt4, 200, "pt4");

            //Create plane 
            Aran.Geometries.Point planePt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt1);
            Aran.Geometries.Point planePt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt2);
            Aran.Geometries.Point planePt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt3);
            PlaneParam planeParam = CommonFunctions.CalcPlaneParam(planePt1, planePt2, planePt3);

            //

            BalkedLandingSurface.GeoPrj = geo;
            BalkedLandingSurface.PlaneParam = planeParam;
            BalkedLandingSurface.LengthOfInnerEdge = lenghtOfInnerEdge;
            BalkedLandingSurface.DistanceFromTheshold = distanceFromThreshold;
            BalkedLandingSurface.Divergence = divergence;
            BalkedLandingSurface.Slope = slope;
            BalkedLandingSurface.StartPoint = _startCntPrj;
            BalkedLandingSurface.Direction = _direction;
            return BalkedLandingSurface;
        }

        public InnerTransitional CreateInnerTransitional(CodeLetter codeLetter)
        {
            var innerTransitional = new InnerTransitional()
                .BuildDrawing(new Strategy.UI.MultiPlaneDrawing())
                .BuildObstacleCalculation(new Strategy.ObstacleCalculation.MultiplePlaneSurfaceCalculation())
                .BuildStyles() as InnerTransitional;

            var leftReportGeo = new Ring();
            var rightReportGeo = new Ring();

            int ptCount = (2 * (InTransPlaneCount + 1)) + 3;
            for (int i = 0; i < ptCount; i++)
            {
                leftReportGeo.Add(new Point());
                rightReportGeo.Add(new Point());
            }

            var tmpConstant = _runwayConstantList[SurfaceType.InnerTransitional, DimensionType.Slope];
            var slope = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            var innerApplength = InnerApproach.Length;
            var innerAppSlope = InnerApproach.Slope;


            //Inner approach calculation
            double H_OFZ = 45;


            tmpConstant = _runwayConstantList[SurfaceType.InnerApproach, DimensionType.DistanceFromThreshold];
            double distanceFromThreshold = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.InnerApproach, DimensionType.Length];
            double length = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.InnerApproach, DimensionType.Width];
            double innerApproachWidth = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            if (_codeNumber > 2 && codeLetter == CodeLetter.F)
                innerApproachWidth = CodeLetterFInnerEdge;

            var side = _direction < 0 ? 1 : -1;

            Aran.Geometries.Point innerApppt1 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -distanceFromThreshold,side * innerApproachWidth / 2);
            Aran.Geometries.Point innerApppt2 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -distanceFromThreshold,- (side * innerApproachWidth / 2));

            double dist = distanceFromThreshold + length + (H_OFZ - 45) / (innerAppSlope / 100);

            innerApppt1.Z = innerApppt2.Z = _startCntPrj.Z;

            Aran.Geometries.Point innerApppt3 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -dist, -(side * innerApproachWidth / 2));
            Aran.Geometries.Point innerApppt4 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -dist,side * innerApproachWidth / 2);

            innerApppt3.Z = innerApppt4.Z = _startCntPrj.Z + ((dist - distanceFromThreshold) * (innerAppSlope / 100)) + (H_OFZ - 45);

            //end calculation innerapproach

            
            
            //First create plane which near inner approach
            

            var widthInAppStart = (_innerHorElevation - _startCntPrj.Z) * 100 / slope;
            var widthInAppEnd = (_innerHorElevation - _startCntPrj.Z - innerAppSlope * innerApplength / 100) * 100 / slope;


            Aran.Geometries.Point innerAppLeftPt1 = ARANFunctions.LocalToPrj(innerApppt1, _direction, 0, side * widthInAppStart);
            Aran.Geometries.Point innerAppLeftPt2 = ARANFunctions.LocalToPrj(innerApppt4, _direction, 0, side * widthInAppEnd);
            innerAppLeftPt1.Z = innerAppLeftPt2.Z = _innerHorElevation;


            Aran.Geometries.Point innerAppRightPt1 = ARANFunctions.LocalToPrj(innerApppt2, _direction, 0, -side * widthInAppStart);
            Aran.Geometries.Point innerAppRightPt2 = ARANFunctions.LocalToPrj(innerApppt3, _direction, 0, -side * widthInAppEnd);
            innerAppRightPt1.Z = innerAppRightPt2.Z = _innerHorElevation;

            //end 

            //Extent

            leftReportGeo[0] = innerAppLeftPt2;
            leftReportGeo[1] = innerAppLeftPt1;
            leftReportGeo[ptCount - 1] = innerApppt4;
            leftReportGeo[ptCount - 2] = innerApppt1;
            //

            var innerAppLeftGeo = new Ring { innerApppt1, innerAppLeftPt1, innerAppLeftPt2, innerApppt4 };

            //Create plane for left geo
            var leftInnerAppLocalPt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, innerAppLeftPt1);
            var leftInnerAppLocalPt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, innerAppLeftPt2);
            var leftInnerAppLocalPt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, innerApppt4);

            var leftPlaneParam = CommonFunctions.CalcPlaneParam(leftInnerAppLocalPt1, leftInnerAppLocalPt2, leftInnerAppLocalPt3);
            var leftInnerAppPlane = new Plane {Geo = innerAppLeftGeo, Param = leftPlaneParam};

           // GlobalParams.UI.DrawRing(innerAppLeftGeo, 100, eFillStyle.sfsBackwardDiagonal);

            //GlobalParams.UI.DrawPoint(innerApppt1, 100, ePointStyle.smsCircle);
           // GlobalParams.UI.DrawPoint(innerAppGeo[3], 100, ePointStyle.smsCircle);

            innerTransitional.Planes.Add(leftInnerAppPlane);

            //right plane
          

            //Extent

            rightReportGeo[0] = innerAppRightPt2;
            rightReportGeo[1] = innerAppRightPt1;
            rightReportGeo[ptCount - 1] = innerApppt2;
            rightReportGeo[ptCount - 2] = innerApppt3;
            //

            var innerAppRightGeo = new Ring
            {
                innerApppt2,
                innerAppRightPt1,
                innerAppRightPt2,
                innerApppt3
            };

            //Create plane for right geo
            var rightInnerAppLocalPt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, innerAppRightPt1);
            var rightInnerAppLocalPt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, innerAppRightPt2);
            var rightInnerAppLocalPt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, innerApppt3);
            var rightInnerAppPlaneParam = CommonFunctions.CalcPlaneParam(rightInnerAppLocalPt1, rightInnerAppLocalPt2, rightInnerAppLocalPt3);
            var rightInnerAppPlane = new Plane {Geo = innerAppRightGeo, Param = rightInnerAppPlaneParam};

            innerTransitional.Planes.Add(rightInnerAppPlane);

            double innerAppWidth = InnerApproach.Width / 2;

            Aran.Geometries.Point currPt = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -distanceFromThreshold, 0);
            currPt.Z = _startCntPrj.Z;

            int nearestProfileneIndexToInnerApproach = NearestProfileneIndexToInnerApproach(currPt);

            //
            for (int i = nearestProfileneIndexToInnerApproach; i < InTransPlaneCount; i++)
            {
                var nextCntPt = ProfileLinePoint[i + 1];
                if (i + 1 == InTransPlaneCount)
                    nextCntPt = _ptRefBalked;

                var widthCur = innerAppWidth + (_innerHorElevation - currPt.Z) * 100 / slope;
                var widthNext = innerAppWidth + (_innerHorElevation - nextCntPt.Z) * 100 / slope;

                Aran.Geometries.Point leftPlanePt1 = ARANFunctions.LocalToPrj(currPt, _direction, 0, innerAppWidth);
                Aran.Geometries.Point leftPlanePt2 = ARANFunctions.LocalToPrj(nextCntPt, _direction, 0, innerAppWidth);
                Aran.Geometries.Point leftPlanePt3 = ARANFunctions.LocalToPrj(nextCntPt, _direction, 0, widthNext);
                Aran.Geometries.Point leftPlanePt4 = ARANFunctions.LocalToPrj(currPt, _direction, 0, widthCur);
                leftPlanePt1.Z = ProfileLinePoint[i].Z;
                leftPlanePt2.Z = nextCntPt.Z;
                leftPlanePt3.Z = leftPlanePt4.Z = _innerHorElevation;

                var leftGeo = new Ring {leftPlanePt1, leftPlanePt2, leftPlanePt3, leftPlanePt4};

                //crete report geo left
                leftReportGeo[i + 2] = leftPlanePt3;
                leftReportGeo[ptCount - 3 - i] = leftPlanePt2;
                //

                //Create left plane param
                var leftPlane = new Plane {Geo = leftGeo};
                innerTransitional.Planes.Add(leftPlane);
                leftPlane.Param = CommonFunctions.CalcPlaneParamFromPrjPts(_startCntPrj, _axis, leftPlanePt1, leftPlanePt2, leftPlanePt3);
                //

                //Create right plane geo
                var rightPlanePt1 = ARANFunctions.LocalToPrj(currPt, _direction, 0, -innerAppWidth);
                var rightPlanePt2 = ARANFunctions.LocalToPrj(nextCntPt, _direction, 0, -innerAppWidth);

                var rightPlanePt3 = ARANFunctions.LocalToPrj(nextCntPt, _direction, 0, -widthNext);
                var rightPlanePt4 = ARANFunctions.LocalToPrj(currPt, _direction, 0, -widthCur);
                rightPlanePt1.Z = ProfileLinePoint[i].Z;
                rightPlanePt2.Z = nextCntPt.Z;
                rightPlanePt3.Z = rightPlanePt4.Z = _innerHorElevation;

                var rightGeo = new Ring {rightPlanePt1, rightPlanePt2, rightPlanePt3, rightPlanePt4};

                //crete report geo right
                rightReportGeo[i + 2] = rightPlanePt3;
                rightReportGeo[ptCount - 3 - i] = rightPlanePt2;
                //

                //Create left plane param
                var rightPlane = new Plane
                {
                    Geo = rightGeo,
                    Param =CommonFunctions.CalcPlaneParamFromPrjPts(_startCntPrj, _axis, rightPlanePt1, rightPlanePt2,
                            rightPlanePt3)
                };

                innerTransitional.Planes.Add(rightPlane);

                currPt = nextCntPt;
            }

            //Create plane which near balked landing surface


            tmpConstant = _runwayConstantList[SurfaceType.BalkedLanding, DimensionType.LengthOfInnerEdge];
            double lenghtOfInnerEdge = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            if (_codeNumber > 2 && codeLetter == CodeLetter.F)
                lenghtOfInnerEdge = CodeLetterFInnerEdge;

            tmpConstant = _runwayConstantList[SurfaceType.BalkedLanding, DimensionType.Divergence];
            double balkedDivergence = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.BalkedLanding, DimensionType.Slope];
            double balkendSlope = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            double lengthBalkedLanding = (_innerHorElevation - _ptRefBalked.Z) * 100 / balkendSlope;

            double widthBalkedLanding = lenghtOfInnerEdge / 2 + ((_innerHorElevation - _ptRefBalked.Z) / balkendSlope) * balkedDivergence;

            Aran.Geometries.Point balkedPt3 = ARANFunctions.LocalToPrj(_ptRefBalked, _direction, lengthBalkedLanding, widthBalkedLanding);
            Aran.Geometries.Point balkedPt4 = ARANFunctions.LocalToPrj(_ptRefBalked, _direction, lengthBalkedLanding, -widthBalkedLanding);
            balkedPt3.Z = balkedPt4.Z = _innerHorElevation;


            var leftBalkedGeo = new Ring
            {
                innerTransitional.Planes[innerTransitional.Planes.Count - 2].Geo[1],
                innerTransitional.Planes[innerTransitional.Planes.Count - 2].Geo[2],
                balkedPt3
            };

            var leftBalkedPlaneParam = CommonFunctions.CalcPlaneParamFromPrjPts(_startCntPrj, _axis, leftBalkedGeo[0], leftBalkedGeo[1], leftBalkedGeo[2]);

            //Report geo
            leftReportGeo[ptCount / 2] = leftBalkedGeo[2];
            //
            var leftBalkedPlane = new Plane {Geo = leftBalkedGeo, Param = leftBalkedPlaneParam};

            //end

            var rightBalkedGeo = new Ring
            {
                innerTransitional.Planes[innerTransitional.Planes.Count - 1].Geo[1],
                innerTransitional.Planes[innerTransitional.Planes.Count - 1].Geo[2],
                balkedPt4
            };

            var rightBalkedPlaneParam = CommonFunctions.CalcPlaneParamFromPrjPts(_startCntPrj, _axis, rightBalkedGeo[0], rightBalkedGeo[1], rightBalkedGeo[2]);

            //report geo
            rightReportGeo[ptCount / 2] = rightBalkedGeo[2];
            //end

            var rightBalkedPlane = new Plane {Geo = rightBalkedGeo, Param = rightBalkedPlaneParam};

            innerTransitional.Planes.Add(leftBalkedPlane);
            innerTransitional.Planes.Add(rightBalkedPlane);

            var resultGeo = new MultiPolygon();
            foreach (var plane in innerTransitional.Planes)
            {
                var poly = new Polygon { ExteriorRing = plane.Geo };
                resultGeo.Add(poly);
            }
            innerTransitional.GeoPrj = resultGeo;

            innerTransitional.Direction = _direction;
            innerTransitional.StartPoint = _startCntPrj;
            innerTransitional.Slope = slope;
            return innerTransitional;
        }

        private int NearestProfileneIndexToInnerApproach(Point innerApproachStartPoint)
        {
            for (int i = 0; i < ProfileLinePoint.Count; i++)
            {
                if (ARANFunctions.ReturnDistanceInMeters(innerApproachStartPoint, ProfileLinePoint[i]) < 0.1)
                    return i;

                var angleFromInnerApproachStartPoint = ARANFunctions.ReturnAngleInRadians(innerApproachStartPoint, ProfileLinePoint[i]);
                if (Math.Abs(angleFromInnerApproachStartPoint - _direction) < 0.1)
                    return i-1;
            }

            throw new Exception("Parameters is not correct to construct Inner Transitional");
        }

        public TakeOffClimb CreateTakeOffSurface(bool isGreaterThan15,double slope)
        {
            var takeOffGeo = new Ring();
            var planes = new List<Plane>();
            var classification = RunwayClassificationType.NonInstrument;

            RunwayConstants tmpConstant = _runwayConstantList[SurfaceType.TakeOffClimb, DimensionType.LengthOfInnerEdge];
            double lengthOfInnerEdge = tmpConstant.GetValue(classification, 0, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.TakeOffClimb, DimensionType.Divergence];
            double divergence = tmpConstant.GetValue(classification, 0, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.TakeOffClimb, DimensionType.DistanceFromRunwayEnd];
            double distanceFromRunwayEnd = tmpConstant.GetValue(classification, 0, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.TakeOffClimb, DimensionType.FinalWidth];
            double finalWidth = tmpConstant.GetValue(classification, 0, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.TakeOffClimb, DimensionType.Length];
            double length = tmpConstant.GetValue(classification, 0, _codeNumber);

            if (Math.Abs(slope-1.2)< 0.01)
                length = 10000;
            else if (Math.Abs(slope-1.0)<0.01)
                length = 12000;

            //tmpConstant = _runwayConstantList[SurfaceType.TakeOffClimb, DimensionType.Slope];
            //double slope = tmpConstant.GetValue(classification, 0, _codeNumber);

            if (_clearWay > distanceFromRunwayEnd)
                distanceFromRunwayEnd = _clearWay;

            if (isGreaterThan15)
                finalWidth = 1800;

            Aran.Geometries.Point pt1 = ARANFunctions.LocalToPrj(_endCntPrj, _direction, distanceFromRunwayEnd, lengthOfInnerEdge / 2);
            Aran.Geometries.Point pt2 = ARANFunctions.LocalToPrj(_endCntPrj, _direction, distanceFromRunwayEnd, -lengthOfInnerEdge / 2);
            pt1.Z = pt2.Z = _endCntPrj.Z;

            double section1Length = ((finalWidth - lengthOfInnerEdge) / 2) * 100 / divergence;

            var pt3 = ARANFunctions.LocalToPrj(_endCntPrj, _direction, section1Length + distanceFromRunwayEnd, -finalWidth / 2);
            pt3.Z = (section1Length * slope) / 100 + _endCntPrj.Z;

            takeOffGeo.Add(pt1);
            takeOffGeo.Add(pt2);
            takeOffGeo.Add(pt3);

            var planePt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt1);
            var planePt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt2);
            var planePt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt3);
            var firstSectionParam = CommonFunctions.CalcPlaneParam(planePt1, planePt2, planePt3);
            //First section plane 
            var planeGeo = new Ring
            {
                pt1,
                pt2,
                pt3,
                ARANFunctions.LocalToPrj(_endCntPrj, _direction,distanceFromRunwayEnd+ section1Length, finalWidth/2)
            };

            var firstSectionPlane = new Plane { Geo = planeGeo, Param = firstSectionParam };

            planes.Add(firstSectionPlane);

            if (_codeNumber <= 2)
            {
                var pt4 = ARANFunctions.LocalToPrj(_endCntPrj, _direction, section1Length + distanceFromRunwayEnd, finalWidth / 2);
                pt4.Z = pt3.Z;
                takeOffGeo.Add(pt4);
            }
            else
            {
                var pt4 = ARANFunctions.LocalToPrj(_endCntPrj, _direction, length + distanceFromRunwayEnd, -finalWidth / 2);
                var pt5 = ARANFunctions.LocalToPrj(_endCntPrj, _direction, length + distanceFromRunwayEnd, finalWidth / 2);
                pt4.Z = pt5.Z = length * slope / 100 + _endCntPrj.Z;

                var pt6 = ARANFunctions.LocalToPrj(_endCntPrj, _direction, section1Length + distanceFromRunwayEnd, finalWidth / 2);
                pt6.Z = pt3.Z;

                takeOffGeo.Add(pt4);
                takeOffGeo.Add(pt5);
                takeOffGeo.Add(pt6);

                var planePt4 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt4);
                var planePt5 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt5);
                var secondSectionParam = CommonFunctions.CalcPlaneParam(planePt3, planePt4, planePt5);

                var secondSectionPlane = new Plane { Geo = new Ring { pt3, pt4, pt5, pt6 }, Param = secondSectionParam };
                planes.Add(secondSectionPlane);
            }

            var mlt = new MultiPolygon { new Polygon { ExteriorRing = takeOffGeo } };

            var takeOfClimb = new TakeOffClimb
            {
                Direction = _direction,
                GeoPrj = mlt,
                Planes = planes,
                StartPoint = _startCntPrj,
                LengthOfInnerEdge = lengthOfInnerEdge,
                DistanceFromThreshold = distanceFromRunwayEnd,
                Divergence = divergence,
                FinalWidth = finalWidth,
                Slope = slope,
                Length = length
            }.BuildDrawing(new Strategy.UI.MultiPlaneDrawing())
                .BuildObstacleCalculation(new Strategy.ObstacleCalculation.MultiplePlaneSurfaceCalculation())
                .BuildStyles() as TakeOffClimb;
            return takeOfClimb;
        }

        public List<Aran.Geometries.Point> ProfileLinePoint { get; private set; }

        public TakeOffClimb CreateTakeOffPlane(double slope)
        {
            var takeOffGeo = new Ring();
            var planes = new List<Plane>();

            TakeOffClimb takeOffClimb = new TakeOffClimb()
                .BuildDrawing(new Strategy.UI.MultiPlaneDrawing())
                .BuildObstacleCalculation(new Strategy.ObstacleCalculation.MultiplePlaneSurfaceCalculation())
                .BuildStyles() as TakeOffClimb;

            RunwayConstants tmpConstant = _runwayConstantList[SurfaceType.TakeOffClimb, DimensionType.LengthOfInnerEdge];
            double lengthOfInnerEdge = tmpConstant.GetValue(_rwyClassifationType, 0, _codeNumber);

            var length = Annex15Surfaces.Area2BLength;

            if (Math.Abs(slope - 1.2) < 0.01)
                length = 10000;
            else if (Math.Abs(slope - 1.0) < 0.01)
                length = 12000;

            //tmpConstant = _runwayConstantList[SurfaceType.TakeOffClimb, DimensionType.Slope];
            //double slope = tmpConstant.GetValue(classification, 0, _codeNumber);

            var pt = _endCntPrj;
            if (Math.Abs(_rwyDir.ClearWay) > 0.1)
            {
                pt = ARANFunctions.LocalToPrj(_endCntPrj, _direction, _rwyDir.ClearWay, 0);
                pt.Z = _endCntPrj.Z;
            }
            double finalWidth = 1800;
            double divergence = 12.5;

            //var pt = ProfileLinePoint[ProfileLinePoint.Count - 1];

            var pt1 = ARANFunctions.LocalToPrj(pt, _direction, 0, lengthOfInnerEdge/2);
            var pt2 = ARANFunctions.LocalToPrj(pt, _direction, 0, -lengthOfInnerEdge/2);
            pt1.Z = pt2.Z = pt.Z;

            double section1Length = ((finalWidth - lengthOfInnerEdge)/2)*100/divergence;

            var pt3 = ARANFunctions.LocalToPrj(pt, _direction, section1Length, -finalWidth/2);
            pt3.Z = (section1Length*slope)/100 + pt.Z;

            takeOffGeo.Add(pt1);
            takeOffGeo.Add(pt2);
            takeOffGeo.Add(pt3);

            var planePt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt1);
            var planePt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt2);
            var planePt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt3);
            var firstSectionParam = CommonFunctions.CalcPlaneParam(planePt1, planePt2, planePt3);
            //First section plane 
            var planeGeo = new Ring
            {
                pt1,
                pt2,
                pt3,
                ARANFunctions.LocalToPrj(pt, _direction, section1Length, finalWidth/2)
            };

            var firstSectionPlane = new Plane {Geo = planeGeo, Param = firstSectionParam};

            planes.Add(firstSectionPlane);


            var pt4 = ARANFunctions.LocalToPrj(pt, _direction, length, -finalWidth/2);
            var pt5 = ARANFunctions.LocalToPrj(pt, _direction, length, finalWidth/2);
            pt4.Z = pt5.Z = length*slope/100 + pt.Z;

            var pt6 = ARANFunctions.LocalToPrj(pt, _direction, section1Length, finalWidth/2);
            pt6.Z = pt3.Z;

            takeOffGeo.Add(pt4);
            takeOffGeo.Add(pt5);
            takeOffGeo.Add(pt6);

            var planePt4 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt4);
            var planePt5 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt5);
            var secondSectionParam = CommonFunctions.CalcPlaneParam(planePt3, planePt4, planePt5);

            var secondSectionPlane = new Plane {Geo = new Ring {pt3, pt4, pt5, pt6}, Param = secondSectionParam};
            planes.Add(secondSectionPlane);

            var resultGeo = new MultiPolygon {new Polygon {ExteriorRing = takeOffGeo}};

            takeOffClimb.GeoPrj = resultGeo;
            takeOffClimb.Planes = planes;
            takeOffClimb.Direction = _direction;
            takeOffClimb.StartPoint = _startCntPrj;
            takeOffClimb.LengthOfInnerEdge = lengthOfInnerEdge;
            takeOffClimb.Divergence = divergence;
            takeOffClimb.Slope = slope;
            takeOffClimb.Length = length;
            takeOffClimb.FinalWidth = finalWidth;
            takeOffClimb.SurfaceType = SurfaceType.TakeOffFlihtPathArea;
            return takeOffClimb;
        }

        private MultiPoint GetRwyPoints()
        {
            var rwyPoints = new MultiPoint();
            foreach (var rwyClass in _selectedRwyClassList)
            {
                rwyPoints.Add(_spatialRefOper.ToPrj(rwyClass.SelectedRwyDirection.StartCntlPt.Location.Geo));
                rwyPoints.Add(_spatialRefOper.ToPrj(rwyClass.SelectedRwyDirection.EndCntlPt.Location.Geo));
            }

            RunwayConstants radiusConstant = _runwayConstantList[SurfaceType.OuterHorizontal, DimensionType.Radius];
            _outerHorRadius = radiusConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            if (_selectedRwyClassList.Count > 1)
            {
                var runwayGeo = ChainHullAlgorithm.ConvexHull(rwyPoints);
                rwyPoints = runwayGeo.ToMultiPoint();
            }
            return rwyPoints;
        }

        #endregion
    }
}
