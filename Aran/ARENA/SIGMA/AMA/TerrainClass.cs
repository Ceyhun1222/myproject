using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SpatialAnalyst;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SigmaChart
{
    public class TerrainClass
    {
        private IPixelBlockCursor _pPixelBlockCursor;
        private int _gridWidth;
        private int _gridHeight;
        private IRaster _selectedRaster;
        private double _xMeanCellSize;
        private double _yMeanCellSize;

        public TerrainClass(IRasterLayer rasterLayer)
        {
            _selectedRaster = rasterLayer.Raster;
            var RasterProps = (IRasterProps)_selectedRaster;

            _gridWidth = RasterProps.Width;
            _gridHeight = RasterProps.Height;

            _pPixelBlockCursor = new PixelBlockCursor();
            _pPixelBlockCursor.InitByRaster(_selectedRaster);

            var rasterProps = (IRasterProps)_selectedRaster;
            _xMeanCellSize = rasterProps.MeanCellSize().X;
            _yMeanCellSize = rasterProps.MeanCellSize().Y;
        }

        ~TerrainClass()
        {
            Marshal.ReleaseComObject(_selectedRaster);
            

        }

        public IPoint HeightstPoint { get; set; }
        public double Height { get; set; }
        public int XMin { get; set; }
        public int XMax { get; set; }

        public int YMin { get; set; }
        public int YMax { get; set; }

        public void Calculate(List<GridMuraClass> gridMuraList)
        {
            _pPixelBlockCursor.ScanMode = 0;
            _pPixelBlockCursor.UpdateBlockSize(_gridWidth, _gridHeight);

            int L = 0, T = 0, W = 0, H = 0;
            IPixelBlock pPixelBlock = _pPixelBlockCursor.NextBlock(ref L, ref T, ref W, ref H);

            int a = 0;
            try
            {
                var heighstPoint = new PointClass();

                int step = 9;
                int count = (int)gridMuraList.Count / step;
                count = count == 0 ? gridMuraList.Count : count;

                ProgressHandler(new ProgClass { Message = "Calculating terrain.. : " + a + "/" + gridMuraList.Count, Value = 5 }, null);

                foreach (var gridMuraClass in gridMuraList)
                {
                    a++;
                    if (GetGridMuraExtent(gridMuraClass))
                    {
                        if (XMax > W) XMax = W;
                        if (YMax > H) YMax = H;

                        double maxVal = double.MinValue;

                        for (int j = YMin; j < YMax; j++)
                        {
                            for (int i = XMin; i < XMax; i++)
                            {
                                object PixVal = pPixelBlock.GetVal(0, i, j);

                                if (PixVal != null)
                                {
                                    double fDixVal = Convert.ToDouble(PixVal);
                                    maxVal = Math.Max(maxVal, fDixVal);
                                }
                            }
                        }
                        if (maxVal!=double.MinValue)
                            gridMuraClass.Elevation = maxVal;
                    }
                    if (a % count == 0)
                        ProgressHandler(new ProgClass { Message = "Terrain calc sectors : " + a + "/" + gridMuraList.Count, Value = 5 }, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Marshal.ReleaseComObject(_pPixelBlockCursor);
                Marshal.ReleaseComObject(pPixelBlock);
            }
        }

        internal void SetExtent(int xCellMin, int yCellMin, int xCellMax, int yCellMax)
        {
            XMin = xCellMin;
            YMin = yCellMin;

            XMax = xCellMax;
            YMax = yCellMax;
        }

        public bool GetGridMuraExtent(GridMuraClass gridMura)
        {
            IGeoDataset2 dataset = _selectedRaster as IGeoDataset2;
            IExtractionOp operation = (IExtractionOp)(new RasterExtractionOp());

            if (gridMura == null) throw new Exception("Grid Mura cannot be null!");

            //find envelope of polygon

            int xCellMin = (int)((gridMura.XMin - dataset.Extent.XMin) / _xMeanCellSize);
            if (xCellMin < 0)
                xCellMin = 0;

            int xCellMax = (int)((gridMura.XMax - dataset.Extent.XMin) / _xMeanCellSize);
            if (xCellMax < 0)
                xCellMax = 0;

            int yCellMin = (int)((dataset.Extent.YMax - gridMura.YMax) / _yMeanCellSize);
            if (yCellMin < 0)
                yCellMin = 0;
            int yCellMax = (int)((dataset.Extent.YMax - gridMura.YMin) / _yMeanCellSize);
            if (yCellMax < 0)
                yCellMax = 0;
            //

            //Grid cell in left side from Raster
            if (gridMura.XMin < dataset.Extent.XMin && gridMura.XMax < dataset.Extent.XMin)
                return false;

            //Grid cell in right side from Raster
            if (gridMura.XMin > dataset.Extent.XMax)
                return false;

            //Grid cell in bottom from Raster
            if (gridMura.YMin < dataset.Extent.YMin && gridMura.YMax < dataset.Extent.YMin)
                return false;

            if (gridMura.YMin > dataset.Extent.YMax)
                return false;

            SetExtent(xCellMin, yCellMin, xCellMax, yCellMax);
            return true;
        }

        public event EventHandler ProgressHandler = delegate { };

    }
}
