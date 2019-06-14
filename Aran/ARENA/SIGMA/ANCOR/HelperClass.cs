using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using ANCOR.MapCore;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using System.Reflection;


namespace ANCOR.MapElements
{
    public class HelperClass
    {
        public static void FormatText(ref TextSymbolClass pTextSymbol, textPosition TextPosition, double Leading, textCase TextCase, horizontalAlignment HorizontalAlignment, verticalAlignment VerticalAlignment, double CharacterSpacing, double CharacterWidth, double WordSpacing)
        {
            IFormattedTextSymbol FrmT = (IFormattedTextSymbol)pTextSymbol;
            if (TextPosition == textPosition.Superscript) FrmT.Position = esriTextPosition.esriTPSuperscript;
            if (TextPosition == textPosition.Subscript) FrmT.Position = esriTextPosition.esriTPSubscript;
            if (TextPosition == textPosition.Normal) FrmT.Position = esriTextPosition.esriTPNormal;

            if (TextCase == textCase.AllCaps) pTextSymbol.Case = esriTextCase.esriTCAllCaps;
            if (TextCase == textCase.Normal) pTextSymbol.Case = esriTextCase.esriTCNormal;
            if (TextCase == textCase.SmallCaps) pTextSymbol.Case = esriTextCase.esriTCSmallCaps;
            if (TextCase == textCase.Lowercase) pTextSymbol.Case = esriTextCase.esriTCLowercase;

            if (HorizontalAlignment == horizontalAlignment.Left) pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
            if (HorizontalAlignment == horizontalAlignment.Right) pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
            if (HorizontalAlignment == horizontalAlignment.Center) pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
            if (HorizontalAlignment == horizontalAlignment.Full) pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull;

            if (VerticalAlignment == verticalAlignment.Baselene) pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline;
            if (VerticalAlignment == verticalAlignment.Bottom) pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
            if (VerticalAlignment == verticalAlignment.Center) pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
            if (VerticalAlignment == verticalAlignment.Top) pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;



            // параметры текста
            pTextSymbol.CharacterSpacing = CharacterSpacing;
            pTextSymbol.CharacterWidth = CharacterWidth;
            pTextSymbol.WordSpacing = WordSpacing;
            pTextSymbol.Leading = Leading;

        }

        public static void CreateFont(ref TextSymbolClass pTextSymbol, AncorFont Font)
        {
            // параметры шрифта
            stdole.IFontDisp pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
            pFontDisp.Name = Font.Name;
            pFontDisp.Bold = Font.Bold;
            pFontDisp.Italic = Font.Italic;
            pFontDisp.Underline = Font.UnderLine;


            pFontDisp.Size = new decimal(Font.Size);

            pTextSymbol.Font = pFontDisp;
            pTextSymbol.Color = Font.FontColor.GetColor();

        }

        public static LineCallout GetSimpleBorder(AncorColor FillColor, fillStyle FillStyle, AncorFrame Border)
        {
            IPoint AnchorPnt = new PointClass();

            LineCallout pCallout = new LineCallout();

            ISimpleFillSymbol smplFill = new SimpleFillSymbol();

            //IRgbColor rgbClr = new RgbColor();  //цвет заполнения
            //rgbClr.Red = FillColor.Red;
            //rgbClr.Blue = FillColor.Blue;
            //rgbClr.FillColor.Green;

            smplFill.Color = FillColor.GetColor(); //цвет заполнения

            //стиль заполнения
            if (FillStyle == fillStyle.fSBackwardDiagonal) smplFill.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;
            if (FillStyle == fillStyle.fSCross) smplFill.Style = esriSimpleFillStyle.esriSFSCross;
            if (FillStyle == fillStyle.fSDiagonalCross) smplFill.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
            if (FillStyle == fillStyle.fSForwardDiagonal) smplFill.Style = esriSimpleFillStyle.esriSFSForwardDiagonal;
            if (FillStyle == fillStyle.fSHollow) smplFill.Style = esriSimpleFillStyle.esriSFSHollow;
            if (FillStyle == fillStyle.fSHorizontal) smplFill.Style = esriSimpleFillStyle.esriSFSHorizontal;
            if (FillStyle == fillStyle.fSNull) smplFill.Style = esriSimpleFillStyle.esriSFSNull;
            if (FillStyle == fillStyle.fSSolid) smplFill.Style = esriSimpleFillStyle.esriSFSSolid;
            if (FillStyle == fillStyle.fSVertical) smplFill.Style = esriSimpleFillStyle.esriSFSVertical;

            if (Border != null)
            {


                IColor lineClr = new RgbColor(); // создание рамки
                ISimpleLineSymbol pSimpleLine = new SimpleLineSymbol();
                pSimpleLine.Width = Border.Thickness;
                pSimpleLine.Color = Border.FrameColor.GetColor(); //цвет рамочного элемента

                if (Border.FrameLineStyle == lineStyle.lsDash) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDash;
                if (Border.FrameLineStyle == lineStyle.lsDashDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDashDot;
                if (Border.FrameLineStyle == lineStyle.lsDashDotDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDashDotDot;
                if (Border.FrameLineStyle == lineStyle.lsDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDot;
                if (Border.FrameLineStyle == lineStyle.lsInsideFrame) pSimpleLine.Style = esriSimpleLineStyle.esriSLSInsideFrame;
                if (Border.FrameLineStyle == lineStyle.lsNull) pSimpleLine.Style = esriSimpleLineStyle.esriSLSNull;
                if (Border.FrameLineStyle == lineStyle.lsSolid) pSimpleLine.Style = esriSimpleLineStyle.esriSLSSolid;


                smplFill.Outline = pSimpleLine;

                pCallout.Border = smplFill;

                
                IPoint pnt = new PointClass();
                pnt.PutCoords(10, 10);
                pCallout.AnchorPoint = pnt;

            }
            else
            {
                pCallout.Border = null;
            }


            pCallout.LeaderLine = null;



            //смещение текста внутри рамки
            if (pCallout != null)
            {
                pCallout.Gap = 0;
                pCallout.AccentBar = null;

                ITextMargins pTextMrgns = (ITextMargins)pCallout;

                if (Border == null) pTextMrgns.PutMargins(0, 0, 0, 0);
                else pTextMrgns.PutMargins(Border.FrameMargins.Left, Border.FrameMargins.Top, Border.FrameMargins.Right, Border.FrameMargins.Bottom);
            }

            return pCallout;

        }

        public static LineCallout GetLeaderLineBorder(AncorColor FillColor, fillStyle FillStyle, AncorFrame Border, AncorLeaderLine LeaderLine, AncorPoint Shift)
        {
            IPoint AnchorPnt = new PointClass();

            LineCallout pCallout = new LineCallout();

            ISimpleFillSymbol smplFill = new SimpleFillSymbol();
            smplFill.Color = FillColor.GetColor(); //цвет заполнения

            //стиль заполнения
            if (FillStyle == fillStyle.fSBackwardDiagonal) smplFill.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;
            if (FillStyle == fillStyle.fSCross) smplFill.Style = esriSimpleFillStyle.esriSFSCross;
            if (FillStyle == fillStyle.fSDiagonalCross) smplFill.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
            if (FillStyle == fillStyle.fSForwardDiagonal) smplFill.Style = esriSimpleFillStyle.esriSFSForwardDiagonal;
            if (FillStyle == fillStyle.fSHollow) smplFill.Style = esriSimpleFillStyle.esriSFSHollow;
            if (FillStyle == fillStyle.fSHorizontal) smplFill.Style = esriSimpleFillStyle.esriSFSHorizontal;
            if (FillStyle == fillStyle.fSNull) smplFill.Style = esriSimpleFillStyle.esriSFSNull;
            if (FillStyle == fillStyle.fSSolid) smplFill.Style = esriSimpleFillStyle.esriSFSSolid;
            if (FillStyle == fillStyle.fSVertical) smplFill.Style = esriSimpleFillStyle.esriSFSVertical;

            if (Border != null)
            {


                IColor lineClr = new RgbColor(); // создание рамки
                ISimpleLineSymbol pSimpleLine = new SimpleLineSymbol();
                pSimpleLine.Width = Border.Thickness;
                pSimpleLine.Color = Border.FrameColor.GetColor();

                if (Border.FrameLineStyle == lineStyle.lsDash) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDash;
                if (Border.FrameLineStyle == lineStyle.lsDashDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDashDot;
                if (Border.FrameLineStyle == lineStyle.lsDashDotDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDashDotDot;
                if (Border.FrameLineStyle == lineStyle.lsDot) pSimpleLine.Style = esriSimpleLineStyle.esriSLSDot;
                if (Border.FrameLineStyle == lineStyle.lsInsideFrame) pSimpleLine.Style = esriSimpleLineStyle.esriSLSInsideFrame;
                if (Border.FrameLineStyle == lineStyle.lsNull) pSimpleLine.Style = esriSimpleLineStyle.esriSLSNull;
                if (Border.FrameLineStyle == lineStyle.lsSolid) pSimpleLine.Style = esriSimpleLineStyle.esriSLSSolid;


                smplFill.Outline = pSimpleLine;

                pCallout.Border = smplFill;


                pCallout.LeaderLine = new SimpleLineSymbolClass();
                if (LeaderLine.LeaderLineStyle == lineStyle.lsDash) ((ISimpleLineSymbol)pCallout.LeaderLine).Style = esriSimpleLineStyle.esriSLSDash;
                if (LeaderLine.LeaderLineStyle == lineStyle.lsDashDot) ((ISimpleLineSymbol)pCallout.LeaderLine).Style = esriSimpleLineStyle.esriSLSDashDot;
                if (LeaderLine.LeaderLineStyle == lineStyle.lsDashDotDot) ((ISimpleLineSymbol)pCallout.LeaderLine).Style = esriSimpleLineStyle.esriSLSDashDotDot;
                if (LeaderLine.LeaderLineStyle == lineStyle.lsDot) ((ISimpleLineSymbol)pCallout.LeaderLine).Style = esriSimpleLineStyle.esriSLSDot;
                if (LeaderLine.LeaderLineStyle == lineStyle.lsInsideFrame) ((ISimpleLineSymbol)pCallout.LeaderLine).Style = esriSimpleLineStyle.esriSLSInsideFrame;
                if (LeaderLine.LeaderLineStyle == lineStyle.lsNull) ((ISimpleLineSymbol)pCallout.LeaderLine).Style = esriSimpleLineStyle.esriSLSNull;
                if (LeaderLine.LeaderLineStyle == lineStyle.lsSolid) ((ISimpleLineSymbol)pCallout.LeaderLine).Style = esriSimpleLineStyle.esriSLSSolid;


               

                if (LeaderLine.EndsWithArrow && LeaderLine.ArrowMarker != null) pCallout.LeaderLine = new CartographicLineSymbolClass();


                pCallout.LeaderLine.Color = LeaderLine.LeaderColor.GetColor();
                pCallout.LeaderLine.Width = LeaderLine.LeaderLineWidth;
                pCallout.LeaderTolerance = 0;

                if (LeaderLine.LeaderStyle == lineCalloutStyle.CSBase) pCallout.Style = esriLineCalloutStyle.esriLCSBase;
                if (LeaderLine.LeaderStyle == lineCalloutStyle.CSCircularCCW) pCallout.Style = esriLineCalloutStyle.esriLCSCircularCCW;
                if (LeaderLine.LeaderStyle == lineCalloutStyle.CSCircularCW) pCallout.Style = esriLineCalloutStyle.esriLCSCircularCW;
                if (LeaderLine.LeaderStyle == lineCalloutStyle.CSFourPoint) pCallout.Style = esriLineCalloutStyle.esriLCSFourPoint;
                if (LeaderLine.LeaderStyle == lineCalloutStyle.CSMidpoint) pCallout.Style = esriLineCalloutStyle.esriLCSMidpoint;
                if (LeaderLine.LeaderStyle == lineCalloutStyle.CSThreePoint) pCallout.Style = esriLineCalloutStyle.esriLCSThreePoint;
                if (LeaderLine.LeaderStyle == lineCalloutStyle.CSUnderline) pCallout.Style = esriLineCalloutStyle.esriLCSUnderline;


              

                if (LeaderLine.EndsWithArrow &&  LeaderLine.ArrowMarker!=null)
                {
                    IArrowMarkerSymbol pMarker = new ArrowMarkerSymbol();
                    pMarker.Length = LeaderLine.ArrowMarker.Length;

                    pMarker.Width = LeaderLine.ArrowMarker.Width;

                    ISimpleLineDecorationElement pLineDecElem = new SimpleLineDecorationElement();
                    pLineDecElem.AddPosition((double)LeaderLine.ArrowMarker.Position);
                    pLineDecElem.MarkerSymbol = (IMarkerSymbol)pMarker;


                    ILineProperties lineProp = (ILineProperties)pCallout.LeaderLine;
                    LineDecoration ld = new LineDecorationClass();
                    ld.AddElement(pLineDecElem);
                    lineProp.LineDecoration = ld;
                }


 

                IPoint pnt = new PointClass();
                pnt.PutCoords(Shift.X, Shift.Y);
                pCallout.AnchorPoint = pnt;

            }
            else
            {
                pCallout.Border = null;
            }



            //смещение текста внутри рамки
            if (pCallout != null)
            {
                pCallout.Gap = 0;
                pCallout.AccentBar = null;

                ITextMargins pTextMrgns = (ITextMargins)pCallout;

                if (Border == null) pTextMrgns.PutMargins(0, 0, 0, 0);
                else pTextMrgns.PutMargins(Border.FrameMargins.Left, Border.FrameMargins.Top, Border.FrameMargins.Right, Border.FrameMargins.Bottom);
            }

            return pCallout;

        }


        public static LineCallout GetmArrowLeaderLine(AncorLeaderLine LeaderLine, AncorArrowMarker ArrowMarker, AncorPoint Shift, AncorColor FillColor, fillStyle FillStyle)
        {
            LineCallout pCallout = new LineCallout();


            // привязка созданного элемента

            pCallout.LeaderLine = new CartographicLineSymbol();
            pCallout.LeaderLine.Color = LeaderLine.LeaderColor.GetColor();
            pCallout.LeaderLine.Width = LeaderLine.LeaderLineWidth;


            if (ArrowMarker != null && LeaderLine.EndsWithArrow)
            {
                IArrowMarkerSymbol pMarker = new ArrowMarkerSymbol();
                pMarker.Length = ArrowMarker.Length;

                pMarker.Width = ArrowMarker.Width;
                pMarker.Color = LeaderLine.LeaderColor.GetColor();


                ISimpleLineDecorationElement pLineDecElem = new SimpleLineDecorationElement();
                pLineDecElem.AddPosition((double)ArrowMarker.Position);
                pLineDecElem.MarkerSymbol = (IMarkerSymbol)pMarker;


                ILineProperties lineProp = (ILineProperties)pCallout.LeaderLine;
                LineDecoration ld = new LineDecorationClass();
                ld.AddElement(pLineDecElem);
                lineProp.LineDecoration = ld;
            }

            pCallout.Gap = 0;
            pCallout.AccentBar = null;

            pCallout.LeaderTolerance = 0;

            if (LeaderLine.LeaderStyle == lineCalloutStyle.CSBase) pCallout.Style = esriLineCalloutStyle.esriLCSBase;
            if (LeaderLine.LeaderStyle == lineCalloutStyle.CSCircularCCW) pCallout.Style = esriLineCalloutStyle.esriLCSCircularCCW;
            if (LeaderLine.LeaderStyle == lineCalloutStyle.CSCircularCW) pCallout.Style = esriLineCalloutStyle.esriLCSCircularCW;
            //if (LineCalloutStyle == lineCalloutStyle.CSCustom) pCallout.Style = esriLineCalloutStyle.esriLCSCustom;
            if (LeaderLine.LeaderStyle == lineCalloutStyle.CSFourPoint) pCallout.Style = esriLineCalloutStyle.esriLCSFourPoint;
            if (LeaderLine.LeaderStyle == lineCalloutStyle.CSMidpoint) pCallout.Style = esriLineCalloutStyle.esriLCSMidpoint;
            if (LeaderLine.LeaderStyle == lineCalloutStyle.CSThreePoint) pCallout.Style = esriLineCalloutStyle.esriLCSThreePoint;
            if (LeaderLine.LeaderStyle == lineCalloutStyle.CSUnderline) pCallout.Style = esriLineCalloutStyle.esriLCSUnderline;


            IPoint pnt = new PointClass();
            pnt.PutCoords(Shift.X, Shift.Y);
            pCallout.AnchorPoint = pnt;

            ISimpleFillSymbol smplFill = new SimpleFillSymbol();

            smplFill.Color = FillColor.GetColor();

            //стиль заполнения
            if (FillStyle == fillStyle.fSBackwardDiagonal) smplFill.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;
            if (FillStyle == fillStyle.fSCross) smplFill.Style = esriSimpleFillStyle.esriSFSCross;
            if (FillStyle == fillStyle.fSDiagonalCross) smplFill.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
            if (FillStyle == fillStyle.fSForwardDiagonal) smplFill.Style = esriSimpleFillStyle.esriSFSForwardDiagonal;
            if (FillStyle == fillStyle.fSHollow) smplFill.Style = esriSimpleFillStyle.esriSFSHollow;
            if (FillStyle == fillStyle.fSHorizontal) smplFill.Style = esriSimpleFillStyle.esriSFSHorizontal;
            if (FillStyle == fillStyle.fSNull) smplFill.Style = esriSimpleFillStyle.esriSFSNull;
            if (FillStyle == fillStyle.fSSolid) smplFill.Style = esriSimpleFillStyle.esriSFSSolid;
            if (FillStyle == fillStyle.fSVertical) smplFill.Style = esriSimpleFillStyle.esriSFSVertical;

            smplFill.Outline = null;

            IFillSymbol fillSym = smplFill;
            pCallout.Border = fillSym;
            //pCallout.Border = null;

            ITextMargins pTextMrgns = (ITextMargins)pCallout;

            pTextMrgns.PutMargins(0, 0, 0, 0);


            return pCallout;
        }


  
        public static void UseHaloMask(ref TextSymbolClass pTextSymbol, double HaloMaskSize, AncorColor HaloColor)
        {
            IMask haloMsk = (IMask)pTextSymbol;
            haloMsk.MaskStyle = esriMaskStyle.esriMSHalo;
            haloMsk.MaskSize = HaloMaskSize;

            ISimpleFillSymbol smplFillHalo = new SimpleFillSymbol();

            smplFillHalo.Color = HaloColor.GetColor();

            ISimpleLineSymbol HaloLine = new SimpleLineSymbolClass();
            HaloLine.Color = HaloColor.GetColor();
            HaloLine.Width = 0;


            smplFillHalo.Outline = HaloLine;


            haloMsk.MaskSymbol = smplFillHalo;
        }

        public static string TextConstructor(List<List<AncorChartElementWord>> AcntLine, bool BreakLineFlag = false, bool IgnorVisibilityFlag = false)
        {
            try
            {
                string res = "";
                string ln = "";

                if ((AcntLine != null) && (AcntLine.Count > 0))
                {
                    foreach (List<AncorChartElementWord> Ln in AcntLine)
                    {
                        foreach (AncorChartElementWord AcntWord in Ln)
                        {
                            if (!AcntWord.Visible && !IgnorVisibilityFlag) continue;
                            //AcntWord.Visible = true;
                            if (AcntWord.TextValue.Length <= 0 && AcntLine.Count != 1) continue;


                            if (AcntWord.TextValue.StartsWith("ignore"))
                                AcntWord.TextValue = AcntWord.TextValue.Remove(0, "ignore".Length);
                            ln = ln + AcntWord.ToString();
                        }

                        if (ln.Length > 0) res = res + ln.TrimEnd();
                        if ((AcntLine.Count > 1 || BreakLineFlag) && (ln.Length > 0))  res = res + "\r" + "\n";
                        ln = "";
                    }
                }



                return res;
            }


            catch { return ""; }


        }

        public static string TextConstructor(List<List<AncorChartElementWord>> AcntLine, rowPosition rowPos)
        {
            try
            {
                string res = "";
                string ln = "";

                if ((AcntLine != null) && (AcntLine.Count > 0))
                {
                    foreach (List<AncorChartElementWord> Ln in AcntLine)
                    {
                        foreach (AncorChartElementWord AcntWord in Ln)
                        {
                            if (AcntWord.TextValue.Length <= 0) continue;
                            ln = ln + AcntWord.ToString() + " ";
                        }

                        if (ln.Length > 0) res = res + ln;
                        if (rowPos == rowPosition.above || rowPos == rowPosition.under) res = res + "\r" + "\n";
                        ln = "";
                    }
                }
                return res;
            }


            catch { return ""; }


        }

        public static string MorseTextConstructor(SigmaCallout_Morse MorseLine)
        {
            try
            {
                string res = "<Sigma value=" + "\"" + MorseLine.MorseText + "\"" + " size = " + "\"" + MorseLine.MorseSize + "\"" +
                    " red = " + "\"" + MorseLine.MorseColor.Red + "\"" + " green = " + "\"" + MorseLine.MorseColor.Green + "\"" + " blue = " + "\"" + MorseLine.MorseColor.Blue + "\"" + "></Sigma>";

                return res;
            }


            catch { return ""; }
        }


        public static bool RemoveIElementFromMap(IGraphicsContainer graphicsContainer, string IElementName)
        {
            try
            {
                graphicsContainer.Reset();

                IElement El = graphicsContainer.Next();
                while (El != null)
                {
                    IElementProperties3 ElProp2 = (IElementProperties3)El;
                    if (ElProp2.Name.CompareTo(IElementName) == 0)
                    {
                        graphicsContainer.DeleteElement(El);
                        break;
                    }
                    else
                        El = graphicsContainer.Next();
                }
                return true;
            }
            catch
            { return false; }

        }

        public static void RemoveAllIElementsFromMap(IGraphicsContainer graphicsContainer)
        {
            graphicsContainer.Reset();

            IElement El = graphicsContainer.Next();
            while (El != null)
            {
                IElementProperties3 ElProp2 = (IElementProperties3)El;
                //if (ElProp2.Name.StartsWith("AcntElement")) 
                    graphicsContainer.DeleteElement(El);

                El = graphicsContainer.Next();
            }
        }

        public static ITextElement CreateShadow(ChartElement_BorderedText masterChartEl)
        {

            foreach (var line in masterChartEl.TextContents)
            {
                foreach (var item in line)
                {
                    item.Font.FontColor = new AncorColor (masterChartEl.Shadow.ShadowColor.Blue,masterChartEl.Shadow.ShadowColor.Green, masterChartEl.Shadow.ShadowColor.Red );
                }
            }

            ITextElement pTextElement = new TextElementClass();

            ////формирование внутреннего текста
            pTextElement.Text = HelperClass.TextConstructor(masterChartEl.TextContents);

            TextSymbolClass pTextSymbol = new TextSymbolClass();


            // форматирование текста
            HelperClass.FormatText(ref pTextSymbol, masterChartEl.TextPosition, masterChartEl.Leading, masterChartEl.TextCase, masterChartEl.HorizontalAlignment, masterChartEl.VerticalAlignment, masterChartEl.CharacterSpacing, masterChartEl.CharacterWidth, masterChartEl.WordSpacing);
            AncorFont shadowFont = new AncorFont(masterChartEl.Font.Name, masterChartEl.Font.Size);
            shadowFont.Bold = masterChartEl.Font.Bold;
            shadowFont.Italic = masterChartEl.Font.Italic;
            shadowFont.UnderLine = masterChartEl.Font.UnderLine;
            shadowFont.FontColor = new AncorColor ( masterChartEl.Shadow.ShadowColor.Blue, masterChartEl.Shadow.ShadowColor.Green, masterChartEl.Shadow.ShadowColor.Red );

            HelperClass.CreateFont(ref pTextSymbol, shadowFont);


            // смещение относительно точки привязки
            pTextSymbol.XOffset = masterChartEl.Shadow.ShadowOffSet;
            pTextSymbol.YOffset = masterChartEl.Shadow.ShadowOffSet * -1;

            // наклон
            pTextSymbol.Angle = masterChartEl.Slope;


            // формирование обрамляющей рамки
            AncorFrame shadowBorder = new AncorFrame { FrameColor = new AncorColor ( masterChartEl.Shadow.ShadowColor.Blue, masterChartEl.Shadow.ShadowColor.Green,masterChartEl.Shadow.ShadowColor.Red ), FrameLineStyle = lineStyle.lsSolid, FrameMargins = masterChartEl.Border.FrameMargins, Offset = masterChartEl.Border.Offset, Thickness = masterChartEl.Border.Thickness };
            LineCallout pCallout = HelperClass.GetSimpleBorder(new AncorColor ( masterChartEl.Shadow.ShadowColor.Blue, masterChartEl.Shadow.ShadowColor.Green,masterChartEl.Shadow.ShadowColor.Red ), fillStyle.fSSolid, shadowBorder);
            pTextSymbol.Background = (ITextBackground)pCallout;


            pTextElement.Symbol = pTextSymbol;


            return pTextElement;
        }

        public static ITextElement CreateShadow(ChartElement_BorderedText_Collout_CaptionBottom masterChartEl)
        {

            foreach (var line in masterChartEl.TextContents)
            {
                foreach (var item in line)
                {
                    item.Font.FontColor = new AncorColor ( masterChartEl.Shadow.ShadowColor.Blue, masterChartEl.Shadow.ShadowColor.Green,masterChartEl.Shadow.ShadowColor.Red );
                }
            }

            foreach (var line in masterChartEl.CaptionTextLine)
            {
                foreach (var item in line)
                {
                    item.TextValue = "";
                    item.Font.FontColor = new AncorColor ( masterChartEl.Shadow.ShadowColor.Blue, masterChartEl.Shadow.ShadowColor.Green,masterChartEl.Shadow.ShadowColor.Red );
                }
            }

            if (masterChartEl.BottomTextLine != null)
            {
                foreach (var line in masterChartEl.BottomTextLine)
                {
                    foreach (var item in line)
                    {
                        item.TextValue = "";
                        item.Font.FontColor = new AncorColor ( masterChartEl.Shadow.ShadowColor.Blue, masterChartEl.Shadow.ShadowColor.Green,masterChartEl.Shadow.ShadowColor.Red );
                    }
                }

            }

            ITextElement pTextElement = new TextElementClass();

            ////формирование внутреннего текста
            pTextElement.Text = masterChartEl.BottomTextLine != null ? HelperClass.TextConstructor(masterChartEl.CaptionTextLine, true) + " " + HelperClass.TextConstructor(masterChartEl.TextContents) + " " + HelperClass.TextConstructor(masterChartEl.BottomTextLine, true) :
                                                   HelperClass.TextConstructor(masterChartEl.CaptionTextLine, true) + " " + HelperClass.TextConstructor(masterChartEl.TextContents);

            TextSymbolClass pTextSymbol = new TextSymbolClass();


            // форматирование текста
            HelperClass.FormatText(ref pTextSymbol, masterChartEl.TextPosition, masterChartEl.Leading, masterChartEl.TextCase, masterChartEl.HorizontalAlignment, masterChartEl.VerticalAlignment, masterChartEl.CharacterSpacing, masterChartEl.CharacterWidth, masterChartEl.WordSpacing);
            AncorFont shadowFont = new AncorFont(masterChartEl.Font.Name, masterChartEl.Font.Size);
            shadowFont.Bold = masterChartEl.Font.Bold;
            shadowFont.Italic = masterChartEl.Font.Italic;
            shadowFont.UnderLine = masterChartEl.Font.UnderLine;
            shadowFont.FontColor = new AncorColor ( masterChartEl.Shadow.ShadowColor.Blue, masterChartEl.Shadow.ShadowColor.Green,masterChartEl.Shadow.ShadowColor.Red );

            HelperClass.CreateFont(ref pTextSymbol, shadowFont);


            // смещение относительно точки привязки
            pTextSymbol.XOffset = masterChartEl.Shadow.ShadowOffSet;
            pTextSymbol.YOffset = masterChartEl.Shadow.ShadowOffSet * -1;

            // наклон
            pTextSymbol.Angle = masterChartEl.Slope;


            // формирование обрамляющей рамки
            AncorFrame shadowBorder = new AncorFrame { FrameColor = new AncorColor ( masterChartEl.Shadow.ShadowColor.Blue, masterChartEl.Shadow.ShadowColor.Green,masterChartEl.Shadow.ShadowColor.Red ), FrameLineStyle = lineStyle.lsSolid, FrameMargins = masterChartEl.Border.FrameMargins, Offset = masterChartEl.Border.Offset, Thickness = masterChartEl.Border.Thickness };
            LineCallout pCallout = HelperClass.GetSimpleBorder(new AncorColor ( masterChartEl.Shadow.ShadowColor.Blue, masterChartEl.Shadow.ShadowColor.Green,masterChartEl.Shadow.ShadowColor.Red ), fillStyle.fSSolid, shadowBorder);
            pTextSymbol.Background = (ITextBackground)pCallout;


            pTextElement.Symbol = pTextSymbol;


            return pTextElement;
        }


     
        
    }
}
