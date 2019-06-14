using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using Accent.MapCore;
using Accent.MapElements;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;

namespace Accent.MapElements
{
    public class ChartElement_BorderedText_Collout : ChartElement_BorderedText
    {
        [XmlElement]
        [Browsable(false)]
        //[XmlIgnore]
        private AcntLeaderLine _leaderLine;
        public AcntLeaderLine LeaderLine
        {
            get { return _leaderLine; }
            set { _leaderLine = value; }
        }

        public ChartElement_BorderedText_Collout()
        {
            this.LeaderLine = new AcntLeaderLine();
            this.LeaderLine.LeaderColor = new AcntColor(0, 0, 0);
        }


        public ChartElement_BorderedText_Collout(string _text)
        {
            this.LeaderLine = new AcntLeaderLine();
            this.LeaderLine.LeaderColor = new AcntColor(0, 0, 0);

            this.TextContents[0][0].TextValue = _text;
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

                LineCallout pCallout = HelperClass.GetLeaderLineBorder(this.FillColor, this.FillStyle, this.Border, this.LeaderLine, this.Shift);
                pTextSymbol.Background = (ITextBackground)pCallout;


                pTextElement.Symbol = pTextSymbol;

                return pTextElement;
            }
            catch { return null; }

        }

    }

}
