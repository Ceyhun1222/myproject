#define CUSTOM_GRAPHICS_

using System;
using System.Collections.Generic;
using Aran.AranEnvironment;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using Aran.Geometries;
using Aran.Converters;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries.SpatialReferences;
using Aran.Geometries.Operators;

namespace MapEnv
{
	internal abstract class AranDrawingBase : IAranDrawing
	{
		public AranDrawingBase()
		{
			_drawedElements = new Dictionary<int, IElement>();
			_index = 0;
		}

		private Dictionary<int, IElement> _drawedElements;
		private int _index;

		public int DrawPoint(Aran.Geometries.Point point, Aran.AranEnvironment.Symbols.PointSymbol symbol, Boolean isVisible = true, Boolean isLocked = true)
		{
			IPoint esriPoint = ConvertToEsriGeom.FromPoint(point);
			ISimpleMarkerSymbol pMarkerSym = null;
			IMarkerElement pMarkerShpElement = ((ESRI.ArcGIS.Carto.IMarkerElement)(new MarkerElement()));
			IElement pElementofpPoint = ((ESRI.ArcGIS.Carto.IElement)(pMarkerShpElement));

			pElementofpPoint.Geometry = esriPoint;

			IRgbColor pRGB = new RgbColor();
			pRGB.RGB = symbol.Color;

			pMarkerSym = new SimpleMarkerSymbol();
			pMarkerSym.Color = pRGB;
			pMarkerSym.Size = symbol.Size;
			pMarkerSym.Style = (esriSimpleMarkerStyle)symbol.Style;

			pMarkerShpElement.Symbol = pMarkerSym;

#if CUSTOM_GRAPHICS

            var id = Globals.GraphicsLayer.AddGeometry(esriPoint as IGeometry, pMarkerSym as ISymbol);
            Globals.GraphicsLayer.RefreshLayer();
            return id;

#else
			if (isVisible)
			{
				IGraphicsContainer pGraphics = GraphicContainer;
				pGraphics.AddElement(pElementofpPoint, 0);
				if (isLocked)
					pElementofpPoint.Locked = true;
				ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return GetHandle(pElementofpPoint);
#endif
		}

		public int DrawPoint(Aran.Geometries.Point point, int color = -1, Boolean isVisible = true, Boolean isLocked = true)
		{
			PointSymbol pointSymbol = new Aran.AranEnvironment.Symbols.PointSymbol();
			if (color < 0)
			{
				Random rnd = new Random();
				color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
			}

			pointSymbol.Color = color;
			//pointSymbol.Size = 8;
			pointSymbol.Style = ePointStyle.smsCircle;
			return DrawPoint(point, pointSymbol, isVisible, isLocked);
		}

		public int DrawPoint(Aran.Geometries.Point point, Aran.AranEnvironment.Symbols.ePointStyle style, int color = -1, Boolean isVisible = true, Boolean isLocked = true)
		{
			PointSymbol pointSymbol = new Aran.AranEnvironment.Symbols.PointSymbol();
			if (color < 0)
			{
				Random rnd = new Random();
				color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
			}

			pointSymbol.Color = color;
			pointSymbol.Size = 8;
			pointSymbol.Style = style;
			return DrawPoint(point, pointSymbol, isVisible, isLocked);
		}

		public int DrawPointWithText(Aran.Geometries.Point point, string text, PointSymbol symbol, Boolean isVisible = true, Boolean isLocked = true)
		{
			IPoint esriPoint = Aran.Converters.ConvertToEsriGeom.FromPoint(point);
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

			pRGB.RGB = (int)symbol.Color;

			ISimpleMarkerSymbol pMarkerSym = new SimpleMarkerSymbol();
			pMarkerSym.Color = pRGB;
			pMarkerSym.Size = symbol.Size;
			pMarkerSym.Style = (esriSimpleMarkerStyle)symbol.Style;
			pMarkerShpElement.Symbol = pMarkerSym;

			IGroupElement pGroupElement = ((ESRI.ArcGIS.Carto.IGroupElement)(new GroupElement()));
			pGroupElement.AddElement(pElementofpPoint);
			pElementofpPoint.Locked = true;
			pGroupElement.AddElement(pTextElement as IElement);

			IElement pCommonElement = ((ESRI.ArcGIS.Carto.IElement)(pGroupElement));

#if CUSTOM_GRAPHICS

            var id = Globals.GraphicsLayer.AddGeometry(esriPoint, text, pMarkerSym as ISymbol);
            Globals.GraphicsLayer.RefreshLayer();
            return id;

#else

			if (isVisible)
			{
				IGraphicsContainer pGraphics = GraphicContainer;
				pGraphics.AddElement(pCommonElement, 0);
				if (isLocked)
					pCommonElement.Locked = true;
				ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}

			return GetHandle(pCommonElement);
#endif
		}

		public int DrawPointWithText(Aran.Geometries.Point point, string text, int color = -1, Boolean isVisible = true, Boolean isLocked = true)
		{
			PointSymbol pointSymbol = new Aran.AranEnvironment.Symbols.PointSymbol();
			if (color < 0)
			{
				Random rnd = new Random();
				color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
			}

			pointSymbol.Color = color;
			pointSymbol.Size = 8;
			pointSymbol.Style = ePointStyle.smsCircle;
			return DrawPointWithText(point, text, pointSymbol, isVisible, isLocked);
		}

		public int DrawLineString(LineString lineString, Aran.AranEnvironment.Symbols.LineSymbol symbol, Boolean isVisible = true, Boolean isLocked = true)
		{
			MultiLineString multiLineString = new MultiLineString();
			multiLineString.Add(lineString);
			return DrawMultiLineString(multiLineString, symbol, isVisible, isLocked);
		}

		public int DrawLineString(LineString lineString, int width, int color, Boolean isVisible = true, Boolean isLocked = true)
		{
			MultiLineString multiLineString = new MultiLineString();
			multiLineString.Add(lineString);

			LineSymbol lineSymbol = new LineSymbol();

			if (color < 0)
			{
				Random rnd = new Random();
				color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
			}

			lineSymbol.Color = color;
			lineSymbol.Style = eLineStyle.slsSolid;
			lineSymbol.Width = width;
			return DrawMultiLineString(multiLineString, lineSymbol, isVisible, isLocked);
		}

		public int DrawMultiLineString(MultiLineString multiLineString, Aran.AranEnvironment.Symbols.LineSymbol symbol, Boolean isVisible = true, Boolean isLocked = true)
		{
			IPolyline esriPolyline = ConvertToEsriGeom.FromMultiLineString(multiLineString);

			ILineElement pLineElement = ((ESRI.ArcGIS.Carto.ILineElement)(new LineElement()));
			IElement pElementOfpLine = ((ESRI.ArcGIS.Carto.IElement)(pLineElement));
			IGeometry pGeometry = ((ESRI.ArcGIS.Geometry.IGeometry)(esriPolyline));

			pElementOfpLine.Geometry = pGeometry;

			IRgbColor pRGB = new RgbColor();
			pRGB.RGB = (int)symbol.Color;
			ISimpleLineSymbol pLineSym = new SimpleLineSymbol();
			pLineSym.Color = pRGB;
			pLineSym.Style = (esriSimpleLineStyle)symbol.Style;
			pLineSym.Width = symbol.Width;

			pLineElement.Symbol = pLineSym;

#if CUSTOM_GRAPHICS

            var id = Globals.GraphicsLayer.AddGeometry(esriPolyline as IGeometry, pLineSym as ISymbol);
            Globals.GraphicsLayer.RefreshLayer();
            return id;

#else
			if (isVisible)
			{
				IGraphicsContainer pGraphics = GraphicContainer;
				pGraphics.AddElement(pElementOfpLine, 0);
				if (isLocked)
					pElementOfpLine.Locked = true;
				ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return GetHandle(pElementOfpLine);
#endif
		}

		public int DrawMultiLineString(MultiLineString multiLineString, int width, int color, Boolean isVisible = true, Boolean isLocked = true)
		{
			if (color < 0)
			{
				Random rnd = new Random();
				color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
			}

			LineSymbol lineSymbol = new LineSymbol();
			lineSymbol.Color = color;
			lineSymbol.Style = eLineStyle.slsSolid;
			lineSymbol.Width = width;
			return DrawMultiLineString(multiLineString, lineSymbol, isVisible, isLocked);
		}

		public int DrawRing(Aran.Geometries.Ring ring, Aran.AranEnvironment.Symbols.FillSymbol symbol, Boolean isVisible = true, Boolean isLocked = true)
		{
			Aran.Geometries.Polygon aranPolygon = new Aran.Geometries.Polygon();
			aranPolygon.ExteriorRing = (Aran.Geometries.Ring)ring.Clone();
			return DrawPolygon(aranPolygon, symbol, isVisible, isLocked);
		}

		public int DrawRing(Aran.Geometries.Ring ring, Aran.AranEnvironment.Symbols.eFillStyle style, int color, Boolean isVisible = true, Boolean isLocked = true)
		{
			if (color < 0)
			{
				Random rnd = new Random();
				color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
			}

			FillSymbol fillSymbol = new FillSymbol();
			fillSymbol.Color = color;
			fillSymbol.Style = style;
			fillSymbol.Outline = new LineSymbol();
			fillSymbol.Outline.Color = color;
			fillSymbol.Outline.Size = fillSymbol.Size;

			return DrawRing(ring, fillSymbol, isVisible, isLocked);
		}

		public int DrawPolygon(Aran.Geometries.Polygon polygon, FillSymbol symbol, Boolean isVisible = true, Boolean isLocked = true)
		{
			MultiPolygon multiPolygon = new MultiPolygon();
			multiPolygon.Add(polygon);
			return DrawMultiPolygon(multiPolygon, symbol, isVisible, isLocked);
		}

		public int DrawPolygon(Aran.Geometries.Polygon polygon, eFillStyle style, int color, Boolean isVisible = true, Boolean isLocked = true)
		{
			if (color < 0)
			{
				Random rnd = new Random();
				color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
			}

			FillSymbol fillSymbol = new FillSymbol();
			fillSymbol.Color = color;
			fillSymbol.Style = style;
			fillSymbol.Outline = new LineSymbol();
			fillSymbol.Outline.Color = color;
			fillSymbol.Outline.Size = fillSymbol.Size;
			return DrawPolygon(polygon, fillSymbol, isVisible, isLocked);
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

			SimpleLineSymbol pLineSimbol = new SimpleLineSymbol();

			IRgbColor lineRgb = new RgbColor();
			lineRgb.RGB = (int)symbol.Outline.Color;

			pLineSimbol.Color = lineRgb;
			pLineSimbol.Width = symbol.Outline.Size;
			pLineSimbol.Style = (esriSimpleLineStyle)symbol.Outline.Style;
			pFillSym.Outline = pLineSimbol;


			pFillShpElement.Symbol = pFillSym;

#if CUSTOM_GRAPHICS
            var id = Globals.GraphicsLayer.AddGeometry(esriPolygon as IGeometry, pFillSym as ISymbol);
            Globals.GraphicsLayer.RefreshLayer();
            return id;
#else
			if (isVisible)
			{
				IGraphicsContainer pGraphics = GraphicContainer;
				pGraphics.AddElement(pElementofPoly, 0);
				if (isLocked)
					pElementofPoly.Locked = true;
				ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}

			return GetHandle(pElementofPoly);
#endif
		}

		public int DrawMultiPolygon(MultiPolygon multiPolygon, Aran.AranEnvironment.Symbols.eFillStyle style, int color, Boolean isVisible = true, Boolean isLocked = true)
		{
			if (color < 0)
			{
				Random rnd = new Random();
				color = rnd.Next(256) | (rnd.Next(256) << 8) | (rnd.Next(256) << 16);
			}

			FillSymbol fillSymbol = new FillSymbol();
			fillSymbol.Color = color;
			fillSymbol.Style = style;
			fillSymbol.Outline = new LineSymbol();
			fillSymbol.Outline.Color = color;
			fillSymbol.Outline.Size = fillSymbol.Size;
			return DrawMultiPolygon(multiPolygon, fillSymbol, isVisible, isLocked);
		}

		public void SetVisible(int graphicHandle, bool isVisible)
		{
			if (_drawedElements.ContainsKey(graphicHandle))
			{
				IGraphicsContainer pGraphics = GraphicContainer;

				if (isVisible)
				{
					if (!IsHandleInContainer(graphicHandle))
					{
						GraphicContainer.AddElement(_drawedElements[graphicHandle], 0);
						ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
					}
				}
				else if (IsHandleInContainer(graphicHandle))
				{
					GraphicContainer.DeleteElement(_drawedElements[graphicHandle]);
					ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
				}
			}
		}

		public void ShowGraphic(int handle, bool isVisible)
		{
			if (!_drawedElements.ContainsKey(handle))
				return;

			var elem = _drawedElements[handle];

			try
			{
				if (isVisible)
					GraphicContainer.AddElement(elem, 0);
				else
					GraphicContainer.DeleteElement(elem);

				ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			catch { }
		}

		public void DeleteGraphic(int handle)
		{

#if CUSTOM_GRAPHICS

            Globals.GraphicsLayer.RemoveGeometry(handle);
            Globals.GraphicsLayer.RefreshLayer();

#else

			if (_drawedElements.ContainsKey(handle))
			{
				try
				{
					GraphicContainer.DeleteElement(_drawedElements[handle]);
					ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

					_drawedElements.Remove(handle);
				}
				catch { }
			}
#endif
		}

		public void SafeDeleteGraphic(int handle)
		{
			if (_drawedElements.ContainsKey(handle))
			{
				try
				{
					IGraphicsContainer pGraphics = GraphicContainer;
					pGraphics.DeleteElement(_drawedElements[handle]);
					ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
				}
				catch { }
				_drawedElements.Remove(handle);
			}
		}

		public void ShowAnimation(bool show)
		{
			throw new NotImplementedException();
		}

		public void HideAnimation()
		{
			throw new NotImplementedException();
		}

		public void ShowAnimation()
		{
			throw new NotImplementedException();
		}

		public void Refresh()
		{
			ActiveView.Refresh();
		}

		private int GetHandle(IElement esriElement)
		{
			_index++;
			_drawedElements.Add(_index, esriElement);
			return _index;
		}

		private bool IsHandleInContainer(int graphicHandle)
		{
			try
			{
				GraphicContainer.UpdateElement(_drawedElements[graphicHandle]);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private bool IsElementInContainer(IElement iElement)
		{
			IGraphicsContainer pGraphics = GraphicContainer;
			pGraphics.Reset();
			IElement esriElement = pGraphics.Next();
			while (esriElement != null)
			{
				if (esriElement == iElement)
					return true;
				esriElement = pGraphics.Next();
			}
			return false;
		}

		protected abstract IGraphicsContainer GraphicContainer { get; }
		protected abstract IActiveView ActiveView { get; }
	}
}
