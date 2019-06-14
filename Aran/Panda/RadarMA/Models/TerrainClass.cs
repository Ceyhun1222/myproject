using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.Models
{
    public class TerrainClass
    {
        private double[,] _approxiMap;
        private IPoint[,] _mappCoordinates;

        public TerrainClass(IRasterLayer rasterLayer)
        {
            RasterLayer = rasterLayer;
        }
        public IRasterLayer RasterLayer { get;}

        public string Name => RasterLayer.Name;

        public double[,] ApproxiMap
        {
            get
            {
                if (_approxiMap==null)
                    AnalysesRaster(RasterLayer.Raster);
                return _approxiMap;
            }
        }

        public IPoint[,] MappCoordinates
        {
            get
            {
                if (_mappCoordinates == null)
                    AnalysesRaster(RasterLayer.Raster);
                return _mappCoordinates;
            }
        }

        public IPoint HeightstPoint { get; set; }
        public double Height { get; set; }
        public int XMin { get; set; }
        public int XMax { get; set; }

        public int YMin { get; set; }
        public int YMax { get; set; }

        public void Calculate()
        {
            var obstacleReports = new List<ObstacleReport>();
            
            var raster = RasterLayer.Raster;
            var rasterProps = (IRasterProps) raster;
            var raster2 = (IRaster2) raster;

            var gridWidth = rasterProps.Width;
            int gridHeight = rasterProps.Height;

            IPixelBlockCursor pPixelBlockCursor = new PixelBlockCursor();
            pPixelBlockCursor.InitByRaster(raster);
            pPixelBlockCursor.ScanMode = 0;
            pPixelBlockCursor.UpdateBlockSize(gridWidth, gridHeight);

            int L = 0, T = 0, W = 0, H = 0;
            IPixelBlock pPixelBlock = pPixelBlockCursor.NextBlock(ref L, ref T, ref W, ref H);

            if (XMax > W) XMax = W;
            if (YMax > H) YMax = H;

            double maxVal = double.MinValue;
            IPoint heighstPoint = null;

            for (int j = YMin; j < YMax; j++)
            {
                for (int i = XMin; i < XMax; i++)
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

                            heighstPoint = new PointClass {X = refX, Y = refY};
                        }
                    }
                }
            }

            Height = maxVal;
            HeightstPoint = heighstPoint;

        }

        public void AnalysesRaster(IRaster raster)
        {
            //IRasterLayer rlayer = layer;// RasterLayers[comboBox8.SelectedIndex];
            IRasterProps RasterProps = (IRasterProps) raster;
            IRaster2 Raster2 = (IRaster2) raster;
            IPnt PntCellSize = RasterProps.MeanCellSize();
            double x = PntCellSize.X;
            double y = PntCellSize.Y;


            double fNoDataValue =-1;

            try
            {
                //fNoDataValue = RasterProps.NoDataValue[0];
            }
            catch
            {
                fNoDataValue = System.Convert.ToDouble(RasterProps.NoDataValue);
            }

            int GridWidth = RasterProps.Width;
            int GridHeight = RasterProps.Height;

            //GridProcessor pbI = new GridProcessor();
            IEnvelope envelope = RasterProps.Extent;
            //IRasterMapModel rasterMapModel = RasterProps.MapModel;

            IPixelBlockCursor pPixelBlockCursor = new PixelBlockCursor();
            pPixelBlockCursor.InitByRaster(Raster2 as IRaster);
            pPixelBlockCursor.ScanMode = 0;
            pPixelBlockCursor.UpdateBlockSize(GridWidth, GridHeight);

            try
            {
                int L = 0, T = 0, W = 0, H = 0;
                IPixelBlock pPixelBlock = pPixelBlockCursor.NextBlock(ref L, ref T, ref W, ref H);

                _approxiMap = new double[GridHeight, GridWidth];
                _mappCoordinates = new IPoint[GridHeight, GridWidth];

                double MaxVal = double.MinValue;
                double MinVal = double.MaxValue;

                Stopwatch st = new Stopwatch();
                st.Start();

                int nullCount = 0;
                for (int j = 0; j < GridHeight; j++)
                {
                    //ProgressBar.Progress = j;
                    for (int i = 0; i < GridWidth; i++)
                    {
                        object PixVal =pPixelBlock.GetVal(0, i, j);

                        _approxiMap[GridHeight - j - 1, i] = double.NaN;
                        _mappCoordinates[GridHeight - j - 1, i] = null;
                        if (PixVal != null)
                        {
                            double fDixVal =Convert.ToDouble(PixVal);

                            _approxiMap[GridHeight - j - 1, i] = fDixVal;
                            if (MaxVal < fDixVal) MaxVal = fDixVal;
                            if (MinVal > fDixVal) MinVal = fDixVal;

                            double refX = 0, refY = 0;
                            Raster2.PixelToMap(i, j, out refX, out refY);

                            _mappCoordinates[GridHeight - j - 1, i] = new PointClass { X = refX, Y = refY };
                        }
                        else
                        {
                            _approxiMap[GridHeight - j - 1, i] = fNoDataValue;
                            _mappCoordinates[GridHeight - j - 1, i] = null;
                            nullCount++;
                        }
                    }
                }

                st.Stop();
                MessageBox.Show(st.Elapsed.ToString()+"  ,"+nullCount.ToString());
                //    int nVal = (int)Math.Ceiling((MaxVal - MinVal) / dVal);
                // MinVal = Math.Floor(MinVal / dVal) * dVal;

                //pbI.Process(envelope.XMin, PntCellSize.X, GridWidth, envelope.YMin, PntCellSize.Y, GridHeight, MinVal, dVal, nVal, approxiMap);
            }
            catch
            {

            }
            
        }


        internal void SetExtent(int xCellMin, int yCellMin, int xCellMax, int yCellMax)
        {
            XMin = xCellMin;
            YMin = yCellMin;

            XMax = xCellMax;
            YMax = yCellMax;
        }
    }

}
