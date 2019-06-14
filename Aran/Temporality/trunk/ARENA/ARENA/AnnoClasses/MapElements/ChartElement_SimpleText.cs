using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using System.Windows.Forms.Design;
using Accent.MapCore;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
//using Accent.MapCore;

namespace Accent.MapElements
{
    [XmlType]
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ChartElement_SimpleText : AbstractChartElement
    {
        [XmlElement]
        //[Browsable(false)]
        [XmlIgnore]
        private object _geometryAnchor;
        public object GeometryAnchor
        {
            get { return _geometryAnchor; }
            set { _geometryAnchor = value; }
        }

        [XmlElement]
        //[Browsable(false)]
        //[XmlIgnore]
        private horizontalAlignment _horizontalAligment;
        public horizontalAlignment HorizontalAlignment
        {
            get { return _horizontalAligment; }
            set { _horizontalAligment = value; }
        }

        [XmlElement]
        //[Browsable(false)]
        //[XmlIgnore]
        private double _slope;
        public double Slope
        {
            get { return _slope; }
            set { _slope = value; }
        }

        [XmlElement]
        [Browsable(false)]
        //[XmlIgnore]
        private System.Collections.Generic.List<List<AcntChartElementWord>> _textContents;
        [Editor(typeof(MyTextContextEditor), typeof(UITypeEditor))]
        public List<List<AcntChartElementWord>> TextContents
        {
            get { return _textContents; }
            set { _textContents = value; }
        }

        [XmlElement]
        //[Browsable(false)]
        //[XmlIgnore]
        private verticalAlignment _verticalAligment;
        public verticalAlignment VerticalAlignment
        {
            get { return _verticalAligment; }
            set { _verticalAligment = value; }
        }

        [XmlElement]
        //[Browsable(false)]
        //[XmlIgnore]
        private AcntFont _font;
        public AcntFont Font
        {
            get { return _font; }
            set { _font = value; }
        }

        [XmlElement]
        //[Browsable(false)]
        //[XmlIgnore]
        private textPosition _textPosition;
        public textPosition TextPosition
        {
            get { return _textPosition; }
            set { _textPosition = value; }
        }

        [XmlElement]
        //[Browsable(false)]
        //[XmlIgnore]
        private double _leading;
        public double Leading
        {
            get { return _leading; }
            set { _leading = value; }
        }

        [XmlElement]
        //[Browsable(false)]
        //[XmlIgnore]
        private double _characterSpacing;
        public double CharacterSpacing
        {
            get { return _characterSpacing; }
            set { _characterSpacing = value; }
        }

        [XmlElement]
        //[Browsable(false)]
        //[XmlIgnore]
        private double _characterWidth;
        public double CharacterWidth
        {
            get { return _characterWidth; }
            set { _characterWidth = value; }
        }

        [XmlElement]
        //[Browsable(false)]
        //[XmlIgnore]
        private double _wordSpacing;
        public double WordSpacing
        {
            get { return _wordSpacing; }
            set { _wordSpacing = value; }
        }

        [XmlElement]
        //[Browsable(false)]
        //[XmlIgnore]
        private textCase _textCase;
        public textCase TextCase
        {
            get { return _textCase; }
            set { _textCase = value; }
        }

        [XmlElement]
        //[Browsable(false)]
        //[XmlIgnore]
        private AcntColor _haloColor;
        [Editor(typeof(MyColorEdotor), typeof(UITypeEditor))]
        public AcntColor HaloColor
        {
            get { return _haloColor; }
            set { _haloColor = value; }
        }

        [XmlElement]
        //[Browsable(false)]
        //[XmlIgnore]
        private double _haloMaskSize;
        public double HaloMaskSize
        {
            get { return _haloMaskSize; }
            set { _haloMaskSize = value; }
        }

        [XmlElement]
        //[Browsable(false)]
        //[XmlIgnore]
        private AcntPoint _shift;
        public AcntPoint Shift
        {
            get { return _shift; }
            set { _shift = value; }
        }

        [XmlElement]
        //[Browsable(false)]
        //[XmlIgnore]
        private AcntColor _fillColor;
        [XmlElement]
        [Editor(typeof(MyColorEdotor), typeof(UITypeEditor))]
        public AcntColor FillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; }
        }

        [XmlElement]
        //[Browsable(false)]
        //[XmlIgnore]
        private fillStyle _fillStyle;
        //[Editor(typeof(FillStyleEditor), typeof(UITypeEditor))]
        public fillStyle FillStyle
        {
            get { return _fillStyle; }
            set { _fillStyle = value; }
        }



        public ChartElement_SimpleText()
        {


            this.Font = new AcntFont();
            this.GeometryAnchor = null;
            this.GraphicsElement = null;
            this.HorizontalAlignment = horizontalAlignment.Center;
            this.VerticalAlignment = verticalAlignment.Center;
            this.Slope = 0;
            this.TextPosition = textPosition.Normal;
            this.TextCase = textCase.Normal;
            this.Shift = new AcntPoint(0, 0);

            this.HaloColor = new AcntColor(255, 255, 255);
            this.FillColor = new AcntColor();
            this.FillStyle = fillStyle.fSNull;

            /////////////////////////////////////////////////////////////////////////////////////////////
            this.TextContents = new List<List<AcntChartElementWord>>();

            List<AcntChartElementWord> txtLine = new List<AcntChartElementWord>(); // создаем строку
            AcntChartElementWord wrd = new AcntChartElementWord();//создаем слово
            wrd.TextValue = "Value";
            wrd.Font.Bold = true;
            //wrd.StartSymbol = new AcntSymbol("+");
            //wrd.EndSymbol = new AcntSymbol("+");
            //wrd.Morse = true;
            txtLine.Add(wrd);
            this.TextContents.Add(txtLine);  // добавим его в строку

           // txtLine = new List<AcntChartElementWord>();//создаем строку
           // wrd = new AcntChartElementWord();//создаем слово
           // wrd.TextValue = "Value";
           // wrd.Font.Bold = true;
           // wrd.StartSymbol = new AcntSymbol("%");
           // wrd.EndSymbol = new AcntSymbol("%");
           // txtLine.Add(wrd);
           //// this.TextContents.Add(txtLine);  // добавим его в строку

           // //txtLine = new List<AcntChartElementWord>();//создаем строку
           // wrd = new AcntChartElementWord();//создаем слово
           // wrd.TextValue = "Value";
           // wrd.Font.Bold = true;
           // wrd.StartSymbol = new AcntSymbol("%");
           // wrd.EndSymbol = new AcntSymbol("%");
           // txtLine.Add(wrd);
           // this.TextContents.Add(txtLine);  // добавим его в строку

            //////////////////////////////////////////////////////////////////////////////////////////
        }

        public ChartElement_SimpleText(string _text)
        {


            this.Font = new AcntFont();
            this.GeometryAnchor = null;
            this.GraphicsElement = null;
            this.HorizontalAlignment = horizontalAlignment.Center;
            this.VerticalAlignment = verticalAlignment.Center;
            this.Slope = 0;
            this.TextPosition = textPosition.Normal;
            this.TextCase = textCase.Normal;
            this.Shift = new AcntPoint(0, 0);

            this.HaloColor = new AcntColor(255, 255, 255);
            this.FillColor = new AcntColor();
            this.FillStyle = fillStyle.fSNull;

            /////////////////////////////////////////////////////////////////////////////////////////////
            this.TextContents = new List<List<AcntChartElementWord>>();

            List<AcntChartElementWord> txtLine = new List<AcntChartElementWord>(); // создаем строку
            AcntChartElementWord wrd = new AcntChartElementWord();//создаем слово
            wrd.TextValue = _text;
            wrd.Font.Bold = true;
            //wrd.StartSymbol = new AcntSymbol("+");
            //wrd.EndSymbol = new AcntSymbol("+");
            //wrd.Morse = true;
            txtLine.Add(wrd);
            this.TextContents.Add(txtLine);  // добавим его в строку

            // txtLine = new List<AcntChartElementWord>();//создаем строку
            // wrd = new AcntChartElementWord();//создаем слово
            // wrd.TextValue = "Value";
            // wrd.Font.Bold = true;
            // wrd.StartSymbol = new AcntSymbol("%");
            // wrd.EndSymbol = new AcntSymbol("%");
            // txtLine.Add(wrd);
            //// this.TextContents.Add(txtLine);  // добавим его в строку

            // //txtLine = new List<AcntChartElementWord>();//создаем строку
            // wrd = new AcntChartElementWord();//создаем слово
            // wrd.TextValue = "Value";
            // wrd.Font.Bold = true;
            // wrd.StartSymbol = new AcntSymbol("%");
            // wrd.EndSymbol = new AcntSymbol("%");
            // txtLine.Add(wrd);
            // this.TextContents.Add(txtLine);  // добавим его в строку

            //////////////////////////////////////////////////////////////////////////////////////////
        }


        public override object ConvertToIElement()
        {
            try
            {

                //IElement Result = null;
                ITextElement pTextElement = new TextElementClass();

                pTextElement.Text = HelperClass.TextConstructor(this.TextContents);

                TextSymbolClass pTextSymbol = new TextSymbolClass();


                // форматирование текста
                HelperClass.FormatText(ref pTextSymbol, this.TextPosition, this.Leading, this.TextCase, this.HorizontalAlignment, this.VerticalAlignment, this.CharacterSpacing, this.CharacterWidth, this.WordSpacing);
                HelperClass.CreateFont(ref pTextSymbol, this.Font);


                if (this.HaloMaskSize > 0) HelperClass.UseHaloMask(ref pTextSymbol, this.HaloMaskSize, this.HaloColor);


                // смещение относительно точки привязки
                pTextSymbol.XOffset = 0;
                pTextSymbol.YOffset = 0;

                // наклон
                pTextSymbol.Angle = this.Slope;

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

        public override void _OnMouseDown(object sender, IPageLayoutControlEvents_OnMouseDownEvent e, IScreenDisplay Dispaly, ISymbol Symbol, double ReferenceScale)
        {
            IPoint _position = new PointClass();
            _position.PutCoords(e.pageX,e.pageY);


           this._feedback = new NewTextFeedbackClass();
           _feedback.Display = Dispaly;

            NewTextFeedbackClass mvPtFeed = (NewTextFeedbackClass)_feedback;
            //Start the feedback using the input (Point) geometry at the current mouse location
            mvPtFeed.Symbol = Symbol;
            mvPtFeed.Start(_position, ReferenceScale);
        }

        public override void _OnMouseMove(object sender, IPageLayoutControlEvents_OnMouseMoveEvent e)
        {
            base._OnMouseMove(sender, e);
        }

        public override void _OnMouseUp(object sender, IPageLayoutControlEvents_OnMouseUpEvent e)
        {
            base._OnMouseUp(sender, e);
        }



    }

    public class MyTextContextEditor : UITypeEditor
    {

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




                        }


                        if (svc.ShowDialog(frm) == System.Windows.Forms.DialogResult.OK)
                        {


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
