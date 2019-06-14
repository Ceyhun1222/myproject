using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using ANCOR.MapCore;
using ANCOR.MapElements;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;

namespace ANCOR.MapElements
{
    public class ChartElement_BorderedText_Collout : ChartElement_BorderedText
    {
       
        private AncorLeaderLine _leaderLine;
        [Category("Decoration")]
        [SkipAttribute(false)]
        public virtual AncorLeaderLine LeaderLine
        {
            get { return _leaderLine; }
            set { _leaderLine = value; }
        }

        

        public ChartElement_BorderedText_Collout()
        {
        }

        public ChartElement_BorderedText_Collout(string _text)
        {
            this.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
            this.LeaderLine.LeaderColor = new AncorColor(0, 0, 0);
            this.Font = new AncorFont("Courier New", 8);
            this.HaloColor = new AncorColor ( 255, 255, 255 );
            this.FillColor = new AncorColor ( 255, 255, 255 );
            this.Border = new AncorFrame(lineStyle.lsSolid);
            this.Anchor = new AncorPoint(0,0);
            this.WordSpacing = 100;
            this.TextContents = new List<List<AncorChartElementWord>>();
            this.Shadow = new AncorShadow { ShadowColor = new AncorColor(84, 84, 84), ShadowOffSet = 0 };
            this.HorizontalAlignment = horizontalAlignment.Center;
            this.VerticalAlignment = verticalAlignment.Center;
            this.TextPosition = textPosition.Normal;
            this.TextCase = textCase.Normal;

            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord(_text,this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd);
            this.TextContents.Add(txtLine); 
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

                if (this.Shadow.ShadowOffSet != 0)
                {
                    this.FillStyle = fillStyle.fSSolid;
                }


                // формирование обрамляющей рамки

                LineCallout pCallout = HelperClass.GetLeaderLineBorder(this.FillColor, this.FillStyle, this.Border, this.LeaderLine, this.Anchor);
                pTextSymbol.Background = (ITextBackground)pCallout;


                pTextElement.Symbol = pTextSymbol;

                if (this.Shadow.ShadowOffSet != 0)
                {
                    ITextElement pShadow = HelperClass.CreateShadow((ChartElement_BorderedText)this.Clone());


                    IGroupElement3 GrpEl = new GroupElementClass();

                    GrpEl.AddElement((IElement)pShadow);
                    GrpEl.AddElement((IElement)pTextElement);

                    return GrpEl;
                }


                return pTextElement;
            }
            catch { return null; }

        }

        public override object Clone()
        {
            ChartElement_BorderedText_Collout o = (ChartElement_BorderedText_Collout)base.Clone();
            o.LeaderLine = (AncorLeaderLine)o.LeaderLine.Clone();
            return o;
        }

        public override IDisplayFeedback GetFeedback()
        {

            ChartElement_BorderedText_Collout symbol = (ChartElement_BorderedText_Collout)this.Clone();
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

    }

}
