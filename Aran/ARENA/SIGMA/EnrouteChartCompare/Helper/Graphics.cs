using System.Collections.Generic;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace EnrouteChartCompare.Helper
{
    public class Graphics
    {
        public Graphics(IActiveView activeView)
        {
            _index = 0;
            _drawedElements = new Dictionary<int, IElement>();
            _activeView = activeView;
        }

        #region EsriDrawing

        public void DrawDisplayLine(IPoint fromPoint, IPoint toPoint, bool isNoColor)
        {
            ILineSymbol pLineSimbol = new SimpleLineSymbol();
            ((ISymbol) pLineSimbol).ROP2 = esriRasterOpCode.esriROPXOrPen;

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

            IPolyline polyLine = new Polyline() as IPolyline;
            polyLine.FromPoint = fromPoint;
            polyLine.ToPoint = toPoint;
            //polyLine.ToPoint = toPoint;


            IScreenDisplay screenDisplay = _activeView.ScreenDisplay;
            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;

            screenDisplay.StartDrawing(screenDisplay.hDC,
                (System.Int16) esriScreenCache.esriNoScreenCache);
            screenDisplay.SetSymbol(pLineSimbol as ISymbol);
            screenDisplay.DrawPolyline(polyLine);
            screenDisplay.FinishDrawing();
            screenDisplay.Invalidate(null, false, (System.Int16) esriScreenCache.esriNoScreenCache);
        }

        public int DrawEsriPolygon(IPolygon poly, ISymbol symbol)
        {
            IPolygon esriPolygon = poly;
            IElement pElementofPoly = null;
            IFillShapeElement pFillShpElement = (IFillShapeElement) new PolygonElement();
            pElementofPoly = (IElement) pFillShpElement;

            pElementofPoly.Geometry = (IGeometry) esriPolygon;

            pFillShpElement.Symbol = (IFillSymbol) symbol;

            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
            pGraphics.AddElement(pElementofPoly, 0);
            _activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pElementofPoly);
        }

        public int DrawEsriPoint(IPoint ptPrj, ISymbol symbol)
        {
            var pMarkerShpElement = new MarkerElement() as IMarkerElement;


            IPoint pt = ptPrj;
            var pointElement = pMarkerShpElement as IElement;

            pointElement.Geometry = pt;

            pMarkerShpElement.Symbol = symbol as IMarkerSymbol;

            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
            pGraphics.AddElement(pointElement, 0);
            _activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pointElement);

        }


        public int DrawEsriPoint(IPoint ptPrj, int size, int color)
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

            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
            pGraphics.AddElement(pointElement, 0);
            _activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pointElement);

        }

        public int DrawEsriDefaultMultiPolygon(IPolygon multiPolygon)
        {
            IPolygon esriPolygon = multiPolygon;

            IRgbColor pRGB = null;
            IElement pElementofPoly = null;

            pRGB = new RgbColor {RGB = Aran.PANDA.Common.ARANFunctions.RGB(128, 128, 128)};
            ISimpleFillSymbol pFillSym = new SimpleFillSymbol();
            IFillShapeElement pFillShpElement = ((IFillShapeElement) (new PolygonElement()));

            pElementofPoly = ((IElement) (pFillShpElement));
            pElementofPoly.Geometry = ((IGeometry) (esriPolygon));


            pFillSym.Color = pRGB;
            pFillSym.Style = esriSimpleFillStyle.esriSFSForwardDiagonal;
            //  (pFillSym as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen; // esriSFSNull 'esriSFSDiagonalCross

            ILineSymbol pLineSimbol = new SimpleLineSymbol();
            //(pLineSimbol as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen;

            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = Aran.PANDA.Common.ARANFunctions.RGB(256, 0, 0);

            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = 4;
            pFillSym.Outline = pLineSimbol;

            pFillShpElement.Symbol = pFillSym;

            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
            pGraphics.AddElement(pElementofPoly, 0);
            _activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            return GetHandle(pElementofPoly);
        }

        public int DrawSector(IPolygon multiPolygon)
        {
            IPolygon esriPolygon = multiPolygon;

            IRgbColor pRGB = null;
            IElement pElementofPoly = null;

            pRGB = new RgbColor {RGB = Aran.PANDA.Common.ARANFunctions.RGB(128, 128, 128)};
            ISimpleFillSymbol pFillSym = new SimpleFillSymbol();
            IFillShapeElement pFillShpElement = ((IFillShapeElement) (new PolygonElement()));

            pElementofPoly = ((IElement) (pFillShpElement));
            pElementofPoly.Geometry = ((IGeometry) (esriPolygon));


            pFillSym.Color = pRGB;
            pFillSym.Style = esriSimpleFillStyle.esriSFSHollow;
            (pFillSym as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen; // esriSFSNull 'esriSFSDiagonalCross

            ILineSymbol pLineSimbol = new SimpleLineSymbol();
            (pLineSimbol as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen;

            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = Aran.PANDA.Common.ARANFunctions.RGB(128, 128, 128);

            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = 2;
            pFillSym.Outline = pLineSimbol;

            pFillShpElement.Symbol = pFillSym;

            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
            pGraphics.AddElement(pElementofPoly, 0);
            _activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

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
                    IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
                    pGraphics.DeleteElement(_drawedElements[handle]);
                    _activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                }
                catch
                {
                }

                _drawedElements.Remove(handle);
            }
        }

        public int DrawMultiLineString(IPolyline esriPolyline, ISymbol symbol)
        {
            ILineElement pLineElement = ((ILineElement) (new LineElement()));
            IElement pElementOfpLine = ((IElement) (pLineElement));
            IGeometry pGeometry = ((IGeometry) (esriPolyline));

            pElementOfpLine.Geometry = pGeometry;

            pLineElement.Symbol = symbol as ISimpleLineSymbol;

            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
            pGraphics.AddElement(pElementOfpLine, 0);
            _activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            return GetHandle(pElementOfpLine);
        }

        public int DrawMultiLineString(IPolyline esriPolyline, int color, esriSimpleLineStyle style)
        {
            ILineElement pLineElement = ((ILineElement) (new LineElement()));
            IElement pElementOfpLine = ((IElement) (pLineElement));
            IGeometry pGeometry = ((IGeometry) (esriPolyline));

            pElementOfpLine.Geometry = pGeometry;

            IRgbColor pRGB = new RgbColor();
            pRGB.RGB = color;
            ISimpleLineSymbol pLineSym = new SimpleLineSymbol();
            pLineSym.Color = pRGB;
            pLineSym.Style = style;
            pLineSym.Width = 3;

            pLineElement.Symbol = pLineSym as ISimpleLineSymbol;

            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
            pGraphics.AddElement(pElementOfpLine, 0);
            _activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            return GetHandle(pElementOfpLine);
        }

        public int DrawPointWithText(IPoint esriPoint, string text, int size, int color)
        {
            ITextElement pTextElement = ((ITextElement) (new TextElement()));
            IElement pElementOfText = ((IElement) (pTextElement));

            ITextSymbol pTextSymbol = new TextSymbol();
            pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
            pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;

            pTextElement.Text = text;
            pTextElement.ScaleText = false;
            pTextElement.Symbol = pTextSymbol;

            pElementOfText.Geometry = esriPoint;

            IMarkerElement pMarkerShpElement = ((IMarkerElement) (new MarkerElement()));

            IElement pElementofpPoint = ((IElement) (pMarkerShpElement));
            pElementofpPoint.Geometry = esriPoint;

            IRgbColor pRGB = new RgbColor();
            pRGB.RGB = color;

            ISimpleMarkerSymbol pMarkerSym = new SimpleMarkerSymbol();
            pMarkerSym.Color = pRGB;
            pMarkerSym.Size = size;
            pMarkerSym.Style = esriSimpleMarkerStyle.esriSMSCircle;

            pMarkerShpElement.Symbol = pMarkerSym;

            IGroupElement pGroupElement = ((IGroupElement) (new GroupElement()));
            pGroupElement.AddElement(pElementofpPoint);
            pElementofpPoint.Locked = true;
            pGroupElement.AddElement(pTextElement as IElement);

            IElement pCommonElement = ((IElement) (pGroupElement));

            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
            pGraphics.AddElement(pCommonElement, 0);
            _activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pCommonElement);
        }

        public int DrawText(IPoint esriPoint, string text, int size, int color)
        {
            ITextElement pTextElement = ((ITextElement) (new TextElement()));
            IElement pElementOfText = ((IElement) (pTextElement));

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

            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
            pGraphics.AddElement(pElementOfText, 0);
            _activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pElementOfText);
        }

        #endregion

        private int _index;
        private readonly Dictionary<int, IElement> _drawedElements;
        private readonly IActiveView _activeView;
    }
}