using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accent.MapCore;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
//using Accent.MapCore;

namespace Accent.MapElements
{
    public class ChartElement_RouteDesignator : ChartElement_SimpleText
    {

        private routeSegmentDirection _routeSegmentDirection;
        public routeSegmentDirection RouteSegmentDirection
        {
            get { return _routeSegmentDirection; }
            set { _routeSegmentDirection = value; }
        }

        private double _routeSignSize;
        public double RouteSignSize
        {
            get { return _routeSignSize; }
            set { _routeSignSize = value; }
        }

        private routeDesignatorPosition _routeDesignatorPosition;
        public routeDesignatorPosition RouteDesignatorPosition
        {
            get { return _routeDesignatorPosition; }
            set { _routeDesignatorPosition = value; }
        }

        private AcntMarkerBackGround _markerBackGround;
        public AcntMarkerBackGround MarkerBackGround
        {
            get { return _markerBackGround; }
            set { _markerBackGround = value; }
        }

        public ChartElement_RouteDesignator()
        {
            this.MarkerBackGround = new AcntMarkerBackGround(84, 40, "Risk Aero");
            this.RouteDesignatorPosition = routeDesignatorPosition.Bottom;
            this.RouteSegmentDirection = routeSegmentDirection.Both;
            this.RouteSignSize = 40;
            this.FillStyle = fillStyle.fSSolid;
            this.HaloColor = new AcntColor();
            this.FillColor = new AcntColor(255, 0, 0);
            this.Font.FontColor = new AcntColor(0, 0, 0);
            this.Font.Size = 10;
            this.ScaleMarkerToFitText = false;
            this.LineSpasing = 15;
            this.BoldDesignatorText = false;

            /////////////////////////////////////////////////////////////////////////////////////////////
            this.TextContents = new List<List<AcntChartElementWord>>();

            List<AcntChartElementWord> txtLine = new List<AcntChartElementWord>(); // создаем строку
            AcntChartElementWord wrd = new AcntChartElementWord();//создаем слово
            wrd.TextValue = "Value";
            wrd.Font.Bold = false;
            //wrd.Morse = true;
            txtLine.Add(wrd);
            this.TextContents.Add(txtLine);  // добавим его в строку
            /////////////////////////////////////////////////////////////////////////////////////////////
            
        }

        public override object ConvertToIElement()
        {
            try
            {

                #region "Создание RouteSign"

                TextSymbolClass textSymbolInner = new TextSymbolClass();

                textSymbolInner.Angle = this.Slope;

                // цвет текста и рамки
                IRgbColor Clr = new RgbColorClass();
                Clr.Red = this.Font.FontColor.Red;
                Clr.Green = this.Font.FontColor.Green;
                Clr.Blue = this.Font.FontColor.Blue;
                textSymbolInner.Color = Clr;

                TextSymbolClass textSymbolOuter = new TextSymbolClass();

                textSymbolOuter.Angle = this.Slope;

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
                ICharacterMarkerSymbol characterMarkerSymbolOuter = new CharacterMarkerSymbolClass();


                stdole.IFontDisp stdFont = new stdole.StdFontClass() as stdole.IFontDisp;
                stdFont.Name = "Risk Aero";


                characterMarkerSymbolInner.Font = stdFont;
                characterMarkerSymbolOuter.Font = stdFont;

                characterMarkerSymbolInner.Angle = this.Slope * Math.PI / 180; ;

                if (this.RouteSegmentDirection == routeSegmentDirection.Both)
                {
                    characterMarkerSymbolInner.CharacterIndex = 84; // "пахлава"
                    characterMarkerSymbolOuter.CharacterIndex = 86; // "пахлава"
                }
                if (this.RouteSegmentDirection == routeSegmentDirection.Forward)
                {
                    characterMarkerSymbolInner.CharacterIndex = 85; // "пахлава"
                    characterMarkerSymbolOuter.CharacterIndex = 87; // "пахлава"
                }
                if (this.RouteSegmentDirection == routeSegmentDirection.Backward)
                {
                    characterMarkerSymbolInner.CharacterIndex = 85; // "пахлава"
                    characterMarkerSymbolInner.Angle = 180;

                    characterMarkerSymbolOuter.CharacterIndex = 87; // "пахлава"
                    characterMarkerSymbolOuter.Angle = 180;
                }


                //размер знака символа
                characterMarkerSymbolInner.Size = this.RouteSignSize;
                characterMarkerSymbolOuter.Size = this.RouteSignSize;// -3;


                //цвет заливки
                IRgbColor FillClr = new RgbColorClass();
                FillClr.Red = this.FillColor.Red;
                FillClr.Green = this.FillColor.Green;
                FillClr.Blue = this.FillColor.Blue;

                characterMarkerSymbolOuter.Color = FillClr;

                characterMarkerSymbolInner.Color = Clr;
                characterMarkerSymbolInner.XOffset = 0;
                characterMarkerSymbolInner.YOffset = 0;


                TxtBackGrndInner.Symbol = characterMarkerSymbolInner;
                TxtBackGrndOuter.Symbol = characterMarkerSymbolOuter;


                textSymbolInner.Background = TxtBackGrndInner;
                textSymbolOuter.Background = TxtBackGrndOuter;


                ISimpleTextSymbol simpleTextSymbolInner = (ISimpleTextSymbol)textSymbolInner;

                simpleTextSymbolInner.TextPath = textPath;
                simpleTextSymbolInner.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                simpleTextSymbolInner.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                simpleTextSymbolInner.Angle = this.Slope;


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

                ISimpleTextSymbol simpleTextSymbolOuter = (ISimpleTextSymbol)textSymbolOuter;
                simpleTextSymbolOuter.TextPath = textPath;
                simpleTextSymbolOuter.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                simpleTextSymbolOuter.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                simpleTextSymbolOuter.Font = pFontDisp;

                ITextElement pTextEl_Outer = new TextElementClass();
                pTextEl_Outer.Text = " ";

                pTextEl_Outer.Symbol = simpleTextSymbolOuter;

                IElement El_Outer = (IElement)pTextEl_Outer;

                #endregion  "Создание RouteSign"

                #region "Создание RouteDesignator"

                //Create a text symbol and grab hold of the ITextSymbol interface
                TextSymbolClass textSymbolRouteDesignator = new TextSymbolClass();
                textSymbolRouteDesignator.XOffset = 0;
                if (this.RouteDesignatorPosition == routeDesignatorPosition.Top) textSymbolRouteDesignator.YOffset = this.LineSpasing;//this.YOffset + this.LineSpasing;
                if (this.RouteDesignatorPosition == routeDesignatorPosition.Bottom) textSymbolRouteDesignator.YOffset = -this.LineSpasing;//this.YOffset - this.LineSpasing;



                Clr = new RgbColorClass();
                Clr.Red = this.Font.FontColor.Red;
                Clr.Green = this.Font.FontColor.Green;
                Clr.Blue = this.Font.FontColor.Blue;
                textSymbolRouteDesignator.Color = Clr;


                pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                pFontDisp.Name = this.Font.Name;
                pFontDisp.Bold = this.BoldDesignatorText;
                pFontDisp.Italic = this.Font.Italic;
                pFontDisp.Underline = this.Font.UnderLine;
                pFontDisp.Size = (decimal)this.Font.Size;
                textSymbolRouteDesignator.Font = pFontDisp;


                textPath = new BezierTextPathClass();
                textPath.XOffset = 0;

                ISimpleTextSymbol simpleTextSymbolRouteDesignator = (ISimpleTextSymbol)textSymbolRouteDesignator;
                simpleTextSymbolRouteDesignator.TextPath = textPath;
                simpleTextSymbolRouteDesignator.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                simpleTextSymbolRouteDesignator.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;

                ITextElement pTextEl_RouteDesignator = new TextElementClass();
                pTextEl_RouteDesignator.Text = "DESIGNATOR"; //AccentObjManipulator.CreateAnnoText(Tmp, mapScale);
                //pTextEl.Text = this.RouteDesignator;

                simpleTextSymbolRouteDesignator.Angle = this.Slope;// +12;

                pTextEl_RouteDesignator.Symbol = simpleTextSymbolRouteDesignator;
                //pTextEl_Inner.Symbol.Angle = this.Slope;

                if (this.TextPosition == textPosition.Superscript) (textSymbolRouteDesignator as IFormattedTextSymbol).Position = esriTextPosition.esriTPSuperscript;
                if (this.TextPosition == textPosition.Subscript) (textSymbolRouteDesignator as IFormattedTextSymbol).Position = esriTextPosition.esriTPSubscript;
                if (this.TextPosition == textPosition.Normal) (textSymbolRouteDesignator as IFormattedTextSymbol).Position = esriTextPosition.esriTPNormal;



                IElement El_RouteDesignator = (IElement)pTextEl_RouteDesignator;

                #endregion

                IGroupElement3 GrpEl = new GroupElementClass();

                //if (this.RouteDesignator.CompareTo("ABCD") != 0) 
                GrpEl.AddElement(El_Outer);
                GrpEl.AddElement(El_Inner);
                GrpEl.AddElement(El_RouteDesignator);



                return GrpEl;
            }
            catch { return null; }
        }

        private bool _scaleMarkerToFitText;

        public bool ScaleMarkerToFitText
        {
            get { return _scaleMarkerToFitText; }
            set { _scaleMarkerToFitText = value; }
        }

        private double _lineSpasing;

        public double LineSpasing
        {
            get { return _lineSpasing; }
            set { _lineSpasing = value; }
        }

        private bool _boldDesignatorText;

        public bool BoldDesignatorText
        {
            get { return _boldDesignatorText; }
            set { _boldDesignatorText = value; }
        }
    }

}
