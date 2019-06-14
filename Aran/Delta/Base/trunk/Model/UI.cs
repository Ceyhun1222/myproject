using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Converters;
using Aran.Geometries;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace Aran.Delta.Model
{
    public class Graphics
    {
        public Graphics()
        {
            _index = 0;
            _drawedElements = new Dictionary<int, IElement>();
            
        }
        public  int DrawPoint(Aran.Geometries.Point geo,ESRI.ArcGIS.Display.ISymbol symbol)
        {
            var pMarkerShpElement = new MarkerElement () as IMarkerElement;
            var ptPrj = GlobalParams.SpatialRefOperation.ToPrj(geo);

            IPoint pt = new ESRI.ArcGIS.Geometry.Point();
            pt.X = ptPrj.X;
            pt.Y = ptPrj.Y;
            var pointElement = pMarkerShpElement as IElement;
                
            pointElement.Geometry = pt;

            pMarkerShpElement.Symbol = symbol as IMarkerSymbol;

            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pointElement, 0);
            GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pointElement);
        }

        public int DrawPointPrj(Aran.Geometries.Point ptPrj, ESRI.ArcGIS.Display.ISymbol symbol)
        {
            var pMarkerShpElement = new MarkerElement() as IMarkerElement;
            
            IPoint pt = new ESRI.ArcGIS.Geometry.Point();
            pt.X = ptPrj.X;
            pt.Y = ptPrj.Y;
            var pointElement = pMarkerShpElement as IElement;

            pointElement.Geometry = pt;

            pMarkerShpElement.Symbol = symbol as IMarkerSymbol;

            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pointElement, 0);
            GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pointElement);
        }

        public int DrawPointPrj(Aran.Geometries.Point ptPrj)
        {
            IPoint pt = new ESRI.ArcGIS.Geometry.Point();
            pt.X = ptPrj.X;
            pt.Y = ptPrj.Y;

            var pMarkerShpElement = new MarkerElement() as IMarkerElement;

            ISimpleMarkerSymbol ptSymbol = new SimpleMarkerSymbol();
            ISimpleMarkerSymbol simpleMarkerSymbol = ptSymbol as ISimpleMarkerSymbol;
            simpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            simpleMarkerSymbol.Size = 8;

            var pointElement = pMarkerShpElement as IElement;

            pointElement.Geometry = pt;

            pMarkerShpElement.Symbol = ptSymbol as IMarkerSymbol;

            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pointElement, 0);
            GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pointElement);

        }
        
        public int DrawMultiPolygon(MultiPolygon multiPolygon,ESRI.ArcGIS.Display.ISymbol symbol)
        {
            IPolygon esriPolygon = Aran.Converters.ConvertToEsriGeom.FromMultiPolygon(multiPolygon);
            IElement pElementofPoly = null;
            IFillShapeElement pFillShpElement = (ESRI.ArcGIS.Carto.IFillShapeElement)new PolygonElement();
            pElementofPoly =(IElement) pFillShpElement;

            pElementofPoly.Geometry = (ESRI.ArcGIS.Geometry.IGeometry)esriPolygon;
            
            pFillShpElement.Symbol = (ESRI.ArcGIS.Display.IFillSymbol)symbol;

            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pElementofPoly, 0);
            GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pElementofPoly);
        }

        public int DrawMultiPolygon(MultiPolygon multiPolygon, Aran.AranEnvironment.Symbols.FillSymbol symbol, Boolean isVisible = true, Boolean isLocked = true)
        {
            IPolygon esriPolygon = Aran.Converters.ConvertToEsriGeom.FromMultiPolygon(multiPolygon);

            IRgbColor pRGB = null;
            IElement pElementofPoly = null;

            pRGB = new RgbColor();
            pRGB.RGB = (int)symbol.Color;
            ISimpleFillSymbol pFillSym = new SimpleFillSymbol();
            IFillShapeElement pFillShpElement = ((ESRI.ArcGIS.Carto.IFillShapeElement)(new PolygonElement()));

            pElementofPoly = ((ESRI.ArcGIS.Carto.IElement)(pFillShpElement));
            pElementofPoly.Geometry = ((ESRI.ArcGIS.Geometry.IGeometry)(esriPolygon));


            pFillSym.Color = pRGB;
            pFillSym.Style = ((esriSimpleFillStyle)(symbol.Style)); // esriSFSNull 'esriSFSDiagonalCross

            ILineSymbol pLineSimbol = new SimpleLineSymbol();

            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = (int)symbol.Outline.Color;

            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = symbol.Outline.Size;
            pFillSym.Outline = pLineSimbol;


            pFillShpElement.Symbol = pFillSym;

            if (isVisible)
            {
                IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
                pGraphics.AddElement(pElementofPoly, 0);
                GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            return GetHandle(pElementofPoly);
        }

        public int DrawDefaultMultiPolygon(MultiPolygon multiPolygon)
        {
            IPolygon esriPolygon = Aran.Converters.ConvertToEsriGeom.FromMultiPolygon(multiPolygon);

            IRgbColor pRGB = null;
            IElement pElementofPoly = null;

            pRGB = new RgbColor();
			//pRGB.RGB = Aran.PANDA.Common.ARANFunctions.RGB(128, 128, 128);
			ISimpleFillSymbol pFillSym = new SimpleFillSymbol();
            IFillShapeElement pFillShpElement = ((ESRI.ArcGIS.Carto.IFillShapeElement)(new PolygonElement()));

            pElementofPoly = ((ESRI.ArcGIS.Carto.IElement)(pFillShpElement));
            pElementofPoly.Geometry = ((ESRI.ArcGIS.Geometry.IGeometry)(esriPolygon));


            pFillSym.Color = pRGB;
            pFillSym.Style = esriSimpleFillStyle.esriSFSNull;
            //(pFillSym as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen; // esriSFSNull 'esriSFSDiagonalCross

            ILineSymbol pLineSimbol = new SimpleLineSymbol();
          //  (pLineSimbol as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen;

            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = Aran.PANDA.Common.ARANFunctions.RGB(255, 0, 0); ;

            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = 3;
            pFillSym.Outline = pLineSimbol;

            pFillShpElement.Symbol = pFillSym;

            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pElementofPoly, 0);
            GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            return GetHandle(pElementofPoly);
        }

        public int DrawDefaultMultiPolygon(MultiPolygon multiPolygon, int handle)
        {
            IPolygon esriPolygon = Aran.Converters.ConvertToEsriGeom.FromMultiPolygon(multiPolygon);

            IRgbColor pRGB = null;
            IElement pElementofPoly = null;

           
            ISimpleFillSymbol pFillSym = new SimpleFillSymbol();
            IFillShapeElement pFillShpElement = ((ESRI.ArcGIS.Carto.IFillShapeElement)(new PolygonElement()));

            pElementofPoly = ((ESRI.ArcGIS.Carto.IElement)(pFillShpElement));
            pElementofPoly.Geometry = ((ESRI.ArcGIS.Geometry.IGeometry)(esriPolygon));

            pFillSym.Style = esriSimpleFillStyle.esriSFSNull;
            (pFillSym as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen; // esriSFSNull 'esriSFSDiagonalCross

            ILineSymbol pLineSimbol = new SimpleLineSymbol();
          //  (pLineSimbol as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen;
            
            pFillShpElement.Symbol = pFillSym;
            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;

            if (_drawedElements.ContainsKey(handle))
            {
                try
                {
                    pGraphics.DeleteElement(_drawedElements[handle]);
                   // GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                }
                catch { }
                _drawedElements.Remove(handle);
            }

            pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pElementofPoly, 0);
            GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, esriPolygon, null);

            return GetHandle(pElementofPoly);
        
        }

        public int DrawMultiPolygon(MultiPolygon multiPolygon, int color, Aran.AranEnvironment.Symbols.eFillStyle style, Boolean isVisible = true, Boolean isLocked = true)
        {
            if (color < 0)
            {
                Random rnd = new Random();
                color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
            }

            Aran.AranEnvironment.Symbols.FillSymbol fillSymbol = new Aran.AranEnvironment.Symbols.FillSymbol();
            fillSymbol.Color = color;
            fillSymbol.Style = style;
            fillSymbol.Outline = new Aran.AranEnvironment.Symbols.LineSymbol();
            fillSymbol.Outline.Color = color;
            fillSymbol.Outline.Size = fillSymbol.Size;
            return DrawMultiPolygon(multiPolygon, fillSymbol, isVisible, isLocked);
        }


        public int DrawPointWithText(Aran.Geometries.Point point, string text)
        {
            IPoint esriPoint = Aran.Converters.ConvertToEsriGeom.FromPoint(point);
            ITextElement pTextElement = ((ESRI.ArcGIS.Carto.ITextElement)(new TextElement()));
            IElement pElementOfText = ((ESRI.ArcGIS.Carto.IElement)(pTextElement));

            ITextSymbol pTextSymbol = GlobalParams.Settings.SymbolModel.TextSymbol as ITextSymbol;

            pTextElement.Text = text;
            pTextElement.ScaleText = false;
            pTextElement.Symbol = pTextSymbol;

            pElementOfText.Geometry = esriPoint;

            IMarkerElement pMarkerShpElement = ((ESRI.ArcGIS.Carto.IMarkerElement)(new MarkerElement()));

            IElement pElementofpPoint = ((ESRI.ArcGIS.Carto.IElement)(pMarkerShpElement));
            pElementofpPoint.Geometry = esriPoint;

            pMarkerShpElement.Symbol = GlobalParams.Settings.SymbolModel.ResultPointSymbol as IMarkerSymbol;

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

        public int DrawMultiLineString(MultiLineString multiLineString, ESRI.ArcGIS.Display.ISymbol symbol)
        {
            var geoPrj = GlobalParams.SpatialRefOperation.ToPrj(multiLineString);
            IPolyline esriPolyline = ConvertToEsriGeom.FromMultiLineString(geoPrj);

            ILineElement pLineElement = ((ESRI.ArcGIS.Carto.ILineElement)(new LineElement()));
            IElement pElementOfpLine = ((ESRI.ArcGIS.Carto.IElement)(pLineElement));
            IGeometry pGeometry = ((ESRI.ArcGIS.Geometry.IGeometry)(esriPolyline));

            pElementOfpLine.Geometry = pGeometry;
            pLineElement.Symbol = symbol as ILineSymbol;

            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pElementOfpLine, 0);
            GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pElementOfpLine);
        }

        public int DrawMultiLineStringPrj(MultiLineString multiLineString, ESRI.ArcGIS.Display.ISymbol symbol)
        {
            var geoPrj = multiLineString;
            IPolyline esriPolyline = ConvertToEsriGeom.FromMultiLineString(geoPrj);

            ILineElement pLineElement = ((ESRI.ArcGIS.Carto.ILineElement)(new LineElement()));
            IElement pElementOfpLine = ((ESRI.ArcGIS.Carto.IElement)(pLineElement));
            IGeometry pGeometry = ((ESRI.ArcGIS.Geometry.IGeometry)(esriPolyline));

            pElementOfpLine.Geometry = pGeometry;
            pLineElement.Symbol = symbol as ILineSymbol;

            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pElementOfpLine, 0);
            GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pElementOfpLine);
        }

        public int DrawText(MultiLineString multiLineString, ESRI.ArcGIS.Display.ISymbol symbol,string text,double direction)
        {
            var geoPrj = GlobalParams.SpatialRefOperation.ToPrj(multiLineString);
            IPolyline esriPolyline = ConvertToEsriGeom.FromMultiLineString(geoPrj);

            ITextElement textElement = new TextElementClass();
            textElement.ScaleText = false;
            textElement.Text = text;
            IElement pElementOfText = ((ESRI.ArcGIS.Carto.IElement)(textElement));

            ITextSymbol textSymbol = symbol as ITextSymbol;
            textSymbol.Angle = direction;
            
            IGeometry pGeometry = (ESRI.ArcGIS.Geometry.IGeometry)esriPolyline;

            pElementOfText.Geometry = pGeometry;
            textElement.Symbol = textSymbol;

            //textElement.Symbol.Angle = direction;

            textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
            textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline;
            textElement.Symbol = textSymbol;

            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pElementOfText, 0);
            GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            return GetHandle(pElementOfText);
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

        public void DrawMultiPolygon(Aran.Geometries.MultiPolygon poly, bool isNullColor)
        {
            IPolygon esriPolygon = Aran.Converters.ConvertToEsriGeom.FromMultiPolygon(poly);
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
            pFillSym.Style = esriSimpleFillStyle.esriSFSSolid;
            (pFillSym as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen; // esriSFSNull 'esriSFSDiagonalCross

            ILineSymbol pLineSimbol = new SimpleLineSymbol();
            (pLineSimbol as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen;

            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = Aran.PANDA.Common.ARANFunctions.RGB(128, 128, 128); ;

            pLineSimbol.Color = lineRgb;
            pLineSimbol.Width = 2;
            pFillSym.Outline = pLineSimbol;

            pFillShpElement.Symbol = pFillSym;

            IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
            pGraphics.AddElement(pElementofPoly, 0);
            GlobalParams.HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            return GetHandle(pElementofPoly);
        }

        #endregion

        private int _index;
        private Dictionary<int, IElement> _drawedElements;
    }
}
