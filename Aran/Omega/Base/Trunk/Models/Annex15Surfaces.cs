using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Aran.Aim.Features;
using Aran.PANDA.Common;
using Aran.Converters;
using Aran.Geometries;
using Aran.PANDA.Constants;
using Aran.Aim;
using Aran.Geometries.Operators;
using Aran.Queries;
using Point = Aran.Geometries.Point;
using System.Threading.Tasks;

namespace Aran.Omega.Models
{
    public class Annex15Surfaces
    {
        public const  double Area2BLength = 10000;
        public const double Area2BSlope = 1.2;

        public const double Area2CLength = 10000;
        public const double Area2CDivergence = 15;

        public const double Area2DRadius = 45000;
       
        public const double Area4LengthOfInnerEdge = 120;
        public const double Area4Length = 900;

        public const double Area3GuidanceLineBufferWidth = 50;
        public const double Area3RunwayBufferWidth = 90;

        private const double Area2AHeight = 3;

        private const double Area2BDivergence = 15;

        private readonly Aran.Geometries.Operators.IGeometryOperators _geomOperators;
        private readonly double _direction;
        private readonly double _axis;
        private Aran.AranEnvironment.IAranGraphics _ui;
        private readonly RwyDirClass _rwyDir;

        private readonly SpatialReferenceOperation _spatialRefOper;
        private Aran.PANDA.Constants.RunwayConstansList _runwayConstantList;
        private Area2A _strip;
        private RwyClass _selectedRwyClass;
        private Aran.Geometries.Point _startCntPrj;
        private List<RwyCenterlineClass> _rwyCntlnClassList;
        private Point _endCntPrj;
        private RunwayClassificationType _rwyClassifationType;
        private CategoryNumber _catNumber;
        private int _codeNumber;
        private double _lengthOfInnerEdge;

        #region -->Ctor
        public Annex15Surfaces(RwyClass rwyClass, RwyDirClass rwyDir, RunwayClassificationType rwyClassifactionType, CategoryNumber catNumber, int codeNumber,double lengthOffInnerEdge)
        {
            try
            {
                _selectedRwyClass = rwyClass;
                _rwyDir = rwyDir;
                _rwyClassifationType = rwyClassifactionType;
                _catNumber = catNumber;
                _codeNumber = codeNumber;
                _lengthOfInnerEdge = lengthOffInnerEdge;

                _runwayConstantList = GlobalParams.Constant.RunwayConstants;
            
                _spatialRefOper = GlobalParams.SpatialRefOperation;
                _geomOperators =GlobalParams. GeomOperators;

                ProfileLinePoint = new List<Point>();

                _startCntPrj = _spatialRefOper.ToPrj(rwyDir.StartCntlPt.Location.Geo);
                _endCntPrj = _spatialRefOper.ToPrj(rwyDir.EndCntlPt.Location.Geo);
                _startCntPrj = _spatialRefOper.ToPrj(rwyDir.StartCntlPt.Location.Geo);
                _startCntPrj.Z = ConverterToSI.Convert(rwyDir.StartCntlPt.Location.Elevation, 0);
                _endCntPrj = _spatialRefOper.ToPrj(rwyDir.EndCntlPt.Location.Geo);
                _endCntPrj.Z = ConverterToSI.Convert(rwyDir.EndCntlPt.Location.Elevation, 0);
                _rwyCntlnClassList = rwyDir.CenterLineClassList;

                _direction = Aran.PANDA.Common.ARANFunctions.ReturnAngleInRadians(_startCntPrj, _endCntPrj);

                _axis = _direction + Math.PI;
                _ui = GlobalParams.UI;
                

                CreateProfileLinePoint();
            
              //  System.Windows.MessageBox.Show(stopWatch.Elapsed.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        #endregion

        #region -->Property
        public Area2A Area2A { get; private set; }
        public Area2B Area2B { get; private set; }
        public Area2C Area2C { get; private set; }
        public Area2D Area2D { get; private set; }

        #endregion

        #region -->Methods

        private void CreateProfileLinePoint()
        {
            RunwayConstants tmpConstant = _runwayConstantList[SurfaceType.Approach, DimensionType.DistanceFromThreshold];
            double distanceFromThreshold = tmpConstant.GetValue(_rwyDir.SelectedClassification, _rwyDir.SelectedCategory, _selectedRwyClass.CodeNumber);

            var rwyDir2 = _selectedRwyClass.RwyDirClassList.FirstOrDefault(rwydir => rwydir != _rwyDir);

            if (rwyDir2 == null) throw new ArgumentNullException($"Runway direction is empty!");

            double maxDistanceFromThreshold = rwyDir2.ClearWay;
            
            maxDistanceFromThreshold =Math.Max(maxDistanceFromThreshold, distanceFromThreshold);

            Point startOffSet = ARANFunctions.LocalToPrj(_startCntPrj, _direction, -maxDistanceFromThreshold, 0);
            startOffSet.Z = _startCntPrj.Z+Area2AHeight;
            ProfileLinePoint.Add(startOffSet);

            foreach (RwyCenterlineClass rwyCnt in _rwyCntlnClassList)
            {
                Point tmpPtPrj = rwyCnt.PtPrj;
                tmpPtPrj.Z = rwyCnt.Elevation+Area2AHeight;
                ProfileLinePoint.Add(tmpPtPrj);
            }

            Point endOffSet = ARANFunctions.LocalToPrj(_endCntPrj, _direction,Math.Max(_rwyDir.ClearWay, distanceFromThreshold));
            endOffSet.Z = _endCntPrj.Z+Area2AHeight;
            ProfileLinePoint.Add(endOffSet);
        }

        public Area2A CreateArea2A()
        {
            Area2A = new Area2A()
                .BuildDrawing(new Strategy.UI.MultiPlaneDrawing())
                .BuildObstacleCalculation(new Strategy.ObstacleCalculation.MultiplePlaneSurfaceCalculation())
                .BuildStyles() as Area2A;
           
            var stripElem = new Ring();

            var rightBackPt = ARANFunctions.LocalToPrj(ProfileLinePoint[0], _direction, 0, -_lengthOfInnerEdge / 2);
            var leftBackPt = ARANFunctions.LocalToPrj(ProfileLinePoint[0], _direction, 0, _lengthOfInnerEdge / 2);

            leftBackPt.Z = rightBackPt.Z = ProfileLinePoint[0].Z;

            stripElem.Add(rightBackPt);
            stripElem.Add(leftBackPt);

            Aran.Geometries.Point profilinePt;
            for (int i = 1; i < ProfileLinePoint.Count; i++)
            {
                profilinePt = ProfileLinePoint[i];

                Point leftPt = ARANFunctions.LocalToPrj(profilinePt, _direction, 0, _lengthOfInnerEdge / 2);
                Point rightPt = ARANFunctions.LocalToPrj(profilinePt, _direction, 0, -_lengthOfInnerEdge / 2);
                leftPt.Z = rightPt.Z = profilinePt.Z;

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
                tmpPlane.JtsGeo =new NetTopologySuite.Geometries.Polygon(Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromRing(stripElem));
                Area2A.Planes.Add(tmpPlane);

                stripElem = new Ring();
                stripElem.Add(rightPt);
                stripElem.Add(leftPt);
            }

            var resultGeo = new MultiPolygon();
            foreach (var plane in Area2A.Planes)
            {
                var poly = new Polygon { ExteriorRing = plane.Geo };
                resultGeo.Add(poly);
            }

            resultGeo = GlobalParams.GeomOperators.SimplifyGeometry(resultGeo) as Aran.Geometries.MultiPolygon;

            Area2A.GeoPrj = resultGeo;
            Area2A.Direction = _direction;
            Area2A.StartPoint = _startCntPrj;
            Area2A.LengthOfInnerEdge = _lengthOfInnerEdge;
            return Area2A;
        }

        public Area2B CreateArea2B() 
        {
            var resultGeo = new MultiPolygon();
            Area2B = new Area2B()
                .BuildDrawing(new Strategy.UI.MultiPlaneDrawing())
                .BuildObstacleCalculation(new Strategy.ObstacleCalculation.MultiplePlaneSurfaceCalculation())
                .BuildStyles() as Area2B;

            for (int i = 0; i < 2; i++)
            {
                var pt = ProfileLinePoint[0];
                double length = -Area2BLength;
                if (i == 1)
                {
                    pt = ProfileLinePoint[ProfileLinePoint.Count - 1];
                    length = Area2BLength;
                }
                var pt1 = ARANFunctions.LocalToPrj(pt, _direction, 0, _lengthOfInnerEdge/2);
                var pt2 = ARANFunctions.LocalToPrj(pt, _direction, 0, -_lengthOfInnerEdge/2);
                pt1.Z = pt2.Z = pt.Z;

                double width = Area2BLength * 15 / 100 + _lengthOfInnerEdge / 2;
                var pt3 = ARANFunctions.LocalToPrj(pt, _direction, length, -width);
                var pt4 = ARANFunctions.LocalToPrj(pt, _direction, length, width);
                pt3.Z = pt4.Z = pt.Z + Area2BLength * Area2BSlope / 100;

                Aran.Geometries.Point planePt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt1);
                Aran.Geometries.Point planePt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt2);
                Aran.Geometries.Point planePt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, pt3);

                var geoPart1 = new Ring {pt1,pt2,pt3,pt4,pt1};

                PlaneParam planeParam = CommonFunctions.CalcPlaneParam(planePt1, planePt2, planePt3);
                Plane tmpPlane = new Plane();
                tmpPlane.Geo = geoPart1;
                tmpPlane.JtsGeo = new NetTopologySuite.Geometries.Polygon(Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromRing(geoPart1));
                tmpPlane.Param = planeParam;
                Area2B.Planes.Add(tmpPlane);

                resultGeo.Add(new Polygon{ExteriorRing=geoPart1});
            }
            Area2B.GeoPrj = resultGeo;
            Area2B.Direction = _direction;
            Area2B.StartPoint = _startCntPrj;
            Area2B.LengthOfInnerEdge = _lengthOfInnerEdge;
            Area2B.Divergence = Area2BDivergence;
            Area2B.Slope = Area2BSlope;
            Area2B.Length = Area2BLength;
            return Area2B;
        }

        public Area2C CreateArea2C()
        {
            Area2C = new Area2C()
                .BuildDrawing(new Strategy.UI.SinglePlaneDrawing())
                .BuildObstacleCalculation(new Strategy.ObstacleCalculation.MultiplePlaneSurfaceCalculation())
                .BuildStyles() as Area2C;

            double slope = Area2BSlope;
            var stripLeft = new Ring();
            var stripRight = new Ring();

            double lengthOfArea2C = _lengthOfInnerEdge / 2 + Area2CLength;

            var leftPt1 = ARANFunctions.LocalToPrj(ProfileLinePoint[0], _direction, 0, -_lengthOfInnerEdge / 2);
            var leftPt2 = ARANFunctions.LocalToPrj(ProfileLinePoint[0], _direction, 0, -lengthOfArea2C);

            double area2BWidth = Area2BLength * Area2CDivergence / 100+_lengthOfInnerEdge/2;

            double divergencyRad = Math.Atan(Area2CDivergence/100);

            var arcLeftPt = ARANFunctions.LocalToPrj(leftPt1, _direction + divergencyRad, -Area2CLength, 0);
            var arcLeft = ARANFunctions.CreateArcPrj(leftPt1, leftPt2, arcLeftPt, TurnDirection.CW);
            
            arcLeft.Insert(0, leftPt1);
            arcLeft.Add(leftPt1);

            for (int i = 1; i < arcLeft.Count - 1; i++)
                arcLeft[i].Z = ProfileLinePoint[0].Z+Area2BLength*Area2BSlope/100;

            Area2C.Planes.Add(new Plane
                        {
                            Geo = arcLeft,
                            CalcType = CalculationType.ByDistance,
                            RefGeometry = leftPt1,
                            JtsGeo  =new NetTopologySuite.Geometries.Polygon(Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromRing(arcLeft))
                        });

            var rightPt1 = ARANFunctions.LocalToPrj(ProfileLinePoint[0], _direction, 0, _lengthOfInnerEdge / 2);
            var rightPt2 = ARANFunctions.LocalToPrj(ProfileLinePoint[0], _direction, 0, lengthOfArea2C);

            var arcRightPt = ARANFunctions.LocalToPrj(rightPt1, _direction - divergencyRad, -Area2CLength,0);
            var arcRight = ARANFunctions.CreateArcPrj(rightPt1, rightPt2, arcRightPt, TurnDirection.CCW);
          
            arcRight.Insert(0, rightPt1);
            arcRight.Add(rightPt1);

            for (int i = 1; i < arcLeft.Count - 1; i++)
                arcRight[i].Z = ProfileLinePoint[0].Z + Area2BLength * Area2BSlope / 100;


            Area2C.Planes.Add(new Plane
                            {
                                Geo = arcRight,
                                CalcType = CalculationType.ByDistance,
                                RefGeometry = rightPt1,
                                JtsGeo  =new NetTopologySuite.Geometries.Polygon(Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromRing(arcRight))
                            });

            leftPt1.Z = rightPt1.Z = ProfileLinePoint[0].Z+3;
            leftPt2.Z = rightPt2.Z = ProfileLinePoint[0].Z+3 + Area2CLength * slope / 100;

            stripLeft.Add(leftPt1);
            stripLeft.Add(leftPt2);

            stripRight.Add(rightPt1);
            stripRight.Add(rightPt2);

            for (int i = 1; i < ProfileLinePoint.Count; i++)
            {
                var profilenePt = ProfileLinePoint[i];
                Point leftPt3 = ARANFunctions.LocalToPrj(profilenePt, _direction, 0, -_lengthOfInnerEdge / 2);
                Point leftPt4 = ARANFunctions.LocalToPrj(profilenePt, _direction, 0, -lengthOfArea2C);

                Point rightPt3 = ARANFunctions.LocalToPrj(profilenePt, _direction, 0, _lengthOfInnerEdge / 2);
                Point rightPt4 = ARANFunctions.LocalToPrj(profilenePt, _direction, 0, lengthOfArea2C);

                leftPt3.Z = rightPt3.Z = profilenePt.Z;
                leftPt4.Z = rightPt4.Z = profilenePt.Z + Area2CLength * slope / 100;

                stripLeft.Add(leftPt4);
                stripLeft.Add(leftPt3);

                stripRight.Add(rightPt4);
                stripRight.Add(rightPt3);

                //Create Plane for calculating Left
                Aran.Geometries.Point leftPlanePt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, stripLeft[0]);
                Aran.Geometries.Point leftPlanePt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, stripLeft[1]);
                Aran.Geometries.Point leftPlanePt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, stripLeft[2]);

                stripLeft.Add(stripLeft[0]);
                PlaneParam leftPlaneParam = CommonFunctions.CalcPlaneParam(leftPlanePt1, leftPlanePt2, leftPlanePt3);
                Plane tmpPlane = new Plane();
                tmpPlane.Geo = stripLeft;
                tmpPlane.JtsGeo =new NetTopologySuite.Geometries.Polygon( Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromRing(stripLeft));
                tmpPlane.Param = leftPlaneParam;
                Area2C.Planes.Add(tmpPlane);

                stripLeft = new Ring();
                stripLeft.Add(leftPt3);
                stripLeft.Add(leftPt4);

                //Create Plane for calculating Left
                Aran.Geometries.Point rightPlanePt1 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, stripRight[0]);
                Aran.Geometries.Point rightPlanePt2 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, stripRight[1]);
                Aran.Geometries.Point rightPlanePt3 = ARANFunctions.PrjToLocal(_startCntPrj, _axis, stripRight[2]);

                stripRight.Add(stripRight[0]);
                PlaneParam rightPlaneParam = CommonFunctions.CalcPlaneParam(rightPlanePt1, rightPlanePt2, rightPlanePt3);
                tmpPlane = new Plane();
                tmpPlane.Geo = stripRight;
                tmpPlane.JtsGeo =new NetTopologySuite.Geometries.Polygon( Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromRing(stripRight));
                tmpPlane.Param = rightPlaneParam;
                Area2C.Planes.Add(tmpPlane);

                stripRight = new Ring();
                stripRight.Add(rightPt3);
                stripRight.Add(rightPt4);
            }

            var arcTmpPt1 = ARANFunctions.LocalToPrj(stripLeft[0], _direction-divergencyRad, Area2CLength,0);
            var arc1 = ARANFunctions.CreateArcPrj(stripLeft[0], stripLeft[1], arcTmpPt1, TurnDirection.CCW);

            arc1.Insert(0, stripLeft[0]);
            arc1.Add(arc1[0]);

            for (int i = 1; i < arc1.Count-1; i++)
                arc1[i].Z = stripLeft[1].Z;
            
            Area2C.Planes.Add(new Plane
            {
                Geo = arc1,
                CalcType = CalculationType.ByDistance,
                RefGeometry = stripLeft[0],
                JtsGeo = new NetTopologySuite.Geometries.Polygon(Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromRing(arc1))
            });

            var arcTmpPt2 = ARANFunctions.LocalToPrj(stripRight[0], _direction+divergencyRad, Area2CLength, 0);
            var arc2 = ARANFunctions.CreateArcPrj(stripRight[0], stripRight[1], arcTmpPt2, TurnDirection.CW);

            arc2.Insert(0, stripRight[0]);
            arc2.Add(stripRight[0]);

            //foreach (Aran.Geometries.Point pt in arc2)
            //    pt.Z = stripRight[1].Z;

            for (int i = 1; i < arc2.Count-1; i++)
                arc2[i].Z = stripRight[1].Z;

            Area2C.Planes.Add(new Plane
            {
                Geo = arc2,
                CalcType = CalculationType.ByDistance,
                RefGeometry = stripRight[0],
                JtsGeo = new NetTopologySuite.Geometries.Polygon(Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromRing(arc2))
            });

            Geometry resultGeo =new MultiPolygon{new Polygon{ExteriorRing= Area2C.Planes[0].Geo}} ;
            for (int i = 1; i < Area2C.Planes.Count; i++)
                resultGeo = _geomOperators.UnionGeometry(resultGeo,new MultiPolygon{new Polygon{ExteriorRing= Area2C.Planes[i].Geo}});

            if (resultGeo.Type == GeometryType.Polygon)
                Area2C.GeoPrj = new MultiPolygon { resultGeo as Polygon };
            else
                Area2C.GeoPrj = resultGeo as MultiPolygon;
            Area2C.Direction = _direction;
            Area2C.StartPoint = _startCntPrj;
            Area2C.Length = Area2CLength;
            Area2C.Divergence = Area2CDivergence;
            return Area2C;
        }

        public Area2D CreateArea2D()
        {
            try
            {
                var tmaGeo = GlobalParams.Database.GetTma();

                var adhpPrj = _spatialRefOper.ToPrj(GlobalParams.Database.AirportHeliport.ARP.Geo);
                var circle = ARANFunctions.CreateCircleAsMultiPolyPrj(adhpPrj, Area2DRadius);

                var resultGeo = circle;
                if (tmaGeo != null)
                    resultGeo = _geomOperators.Intersect(circle, tmaGeo) as MultiPolygon;

                var unionGeo = _geomOperators.UnionGeometry(Area2B.GeoPrj, Area2C.GeoPrj);

                unionGeo = _geomOperators.UnionGeometry(unionGeo, Area2A.GeoPrj);
                var geo = _geomOperators.Difference(resultGeo, unionGeo);

                var area2D = new Area2D
                {
                    GeoPrj = geo as MultiPolygon,
                    StartPoint = _startCntPrj,
                    Direction = _direction,
                    Radius = Area2DRadius
                }.BuildDrawing(new Strategy.UI.SinglePlaneDrawing())
                .BuildObstacleCalculation(new Strategy.ObstacleCalculation.SingleSurfaceCalculation())
                .BuildStyles() as Area2D ;

                return area2D;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error happened when creating Area2D ");
                GlobalParams.AranEnvironment?.GetLogger("Omega").Error(ex, ex.Message);
                //throw;
            }
            return null;
        }

        public Area4 CreateArea4(bool isMountaines)
        {
            Area4 area4 = new Area4()
                .BuildDrawing(new Strategy.UI.MultiPlaneDrawing())
                .BuildObstacleCalculation(new Strategy.ObstacleCalculation.MultiplePlaneSurfaceCalculation())
                .BuildStyles() as Area4;

            var geo = new Aran.Geometries.MultiPolygon();
            foreach (var rwyDir in _selectedRwyClass.RwyDirClassList)
            {
                var startRwyDir1 = _spatialRefOper.ToPrj(rwyDir.StartCntlPt.Location.Geo);

                int tmpDir = 1;
                if (rwyDir == _rwyDir)
                    tmpDir = -1;

                double length = Area4Length;
                if (isMountaines)
                    length = 2000;

                var pt1 = ARANFunctions.LocalToPrj(startRwyDir1, _direction, 0, Area4LengthOfInnerEdge / 2);
                var pt2 = ARANFunctions.LocalToPrj(startRwyDir1, _direction, 0, -Area4LengthOfInnerEdge / 2);
                var pt3 = ARANFunctions.LocalToPrj(startRwyDir1, _direction, tmpDir * length, -Area4LengthOfInnerEdge / 2);
                var pt4 = ARANFunctions.LocalToPrj(startRwyDir1, _direction, tmpDir * length, Area4LengthOfInnerEdge / 2);

                pt1.Z = pt2.Z = pt3.Z = pt4.Z =ConverterToSI.Convert(rwyDir.StartCntlPt.Location.Elevation,0);

                var area4Geo = new Ring { pt1, pt2, pt3, pt4,pt1 };

                var tmpPlane = new Plane
                {
                    Geo = area4Geo,
                    JtsGeo =
                        new NetTopologySuite.Geometries.Polygon(Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromRing(area4Geo))
                };
                geo.Add(new Polygon { ExteriorRing = area4Geo });
                area4.Planes.Add(tmpPlane);
            }
            area4.GeoPrj = geo;
            area4.LenghtOfInnerEdge = Area4LengthOfInnerEdge;
            area4.Length = Area4Length;
            return area4;
        }

        public List<Aran.Geometries.Point> ProfileLinePoint { get; private set; }

        public Area3 CreateArea3()
        {
            try
            {
                var guidanceLineList = GlobalParams.Database.GetGuidanceLine();
                Geometry guidanceLineElementGeo = new MultiPolygon();
                Geometry resultGeo = new MultiPolygon();
                Geometry differenceUnion = new MultiPolygon();

                //Get GuidanceLine Geo
                if (guidanceLineList.Count == 0)
                {
                    MessageBox.Show(
                        "There are not any GuidanceLine!It is not possible to construct Area 3 due to absence of GuidanceLines",
                        "Omega Annex 15", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return null;
                }

                //var jtsGeometryOperators = GlobalParams.GeomOperators;// new JtsGeometryOperators();
                var jtsGeometryOperators = new JtsGeometryOperators();
                foreach (var guidanceLine in guidanceLineList)
                {
                    if (guidanceLine.Extent?.Geo == null)
                        continue;

                    var featRef = guidanceLine.ConnectedTaxiway[0].Feature;
                    if (featRef.FeatureType == null)
                        featRef.FeatureType = FeatureType.Taxiway;

                    var taxiWay = featRef.GetFeature() as Taxiway;

                    if (taxiWay == null) continue;

                    var guidanceLineGeom = _spatialRefOper.ToPrj(guidanceLine.Extent.Geo);

                    //for presentation
                    double width = 23;
                    if (taxiWay.Width != null)
                        width = ConverterToSI.Convert(taxiWay.Width, 0);

                    var tmpBuffer1 = jtsGeometryOperators.BufferWithCapStyle(guidanceLineGeom, width/2,
                        GeoAPI.Operation.Buffer.EndCapStyle.Round);
                    guidanceLineElementGeo = jtsGeometryOperators.UnionGeometry(tmpBuffer1, guidanceLineElementGeo);

                    var tmpBuffer2 = jtsGeometryOperators.BufferWithCapStyle(guidanceLineGeom,
                        width / 2 + Area3GuidanceLineBufferWidth, GeoAPI.Operation.Buffer.EndCapStyle.Round);
                    resultGeo = _geomOperators.UnionGeometry(resultGeo, tmpBuffer2);
                }

                if (resultGeo.IsEmpty) return null;

                differenceUnion = guidanceLineElementGeo;
                //End

                //Get apronelement geo
                var apronElementList = GlobalParams.Database.GetApronElementList();
                foreach (var appronElement in apronElementList)
                {
                    if (appronElement.Extent == null || appronElement.Extent.Geo == null) continue;

                    var geo = _spatialRefOper.ToPrj(appronElement.Extent.Geo);
                    var appronElementBuffer = _geomOperators.Buffer(geo, Area3GuidanceLineBufferWidth);
                    differenceUnion = _geomOperators.UnionGeometry(differenceUnion, geo);
                    //var difference = _geomOperators.Difference(appronElementBuffer, geo);
                    resultGeo = _geomOperators.UnionGeometry(resultGeo, appronElementBuffer);
                }
                //end

                //Get RwyElement geo
                var rwyLine = new LineString();
                for (int i = 1; i < ProfileLinePoint.Count - 1; i++)
                    rwyLine.Add(ProfileLinePoint[i]);

                var rwyBuffer = jtsGeometryOperators.BufferWithCapStyle(rwyLine, Area3RunwayBufferWidth,
                    GeoAPI.Operation.Buffer.EndCapStyle.Square);

                resultGeo = _geomOperators.UnionGeometry(resultGeo, rwyBuffer);

                var rwyElement = GlobalParams.Database.GetRunwayelement(_selectedRwyClass.Identifier);
                if (rwyElement?.Extent?.Geo != null)
                {
                    var rwyElementGeo = _spatialRefOper.ToPrj(rwyElement.Extent.Geo);
                    //end

                    differenceUnion = _geomOperators.UnionGeometry(differenceUnion, rwyElementGeo);
                    var rwyElementDifference = _geomOperators.Difference(rwyBuffer, rwyElementGeo);

                    resultGeo = _geomOperators.Difference(resultGeo, differenceUnion);
                }
                //GlobalParams.UI.DrawMultiPolygon(rwyBuffer as MultiPolygon, 1, Aran.AranEnvironment.Symbols.eFillStyle.sfsHorizontal, true, false);
                var area3 = new Area3()
                    .BuildDrawing(new Strategy.UI.SinglePlaneDrawing())
                    .BuildObstacleCalculation(new Strategy.ObstacleCalculation.MultiplePlaneSurfaceCalculation())
                    .BuildStyles() as Area3;

                if (resultGeo?.Type == GeometryType.Polygon)
                    area3.GeoPrj = new MultiPolygon { resultGeo as Aran.Geometries.Polygon };
                else
                    area3.GeoPrj = resultGeo as MultiPolygon;

                return area3;

            }

            catch (Exception)
            {
                MessageBox.Show("Error happened when creating Area3","Omega Annex 15",MessageBoxButton.OK,MessageBoxImage.Error);
                return null;
            }

            #endregion
        }

        private double MinCenterElevation()
        {
            var minCenter =_rwyCntlnClassList.Min(rwyCenter => rwyCenter.Elevation);
            return minCenter;
        }

        private void AssignElevationToGeo(MultiPolygon mlt,double z)
        {
            foreach (Aran.Geometries.Polygon poly in mlt)
            {
                foreach (Aran.Geometries.Point pt in poly.ExteriorRing)
                    pt.Z = z;
            }
        }
    }
}
