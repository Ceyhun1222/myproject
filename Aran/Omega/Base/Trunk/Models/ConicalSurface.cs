using System.Collections.Generic;
using Aran.Converters;
using Aran.Aim.Features;
using Aran.Aim;
using Aran.Geometries;
using Aran.Omega.Enums;
using Aran.PANDA.Common;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Collections.Concurrent;
using Aran.Omega.Strategy.ObstacleCalculation;
using Aran.Omega.Strategy.UI;

namespace Aran.Omega.Models
{
    public class ConicalSurface : SurfaceBase
    {
        public ConicalSurface()
        {
            SurfaceType = PANDA.Constants.SurfaceType.CONICAL;
        }
        public double Slope { get; set; }
        public double Height { get; set; }

        public double ElevInnerHor { get; set; }
        public double Elevation { get; set; }

        public double Radius { get; set; }
        public double  InnerHorRadius { get; set; }

        public Aran.Geometries.Point  EndCntlnPt { get; set; }

        public Aran.Geometries.MultiPolygon InnerHorGeo { get; set; }

        public override void CreateReport()
        {
            _report = new MtObservableCollection<ObstacleReport>();

            double slope = Math.Round(Common.ConvertDistance(Slope / 100, RoundType.RealValue), 5);
            double elevation = Common.ConvertHeight(Height, RoundType.ToNearest);

            var extentConnicalJtsGeo = Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(GeoPrj);
            var extentInnerJtsGeo = Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(InnerHorGeo);

            //foreach (VerticalStructure vs in GlobalParams.AdhpObstacleList)
            //{
            var tmpReport = new BlockingCollection<ObstacleReport>();
            Parallel.ForEach(GlobalParams.AdhpObstacleList, (vs) =>
            {
                int partNumber = -1;
                VerticalStructurePart penetratedPart = null;
                double maxElevation = 0;

                Geometry geo = new Aran.Geometries.Point();
                double distanceToInnerHor = 0;
                double surfaceElevation = 0;
                double horAccuracy = 0, verAccuracy = 0;
                var geomType = ObstacleGeomType.Point;
                Aran.Geometries.MultiPolygon bufferGeom = null;

                foreach (var part in vs.Part)
                {
                    bool isIntersect = false;
                    double maxPenetrate = -10000;

                    partNumber++;

                    Aran.Geometries.Geometry partGeo = vs.GetGeom(partNumber);
                    VerticalStructurePartGeometry horizontalProj = part.HorizontalProjection;

                    var jtsBuffer = vs.GetJtsBuffer(partNumber);

                    if (partGeo == null)
                        continue;

                    double tmpVerAccuracy = Area2A.VerticalBufferWidth, tmpHorAccuracy = Area2A.HorizontalBufferWidth;
                    CommonFunctions.GetVerticalHorizontalAccuracy(horizontalProj, ref tmpVerAccuracy, ref tmpHorAccuracy);

                    if (jtsBuffer == null)
                        jtsBuffer = vs.GetJtsGeom(partNumber).Buffer(tmpHorAccuracy);

                    if (!extentConnicalJtsGeo.Disjoint(jtsBuffer))
                    {
                        distanceToInnerHor = jtsBuffer.Distance(extentInnerJtsGeo);
                        //double partSurfaceElevation = slope*(partDistanceToArp - InnerHorRadius) + ElevInnerHor;
                        double partSurfaceElevation = slope * distanceToInnerHor + ElevInnerHor;
                        double obstacleElev = vs.GetElevation(partNumber);
                        double tmpPenetrate = obstacleElev - partSurfaceElevation + tmpVerAccuracy;

                        if (tmpPenetrate > maxPenetrate)
                        {
                            geo = partGeo;
                            maxPenetrate = tmpPenetrate;
                            maxElevation = obstacleElev;
                            //distanceToArp = partDistanceToArp;
                            surfaceElevation = partSurfaceElevation;
                            horAccuracy = tmpHorAccuracy;
                            verAccuracy = tmpVerAccuracy;
                            geomType = CommonFunctions.GetGeomType(partGeo);
                            bufferGeom = new MultiPolygon { Aran.Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(jtsBuffer) as Polygon };
                            penetratedPart = part;
                            isIntersect = true;

                        }
                    }

                    if (isIntersect)
                    {

                        var obsReport = new ObstacleReport(SurfaceType);
                        obsReport.Id = (int)vs.Id;
                        obsReport.Name = vs.Name;
                        obsReport.Elevation = Common.ConvertHeight(maxElevation, RoundType.ToNearest);
                        obsReport.Penetrate = Common.ConvertHeight(maxPenetrate, RoundType.ToNearest);
                        obsReport.Plane = "Z = " + slope + "*D0 +" + ElevInnerHor + " = " +
                                          Common.ConvertAccuracy(surfaceElevation, RoundType.ToNearest,
                                              InitOmega.HeightConverter);
                        if (InitOmega.HeightConverter.Unit == "ft")
                        {
                            obsReport.Plane += " (m) =" +
                                               Common.ConvertHeight(surfaceElevation, RoundType.ToNearest).ToString() +
                                               " ft ";
                        }
                        obsReport.HorizontalAccuracy = horAccuracy;
                        obsReport.VerticalAccuracy = verAccuracy;
                        obsReport.SurfaceElevation = surfaceElevation;
                        obsReport.Obstacle = vs;
                        obsReport.Distance = distanceToInnerHor;
                        obsReport.GeomType = geomType;
                        obsReport.GeomPrj = geo;
                        obsReport.BufferPrj = bufferGeom;
                        obsReport.X = Common.ConvertDistance(distanceToInnerHor, RoundType.ToNearest);
                        obsReport.VsType = vs.Type;
                        obsReport.Part = penetratedPart;

                        tmpReport.TryAdd(obsReport,100);
                        //lock (_lockObject)
                        //{
                        //    _report.Add(obsReport);
                        //}
                    }
                }
            });

            foreach (var reportItem in tmpReport)
                _report.Add(reportItem);

        }

        private IList<Info> _propList = new List<Info>();
        public override IList<Info> PropertyList
        {
            get
            {
                _propList.Clear();
                _propList.Add(new Info("Slope", Slope.ToString(CultureInfo.InvariantCulture), "%"));
                _propList.Add(new Info("Radius", Common.ConvertDistance(Radius, RoundType.ToNearest).ToString(CultureInfo.InvariantCulture), InitOmega.DistanceConverter.Unit));
                _propList.Add(new Info("Height", Common.ConvertHeight(Height, RoundType.ToNearest).ToString(CultureInfo.InvariantCulture), InitOmega.HeightConverter.Unit));
                return _propList;
            }
            set => _propList = value;
        }

        public override PointPenetrateModel GetManualReport(Point pt)
        {
            if (GeoPrj.IsPointInside(pt))
            {
                var result = new PointPenetrateModel();
                result.Surface = "Conical";
                result.Elevation =Common.ConvertHeight(pt.Z,RoundType.ToNearest);
               
                var distanceToInnerHor = CommonFunctions.GetDistance(pt, InnerHorGeo.ToMultiPoint());
                double slope = Math.Round(Common.ConvertDistance(Slope / 100, RoundType.RealValue), 5);
                var surfaceElevation = slope * distanceToInnerHor + ElevInnerHor;
                result.Penetration =Common.ConvertHeight(pt.Z-surfaceElevation,RoundType.ToNearest);
                result.Plane = "Z = " + slope + "*D0 +" + ElevInnerHor + " = " +
                            Common.ConvertHeight(surfaceElevation, RoundType.ToNearest).ToString();
              
                return result;
            }
            return null;
        }
    }
}
