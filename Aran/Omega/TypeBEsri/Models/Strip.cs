using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.AranEnvironment.Symbols;
using Aran.Converters;
using Aran.Geometries;
using Aran.Omega.TypeBEsri.Enums;
using Aran.Panda.Common;
using Aran.Panda.Constants;
using ESRI.ArcGIS.Geometry;
using Point = Aran.Geometries.Point;
using Polygon = Aran.Geometries.Polygon;

namespace Aran.Omega.TypeBEsri.Models
{
    public class Strip : SurfaceBase
    {
        public Strip()
        {
            LeftPts = new MultiPoint();
            RightPts = new MultiPoint();
            Planes = new List<Plane>();
            SelectedStripHandles = new List<int>();
            DefaultStripHandles = new List<int>();
            SurfaceType = Aran.Panda.Constants.SurfaceType.Strip;
           
            var surfaceModel = CommonFunctions.GetSurfaceModel(SurfaceType);
            if (surfaceModel != null)
            {
                DefaultSymbol = surfaceModel.Symbol;
                SelectedSymbol = surfaceModel.SelectedSymbol;
            }

        }

        public MultiPoint LeftPts { get; set; }

        public MultiPoint RightPts { get; set; }

        public List<Plane> Planes { get; private set; }

        public List<int> SelectedStripHandles { get; private set; }
        public List<int> DefaultStripHandles { get; private set; }

        public double LengthOfInnerEdge { get; set; }

        public Aran.Geometries.MultiPolygon GeoForObstacleCalculating { get; set; }

        public override void Draw(bool isSelected)
        {
            if (isSelected)
            {
                ClearSelected();
                foreach (Plane strip in Planes)
                {
                    SelectedStripHandles.Add(GlobalParams.UI.DrawRing(strip.Geo, SelectedSymbol));
                }
            }
            else
            {
                ClearDefault();
                foreach (Plane strip in Planes)
                {
                    DefaultStripHandles.Add(GlobalParams.UI.DrawRing(strip.Geo, DefaultSymbol));
                }
            }
            
        }

        public override void ClearSelected()
        {
            foreach (int handle in SelectedStripHandles)
            {
                GlobalParams.UI.SafeDeleteGraphic(handle);
            }
            SelectedStripHandles.Clear();
        }

        public override void ClearAll()
        {
            ClearDefault();
            ClearSelected();
        }

        public override void ClearDefault()
        {
            foreach (int handle in DefaultStripHandles)
            {
                GlobalParams.UI.SafeDeleteGraphic(handle);
            }
            DefaultStripHandles.Clear();
        }

        public override void CreateReport()
        {
            _report = new List<ObstacleReport>();

            var mltExtent = (from Plane plane in Planes
                             from Aran.Geometries.Point pt in plane.Geo
                             select pt).ToMultiPoint();

            var minMaxPoint = TransForm.QueryCoords(mltExtent);
            //var ptMinGeo =
            //    GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin));
            //var ptMaxGeo =
            //    GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax));
            //var extent = new MultiPolygon { CommonFunctions.CreateExtent(ptMinGeo.X, ptMinGeo.Y, ptMaxGeo.X, ptMaxGeo.Y) };
            //List<VerticalStructure> vsList = GlobalParams.Database.GetVerticalStructureList(extent);

            var ptMin = new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin);
            var ptMax = new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax);
            var extent = new MultiPolygon { CommonFunctions.CreateExtent(ptMin.X, ptMin.Y, ptMax.X, ptMax.Y) };
            var extentEsriGeo = ConvertToEsriGeom.FromMultiPolygon(extent);

            try
            {
                foreach (VerticalStructure vs in GlobalParams.AdhpObstacleList)
                {
                    int partNumber = -1;
                    var geomType = ObstacleGeomType.Point;
                    double X = 0, Y = 0;
                    double obstacleElev = 0;
                    bool isIntersect = false;
                    double maxPenetrate = -10000;

                    Geometry geom = null;
                    double maxObstacleElev = 0;
                    var exactVertex = new Point();
                    string equation = "";

                    foreach (VerticalStructurePart vsPart in vs.Part)
                    {
                        partNumber++;
                        VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;

                        if (vs.GetGeom(partNumber) == null)
                            continue;

                        #region ElevatedPoint calculation

                        if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
                        {
                            Aran.Geometries.Point obstaclePt = (Aran.Geometries.Point)vs.GetGeom(partNumber);
                                //GlobalParams.SpatialRefOperation.ToPrj<Aran.Geometries.Point>(horizontalProj.Location.Geo);

                            if (extent.IsPointInside(obstaclePt))
                            {
                                foreach (Plane plane in Planes)
                                {
                                    if (plane.Geo.IsPointInside(obstaclePt))
                                    {
                                        //Get two point from strip which can create plane
                                        //Then calculate  x1 and x2 distance from start point.
                                        var localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction + Math.PI,
                                            obstaclePt);
                                        obstacleElev = ConverterToSI.Convert(horizontalProj.Location.Elevation, 0);

                                        var surfaceElevation = plane.Param.GetZ(localObstaclePt);
                                        double tmpPenetrate = obstacleElev - surfaceElevation;
                                        if (tmpPenetrate > maxPenetrate)
                                        {
                                            equation = plane.Param.CreateEquationStr(surfaceElevation);
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
                        }
                        else
                        {
                            Geometry vsPartPrj = vs.GetGeom(partNumber);
                            //IGeometry vsPartEsriPrj = vs.GetEsriGeom(partNumber);// GlobalParams.SpatialRefOperation.ToPrj(horizontalProj.LinearExtent.Geo);
                            
                            var partGeometryChoice = VerticalStructurePartGeometryChoice.ElevatedSurface;

                            if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedSurface)
                            {
                                obstacleElev = ConverterToSI.Convert(horizontalProj.SurfaceExtent.Elevation, 0);
                            }
                            else
                            {
                                partGeometryChoice = VerticalStructurePartGeometryChoice.ElevatedCurve;
                                obstacleElev = ConverterToSI.Convert(horizontalProj.LinearExtent.Elevation, 0);
                            }

                            //var intersect = GlobalParams.GeomOperators.Intersect(extentEsriGeo, vsPartEsriPrj);
                            var intersect = GlobalParams.GeomOperators.Intersect(extent, vsPartPrj);
                            if (intersect != null && !intersect.IsEmpty)
                            {
                                foreach (Plane plane in Planes)
                                {
                                    var mltPolygon = new MultiPolygon {new Polygon {ExteriorRing = plane.Geo}};
                                    var planeVsInersect = GlobalParams.GeomOperators.Intersect(mltPolygon, vsPartPrj);

                                    if (planeVsInersect.IsEmpty)
                                        continue;

                                    MultiPoint intersectPts = new MultiPoint();
                                    if (planeVsInersect.Type == GeometryType.Polygon)
                                        intersectPts = (planeVsInersect as Aran.Geometries.Polygon).ToMultiPoint();
                                    else if (planeVsInersect.Type == GeometryType.MultiPolygon)
                                        intersectPts = (planeVsInersect as Aran.Geometries.MultiPolygon).ToMultiPoint();
                                    else if (planeVsInersect.Type == GeometryType.LineString)
                                        intersectPts = (planeVsInersect as Aran.Geometries.LineString).ToMultiPoint();
                                    else if (planeVsInersect.Type == GeometryType.MultiLineString)
                                        intersectPts = (planeVsInersect as Aran.Geometries.MultiLineString).ToMultiPoint();

                                    foreach (Aran.Geometries.Point obstaclePt in intersectPts)
                                    {

                                        //Get two point from strip which can create plane
                                        //Then calculate  x1 and x2 distance from start point.
                                        var localObstaclePt = ARANFunctions.PrjToLocal(StartPoint,
                                            Direction + Math.PI,
                                            obstaclePt);

                                        var surfaceElevation = plane.Param.GetZ(localObstaclePt);
                                        double tmpPenetrate = obstacleElev - surfaceElevation;
                                        if (tmpPenetrate > maxPenetrate)
                                        {
                                            equation = plane.Param.CreateEquationStr(surfaceElevation);
                                            X = localObstaclePt.X;
                                            Y = localObstaclePt.Y;
                                            maxPenetrate = tmpPenetrate;
                                            maxObstacleElev = obstacleElev;
                                            geom = vsPartPrj;
                                            exactVertex = obstaclePt;
                                            geomType = partGeometryChoice ==
                                                       VerticalStructurePartGeometryChoice.ElevatedSurface
                                                ? ObstacleGeomType.Polygon
                                                : ObstacleGeomType.PolyLine;
                                            isIntersect = true;
                                        }

                                    }
                                }
                            }
                        }

                        #endregion
                    }
                    if (isIntersect)
                    {
                        var obstacleReport = new ObstacleReport(SurfaceType);
                        obstacleReport.Id = vs.Id;
                        obstacleReport.Name = vs.Name;
                        obstacleReport.Obstacle = vs;
                        obstacleReport.Plane = equation;
                        obstacleReport.Elevation = Common.ConvertHeight(obstacleElev, RoundType.ToNearest);
                        obstacleReport.Penetrate = Common.ConvertHeight(maxPenetrate, RoundType.ToNearest);
                        obstacleReport.X = Common.ConvertDistance(X, RoundType.ToNearest);
                        obstacleReport.Y = Common.ConvertDistance(Y, RoundType.ToNearest);
                        obstacleReport.GeomPrj = geom;
                        obstacleReport.ExactVertexGeom = exactVertex;
                        obstacleReport.SurfaceElevation = maxObstacleElev - maxPenetrate;
                        obstacleReport.GeomType = geomType;
                        obstacleReport.VsType = vs.Type;

                        _report.Add(obstacleReport);

                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error Create reporting Strip!");

            }
        }

        public override IList<Info> PropertyList
        {
            get { return new List<Info>{new Info("Length of inner edge",Common.ConvertDistance(LengthOfInnerEdge,RoundType.ToNearest).ToString(),InitOmega.DistanceConverter.Unit)}; }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override PointPenetrateModel GetManualReport(Point obstaclePt)
        {
            foreach (Plane plane in Planes)
            {
                if (plane.Geo.IsPointInside(obstaclePt))
                {
                    //Get two point from strip which can create plane
                    //Then calculate  x1 and x2 distance from start point.
                    var localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction + Math.PI,
                        obstaclePt);
                    var obstacleElev = obstaclePt.Z;

                    var surfaceElevation = plane.Param.GetZ(localObstaclePt);

                    var result = new PointPenetrateModel();
                    result.Surface = "Strip";
                    result.Elevation =Common.ConvertHeight(obstaclePt.Z,RoundType.ToNearest);
                    result.Plane = plane.Param.CreateEquationStr(surfaceElevation);
                    result.Penetration =Common.ConvertHeight(obstacleElev - surfaceElevation,RoundType.ToNearest);
                    return result;
                }
            }
            return null;
        }
    }
}
