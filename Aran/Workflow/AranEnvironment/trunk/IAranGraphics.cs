using System;
using Aran.Geometries;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries.SpatialReferences;
using System.Collections.Generic;

namespace Aran.AranEnvironment
{
	public interface IAranGraphics:IAranDrawing
	{
        void SetMapTool (MapTool mapTool);
        bool SelectSymbol(BaseSymbol inSymbol, out BaseSymbol outSymbol, int hwnd);
        List<Geometry> GetSelectedGraphicGeometries();

        SpatialReference ViewProjection { get; set; }
        SpatialReference WGS84SR { get; }

        PointSymbol SelectedPointSymbol { get; set; }
        LineSymbol SelectedLineSymbol { get; set; }
        FillSymbol SelectedFillSymbol { get; set; }

		void GetExtent(out double xmin, out double ymin, out double xmax, out double ymax);
		Box Extent { get; set; }

		void SetExtent(double xmin, double ymin, double xmax, double ymax);
	}
}
