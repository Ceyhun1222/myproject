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
    public class ChartElement_SigmaCollout : ChartElement_BorderedText_Collout_CaptionBottom
    {

        // 0 = ChartElement_SigmaCollout, 
        // 1 = ChartElement_SigmaCollout_Navaid, 
        // 2 = ChartElement_SigmaCollout_Airspace, 
        // 3 = ChartElement_SigmaCollout_Designatedpoin, 
        // 4 = ChartElement_SigmaCollout_AccentBar
        [Browsable(false)]
        public int ColloutType { get; set; } 

        private SigmaCallout_Morse _morseTextLine;
        [SkipAttribute(false)]
        [Category("Decoration")]
        public virtual SigmaCallout_Morse MorseTextLine
        {
            get { return _morseTextLine; }
            set { _morseTextLine = value; }
        }

        private SigmaCallout_AirspaceSign _AirspaceSign;
        [SkipAttribute(false)]
        [Category("Decoration")]
        public virtual SigmaCallout_AirspaceSign AirspaceSign
        {
            get { return _AirspaceSign; }
            set { _AirspaceSign = value; }
        }


        private double _LeaderTolerance;
        [SkipAttribute(false)]
        //[Browsable(false)]
        [Category("Decoration")]
        [DisplayName("Gap")]
        public double LeaderTolerance
        {
            get { return _LeaderTolerance; }
            set { _LeaderTolerance = value; }
        }

        private sigmaCalloutLeaderLineSnap _Snap;
        [SkipAttribute(false)]
        [Category("Decoration")]
        public sigmaCalloutLeaderLineSnap Snap
        {
            get { return _Snap; }
            set { _Snap = value; }
        }

        private bool _hasFooter;
        [SkipAttribute(false)]
        [Browsable(false)]
        public bool HasFooter
        {
            get { return _hasFooter; }
            set { _hasFooter = value; }
        }

        private bool _hasHeader;
        [SkipAttribute(false)]
        [Browsable(false)]
        public bool HasHeader
        {
            get { return _hasHeader; }
            set { _hasHeader = value; }
        }

        private SigmaCallout_Frame _Frame;
        [SkipAttribute(false)]
        [Category("Decoration")]
        public virtual SigmaCallout_Frame Frame
        {
            get { return _Frame; }
            set { _Frame = value; }
        }

        private SigmaCallout_Shadow _elementShadow;
        [SkipAttribute(false)]
        [Category("Decoration")]
        [DisplayName("Shadow")]
        public  virtual SigmaCallout_Shadow ElementShadow
        {
            get { return _elementShadow; }
            set { _elementShadow = value; }
        }

        [Browsable(false)]
        public override AncorFrame Border
        {
            get
            {
                return base.Border;
            }
            set
            {
                base.Border = value;
            }
        }

        [Browsable(false)]
        public override AncorShadow Shadow
        {
            get
            {
                return base.Shadow;
            }
            set
            {
                base.Shadow = value;
            }
        }

        [Browsable(false)]
        public override AncorLeaderLine LeaderLine
        {
            get
            {
                return base.LeaderLine;
            }
            set
            {
                base.LeaderLine = value;
            }
        }

        [Browsable(false)]
        public override AncorColor HaloColor
        {
            get
            {
                return base.HaloColor;
            }
            set
            {
                base.HaloColor = value;
            }
        }

        [Browsable(false)]
        public override double HaloMaskSize
        {
            get
            {
                return base.HaloMaskSize;
            }
            set
            {
                base.HaloMaskSize = value;
            }
        }

        public ChartElement_SigmaCollout()
        {
        }

        public ChartElement_SigmaCollout(string CaptionText)
        {
            #region Text formating

            this.ColloutType = 0;
            this.Font = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "Arial", Size = 8, UnderLine = false };
            this.FillColor = new AncorColor (255,255,255);
            //this.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
            this.HaloColor = new AncorColor(255, 255, 255);
            this.HorizontalAlignment = horizontalAlignment.Center;
            this.VerticalAlignment = verticalAlignment.Center;
            this.TextPosition = textPosition.Normal;
            this.TextCase = textCase.Normal;
            this.WordSpacing = 50;
            ///////////////////////////////////////////////////////////////////////////////////////////
            this.CaptionTextLine = new List<List<AncorChartElementWord>>(); // создаем заголовок

            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord(CaptionText, this.Font);//создаем слово
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("+");
            wrd.EndSymbol = new AncorSymbol("+");
            wrd.Morse = false;
            txtLine.Add(wrd);
            this.CaptionTextLine.Add(txtLine);  // добавим его в заголовок
            ////////////////////////////////////////////////////////////////////////////////////////////

            this.TextContents = new List<List<AncorChartElementWord>>();

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("VOR/DME ", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.WordSpacing = 50;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine); // добавим строку


            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("116.9", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            wrd.Morse = false;
            txtLine.Add(wrd); // добавим его в строку

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("BOR", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            //wrd.WordSpacing = 300;

            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            txtLine.Add(wrd); // добавим его в строку


            this.TextContents.Add(txtLine);  // добавим строку


            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("52°22'06 N", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;

            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            wrd.Morse = false;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);  // добавим строку

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("036°34'45 W", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            wrd.Morse = false;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);  // добавим строку

            ///////////////////////////////////////////////////////////////////////////////////////////

            this.BottomTextLine = new List<List<AncorChartElementWord>>(); // создаем нижнюю строку

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("30 ", this.Font);//создаем слово
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.WordSpacing = 50;
            //wrd.WordSpacing = 30;
            txtLine.Add(wrd);

            wrd = new AncorChartElementWord("M", this.Font);//создаем слово
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            wrd.Morse = false;
            //wrd.WordSpacing = 50;
            txtLine.Add(wrd);

            this.BottomTextLine.Add(txtLine);  // добавим его в нижнюю строку

            ////////////////////////////////////////////////////////////////////////////////////////

            #endregion

            this.Border = new AncorFrame(lineStyle.lsSolid);
            this.Border.FrameMargins = new AncorFrameMargins(0, 0, 0, 0);

            //sigmaCallout properties
            this.AirspaceSign = new SigmaCallout_AirspaceSign { AirspaceSymbols = "A,C", AirspaceOnLeftSide = true, AirspaceBackColor = new AncorColor(255, 0, 0), AirspaceSignFont = new AncorFont(true, new AncorColor(255, 255, 255), false, "Arial", 8, false), AirspaceSignScaleToFit = true, AirspaceSignSize = 12 };
            this.Frame = new SigmaCallout_Frame { FrameLineStyle = lineStyle.lsSolid, Offset = 0, Thickness = 1, FrameColor = new AncorColor(255, 0, 0), FrameMargins = new SigmaCallout_FrameMargins(0, 0, 0, 0) };
            //this.Snap = 0;
            this.ElementShadow = new SigmaCallout_Shadow { ShadowSize = 0, ShadowCoLor = new AncorColor(255, 0, 0) };
            this.LeaderTolerance = 0;

            this.HasHeader = true;
            this.HasFooter = true;
            //=================Morse
            this.MorseTextLine = new SigmaCallout_Morse { MorseColor = new AncorColor(255, 0, 0), MorseLeading = -10, MorseText = "BOR", MorseSize = 8, MorseLocationShiftOnX = 0, MorseLocationShiftOnY = 0 };
            //==================================================================End Morse
            this.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
        }

        [Browsable(false)]
        public virtual sigmaCalloutAccentbarPosition AccentBarPosition { get; set; }

        public override object ConvertToIElement()
        {
            try
            {
                ITextElement pTextElement = new TextElementClass();

                ////формирование внутреннего текста
                pTextElement.Text = this.BottomTextLine != null ? HelperClass.TextConstructor(this.CaptionTextLine, true) + " " + HelperClass.TextConstructor(this.TextContents) + " " + HelperClass.TextConstructor(this.BottomTextLine, true) :
                                                                HelperClass.TextConstructor(this.CaptionTextLine, true) + " " + HelperClass.TextConstructor(this.TextContents);

                pTextElement.Text = pTextElement.Text.TrimEnd();

                //pTextElement.Text = pTextElement.Text.EndsWith("  ") ? pTextElement.Text.Remove(pTextElement.Text.Length - 2, 2) : pTextElement.Text;

                #region Morse

                if (this.MorseTextLine != null)
                {
                    bool flag = false;
                    List<AncorChartElementWord> morseLn = null;

                    int counter = 0;
                    foreach (var ln in this.TextContents)
                    {
                        foreach (var wrd in ln)
                        {
                            if (wrd.TextValue.CompareTo(this.MorseTextLine.MorseText) == 0 && wrd.Font.Name.CompareTo("Morse") != 0)
                            {
                                morseLn = ln;
                                flag = true;
                                break;
                            }

                        }

                        if (flag) break;

                        counter++;

                    }

                    if (flag && morseLn != null)
                    {
                        morseLn.RemoveRange(1, morseLn.Count - 1);

                    }



                    if (this.MorseTextLine.VerticalMorse)
                    {
                        pTextElement.Text = this.BottomTextLine != null ? HelperClass.TextConstructor(this.CaptionTextLine, true) + " " + HelperClass.TextConstructor(this.TextContents) + " " + HelperClass.TextConstructor(this.BottomTextLine, true) :
                                           HelperClass.TextConstructor(this.CaptionTextLine, true) + " " + HelperClass.TextConstructor(this.TextContents);

                        pTextElement.Text = pTextElement.Text.Remove(pTextElement.Text.Length - 2, 2);
                        pTextElement.Text = pTextElement.Text.Trim();

                        pTextElement.Text = pTextElement.Text + "\r" + "\n" + HelperClass.MorseTextConstructor(this.MorseTextLine);
                    }
                    else
                    {
                        AncorChartElementWord wrdMorse = new AncorChartElementWord(this.MorseTextLine.MorseText, this.Font);
                        wrdMorse.Font.Name = "Morse";
                        wrdMorse.Font.FontColor = new AncorColor(this.MorseTextLine.MorseColor.Red, this.MorseTextLine.MorseColor.Green, this.MorseTextLine.MorseColor.Blue);
                        wrdMorse.Font.Bold = true;


                        if (flag && morseLn != null) morseLn.Add(wrdMorse); 
                        //{
                        //    List<AncorChartElementWord> nL = new List<AncorChartElementWord>();
                        //    nL.Add(wrdMorse);
                        //    this.TextContents.Insert(counter +1, nL);
                        //}
                        pTextElement.Text = this.BottomTextLine != null ? HelperClass.TextConstructor(this.CaptionTextLine, true) + " " + HelperClass.TextConstructor(this.TextContents) + " " + HelperClass.TextConstructor(this.BottomTextLine, true) :
                                                               HelperClass.TextConstructor(this.CaptionTextLine, true) + " " + HelperClass.TextConstructor(this.TextContents);

                        pTextElement.Text = pTextElement.Text.Remove(pTextElement.Text.Length - 2, 2);
                        pTextElement.Text = pTextElement.Text.Trim();

                    }
                }

                #endregion



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


                SigmaCallout.SigmaCallout pCallout = new SigmaCallout.SigmaCallout();

                pCallout.HasFooter = this.HasFooter;
                pCallout.HasHeader = this.HasHeader;
                pCallout.Version = this.ColloutType;

                if (this.MorseTextLine != null)
                {
                    pCallout.MorseLeading = this.MorseTextLine.MorseLeading;
                    pCallout.MorseLocationShiftOnX = this.MorseTextLine.MorseLocationShiftOnX;
                    pCallout.MorseLocationShiftOnY = this.MorseTextLine.MorseLocationShiftOnY;
                    pCallout.verticalMorse = this.MorseTextLine.VerticalMorse;
                }

                pCallout.TopMargin = this.Frame.FrameMargins.TopMargin;
                pCallout.HeaderHorizontalMargin = this.Frame.FrameMargins.HeaderHorizontalMargin;
                pCallout.FooterHorizontalMargin = this.Frame.FrameMargins.FooterHorizontalMargin;
                pCallout.BottomMargin = this.Frame.FrameMargins.BottomMargin;
                pCallout.Width = this.Frame.Offset;
                pCallout.Snap = (int)this.Snap;
                pCallout.LeaderTolerance = this.LeaderTolerance;
                pCallout.DrawLeader = this.Frame.DrawLeader;

                pCallout.AccentBarPosition = (int)this.AccentBarPosition;

                if (this.ElementShadow != null)
                {
                    pCallout.Shadow = this.ElementShadow.ShadowSize;
                    pCallout.ShadowBackColor = this.ElementShadow.ShadowCoLor.GetColor();
                }

                if (AirspaceSign != null)
                {
                    pCallout.AirspaceSymbols = this.AirspaceSign.AirspaceSymbols;
                    pCallout.AirspaceOnLeftSide = this.AirspaceSign.AirspaceOnLeftSide;

                    pCallout.AirspaceBackColor = this.AirspaceSign.AirspaceBackColor.GetColor();

                    pCallout.AirspaceFontSize = this.AirspaceSign.AirspaceSignFont.Size;
                    pCallout.AirsapceFontName = this.AirspaceSign.AirspaceSignFont.Name;
                    pCallout.AirspaceFontColorBlue = this.AirspaceSign.AirspaceSignFont.FontColor.Blue;
                    pCallout.AirspaceFontColorRed = this.AirspaceSign.AirspaceSignFont.FontColor.Red;
                    pCallout.AirspaceFontColorGreen = this.AirspaceSign.AirspaceSignFont.FontColor.Green;
                    pCallout.AirspaceFontUnderLine = this.AirspaceSign.AirspaceSignFont.UnderLine;
                    pCallout.AirspaceFontBold = this.AirspaceSign.AirspaceSignFont.Bold;
                    pCallout.AirspaceFontItalic = this.AirspaceSign.AirspaceSignFont.Italic;
                    pCallout.AirspaceSignScaleToFit = this.AirspaceSign.AirspaceSignScaleToFit;
                    pCallout.AirspaceSignSize = this.AirspaceSign.AirspaceSignSize;
                }

                pCallout.BackColor = this.FillColor.GetColor(); 

                if (this.FillStyle == fillStyle.fSBackwardDiagonal) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSBackwardDiagonal;
                if (this.FillStyle == fillStyle.fSCross) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSCross;
                if (this.FillStyle == fillStyle.fSDiagonalCross) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSDiagonalCross;
                if (this.FillStyle == fillStyle.fSForwardDiagonal) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSForwardDiagonal;
                if (this.FillStyle == fillStyle.fSHollow) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSHollow;
                if (this.FillStyle == fillStyle.fSHorizontal) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSHorizontal;
                if (this.FillStyle == fillStyle.fSNull) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSNull;
                if (this.FillStyle == fillStyle.fSSolid) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSSolid;
                if (this.FillStyle == fillStyle.fSVertical) pCallout.calloutFillStyle = esriSimpleFillStyle.esriSFSVertical;


                pCallout.LineSymbol = new SimpleLineSymbol();
                pCallout.LineSymbol.Width = this.Border.Thickness;
                //IRgbColor lineClr = new RgbColorClass(); // создание рамки
                //lineClr.Red = this.Frame.FrameColor.Red;
                //lineClr.Green = this.Frame.FrameColor.Green;
                //lineClr.Blue = this.Frame.FrameColor.Blue;
                pCallout.LineSymbol.Color = this.Frame.FrameColor.GetColor();

                if (this.Frame.FrameLineStyle == lineStyle.lsDash) pCallout.LineSymbol.Style = esriSimpleLineStyle.esriSLSDash;
                if (this.Frame.FrameLineStyle == lineStyle.lsDashDot) pCallout.LineSymbol.Style = esriSimpleLineStyle.esriSLSDashDot;
                if (this.Frame.FrameLineStyle == lineStyle.lsDashDotDot) pCallout.LineSymbol.Style = esriSimpleLineStyle.esriSLSDashDotDot;
                if (this.Frame.FrameLineStyle == lineStyle.lsDot) pCallout.LineSymbol.Style = esriSimpleLineStyle.esriSLSDot;
                if (this.Frame.FrameLineStyle == lineStyle.lsInsideFrame) pCallout.LineSymbol.Style = esriSimpleLineStyle.esriSLSInsideFrame;
                if (this.Frame.FrameLineStyle == lineStyle.lsNull) pCallout.LineSymbol.Style = esriSimpleLineStyle.esriSLSNull;
                if (this.Frame.FrameLineStyle == lineStyle.lsSolid) pCallout.LineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;


                //pCallout.DrawLeader = this.LeaderLine.LeaderLineWidth > 0;
                pCallout.LineSymbol.Width = this.Frame.Thickness;
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
            ChartElement_SigmaCollout o = (ChartElement_SigmaCollout)base.Clone();
            if (o.AirspaceSign != null) o.AirspaceSign = (SigmaCallout_AirspaceSign)o.AirspaceSign.Clone();
            if (o.MorseTextLine != null) o.MorseTextLine = (SigmaCallout_Morse)o.MorseTextLine.Clone();
            if (o.ElementShadow != null) o.ElementShadow = (SigmaCallout_Shadow)o.ElementShadow.Clone();
            if (o.Frame != null) o.Frame = (SigmaCallout_Frame)o.Frame.Clone();
            return o;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override IGeometry StopFeedback(IDisplayFeedback feedBack, int X, int Y, IGeometry LinkedGeometry, int Shift)
        {
            return base.StopFeedback(feedBack, X, Y, LinkedGeometry, Shift);
        }

        public override IDisplayFeedback GetFeedback()
        {
            ChartElement_SigmaCollout symbol = (ChartElement_SigmaCollout)this.Clone();
            symbol.MorseTextLine = null;
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
            mvPtFeed.Symbol = (ISymbol)txtS;

            return mvPtFeed;
        }


        //public override void StartFeedback(IDisplayFeedback feedBack, IPoint _position, double scale, IGeometry LinkedGeometry)
        //{
        //    ChartElement_SigmaCollout symbol = (ChartElement_SigmaCollout)this.Clone();
        //    symbol.MorseTextLine = null;
        //    IElement iEl = (IElement)symbol.ConvertToIElement();
        //    ITextSymbol txtS = null;


        //    txtS = ((ITextElement)iEl).Symbol;


        //    feedBack = new CalloutFeedbackClass();
        //    feedBack.Display = _activeView.ScreenDisplay;
        //    // QI for the IMovePointFeedback interface
        //    CalloutFeedbackClass mvPtFeed = (CalloutFeedbackClass)feedBack;
        //    //Start the feedback using the input (Point) geometry at the current mouse location

        //    mvPtFeed.Symbol = txtS as ISymbol;

        //    mvPtFeed.Start((ISymbol)txtS, _position, _position, scale);
        //}

    }

    public class ChartElement_SigmaCollout_Navaid : ChartElement_SigmaCollout
    {
        public ChartElement_SigmaCollout_Navaid()
        {
        }

        public ChartElement_SigmaCollout_Navaid(string CaptionText)
        {
            #region Text formating
            this.ColloutType = 1;

            this.Font = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "Arial", Size = 8, UnderLine = false };
            this.FillColor = new AncorColor (255,255,255);
            //this.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
            this.HaloColor = new AncorColor(255, 255, 255);
            this.HorizontalAlignment = horizontalAlignment.Center;
            this.VerticalAlignment = verticalAlignment.Center;
            this.TextPosition = textPosition.Normal;
            this.TextCase = textCase.Normal;
            this.WordSpacing = 50;
            ///////////////////////////////////////////////////////////////////////////////////////////
            this.CaptionTextLine = new List<List<AncorChartElementWord>>(); // создаем заголовок

            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord(CaptionText, this.Font);//создаем слово
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("+");
            wrd.EndSymbol = new AncorSymbol("+");
            wrd.Morse = false;
            txtLine.Add(wrd);
            this.CaptionTextLine.Add(txtLine);  // добавим его в заголовок
            ////////////////////////////////////////////////////////////////////////////////////////////

            this.TextContents = new List<List<AncorChartElementWord>>();

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("VOR/DME ", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.WordSpacing = 50;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine); // добавим строку


            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("116.9", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            wrd.Morse = false;
            txtLine.Add(wrd); // добавим его в строку

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("BOR", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            //wrd.WordSpacing = 300;

            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            txtLine.Add(wrd); // добавим его в строку


            this.TextContents.Add(txtLine);  // добавим строку


            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("52°22'06 N", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;

            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            wrd.Morse = false;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);  // добавим строку

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("036°34'45 W", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            wrd.Morse = false;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);  // добавим строку

            ///////////////////////////////////////////////////////////////////////////////////////////

            this.BottomTextLine = new List<List<AncorChartElementWord>>(); // создаем нижнюю строку

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("30 ", this.Font);//создаем слово
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.WordSpacing = 50;
            //wrd.WordSpacing = 30;
            txtLine.Add(wrd);

            wrd = new AncorChartElementWord("M", this.Font);//создаем слово
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            wrd.Morse = false;
            //wrd.WordSpacing = 50;
            txtLine.Add(wrd);

            this.BottomTextLine.Add(txtLine);  // добавим его в нижнюю строку

            ////////////////////////////////////////////////////////////////////////////////////////

            #endregion

            this.Border = new AncorFrame(lineStyle.lsSolid);
            this.Border.FrameMargins = new AncorFrameMargins(0, 0, 0, 0);

            //sigmaCallout properties
            this.AirspaceSign = null;
            this.Frame = new SigmaCallout_Frame { FrameLineStyle = lineStyle.lsSolid, Offset = 0, Thickness = 1, FrameColor = new AncorColor(255, 0, 0), FrameMargins = new SigmaCallout_FrameMargins(0, 0, 0, 0) };
            //this.Snap = 0;
            this.ElementShadow = new SigmaCallout_Shadow { ShadowSize = 0, ShadowCoLor = new AncorColor(255, 0, 0) };
            this.LeaderTolerance = 0;

            this.HasHeader = true;
            this.HasFooter = true;
            //=================Morse
            this.MorseTextLine = new SigmaCallout_Morse { MorseColor = new AncorColor(255, 0, 0), MorseLeading = -10, MorseText = "BOR", MorseSize = 8, MorseLocationShiftOnX = 0, MorseLocationShiftOnY = 0 };
            //==================================================================End Morse
            this.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
        }


        [Browsable(false)]
        public override SigmaCallout_AirspaceSign AirspaceSign
        {
            get
            {
                return base.AirspaceSign;
            }
            set
            {
                base.AirspaceSign = value;
            }
        }

        [Browsable(false)]
        public override sigmaCalloutAccentbarPosition AccentBarPosition { get => base.AccentBarPosition; set => base.AccentBarPosition = value; }
    }

    public class ChartElement_SigmaCollout_Airspace : ChartElement_SigmaCollout
    {
        public ChartElement_SigmaCollout_Airspace()
        {
        }

        public ChartElement_SigmaCollout_Airspace(string CaptionText)
        {
            #region Text formating

            this.ColloutType = 2;


            this.Font = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "Arial", Size = 8, UnderLine = false };
            this.FillColor = new AncorColor (255,255,255);
            //this.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
            this.HaloColor = new AncorColor(255, 255, 255);
            this.HorizontalAlignment = horizontalAlignment.Center;
            this.VerticalAlignment = verticalAlignment.Center;
            this.TextPosition = textPosition.Normal;
            this.TextCase = textCase.Normal;
            this.WordSpacing = 50;
            ///////////////////////////////////////////////////////////////////////////////////////////
            this.CaptionTextLine = new List<List<AncorChartElementWord>>(); // создаем заголовок

            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord(CaptionText, this.Font);//создаем слово
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("+");
            wrd.EndSymbol = new AncorSymbol("+");
            wrd.Morse = false;
            txtLine.Add(wrd);
            this.CaptionTextLine.Add(txtLine);  // добавим его в заголовок
            ////////////////////////////////////////////////////////////////////////////////////////////

            this.TextContents = new List<List<AncorChartElementWord>>();

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("VOR/DME ", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.WordSpacing = 50;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine); // добавим строку


            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("116.9", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            wrd.Morse = false;
            txtLine.Add(wrd); // добавим его в строку

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("BOR", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            //wrd.WordSpacing = 300;

            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            txtLine.Add(wrd); // добавим его в строку


            this.TextContents.Add(txtLine);  // добавим строку


            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("52°22'06 N", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;

            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            wrd.Morse = false;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);  // добавим строку

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("036°34'45 W", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            wrd.Morse = false;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);  // добавим строку

            ///////////////////////////////////////////////////////////////////////////////////////////

            this.BottomTextLine = new List<List<AncorChartElementWord>>(); // создаем нижнюю строку

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("30 ", this.Font);//создаем слово
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.WordSpacing = 50;
            //wrd.WordSpacing = 30;
            txtLine.Add(wrd);

            wrd = new AncorChartElementWord("M", this.Font);//создаем слово
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            wrd.Morse = false;
            //wrd.WordSpacing = 50;
            txtLine.Add(wrd);

            this.BottomTextLine.Add(txtLine);  // добавим его в нижнюю строку

            ////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            this.Border = new AncorFrame(lineStyle.lsSolid);
            this.Border.FrameMargins = new AncorFrameMargins(0, 0, 0, 0);

            //sigmaCallout properties
            this.AirspaceSign = new SigmaCallout_AirspaceSign { AirspaceSymbols = "A", AirspaceOnLeftSide = true, AirspaceBackColor = new AncorColor(255, 0, 0), AirspaceSignFont = new AncorFont(true, new AncorColor(255, 255, 255), false, "Arial", 8, false), AirspaceSignScaleToFit = true, AirspaceSignSize = 12 };
            this.Frame = new SigmaCallout_Frame { FrameLineStyle = lineStyle.lsSolid, Offset = 0, Thickness = 1, FrameColor = new AncorColor(255, 0, 0), FrameMargins = new SigmaCallout_FrameMargins(0, 0, 0, 0) };
            //this.Snap = 0;
            this.ElementShadow = new SigmaCallout_Shadow { ShadowSize = 0, ShadowCoLor = new AncorColor(255, 0, 0) };
            this.LeaderTolerance = 0;

            this.HasHeader = true;
            this.HasFooter = true;
            //=================Morse
            this.MorseTextLine = null;
            //==================================================================End Morse
            this.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
            this.LeaderLine.LeaderLineWidth = 1;
        }


        [Browsable(false)]
        public override SigmaCallout_Morse MorseTextLine
        {
            get
            {
                return base.MorseTextLine;
            }
            set
            {
                base.MorseTextLine = value;
            }
        }

        [Browsable(false)]
        public override sigmaCalloutAccentbarPosition AccentBarPosition { get => base.AccentBarPosition; set => base.AccentBarPosition = value; }
    }

    public class ChartElement_SigmaCollout_Designatedpoint : ChartElement_SigmaCollout
    {
        public ChartElement_SigmaCollout_Designatedpoint()
        {
        }

        public ChartElement_SigmaCollout_Designatedpoint(string CaptionText)
        {
            #region Text formating

            this.ColloutType = 3;

            this.Font = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "Arial", Size = 8, UnderLine = false };
            this.FillColor = new AncorColor (255,255,255);
            //this.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
            this.HaloColor = new AncorColor(255, 255, 255);
            this.HorizontalAlignment = horizontalAlignment.Center;
            this.VerticalAlignment = verticalAlignment.Center;
            this.TextPosition = textPosition.Normal;
            this.TextCase = textCase.Normal;
            this.WordSpacing = 50;
            ///////////////////////////////////////////////////////////////////////////////////////////
            this.CaptionTextLine = new List<List<AncorChartElementWord>>(); // создаем заголовок

            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord(CaptionText, this.Font);//создаем слово
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("+");
            wrd.EndSymbol = new AncorSymbol("+");
            wrd.Morse = false;
            txtLine.Add(wrd);
            this.CaptionTextLine.Add(txtLine);  // добавим его в заголовок
            ////////////////////////////////////////////////////////////////////////////////////////////

            this.TextContents = new List<List<AncorChartElementWord>>();

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("VOR/DME ", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.WordSpacing = 50;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine); // добавим строку


            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("116.9", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            wrd.Morse = false;
            txtLine.Add(wrd); // добавим его в строку

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("BOR", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            //wrd.WordSpacing = 300;

            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            txtLine.Add(wrd); // добавим его в строку


            this.TextContents.Add(txtLine);  // добавим строку


            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("52°22'06 N", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;

            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            wrd.Morse = false;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);  // добавим строку

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("036°34'45 W", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            wrd.Morse = false;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);  // добавим строку

            #endregion


            this.Border = new AncorFrame(lineStyle.lsSolid);
            this.Border.FrameMargins = new AncorFrameMargins(0, 0, 0, 0);

            //sigmaCallout properties
            this.AirspaceSign = null;
            this.Frame = new SigmaCallout_Frame { FrameLineStyle = lineStyle.lsSolid, Offset = 0, Thickness = 1, FrameColor = new AncorColor(255, 0, 0), FrameMargins = new SigmaCallout_FrameMargins(0, 0, 0, 0) };
            //this.Snap = 0;
            this.ElementShadow = new SigmaCallout_Shadow { ShadowSize = 0, ShadowCoLor = new AncorColor(255, 0, 0) };
            this.LeaderTolerance = 0;

            this.HasHeader = true;
            this.HasFooter = false;
            //=================Morse
            this.MorseTextLine = null;
            //==================================================================End Morse
            this.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
        }


        [Browsable(false)]
        public override SigmaCallout_AirspaceSign AirspaceSign
        {
            get
            {
                return base.AirspaceSign;
            }
            set
            {
                base.AirspaceSign = value;
            }
        }

        [Browsable(false)]
        public override SigmaCallout_Morse MorseTextLine
        {
            get
            {
                return base.MorseTextLine;
            }
            set
            {
                base.MorseTextLine = value;
            }
        }

        [Browsable(false)]
        public override sigmaCalloutAccentbarPosition AccentBarPosition { get => base.AccentBarPosition; set => base.AccentBarPosition = value; }

    }

    public class ChartElement_SigmaCollout_AccentBar : ChartElement_SigmaCollout
    {
        public ChartElement_SigmaCollout_AccentBar()
        { }

        public ChartElement_SigmaCollout_AccentBar(string _Text)
        {
            #region Text formating
            this.ColloutType = 4;


            this.Font = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "Arial", Size = 8, UnderLine = false };
            this.FillColor = new AncorColor(255, 255, 255);
            //this.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
            this.HaloColor = new AncorColor(255, 255, 255);
            this.HorizontalAlignment = horizontalAlignment.Center;
            this.VerticalAlignment = verticalAlignment.Center;
            this.TextPosition = textPosition.Normal;
            this.TextCase = textCase.Normal;
            this.WordSpacing = 50;

            ///////////////////////////////////////////////////////////////////////////////////////////
            this.CaptionTextLine = null;
            ////////////////////////////////////////////////////////////////////////////////////////////

            this.TextContents = new List<List<AncorChartElementWord>>();

            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord(_Text, this.Font);//создаем слово
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine); // добавим строку
            

            ///////////////////////////////////////////////////////////////////////////////////////////

            this.BottomTextLine = null;

            ////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            this.Border = new AncorFrame(lineStyle.lsSolid);
            this.Border.FrameMargins = new AncorFrameMargins(0, 0, 0, 0);

            //sigmaCallout properties
            this.AirspaceSign = null;//new SigmaCallout_AirspaceSign { AirspaceSymbols = "A", AirspaceOnLeftSide = true, AirspaceBackColor = new AncorColor(255, 0, 0), AirspaceSignFont = new AncorFont(true, new AncorColor(255, 255, 255), false, "Arial", 8, false), AirspaceSignScaleToFit = true, AirspaceSignSize = 12 };
            this.Frame = new SigmaCallout_Frame { FrameLineStyle = lineStyle.lsSolid, Offset = 0, Thickness = 1, FrameColor = new AncorColor(255, 0, 0), FrameMargins = new SigmaCallout_FrameMargins(0, 0, 0, 0) };
            //this.Snap = 0;
            this.ElementShadow = null;//new SigmaCallout_Shadow { ShadowSize = 0, ShadowCoLor = new AncorColor(255, 0, 0) };
            this.LeaderTolerance = 0;

            this.HasHeader = false;
            this.HasFooter = false;
            //=================Morse
            this.MorseTextLine = null;
            //==================================================================End Morse
            this.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
            this.LeaderLine.LeaderLineWidth = 1;

            this.AccentBarPosition = sigmaCalloutAccentbarPosition.left;

        }


        [Browsable(false)]
        public override SigmaCallout_AirspaceSign AirspaceSign
        {
            get
            {
                return base.AirspaceSign;
            }
            set
            {
                base.AirspaceSign = value;
            }
        }

        [Browsable(false)]
        public override SigmaCallout_Morse MorseTextLine
        {
            get
            {
                return base.MorseTextLine;
            }
            set
            {
                base.MorseTextLine = value;
            }
        }

        [DisplayName("AccentBar")]
        public override SigmaCallout_Frame Frame { get => base.Frame; set => base.Frame = value; }


        [Browsable(false)]
        public override AncorShadow Shadow { get => base.Shadow; set => base.Shadow = value; }

        [Browsable(false)]
        public override SigmaCallout_Shadow ElementShadow { get => base.ElementShadow; set => base.ElementShadow = value; }

        [Browsable(true)]
        public override sigmaCalloutAccentbarPosition AccentBarPosition { get => base.AccentBarPosition; set => base.AccentBarPosition = value; }

        [Browsable(false)]
        public override AncorLeaderLine LeaderLine { get => base.LeaderLine; set => base.LeaderLine = value; }


    }


}

