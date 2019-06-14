using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using ANCOR.MapCore;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
//using Accent.MapCore;

namespace ANCOR.MapElements
{
    public class ChartElement_TextArrow : ChartElement_BorderedText_Collout
    {

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

        //[Browsable(true)]
        //public override AncorPoint Anchor { get => base.Anchor; set => base.Anchor = value; }


        public ChartElement_TextArrow()
        {
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

        public ChartElement_TextArrow(string _text)
        {
            this.HorizontalAlignment = horizontalAlignment.Center;
            this.VerticalAlignment = verticalAlignment.Center;
            this.TextCase = textCase.Normal;
            this.TextPosition = textPosition.Normal;
            this.Font = new AncorFont("Courier New", 8);
            
            /////////////////////////////////////////////////////////////////////////////////////////////
            this.TextContents = new List<List<AncorChartElementWord>>();

            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord(_text, this.Font);//создаем слово
            wrd.Font.Bold = false;
           
            //wrd.Morse = true;
            txtLine.Add(wrd);
            this.TextContents.Add(txtLine);  // добавим его в строку
            /////////////////////////////////////////////////////////////////////////////////////////////

            //this.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
            //this.ArrowMarker = new AncorArrowMarker {Length =4, Position = arrowPosition.Start,Width =4};
            
            this.HaloColor = new AncorColor(255,255,255);
            this.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
            this.LeaderLine.ArrowMarker = new AncorArrowMarker { Length = 4, Position = arrowPosition.Start, Width = 4 };
            this.LeaderLine.EndsWithArrow = true;
            //this.Border = new AcntFrame(lineStyle.lsSolid);
            this.FillColor = new AncorColor(255, 255, 255);
            //this.Shadow = new AncorShadow { ShadowColor = new AncorColor(84, 84, 84), ShadowOffSet = 0 };

        }

        public override object ConvertToIElement()
        {
            try
            {
                ITextElement pTextElement = new TextElementClass();

                ////формирование внутреннего текста
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


                // формирование обрамляющей рамки
                
                LineCallout pCallout = HelperClass.GetmArrowLeaderLine(this.LeaderLine, this.LeaderLine.ArrowMarker, this.Anchor, this.FillColor, this.FillStyle);
                pTextSymbol.Background = (ITextBackground)pCallout;


                pTextElement.Symbol = pTextSymbol;

                return pTextElement;
            }
            catch { return null; }

        }

        private object feedBackIElement()
        {
            try
            {
                ITextElement pTextElement = new TextElementClass();

                ////формирование внутреннего текста
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
            catch { return null; }

        }


        public override object Clone()
        {
            ChartElement_TextArrow o = (ChartElement_TextArrow)base.Clone();

            //o.ArrowMarker = (AncorArrowMarker)o.ArrowMarker.Clone();
            o.LeaderLine = (AncorLeaderLine)o.LeaderLine.Clone();

            return o;


        }

        public override IDisplayFeedback GetFeedback()
        {

            ChartElement_TextArrow symbol = (ChartElement_TextArrow)this.Clone();
            
            IElement iEl = (IElement)(symbol).feedBackIElement();
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
    }

}
