using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

namespace EOSID
{
	public static class Graphics
	{
		public static IElement DrawPoint(IPoint pPoint, int Color = - 1, bool drawFlg = true)
		{
			IElement pResultElement = new MarkerElement();
			pResultElement.Geometry = pPoint;

			IRgbColor pRGB = new RgbColor();
			if (Color != -1)
				pRGB.RGB = Color;
			else
			{
				Random rnd = new Random();
				pRGB.Red = rnd.Next(256);
				pRGB.Green = rnd.Next(256);
				pRGB.Blue = rnd.Next(256);
			}

			ISimpleMarkerSymbol pMarkerSym = new SimpleMarkerSymbol();
			pMarkerSym.Color = pRGB;
			pMarkerSym.Size = 8;

			IMarkerElement pMarkerElement = (IMarkerElement)pResultElement;
			pMarkerElement.Symbol = pMarkerSym;

			if (drawFlg)
			{
				IGraphicsContainer pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(pResultElement, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return pResultElement;
		}

		public static IElement DrawPointWithText(IPoint pPoint, string sText, int Color = -1, bool drawFlg = true)
		{
			ITextSymbol pTextSymbol = new TextSymbol();
			pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
			pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;

			IElement pElementOfText = new TextElement();
			pElementOfText.Geometry = pPoint;

			ITextElement pTextElement = (ITextElement)pElementOfText;
			pTextElement.Text = sText;
			pTextElement.ScaleText = false;
			pTextElement.Symbol = pTextSymbol;

			IRgbColor pRGB = new RgbColor();
			if (Color != -1)
				pRGB.RGB = Color;
			else
			{
				Random rnd = new Random();
				pRGB.Red = rnd.Next(256);
				pRGB.Green = rnd.Next(256);
				pRGB.Blue = rnd.Next(256);
			}

			ISimpleMarkerSymbol pMarkerSym = new SimpleMarkerSymbol();
			pMarkerSym.Color = pRGB;
			pMarkerSym.Size = 6;

			IElement pElementofpPoint = new MarkerElement();
			pElementofpPoint.Geometry = pPoint;

			IMarkerElement pMarkerShpElement = (IMarkerElement)pElementofpPoint;
			pMarkerShpElement.Symbol = pMarkerSym;

			IGroupElement pGroupElement = (IGroupElement)new GroupElement();
			pGroupElement.AddElement(pElementofpPoint);
			pGroupElement.AddElement(pTextElement as IElement);
			IElement result = (IElement)pGroupElement;

			if (drawFlg)
			{
				IGraphicsContainer pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(result, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return result;
		}

		public static IElement DrawLine(ILine pLine, int Color = -1, double Width = 1, esriSimpleLineStyle Style = esriSimpleLineStyle.esriSLSSolid, bool drawFlg = true)
		{
			IPolyline pPolyLine = (IPolyline)new Polyline();
			pPolyLine.FromPoint = pLine.FromPoint;
			pPolyLine.ToPoint = pLine.ToPoint;
			return DrawPolyline(pPolyLine, Color, Width, Style, drawFlg);
		}

		public static IElement DrawLine(IPointCollection pLine, int Color = -1, double Width = 1, esriSimpleLineStyle Style = esriSimpleLineStyle.esriSLSSolid, bool drawFlg = true)
		{
			return DrawLine(pLine as ILine, Color, Width, Style, drawFlg);
		}

		public static IElement DrawPolyline(IPolyline pLine, int Color = - 1, double Width = 1, esriSimpleLineStyle Style = esriSimpleLineStyle.esriSLSSolid, bool drawFlg = true)
		{
			IRgbColor pRGB = new RgbColor();
			if (Color != -1)
				pRGB.RGB = Color;
			else
			{
				Random rnd = new Random();
				pRGB.Red = rnd.Next(256);
				pRGB.Green = rnd.Next(256);
				pRGB.Blue = rnd.Next(256);
			}

			ISimpleLineSymbol pLineSym = new SimpleLineSymbol();
			pLineSym.Color = pRGB;
			pLineSym.Style = Style;
			pLineSym.Width = Width;

			IElement pResultElement = new LineElement();
			pResultElement.Geometry = pLine;

			ILineElement pLineElement = (ILineElement)pResultElement;
			pLineElement.Symbol = pLineSym;

			if (drawFlg)
			{
				IGraphicsContainer pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(pResultElement, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return pResultElement;
		}

		public static IElement DrawPolyline(IPointCollection pLine, int pColor = -1, double Width = 1, esriSimpleLineStyle Style = esriSimpleLineStyle.esriSLSSolid, bool drawFlg = true)
		{
			return DrawPolyline(pLine as IPolyline, pColor, Width, Style, drawFlg);
		}

		public static IElement DrawPolyLineSFS(IPolyline pLine, ISimpleLineSymbol pLineSym, bool drawFlg = true)
		{
			IElement pResultElement = new LineElement();
			pResultElement.Geometry = pLine;

			ILineElement pLineElement = (ILineElement)pResultElement;
			pLineElement.Symbol = pLineSym;

			if (drawFlg)
			{
				IGraphicsContainer pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(pResultElement, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}

			return pResultElement;
		}

		public static IElement DrawPolygon(IPolygon pPolygon, int Color = -1, esriSimpleFillStyle SFStyle = esriSimpleFillStyle.esriSFSNull, bool drawFlg = true)
		{
			IRgbColor pRGB = new RgbColor();

			if (Color != -1)
				pRGB.RGB = Color;
			else
			{
				Random rnd = new Random();
				pRGB.Red = rnd.Next(256);
				pRGB.Green = rnd.Next(256);
				pRGB.Blue = rnd.Next(256);
			}

			ILineSymbol pLineSimbol = new SimpleLineSymbol();
			pLineSimbol.Color = pRGB;
			pLineSimbol.Width = 1;

			ISimpleFillSymbol pFillSym = new SimpleFillSymbol();
			pFillSym.Color = pRGB;
			pFillSym.Style = SFStyle;
			pFillSym.Outline = pLineSimbol;

			IElement pElementofPoly = new PolygonElement();
			pElementofPoly.Geometry = pPolygon;

			IFillShapeElement pFillShpElement = (IFillShapeElement)pElementofPoly;
			pFillShpElement.Symbol = pFillSym;

			if (drawFlg)
			{
				IGraphicsContainer pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(pElementofPoly, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return pElementofPoly;
		}

		public static IElement DrawPolygon(IPointCollection pPolygon, int Color = -1, esriSimpleFillStyle SFStyle = esriSimpleFillStyle.esriSFSNull, bool drawFlg = true)
		{
			return DrawPolygon(pPolygon as IPolygon, Color, SFStyle, drawFlg);
		}

		public static IElement DrawPolygonSFS(IPolygon pPolygon, ISimpleFillSymbol pFillSym, bool drawFlg = true)
		{
			IElement pElementofPoly = new PolygonElement();
			pElementofPoly.Geometry = pPolygon;

			IFillShapeElement pFillShpElement = (IFillShapeElement)pElementofPoly;
			pFillShpElement.Symbol = pFillSym;

			if (drawFlg)
			{
				IGraphicsContainer pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(pElementofPoly, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return pElementofPoly;
		}

		public static IElement DrawPolygonSFS(IPointCollection pPolygon, ISimpleFillSymbol pFillSym, bool drawFlg = true)
		{
			return DrawPolygonSFS(pPolygon as IPolygon, pFillSym, drawFlg);
		}

		public static void DeleteElement(IElement pElement)
		{
			try
			{
				IGraphicsContainer pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.DeleteElement(pElement);
			}
			catch
			{
			}
		}

		public static void RefreshGraphics()
		{
			IGraphicsContainer pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
			GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
		}

	}
}
