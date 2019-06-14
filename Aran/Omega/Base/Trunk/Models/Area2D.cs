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
    public class Area2D:SurfaceBase
    {
        public Area2D()
        {
            EtodSurfaceType = PANDA.Constants.EtodSurfaceType.Area2D;
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
                    var partNumber = -1;
                    VerticalStructurePart penetratedPart = null;
                    var geomType = ObstacleGeomType.Point;
                    double horAccuracy = 0, verAccuracy = 0;
                    double obstacleHeight = 0;
                    var obstacleElev = 0.0;
                    var isIntersect = false;
                    double maxPenetrate = -10000;

                    Geometry geom = null, bufferGeom = null;
                    double maxObstacleHeight = 0;
                    var equation = "";
                    foreach (var vsPart in vs.Part)
                    {
                        partNumber++;
                        VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;

                        if (vs.GetGeom(partNumber) == null || vs.GetGeom(partNumber).IsEmpty)
                            continue;

                        var jtsBuffer = vs.GetJtsBuffer(partNumber);
                        
                        double tmpVerAccuracy = Area2A.VerticalBufferWidth, tmpHorAccuracy = Area2A.HorizontalBufferWidth;
                        CommonFunctions.GetVerticalHorizontalAccuracy(horizontalProj, ref tmpVerAccuracy, ref tmpHorAccuracy);

                        if (jtsBuffer == null)
                            jtsBuffer = vs.GetJtsGeom(partNumber).Buffer(tmpHorAccuracy);

                        var vsPartPrj = vs.GetGeom(partNumber);

                        if (jtsBuffer == null || (extentJtsGeo.Disjoint(jtsBuffer))) continue;

                        obstacleHeight = ConverterToSI.Convert(vsPart.VerticalExtent,0);
                        tmpVerAccuracy = ConverterToSI.Convert(vsPart.VerticalExtentAccuracy, Area2A.VerticalBufferWidth);
                            
                        //if ((obstacleHeight+tmpVerAccuracy) < 100)
                        //    continue;

                        double tmpPenetrate = obstacleHeight + tmpVerAccuracy - 100;
                        if (tmpPenetrate > maxPenetrate)
                        {
                            var aranGeo = Aran.Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(jtsBuffer);
                            if (aranGeo == null) continue;
                            if (aranGeo.Type == GeometryType.Polygon)
                                bufferGeom = new MultiPolygon { aranGeo as Polygon };
                            else
                                bufferGeom = aranGeo as MultiPolygon;


                            equation = "Z="+Common.ConvertHeight(100,RoundType.ToNearest);
                            maxPenetrate = tmpPenetrate;
                            maxObstacleHeight = obstacleHeight;
                            obstacleElev = vs.GetElevation(partNumber);
                            geom = vsPartPrj;
                            verAccuracy = tmpVerAccuracy;
                            horAccuracy = tmpHorAccuracy;
                            if (vsPartPrj.Type == GeometryType.Point)
                                geomType = ObstacleGeomType.Point;
                            else if (vsPartPrj.Type == GeometryType.MultiPolygon)
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
                        obstacleReport.Elevation = Common.ConvertHeight(obstacleElev, RoundType.ToUp);
                        obstacleReport.Height = Common.ConvertHeight(maxObstacleHeight, RoundType.ToUp).ToString();
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
                throw new Exception("Error Create reporting Strip!");

            }
        }

        public double Radius { get; set; }

        public override IList<Info> PropertyList
        {
            get
            {
                _propertyList.Clear();
                _propertyList.Add(new Info("Radius", Common.ConvertDistance(Radius, RoundType.ToNearest).ToString(CultureInfo.InvariantCulture), InitOmega.DistanceConverter.Unit));
                return _propertyList;
            }
            set => _propertyList = value;
        }

        public override PointPenetrateModel GetManualReport(Geometries.Point obstaclePt)
        {
            if (GeoPrj.IsPointInside(obstaclePt))
            {
                //Get two point from strip which can create plane
                //Then calculate  x1 and x2 distance from start point.
         
                var result = new PointPenetrateModel();
                result.Surface = "Area 2D";
                return result;
            }
            return null;
        }
    }
}
