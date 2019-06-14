using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Omega.Enums;
using Aran.Omega.Strategy.ObstacleCalculation;
using Aran.Omega.Strategy.UI;

namespace Aran.Omega.Models
{
    public class Area1:SurfaceBase
    {
        public Area1()
        {
            EtodSurfaceType = PANDA.Constants.EtodSurfaceType.Area1;
        }
        public const double VerticalBufferWidth = 30;
        public const double HorizontalBufferWidth = 50;

        public override void CreateReport()
        {
            _report = new MtObservableCollection<ObstacleReport>();
            int i = 0;

            GeoAPI.Geometries.IMultiPolygon firGeomPrj = null;
            if (GlobalParams.FirGeomPrj != null)
                firGeomPrj =Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(GlobalParams.FirGeomPrj);

            var vsList = GlobalParams.Database.GetVerticalStructureList();

            try
            {
                foreach (VerticalStructure vs in vsList)
                {
                    i++;
                    VerticalStructurePart penetratedPart =null;
                    var geomType = ObstacleGeomType.Point;
                    double horAccuracy = 0, verAccuracy = 0;
                    double obstacleHeight = 0;
                    double obstacleElevation = 0;
                    bool isIntersect = false;
                    double maxPenetrate = 0;

                    Geometry geom = null, bufferGeom = null;
                    double maxObstacleHeight = 0;
                    string equation = "";

                    foreach (VerticalStructurePart vsPart in vs.Part)
                    {

                        VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;
                        if (horizontalProj == null)
                            continue;

                        double tmpVerAccuracy = Area4.VerticalBufferWidth, tmpHorAccuracy = Area4.HorizontalBufferWidth;
                        CommonFunctions.GetVerticalHorizontalAccuracy(horizontalProj, ref tmpVerAccuracy, ref tmpHorAccuracy);

                        obstacleHeight = ConverterToSI.Convert(vsPart.VerticalExtent, 0);
                        tmpVerAccuracy = ConverterToSI.Convert(vsPart.VerticalExtentAccuracy, Area1.VerticalBufferWidth);
                        

                        if ((obstacleHeight + tmpVerAccuracy) < 100)
                            continue;

                        double tmpObstacleElev = 0;
                        Geometry partGeo = null;
                        if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
                        {
                            partGeo = horizontalProj.Location?.Geo;
                            tmpObstacleElev = ConverterToSI.Convert(horizontalProj.Location?.Elevation, 0);
                        }
                        else if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedSurface)
                        {
                            partGeo = horizontalProj.SurfaceExtent?.Geo;
                            tmpObstacleElev = ConverterToSI.Convert(horizontalProj.SurfaceExtent?.Elevation, 0);
                        }
                        else if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedCurve)
                        {
                            partGeo = horizontalProj.LinearExtent?.Geo;
                            tmpObstacleElev = ConverterToSI.Convert(horizontalProj.LinearExtent?.Elevation, 0);
                        }

                        if (partGeo == null)
                            continue;
                        if (partGeo.IsEmpty)
                        {
                            GlobalParams.AranEnvironment.GetLogger("Omega").Warn(vs.Name+"'s Part is empty!");
                            continue;
                        }

                        var partPrj = GlobalParams.SpatialRefOperation.ToPrj(partGeo);
                        var jtsGeo = Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromGeometry(partPrj);

                        if (jtsGeo == null || jtsGeo.IsEmpty)
                            continue;

                        var jtsBuffer = jtsGeo.Buffer(tmpHorAccuracy);

                        if (firGeomPrj != null)
                        {
                            if (firGeomPrj.Disjoint(jtsBuffer))
                                continue;
                        }

                        //Get two point from strip which can create plane
                        //Then calculate  x1 and x2 distance from start point.

                        double tmpPenetrate = obstacleHeight + tmpVerAccuracy - 100;
                        if (tmpPenetrate > maxPenetrate)
                        {
                            var aranGeo = Aran.Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(jtsBuffer);
                            if (aranGeo == null) continue;
                            if (aranGeo.Type == GeometryType.Polygon)
                                bufferGeom = new MultiPolygon { aranGeo as Polygon };
                            else
                                bufferGeom = aranGeo as MultiPolygon;

                            equation = "Z=" + Common.ConvertHeight(100, RoundType.ToNearest);
                            maxPenetrate = tmpPenetrate;
                            maxObstacleHeight = obstacleHeight;
                            obstacleElevation = Common.ConvertHeight(tmpObstacleElev,RoundType.ToNearest);
                            geom = partPrj;
                            verAccuracy = tmpVerAccuracy;
                            horAccuracy = tmpHorAccuracy;
                            if (partPrj.Type == GeometryType.Point)
                                geomType = ObstacleGeomType.Point;
                            else if (partPrj.Type == GeometryType.MultiPolygon)
                                geomType = ObstacleGeomType.Polygon;
                            else
                                geomType = ObstacleGeomType.PolyLine;
                            isIntersect = true;
                            penetratedPart = vsPart;
                        }
                    }
                    if (isIntersect)
                    {
                        var obstacleReport = new ObstacleReport(EtodSurfaceType);
                        obstacleReport.Id = vs.Id;
                        obstacleReport.Name = vs.Name;
                        obstacleReport.Obstacle = vs;
                        obstacleReport.Plane = equation;
                        obstacleReport.Height =Math.Abs(obstacleHeight) < 0.01?"-":Common.ConvertHeight(obstacleHeight, RoundType.ToUp).ToString();
                        obstacleReport.Elevation = Common.ConvertHeight(obstacleElevation, RoundType.ToUp); 
                        obstacleReport.Penetrate = Common.ConvertHeight(maxPenetrate, RoundType.ToUp);
                        obstacleReport.HorizontalAccuracy = horAccuracy;
                        obstacleReport.VerticalAccuracy = verAccuracy;
                        obstacleReport.GeomPrj = geom;
                        obstacleReport.BufferPrj = bufferGeom;
                        obstacleReport.SurfaceElevation = maxObstacleHeight - maxPenetrate;
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
                throw new Exception("Error Create reporting Area1!");

            }
        }

        public override IList<Info> PropertyList
        {
            get => new List<Info>();
            set {  }
        }

        public override PointPenetrateModel GetManualReport(Geometries.Point obstaclePt)
        {
            return null;
        }
    }
}
