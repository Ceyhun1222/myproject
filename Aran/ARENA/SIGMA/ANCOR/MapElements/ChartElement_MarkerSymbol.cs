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
    public class ChartElement_MarkerSymbol  : ChartElement_SimpleText
    {
        //[Browsable(false)]
        public override AncorFont Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }


        private AncorMarkerBackGround _markerBackGround;
        [Category("Decoration")]
        //[Browsable(false)]
        [SkipAttribute(false)]
        public AncorMarkerBackGround MarkerBackGround
        {
            get { return _markerBackGround; }
            set { _markerBackGround = value; }
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

        private AncorColor _markerFrameColor;
        [Category("Decoration")]
        //[Browsable(false)]
        [SkipAttribute(false)]
        [Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
        public AncorColor MarkerFrameColor
        {
            get { return _markerFrameColor; }
            set { _markerFrameColor = value; }
        }

        //[Browsable(false)]
        public override AncorColor FillColor
        {
            get
            {
                return base.FillColor;
            }
            set
            {
                base.FillColor = value;
            }
        }

         //[Browsable(false)]
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

        // [Browsable(false)]
         public override fillStyle FillStyle
         {
             get
             {
                 return base.FillStyle;
             }
             set
             {
                 base.FillStyle = value;
             }
         }


        [Browsable(false)]
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

        private bool _useLeader;
        [Category("Decoration")]
        //[Browsable(false)]
        [SkipAttribute(false)]
        public bool UseLeader
        {
            get { return _useLeader; }
            set { _useLeader = value; }
        }


        private AncorLeaderLine _leaderLine;
        [SkipAttribute(false)]
        public AncorLeaderLine LeaderLine
        {
            get { return _leaderLine; }
            set { _leaderLine = value; }
        }

        
       

        public ChartElement_MarkerSymbol(string _txt)
        {
            this.MarkerBackGround = new AncorMarkerBackGround(106,201, 40, "AeroSigma");
            this.FillStyle = fillStyle.fSSolid;
            this.HaloColor = new AncorColor (255,255,255);
            this.FillColor = new AncorColor(0, 0, 0);
            this.MarkerFrameColor = new AncorColor(0, 0, 0);
            //this.Font.FontColor = new AcntColor(0, 0, 0);
            //this.Font.Size = 10;
            this.Font = new AncorFont("Courier New", 10);
            this.ScaleMarkerToFitText = false;
            this.HorizontalAlignment = horizontalAlignment.Center;
            this.VerticalAlignment = verticalAlignment.Center;
            this.TextPosition = textPosition.Normal;
            this.TextCase = textCase.Normal;

            this.FillStyle = fillStyle.fSSolid;
            this.HaloColor = new AncorColor (255,255,255);
            this.FillColor = new AncorColor(255, 0, 0);

            this.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
            this.LeaderLine.LeaderColor = this.Font.FontColor;
            /////////////////////////////////////////////////////////////////////////////////////////////
            this.TextContents = new List<List<AncorChartElementWord>>();

            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord(_txt, this.Font);//создаем слово
            //wrd.TextValue = _txt;
            wrd.Font.Bold = false;
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);

            /////////////////////////////////////////////////////////////////////////////////////////////
            
        }

        public ChartElement_MarkerSymbol()
        {
        }

 
        public override object ConvertToIElement()
        {
            try
            {

                #region "Создание RouteSign"

                TextSymbolClass textSymbolMarkerSign = new TextSymbolClass();

                textSymbolMarkerSign.Angle = this.Slope;

                // цвет текста и рамки
                //IRgbColor Clr = new RgbColorClass();
                //Clr.Red = this.MarkerFrameColor.Red;//this.Font.FontColor.Red;
                //Clr.Green = this.MarkerFrameColor.Green;//this.Font.FontColor.Green;
                //Clr.Blue = this.MarkerFrameColor.Blue;//this.Font.FontColor.Blue;



                /// временная затычка для белорусии.убарть!!!!
                ///
                //this.MarkerFrameColor.AncorColor_CMYK = 100;

                textSymbolMarkerSign.Color = this.MarkerFrameColor.GetColor();//Clr;
                textSymbolMarkerSign.WordSpacing = this.WordSpacing;

                TextSymbolClass textSymbolFiller = new TextSymbolClass();
                textSymbolFiller.Angle = this.Slope;
                textSymbolFiller.WordSpacing = this.WordSpacing;
                // включаем маску Halo
                if (this.HaloMaskSize > 0)
                {
                    #region

                    IMask haloMsk = (IMask)textSymbolMarkerSign;
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

                IMarkerTextBackground TxtBackGrndMarkerSign = new MarkerTextBackground();
                TxtBackGrndMarkerSign.ScaleToFit = this.ScaleMarkerToFitText;


                IMarkerTextBackground TxtBackGrndFiller = new MarkerTextBackground();
                TxtBackGrndFiller.ScaleToFit = this.ScaleMarkerToFitText;


                ICharacterMarkerSymbol characterMarkerSymbolMarkerSign = new CharacterMarkerSymbolClass();
                ICharacterMarkerSymbol characterMarkerSymbolFiller = new CharacterMarkerSymbolClass();


                stdole.IFontDisp stdFont = new stdole.StdFontClass() as stdole.IFontDisp;
                stdFont.Name = this.MarkerBackGround.FontName;
                


                characterMarkerSymbolMarkerSign.Font = stdFont;
                characterMarkerSymbolFiller.Font = stdFont;

                characterMarkerSymbolMarkerSign.Angle = this.Slope * Math.PI / 180; ;

               
                characterMarkerSymbolMarkerSign.CharacterIndex = this.MarkerBackGround.CharacterIndex; // "пахлава"
                characterMarkerSymbolFiller.CharacterIndex = this.MarkerBackGround.InnerCharacterIndex; // "пахлава"
                
                



                //размер знака символа
                characterMarkerSymbolMarkerSign.Size = this.MarkerBackGround.CharacterSize;
                characterMarkerSymbolFiller.Size = this.MarkerBackGround.CharacterSize;// -3;


                //цвет заливки
                //IRgbColor FillClr = new RgbColorClass();
                //FillClr.Red = this.FillColor.Red;
                //FillClr.Green = this.FillColor.Green;
                //FillClr.Blue = this.FillColor.Blue;

                characterMarkerSymbolFiller.Color = this.FillColor.GetColor();//FillClr;

                characterMarkerSymbolMarkerSign.Color = this.MarkerFrameColor.GetColor();//Clr;
                characterMarkerSymbolMarkerSign.XOffset = 0;
                characterMarkerSymbolMarkerSign.YOffset = 0;


                TxtBackGrndMarkerSign.Symbol = characterMarkerSymbolMarkerSign;
                TxtBackGrndFiller.Symbol = characterMarkerSymbolFiller;


                textSymbolMarkerSign.Background = TxtBackGrndMarkerSign;
                textSymbolFiller.Background = TxtBackGrndFiller;


                ISimpleTextSymbol simpleTextSymbolMarkerSign = (ISimpleTextSymbol)textSymbolMarkerSign;

                simpleTextSymbolMarkerSign.TextPath = textPath;
                simpleTextSymbolMarkerSign.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                simpleTextSymbolMarkerSign.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                simpleTextSymbolMarkerSign.Angle = this.Slope;


                // размер и характеристика шрифта для внутреннего текста
                stdole.IFontDisp pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                pFontDisp.Name = this.Font.Name;
                pFontDisp.Bold = this.Font.Bold;
                pFontDisp.Italic = this.Font.Italic;
                pFontDisp.Underline = this.Font.UnderLine;
                pFontDisp.Size = (decimal)this.Font.Size;
                simpleTextSymbolMarkerSign.Font = pFontDisp;

                if (this.TextContents != null && this.TextContents.Count == 1 && this.TextContents[0][0].DataSource.Condition.StartsWith("NoneScale"))
                {
                    this.TextContents[0][0].Font.FontColor = new AncorColor(this.FillColor.Red, this.FillColor.Green, this.FillColor.Blue);
                }

                ITextElement pTextEl_MarkerSign = new TextElementClass();
                pTextEl_MarkerSign.Text = HelperClass.TextConstructor(this.TextContents); 
                pTextEl_MarkerSign.ScaleText = false;
                pTextEl_MarkerSign.Symbol = simpleTextSymbolMarkerSign;


                IElement El_MarkerSign = (IElement)pTextEl_MarkerSign;

                ISimpleTextSymbol simpleTextSymbolFiller = (ISimpleTextSymbol)textSymbolFiller;
                simpleTextSymbolFiller.TextPath = textPath;
                simpleTextSymbolFiller.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                simpleTextSymbolFiller.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                simpleTextSymbolFiller.Font = pFontDisp;




                ITextElement pTextEl_Filler = new TextElementClass();
                pTextEl_Filler.Text = HelperClass.TextConstructor(this.TextContents);

                pTextEl_Filler.Symbol = simpleTextSymbolFiller;



                IElement El_Filler = (IElement)pTextEl_Filler;

                #endregion  "Создание RouteSign"

                      IGroupElement3 GrpEl = new GroupElementClass();

                if (this.FillStyle != fillStyle.fSNull) GrpEl.AddElement(El_Filler);
                GrpEl.AddElement(El_MarkerSign);



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

   
            IDisplayFeedback _feedback = new NewTextFeedbackClass();
            NewTextFeedbackClass mvPtFeed = (NewTextFeedbackClass)_feedback;
            mvPtFeed.Symbol = (ISymbol)txtS;

            return mvPtFeed;
        }

        public override object Clone()
        {
            ChartElement_MarkerSymbol o = (ChartElement_MarkerSymbol)base.Clone();

            o.MarkerBackGround = (AncorMarkerBackGround)o.MarkerBackGround.Clone();
            o.LeaderLine = (AncorLeaderLine)o.LeaderLine.Clone();
            o.MarkerFrameColor = (AncorColor)o.MarkerFrameColor.Clone();
            return o;
        }
    }
}
