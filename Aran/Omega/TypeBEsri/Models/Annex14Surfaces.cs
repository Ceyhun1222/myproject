using System;
using System.Collections.Generic;
using System.Diagnostics;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Omega.Models;
using Aran.Panda.Common;
using Aran.Panda.Constants;
using Point = Aran.Geometries.Point;

namespace Aran.Omega.TypeB.Models
{
    public class Annex14Surfaces
    {
        private const double CodeLetterFInnerEdge=155;

        #region -->Fields

        //private List<RunwayCentrelinePoint> _rwyCntlnPtList;
        private List<RwyCenterlineClass> _rwyCntlnClassList;
        private RunwayCentrelinePoint _startCntlnPt, _endCntlnPt;
        private Point _startCntPrj, _endCntPrj;
        private SpatialReferenceOperation _spatialRefOper;
        //private Aran.Geometries.Operators.GeometryOperators _geomOperators;
        private Topology.GeometryOperators _geomOperators;
        private double _direction;
        private Aran.AranEnvironment.IAranGraphics _ui;
        private ElevationDatum _elevDatum;
        private CategoryNumber _catNumber;
        private int _codeNumber;
        private Aran.Panda.Constants.RunwayConstansList _runwayConstantList;
        private double _refH;
        private RunwayClassificationType _rwyClassifationType;
        private Strip _strip;
        private double _innerHorElevation;
        private RwyDirClass _rwyDir;
        private double _axis;
        private double _clearWay;
        private int InTransPlaneCount;
        private Aran.Geometries.Point _ptRefBalked;
        private IList<RwyView> _selectedRwyViewList;
        private double _outerHorRadius;

        #endregion

        #region -->Ctor
        public Annex14Surfaces(IList<RwyView> rwyViewList, RwyDirClass rwyDir, ElevationDatum elevDatum,
            RunwayClassificationType rwyClassifactionType, CategoryNumber catNumber, int codeNumber)
        {
            try
            {
                _selectedRwyViewList = rwyViewList;
                _rwyDir = rwyDir;
                _elevDatum = elevDatum;
                _refH = elevDatum.Height;
                _rwyClassifationType = rwyClassifactionType;
                _catNumber = catNumber;
                _codeNumber = codeNumber;

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
                StartCntPtPrj = _startCntPrj;
                EndCntPtrj = _endCntPrj;
                ClearWay = _clearWay;

                ProfileLinePoint = new List<Point>();

                _direction = ARANFunctions.ReturnAngleInRadians(_startCntPrj, _endCntPrj);
                _axis = _direction + Math.PI;
                _ui = GlobalParams.UI;

                RwyPoints = GetRwyPoints();
                CreateProfileLinePoint();
                CreateInnerHorizontal();
                //   AnalysesAirportSurfaces();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        #endregion

        #region -->Property
        private InnerHorizontal _innerHorizontal;
        public InnerHorizontal InnerHorizontalSurface
        {
            get
            {
                if (_innerHorElevation == null)
                    CreateInnerHorizontal();
                return _innerHorizontal;
            }
            set 
            {
                _innerHorizontal = value;
            }
        }
        public ConicalSurface ConicalSurface { get; private set; }
        public Strip StripSurface { get; private set; }
        public Approach ApproachSurface { get; set; }
        public InnerApproach InnerApproach { get; private set; }
        public BalkedLanding BalkedLandingSurface { get; private set; }
        public MultiPoint RwyPoints { get; set; }
        public TakeOffClimb TakeOffSurface { get; set; }

        public Aran.Geometries.Point StartCntPtPrj { get; set; }
        public Aran.Geometries.Point EndCntPtrj { get; set; }
        public double ClearWay { get; set; }

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
            foreach (var rwyClass in _selectedRwyViewList)
            {
                //Calculate Arc with start point
                Point starPtPrj = _spatialRefOper.ToPrj(rwyClass.RWYDirection.StartCntlPt.Location.Geo);
                Point endPrj = _spatialRefOper.ToPrj(rwyClass.RWYDirection.EndCntlPt.Location.Geo);
                starPtPrj.Z = endPrj.Z = _innerHorElevation;
                innerGeo.Add(starPtPrj);
                innerGeo.Add(endPrj);
            }

            Aran.Geometries.Polygon poly = new Polygon();
            Aran.Geometries.MultiPolygon resultGeo = null;
            if (_selectedRwyViewList.Count > 2)
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
            {
                pt.Z = _innerHorElevation;
            }

            InnerHorizontalSurface = new InnerHorizontal();
            InnerHorizontalSurface.Geo = resultGeo;
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

                var innerHorizontalGeo = InnerHorizontalSurface.Geo[0];

                var buffer = _geomOperators.Buffer(innerHorizontalGeo, r_conic);
                var resultGeo = (MultiPolygon)_geomOperators.Difference(buffer, innerHorizontalGeo);

                double elevConic = InnerHorizontalSurface.Elevation + heightConst;
                foreach (Aran.Geometries.Polygon polygon in resultGeo)
                {
                    foreach (Aran.Geometries.Ring ring in polygon.InteriorRingList)
                        foreach (Aran.Geometries.Point pt in ring)
                            pt.Z = _innerHorElevation;

                    foreach (Aran.Geometries.Point pt in polygon.ExteriorRing)
                        pt.Z = elevConic;
                }

                ConicalSurface = new ConicalSurface
                {
                    Geo = resultGeo,
                    StartPoint = _startCntPrj,
                    Height = heightConst,
                    ElevInnerHor = _innerHorElevation,
                    Elevation = elevConic,
                    Slope = slope,
                    EndCntlnPt = _endCntPrj,
                    Radius = r_conic,
                    Direction = _direction,
                    InnerHorRadius = InnerHorizontalSurface.Radius,
                    InnerHorGeo = InnerHorizontalSurface.Geo
                };

                return ConicalSurface;
            }
            catch (Exception)
            {
                throw new Exception("Error when creating Conical plane");
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
            conicExteriorPolygon.ExteriorRing = ConicalSurface.Geo[0].ExteriorRing;

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

            var outerHorizontal = new OuterHorizontal();
            outerHorizontal.Geo = resultGeo;
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

            double maxDistanceFromThreshold = _rwyDir.StopWay;
            if (distanceFromThreshold > maxDistanceFromThreshold)
                maxDistanceFromThreshold = distanceFromThreshold;

            Point startOffSet = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -distanceFromThreshold, 0);
            startOffSet.Z = _startCntPrj.Z;
            ProfileLinePoint.Add(startOffSet);

            for (int i = 0; i < _rwyCntlnClassList.Count; i++)
            {
                Point tmpPtPrj = _rwyCntlnClassList[i].PtPrj;
                tmpPtPrj.Z = _rwyCntlnClassList[i].Elevation;
                ProfileLinePoint.Add(tmpPtPrj);
            }

            Point endOffSet = ARANFunctions.LocalToPrj(_endCntPrj, _direction, maxDistanceFromThreshold, 0);
            endOffSet.Z = _endCntPrj.Z;
            ProfileLinePoint.Add(endOffSet);
        }

        public Strip CreateStrip()
        {
            _strip = new Strip();
            var leftMltPt = new MultiPoint();
            var rightMltPt = new MultiPoint();

            RunwayConstants distanceFromThersholdConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.DistanceFromThreshold];
            double distanceFromThreshold = distanceFromThersholdConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            RunwayConstants lengthOfInnerEdgeConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.LengthOfInnerEdge];
            double lengthOfInnerEdge = lengthOfInnerEdgeConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            var stripElem = new Ring();

            var startPrj = _spatialRefOper.ToPrj(_startCntlnPt.Location.Geo);

            var mainRing = new Ring();

            var rightBackPt = ARANFunctions.LocalToPrj(ProfileLinePoint[0], _direction, 0, -lengthOfInnerEdge / 2);
            var leftBackPt = ARANFunctions.LocalToPrj(ProfileLinePoint[0], _direction, 0, lengthOfInnerEdge / 2);

            leftBackPt.Z = rightBackPt.Z = ProfileLinePoint[0].Z;

            stripElem.Add(rightBackPt);
            stripElem.Add(leftBackPt);

            leftMltPt.Add(leftBackPt);
            rightMltPt.Add(rightBackPt);

            Aran.Geometries.Point profilinePt;
            for (int i = 1; i < ProfileLinePoint.Count; i++)
            {
                profilinePt = ProfileLinePoint[i];

                Point leftPt = ARANFunctions.LocalToPrj(profilinePt, _direction, 0, lengthOfInnerEdge / 2);
                Point rightPt = ARANFunctions.LocalToPrj(profilinePt, _direction, 0, -lengthOfInnerEdge / 2);
                leftPt.Z = rightPt.Z = profilinePt.Z;

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
                var poly = new Polygon { ExteriorRing = plane.Geo };
                resultGeo.Add(poly);
            }

            _strip.Geo = resultGeo;
            _strip.Direction = _direction;
            _strip.StartPoint = _startCntPrj;
            _strip.LeftPts = leftMltPt;
            _strip.RightPts = rightMltPt;
            _strip.GeoForObstacleCalculating = _strip.Geo;
            _strip.LengthOfInnerEdge = lengthOfInnerEdge;
            return _strip;
        }

        public Approach CreateApproach()
        {
            try
            {
                Aran.Geometries.Point planePt1, planePt2, planePt3;
                double secondSectionSlopeConst = 0;
                double horizontalSecLenConst = 0;
                double secondSectionLenConst = 0, firstSectionSlope;

                Approach approach = new Approach();
                Aran.Geometries.Ring geoForReport = new Ring();
                Aran.Geometries.Ring firstSectoinPlane = new Ring();

                //first section of Approcah
                RunwayConstants tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.LengthOfInnerEdge];
                double lengthOfInnerEdgeConst = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

                tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.Divergence];
                double divergenceConst = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

                tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.DistanceFromThreshold];
                double distanceFromThresholdConst = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

                Aran.Geometries.Point firstSecPt1 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -distanceFromThresholdConst, lengthOfInnerEdgeConst / 2);
                Aran.Geometries.Point firstSecPt2 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -distanceFromThresholdConst, -lengthOfInnerEdgeConst / 2);
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


                double widthSec1 = lengthOfInnerEdgeConst / 2 + firstSectionLenConst * divergenceConst / 100;
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

                planePt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, firstSecPt2);
                planePt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, firstSecPt3);
                planePt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, firstSecPt4);

                approach.Section1.Param = CommonFunctions.CalcPlaneParam(planePt1, planePt2, planePt3);

                //second section of Approach

                bool secondPlane = true;
                if (_rwyClassifationType == RunwayClassificationType.NonInstrument || (_rwyClassifationType == RunwayClassificationType.NonPrecisionApproach && _codeNumber == 1 || _codeNumber == 2))
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

                    secondSectionPlane.Add(firstSectoinPlane[3]);
                    secondSectionPlane.Add(firstSectoinPlane[2]);

                    double lengthSec2 = lengthFromStartToSec1 + secondSectionLenConst;
                    double widthSec2 = lengthOfInnerEdgeConst / 2 + (firstSectionLenConst + secondSectionLenConst) * divergenceConst / 100;
                    Aran.Geometries.Point secondSecPt3 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -lengthSec2, -widthSec2);
                    Aran.Geometries.Point secondSecPt4 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -lengthSec2, widthSec2);

                    tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.SecondSectionSlope];
                    secondSectionSlopeConst = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

                    secondSecPt3.Z = secondSecPt4.Z = (secondSectionLenConst * secondSectionSlopeConst) / 100 + secondSectionPlane[0].Z;
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

                    horizontalSectionPlane.Add(secondSectionPlane[3]);
                    horizontalSectionPlane.Add(secondSectionPlane[2]);

                    double lengthHorizontalSec = lengthSec2 + horizontalSecLenConst;
                    double widthHorizontalSec = lengthOfInnerEdgeConst / 2 + (firstSectionLenConst + secondSectionLenConst + horizontalSecLenConst) * divergenceConst / 100;

                    Aran.Geometries.Point horizontalSecPt3 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -lengthHorizontalSec, -widthHorizontalSec);
                    Aran.Geometries.Point horizontalSecPt4 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -lengthHorizontalSec, widthHorizontalSec);
                    horizontalSecPt3.Z = horizontalSecPt4.Z = secondSectionPlane[2].Z;

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

                approach.Geo = resultGeo;
                approach.Direction = _direction;
                approach.StartPoint = _startCntPrj;
                approach.SecondPlane = secondPlane;
                approach.LengthOfInnerEdge = lengthOfInnerEdgeConst;
                approach.DistanceFromThreshold = distanceFromThresholdConst;
                approach.FirstSectionSlope = firstSectionSlope;
                approach.FirstSectionLength = firstSectionLenConst;
                approach.Divergence = divergenceConst;
                approach.SecondSectionSlope = secondSectionSlopeConst;
                approach.SecondSectionLength = secondSectionLenConst;
                approach.HorizontalSectionLength = horizontalSecLenConst;
                approach.SecondSectionSlope = secondSectionSlopeConst;
                ApproachSurface = approach;

                return approach;
            }
            catch (Exception)
            {
                throw new Exception("Error when creating Approach surface");
            }

        }

        public Transitional CreateTransitionalSurface()
        {
            var transitional = new Transitional();

            CreateStrip();
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

            tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.LengthOfInnerEdge];
            double appLenInnerEdge = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            //End getting constants

            var planes = new List<Plane>();
            //Create Left transitional strips
            var leftPts = _strip.LeftPts;
            var rightPts = _strip.RightPts;

            double withInnerEdgeMax = 0;

            for (int i = 0; i < leftPts.Count - 1; i++)
            {
                
                double withInnerEdgeCur = (_innerHorElevation - leftPts[i].Z) * 100 / transSlope;
                double withInnerEdgeNext = (_innerHorElevation - leftPts[i + 1].Z) * 100 / transSlope;

                //Left Plane Calculation
                var ptCurLeft = ARANFunctions.LocalToPrj(leftPts[i], _direction, 0, withInnerEdgeCur);
                var ptNextLeft = ARANFunctions.LocalToPrj(leftPts[i + 1], _direction, 0, withInnerEdgeNext);

                ptCurLeft.Z = ptNextLeft.Z = _innerHorElevation;

                var ptCurLeftLocal = ARANFunctions.PrjToLocal(_startCntPrj, _axis, ptCurLeft);
                ptCurLeftLocal.Z = ptCurLeft.Z;
                var ptNextCurLocal = ARANFunctions.PrjToLocal(_startCntPrj, _axis, ptNextLeft);
                ptNextCurLocal.Z = ptNextLeft.Z;
                var leftPtLocal = ARANFunctions.PrjToLocal(_startCntPrj, _axis, leftPts[i]);
                leftPtLocal.Z = leftPts[i].Z;

                var tmpPlaneLeft = new Plane { Geo = new Ring { leftPts[i], ptCurLeft, ptNextLeft, leftPts[i + 1] }, Param = CommonFunctions.CalcPlaneParam(leftPtLocal, ptCurLeftLocal, ptNextCurLocal) };

                planes.Add(tmpPlaneLeft);

                //GlobalParams.UI.DrawRing(tmpPlaneLeft.Geo, 100, AranEnvironment.Symbols.eFillStyle.sfsSolid);

                //End left plane calculation

                //Right plane calculation
                var ptCurRight = ARANFunctions.LocalToPrj(rightPts[i], _direction, 0, -withInnerEdgeCur);
                var ptNextRight = ARANFunctions.LocalToPrj(rightPts[i + 1], _direction, 0, -withInnerEdgeNext);

                ptCurRight.Z = ptNextRight.Z = _innerHorElevation;

                var ptCurRightLocal = ARANFunctions.PrjToLocal(_startCntPrj, _axis, ptCurRight);
                ptCurLeftLocal.Z = ptCurLeft.Z;
                var ptNextLocalRight = ARANFunctions.PrjToLocal(_startCntPrj, _axis, ptNextRight);
                ptNextCurLocal.Z = ptNextLeft.Z;
                var rightPtLocal = ARANFunctions.PrjToLocal(_startCntPrj, _axis, rightPts[i]);
                rightPtLocal.Z = rightPts[i].Z;

                var tmpPlaneRight = new Plane
                        {
                            Geo = new Ring { rightPts[i], ptCurRight, ptNextRight, rightPts[i + 1] },
                            Param = CommonFunctions.CalcPlaneParam(rightPtLocal, ptCurRightLocal, ptNextLocalRight)
                        };

                planes.Add(tmpPlaneRight);
                //end plane calculation

                //Get the max innerEdge in the right section of runway direction
                if (withInnerEdgeCur > withInnerEdgeMax)
                    withInnerEdgeMax = withInnerEdgeCur;
            }

            var app45LineLeft = new Ring();
            double appLength = distanceFromThreshold + (InnerHorizontalSurface.Elevation-_startCntPrj.Z) * 100 / appSlope;
            double appWidth = (appLength - distanceFromThreshold) * appDiver / 100 + appLenInnerEdge / 2;

            Aran.Geometries.Point leftAppPt = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -appLength, appWidth);
            Aran.Geometries.Point rightAppPt = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -appLength, -appWidth);
            leftAppPt.Z = rightAppPt.Z = _innerHorElevation;

            var leftPlane = new Plane();
            app45LineLeft.Add(leftAppPt);
            app45LineLeft.Add(leftPts[0]);
            app45LineLeft.Add(planes[0].Geo[1]);
            

            var localApp1Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, leftAppPt);
            var localApp2Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, leftPts[0]);
            var localApp3Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, planes[0].Geo[1]);

            var leftAppPlaneParam = CommonFunctions.CalcPlaneParam(localApp1Pt, localApp2Pt, localApp3Pt);
            leftPlane.Param = leftAppPlaneParam;
            leftPlane.Geo = app45LineLeft;
            planes.Add(leftPlane);

            var rightPlane = new Plane();
            var app45LineRight = new Ring {rightAppPt, rightPts[0], planes[1].Geo[1]};

            var localAppRight1Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, rightAppPt);
            var localAppRight2Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, rightPts[0]);
            var localAppRight3Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, planes[1].Geo[1]);

            var rightPlaneParam = CommonFunctions.CalcPlaneParam(localAppRight1Pt, localAppRight2Pt, localAppRight3Pt);

            rightPlane.Param = rightPlaneParam;
            rightPlane.Geo = app45LineRight;
            planes.Add(rightPlane);

            //Get the extent transitional surface for calculating obstacle easy
            var extentPt1 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -appLength, withInnerEdgeMax);
            var extentPt2 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -appLength, -withInnerEdgeMax);
            var extentPt3 = ARANFunctions.LocalToPrj(rightPts[rightPts.Count - 1], _direction, 0, -withInnerEdgeMax);
            var extentPt4 = ARANFunctions.LocalToPrj(leftPts[leftPts.Count - 1], _direction, 0, withInnerEdgeMax);

           // transitional.Geo = CommonFunctions.CreateMultipolygonFromPoints(new Aran.Geometries.Point[] { extentPt1, extentPt2, extentPt3, extentPt4, extentPt1 });

            //
            var resultGeo = new MultiPolygon();
            foreach (var plane in planes)
            {
                var poly = new Polygon { ExteriorRing = plane.Geo };
                resultGeo.Add(poly);
            }
            transitional.Geo = resultGeo;
            transitional.Slope = transSlope;
            transitional.Planes = planes;
            transitional.StartPoint = _startCntPrj;
            transitional.Direction = _direction;
            return transitional;
        }

        public TakeOffClimb CreateTakeOffSurface(bool isGreaterThan15)
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

            tmpConstant = _runwayConstantList[SurfaceType.TakeOffClimb, DimensionType.Slope];
            double slope = tmpConstant.GetValue(classification, 0, _codeNumber);

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

            //var planePt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt1);
            //var planePt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt2);
            //var planePt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt3);
            var planePt1 = ARANFunctions.PrjToLocal(_endCntPrj, _direction, pt1);
            var planePt2 = ARANFunctions.PrjToLocal(_endCntPrj, _direction, pt2);
            var planePt3 = ARANFunctions.PrjToLocal(_endCntPrj, _direction, pt3);
            var firstSectionParam = CommonFunctions.CalcPlaneParam(planePt1, planePt2, planePt3);
            //First section plane 
            var planeGeo = new Ring
            {
                pt1,
                pt2,
                pt3,
                ARANFunctions.LocalToPrj(_endCntPrj, _direction, section1Length, finalWidth/2)
            };

            var firstSectionPlane = new Plane { Geo = planeGeo, Param = firstSectionParam };

            planes.Add(firstSectionPlane);

            if (_codeNumber <= 2)
            {
                Aran.Geometries.Point pt4 = ARANFunctions.LocalToPrj(_endCntPrj, _direction, section1Length + distanceFromRunwayEnd, finalWidth / 2);
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

                //var planePt4 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt4);
                //var planePt5 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt5);
                var planePt4 = ARANFunctions.PrjToLocal(_endCntPrj, _direction, pt4);
                var planePt5 = ARANFunctions.PrjToLocal(_endCntPrj, _direction, pt5);
                var secondSectionParam = CommonFunctions.CalcPlaneParam(planePt3, planePt4, planePt5);

                var secondSectionPlane = new Plane { Geo = new Ring { pt3, pt4, pt5, pt6 }, Param = secondSectionParam };
                planes.Add(secondSectionPlane);
            }

            var mlt = new MultiPolygon { new Polygon { ExteriorRing = takeOffGeo } };

            var takeOfClimb = new TakeOffClimb
            {
                Direction = _direction,
                Geo = mlt,
                Planes = planes,
                StartPoint = _startCntPrj,
                LengthOfInnerEdge = lengthOfInnerEdge,
                DistanceFromThreshold = distanceFromRunwayEnd,
                Divergence = divergence,
                FinalWidth = finalWidth,
                Slope = slope,
                Length = length,
                EndPoint=_endCntPrj
            };
            TakeOffSurface = takeOfClimb; 
            return takeOfClimb;
        }

        public void AnalysesAirportSurfaces()
        {
            var rwyPoints = new LineString();
            foreach (var rwyClass in _selectedRwyViewList)
            {
                rwyPoints.Add(_spatialRefOper.ToPrj(rwyClass.RWYDirection.StartCntlPt.Location.Geo));
                rwyPoints.Add(_spatialRefOper.ToPrj(rwyClass.RWYDirection.EndCntlPt.Location.Geo));
            }

            RunwayConstants radiusConstant = _runwayConstantList[SurfaceType.OuterHorizontal, DimensionType.Radius];
            _outerHorRadius = radiusConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            if (_selectedRwyViewList.Count > 1)
            {
                var runwayGeo = ChainHullAlgorithm.ConvexHull(rwyPoints);
                //rwyPoints = runwayGeo.ToMultiPoint();
                //rwyPoints = runwayGeo.ExteriorRing;
            }
            List<VerticalStructure> vsList = GlobalParams.Database.GetVerticalStructureList(
                GlobalParams.Database.AirportHeliport.ARP.Geo, _outerHorRadius);

            _geomOperators.CurrentGeometry = rwyPoints;
            foreach (var vs in vsList)
            {
                try
                {
                    int partNumber = -1;
                    foreach (var vsPart in vs.Part)
                    {
                        partNumber++;
                        if (vsPart.HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
                        {
                            var pt = GlobalParams.SpatialRefOperation.ToPrj(vsPart.HorizontalProjection.Location.Geo);
                           // var esriGeom = ConvertToEsriGeom.FromGeometry(pt);
                            //var distance = _geomOperators.GetDistance(esriGeom);
                           
                            //var distance = _geomOperators.GetDistance(pt);
                            //vs.SetDistance(partNumber, distance); //CommonFunctions.GetDistance(pt, rwyPoints));
                            vs.SetElevation(partNumber,
                                ConverterToSI.Convert(vsPart.HorizontalProjection.Location.Elevation, 0));
                            vs.SetGeom(partNumber, pt);
                         //   vs.SetEsriGeom(partNumber, esriGeom);
                        }
                        else if (vsPart.HorizontalProjection.Choice ==
                                 VerticalStructurePartGeometryChoice.ElevatedSurface)
                        {
                            var surface =
                                GlobalParams.SpatialRefOperation.ToPrj(vsPart.HorizontalProjection.SurfaceExtent.Geo);
                          
                          //  var esriGeom = ConvertToEsriGeom.FromGeometryIntersect(surface);
                           // var distance = _geomOperators.GetDistance(esriGeom);
                            
                            //var distance = _geomOperators.GetDistance(surface);
                            //vs.SetDistance(partNumber, distance);
                            vs.SetElevation(partNumber,
                                ConverterToSI.Convert(vsPart.HorizontalProjection.SurfaceExtent.Elevation, 0));
                            vs.SetGeom(partNumber, surface);
                         //   vs.SetEsriGeom(partNumber, esriGeom);

                        }
                        else if (vsPart.HorizontalProjection.Choice ==
                                 VerticalStructurePartGeometryChoice.ElevatedCurve)
                        {
                            var curve =
                                GlobalParams.SpatialRefOperation.ToPrj(
                                    vsPart.HorizontalProjection.LinearExtent.Geo);

                            //var esriGeom = ConvertToEsriGeom.FromGeometryIntersect(curve);
                            //var distance = _geomOperators.GetDistance(esriGeom);
                            
                            //var distance = _geomOperators.GetDistance(curve);
                            //vs.SetDistance(partNumber, distance);
                            vs.SetElevation(partNumber,
                                ConverterToSI.Convert(vsPart.HorizontalProjection.LinearExtent.Elevation, 0));
                            vs.SetGeom(partNumber, curve);
                            //vs.SetEsriGeom(partNumber, esriGeom);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;    
                }
            }
            GlobalParams.AdhpObstacleList = vsList;
        }

        public List<Aran.Geometries.Point> ProfileLinePoint { get; private set; }

        private MultiPoint GetRwyPoints()
        {
            var rwyPoints = new MultiPoint();
            foreach (var rwyClass in _selectedRwyViewList)
            {
                rwyPoints.Add(_spatialRefOper.ToPrj(rwyClass.RWYDirection.StartCntlPt.Location.Geo));
                rwyPoints.Add(_spatialRefOper.ToPrj(rwyClass.RWYDirection.EndCntlPt.Location.Geo));
            }

            RunwayConstants radiusConstant = _runwayConstantList[SurfaceType.OuterHorizontal, DimensionType.Radius];
            _outerHorRadius = radiusConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            if (_selectedRwyViewList.Count > 1)
            {
                var runwayGeo = ChainHullAlgorithm.ConvexHull(rwyPoints);
                rwyPoints = runwayGeo.ToMultiPoint();
            }
            return rwyPoints;
        }

        #endregion
    }
}
