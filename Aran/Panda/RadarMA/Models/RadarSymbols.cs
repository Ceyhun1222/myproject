using Aran.PANDA.Common;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;

namespace Aran.Panda.RadarMA.Models
{
    public class RadarSymbols
    {
        private static RadarSymbols _instance;
        private RadarSymbols()
        {
            
        }

        ~RadarSymbols()
        {
            _instance = null;
        }

        public static RadarSymbols Instance()
        {
            if (_instance == null)
            {
                _instance=new RadarSymbols();
                InitializeSymbols(_instance);

            }

            return _instance;
        }

        private static void InitializeSymbols(RadarSymbols symbols)
        {
            symbols.VectoringAreaSymbol = CreateSymbol(0, 0, 2);


            symbols.CircleSymbol = CreateSymbol(0, Aran.PANDA.Common.ARANFunctions.RGB(255, 0, 0), 2,
                esriSimpleFillStyle.esriSFSNull);

            symbols.DrawingSymbol = CreateSymbol(Aran.PANDA.Common.ARANFunctions.RGB(128, 128, 0), 0, 1,
                esriSimpleFillStyle.esriSFSBackwardDiagonal);

            var blue = ARANFunctions.RGB(blue: 188);
            symbols.SectorProcessingSectorSybmol = CreateSymbol(0,blue,2,esriSimpleFillStyle.esriSFSCross);

            symbols.SectorSymbol = CreateSymbol(Aran.PANDA.Common.ARANFunctions.RGB(128, 128, 34), 0, 2,
                esriSimpleFillStyle.esriSFSCross);

            symbols.RadarAreaSymbol = CreateSymbol(ARANFunctions.RGB(100, 100, 100), 3);


            ILineSymbol pLineSimbol = new SimpleLineSymbol();
            (pLineSimbol as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen;

            int color = 124;

            (pLineSimbol as ISimpleLineSymbol).Style = esriSimpleLineStyle.esriSLSDashDot;
            IRgbColor lineRgb = new RgbColor();


            lineRgb.Red = color;
            lineRgb.Green = color;
            lineRgb.Blue = color;

            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = 1;

            symbols.CircleDashSymbol = pLineSimbol as ISymbol;
        }

        private static ISymbol CreateSymbol(int color, int lineColor,int width=2, esriSimpleFillStyle style= esriSimpleFillStyle.esriSFSNull)
        {
            IRgbColor pRGB = null;

            pRGB = new RgbColor();
            pRGB.RGB = color;
            ISimpleFillSymbol pFillSym = new SimpleFillSymbol();
            IFillShapeElement pFillShpElement = ((ESRI.ArcGIS.Carto.IFillShapeElement)(new PolygonElement()));


            pFillSym.Color = pRGB;
            pFillSym.Style = style;

            ILineSymbol pLineSimbol = new SimpleLineSymbol();

            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = lineColor;

            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = width;
            pFillSym.Outline = pLineSimbol;

            return pFillSym as ISymbol;
        }

        public ISymbol VectoringAreaSymbol { get; set; }
        public ISymbol SectorProcessingSectorSybmol { get; set; }
        public ISymbol DrawingSymbol { get; set; }
        public ISymbol CircleDashSymbol { get; set; }
        public ISymbol CircleSymbol { get; set; }
        public ISymbol SectorSymbol { get; set; }
        public ISymbol RadarAreaSymbol { get; set; }
    }
}