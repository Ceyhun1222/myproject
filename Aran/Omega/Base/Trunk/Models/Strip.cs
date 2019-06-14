using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.AranEnvironment.Symbols;
using Aran.Converters;
using Aran.Geometries;
using Aran.Omega.Enums;
using Aran.Omega.Strategy.ObstacleCalculation;
using Aran.Omega.Strategy.UI;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using ESRI.ArcGIS.Geometry;
using Point = Aran.Geometries.Point;
using Polygon = Aran.Geometries.Polygon;

namespace Aran.Omega.Models
{
    public class Strip : SurfaceBase, IMultiplePlane
    {
        public Strip()
        {
            LeftPts = new MultiPoint();
            RightPts = new MultiPoint();
            Planes = new List<Plane>();
            SurfaceType = Aran.PANDA.Constants.SurfaceType.Strip;
            Height = 0;
        }

        public MultiPoint LeftPts { get; set; }

        public MultiPoint RightPts { get; set; }


        public List<int> SelectedStripHandles { get; private set; }
        public List<int> DefaultStripHandles { get; private set; }

        public double LengthOfInnerEdge { get; set; }
        public double Height { get; set; }

        private IList<Info> _propList = new List<Info>();
        public override IList<Info> PropertyList
        {
            get {
                _propList.Clear();
                _propList.Add(new Info("Length of inner edge",Common.ConvertDistance(LengthOfInnerEdge,RoundType.ToNearest).ToString(),InitOmega.DistanceConverter.Unit));
                return _propList;
            }
            set => _propList = value;
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
        //public override void CreateReport()
        //{
        //    _report = new MtObservableCollection<ObstacleReport>();

        //    var mltExtent = (from Plane plane in Planes
        //                     from Aran.Geometries.Point pt in plane.Geo
        //                     select pt).ToMultiPoint();

        //    var minMaxPoint = Aran.Omega.Models.TransForm.QueryCoords(mltExtent);

        //    var ptMin = new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin);
        //    var ptMax = new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax);
        //    var extent = new MultiPolygon { CommonFunctions.CreateExtent(ptMin.X, ptMin.Y, ptMax.X, ptMax.Y) };
        //    var extentJtsGeo = Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(extent);

        //    int i = 0;
        //    try
        //    {
        //        //foreach (VerticalStructure vs in GlobalParams.AdhpObstacleList)
        //        //{
        //        Parallel.ForEach(GlobalParams.AdhpObstacleList, (vs) =>
        //        {
        //            i++;
        //            int partNumber = -1;
        //            VerticalStructurePart penetratedPart = null;
        //            var geomType = ObstacleGeomType.Point;
        //            double X = 0, Y = 0;
        //            double horAccuracy = 0, verAccuracy = 0;
        //            double obstacleElev = 0;


        //            Geometry geom = null, bufferGeom = null;
        //            double maxObstacleElev = 0;
        //            var exactVertex = new Point();
        //            string equation = "";

        //            foreach (VerticalStructurePart vsPart in vs.Part)
        //            {
        //                bool isIntersect = false;
        //                double maxPenetrate = -10000;

        //                partNumber++;
        //                VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;

        //                if (vs.GetGeom(partNumber) == null || vs.GetGeom(partNumber).IsEmpty)
        //                    continue;

        //                var jtsBuffer = vs.GetJtsBuffer(partNumber);
        //                double tmpVerAccuracy = Area2A.VerticalBufferWidth,
        //                    tmpHorAccuracy = Area2A.HorizontalBufferWidth;
        //                CommonFunctions.GetVerticalHorizontalAccuracy(horizontalProj, ref tmpVerAccuracy,
        //                    ref tmpHorAccuracy);

        //                if (jtsBuffer == null)
        //                    jtsBuffer = vs.GetJtsGeom(partNumber).Buffer(tmpHorAccuracy);

        //                var vsPartPrj = vs.GetGeom(partNumber);

        //                if (jtsBuffer != null && (!extentJtsGeo.Disjoint(jtsBuffer)))
        //                {
        //                    foreach (Plane plane in Planes)
        //                    {
        //                        var planeVsInersectJts = plane.JtsGeo?.Intersection(jtsBuffer);
        //                        if (planeVsInersectJts == null || planeVsInersectJts.IsEmpty)
        //                            continue;

        //                        var planeVsInersect = Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(planeVsInersectJts);
        //                        var intersectPts = planeVsInersect?.ToMultiPoint();

        //                        obstacleElev = vs.GetElevation(partNumber);
        //                        if (intersectPts != null)
        //                        {
        //                            foreach (Aran.Geometries.Point obstaclePt in intersectPts)
        //                            {

        //                                //Get two point from strip which can create plane
        //                                //Then calculate  x1 and x2 distance from start point.
        //                                var localObstaclePt = ARANFunctions.PrjToLocal(StartPoint,
        //                                Direction + Math.PI,
        //                                obstaclePt);

        //                                var surfaceElevation = plane.Param.GetZ(localObstaclePt);
        //                                double tmpPenetrate = obstacleElev + tmpVerAccuracy - surfaceElevation;
        //                                if (tmpPenetrate > maxPenetrate)
        //                                {
        //                                    equation = plane.Param.CreateEquationStr(surfaceElevation);
        //                                    X = localObstaclePt.X;
        //                                    Y = localObstaclePt.Y;
        //                                    maxPenetrate = tmpPenetrate;
        //                                    maxObstacleElev = obstacleElev;
        //                                    geom = vsPartPrj;
        //                                    bufferGeom = new MultiPolygon
        //                                {
        //                                        Aran.Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(jtsBuffer) as Polygon
        //                                };
        //                                    exactVertex = obstaclePt;
        //                                    verAccuracy = tmpVerAccuracy;
        //                                    horAccuracy = tmpHorAccuracy;
        //                                    geomType = CommonFunctions.GetGeomType(vsPartPrj);
        //                                    isIntersect = true;
        //                                    penetratedPart = vsPart;
        //                                }

        //                            }
        //                        }
        //                    }
        //                }

        //                if (isIntersect)
        //                {
        //                    var obstacleReport = new ObstacleReport(SurfaceType)
        //                    {
        //                        Id = vs.Id,
        //                        Name = vs.Name,
        //                        Obstacle = vs,
        //                        Plane = equation,
        //                        Elevation = Common.ConvertHeight(obstacleElev, RoundType.ToUp),
        //                        Penetrate = Common.ConvertHeight(maxPenetrate, RoundType.ToUp),
        //                        X = Common.ConvertDistance(X, RoundType.ToNearest),
        //                        Y = Common.ConvertDistance(Y, RoundType.ToNearest),
        //                        HorizontalAccuracy = horAccuracy,
        //                        VerticalAccuracy = verAccuracy,
        //                        GeomPrj = geom,
        //                        BufferPrj = bufferGeom,
        //                        ExactVertexGeom = exactVertex,
        //                        SurfaceElevation = maxObstacleElev - maxPenetrate,
        //                        GeomType = geomType,
        //                        VsType = vs.Type,
        //                        Part = penetratedPart,
        //                    };
        //                    lock (_lockObject)
        //                    {
        //                        _report.Add(obstacleReport);
        //                    }
        //                }
        //            }

        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Error Create reporting Strip!");

        //    }
        //}
    }
}
