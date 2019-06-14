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
    /// <summary>
    /// </summary>
    public class ChartElement_BorderedText : ChartElement_SimpleText
    {
        [XmlElement]
        [Browsable(false)]
        //[XmlIgnore]
        private AcntFrame _border;
        public AcntFrame Border
        {
            get { return _border; }
            set { _border = value; }
        }


        public ChartElement_BorderedText()
        {
            this.Border = new AcntFrame();
            //this.Color = new AcntColor();
            this.FillColor = new AcntColor();
            this.FillStyle = fillStyle.fSSolid;
            this.HaloColor = new AcntColor();
            this.HaloMaskSize = 0.5;


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

                LineCallout pCallout = HelperClass.GetSimpleBorder(this.FillColor, this.FillStyle, this.Border);
                pTextSymbol.Background = (ITextBackground)pCallout;


                pTextElement.Symbol = pTextSymbol;

                return pTextElement;
            }
            catch { return null; }
        }

    }

}
