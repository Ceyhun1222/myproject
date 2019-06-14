using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class Area3:SurfaceBase
    {
        public const double Area3MaxHeight = 0.5;
        public const double VerticalBufferWidth = 0.5;
        public const double HorizontalBufferWidth = 0.5;

        public Area3()
        {
            EtodSurfaceType = PANDA.Constants.EtodSurfaceType.Area3;
            SurfaceType = PANDA.Constants.SurfaceType.TakeOffClimb;
        }
        public override void CreateReport()
        {
            _report = new MtObservableCollection<ObstacleReport>();

            var extentJtsGeo = Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(GeoPrj);

            int i = 0;
            try
            {
                foreach (VerticalStructure vs in GlobalParams.AdhpObstacleList)
                {
                    i++;
                    int partNumber = -1;
                    VerticalStructurePart penetratedPart = null;
                    var geomType = ObstacleGeomType.Point;
                    double horAccuracy = 0, verAccuracy = 0;
                    double obstacleElevation = 0;
                    bool isIntersect = false;
                    double maxPenetrate = -10000;

                    Geometry geom = null, bufferGeom = null;
                    double maxObstacleHeight = 0;
                    var equation = "";
                    foreach (VerticalStructurePart vsPart in vs.Part)
                    {
                        partNumber++;
                        VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;

                        if (vs.GetGeom(partNumber) == null || vs.GetGeom(partNumber).IsEmpty)
                            continue;


                        double tmpVerAccuracy = Area3.VerticalBufferWidth, tmpHorAccuracy = Area3.HorizontalBufferWidth;
                        CommonFunctions.GetVerticalHorizontalAccuracy(horizontalProj, ref tmpVerAccuracy, ref tmpHorAccuracy);

                        var jtsBuffer = vs.GetJtsGeom(partNumber).Buffer(tmpHorAccuracy);

                        var vsPartPrj = vs.GetGeom(partNumber);

                        if (jtsBuffer != null && (!extentJtsGeo.Disjoint(jtsBuffer)))
                        {
                            var obstacleHeight = ConverterToSI.Convert(vsPart.VerticalExtent, 0);
                            var extentAccuracy = ConverterToSI.Convert(vsPart.VerticalExtentAccuracy, -1);
                            if (extentAccuracy > -1)
                                tmpVerAccuracy = extentAccuracy;

                            double tmpPenetrate = (obstacleHeight + tmpVerAccuracy - Area3MaxHeight);
                            if (tmpPenetrate > maxPenetrate)
                            {
                                var aranGeo = Aran.Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(jtsBuffer);
                                if (aranGeo == null) continue;
                                if (aranGeo.Type == GeometryType.Polygon)
                                    bufferGeom = new MultiPolygon { aranGeo as Polygon };
                                else
                                    bufferGeom = aranGeo as MultiPolygon;


                                equation = "Z=" + Common.ConvertHeight(Area3MaxHeight, RoundType.ToNearest);
                                maxPenetrate = tmpPenetrate;
                                maxObstacleHeight = obstacleHeight;
                                obstacleElevation = Common.ConvertHeight(vs.GetElevation(partNumber), RoundType.ToUp);
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
                    if (isIntersect)
                    {
                        var obstacleReport = new ObstacleReport(EtodSurfaceType);
                        obstacleReport.Id = vs.Id;
                        obstacleReport.Name = vs.Name;
                        obstacleReport.Obstacle = vs;
                        obstacleReport.Plane = equation;
                        obstacleReport.Height = Common.ConvertHeight(maxObstacleHeight, RoundType.ToNearest).ToString();
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
                GlobalParams.AranEnvironment.GetLogger("Omega").Error(e.StackTrace);
                throw new Exception("Error Create reporting Area3!");
            }
        }


        public override IList<Info> PropertyList
        {
            get
            {
                _propertyList.Clear();
                _propertyList.Add(new Info("Height", Common.ConvertDistance(Area3MaxHeight, RoundType.ToNearest).ToString(CultureInfo.InvariantCulture), InitOmega.DistanceConverter.Unit));
                return _propertyList;
            }
            set => _propertyList = value;
        }

        public override PointPenetrateModel GetManualReport(Geometries.Point obstaclePt)
        {
            if (GeoPrj.IsPointInside(obstaclePt))
            {
                var result = new PointPenetrateModel();
                result.Surface = "Area 3";
                result.Plane = "Z=0.5";
                return result;
            }
            return null;
        }
    }
}
