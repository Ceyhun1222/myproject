using System;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using Aran.Panda.Common;

namespace ChartTypeA.Models
{
    public class Profile
    {
        private IActiveView _activeView;
        public Profile(RwyDirWrapper rwyDir)
        {
            var startPoint = rwyDir.Threshold;

            IDisplayTransformation mapTransformation = ((IActiveView)GlobalParams.HookHelper.FocusMap).ScreenDisplay.DisplayTransformation;

            var pageLayout = GlobalParams.HookHelper.PageLayout;
            _activeView = pageLayout as IActiveView;
            if (_activeView == null) return;

            var pageTransformation = _activeView.ScreenDisplay.DisplayTransformation;
            
            int x, y;

            mapTransformation.FromMapPoint(startPoint,out x,out y);
            IPoint pagePoint = pageTransformation.ToMapPoint(x, y);

            //displayTransformation.FromMapPoint(startPoint, out x, out y);

            var pt = new Point();
            pt.X = x;
            pt.Y = y;
            //DrawEsriPoint(pt);

        }

        public void DrawEsriPoint(ESRI.ArcGIS.Geometry.IPoint ptPrj)
        {
            var pMarkerShpElement = new MarkerElement() as IMarkerElement;

            ISimpleMarkerSymbol ptSymbol = new SimpleMarkerSymbol();
            ISimpleMarkerSymbol simpleMarkerSymbol = ptSymbol as ISimpleMarkerSymbol;
            simpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            simpleMarkerSymbol.Size = 8;

            var m_hookHelper = GlobalParams.HookHelper;
            var docPageLayout = m_hookHelper.PageLayout;
            var docActiveView = docPageLayout as IActiveView;
            var GContainer = docPageLayout as IGraphicsContainer;
            //Verify ArcMap is in layout view.
            if (m_hookHelper.ActiveView == m_hookHelper.PageLayout)
            {
                var docTextElement = new TextElementClass();
                var docElement = docTextElement as IElement;
                //Create a point from the x,y coordinate parameters.
                var PtPoint = ptPrj;// docActiveView.ScreenDisplay.DisplayTransformation.ToMaPtPoint(X, Y);
                docTextElement.Text = "My Map";
                docElement.Geometry = PtPoint;
                GContainer.AddElement(docTextElement as IElement, 0);
                //Refresh only the page layout's graphics.
                docActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            else
            {
                //MessageBox.Show("This tool only works in layout view");
                return;
            }


           // return GetHandle(pointElement);
        }

    }
}
