using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using Accent.MapCore;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
//using Accent.MapCore;

namespace Accent.MapElements
{
    public class ChartElement_BorderedText_Collout_CaptionBottom : ChartElement_BorderedText_Collout
    {
        [XmlElement]
        [Browsable(false)]
        private List<List<AcntChartElementWord>> _bottomTextLine;
        [Editor(typeof(MyTextContextEditor), typeof(UITypeEditor))]
        public List<List<AcntChartElementWord>> BottomTextLine
        {
            get { return _bottomTextLine; }
            set { _bottomTextLine = value; }
        }

        [XmlElement]
        [Browsable(false)]
        private List<List<AcntChartElementWord>> _captionTextLine;
        [Editor(typeof(MyTextContextEditor), typeof(UITypeEditor))]
        public List<List<AcntChartElementWord>> CaptionTextLine
        {
            get { return _captionTextLine; }
            set { _captionTextLine = value; }
        }


        public ChartElement_BorderedText_Collout_CaptionBottom()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////
            this.CaptionTextLine = new List<List<AcntChartElementWord>>(); // создаем заголовок

            List<AcntChartElementWord> txtLine = new List<AcntChartElementWord>(); // создаем строку
            AcntChartElementWord wrd = new AcntChartElementWord();//создаем слово
            wrd.TextValue = "Value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AcntSymbol("+");
            wrd.EndSymbol = new AcntSymbol("+");
            //wrd.Morse = true;
            txtLine.Add(wrd);
            this.CaptionTextLine.Add(txtLine);  // добавим его в заголовок
            //////////////////////////////////////////////////////////////////////////////////////////////

            this.BottomTextLine = new List<List<AcntChartElementWord>>(); // создаем нижнюю строку

            txtLine = new List<AcntChartElementWord>(); // создаем строку
            wrd = new AcntChartElementWord();//создаем слово
            wrd.TextValue = "Value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AcntSymbol("+");
            wrd.EndSymbol = new AcntSymbol("+");
            //wrd.Morse = true;
            txtLine.Add(wrd);
            this.BottomTextLine.Add(txtLine);  // добавим его в нижнюю строку

            //////////////////////////////////////////////////////////////////////////////////////////


            this.Border.FrameMargins = new AcntFrameMargins(-5, -4, 0, 0);

        }

        public override object ConvertToIElement()
        {
            try
            {
                ITextElement pTextElement = new TextElementClass();

                ////формирование внутреннего текста
                pTextElement.Text = HelperClass.TextConstructor(this.CaptionTextLine) + " " + HelperClass.TextConstructor(this.TextContents) + " " + HelperClass.TextConstructor(this.BottomTextLine);

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
