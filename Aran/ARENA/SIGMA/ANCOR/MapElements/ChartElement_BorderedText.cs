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
    /// <summary>
    /// </summary>
     [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ChartElement_BorderedText : ChartElement_SimpleText
    {
        
        //[Browsable(false)]
        private AncorFrame _border;
        [Category("Decoration")]
        [SkipAttribute(false)]
        public virtual AncorFrame Border
        {
            get { return _border; }
            set { _border = value; }
        }

        private AncorShadow _shadow;
        [Category("Decoration")]
        [SkipAttribute(false)]
        public virtual AncorShadow Shadow
        {
            get { return _shadow; }
            set { _shadow = value; }
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

        [Browsable(true)]
        public override double CharacterSpacing
        {
            get
            {
                return base.CharacterSpacing;
            }
            set
            {
                base.CharacterSpacing = value;
            }
        }

         [Browsable(true)]
         public override double CharacterWidth
         {
             get
             {
                 return base.CharacterWidth;
             }
             set
             {
                 base.CharacterWidth = value;
             }
         }

         [Browsable(true)]
         public override double WordSpacing
         {
             get
             {
                 return base.WordSpacing;
             }
             set
             {
                 base.WordSpacing = value;
             }
         }

         [SkipAttribute(false)]
         public override double Slope
         {
             get
             {
                 return base.Slope;
             }
             set
             {
                 base.Slope = value;
             }
         }

        public ChartElement_BorderedText()
        {
        }

        public ChartElement_BorderedText(string _text)
        {
            this.Border = new AncorFrame(lineStyle.lsSolid);
            this.Font = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "Courier New", Size = 8, UnderLine = false };
            this.FillColor = new AncorColor (255,255,255);
            this.FillStyle = fillStyle.fSSolid;
            this.HaloColor = new AncorColor (255,255,255);
            this.HaloMaskSize = 0.5;
            this.HorizontalAlignment = horizontalAlignment.Center;
            this.VerticalAlignment = verticalAlignment.Center;
            this.FillStyle = fillStyle.fSNull;
            this.TextPosition = textPosition.Normal;
            this.TextCase = textCase.Normal;
            this.Anchor = new AncorPoint(0, 0);
            this.WordSpacing = 100;
            this.Shadow = new AncorShadow { ShadowColor = new AncorColor(84, 84, 84), ShadowOffSet = 0 };

            /////////////////////////////////////////////////////////////////////////////////////////////
            this.TextContents = new List<List<AncorChartElementWord>>();

            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord(_text, this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd);
            this.TextContents.Add(txtLine);  // добавим его в строку

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

                if (this.HaloMaskSize > 0)  HelperClass.UseHaloMask(ref pTextSymbol, this.HaloMaskSize, this.HaloColor);


                // смещение относительно точки привязки
                pTextSymbol.XOffset = 0;
                pTextSymbol.YOffset = 0;

                // наклон
                pTextSymbol.Angle = this.Slope;


                // формирование обрамляющей рамки
                if (this.Shadow.ShadowOffSet != 0)
                {
                    this.FillStyle = fillStyle.fSSolid;
                }

                LineCallout pCallout = HelperClass.GetSimpleBorder(this.FillColor, this.FillStyle, this.Border);
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
            ChartElement_BorderedText o = (ChartElement_BorderedText)base.Clone();
            if (o.Border!=null) o.Border = (AncorFrame)o.Border.Clone();
            return o;
        }

        //public override IDisplayFeedback GetFeedback()
        //{
        //    ChartElement_BorderedText symbol = (ChartElement_BorderedText)this.Clone();
        //    IElement iEl = (IElement)symbol.ConvertToIElement();
        //    ITextSymbol txtS =null;

        //    if (iEl is IGroupElement)
        //    {
        //        txtS = ((ITextElement)((IGroupElement)iEl).get_Element(1)).Symbol;
        //    }
        //    else
        //        txtS = ((ITextElement)symbol.ConvertToIElement()).Symbol;

        //    IDisplayFeedback _feedback = new CalloutFeedbackClass();
        //    CalloutFeedbackClass mvPtFeed = (CalloutFeedbackClass)_feedback;
        //    mvPtFeed.Symbol = (ISymbol)txtS;

        //    return mvPtFeed;

        //}

        //public override void StartFeedback(IDisplayFeedback feedBack, IPoint _position, double scale, IGeometry LinkedGeometry)
        //{
        //    CalloutFeedbackClass mvPtFeed = (CalloutFeedbackClass)feedBack;

        //    ChartElement_BorderedText_Collout symbol = (ChartElement_BorderedText_Collout)this.Clone();
        //    symbol.Anchor = new AncorPoint(0, 0);
        //    IElement iEl = (IElement)symbol.ConvertToIElement();
            
        //    ITextSymbol txtS = null;

        //    if (iEl is IGroupElement)
        //    {
        //        ((IGroupElement)iEl).get_Element(1).Geometry = LinkedGeometry;
        //        txtS = ((ITextElement)((IGroupElement)iEl).get_Element(1)).Symbol;
        //    }
        //    else
        //    {
        //        iEl.Geometry = LinkedGeometry;
        //        txtS = ((ITextElement)iEl).Symbol;
        //    }

        //    mvPtFeed.Start((ISymbol)txtS, _position, _position,scale);
        //}

        //public override void MoveFeedback(IDisplayFeedback feedBack, IPoint _position, IGeometry LinkedGeometry)
        //{
        //    (feedBack as CalloutFeedbackClass).MoveTo(_position);
        //}

        //public override void StopFeedback(IDisplayFeedback feedBack)
        //{
        //    (feedBack as CalloutFeedbackClass).Stop();
        //}

    }

}
