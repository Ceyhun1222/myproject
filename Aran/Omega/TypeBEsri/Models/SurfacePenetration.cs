using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Converters;
using Aran.Panda.Common;

namespace Aran.Omega.TypeBEsri.Models
{
    public class SurfacePenetration
    {
        private TypeBSurfaces _typeBSurfaces;
        private InnerHorizontal _innerHorizontal;
        private Approach _approach;
        private TakeOffClimb _takeOffClimb;
        private double _clearWay;
        private double _approachSlope;
        private double _appDistanceFromThreshold;
        private double _takeOffSlope;
        private Aran.Geometries.Point _startCntPrj;
        private double _direction;
        private RwyClass _selectedRwyClass;
        private RwyDirClass _rwyDir;

        public SurfacePenetration(TypeBSurfaces surfaces,RwyClass rwyClass,RwyDirClass rwyDirClass)
        {
            _typeBSurfaces = surfaces;
            _selectedRwyClass = rwyClass;
            _rwyDir = rwyDirClass;
            _innerHorizontal = _typeBSurfaces.InnerHorizontalSurface;
            _clearWay = _typeBSurfaces.ClearWay;
            _approachSlope = _typeBSurfaces.ApproachSurface.FirstSectionSlope;
            _appDistanceFromThreshold = _typeBSurfaces.ApproachSurface.DistanceFromThreshold;
            _takeOffSlope = _typeBSurfaces.TakeOffSurface.Slope;
            _startCntPrj = _typeBSurfaces.StartCntPtPrj;
            _direction = _typeBSurfaces.ApproachSurface.Direction;
            _approach = _typeBSurfaces.ApproachSurface;
            _takeOffClimb = _typeBSurfaces.TakeOffSurface;
            if (_typeBSurfaces.ClearWay<1)
                _clearWay = _takeOffClimb.DistanceFromThreshold;
        }

        public void CutInnerHorizontal()
        {
            for (int i = 0; i < 2; i++)
            {
                var rwyDir =_selectedRwyClass.RwyDirClassList[i];
                var isSameDirection = true;
                var threshold = _startCntPrj;
                var tmpDirection = _direction;
                if (_rwyDir.Name != rwyDir.Name)
                {
                    isSameDirection = false;
                    threshold = GlobalParams.SpatialRefOperation.ToPrj(rwyDir.StartCntlPt.Location.Geo);
                    threshold.Z = ConverterToSI.Convert(rwyDir.StartCntlPt.Location.Elevation, 0);
                    tmpDirection = _direction + Math.PI;
                }

                double approachInnerIntersectLength = ((_innerHorizontal.Elevation - threshold.Z) * 100 / _approachSlope) + _typeBSurfaces.ApproachSurface.DistanceFromThreshold;

                double takeOffInnerIntersectionLength = (_innerHorizontal.Elevation - threshold.Z) * 100 / _takeOffSlope + _clearWay;

                double takeOffApproachIntersectionLength = 0;
                bool isIntersection = IsIntersection(ref takeOffApproachIntersectionLength);

                //If takeOff surface and approach is intersect
                if (isIntersection)
                {
                    //If approach is started first
                    if (_appDistanceFromThreshold < _clearWay)
                    {
                        if (takeOffApproachIntersectionLength < takeOffInnerIntersectionLength)
                        {
                            //takOff construction from start to intersection approach and takeoff
                            var takeOffRing = ConstructTakeOffPart(threshold,tmpDirection,_clearWay, takeOffApproachIntersectionLength);
                            _takeOffClimb.CuttingGeo1.Add(new Geometries.Polygon { ExteriorRing = takeOffRing });
                            //End 

                            var appRing2 = ConstructApproachPart(threshold,tmpDirection,_appDistanceFromThreshold, approachInnerIntersectLength);
                            var approachGeo = GlobalParams.GeomOperators.Difference(new Aran.Geometries.Polygon { ExteriorRing = appRing2 }, new Aran.Geometries.Polygon { ExteriorRing = takeOffRing }) as Aran.Geometries.MultiPolygon;
                            
                            var appFirstSectionGeoSlope = new GeoSlope();
                            appFirstSectionGeoSlope.Geo = approachGeo;
                            appFirstSectionGeoSlope.Slope = _approach.FirstSectionSlope;
                            if (isSameDirection)
                            {
                                _approach.CuttingGeo1 = approachGeo;
                                _approach.CuttingGeo1Planes.Add(appFirstSectionGeoSlope);

                            }
                            else
                            {
                                _approach.CuttingGeo2 = approachGeo;
                                _approach.CuttingGeo2Planes.Add(appFirstSectionGeoSlope);
                            }
                        }
                        else
                        {
                            var appRing = ConstructApproachPart(threshold,tmpDirection,_appDistanceFromThreshold, takeOffInnerIntersectionLength);
                            
                            //var appFirstSectionPlane = new Plane();
                            //appFirstSectionPlane.Geo = appRing;
                            //appFirstSectionPlane.Slope = _approach.FirstSectionSlope;
                            if (isSameDirection)
                            {
                                _approach.CuttingGeo1.Add(new Geometries.Polygon { ExteriorRing = appRing });
                                //_approach.CuttingGeo1Planes.Add(appFirstSectionPlane);
                            }
                            else
                            {
                                _approach.CuttingGeo2.Add(new Geometries.Polygon { ExteriorRing = appRing });
                                //_approach.CuttingGeo2Planes.Add(appFirstSectionPlane);
                            }
                            var takeOffRing = ConstructTakeOffPart(threshold, tmpDirection, _clearWay, takeOffInnerIntersectionLength);
                            if (isSameDirection)
                                _takeOffClimb.CuttingGeo1.Add(new Geometries.Polygon { ExteriorRing = takeOffRing });
                            else
                                _takeOffClimb.CuttingGeo2.Add(new Geometries.Polygon { ExteriorRing = takeOffRing });

                            var approachGeo = GlobalParams.GeomOperators.Difference(new Aran.Geometries.Polygon { ExteriorRing = appRing }, new Aran.Geometries.Polygon { ExteriorRing = takeOffRing }) as Aran.Geometries.MultiPolygon;
                            
                            var appFirstSectionGeoSlope = new GeoSlope();
                            appFirstSectionGeoSlope.Geo = approachGeo;
                            appFirstSectionGeoSlope.Slope = _approach.FirstSectionSlope;

                            if (isSameDirection)
                            {
                                _approach.CuttingGeo1 = approachGeo;
                                _approach.CuttingGeo1Planes.Add(appFirstSectionGeoSlope);
                            }
                            else
                            {
                                _approach.CuttingGeo2 = approachGeo;
                                _approach.CuttingGeo2Planes.Add(appFirstSectionGeoSlope);
                            }
                        }
                    }
                }
                //takeoff and approach doesn't intersect
                else
                {
                    //Approach is started first
                    if (_appDistanceFromThreshold < _clearWay)
                    {
                        double axisIntersectionLength = 0;
                        //Approach and take off 2D surfaces is intersect
                        if (AxisIntersection(ref axisIntersectionLength))
                        {
                            var appLengthOffInnerEdgeInClearWay = _approach.LengthOfInnerEdge / 2 + LengthOffInnerEdge(_clearWay - _appDistanceFromThreshold, _approach.Divergence);

                            var takeOffRing = ConstructTakeOffPart(threshold, tmpDirection, _clearWay, takeOffInnerIntersectionLength);
                            _takeOffClimb.CuttingGeo1.Add(new Geometries.Polygon { ExteriorRing = takeOffRing });

                            double appLength = axisIntersectionLength >= approachInnerIntersectLength ? axisIntersectionLength : approachInnerIntersectLength;
                            if (appLengthOffInnerEdgeInClearWay > _takeOffClimb.LengthOfInnerEdge / 2)
                                appLength = axisIntersectionLength <= approachInnerIntersectLength ? axisIntersectionLength : approachInnerIntersectLength;

                            var appRing = ConstructApproachPart(threshold, tmpDirection, _approach.DistanceFromThreshold, appLength);

                            var approachGeo = GlobalParams.GeomOperators.Difference(new Aran.Geometries.Polygon { ExteriorRing = appRing }, new Aran.Geometries.Polygon { ExteriorRing = takeOffRing }) as Aran.Geometries.MultiPolygon;

                            if (approachGeo != null && !approachGeo.IsEmpty)
                            {
                                var appFirstSectionGeoSlope = new GeoSlope();
                                appFirstSectionGeoSlope.Geo = approachGeo;
                                appFirstSectionGeoSlope.Slope = _approach.FirstSectionSlope;

                                if (isSameDirection)
                                {
                                    _approach.CuttingGeo1 = approachGeo;
                                    _approach.CuttingGeo1Planes.Clear();
                                    _approach.CuttingGeo1Planes.Add(appFirstSectionGeoSlope);
                                }
                                else
                                {
                                    _approach.CuttingGeo2 = approachGeo;
                                    _approach.CuttingGeo2Planes.Clear();
                                    _approach.CuttingGeo2Planes.Add(appFirstSectionGeoSlope);
                                }
                            }
                        }
                        else
                        {
                            var takeOffRing = ConstructTakeOffPart(threshold, tmpDirection, _clearWay, takeOffInnerIntersectionLength);
                            _takeOffClimb.CuttingGeo1.Add(new Geometries.Polygon { ExteriorRing = takeOffRing });

                            var appRing = ConstructApproachPart(threshold, tmpDirection, _approach.DistanceFromThreshold, approachInnerIntersectLength);

                            var approachGeo = GlobalParams.GeomOperators.Difference(new Geometries.Polygon { ExteriorRing = appRing }, new Geometries.Polygon { ExteriorRing = takeOffRing });

                            var appFirstSectionGeoSlope = new GeoSlope();
                            appFirstSectionGeoSlope.Geo = approachGeo as Aran.Geometries.MultiPolygon;
                            appFirstSectionGeoSlope.Slope = _approach.FirstSectionSlope;

                            if (isSameDirection)
                            {
                                _approach.CuttingGeo1 = approachGeo as Aran.Geometries.MultiPolygon;
                                _approach.CuttingGeo1Planes.Clear();
                                _approach.CuttingGeo1Planes.Add(appFirstSectionGeoSlope);
                            }
                            else
                            {
                                _approach.CuttingGeo2 = approachGeo as Aran.Geometries.MultiPolygon;
                                _approach.CuttingGeo2Planes.Clear();
                                _approach.CuttingGeo2Planes.Add(appFirstSectionGeoSlope);
                            }
                        }

                    }
                    //They start same point
                    //They doesn't intersect
                    else if (Math.Abs(_appDistanceFromThreshold - _clearWay) < 0.1)
                    {
                        if (_takeOffClimb.Slope < _approach.FirstSectionSlope)
                        {
                            var takeOffRing = ConstructTakeOffPart(threshold, tmpDirection, _clearWay, takeOffInnerIntersectionLength);
                            _takeOffClimb.CuttingGeo1.Add(new Geometries.Polygon { ExteriorRing = takeOffRing });

                            if (_takeOffClimb.LengthOfInnerEdge < _approach.LengthOfInnerEdge)
                            {
                                var appRing = ConstructApproachPart(threshold, tmpDirection, _approach.DistanceFromThreshold, approachInnerIntersectLength);
                                var approachGeo = GlobalParams.GeomOperators.Difference(new Geometries.Polygon { ExteriorRing = appRing }, new Geometries.Polygon { ExteriorRing = takeOffRing });

                            }
                        }
                        else if (_takeOffClimb.Slope == _approach.FirstSectionSlope)
                        {
                            if (_takeOffClimb.LengthOfInnerEdge > _approach.LengthOfInnerEdge)
                            {
                                var takeOffRing = ConstructTakeOffPart(threshold, tmpDirection, _clearWay, takeOffInnerIntersectionLength);
                                if (isSameDirection)
                                    _takeOffClimb.CuttingGeo1.Add(new Geometries.Polygon { ExteriorRing = takeOffRing });
                                else
                                    _takeOffClimb.CuttingGeo2.Add(new Geometries.Polygon { ExteriorRing = takeOffRing });
                            }
                            else
                            {
                                var appRing = ConstructApproachPart(threshold, tmpDirection, _approach.DistanceFromThreshold, approachInnerIntersectLength);

                                var appFirstSectionGeoSlope = new GeoSlope();
                                appFirstSectionGeoSlope.Geo = new Aran.Geometries.MultiPolygon{new Aran.Geometries.Polygon{ExteriorRing= appRing}};
                                appFirstSectionGeoSlope.Slope = _approach.FirstSectionSlope;

                                if (isSameDirection)
                                {
                                    _approach.CuttingGeo1.Add(new Geometries.Polygon { ExteriorRing = appRing });
                                    _approach.CuttingGeo1Planes.Add(appFirstSectionGeoSlope);
                                }
                                else
                                {
                                    _approach.CuttingGeo2.Add(new Geometries.Polygon { ExteriorRing = appRing });
                                    _approach.CuttingGeo2Planes.Add(appFirstSectionGeoSlope);
                                }
                            }
                        }
                        else
                        {
                            var appRing = ConstructApproachPart(threshold, tmpDirection, _approach.DistanceFromThreshold, approachInnerIntersectLength);
                            
                            var appFirstSectionGeoSlope = new GeoSlope();
                            appFirstSectionGeoSlope.Geo = new Aran.Geometries.MultiPolygon { new Aran.Geometries.Polygon { ExteriorRing = appRing } };
                            appFirstSectionGeoSlope.Slope = _approach.FirstSectionSlope;

                            if (isSameDirection)
                            {
                                //_approach.CuttingGeo
                                _approach.CuttingGeo1.Add(new Geometries.Polygon { ExteriorRing = appRing });
                                _approach.CuttingGeo1Planes.Add(appFirstSectionGeoSlope);
                            }
                            else
                            {
                                _approach.CuttingGeo2.Add(new Geometries.Polygon { ExteriorRing = appRing });
                                _approach.CuttingGeo2Planes.Add(appFirstSectionGeoSlope);
                            }
                           
                            if (_approach.LengthOfInnerEdge < _takeOffClimb.LengthOfInnerEdge)
                            {
                                var takeOffRing = ConstructTakeOffPart(threshold, tmpDirection, _clearWay, takeOffInnerIntersectionLength);
                                var takeOffGeo = GlobalParams.GeomOperators.Difference(new Geometries.Polygon { ExteriorRing = takeOffRing }, new Geometries.Polygon { ExteriorRing = takeOffRing });

                            }
                        }
                        //Approach and take off 2D surfaces is intersect

                    }
                    //Takeof started first
                    else
                    {

                        var appRing = ConstructApproachPart(threshold, tmpDirection, _approach.DistanceFromThreshold, approachInnerIntersectLength);

                        var appFirstSectionGeoSlope = new GeoSlope();
                        appFirstSectionGeoSlope.Geo = new Aran.Geometries.MultiPolygon { new Aran.Geometries.Polygon { ExteriorRing = appRing } };
                        appFirstSectionGeoSlope.Slope = _approach.FirstSectionSlope;

                        if (isSameDirection)
                        {
                            _approach.CuttingGeo1.Add(new Geometries.Polygon { ExteriorRing = appRing });
                            _approach.CuttingGeo1Planes.Add(appFirstSectionGeoSlope);
                        }
                        else
                        {
                            _approach.CuttingGeo2.Add(new Geometries.Polygon { ExteriorRing = appRing });
                            _approach.CuttingGeo2Planes.Add(appFirstSectionGeoSlope);
                        }

                        var takeOffRing = ConstructTakeOffPart(threshold, tmpDirection, _clearWay, _approach.DistanceFromThreshold);
                        if (isSameDirection)
                            _takeOffClimb.CuttingGeo1.Add(new Geometries.Polygon { ExteriorRing = takeOffRing });
                        else
                            _takeOffClimb.CuttingGeo2.Add(new Geometries.Polygon { ExteriorRing = takeOffRing });

                    }
                }
            }

        }

        public void CutConicalSurface()
        {
            ConicalSurface conical = _typeBSurfaces.ConicalSurface;
            conical.CuttingGeo = conical.Geo;
            OuterHorizontal outerHorizontal = _typeBSurfaces.OuterHorizontal;
            var b1 = conical.ElevInnerHor;
            for (int j = 0; j < 2; j++)
            {
                if (_approach.SecondPlane)
                {
                    var rwyDir = _selectedRwyClass.RwyDirClassList[j];
                    var isSameDirection = true;
                    var threshold = _startCntPrj;
                    var tmpDirection = _direction;
                    if (_rwyDir.Name != rwyDir.Name)
                    {
                        isSameDirection = false;
                        threshold = GlobalParams.SpatialRefOperation.ToPrj(rwyDir.StartCntlPt.Location.Geo);
                        threshold.Z = ConverterToSI.Convert(rwyDir.StartCntlPt.Location.Elevation, 0);
                        tmpDirection = _direction + Math.PI;
                    }
                    var b2 = -_approach.Planes1[1].Param.D;

                    var k1 = conical.Slope / 100;
                    var k2 = _approach.SecondSectionSlope / 100;
                    double x1 = (k1 * conical.InnerHorRadius + b2 - b1) / (k1 - k2);
                    double x2 = (b1 + b2 - k1 * conical.InnerHorRadius) / (k1 + k2);

                    var a = (x1 - x2) / 2;
                    var centerLocal = x1 - a;

                    //  var b = a * (k1 - k2) / k1;
                    var h = k2 * (centerLocal) + b2;
                    var conicWidth = (h - b1) / k1 + _innerHorizontal.Radius;
                    var b = Math.Sqrt(conicWidth * conicWidth - centerLocal * centerLocal);

                    int steps = 20;
                    int stepsCount = Convert.ToInt32(b / steps);
                    var mltEllipse = new Aran.Geometries.MultiPoint();
                    for (int i = 0; i < stepsCount; i++)
                    {
                        var y = 20 * i;
                        var dx1 = a * Math.Sqrt(1 - (y * y) / (b * b)) + centerLocal;
                        var dx2 = -a * Math.Sqrt(1 - (y * y) / (b * b)) + centerLocal;

                        var ellipsePt1 = ARANFunctions.LocalToPrj(threshold, tmpDirection, -dx1, -y);
                        var ellipsePt2 = ARANFunctions.LocalToPrj(threshold, tmpDirection, -dx1, y);
                        var ellipsePt3 = ARANFunctions.LocalToPrj(threshold, tmpDirection, -dx2, -y);
                        var ellipsePt4 = ARANFunctions.LocalToPrj(threshold, tmpDirection, -dx2, y);
                        mltEllipse.Add(ellipsePt1);
                        mltEllipse.Add(ellipsePt2);
                        mltEllipse.Add(ellipsePt3);
                        mltEllipse.Add(ellipsePt4);
                    }

                    var polyEllipse = ChainHullAlgorithm.ConvexHull(mltEllipse);
                    var conicElevation = conical.Elevation;
                    var approachElev = threshold.Z + _approach.FirstSectionLength * _approach.FirstSectionSlope / 100 + _approach.SecondSectionLength * _approach.SecondSectionSlope / 100;
                    var geo = new Aran.Geometries.Polygon();
                    if (approachElev < conicElevation)
                    {
                        if (isSameDirection)
                            geo.ExteriorRing = _approach.Planes1[1].Geo;
                        else
                            geo.ExteriorRing = _approach.Planes2[1].Geo;
                    }
                    else 
                    {
                        var length = (conicElevation - threshold.Z - _approach.FirstSectionLength * _approach.FirstSectionSlope / 100) * 100 / _approach.SecondSectionSlope;
                        var appPart  =ConstructApproachPart(threshold, tmpDirection, _approach.FirstSectionLength + _approach.DistanceFromThreshold, length+_approach.FirstSectionLength+_approach.DistanceFromThreshold);
                        geo.ExteriorRing = appPart;
                    }

                    var approachCircleIntersection = GlobalParams.GeomOperators.Difference(geo, polyEllipse) as Aran.Geometries.MultiPolygon;
                    if (approachCircleIntersection != null)
                    {
                        var appSecondSectionGeoSlope = new GeoSlope();
                        appSecondSectionGeoSlope.Geo = approachCircleIntersection;
                        appSecondSectionGeoSlope.Slope = _approach.SecondSectionSlope;

                        if (isSameDirection)
                        {
                            foreach (var appPart in approachCircleIntersection)
                                _approach.CuttingGeo1.Add(appPart as Aran.Geometries.Polygon);

                            _approach.CuttingGeo1Planes.Add(appSecondSectionGeoSlope);
                        }
                        else
                        {
                            foreach (var appPart in approachCircleIntersection)
                                _approach.CuttingGeo2.Add(appPart as Aran.Geometries.Polygon);

                            _approach.CuttingGeo2Planes.Add(appSecondSectionGeoSlope);
                        }
                    }

                    //Finding approach outerhorizontal cutting geo
                    
                    if (approachElev < conicElevation)
                    {
                        var appSecondSectionGeoSlope = new GeoSlope();
                        appSecondSectionGeoSlope.Geo = new Geometries.MultiPolygon{new Aran.Geometries.Polygon { ExteriorRing = _approach.Planes1[2].Geo }};

                        if (isSameDirection)
                        {
                            _approach.CuttingGeo1.Add(new Aran.Geometries.Polygon { ExteriorRing = _approach.Planes1[2].Geo });
                            _approach.CuttingGeo1Planes.Add(appSecondSectionGeoSlope);
                        }
                        else
                        {
                            _approach.CuttingGeo2.Add(new Aran.Geometries.Polygon { ExteriorRing = _approach.Planes2[2].Geo });
                            _approach.CuttingGeo2Planes.Add(appSecondSectionGeoSlope);
                        }
                    }

                    var conicDifference = GlobalParams.GeomOperators.Difference(conical.CuttingGeo, approachCircleIntersection);
                    conical.CuttingGeo = conicDifference as Aran.Geometries.MultiPolygon;

                }
            }

           // var conicDifference = GlobalParams.GeomOperators.Difference(conical.Geo, approachCircleIntersection);
            if (_takeOffClimb.Length > _innerHorizontal.Radius)
            {

                var b2 = -_takeOffClimb.Planes2[0].Param.D ;
                //if (!isSameDirection)
                //    b2 = -_approach.Planes2[1].Param.D;

                for (int j = 0; j < 2; j++)
                {
                    var rwyDir = _selectedRwyClass.RwyDirClassList[j];
                    var isSameDirection = true;
                    //var threshold = _startCntPrj;
                    var endCntlnPt = GlobalParams.SpatialRefOperation.ToPrj(rwyDir.EndCntlPt.Location.Geo);
                    endCntlnPt.Z = ConverterToSI.Convert(rwyDir.EndCntlPt.Location.Elevation, 0);
                    var tmpDirection = _direction+Math.PI;
                    if (_rwyDir.Name != rwyDir.Name)
                    {
                        isSameDirection = false;
                        //threshold = GlobalParams.SpatialRefOperation.ToPrj(rwyDir.EndCntlPt.Location.Geo);
                        //threshold.Z = ConverterToSI.Convert(rwyDir.EndCntlPt.Location.Elevation, 0);
                        tmpDirection = _direction;
                    }

                    var k1 = conical.Slope / 100;
                    var k2 = _takeOffClimb.Slope / 100;
                    double x1 = (k1 * conical.InnerHorRadius + b2 - b1) / (k1 - k2);
                    double x2 = (b1 + b2 - k1 * conical.InnerHorRadius) / (k1 + k2);

                    var a = (x1 - x2) / 2;
                    var centerLocal = x1 - a;

                    //  var b = a * (k1 - k2) / k1;
                    var h = k2 * (centerLocal) + b2;
                    var conicWidth = (h - b1) / k1 + _innerHorizontal.Radius;
                    var b = Math.Sqrt(conicWidth * conicWidth - centerLocal * centerLocal);

                    int steps = 20;
                    int stepsCount = Convert.ToInt32(b / steps);
                    var mltEllipse = new Aran.Geometries.MultiPoint();
                    for (int i = 0; i < stepsCount; i++)
                    {
                        var y = 20 * i;
                        var dx1 = a * Math.Sqrt(1 - (y * y) / (b * b)) + centerLocal;
                        var dx2 = -a * Math.Sqrt(1 - (y * y) / (b * b)) + centerLocal;

                        var ellipsePt1 = ARANFunctions.LocalToPrj(endCntlnPt, tmpDirection, -dx1, -y);
                        var ellipsePt2 = ARANFunctions.LocalToPrj(endCntlnPt, tmpDirection, -dx1, y);
                        var ellipsePt3 = ARANFunctions.LocalToPrj(endCntlnPt, tmpDirection, -dx2, -y);
                        var ellipsePt4 = ARANFunctions.LocalToPrj(endCntlnPt, tmpDirection, -dx2, y);
                        mltEllipse.Add(ellipsePt1);
                        mltEllipse.Add(ellipsePt2);
                        mltEllipse.Add(ellipsePt3);
                        mltEllipse.Add(ellipsePt4);
                    }

                    var polyEllipse = ChainHullAlgorithm.ConvexHull(mltEllipse);
                    var takeOffSecondSector = new Aran.Geometries.Polygon();
                    if (isSameDirection)
                        takeOffSecondSector.ExteriorRing = _takeOffClimb.Planes1[1].Geo;
                    else
                        takeOffSecondSector.ExteriorRing = _takeOffClimb.Planes2[1].Geo;

                    var takeOffCircleIntersection = GlobalParams.GeomOperators.Difference(takeOffSecondSector, polyEllipse) as Aran.Geometries.MultiPolygon;
                    if (takeOffCircleIntersection != null)
                    {
                        var lengthToIntersectOuterHor = (conical.Elevation - endCntlnPt.Z) *100 / _takeOffClimb.Slope+_clearWay;

                        var takeOffToOuter = new Aran.Geometries.Polygon { ExteriorRing = ConstructTakeOffPart(endCntlnPt, tmpDirection, _clearWay, lengthToIntersectOuterHor) };
                        Aran.Geometries.Polygon intersect = null;
                        if (isSameDirection)
                            intersect = GlobalParams.GeomOperators.Intersect(_takeOffClimb.Geo, takeOffToOuter) as Aran.Geometries.Polygon;
                        else
                            intersect = GlobalParams.GeomOperators.Intersect(_takeOffClimb.Geo2, takeOffToOuter) as Aran.Geometries.Polygon;

                       var takeOffCutToOuter = GlobalParams.GeomOperators.Intersect(takeOffCircleIntersection, intersect) as Aran.Geometries.Polygon;

                        //GlobalParams.UI.DrawPolygon(intersect, 100, AranEnvironment.Symbols.eFillStyle.sfsHorizontal);

                        if (isSameDirection)
                        {
                           // foreach (var takeOffPart in takeOffCircleIntersection)
                            _takeOffClimb.CuttingGeo1.Add(takeOffCutToOuter);
                        }
                        else
                        {
                            //foreach (var takeOffPart in takeOffCircleIntersection)
                             _takeOffClimb.CuttingGeo2.Add(takeOffCutToOuter);
                             
                        }
                       
                        Aran.Geometries.Operators.GeometryOperators esriGeomOperators = new Geometries.Operators.GeometryOperators();
                        if (isSameDirection)
                        {
                           var appCut = esriGeomOperators.Difference(_approach.CuttingGeo2, takeOffCutToOuter) as Aran.Geometries.MultiPolygon;
                            if (appCut!=null)
                                _approach.CuttingGeo2 = appCut;
                        }
                        else
                        {
                            var appCut = esriGeomOperators.Difference(_approach.CuttingGeo1, takeOffCutToOuter) as Aran.Geometries.MultiPolygon;
                            if (appCut!=null)
                                _approach.CuttingGeo1 = appCut;                            
                        }

                        var conicDifference = GlobalParams.GeomOperators.Difference(conical.CuttingGeo, takeOffCutToOuter);
                        conical.CuttingGeo = conicDifference as Aran.Geometries.MultiPolygon;
                       
                    }
                }
                
            }
            Aran.Geometries.Geometry outerCuttingGeo = outerHorizontal.Geo;
            if (_approach.CuttingGeo1!=null)
                outerCuttingGeo = GlobalParams.GeomOperators.Difference(outerHorizontal.Geo, _approach.CuttingGeo1);
            if (_approach.CuttingGeo2!=null)
                outerCuttingGeo = GlobalParams.GeomOperators.Difference(outerCuttingGeo, _approach.CuttingGeo2);
            if (_takeOffClimb.CuttingGeo1!=null)
                outerCuttingGeo = GlobalParams.GeomOperators.Difference(outerCuttingGeo, _takeOffClimb.CuttingGeo1);
            if (_takeOffClimb.CuttingGeo2!=null)
                outerCuttingGeo = GlobalParams.GeomOperators.Difference(outerCuttingGeo, _takeOffClimb.CuttingGeo2);
            outerHorizontal.CuttingGeo = outerCuttingGeo as Aran.Geometries.MultiPolygon;
        }

        private Aran.Geometries.Ring ConstructApproachPart(Aran.Geometries.Point startPtPrj,double direction,double dist1,double dist2) 
        {
            double lengthOffInnerEdge = _approach.LengthOfInnerEdge / 2;
            double width = lengthOffInnerEdge + (dist2 - _approach.DistanceFromThreshold) * _approach.Divergence / 100;
            double width2 = _approach.LengthOfInnerEdge/2;
            if (Math.Abs(dist1 - _approach.DistanceFromThreshold) > 0.1)
                width2 = lengthOffInnerEdge + (dist1 - _approach.DistanceFromThreshold) * _approach.Divergence / 100;

            var pt1 = ARANFunctions.LocalToPrj(startPtPrj, direction, -dist1, -width2);
            var pt2 = ARANFunctions.LocalToPrj(startPtPrj, direction, -dist1, width2);
            var pt3 = ARANFunctions.LocalToPrj(startPtPrj, direction, -dist2, width);
            var pt4 = ARANFunctions.LocalToPrj(startPtPrj, direction, -dist2, -width);
            
            var result = new Aran.Geometries.Ring();
            result.Add(pt1);
            result.Add(pt2);
            result.Add(pt3);
            result.Add(pt4);
            return result;
        }

        private Aran.Geometries.Ring ConstructTakeOffPart(Aran.Geometries.Point startPtPrj, double direction, double dist1, double dist2)
        {
            double lengthOffInnerEdge = _takeOffClimb.LengthOfInnerEdge / 2;
            double width = lengthOffInnerEdge + (dist2 - _clearWay) * _takeOffClimb.Divergence / 100;

            double width2 = _takeOffClimb.LengthOfInnerEdge / 2;
            if (Math.Abs(dist1 - _clearWay) > 0.1)
                width2 = lengthOffInnerEdge + (dist1 - _takeOffClimb.DistanceFromThreshold) * _takeOffClimb.Divergence / 100;
            var pt1 = ARANFunctions.LocalToPrj(startPtPrj, direction, -dist1, -lengthOffInnerEdge);
            var pt2 = ARANFunctions.LocalToPrj(startPtPrj, direction, -dist1, lengthOffInnerEdge);
            var pt3 = ARANFunctions.LocalToPrj(startPtPrj, direction, -dist2, width);
            var pt4 = ARANFunctions.LocalToPrj(startPtPrj, direction, -dist2, -width);

            var result = new Aran.Geometries.Ring();
            result.Add(pt1);
            result.Add(pt2);
            result.Add(pt3);
            result.Add(pt4);
            return result;
        }

        private bool IsIntersection(ref double length) 
        {
            double clearWay = _typeBSurfaces.ClearWay;
            double approachSlope = _typeBSurfaces.ApproachSurface.FirstSectionSlope/100;
            double appDistanceFromThreshold =_typeBSurfaces.ApproachSurface.DistanceFromThreshold;
            double takeOffSlope = _typeBSurfaces.TakeOffSurface.Slope/100;

            if (takeOffSlope <= approachSlope)
                return false;

            length = (appDistanceFromThreshold * approachSlope - takeOffSlope * clearWay) / (approachSlope - takeOffSlope);
            if (length<0)
                return false;
            return true;
        }

        private bool AxisIntersection(ref double distance)
        {
            double lengthOffInnerEdge1 = _takeOffClimb.LengthOfInnerEdge / 2;
            double lengthOffInnerEdge2 = _approach.LengthOfInnerEdge/2;
            double D1  =_takeOffClimb.Divergence/100;
            double D2 = _approach.Divergence/100;
            if (D1 == D2)
                return false;
            distance = (lengthOffInnerEdge2 - lengthOffInnerEdge1 + D1 * _clearWay - D2 * _appDistanceFromThreshold) / (D1 - D2);
            if (distance < _clearWay || distance < _appDistanceFromThreshold)
                return false;
            return true;
        }

        private double LengthOffInnerEdge(double length, double divergence)
        {
            return length * divergence/ 100;
        }
    }
}
