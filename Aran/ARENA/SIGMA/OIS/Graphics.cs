using System;
using System.Collections.Generic;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace OIS
{
    public class Graphics
    {
        private ISymbol symbol;

		public Graphics ( )
		{
			_index = 0;
			_drawedElements = new Dictionary<int, IElement> ( );
		}

		public Graphics(IActiveView activeView) : this()
        {
            _activeView = activeView;
        }

        #region EsriDrawing

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
            pLineSimbol.Width = 5;

            IPolyline polyLine = new ESRI.ArcGIS.Geometry.Polyline() as IPolyline;
            polyLine.FromPoint = fromPoint;
            polyLine.ToPoint = toPoint;
            //polyLine.ToPoint = toPoint;


            IScreenDisplay screenDisplay =_activeView.ScreenDisplay;
            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;

            screenDisplay.StartDrawing(screenDisplay.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache);
            screenDisplay.SetSymbol(pLineSimbol as ISymbol);
            screenDisplay.DrawPolyline(polyLine);
            screenDisplay.FinishDrawing();
            screenDisplay.Invalidate(null, false, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache);
        }


		public int DrawPolyline ( IPolyline esriPolyline, int color, esriSimpleLineStyle style )
		{
			ILineElement pLineElement = ( ( ESRI.ArcGIS.Carto.ILineElement ) ( new LineElement ( ) ) );
			IElement pElementOfpLine = ( ( ESRI.ArcGIS.Carto.IElement ) ( pLineElement ) );
			IGeometry pGeometry = ( ( ESRI.ArcGIS.Geometry.IGeometry ) ( esriPolyline ) );

			pElementOfpLine.Geometry = pGeometry;

			IRgbColor pRGB = new RgbColor ( );
			pRGB.RGB = color;
			ISimpleLineSymbol pLineSym = new SimpleLineSymbol ( );
			pLineSym.Color = pRGB;
			pLineSym.Style = style;
			pLineSym.Width = 3;

			pLineElement.Symbol = pLineSym as ISimpleLineSymbol;

			IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
			pGraphics.AddElement ( pElementOfpLine, 0 );
			_activeView.PartialRefresh ( esriViewDrawPhase.esriViewGraphics, null, null );

			return GetHandle ( pElementOfpLine );
		}

		public int DrawEsriPolygon(IPolygon poly, ESRI.ArcGIS.Display.ISymbol symbol)
        {
            IPolygon esriPolygon = poly;
            IElement pElementofPoly = null;
            IFillShapeElement pFillShpElement = (ESRI.ArcGIS.Carto.IFillShapeElement)new PolygonElement();
            pElementofPoly = (IElement)pFillShpElement;

            pElementofPoly.Geometry = (ESRI.ArcGIS.Geometry.IGeometry)esriPolygon;

            pFillShpElement.Symbol = (ESRI.ArcGIS.Display.IFillSymbol)symbol;

            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
            pGraphics.AddElement(pElementofPoly, 0);
            _activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pElementofPoly);
        }

        public int DrawEsriPoint(ESRI.ArcGIS.Geometry.IPoint ptPrj, ESRI.ArcGIS.Display.ISymbol symbol)
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

            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
            pGraphics.AddElement(pointElement, 0);
            _activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pointElement);

        }

        public int DrawEsriDefaultMultiPolygon(IPolygon multiPolygon, bool isVisible)
        {
            IPolygon esriPolygon = multiPolygon;

            IRgbColor pRGB = null;
            IElement pElementofPoly = null;

            pRGB = new RgbColor();
			if (isVisible)
				pRGB.RGB = ARANFunctions.RGB (255, 0, 0);
			else
				pRGB.RGB = ARANFunctions.RGB (0, 255, 0);
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
			if(isVisible)
				lineRgb.RGB = ARANFunctions.RGB(255, 0, 0);
			else
				lineRgb.RGB = ARANFunctions.RGB (0, 255, 0);

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

            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
            pGraphics.AddElement(pElementOfpLine, 0);
            _activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

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
            pLineSym.Width = 3;

            pLineElement.Symbol = pLineSym as ISimpleLineSymbol;

            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
            pGraphics.AddElement(pElementOfpLine, 0);
            _activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

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

            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
            pGraphics.AddElement(pCommonElement, 0);
            _activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
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

            IGraphicsContainer pGraphics = _activeView.GraphicsContainer;
            pGraphics.AddElement(pElementOfText, 0);
            _activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pElementOfText);
        }
        #endregion

		public void Clear()
		{
			var keys = new List<int> (_drawedElements.Keys);
			foreach (var key in keys)
			{
				SafeDeleteGraphic (key);
			}
			_activeView.Refresh ( );
		}

        private int _index;
        private Dictionary<int, IElement> _drawedElements;
        private IActiveView _activeView;
    }
}
