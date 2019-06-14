using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Omega.Enums;
using Aran.Omega.Strategy.ObstacleCalculation;
using Aran.Omega.Strategy.UI;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Geometry;
using Point = Aran.Geometries.Point;
using Polygon = Aran.Geometries.Polygon;

namespace Aran.Omega.Models
{
    public class InnerApproach:SurfaceBase
    {
        public InnerApproach()
        {
            SurfaceType = Aran.PANDA.Constants.SurfaceType.InnerApproach;

            //var surfaceModel = CommonFunctions.GetSurfaceModel(Aran.PANDA.Constants.SurfaceType.InnerApproach);
        }

        public double DistanceFromThreshold { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Slope { get; set; }
        public double H_OFZ { get; set; }

        private IList<Info> _propList = new List<Info>(); 
        public override IList<Info> PropertyList
        {
            get
            {
                _propList.Clear();
                _propList.Add(new Info("Length of inner edge", Common.ConvertDistance(Length, RoundType.ToNearest).ToString(CultureInfo.InvariantCulture), InitOmega.DistanceConverter.Unit));
                _propList.Add(new Info("Distance from runway end", Common.ConvertDistance(DistanceFromThreshold, RoundType.ToNearest).ToString(CultureInfo.InvariantCulture), InitOmega.DistanceConverter.Unit));
                _propList.Add(new Info("Width", Common.ConvertDistance(Width, RoundType.ToNearest).ToString(CultureInfo.InvariantCulture), InitOmega.DistanceConverter.Unit));
                _propList.Add(new Info("Slope", Slope.ToString(CultureInfo.InvariantCulture),"%"));
                return _propList;
            }
            set => _propList = value;
        }

        public override PointPenetrateModel GetManualReport(Point obstaclePt)
        {
            if (GeoPrj.IsPointInside(obstaclePt))
            {
                var localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction + Math.PI, obstaclePt);
                var obstacleElev = obstaclePt.Z;
                double surfaceElevation = PlaneParam.GetZ(localObstaclePt);

                var result = new PointPenetrateModel();
                result.Surface = "Inner Approach";
                result.Elevation =Common.ConvertHeight(obstaclePt.Z,RoundType.ToNearest);
                result.Plane = PlaneParam.CreateEquationStr(surfaceElevation);
                result.Penetration =Common.ConvertHeight(obstacleElev - surfaceElevation,RoundType.ToNearest);
                return result;
            }
            return null;
        }

        //public override void CreateReport()
        //{
        //    _report = new MtObservableCollection<ObstacleReport>();
        //    double obstacleElev = 0;
        //    string equation = "";

        //    var extentApproachJtsGeo = Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(Geo);
        //    Parallel.ForEach(GlobalParams.AdhpObstacleList, (vs) =>
        //    {
        //        //foreach (var vs in GlobalParams.AdhpObstacleList)
        //        //{
        //        int partNumber = -1;
        //        VerticalStructurePart penetratedPart = null;


        //        double horAccuracy = 0, verAccuracy = 0;

        //        Geometry geom = null;
        //        var maxObstacleElev = 0.0;
        //        var exactVertex = new Aran.Geometries.Point();
        //        double x = 0, y = 0;
        //        var geomType = ObstacleGeomType.Point;
        //        Aran.Geometries.MultiPolygon bufferGeom = null;

        //        foreach (var vsPart in vs.Part)
        //        {
        //            bool isIntersect = false;
        //            double maxPenetrate = -10000;

        //            partNumber++;
        //            VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;

        //            Geometry extentPrj = vs.GetGeom(partNumber);
        //            if (extentPrj == null)
        //                continue;

        //            var jtsBuffer = vs.GetJtsBuffer(partNumber);
        //            obstacleElev = vs.GetElevation(partNumber);

        //            double tmpVerAccuracy = Area2A.VerticalBufferWidth, tmpHorAccuracy = Area2A.HorizontalBufferWidth;
        //            CommonFunctions.GetVerticalHorizontalAccuracy(horizontalProj, ref tmpVerAccuracy, ref tmpHorAccuracy);

        //            if (jtsBuffer == null)
        //                jtsBuffer = vs.GetJtsGeom(partNumber).Buffer(tmpHorAccuracy);

        //            if (jtsBuffer != null && (!extentApproachJtsGeo.Disjoint(jtsBuffer)))
        //            {
        //                var intersectJtsGeo = extentApproachJtsGeo.Intersection(jtsBuffer);

        //                if (intersectJtsGeo == null) continue;

        //                var intersectGeo = Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(intersectJtsGeo);
        //                var intersectPts = intersectGeo?.ToMultiPoint();
        //                if (intersectPts == null) continue;

        //                foreach (Aran.Geometries.Point obstaclePt in intersectPts)
        //                {
        //                    var localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction + Math.PI, obstaclePt);

        //                    var surfaceElevation = PlaneParam.GetZ(localObstaclePt);
        //                    double tmpPenetrate = obstacleElev - surfaceElevation + tmpVerAccuracy;

        //                    if (tmpPenetrate > maxPenetrate)
        //                    {
        //                        var aranGeo = Aran.Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(jtsBuffer);
        //                        if (aranGeo == null) continue;
        //                        if (aranGeo.Type == GeometryType.Polygon)
        //                            bufferGeom = new MultiPolygon { aranGeo as Polygon };
        //                        else
        //                            bufferGeom = aranGeo as MultiPolygon;

        //                        equation = PlaneParam.CreateEquationStr(surfaceElevation);
        //                        x = localObstaclePt.X;
        //                        y = localObstaclePt.Y;
        //                        maxPenetrate = tmpPenetrate;
        //                        maxObstacleElev = obstacleElev;
        //                        exactVertex = obstaclePt;
        //                        verAccuracy = tmpVerAccuracy;
        //                        horAccuracy = tmpHorAccuracy;
        //                        geom = extentPrj;
        //                        geomType = CommonFunctions.GetGeomType(extentPrj);
        //                        penetratedPart = vsPart;
        //                        isIntersect = true;
        //                    }
        //                }
        //            }

        //            if (isIntersect)
        //            {

        //                var obstacleReport = new ObstacleReport(SurfaceType)
        //                {
        //                    Id = vs.Id,
        //                    Name = vs.Name,
        //                    Obstacle = vs,
        //                    Elevation = Common.ConvertHeight(obstacleElev, RoundType.ToUp),
        //                    Penetrate = Common.ConvertHeight(maxPenetrate, RoundType.ToUp),
        //                    X = Common.ConvertDistance(x, RoundType.ToNearest),
        //                    Y = Common.ConvertDistance(y, RoundType.ToNearest),
        //                    HorizontalAccuracy = horAccuracy,
        //                    VerticalAccuracy = verAccuracy,
        //                    Plane = equation,
        //                    GeomPrj = geom,
        //                    BufferPrj = bufferGeom,
        //                    ExactVertexGeom = exactVertex,
        //                    SurfaceElevation = maxObstacleElev - maxPenetrate,
        //                    GeomType = geomType,
        //                    VsType = vs.Type,
        //                    Part = penetratedPart
        //                };
        //                lock (_lockObject)
        //                {
        //                    _report.Add(obstacleReport);
        //                }
        //            }
        //        }
        //    });
        //}

    }
}
