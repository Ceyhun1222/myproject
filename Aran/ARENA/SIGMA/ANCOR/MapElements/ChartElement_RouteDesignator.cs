using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANCOR.MapCore;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using System.ComponentModel;
using System.Drawing.Design;
//using Accent.MapCore;

namespace ANCOR.MapElements
{
    public class ChartElement_RouteDesignator : ChartElement_SimpleText
    {

        private routeSegmentDirection _routeSegmentDirection;
        [Category("Decoration")]
        [SkipAttribute(true)]
        public routeSegmentDirection RouteSegmentDirection
        {
            get { return _routeSegmentDirection; }
            set { _routeSegmentDirection = value; }
        }

        private double _routeSignSize;
        [Category("Decoration")]
        [SkipAttribute(false)]
        public double RouteSignSize
        {
            get { return _routeSignSize; }
            set { _routeSignSize = value; }
        }

        private routeDesignatorPosition _routeDesignatorPosition;
        [Category("Alligment")]
        [SkipAttribute(false)]
        public routeDesignatorPosition RouteDesignatorPosition
        {
            get { return _routeDesignatorPosition; }
            set { _routeDesignatorPosition = value; }
        }

        private AncorMarkerBackGround _markerBackGround;
        [Category("Decoration")]
        [Browsable(false)]
        [SkipAttribute(false)]
        public AncorMarkerBackGround MarkerBackGround
        {
            get { return _markerBackGround; }
            set { _markerBackGround = value; }
        }

        private List<List<AncorChartElementWord>> _RouteDesignatorSource;
        [Editor(typeof(MyTextContextEditor), typeof(UITypeEditor))]
        [Browsable(false)]
        [SkipAttribute(false)]
        public List<List<AncorChartElementWord>> RouteDesignatorSource
        {
            get { return _RouteDesignatorSource; }
            set { _RouteDesignatorSource = value; }
        }

        private bool _scaleMarkerToFitText;
        [Category("Decoration")]
        //[Browsable(false)]
        [SkipAttribute(false)]
        public bool ScaleMarkerToFitText
        {
            get { return _scaleMarkerToFitText; }
            set { _scaleMarkerToFitText = value; }
        }

        private double _lineSpasing;
        [Category("Decoration")]
        [SkipAttribute(false)]
       // [Browsable(false)]
        public double LineSpasing
        {
            get { return _lineSpasing; }
            set { _lineSpasing = value; }
        }

        private bool _boldDesignatorText;
        [Category("Decoration")]
        [SkipAttribute(false)]
        public bool BoldDesignatorText
        {
            get { return _boldDesignatorText; }
            set { _boldDesignatorText = value; }
        }

        private bool _hideDesignatorText;
        [Category("Decoration")]
        [SkipAttribute(false)]
        //[Browsable(false)]
        public bool HideDesignatorText
        {
            get { return _hideDesignatorText; }
            set { _hideDesignatorText = value; }
        }

        private bool _wrapRouteDesignatorText;
        [Category("Decoration")]
        [SkipAttribute(false)]
        public bool WrapRouteDesignatorText
        {
            get { return _wrapRouteDesignatorText; }
            set { _wrapRouteDesignatorText = value; }
        }

        private bool _reverseSign;
        [Browsable(false)]
        [SkipAttribute(false)]
        public bool ReverseSign
        {
            get { return _reverseSign; }
            set { _reverseSign = value; }
        }

        [Skip(true)]
        public override AncorPoint Anchor
        {
            get
            {
                return base.Anchor;
            }
            set
            {
                base.Anchor = value;
            }
        }


        public ChartElement_RouteDesignator(string _txt)
        {
            this.MarkerBackGround = new AncorMarkerBackGround(84, 40, "AeroSigma");
            this.RouteDesignatorPosition = routeDesignatorPosition.Bottom;
            this.RouteSegmentDirection = routeSegmentDirection.Both;
            this.RouteSignSize = 40;
            this.FillStyle = fillStyle.fSSolid;
            this.HaloColor = new AncorColor (255,255,255);
            this.FillColor = new AncorColor(255, 0, 0);
            //this.Font.FontColor = new AcntColor(0, 0, 0);
            //this.Font.Size = 10;
            this.Font = new AncorFont("Courier New", 10);
            this.ScaleMarkerToFitText = false;
            this.LineSpasing = 15;
            this.BoldDesignatorText = false;
            this.HideDesignatorText = false;
            this.HorizontalAlignment = horizontalAlignment.Center;
            this.VerticalAlignment = verticalAlignment.Center;
            this.TextPosition = textPosition.Normal;
            this.TextCase = textCase.Normal;
            /////////////////////////////////////////////////////////////////////////////////////////////
            this.TextContents = new List<List<AncorChartElementWord>>();

            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord(_txt, this.Font);//создаем слово
            //wrd.TextValue = _txt;
            wrd.Font.Bold = false;
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);


            this.RouteDesignatorSource = new List<List<AncorChartElementWord>>();
            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("RouteDesignator", this.Font);//создаем слово
            //wrd.TextValue = _txt;
            wrd.Font.Bold = false;
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку
            this.RouteDesignatorSource.Add(txtLine);

            /////////////////////////////////////////////////////////////////////////////////////////////
            
        }

        public ChartElement_RouteDesignator()
        {
        }

        private List<AncorChartElementWord> CreateLine(string WordText, string objectUid = null)
        {
            List<AncorChartElementWord> txtLineEmpty = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrdEmpty = new AncorChartElementWord(WordText);//создаем слово пустышку
            wrdEmpty.Font.Bold = this.TextContents[0][0].Font.Bold;//false;
            wrdEmpty.Font.Size = this.TextContents[0][0].Font.Size;//false;
            wrdEmpty.Font.Italic = this.TextContents[0][0].Font.Italic;//false;
            wrdEmpty.Font.UnderLine = this.TextContents[0][0].Font.UnderLine;//false;
            wrdEmpty.Font.Name = this.TextContents[0][0].Font.Name;
            wrdEmpty.Font.Size = this.TextContents[0][0].Font.Size;
            wrdEmpty.Font.FontColor = new AncorColor(this.TextContents[0][0].Font.FontColor.Red, this.TextContents[0][0].Font.FontColor.Green, this.TextContents[0][0].Font.FontColor.Blue);
            wrdEmpty.DataSource.Link = "empty";

            if (objectUid != null) wrdEmpty.DataSource.Condition = objectUid;
            txtLineEmpty.Add(wrdEmpty); // добавим его в строку

            return txtLineEmpty;
        }

        public void WrapDesignatorText()
        {
            string[] lst = this.RouteDesignatorSource[0][0].TextValue.Split('/');
            string[] uids = this.RouteDesignatorSource[0][0].DataSource.Condition.Split('/');

            this.RouteDesignatorSource[0][0].TextValue = lst[0];
            this.RouteDesignatorSource[0][0].DataSource.Condition = uids[0];
            //////////////////
            if (!this.RouteDesignatorSource[0][0].DataSource.Link.StartsWith("empty"))
            {
                List<AncorChartElementWord> ln = CreateLine(" ");
                this.RouteDesignatorSource.Insert(0, ln);
            }
            //////////////////////

            if (lst.Length > 1)
            {
                for (int i = 1; i <= lst.Length - 1; i++)
                {
                    // добавим его в строку
                    this.RouteDesignatorSource.Add(CreateLine(lst[i], uids[i]));
                }

            }
            
            //this.LineSpasing = this.RouteDesignatorPosition == routeDesignatorPosition.Bottom ? -3 : 4;
        }

        public void ExpandDesignatorText()
        {
            string name = "";
            string uid = "";

            // remove first "empty" line
            if (this.RouteDesignatorSource[0][0].DataSource.Link.StartsWith("empty"))
                this.RouteDesignatorSource.RemoveAt(0);

            foreach (var line in this.RouteDesignatorSource)
            {
                foreach (var wrd in line)
                {
                    if (wrd.TextValue.Length <= 0) continue;
                    name = name + wrd.TextValue + "/";
                    uid = uid + wrd.DataSource.Condition + "/";

                }
            }

            this.RouteDesignatorSource[0][0].TextValue = name.TrimEnd('/');
            this.RouteDesignatorSource[0][0].TextValue = this.RouteDesignatorSource[0][0].TextValue.TrimStart('/');
            this.RouteDesignatorSource[0][0].DataSource.Condition = uid.TrimEnd('/');
            this.RouteDesignatorSource.RemoveRange(1, this.RouteDesignatorSource.Count - 1);
            this.LineSpasing = 5;
        }

        public override object ConvertToIElement()
        {
            try
            {
                #region Wrap

                if (this.WrapRouteDesignatorText)
                {
                    this.WrapDesignatorText();
                }
                else
                {
                    this.ExpandDesignatorText();
                }

                #endregion

                #region "Создание RouteSign"

                TextSymbolClass textSymbolInner = new TextSymbolClass();

                textSymbolInner.Angle = this.Slope;

                // цвет текста и рамки
                //IRgbColor Clr = new RgbColorClass();
                //Clr.Red = this.Font.FontColor.Red;
                //Clr.Green = this.Font.FontColor.Green;
                //Clr.Blue = this.Font.FontColor.Blue;
                textSymbolInner.Color = this.Font.FontColor.GetColor(); // Clr;

                TextSymbolClass textSymbolFiller = new TextSymbolClass();
                textSymbolFiller.Angle = this.Slope;

                // включаем маску Halo
                if (this.HaloMaskSize > 0)
                {
                    #region

                    IMask haloMsk = (IMask)textSymbolInner;
                    haloMsk.MaskStyle = esriMaskStyle.esriMSHalo;
                    haloMsk.MaskSize = this.HaloMaskSize;

                    ISimpleFillSymbol smplFillHalo = new SimpleFillSymbol();

                    IRgbColor rgbClrHalo = new RgbColor();  //цвет заполнения
                    rgbClrHalo.Red = this.HaloColor.Red;
                    rgbClrHalo.Blue = this.HaloColor.Blue;
                    rgbClrHalo.Green = this.HaloColor.Green;

                    smplFillHalo.Color = rgbClrHalo;

                    ISimpleLineSymbol HaloLine = new SimpleLineSymbolClass();
                    HaloLine.Color = rgbClrHalo;
                    HaloLine.Width = 0;


                    smplFillHalo.Outline = HaloLine;


                    haloMsk.MaskSymbol = smplFillHalo;

                    #endregion

                }


                ITextPath textPath = new BezierTextPathClass();
                textPath.XOffset = 0;
                textPath.YOffset = 0;

                IMarkerTextBackground TxtBackGrndInner = new MarkerTextBackground();
                TxtBackGrndInner.ScaleToFit = this.ScaleMarkerToFitText;


                IMarkerTextBackground TxtBackGrndOuter = new MarkerTextBackground();
                TxtBackGrndOuter.ScaleToFit = this.ScaleMarkerToFitText;


                ICharacterMarkerSymbol characterMarkerSymbolInner = new CharacterMarkerSymbolClass();
                ICharacterMarkerSymbol characterMarkerSymbolFiller = new CharacterMarkerSymbolClass();


                stdole.IFontDisp stdFont = new stdole.StdFontClass() as stdole.IFontDisp;
                stdFont.Name = this.MarkerBackGround.FontName;


                characterMarkerSymbolInner.Font = stdFont;
                characterMarkerSymbolFiller.Font = stdFont;

                characterMarkerSymbolInner.Angle = this.Slope * Math.PI / 180;

                if (this.RouteSegmentDirection == routeSegmentDirection.Both)
                {
                    characterMarkerSymbolInner.CharacterIndex = 84; // "пахлава"
                    characterMarkerSymbolFiller.CharacterIndex = 86; // "пахлава"
                }

                if (this.RouteSegmentDirection == routeSegmentDirection.Forward)
                {
                    characterMarkerSymbolInner.CharacterIndex = 93; // "пахлава"
                    characterMarkerSymbolFiller.CharacterIndex = 95; // "пахлава"
                    if (this.ReverseSign)
                    {
                        characterMarkerSymbolInner.CharacterIndex = 83;
                        characterMarkerSymbolFiller.CharacterIndex = 87;
                    }
                }


                if (this.RouteSegmentDirection == routeSegmentDirection.Backward)
                {
                    characterMarkerSymbolInner.CharacterIndex = 83; // "пахлава"
                    characterMarkerSymbolFiller.CharacterIndex = 87; // "пахлава"
                    if (this.ReverseSign)
                    {
                        characterMarkerSymbolInner.CharacterIndex = 93;
                        characterMarkerSymbolFiller.CharacterIndex = 95;
                    }
                    
                }

                //размер знака символа
                characterMarkerSymbolInner.Size = this.RouteSignSize;
                characterMarkerSymbolFiller.Size = this.RouteSignSize;// -3;


                //цвет заливки
                IRgbColor FillClr = new RgbColorClass();
                FillClr.Red = this.FillColor.Red;
                FillClr.Green = this.FillColor.Green;
                FillClr.Blue = this.FillColor.Blue;

                characterMarkerSymbolFiller.Color = this.FillColor.GetColor();//  FillClr;
                //characterMarkerSymbolFiller.XOffset = this.Anchor.X;
                //characterMarkerSymbolFiller.YOffset = this.Anchor.Y;

                characterMarkerSymbolInner.Color = this.Font.FontColor.GetColor();//Clr;
                //characterMarkerSymbolInner.XOffset = this.Anchor.X;
                //characterMarkerSymbolInner.YOffset = this.Anchor.Y;


                TxtBackGrndInner.Symbol = characterMarkerSymbolInner;
                TxtBackGrndOuter.Symbol = characterMarkerSymbolFiller;


                textSymbolInner.Background = TxtBackGrndInner;
                textSymbolFiller.Background = TxtBackGrndOuter;


                ISimpleTextSymbol simpleTextSymbolInner = (ISimpleTextSymbol)textSymbolInner;

                simpleTextSymbolInner.TextPath = textPath;
                simpleTextSymbolInner.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                simpleTextSymbolInner.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                simpleTextSymbolInner.Angle = this.Slope;
                simpleTextSymbolInner.XOffset = this.Anchor.X;
                simpleTextSymbolInner.YOffset = this.Anchor.Y;

                // размер и характеристика шрифта для внутреннего текста
                stdole.IFontDisp pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                pFontDisp.Name = this.Font.Name;
                pFontDisp.Bold = this.Font.Bold;
                pFontDisp.Italic = this.Font.Italic;
                pFontDisp.Underline = this.Font.UnderLine;
                pFontDisp.Size = (decimal)this.Font.Size;
                simpleTextSymbolInner.Font = pFontDisp;


                ITextElement pTextEl_Inner = new TextElementClass();
                pTextEl_Inner.Text = HelperClass.TextConstructor(this.TextContents); //AccentObjManipulator.CreateAnnoText(this.InnerText, mapScale);
                pTextEl_Inner.ScaleText = false;
                pTextEl_Inner.Symbol = simpleTextSymbolInner;


                IElement El_Inner = (IElement)pTextEl_Inner;

                ISimpleTextSymbol simpleTextSymbolFiller = (ISimpleTextSymbol)textSymbolFiller;
                simpleTextSymbolFiller.TextPath = textPath;
                simpleTextSymbolFiller.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                simpleTextSymbolFiller.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                simpleTextSymbolFiller.Font = pFontDisp;
                simpleTextSymbolFiller.XOffset = this.Anchor.X;
                simpleTextSymbolFiller.YOffset = this.Anchor.Y;



                ITextElement pTextEl_filler = new TextElementClass();
                pTextEl_filler.Text = " ";
                pTextEl_filler.Text = HelperClass.TextConstructor(this.TextContents);
                pTextEl_filler.ScaleText = false;
                pTextEl_filler.Symbol = simpleTextSymbolFiller;



                IElement El_Filler = (IElement)pTextEl_filler;

                #endregion  "Создание RouteSign"

                #region "Создание RouteDesignator" (название маршрута)

                //Create a text symbol and grab hold of the ITextSymbol interface
                TextSymbolClass textSymbolRouteDesignator = new TextSymbolClass();
                textSymbolRouteDesignator.XOffset = 0;

                double ls = this.RouteDesignatorPosition == routeDesignatorPosition.Bottom ?  3+ this.LineSpasing : 4 + this.LineSpasing;

                if (this.RouteDesignatorPosition == routeDesignatorPosition.Top) textSymbolRouteDesignator.YOffset = this.Anchor.Y + ls;//this.LineSpasing;
                if (this.RouteDesignatorPosition == routeDesignatorPosition.Bottom) textSymbolRouteDesignator.YOffset = this.Anchor.Y + ls;//-this.LineSpasing;

                //Clr = new RgbColorClass();
                //Clr.Red = this.Font.FontColor.Red;
                //Clr.Green = this.Font.FontColor.Green;
                //Clr.Blue = this.Font.FontColor.Blue;
                textSymbolRouteDesignator.Color = this.Font.FontColor.GetColor();//Clr;


                pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                pFontDisp.Name = this.Font.Name;
                pFontDisp.Bold = this.BoldDesignatorText;
                pFontDisp.Italic = this.Font.Italic;
                pFontDisp.Underline = this.Font.UnderLine;
                pFontDisp.Size = (decimal)this.Font.Size;
                textSymbolRouteDesignator.Font = pFontDisp;


                textPath = new BezierTextPathClass();
                textPath.XOffset = 0;
                //textPath.YOffset = 10;

                ISimpleTextSymbol simpleTextSymbolRouteDesignator = (ISimpleTextSymbol)textSymbolRouteDesignator;
                simpleTextSymbolRouteDesignator.TextPath = textPath;
                simpleTextSymbolRouteDesignator.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                simpleTextSymbolRouteDesignator.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;


                ///////////////////////////////////////


                // форматирование текста
                this.VerticalAlignment = this.RouteDesignatorPosition == routeDesignatorPosition.Top ? verticalAlignment.Bottom : verticalAlignment.Top;
                HelperClass.FormatText(ref textSymbolRouteDesignator, this.TextPosition, this.Leading, this.TextCase, this.HorizontalAlignment, this.VerticalAlignment, this.CharacterSpacing, this.CharacterWidth, this.WordSpacing);
                HelperClass.CreateFont(ref textSymbolRouteDesignator, this.Font);
                ///////////////////////////////////////


                ITextElement pTextEl_RouteDesignator = new TextElementClass();
                pTextEl_RouteDesignator.Text = HelperClass.TextConstructor(this.RouteDesignatorSource); //AccentObjManipulator.CreateAnnoText(Tmp, mapScale);
                //pTextEl.Text = this.RouteDesignator;

                simpleTextSymbolRouteDesignator.Angle = this.Slope;



                pTextEl_RouteDesignator.Symbol = simpleTextSymbolRouteDesignator;
                //pTextEl_Inner.Symbol.Angle = this.Slope;

                if (this.TextPosition == textPosition.Superscript) (textSymbolRouteDesignator as IFormattedTextSymbol).Position = esriTextPosition.esriTPSuperscript;
                if (this.TextPosition == textPosition.Subscript) (textSymbolRouteDesignator as IFormattedTextSymbol).Position = esriTextPosition.esriTPSubscript;
                if (this.TextPosition == textPosition.Normal) (textSymbolRouteDesignator as IFormattedTextSymbol).Position = esriTextPosition.esriTPNormal;



                IElement El_RouteDesignator = (IElement)pTextEl_RouteDesignator;

                #endregion

                IGroupElement3 GrpEl = new GroupElementClass();

                if (this.FillStyle != fillStyle.fSNull)
                    GrpEl.AddElement(El_Filler);
                GrpEl.AddElement(El_Inner);
                if (!this.HideDesignatorText) GrpEl.AddElement(El_RouteDesignator);



                return GrpEl;
            }
            catch { return null; }
        }


      


        public override IDisplayFeedback GetFeedback()
        {
            ChartElement_SimpleText symbol = (ChartElement_SimpleText)this.Clone();
            IElement iEl = (IElement)symbol.ConvertToIElement();
            ITextSymbol txtS = null;

            if (iEl is IGroupElement)
            {
                txtS = ((ITextElement)((IGroupElement)iEl).get_Element(1)).Symbol;
                if (this.FillStyle == fillStyle.fSNull) txtS = ((ITextElement)((IGroupElement)iEl).get_Element(0)).Symbol;
            }
            else
            {
                txtS = ((ITextElement)iEl).Symbol;
            }

            //((ISimpleTextSymbol)txtS).XOffset = 0;
            //((ISimpleTextSymbol)txtS).YOffset = 0;



            IDisplayFeedback _feedback = new NewTextFeedbackClass();
            NewTextFeedbackClass mvPtFeed = (NewTextFeedbackClass)_feedback;
            mvPtFeed.Symbol = (ISymbol)txtS;

            return mvPtFeed;
        }

        public override object Clone()
        {
            ChartElement_RouteDesignator o = (ChartElement_RouteDesignator)base.Clone();

            o.MarkerBackGround = (AncorMarkerBackGround)o.MarkerBackGround.Clone();
            o.RouteDesignatorSource = CloneTextContenst(o.RouteDesignatorSource);

            return o;
        }
    }

}
