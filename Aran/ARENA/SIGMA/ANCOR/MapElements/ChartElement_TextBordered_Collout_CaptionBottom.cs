using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using ANCOR.MapCore;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
//using Accent.MapCore;

namespace ANCOR.MapElements
{
    public class ChartElement_BorderedText_Collout_CaptionBottom : ChartElement_BorderedText_Collout
    {
        private List<List<AncorChartElementWord>> _bottomTextLine;
        [Editor(typeof(MyTextContextEditor), typeof(UITypeEditor))]
        [Browsable(false)]
        [SkipAttribute(false)]
        public List<List<AncorChartElementWord>> BottomTextLine
        {
            get { return _bottomTextLine; }
            set { _bottomTextLine = value; }
        }

        private List<List<AncorChartElementWord>> _captionTextLine;
        [Editor(typeof(MyTextContextEditor), typeof(UITypeEditor))]
        [Browsable(false)]
        [SkipAttribute(false)]
        public List<List<AncorChartElementWord>> CaptionTextLine
        {
            get { return _captionTextLine; }
            set { _captionTextLine = value; }
        }




        public ChartElement_BorderedText_Collout_CaptionBottom()
        {
        }

        public ChartElement_BorderedText_Collout_CaptionBottom(string CaptionText)
        {
            this.Font = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "Courier New", Size = 8, UnderLine = false };
            this.FillColor = new AncorColor (255,255,255);
            this.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
            this.HaloColor = new AncorColor(255, 255, 255);
            this.HorizontalAlignment = horizontalAlignment.Center;
            this.VerticalAlignment = verticalAlignment.Center;
            this.TextPosition = textPosition.Normal;
            this.TextCase = textCase.Normal;
            this.Shadow = new AncorShadow { ShadowColor = new AncorColor(84, 84, 84), ShadowOffSet = 0 };

            /////////////////////////////////////////////////////////////////////////////////////////////
            this.CaptionTextLine = new List<List<AncorChartElementWord>>(); // создаем заголовок

            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord(CaptionText, this.Font);//создаем слово
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("+");
            wrd.EndSymbol = new AncorSymbol("+");
            //wrd.Morse = true;
            txtLine.Add(wrd);
            this.CaptionTextLine.Add(txtLine);  // добавим его в заголовок
            //////////////////////////////////////////////////////////////////////////////////////////////

            this.TextContents = new List<List<AncorChartElementWord>>();

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("NavType", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine); // добавим строку


            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("FreqValue", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку

            wrd = new AncorChartElementWord("FreqUom", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку

            this.TextContents.Add(txtLine);  // добавим строку

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("ABC", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd);

            wrd = new AncorChartElementWord("ABC", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);  // добавим строку

            
            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("Location X", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);  // добавим строку

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("Location Y", this.Font);//создаем слово
            //wrd.TextValue = "value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку
            this.TextContents.Add(txtLine);  // добавим строку


            /////////////////////////////////////////////////////////////////////////////////////////////

            this.BottomTextLine = new List<List<AncorChartElementWord>>(); // создаем нижнюю строку

            txtLine = new List<AncorChartElementWord>(); // создаем строку
            wrd = new AncorChartElementWord("Elevation", this.Font);//создаем слово
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd);

            wrd = new AncorChartElementWord("ElevationUOM", this.Font);//создаем слово
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd);

            this.BottomTextLine.Add(txtLine);  // добавим его в нижнюю строку

            //////////////////////////////////////////////////////////////////////////////////////////
            this.Border = new AncorFrame(lineStyle.lsSolid);

            this.Border.FrameMargins = new AncorFrameMargins(-5, -4, 0, 0);

        }

        public override object ConvertToIElement()
        {
            try
            {
                ChartElement_TextArrow fictionLeader = null;
                ITextElement pTextElement = new TextElementClass();

                ////формирование внутреннего текста
                pTextElement.Text = this.BottomTextLine != null ? HelperClass.TextConstructor(this.CaptionTextLine, true) + " " + HelperClass.TextConstructor(this.TextContents) + " " + HelperClass.TextConstructor(this.BottomTextLine, true) :
                                                                HelperClass.TextConstructor(this.CaptionTextLine, true) + " " + HelperClass.TextConstructor(this.TextContents);

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
                if (this.Shadow.ShadowOffSet != 0) this.FillStyle = fillStyle.fSSolid;

                
                LineCallout pCallout = HelperClass.GetLeaderLineBorder(this.FillColor, this.FillStyle, this.Border, this.LeaderLine, this.Anchor);

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (this.LeaderLine.LeaderStyle == lineCalloutStyle.CSFree)
                {
                    pCallout = HelperClass.GetSimpleBorder(this.FillColor, this.FillStyle, this.Border);
                    fictionLeader = new ChartElement_TextArrow(".");
                    fictionLeader.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
                    //fictionLeader.ArrowMarker = new AncorArrowMarker { Length = 4, Position = arrowPosition.Start, Width = 4 };
                    fictionLeader.Font = new AncorFont("Courier New", 8);
                    fictionLeader.HaloColor = new AncorColor(this.HaloColor.Red, this.HaloColor.Green, this.HaloColor.Blue);
                    fictionLeader.LeaderLine = new AncorLeaderLine(lineCalloutStyle.CSBase);
                    fictionLeader.LeaderLine.LeaderLineStyle = this.LeaderLine.LeaderLineStyle;
                    fictionLeader.LeaderLine.LeaderLineWidth = this.LeaderLine.LeaderLineWidth;
                    fictionLeader.LeaderLine.LeaderColor = new AncorColor(this.LeaderLine.LeaderColor.Red, this.LeaderLine.LeaderColor.Green, this.LeaderLine.LeaderColor.Blue);

                    fictionLeader.Anchor = new AncorPoint(this.Anchor.X, this.Anchor.Y);
                    //fictionLeader.ArrowMarker = null;
                    //this.Border = new AcntFrame(lineStyle.lsSolid);
                    this.FillColor = new AncorColor(this.FillColor.Red, this.FillColor.Green, this.FillColor.Blue);
                    
                }
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                pTextSymbol.Background = (ITextBackground)pCallout;


                pTextElement.Symbol = pTextSymbol;

                if (this.Shadow.ShadowOffSet != 0 || this.LeaderLine.LeaderStyle == lineCalloutStyle.CSFree)
                {
                    IGroupElement3 GrpEl = new GroupElementClass();

                    if (this.Shadow.ShadowOffSet != 0)
                    {
                        ITextElement pShadow = HelperClass.CreateShadow((ChartElement_BorderedText_Collout_CaptionBottom)this.Clone());
                        GrpEl.AddElement((IElement)pShadow);
                    }


                    /////////////////////////////////////////////////////////////////////
                    if (this.LeaderLine.LeaderStyle == lineCalloutStyle.CSFree)
                    {
                        IElement fictionLeaderEl = (IElement)fictionLeader.ConvertToIElement();
                        GrpEl.AddElement((IElement)fictionLeaderEl);

                    }
                    //////////////////////////////////////////////////////////////////////

                    GrpEl.AddElement((IElement)pTextElement);

                    return GrpEl;
                }



                return pTextElement;
            }
            catch { return null; }

        }

        public override object Clone()
        {
            ChartElement_BorderedText_Collout_CaptionBottom chartEl = (ChartElement_BorderedText_Collout_CaptionBottom)base.Clone();

            if (chartEl.CaptionTextLine != null && chartEl.CaptionTextLine.Count > 0)
                chartEl.CaptionTextLine = chartEl.CloneTextContenst(chartEl.CaptionTextLine);

            if (chartEl.BottomTextLine != null && chartEl.BottomTextLine.Count > 0)
                chartEl.BottomTextLine = chartEl.CloneTextContenst(chartEl.BottomTextLine);

            return chartEl;
        }



    }

}
