using System;
using System.IO;
using Aran.Panda.RadarMA.Models;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.Utils
{
    public class EsriFunctions
    {
        public static ILayer GetLayerByName(string layerName)
        {
            var upLayerName = layerName.ToUpper();
            for (int i = 0; i < GlobalParams.Map.LayerCount; i++)
            {
                var layer = GlobalParams.Map.Layer[i];
                if (layer.Name.ToUpper() == upLayerName)
                    return layer;
            }
            return null;

        }

        public static System.Windows.Forms.Cursor GetCursor(string name)
        {
            string cursorFileName = @"..\images\" + name + ".cur";// Application.LocalUserAppDataPath + "\\" + name + ".cur";

            if (!File.Exists(cursorFileName))
            {
                FileStream fs = new FileStream(cursorFileName, FileMode.Create, FileAccess.Write);

                object obj = Properties.Resources.ResourceManager.GetObject(name);

                var data = (byte[])obj;
                fs.Write(data, 0, data.Length);
                fs.Close();
            }

            IntPtr imgHandle = Win32.LoadImage(IntPtr.Zero,
                cursorFileName,
                Win32.IMAGE_CURSOR,
                0,
                0,
                Win32.LR_LOADFROMFILE);

            return new System.Windows.Forms.Cursor(imgHandle);
        }

        public static bool IsInside(IPolygon poly, IGeometry insideGeo)
        {
            IRelationalOperator relOper = poly as IRelationalOperator;
            return relOper != null && !relOper.Disjoint(insideGeo);
        }

        public static ISymbol CreatePolygonSymbol(int lineSize, esriSimpleFillStyle simpleFillStyle)
        {
            IRgbColor pRGB = null;

            pRGB = new RgbColor();
            //pRGB.RGB = Aran.Panda.Common.ARANFunctions.RGB(128, 128, 128);
            ISimpleFillSymbol pFillSym = new SimpleFillSymbol();

            pFillSym.Color = pRGB;
            pFillSym.Style = simpleFillStyle;
            //(pFillSym as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen; // esriSFSNull 'esriSFSDiagonalCross

            ILineSymbol pLineSimbol = new SimpleLineSymbol();

            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = 128;

            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = lineSize;
            pFillSym.Outline = pLineSimbol;

            return pFillSym as ISymbol;

        }

        //public static double GetRasterElevation(IPolygon stateGeo, ref IPoint ptMaxElevationPoint)
        //{
        //    var dataset =(IGeoDataset) GlobalParams.SelectedRaster;
        //    if (dataset == null)
        //        return double.MinValue;
        //    IExtractionOp operation = (IExtractionOp)(new RasterExtractionOp());

        //    //IRaster raster = (IRaster) rasterBandCollection;

        //    //IEnvelope envelope = rasterBandCollection.Extent;
        //    if (stateGeo == null) return Double.MinValue;

        //    //  stateGeo.Generalize(0.01);
        //    var geo = (IPolygon)GlobalParams.SpatialRefOperation.ToEsriGeo(stateGeo);

        //    IRaster raster =
        //        (IRaster)operation.Polygon((IGeoDataset)dataset, geo, true);

        //    //find envelope of polygon

        //    IRasterProps rasterProps = (IRasterProps)raster;
        //    IEnvelope envStateGeo = geo.Envelope;
        //    double xMeanCellSize = rasterProps.MeanCellSize().X;
        //    double yMeanCellSize = rasterProps.MeanCellSize().Y;

        //    int xCellMin = Convert.ToInt32((envStateGeo.XMin - dataset.Extent.XMin) / xMeanCellSize);
        //    if (xCellMin < 0)
        //        xCellMin = 0;

        //    int xCellMax = Convert.ToInt32((envStateGeo.XMax - dataset.Extent.XMin) / xMeanCellSize);
        //    if (xCellMax < 0)
        //        xCellMax = 0;

        //    int yCellMin = Convert.ToInt32((dataset.Extent.YMax - envStateGeo.YMax) / yMeanCellSize);
        //    if (yCellMin < 0)
        //        yCellMin = 0;
        //    int yCellMax = Convert.ToInt32((dataset.Extent.YMax - envStateGeo.YMin) / yMeanCellSize);
        //    if (yCellMax < 0)
        //        yCellMax = 0;
        //    //

        //    IRasterLayer newLyr = new RasterLayer();
        //    newLyr.Name = "Extracted";
        //    newLyr.CreateFromRaster(raster);
        //    TerrainClass terrainClass = new TerrainClass { RasterLayer = newLyr };
        //    terrainClass.SetExtent(xCellMin, yCellMin, xCellMax, yCellMax);
        //    terrainClass.Calculate();
        //    ptMaxElevationPoint = terrainClass.HeightstPoint;

        //    return terrainClass.Height;
        //}

        public static string DD2DMSText(double val)
        {
            double x;
            double dx;
            double xDeg, xMin, xSec;
            double Sign = 1;

            x = System.Math.Abs(System.Math.Round(System.Math.Abs(val) * Sign, 10));

            xDeg = Fix(x);
            dx = (x - xDeg) * 60;
            dx = System.Math.Round(dx, 8);
            xMin = Fix(dx);
            xSec = (dx - xMin) * 60;
            xSec = System.Math.Round(xSec, 6);

            return xDeg + "°" + xMin + "'" + xSec + "''";
        }
        private static int Fix(double x)
        {
            return (int)(System.Math.Sign(x) * System.Math.Floor(System.Math.Abs(x)));
        }
    }
}
