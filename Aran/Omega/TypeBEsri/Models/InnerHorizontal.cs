using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Converters;
using Aran.Aim.Features;
using Aran.Aim;
using Aran.Omega.TypeBEsri.Enums;
using Aran.Geometries;

namespace Aran.Omega.TypeBEsri.Models
{
    public class InnerHorizontal : SurfaceBase
    {
        public InnerHorizontal()
        {
            SurfaceType = Aran.Panda.Constants.SurfaceType.InnerHorizontal;

            var innerHorSurfaceModel =
                CommonFunctions.GetSurfaceModel(Aran.Panda.Constants.SurfaceType.InnerHorizontal);

            if (innerHorSurfaceModel != null)
            {
                DefaultSymbol = innerHorSurfaceModel.Symbol;
                DefaultSymbol.Style = AranEnvironment.Symbols.eFillStyle.sfsNull;
                SelectedSymbol = innerHorSurfaceModel.SelectedSymbol;
            }
        }

        public double Radius { get; set; }
        public double Elevation { get; set; }
        public double Height { get; set; }


        public override void CreateReport()
        {
            _report = new List<ObstacleReport>();

            double innerHorElevation = Common.ConvertHeight(Elevation, RoundType.ToNearest);

            foreach (VerticalStructure vs in GlobalParams.AdhpObstacleList)
            {
                int partNumber = -1;

                double maxElevation = 0;
                Aran.Geometries.Geometry geo = new Aran.Geometries.Point();
                bool isIntersect = false;
                foreach (var part in vs.Part)
                {
                    partNumber++;
                    Aran.Geometries.Geometry partGeo = vs.GetGeom(partNumber);
                    if (partGeo == null)
                        continue;

                    //double distanceToPart = vs.GetDistance(partNumber);
                    if (!GlobalParams.GeomOperators.Disjoint(partGeo, Geo))
                    //if (distanceToPart < Radius && distanceToPart > 0)
                    {
                        double vsElevation = vs.GetElevation(partNumber);

                        if (vsElevation > maxElevation)
                        {
                            geo = vs.GetGeom(partNumber);
                            maxElevation = vsElevation;
                            isIntersect = true;
                        }
                    }
                }
                
                if (isIntersect)
                {
                    var geomType = ObstacleGeomType.Point;
                    if (geo.Type == Geometries.GeometryType.MultiPolygon)
                        geomType = ObstacleGeomType.Polygon;
                    else if (geo.Type == Geometries.GeometryType.MultiLineString)
                        geomType = ObstacleGeomType.PolyLine;
                    if (maxElevation - Elevation >= 0)
                    {
                        var obsReport = new ObstacleReport(SurfaceType)
                        {
                            Id = (int)vs.Id,
                            Name = vs.Name,
                            Elevation =
                                Common.ConvertHeight(
                                    maxElevation,
                                    RoundType.ToNearest),
                            Penetrate =
                                Common.ConvertHeight(
                                    maxElevation - Elevation, RoundType.ToNearest),
                            Plane = "Z = " + innerHorElevation,
                            Obstacle = vs,
                            SurfaceElevation = Elevation,
                            GeomType = geomType,
                            GeomPrj = geo,
                            VsType = vs.Type,

                        };
                        _report.Add(obsReport);
                    }
                }
            }

        }

        private IList<Info> _propList;

        public override IList<Info> PropertyList
        {
            get
            {
                _propList = new List<Info>();
                _propList.Add(new Info("Radius",Common.ConvertDistance(Radius, RoundType.ToNearest).ToString(),InitOmega.DistanceConverter.Unit));
                _propList.Add(new Info("Height",Common.ConvertHeight(Height,RoundType.ToNearest).ToString(),InitOmega.HeightConverter.Unit));
                return _propList;
            }
            set { _propList = value; }
        }

        public override PointPenetrateModel GetManualReport(Geometries.Point pt)
        {
            if (Geo.IsPointInside(pt)) 
            {
                var result = new PointPenetrateModel();
                result.Surface = "Inner Horizontal";
                result.Penetration =Common.ConvertHeight(pt.Z - Elevation,RoundType.ToNearest);
                result.Elevation = Common.ConvertHeight(pt.Z,RoundType.ToNearest);
                result.Plane = "Z = " + Common.ConvertHeight(Elevation, RoundType.ToNearest);
                return result;
            }
            return null;
        }

        public Aran.Geometries.MultiPolygon CuttingGeo { get; set; }

    }
}
