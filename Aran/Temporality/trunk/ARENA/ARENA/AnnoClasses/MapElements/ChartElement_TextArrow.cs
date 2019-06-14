using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Accent.MapCore;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
//using Accent.MapCore;

namespace Accent.MapElements
{
    public class ChartElement_TextArrow : ChartElement_BorderedText_Collout
    {
        [XmlElement]
        [Browsable(false)]
        //[XmlIgnore]
        private AcntArrowMarker _arrowMarker;
        public AcntArrowMarker ArrowMarker
        {
            get { return _arrowMarker; }
            set { _arrowMarker = value; }
        }

        [XmlElement]
        [Browsable(false)]
        //[XmlIgnore]
        private AcntLeaderLine _leader;
        public AcntLeaderLine Leader
        {
            get { return _leader; }
            set { _leader = value; }
        }

        public ChartElement_TextArrow()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////
            this.TextContents = new List<List<AcntChartElementWord>>();

            List<AcntChartElementWord> txtLine = new List<AcntChartElementWord>(); // создаем строку
            AcntChartElementWord wrd = new AcntChartElementWord();//создаем слово
            wrd.TextValue = "abc";
            wrd.Font.Bold = false;
            //wrd.Morse = true;
            txtLine.Add(wrd);
            this.TextContents.Add(txtLine);  // добавим его в строку
            /////////////////////////////////////////////////////////////////////////////////////////////

            this.Leader = new AcntLeaderLine();
            this.ArrowMarker = new AcntArrowMarker();
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

                LineCallout pCallout = HelperClass.GetmArrowLeaderLine(this.LeaderLine, this.ArrowMarker, this.Shift);
                pTextSymbol.Background = (ITextBackground)pCallout;


                pTextElement.Symbol = pTextSymbol;

                return pTextElement;
            }
            catch { return null; }

        }
    }

}
