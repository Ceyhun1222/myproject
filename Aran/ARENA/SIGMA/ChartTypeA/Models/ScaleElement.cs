using ESRI.ArcGIS.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace ChartTypeA.Models
{
    public enum Side{
        Right=1,
        Left=-1
    }

    public class ScaleElement
    {
        private IGroupElement _scaleGrp;
        private IGroupElement _textGrp;
        private IGraphicsContainer _pageLayGraphics;
        const int FontSize = 6;
        private const double WidthOfEachParam = 0.2;
        private const double DistFromCenter = 0.1;
        private TextCreater _textCreater;
        private LineElementCreater _lineElementCreater;
        private IActiveView _pageLayoutView;

        public ScaleElement(IGraphicsContainer pageLayoutGraphicsContainer,int color,LineElementCreater lineElementCreater)
        {
            _pageLayGraphics = pageLayoutGraphicsContainer;
            Color = color;
            _textCreater = new TextCreater(FontSize);
            _lineElementCreater = lineElementCreater;
            

            var pageLayout = GlobalParams.HookHelper.PageLayout;
            _pageLayoutView = (IActiveView)pageLayout;
        }

        public int Color { get;}

        private IPoint AdjustScalePosition(IPoint pnt)
        {
            var result = new PointClass();
            if (_scaleGrp?.ElementCount > 0)
            {
                var allelements = _scaleGrp as IElement;
                if (allelements != null)
                {
                    var env = allelements.Geometry.Envelope;

                    result.X = env.XMin + 0.6;
                    result.Y = env.YMin + 0.8;
                }
            }
            else
            {
                result.X = pnt.X - 5;
                result.Y = pnt.Y + 5;
            }
            return result;
        }

        private IElement CreateCaptionElement(IPoint pnt)
        {
            var tmpPt = new PointClass() {X = pnt.X, Y = pnt.Y};

            tmpPt.Y -= 0.4;
            tmpPt.X += 0.2;

            var captionElement = _textCreater.CreateHorizontalText("VERTICAL\r\nSCALE\r\nAMSL", tmpPt, 1);
            return captionElement;
        }

        public void AddScalePart(IPoint startPoint,double height,double heightInSm,int stepCount,int stepNumber,string caption,Side side)
        {
            //Left Line
            var leftStartPt = new PointClass();
            leftStartPt.PutCoords(startPoint.X + (int)side * DistFromCenter, startPoint.Y);

            IPoint upperPnt = EsriFunctions.LocalToPrj(leftStartPt, ARANMath.C_PI_2, heightInSm);
            IElement leftLine = _lineElementCreater.CreateLineElement(leftStartPt, upperPnt);
            _scaleGrp.AddElement(leftLine);
            //

            //Right line
            var rightStartPt = new PointClass();
            rightStartPt.PutCoords(startPoint.X+(int)side*(DistFromCenter+WidthOfEachParam),startPoint.Y);

            upperPnt = EsriFunctions.LocalToPrj(rightStartPt, ARANMath.C_PI_2, heightInSm);
            IElement rightLine = _lineElementCreater.CreateLineElement(rightStartPt, upperPnt);
            _scaleGrp.AddElement(rightLine);

            //
            double stepHeight = heightInSm / stepCount;
            for (int i = 1; i <= stepCount; i++)
            {
                leftStartPt.Y +=stepHeight;
                rightStartPt.Y +=stepHeight;
                if (i % stepNumber == 0)
                {
                    var rightStepPt = new PointClass()
                    {
                        X = rightStartPt.X + (int) side * WidthOfEachParam/2,
                        Y = rightStartPt.Y
                    };
                    var text = (i * (height/stepCount)).ToString();
                    var longLineElement = _textCreater.CreateVerticalText(text, rightStepPt, (int)side);
                    _scaleGrp.AddElement(longLineElement);
                    if (i == stepCount)
                    {
                        leftStartPt.Y = leftStartPt.Y + 0.3;
                        leftStartPt.X = leftStartPt.X -(int)side* 0.1;
                        var captionElement = _textCreater.CreateVerticalText(caption, leftStartPt, (int)side);
                        _scaleGrp.AddElement(captionElement);
                        leftStartPt.Y = leftStartPt.Y - 0.3;
                        leftStartPt.X = leftStartPt.X + (int)side *0.1;
                    }
                    _scaleGrp.AddElement(_lineElementCreater.CreateLineElement(leftStartPt, rightStepPt));
                }
                else
                    _scaleGrp.AddElement(_lineElementCreater.CreateLineElement(leftStartPt, rightStartPt));
            }
        }

        public void CreateScale(IPoint pnt, double scale)
        {
           pnt = AdjustScalePosition(pnt);

            DeleteScale();
            _scaleGrp = new GroupElementClass();

            var tmpLeft = EsriFunctions.LocalToPrj(pnt, 0, WidthOfEachParam+DistFromCenter);
            var tmpRight = EsriFunctions.LocalToPrj(pnt, 0, -WidthOfEachParam - DistFromCenter);
            _scaleGrp.AddElement(_lineElementCreater.CreateLineElement(tmpLeft, tmpRight));

            var captionElement = CreateCaptionElement(pnt);
            _scaleGrp.AddElement(captionElement);

            var meterHeight = 100 * 100 / scale;
            AddScalePart(pnt,100,meterHeight,25,5,"METRES",Side.Right);

            double ftHeight = 300 * 30.48/scale;
            AddScalePart(pnt,300,ftHeight,20,5,"FEET",Side.Left);
           
            _pageLayGraphics.AddElement((IElement)_scaleGrp, 1);
            _pageLayoutView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        public void DeleteScale()
        {
            if (_scaleGrp != null)
            {
                IGraphicsContainer pGraphics = GlobalParams.HookHelper.ActiveView.GraphicsContainer;
                pGraphics.DeleteElement(_scaleGrp as IElement);
            }
        }
    }
}
