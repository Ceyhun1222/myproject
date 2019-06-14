using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Omega.TypeBEsri.Enums;
using Aran.Panda.Common;
using ESRI.ArcGIS.Geometry;

namespace Aran.Omega.TypeBEsri.Models
{
    public class InnerTransitional : SurfaceBase
    {
        public InnerTransitional()
        {
            Planes = new List<Plane>();
            PlanesHandle = new List<int>();
            SelectedPlanesHandle = new List<int>();
            SurfaceType = Aran.Panda.Constants.SurfaceType.InnerTransitional;

            var surfaceModel = CommonFunctions.GetSurfaceModel(SurfaceType);

            if (surfaceModel != null)
            {
                DefaultSymbol = surfaceModel.Symbol;
                SelectedSymbol = surfaceModel.SelectedSymbol;
            }
        }

        public double Slope { get; set; }
        public List<Plane> Planes { get; set; }
        public List<int> PlanesHandle { get; private set; }

        public List<int> SelectedPlanesHandle { get; private set; }

        public override void CreateReport()
        {
            _report = new List<ObstacleReport>();

            var minMaxPoint = TransForm.QueryCoords(Geo);

            //var ptMinGeo = GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin));
            //var ptMaxGeo = GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax));
            //var extent = new MultiPolygon { CommonFunctions.CreateExtent(ptMinGeo.X, ptMinGeo.Y, ptMaxGeo.X, ptMaxGeo.Y) };

            var ptMin = new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin);
            var ptMax = new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax);
            var extent = new MultiPolygon { CommonFunctions.CreateExtent(ptMin.X, ptMin.Y, ptMax.X, ptMax.Y) };
            var extentEsriGeo = ConvertToEsriGeom.FromMultiPolygon(extent);
            var extentPrj = GlobalParams.SpatialRefOperation.ToPrj(extent);
          //  List<VerticalStructure> vsList = GlobalParams.Database.GetVerticalStructureList(extent);

            Topology.GeometryOperators geomOperators = new Topology.GeometryOperators();
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
                    var exactVertex = new Aran.Geometries.Point();
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
                               // GlobalParams.SpatialRefOperation.ToPrj<Aran.Geometries.Point>(horizontalProj.Location.Geo);

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
                            #endregion
                        
                            #region ElevatedSurface

                        else
                        {
                            Geometry vsPartPrj = vs.GetGeom(partNumber);
                            //IGeometry vsPartEsriPrj = vs.GetEsriGeom(partNumber);
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
                            //var intersect = GlobalParams.GlobalParams.GeomOperators(extentEsriGeo, extentEsriPrj);
                            var intersect = GlobalParams.GeomOperators.Intersect(extent, vsPartPrj);

                            if (intersect != null && !intersect.IsEmpty)
                            {
                                foreach (Plane plane in Planes)
                                {
                                    var mltPolygon = new MultiPolygon {new Aran.Geometries.Polygon {ExteriorRing = plane.Geo}};
                                    //var planeVsInersect = GlobalParams.GlobalParams.GeomOperators(mltPolygon, vsPartPrj);
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
                throw new Exception("Error Create reporting Inner Transitional!");

            }
        }

        public override void Draw(bool isSelected)
        {
            if (isSelected)
            {
                ClearSelected();
                foreach (Plane tmpPlane in Planes)
                {
                    SelectedPlanesHandle.Add(GlobalParams.UI.DrawRing(tmpPlane.Geo, SelectedSymbol));
                }
            }
            else
            {
                ClearDefault();
                foreach (Plane tmpPlane in Planes)
                {
                    PlanesHandle.Add(GlobalParams.UI.DrawRing(tmpPlane.Geo, DefaultSymbol));
                }
            }
        }

        public override void ClearAll()
        {
            ClearSelected();
            ClearDefault();
        }

        public override void ClearSelected()
        {
            foreach (int planeHandle in SelectedPlanesHandle)
            {
                GlobalParams.UI.SafeDeleteGraphic(planeHandle);
            }

            SelectedPlanesHandle.Clear();
        }

        public override void ClearDefault()
        {
            foreach (int planeHandle in PlanesHandle)
            {
                GlobalParams.UI.SafeDeleteGraphic(planeHandle);
            }

            PlanesHandle.Clear();
        }

        public override IList<Info> PropertyList
        {
            get
            {
                return new List<Info> { new Info("Slope", Slope.ToString(), "%") };
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override PointPenetrateModel GetManualReport(Aran.Geometries.Point obstaclePt)
        {
            foreach (Plane plane in Planes)
            {
                if (plane.Geo.IsPointInside(obstaclePt))
                {
                    //Get two point from strip which can create plane
                    //Then calculate  x1 and x2 distance from start point.
                    var localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction + Math.PI,
                        obstaclePt);
                    var obstacleElev =obstaclePt.Z;
                    var surfaceElevation = plane.Param.GetZ(localObstaclePt);

                    var result = new PointPenetrateModel();
                    result.Surface = "Inner Transitional";
                    result.Elevation =Common.ConvertHeight(obstaclePt.Z,RoundType.ToNearest);
                    result.Plane = plane.Param.CreateEquationStr(surfaceElevation);
                    result.Penetration = Common.ConvertHeight(obstacleElev - surfaceElevation,RoundType.ToNearest);
                    return result;
                }
            }
            return null;
        }
    }
}
