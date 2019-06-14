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
    class ApproachObstacleCalculation : IObstacleCalculation
    {
        public MtObservableCollection<ObstacleReport> CalculateReport(SurfaceBase surface)
        {
            var approach = surface as Approach;

            if (approach == null)
                throw new ArgumentException("Input parametr must be Approach");

            var minMaxPoint = Models.TransForm.QueryCoords(approach.GeoPrj);
            var ptMinGeo = new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin);
            var ptMaxGeo = new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax);
            var extent = new MultiPolygon { CommonFunctions.CreateExtent(ptMinGeo.X, ptMinGeo.Y, ptMaxGeo.X, ptMaxGeo.Y) };
            //List<VerticalStructure> vsList = GlobalParams.Database.GetVerticalStructureList(extent);

            var extentApproachJtsGeo = Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(extent);


            var concurentReportList = new BlockingCollection<ObstacleReport>();

            Parallel.ForEach(GlobalParams.AdhpObstacleList, (vs) =>
            {
                var obsReport = GetItem(vs, extentApproachJtsGeo, approach);
                if (obsReport != null)
                    concurentReportList.Add(obsReport);
            });

            return new MtObservableCollection<ObstacleReport>(concurentReportList);
        }

        public MtObservableCollection<ObstacleReport> CalculateReportSync(SurfaceBase surface)
        {
            var approach = surface as Approach;

            if (approach == null)
                throw new ArgumentException("Input parametr must be Approach");

            var minMaxPoint = Models.TransForm.QueryCoords(approach.GeoPrj);
            var ptMinGeo = new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin);
            var ptMaxGeo = new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax);
            var extent = new MultiPolygon { CommonFunctions.CreateExtent(ptMinGeo.X, ptMinGeo.Y, ptMaxGeo.X, ptMaxGeo.Y) };
            //List<VerticalStructure> vsList = GlobalParams.Database.GetVerticalStructureList(extent);

            var extentApproachJtsGeo = Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(extent);


            var concurentReportList = new BlockingCollection<ObstacleReport>();

            foreach (var vs in GlobalParams.AdhpObstacleList)
            {
                var obsReport = GetItem(vs, extentApproachJtsGeo, approach);
                if (obsReport != null)
                    concurentReportList.Add(obsReport);
            };

            return new MtObservableCollection<ObstacleReport>(concurentReportList);
        }


        private ObstacleReport GetItem(VerticalStructure vs, GeoAPI.Geometries.IMultiPolygon extentApproachJtsGeo, Approach approach)
        {
            try
            {
                var equation = "";
                var partNumber = -1;
                VerticalStructurePart penetratedPart = null;

                Geometry geom = null;
                double maxObstacleElev = 0;
                var exactVertex = new Point();
                var geomType = ObstacleGeomType.Point;
                double x = 0, y = 0;
                double obstacleElev = 0;
                double horAccuracy = 0, verAccuracy = 0;
                Aran.Geometries.MultiPolygon bufferGeom = null;

                foreach (var vsPart in vs.Part)
                {
                    double maxPenetrate = -10000;
                    var isIntersect = false;
                    partNumber++;
                    if (vs.GetGeom(partNumber) == null)
                        continue;

                    VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;

                    var jtsBuffer = vs.GetJtsBuffer(partNumber);
                    Geometry extentPrj = vs.GetGeom(partNumber);
                    obstacleElev = vs.GetElevation(partNumber);

                    double tmpVerAccuracy = Area2A.VerticalBufferWidth, tmpHorAccuracy = Area2A.HorizontalBufferWidth;
                    CommonFunctions.GetVerticalHorizontalAccuracy(horizontalProj, ref tmpVerAccuracy, ref tmpHorAccuracy);

                    if (jtsBuffer == null)
                        jtsBuffer = vs.GetJtsGeom(partNumber).Buffer(tmpHorAccuracy);

                    isIntersect = false;

                    if (jtsBuffer != null && (!extentApproachJtsGeo.Disjoint(jtsBuffer)))
                    {
                        var intersectJtsGeo = extentApproachJtsGeo.Intersection(jtsBuffer);
                        if (intersectJtsGeo == null) continue;

                        var intersectGeo = Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(intersectJtsGeo);
                        var intersectPts = intersectGeo?.ToMultiPoint();
                        if (intersectPts == null) continue;

                        foreach (Aran.Geometries.Point obstaclePt in intersectPts)
                        {
                            double surfaceElevation = 0;
                            if (approach.Section1.Geo.IsPointInside(obstaclePt))
                            {
                                var localObstaclePt = ARANFunctions.PrjToLocal(approach.StartPoint, approach.Direction + Math.PI, obstaclePt);
                                surfaceElevation = approach.Section1.Param.GetZ(localObstaclePt);
                                var tmpPenetrate = obstacleElev - surfaceElevation + tmpVerAccuracy;

                                if (tmpPenetrate > maxPenetrate)
                                {
                                    var aranGeo = Aran.Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(jtsBuffer);
                                    if (aranGeo == null) continue;
                                    if (aranGeo.Type == GeometryType.Polygon)
                                        bufferGeom = new MultiPolygon { aranGeo as Polygon };
                                    else
                                        bufferGeom = aranGeo as MultiPolygon;

                                    equation = "I Sector:" + approach.Section1.Param.CreateEquationStr(surfaceElevation);
                                    x = localObstaclePt.X;
                                    y = localObstaclePt.Y;
                                    maxPenetrate = tmpPenetrate;
                                    maxObstacleElev = obstacleElev;
                                    verAccuracy = tmpVerAccuracy;
                                    horAccuracy = tmpHorAccuracy;
                                    geom = extentPrj;
                                    exactVertex = obstaclePt;
                                    geomType = CommonFunctions.GetGeomType(extentPrj);
                                    penetratedPart = vsPart;
                                }
                                isIntersect = true;
                            }

                            else if (approach.SecondPlane)
                            {
                                if (approach.Section2.Geo.IsPointInside(obstaclePt))
                                {
                                    var localObstaclePt = ARANFunctions.PrjToLocal(approach.StartPoint, approach.Direction + Math.PI, obstaclePt);

                                    surfaceElevation = approach.Section2.Param.GetZ(localObstaclePt);
                                    double tmpPenetrate = obstacleElev - surfaceElevation + tmpVerAccuracy;

                                    if (tmpPenetrate > maxPenetrate)
                                    {

                                        var aranGeo = Aran.Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(jtsBuffer);
                                        if (aranGeo == null) continue;
                                        if (aranGeo.Type == GeometryType.Polygon)
                                            bufferGeom = new MultiPolygon { aranGeo as Polygon };
                                        else
                                            bufferGeom = aranGeo as MultiPolygon;

                                        x = localObstaclePt.X;
                                        y = localObstaclePt.Y;
                                        maxPenetrate = tmpPenetrate;
                                        maxObstacleElev = obstacleElev;
                                        equation = "II Sector" +
                                                   approach.Section2.Param.CreateEquationStr(surfaceElevation);
                                        geom = extentPrj;
                                        exactVertex = obstaclePt;
                                        verAccuracy = tmpVerAccuracy;
                                        horAccuracy = tmpHorAccuracy;
                                        geomType = CommonFunctions.GetGeomType(extentPrj);
                                        penetratedPart = vsPart;
                                    }
                                    isIntersect = true;
                                }
                                else if (approach.Section3.Geo.IsPointInside(obstaclePt))
                                {
                                    double tmpPenetrate = obstacleElev - approach.Section3.Param.D + tmpVerAccuracy;

                                    if (tmpPenetrate > maxPenetrate)
                                    {
                                        var aranGeo = Aran.Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(jtsBuffer);
                                        if (aranGeo == null) continue;
                                        if (aranGeo.Type == GeometryType.Polygon)
                                            bufferGeom = new MultiPolygon { aranGeo as Polygon };
                                        else
                                            bufferGeom = aranGeo as MultiPolygon;

                                        var localObstaclePt = ARANFunctions.PrjToLocal(approach.StartPoint, approach.Direction + Math.PI, obstaclePt);

                                        x = localObstaclePt.X;
                                        y = localObstaclePt.Y;
                                        maxPenetrate = tmpPenetrate;
                                        maxObstacleElev = obstacleElev;
                                        equation = "Horizontal Sector: Z =" + approach.Section3.Param.D;
                                        geom = extentPrj;
                                        verAccuracy = tmpVerAccuracy;
                                        horAccuracy = tmpHorAccuracy;
                                        exactVertex = obstaclePt;
                                        geomType = CommonFunctions.GetGeomType(extentPrj);
                                        penetratedPart = vsPart;
                                    }
                                    isIntersect = true;
                                }
                            }
                        }
                    }

                    if (isIntersect)
                    {
                        var obstacleReport = new ObstacleReport(approach.SurfaceType)
                        {
                            Id = vs.Id,
                            Name = vs.Name,
                            Obstacle = vs,
                            Plane = equation,
                            Elevation = Common.ConvertHeight(obstacleElev, RoundType.ToUp),
                            Penetrate = Common.ConvertHeight(maxPenetrate, RoundType.ToUp),
                            X = Common.ConvertDistance(x, RoundType.ToNearest),
                            Y = Common.ConvertDistance(y, RoundType.ToNearest),
                            HorizontalAccuracy = horAccuracy,
                            VerticalAccuracy = verAccuracy,
                            GeomPrj = geom,
                            BufferPrj = bufferGeom,
                            ExactVertexGeom = exactVertex,
                            SurfaceElevation = maxObstacleElev - maxPenetrate,
                            GeomType = geomType,
                            VsType = vs.Type,
                            Part = penetratedPart,
                        };

                        return obstacleReport;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                GlobalParams.AranEnvironment.GetLogger("Omega").Error(e);
                throw;
            }
        }
    }
}
