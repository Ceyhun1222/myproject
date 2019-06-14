using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Omega;
using Aran.Omega.Enums;
using Aran.Omega.Models;
using Aran.Omega.Strategy.ObstacleCalculation;
using Aran.Omega.Strategy.UI;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;


namespace Aran.Omega.Models
{
    public class Area2B:SurfaceBase,IMultiplePlane
    {
        public Area2B()
        {
            SurfaceType = PANDA.Constants.SurfaceType.Approach;
            EtodSurfaceType = PANDA.Constants.EtodSurfaceType.Area2B;
            Planes = new List<Plane>();
    
        }

        public double LengthOfInnerEdge { get; set; }
        public double  Divergence { get; set; }
        public double Length { get; set; }
        public double Slope { get; set; }

        //public override void Draw(bool isSelected)
        //{
        //    if (isSelected)
        //    {
        //        ClearSelected();
        //        foreach (Plane strip in Planes)
        //        {
        //            SelectedStripHandles.Add(GlobalParams.UI.DrawRing(strip.Geo, SelectedSymbol));
        //        }
        //    }
        //    else
        //    {
        //        ClearDefault();
        //        foreach (Plane strip in Planes)
        //        {
        //            DefaultStripHandles.Add(GlobalParams.UI.DrawRing(strip.Geo, DefaultSymbol));
        //        }
        //    }

        //}

        //public override void ClearSelected()
        //{
        //    foreach (int handle in SelectedStripHandles)
        //    {
        //        GlobalParams.UI.SafeDeleteGraphic(handle);
        //    }
        //    SelectedStripHandles.Clear();
        //}

        //public override void ClearAll()
        //{
        //    ClearDefault();
        //    ClearSelected();
        //}

        //public override void ClearDefault()
        //{
        //    foreach (int handle in DefaultStripHandles)
        //    {
        //        GlobalParams.UI.SafeDeleteGraphic(handle);
        //    }
        //    DefaultStripHandles.Clear();
        //}

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
                    var geomType = ObstacleGeomType.Point;
                    VerticalStructurePart penetratedPart = null;
                    double X = 0, Y = 0;
                    double horAccuracy = 0, verAccuracy = 0;
                    double obstacleElev;
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
                        VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;

                        if (vs.GetGeom(partNumber)==null || vs.GetGeom(partNumber).IsEmpty)
                            continue;

                        var height = ConverterToSI.Convert(vsPart.VerticalExtent, 0);

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

                               

                                obstacleElev = vs.GetElevation(partNumber);

                                foreach (Aran.Geometries.Point obstaclePt in intersectPts)
                                {

                                    //Get two point from strip which can create plane
                                    //Then calculate  x1 and x2 distance from start point.
                                    var localObstaclePt = ARANFunctions.PrjToLocal(StartPoint,
                                        Direction + Math.PI,
                                        obstaclePt);

                                    var surfaceElevation = plane.Param.GetZ(localObstaclePt);
                                    double tmpPenetrate = obstacleElev + tmpVerAccuracy - surfaceElevation;
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
                                        obstacleHeight = height;
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
                                        isIntersect = true;
                                        penetratedPart = vsPart;
                                    }

                                }
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
                        obstacleReport.Elevation = Common.ConvertHeight(maxObstacleElev, RoundType.ToUp);
                        obstacleReport.Height = obstacleHeight == 0 ? "-" : Common.ConvertHeight(obstacleHeight, RoundType.ToUp).ToString();
                        obstacleReport.Penetrate = Common.ConvertHeight(maxPenetrate, RoundType.ToUp);
                        obstacleReport.X = Common.ConvertDistance(X, RoundType.ToNearest);
                        obstacleReport.Y = Common.ConvertDistance(Y, RoundType.ToNearest);
                        obstacleReport.HorizontalAccuracy = horAccuracy;
                        obstacleReport.VerticalAccuracy = verAccuracy;
                        obstacleReport.GeomPrj = geom;
                        obstacleReport.BufferPrj = bufferGeom;
                        obstacleReport.ExactVertexGeom = exactVertex;
                        obstacleReport.SurfaceElevation = maxObstacleElev - maxPenetrate;
                        obstacleReport.GeomType = geomType;
                        obstacleReport.VsType = vs.Type;
                        obstacleReport.Part = penetratedPart;

                        _report.Add(obstacleReport);

                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error Create reporting Area2B!");

            }
        }

        public override IList<Info> PropertyList
        {
            get
            {
                _propertyList.Clear();
                _propertyList.Add(new Info("Length of inner edge", Common.ConvertDistance(LengthOfInnerEdge, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                _propertyList.Add(new Info("Length", Common.ConvertDistance(Length, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                _propertyList.Add(new Info("Slope", Slope.ToString(), "%"));
                _propertyList.Add(new Info("Divergence", Divergence.ToString(), "%"));
                return _propertyList;
            }
            set => _propertyList = value;
        }

        public override PointPenetrateModel GetManualReport(Aran.Geometries.Point obstaclePt)
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
                    result.Surface = "Area 2B";
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
