using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Omega.Enums;
using Aran.Omega.Models;
using Aran.PANDA.Common;

namespace Aran.Omega.Strategy.ObstacleCalculation
{
    class SingleSurfaceCalculation : IObstacleCalculation
    {
        public MtObservableCollection<ObstacleReport> CalculateReport(SurfaceBase surface)
        {
            var concurentReportList = new BlockingCollection<ObstacleReport>();

            var extentJtsGeo = Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(surface.GeoPrj);
            try
            {
                Parallel.ForEach(GlobalParams.AdhpObstacleList, (vs) =>
                {
                    var obsReport = GetItem(vs, extentJtsGeo, surface);
                    if (obsReport != null)
                        concurentReportList.Add(obsReport);
                });
            }
            catch (AggregateException ex)
            {
                foreach (var innerEx in ex.InnerExceptions)
                    GlobalParams.Logger.Error(innerEx);
            }
            return new MtObservableCollection<ObstacleReport>(concurentReportList);
        }

        public MtObservableCollection<ObstacleReport> CalculateReportSync(SurfaceBase surface)
        {
            var concurentReportList = new BlockingCollection<ObstacleReport>();

            var extentJtsGeo = Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(surface.GeoPrj);
            try
            {
                foreach (var vs in GlobalParams.AdhpObstacleList)
                {
                    var obsReport = GetItem(vs, extentJtsGeo, surface);
                    if (obsReport != null)
                        concurentReportList.Add(obsReport);
                }
            }
            catch (AggregateException ex)
            {
                foreach (var innerEx in ex.InnerExceptions)
                    GlobalParams.Logger.Error(innerEx);
            }
            return new MtObservableCollection<ObstacleReport>(concurentReportList);
        }

        private ObstacleReport GetItem(VerticalStructure vs, GeoAPI.Geometries.IMultiPolygon extentJtsGeo, SurfaceBase surface)
        {
            double obstacleElev = 0;
            string equation = "";
            int partNumber = -1;

            VerticalStructurePart penetratedPart = null;

            double horAccuracy = 0, verAccuracy = 0;

            Geometry geom = null;
            var maxObstacleElev = 0.0;
            var exactVertex = new Aran.Geometries.Point();
            double x = 0, y = 0;
            var geomType = ObstacleGeomType.Point;
            Aran.Geometries.MultiPolygon bufferGeom = null;

            foreach (var vsPart in vs.Part)
            {
                bool isIntersect = false;
                double maxPenetrate = -10000;

                partNumber++;
                VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;

                Geometry extentPrj = vs.GetGeom(partNumber);
                if (extentPrj == null)
                    continue;

                var jtsBuffer = vs.GetJtsBuffer(partNumber);
                obstacleElev = vs.GetElevation(partNumber);

                double tmpVerAccuracy = Area2A.VerticalBufferWidth, tmpHorAccuracy = Area2A.HorizontalBufferWidth;
                CommonFunctions.GetVerticalHorizontalAccuracy(horizontalProj, ref tmpVerAccuracy, ref tmpHorAccuracy);

                if (jtsBuffer == null)
                    jtsBuffer = vs.GetJtsGeom(partNumber).Buffer(tmpHorAccuracy);

                if (jtsBuffer != null && (!extentJtsGeo.Disjoint(jtsBuffer)))
                {
                    var intersectJtsGeo = extentJtsGeo.Intersection(jtsBuffer);

                    if (intersectJtsGeo == null) continue;

                    var intersectGeo = Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(intersectJtsGeo);
                    var intersectPts = intersectGeo?.ToMultiPoint();
                    if (intersectPts == null) continue;

                    foreach (Aran.Geometries.Point obstaclePt in intersectPts)
                    {
                        var localObstaclePt = ARANFunctions.PrjToLocal(surface.StartPoint, surface.Direction + Math.PI, obstaclePt);

                        var surfaceElevation = surface.PlaneParam.GetZ(localObstaclePt);
                        double tmpPenetrate = obstacleElev - surfaceElevation + tmpVerAccuracy;

                        if (tmpPenetrate > maxPenetrate)
                        {
                            var aranGeo = Aran.Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(jtsBuffer);
                            if (aranGeo == null) continue;
                            if (aranGeo.Type == GeometryType.Polygon)
                                bufferGeom = new MultiPolygon { aranGeo as Polygon };
                            else
                                bufferGeom = aranGeo as MultiPolygon;

                            equation = surface.PlaneParam.CreateEquationStr(surfaceElevation);
                            x = localObstaclePt.X;
                            y = localObstaclePt.Y;
                            maxPenetrate = tmpPenetrate;
                            maxObstacleElev = obstacleElev;
                            exactVertex = obstaclePt;
                            verAccuracy = tmpVerAccuracy;
                            horAccuracy = tmpHorAccuracy;
                            geom = extentPrj;
                            geomType = CommonFunctions.GetGeomType(extentPrj);
                            penetratedPart = vsPart;
                            isIntersect = true;
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
                        Elevation = Common.ConvertHeight(obstacleElev, RoundType.ToUp),
                        Penetrate = Common.ConvertHeight(maxPenetrate, RoundType.ToUp),
                        X = Common.ConvertDistance(x, RoundType.ToNearest),
                        Y = Common.ConvertDistance(y, RoundType.ToNearest),
                        HorizontalAccuracy = horAccuracy,
                        VerticalAccuracy = verAccuracy,
                        Plane = equation,
                        GeomPrj = geom,
                        BufferPrj = bufferGeom,
                        ExactVertexGeom = exactVertex,
                        SurfaceElevation = maxObstacleElev - maxPenetrate,
                        GeomType = geomType,
                        VsType = vs.Type,
                        Part = penetratedPart
                    };
                    return obstacleReport;
                }
            }
            return null;
        }

    }
}
