using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace ChartTypeA
{
    public class Graphics
    {
        public Graphics()
        {
            _index = 0;
            _drawedElements = new Dictionary<int, IElement>();
            
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
                    IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
                    pGraphics.DeleteElement(_drawedElements[handle]);
                    GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                }
                catch { }
                _drawedElements.Remove(handle);
            }
        }


        #region EsriDrawing
        public void DrawMultiPolygon(IPolygon poly,bool isNullColor)
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
            IRgbColor lineRgb =  new RgbColor();
            lineRgb.Red = color;
            lineRgb.Green = color;
            lineRgb.Blue = color;

            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = 1;
            pFillSym.Outline = pLineSimbol;
            ISymbol symbol = pFillSym as ISymbol;
            symbol.ROP2 = esriRasterOpCode.esriROPNot;

            //ESRI.ArcGIS.Display.IRubberBand rubberBand = new ESRI.ArcGIS.Display.RubberPolygon();
            

            IScreenDisplay screenDisplay = GlobalParams.HookHelper.ActiveView.ScreenDisplay;
            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            //IGeometry geom = esriPolygon;
            
            //if (!rubberBand.TrackExisting(screenDisplay, symbol, esriPolygon))
            //    geom = rubberBand.TrackNew(screenDisplay, symbol);
            
            GlobalParams.HookHelper.ActiveView.ScreenDisplay.StartDrawing(screenDisplay.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache);
            GlobalParams.HookHelper.ActiveView.ScreenDisplay.SetSymbol(symbol);
            GlobalParams.HookHelper.ActiveView.ScreenDisplay.DrawPolygon(esriPolygon as IPolygon);
            GlobalParams.HookHelper.ActiveView.ScreenDisplay.FinishDrawing();
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


			IScreenDisplay screenDisplay = GlobalParams.HookHelper.ActiveView.ScreenDisplay;
			IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer; 

            screenDisplay.StartDrawing(screenDisplay.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache);
            screenDisplay.SetSymbol(pLineSimbol as ISymbol);
            screenDisplay.DrawPolyline(polyLine);
            screenDisplay.FinishDrawing();
            screenDisplay.Invalidate(null, false, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache);
        }

		public IElement GetPolylineElement ( List<IPoint> points, int color, esriSimpleLineStyle style = esriSimpleLineStyle.esriSLSSolid)
		{
			ILineSymbol pLineSymbol = new SimpleLineSymbol ( );
			( pLineSymbol as ISimpleLineSymbol ).Style = style;
			IRgbColor lineRgb = new RgbColor ( );
			lineRgb.RGB = color;

			pLineSymbol.Color = lineRgb;
			pLineSymbol.Width = 1;			

			IPointCollection polyLine = new ESRI.ArcGIS.Geometry.Polyline ( );
			foreach ( var pnt in points )
			{
				polyLine.AddPoint ( pnt );	
			}

			IElement element = new LineElement ( );
			element.Geometry = (IPolyline) polyLine;
			ILineElement lineElement = element as ILineElement;
			lineElement.Symbol = pLineSymbol;

			return element;
		}

        public IElement GetObstaclePolylineElement(List<IPoint> points, int color, esriSimpleLineStyle style = esriSimpleLineStyle.esriSLSSolid)
        {
            ILineSymbol pLineSymbol = new SimpleLineSymbol();
            (pLineSymbol as ISimpleLineSymbol).Style = style;
            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = color;

            pLineSymbol.Color = lineRgb;
            pLineSymbol.Width = 2;

            IPointCollection polyLine = new ESRI.ArcGIS.Geometry.Polyline();
            foreach (var pnt in points)
            {
                polyLine.AddPoint(pnt);
            }

            IElement element = new LineElement();
            element.Geometry = (IPolyline)polyLine;
            ILineElement lineElement = element as ILineElement;
            lineElement.Symbol = pLineSymbol;

            return element;
        }

        public int DrawEsriPolygon(IPolygon poly, ESRI.ArcGIS.Display.ISymbol symbol)
        {
            IPolygon esriPolygon = poly;
            IElement pElementofPoly = null;
            IFillShapeElement pFillShpElement = (ESRI.ArcGIS.Carto.IFillShapeElement)new PolygonElement();
            pElementofPoly = (IElement)pFillShpElement;

            pElementofPoly.Geometry = (ESRI.ArcGIS.Geometry.IGeometry)esriPolygon;

            pFillShpElement.Symbol = (ESRI.ArcGIS.Display.IFillSymbol)symbol;

            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pElementofPoly, 0);
            GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pElementofPoly);
        }


        public int DrawEsriPoint(ESRI.ArcGIS.Geometry.IPoint ptPrj, ESRI.ArcGIS.Display.ISymbol symbol)
        {
            var pMarkerShpElement = new MarkerElement() as IMarkerElement;


            IPoint pt = ptPrj;
            var pointElement = pMarkerShpElement as IElement;

            pointElement.Geometry = pt;

            pMarkerShpElement.Symbol = symbol as IMarkerSymbol;

            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pointElement, 0);
            GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pointElement);
        
        }

        public int DrawEsriPoint(ESRI.ArcGIS.Geometry.IPoint ptPrj)
        {
            var pMarkerShpElement = new MarkerElement() as IMarkerElement;

            ISimpleMarkerSymbol ptSymbol = new SimpleMarkerSymbol();
            ISimpleMarkerSymbol simpleMarkerSymbol = ptSymbol as ISimpleMarkerSymbol;
            simpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            simpleMarkerSymbol.Size = 8;

            IPoint pt = ptPrj;
            var pointElement = pMarkerShpElement as IElement;

            pointElement.Geometry = pt;

            pMarkerShpElement.Symbol = ptSymbol as IMarkerSymbol;

            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pointElement, 0);
            GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
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
            pFillSym.Style = esriSimpleFillStyle.esriSFSCross;

            ILineSymbol pLineSimbol = new SimpleLineSymbol();

            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = Aran.PANDA.Common.ARANFunctions.RGB(253, 0, 0); ;

            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = 3;
            pFillSym.Outline = pLineSimbol;

            pFillShpElement.Symbol = pFillSym;

            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pElementofPoly, 0);
            GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            return GetHandle(pElementofPoly);
        }

        public IElement DrawPointWithText(IPoint point, string text)
        {
            IPoint esriPoint = point;
            ITextElement pTextElement = ((ESRI.ArcGIS.Carto.ITextElement)(new TextElement()));
            IElement pElementOfText = ((ESRI.ArcGIS.Carto.IElement)(pTextElement));

            ITextSymbol pTextSymbol =new TextSymbolClass();
            pTextSymbol.Size = 6;

            pTextElement.Text = text;
            pTextElement.ScaleText = false;
            pTextElement.Symbol = pTextSymbol;

            pElementOfText.Geometry = esriPoint;

            ISimpleMarkerSymbol ptSymbol = new SimpleMarkerSymbol();
            ISimpleMarkerSymbol simpleMarkerSymbol = ptSymbol as ISimpleMarkerSymbol;
            simpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            simpleMarkerSymbol.Size = 8;


            IMarkerElement pMarkerShpElement = ((ESRI.ArcGIS.Carto.IMarkerElement)(new MarkerElement()));

            IElement pElementofpPoint = ((ESRI.ArcGIS.Carto.IElement)(pMarkerShpElement));
            pElementofpPoint.Geometry = esriPoint;

            pMarkerShpElement.Symbol = ptSymbol as IMarkerSymbol;

            IGroupElement pGroupElement = ((ESRI.ArcGIS.Carto.IGroupElement)(new GroupElement()));
         //   pGroupElement.AddElement(pElementofpPoint);
            pElementofpPoint.Locked = true;
            pGroupElement.AddElement(pTextElement as IElement);

            IElement pCommonElement = ((ESRI.ArcGIS.Carto.IElement)(pGroupElement));

            return pCommonElement;
        }

        #endregion

        private int _index;
        private Dictionary<int, IElement> _drawedElements;
    }
}
