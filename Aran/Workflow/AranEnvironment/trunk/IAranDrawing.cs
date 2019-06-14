using Aran.Geometries;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries.SpatialReferences;
using System.Collections.Generic;
using System;

namespace Aran.AranEnvironment
{
	public interface IAranDrawing
	{
		int DrawPoint(Point point, PointSymbol symbol, Boolean isVisible = true, Boolean isLocked = true);
		int DrawPoint(Point point, Int32 color = -1, Boolean isVisible = true, Boolean isLocked = true);
		int DrawPoint(Point point, ePointStyle style, Int32 color = -1, Boolean isVisible = true, Boolean isLocked = true);

		int DrawPointWithText(Point point, string text, PointSymbol symbol, Boolean isVisible = true, Boolean isLocked = true);
		int DrawPointWithText(Point point, string text, Int32 color = -1, Boolean isVisible = true, Boolean isLocked = true);

		int DrawLineString(LineString lineString, LineSymbol lineSymbol, Boolean isVisible = true, Boolean isLocked = true);
		int DrawLineString(LineString lineString, Int32 width = 1, Int32 color = -1, Boolean isVisible = true, Boolean isLocked = true);
		int DrawMultiLineString(MultiLineString multiLineString, LineSymbol lineSymbol, Boolean isVisible = true, Boolean isLocked = true);
		int DrawMultiLineString(MultiLineString multiLineString, Int32 width = 1, Int32 color = -1, Boolean isVisible = true, Boolean isLocked = true);

		int DrawRing(Ring ring, FillSymbol fillSymbol, Boolean isVisible = true, Boolean isLocked = true);
		int DrawRing(Ring ring, eFillStyle fillStyle = eFillStyle.sfsHollow, Int32 color = -1, Boolean isVisible = true, Boolean isLocked = true);
		int DrawPolygon(Polygon polygon, FillSymbol fillSymbol, Boolean isVisible = true, Boolean isLocked = true);
		int DrawPolygon(Polygon polygon, eFillStyle fillStyle = eFillStyle.sfsHollow, Int32 color = -1, Boolean isVisible = true, Boolean isLocked = true);
		int DrawMultiPolygon(MultiPolygon multiPolygon, FillSymbol fillSymbol, Boolean isVisible = true, Boolean isLocked = true);
		int DrawMultiPolygon(MultiPolygon multiPolygon, eFillStyle fillStyle = eFillStyle.sfsHollow, Int32 color = -1, Boolean isVisible = true, Boolean isLocked = true);

		void SetVisible(Int32 graphicHandle, Boolean isVisible);
		void DeleteGraphic(Int32 handle);
		void ShowGraphic(int handle, bool isVisible);
		void SafeDeleteGraphic(Int32 handle);
		void ShowAnimation(Boolean show);
		void HideAnimation();
		void ShowAnimation();
		void Refresh();
	}
}
