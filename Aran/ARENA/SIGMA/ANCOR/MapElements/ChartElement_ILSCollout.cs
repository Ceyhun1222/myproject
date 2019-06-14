using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using ANCOR.MapCore;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using GeometryFunctions;
using ESRI.ArcGIS.esriSystem;
using SigmaCallout;


namespace ANCOR.MapElements
{
    public class ChartElement_ILSCollout : AbstractChartElement, ICloneable
    {
        [Category("Decoration")]
        [SkipAttribute(false)]
        [PropertyOrder(10)]
        public SigmaIlsStyle ILSStyle { get; set; }

        [Category("Decoration")]
        [SkipAttribute(false)]
        [PropertyOrder(15)]
        public SigmaIlsAnchorPoint IlsAnchorPoint { get; set; }

        [Browsable(false)]
        public double locX { get; set; }
        [Browsable(false)]
        public double locY { get; set; }
        [Browsable(false)]
        public double gpX { get; set; }
        [Browsable(false)]
        public double gpY { get; set; }
        [Browsable(false)]
        public double DistToLOC { get; set; }
        [Browsable(false)]
        public double DistToGP { get; set; }

        [Category("Decoration")]
        [SkipAttribute(false)]
        [PropertyOrder(20)]
        [ReadOnly(true)]
        public double Slope { get; set; }

        [Category("Decoration")]
        [SkipAttribute(false)]
        [PropertyOrder(30)]
        public int Length { get; set; }

        [Category("Decoration")]
        [SkipAttribute(false)]
        [PropertyOrder(40)]
        public double Width{ get; set; }

        [Category("Decoration")]
        [SkipAttribute(false)]
        [PropertyOrder(50)]
        public double Depth{ get; set; }

        //private AncorColor _haloColor;
        //[Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
        //[Category("Decoration")]
        //[SkipAttribute(false)]
        //[PropertyOrder(80)]
        //public virtual AncorColor HaloColor
        //{
        //    get { return _haloColor; }
        //    set { _haloColor = value; }
        //}


        //private double _haloMaskSize;
        //[Category("Decoration")]
        //[SkipAttribute(false)]
        //[PropertyOrder(90)]
        //public virtual double HaloMaskSize
        //{
        //    get { return _haloMaskSize; }
        //    set { _haloMaskSize = value; }
        //}


        private AncorColor _fillColor;
        [Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
        [Category("Decoration")]
        //[Browsable(false)]
        [SkipAttribute(false)]
        [PropertyOrder(100)]
        public virtual AncorColor FillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; }
        }

        //private fillStyle _fillStyle;
        //[Editor(typeof(FillStyleEditor), typeof(UITypeEditor))]
        //[Category("Decoration")]
        ////[Browsable(false)]
        //[SkipAttribute(false)]
        //[PropertyOrder(110)]
        //public virtual fillStyle FillStyle
        //{
        //    get { return _fillStyle; }
        //    set { _fillStyle = value; }
        //}

        [Browsable(false)]
        public AncorPoint Anchor { get; set; }

        public ChartElement_ILSCollout()
        {

        }

        public override object ConvertToIElement()
        {

            try
            {
                ITextElement pTextElement = new TextElementClass();


                pTextElement.Text = "ilsglidepath";


                TextSymbolClass pTextSymbol = new TextSymbolClass();

                //if (this.HaloMaskSize > 0) HelperClass.UseHaloMask(ref pTextSymbol, this.HaloMaskSize, this.HaloColor);


                // смещение относительно точки привязки
                pTextSymbol.XOffset = 0;
                pTextSymbol.YOffset = 0;
                pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                // наклон
                pTextSymbol.Angle = this.Slope;


                SigmaCallout.PolygonCallout pCallout = new SigmaCallout.PolygonCallout();

                pCallout.Version = 1;
                pCallout.Length = this.Length;
                pCallout.Width = this.Width;
                pCallout.Depth = this.Depth;
                pCallout.ILSStyle = (int)this.ILSStyle;
                pCallout.Slope = this.Slope;

                pCallout.BackColor = this.FillColor.GetColor();

                //if (this.FillStyle == fillStyle.fSBackwardDiagonal) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSBackwardDiagonal;
                //if (this.FillStyle == fillStyle.fSCross) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSCross;
                //if (this.FillStyle == fillStyle.fSDiagonalCross) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSDiagonalCross;
                //if (this.FillStyle == fillStyle.fSForwardDiagonal) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSForwardDiagonal;
                //if (this.FillStyle == fillStyle.fSHollow) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSHollow;
                //if (this.FillStyle == fillStyle.fSHorizontal) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSHorizontal;
                //if (this.FillStyle == fillStyle.fSNull) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSNull;
                //if (this.FillStyle == fillStyle.fSSolid) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSSolid;
                //if (this.FillStyle == fillStyle.fSVertical) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSVertical;


                pCallout.LineSymbol = new SimpleLineSymbol();
                pCallout.LineSymbol.Width = 1;
                //IRgbColor lineClr = new RgbColorClass(); // создание рамки
                //lineClr.Red = this.Frame.FrameColor.Red;
                //lineClr.Green = this.Frame.FrameColor.Green;
                //lineClr.Blue = this.Frame.FrameColor.Blue;


                //pCallout.DrawLeader = this.LeaderLine.LeaderLineWidth > 0;
                //pCallout.LineSymbol.Width = this.Frame.Thickness;
                ////////////////////////////////////////////

                IPoint pnt = new PointClass();
                pnt.PutCoords(this.Anchor.X, this.Anchor.Y);
                pCallout.AnchorPoint = pnt;

                ///////////////////////////////////////////

                pTextSymbol.Background = (ITextBackground)pCallout;


                pTextElement.Symbol = pTextSymbol;


                return pTextElement;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }


        }

        public override object Clone()
        {
            return base.Clone();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override IGeometry StopFeedback(IDisplayFeedback feedBack, int X, int Y, IGeometry LinkedGeometry, int Shift)
        {
            IPoint res = new PointClass();
            res.PutCoords(X, Y);


            switch (LinkedGeometry.GeometryType)
            {

                case esriGeometryType.esriGeometryLine:
                case esriGeometryType.esriGeometryPolyline:

                    if (Shift != 3) res = SlideAlongLine(res, LinkedGeometry);

                    break;
                case esriGeometryType.esriGeometryPoint:
                case esriGeometryType.esriGeometryPolygon:
                    res.PutCoords(X, Y);
                    break;
                default:
                    res.PutCoords(X, Y);
                    break;
            }

            if (Shift == 3) res.PutCoords(X, Y);

            ((NewTextFeedbackClass)feedBack).Stop();

            return res;
        }


        public override IDisplayFeedback GetFeedback()
        {
            ChartElement_ILSCollout symbol = (ChartElement_ILSCollout)this.Clone();
            IElement iEl = (IElement)symbol.ConvertToIElement();
            ITextSymbol txtS = null;


            txtS = ((ITextElement)iEl).Symbol;
            

            IDisplayFeedback _feedback = new NewTextFeedbackClass();
            NewTextFeedbackClass mvPtFeed = (NewTextFeedbackClass)_feedback;
            mvPtFeed.Symbol = (ISymbol)txtS;

            return mvPtFeed;

        }

        public override void StartFeedback(IDisplayFeedback feedBack, IPoint _position, double scale, IGeometry LinkedGeometry)
        {
            ((NewTextFeedbackClass)feedBack).Start(_position, scale);
        }

        public override void MoveFeedback(IDisplayFeedback feedBack, IPoint _position, IGeometry LinkedGeometry, int Shift)
        {
            switch (LinkedGeometry.GeometryType)
            {

                case esriGeometryType.esriGeometryLine:
                case esriGeometryType.esriGeometryPolyline:
                    if (Shift != 3) feedBack.MoveTo(SlideAlongLine(_position, LinkedGeometry));
                    break;
                case esriGeometryType.esriGeometryPoint:
                case esriGeometryType.esriGeometryPolygon:
                    feedBack.MoveTo(_position);
                    break;
                default:
                    feedBack.MoveTo(_position);
                    break;
            }

            if (Shift == 3) feedBack.MoveTo(_position);

        }

        private IPoint SlideAlongLine(IPoint _position, IGeometry LinkedGeometry)
        {
            IProximityOperator pProximity = LinkedGeometry as IProximityOperator;
            IPoint pNearestPt = pProximity.ReturnNearestPoint(_position, esriSegmentExtension.esriNoExtension);

            return pNearestPt;


        }

    }
}
