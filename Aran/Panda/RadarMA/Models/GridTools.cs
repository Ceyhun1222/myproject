using System;
using System.Collections.Generic;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.Models
{

	[System.Runtime.InteropServices.ComVisible(false)]
	public class GridTools
	{
	    public GridTools()
	    {
            RasterLayers = new List<TerrainClass>();
            FillGridCombo();
	    }
	    public List<TerrainClass> RasterLayers { get; private set; }


        private void FillGridCombo()
        {
            RasterLayers.Clear();

            for (int i = 0; i < GlobalParams.Map.LayerCount; i++)
            {
                ILayer layer = GlobalParams.Map.Layer[i];
                if (layer is IRasterLayer)
                {
                    try
                    {
                        IRasterLayer rlayer = (IRasterLayer)layer;
                        IRasterProps RasterProps = (IRasterProps)rlayer.Raster; //RLayer.Raster.QueryInterface(IRasterProps, RasterProps);
                    }
                    catch
                    {
                        continue;
                    }

                    RasterLayers.Add(new TerrainClass(layer as IRasterLayer));
                }
            }
           // AnalysesRaster(RasterLayers[0].RasterLayer);
        }

        private void AnalysesRaster(IRasterLayer layer)
        {
            IRasterLayer rlayer = layer;// RasterLayers[comboBox8.SelectedIndex];
            IRasterProps RasterProps = (IRasterProps)rlayer.Raster;
            IRaster2 Raster2 = (IRaster2)rlayer.Raster;
            IPnt PntCellSize = RasterProps.MeanCellSize();

            double fNoDataValue =-1;

            try
            {
                var noData =(Array) RasterProps.NoDataValue;
                //fNoDataValue = noData[0];
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

                double[,] approxiMap = new double[GridHeight, GridWidth];

                double MaxVal = double.MinValue;
                double MinVal = double.MaxValue;

                for (int j = 0; j < GridHeight; j++)
                {
                    //ProgressBar.Progress = j;
                    for (int i = 0; i < GridWidth; i++)
                    {
                        object PixVal = pPixelBlock.GetVal(0, i, j);

                        approxiMap[GridHeight - j - 1, i] = double.NaN;
                        if (PixVal != null)
                        {
                            double fDixVal = Convert.ToDouble(PixVal);

                            approxiMap[GridHeight - j - 1, i] = fDixVal;
                            if (MaxVal < fDixVal) MaxVal = fDixVal;
                            if (MinVal > fDixVal) MinVal = fDixVal;
                        }
                        else
                            approxiMap[GridHeight - j - 1, i] = fNoDataValue;
                    }
                }

            //    int nVal = (int)Math.Ceiling((MaxVal - MinVal) / dVal);
               // MinVal = Math.Floor(MinVal / dVal) * dVal;

                //pbI.Process(envelope.XMin, PntCellSize.X, GridWidth, envelope.YMin, PntCellSize.Y, GridHeight, MinVal, dVal, nVal, approxiMap);
            }
            catch
            {

            }

            try
            {
                IGeoDataset pGeoDS = (IGeoDataset)rlayer;
                //CommonTools.pSpRefShp = pGeoDS.SpatialReference;
                //pbI.SaveResult();
            }
            catch
            {
            }
        }

	}
}