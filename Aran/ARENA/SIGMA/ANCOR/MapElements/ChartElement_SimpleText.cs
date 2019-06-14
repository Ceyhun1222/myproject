using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using System.Windows.Forms.Design;
using ANCOR.MapCore;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using System.IO;
using System.Drawing;
//using Accent.MapCore;

namespace ANCOR.MapElements
{
     [TypeConverter(typeof(PropertySorter))]
    public class ChartElement_SimpleText : AbstractChartElement, ICloneable
    {
        private horizontalAlignment _horizontalAligment;
        [Category("Alligment")]
        [SkipAttribute(false)]
        [PropertyOrder(10)]
        public virtual horizontalAlignment HorizontalAlignment
        {
            get { return _horizontalAligment; }
            set { _horizontalAligment = value; }
        }
        
        private verticalAlignment _verticalAligment;
        [Category("Alligment")]
        [SkipAttribute(false)]
        [PropertyOrder(20)]
        public virtual verticalAlignment VerticalAlignment
        {
            get { return _verticalAligment; }
            set { _verticalAligment = value; }
        }
        
        private textPosition _textPosition;
        [Category("Alligment")]
        [SkipAttribute(false)]
        [PropertyOrder(30)]
        public textPosition TextPosition
        {
            get { return _textPosition; }
            set { _textPosition = value; }
        }

        private AncorPoint _anchor;
        // [Browsable(false)]
        [Category("Alligment")]
        [DisplayName("Offset")]
        [SkipAttribute(false)]
        [PropertyOrder(40)]
        public virtual AncorPoint Anchor
        {
            get { return _anchor; }
            set { _anchor = value; }
        }
        
        private double _slope;
        [Category("Decoration")]
        //[Browsable(false)]
        [SkipAttribute(true)]
        [PropertyOrder(50)]
        public virtual double Slope
        {
            get { return _slope; }
            set { _slope = value; }
        }

        private AncorFont _font;
        [Category("Decoration")]
        //[Browsable(false)]
        [SkipAttribute(false)]
        [PropertyOrder(60)]
        [DisplayName("Font pattern setter")]
        public virtual AncorFont Font
        {
            get { return _font; }
            set 
            { 
                _font = value;
            }
        }

        private double _leading;
        [Category("Decoration")]
        [SkipAttribute(false)]
        [PropertyOrder(70)]
        //[Browsable(false)]
        public double Leading
        {
            get { return _leading; }
            set { _leading = value; }
        }

        private AncorColor _haloColor;
        [Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
        [Category("Decoration")]
        [SkipAttribute(false)]
        [PropertyOrder(80)]
        public virtual AncorColor HaloColor
        {
            get { return _haloColor; }
            set { _haloColor = value; }
        }


        private double _haloMaskSize;
        [Category("Decoration")]
        [SkipAttribute(false)]
        [PropertyOrder(90)]
        public virtual double HaloMaskSize
        {
            get { return _haloMaskSize; }
            set { _haloMaskSize = value; }
        }


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

        private fillStyle _fillStyle;
        [Editor(typeof(FillStyleEditor), typeof(UITypeEditor))]
        [Category("Decoration")]
        //[Browsable(false)]
        [SkipAttribute(false)]
        [PropertyOrder(110)]
        public virtual fillStyle FillStyle
        {
            get { return _fillStyle; }
            set { _fillStyle = value; }
        }


        private System.Collections.Generic.List<List<AncorChartElementWord>> _textContents;
        [Editor(typeof(MyTextContextEditor), typeof(UITypeEditor))]
        //[Browsable(false)]
        [Category("Text")]
        [SkipAttribute(true)]
        [PropertyOrder(120)]
        public virtual List<List<AncorChartElementWord>> TextContents
        {
            get { return _textContents; }
            set { _textContents = value; }
        }

      
        private double _characterSpacing;
        [Category("Text")]
        [SkipAttribute(false)]
        [PropertyOrder(130)]
        //[Browsable(false)]
        public virtual double CharacterSpacing
        {
            get { return _characterSpacing; }
            set { _characterSpacing = value; }
        }

        
        private double _characterWidth;
        [Category("Text")]
        [SkipAttribute(false)]
        [PropertyOrder(140)]
        [Browsable(false)]
        public virtual double CharacterWidth
        {
            get { return _characterWidth; }
            set { _characterWidth = value; }
        }

        
        private double _wordSpacing;
        [Category("Text")]
        [SkipAttribute(false)]
        [PropertyOrder(150)]
        //[Browsable(false)]
        public virtual double WordSpacing
        {
            get { return _wordSpacing; }
            set { _wordSpacing = value; }
        }

        
        private textCase _textCase;
        [Category("Text")]
        [SkipAttribute(false)]
        [PropertyOrder(160)]
        public textCase TextCase
        {
            get { return _textCase; }
            set { _textCase = value; }
        }


        private coordtype _CoordType;
        [Browsable(false)]
        public coordtype CoordType
        {
            get { return _CoordType; }
            set { _CoordType = value; }
        }
    
        public ChartElement_SimpleText()
        {
            
        }

        
        public ChartElement_SimpleText(string _txt)
        {

            this.Font = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "Courier New", Size = 8, UnderLine = false };
            //this.GeometryAnchor = null;
            //this.GraphicsElement = null;
            this.HorizontalAlignment = horizontalAlignment.Center;
            this.VerticalAlignment = verticalAlignment.Center;
            this.Slope = 0;
            this.TextPosition = textPosition.Normal;
            this.TextCase = textCase.Normal;
            this.Anchor = new AncorPoint(0, 0);
            this.WordSpacing = 100;
            this.HaloColor = new AncorColor(255, 255, 255);
            this.FillColor = new AncorColor (255,255,255);
            this.FillStyle = fillStyle.fSNull;
            this.CoordType = coordtype.DDMMSSN_1;

            /////////////////////////////////////////////////////////////////////////////////////////////
            this.TextContents = new List<List<AncorChartElementWord>>();

            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord(_txt,this.Font);//создаем слово
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку

            this.TextContents.Add(txtLine);

            //////////////////////////////////////////////////////////////////////////////////////////
        }

        public ChartElement_SimpleText(string _txt, AncorFont Fnt)
        {
            this.Font = new AncorFont { Bold = Fnt.Bold, FontColor = new AncorColor(0, 0, 0), Italic = Fnt.Italic, Name = Fnt.Name, Size = Fnt.Size, UnderLine = Fnt.UnderLine };
            //this.GeometryAnchor = null;
            //this.GraphicsElement = null;
            this.HorizontalAlignment = horizontalAlignment.Center;
            this.VerticalAlignment = verticalAlignment.Center;
            this.Slope = 0;
            this.TextPosition = textPosition.Normal;
            this.TextCase = textCase.Normal;
            this.Anchor = new AncorPoint(0, 0);
            this.WordSpacing = 100;
            this.HaloColor = new AncorColor(255, 255, 255);
            this.FillColor = new AncorColor (255,255,255);
            this.FillStyle = fillStyle.fSNull;
            this.CoordType = coordtype.DDMMSSN_1;

            /////////////////////////////////////////////////////////////////////////////////////////////
            this.TextContents = new List<List<AncorChartElementWord>>();

            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord(_txt, this.Font);//создаем слово
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку

            this.TextContents.Add(txtLine);
            
            
            //////////////////////////////////////////////////////////////////////////////////////////
        }

       

        public string GetTextContensAsString()
        {
            string res = "";
            if (this.TextContents != null)
            {
                foreach (var ln in this.TextContents)
                {
                    foreach (var wrd in ln)
                    {
                        res = res + wrd.ToString();
                    }
                }
            }
            return res;
        }

        public override object ConvertToIElement()
        {
            try
            {

                //IElement Result = null;
                ITextElement pTextElement = new TextElementClass();

                pTextElement.Text = HelperClass.TextConstructor(this.TextContents);
                if (pTextElement.Text.Length <= 0) { pTextElement.Text = HelperClass.TextConstructor(this.TextContents, IgnorVisibilityFlag: true); this.Placed = false; };
                pTextElement.Text = pTextElement.Text.TrimEnd();

                TextSymbolClass pTextSymbol = new TextSymbolClass();


                // форматирование текста
                HelperClass.FormatText(ref pTextSymbol, this.TextPosition, this.Leading, this.TextCase, this.HorizontalAlignment, this.VerticalAlignment, this.CharacterSpacing, this.CharacterWidth, this.WordSpacing);
                HelperClass.CreateFont(ref pTextSymbol, this.Font);


                if (this.HaloMaskSize > 0) HelperClass.UseHaloMask(ref pTextSymbol, this.HaloMaskSize, this.HaloColor);


                // смещение относительно точки привязки
                pTextSymbol.XOffset = this.Anchor.X;
                pTextSymbol.YOffset = this.Anchor.Y;

                // наклон
                pTextSymbol.Angle = this.Slope;

                /////////////////////////
                if (this.FillStyle != fillStyle.fSNull)
                {
                    AncorFrame brdr = new AncorFrame(this.FillColor, new AncorFrameMargins(0, 0, 0, 0), 0, 0.001, lineStyle.lsNull);
                    LineCallout pCallout = HelperClass.GetSimpleBorder(this.FillColor, this.FillStyle, brdr);
                    pTextSymbol.Background = (ITextBackground)pCallout;
                }
                ///////////////////////////


                pTextElement.Symbol = pTextSymbol;


                 return pTextElement;


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public override object ChartElementChanged()
        {
            return this.ConvertToIElement();
        }


        public override object Clone()
        {
            ChartElement_SimpleText chartEl = (ChartElement_SimpleText)base.Clone();
            chartEl.Anchor = (AncorPoint)chartEl.Anchor.Clone(); 
            chartEl.FillColor = (AncorColor)chartEl.FillColor.Clone();
            chartEl.Font = (AncorFont)chartEl.Font.Clone();

            chartEl.TextContents = CloneTextContenst(chartEl.TextContents);

            return chartEl;
        }

        public List<List<AncorChartElementWord>> CloneTextContenst(List<List<AncorChartElementWord>> txtContext)
        {
            List<List<AncorChartElementWord>> res = new List<List<AncorChartElementWord>>();
            List<List<AncorChartElementWord>> baseTxtContext = new List<List<AncorChartElementWord>>();
            baseTxtContext = txtContext;

            for (int i = 0; i < baseTxtContext.Count; i++)
            {
                List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>();

                for (int j = 0; j < baseTxtContext[i].Count; j++)
                {
                    txtLine.Add((AncorChartElementWord)baseTxtContext[i][j].Clone());

                }

                res.Add(txtLine);
            }

            return res;

        }

        public override IDisplayFeedback GetFeedback()
        {
            ChartElement_SimpleText symbol = (ChartElement_SimpleText)this.Clone();
            IElement iEl = (IElement)symbol.ConvertToIElement();
            ITextSymbol txtS = null;

            if (iEl is IGroupElement)
            {
                txtS = ((ITextElement)((IGroupElement)iEl).get_Element(1)).Symbol;
            }
            else
            {
                txtS = ((ITextElement)iEl).Symbol;
            }

          
            IDisplayFeedback _feedback = new NewTextFeedbackClass();
            NewTextFeedbackClass mvPtFeed = (NewTextFeedbackClass)_feedback;
            mvPtFeed.Symbol =(ISymbol)txtS;

            return mvPtFeed;

        }

        public override void StartFeedback(IDisplayFeedback feedBack, IPoint _position, double scale, IGeometry LinkedGeometry)
        {
            ((NewTextFeedbackClass)feedBack).Start(_position, scale);
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
                    if (this.Name.StartsWith("CircleDistace"))
                        res = SlideAlongLine(res, LinkedGeometry);
                    else
                        res.PutCoords(X, Y);
                    break;
                default:
                    res.PutCoords(X, Y);
                    break;
            }

            if (Shift == 3) res.PutCoords(X, Y);
            if (res.IsEmpty) res.PutCoords(X, Y);
            ((NewTextFeedbackClass)feedBack).Stop();

            return res;
        }

        public override void MoveFeedback(IDisplayFeedback feedBack, IPoint _position, IGeometry LinkedGeometry, int Shift)
        {
            switch (LinkedGeometry.GeometryType)
            {

                case esriGeometryType.esriGeometryLine:
                case esriGeometryType.esriGeometryPolyline:
                    if (Shift !=3) feedBack.MoveTo( SlideAlongLine(_position, LinkedGeometry));
                    break;
                case esriGeometryType.esriGeometryPoint:
                case esriGeometryType.esriGeometryPolygon:
                    if (this.Name.StartsWith("CircleDistace"))
                        feedBack.MoveTo(SlideAlongLine(_position, LinkedGeometry));
                    else
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


    public class MyTextContextEditor : UITypeEditor
    {
        public static Action ObjectTextContensChanged;

        public override System.Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, System.Object value)
        {
            if ((context != null) && (provider != null))
            {

                IWindowsFormsEditorService svc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (svc != null)
                {
                    
                    using (TextContestEditorForm frm = new TextContestEditorForm())
                    {
                        System.Object OldVal = new System.Object();
                        OldVal = value;

                        if (context.Instance is ChartElement_SimpleText)
                        {
                            #region InnerTextContents
                            if ((context.Instance as ChartElement_SimpleText).TextContents != null)
                            {
                                if ((context.Instance as ChartElement_SimpleText).TextContents.Count > 0)
                                {
                                    frm.TextContents = (context.Instance as ChartElement_SimpleText).TextContents;

                                    frm.Objectname = context.Instance.ToString();
                                    frm.relFeaturename = (context.Instance as ChartElement_SimpleText).RelatedFeature;
                                    frm.ObjectID = (context.Instance as ChartElement_SimpleText).Id.ToString();
                                    frm.ResStr = "";
                                }
                            }
                            #endregion

                            if (context.Instance is ChartElement_BorderedText_Collout_CaptionBottom)
                            {
                                #region CaptionTextLine
                                if ((context.Instance as ChartElement_BorderedText_Collout_CaptionBottom).CaptionTextLine !=null )
                                {
                                    if ((context.Instance as ChartElement_BorderedText_Collout_CaptionBottom).CaptionTextLine.Count > 0)
                                    {
                                        frm.CaptionTextContest = (context.Instance as ChartElement_BorderedText_Collout_CaptionBottom).CaptionTextLine;
                                    }
                                }
                                #endregion

                                #region BottomTextLine
                                if ((context.Instance as ChartElement_BorderedText_Collout_CaptionBottom).BottomTextLine != null)
                                {
                                    if ((context.Instance as ChartElement_BorderedText_Collout_CaptionBottom).BottomTextLine.Count > 0)
                                    {
                                        frm.BottomTextContest = (context.Instance as ChartElement_BorderedText_Collout_CaptionBottom).BottomTextLine;
                                    }
                                }
                                #endregion
                            }

                            if (context.Instance is ChartElement_RouteDesignator)
                            {
                                #region BottomTextLine
                                if ((context.Instance as ChartElement_RouteDesignator).RouteDesignatorSource != null)
                                {
                                    if ((context.Instance as ChartElement_RouteDesignator).RouteDesignatorSource.Count > 0)
                                    {
                                        frm.BottomTextContest = (context.Instance as ChartElement_RouteDesignator).RouteDesignatorSource;
                                    }
                                }
                                #endregion
                            }


                        }
                        else if (context.Instance is GraphicsChartElement_SafeArea)
                        {
                            if ((context.Instance as GraphicsChartElement_SafeArea).TextContents != null)
                            {
                                if ((context.Instance as GraphicsChartElement_SafeArea).TextContents.Count > 0)
                                {
                                    frm.TextContents = (context.Instance as GraphicsChartElement_SafeArea).TextContents;
                                }
                            }
                        }
                        else if (context.Instance is GraphicsChartElement_NorthArrow)
                        {
                            if ((context.Instance as GraphicsChartElement_NorthArrow).TextContents != null)
                            {
                                if ((context.Instance as GraphicsChartElement_NorthArrow).TextContents.Count > 0)
                                {
                                    frm.TextContents = (context.Instance as GraphicsChartElement_NorthArrow).TextContents;
                                }
                            }
                        }


                        if (svc.ShowDialog(frm) == System.Windows.Forms.DialogResult.OK)
                        {
                            if (frm.ResStr!=null && frm.ResStr.Length > 0)
                            {
                                (context.Instance as AbstractChartElement).LogTxt = frm.ResStr;
                                ObjectTextContensChanged();
                            }
                        }
                        else
                        {
                            value = OldVal;
                        }

                    }


                }
            }


            return base.EditValue(context, provider, value);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null)
                return UITypeEditorEditStyle.Modal;
            else
                return base.GetEditStyle(context);
        }



    }


}
