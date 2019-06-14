using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aran.PANDA.Common;
using ChartTypeA.Utils;
using ESRI.ArcGIS.Geometry;
using PDM;
using Point = ESRI.ArcGIS.Geometry.Point;

namespace ChartTypeA.Models
{
    public class TakeOffClimb
    {
        private RwyDirWrapper _rwyDir;
        private readonly IPoint _endCntPrj;
        private double _direction, _axis;
        private IPoint _startCntPrj;
        private double _clearwayDirection;
        private const double WidthInPointOfOrigin =180;
        private const double LengthOfTakeOffWithOnePersent = 12000;
        private const double LengthOfTakeOff = 10000;
        private const double divergence = 12.5;
        private const double finalWidth = 1800;

        public TakeOffClimb(RwyDirWrapper rwyDir,double turnAngle=0)
        {
            _rwyDir = rwyDir;
            _clearwayDirection = rwyDir.Direction;
            _direction = rwyDir.Direction-turnAngle;
            _axis = _direction + Math.PI;
            _endCntPrj = rwyDir.EndPt;
            _startCntPrj = rwyDir.Threshold;
            Name = rwyDir.Name + "- TAKEOFF";
        }

        public Plane TakeOffPlane { get; set; }
        public double LengthOfInnerEdge { get; set; }
        public double DistanceFromThreshold { get; set; }
        public double Divergence { get; set; }
        public double FinalWidth { get; set; }
        public double Slope { get; set; }
        public double Length { get; set; }
        public double WidthPointOfOrigin { get; set; }
        public IGeometry Geo { get; set; }
        public IGeometry GeoForChart { get; set; }
        public IPoint StartPoint { get; set; }
        public IPoint EndPoint { get; set; }
        public string Name { get; set; }

        public double Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public List<ObstacleReport> Report
        {
            get { return Report1; }
            set { Report1 = value; }
        }

        public List<ObstacleReport> ObstacleUsedInChart { get; set; }
        public List<ObstacleReport> ShadowedObstacleList { get; set; }

        public void CreateReport()
        {
            Report1 = new List<ObstacleReport>();


            GlobalParams.ObstacleList = GlobalParams.DbModule.GetObstacleList;
            int j = 0;
            try
            {
                var horAccuracy = Common.DeConvertDistance(GlobalParams.TypeAChartParams.HorAccuracy);

                var verAccuracy = Common.DeConvertHeight(GlobalParams.TypeAChartParams.VerAccuracy);

                foreach (VerticalStructure vs in GlobalParams.ObstacleList)
                {
                    try
                    {

                        int partNumber = -1;
                        var geomType = ObstacleGeomType.Point;
                        double X = 0, Y = 0;
                        double obstacleElev = 0;
                        bool isIntersect = false;
                        double maxPenetrate = -10000;

                        IGeometry geom = null;
                        double maxObstacleElev = 0;
                        var exactVertex = new Point();
                        string equation = "";

                        vs.RebuildGeo();
                        j++;

                        foreach (VerticalStructurePart vsPart in vs.Parts)
                        {
                            partNumber++;
                            if (vsPart.Geo == null)
                                vsPart.RebuildGeo();

                            IGeometry partGeo = vsPart.Geo;

                            if (partGeo == null)
                                continue;

                            #region ElevatedPoint calculation

                            if (partGeo is IPoint)
                            {
                                var obstaclePt = GlobalParams.SpatialRefOperation.ToEsriPrj(partGeo) as IPoint;
                                //GlobalParams.SpatialRefOperation.ToPrj<Aran.Geometries.Point>(horizontalProj.Location.Geo);

                                var bufferGeo = (IGeometry)obstaclePt;
                                if (GlobalParams.TypeAChartParams.HorAccuracy > 0)
                                    bufferGeo = EsriFunctions.Buffer(obstaclePt, horAccuracy);

                                if (!EsriFunctions.Disjoint(Geo, bufferGeo))
                                {
                                    var intersectGeo = EsriFunctions.Intersect(Geo, bufferGeo) as IPointCollection;

                                    for (int i = 0; i < intersectGeo.PointCount; i++)
                                    {
                                        var vertexPt = (IPoint)intersectGeo.Point[i];

                                        var localObstaclePt = EsriFunctions.PrjToLocal(EndPoint, Direction,
                                            vertexPt);

                                        if (vsPart.Elev.HasValue)
                                            obstacleElev = vsPart.ConvertValueToMeter(vsPart.Elev.Value,
                                                vsPart.Elev_UOM.ToString());

                                        var surfaceElevation = TakeOffPlane.Param.GetZ(localObstaclePt);
                                        double tmpPenetrate = obstacleElev - surfaceElevation + verAccuracy;
                                        if (tmpPenetrate > maxPenetrate)
                                        {
                                            equation = TakeOffPlane.Param.CreateEquationStr(surfaceElevation);
                                            X = localObstaclePt.X;
                                            Y = localObstaclePt.Y;
                                            maxPenetrate = tmpPenetrate;
                                            maxObstacleElev = obstacleElev;
                                            geomType = ObstacleGeomType.Point;
                                            geom = obstaclePt;
                                            isIntersect = true;
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region ElevatedSurface

                            else
                            {
                                IGeometry extentPrj = GlobalParams.SpatialRefOperation.ToEsriPrj(partGeo);
                                //IGeometry extentEsriPrj = vs.GetEsriGeom(partNumber);
                                if (vsPart.Elev.HasValue)
                                    obstacleElev = EsriFunctions.FromHeightVerticalM(vsPart.Elev_UOM, vsPart.Elev.Value);

                                var intersectGeo = EsriFunctions.Intersect(Geo, extentPrj);

                                if (intersectGeo != null && !intersectGeo.IsEmpty)
                                {
                                    IPointCollection intersectPts = intersectGeo as IPointCollection;

                                    for (int i = 0; i < intersectPts.PointCount; i++)
                                    {
                                        var obstaclePt = intersectPts.Point[i];

                                        //Get two point from strip which can create plane
                                        //Then calculate  x1 and x2 distance from start point.
                                        var localObstaclePt = EsriFunctions.PrjToLocal(EndPoint,
                                            Direction,
                                            obstaclePt);

                                        var surfaceElevation = TakeOffPlane.Param.GetZ(localObstaclePt);
                                        double tmpPenetrate = obstacleElev - surfaceElevation;
                                        if (tmpPenetrate > maxPenetrate)
                                        {
                                            equation = TakeOffPlane.Param.CreateEquationStr(surfaceElevation);
                                            X = localObstaclePt.X;
                                            Y = localObstaclePt.Y;
                                            maxPenetrate = tmpPenetrate;
                                            maxObstacleElev = obstacleElev;
                                            geom = extentPrj;
                                            isIntersect = true;
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                        if (isIntersect && maxPenetrate >= 0)
                        {
                            var obstacleReport = new ObstacleReport();
                            //obstacleReport.Id =Mat vs.ID;
                            obstacleReport.Name = vs.Name;
                            obstacleReport.Obstacle = vs;
                            obstacleReport.Plane = equation;
                            obstacleReport.Elevation = Common.ConvertHeight(maxObstacleElev, roundType.toNearest);
                            obstacleReport.Penetrate = Common.ConvertHeight(maxPenetrate, roundType.toNearest);
                            obstacleReport.X = Common.ConvertDistance(X, roundType.toNearest);
                            obstacleReport.Y = Common.ConvertDistance(Y, roundType.toNearest);
                            obstacleReport.GeomPrj = geom;
                            obstacleReport.ExactVertexGeom = exactVertex;
                            obstacleReport.SurfaceElevation = maxObstacleElev - maxPenetrate;
                            obstacleReport.GeomType = geomType;
                            //  obstacleReport.IntersectGeom = intersectGeo;
                            obstacleReport.RwyDir = _rwyDir.Name;

                            Report1.Add(obstacleReport);
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                        continue;
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception("Error Create reporting Inner Transitional!");

            }
        }

        public void CreateTakeOffPlane(double slope)
        {
            IPointCollection takeOffGeo = new ESRI.ArcGIS.Geometry.RingClass();
            var planes = new List<Plane>();

            var length = LengthOfTakeOffWithOnePersent;

            if (Math.Abs(slope - 1.2) < 0.01)
                length = LengthOfTakeOff;

            var pt = _endCntPrj;
            var clearWay =Common.DeConvertDistance(_rwyDir.ClearWay);
            if (Math.Abs(clearWay) > 0.1)
            {
                pt = EsriFunctions.LocalToPrj(_endCntPrj, _clearwayDirection, clearWay, 0);
                pt.Z = _endCntPrj.Z;
            }
            //var pt = ProfileLinePoint[ProfileLinePoint.Count - 1];

            var pt1 = EsriFunctions.LocalToPrj(pt, _direction, 0, WidthInPointOfOrigin/2);
            var pt2 = EsriFunctions.LocalToPrj(pt, _direction, 0, -WidthInPointOfOrigin/2);
            pt1.Z = pt2.Z = pt.Z;

            double section1Length = ((finalWidth - WidthInPointOfOrigin)/2)*100/divergence;

            var pt3 = EsriFunctions.LocalToPrj(pt, _direction, section1Length, -finalWidth/2);
            pt3.Z = (section1Length*slope)/100 + pt.Z;

            takeOffGeo.AddPoint(pt1);
            takeOffGeo.AddPoint(pt2);
            takeOffGeo.AddPoint(pt3);

           

            var pt4 = EsriFunctions.LocalToPrj(pt, _direction, length, -finalWidth/2);
            var pt5 = EsriFunctions.LocalToPrj(pt, _direction, length, finalWidth/2);
            pt4.Z = pt5.Z = length*slope/100 + pt.Z;

            var pt6 = EsriFunctions.LocalToPrj(pt, _direction, section1Length, finalWidth/2);
            pt6.Z = pt3.Z;

            takeOffGeo.AddPoint(pt4);
            takeOffGeo.AddPoint(pt5);
            takeOffGeo.AddPoint(pt6);



            var planePt1 = EsriFunctions.PrjToLocal(_endCntPrj, Direction, pt1);
            var planePt2 = EsriFunctions.PrjToLocal(_endCntPrj, Direction, pt2);
            var planePt3 = EsriFunctions.PrjToLocal(_endCntPrj, Direction, pt3);
            var planePt5 = EsriFunctions.PrjToLocal(_endCntPrj, Direction, pt5);
            var planeParam = EsriFunctions.CalcPlaneParam(planePt1, planePt2, planePt5);


            IGeometryCollection resultGeo = new Polygon() as IGeometryCollection;

            if (!(takeOffGeo as IRing).IsExterior)
                (takeOffGeo as IRing).ReverseOrientation();
            resultGeo.AddGeometry((IGeometry) takeOffGeo); //{ new Polygon { ExteriorRing = takeOffGeo } };

            ITopologicalOperator2 pTopo = resultGeo as ITopologicalOperator2;
            pTopo.IsKnownSimple_2 = false;
            pTopo.Simplify();


            var clearWayGeo =GlobalParams.SpatialRefOperation.ToEsriPrj(RectancleCreater.CreateRectancle(_endCntPrj, _clearwayDirection, clearWay, WidthInPointOfOrigin));

            this.Geo =(IPolygon) resultGeo;
            this.ClearWayGeo = clearWayGeo;
            this.TakeOffPlane = new Plane { Param = planeParam }; ;
            this.Direction = _direction;
            this.StartPoint = _startCntPrj;
            this.EndPoint = _endCntPrj;
            this.LengthOfInnerEdge = WidthInPointOfOrigin;
            this.Divergence = divergence;
            this.Slope = slope;
            this.Length = length;
            this.FinalWidth = finalWidth;
            this.WidthPointOfOrigin = WidthInPointOfOrigin;
            this.Divergence = divergence;

            //  this.SurfaceType = SurfaceType.TakeOffFlihtPathArea;
            //return takeOffClimb;
        }

        public void CalculateObstacleInChart()
        {
            if (Report1 == null || Report1.Count==0) return;

            var sortedReportList = Report1.OrderBy(obs=>obs.X).ToList();

            ObstacleUsedInChart = new List<ObstacleReport>();
            ShadowedObstacleList = new List<ObstacleReport>();

            
            var prevObstacle = sortedReportList[0];
            ObstacleUsedInChart.Add(sortedReportList[0]);
            var clearWay =Common.DeConvertDistance(_rwyDir.ClearWay);
            for (int i = 1; i < sortedReportList.Count; i++)
            {
                var curObstacle = sortedReportList[i];
                if (curObstacle.Elevation > prevObstacle.Elevation)
                {
                    var distObstToClearway = Common.DeConvertDistance(curObstacle.X) - clearWay;
                    if (distObstToClearway <= 300)
                    {
                        ObstacleUsedInChart.Add(curObstacle);
                        prevObstacle = curObstacle;
                    }
                    else if (distObstToClearway > 300 && curObstacle.Penetrate > prevObstacle.Penetrate)
                    {
                        ObstacleUsedInChart.Add(curObstacle);
                        prevObstacle = curObstacle;
                    }
                    else
                    {
                        curObstacle.ShadowedBy = prevObstacle.Name;
                        ShadowedObstacleList.Add(curObstacle);
                    }
                }
                else
                {
                    curObstacle.ShadowedBy = prevObstacle.Name;
                    ShadowedObstacleList.Add(curObstacle);
                }
            }

            GeoForChart = CreateCutinGeometry();

        }

        private IGeometry CreateCutinGeometry()
        {
            if (ObstacleUsedInChart.Count == 0) return null;

            var distance = Common.DeConvertDistance(ObstacleUsedInChart.Max(obst => obst.X) + 
                _rwyDir.ClearWay) + 100;

            IPointCollection takeOffGeo = new ESRI.ArcGIS.Geometry.RingClass();

            var length = distance;

            //tmpConstant = _runwayConstantList[SurfaceType.TakeOffClimb, DimensionType.Slope];
            //double slope = tmpConstant.GetValue(classification, 0, _codeNumber);

            var pt = _endCntPrj;
            var _clearWay = Common.DeConvertDistance(_rwyDir.ClearWay);
            if (Math.Abs(_clearWay) > 0.1)
            {
                pt = EsriFunctions.LocalToPrj(_endCntPrj, _clearwayDirection, _clearWay, 0);
                pt.Z = _endCntPrj.Z;
            }
            double finalWidth = 1800;
            double divergence = 12.5;

            //var pt = ProfileLinePoint[ProfileLinePoint.Count - 1];

            var pt1 = EsriFunctions.LocalToPrj(pt, _direction, 0, WidthInPointOfOrigin / 2);
            var pt2 = EsriFunctions.LocalToPrj(pt, _direction, 0, -WidthInPointOfOrigin / 2);
            pt1.Z = pt2.Z = pt.Z;

            double section1Length = ((finalWidth - WidthInPointOfOrigin) / 2) * 100 / divergence;

            if (section1Length < length)
            {
                var pt3 = EsriFunctions.LocalToPrj(pt, _direction, section1Length, -finalWidth/2);
                pt3.Z = (section1Length*Slope)/100 + pt.Z;
                var pt4 = EsriFunctions.LocalToPrj(pt, _direction, section1Length, finalWidth/2);
                pt4.Z = pt3.Z;

                var pt5 = EsriFunctions.LocalToPrj(pt, _direction, length, -finalWidth / 2);
                var pt6 = EsriFunctions.LocalToPrj(pt, _direction, length, finalWidth / 2);
                pt4.Z = pt5.Z = length * Slope / 100 + pt.Z;

                takeOffGeo.AddPoint(pt6);
                takeOffGeo.AddPoint(pt4);
                takeOffGeo.AddPoint(pt1);
                takeOffGeo.AddPoint(pt2);
                takeOffGeo.AddPoint(pt3);
                takeOffGeo.AddPoint(pt5);

            }
            else
            {
                var width = (length * divergence) / 100 + WidthInPointOfOrigin / 2;
                var pt3 = EsriFunctions.LocalToPrj(pt, _direction, length, -width);
                pt3.Z = (section1Length * Slope) / 100 + pt.Z;
                var pt4 = EsriFunctions.LocalToPrj(pt, _direction, length, width);
                pt4.Z = pt3.Z;

                takeOffGeo.AddPoint(pt4);
                takeOffGeo.AddPoint(pt1);
                takeOffGeo.AddPoint(pt2);
                takeOffGeo.AddPoint(pt3);
            }

            IGeometryCollection resultGeo = new Polygon() as IGeometryCollection;

            //if (!(takeOffGeo as IRing).IsExterior)
            //    (takeOffGeo as IRing).ReverseOrientation();
            resultGeo.AddGeometry((IGeometry)takeOffGeo);
           // EsriFunctions.SimplifyGeometry((IGeometry)resultGeo);
            return (IGeometry)resultGeo;

        }

        public IGeometry ClearWayGeo { get; set; }
        public List<ObstacleReport> Report1 { get; set; }
    }

}
