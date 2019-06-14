using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ANCOR.MapCore;
using ANCOR.MapElements;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace ChartTypeA.Models
{
    public class VerticalObstacleCreater
    {
        private List<ObstacleReport> _obstacleList;
        private double _horizontalScale;
        private double _verticalScale;
        private IActiveView _dataView;
        private IActiveView _pageLayoutView;
        private IGraphicsContainer _pageLayGraphics;
        private double _frameHeight;
        private double _baseElevationInM;
        private double _startHeight;
        private IPoint _rwyCntLinePt;
        public VerticalObstacleCreater(List<ObstacleReport> obstacleList,double verticalScale,double horizontalScale,double frameHegiht,double baseElevation,IPoint rwyCntLinePt)
        {
            _obstacleList = obstacleList;
            _horizontalScale = horizontalScale;
            _verticalScale = verticalScale;
            _frameHeight = frameHegiht;
            _baseElevationInM =Common.DeConvertHeight(baseElevation);

            var focusMap = GlobalParams.HookHelper.FocusMap;
            _dataView = (IActiveView)focusMap;
            var pageLayout = GlobalParams.HookHelper.PageLayout;
            _pageLayoutView = (IActiveView)pageLayout;
            _pageLayGraphics = (IGraphicsContainer)pageLayout;

            _rwyCntLinePt = rwyCntLinePt;
        }

        public void Create()
        {
            _horizontalScale = GlobalParams.HookHelper.FocusMap.MapScale;
            _verticalScale = _horizontalScale/10;

            ObstaGroupElement =(IGroupElement) new GroupElement();
            var graphicsContainer = GlobalParams.HookHelper.PageLayout as IGraphicsContainer;

            graphicsContainer.Reset();
            var element = graphicsContainer.Next();

            ITextSymbol symbol = null;

            int x, y;
            _dataView.ScreenDisplay.DisplayTransformation.FromMapPoint(_rwyCntLinePt, out x, out y);
            var centerPtPgLayout = _pageLayoutView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
            _startHeight = centerPtPgLayout.Y + _frameHeight;

            while (element != null)
            {
                IElementProperties3 elemproperties = element as IElementProperties3;
                if (elemproperties.Name == "ObstacleVerSymbol")
                {
                    var tmpElement = element as ITextElement;
                    if (tmpElement != null)
                    {
                        symbol = tmpElement.Symbol;

                    }
                }
                element = graphicsContainer.Next();
            }

            int i = 0;
            foreach (var obs in _obstacleList)
            {
                i++;
                IArea area = obs.GeomPrj as IArea;
                IPoint centeroid = obs.GeomPrj as IPoint;
                if (area != null)
                    centeroid = area.Centroid;

                if (obs.GeomPrj.GeometryType == esriGeometryType.esriGeometryPolyline)
                    centeroid = (obs.GeomPrj as IPointCollection).Point[0];

                _dataView.ScreenDisplay.DisplayTransformation.FromMapPoint((IPoint)centeroid, out x, out y);
                var pnt1PgLayout = _pageLayoutView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                pnt1PgLayout.Y = _startHeight; // pnt1PgLayout.Y + _frameHeight;

                IPoint pnt2PgLayout = new Point();
                pnt2PgLayout.X = pnt1PgLayout.X;
                var obsElevationInM = Common.DeConvertHeight(obs.Elevation);
                pnt2PgLayout.Y = pnt1PgLayout.Y + (obsElevationInM - _baseElevationInM) * 100 / _verticalScale;
                var lineElem = CreateLineElement(pnt1PgLayout, pnt2PgLayout);


                IPoint circleGeometry = pnt1PgLayout;
                circleGeometry.Y = _startHeight + Math.Abs((pnt2PgLayout.Y - _startHeight) / 2);

                ITextElement circleTextElement = new TextElementClass();
                circleTextElement.Symbol = symbol;
                circleTextElement.Text = i.ToString();
                circleTextElement.Symbol.Text = i.ToString();


                IElement circleElem = circleTextElement as IElement;
                circleElem.Geometry = circleGeometry;

                IGroupElement grElement = new GroupElementClass();
                grElement.AddElement(lineElem);
                grElement.AddElement(circleElem);


                //ObstaGroupElement.AddElement(circleElem);
                ObstaGroupElement.AddElement((GroupElement) grElement);
                //_pageLayGraphics.AddElement((GroupElement)grElement, 0);
                //_pageLayoutView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
        }

        private IElement CreateLineElement(params IPoint[] points)
        {
            List<IPoint> list = new List<IPoint>();
            foreach (var item in points)
            {
                list.Add(item);
            }
            return GlobalParams.UI.GetObstaclePolylineElement(list, 235, esriSimpleLineStyle.esriSLSSolid);
        }

        public IGroupElement ObstaGroupElement { get; set; }

        public double FrameHeight
        {
            get { return _frameHeight; }
            set
            {
                _frameHeight = value;
            }
        }
        
    }
}
