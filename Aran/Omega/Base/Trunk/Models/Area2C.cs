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
using Aran.PANDA.Common;

namespace Aran.Omega.Models
{
    public class Area2C:SurfaceBase,IMultiplePlane
    {
        public Area2C()
        { 
            EtodSurfaceType = PANDA.Constants.EtodSurfaceType.Area2C;
            SurfaceType = PANDA.Constants.SurfaceType.CONICAL;
            Planes = new List<Plane>();
        }


        public override void CreateReport()
        {
            _report = new MtObservableCollection<ObstacleReport>();

            var mltExtent = (from Plane plane in Planes
                             from Aran.Geometries.Point pt in plane.Geo
                             select pt).ToMultiPoint();

            var minMaxPoint = Aran.Omega.Models.TransForm.QueryCoords(mltExtent);

            var ptMin = new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin);
            var ptMax = new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax);
            var extent = new MultiPolygon { CommonFunctions.CreateExtent(ptMin.X, ptMin.Y, ptMax.X, ptMax.Y) };
            var extentJtsGeo = Aran.Converters.ConverterJtsGeom.ConvertToJtsGeo.FromMultiPolygon(extent);

            int i = 0;
            try
            {
                foreach (VerticalStructure vs in GlobalParams.AdhpObstacleList)
                {
                    i++;
                    int partNumber = -1;
                    VerticalStructurePart penetratedPart = null;
                    var geomType = ObstacleGeomType.Point;
                    double X = 0, Y = 0;
                    double horAccuracy = 0, verAccuracy = 0;
                    double obstacleElev = 0;
                    double obstacleHeight = 0;
                    bool isIntersect = false;
                    double maxPenetrate = -10000;

                    Geometry geom = null, bufferGeom = null;
                    double maxObstacleElev = 0;
                    var exactVertex = new Point();
                    string equation = "";

                    foreach (VerticalStructurePart vsPart in vs.Part)
                    {
                        partNumber++;
                        obstacleElev = vs.GetElevation(partNumber);

                        VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;

                        if (vs.GetGeom(partNumber) == null || vs.GetGeom(partNumber).IsEmpty)
                            continue;

                        var jtsBuffer = vs.GetJtsBuffer(partNumber);

                        double tmpVerAccuracy = Area2A.VerticalBufferWidth, tmpHorAccuracy = Area2A.HorizontalBufferWidth;
                        CommonFunctions.GetVerticalHorizontalAccuracy(horizontalProj, ref tmpVerAccuracy, ref tmpHorAccuracy);

                        if (jtsBuffer == null)
                            jtsBuffer = vs.GetJtsGeom(partNumber).Buffer(tmpHorAccuracy);

                        var vsPartPrj = vs.GetGeom(partNumber);

                        if (jtsBuffer != null && (!extentJtsGeo.Disjoint(jtsBuffer)))
                        {
                            foreach (Plane plane in Planes)
                            {
                                try
                                {
                                    var planeVsInersectJts = plane.JtsGeo.Intersection(jtsBuffer);

                                    if (planeVsInersectJts.IsEmpty)
                                        continue;

                                    var planeVsInersect = Aran.Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(planeVsInersectJts);

                                    MultiPoint intersectPts = new MultiPoint();
                                    if (planeVsInersect.Type == GeometryType.Polygon)
                                        intersectPts = (planeVsInersect as Aran.Geometries.Polygon).ToMultiPoint();
                                    else if (planeVsInersect.Type == GeometryType.MultiPolygon)
                                        intersectPts = (planeVsInersect as Aran.Geometries.MultiPolygon).ToMultiPoint();
                                    else if (planeVsInersect.Type == GeometryType.LineString)
                                        intersectPts = (planeVsInersect as Aran.Geometries.LineString).ToMultiPoint();
                                    else if (planeVsInersect.Type == GeometryType.MultiLineString)
                                        intersectPts = (planeVsInersect as Aran.Geometries.MultiLineString).ToMultiPoint();


                                    foreach (Aran.Geometries.Point obstaclePt in intersectPts)
                                    {

                                        //Get two point from strip which can create plane
                                        //Then calculate  x1 and x2 distance from start point.
                                        double surfaceElevation = 0;
                                        var localObstaclePt = ARANFunctions.PrjToLocal(StartPoint,
                                                Direction + Math.PI,
                                                obstaclePt);

                                        var dist = 0.0;
                                        if (plane.CalcType == CalculationType.ByCoord)
                                        {
                                            surfaceElevation = plane.Param.GetZ(localObstaclePt);
                                        }
                                        else
                                        {
                                            var refPoint = plane.RefGeometry as Aran.Geometries.Point;
                                            dist = ARANFunctions.ReturnDistanceInMeters(obstaclePt, refPoint);
                                            surfaceElevation = refPoint.Z + dist * Annex15Surfaces.Area2BSlope / 100;
                                        }
                                        double tmpPenetrate = obstacleElev + tmpVerAccuracy - surfaceElevation;

                                        if (tmpPenetrate > maxPenetrate)
                                        {
                                            X = localObstaclePt.X;
                                            Y = localObstaclePt.Y;
                                            if (plane.CalcType == CalculationType.ByCoord)
                                                equation = plane.Param.CreateEquationStr(surfaceElevation);
                                            else
                                                equation = "Z=" +Common.ConvertHeight(surfaceElevation,RoundType.ToNearest);

                                            var aranGeo = Aran.Converters.ConverterJtsGeom.ConvertFromJtsGeo.ToGeometry(jtsBuffer);
                                            if (aranGeo == null) continue;
                                            if (aranGeo.Type == GeometryType.Polygon)
                                                bufferGeom = new MultiPolygon { aranGeo as Polygon };
                                            else
                                                bufferGeom = aranGeo as MultiPolygon;

                                            maxPenetrate = tmpPenetrate;
                                            maxObstacleElev = obstacleElev;
                                            obstacleHeight = ConverterToSI.Convert(vsPart.VerticalExtent, 0); ;
                                            geom = vsPartPrj;
                                            exactVertex = obstaclePt;
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
                                catch (Exception e)
                                {
                                    GlobalParams.Logger.Error("Error occured in plane" + e.Message);
                                }
                            }
                        }
                    }
                    if (isIntersect)
                    {
                        var obstacleReport = new ObstacleReport(EtodSurfaceType)
                        {
                            Id = vs.Id,
                            Name = vs.Name,
                            Obstacle = vs,
                            Plane = equation,
                            Elevation = Common.ConvertHeight(maxObstacleElev, RoundType.ToUp),
                            Height =
                                Math.Abs(obstacleHeight) < 0.01
                                    ? "-"
                                    : Common.ConvertHeight(obstacleHeight, RoundType.ToUp).ToString(),
                            Penetrate = Common.ConvertHeight(maxPenetrate, RoundType.ToUp),
                            X = Common.ConvertDistance(X, RoundType.ToNearest),
                            Y = Common.ConvertDistance(Y, RoundType.ToNearest),
                            HorizontalAccuracy = horAccuracy,
                            VerticalAccuracy = verAccuracy,
                            GeomPrj = geom,
                            BufferPrj = bufferGeom,
                            ExactVertexGeom = exactVertex,
                            SurfaceElevation = maxObstacleElev - maxPenetrate,
                            GeomType = geomType,
                            VsType = vs.Type,
                            Part = penetratedPart
                        };

                        _report.Add(obstacleReport);

                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error when createating report Area2C! "+e.Message);

            }
        }

        public double Length { get; set; }
        public double Divergence { get; set; }

        public override IList<Info> PropertyList
        {
            get
            {
                _propertyList.Clear();
                _propertyList.Add(new Info("Divergence", Divergence.ToString(CultureInfo.InvariantCulture), "%"));
                _propertyList.Add(new Info("Length", Common.ConvertDistance(Length, RoundType.ToNearest).ToString(CultureInfo.InvariantCulture), InitOmega.DistanceConverter.Unit));
                return _propertyList; 
            }
            set => _propertyList = value;
        }

        public override PointPenetrateModel GetManualReport(Geometries.Point obstaclePt)
        {
            foreach (Plane plane in Planes)
            {
                if (plane.Geo.IsPointInside(obstaclePt))
                {
                    //Get two point from strip which can create plane
                    //Then calculate  x1 and x2 distance from start point.
                    var localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction + Math.PI,
                        obstaclePt);
                    var obstacleElev = obstaclePt.Z;

                    var surfaceElevation = plane.Param.GetZ(localObstaclePt);

                    var result = new PointPenetrateModel();
                    result.Surface = "Area 2C";
                    result.Elevation = Common.ConvertHeight(obstaclePt.Z, RoundType.ToNearest);
                    result.Plane = plane.Param.CreateEquationStr(surfaceElevation);
                    result.Penetration = Common.ConvertHeight(obstacleElev - surfaceElevation, RoundType.ToNearest);
                    return result;
                }
            }
            return null;
        }
    }
}
