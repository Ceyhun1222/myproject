using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace SigmaChart
{
    public class Graphics
    {
        private ISymbol symbol;
        public Graphics()
        {
            _index = 0;
            _drawedElements = new Dictionary<int, IElement>();
        }

        #region EsriDrawing
        public void DrawMultiPolygon(IPolygon poly, bool isNullColor)
        {
            IPolygon esriPolygon = poly;//; Aran.Converters.ConvertToEsriGeom.FromMultiPolygon(multiPolygon);
            IRgbColor pRGB = null;

            pRGB = new RgbColor();
            int color = 255;
            if (isNullColor)
                pRGB.NullColor = true;
            else
            {
                pRGB.Red = color;
                pRGB.Blue = color;
                pRGB.Green = color;
            }
            ISimpleFillSymbol pFillSym = new SimpleFillSymbol();

            pFillSym.Color = pRGB;
            pFillSym.Style = esriSimpleFillStyle.esriSFSNull; // esriSFSNull 'esriSFSDiagonalCross

            ILineSymbol pLineSimbol = new SimpleLineSymbol();
            (pLineSimbol as ISymbol).ROP2 = esriRasterOpCode.esriROPNot;

            color = 255;
            IRgbColor lineRgb = new RgbColor();
            lineRgb.Red = color;
            lineRgb.Green = color;
            lineRgb.Blue = color;

            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = 1;
            pFillSym.Outline = pLineSimbol;
            ISymbol symbol = pFillSym as ISymbol;
            symbol.ROP2 = esriRasterOpCode.esriROPNot;

            //ESRI.ArcGIS.Display.IRubberBand rubberBand = new ESRI.ArcGIS.Display.RubberPolygon();


            IScreenDisplay screenDisplay = GlobalParams.ActiveView.ScreenDisplay;
            IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
            //IGeometry geom = esriPolygon;

            //if (!rubberBand.TrackExisting(screenDisplay, symbol, esriPolygon))
            //    geom = rubberBand.TrackNew(screenDisplay, symbol);

            GlobalParams.ActiveView.ScreenDisplay.StartDrawing(screenDisplay.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache);
            GlobalParams.ActiveView.ScreenDisplay.SetSymbol(symbol);
            GlobalParams.ActiveView.ScreenDisplay.DrawPolygon(esriPolygon as IPolygon);
            GlobalParams.ActiveView.ScreenDisplay.FinishDrawing();
        }

        public void DrawDisplayLine(IPoint fromPoint, IPoint toPoint, bool isNoColor)
        {
            ILineSymbol pLineSimbol = new SimpleLineSymbol();
            (pLineSimbol as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen;

            int color = 124;

            IRgbColor lineRgb = new RgbColor();

            if (isNoColor)
                lineRgb.NullColor = true;
            else
            {
                lineRgb.Red = color;
                lineRgb.Green = color;
                lineRgb.Blue = color;
            }
            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = 1;

            IPolyline polyLine = new ESRI.ArcGIS.Geometry.Polyline() as IPolyline;
            polyLine.FromPoint = fromPoint;
            polyLine.ToPoint = toPoint;
            //polyLine.ToPoint = toPoint;


            IScreenDisplay screenDisplay = GlobalParams.ActiveView.ScreenDisplay;
            IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;

            screenDisplay.StartDrawing(screenDisplay.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache);
            screenDisplay.SetSymbol(pLineSimbol as ISymbol);
            screenDisplay.DrawPolyline(polyLine);
            screenDisplay.FinishDrawing();
            screenDisplay.Invalidate(null, false, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache);
        }

        public int DrawEsriPolygon(IPolygon poly, ESRI.ArcGIS.Display.ISymbol symbol)
        {
            IPolygon esriPolygon = poly;
            IElement pElementofPoly = null;
            IFillShapeElement pFillShpElement = (ESRI.ArcGIS.Carto.IFillShapeElement)new PolygonElement();
            pElementofPoly = (IElement)pFillShpElement;

            pElementofPoly.Geometry = (ESRI.ArcGIS.Geometry.IGeometry)esriPolygon;

            pFillShpElement.Symbol = (ESRI.ArcGIS.Display.IFillSymbol)symbol;

            IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pElementofPoly, 0);
            GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pElementofPoly);
        }

        public int DrawEsriPoint(ESRI.ArcGIS.Geometry.IPoint ptPrj, ESRI.ArcGIS.Display.ISymbol symbol)
        {
            var pMarkerShpElement = new MarkerElement() as IMarkerElement;


            IPoint pt = ptPrj;
            var pointElement = pMarkerShpElement as IElement;

            pointElement.Geometry = pt;

            pMarkerShpElement.Symbol = symbol as IMarkerSymbol;

            IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pointElement, 0);
            GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pointElement);

        }

        public int DrawEsriPoint(ESRI.ArcGIS.Geometry.IPoint ptPrj)
        {
            var pMarkerShpElement = new MarkerElement() as IMarkerElement;

            ISimpleMarkerSymbol ptSymbol = new SimpleMarkerSymbol();
            ISimpleMarkerSymbol simpleMarkerSymbol = ptSymbol as ISimpleMarkerSymbol;
            simpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            simpleMarkerSymbol.Size = 14;

            var m_hookHelper = GlobalParams.HookHelper;
            var docPageLayout = m_hookHelper.PageLayout;
            var docActiveView = docPageLayout as IActiveView;
            var GContainer = docPageLayout as IGraphicsContainer;

            IPoint pt = ptPrj;
            var pointElement = pMarkerShpElement as IElement;

            pointElement.Geometry = pt;

            pMarkerShpElement.Symbol = ptSymbol as IMarkerSymbol;

            GContainer.AddElement(pointElement, 0);
            docActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pointElement);

        }

        public int DrawEsriPoint(ESRI.ArcGIS.Geometry.IPoint ptPrj, int size, int color)
        {
            var pMarkerShpElement = new MarkerElement() as IMarkerElement;

            ISimpleMarkerSymbol ptSymbol = new SimpleMarkerSymbol();
            ISimpleMarkerSymbol simpleMarkerSymbol = ptSymbol as ISimpleMarkerSymbol;
            simpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            simpleMarkerSymbol.Size = size;

            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = color;
            simpleMarkerSymbol.Color = lineRgb;

            IPoint pt = ptPrj;
            var pointElement = pMarkerShpElement as IElement;

            pointElement.Geometry = pt;

            pMarkerShpElement.Symbol = ptSymbol as IMarkerSymbol;

            IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pointElement, 0);
            GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pointElement);

        }

        public int DrawEsriDefaultMultiPolygon(IPolygon multiPolygon)
        {
            IPolygon esriPolygon = multiPolygon;

            IRgbColor pRGB = null;
            IElement pElementofPoly = null;

            pRGB = new RgbColor();
            pRGB.RGB = Aran.PANDA.Common.ARANFunctions.RGB(128, 128, 128);
            ISimpleFillSymbol pFillSym = new SimpleFillSymbol();
            IFillShapeElement pFillShpElement = ((ESRI.ArcGIS.Carto.IFillShapeElement)(new PolygonElement()));

            pElementofPoly = ((ESRI.ArcGIS.Carto.IElement)(pFillShpElement));
            pElementofPoly.Geometry = ((ESRI.ArcGIS.Geometry.IGeometry)(esriPolygon));


            pFillSym.Color = pRGB;
            pFillSym.Style = esriSimpleFillStyle.esriSFSForwardDiagonal;
            //  (pFillSym as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen; // esriSFSNull 'esriSFSDiagonalCross

            ILineSymbol pLineSimbol = new SimpleLineSymbol();
            //(pLineSimbol as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen;

            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = Aran.PANDA.Common.ARANFunctions.RGB(128, 128, 128); ;

            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = 2;
            pFillSym.Outline = pLineSimbol;

            pFillShpElement.Symbol = pFillSym;

            IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pElementofPoly, 0);
            GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            return GetHandle(pElementofPoly);
        }

        public int DrawSector(IPolygon multiPolygon)
        {
            IPolygon esriPolygon = multiPolygon;

            IRgbColor pRGB = null;
            IElement pElementofPoly = null;

            pRGB = new RgbColor();
            pRGB.RGB = Aran.PANDA.Common.ARANFunctions.RGB(128, 128, 128);
            ISimpleFillSymbol pFillSym = new SimpleFillSymbol();
            IFillShapeElement pFillShpElement = ((ESRI.ArcGIS.Carto.IFillShapeElement)(new PolygonElement()));

            pElementofPoly = ((ESRI.ArcGIS.Carto.IElement)(pFillShpElement));
            pElementofPoly.Geometry = ((ESRI.ArcGIS.Geometry.IGeometry)(esriPolygon));


            pFillSym.Color = pRGB;
            pFillSym.Style = esriSimpleFillStyle.esriSFSHollow;
            (pFillSym as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen; // esriSFSNull 'esriSFSDiagonalCross

            ILineSymbol pLineSimbol = new SimpleLineSymbol();
            (pLineSimbol as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen;

            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = Aran.PANDA.Common.ARANFunctions.RGB(128, 128, 128); ;

            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = 2;
            pFillSym.Outline = pLineSimbol;

            pFillShpElement.Symbol = pFillSym;

            IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pElementofPoly, 0);
            GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            return GetHandle(pElementofPoly);
        }

        private int GetHandle(IElement esriElement)
        {
            _index++;
            _drawedElements.Add(_index, esriElement);
            return _index;
        }

        public void SafeDeleteGraphic(int handle)
        {
            if (_drawedElements.ContainsKey(handle))
            {
                try
                {
                    IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
                    pGraphics.DeleteElement(_drawedElements[handle]);
                    GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                }
                catch { }
                _drawedElements.Remove(handle);
            }
        }

        public int DrawMultiLineString(IPolyline esriPolyline, ISymbol symbol)
        {
            ILineElement pLineElement = ((ESRI.ArcGIS.Carto.ILineElement)(new LineElement()));
            IElement pElementOfpLine = ((ESRI.ArcGIS.Carto.IElement)(pLineElement));
            IGeometry pGeometry = ((ESRI.ArcGIS.Geometry.IGeometry)(esriPolyline));

            pElementOfpLine.Geometry = pGeometry;

            pLineElement.Symbol = symbol as ISimpleLineSymbol;

            IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pElementOfpLine, 0);
            GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            return GetHandle(pElementOfpLine);
        }

        public int DrawMultiLineString(IPolyline esriPolyline, int color, esriSimpleLineStyle style)
        {
            ILineElement pLineElement = ((ESRI.ArcGIS.Carto.ILineElement)(new LineElement()));
            IElement pElementOfpLine = ((ESRI.ArcGIS.Carto.IElement)(pLineElement));
            IGeometry pGeometry = ((ESRI.ArcGIS.Geometry.IGeometry)(esriPolyline));

            pElementOfpLine.Geometry = pGeometry;

            IRgbColor pRGB = new RgbColor();
            pRGB.RGB = color;
            ISimpleLineSymbol pLineSym = new SimpleLineSymbol();
            pLineSym.Color = pRGB;
            pLineSym.Style = style;
            pLineSym.Width = 2;

            pLineElement.Symbol = pLineSym as ISimpleLineSymbol;

            IGraphicsContainer pGraphics = GlobalParams.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pElementOfpLine, 0);
            GlobalParams.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            return GetHandle(pElementOfpLine);
        }

        public int DrawPointWithText(IPoint esriPoint, string text, int size, int color)
        {
            ITextElement pTextElement = ((ESRI.ArcGIS.Carto.ITextElement)(new TextElement()));
            IElement pElementOfText = ((ESRI.ArcGIS.Carto.IElement)(pTextElement));

            ITextSymbol pTextSymbol = new TextSymbol();
            pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
            pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;

            pTextElement.Text = text;
            pTextElement.ScaleText = false;
            pTextElement.Symbol = pTextSymbol;

            pElementOfText.Geometry = esriPoint;

            IMarkerElement pMarkerShpElement = ((ESRI.ArcGIS.Carto.IMarkerElement)(new MarkerElement()));

            IElement pElementofpPoint = ((ESRI.ArcGIS.Carto.IElement)(pMarkerShpElement));
            pElementofpPoint.Geometry = esriPoint;

            IRgbColor pRGB = new RgbColor();
            pRGB.RGB = color;

            ISimpleMarkerSymbol pMarkerSym = new SimpleMarkerSymbol();
            pMarkerSym.Color = pRGB;
            pMarkerSym.Size = size;
            pMarkerSym.Style = esriSimpleMarkerStyle.esriSMSCircle;

            pMarkerShpElement.Symbol = pMarkerSym;

            IGroupElement pGroupElement = ((ESRI.ArcGIS.Carto.IGroupElement)(new GroupElement()));
            pGroupElement.AddElement(pElementofpPoint);
            pElementofpPoint.Locked = true;
            pGroupElement.AddElement(pTextElement as IElement);

            IElement pCommonElement = ((ESRI.ArcGIS.Carto.IElement)(pGroupElement));

            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pCommonElement, 0);
            GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pCommonElement);
        }

        public int DrawText(IPoint esriPoint, string text, int size, int color)
        {
            ITextElement pTextElement = ((ESRI.ArcGIS.Carto.ITextElement)(new TextElement()));
            IElement pElementOfText = ((ESRI.ArcGIS.Carto.IElement)(pTextElement));

            IRgbColor pRGB = new RgbColor();
            pRGB.RGB = color;

            ITextSymbol pTextSymbol = new TextSymbol();
            pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
            pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
            pTextSymbol.Color = pRGB;
            pTextSymbol.Size = size;
            pTextElement.Text = text;
            pTextElement.ScaleText = false;
            pTextElement.Symbol = pTextSymbol;

            pElementOfText.Geometry = esriPoint;

            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pElementOfText, 0);
            GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pElementOfText);
        }
        #endregion

        private int _index;
        private Dictionary<int, IElement> _drawedElements;
    }
}
