using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SpatialAnalyst;

namespace SigmaChart
{
    public class CreateAMA
    {
        private IActiveView _activeView;
        private IPageLayout _pageLayout;
        private IMapGrid _mapGrid;
        private IMapFrame _mapFrame;
        private IRaster _selectedRaster;
        private readonly IEnvelope _geoEnvelope;
        const Double Epsilon = 0.00001;
        private SpatialReferenceOperation _spOperation;
        private int _progStepValue;

        public CreateAMA(IActiveView activeView, IPageLayout pageLayout, IMapGrid mapGrid, IRaster raster)
        {
            _activeView = activeView;
            _pageLayout = pageLayout;
            _selectedRaster = raster;
            _mapGrid = mapGrid;
            IRasterLayer newLyr = new RasterLayer();
            newLyr.Name = "Extracted";

            var graphicsContainer = _pageLayout as IGraphicsContainer;
            if (graphicsContainer != null)
            {
                var map = GlobalParams.HookHelper.ActiveView.FocusMap;
                var frameElement = graphicsContainer.FindFrame(map);
                _mapFrame = frameElement as IMapFrame;

            }

            IEnvelope envelope = _mapFrame.MapBounds;
            _spOperation = new SpatialReferenceOperation(0);
            _spOperation.ChangeSpatialReference(envelope.SpatialReference);
            _geoEnvelope = _spOperation.ToEsriGeo(envelope);

            if (raster != null)
            {
                newLyr.CreateFromRaster(raster);
                Terrain = new TerrainClass(newLyr);
            }

            _progStepValue = 10;
        }

        public TerrainClass Terrain { get; }

        public async Task<IList<GridMuraClass>> GetAmaList(bool isObstacle, bool isRaster)
        {
            if (_mapGrid == null)
                throw new Exception("No grid found");

            ProgressHandler("Calculating AMA geometries", null);

            var rasterTask = Task.Factory.StartNew<IList<GridMuraClass>>(() =>
            {
                var gridMuraList = GetPolygonList();

                if (isRaster && Terrain != null)
                    Terrain.Calculate(gridMuraList);
                if (isObstacle)
                    CalculateObstacles(gridMuraList);
                return gridMuraList;
            });

            var result = await rasterTask;

            //Task.WaitAll(taskList);

            return result;
        }

        public IList<GridMuraClass> GetAmaListSync(bool isObstacle, bool isRaster)
        {
            if (_mapGrid == null)
                throw new Exception("No grid found");

            //ProgressHandler("Calculating AMA geometries", null);

            var gridMuraList = GetPolygonList();

           
            if (isRaster && isObstacle)
                _progStepValue = 5;

            if (isRaster)
                Terrain?.Calculate(gridMuraList);
            if (isObstacle)
                CalculateObstacles(gridMuraList);
            
            return gridMuraList;

        }

        public double GetElevation(IPolygon stateGeo)
        {
            //burani duzeltmek lazimdir
            var dataset = _selectedRaster as IGeoDataset2;
            //IGeoDataset dataset = null;
            var operation = (IExtractionOp)(new RasterExtractionOp());

            //IRaster raster = (IRaster) rasterBandCollection;

            //IEnvelope envelope = rasterBandCollection.Extent;
            if (stateGeo == null) return Double.MinValue;

            //  stateGeo.Generalize(0.01);
            var geo = stateGeo;

            //IRaster raster =
            //    (IRaster)operation.Polygon((IGeoDataset)dataset, geo, true);

            //find envelope of polygon

            IRasterProps rasterProps = (IRasterProps)_selectedRaster;
            IEnvelope envStateGeo = geo.Envelope;
            double xMeanCellSize = rasterProps.MeanCellSize().X;
            double yMeanCellSize = rasterProps.MeanCellSize().Y;

            int xCellMin = (int)((envStateGeo.XMin - dataset.Extent.XMin) / xMeanCellSize);
            if (xCellMin < 0)
                xCellMin = 0;

            int xCellMax = (int)((envStateGeo.XMax - dataset.Extent.XMin) / xMeanCellSize);
            if (xCellMax < 0)
                xCellMax = 0;

            int yCellMin = (int)((dataset.Extent.YMax - envStateGeo.YMax) / yMeanCellSize);
            if (yCellMin < 0)
                yCellMin = 0;
            int yCellMax = (int)((dataset.Extent.YMax - envStateGeo.YMin) / yMeanCellSize);
            if (yCellMax < 0)
                yCellMax = 0;
            //

            Terrain.SetExtent(xCellMin, yCellMin, xCellMax, yCellMax);
            // _terrainClass.Calculate();

            var height = Terrain.Height;

            return height;
        }

        private List<GridMuraClass> GetPolygonList()
        {

            if (_mapFrame == null) return null;

            var result = new List<GridMuraClass>();

            var xmin = _geoEnvelope.XMin;
            var ymin = _geoEnvelope.YMin;
            var xMax = _geoEnvelope.XMax;
            var yMax = _geoEnvelope.YMax;

            var measureGrid = _mapGrid as IMeasuredGrid;
            if (measureGrid != null)
            {
                var xIntervalSize = measureGrid.XIntervalSize;
                var yIntervalSize = measureGrid.YIntervalSize;
                var xOrigin = measureGrid.XOrigin;
                var yOrigin = measureGrid.YOrigin;
                var gridUnit = measureGrid.Units;

                var frameGridStartIndexX = Convert.ToInt32(Math.Floor((xmin - xOrigin) / xIntervalSize));
                var frameGridStartIndexY = Convert.ToInt32(Math.Floor((ymin - yOrigin) / yIntervalSize));

                var frameGridEndIndexX = Convert.ToInt32(Math.Ceiling((xMax - xOrigin) / xIntervalSize));
                var frameGridEndIndexY = Convert.ToInt32(Math.Ceiling((yMax - yOrigin) / yIntervalSize));

                double yBottom = ymin;
                int k = 0;
                for (int j = frameGridStartIndexY; j < frameGridEndIndexY; j++)
                {
                    double yTop = (j + 1) * yIntervalSize + yOrigin;
                    if (yTop > yMax)
                        yTop = yMax;

                    double xLeft = xmin;
                    for (int i = frameGridStartIndexX; i < frameGridEndIndexX; i++)
                    {
                        double xRight = (i + 1) * xIntervalSize + xOrigin;
                        if (xRight > xMax)
                            xRight = xMax;

                        IPolygon poly = new PolygonClass();
                        IRing ring = new RingClass();
                        var ptCollection = ring as IPointCollection;

                        ptCollection.AddPoint(new PointClass { X = xLeft, Y = yTop });
                        ptCollection.AddPoint(new PointClass { X = xRight, Y = yTop });
                        ptCollection.AddPoint(new PointClass { X = xRight, Y = yBottom });
                        ptCollection.AddPoint(new PointClass { X = xLeft, Y = yBottom });
                        ptCollection.AddPoint(new PointClass { X = xLeft, Y = yTop });

                        poly.SpatialReference = GlobalParams.HookHelper.FocusMap.SpatialReference;

                        var geometryCollection = poly as IGeometryCollection;
                        geometryCollection.AddGeometry(ring);

                        var gridMura = new GridMuraClass { Geo = poly, XMin = xLeft, XMax = xRight, YMin = yBottom, YMax = yTop };

                        result.Add(gridMura);

                        xLeft = xRight;
                    }
                    yBottom = yTop;
                }
            }
            return result;
        }

        private void CalculateObstacles(List<GridMuraClass> muraClassList)
        {
            try
            {

                ProgressHandler(new ProgClass { Message = "Geting obstacles within Envelope", Value = _progStepValue }, null);

                // var obstacleList = GlobalParams.DbModule.VerticalStructureList;// GetVsWithinEnvelope();
                var obstacleList = GetVsWithinEnvelope();

                int step = 9;
                int count = (int) muraClassList.Count / step;
                count = count == 0 ? muraClassList.Count : count;

                HashSet <PDM.VerticalStructure> findSet = new HashSet<PDM.VerticalStructure>();

                ProgressHandler(new ProgClass { Message = "Calculating obstacles", Value = _progStepValue } , null);

                int j = 0;

                foreach (var gridMuraClass in muraClassList)
                {
                    j++;

                    var gridElevation = gridMuraClass.Elevation.HasValue ? gridMuraClass.Elevation.Value : double.MinValue;
                    for (int i = 0; i < obstacleList.Count; i++)
                    {
                        bool isInside = false;
                        var obstacle = obstacleList[i];
                        //if (findSet.Contains(obstacle)) continue;

                        var muraGeo = _spOperation.ToEsriPrj(gridMuraClass.Geo);
                        foreach (var obstaclePart in obstacle.Parts)
                        {
                            
                            if (obstaclePart.Elev.HasValue)
                            {
                                var obstacleElev = obstaclePart.ConvertValueToMeter(obstaclePart.Elev.Value,
                                    obstaclePart.Elev_UOM.ToString());


                                var partPrj = _spOperation.ToEsriPrj(obstaclePart.Geo);

                                if (obstacleElev > gridElevation)
                                {
                                    if (obstaclePart.Geo != null &&
                                        !GeomOperators.Disjoint(muraGeo,partPrj))
                                    {
                                        gridMuraClass.Elevation = Math.Max(obstacleElev, gridElevation);
                                        gridMuraClass.ObstacleGuid = obstacle.ID;
                                        gridMuraClass.ObstacleName = obstacle.Name;
                                        //findSet.Add(obstacle);
                                        isInside = true;
                                    }
                                }
                            }
                        }
                        if (isInside)
                            break;
                    }

                    if (j % count == 0)
                        ProgressHandler(new ProgClass { Message = "Calcaluted sectors : " + j + " / " + muraClassList.Count, Value = _progStepValue } , null);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
                throw;
            }
        }

        private List<Obstacle> GetObstacleWithinEnvelope()
        {
            var envelope = _geoEnvelope;
            IPointCollection boundsGeo = new Polygon();
            boundsGeo.AddPoint(new PointClass { X = envelope.XMin, Y = envelope.YMin });
            boundsGeo.AddPoint(new PointClass { X = envelope.XMin, Y = envelope.YMax });
            boundsGeo.AddPoint(new PointClass { X = envelope.XMax, Y = envelope.YMax });
            boundsGeo.AddPoint(new PointClass { X = envelope.XMax, Y = envelope.YMin });
            boundsGeo.AddPoint(new PointClass { X = envelope.XMin, Y = envelope.YMin });

            var obstacleList = (from obstacle in GlobalParams.DbModule.ObstacleList
                                from obstaclePart in obstacle.ObstacleParts
                                where obstaclePart.ObstacleType == ObstaclePartType.Point
                                where obstaclePart.Geometry != null
                                where IsPointInside((IPoint)obstaclePart.Geometry.Shape, (IPolygon)boundsGeo)
                                select obstacle).ToList<Obstacle>();

            return obstacleList;
        }

        private List<PDM.VerticalStructure> GetVsWithinEnvelope()
        {
            var envelope = _geoEnvelope;
            IPointCollection boundsGeo = new Polygon();
            boundsGeo.AddPoint(new PointClass { X = envelope.XMin, Y = envelope.YMin });
            boundsGeo.AddPoint(new PointClass { X = envelope.XMin, Y = envelope.YMax });
            boundsGeo.AddPoint(new PointClass { X = envelope.XMax, Y = envelope.YMax });
            boundsGeo.AddPoint(new PointClass { X = envelope.XMax, Y = envelope.YMin });
            boundsGeo.AddPoint(new PointClass { X = envelope.XMin, Y = envelope.YMin });

            ProgressHandler(this, null);

            var vsSortSet = new List<PDM.VerticalStructure>();
            foreach (var obstacle in GlobalParams.DbModule.VerticalStructureList)
            {
                var obstacleElev = 0.0;
                var isInside =false;
                foreach (var vsPart in obstacle.Parts)
                {
                    if (vsPart.Geo == null)
                        vsPart.RebuildGeo();

                    IGeometry partGeo = vsPart.Geo;

                    if (partGeo == null ||!vsPart.Elev.HasValue || vsPart.Elev.Value<Epsilon) continue;

                    obstacleElev =Math.Max(vsPart.ConvertValueToMeter(vsPart.Elev.Value,
                                           vsPart.Elev_UOM.ToString()),obstacleElev);
                    if (!GeomOperators.Disjoint(boundsGeo as IGeometry, partGeo))
                    {
                        obstacle.Elev = obstacleElev;
                        isInside = true;
                    }
                }
                if (isInside)
                        vsSortSet.Add(obstacle);
            }

            vsSortSet.Sort(new VsComparer());
            return vsSortSet;
        }

        private bool IsPointInside(IPoint p, IPolygon poly)
        {
            IPointCollection ptCollection = poly as IPointCollection;
            int cnt = ptCollection.PointCount;

            if (cnt > 1)
            {
                double d = Math.Abs(ptCollection.Point[cnt - 1].X - ptCollection.Point[0].X) + Math.Abs(ptCollection.Point[cnt - 1].Y - ptCollection.Point[0].Y);
                if (d < Epsilon)
                    cnt--;
            }

            if (cnt < 3)
                return false;

            double x = p.X;
            double y = p.Y;

            double x1 = ptCollection.Point[cnt - 1].X;		// Start with the last edge of p
            double y1 = ptCollection.Point[cnt - 1].Y;

            int numOfCrossings = 0;				// Count of poly's edges crossed

            //  For each edge e of polygon p, see if the ray from (x, y) to (infinity, y)
            //  crosses e:

            for (int i = 0; i < cnt; i++)
            {
                double x2 = ptCollection.Point[i].X;
                double y2 = ptCollection.Point[i].Y;

                // If y is between (y1, y2] (e's y-range),
                // and (x,y) is to the left of e, then
                //     the ray crosses e:

                if ((((y2 <= y) && (y < y1)) || ((y1 <= y) && (y < y2)))
                && (x < x2 + (x1 - x2) * (y - y2) / (y1 - y2)))
                    numOfCrossings++;

                x1 = x2;
                y1 = y2;
            }

            return (numOfCrossings & 1) == 1;
        }

        public event EventHandler ProgressHandler = delegate { };
    }

    public class VsComparer : IComparer<PDM.VerticalStructure> 
    {
        public int Compare(PDM.VerticalStructure x, PDM.VerticalStructure y)
        {
            if (x.Elev == y.Elev)
                return 0;

            if (x.Elev < y.Elev)
                return 1;
            else
                return -1;
        }
    }


}
