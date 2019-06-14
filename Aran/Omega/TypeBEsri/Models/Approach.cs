using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Omega.TypeBEsri.Enums;
using Aran.Panda.Common;
using ESRI.ArcGIS.Geometry;
using Point = Aran.Geometries.Point;

namespace Aran.Omega.TypeBEsri.Models
{
    public class Approach : SurfaceBase
    {
        private int _cuttingGeoHandle;
        private List<Plane> _allPlanes;

        public Approach()
        {
            SurfaceType = Aran.Panda.Constants.SurfaceType.Approach;
            Handles = new List<int>();
            SelectedHandles = new List<int>();
            Planes1 = new List<Plane>();
            Planes2 = new List<Plane>();

            var surfaceModel = CommonFunctions.GetSurfaceModel(SurfaceType);
            CuttingGeo1 = new MultiPolygon();
            CuttingGeo2 = new MultiPolygon();
            CuttingGeo1Planes = new List<GeoSlope>();
            CuttingGeo2Planes = new List<GeoSlope>();
      

            if (surfaceModel != null)
            {
                DefaultSymbol = surfaceModel.Symbol;
                SelectedSymbol = surfaceModel.SelectedSymbol;
                CuttingGeoSymbol = surfaceModel.SelectedSymbol;
            }
        }

        public List<int> Handles { get; set; }
        public List<int> SelectedHandles { get; set; }
        public List<Plane> Planes1 { get; set; }
        public List<Plane> Planes2 { get; set; }

        public Aran.Geometries.MultiPolygon Geo2 { get; set; }

        public bool SecondPlane { get; set; }

        public double LengthOfInnerEdge { get; set; }
        public double DistanceFromThreshold { get; set; }
        public double Divergence { get; set; }
        public double FirstSectionLength { get; set; }
        public double FirstSectionSlope { get; set; }
        public double SecondSectionLength { get; set; }
        public double SecondSectionSlope { get; set; }
        public double HorizontalSectionLength { get; set; }
        public double HorizontalSectionTotalLength { get; set; }
        public List<GeoSlope> CuttingGeo1Planes { get; set; }
        public List<GeoSlope> CuttingGeo2Planes { get; set; }
        public Aran.Geometries.MultiPolygon CuttingGeo1 { get; set; }
        public Aran.Geometries.MultiPolygon CuttingGeo2 { get; set; }
        public double FinalElevation { get; set; }

        public override void Draw(bool isSelected)
        {
            if (isSelected)
            {
                ClearSelected();
                foreach (var plane in Planes1)
                {
                    SelectedHandles.Add(GlobalParams.UI.DrawRing(plane.Geo, SelectedSymbol, true, false));
                }
                
                foreach (var plane in Planes2)
                {
                    SelectedHandles.Add(GlobalParams.UI.DrawRing(plane.Geo, SelectedSymbol,true,false));
                }
            }
            else
            {
                ClearDefault();
                //var cuttingGeoSymbol = new AranEnvironment.Symbols.FillSymbol();
                //cuttingGeoSymbol.Color = DefaultSymbol.Color;
                //cuttingGeoSymbol.Style = AranEnvironment.Symbols.eFillStyle.sfsNull;
                //cuttingGeoSymbol.Outline.Style = AranEnvironment.Symbols.eLineStyle.slsDashDot;
                //cuttingGeoSymbol.Outline.Width = 1;

                foreach (GeoSlope geoSlope in CuttingGeo1Planes)
                {
                    if (geoSlope.Geo != null)
                        Handles.Add(GlobalParams.UI.DrawMultiPolygon(geoSlope.Geo, DefaultSymbol));
                }
                foreach (GeoSlope geoSlope in CuttingGeo2Planes)
                {
                    if (geoSlope.Geo != null)
                         Handles.Add(GlobalParams.UI.DrawMultiPolygon(geoSlope.Geo, DefaultSymbol));
                }

                //if (CuttingGeo1!=null)
                //    Handles.Add(GlobalParams.UI.DrawMultiPolygon(CuttingGeo1,DefaultSymbol, true, false));
                //if (CuttingGeo2 != null)
                //    Handles.Add(GlobalParams.UI.DrawMultiPolygon(CuttingGeo2, DefaultSymbol, true, false));
            }

        }

        public override void ClearSelected()
        {
            foreach (var handle in SelectedHandles)
            {
                GlobalParams.UI.SafeDeleteGraphic(handle);
            }
            SelectedHandles.Clear();
        }

        public override void ClearDefault()
        {
            foreach (var handle in Handles)
            {
                GlobalParams.UI.SafeDeleteGraphic(handle);
            }
            Handles.Clear();
        }

        public override void ClearAll()
        {
            ClearSelected();
            ClearDefault();
            GlobalParams.UI.SafeDeleteGraphic(_cuttingGeoHandle);
        }

        public override void CreateReport()
        {
            _report = new List<ObstacleReport>();

            var minMaxPoint = TransForm.QueryCoords(Geo);

            //var ptMinGeo = GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin));
            //var ptMaxGeo = GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax));
            //var extent = new MultiPolygon { CommonFunctions.CreateExtent(ptMinGeo.X, ptMinGeo.Y, ptMaxGeo.X, ptMaxGeo.Y) };
            var esriGeo = ConvertToEsriGeom.FromMultiPolygon(Geo);
            //List<VerticalStructure> vsList = GlobalParams.Database.GetVerticalStructureList(extent);
            _allPlanes = new List<Plane>(Planes1);
            _allPlanes.AddRange(Planes2);

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
                            var obstaclePt = (Aran.Geometries.Point)vs.GetGeom(partNumber);
                            //GlobalParams.SpatialRefOperation.ToPrj<Aran.Geometries.Point>(horizontalProj.Location.Geo);

                            foreach (Plane plane in _allPlanes)
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
                        #endregion
                        #region ElevatedSurface

                        else
                        {
                            Geometry extentPrj = vs.GetGeom(partNumber);
                            //IGeometry extentEsriPrj = vs.GetEsriGeom(partNumber);
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
                            var intersect = GlobalParams.GeomOperators.Intersect(Geo, extentPrj);

                            if (intersect != null && !intersect.IsEmpty)
                            {
                                MultiPoint intersectPts = new MultiPoint();
                                if (intersect.Type == GeometryType.Polygon)
                                    intersectPts = (intersect as Aran.Geometries.Polygon).ToMultiPoint();
                                else if (intersect.Type == GeometryType.MultiPolygon)
                                    intersectPts = (intersect as Aran.Geometries.MultiPolygon).ToMultiPoint();
                                else if (intersect.Type == GeometryType.LineString)
                                    intersectPts = (intersect as Aran.Geometries.LineString).ToMultiPoint();
                                else if (intersect.Type == GeometryType.MultiLineString)
                                    intersectPts = (intersect as Aran.Geometries.MultiLineString).ToMultiPoint();

                                foreach (Aran.Geometries.Point obstaclePt in intersectPts)
                                {
                                    foreach (Plane plane in _allPlanes)
                                    {
                                        if (plane.Geo.IsPointInside(obstaclePt))
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
                                                geom = extentPrj;
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
                        }
                        #endregion
                    }

                    if (isIntersect)
                    {
                        if (maxPenetrate >= 0)
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
            }
            catch (Exception e)
            {
                throw new Exception("Error Create reporting Inner Transitional!");

            }
        }

        private IList<Info> _propList;
        
        public override IList<Info> PropertyList
        {
            get
            {
                _propList = new List<Info>();
                _propList.Add(new Info("Length of inner edge", Common.ConvertDistance(LengthOfInnerEdge, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                _propList.Add(new Info("Distance from threshold", Common.ConvertDistance(DistanceFromThreshold, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                _propList.Add(new Info("Divergence", Divergence.ToString(), "%"));
                _propList.Add(new Info("First section length", Common.ConvertDistance(FirstSectionLength, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                _propList.Add(new Info("First section slope", FirstSectionSlope.ToString(), "%"));
                if (SecondPlane)
                {
                    _propList.Add(new Info("Second section length", Common.ConvertDistance(SecondSectionLength, RoundType.ToNearest).ToString(),InitOmega.DistanceConverter.Unit));
                    _propList.Add(new Info("Second section slope", SecondSectionSlope.ToString(),"%"));
                    _propList.Add(new Info("Horizontal section length",Common.ConvertDistance(HorizontalSectionLength,RoundType.ToNearest).ToString(),InitOmega.DistanceConverter.Unit));
                    _propList.Add(new Info("Total length", Common.ConvertDistance(FirstSectionLength+SecondSectionLength+HorizontalSectionLength, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                }
                return _propList;
            }
            set { _propList = value; }
        }

        public override PointPenetrateModel GetManualReport(Point obstaclePt)
        {
            var localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction+Math.PI,
                        obstaclePt);
            var obstacleElev = obstaclePt.Z;
            IList<Plane> planes = Planes1;
            planes.Concat(Planes2);
            foreach (var plane in planes)
            {
                if (plane.Geo.IsPointInside(obstaclePt))
                {
                    //Get two point from strip which can create plane
                    //Then calculate  x1 and x2 distance from start point.

                    var surfaceElevation = plane.Param.GetZ(localObstaclePt);

                    var result = new PointPenetrateModel();
                    result.Surface = "Approach";
                    result.Elevation = Common.ConvertHeight(obstaclePt.Z, RoundType.ToNearest);
                    result.Plane = plane.Param.CreateEquationStr(surfaceElevation);
                    result.Penetration = Common.ConvertHeight(obstacleElev - surfaceElevation, RoundType.ToNearest);
                    return result;
                }
            }
            return null;
        }

        public override MultiPolygon GetCuttingGeometry()
        {
            var result = new MultiPolygon();

            foreach (Aran.Geometries.Polygon poly in CuttingGeo1)
            {
                result.Add(poly);
            }
            foreach (Aran.Geometries.Polygon poly in CuttingGeo2)
            {
                result.Add(poly);
            }

            //foreach (GeoSlope geoSlope in CuttingGeo1Planes)
            //{
            //    if (geoSlope.Geo!=null)
            //        GlobalParams.UI.DrawMultiPolygon(geoSlope.Geo, DefaultSymbol);
            //}
            //foreach (GeoSlope geoSlope in CuttingGeo2Planes)
            //{
            //    if (geoSlope.Geo != null)
            //        GlobalParams.UI.DrawMultiPolygon(geoSlope.Geo, DefaultSymbol);
            //}

            return result;
        }
    
    }

    public class GeoSlope 
    {
        public Aran.Geometries.MultiPolygon Geo { get; set; }
        public double Slope { get; set; }
    
    }

}
