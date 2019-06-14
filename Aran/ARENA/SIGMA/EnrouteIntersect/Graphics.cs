using System;
using System.Collections.Generic;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace EnrouteIntersect
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
	                IGraphicsContainer pGraphics = ((IActiveView) GlobalParams.HookHelper.FocusMap).GraphicsContainer;
                    pGraphics.DeleteElement(_drawedElements[handle]);
					( ( IActiveView ) GlobalParams.HookHelper.FocusMap ).PartialRefresh ( esriViewDrawPhase.esriViewGraphics, null, null );
                }
                catch { }
                _drawedElements.Remove(handle);
            }
        }

		public int DrawMultiLineString ( IPolyline esriPolyline)
		{
			ILineElement pLineElement = ( ( ESRI.ArcGIS.Carto.ILineElement ) ( new LineElement ( ) ) );
			IElement pElementOfpLine = ( ( ESRI.ArcGIS.Carto.IElement ) ( pLineElement ) );
			IGeometry pGeometry = ( ( ESRI.ArcGIS.Geometry.IGeometry ) ( esriPolyline ) );

			//if (GlobalParams.HookHelper.ActiveView is IMap)
				pElementOfpLine.Geometry = pGeometry;
			//else
			//{
			//	IPointCollection pntColl = (IPointCollection) esriPolyline;
			//	var dataView = (IActiveView) GlobalParams.HookHelper.FocusMap;
			//	var pgLayView = (IActiveView) GlobalParams.HookHelper.PageLayout;
			//	int x, y;
			//	IPolyline pageLayPolyline = new PolylineClass();
			//	IPointCollection pntCollResult = (IPointCollection) pageLayPolyline;
			//	for ( int i = 0; i < pntColl.PointCount; i++ )
			//	{
			//		dataView.ScreenDisplay.DisplayTransformation.FromMapPoint(pntColl.Point[i], out x, out y);
			//		pntCollResult.AddPoint(pgLayView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y));
			//	}
			//	pElementOfpLine.Geometry = pageLayPolyline;
			//}

			IRgbColor pRGB = new RgbColor ( );
			pRGB.RGB = 255;
			ISimpleLineSymbol pLineSym = new SimpleLineSymbol ( );
			pLineSym.Color = pRGB;
			pLineSym.Style = esriSimpleLineStyle.esriSLSSolid;
			pLineSym.Width = 3;

			pLineElement.Symbol = pLineSym as ISimpleLineSymbol;

			IGraphicsContainer pGraphics = ((IActiveView) GlobalParams.HookHelper.FocusMap).GraphicsContainer;
			pGraphics.AddElement ( pElementOfpLine, 0 );
			( ( IActiveView ) GlobalParams.HookHelper.FocusMap ).PartialRefresh ( esriViewDrawPhase.esriViewGraphics, null, null );

			return GetHandle ( pElementOfpLine );
		}
		
        #endregion

		public void Clear()
		{
			var keys = new List<int> (_drawedElements.Keys);
			foreach (var key in keys)
			{
				SafeDeleteGraphic (key);
			}
			((IActiveView) GlobalParams.HookHelper.FocusMap).Refresh();
		}

        private int _index;
        private Dictionary<int, IElement> _drawedElements;
    }
}
