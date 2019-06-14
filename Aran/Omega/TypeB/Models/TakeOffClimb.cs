using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Features;
using Aran.Aim;
using Aran.Omega;
using Aran.Omega.TypeB.Enums;
using Aran.Panda.Common;
using Aran.Converters;
using Aran.Geometries;
using ESRI.ArcGIS.Geometry;
using Point = Aran.Geometries.Point;

namespace Aran.Omega.TypeB.Models
{
    public class TakeOffClimb:SurfaceBase
    {
        private int _cuttingGeoHandle;
        private List<Plane> _allPlanes;

        public TakeOffClimb()
        {
            SurfaceType = Aran.Panda.Constants.SurfaceType.TakeOffClimb;
            Handles = new List<int>();
            SelectedHandles = new List<int>();
            Planes1 = new List<Plane>();
            Planes2 = new List<Plane>();
            CuttingGeo1 = new MultiPolygon();
            CuttingGeo2 = new MultiPolygon();

         

            Aran.Omega.SettingsUI.SurfaceModel surfaceModel = CommonFunctions.GetSurfaceModel(SurfaceType);
            if (surfaceModel != null)
            {
                DefaultSymbol = surfaceModel.Symbol;
                SelectedSymbol = surfaceModel.SelectedSymbol;
            }
        }
        //public PlaneParam PlaneParam { get; set; }

        public List<int> Handles { get; set; }
        public List<int> SelectedHandles { get; set; }
        public List<Plane> Planes1 { get; set; }
        public List<Plane> Planes2 { get; set; }
        public MultiPolygon Geo2 { get; set; }


        public double LengthOfInnerEdge { get; set; }
        public double DistanceFromThreshold { get; set; }
        public double Divergence { get; set; }
        public double FinalWidth { get; set; }
        public double Slope { get; set; }
        public Aran.Geometries.MultiPolygon CuttingGeo1 { get; set; }
        public Aran.Geometries.MultiPolygon CuttingGeo2 { get; set; }
        public Aran.Geometries.Point EndPoint { get; set; }

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
                    int partNumber=-1;
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
                            var obstaclePt = (Aran.Geometries.Point) vs.GetGeom(partNumber);
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

        //public override void CreateReport()
        //{
        //    _report = new List<ObstacleReport>();
        //    double X = 0, Y = 0;
        //    double maxPenetrate = 0, tmpPenetrate;
        //    bool isIntersect;
        //    double obstacleElev = 0;
        //    Aran.Geometries.Point localObstaclePt = new Aran.Geometries.Point();
        //    double Z = 0;

        //    var minMaxPoint = TransForm.QueryCoords(Geo);
        //    var ptMinGeo = GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin));
        //    var ptMaxGeo = GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax));
        //    var extent = new MultiPolygon { CommonFunctions.CreateExtent(ptMinGeo.X, ptMinGeo.Y, ptMaxGeo.X, ptMaxGeo.Y) };
        //    List<VerticalStructure> vsList = GlobalParams.Database.GetVerticalStructureList(extent);

        //    foreach (VerticalStructure vs in vsList)
        //    {
        //        isIntersect = false;
        //        foreach (VerticalStructurePart vsPart in vs.Part)
        //        {
        //            VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;

        //            #region ElevatedPoint calculation
        //            if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
        //            {
        //                Aran.Geometries.Point obstaclePt =
        //                    GlobalParams.SpatialRefOperation.ToPrj<Aran.Geometries.Point>(horizontalProj.Location.Geo);
        //                if (Geo.IsPointInside(obstaclePt))
        //                {
        //                    localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction + Math.PI, obstaclePt);

        //                    obstacleElev = ConverterToSI.Convert(horizontalProj.Location.Elevation, 0);

        //                    Z = PlaneParam.GetZ(localObstaclePt);
        //                    maxPenetrate = obstacleElev - Z;

        //                    ObstacleReport obstacleReport = new ObstacleReport(SurfaceType);
        //                    obstacleReport.Id = vs.Id;
        //                    obstacleReport.Name = vs.Name;
        //                    obstacleReport.Obstacle = vs;
        //                    obstacleReport.Plane = PlaneParam.CreateEquationStr(Z);
        //                    obstacleReport.Elevation = Common.ConvertHeight(obstacleElev, RoundType.ToNearest);
        //                    obstacleReport.Penetrate = Common.ConvertHeight(maxPenetrate, RoundType.ToNearest);
        //                    obstacleReport.X = Common.ConvertHeight(localObstaclePt.X, RoundType.ToNearest);
        //                    obstacleReport.Y = Common.ConvertHeight(localObstaclePt.Y, RoundType.ToNearest);
        //                    obstacleReport.SurfaceElevation = Z;
        //                    obstacleReport.GeomPrj = obstaclePt;
        //                    obstacleReport.GeomType = ObstacleGeomType.Point;
        //                    obstacleReport.VsType = vs.Type;
        //                    _report.Add(obstacleReport);

        //                }
        //            }
        //            #endregion
        //            //It must be look again.Maybe we can join this two surface type

        //            #region Elevatedsurface calculation

        //            else if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedSurface)
        //            {
        //                MultiPolygon extentPrj = GlobalParams.SpatialRefOperation.ToPrj(horizontalProj.SurfaceExtent.Geo);
        //                MultiPolygon intersect = GlobalParams.GlobalParams.GeomOperators(Geo, extentPrj) as MultiPolygon;

        //                maxPenetrate = -10000;
        //                isIntersect = false;
        //                obstacleElev = ConverterToSI.Convert(horizontalProj.SurfaceExtent.Elevation, 0);
        //                if (intersect != null && !intersect.IsEmpty)
        //                {
        //                    foreach (Aran.Geometries.Point obstaclePt in intersect.ToMultiPoint())
        //                    {
        //                        localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction + Math.PI, obstaclePt);

        //                        Z = PlaneParam.GetZ(localObstaclePt);
        //                        tmpPenetrate = obstacleElev - Z;

        //                        if (tmpPenetrate > maxPenetrate)
        //                        {
        //                            maxPenetrate = tmpPenetrate;
        //                            X = localObstaclePt.X;
        //                            Y = localObstaclePt.Y;
        //                        }
        //                    }

        //                    isIntersect = true;
        //                }

        //                if (isIntersect)
        //                {
        //                    ObstacleReport obstacleReport = new ObstacleReport(SurfaceType);
        //                    obstacleReport.Id = vs.Id;
        //                    obstacleReport.Name = vs.Name;
        //                    obstacleReport.Obstacle = vs;
        //                    obstacleReport.Elevation = Common.ConvertHeight(obstacleElev, RoundType.ToNearest);
        //                    obstacleReport.Penetrate = Common.ConvertHeight(maxPenetrate, RoundType.ToNearest);
        //                    obstacleReport.X = Common.ConvertHeight(X, RoundType.ToNearest);
        //                    obstacleReport.Y = Common.ConvertHeight(Y, RoundType.ToNearest);
        //                    obstacleReport.Plane = PlaneParam.CreateEquationStr(obstacleElev - maxPenetrate);
        //                    obstacleReport.SurfaceElevation = obstacleElev - maxPenetrate;
        //                    obstacleReport.GeomType = ObstacleGeomType.Polygon;
        //                    obstacleReport.GeomPrj = extentPrj;
        //                    obstacleReport.VsType = vs.Type;
        //                    _report.Add(obstacleReport);
        //                }
        //            }
        //            #endregion

        //            #region ElevatedCurve Calculation
        //            else if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedCurve)
        //            {
        //                MultiLineString linearExtent = GlobalParams.SpatialRefOperation.ToPrj(horizontalProj.LinearExtent.Geo);
        //                MultiPolygon intersect = GlobalParams.GlobalParams.GeomOperators(Geo, linearExtent) as MultiPolygon;
        //                isIntersect = false;
        //                if (intersect != null && !intersect.IsEmpty)
        //                {
        //                    obstacleElev = ConverterToSI.Convert(horizontalProj.LinearExtent.Elevation, 0);
        //                    foreach (Aran.Geometries.Point obstaclePt in intersect.ToMultiPoint())
        //                    {
        //                        localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction + Math.PI, obstaclePt);
        //                        Z = PlaneParam.GetZ(localObstaclePt);
        //                        tmpPenetrate = obstacleElev - Z;

        //                        if (tmpPenetrate > maxPenetrate)
        //                        {
        //                            maxPenetrate = tmpPenetrate;
        //                            X = localObstaclePt.X;
        //                            Y = localObstaclePt.Y;
        //                        }
        //                    }
        //                    isIntersect = true;
        //                }

        //                if (isIntersect)
        //                {
        //                    ObstacleReport obstacleReport = new ObstacleReport(SurfaceType);
        //                    obstacleReport.Id = vs.Id;
        //                    obstacleReport.Name = vs.Name;
        //                    obstacleReport.Obstacle = vs;
        //                    obstacleReport.Elevation = Common.ConvertHeight(obstacleElev, RoundType.ToNearest);
        //                    obstacleReport.Penetrate = Common.ConvertHeight(maxPenetrate, RoundType.ToNearest);
        //                    obstacleReport.X = Common.ConvertHeight(X, RoundType.ToNearest);
        //                    obstacleReport.Y = Common.ConvertHeight(Y, RoundType.ToNearest);
        //                    obstacleReport.Plane = PlaneParam.CreateEquationStr(obstacleElev - maxPenetrate);
        //                    obstacleReport.SurfaceElevation = obstacleElev - maxPenetrate;
        //                    obstacleReport.GeomPrj = linearExtent;
        //                    obstacleReport.GeomType = ObstacleGeomType.PolyLine;
        //                    obstacleReport.VsType = vs.Type;
        //                    _report.Add(obstacleReport);
        //                }
        //            }

        //            #endregion
        //        }

        //    }

        //}

        public override void Draw(bool isSelected)
        {
            if (isSelected)
            {
                ClearSelected();
                SelectedHandles.Add(GlobalParams.UI.DrawMultiPolygon(Geo, SelectedSymbol, true, false));

                SelectedHandles.Add(GlobalParams.UI.DrawMultiPolygon(Geo2, SelectedSymbol, true, false));
            }
            else
            {
                ClearDefault();
                if (CuttingGeo1 != null)
                    Handles.Add(GlobalParams.UI.DrawMultiPolygon(CuttingGeo1, DefaultSymbol,true,false));
                if (CuttingGeo2 != null)
                    Handles.Add(GlobalParams.UI.DrawMultiPolygon(CuttingGeo2, DefaultSymbol,true,false));
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

        public IList<Info> _propList { get; set; }

        public override IList<Info> PropertyList
        {
            get
            {
                _propList = new List<Info>();
                _propList.Add(new Info("Length of inner edge", Common.ConvertDistance(LengthOfInnerEdge, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                _propList.Add(new Info("Distance from threshold", Common.ConvertDistance(DistanceFromThreshold, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                _propList.Add(new Info("Divergence", Divergence.ToString(), "%"));
                _propList.Add(new Info("Final width", Common.ConvertDistance(FinalWidth, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                _propList.Add(new Info("Slope", Slope.ToString(), "%"));
                _propList.Add(new Info("Length", Common.ConvertDistance(Length, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                return _propList;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double Length { get; set; }

        public override PointPenetrateModel GetManualReport(Point obstaclePt)
        {
            var localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction+Math.PI,
                        obstaclePt);
            var obstacleElev = obstaclePt.Z;
            IList<Plane> planes = Planes1;
            planes = planes.Concat(Planes2).ToList();

            int x = 60;
            int y = -150;

            var z = 0.02 * localObstaclePt.X + 35.1 - 36.36 - 0.0107 * localObstaclePt.X + 0.143 * localObstaclePt.Y - 24.9 + 37.87;

            var a = 119 * 0.3048;

            foreach (var plane in planes)
            {
                if (plane.Geo.IsPointInside(obstaclePt))
                {
                    //Get two point from strip which can create plane
                    //Then calculate  x1 and x2 distance from start point.

                    var surfaceElevation = plane.Param.GetZ(localObstaclePt);

                    var result = new PointPenetrateModel();
                    result.Surface = "Take of climb";
                    result.Elevation = Common.ConvertHeight(obstaclePt.Z, RoundType.ToNearest);
                    result.Plane = plane.Param.CreateEquationStr(surfaceElevation);
                    result.Penetration = Common.ConvertHeight(obstacleElev - surfaceElevation, RoundType.ToNearest);
                    return result;
                }
            }

                   
            return null;
        }

        public void DrawCuttingGeo() 
        {
            _cuttingGeoHandle = GlobalParams.UI.DrawMultiPolygon(CuttingGeo1,DefaultSymbol,true,false);
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
            return result;
        }
    }
}
