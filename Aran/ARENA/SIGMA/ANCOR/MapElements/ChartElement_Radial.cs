using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using System.Windows.Forms.Design;
using ANCOR.MapCore;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using System.IO;
using System.Drawing;

namespace ANCOR.MapElements
{
    public class ChartElement_Radial : ChartElement_SimpleText
    {
        public ChartElement_Radial()
        {
        }

        private rowPosition _RowPosition;
        [Category("Arrow attributes")]
        [SkipAttribute(true)]
        public rowPosition RowPosition
        {
            get { return _RowPosition; }
            set { _RowPosition = value; }
        }

        private arrowPosition _arrowPosition;
        [Category("Arrow attributes")]
        [SkipAttribute(true)]
        public arrowPosition ArrowPosition
        {
            get { return _arrowPosition; }
            set { _arrowPosition = value; }
        }

        private int _rowLen;
        [Category("Arrow attributes")]
        public int RowLen
        {
            get { return _rowLen; }
            set { _rowLen = value; }
        }

        private int _arrowSize;
        [Category("Arrow attributes")]
        public int ArrowSize
        {
            get { return _arrowSize; }
            set { _arrowSize = value; }
        }


        private int _dotSize;
        [Category("Arrow attributes")]
        public int DotSize
        {
            get { return _dotSize; }
            set { _dotSize = value; }
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

        [Browsable(false)]
        public override List<List<AncorChartElementWord>> TextContents { get => base.TextContents; set => base.TextContents = value; }

        [Browsable(false)]
        public override verticalAlignment VerticalAlignment { get => base.VerticalAlignment; set => base.VerticalAlignment = value; }

        [Browsable(false)]
        public override horizontalAlignment HorizontalAlignment { get => base.HorizontalAlignment; set => base.HorizontalAlignment = value; }
        public ChartElement_Radial(string _txt, string _angle)
        {


            this.Font = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "Arial", Size = 6, UnderLine = false };
            //this.GeometryAnchor = null;
            //this.GraphicsElement = null;
            this.HorizontalAlignment = horizontalAlignment.Left;
            this.VerticalAlignment = verticalAlignment.Bottom;
            this.Slope = 0;
            this.TextPosition = textPosition.Normal;
            this.TextCase = textCase.Normal;
            this.Anchor = new AncorPoint(0, 0);
            this.WordSpacing = 100;
            this.HaloColor = new AncorColor(255, 255, 255);
            this.FillColor = new AncorColor (255, 255, 255);
            this.FillStyle = fillStyle.fSNull;
            this.CoordType = coordtype.DDMMSSN_1;
            this.RowPosition = rowPosition.above;
            this.Leading = -4;
            this.RowLen = 8;
            this.ArrowPosition = arrowPosition.End;
            this.ArrowSize = 8;
            this.DotSize = 8;
            /////////////////////////////////////////////////////////////////////////////////////////////
            this.TextContents = new List<List<AncorChartElementWord>>();

            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            AncorChartElementWord wrd = new AncorChartElementWord(_txt,this.Font);//создаем слово
            wrd.Font = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "Arial", Size = 6, UnderLine = false };
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку

            wrd = new AncorChartElementWord(_angle, this.Font);//создаем слово
            wrd.Font = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "Arial", Size = 6, UnderLine = false };
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AncorSymbol("");
            wrd.EndSymbol = new AncorSymbol("°");
            //wrd.Morse = true;
            txtLine.Add(wrd); // добавим его в строку

            this.TextContents.Add(txtLine);

            //////////////////////////////////////////////////////////////////////////////////////////
        }

        public override object ConvertToIElement()
        {
            try
            {

                ITextElement pTextElement = new TextElementClass();

                List<AncorChartElementWord> rowLine = new List<AncorChartElementWord>(); // создаем строку
                AncorChartElementWord Artrow_wrd = new AncorChartElementWord("p");//создаем слово
                if (this.ArrowPosition == arrowPosition.Start) Artrow_wrd = new AncorChartElementWord("x");
                //Artrow_wrd.Font = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "AeroSigma", Size = this.Font.Size, UnderLine = false };                
                Artrow_wrd.Font.Bold = false;
                Artrow_wrd.Font.FontColor = new AncorColor(0, 0, 0);
                Artrow_wrd.Font.Italic = false;
                Artrow_wrd.Font.Name = "AeroSigma";
                Artrow_wrd.Font.UnderLine = false ;
                Artrow_wrd.StartSymbol = new AncorSymbol("");
                Artrow_wrd.EndSymbol = new AncorSymbol("");
                Artrow_wrd.Font.Size = this.ArrowSize;
                Artrow_wrd.DataSource.Value = "arrow";
                Artrow_wrd.TextPosition = textPosition.Subscript;

                AncorChartElementWord wrd = new AncorChartElementWord("");
                for (int i = 0; i < this.RowLen; i++)
                {
                    wrd.TextValue = "Ä" + wrd.TextValue;
                    wrd.DataSource.Value = "arrow";
                    wrd.Font = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "AeroSigma", Size = this.DotSize, UnderLine = false };
                    wrd.Font.Bold = true;
                    wrd.StartSymbol = new AncorSymbol("");
                    wrd.EndSymbol = new AncorSymbol("");
                }


                if (this.ArrowPosition == arrowPosition.Start)
                {
                    rowLine.Add(Artrow_wrd);
                    rowLine.Add(wrd); // добавим его в строку
                }
                else
                {
                    rowLine.Add(wrd); // добавим его в строку
                    rowLine.Add(Artrow_wrd);
                }


                bool flag = false;
                foreach (var ln in this.TextContents)
                {
                    foreach (var wrdRow in ln)
                    {
                        if (wrdRow.TextValue.Contains("Ä"))
                        {
                            flag = true;
                            ln.Remove(wrdRow);
                            break;
                        }
                    }
                    if (flag)
                    {
                        this.TextContents.Remove(ln);
                        break;
                    }

                }

                foreach (var ln in this.TextContents)
                {
                    foreach (var wrdRow in ln)
                    {
                        if (!wrdRow.DataSource.Value.StartsWith("arrow"))
                        {
                            wrdRow.Font = (AncorFont)this.Font.Clone();
                            if (wrdRow.StartSymbol!=null) wrdRow.StartSymbol.TextFont = (AncorFont)this.Font.Clone(); 
                            if (wrdRow.EndSymbol!=null) wrdRow.EndSymbol.TextFont = (AncorFont)this.Font.Clone();
                            
                        }
                        else
                            wrdRow.TextPosition = textPosition.Subscript;
                    }
                }

                switch (this.RowPosition)
                {
                    case rowPosition.under:
                        this.TextContents.Add(rowLine);
                        break;

                    case rowPosition.above:
                        this.TextContents.Insert(0, rowLine);
                        break;

                    case rowPosition.before:
                        this.TextContents.Insert(0, rowLine);
                        rowLine[0].TextPosition = textPosition.Normal;
                        break;

                    case rowPosition.after:
                        this.TextContents.Add(rowLine);
                        rowLine[0].TextPosition = textPosition.Normal;
                        break;

                    default:
                        break;
                }

                pTextElement.Text = HelperClass.TextConstructor(this.TextContents, this.RowPosition);

                TextSymbolClass pTextSymbol = new TextSymbolClass();
                

                // форматирование текста
                HelperClass.FormatText(ref pTextSymbol, this.TextPosition, this.Leading, this.TextCase, horizontalAlignment.Left, verticalAlignment.Bottom, this.CharacterSpacing, this.CharacterWidth, this.WordSpacing);
                HelperClass.CreateFont(ref pTextSymbol, this.Font);


                if (this.HaloMaskSize > 0) HelperClass.UseHaloMask(ref pTextSymbol, this.HaloMaskSize, this.HaloColor);


                // смещение относительно точки привязки
                pTextSymbol.XOffset = this.Anchor.X;
                pTextSymbol.YOffset = this.Anchor.Y;

                // наклон
                pTextSymbol.Angle = this.Slope;

                /////////////////////////
                if (this.FillStyle != fillStyle.fSNull)
                {
                    AncorFrame brdr = new AncorFrame(this.FillColor, new AncorFrameMargins(0, 0, 0, 0), 0, 0.001, lineStyle.lsNull);
                    LineCallout pCallout = HelperClass.GetSimpleBorder(this.FillColor, this.FillStyle, brdr);
                    pTextSymbol.Background = (ITextBackground)pCallout;
                }
                ///////////////////////////


                pTextElement.Symbol = pTextSymbol;


                return pTextElement;


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        //public override object ConvertToIElement()
        //{
        //    try
        //    {

        //        ITextElement pTextElement = new TextElementClass();

        //        List<AncorChartElementWord> rowLine = new List<AncorChartElementWord>(); // создаем строку
        //        AncorChartElementWord wrdLn = new AncorChartElementWord("");//создаем слово
        //        //if (this.ArrowPosition == arrowPosition.Start) wrdLn = new AncorChartElementWord("x");
        //        for (int i = 0; i < this.RowLen; i++)
        //        {
        //            //wrdLn.TextValue = this.ArrowPosition == arrowPosition.End ? "Ä"  : wrdLn.TextValue + "Ä";
        //            wrdLn.TextValue = wrdLn.TextValue + "Ä";

        //        }
        //        wrdLn.Font = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "AeroSigma", Size = 20, UnderLine = false };
        //        wrdLn.Font.Bold = true;
        //        wrdLn.StartSymbol = new AncorSymbol("");
        //        wrdLn.EndSymbol = new AncorSymbol("");
        //        wrdLn.TextPosition = textPosition.Subscript;
        //        //wrd.Morse = true;
        //        rowLine.Add(wrdLn); // добавим его в строку



        //        wrdLn = new AncorChartElementWord("");//создаем слово
        //        //if (this.ArrowPosition == arrowPosition.Start) wrdLn = new AncorChartElementWord("x");
        //        for (int i = 0; i < this.RowLen; i++)
        //        {
        //            wrdLn.TextValue = this.ArrowPosition == arrowPosition.End ? "p" : "x";

        //        }
        //        wrdLn.Font = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "AeroSigma", Size = 20, UnderLine = false };
        //        wrdLn.Font.Bold = true;
        //        wrdLn.StartSymbol = new AncorSymbol("");
        //        wrdLn.EndSymbol = new AncorSymbol("");
        //        wrdLn.TextPosition = textPosition.Subscript;
        //        //wrd.Morse = true;
        //        if (this.ArrowPosition == arrowPosition.End) rowLine.Add(wrdLn); // добавим его в строку
        //        else rowLine.Insert(0, wrdLn);


        //        bool flag = false;
        //        foreach (var ln in this.TextContents)
        //        {
        //            foreach (var wrdRow in ln)
        //            {
        //                if (wrdRow.Font.Name.CompareTo("AeroSigma") == 0)
        //                {
        //                    flag = true;
        //                    ln.Remove(wrdRow);
        //                    break;
        //                }
        //            }
        //            if (flag)
        //            {
        //                this.TextContents.Remove(ln);
        //                break;
        //            }
        //        }

        //        switch (this.RowPosition)
        //        {
        //            case rowPosition.under:
        //                rowLine[0].TextPosition = textPosition.Superscript;
        //                this.TextContents.Add(rowLine);

        //                break;

        //            case rowPosition.above:
        //                this.TextContents.Insert(0, rowLine);
        //                rowLine[0].TextPosition = textPosition.Subscript;
        //                break;

        //            case rowPosition.before:
        //                this.TextContents.Insert(0, rowLine);
        //                rowLine[0].TextPosition = textPosition.Normal;
        //                break;

        //            case rowPosition.after:
        //                this.TextContents.Add(rowLine);
        //                rowLine[0].TextPosition = textPosition.Normal;
        //                break;

        //            default:
        //                break;
        //        }

        //        pTextElement.Text = HelperClass.TextConstructor(this.TextContents, this.RowPosition).Trim();

        //        TextSymbolClass pTextSymbol = new TextSymbolClass();


        //        // форматирование текста
        //        HelperClass.FormatText(ref pTextSymbol, this.TextPosition, this.Leading, this.TextCase, this.HorizontalAlignment, this.VerticalAlignment, this.CharacterSpacing, this.CharacterWidth, this.WordSpacing);
        //        HelperClass.CreateFont(ref pTextSymbol, this.Font);


        //        if (this.HaloMaskSize > 0) HelperClass.UseHaloMask(ref pTextSymbol, this.HaloMaskSize, this.HaloColor);


        //        // смещение относительно точки привязки
        //        pTextSymbol.XOffset = this.Anchor.X;
        //        pTextSymbol.YOffset = this.Anchor.Y;

        //        // наклон
        //        pTextSymbol.Angle = this.Slope;

        //        /////////////////////////
        //        if (this.FillStyle != fillStyle.fSNull)
        //        {
        //            AncorFrame brdr = new AncorFrame(this.FillColor, new AncorFrameMargins(0, 0, 0, 0), 0, 0.001, lineStyle.lsNull);
        //            LineCallout pCallout = HelperClass.GetSimpleBorder(this.FillColor, this.FillStyle, brdr);
        //            pTextSymbol.Background = (ITextBackground)pCallout;
        //        }
        //        ///////////////////////////


        //        pTextElement.Symbol = pTextSymbol;


        //        return pTextElement;


        //    }

        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return null;
        //    }

        //}


    }
}
