using System;
using System.Collections.Generic;
using System.Windows;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Omega.TypeB.Enums;
using Aran.Panda.Common;
using Aran.Panda.Constants;
using ESRI.ArcGIS.Geometry;
using Point = Aran.Geometries.Point;

namespace Aran.Omega.TypeB.Models
{
    public class BalkedLanding : SurfaceBase
    {
        public BalkedLanding()
        {
            SurfaceType = SurfaceType.BalkedLanding;

            Aran.Omega.SettingsUI.SurfaceModel surfaceModel = CommonFunctions.GetSurfaceModel(SurfaceType);
            if (surfaceModel != null)
            {
                DefaultSymbol = surfaceModel.Symbol;
                SelectedSymbol = surfaceModel.SelectedSymbol;
            }
        }

        public double LengthOfInnerEdge { get; set; }
        public double DistanceFromTheshold { get; set; }
        public double Divergence { get; set; }
        public double Slope { get; set; }
        public PlaneParam PlaneParam { get; set; }

        public override void CreateReport()
        {
            _report = new List<ObstacleReport>();
            double obstacleElev = 0;
            var localObstaclePt = new Aran.Geometries.Point();
            double surfaceElevation = 0;
            string equation = "";

            //var minMaxPoint = TransForm.QueryCoords(Geo);
            //var ptMinGeo = GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin));
            //var ptMaxGeo = GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax));
            //var extent = new MultiPolygon { CommonFunctions.CreateExtent(ptMinGeo.X, ptMinGeo.Y, ptMaxGeo.X, ptMaxGeo.Y) };
            //List<VerticalStructure> vsList = GlobalParams.Database.GetVerticalStructureList(extent);

            var esriGeom = ConvertToEsriGeom.FromMultiPolygon(Geo);
            Topology.GeometryOperators geomOperators = new Topology.GeometryOperators();

            foreach (VerticalStructure vs in GlobalParams.AdhpObstacleList)
            {
                int partNumber = -1;
                bool isIntersect = false;
                double maxPenetrate = -10000;

                Geometry geom = null;
                var maxObstacleElev = 0;
                var exactVertex = new Aran.Geometries.Point();
                var geomType = ObstacleGeomType.Point;
                double x = 0, y = 0;


                foreach (VerticalStructurePart vsPart in vs.Part)
                {
                    partNumber++;
                    VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;

                    if (vs.GetGeom(partNumber) == null)
                        continue;
                    #region ElevatedPoint calculation

                    if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
                    {
                        var obstaclePt = (Aran.Geometries.Point)vs.GetGeom(partNumber);

                        if (Geo.IsPointInside(obstaclePt))
                        {
                            localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction + Math.PI, obstaclePt);
                            obstacleElev = ConverterToSI.Convert(horizontalProj.Location.Elevation, 0);
                            surfaceElevation = PlaneParam.GetZ(localObstaclePt);
                            double tmpPenetrate = obstacleElev - surfaceElevation;

                            if (tmpPenetrate > maxPenetrate)
                            {
                                x = localObstaclePt.X;
                                y = localObstaclePt.Y;
                                maxPenetrate = tmpPenetrate;
                                geom = obstaclePt;
                                geomType = ObstacleGeomType.Point;
                                equation = PlaneParam.CreateEquationStr(surfaceElevation);
                                isIntersect = true;
                            }
                        }

                    }
                    #endregion
                    //It must be look again.Maybe we can join this two surface type

                    #region Elevatedsurface calculation

                    else
                    {
                        Geometry extentPrj = vs.GetGeom(partNumber);
                        IGeometry extentEsriPrj = vs.GetEsriGeom(partNumber);

                        //var intersect = GlobalParams.GlobalParams.GeomOperators.Intersect(esriGeom, extentPrj);
                        var partGeometryChoice = VerticalStructurePartGeometryChoice.ElevatedSurface;
                        //var intersect = GlobalParams.GlobalParams.GeomOperators.Intersect(esriGeom, extentEsriPrj);

                        Aran.Geometries.Geometry intersect = null;
                        try
                        {

                            intersect = GlobalParams.GeomOperators.Intersect(extentPrj, Geo);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(vs.Name);
                        }

                        if (intersect != null && !intersect.IsEmpty)
                        {
                            var intersectPts = new MultiPoint();
                            if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedSurface)
                            {
                                if (intersect.Type == GeometryType.Polygon)
                                    intersectPts = (intersect as Aran.Geometries.Polygon).ToMultiPoint();
                                else if (intersect.Type == GeometryType.MultiPolygon)
                                    intersectPts = (intersect as Aran.Geometries.MultiPolygon).ToMultiPoint();

                                obstacleElev = ConverterToSI.Convert(horizontalProj.SurfaceExtent.Elevation, 0);
                            }
                            else
                            {
                                if (intersect.Type == GeometryType.LineString)
                                    intersectPts = (intersect as Aran.Geometries.LineString).ToMultiPoint();
                                else if (intersect.Type == GeometryType.MultiLineString)
                                    intersectPts = (intersect as Aran.Geometries.MultiLineString).ToMultiPoint();

                                partGeometryChoice = VerticalStructurePartGeometryChoice.ElevatedCurve;
                                obstacleElev = ConverterToSI.Convert(horizontalProj.LinearExtent.Elevation, 0);
                            }


                            foreach (Aran.Geometries.Point obstaclePt in intersectPts)
                            {
                                localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction + Math.PI, obstaclePt);

                                surfaceElevation = PlaneParam.GetZ(localObstaclePt);
                                double tmpPenetrate = obstacleElev - surfaceElevation;

                                if (tmpPenetrate > maxPenetrate)
                                {
                                    equation = PlaneParam.CreateEquationStr(surfaceElevation);
                                    x = localObstaclePt.X;
                                    y = localObstaclePt.Y;
                                    maxPenetrate = tmpPenetrate;
                                    exactVertex = obstaclePt;
                                    geom = extentPrj;
                                    geomType = partGeometryChoice == VerticalStructurePartGeometryChoice.ElevatedSurface
                                        ? ObstacleGeomType.Polygon
                                        : ObstacleGeomType.PolyLine;
                                    isIntersect = true;
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
                    obstacleReport.Elevation = Common.ConvertHeight(obstacleElev, RoundType.ToNearest);
                    obstacleReport.Penetrate = Common.ConvertHeight(maxPenetrate, RoundType.ToNearest);
                    obstacleReport.X = Common.ConvertDistance(x, RoundType.ToNearest);
                    obstacleReport.Y = Common.ConvertDistance(y, RoundType.ToNearest);
                    obstacleReport.Plane = equation;
                    obstacleReport.GeomPrj = geom;
                    obstacleReport.ExactVertexGeom = exactVertex;
                    obstacleReport.SurfaceElevation = maxObstacleElev - maxPenetrate;
                    obstacleReport.GeomType = geomType;
                    obstacleReport.VsType = vs.Type;

                    _report.Add(obstacleReport);
                }
            }

        }

        private IList<Info> _propList;
        public override IList<Info> PropertyList
        {
            get
            {
                _propList = new List<Info>();
                _propList.Add(new Info("Length of inner edge", Common.ConvertDistance(LengthOfInnerEdge, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                _propList.Add(new Info("Distance from threshold", Common.ConvertDistance(DistanceFromTheshold, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                _propList.Add(new Info("Divergence", Divergence.ToString(), "%"));
                _propList.Add(new Info("Slope", Slope.ToString(), "%"));
                return _propList;
            }
            set { _propList = value; }
        }

        public override PointPenetrateModel GetManualReport(Point obstaclePt)
        {
            if (Geo.IsPointInside(obstaclePt))
            {
                var localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction + Math.PI, obstaclePt);
                var obstacleElev = obstaclePt.Z ;
                double surfaceElevation = PlaneParam.GetZ(localObstaclePt);

                var result = new PointPenetrateModel();
                result.Surface = "Balked Landing";
                result.Elevation = obstaclePt.Z;
                result.Plane = PlaneParam.CreateEquationStr(surfaceElevation);
                result.Penetration = result.Penetration = Common.ConvertHeight(obstacleElev - surfaceElevation, RoundType.ToNearest);
                return result;
            }
            return null;
        }
    }
}