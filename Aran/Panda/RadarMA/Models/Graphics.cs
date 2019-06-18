﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.Models
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
            simpleMarkerSymbol.Size = 8;

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
            pRGB.RGB = Aran.Panda.Common.ARANFunctions.RGB(128, 128, 128);
            ISimpleFillSymbol pFillSym = new SimpleFillSymbol();
            IFillShapeElement pFillShpElement = ((ESRI.ArcGIS.Carto.IFillShapeElement)(new PolygonElement()));

            pElementofPoly = ((ESRI.ArcGIS.Carto.IElement)(pFillShpElement));
            pElementofPoly.Geometry = ((ESRI.ArcGIS.Geometry.IGeometry)(esriPolygon));


            pFillSym.Color = pRGB;
            pFillSym.Style = esriSimpleFillStyle.esriSFSNull;
            (pFillSym as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen; // esriSFSNull 'esriSFSDiagonalCross

            ILineSymbol pLineSimbol = new SimpleLineSymbol();
            (pLineSimbol as ISymbol).ROP2 = esriRasterOpCode.esriROPXOrPen;

            IRgbColor lineRgb = new RgbColor();
            lineRgb.RGB = Aran.Panda.Common.ARANFunctions.RGB(128, 128, 128); ;

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
            pRGB.RGB = Aran.Panda.Common.ARANFunctions.RGB(128, 128, 128);
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
            lineRgb.RGB = Aran.Panda.Common.ARANFunctions.RGB(128, 128, 128); ;

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

        #endregion

        private int _index;
        private Dictionary<int, IElement> _drawedElements;
    }
}