using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RadarMA.Models;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SpatialAnalyst;

namespace Aran.Panda.RadarMA.ElevationCalculator
{
    class RasterObstacleReportCalculator:IObstacleReportCalculator
    {
        private readonly IRaster _raster;
        private UnitConverter _unitConverter;

        public RasterObstacleReportCalculator(IRaster raster,UnitConverter unitConverter)
        {
            _raster = raster;
            _unitConverter = unitConverter;
        }
        public IEnumerable<ObstacleReport> GetReports(IPolygon stateGeo)
        {
            var dataset = (IGeoDataset)_raster;
            if (dataset == null)
                return new List<ObstacleReport>();

            IExtractionOp operation = (IExtractionOp)(new RasterExtractionOp());

            if (stateGeo == null || stateGeo.IsEmpty)
                throw new ArgumentNullException($"State geometry is null or empty");
            
            var geo = GlobalParams.SpatialRefOperation.ToEsriGeo(stateGeo);

            IRaster raster =
                (IRaster)operation.Polygon((IGeoDataset)dataset, geo, true);

            //find envelope of polygon

            var extentGeo = FindExtent(geo.Envelope, dataset.Extent);

            //

            IRasterLayer newLyr = new RasterLayer();
            newLyr.Name = "Extracted";
            newLyr.CreateFromRaster(raster);

            return Calculate(newLyr, extentGeo);
        }

        private IEnumerable<ObstacleReport> Calculate(IRasterLayer rasterLayer,Extent extent)
        {
            var raster = rasterLayer.Raster;
            var rasterProps = (IRasterProps)raster;
            var raster2 = (IRaster2)raster;

            var gridWidth = rasterProps.Width;
            int gridHeight = rasterProps.Height;

            IPixelBlockCursor pPixelBlockCursor = new PixelBlockCursor();
            pPixelBlockCursor.InitByRaster(raster);
            pPixelBlockCursor.ScanMode = 0;
            pPixelBlockCursor.UpdateBlockSize(gridWidth, gridHeight);

            int L = 0, T = 0, W = 0, H = 0;
            IPixelBlock pPixelBlock = pPixelBlockCursor.NextBlock(ref L, ref T, ref W, ref H);

            int xMin = extent.XMin;
            int xMax = extent.XMax;
            int yMin = extent.YMin;
            int yMax = extent.YMax;

            if (xMax > W) xMax = W;
            if (yMax > H) yMax = H;

            double maxVal = double.MinValue;

            for (int j = yMin; j < yMax; j++)
            {
                for (int i = xMin; i < xMax; i++)
                {
                    object pixVal = pPixelBlock.GetVal(0, i, j);

                    if (pixVal != null)
                    {
                        double fDixVal = Convert.ToDouble(pixVal);

                        if (maxVal < fDixVal)
                        {
                            maxVal = fDixVal;
                            double refX, refY;
                            raster2.PixelToMap(i, j, out refX, out refY);

                            var heightInDisplayUnit = _unitConverter.HeightToDisplayUnits(fDixVal);  

                            yield return new ObstacleReport(i+"x "+j+"y",new PointClass { X = refX, Y = refY },null, heightInDisplayUnit, "Terrain");
                        }
                    }
                }
            }
        }

        private Extent FindExtent(IEnvelope envStateGeo, IEnvelope extentDataSet)
        {
            IRasterProps rasterProps = (IRasterProps)_raster;
            double xMeanCellSize = rasterProps.MeanCellSize().X;
            double yMeanCellSize = rasterProps.MeanCellSize().Y;

            int xCellMin = Convert.ToInt32((envStateGeo.XMin - extentDataSet.XMin) / xMeanCellSize);
            if (xCellMin < 0)
                xCellMin = 0;

            int xCellMax = Convert.ToInt32((envStateGeo.XMax - extentDataSet.XMin) / xMeanCellSize);
            if (xCellMax < 0)
                xCellMax = 0;

            int yCellMin = Convert.ToInt32((extentDataSet.YMax - envStateGeo.YMax) / yMeanCellSize);
            if (yCellMin < 0)
                yCellMin = 0;
            int yCellMax = Convert.ToInt32((extentDataSet.YMax - envStateGeo.YMin) / yMeanCellSize);
            if (yCellMax < 0)
                yCellMax = 0;

            return new Extent(xCellMin,yCellMin,xCellMax,yCellMax);
        }
    }

    public class Extent
    {
        public Extent(int xMin, int yMin, int xMax, int yMax)
        {
            XMin = xMin;
            YMin = yMin;
            XMax = xMax;
            YMax = yMax;
        }

        public int XMin { get; set; }

        public int YMin { get; set; }

        public int XMax { get; set; }

        public int YMax { get; set; }


    }
}
