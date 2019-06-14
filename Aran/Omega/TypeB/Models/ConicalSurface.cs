using System.Collections.Generic;
using Aran.Converters;
using Aran.Aim.Features;
using Aran.Aim;
using Aran.Geometries;
using Aran.Omega.TypeB.Enums;
using Aran.Panda.Common;
using System;

namespace Aran.Omega.TypeB.Models
{
    public class ConicalSurface : SurfaceBase
    {
        private int _cuttingGeoHandle;
        public ConicalSurface()
        {
            SurfaceType = Aran.Panda.Constants.SurfaceType.CONICAL;

            Aran.Omega.SettingsUI.SurfaceModel surfaceModel = CommonFunctions.GetSurfaceModel(Aran.Panda.Constants.SurfaceType.CONICAL);

            if (surfaceModel != null)
            {
                DefaultSymbol = surfaceModel.Symbol;
                SelectedSymbol = surfaceModel.SelectedSymbol;
            }
        }
        public double Slope { get; set; }
        public double Height { get; set; }

        public double ElevInnerHor { get; set; }
        public double Elevation { get; set; }

        public double Radius { get; set; }
        public double  InnerHorRadius { get; set; }

        public Aran.Geometries.Point  EndCntlnPt { get; set; }

        public Aran.Geometries.MultiPolygon InnerHorGeo { get; set; }

        public Aran.Geometries.MultiPolygon CuttingGeo { get; set; }

        public override void CreateReport()
        {
            _report = new List<ObstacleReport>();

            double slope = Math.Round(Common.ConvertDistance(Slope / 100, RoundType.RealValue), 5);
            double elevation = Common.ConvertHeight(Height, RoundType.ToNearest);

            foreach (VerticalStructure vs in GlobalParams.AdhpObstacleList)
            {
                int partNumber = -1;
                double maxElevation = 0;
                bool isIntersect = false;
                double maxPenetrate = -10000;
                Geometry geo = new Aran.Geometries.Point();
                double distanceToInnerHor = 0;
                double surfaceElevation = 0;
                foreach (var part in vs.Part)
                {
                    partNumber++;

                    Aran.Geometries.Geometry partGeo = vs.GetGeom(partNumber);
                    if (partGeo == null)
                        continue;
         
         //           double partDistanceToArp = vs.GetDistance(partNumber);
           //         if (partDistanceToArp > InnerHorRadius && partDistanceToArp < (InnerHorRadius + Radius))
                    if (!GlobalParams.GeomOperators.Disjoint(partGeo, Geo))
                    {
                        distanceToInnerHor = GlobalParams.GeomOperators.GetDistance(partGeo, InnerHorGeo);
                        //double partSurfaceElevation = slope*(partDistanceToArp - InnerHorRadius) + ElevInnerHor;
                        double partSurfaceElevation = slope * distanceToInnerHor + ElevInnerHor;
                        double obstacleElev = vs.GetElevation(partNumber);
                        double tmpPenetrate = obstacleElev - partSurfaceElevation;

                        if (tmpPenetrate > maxPenetrate)
                        {
                            geo = partGeo;
                            maxPenetrate = tmpPenetrate;
                            maxElevation = obstacleElev;
                            //distanceToArp = partDistanceToArp;
                            surfaceElevation = partSurfaceElevation;
                            isIntersect = true;
                        }
                    }
                }
                if (isIntersect)
                {
                    if (maxPenetrate >= 0)
                    {
                        var geomType = ObstacleGeomType.Point;
                        if (geo.Type == Geometries.GeometryType.MultiPolygon)
                            geomType = ObstacleGeomType.Polygon;
                        else if (geo.Type == Geometries.GeometryType.MultiLineString)
                            geomType = ObstacleGeomType.PolyLine;

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
                        obsReport.SurfaceElevation = surfaceElevation;
                        obsReport.Obstacle = vs;
                        obsReport.Distance = distanceToInnerHor;
                        obsReport.GeomType = geomType;
                        obsReport.GeomPrj = geo;
                        obsReport.X = Common.ConvertDistance(distanceToInnerHor, RoundType.ToNearest);
                        obsReport.VsType = vs.Type;
                        _report.Add(obsReport);
                    }
                }
            }

        }

        public override void Draw(bool isSelected)
        {
            //base.Draw(isSelected);
            if (!isSelected) 
            {
                DrawCuttingGeo(); 
            }
        }

        public void DrawCuttingGeo()
        {
            GlobalParams.UI.SafeDeleteGraphic(_cuttingGeoHandle);
            if (CuttingGeo == null || CuttingGeo.IsEmpty)
                return;
            var cuttingGeoSymbol = new AranEnvironment.Symbols.FillSymbol();
            cuttingGeoSymbol.Color = DefaultSymbol.Color;
            cuttingGeoSymbol.Style = AranEnvironment.Symbols.eFillStyle.sfsNull;
            cuttingGeoSymbol.Outline.Style = AranEnvironment.Symbols.eLineStyle.slsDashDot;
            cuttingGeoSymbol.Outline.Width = 1;

            _cuttingGeoHandle = GlobalParams.UI.DrawMultiPolygon(CuttingGeo, cuttingGeoSymbol, true, false);
        }
        public override void ClearAll()
        {
            ClearSelected();
            ClearDefault();
            GlobalParams.UI.SafeDeleteGraphic(_cuttingGeoHandle);
        }
        //private bool IsPenetrate(Aran.Geometries.Point obstaclePt,ref double surfaceElevation)
        //{

        //    SideDirection obstacleSideFromStart = ARANMath.SideDef(StartPoint, Direction - Math.PI / 2, obstaclePt);
        //    SideDirection obstacleSideFromEnd = ARANMath.SideDef(EndCntlnPt, Direction - Math.PI / 2, obstaclePt);
        //    bool isPenetrate = false;
        //    double distToInnerHor = 0;
        //    //If startpoint side
        //    if ((obstacleSideFromStart == obstacleSideFromEnd) && obstacleSideFromStart != _endPtSideFromStart)
        //    {
        //        double distToStartPoint = ARANFunctions.ReturnDistanceInMeters(obstaclePt, StartPoint);

        //        if ((distToStartPoint > InnerHorRadius) && (distToStartPoint < (Radius + InnerHorRadius)))
        //        {
        //            distToInnerHor = distToStartPoint - InnerHorRadius;
        //            surfaceElevation = Slope * distToInnerHor / 100 + HeightInnerHor;
        //            isPenetrate = true;
        //        }

        //    }
        //    //If beetween start and endpont
        //    else if (obstacleSideFromStart != obstacleSideFromEnd)
        //    {
        //        double distToStart = ARANFunctions.ReturnDistanceInMeters(obstaclePt, StartPoint);
        //        double radToStart = ARANFunctions.ReturnAngleInRadians(StartPoint, obstaclePt);
        //        double distToRunway = Math.Abs(distToStart * Math.Sin(radToStart));
               
        //        if (distToRunway < Radius && distToRunway > InnerHorRadius)
        //        {
        //            surfaceElevation = Slope * (distToInnerHor) / 100 + HeightInnerHor;
        //            isPenetrate = true;
        //        }

        //    }
        //    //if end point side
        //    else
        //    {
        //        double distToEndPt = ARANFunctions.ReturnDistanceInMeters(obstaclePt, EndCntlnPt);
        //        if ((distToEndPt > InnerHorRadius) && (distToEndPt < (Radius + InnerHorRadius)))
        //        {
        //            surfaceElevation = Slope * (distToEndPt - InnerHorRadius) / 100 + HeightInnerHor;
        //            isPenetrate = true;
        //        }
        //    }
        //    return isPenetrate;
        //}

        private IList<Info> _propList;
        public override IList<Info> PropertyList
        {
            get
            {
                _propList = new List<Info>();

                _propList.Add(new Info("Slope", Slope.ToString(), "%"));
                _propList.Add(new Info("Radius", Common.ConvertDistance(Radius, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                _propList.Add(new Info("Height", Common.ConvertHeight(Height, RoundType.ToNearest).ToString(), InitOmega.HeightConverter.Unit));
                return _propList;
            }
            set { _propList = value; }
        }

        public override PointPenetrateModel GetManualReport(Point pt)
        {
            if (Geo.IsPointInside(pt))
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

        public override MultiPolygon GetCuttingGeometry()
        {
            return CuttingGeo;
        }
    }
}
