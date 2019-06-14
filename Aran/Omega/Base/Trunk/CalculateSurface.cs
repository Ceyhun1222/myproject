using System;
using System.Collections.Generic;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Panda.Common;
using Aran.Panda.Constants;
using Omega.Models;
using System.Collections.ObjectModel;

namespace Omega
{
    public class CalculateSurface
    {
        #region -->Fields

        //private List<RunwayCentrelinePoint> _rwyCntlnPtList;
        private List<RwyCenterlineClass> _rwyCntlnClassList;
        private RunwayCentrelinePoint _startCntlnPt, _endCntlnPt;
        private Point _startCntPrj, _endCntPrj;
        private SpatialReferenceOperation _spatialRefOper;
        private GeometryOperators _geomOperators;
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
        private IList<RwyClass> _selectedRwyClassList;


        #endregion

        #region -->Ctor
        public CalculateSurface(IList<RwyClass> rwyClassList,RwyDirClass rwyDir,ElevationDatum elevDatum,
            RunwayClassificationType rwyClassifactionType,CategoryNumber catNumber,int codeNumber)
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

                _spatialRefOper = GlobalParams.SpatialRefOperation;
                _geomOperators = GlobalParams.GeomOperators;

                _runwayConstantList = GlobalParams.Constant_G.RunwayConstants;
                //_rwyCntlnPtList = rwyDir.RwyCntrPtList;
                _rwyCntlnClassList = rwyDir.CenterLineClassList;
                _startCntlnPt = rwyDir.StartCntlPt;
                _endCntlnPt = rwyDir.EndCntlPt;
                _startCntPrj = _spatialRefOper.GeoToPrj(_startCntlnPt.Location.Geo);
                _startCntPrj.Z = ConverterToSI.Convert(_startCntlnPt.Location.Elevation, 0);
                _endCntPrj = _spatialRefOper.GeoToPrj(_endCntlnPt.Location.Geo);
                _endCntPrj.Z = ConverterToSI.Convert(_endCntlnPt.Location.Elevation, 0);
                _clearWay = _rwyDir.ClearWay;

                ProfileLinePoint = new List<Point>();

                _direction = ARANFunctions.ReturnAngleInRadians(_startCntPrj, _endCntPrj);
                _axis = _direction + Math.PI;
                _ui = GlobalParams.UI;
                CreateProfileLinePoint();
            }
            catch (Exception e)
            {
                
                throw e;
            }
            

        }

        private void CreateProfileLinePoint()
        {
            RunwayConstants tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.DistanceFromThreshold];
            double distanceFromThreshold = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            Point startOffSet = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -distanceFromThreshold, 0);
            startOffSet.Z = _startCntPrj.Z;
            ProfileLinePoint.Add(startOffSet);

            for (int i = 0; i < _rwyCntlnClassList.Count; i++)
            {
                Point tmpPtPrj = _rwyCntlnClassList[i].PtPrj;
                tmpPtPrj.Z = _rwyCntlnClassList[i].Elevation;
                ProfileLinePoint.Add(tmpPtPrj);
            }

            Point endOffSet = ARANFunctions.LocalToPrj(_endCntPrj, _direction, distanceFromThreshold, 0);
            endOffSet.Z = _endCntPrj.Z;
            ProfileLinePoint.Add(endOffSet);
        }
        #endregion

        #region -->Property
        public InnerHorizontal InnerHorizontalSurface { get; private set; }
        public ConicalSurface ConicalSurface { get; private set; }
        public Strip StripSurface { get; private set; }
        public InnerApproach InnerApproach { get; private set; }
        public BalkedLanding BalkedLandingSurface { get; private set; }
        
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
                Point starPtPrj = _spatialRefOper.GeoToPrj(rwyClass.RwyDirClassList[0].StartCntlPt.Location.Geo);
                Point endPrj = _spatialRefOper.GeoToPrj(rwyClass.RwyDirClassList[0].EndCntlPt.Location.Geo);
                starPtPrj.Z =endPrj.Z= _innerHorElevation;
                innerGeo.Add(starPtPrj);
                innerGeo.Add(endPrj);
            }
            
            Aran.Geometries.Polygon poly = new Polygon();
            Aran.Geometries.MultiPolygon resultGeo = null;
            if (_selectedRwyClassList.Count > 1)
            {
                
                poly.ExteriorRing = innerGeo;
                resultGeo = GlobalParams.GeomOperators.ConvexHull(poly) as MultiPolygon;
                resultGeo = GlobalParams.GeomOperators.Buffer(resultGeo,radius) as MultiPolygon;
            }
            else
            {
                LineString ln = ARANFunctions.ConvertPointsToTrackLIne(innerGeo);
                resultGeo = GlobalParams.GeomOperators.Buffer(ln, radius) as MultiPolygon;
            }
            //End

            InnerHorizontalSurface = new InnerHorizontal();
            InnerHorizontalSurface.Geo = resultGeo;
            InnerHorizontalSurface.Elevation = _innerHorElevation;
            InnerHorizontalSurface.Radius = radius;

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

                Aran.Geometries.Polygon innerHorizontalGeo = InnerHorizontalSurface.Geo[0];

                Geometry buffer = _geomOperators.Buffer(innerHorizontalGeo, r_conic);
                MultiPolygon resultGeo = (MultiPolygon)_geomOperators.Difference(buffer, innerHorizontalGeo);

                double heightConic = InnerHorizontalSurface.Elevation + heightConst;
                foreach (Aran.Geometries.Polygon polygon in resultGeo)
                {
                    foreach (Aran.Geometries.Ring ring in polygon.InteriorRingList)
                        foreach (Aran.Geometries.Point pt in ring)
                            pt.Z = InnerHorizontalSurface.Elevation;

                    foreach (Aran.Geometries.Point pt in polygon.ExteriorRing)
                        pt.Z = heightConic;
                }

                ConicalSurface = new ConicalSurface();
                ConicalSurface.Geo = resultGeo;
                ConicalSurface.Height = heightConic;
                ConicalSurface.Slope = slope;

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
            double radius = radiusConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            RunwayConstants heightConstant = _runwayConstantList[SurfaceType.CONICAL, DimensionType.Height];
            double height_conic = heightConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            AirportHeliport adhp = GlobalParams.Database.AirportHeliport;

            Point adhpPrj = _spatialRefOper.GeoToPrj(adhp.ARP.Geo);
            Ring outerFull = ARANFunctions.CreateCirclePrj(adhpPrj, radius);
            Aran.Geometries.Polygon tmp = new Polygon();
            tmp.InteriorRingList.Add(outerFull);

            Aran.Geometries.Polygon conicExteriorPolygon = new Polygon();
            conicExteriorPolygon.ExteriorRing = ConicalSurface.Geo[0].ExteriorRing;

            MultiPolygon resultGeo = (MultiPolygon)_geomOperators.Difference(tmp, conicExteriorPolygon);

            double heightOuterHorizontal = ConicalSurface.Height;

            foreach (Aran.Geometries.Polygon poly in resultGeo)
            {
                foreach (Aran.Geometries.Ring interiorRing in poly.InteriorRingList)
                {
                    foreach (Aran.Geometries.Point interiorPt in interiorRing)
                    {
                        interiorPt.Z = heightOuterHorizontal;
                    }
                }
                foreach (Aran.Geometries.Point exterirorPt in poly.ExteriorRing)
                {
                    exterirorPt.Z = heightOuterHorizontal + height_conic;
                }
            }

            OuterHorizontal outerHorizontal = new OuterHorizontal();
            outerHorizontal.Geo = resultGeo;
            outerHorizontal.Radius = radius;
            outerHorizontal.Elevation = heightOuterHorizontal;
            outerHorizontal.SurfaceType = SurfaceType.OuterHorizontal;
            return outerHorizontal;
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
                    approach.Section2.Param = CommonFunctions.CalcPlaneParam(planePt1,planePt2,planePt3);

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
            catch (Exception e)
            {
                throw new Exception("Error when creating Approach surface");
            }
            
        }

        public Strip CreateStrip()
        {
            _strip = new Strip();
            MultiPoint leftMltPt = new MultiPoint();
            MultiPoint rightMltPt = new MultiPoint();

            RunwayConstants distanceFromThersholdConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.DistanceFromThreshold];
            double distanceFromThreshold = distanceFromThersholdConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            RunwayConstants lengthOfInnerEdgeConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.LengthOfInnerEdge];
            double lengthOfInnerEdge = lengthOfInnerEdgeConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            Ring stripElem = new Ring();

            Aran.Geometries.Point startPrj = _spatialRefOper.GeoToPrj(_startCntlnPt.Location.Geo);

            Aran.Geometries.Ring mainRing = new Ring();


            Point rightBackPt = ARANFunctions.LocalToPrj(ProfileLinePoint[0], _direction, 0, -lengthOfInnerEdge / 2);
            Point leftBackPt = ARANFunctions.LocalToPrj(ProfileLinePoint[0], _direction, 0, lengthOfInnerEdge / 2);

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
                
                _strip.StripList.Add(tmpPlane);

                stripElem = new Ring();
                stripElem.Add(rightPt);
                stripElem.Add(leftPt);
            }

            mainRing.Add(leftMltPt[0]);
            mainRing.Add(rightMltPt[0]);
            mainRing.Add(rightMltPt[rightMltPt.Count - 1]);
            mainRing.Add(leftMltPt[leftMltPt.Count - 1]);

            Aran.Geometries.Polygon poly = new Polygon();
            poly.ExteriorRing = mainRing;
            MultiPolygon resultGeoForObstacle = new MultiPolygon();
            resultGeoForObstacle.Add(poly);

            _strip.Geo = resultGeoForObstacle;
            _strip.Direction = _direction;
            _strip.StartPoint = _startCntPrj;
            _strip.LeftPts = leftMltPt;
            _strip.RightPts = rightMltPt;
            _strip.GeoForObstacleCalculating = resultGeoForObstacle;
            return _strip;
        }
      
        public InnerApproach CreateInnerApproach()
        {
            InnerApproach = new InnerApproach();
            RunwayConstants tmpConstant = _runwayConstantList[SurfaceType.InnerApproach, DimensionType.DistanceFromThreshold];
            double distanceFromThreshold = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.InnerApproach, DimensionType.Length];
            double length = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.InnerApproach, DimensionType.Width];
            double width = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.InnerApproach, DimensionType.Slope];
            double slope = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            double H_OFZ = 45;

            Aran.Geometries.Point pt1 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -distanceFromThreshold, width / 2);
            Aran.Geometries.Point pt2 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -distanceFromThreshold, -width / 2);
            
            double dist = distanceFromThreshold + length + (H_OFZ - 45) / (slope / 100);

            pt1.Z = pt2.Z = _startCntPrj.Z;

            Aran.Geometries.Point pt3 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -dist, -width / 2);
            Aran.Geometries.Point pt4 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -dist, width / 2);

            pt3.Z = pt4.Z = _startCntPrj.Z + ((dist-distanceFromThreshold) * (slope / 100)) + (H_OFZ - 45);
            
            Aran.Geometries.MultiPolygon geo = CommonFunctions.CreateMultipolygonFromPoints(new Aran.Geometries.Point[] {pt1,pt2,pt3,pt4});

            //It is for Calculating  plane equation.Get the thres point of inner approach.
            //Then calculate param.And it will be used Inner Approach class when calculating obstruction

            Aran.Geometries.Point planePt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt1);
            Aran.Geometries.Point planePt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt2);
            
            Aran.Geometries.Point planePt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt3);
            //End calculating
            InnerApproach.PlaneParam = CommonFunctions.CalcPlaneParam(planePt1, planePt2, planePt3);
            InnerApproach.Geo = geo;
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
            Transitional transitional = new Transitional();

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

            List<Plane> planes = new List<Plane>();
            //Create Left transitional strips
            MultiPoint leftPts = _strip.LeftPts;

            double withInnerEdgeMax = 0;

            double withInnerEdge = 0;
            for (int i = 0; i < leftPts.Count - 1; i++)
            {

                Plane tmpPlane = new Plane();
                Aran.Geometries.Ring tmpRing = new Ring();

                withInnerEdge = (_innerHorElevation - leftPts[i].Z) * 100 / transSlope;
                Aran.Geometries.Point pt1 = ARANFunctions.LocalToPrj(leftPts[i], _direction, 0, withInnerEdge);

                Aran.Geometries.Point pt2 = ARANFunctions.LocalToPrj(leftPts[i + 1], _direction, 0, (_innerHorElevation - leftPts[i + 1].Z) * 100 / transSlope);

                pt1.Z = pt2.Z = _innerHorElevation;

                Aran.Geometries.Point pt1Local = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt1);
                pt1Local.Z = pt1.Z;
                Aran.Geometries.Point pt2Local = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt2);
                pt2Local.Z = pt2.Z;
                Aran.Geometries.Point leftPtLocal = ARANFunctions.PrjToLocal(_startCntPrj, _axis, leftPts[i]);
                leftPtLocal.Z = leftPts[i].Z;

                PlaneParam planeParam = CommonFunctions.CalcPlaneParam(leftPtLocal, pt1Local, pt2Local);

                double result = pt2Local.X * planeParam.A + pt2Local.Y * planeParam.B + pt2Local.Z * planeParam.C + planeParam.D;

                tmpRing.Add(leftPts[i]);
                tmpRing.Add(pt1);
                tmpRing.Add(pt2);
                tmpRing.Add(leftPts[i + 1]);
                tmpPlane.Geo = tmpRing;

                tmpPlane.Param = planeParam;
                planes.Add(tmpPlane);

                //Get the max innerEdge in the right section of runway direction
                if (withInnerEdge > withInnerEdgeMax)
                    withInnerEdgeMax = withInnerEdge;
            }

            MultiPoint rightPts = _strip.RightPts;
            for (int i = 0; i < rightPts.Count - 1; i++)
            {
                Plane tmpPlane = new Plane();
                Aran.Geometries.Ring tmpRing = new Ring();
                Aran.Geometries.Point pt1 = ARANFunctions.LocalToPrj(rightPts[i], _direction, 0, -(_innerHorElevation - rightPts[i].Z) * 100 / transSlope);

                Aran.Geometries.Point pt2 = ARANFunctions.LocalToPrj(rightPts[i + 1], _direction, 0, -
                                                                                  (_innerHorElevation - rightPts[i + 1].Z) * 100 / transSlope);
                pt1.Z = pt2.Z = _innerHorElevation;

                Aran.Geometries.Point pt1Local = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt1);
                pt1Local.Z = pt1.Z;
                
                Aran.Geometries.Point pt2Local = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt2);
                pt2Local.Z = pt2.Z;
                
                Aran.Geometries.Point rightPtLocal = ARANFunctions.PrjToLocal(_startCntPrj, _axis, rightPts[i]);
                rightPtLocal.Z = rightPts[i].Z;
                
                PlaneParam planeParam = CommonFunctions.CalcPlaneParam(rightPtLocal, pt1Local, pt2Local);

                tmpRing.Add(rightPts[i]);
                tmpRing.Add(pt1);
                tmpRing.Add(pt2);
                tmpRing.Add(rightPts[i + 1]);
                tmpPlane.Geo = tmpRing;
                tmpPlane.Param = planeParam;
                planes.Add(tmpPlane);
            }

            Ring app45LineLeft = new Ring();
            double appLength = distanceFromThreshold + (innerHorHeight) * 100 / appSlope;
            double appWidth = (appLength - distanceFromThreshold) * appDiver / 100 + appLenInnerEdge / 2;

            Aran.Geometries.Point leftAppPt = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -appLength, appWidth);
            Aran.Geometries.Point rightAppPt = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -appLength, -appWidth);
            leftAppPt.Z = rightAppPt.Z = _innerHorElevation;

            Plane leftPlane = new Plane();
            app45LineLeft.Add(leftAppPt);
            app45LineLeft.Add(leftPts[0]);
            app45LineLeft.Add(planes[0].Geo[1]);

            Aran.Geometries.Point localApp1Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, leftAppPt);
            Aran.Geometries.Point localApp2Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, leftPts[0]);
            Aran.Geometries.Point localApp3Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, planes[0].Geo[1]);

            PlaneParam leftPlaneParam = CommonFunctions.CalcPlaneParam(localApp1Pt, localApp2Pt, localApp3Pt);
            leftPlane.Param = leftPlaneParam;
            leftPlane.Geo = app45LineLeft;
            planes.Add(leftPlane);

            Plane rightPlane = new Plane();
            Ring app45LineRight = new Ring();
            app45LineRight.Add(rightAppPt);
            app45LineRight.Add(rightPts[0]);
            app45LineRight.Add(planes[leftPts.Count - 1].Geo[1]);

            Aran.Geometries.Point localAppRight1Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, rightAppPt);
            Aran.Geometries.Point localAppRight2Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, rightPts[0]);
            Aran.Geometries.Point localAppRight3Pt = ARANFunctions.PrjToLocal(_startCntPrj, _axis, planes[leftPts.Count - 1].Geo[1]);

            PlaneParam rightPlaneParam = CommonFunctions.CalcPlaneParam(localAppRight1Pt, localAppRight2Pt, localAppRight3Pt);

            rightPlane.Param = rightPlaneParam;
            rightPlane.Geo = app45LineRight;
            planes.Add(rightPlane);
            

            //Get the extent transitional surface for calculating obstacle easy
            Aran.Geometries.Point extentPt1 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -appLength, withInnerEdgeMax);
            Aran.Geometries.Point extentPt2 = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -appLength, -withInnerEdgeMax);
            Aran.Geometries.Point extentPt3 = ARANFunctions.LocalToPrj(rightPts[rightPts.Count - 1], _direction, 0, -withInnerEdgeMax);
            Aran.Geometries.Point extentPt4 = ARANFunctions.LocalToPrj(leftPts[leftPts.Count - 1], _direction, 0, withInnerEdgeMax);

            transitional.Geo = CommonFunctions.CreateMultipolygonFromPoints(new Aran.Geometries.Point[] { extentPt1, extentPt2, extentPt3, extentPt4,extentPt1 });

            //
            transitional.Slope = transSlope;
            transitional.Planes = planes;
            transitional.StartPoint = _startCntPrj;
            transitional.Direction = _direction;
            return transitional;
        }

        public BalkedLanding CreateBalkedLandingSurface(CodeLetter codeLetter) 
        {
            BalkedLandingSurface = new BalkedLanding();

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
            if (_codeNumber>2  && codeLetter == CodeLetter.F)
                lenghtOfInnerEdge = 155;
            
            if (_codeNumber < 3) 
                distanceFromThreshold = runwayLength;
            else
                if (runwayLength < 1800)
                    distanceFromThreshold = runwayLength;

            _ptRefBalked = ARANFunctions.LocalToPrj(_startCntPrj, _direction, distanceFromThreshold,0);
            
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
                    SideDirection side2 = ARANMath.SideDef(_ptRefBalked, _direction + Math.PI/2, ProfileLinePoint[i]);
                    
                    if ((int)side1 * (int)side2<0)
                    {
                        Aran.Geometries.Point tmpPt1 = ProfileLinePoint[i - 1];
                        Aran.Geometries.Point tmpPt2 = ProfileLinePoint[i];
                        double fTmp = ARANFunctions.ReturnDistanceInMeters(tmpPt1,tmpPt2 );
                        double fTmp1 = ARANFunctions.ReturnDistanceInMeters(tmpPt1, _ptRefBalked);

                        _ptRefBalked.Z = tmpPt1.Z+ fTmp1 * (tmpPt2.Z - tmpPt1.Z) / fTmp;
                        InTransPlaneCount = i;
                        break;
                    }
                }
            }

            Aran.Geometries.Point pt1 = ARANFunctions.LocalToPrj(_ptRefBalked, _direction, 0, lenghtOfInnerEdge / 2);
            Aran.Geometries.Point pt2 = ARANFunctions.LocalToPrj(_ptRefBalked, _direction, 0, -lenghtOfInnerEdge / 2);

            pt1.Z =pt2.Z = _ptRefBalked.Z;

            double lengthBalkedLanding = (_innerHorElevation - _ptRefBalked.Z) * 100 / slope;
            
            double widthBalkedLanding =lenghtOfInnerEdge/2+ ((_innerHorElevation - _ptRefBalked.Z)/slope) * divergence;

            Aran.Geometries.Point pt3 = ARANFunctions.LocalToPrj(_ptRefBalked, _direction, lengthBalkedLanding, -widthBalkedLanding);
            Aran.Geometries.Point pt4 = ARANFunctions.LocalToPrj(_ptRefBalked, _direction, lengthBalkedLanding, widthBalkedLanding);
            pt3.Z = pt4.Z = _innerHorElevation;
            MultiPolygon geo = CommonFunctions.CreateMultipolygonFromPoints(new Aran.Geometries.Point[] { pt1, pt2, pt3, pt4 });
            
            //Create plane 
            Aran.Geometries.Point planePt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt1);
            Aran.Geometries.Point planePt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt2);
            Aran.Geometries.Point planePt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt3);
            PlaneParam planeParam =  CommonFunctions.CalcPlaneParam(planePt1, planePt2, planePt3);
            
            //

            BalkedLandingSurface.Geo = geo;
            BalkedLandingSurface.PlaneParam = planeParam;
            BalkedLandingSurface.LengthOfInnerEdge = lenghtOfInnerEdge;
            BalkedLandingSurface.DistanceFromTheshold = distanceFromThreshold;
            BalkedLandingSurface.Divergence = divergence;
            BalkedLandingSurface.Slope = slope;
            BalkedLandingSurface.StartPoint = _startCntPrj;
            BalkedLandingSurface.Direction = _direction;
            return BalkedLandingSurface;
        }

        public InnerTransitional CreateInnerTransitional()
        {
            InnerTransitional innerTransitional = new InnerTransitional();
            Ring leftReportGeo = new Ring();
            Ring rightReportGeo = new Ring();

            int ptCount = (2 * (InTransPlaneCount+1)) + 3;
            for (int i = 0; i < ptCount; i++)
            {
                leftReportGeo.Add(new Point());
                rightReportGeo.Add(new Point());
            }

            RunwayConstants tmpConstant = _runwayConstantList[SurfaceType.InnerTransitional, DimensionType.Slope];
            double slope = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            double innerApplength = InnerApproach.Length;
            double innerAppSlope = InnerApproach.Slope;

            //First create plane which near inner approach
            double widthInAppStart =(_innerHorElevation-_startCntPrj.Z)*100/slope;
            double widthInAppEnd = (_innerHorElevation - _startCntPrj.Z - innerAppSlope * innerApplength / 100) * 100 / slope;

            Ring innerAppGeo = InnerApproach.Geo[0].ExteriorRing;
            
            Aran.Geometries.Point innerAppLeftPt1 = ARANFunctions.LocalToPrj(innerAppGeo[0],_direction,0,widthInAppStart);
            Aran.Geometries.Point innerAppLeftPt2 = ARANFunctions.LocalToPrj(innerAppGeo[3], _direction, 0, widthInAppEnd);
            innerAppLeftPt1.Z = innerAppLeftPt2.Z = _innerHorElevation;

            //Extent

            leftReportGeo[0] = innerAppLeftPt2;
            leftReportGeo[1] = innerAppLeftPt1;
            leftReportGeo[ptCount - 1] = innerAppGeo[3];
            leftReportGeo[ptCount - 2] = innerAppGeo[0];
            //
           
            Aran.Geometries.Ring innerAppLeftGeo = new Ring();
            innerAppLeftGeo.Add(innerAppGeo[0]);
            innerAppLeftGeo.Add(innerAppLeftPt1);
            innerAppLeftGeo.Add(innerAppLeftPt2);
            innerAppLeftGeo.Add(innerAppGeo[3]);

            //Create plane for left geo
            Aran.Geometries.Point leftInnerAppLocalPt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, innerAppLeftPt1);
            Aran.Geometries.Point leftInnerAppLocalPt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, innerAppLeftPt2);
            Aran.Geometries.Point leftInnerAppLocalPt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, innerAppGeo[2]);
            PlaneParam leftPlaneParam = CommonFunctions.CalcPlaneParam(leftInnerAppLocalPt1, leftInnerAppLocalPt2, leftInnerAppLocalPt3);
            Plane leftInnerAppPlane = new Plane();
            leftInnerAppPlane.Geo = innerAppLeftGeo;
            leftInnerAppPlane.Param = leftPlaneParam;

            innerTransitional.Planes.Add(leftInnerAppPlane);

            //right plane
            Aran.Geometries.Point innerAppRightPt1 = ARANFunctions.LocalToPrj(innerAppGeo[1], _direction, 0, -widthInAppStart);
            Aran.Geometries.Point innerAppRightPt2 = ARANFunctions.LocalToPrj(innerAppGeo[2], _direction,0, -widthInAppEnd);
            innerAppRightPt1.Z = innerAppRightPt2.Z = _innerHorElevation;

            //Extent

            rightReportGeo[0] = innerAppRightPt2;
            rightReportGeo[1] = innerAppRightPt1;
            rightReportGeo[ptCount-1] = innerAppGeo[2];
            rightReportGeo[ptCount - 2] = innerAppGeo[2];
            //
            
            Aran.Geometries.Ring innerAppRightGeo = new Ring();
            innerAppRightGeo.Add(innerAppGeo[1]);
            innerAppRightGeo.Add(innerAppRightPt1);
            innerAppRightGeo.Add(innerAppRightPt2);
            innerAppRightGeo.Add(innerAppGeo[2]);

            //Create plane for right geo
            Aran.Geometries.Point rightInnerAppLocalPt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, innerAppRightPt1);
            Aran.Geometries.Point rightInnerAppLocalPt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, innerAppRightPt2);
            Aran.Geometries.Point rightInnerAppLocalPt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, innerAppGeo[3]);
            PlaneParam rightInnerAppPlaneParam = CommonFunctions.CalcPlaneParam(rightInnerAppLocalPt1, rightInnerAppLocalPt2, rightInnerAppLocalPt3);
            Plane rightInnerAppPlane = new Plane();
            rightInnerAppPlane.Geo = innerAppRightGeo;
            rightInnerAppPlane.Param = rightInnerAppPlaneParam;
           
            innerTransitional.Planes.Add(rightInnerAppPlane);

            double innerAppWidth = InnerApproach.Width/2;

            //
            Aran.Geometries.Point nextCntPt = ProfileLinePoint[0];
            for (int i = 0; i < InTransPlaneCount; i++)
            {
                nextCntPt = ProfileLinePoint[i+1];
                if (i+1 == InTransPlaneCount)
                    nextCntPt = _ptRefBalked;



                double widthCur = innerAppWidth + (_innerHorElevation - ProfileLinePoint[i].Z) * 100 / slope;
                double widthNext = innerAppWidth + (_innerHorElevation - nextCntPt.Z) * 100 / slope;

                Aran.Geometries.Point leftPlanePt1 = ARANFunctions.LocalToPrj(ProfileLinePoint[i], _direction, 0, innerAppWidth);
                Aran.Geometries.Point leftPlanePt2 = ARANFunctions.LocalToPrj(nextCntPt, _direction, 0, innerAppWidth);
                Aran.Geometries.Point leftPlanePt3 = ARANFunctions.LocalToPrj(nextCntPt, _direction, 0, widthNext);
                Aran.Geometries.Point leftPlanePt4 = ARANFunctions.LocalToPrj(ProfileLinePoint[i], _direction, 0, widthCur);
                leftPlanePt1.Z = ProfileLinePoint[i].Z;
                leftPlanePt2.Z = nextCntPt.Z;
                leftPlanePt3.Z = leftPlanePt4.Z = _innerHorElevation;

                Aran.Geometries.Ring leftGeo = new Ring();
                leftGeo.Add(leftPlanePt1);
                leftGeo.Add(leftPlanePt2);
                leftGeo.Add(leftPlanePt3);
                leftGeo.Add(leftPlanePt4);

                //crete report geo left
                leftReportGeo[i + 2] = leftPlanePt3;
                leftReportGeo[ptCount - 3 - i] = leftPlanePt2;
                //

                //Create left plane param
                Plane leftPlane = new Plane();
                leftPlane.Geo = leftGeo;
                innerTransitional.Planes.Add(leftPlane);
                leftPlane.Param = CommonFunctions.CalcPlaneParamFromPrjPts(_startCntPrj, _axis, leftPlanePt1, leftPlanePt2, leftPlanePt3);
                //

                //Create right plane geo
                Aran.Geometries.Point rightPlanePt1 = ARANFunctions.LocalToPrj(ProfileLinePoint[i], _direction, 0, -innerAppWidth);
                Aran.Geometries.Point rightPlanePt2 = ARANFunctions.LocalToPrj(nextCntPt, _direction, 0, -innerAppWidth);

                Aran.Geometries.Point rightPlanePt3 = ARANFunctions.LocalToPrj(nextCntPt, _direction, 0, -widthNext);
                Aran.Geometries.Point rightPlanePt4 = ARANFunctions.LocalToPrj(ProfileLinePoint[i], _direction, 0, -widthCur);
                rightPlanePt1.Z = ProfileLinePoint[i].Z;
                rightPlanePt2.Z = nextCntPt.Z;
                rightPlanePt3.Z = rightPlanePt4.Z = _innerHorElevation;

                Aran.Geometries.Ring rightGeo = new Ring();
                rightGeo.Add(rightPlanePt1);
                rightGeo.Add(rightPlanePt2);
                rightGeo.Add(rightPlanePt3);
                rightGeo.Add(rightPlanePt4);

                //crete report geo right
                rightReportGeo[i + 2] = rightPlanePt3;
                rightReportGeo[ptCount - 3 - i] = rightPlanePt2;
                //

                //Create left plane param
                Plane rightPlane = new Plane();

                rightPlane.Geo = rightGeo;
                rightPlane.Param = CommonFunctions.CalcPlaneParamFromPrjPts(_startCntPrj, _axis, rightPlanePt1, rightPlanePt2, rightPlanePt3);
                innerTransitional.Planes.Add(rightPlane);
            }
            
            //Create plane which near balked landing surface
            Aran.Geometries.Ring leftBalkedGeo = new Ring();
            leftBalkedGeo.Add(innerTransitional.Planes[innerTransitional.Planes.Count - 2].Geo[1]);
            leftBalkedGeo.Add(innerTransitional.Planes[innerTransitional.Planes.Count - 2].Geo[2]);
            leftBalkedGeo.Add(BalkedLandingSurface.Geo[0].ExteriorRing[0]);

            PlaneParam leftBalkedPlaneParam = CommonFunctions.CalcPlaneParamFromPrjPts(_startCntPrj, _axis, leftBalkedGeo[0], leftBalkedGeo[1], leftBalkedGeo[2]);

            //Report geo
            leftReportGeo[ptCount/2] =leftBalkedGeo[2];
            //
            Plane leftBalkedPlane = new Plane();
            leftBalkedPlane.Geo = leftBalkedGeo;
            leftBalkedPlane.Param = leftBalkedPlaneParam;
            
            //end
            
            Aran.Geometries.Ring rightBalkedGeo = new Ring();
            rightBalkedGeo.Add(innerTransitional.Planes[innerTransitional.Planes.Count - 1].Geo[1]);
            rightBalkedGeo.Add(innerTransitional.Planes[innerTransitional.Planes.Count - 1].Geo[2]);
            rightBalkedGeo.Add(BalkedLandingSurface.Geo[0].ExteriorRing[1]);

            PlaneParam rightBalkedPlaneParam = CommonFunctions.CalcPlaneParamFromPrjPts(_startCntPrj, _axis, rightBalkedGeo[0], rightBalkedGeo[1], rightBalkedGeo[2]);

            //report geo
            rightReportGeo[ptCount/2] =rightBalkedGeo[2];
            //end

            Plane rightBalkedPlane = new Plane();
            rightBalkedPlane.Geo = rightBalkedGeo;
            rightBalkedPlane.Param = rightBalkedPlaneParam;

            innerTransitional.Planes.Add(leftBalkedPlane);
            innerTransitional.Planes.Add(rightBalkedPlane);

            Aran.Geometries.Polygon leftPoly = new Polygon();
            leftPoly.ExteriorRing = leftReportGeo;

            Aran.Geometries.Polygon rightPoly = new Polygon();
            rightPoly.ExteriorRing = rightReportGeo;

            Aran.Geometries.MultiPolygon resultGeo = new MultiPolygon();
            resultGeo.Add(leftPoly);
            resultGeo.Add(rightPoly);

            innerTransitional.Geo = resultGeo;
            innerTransitional.Direction = _direction;
            innerTransitional.StartPoint = _startCntPrj;
            //
            return innerTransitional;

        }

        public TakeOffClimb CreateTakeOffSurface(bool isGreaterThan15)
        {
            Ring transGeo = new Ring();
            RunwayConstants tmpConstant = _runwayConstantList[SurfaceType.TakeOffClimb, DimensionType.LengthOfInnerEdge];
            double lengthOfInnerEdge = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.TakeOffClimb, DimensionType.Divergence];
            double divergence = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.TakeOffClimb, DimensionType.DistanceFromRunwayEnd];
            double distanceFromRunwayEnd = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.TakeOffClimb, DimensionType.FinalWidth];
            double finalWidth = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.TakeOffClimb, DimensionType.Length];
            double length = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            tmpConstant = _runwayConstantList[SurfaceType.TakeOffClimb, DimensionType.Slope];
            double slope = tmpConstant.GetValue(_rwyClassifationType, _catNumber, _codeNumber);

            if (_clearWay > distanceFromRunwayEnd)
                distanceFromRunwayEnd = _clearWay;

            if (isGreaterThan15)
                finalWidth = 18000;

            Aran.Geometries.Point pt1 = ARANFunctions.LocalToPrj(_endCntPrj, _direction, distanceFromRunwayEnd, lengthOfInnerEdge / 2);
            Aran.Geometries.Point pt2 = ARANFunctions.LocalToPrj(_endCntPrj, _direction, distanceFromRunwayEnd, -lengthOfInnerEdge / 2);
            pt1.Z = pt2.Z = _endCntPrj.Z;
            
            double section1Length = ((finalWidth - lengthOfInnerEdge) / 2) * 100 / divergence;

            Aran.Geometries.Point pt3 = ARANFunctions.LocalToPrj(_endCntPrj, _direction, section1Length, -finalWidth / 2);
            pt3.Z = (((finalWidth - lengthOfInnerEdge) / 2* divergence) * slope) / 100 + _endCntPrj.Z;

            transGeo.Add(pt1);
            transGeo.Add(pt2);
            transGeo.Add(pt3);

            Aran.Geometries.Point planePt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt1);
            Aran.Geometries.Point planePt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt2);

            Aran.Geometries.Point planePt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt3);
            if (_codeNumber <= 2) 
            {
                Aran.Geometries.Point pt4 = ARANFunctions.LocalToPrj(_endCntPrj, _direction, section1Length, finalWidth / 2);
                pt4.Z = pt3.Z;
                transGeo.Add(pt4);
            }
            else
            {
                Aran.Geometries.Point pt4 = ARANFunctions.LocalToPrj(_endCntPrj, _direction, length, -finalWidth / 2);
                Aran.Geometries.Point pt5 = ARANFunctions.LocalToPrj(_endCntPrj, _direction, length, finalWidth / 2);
                pt4.Z =pt5.Z = length * slope / 100 + _endCntPrj.Z;

                Aran.Geometries.Point pt6 = ARANFunctions.LocalToPrj(_endCntPrj, _direction, section1Length, finalWidth / 2);
                pt6.Z = pt3.Z;
                transGeo.Add(pt4);
                transGeo.Add(pt5);
                transGeo.Add(pt6);

                planePt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt4);
            }

            PlaneParam planeParam = CommonFunctions.CalcPlaneParam(planePt1, planePt2, planePt3);
            Aran.Geometries.Polygon poly = new Polygon();
            poly.ExteriorRing = transGeo;
            Aran.Geometries.MultiPolygon mlt = new MultiPolygon();
            mlt.Add(poly);
            TakeOffClimb takeOfClimb = new TakeOffClimb();
            takeOfClimb.Geo = mlt;
            takeOfClimb.PlaneParam = planeParam;
            return takeOfClimb;
        }

        public List<Aran.Geometries.Point> ProfileLinePoint { get; private set; }
        
        #endregion
    }
}
      