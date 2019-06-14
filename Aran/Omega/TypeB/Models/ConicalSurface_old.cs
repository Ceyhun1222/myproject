using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Animation;
using Aran.Converters;
using Aran.Aim.Features;
using Aran.Aim;
using Aran.Geometries;
using Aran.Omega.SettingsUI;
using System.Linq;
using Aran.Panda.Common;
using System;

namespace Omega.Models
{
    public class ConicalSurface_old : SurfaceBase
    {
        private SideDirection _endPtSideFromStart;

        public ConicalSurface_old()
        {
            SurfaceType = Aran.Panda.Constants.SurfaceType.CONICAL;

            SurfaceModel surfaceModel = CommonFunctions.GetSurfaceModel(Aran.Panda.Constants.SurfaceType.CONICAL);

            if (surfaceModel != null)
            {
                DefaultSymbol = surfaceModel.Symbol;
                SelectedSymbol = surfaceModel.SelectedSymbol;
            }
        }
        public double Slope { get; set; }
        public double Height { get; set; }

        public double HeightInnerHor { get; set; }

        public double Radius { get; set; }
        public double  InnerHorRadius { get; set; }

        public Aran.Geometries.Point  EndCntlnPt { get; set; }

        public override void CreateReport()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            _report = new List<ObstacleReport>();
            double surfaceElevation = 0;

            var minMaxPoint = TransForm.QueryCoords(Geo);
            var ptMinGeo = GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin));
            var ptMaxGeo = GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax));
            var extent = new MultiPolygon { CommonFunctions.CreateExtent(ptMinGeo.X, ptMinGeo.Y, ptMaxGeo.X, ptMaxGeo.Y) };

            Polygon polygon = CommonFunctions.CreateExtent(minMaxPoint.XMin, minMaxPoint.YMin, minMaxPoint.XMax,
                minMaxPoint.YMax);

            GlobalParams.UI.DrawPolygon(polygon, 100, Aran.AranEnvironment.Symbols.eFillStyle.sfsHorizontal);
            List<VerticalStructure> vsList = GlobalParams.Database.GetVerticalStructureList(extent);

            _endPtSideFromStart = ARANMath.SideDef(StartPoint, Direction - Math.PI / 2, EndCntlnPt);
            int i = 0;

            foreach (VerticalStructure vs in vsList)
            {
                i++;
                foreach (var vsPart in vs.Part)
                {
                  //  GlobalParams.UI.DrawMultiPolygon(extent, 100, Aran.AranEnvironment.Symbols.eFillStyle.sfsHorizontal);
                    VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;
                    if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
                    {
                        var obstaclePt =
                            GlobalParams.SpatialRefOperation.ToPrj<Aran.Geometries.Point>(horizontalProj.Location.Geo);

                        if (IsPenetrate(obstaclePt, ref surfaceElevation))
                        {
                            Aran.Geometries.Point localObstaclePt = ARANFunctions.PrjToLocal(StartPoint,
                                Direction + Math.PI, obstaclePt);

                            var obsReport = new ObstacleReport(SurfaceType)
                            {
                                Id = (int) vs.Id,
                                Name = vs.Name,
                                Elevation =
                                    Common.ConvertHeight(
                                        ConverterToSI.Convert(vsPart.HorizontalProjection.Location.Elevation, 0),
                                        RoundType.ToNearest),
                                Penetrate =
                                    Common.ConvertHeight(
                                        ConverterToSI.Convert(vsPart.HorizontalProjection.Location.Elevation, 0) -
                                        surfaceElevation, RoundType.ToNearest),
                                Plane =
                                    "Z = " + Math.Round(Common.ConvertDistance(Slope/100, RoundType.RealValue), 5) +
                                    "*D0 +" + Common.ConvertHeight(Height, RoundType.ToNearest).ToString() + " = " +
                                    Common.ConvertHeight(surfaceElevation, RoundType.ToNearest).ToString(),
                                X = Common.ConvertHeight(localObstaclePt.X, RoundType.ToNearest),
                                Y = Common.ConvertHeight(localObstaclePt.Y, RoundType.ToNearest),
                                SurfaceElevation = surfaceElevation,
                                Obstacle = vs,
                                GeomType = ObstacleGeomType.Point,
                                GeomPrj = obstaclePt,
                            };
                            _report.Add(obsReport);
                        }
                    }

                    else if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedSurface)
                    {
                        MultiPolygon extentPrj = GlobalParams.SpatialRefOperation.ToPrj(horizontalProj.SurfaceExtent.Geo);

                        MultiPoint mlt = extentPrj.ToMultiPoint();

                        double minSurfaceElevation = 0;
                        bool isPenetrate = IsPenetrate(mlt[0], ref minSurfaceElevation);
                        for (int j = 1; j < mlt.Count; j++)
                        {
                            if (IsPenetrate(mlt[j], ref surfaceElevation))
                            {
                                isPenetrate = true;
                                if (minSurfaceElevation < surfaceElevation)
                                    minSurfaceElevation = surfaceElevation;
                            }
                        }

                        //  surfaceElevation = Slope * dist / 100 + Height;

                        if (isPenetrate)
                        {
                            var obsReport = new ObstacleReport(SurfaceType)
                            {
                                Id = (int) vs.Id,
                                Name = vs.Name,
                                Elevation =
                                    Common.ConvertHeight(
                                        ConverterToSI.Convert(vsPart.HorizontalProjection.SurfaceExtent.Elevation, 0),
                                        RoundType.ToNearest),
                                Penetrate =
                                    Common.ConvertHeight(
                                        ConverterToSI.Convert(vsPart.HorizontalProjection.SurfaceExtent.Elevation, 0) -
                                        surfaceElevation, RoundType.ToNearest),
                                Plane =
                                    "Z = " + (Slope/100) + "*DO +" +
                                    Common.ConvertHeight(Height, RoundType.ToNearest).ToString() + " = " +
                                    Common.ConvertHeight(surfaceElevation, RoundType.ToNearest).ToString(),
                                SurfaceElevation = surfaceElevation,
                                Obstacle = vs,
                                GeomType = ObstacleGeomType.Polygon,
                                GeomPrj = extentPrj

                            };
                            _report.Add(obsReport);
                        }


                    }

                    else if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedCurve)
                    {
                        MultiLineString linearExtent =
                            GlobalParams.SpatialRefOperation.ToPrj(horizontalProj.LinearExtent.Geo);

                        MultiPoint mlt = linearExtent.ToMultiPoint();

                        double minSurfaceElevation = 0;
                        if (mlt.Count == 0)
                            continue;
                        bool isPenetrate = IsPenetrate(mlt[0], ref minSurfaceElevation);
                        for (int j = 1; j < mlt.Count; j++)
                        {
                            if (IsPenetrate(mlt[j], ref surfaceElevation))
                            {
                                isPenetrate = true;
                                if (minSurfaceElevation < surfaceElevation)
                                    minSurfaceElevation = surfaceElevation;
                            }
                        }

                        if (isPenetrate)
                        {
                            var obsReport = new ObstacleReport(SurfaceType)
                            {
                                Id = (int) vs.Id,
                                Name = vs.Name,
                                Elevation =
                                    Common.ConvertHeight(
                                        ConverterToSI.Convert(vsPart.HorizontalProjection.LinearExtent.Elevation, 0),
                                        RoundType.ToNearest),
                                Penetrate =
                                    Common.ConvertHeight(
                                        ConverterToSI.Convert(vsPart.HorizontalProjection.LinearExtent.Elevation, 0) -
                                        surfaceElevation, RoundType.ToNearest),
                                Plane =
                                    "Z = " + (Slope/100) + "*DO +" +
                                    Common.ConvertHeight(Height, RoundType.ToNearest).ToString() + " = " +
                                    Common.ConvertHeight(surfaceElevation, RoundType.ToNearest)
                                        .ToString(CultureInfo.InvariantCulture),
                                SurfaceElevation = surfaceElevation,
                                Obstacle = vs,
                                GeomType = ObstacleGeomType.PolyLine,
                                GeomPrj = linearExtent

                            };
                            _report.Add(obsReport);
                        }
                    }
                }
            }
            stopWatch.Stop();

            MessageBox.Show(stopWatch.Elapsed.ToString());
        }

        private bool IsPenetrate(Aran.Geometries.Point obstaclePt,ref double surfaceElevation)
        {

            SideDirection obstacleSideFromStart = ARANMath.SideDef(StartPoint, Direction - Math.PI / 2, obstaclePt);
            SideDirection obstacleSideFromEnd = ARANMath.SideDef(EndCntlnPt, Direction - Math.PI / 2, obstaclePt);
            bool isPenetrate = false;
            double distToInnerHor = 0;
            //If startpoint side
            if ((obstacleSideFromStart == obstacleSideFromEnd) && obstacleSideFromStart != _endPtSideFromStart)
            {
                double distToStartPoint = ARANFunctions.ReturnDistanceInMeters(obstaclePt, StartPoint);

                if ((distToStartPoint > InnerHorRadius) && (distToStartPoint < (Radius + InnerHorRadius)))
                {
                    distToInnerHor = distToStartPoint - InnerHorRadius;
                    surfaceElevation = Slope * distToInnerHor / 100 + HeightInnerHor;
                    isPenetrate = true;
                }

            }
            //If beetween start and endpont
            else if (obstacleSideFromStart != obstacleSideFromEnd)
            {
                double distToStart = ARANFunctions.ReturnDistanceInMeters(obstaclePt, StartPoint);
                double radToStart = ARANFunctions.ReturnAngleInRadians(StartPoint, obstaclePt);
                double distToRunway = Math.Abs(distToStart * Math.Sin(radToStart));
               
                if (distToRunway < Radius && distToRunway > InnerHorRadius)
                {
                    surfaceElevation = Slope * (distToInnerHor) / 100 + HeightInnerHor;
                    isPenetrate = true;
                }

            }
            //if end point side
            else
            {
                double distToEndPt = ARANFunctions.ReturnDistanceInMeters(obstaclePt, EndCntlnPt);
                if ((distToEndPt > InnerHorRadius) && (distToEndPt < (Radius + InnerHorRadius)))
                {
                    surfaceElevation = Slope * (distToEndPt - InnerHorRadius) / 100 + HeightInnerHor;
                    isPenetrate = true;
                }
            }
            return isPenetrate;
        }
    }
}
