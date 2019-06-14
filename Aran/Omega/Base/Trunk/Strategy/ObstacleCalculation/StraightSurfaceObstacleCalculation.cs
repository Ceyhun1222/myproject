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

namespace Aran.Omega.Strategy.ObstacleCalculation
{
    class StraightSurfaceObstacleCalculation : IObstacleCalculation
    {
        public MtObservableCollection<ObstacleReport> CalculateReport(SurfaceBase surface)
        {
            var straightSurface = surface as IStraightSurface;

            if (straightSurface == null)
                throw new ArgumentException("Surface must be Straight Surface");

            double innerHorElevation = Common.ConvertHeight(straightSurface.Elevation, RoundType.ToNearest);
            var extentJtsGeo = Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(surface.GeoPrj);

            var concurentReportList = new BlockingCollection<ObstacleReport>();

            Parallel.ForEach(GlobalParams.AdhpObstacleList, (vs) =>
            {
                var obsReport = GetItem(vs, extentJtsGeo, surface,innerHorElevation);
                if (obsReport != null)
                    concurentReportList.Add(obsReport);
            });

            return new MtObservableCollection<ObstacleReport>(concurentReportList);
        }

        public MtObservableCollection<ObstacleReport> CalculateReportSync(SurfaceBase surface)
        {
            var straightSurface = surface as IStraightSurface;

            if (straightSurface == null)
                throw new ArgumentException("Surface must be Straight Surface");

            double innerHorElevation = Common.ConvertHeight(straightSurface.Elevation, RoundType.ToNearest);
            var extentJtsGeo = Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(surface.GeoPrj);

            var concurentReportList = new BlockingCollection<ObstacleReport>();

            foreach (var vs in GlobalParams.AdhpObstacleList)
            {
                var obsReport = GetItem(vs, extentJtsGeo, surface, innerHorElevation);
                if (obsReport != null)
                    concurentReportList.Add(obsReport);
            }

            return new MtObservableCollection<ObstacleReport>(concurentReportList);
        }

        private ObstacleReport GetItem(VerticalStructure vs, GeoAPI.Geometries.IMultiPolygon extentJtsGeo, SurfaceBase surface,double innerHorElevation)
        {
            try
            {
                var straightSurface = surface as IStraightSurface;

                int partNumber = -1;
                VerticalStructurePart penetratedPart = null;

                Aran.Geometries.Geometry geo = new Aran.Geometries.Point();

                double horAccuracy = 0, verAccuracy = 0;
                Aran.Geometries.MultiPolygon bufferGeom = null;

                foreach (var part in vs.Part)
                {
                    double maxElevation = 0;
                    partNumber++;

                    bool isIntersect = false;
                    Aran.Geometries.Geometry partGeo = vs.GetGeom(partNumber);
                    if (partGeo == null)
                        continue;

                    var jtsBuffer = vs.GetJtsBuffer(partNumber);

                    VerticalStructurePartGeometry horizontalProj = part.HorizontalProjection;
                    double tmpVerAccuracy = Area2A.VerticalBufferWidth, tmpHorAccuracy = Area2A.HorizontalBufferWidth;
                    CommonFunctions.GetVerticalHorizontalAccuracy(horizontalProj, ref tmpVerAccuracy, ref tmpHorAccuracy);

                    if (jtsBuffer == null)
                        jtsBuffer = vs.GetJtsGeom(partNumber).Buffer(tmpHorAccuracy);

                    if (!jtsBuffer.Disjoint(extentJtsGeo))
                    {
                        double vsElevation = vs.GetElevation(partNumber) + tmpVerAccuracy;

                        if (vsElevation > maxElevation)
                        {
                            var aranGeo = Aran.Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(jtsBuffer);
                            if (aranGeo == null) continue;

                            geo = vs.GetGeom(partNumber);
                            maxElevation = vsElevation;
                            horAccuracy = tmpHorAccuracy;
                            verAccuracy = tmpVerAccuracy;

                            if (aranGeo.Type == GeometryType.Polygon)
                                bufferGeom = new MultiPolygon { aranGeo as Polygon };
                            else
                                bufferGeom = aranGeo as MultiPolygon;

                            penetratedPart = part;

                            isIntersect = true;

                        }
                        if (isIntersect)
                        {
                            var geomType = ObstacleGeomType.Point;
                            if (geo.Type == Geometries.GeometryType.MultiPolygon)
                                geomType = ObstacleGeomType.Polygon;
                            else if (geo.Type == Geometries.GeometryType.MultiLineString)
                                geomType = ObstacleGeomType.PolyLine;

                            var obsReport = new ObstacleReport(surface.SurfaceType)
                            {
                                Id = (int)vs.Id,
                                Name = vs.Name,
                                Elevation = Common.ConvertHeight(maxElevation, RoundType.ToNearest),
                                Penetrate = Common.ConvertHeight(maxElevation - straightSurface.Elevation, RoundType.ToNearest),
                                Plane = "Z = " + innerHorElevation,
                                Obstacle = vs,
                                SurfaceElevation = straightSurface.Elevation,
                                GeomType = geomType,
                                GeomPrj = geo,
                                BufferPrj = bufferGeom,
                                VsType = vs.Type,
                                VerticalAccuracy = verAccuracy,
                                HorizontalAccuracy = horAccuracy,
                                Part = penetratedPart
                            };

                            return obsReport;
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                GlobalParams.Logger.Error(e);
                throw;
            }
        }
    }
}
