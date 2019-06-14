using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using System.Diagnostics;
using System.Windows.Forms;
using Aran.Geometries.Operators;

namespace Holding
{
    public class HoldingReport
    {
        #region :>Fields
        private HoldingGeometry _holdingGeom;
        private double _moc;
        #endregion
        
        #region :>Constructor
        public HoldingReport(HoldingGeometry holdingGeom, double altitude, double moc)
        {
            _holdingGeom = holdingGeom;
            _moc = moc;
            Altitude = altitude;
            GetObstacleList();
        }
        #endregion

        #region :>Property
        
        public BindingList<Report> ObstacleReport { get; set; }

        public BindingList<Report> PenetratedObstacleList { get; set; }

        public int ReportCount { get; private set; }

        public List<VerticalStructure> VerticalStructureList 
        {
            get
            {
                return ObstacleReport.Select(rep => rep.Obstacle).ToList<VerticalStructure>();
            }
        }

        public bool LastReport { get; set; }

        public double Moc 
        {
            get { return Common.ConvertHeight(_moc,roundType.realValue); }
        }

        public double Altitude { get;private set; }

        #endregion

        #region :>Methods

        public void CreateReport(HoldingGeometry holdingGeom, double altitude, double moc)
        {
            _holdingGeom = holdingGeom;
            Altitude = altitude;
            _moc = moc;
            GetObstacleList();
        }

        public void SetHReport(double moc)
        {
            foreach (var item in ObstacleReport)
            {
                double prevMoc = Common.DeConvertHeight(item.Moc);
                double elev = Common.DeConvertHeight(item.Elevation);
                double curMoc = prevMoc + (moc - _moc);
                item.Moc = Common.ConvertHeight(curMoc,roundType.toUp);
                item.Penetrate =Common.ConvertHeight(elev + curMoc-Altitude, roundType.toNearest);                                 
                item.Req_H = Math.Round(Common.ConvertHeight(elev+curMoc,roundType.toUp),0);
                item.Validation = Common.ConvertHeight(Altitude - elev - curMoc, roundType.toNearest)>=0;
            }
            _moc = moc;
        }

        private void GetObstacleList()
        {
            IGeometryOperators geomOperators = new JtsGeometryOperators();
            var elevation = 0;

            var minMaxPoint = TransForm.QueryCoords(_holdingGeom.FullArea);
            var ptMin = new Aran.Geometries.Point(minMaxPoint.XMin, minMaxPoint.YMin);
            var ptMax = new Aran.Geometries.Point(minMaxPoint.XMax, minMaxPoint.YMax);
            var extent = new Aran.Geometries.MultiPolygon { GeomFunctions.CreateExtent(ptMin.X, ptMin.Y, ptMax.X, ptMax.Y) };

            Aran.Geometries.MultiPolygon extentGeo = GlobalParams.SpatialRefOperation.ToGeo(extent);

            var verticalStructureList = GlobalParams.Database.HoldingQpi.GetVerticalStructureList(extentGeo[0]);
            
            var reportList = new List<Report>();
            int j = 0;
            try
            {
                foreach (var vs in verticalStructureList)
                {
                    j++;
                    double vsMoc = 0;
                    double maxPenetrate = -10000;
                    var vsAreaNumber = 0;
                    bool isIntersect = false;
                    double vsElevation = 0;
                    var vsGeomType = ObstacleGeomType.Point;
                    var vsSurfaceType = ObstactleReportType.BasicArea;
                    Aran.Geometries.Geometry vsGeom = null;
                    var vsHorAccuracy = 0.0;
                    var vsVerAccuracy = 0.0;
                    foreach (var vsPart in vs.Part)
                    {
                        Aran.Geometries.Geometry partGeo = null;
                        double partElevation = 0;
                        var horProjection = vsPart.HorizontalProjection;
                        var partGeomType = ObstacleGeomType.Point;
                        double partVerAccuracy = 0;
                        double partHorAccuracy = 0;
                        if (horProjection.Choice == Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedPoint)
                        {
                            if (horProjection.Location == null || horProjection.Location.Geo == null)
                            {
                                continue;
                            }
                            else
                            {
                                partGeo = GlobalParams.SpatialRefOperation.ToPrj(horProjection.Location.Geo);
                                if (partGeo.IsEmpty)
                                    continue;
                                partElevation = Aran.Converters.ConverterToSI.Convert(horProjection.Location.Elevation, 0);
                                partVerAccuracy = Aran.Converters.ConverterToSI.Convert(horProjection.Location.VerticalAccuracy, 0);
                                partHorAccuracy = Aran.Converters.ConverterToSI.Convert(horProjection.Location.HorizontalAccuracy, 0);
                            }
                        }
                        else if (horProjection.Choice == Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedSurface)
                        {
                            if (horProjection.SurfaceExtent == null || horProjection.SurfaceExtent.Geo == null)
                            {
                                continue;
                            }
                            else
                            {
                                partGeo = GlobalParams.SpatialRefOperation.ToPrj(horProjection.SurfaceExtent.Geo);
                                if (partGeo.IsEmpty)
                                    continue;
                                partElevation = Aran.Converters.ConverterToSI.Convert(horProjection.SurfaceExtent.Elevation, 0);
                                partGeomType = ObstacleGeomType.Polygon;
                                partVerAccuracy = Aran.Converters.ConverterToSI.Convert(horProjection.SurfaceExtent.VerticalAccuracy, 0);
                                partHorAccuracy = Aran.Converters.ConverterToSI.Convert(horProjection.SurfaceExtent.HorizontalAccuracy, 0);
                            }
                        }
                        else if (horProjection.Choice == Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedCurve)
                        {
                            if (horProjection.LinearExtent == null || horProjection.LinearExtent.Geo == null)
                            {
                                continue;
                            }
                            else
                            {
                                partGeo = GlobalParams.SpatialRefOperation.ToPrj(horProjection.LinearExtent.Geo);
                                if (partGeo.IsEmpty)
                                    continue;
                                partElevation = Aran.Converters.ConverterToSI.Convert(horProjection.LinearExtent.Elevation, 0);
                                partVerAccuracy = Aran.Converters.ConverterToSI.Convert(horProjection.LinearExtent.VerticalAccuracy, 0);
                                partHorAccuracy = Aran.Converters.ConverterToSI.Convert(horProjection.LinearExtent.HorizontalAccuracy, 0);
                                partGeomType = ObstacleGeomType.PolyLine;
                            }
                        }

                        if (!geomOperators.Disjoint(_holdingGeom.AreaWithSectors, partGeo))
                        {
                            var tmpPenetrate = partElevation + _moc+partVerAccuracy - Altitude;

                            if (maxPenetrate < tmpPenetrate)
                            {
                                maxPenetrate = tmpPenetrate;
                                vsMoc = _moc;
                                vsSurfaceType = ObstactleReportType.BasicArea;
                                vsAreaNumber = 0;
                                vsElevation = partElevation;
                                vsGeom = partGeo;
                                vsHorAccuracy = partHorAccuracy;
                                vsVerAccuracy = partVerAccuracy;
                                vsGeomType = partGeomType;
                            }

                            isIntersect = true;

                        }
                        else
                        {
                            var distance = geomOperators.GetDistance(_holdingGeom.AreaWithSectors, partGeo) - partHorAccuracy;
                            if (distance > 5 * BaseArea.areaWidth)
                                continue;
                            var partAreaNumber = (int)Math.Floor(distance / BaseArea.areaWidth)+1;
                            var curMoc = 0.0;
                            if (partAreaNumber > 1)
                                curMoc = _moc - 150 - 30 * (partAreaNumber - 2);
                            else
                                curMoc = _moc;

                            var tmpPenetrate = elevation + curMoc+partVerAccuracy - Altitude;
                            if (maxPenetrate < tmpPenetrate)
                            {
                                maxPenetrate = tmpPenetrate;
                                vsMoc = curMoc;
                                vsSurfaceType = ObstactleReportType.SecondaryArea;
                                vsAreaNumber = partAreaNumber;
                                vsElevation = partElevation;
                                vsHorAccuracy = partHorAccuracy;
                                vsVerAccuracy = partVerAccuracy;
                                vsGeom = partGeo;
                                vsGeomType = partGeomType;
                            }

                            isIntersect = true;
                        }
                    }
                    if (isIntersect)
                    {
                        var vsReport = new Report();
                        vsReport.Id = (int)vs.Id;
                        vsReport.Name = vs.Name;
                        vsReport.Elevation = Common.ConvertHeight(vsElevation, roundType.toUp);
                        vsReport.Moc = Math.Round(Common.ConvertHeight(vsMoc, roundType.toUp), 0);
                        vsReport.Req_H = Common.ConvertHeight(vsElevation + vsMoc, roundType.toUp);
                        vsReport.Penetrate = Common.ConvertHeight(vsElevation + vsMoc - Altitude, roundType.toUp);
                        vsReport.Validation = vsReport.Penetrate <= 0;
                        vsReport.SurfaceType = vsSurfaceType;
                        vsReport.Area = vsSurfaceType == ObstactleReportType.BasicArea ? "Primary " : "Secondary ";
                        vsReport.Obstacle = vs;
                        vsReport.AreaNumber = vsAreaNumber;
                        vsReport.GeomType = vsGeomType;
                        vsReport.GeomPrj = vsGeom;
                        vsReport.HorAccuracy = vsHorAccuracy;
                        vsReport.VerAccuracy = vsVerAccuracy;
                        reportList.Add(vsReport);
                    }
                }
            }
            catch (Exception e)
            {
                GlobalParams.AranEnvironment.GetLogger("Rnav Holding").Error(e);
                System.Windows.Forms.MessageBox.Show(j.ToString());
            }
            
            ObstacleReport = new BindingList<Report>(reportList);
            ObstacleReport.AllowEdit = true;
            ObstacleReport.AllowNew = false;
            ObstacleReport.AllowRemove = false;
            ReportCount = reportList.Count;

            PenetratedObstacleList =
                new BindingList<Report>(ObstacleReport.Where(obstacle => obstacle.Penetrate>=0).ToList());
            PenetratedObstacleList.AllowEdit = true;
            PenetratedObstacleList.AllowNew = false;
            PenetratedObstacleList.AllowRemove = false;
        }

        #endregion

        #region vExtent
          //obstacleVerticalExtent = ConvertToDistance.ConvertDistance(vExtent);
         //Req_H = Math.Round(Common.ConvertHeight(_altitude - elevation-obstacleVerticalExtent-_moc,roundType.toNearest),0),
        #endregion

       
    }
    
}
