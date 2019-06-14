using System.Collections.Generic;
using System.Linq;
using Aran.Aim.Features;
using Aran.Aim;
using Aran.Converters;
using Aran.Geometries;
using Aran.Omega.TypeBEsri.Enums;

namespace Aran.Omega.TypeBEsri.Models
{
    public class OuterHorizontal:SurfaceBase
    {
        private int _cuttingGeoHandle;
        public OuterHorizontal()
        {
            SurfaceType = Aran.Panda.Constants.SurfaceType.OuterHorizontal;
            
            var surfaceModel = CommonFunctions.GetSurfaceModel(SurfaceType);

            if (surfaceModel != null)
            {
                DefaultSymbol = surfaceModel.Symbol;
                SelectedSymbol = surfaceModel.SelectedSymbol;
            }
        }
        public double Radius { get; set; }
        public double ConicRadius { get; set; }
        public double Elevation { get; set; }

        public ConicalSurface Conical { get; set; }
        public InnerHorizontal InnerHorizontal { get; set; }
        public Aran.Geometries.MultiPolygon CuttingGeo { get; set; }

        public override void CreateReport()
        {
            _report = new List<ObstacleReport>();

            double outerElevation = Common.ConvertHeight(Elevation, RoundType.ToNearest);

            //Aran.Geometries.Point adhpPrj =
            //    GlobalParams.SpatialRefOperation.ToPrj(GlobalParams.Database.AirportHeliport.ARP.Geo);

            //var outerList = (from vs in GlobalParams.AdhpObstacleList
            //    let reportList = InnerHorizontal.Report.Union(Conical.Report)
            //    where !(reportList.Any(report => report.Name == vs.Name))
            //    select vs).ToList();

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
                    if (vs.GetGeom(partNumber).Type == Geometries.GeometryType.MultiPolygon)
                        geomType = ObstacleGeomType.Polygon;
                    else if (vs.GetGeom(partNumber).Type == Geometries.GeometryType.MultiLineString)
                        geomType = ObstacleGeomType.PolyLine;

                    if (maxElevation - Elevation > 0)
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
                            Plane = "Z = " + outerElevation,
                            SurfaceElevation = Elevation,
                            Obstacle = vs,
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

                _propList.Add(new Info("Radius", Common.ConvertDistance(Radius, RoundType.ToNearest).ToString(), InitOmega.DistanceConverter.Unit));
                return _propList;
            }
            set { _propList = value; }
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

            _cuttingGeoHandle = GlobalParams.UI.DrawMultiPolygon(CuttingGeo, DefaultSymbol, true, false);
        }
        public override void ClearAll()
        {
            ClearSelected();
            ClearDefault();
            GlobalParams.UI.SafeDeleteGraphic(_cuttingGeoHandle);
        }

        public override PointPenetrateModel GetManualReport(Point obstaclePt)
        {
            var distanceToArp = CommonFunctions.GetDistance(obstaclePt, GlobalParams.TypeBViewModel.Annex14Surfaces.RwyPoints);
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

        public override MultiPolygon GetCuttingGeometry()
        {
            return CuttingGeo;
        }
    }
}
