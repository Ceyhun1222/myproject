using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows;
using Aran.Converters;
using Aran.Aim.Features;
using Aran.Aim;
using Aran.Geometries;
using Aran.Panda.Constants;
using Aran.Panda.Common;
using Aran.AranEnvironment.Symbols;
using Aran.Omega.SettingsUI;

namespace Omega.Models
{
    public class InnerHorizontal_old : SurfaceBase
    {
        public InnerHorizontal_old()
        {
            SurfaceType = Aran.Panda.Constants.SurfaceType.InnerHorizontal;

            SurfaceModel innerHorSurfaceModel = CommonFunctions.GetSurfaceModel(Aran.Panda.Constants.SurfaceType.InnerHorizontal);
            
            if (innerHorSurfaceModel != null)
            {
                DefaultSymbol = innerHorSurfaceModel.Symbol;
                SelectedSymbol = innerHorSurfaceModel.SelectedSymbol;
            }
        }
        public double Radius { get; set; }
        public double Elevation { get; set; }

        public override void CreateReport()
        {
            _report = new List<ObstacleReport>();
            var minMaxPoint = TransForm.QueryCoords(Geo);
            var ptMinGeo = GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin));
            var ptMaxGeo = GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax));
            var extent = new MultiPolygon {CommonFunctions.CreateExtent(ptMinGeo.X, ptMinGeo.Y, ptMaxGeo.X, ptMaxGeo.Y)};
            List<VerticalStructure> vsList = GlobalParams.Database.GetVerticalStructureList(extent);
          
           foreach (VerticalStructure vs in vsList)
            {
                foreach (VerticalStructurePart vsPart in vs.Part)
                {
                    VerticalStructurePartGeometry horizontalProj = vsPart.HorizontalProjection;
                    if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
                    {
                        var pt =
                            GlobalParams.SpatialRefOperation.ToPrj<Aran.Geometries.Point>(horizontalProj.Location.Geo);
                        if (Geo.IsPointInside(pt))
                        {
                           // Aran.Geometries.Point localObstaclePt = ARANFunctions.PrjToLocal(StartPoint, Direction + Math.PI, pt);
                            var obsReport = new ObstacleReport(SurfaceType)
                                            {
                                                Id = (int)vs.Id,
                                                Name = vs.Name,
                                                Elevation = Common.ConvertHeight(ConverterToSI.Convert(vsPart.HorizontalProjection.Location.Elevation, 0), RoundType.ToNearest),
                                                Penetrate = Common.ConvertHeight(ConverterToSI.Convert(vsPart.HorizontalProjection.Location.Elevation, 0) - Elevation, RoundType.ToNearest),
                                                Plane = "Z = " + Common.ConvertHeight(Elevation, RoundType.ToNearest),
                                                Obstacle = vs,
                                                SurfaceElevation = Elevation,
                                                GeomType = ObstacleGeomType.Point,
                                                GeomPrj = pt

                                            };
                            _report.Add(obsReport);

                        }
                    }
                    else if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedSurface)
                    {
                        
                        MultiPolygon extentPrj = GlobalParams.SpatialRefOperation.ToPrj(horizontalProj.SurfaceExtent.Geo);
                        MultiPoint mlt = extentPrj.ToMultiPoint();
                        //MultiPolygon intersect = GlobalParams.GeomOperators.Intersect(Geo, extentPrj) as MultiPolygon;

                        bool isIntersect =mlt.Any(pt => Geo.IsPointInside(pt));
                       
                      //  if (intersect != null && !intersect.IsEmpty)
                        if(isIntersect)
                        {
                            var obsReport = new ObstacleReport(SurfaceType)
                            {
                                Id = (int)vs.Id,
                                Name = vs.Name,
                                Elevation = Common.ConvertHeight(ConverterToSI.Convert(vsPart.HorizontalProjection.SurfaceExtent.Elevation, 0), RoundType.ToNearest),
                                Penetrate = Common.ConvertHeight(ConverterToSI.Convert(vsPart.HorizontalProjection.SurfaceExtent.Elevation, 0) - Elevation, RoundType.ToNearest),
                                Plane = "Z = " + Common.ConvertHeight(Elevation, RoundType.ToNearest),
                                Obstacle = vs,
                                SurfaceElevation = Elevation,
                                GeomType = ObstacleGeomType.Polygon,
                                GeomPrj = extentPrj
                            };
                            _report.Add(obsReport);
                        }

                    }

                    else if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedCurve)
                    {
                        MultiLineString linearExtent = GlobalParams.SpatialRefOperation.ToPrj(horizontalProj.LinearExtent.Geo);
                        //Geometry intersect = GlobalParams.GeomOperators.Intersect(Geo, linearExtent);
                        bool isIntersect =  linearExtent.ToMultiPoint().Any(pt => Geo.IsPointInside(pt));
                        
                      //  if (intersect != null && !intersect.IsEmpty)
                        if (isIntersect)
                        {
                            var obsReport = new ObstacleReport(SurfaceType)
                            {
                                Id = (int)vs.Id,
                                Name = vs.Name,
                                Elevation = Common.ConvertHeight(ConverterToSI.Convert(vsPart.HorizontalProjection.LinearExtent.Elevation, 0), RoundType.ToNearest),
                                Penetrate = Common.ConvertHeight(ConverterToSI.Convert(vsPart.HorizontalProjection.LinearExtent.Elevation, 0) - Elevation, RoundType.ToNearest),
                                Plane = "Z = " + Common.ConvertHeight(Elevation, RoundType.ToNearest),
                                Obstacle = vs,
                                SurfaceElevation = Elevation,
                                GeomType = ObstacleGeomType.PolyLine,
                                GeomPrj = linearExtent
                            };
                            _report.Add(obsReport);
                        }
                    }
                
                }
            }
          
        }
    }
}
