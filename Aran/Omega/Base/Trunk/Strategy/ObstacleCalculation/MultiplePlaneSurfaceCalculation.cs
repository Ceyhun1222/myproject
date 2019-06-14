using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Omega.Enums;
using Aran.Omega.Models;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;

namespace Aran.Omega.Strategy.ObstacleCalculation
{
    class MultiplePlaneSurfaceCalculation : IObstacleCalculation
    {
        public MtObservableCollection<ObstacleReport> CalculateReport(SurfaceBase surface)
        {
            var report = new System.Collections.Concurrent.BlockingCollection<ObstacleReport>();

            var mltExtent = (from Plane plane in surface.Planes
                             from Aran.Geometries.Point pt in plane.Geo
                             select pt).ToMultiPoint();
            var minMaxPoint = Models.TransForm.QueryCoords(mltExtent);

            var ptMin = new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin);
            var ptMax = new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax);
            var extent = new MultiPolygon { CommonFunctions.CreateExtent(ptMin.X, ptMin.Y, ptMax.X, ptMax.Y) };
            var extentJtsGeo = Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(extent);

            try
            {
                Parallel.ForEach(GlobalParams.AdhpObstacleList, (vs) =>
                {
                    var obsReport = GetItem(vs,extentJtsGeo,surface);
                    if (obsReport != null)
                        report.Add(obsReport);
                });

                return new MtObservableCollection<ObstacleReport>(report);
            }
            catch (Exception e)
            {
                GlobalParams.Logger.Error(e);
                throw new Exception("Error happened when create report of Transitional surface!");
            }
        }

        public MtObservableCollection<ObstacleReport> CalculateReportSync(SurfaceBase surface)
        {
            var report = new List<ObstacleReport>();

            var mltExtent = (from Plane plane in surface.Planes
                             from Aran.Geometries.Point pt in plane.Geo
                             select pt).ToMultiPoint();
            var minMaxPoint = Models.TransForm.QueryCoords(mltExtent);

            var ptMin = new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin);
            var ptMax = new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax);
            var extent = new MultiPolygon { CommonFunctions.CreateExtent(ptMin.X, ptMin.Y, ptMax.X, ptMax.Y) };
            var extentJtsGeo = Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(extent);

            try
            {
                foreach (var vs in GlobalParams.AdhpObstacleList)
                {
                    var obsReport = GetItem(vs, extentJtsGeo, surface);
                    if (obsReport != null)
                        report.Add(obsReport);
                }
                return new MtObservableCollection<ObstacleReport>(report);
            }
            catch (Exception e)
            {
                GlobalParams.Logger.Error(e);
                throw new Exception("Error happened when create report of Transitional surface!");
            }
        }

        private ObstacleReport GetItem(VerticalStructure vs, GeoAPI.Geometries.IMultiPolygon extentJtsGeo, SurfaceBase surface)
        {
            int partNumber = -1;
            VerticalStructurePart penetratedPart = null;

            var geomType = ObstacleGeomType.Point;
            double X = 0, Y = 0;
            double obstacleElev = 0;

            Geometry geom = null;
            double maxObstacleElev = 0;
            var exactVertex = new Aran.Geometries.Point();
            string equation = "";
            double horAccuracy = 0, verAccuracy = 0;
            Aran.Geometries.MultiPolygon bufferGeom = null;

            foreach (var vsPart in vs.Part)
            {
                bool isIntersect = false;
                double maxPenetrate = -10000;

                partNumber++;
                VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;
                Geometry vsPartPrj = vs.GetGeom(partNumber);

                if (vsPartPrj == null)
                    continue;

                var jtsBuffer = vs.GetJtsBuffer(partNumber);
                obstacleElev = vs.GetElevation(partNumber);

                double tmpVerAccuracy = Area2A.VerticalBufferWidth, tmpHorAccuracy = Area2A.HorizontalBufferWidth;
                CommonFunctions.GetVerticalHorizontalAccuracy(horizontalProj, ref tmpVerAccuracy, ref tmpHorAccuracy);

                if (jtsBuffer == null)
                    jtsBuffer = vs.GetJtsGeom(partNumber).Buffer(tmpHorAccuracy);

                int k = 0;
                if (jtsBuffer != null && !jtsBuffer.Disjoint(extentJtsGeo))
                {
                    foreach (var plane in surface.Planes)
                    {
                        k++;

                        var intersectJtsGeo = jtsBuffer.Intersection(plane.JtsGeo);
                        if (intersectJtsGeo == null) continue;

                        var intersectGeo = Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(intersectJtsGeo);
                        var intersectPts = intersectGeo?.ToMultiPoint();
                        if (intersectPts == null) continue;

                        foreach (Aran.Geometries.Point obstaclePt in intersectPts)
                        {

                            //Get two point from strip which can create plane
                            //Then calculate  x1 and x2 distance from start point.
                            var localObstaclePt = ARANFunctions.PrjToLocal(surface.StartPoint,
                                surface.Direction + Math.PI,
                                obstaclePt);

                            var surfaceElevation = plane.Param.GetZ(localObstaclePt);
                            double tmpPenetrate = obstacleElev - surfaceElevation + tmpVerAccuracy;
                            if (tmpPenetrate > maxPenetrate)
                            {
                                var aranGeo = Aran.Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(jtsBuffer);
                                if (aranGeo == null) continue;
                                if (aranGeo.Type == GeometryType.Polygon)
                                    bufferGeom = new MultiPolygon { aranGeo as Polygon };
                                else
                                    bufferGeom = aranGeo as MultiPolygon;

                                equation = plane.Param.CreateEquationStr(surfaceElevation);
                                X = localObstaclePt.X;
                                Y = localObstaclePt.Y;
                                maxPenetrate = tmpPenetrate;
                                maxObstacleElev = obstacleElev;
                                geom = vsPartPrj;

                                exactVertex = obstaclePt;
                                geomType = CommonFunctions.GetGeomType(vsPartPrj);
                                horAccuracy = tmpHorAccuracy;
                                verAccuracy = tmpVerAccuracy;
                                isIntersect = true;
                                penetratedPart = vsPart;
                            }

                        }
                    }
                }

                if (isIntersect)
                {
                    var obstacleReport = new ObstacleReport(surface.SurfaceType)
                    {
                        Id = vs.Id,
                        Name = vs.Name,
                        Obstacle = vs,
                        Plane = equation,
                        Elevation = Common.ConvertHeight(obstacleElev, RoundType.ToUp),
                        Penetrate = Common.ConvertHeight(maxPenetrate, RoundType.ToUp),
                        X = Common.ConvertDistance(X, RoundType.ToNearest),
                        Y = Common.ConvertDistance(Y, RoundType.ToNearest),
                        GeomPrj = geom,
                        BufferPrj = bufferGeom,
                        ExactVertexGeom = exactVertex,
                        SurfaceElevation = maxObstacleElev - maxPenetrate,
                        GeomType = geomType,
                        VsType = vs.Type,
                        VerticalAccuracy = verAccuracy,
                        HorizontalAccuracy = horAccuracy,
                        Part = penetratedPart
                    };
                    return obstacleReport;
                }
            }
            return null;
        }
    }
}
