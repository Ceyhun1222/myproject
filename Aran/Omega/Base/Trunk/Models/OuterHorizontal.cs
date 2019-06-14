using System.Collections.Generic;
using System.Linq;
using Aran.Aim.Features;
using Aran.Aim;
using Aran.Converters;
using Aran.Geometries;
using Aran.Omega.Enums;
using System.Threading.Tasks;
using System.Windows.Threading;
using System;
using Aran.Omega.Strategy.ObstacleCalculation;
using Aran.Omega.Strategy.UI;

namespace Aran.Omega.Models
{
    public class OuterHorizontal:SurfaceBase, IStraightSurface
    {
        public OuterHorizontal()
        {
            SurfaceType = Aran.PANDA.Constants.SurfaceType.OuterHorizontal;
        }
        public double Radius { get; set; }
        public double ConicRadius { get; set; }
        public double Elevation { get; set; }

        public ConicalSurface Conical { get; set; }
        public InnerHorizontal InnerHorizontal { get; set; }

        public override void CreateReport()
        {
            _report = new MtObservableCollection<ObstacleReport>();

            double outerElevation = Common.ConvertHeight(Elevation, RoundType.ToNearest);

            var extentJtsGeo = Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(GeoPrj);

            //Aran.Geometries.Point adhpPrj =
            //    GlobalParams.SpatialRefOperation.ToPrj(GlobalParams.Database.AirportHeliport.ARP.Geo);

            //var outerList = (from vs in GlobalParams.AdhpObstacleList
            //    let reportList = InnerHorizontal.Report.Union(Conical.Report)
            //    where !(reportList.Any(report => report.Name == vs.Name))
            //    select vs).ToList();
            Parallel.ForEach(GlobalParams.AdhpObstacleList, (vs) =>
            {
                //    foreach (VerticalStructure vs in GlobalParams.AdhpObstacleList)
                //{
                int partNumber = -1;
                VerticalStructurePart penetratedPart = null;

                Aran.Geometries.Geometry geo = new Aran.Geometries.Point();

                double horAccuracy = 0, verAccuracy = 0;
                Aran.Geometries.MultiPolygon bufferGeom = null;

                foreach (var part in vs.Part)
                {
                    bool isIntersect = false;
                    double maxElevation = 0;
                    var geomType = ObstacleGeomType.Point;

                    partNumber++;
                    Aran.Geometries.Geometry partGeo = vs.GetGeom(partNumber);
                    if (partGeo == null)
                        continue;

                    var jtsBuffer = vs.GetJtsBuffer(partNumber);

                    double tmpVerAccuracy = Area2A.VerticalBufferWidth, tmpHorAccuracy = Area2A.HorizontalBufferWidth;
                    CommonFunctions.GetVerticalHorizontalAccuracy(part.HorizontalProjection, ref tmpVerAccuracy, ref tmpHorAccuracy);

                    if (jtsBuffer == null)
                        jtsBuffer = vs.GetJtsGeom(partNumber).Buffer(tmpHorAccuracy);

                    if (jtsBuffer != null && !jtsBuffer.Disjoint(extentJtsGeo))
                    {
                        double vsElevation = vs.GetElevation(partNumber);

                        if (vsElevation > maxElevation)
                        {
                            geo = vs.GetGeom(partNumber);
                            maxElevation = vsElevation;
                            verAccuracy = tmpVerAccuracy;
                            horAccuracy = tmpHorAccuracy;
                            bufferGeom = new MultiPolygon { Aran.Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(jtsBuffer) as Polygon };
                            penetratedPart = part;
                            isIntersect = true;
                            geomType = CommonFunctions.GetGeomType(geo);

                        }
                    }

                    if (isIntersect)
                    {
                        var obsReport = new ObstacleReport(SurfaceType)
                        {
                            Id = (int)vs.Id,
                            Name = vs.Name,
                            Elevation =Common.ConvertHeight(maxElevation,RoundType.ToNearest),
                            Penetrate =Common.ConvertHeight(maxElevation - Elevation + verAccuracy, RoundType.ToNearest),
                            Plane = "Z = " + outerElevation,
                            SurfaceElevation = Elevation,
                            Obstacle = vs,
                            GeomType = geomType,
                            HorizontalAccuracy = horAccuracy,
                            VerticalAccuracy = verAccuracy,
                            GeomPrj = geo,
                            BufferPrj = bufferGeom,
                            VsType = vs.Type,
                            Part = penetratedPart,
                        };
                        lock (_lockObject)
                        {
                            _report.Add(obsReport);
                        }
                    }
                }
            });
        }

        private IList<Info> _propList;
        public override IList<Info> PropertyList
        {
            get
            {
                _propList = new List<Info>();

                _propList.Add(new Info("Radius", Common.ConvertDistance(Radius, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                return _propList;
            }
            set => _propList = value;
        }

        public override PointPenetrateModel GetManualReport(Point obstaclePt)
        {
            var distanceToArp = CommonFunctions.GetDistance(obstaclePt, GlobalParams.OlsViewModel.Annex14Surfaces.RwyPoints);
            if (distanceToArp > ConicRadius && distanceToArp < Radius)
            {
                var result = new PointPenetrateModel
                {
                    Surface = "Outer Horizontal",
                    Penetration = Common.ConvertHeight(obstaclePt.Z - Elevation, RoundType.ToNearest),
                    Plane = "Z = " + Common.ConvertHeight(Elevation, RoundType.ToNearest)
                };
                return result;
            }
            return null;
        }
    }
}
