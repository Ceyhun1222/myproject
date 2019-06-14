using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Omega.Enums;
using Aran.Omega.Strategy.ObstacleCalculation;
using Aran.Omega.Strategy.UI;
using Aran.PANDA.Common;

namespace Aran.Omega.Models
{
    public class Area4:SurfaceBase,IMultiplePlane
    {
        public const double VerticalBufferWidth = 1;
        public const double HorizontalBufferWidth = 2.5;

        public Area4()
        {
            Planes = new List<Plane>();

            EtodSurfaceType = PANDA.Constants.EtodSurfaceType.Area4;
            SurfaceType = PANDA.Constants.SurfaceType.InnerHorizontal;
        }

        public override void CreateReport()
        {
            _report = new MtObservableCollection<ObstacleReport>();

            var mltExtent = (from Plane plane in Planes
                             from Aran.Geometries.Point pt in plane.Geo
                             select pt).ToMultiPoint();

            var minMaxPoint = Aran.Omega.Models.TransForm.QueryCoords(mltExtent);

            var ptMin = new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin);
            var ptMax = new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax);
            var extent = new MultiPolygon { CommonFunctions.CreateExtent(ptMin.X, ptMin.Y, ptMax.X, ptMax.Y) };
            var extentJtsGeo = Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(extent);

            int i = 0;
            try
            {
                foreach (VerticalStructure vs in GlobalParams.AdhpObstacleList)
                {
                    i++;
                    int partNumber = -1;
                    VerticalStructurePart penetratedPart = null;
                    var geomType = ObstacleGeomType.Point;
                    double X = 0, Y = 0;
                    double horAccuracy = 0, verAccuracy = 0;
                    double obstacleElev = 0;
                    bool isIntersect = false;
                    double maxPenetrate = -10000;

                    Geometry geom = null, bufferGeom = null;
                    double maxObstacleElev = 0;
                    var exactVertex = new Point();
                    string equation = "";

                    foreach (VerticalStructurePart vsPart in vs.Part)
                    {
                        partNumber++;
                        VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;

                        if (vs.GetGeom(partNumber) == null || vs.GetGeom(partNumber).IsEmpty)
                            continue;

                        var jtsBuffer = vs.GetJtsBuffer(partNumber);
                        double tmpVerAccuracy = Area4.VerticalBufferWidth, tmpHorAccuracy = Area4.HorizontalBufferWidth;
                        CommonFunctions.GetVerticalHorizontalAccuracy(horizontalProj, ref tmpVerAccuracy, ref tmpHorAccuracy);

                        if (jtsBuffer == null)
                            jtsBuffer = vs.GetJtsGeom(partNumber).Buffer(tmpHorAccuracy);

                        var vsPartPrj = vs.GetGeom(partNumber);

                        if (jtsBuffer != null && (!extentJtsGeo.Disjoint(jtsBuffer)))
                        {
                            foreach (Plane plane in Planes)
                            {
                                var planeVsInersectJts = plane.JtsGeo.Intersection(jtsBuffer);

                                if (planeVsInersectJts.IsEmpty)
                                    continue;

                                obstacleElev = vs.GetElevation(partNumber);

                                //Get two point from strip which can create plane
                                //Then calculate  x1 and x2 distance from start point.

                                var surfaceElevation = plane.Geo[0].Z;
                                double tmpPenetrate = obstacleElev + tmpVerAccuracy - surfaceElevation;
                                if (tmpPenetrate > maxPenetrate)
                                {
                                    var aranGeo = Aran.Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(jtsBuffer);
                                    if (aranGeo == null) continue;
                                    if (aranGeo.Type == GeometryType.Polygon)
                                        bufferGeom = new MultiPolygon { aranGeo as Polygon };
                                    else
                                        bufferGeom = aranGeo as MultiPolygon;

                                    equation = "Z="+Common.ConvertHeight(surfaceElevation,RoundType.ToNearest);
                                    maxPenetrate = tmpPenetrate;
                                    maxObstacleElev = obstacleElev;
                                    geom = vsPartPrj;
                                    verAccuracy = tmpVerAccuracy;
                                    horAccuracy = tmpHorAccuracy;
                                    if (vsPartPrj.Type == GeometryType.Point)
                                        geomType = ObstacleGeomType.Point;
                                    else if (vsPartPrj.Type == GeometryType.MultiPolygon)
                                        geomType = ObstacleGeomType.Polygon;
                                    else
                                        geomType = ObstacleGeomType.PolyLine;
                                    penetratedPart = vsPart;
                                    isIntersect = true;
                                }
                            }
                        }
                    }
                    if (isIntersect)
                    {
                        var obstacleReport = new ObstacleReport(EtodSurfaceType);
                        obstacleReport.Id = vs.Id;
                        obstacleReport.Name = vs.Name;
                        obstacleReport.Obstacle = vs;
                        obstacleReport.Plane = equation;
                        obstacleReport.Elevation = Common.ConvertHeight(obstacleElev, RoundType.ToUp);
                        obstacleReport.Penetrate = Common.ConvertHeight(maxPenetrate, RoundType.ToUp);
                        obstacleReport.HorizontalAccuracy = horAccuracy;
                        obstacleReport.VerticalAccuracy = verAccuracy;
                        obstacleReport.GeomPrj = geom;
                        obstacleReport.BufferPrj = bufferGeom;
                        obstacleReport.SurfaceElevation = maxObstacleElev - maxPenetrate;
                        obstacleReport.GeomType = geomType;
                        obstacleReport.VsType = vs.Type;
                        obstacleReport.Part = penetratedPart;

                        _report.Add(obstacleReport);

                    }
                }
            }
            catch (Exception e)
            {
                GlobalParams.AranEnvironment.GetLogger("Omega").Error(e);
                throw new Exception("Error Create reporting Area2B!");

            }
        }

        public double LenghtOfInnerEdge { get; set; }
        public double Length { get; set; }

        public override IList<Info> PropertyList
        {
            get
            {
                _propertyList  = new List<Info>();
                _propertyList.Add(new Info("Length of inner edge", Common.ConvertDistance(LenghtOfInnerEdge, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                _propertyList.Add(new Info("Length", Common.ConvertDistance(Length, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                return _propertyList;
            }
            set
            {
                _propertyList = value;
            }
        }

        public override PointPenetrateModel GetManualReport(Geometries.Point obstaclePt)
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
                    result.Elevation = Common.ConvertHeight(obstaclePt.Z, RoundType.ToNearest);
                    result.Plane = plane.Param.CreateEquationStr(surfaceElevation);
                    result.Penetration = Common.ConvertHeight(obstacleElev - surfaceElevation, RoundType.ToNearest);
                    return result;
                }
            }
            return null;

        }
    }
}
